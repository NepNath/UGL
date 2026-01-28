using UnityEngine;

public class playerClass : entityClass
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float accelerationMultiplier = 1.5f;
    [SerializeField] private float attackCooldown = 0f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float rotationSpeed = 10f;   

    [Header("Player Inventory Slots")]
    [SerializeField] private bool slotWeapon;
    [SerializeField] private bool slotAccessory;

    [Header("Attack Visual")]
    [SerializeField] private LineRenderer attackArc;
    [SerializeField] private int arcSegments = 20;
    [SerializeField] private float arcAngle = 90f;
    [SerializeField] private float arcDuration = 0.5f;
    [SerializeField] private float projectileSpeed = 40f;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject projectilePrefab;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;

    private static readonly Matrix4x4 isometricMatrix = Matrix4x4.Rotate(
        Quaternion.Euler(0, 45, 0)
    );

    private float lastAttackTime = 0f;
    private float arcTimer = 0f;
    private bool arcVisible = false;

    private Vector3 respawnPosition;

    private void Start()
    {
        maxHealth = 100f;
        health = maxHealth;
        attackPower = 15f;
        speed = moveSpeed;
        respawnPosition = transform.position;

        if (attackArc == null)
            attackArc = GetComponent<LineRenderer>();

        if (attackArc != null)
        {
            attackArc.enabled = false;
            attackArc.useWorldSpace = true;
            attackArc.widthMultiplier = 0.1f;
        }

        checkEquipment();
    }

    private void Update()
    {
        GatherInput();
        HandleRotation();

        if (Input.GetMouseButtonDown(0))
        {
            checkAttack();
        }

        if (arcVisible)
        {
            arcTimer -= Time.deltaTime;
            if (arcTimer <= 0f && attackArc != null)
            {
                attackArc.enabled = false;
                arcVisible = false;
            }
        }

        move(moveDirection);
    }

    private void GatherInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 rawInput = new Vector3(x, 0f, y);

        moveDirection = isometricMatrix.MultiplyVector(rawInput);

        if (moveDirection.magnitude > 0.1f)
        {
            moveDirection.Normalize();
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    private void HandleRotation()
    {
        if (moveDirection.magnitude < 0.1f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public override void move(Vector3 direction)
    {
        if (direction.magnitude > 0.01f)
        {
            Vector3 targetVelocity = direction * moveSpeed;

            if (direction.magnitude > 0.1f)
                targetVelocity *= accelerationMultiplier;

            currentVelocity = Vector3.Lerp(
                currentVelocity,
                targetVelocity,
                Time.deltaTime * 5f
            );

            transform.position += currentVelocity * Time.deltaTime;
        }
        else
        {
            currentVelocity = Vector3.zero;
        }
    }

    public override void getDamaged(float amount)
    {
        base.getDamaged(amount);

        if (isDead())
        {
            StartCoroutine(respawnAfterDelay());
        }
    }

    private void respawn()
    {
        health = maxHealth;
        transform.position = respawnPosition;
    }

    private System.Collections.IEnumerator respawnAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        respawn();
    }

    private void checkAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;
        attack();
    }

    private void attack()
    {
        if (isWeaponEquipped())
        {
            shootProjectile();
        }
        else
        {
            showAttackArc();

            Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

            foreach (Collider hit in hits)
            {
                entityClass target = hit.GetComponent<entityClass>();
                if (target != null && !target.isDead())
                {
                    dealDamaged(target);
                }
            }
        }
    }

    private void showAttackArc()
    {
        if (attackArc == null)
            return;

        attackArc.enabled = true;
        arcVisible = true;
        arcTimer = arcDuration;

        attackArc.positionCount = arcSegments + 1;

        Vector3 center = transform.position + Vector3.up * 0.05f;

        float halfAngle = arcAngle * 0.5f;
        float startAngle = -halfAngle;
        float angleStep = arcAngle / arcSegments;

        Vector3 baseForward = transform.forward;
        baseForward.y = 0f;
        if (baseForward.sqrMagnitude < 0.001f)
            baseForward = Vector3.forward;

        Quaternion baseRot = Quaternion.LookRotation(baseForward, Vector3.up);

        for (int i = 0; i <= arcSegments; i++)
        {
            float angle = startAngle + angleStep * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 dirLocal = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad));
            Vector3 dirWorld = baseRot * dirLocal;

            Vector3 point = center + dirWorld * attackRange;
            attackArc.SetPosition(i, point);
        }

        Debug.Log("Showing attack arc");
    }

    private void shootProjectile()
    {
        Vector3 forward = transform.forward;
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.001f)
            forward = Vector3.forward;
        forward.Normalize();

        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, forward);
        RaycastHit hit;
        float maxDistance = 20f; 

        if (Physics.Raycast(ray, out hit, maxDistance, enemyLayer))
        {
            entityClass target = hit.collider.GetComponent<entityClass>();
            if (target != null && !target.isDead())
            {
                dealDamaged(target); 
            }
        }

        if (projectilePrefab != null)
        {
            Vector3 spawnPos = projectileSpawnPoint != null
                ? projectileSpawnPoint.position
                : transform.position + forward * 1f;

            Quaternion spawnRot = Quaternion.LookRotation(forward, Vector3.up);

            GameObject proj = Instantiate(projectilePrefab, spawnPos, spawnRot);

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.linearVelocity = forward * projectileSpeed;
            }

            Destroy(proj, 0.5f); 
        }
    }



    private void checkEquipment()
    {
        float baseMaxHealth = 100f;
        float baseAttack = 15f;
        float baseSpeed = moveSpeed;

        maxHealth = baseMaxHealth;
        attackPower = baseAttack;
        moveSpeed = baseSpeed;


        if (isAccessoryEquipped())
            attackPower += 40f;

        if (health < maxHealth)
            health = maxHealth;
    }

    public bool isWeaponEquipped() => slotWeapon;
    public bool isAccessoryEquipped() => slotAccessory;
}

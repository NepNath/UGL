using UnityEngine;

public class playerClass : entityClass
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float accelerationMultiplier = 1.5f;
    [SerializeField] private float attackCooldown = 0f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float rotationSpeed = 10f;   // Vitesse de rotation du player

    [Header("Player Inventory Slots")]
    [SerializeField] private bool slotWeapon;
    [SerializeField] private bool slotHelmet;
    [SerializeField] private bool slotBoots;
    [SerializeField] private bool slotLeggings;
    [SerializeField] private bool slotChestplate;
    [SerializeField] private bool slotAccessory;

    [Header("Attack Visual")]
    [SerializeField] private LineRenderer attackArc;
    [SerializeField] private int arcSegments = 20;
    [SerializeField] private float arcAngle = 90f;
    [SerializeField] private float arcDuration = 0.5f;
    [SerializeField] private float projectileSpeed = 40f;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject projectilePrefab;

    // Mouvement
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;

    // Matrice isométrique (45°)
    private static readonly Matrix4x4 isometricMatrix = Matrix4x4.Rotate(
        Quaternion.Euler(0, 45, 0)
    ); // [web:48]

    // Attaque
    private float lastAttackTime = 0f;
    private float arcTimer = 0f;
    private bool arcVisible = false;

    // Respawn
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

        // Conversion en direction isométrique (45°)
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

            // AOE cac
            Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer); // [web:32]

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
        if (projectilePrefab == null)
            return;

        Vector3 spawnPos = projectileSpawnPoint != null
            ? projectileSpawnPoint.position
            : transform.position + transform.forward * 1f;

        // Direction = là où le player regarde
        Vector3 forward = transform.forward;
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.001f)
            forward = Vector3.forward;

        Quaternion spawnRot = Quaternion.LookRotation(forward, Vector3.up); // [web:50]

        GameObject proj = Instantiate(projectilePrefab, spawnPos, spawnRot);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = proj.transform.forward * projectileSpeed;
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

        if (isHelmetEquipped())
            maxHealth += 40f;

        if (isChestplateEquipped())
            maxHealth += 200f;

        if (isLeggingsEquipped())
            maxHealth += 80f;

        if (isBootsEquipped())
            moveSpeed += 2f;

        if (isAccessoryEquipped())
            attackPower += 40f;

        if (health < maxHealth)
            health = maxHealth;
    }

    public bool isWeaponEquipped() => slotWeapon;
    public bool isHelmetEquipped() => slotHelmet;
    public bool isBootsEquipped() => slotBoots;
    public bool isLeggingsEquipped() => slotLeggings;
    public bool isChestplateEquipped() => slotChestplate;
    public bool isAccessoryEquipped() => slotAccessory;
}

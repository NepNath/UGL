using UnityEngine;
public class baseEnnemi : entityClass
{
    [SerializeField] protected float aggroRange = 70f;
    [SerializeField] protected bool isAggro = false;
    [SerializeField] protected bool isElite = false;
    [SerializeField] protected bool useProjectile = false;
    [SerializeField] protected bool isInRange = false;
    [SerializeField] protected float projectileSpeed = 20f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float currentCooldown = 0f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    
    public void setEliteStatus(bool status)
    {

        if (Random.value <= 0.01f)
        {
            isElite = true;
        }
        if (isElite)
        {
            health *= 1.5f;
            maxHealth *= 1.5f;
            attackPower *= 1.5f;
            speed *= 1.2f;
            attackRange *= 1.2f;
            aggroRange *= 1.2f;
        }  
    }

    public void aggroStatus()
    {
        targetPlayer targetPlayerScript = GetComponent<targetPlayer>();
        GameObject player = targetPlayerScript.getClosestPlayer(aggroRange);

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= aggroRange)
            {
                isAggro = true;
            }
            else
            {
                isAggro = false;
            }
        }
        else
        {
            isAggro = false;
        }
    }

    public void attackCooldownTimer()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

       
        if (currentCooldown <= 0)
        {
            attackPlayer();
            currentCooldown = attackCooldown;
        }
    }

    public void attackPlayer()
    {
        if (!isAggro) return;

        targetPlayer targetPlayerScript = GetComponent<targetPlayer>();
        GameObject player = targetPlayerScript.getClosestPlayer(aggroRange);

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= attackRange)
            {
                isInRange = true;
                if (useProjectile)
                {
                    shootProjectile(player);
                }
                else
                {
                    entityClass playerEntity = player.GetComponent<entityClass>();
                    if (playerEntity != null)
                    {
                        dealDamaged(playerEntity);
                    }
                }
            }
            else
            {
                isInRange = false;
            }
        }
    }

    public void shootProjectile(GameObject target)
    {
        if (projectilePrefab == null)
        return;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile projScript = projectile.GetComponent<projectile>();
        if (projScript != null)
        {
            projScript.Initialize(target.transform, attackPower);
        }
        
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (target.transform.position - projectileSpawnPoint.position).normalized;
            rb.linearVelocity = direction * projectileSpeed;
        }
    }
    
}

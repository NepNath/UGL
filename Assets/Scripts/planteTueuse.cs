using UnityEngine;

public class planteTueuse : baseEnnemi
{
    
    void Start()
    {
        health = 100f;
        maxHealth = 100f;
        attackPower = 10f;
        speed = 3f;
        attackRange = 60f;
        aggroRange = 95f;
        useProjectile = true;
        projectileSpeed = 25f;
        setEliteStatus(isElite);
    }

    
    void Update()
    {
        aggroStatus();
        attackCooldownTimer();
    }
}

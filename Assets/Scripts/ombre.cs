using UnityEngine;

public class ombre : baseEnnemi
{
    
    void Start()
    {
        health = 100f;
        maxHealth = 100f;
        attackPower = 10f;
        speed = 3f;
        attackRange = 40f;
        aggroRange = 95f;
        setEliteStatus(isElite);
    }

    
    void Update()
    {
        aggroStatus();
    }
}

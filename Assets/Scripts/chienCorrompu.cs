using UnityEngine;

public class chienCorrompu : baseEnnemi
{
    
    void Start()
    {
        health = 150f;
        maxHealth = 150f;
        attackPower = 25f;
        speed = 7f;
        attackRange = 10f;
        aggroRange = 75f;
        setEliteStatus(isElite);
    }

    
    void Update()
    {
        aggroStatus();
    }
}

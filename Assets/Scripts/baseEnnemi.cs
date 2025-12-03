using UnityEngine;
public class baseEnnemi : entityClass
{
    float aggroRange = 70f;
    bool isAggro = false;
    bool isElite = false;
    
    public void setEliteStatus(bool status)
    {

        if (random.NextDouble() < 0.01)
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
        TargetPlayer targetPlayerScript = GetComponent<TargetPlayer>();
        GameObject player = targetPlayerScript.GetClosestPlayer();

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


}

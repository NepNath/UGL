using UnityEngine;
using UnityEngine.AI;

public class targetPlayer : MonoBehaviour
{

    private GameObject player;
    public string playerTag = "Player";
    public NavMeshAgent agent;
    void Start()
    {
        player = getClosestPlayer(70.0f);
    }

    void Update()
    {
        player = getClosestPlayer(70.0f);
        if (player != null)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    public GameObject getClosestPlayer(float aggroRange)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        Vector3 currentPos = transform.position;

        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(currentPos, player.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = player;
            }
        }
        if (Vector3.Distance(currentPos, nearest.transform.position) < aggroRange)
        {
            return nearest;
        }
        else
        {
            return null;
        }
    }
}

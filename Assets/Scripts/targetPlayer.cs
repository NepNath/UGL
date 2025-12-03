using UnityEngine;
using UnityEngine.AI;

public class targetPlayer : MonoBehaviour
{

    private GameObject player;
    public string playerTag = "Player";
    public NavMeshAgent agent;
    void Start()
    {
        player = getClosestPlayer();
    }

    void Update()
    {
        player = getClosestPlayer();
        if (player != null)
        {
            agent.SetDestination(Player.transform.position);
        }
    }

    public GameObject getClosestPlayer()
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
        if (Vector3.Distance(currentPos, nearest.transform.position) < 30f)
        {
            return nearest;
        }
        else
        {
            return null;
        }
    }
}

using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject bossRoomPrefab;

    void Start()
    {
        Invoke(nameof(FinishGeneration), 1f);
    }

    void FinishGeneration()
    {
       // RoomManager.instance.ReplaceFurthestRoomWithBoss(bossRoomPrefab);
    }
}

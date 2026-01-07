using UnityEngine;

public class RoomDatabase : MonoBehaviour
{
    public static RoomDatabase instance;
    public GameObject[] normalRooms;

    void Awake()
    {
        instance = this;
    }

    public GameObject GetRandomRoom()
    {
        return normalRooms[Random.Range(0, normalRooms.Length)];
    }
}

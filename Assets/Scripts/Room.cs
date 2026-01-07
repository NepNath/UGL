using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isStartRoom = false;

    void Start()
    {
        if (!isStartRoom)
            RoomManager.instance.RegisterRoom(this);
    }
}

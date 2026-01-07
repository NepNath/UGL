using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    public int maxRooms = 15;          // nombre total de salles normales + boss
    public GameObject bossPrefab;      // prefab de la salle de boss (assigner dans l’inspector)

    public List<Room> allRooms = new List<Room>();
    private bool bossSpawned = false;

    void Awake()
    {
        instance = this;
    }

    // Vérifie si on peut générer une salle normale
    public bool CanSpawnNormalRoom()
    {
        // maxRooms - 1 car le boss sera généré après
        return allRooms.Count < maxRooms - 1;
    }

    public void RegisterRoom(Room room)
    {
        allRooms.Add(room);

        // Si toutes les salles normales ont été générées, spawn le boss
        if (allRooms.Count == maxRooms - 1 && !bossSpawned)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        if (bossSpawned) return;

        bossSpawned = true;

        // La dernière salle normale devient le boss
        Room lastRoom = allRooms[allRooms.Count - 1];
        Vector3 pos = lastRoom.transform.position;

        Destroy(lastRoom.gameObject);

        GameObject bossRoom = Instantiate(bossPrefab, pos, Quaternion.identity);
        bossRoom.tag = "Boss"; // assure qu'il n'y a qu'un seul boss
        Room roomComp = bossRoom.GetComponent<Room>();
        if (roomComp != null)
            RegisterRoom(roomComp); // ajoute le boss à la liste
    }
}

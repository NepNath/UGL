using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool spawned;
    public LayerMask roomLayer;
    public float checkRadius = 1.5f;
    public float roomSize = 10f; // distance pour que les salles se touchent

    void Start()
    {
        // petit délai aléatoire pour éviter que plusieurs SpawnPoints spawnent en même temps
        Invoke(nameof(TrySpawn), Random.Range(0.05f, 0.2f));
    }

    void TrySpawn()
    {
        if (spawned) return;

        // Vérifie si on peut encore générer des salles normales
        if (!RoomManager.instance.CanSpawnNormalRoom())
        {
            spawned = true; // toutes les salles normales générées ? pas de spawn
            return;
        }

        Vector3 direction = transform.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 spawnPos = transform.position + direction * roomSize;
        spawnPos.y = 0;

        // Vérifie si une salle existe déjà à cet endroit
        Collider[] hits = Physics.OverlapSphere(spawnPos, checkRadius, roomLayer);
        if (hits.Length > 0)
        {
            spawned = true;
            return;
        }

        // Génération d'une salle normale
        GameObject newRoom = Instantiate(RoomDatabase.instance.GetRandomRoom(),
                                         spawnPos,
                                         Quaternion.identity);

        // Enregistre la salle dans RoomManager
        Room roomComp = newRoom.GetComponent<Room>();
        if (roomComp != null)
            RoomManager.instance.RegisterRoom(roomComp);

        // Réinitialise les SpawnPoints de la nouvelle salle
        SpawnPoint[] childSpawns = newRoom.GetComponentsInChildren<SpawnPoint>();
        foreach (var sp in childSpawns)
            sp.spawned = false;

        spawned = true;
    }
}

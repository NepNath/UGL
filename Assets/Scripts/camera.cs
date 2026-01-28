using UnityEngine;

public class IsometricCameraFollower : MonoBehaviour
{
    [Header("Cible")]
    [SerializeField] private Transform playerTransform;
    
    [Header("Position")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 8, -8);
    [SerializeField] private float smoothSpeed = 5f;
    
    [Header("Limites (optionnel)")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector3 boundsMin = new Vector3(-50, 0, -50);
    [SerializeField] private Vector3 boundsMax = new Vector3(50, 0, 50);

    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero; // ← NOUVEAU

    private void LateUpdate()
{
    if (playerTransform == null)
        return;

    // Calculer la position cible : joueur + offset fixe
    targetPosition = playerTransform.position + cameraOffset;

    // Appliquer les limites si activées
    if (useBounds)
    {
        targetPosition.x = Mathf.Clamp(targetPosition.x, boundsMin.x, boundsMax.x);
        targetPosition.z = Mathf.Clamp(targetPosition.z, boundsMin.z, boundsMax.z);
    }

    // Suivi direct sans interpolation
    transform.position = targetPosition;
}

    private void OnDestroy()
    {
        // Defensive logging to help track down MissingReferenceException
        Debug.Log($"IsometricCameraFollower on '{gameObject.name}' was destroyed.");

        // Clear target reference to avoid other systems keeping a stale link
        playerTransform = null;
    }
}
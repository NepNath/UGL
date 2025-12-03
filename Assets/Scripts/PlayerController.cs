using UnityEngine;

public class IsometricController : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float accelerationMultiplier = 1.5f;

    [Header("Références")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody rb;

    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;

    // Matrice isométrique pour transformer les inputs
    private static readonly Matrix4x4 isometricMatrix = Matrix4x4.Rotate(
        Quaternion.Euler(0, 45, 0)
    );

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }
    private void Update()
    {
        GatherInput();
        HandleRotation();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void GatherInput()
    {
        // Récupérer l'input brut WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 rawInput = new Vector3(horizontal, 0, vertical);

        // Transformer l'input avec la matrice isométrique
        moveDirection = isometricMatrix.MultiplyVector(rawInput);

        // Normaliser si direction != 0
        if (moveDirection.magnitude > 0.1f)
        {
            moveDirection.Normalize();
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    private void HandleRotation()
    {
        // Ne pas tourner si pas de mouvement
        if (moveDirection.magnitude < 0.1f)
            return;

        // Créer la direction de rotation souhaitée
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        // Interpoler vers la rotation cible
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void ApplyMovement()
    {
        // Calculer la vélocité cible
        Vector3 targetVelocity = moveDirection * moveSpeed;

        // Appliquer accélération pour un mouvement plus naturel
        if (moveDirection.magnitude > 0.1f)
        {
            targetVelocity *= accelerationMultiplier;
        }

        // Interpoler vers la vélocité cible
        rb.linearVelocity = Vector3.Lerp(
            rb.linearVelocity,
            targetVelocity,
            Time.fixedDeltaTime * 5f
        );
    }

    /// <summary>
    /// <summary>
    /// Méthode utilitaire pour transformer un vecteur en isométrique
    /// Utilisable : Vector3 input = IsometricController.ToIso(rawInput);
    /// </summary>
    public static Vector3 ToIso(Vector3 input)
    {
        return isometricMatrix.MultiplyVector(input);
    }
}
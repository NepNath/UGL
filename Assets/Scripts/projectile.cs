using UnityEngine;

public class projectile : MonoBehaviour
{
    private Transform target;
    private float damage;
    private float lifetime = 5f;
    private float timer = 0f;

    public void Initialize(Transform targetTransform, float damageAmount)
    {
        target = targetTransform;
        damage = damageAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        entityClass entity = other.GetComponent<entityClass>();
        if (entity != null)
        {
            entity.getDamaged(damage);
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
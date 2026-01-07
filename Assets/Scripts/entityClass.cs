using UnityEngine;

public abstract class entityClass: MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float attackPower;
    [SerializeField] protected float speed;
    [SerializeField] protected float attackRange;
    /*float def;*/

    public virtual void GetDamaged(float amount)
    {
        if (amount <= 0f) return;

        health -= amount;

        if (health < 0f)
            health = 0f;
    }

    public virtual void getDamaged(float amount)
    {
        GetDamaged(amount);
    }

    public virtual void Heal(float amount)
    {
        if (amount <= 0f) return;

        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }
    public virtual void dealDamaged(entityClass target){
        target.getDamaged(attackPower);
    }
    public virtual bool isDead(){
        return health <= 0;
    }

    public virtual bool healFullHealth(){
        health = maxHealth;
        return true;
    }
    public virtual bool isFullyHealed(){
        return health == maxHealth;
    }

}
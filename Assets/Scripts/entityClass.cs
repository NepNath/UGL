using UnityEngine;

public abstract class entityClass: MonoBehaviour
{

    protected float health;
    protected float maxHealth;
    protected float attackPower;
    protected float speed;
    protected float attackRange;
    /*float def;*/
    public virtual void getDamaged(float amount)
    {
        if (amount <= 0f) return;

        health -= amount;
        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    public virtual void heal(float amount)
    {
        if (amount <= 0f) return;

        health += amount;
        health = Mathf.Clamp(health, 0f, maxHealth);
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
        return healthleft == maxHealth;
    }
    public abstract void move(Vector3 direction);

}

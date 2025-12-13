using UnityEngine;

public abstract class entityClass: MonoBehaviour
{
    protected float health;
    protected float maxHealth;
    protected float attackPower;
    protected float speed;
    protected float attackRange;
    /*float def;*/
    public virtual void getDamaged(int amount){

        float healthLeft = health - amount;
        if (isFullyHealed() == true){
            amount = 0;
            health -= amount;
        }
        if (healthLeft > maxHealth){
            health = maxHealth;
        }
        else{
            health -= amount;
            }
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

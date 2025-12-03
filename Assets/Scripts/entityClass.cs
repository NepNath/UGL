using UnityEngine;

public abstract class entityClass : MonoBehaviour
{
    float health;
    float maxHealth;
    float attackPower;
    float speed;
    float attackRange;
    /*float def;*/
    public abstract void getDamaged(int amount){
        if (isFullyHealed() == true){
            amount = 0;
            health -= amount;
        }
        if ((health -= amount ) > maxHealth){
            health = maxHealth;
        }
        else{
            health -= amount;
            }
    }
    public abstract void dealDamaged(entityClass target){
        target.getDamaged(attackPower);
    }
    public abstract bool isDead(){
        return health <= 0;
    }

    public abstract bool healFullHealth(){
        health = maxHealth;
        return true;
    }
    public abstract bool isFullyHealed(){
        return health == maxHealth;
    }
}

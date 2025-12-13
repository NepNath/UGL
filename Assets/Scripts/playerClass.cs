using UnityEngine;

public class playerClass : entityClass
{

    
    private bool slotWeapon;
    private bool slotHelmet;
    private bool slotBoots;
    private bool slotleggings;
    private bool slotChestplate;
    private bool slotAccessory;



    public bool isWeaponEquipped(){
        if (slotWeapon != ""){
            return true;
        }
        else{
            return false;
        }
    }

    public bool isHelmetEquipped(){
        if (slotHelmet != ""){
            return true;
        }
        else{
            return false;
        }
    }

    public bool isBootsEquipped(){
        if (slotBoots != ""){
            return true;
        }
        else{
            return false;
        }
    }

    public bool isLeggingsEquipped(){
        if (slotleggings != ""){
            return true;
        }
        else{
            return false;
        }
    }

    public bool isChestplateEquipped(){
        if (slotChestplate != ""){
            return true;
        }
        else{
            return false;
        }
    }

    public bool isAccessoryEquipped(){
        if (slotAccessory != ""){
            return true;
        }
        else{
            return false;
        }
    }


}

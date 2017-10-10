using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    // the position where the weapon will be instantiated
    public Transform weaponHold;
    // the gun prefab to start with
    public Gun startingGun;
    // a ref to the gun equipped
    Gun equippedGun;

    void Start()
    {
        // if a prefab is put, equip it 
        if (startingGun != null)
        {
            EquipGun(startingGun);
        }
    }

    public void EquipGun(Gun gunToEquip)
    {
        // if a gun is equipped replace it by destroying the previous one
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        // instantiate, the parent can be set directly in the instiante method
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation, weaponHold) as Gun;
        
    }

    public void Shoot()
    {
        // if we have a gun shoot
        if (equippedGun != null)
        {
            equippedGun.Shoot();
        }
    }


}

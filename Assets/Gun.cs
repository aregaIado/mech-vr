using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FireMode { Semi, Burst, Auto};
public class Gun : MonoBehaviour
{
    [Header("Weapon Settings")]
    public FireMode mode = FireMode.Semi;
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;
    public int magSize = 10;
    public bool infiniteAmmo = false;

    [Header("Weapon Sources")]
    public Transform bulletSpawn;
    public GameObject impactMesh;
    public GameObject muzzleflash;
    public GameObject impactEffect;
    //[SerializeField] private GameObject magazine;
    //public Text ammoCounter;

    bool isTriggerDown = false;
    bool fullAuto = false;
    bool burst = false;
    bool outOfAmmo = false;
    float nextTimeToFire = 0f;
    int currentAmmoCount;

    private void Start()
    {
        RefillAmmo();
    }
    private void Update()
    {
        if (isTriggerDown == true)
        {
            switch (mode)
            {
                case FireMode.Auto:
                    //fullAuto = true;
                    FireAction();
                    break;

                default:
                    fullAuto = false;
                    FireAction();
                    isTriggerDown = false;
                    break;
            }
        }
    }
    private bool CanFire()
    {
        if (Time.time >= nextTimeToFire) return false;
        if (burst == true) return false;
        return true;
    }
    private void FireAction()
    {
        if (Time.time >= nextTimeToFire && burst == false) //Time.time >= nextTimeToFire && burst == false
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            //UpdateConsole("FireAction() before Fire()");
            Fire();
            //UpdateConsole("FireAction() after Fire()");

            //burst fire
            if (mode == FireMode.Burst)
            {
                //UpdateConsole("should be bursting \nMode: " + mode);
                burst = true;
                StartCoroutine(BurstFire());
            }
        }

    }
    private void Fire()
    {
        if (outOfAmmo != true)
        {
            //UpdateConsole("outOfAmmo != true");
            //create muzzleflash
            var flash = Instantiate(muzzleflash, bulletSpawn);
            Destroy(flash, 0.2f);

            //decrease ammo counter
            if (infiniteAmmo != true)
            {
                currentAmmoCount = currentAmmoCount - 1;
            }
            UpdateAmmoStatus();

            RaycastHit hit;
            if (Physics.Raycast(bulletSpawn.position, bulletSpawn.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
                //UpdateConsole("beginning of raycast");
                //spawn object at hit.point
                GameObject spawnedBullet = Instantiate(impactMesh, hit.point, bulletSpawn.rotation);
                spawnedBullet.transform.SetParent(hit.transform);

                //addforce to rigidbody if hitobject has one
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(bulletSpawn.forward * impactForce);
                }

                var spawn = Instantiate(impactEffect, hit.point, Quaternion.Euler(hit.point.normalized));
                Destroy(spawn, 0.25f);

                Destroy(spawnedBullet, 2);


                //do damage
                /*if (hit.transform.GetComponent<HealthProperties>() == true)
                {
                    hit.transform.GetComponent<HealthProperties>().TakeDamage(damage);
                }
                else if (hit.transform.parent && hit.transform.parent.GetComponent<HealthProperties>() == true)
                {
                    hit.transform.parent.GetComponent<HealthProperties>().TakeDamage(damage);
                }*/
            }

        }
        //UpdateConsole("End of Fire()");
    }
    private IEnumerator BurstFire()
    {
        //UpdateConsole("BurstFire()");
        yield return new WaitForSeconds(1f / fireRate);
        Fire();
        yield return new WaitForSeconds(1f / fireRate);
        Fire();
        yield return new WaitForSeconds((1f / fireRate) + 0.1f);
        burst = false;
    }
    public void TriggerDown()
    {
        isTriggerDown = true;
    }
    public void TriggerUp()
    {
        isTriggerDown = false;
    }
    public void RefillAmmo()
    {
        //magazine.GetComponent<MeshRenderer>().enabled = true;
        burst = false;
        if (!infiniteAmmo)
        {
            currentAmmoCount = magSize;
        }
        else
        {
            magSize = 99;
            currentAmmoCount = magSize;
        }
        //UpdateAmmoStatus();
    }
    private void UpdateAmmoStatus()
    {
        //should work for both refil and fire functions
        if (currentAmmoCount <= 0)
        {
            outOfAmmo = true;
            currentAmmoCount = 0;
        }
        else
            outOfAmmo = false;

        UpdateAmmoCounter();
    }
    private void UpdateAmmoCounter()
    {
        //ammoCounter.text = currentAmmoCount.ToString();
    }
}

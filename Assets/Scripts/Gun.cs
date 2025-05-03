using System;
using UnityEngine;


public class Gun : MonoBehaviour
{
    public static event EventHandler OnAnyShoot;
    public static event EventHandler<OnAnyBulletChangeEventArgs> OnAnyBulletChange;
    public class OnAnyBulletChangeEventArgs : EventArgs
    {
        public int magazineBullets;
        public int fullBullets;
    }

    [SerializeField] private Camera fpsCamera;
    [SerializeField] private Transform shootParticleEffect;

    [Header("Shooting")]
    [SerializeField] private Player player;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Transform gunTip;

    [Header("BulletCounting")]
    [SerializeField] private int magazineCapacity = 8;
    [SerializeField] private int maxBullets = 50;
    [SerializeField] private int magazineBullets = 8;
    [SerializeField] private int fullBullets = 50;
    private int shotsFired = 0;


    private float nextTimeToFire = 0f;

    private float reloadTime = 3f;
    private bool isAbleToShoot = true;

    private void Start()
    {
        GameInput.Instance.OnReload += GameInput_OnReload;
    }

    

    // Update is called once per frame
    void Update() {
        
        if (player.enabled == false)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire )
        {
            nextTimeToFire = Time.time +1f / fireRate;   
            Shoot();
        }
                
   }

    void Shoot()
    {
        if (!isAbleToShoot || magazineBullets <= 0) return;
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
        magazineBullets--;
        shotsFired++;
        if (magazineBullets <= 0)
        {
            isAbleToShoot = false;
            Invoke(nameof(Reload), reloadTime);
        }

        Transform shootParticleEffectTransform = Instantiate(shootParticleEffect, gunTip.position, gunTip.rotation);
        OnAnyShoot?.Invoke(this, EventArgs.Empty);
        OnAnyBulletChange?.Invoke(this, new OnAnyBulletChangeEventArgs
        {
            magazineBullets = magazineBullets,
            fullBullets = fullBullets
        });
    }
    private void GameInput_OnReload(object sender, EventArgs e)
    {
        if (isAbleToShoot && magazineBullets < magazineCapacity)
        {
            isAbleToShoot = false;
            Invoke(nameof(Reload), reloadTime);
        } 
    }
    private void Reload()
    {
        if (fullBullets >= magazineCapacity)
        {
            magazineBullets = magazineCapacity;
        }
        else
        {
            magazineBullets = fullBullets;
        }
        fullBullets -= shotsFired;
        shotsFired = 0;
        if (fullBullets <= 0)
        {
            fullBullets = 0;
        }
        OnAnyBulletChange?.Invoke(this, new OnAnyBulletChangeEventArgs
        {
            magazineBullets = magazineBullets,
            fullBullets = fullBullets
        });
        isAbleToShoot = true;
    }
    public Transform GetGunTip()
    {
        return gunTip;
    }
}
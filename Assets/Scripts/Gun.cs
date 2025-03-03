using UnityEngine;


public class Gun : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 10000f;
    [SerializeField] private Transform gunTip;


    private float nextTimeToFire = 0f;
   
        
  // Update is called once per frame
  void Update() {
  
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire )
        {
            nextTimeToFire = Time.time +1f / fireRate;   
            Shoot();
        }
                
   }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(gunTip.position, gunTip.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
                Debug.Log(target.health);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bulletCounterText;
    private void Start()
    {
        Gun.OnAnyBulletChange += Gun_OnAnyBulletChange;
    }

    private void Gun_OnAnyBulletChange(object sender, Gun.OnAnyBulletChangeEventArgs e)
    {
        bulletCounterText.text = e.magazineBullets + "/" + e.maxBullets;
    }
}

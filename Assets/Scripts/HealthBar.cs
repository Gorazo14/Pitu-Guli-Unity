using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Start()
    {
        transform.parent.GetComponent<Target>().OnTargetHit += HealthBar_OnTargetHit;
    }

    private void HealthBar_OnTargetHit(object sender, Target.OnTargetHitEventArgs e)
    {
        image.fillAmount = e.healthNormalized;
    }
}

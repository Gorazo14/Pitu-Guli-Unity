using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Start()
    {
        transform.parent.GetComponent<Target>().OnTargetHit += Target_OnTargetHit;
    }

    private void Target_OnTargetHit(object sender, Target.OnTargetHitEventArgs e)
    {
        image.fillAmount = e.healthNormalized;
    }
}

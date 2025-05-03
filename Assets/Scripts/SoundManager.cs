using GDL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundEffectsSO soundEffectSO;
    private void Start()
    {
        Gun.OnAnyShoot += Gun_OnAnyShoot;
        EnemyShooter.OnAnyEnemyShoot += EnemyShooter_OnAnyEnemyShoot;
    }

    private void EnemyShooter_OnAnyEnemyShoot(object sender, System.EventArgs e)
    {
        EnemyShooter enemyShooter = sender as EnemyShooter;
        float volume = 1f;

        PlaySound(soundEffectSO.rifleShot, enemyShooter.GetShootPoint().position, volume);
    }

    private void Gun_OnAnyShoot(object sender, System.EventArgs e)
    {
        Gun gun = sender as Gun;
        float volume = 0.7f;

        PlaySound(soundEffectSO.rifleShot, gun.GetGunTip().position, volume);
    }

    private void PlaySound(AudioClip clip, Vector3 position, float volume=1f)
    {
        AudioSource.PlayClipAtPoint(clip, position, volume);    
    }
}

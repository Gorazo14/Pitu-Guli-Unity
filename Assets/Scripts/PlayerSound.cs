using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private Player player;
    private float timer;
    private float timerMax = .5f;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            timer = timerMax;

            if (player.IsPlayerMoving())
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootstepSound(transform.position, volume);
            }
        }
    }
}

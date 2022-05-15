using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBotWallCheck : MonoBehaviour
{
    private SpiderBot _spiderBot;

    private void Awake()
    {
        _spiderBot = transform.parent.GetComponent<SpiderBot>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") && _spiderBot.facingRight)
        {
            _spiderBot.Flip(-180);
        }
        else if (other.gameObject.CompareTag("Wall") && !_spiderBot.facingRight)
        {
            _spiderBot.Flip(0);
        }
    }
}

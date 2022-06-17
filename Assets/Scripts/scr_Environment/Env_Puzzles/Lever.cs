using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject mirror;
    [SerializeField] private LayerMask whatisPlayer;
    private void OnMouseUp()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, 1, whatisPlayer);
            if (player.CompareTag("Player"))
                mirror.GetComponent<Mirror>().Toggle();
    }
}

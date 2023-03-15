using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    HPHandler hp;

    private void Awake()
    {
        hp = FindObjectOfType<HPHandler>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        BirdPool.ReturnToPoolPos(col.transform);

        hp.TakeDamage();
    }
}

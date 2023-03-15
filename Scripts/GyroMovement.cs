using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroMovement : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        Input.gyro.enabled = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.acceleration.x * 5, 0);
    }
}

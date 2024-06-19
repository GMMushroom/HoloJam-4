using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float speed = 5f;
    private float _vertical;
    private float _horizontal;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _vertical = Input.GetAxisRaw("Vertical");
        _horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_horizontal, _vertical).normalized * speed;
    }
}
    
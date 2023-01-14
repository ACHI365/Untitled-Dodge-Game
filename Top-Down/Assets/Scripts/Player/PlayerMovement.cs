using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // movement
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 movement;
    
    // camera
    [SerializeField] private Camera cam;
    private Vector2 mousePos;
    [SerializeField] private float offset;
    
    // aim
    [SerializeField] private GameObject aim;
    
    // animator
    [SerializeField] private Animator _animator;
    
    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        aim.GetComponent<Rigidbody2D>().position = mousePos;
        
        _animator.SetBool("isMoving", movement.x != 0 || movement.y != 0);
    }
    
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - offset;

        rb.rotation = angle;
    }
}

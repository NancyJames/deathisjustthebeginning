using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D myBody;
    Animator myAnimator;
    Vector2 moveInput;
    [SerializeField] float speed = 25f;
    [SerializeField] float jumpForce = 5f;
    float initialGravity;
    bool isAlive = true;
    [SerializeField] Transform gun;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float gunForce;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
        initialGravity = myBody.gravityScale;
    }


    private void FixedUpdate()
    {
        if (isAlive)
        {
            Run();
            Climb();
            FlipSprite();
        }

    }

    IEnumerator Die()
    {
        if (!isAlive) { yield break; }
        isAlive = false;
        myBody.velocity = new Vector2(0, 0);
        myAnimator.SetTrigger("isRolling");
        yield return new WaitForSeconds(1f);
       // FindObjectOfType<GameSession>().ProcessPlayerDeath();

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            StartCoroutine(Die());
        }
    }

    private void Climb()
    {
        if (GetComponent<CapsuleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            myBody.gravityScale = 0;
            myBody.velocity = new Vector2(myBody.velocity.x, moveInput.y * speed * Time.deltaTime);
            myAnimator.SetBool("isClimbing", Mathf.Abs(myBody.velocity.y) > Mathf.Epsilon);
        }
        else
        {
            myBody.gravityScale = initialGravity;
        }
    }

    private void Run()
    {
        myAnimator.SetBool("isClimbing", false);
        myBody.velocity = new Vector2(moveInput.x * speed * Time.deltaTime, myBody.velocity.y);
        // myAnimator.SetBool("isRunning", Mathf.Abs(myBody.velocity.x) > Mathf.Epsilon);
        if (Mathf.Abs(myBody.velocity.x) > Mathf.Epsilon) { myAnimator.SetTrigger("isRunning"); }
        else
        {
            myAnimator.SetTrigger("isIdle");
        }
    }

    private void FlipSprite()
    {
        // bool playerMoving;
        if (Mathf.Abs(myBody.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Sign(myBody.velocity.x), 1);
        }

    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed && GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Platforms")))
        {
            // myBody.velocity += new Vector2(0f, jumpForce);
            myBody.AddForce(Vector2.up * jumpForce);
            if (Mathf.Abs(myBody.velocity.x) > Mathf.Epsilon)
            {
                myAnimator.SetTrigger("isJumping");
            }
            
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed)
        {
           // GameObject bullet = Instantiate(bulletPrefab, gun.position, transform.rotation);
            //bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x * gunForce, 0f));
        }

    }
}

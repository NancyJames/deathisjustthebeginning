using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using HR.Utilities.Variables;
using HR.Utilities.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D myBody;
    [SerializeField] float speed = 250f;
    [SerializeField] float jumpForce = 50f;
    Vector2 moveInput;
    bool canMove = true;
    [SerializeField] FloatVariable health;
    [SerializeField] FloatVariable energy;
    [SerializeField] IntVariable debuffs;

    [SerializeField] float jumpCost;
    [SerializeField] float regenFactor=75f;

    float timeSinceJumped = 0f;
    float jumpClearance = 0.2f;

    bool grounded = true;

    Collider2D myFeet;

    ColorAdjustments colorAdjustments;
    Volume cameraVolume;
    List<float> debuffAmounts = new();
    List<float> debuffTimers = new();

    [Header("Post Processing")]
    [SerializeField] bool desaturateByHeight = false;
    [SerializeField] bool realityFlashes = false;
    [SerializeField] bool colorize = false;
    



    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        myFeet = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        cameraVolume = Camera.main.GetComponent<Volume>();
        VolumeProfile profile = cameraVolume.sharedProfile;
        profile.TryGet(out colorAdjustments);
        //initialGravity = myBody.gravityScale;
    }



    private void FixedUpdate()
    {
        CheckGrounded();
        if (canMove)
        {
            Run();
            FlipSprite();  
        }
        Regenerate();

        if(desaturateByHeight)
        {
            SetSaturation();
        }
        
        UpdateDebuffTimers();

    }

    private void CheckGrounded()
    {
        timeSinceJumped += Time.deltaTime;
        if(!grounded && myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))&&timeSinceJumped>jumpClearance)
        {
            grounded = true;
        }
    }

    private void UpdateDebuffTimers()
    {
        int debuffLength = debuffTimers.Count-1;
        for(int i = debuffLength; i>=0;i--)
        {
            debuffTimers[i] -= Time.deltaTime;
            if(debuffTimers[i]<=0)
            {
                debuffTimers.RemoveAt(i);
                RemoveSlow(debuffAmounts[i]);
                debuffAmounts.RemoveAt(i);
                debuffs.Decrement();
            }
        }
    }

    private void SetSaturation()
    {
        
        if(colorAdjustments!=null)
        {
            float y = transform.position.y*2;
            colorAdjustments.saturation.value = Mathf.Clamp(-100f + y, -100f, 0);
        }
    }

    private void Regenerate()
    {
        energy.Increment(GetRegenRate() * Time.deltaTime);
    }

    private float GetRegenRate()
    {
        return (health.Get() / regenFactor);
    }

    private void Run()
    {
        if(grounded)
        {
            myBody.velocity = new Vector2(moveInput.x * speed * Time.deltaTime, myBody.velocity.y);
        }
         
        //Debug.Log(moveInput.x * speed * Vector2.right);
        //myBody.AddForce(moveInput.x * speed * Vector2.right, ForceMode2D.Force);

    }

    private void FlipSprite()
    {
        // bool playerMoving;
        if (Mathf.Abs(myBody.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Sign(myBody.velocity.x)*transform.localScale.x, transform.localScale.y);
        }

    }

    void OnMove(InputValue value)
    {
        if (!canMove) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {

        if (!canMove) { return; }
        if (value.isPressed && myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) &&HasEnergy(jumpCost))
        {
            grounded = false;
            timeSinceJumped = 0;
            SpendEnergy(jumpCost);
            //this is a fake change;
            //myBody.velocity += new Vector2(myBody.velocity.x, jumpForce)*Time.deltaTime;
            myBody.AddForce((moveInput*speed*10)*Time.deltaTime+ (Vector2.up * jumpForce), ForceMode2D.Impulse);


            if (Mathf.Abs(myBody.velocity.x) > Mathf.Epsilon)
            {
                //myAnimator.SetTrigger("isJumping");
            }

        }
    }

    bool HasEnergy(float cost)
    {
        if(energy.Get()>=cost)
        {
            return true;
        }
        return false;
    }

    void SpendEnergy(float amount)
    {
        energy.Decrement(amount);
    }

    public void Slow(float amount, float time)
    {
        myBody.gravityScale += amount;
        speed = Mathf.Clamp(speed - (amount * 5), 0, float.MaxValue);
        debuffAmounts.Add(amount);
        debuffTimers.Add(time);
        debuffs.Increment();
    }
    private void RemoveSlow(float amount)
    {
        myBody.gravityScale -= amount;
        speed+=amount*5;
    }


}

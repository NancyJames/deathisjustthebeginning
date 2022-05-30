using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using HR.Utilities.Variables;
using HR.Utilities.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PlayerController : MonoBehaviour
{
    
    [Header("Physics")]
    [SerializeField] float speed = 250f;
    [SerializeField] float jumpForce = 50f;
    Rigidbody2D myBody;
    Vector2 moveInput;
    [Header("Ability Flags")]
    bool canMove = true;
    [SerializeField] bool canAttack = false;
    [SerializeField] bool canJump=true;
    [Header("Shared Variables")]
    [SerializeField] FloatVariable health;
    [SerializeField] FloatVariable energy;
    [SerializeField] FloatVariable maxHealth;
    [SerializeField] FloatVariable maxEnergy;
    [SerializeField] IntVariable debuffs;
    [SerializeField] FloatVariable xpos;
    [SerializeField] FloatVariable cryCooldown;
    [SerializeField] FloatVariable currentCryCooldown;
    [Header("Special Abilities")]
    [SerializeField] float fireRadius = 5;
    [SerializeField] float attackCost = 10;
    [SerializeField] float jumpCost;
    [SerializeField] float regenFactor=75f;
    [SerializeField] float cryHealth = 15f;
    [SerializeField] float cryEnergy = 10f;
    [SerializeField] float cryDuration = 1.5f;
    [SerializeField] float goodMemoryEnergy = 10f;
    [SerializeField] float newLevelHealthBoost = 5f;
    [SerializeField] ParticleSystem boom;
    [SerializeField] ParticleSystem cryParticles;
    [SerializeField] ParticleSystem goodMemoryParticles;
    [SerializeField] CircleCollider2D cryForcefield;
    [SerializeField] List<StoryPoint_SO> cryStories = new();


    [Header("Other")]
    [SerializeField] GameEvent triggerPortalEvent;
    [SerializeField] GameEvent storyTriggered;
    [SerializeField] GameEvent cryTriggered;
    [Header("Finale")]
    [SerializeField] GameEvent finale;
    [SerializeField] Transform finishLocation;
    [SerializeField] float floatSpeed;
    bool isFloating;

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
    [SerializeField] [Range(-100,100)] float desaturationValue;
    [SerializeField] [Range(-100, 100)] float overSaturationValue;
    [SerializeField] bool colorize = false;
    [SerializeField] Color colorOverlay;
    [SerializeField] float flashSaturationValue = -100f;
    [SerializeField] float flashSaturationTime = 1f;

    Transform t;
    bool freezeMovement = false;

    //not used but keeps the variables persisting. Maybe only be required in the editor
    [Header("Level Visits")]
    [SerializeField] IntVariable depressionVisited;
    [SerializeField] IntVariable denialVisited;
    [SerializeField] IntVariable angerVisited;
    [SerializeField] IntVariable regretVisited;

    //rare bug where player can fall through floor even though collision detection is continuous. Bit of a kludge but don't want player stuck
    [Header("Falling Through Floor Kludge")]
    [SerializeField] float minY=3.5f;
    [SerializeField] float maxY=14f;

    PlayerInput playerInput;
    MenuManager menuManager;
    



    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        myFeet = GetComponentInChildren<CircleCollider2D>();
        playerInput = GetComponent<PlayerInput>();
        menuManager = FindObjectOfType<MenuManager>();
        Cursor.visible = false;
    }

    private void Start()
    {
        cameraVolume = Camera.main.GetComponent<Volume>();
        VolumeProfile profile = cameraVolume.sharedProfile;
        profile.TryGet(out colorAdjustments);
        t = transform;
        //initialGravity = myBody.gravityScale;
        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.overrideState = false;
            colorAdjustments.colorFilter.overrideState = false;
            if (colorize)
            {
                colorAdjustments.colorFilter.overrideState = true;
                colorAdjustments.colorFilter.value = colorOverlay;
            }
            if (realityFlashes)
            {
                colorAdjustments.saturation.overrideState = true;
                colorAdjustments.saturation.value = overSaturationValue;
            }
            if(desaturateByHeight)
            {
                colorAdjustments.saturation.overrideState = true;
            }

        }
        
    }

    private void Update()
    {
        if (desaturateByHeight)
        {
            SetSaturation();
        }
        Regenerate();
        CooldownCry();
        UpdateDebuffTimers();
        CheckIfFallenThroughFloor();
    }

    private void CheckIfFallenThroughFloor()
    {
        //only happens in anger level
        if(canAttack)
        {
            if(t.position.y<minY)
            {
                t.position = new Vector3(t.position.x, maxY, t.position.z);
            }
        }
    }

    private void CooldownCry()
    {
        if(currentCryCooldown.Get()>0)
        {
            currentCryCooldown.Decrement(Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        xpos.Set(t.position.x);
        CheckGrounded();

        Run();
        FlipSprite();
        Float();
        

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

    public void RealityFlash()
    {
        if(colorAdjustments!=null)
        {
            StartCoroutine(FlashSaturation());
        }
    }
    IEnumerator FlashSaturation()
    {
        float startingSaturation = colorAdjustments.saturation.value;
        colorAdjustments.saturation.value = flashSaturationValue;
        yield return new WaitForSeconds(flashSaturationTime);
        colorAdjustments.saturation.value = startingSaturation;
    }

    private void Regenerate()
    {
        if(energy.Get()<maxEnergy.Get())
        {
            energy.Increment(GetRegenRate() * Time.deltaTime);
        }
        
    }

    private float GetRegenRate()
    {
        return (health.Get() / regenFactor);
    }

    private void Run()
    {
        if(freezeMovement)
        {
            myBody.velocity = new Vector2(0, 0);
            return;
        }
        if(grounded && myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            float newXVel = moveInput.x * speed * Time.deltaTime;
            myBody.velocity = new Vector2(newXVel, myBody.velocity.y);
        }
    }
    private void Float()
    {
        if(isFloating && finishLocation!=null)
        {
            transform.position = Vector2.MoveTowards(transform.position, finishLocation.position, Time.deltaTime * floatSpeed);
        }
        
    }

    private void FlipSprite()
    {
        // bool playerMoving;
        if (Mathf.Abs(myBody.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Sign(myBody.velocity.x)*transform.localScale.x, transform.localScale.y);
        }

    }

    void OnFire(InputValue value)
    {
        
        if(value.isPressed && canAttack && HasEnergy(attackCost))
        {
            boom.Play();
            SpendEnergy(attackCost);
            RaycastHit2D[] hits = Physics2D.CircleCastAll(t.position, fireRadius, new Vector2(0, 0), 0, LayerMask.GetMask("Enemy"));
            foreach(RaycastHit2D hit in hits)
            {
                hit.collider.gameObject.GetComponent<Enemy>().KillMe();
            }
        }
    }

    void OnMove(InputValue value)
    {
        if (!canMove) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        
        if (!canMove || ! canJump) { return; }
        if (value.isPressed && myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) &&HasEnergy(jumpCost))
        {
            grounded = false;
            timeSinceJumped = 0;
            SpendEnergy(jumpCost);
            //this is a fake change;
            //myBody.velocity += new Vector2(myBody.velocity.x, jumpForce)*Time.deltaTime;
            myBody.AddForce(new Vector2(moveInput.x*speed*0.02f,0f)+ (Vector2.up * jumpForce), ForceMode2D.Impulse);

            if (Mathf.Abs(myBody.velocity.x) > Mathf.Epsilon)
            {
                //myAnimator.SetTrigger("isJumping");
            }

        }
    }

    void OnCry(InputValue value)
    {
        if(canAttack && currentCryCooldown.Get()<=0)
        {
            if(cryStories.Count>0)
            {
                int index = Random.Range(0, cryStories.Count);
                StoryPoint_SO story = cryStories[index];
                cryStories.RemoveAt(index);
                GameManager.instance.SetCurrentStory(story);
                storyTriggered?.Raise();
            }
            cryTriggered?.Raise();
            StartCoroutine(Cry());
            currentCryCooldown.Set(cryCooldown.Get());
        }
    }
    IEnumerator Cry()
    {
        canMove = false;
        canAttack = false;
        float hps = cryHealth / cryDuration;
        float eps = cryEnergy / cryDuration;
        float currentDuration = 0;
        cryParticles.Play();
        cryForcefield.enabled = true;
        float initialRadius = cryForcefield.radius;
        while(currentDuration<cryDuration)
        {
            float delta = hps * Time.deltaTime;
            IncrementHealth(delta);
            float delta2 = eps * Time.deltaTime;
            IncrementEnergy(delta2);

            currentDuration += Time.deltaTime;
            cryForcefield.radius += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cryForcefield.enabled = false;
        cryForcefield.radius = initialRadius;
        cryParticles.Stop();
        canMove = true;
        canAttack = true;
    }

    private void IncrementHealth(float amount)
    {
        amount = Mathf.Clamp(amount, 0 , maxHealth.Get() - health.Get());
        health.Increment(amount);
    }

    private void IncrementEnergy(float amount)
    {
        amount = Mathf.Clamp(amount, 0, maxEnergy.Get() - energy.Get());
        energy.Increment(amount);
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

    public void StopMovement()
    {
        moveInput = new Vector2(0, 0);
        canMove = false;
        freezeMovement = true;
    }

    public void ResetMovement()
    {
        canMove = true;
        freezeMovement = false;
    }

    public void CheckHealth()
    {
        if(health.Get()<=0)
        {
           // health.Set(newLevelHealthBoost);
            triggerPortalEvent?.Raise();
        }
        
    }

    public void GoodMemory()
    {
        energy.Increment(goodMemoryEnergy);
        goodMemoryParticles.Play();
    }

    public void StartHealth()
    {
        health.Increment(newLevelHealthBoost);
        //kludge, don't have time to do it properly
        if(speed<150f)
        {
            //must be on depression level, increase speed for every visit to make it less painful
            speed += 10f * depressionVisited.Get();
        }
    }

    public void Finale()
    {
        canMove = false;
        isFloating = true;
        freezeMovement = true;
        
    }

    void OnOpenPause()
    {
        playerInput.SwitchCurrentActionMap("UI");
        Time.timeScale = 0;
        menuManager.ShowPause();
    }
    void OnClosePause()
    {
        playerInput.SwitchCurrentActionMap("Player");
        Time.timeScale = 1;
        menuManager.HidePause();
    }


}

using System;
using Combat;
using Combat.Damages;
using Combat.Heals;
using Combat.Health;
using Combat.Projectile;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Characters;
using Cinemachine;
using UnityEngine.UIElements;

public class PlayerManager : Character {
    // Cached references
    private RangeAttack rAttack;
    private MeleeAttack mAttack;

    private HealthSystem _healthSystem;
    
    // States
    private bool isMoveInputDown;
    public bool IsMoveInputDown {
        get {return this.isMoveInputDown;}
        private set {this.isMoveInputDown = value;}
    }

    private bool isLookDown;
    public bool IsLookDown {
        get {return isLookDown;}
        private set {this.isLookDown = value;}
    }

    private bool isLookUp;
    public bool IsLookUp {
        get {return isLookUp;}
        private set {this.isLookUp = value;}
    }

    private float inputX;
    [SerializeField] float speed;
    [SerializeField] private float jumpVelocity;            // applied via OnJump
    
    private const float projSpeed = 5f;


    // Paths & Names
    private const string projSpawnPositionName = "projectile_position";
    private const string projPrefabPath = "Prefabs/PlayerProjectile";
    // private const string animHorizontalSpeedName = "HorizontalSpeed";
    // private const string animVerticalVelocityName = "VerticalVelocity";
    // private const string animIsGroundedName = "IsGrounded";


    private const string UIHEALTH = "UIHealth";

    
    // dictionary -> { mode: projectile }
    // rAttack.setProjectile(proj.get(mode)); rAttack.SpawnProjectile();
    

    // Called when script instance is being loaded.
    void Awake() {   
    }
    
    private void OnEnable() {

    }
    
    // Start is called on the frame when a script is enabled
    // just before any of the Update methods are called the first time.
    // CALLED ONCE.

    protected override void Start() {
        base.Start();
        // Debug.Log("GameObject name: " + gameObject.name);
        // Debug.Log("GameObject tag: " + gameObject.tag);     // tag & layers not set
        // Debug.Log("Tag: " + this.tag);

        // rBody = gameObject.GetComponent<Rigidbody2D>();
        // boxCollider2d = gameObject.GetComponent<BoxCollider2D>();
        // animator = gameObject.GetComponent<Animator>();

	    rBody.freezeRotation = true;
  
        // Debug.Log(String.Format("Ground Layer: {0}\n", groundLayer.value));
        // 1. configure projectile

        Projectile projectile = Resources.Load<GameObject>(projPrefabPath).GetComponent<TimedProjectile>();
        projectile.InitSpeed = 5f;
        // 2. pass into range attack
        rAttack = new RangeAttack(this, 
            transform.Find(projSpawnPositionName).gameObject,
            projectile);
        
        // Health System
        _healthSystem = new HealthSystem();

        NormalHealthUI nhui = GameObject.Find(UIHEALTH).GetComponent<NormalHealthUI>();
        HealthBar nhb = new NormalHealthBar(true);
        nhb.registerUIDelegate(nhui);
        
        _healthSystem.AddHealthBar(nhb);
    }

    protected override void Update() {
        base.Update();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        
        rBody.velocity = new Vector2(inputX * speed, rBody.velocity.y);
    }

    protected override void LateUpdate() {
        base.LateUpdate();

        animator.SetBool("MoveInputDown", IsMoveInputDown);
    }

    // private void AnimParams() {
    //     animator.SetFloat(animHorizontalSpeedName, Mathf.Abs(rBody.velocity.x));
    //     animator.SetFloat(animVerticalVelocityName, rBody.velocity.y);
    //     animator.SetBool(animIsGroundedName, IsGrounded);
    // }

    // Called more frequently than Update() -> e.g. multiple times per frame.
    // All physics calculations and updates occur immediately after FixedUpdate.
    // Time.deltaTime is not required to be multiplied.

    // void OnTriggerEnter2D(Collider2D collision) {
    //     Debug.Log("EnterTriggered");
    //     if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName)) {
    //         IsGrounded = true;
    //     }
    // }

    // void OnTriggerExit2D(Collider2D collision) {
    //     Debug.Log("ExitTriggered");
    //     if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName)) {
    //         IsGrounded = false;
    //     }
    // }

    // Action: Jump
    public void OnJump(InputAction.CallbackContext ctx)
    {
        // Debug.Log("OnJump invoked.");
        // Debug.Log("Jumping " + ctx.phase + ".");

        if (ctx.canceled && rBody.velocity.y > 0) {
            rBody.velocity = Vector2.zero;
        }

        if (ctx.interaction is PressInteraction) {
            PressInteraction press = (PressInteraction) ctx.interaction;
            // Debug.Log("Press behavior: " + press.behavior);
            // Debug.Log("Press point: " + press.pressPoint);
        }
        
        if (IsGrounded) {
            // (Phase = performed only) -- applies a jumpVelocity when a jump event is detected.
            if (ctx.performed) {
                rBody.velocity = new Vector2(rBody.velocity.x, jumpVelocity);
            }
            // TODO: use duration for charged jump
            // String s = String.Format("{0}:\tstart time: {1}\ttime: {2}\telapsed: {3}\n", 
            //     ctx.phase, ctx.startTime, ctx.time,
            //     ctx.time- ctx.startTime);
            // Debug.Log(s);
            // Debug.Log("Duration: " + ctx.duration);
        }
    }

    // Action: Move horizontally
    public void OnMove(InputAction.CallbackContext ctx)
    {
        // Debug.Log("OnMove triggered.");
        
        // apply a speed to rigid body based on input.
        // inputX stays at {-1, 1} when button held -- Update() then applies the velocity to the body per frame.
        inputX = ctx.ReadValue<Vector2>().x;        
        
        // Update IsMoveInputDown
        if (ctx.performed) {
            IsMoveInputDown = true;
        } else if (ctx.canceled) {
            IsMoveInputDown = false;
        }
    }

    // Action: Do range attack
    public void OnRangeAttack(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            rAttack.SpawnProjectile();
        }
    }

    // Action: Pan camera down
    public void OnLookDown(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            IsLookDown = true;
        } else if (ctx.canceled) {
            IsLookDown = false;
        }
    }

    // Action: Pan camera up
    public void OnLookUp(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            IsLookUp = true;
        } else if (ctx.canceled) {
            IsLookUp = false;
        }
    }
    
    // UI TEST
    public void OnTakeDamage(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            _healthSystem.TakeDamage(new DiscreteDamage(1));
        }
    }

    public void OnRestoreHealth(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            _healthSystem.RestoreHealth(new DiscreteHeal(1));
        }
    }
}

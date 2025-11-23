using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{



    #region Variables
    
    

    [Header("Movement Stats")]
    public float speed = 15;
    [SerializeField, Range(0,20)]public float baseSpeed = 15;
    [SerializeField, Range(0, 20)] public float stealthSpeed = 8;
    [SerializeField, Range(0, 100)] public float maxAcceleration = 55f;
    [SerializeField, Range(0, 100)] public float maxDeceleration = 60f;
    [SerializeField, Range(0, 100)] public float maxTurnSpeed = 55f;
    [SerializeField, Range(0, 100)] public float maxAirAcceleration = 40f;
    [SerializeField, Range(0, 100)] public float maxAirDeceleration = 40f;
    [SerializeField, Range(0, 100)] public float maxAirTurnSpeed = 80f;

    [Space(20)]

    [Header("Jump Stats")]
    [SerializeField, Range(0,20)]public float jumpHeight = 5f;
    [SerializeField,Range(0.2f,1.2f)][Tooltip("Tiempo en que tarda llegar a su altura máxima")] public float timeToJumpUp = 0.4f;
    [SerializeField, Range(0,5)][Tooltip("Multiplicador de gravedad cuando salta")]public float upwardMovementMultiplier = 1f;
    [SerializeField, Range(0, 10)][Tooltip("Multiplicador de gravedad cuando cae")] public float downwardMovementMultiplier = 7f;
    [SerializeField][Tooltip("La velocidad máxima a la que el jugador cae")] public float speedLimit = 5f;
    [SerializeField][Tooltip("Cuanto dura el Coyote Time")] public float coyoteTime = 0.5f;
    [HideInInspector] public float coyoteTimeCounter;
    [HideInInspector] public bool canJumpCoyoteTime;
    [Header("WallJump")]
    [SerializeField, Range(0, 40)] public float wallJumpPowerX;
    [SerializeField, Range(0, 70)] public float wallJumpPowerY;

    [Space(20)]

    [Header("Particle")]
    [SerializeField] public GameObject jumpLandObj;
    [SerializeField] public ParticleSystem jumpLandParticles;
    [SerializeField] public GameObject walkParticles;
    [SerializeField] public ParticleSystem walkParticlesTrue;
    [SerializeField] public GameObject lavaParticlesObj; 

    [Space(20)]

    [Header("ScreenShake Achoo")]
    [SerializeField] public CinemachineVirtualCamera virtualCam;
    [HideInInspector] private CinemachineBasicMultiChannelPerlin cameraNoise;
    [SerializeField] public float amplitudeGain;
    [SerializeField] public float frequencyGain;
    [SerializeField] public float time;

    [Space(20)]

    [Header("Other Changes")]
    [SerializeField][Tooltip("El tiempo que el personaje esta obligatoriamente dentro del arbusto")] public float bushDelay = 0.5f;
    [HideInInspector] public float ladderDelay = 2f;
    [SerializeField, Range(0,20)][Tooltip("Speed to go up and down in a ladder")] public float ladderSpeed = 5f;
    [HideInInspector] public bool OnFallingPlatform;
    [SerializeField] public LayerMask FallingPlatform;

    [Space(20)]

    [Header("Components")]
    [SerializeField] public GameObject spritePlayer;
    [SerializeField] public SpriteRenderer[] totalSpritesPlayer;
    [SerializeField] public BoxCollider2D colliderPlayer;
    [SerializeField] public Rigidbody2D rbPlayer;
    [SerializeField] public GameObject playerLight;

    [Space(20)]

    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsWall;

    [Space(20)]

    [Header("Other")]

    public NarratorComment[] deathCommentsStealth;
    public NarratorComment[] deathCommentsPlatformer;
    public float jumpForce;
    public float facDir;
    public bool groundCheckDavid;
    public bool wallCheckDavid;
    public bool bushNear;
    public bool ladderNear;
    public bool grandmaNear;
    public GameObject currentLadder;
    private GameObject currentGrandma;
    public bool isAlive = true;
    public bool canMove = true;
    public bool canKillGrandma = false;
    [HideInInspector]public bool CurtainCloserUsedByTween = false;

    public bool[] grandmaObjects = new bool[2];
    public GameObject[] grandmaObjectsIcons = new GameObject[2];

    [HideInInspector] public float timeInBush;
    [HideInInspector] public float timeInLadder;
    [HideInInspector] public float wallJumpCounter;
    [HideInInspector] public float wallJumpDirection;
    [HideInInspector] public bool StealthZone;
    [HideInInspector] public bool PlattformingZone;

    [Space(20)]

    [Header("Inputs")]

    PlayerControl control;
    public Vector2 movement;
    [SerializeField] public bool pressedJump;
    [SerializeField] public bool desiredJump;
    [HideInInspector] public delegate void onPressedJump();
    [HideInInspector] public static event onPressedJump OnPressedJump;
    [HideInInspector] public bool pressedInteract;
    [HideInInspector] public delegate void onGamePaused();
    [HideInInspector] public static event onGamePaused OnGamePaused;

    [SerializeField]private LayerMask Ground;
    //Singleton
    public static Player Instance;

    //Death System
    [SerializeField]private GameObject currentCheckpoint;
    [SerializeField] private GameObject curtain;
    #endregion

    public PlayerState debugState;
    //Component References
    public Animator anim {  get; private set; }
    public Rigidbody2D rb { get; private set; }

    public AudioManager am {  get; private set; }

    //StateMachine
    public PlayerStateMachine stateMachine {  get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerBushState bushState { get; private set; }
    public PlayerLadderState ladderState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public WallJumpState wallJumpState { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    
    public void Awake()
    {
     
        //StateMachine
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        bushState = new PlayerBushState(this, stateMachine, "Bush");
        ladderState = new PlayerLadderState(this, stateMachine, "Ladder");
        airState =  new PlayerAirState (this, stateMachine, "Fall");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new WallJumpState(this, stateMachine, "WallJump");
        deathState = new PlayerDeathState(this, stateMachine, "Death");
        //
    }

    private void Start()
    {
        //References
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        am = FindObjectOfType<AudioManager>();
        cameraNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        stateMachine.Initialize(idleState);
        //

        //Input System
        control = new PlayerControl();
        control.Enable();
        StartKeyboardInputEvents();
        StartGamepadInputEvents();
        //

        //Other
        StartCoroutine(BeginState());


        

    }
    private void Update()
    {
        stateMachine.currentState.Update();
        timeInBush += Time.deltaTime;
        timeInLadder += Time.deltaTime;
        wallJumpCounter += Time.deltaTime;
        groundCheckDavid = IsGroundDetected();
        wallCheckDavid = IsWallDetected();

        

        if (StealthZone)
        {
            anim.SetFloat("Stealth", 1);
            playerLight.SetActive(true);
        }
        else
        {
            anim.SetFloat("Stealth", 0);
            playerLight.SetActive(false);
        }
        if(PlattformingZone)
        {
            lavaParticlesObj.SetActive(true);
        }
        else
        {
            lavaParticlesObj.SetActive(false);
        }
        if(!groundCheckDavid && !pressedJump)
        {
            coyoteTimeCounter += Time.deltaTime;
           
        }
        else
        {
            coyoteTimeCounter = 0;
        }
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, -transform.right, groundCheckDistance);
        if (hit)
        {
            if (hit.transform.gameObject.layer == 12)
            {
                OnFallingPlatform = true;
            }
            else
            {
                OnFallingPlatform = false;
            }
        }
        if(grandmaNear && pressedInteract && canKillGrandma)
        {
            AttackGrandma();
        }

        debugState = stateMachine.currentState;
    }
    
    private void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdate();
    }

    #region Collider Detector
    /*public bool IsWallDetected()
    {
        Vector3 startPos = transform.position + new Vector3(0.55f, -0.825f, 0);
        Vector3 endPos = transform.position + new Vector3(0.55f, 0.825f, 0);
        Debug.DrawLine(startPos, endPos, Color.green);
        RaycastHit2D hit = Physics2D.Linecast(startPos, endPos, Ground);
        if (hit.collider != null)
        {
            return wallCheckDavid = true;
        }
        else
        {
            return wallCheckDavid = false;
        }
    }*/
    
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, -transform.right * groundCheckDistance, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.down * wallCheckDistance, wallCheckDistance, whatIsWall);
    private void OnDrawGizmos()
    {
        /*Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x - groundCheckDistance, groundCheck.position.y));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x, wallCheck.position.y - wallCheckDistance));*/
        Gizmos.DrawRay(groundCheck.position, -transform.right * groundCheckDistance);
        Gizmos.DrawRay(wallCheck.position, Vector2.down * wallCheckDistance);

    }

    #endregion
    private void OnDisable()
    {
        DisableKeyboardInputEvents();
        DisableGamepadInputEvents();
    }
    #region Input Events
    #region StartAndDisableInputs
    public void StartKeyboardInputEvents()
    {
        control.Keyboard.Movement.performed += Movement;

        control.Keyboard.Movement.canceled += MovementFinished;

        control.Keyboard.Jump.performed += Jump;

        control.Keyboard.Jump.canceled += JumpFinished;

        control.Keyboard.Interact.performed += Interact;

        control.Keyboard.Interact.canceled += InteractFinished;

        control.Keyboard.Pause.performed += Pause;
    }

    public void StartGamepadInputEvents()
    {
        control.Gamepad.Movement.performed += Movement;

        control.Gamepad.Movement.canceled += MovementFinished;

        control.Gamepad.Jump.performed += Jump;

        control.Gamepad.Jump.canceled += JumpFinished;

        control.Gamepad.Interact.performed += Interact;

        control.Gamepad.Interact.canceled += InteractFinished;

        control.Gamepad.Pause.performed += Pause;
    }

    public void DisableKeyboardInputEvents()
    {
        control.Keyboard.Movement.performed -= Movement;

        control.Keyboard.Movement.canceled -= MovementFinished;

        control.Keyboard.Jump.performed -= Jump;

        control.Keyboard.Jump.canceled -= JumpFinished;

        control.Keyboard.Interact.performed -= Interact;

        control.Keyboard.Interact.canceled -= InteractFinished;

        control.Keyboard.Pause.performed -= Pause;
    }

    public void DisableGamepadInputEvents()
    {
        control.Gamepad.Movement.performed -= Movement;

        control.Gamepad.Movement.canceled -= MovementFinished;

        control.Gamepad.Jump.performed -= Jump;

        control.Gamepad.Jump.canceled -= JumpFinished;

        control.Gamepad.Interact.performed -= Interact;

        control.Gamepad.Interact.canceled -= InteractFinished;

        control.Gamepad.Pause.performed -= Pause;
    }

    #endregion

    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 inputMovement = context.ReadValue<Vector2>();
        movement = inputMovement;
    }

    public void MovementFinished(InputAction.CallbackContext context)
    {
        movement.x = 0;
        movement.y = 0;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        OnPressedJump?.Invoke();
        pressedJump = true;
        desiredJump = true;
    }

    public void JumpFinished(InputAction.CallbackContext context)
    {
        pressedJump = false;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        pressedInteract = true;
    }

    public void InteractFinished(InputAction.CallbackContext context)
    {
        pressedInteract = false;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        OnGamePaused?.Invoke();
    }
    #endregion

    #region TriggerEnter/Exit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Narrator")
        {
            other.GetComponent<NarratorTrigger>().TriggerComment();
        }
        if(other.gameObject.tag == "Sigilo")
        {
            StealthZone = true;
        }
        if (other.gameObject.tag == "Plataformas")
        {
            PlattformingZone = true;
        }
        if (other.gameObject.tag == "Arbusto")
        {
            bushNear = true;
        }
        if (other.gameObject.tag == "Escalera")
        {
            ladderNear = true;
            currentLadder = other.gameObject;
        }
        if (other.gameObject.tag == "Damage" && isAlive)
        {
            Unalive();
        }
        if(other.gameObject.tag == "ParteAbuela")
        {
            UnlockParteAbuela(other.gameObject.GetComponent<ParteAbuela>().index);
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "Checkpoint")
        {
            NewCheckpoint(other.gameObject);
        }
        if(other.gameObject.tag == "Portal")
        {
            StartCoroutine("ChangeScene", other.gameObject.GetComponent<SceneChange>());
        }
        if (other.gameObject.CompareTag("CinemachineFramer"))
        {
            FindObjectOfType<GameManager>().ToggleFramer(other.transform.parent.gameObject);
        }
        if (other.gameObject.CompareTag("AbuelaHablando"))
        {
            am.PlaySFX(3, null);
        }
        if (other.gameObject.CompareTag("Abuela"))
        {
            grandmaNear = true;
            currentGrandma = other.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Sigilo")
        {
            StealthZone = false;
        }
        if (other.gameObject.tag == "Plataformas")
        {
            PlattformingZone = false;
        }
        if (other.gameObject.tag == "Arbusto")
        {
            bushNear = false;
        }
        if (other.gameObject.tag == "Escalera")
        {
            ladderNear = false;
            currentLadder = null;
        }
        if (other.gameObject.CompareTag("CinemachineFramer"))
        {
            FindObjectOfType<GameManager>().UntoggleFramer();
        }
        if (other.gameObject.CompareTag("Abuela"))
        {
            grandmaNear = false;
        }
    }
    #endregion

    #region Checkpoints
    public void Unalive()
    {
        AudioManager.instance.PlaySFX(5, gameObject.transform);
        rb.velocity = Vector2.zero;
        rb.AddForce(-transform.right * 5f + new Vector3(0f, 15f), ForceMode2D.Impulse);
        isAlive = false;
        canMove = false;
        StartCoroutine(DamageFeedback());
        stateMachine.ChangeState(deathState);
        ManageCurtain(0, 2, Ease.OutQuart);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2.0f);
        transform.position = currentCheckpoint.transform.position;
        isAlive = true;
        canMove = true;
        stateMachine.ChangeState(idleState);
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.PlaySFX(4,null);
        int rand = Random.Range(0, 3);
        if(rand == 1)
        {
            DeathDialogueTrigger();
        }
        ManageCurtain(80, 2, Ease.InExpo);
    }

    #endregion
    void AttackGrandma()
    {
        transform.position = currentGrandma.transform.position - new Vector3(1.65f, -0.67f, 0);
        transform.eulerAngles = Vector3.zero;
        FindObjectOfType<NarratorManager>().CutComment();
        anim.Play("Backflip ataque");
    }
    IEnumerator ChangeScene(SceneChange other)
    {
        CurtainCloserUsedByTween = true;
        ManageCurtain(0, 2, Ease.OutQuart);
        spritePlayer.SetActive(false);
        canMove = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(other.sceneName);
        spritePlayer.SetActive(true);
        transform.position = other.scenePosition;
        yield return new WaitForSeconds(0.1f);
        switch (other.sceneName)
        {
            case "Lobby":
                am.PlayBGM(0);
                break;
            case "Escena Sigilo":
                am.PlayBGM(1);
                break;
            case "Escena Escalada":
                am.PlayBGM(2);
                break;
            default:
                break;

        }
        ManageCurtain(80, 2, Ease.InExpo);
        UnlockPortals();
        yield return new WaitForSeconds(1.7f);
        canMove = true;
        yield return new WaitForSeconds(0.3f);
        CurtainCloserUsedByTween = false;


    }


    void ManageCurtain(float endScale, float duration, Ease ease)
    {
        Tween curtainTween = DOTween.To(() => curtain.transform.localScale, x => curtain.transform.localScale = x, new Vector3(endScale, endScale, endScale), duration);
        curtainTween.SetEase(ease);
    }

    void UnlockParteAbuela(int index)
    {
        am.PlaySFX(6, null);

        grandmaObjects[index] = true;
        grandmaObjectsIcons[index].SetActive(true);
        Debug.Log("I unlocked object " +  index);
    }

    public void PlayJumpFallParticles(Vector2 position)
    {
        jumpLandObj.transform.position = position;
        jumpLandParticles.Play();
    }

    public IEnumerator DamageFeedback()
    {
        Color[] defaultcolor = new Color[totalSpritesPlayer.Length];
        for (int i = 0; i < totalSpritesPlayer.Length; i++)
        {
            defaultcolor[i] = totalSpritesPlayer[i].color;
            totalSpritesPlayer[i].color = Color.red;
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < totalSpritesPlayer.Length; i++)
        {
            totalSpritesPlayer[i].color = defaultcolor[i];
        }
    }

    public void Shake()
    {
        cameraNoise.m_AmplitudeGain = amplitudeGain;
        cameraNoise.m_FrequencyGain = frequencyGain;

        CancelInvoke();
        Invoke("StopShake", time);
    }

    public void StopShake()
    {
        cameraNoise.m_AmplitudeGain = 0;
        cameraNoise.m_FrequencyGain = 0;
    }

    public void NewCheckpoint(GameObject other)
    {
        if (currentCheckpoint != other.gameObject)
        {
            am.PlaySFX(0, null);
            currentCheckpoint = other.gameObject;
            other.GetComponentInChildren<ParticleSystem>().Play();
            Debug.Log("New Checkpoint Acquired");
        }
        other.GetComponentInChildren<Animator>().Play("CheckpointAdquired");

    }

    public IEnumerator BeginState()
    {
        rbPlayer.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(1f);
        rbPlayer.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void DeathDialogueTrigger()
    {
        int rand;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Escena Escalada":
                rand = Random.Range(0, deathCommentsPlatformer.Length);
                FindObjectOfType<NarratorManager>().StartComment(deathCommentsPlatformer[rand]);
                break;
            case "Escena Sigilo":
                rand = Random.Range(0, deathCommentsStealth.Length);
                FindObjectOfType<NarratorManager>().StartComment(deathCommentsStealth[rand]);
                break;
            default:
                break;
        }

    }

    #region Character Animation Juice Tweens

    private float squashStretchAmount = .17f;
    [HideInInspector]public float squashStretchTime = .5f;

    public void Squash()
    {
        Tween stretchTween = spritePlayer.transform.DOScale(new Vector3(1 + squashStretchAmount, 1 - squashStretchAmount, 1f), squashStretchTime);
        stretchTween.SetEase(Ease.OutElastic);
    }

    public void Stretch()
    {
        Tween stretchTween = spritePlayer.transform.DOScale(new Vector3(1 - squashStretchAmount, 1 + squashStretchAmount, 1f), squashStretchTime);
        stretchTween.SetEase(Ease.OutElastic);
    }

    public void ResetTween()
    {
        Tween stretchTween = spritePlayer.transform.DOScale(new Vector3(1f, 1f, 1f), squashStretchTime);
        stretchTween.SetEase(Ease.OutCubic);
    }
    #endregion
    #region NarratorEvents

    public void UnlockPortals()
    {
        GameObject platformer = GameObject.Find("PortalToPlatformer");
        GameObject stealth = GameObject.Find("PortalToStealth");
        

        Tween portalTween = DOTween.To(() => platformer.transform.localScale, x => platformer.transform.localScale = x, new Vector3(1.5f, 1.5f, 1.5f), 2);
        Tween portalTween2 = DOTween.To(() => stealth.transform.localScale, x => stealth.transform.localScale = x, new Vector3(1.5f, 1.5f, 1.5f), 2);
        
        portalTween.SetEase(Ease.OutElastic);
        portalTween2.SetEase(Ease.OutElastic);

        if(GameObject.Find("One-way Platform") != null)
        {
            GameObject platform;
            platform = GameObject.Find("One-way Platform");
            platform.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
        }
        
    }

    public void BlockPlayerMovement()
    {
        canMove = false;
    }

    public void UnblockPlayerMovement() 
    {
        canMove = true;
    }

    public void PlayerCanKillGrandma()
    {
        canKillGrandma = true;
    }
    public void SwitchGrandmaCamera()
    {
        GameObject.Find("CameraFramerGrandma").SetActive(false);
        GameObject.Find("CameraFramerPortals").transform.localScale = new Vector3(1, 1, 1);
    }
    public void AchooEvent()
    {
        Shake();
    }

    #endregion
}

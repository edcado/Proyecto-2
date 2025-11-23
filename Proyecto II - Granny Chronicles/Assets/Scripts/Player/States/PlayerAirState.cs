using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    //Estado en el aire, se entraa este estado al tener velocidad en y < 0

    public float jumpSpeed;
    private float defaultGravityScale;
    public float gravMultiplier;

    public Vector2 velocity;
    private Vector2 desiredVelocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        defaultGravityScale = 1f; //Setea la gravedad a 1
        
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravityScale;

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        velocity = player.rb.velocity; //Aplica la velocidad al rb del player

        Move();

        CalculateGravity();
        
    }

    public override void Update()
    {
        base.Update();

        SetGravity(); //Mover a FixedUpdate??

        player.desiredJump = false; //Arreglar esto

        if (player.groundCheckDavid)
        {
            Vector2 thisposition = new Vector2(player.transform.position.x, player.transform.position.y - 0.8f);
            player.PlayJumpFallParticles(thisposition); //Particulas
            player.Squash(); //Aplasta al jugador
            player.Invoke("ResetTween", player.squashStretchTime / 2); //Particulas
            stateMachine.ChangeState(player.idleState);//Cambia a Idle al tocar el suelo
        }

        if (player.wallCheckDavid && player.wallJumpCounter >= 0.2f && player.PlattformingZone)
        {
            stateMachine.ChangeState(player.wallSlideState); //Cambia a wallSlide
        }
    }

    public void SetGravity() //Añade la gravedad calculada al player
    {
        Vector2 newGravity = new Vector2(0, (-2 * player.jumpHeight) / (player.timeToJumpUp * player.timeToJumpUp));
        player.rb.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
    }

    public void CalculateGravity()
    {
        if (player.rb.velocity.y > 0.01f) //SI VELOCIDAD EN Y POSITIVA
        {
                gravMultiplier = player.upwardMovementMultiplier;
        }
        else if (player.rb.velocity.y < -0.01f) //SI VELOCIDAD NEGATIVA
        {
                gravMultiplier = player.downwardMovementMultiplier;
        }
        else //Si la velocidad es 0 gravedad es estandard
        {
            gravMultiplier = defaultGravityScale;
        }

        player.rb.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -player.speedLimit, 100)); //Calcula la velocidad del jugador y añade un limite entre -Infinito y 100

    }
    public void Move()//Mueve al player en el aire
    {
        desiredVelocity = new Vector2(player.movement.x * Mathf.Max(player.speed, 0), player.rb.velocity.y); //Mueve al personaje en el aire con la velocidad maxima en horizontal y la vertical del jugador

        if (player.StealthZone)//AÑADIR ESTO TAMBIEN AL EJE Y??
        {
            player.speed = 8;//Si esta en zona de sigilo la velocidad en el aire se ve reducida a 8

            velocity = desiredVelocity;//Setea la velocidad a la calculada arriba

            rb.velocity = velocity;//Añade la velocidad al player
        }
        else if (!player.StealthZone)
        {
            player.speed = 15;
            CalculeMaxSpeedChange();

            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        }

        rb.velocity = velocity;

        if (player.movement.x > 0)
        {
            player.facDir = 1;
            player.transform.eulerAngles = new Vector2(0, 0);
        }
        if (player.movement.x < 0)
        {
            player.facDir = -1;
            player.transform.eulerAngles = new Vector2(0, 180);
        }

    }

    public void CalculeMaxSpeedChange()//Calcula las aceleraciones de velocidad 
    {
        acceleration = player.groundCheckDavid ? player.maxAcceleration : player.maxAirAcceleration;
        deceleration = player.groundCheckDavid ? player.maxDeceleration : player.maxAirDeceleration;
        turnSpeed = player.groundCheckDavid ? player.maxTurnSpeed : player.maxAirTurnSpeed;

        if (player.movement.x != 0)
        {
            if (Mathf.Sign(player.movement.x) != Mathf.Sign(velocity.x))
            {
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
            else
            {
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else
        {
            maxSpeedChange = deceleration * Time.deltaTime;
        }
    }
}

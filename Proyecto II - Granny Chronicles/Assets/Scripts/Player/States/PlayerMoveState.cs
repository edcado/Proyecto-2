using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveState : PlayerState
{

    private ParticleSystem.EmissionModule walkParticlesEmission;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;
    
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.OnPressedJump += ChangeJumpState;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        walkParticlesEmission = player.walkParticlesTrue.emission; 
        //player.walkParticles.SetActive(true);
        walkParticlesEmission.enabled = true;

        if(player.PlattformingZone || player.StealthZone)
            player.am.PlaySFX(2, null);
        else
            player.am.PlaySFX(3, null);
    }

    public override void Exit()
    {
        base.Exit();
        Player.OnPressedJump -= ChangeJumpState;
        //player.walkParticles.SetActive(false);
        walkParticlesEmission.enabled = false;

        player.am.StopSFX(2);
        player.am.StopSFX(3);
    }
    
    public override void Update()
    {
        base.Update();
        

        /////
        if (player.bushNear && player.pressedInteract && player.timeInBush > player.bushDelay && player.StealthZone)
        {
            stateMachine.ChangeState(player.bushState);
            velocity.x = 0;
            //rb.velocity = velocity;
            player.timeInBush = 0;
        }
        else if (player.ladderNear && player.timeInLadder > player.ladderDelay)
        {
            stateMachine.ChangeState(player.ladderState);
            player.timeInLadder = 0;
        }
       /* else if (player.pressedJump && player.groundCheckDavid)                                                 //Hay dos IFs para cambiar de estado debido a que para que el sistema de particulas
        {                                                                                                       //no se active cuando se cae de una plataforma sin necesidad de saltar
            Vector2 thisposition = new Vector2(player.transform.position.x, player.transform.position.y - 0.8f);//además, espero que así se note más en código como se activa el coyote time 
            player.PlayJumpFallParticles(thisposition);
            stateMachine.ChangeState(player.jumpState);
        }*/
        else if(player.coyoteTimeCounter > 0.03f && player.coyoteTimeCounter < player.coyoteTime)
        {
            stateMachine.ChangeState(player.jumpState);
            player.canJumpCoyoteTime = true;
        }
        else if (rb.velocity.x == 0 && player.groundCheckDavid)
            stateMachine.ChangeState(player.idleState);
        else if (!player.groundCheckDavid && player.coyoteTimeCounter > player.coyoteTime)
            stateMachine.ChangeState(player.airState);
        
        
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        velocity = rb.velocity;

        Move();
    }

    public void ChangeJumpState()
    {
        if (player.groundCheckDavid)                                                 
        {                                                                                                       
            Vector2 thisposition = new Vector2(player.transform.position.x, player.transform.position.y - 0.8f);
            player.PlayJumpFallParticles(thisposition);
            stateMachine.ChangeState(player.jumpState);
        }
    }

    public void Move()
    {
        desiredVelocity = new Vector2(player.movement.x, 0) * Mathf.Max(player.speed,0);

        if (player.StealthZone)
        {
            player.speed = player.stealthSpeed;

            velocity.x = desiredVelocity.x;

            rb.velocity = velocity;
        }
        else if (!player.StealthZone)
        {
            player.speed = player.baseSpeed;
            CalculeMaxSpeedChange();

            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        }
        
        rb.velocity = velocity;
        //Debug.Log(velocity.x);

        if (player.movement.x > 0)
        {
            player.facDir = 1;
            player.transform.eulerAngles = new Vector2(0,0);
        }
        if(player.movement.x < 0)
        {
            player.facDir = -1;
            player.transform.eulerAngles = new Vector2(0, 180);
        }

    }

    /*public void MoveStealth()
    {
        desiredVelocity = new Vector2(player.movement.x, 0) * Mathf.Max(maxSpeed, 0);

        velocity.x = desiredVelocity.x;

        rb.velocity = velocity;

        if (player.movement.x > 0)
        {
            player.transform.eulerAngles = new Vector2(0, 0);
        }
        if (player.movement.x < 0)
        {
            player.transform.eulerAngles = new Vector2(0, 180);
        }
    }*/

    public void CalculeMaxSpeedChange()
    {
        acceleration = player.groundCheckDavid ? player.maxAcceleration : player.maxAirAcceleration;
        deceleration = player.groundCheckDavid ? player.maxDeceleration : player.maxAirDeceleration;
        turnSpeed = player.groundCheckDavid ? player.maxTurnSpeed : player.maxAirTurnSpeed;

        if (player.movement.x != 0)
        {
            if(Mathf.Sign(player.movement.x) != Mathf.Sign(velocity.x))
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

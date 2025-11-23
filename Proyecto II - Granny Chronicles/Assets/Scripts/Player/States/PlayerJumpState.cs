using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpState : PlayerState
{

    

    public float jumpSpeed;
    private float defaultGravityScale;
    public float gravMultiplier;

    public Vector2 velocity;
    private Vector2 desiredVelocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravityScale = 1f;
        gravMultiplier = defaultGravityScale;
        player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.Stretch();
        player.Invoke("ResetTween", player.squashStretchTime / 2);
        player.am.PlaySFX(1, null);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(player.coyoteTimeCounter > player.coyoteTime)
        {
            player.canJumpCoyoteTime = false;
        }
        if (player.rb.velocity.y < 0 && !player.canJumpCoyoteTime)
        {
            stateMachine.ChangeState(player.airState);
        }

        

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        velocity = player.rb.velocity;

        SetGravity();

        Move();

        if (player.desiredJump)
        {
            DoAJump();
            player.desiredJump = false;

            return;
        }


        CalculateGravity();
    }

    public void SetGravity()
    {
        Vector2 newGravity = new Vector2(0, (-2 * player.jumpHeight) / (player.timeToJumpUp * player.timeToJumpUp));
        player.rb.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
    }

    public void CalculateGravity()
    {
       if(player.rb.velocity.y > 0.01f)
        {
            if (player.groundCheckDavid)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                gravMultiplier = player.upwardMovementMultiplier;
            }
        }
        /*else if(player.rb.velocity.y < -0.01f)
        {
            if (groundCheck)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                gravMultiplier = downwardMovementMultiplier;
            }
        }*/
        else
        {
            gravMultiplier = defaultGravityScale;
        }

        player.rb.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -player.speedLimit , 100));

    }

    public void DoAJump()
    {
        if (player.groundCheckDavid || player.canJumpCoyoteTime)
        {
            player.coyoteTimeCounter = 0;
            player.canJumpCoyoteTime = false;
            //rb.AddForce(new Vector2(0, player.jumpForce), ForceMode2D.Impulse);

            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * player.rb.gravityScale * player.jumpHeight);  //FORMULA QUE DEFINE LA FUERZA DE SALTO
            //Debug.Log(player.rb.gravityScale);
            //Debug.Log(jumpHeight);
            //Debug.Log(Physics2D.gravity.y);
            //Debug.Log(jumpSpeed);

            /*if(velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(player.rb.velocity.y); 
            }*/                                         

            velocity.y += jumpSpeed;

            player.rb.velocity = velocity;

        }

    }

    public void Move()
    {
        desiredVelocity = new Vector2(player.movement.x, 0) * Mathf.Max(player.speed, 0);

        if (player.StealthZone)
        {
            player.speed = 8;

            velocity.x = desiredVelocity.x;

            rb.velocity = velocity;
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

    public void CalculeMaxSpeedChange()
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

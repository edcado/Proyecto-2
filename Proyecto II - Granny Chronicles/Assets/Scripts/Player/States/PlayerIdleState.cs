using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{

    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.OnPressedJump += ChangeJumpState;
    }

    public override void Exit()
    {
        base.Exit();
        Player.OnPressedJump -= ChangeJumpState;
    }

    public override void Update()
    {
        base.Update();

        if (player.movement.x == 0)
        {
            rb.constraints = /*RigidbodyConstraints2D.FreezePositionX |*/ RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        
        if (player.movement.x != 0 && player.canMove)
        {
            stateMachine.ChangeState(player.moveState);
        }
        
        /*if (player.pressedJump && player.groundCheckDavid && player.canMove)
        {
            Vector2 thisposition = new Vector2(player.transform.position.x, player.transform.position.y - 0.8f);
            player.PlayJumpFallParticles(thisposition);
            stateMachine.ChangeState(player.jumpState);
        }*/
        
        if (!player.groundCheckDavid)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (player.bushNear && player.pressedInteract && player.timeInBush > player.bushDelay && player.StealthZone)
        {
            stateMachine.ChangeState(player.bushState);
        }

    }

    public void ChangeJumpState()
    {
        if (player.groundCheckDavid && player.canMove)
        {
            Vector2 thisposition = new Vector2(player.transform.position.x, player.transform.position.y - 0.8f);
            player.PlayJumpFallParticles(thisposition);
            stateMachine.ChangeState(player.jumpState);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : PlayerState
{
    public WallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Stretch();
        player.Invoke("ResetTween", player.squashStretchTime / 2);

    }

    public override void Exit()
    {
        base.Exit();
        player.wallJumpCounter = 0;
    }

    public override void Update()
    {
        base.Update();

        SetDirection();

        if (player.desiredJump)
        {
            player.rbPlayer.velocity = new Vector2(player.wallJumpDirection * player.wallJumpPowerX, player.wallJumpPowerY);
            player.desiredJump = false;

            if (player.wallJumpDirection > 0)
            {
                player.facDir = 1;
                player.transform.eulerAngles = new Vector2(0, 0);
            }
            else
            {
                player.facDir = -1;
                player.transform.eulerAngles = new Vector2(0, 180);
            }
            stateMachine.ChangeState(player.airState);
        }
    }


    public void SetDirection()
    {
        if(player.transform.rotation.y == 0)
        {
            player.wallJumpDirection = -1;
        }
        else/*(player.transform.rotation.y == 180)*/
        {
            player.wallJumpDirection = 1;
        }
    }

    





}

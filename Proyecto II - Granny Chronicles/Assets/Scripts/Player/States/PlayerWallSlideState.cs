using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.OnPressedJump += ChangeWallJumpState;
        rb.velocity = new Vector2 (0, 0);
    }

    public override void Exit()
    {
        base.Exit();
        Player.OnPressedJump -= ChangeWallJumpState;
    }

    public override void Update()
    {
        base.Update();

        if (player.movement.y > 0)//Si mantienes la tecla de accion de vertical positiva (ej: W) la velocidad de caida se vemultiplicada por 0,5
        {
            rb.velocity = new Vector2(0, rb.velocity.y * .5f);
        }
        else if (player.movement.y < 0)//Si mantienes la tecla de accion de vertical negativa (ej: S) la velocidad es la standard
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else//Si no se presiona ninguna tecla la velocidad es reducida en un 30%
        {
            rb.velocity = new Vector2(0, rb.velocity.y * .7f);
        }




        if (player.movement.x != 0 && player.facDir != player.movement.x)//Si se presiona tecla de movimiento Y la direccion del personaje es diferente de la input
        {                                                                //cambia a Idle
            stateMachine.ChangeState(player.idleState);
        }
        if (player.groundCheckDavid)//Si toca el suelo cambia a Idle
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (!player.groundCheckDavid && !player.wallCheckDavid)//Si no detecta ni suelo ni pared cambia al estado de aire
        {
            stateMachine.ChangeState(player.airState);
        }
        /*if (player.pressedJump)//Si se presiona la tecla de accion de salto cambia al estado de walljump
        {
            player.desiredJump = true;
            stateMachine.ChangeState(player.wallJumpState);
        }*/
    }

    public void ChangeWallJumpState()
    {
        player.desiredJump = true;
        stateMachine.ChangeState(player.wallJumpState);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBushState : PlayerState
{

    

    public PlayerBushState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(6, rb.velocity.y);//Frena al personaje
        player.timeInBush = 0; //Reinicia el contador
        player.rbPlayer.constraints = RigidbodyConstraints2D.FreezeAll;//Inmoviliza el personaje
        player.spritePlayer.SetActive(false); //Esconde el sprite 
        player.colliderPlayer.enabled = false;//Desactiva el collider
        Debug.Log("I constrained player in Y");

        player.am.PlaySFX(10, null);

    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log(player.timeInBush);
        player.spritePlayer.SetActive(true);//Activa el sprite
        player.colliderPlayer.enabled = true;//Activa el collider
        player.rbPlayer.constraints = RigidbodyConstraints2D.FreezeRotation;//Libera el control al jugador
        player.timeInBush = 0;//Resetea el tiempo en arbusto

        player.am.PlaySFX(10, null);

    }

    public override void Update()
    {
        base.Update();

        player.desiredJump = false;

        if (player.pressedInteract && player.timeInBush > player.bushDelay)//Si el jugador presiona la tecla de interaccion y el tiempo transcurrido en el arbusto es mayor que el minimo
        {
            stateMachine.ChangeState(player.idleState);//Cambia a Idle
            
        }

    }
}

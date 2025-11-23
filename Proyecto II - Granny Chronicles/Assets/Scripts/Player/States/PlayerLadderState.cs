using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderState : PlayerState
{
    private Rigidbody2D rb;

    private Vector2 desiredVelocity;
    private Vector2 velocity;

    private float gravitySaved;

    public PlayerLadderState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        rb = player.GetComponent<Rigidbody2D>();
        player.timeInLadder = 0;
        player.rb.velocity = new Vector2(0, 0);
        player.rb.position = new Vector2(player.currentLadder.transform.position.x, player.rb.position.y);
        //player.rbPlayer.constraints = RigidbodyConstraints2D.FreezePositionX;
        gravitySaved = rb.gravityScale;
        //player.rbPlayer.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        velocity = rb.velocity;
        rb.gravityScale = 0;

        player.desiredJump = false;

        MoveLadder();

        ExitState();
    }

    public void ExitState()
    {
        if (player.pressedInteract && player.timeInLadder > player.ladderDelay && player.ladderNear)
        {
            stateMachine.ChangeState(player.idleState);
            //player.rbPlayer.constraints = RigidbodyConstraints2D.FreezeRotation;
            player.timeInLadder = 0;
            rb.gravityScale = gravitySaved;
        }
    }

    public void MoveLadder()
    {
        desiredVelocity = new Vector2(0, player.movement.y) * Mathf.Max(player.ladderSpeed,0);

        velocity.y = desiredVelocity.y;

        rb.velocity = velocity;
    }
}

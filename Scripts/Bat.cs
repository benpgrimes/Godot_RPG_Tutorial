using Godot;
using System;


public partial class Bat : CharacterBody2D
{
    enum ActionState
    {
        IDLE,
        WANDER,
        CHASE
    }

    [Export]
    private int MAX_SPEED = 50;
    [Export]
    private int ACCELERATION = 300;
    [Export]
    private int KNOCKBACK = 150;
    [Export]
    private int FRICTION = 200;
    [Export]
    private int PUSH_STRENGTH = 200;

    Stats stats;
    PlayerDetectionZone detectionZone;
    AnimatedSprite2D animatedSprite;
    SoftCollision softCollision;
    ActionState actionState = ActionState.IDLE;

    public override void _Ready()
    {
        stats = this.GetNode<Stats>("Stats");
        detectionZone = this.GetNode<PlayerDetectionZone>("PlayerDetectionZone");
        animatedSprite = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        softCollision = this.GetNode<SoftCollision>("SoftCollision");
    }

    public override void _PhysicsProcess(double delta)
    {
        float fdelta = (float)delta;

        this.ApplyFriction(fdelta);
        this.MoveAndSlide();

        Vector2 pushVector = softCollision.GetPushVector();
        Velocity += pushVector * fdelta * PUSH_STRENGTH;

        switch(actionState)
        {
            case ActionState.IDLE:
                this.IdleState(fdelta);
                break;
            case ActionState.WANDER:
                break;
            case ActionState.CHASE:
                this.ChaseState(fdelta);
                break;
        }
    }

    public void _OnHurtboxAreaEntered(SwordHitbox area)
    {
        int currentHealth = this.stats.getHealth();
        this.stats.setHealth(currentHealth - area.damage);

        this.Velocity = KNOCKBACK * area.knockbackVector;
    }

    public void _OnStatsNoHealth()
    {
        SharedMethods.createEffect(this, "res://Scenes/EnemyDeathEffect.tscn", offsetY: -12);
        this.QueueFree();
    }

    private void ChaseState(float delta)
    {
        if(detectionZone.CanSeePlayer())
        {
            Vector2 direction =  detectionZone.getPlayerCoordinates().Value - this.Position;
            direction = direction.Normalized();

            this.Velocity = this.Velocity.MoveToward(direction * MAX_SPEED, ACCELERATION * delta);

            animatedSprite.FlipH = this.Velocity.X < 0;

        }
        else
        {
            this.actionState = ActionState.IDLE;
        }
    }

    private void IdleState(float delta)
    {
        this.SeekPlayer();
    }

    private void WanderState()
    {

    }

    private void SeekPlayer()
    {
        if(detectionZone.CanSeePlayer())
        {
            actionState = ActionState.CHASE;
        }
    }

    private void ApplyFriction(float delta)
    {
        this.Velocity = Velocity.MoveToward(Vector2.Zero, FRICTION * delta);
    }
}

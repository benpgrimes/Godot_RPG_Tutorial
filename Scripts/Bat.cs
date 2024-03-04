using Godot;
using Godot.Collections;
using System.Runtime.CompilerServices;

public partial class Bat : CharacterBody2D
{
    enum ActionState
    {
        IDLE,
        WANDER,
        CHASE
    }

    [Export]
    private int MAX_SPEED = 150;
    [Export]
    private int ACCELERATION = 90;
    [Export]
    private int KNOCKBACK = 100;
    [Export]
    private int FRICTION = 30;
    [Export]
    private int PUSH_STRENGTH = 50;

    Stats stats;
    PlayerDetectionZone detectionZone;
    AnimatedSprite2D animatedSprite;
    SoftCollision softCollision;
    WanderController wanderController;
    AnimationPlayer animationPlayer;
    Hurtbox hurtbox;

    ActionState actionState = ActionState.IDLE;

    public override void _Ready()
    {
        stats = this.GetNode<Stats>("Stats");
        detectionZone = this.GetNode<PlayerDetectionZone>("PlayerDetectionZone");
        animatedSprite = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        softCollision = this.GetNode<SoftCollision>("SoftCollision");
        wanderController = this.GetNode<WanderController>("WanderController");
        animationPlayer = this.GetNode<AnimationPlayer>("AnimationPlayer");
        hurtbox = this.GetNode<Hurtbox>("Hurtbox");

        this.RandomizeState();
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
                this.WanderState(fdelta);
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
        this.hurtbox.startInvincibility(.3);

        this.Velocity = KNOCKBACK * area.knockbackVector;
    }

    public void _OnStatsNoHealth()
    {
        SharedMethods.createEffect(this, "res://Scenes/EnemyDeathEffect.tscn", offsetY: -12);
        this.QueueFree();
    }
    
    public void _OnInvincibilityStarted()
    {
        this.animationPlayer.Play("Start");
    }

    public void _OnInvincibilityEnded()
    {
        this.animationPlayer.Play("Stop");
    }

    private void ChaseState(float delta)
    {
        if(detectionZone.CanSeePlayer())
        {
            AccelerateToward(detectionZone.getPlayerPosition().Value, delta);
        }
        else
        {
            this.actionState = ActionState.IDLE;
        }
    }

    private void IdleState(float delta)
    {
        this.SeekPlayer();

        if(this.wanderController.GetTimeLeft() == 0)
        {
            this.RandomizeState();

            this.wanderController.SetTimer(GD.RandRange(1, 3));
        }
    }

    private void WanderState(float delta)
    {
        this.SeekPlayer();

        AccelerateToward(wanderController.GetTargetPosition(), delta);

        if (this.wanderController.GetTimeLeft() == 0 || HasReachedDestination())
        {
            this.RandomizeState();

            this.wanderController.SetTimer(GD.RandRange(1, 3));
        }
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

    private void RandomizeState()
    {
        ActionState[] states = { ActionState.IDLE, ActionState.WANDER };
        this.actionState = new Array<ActionState>(states).PickRandom();
    }

    private void AccelerateToward(Vector2 point, float delta)
    {
        Vector2 direction = this.GlobalPosition.DirectionTo(point);
        this.Velocity = this.Velocity.MoveToward(direction * MAX_SPEED, ACCELERATION * delta);

        animatedSprite.FlipH = direction.X < 0;
    }

    private bool HasReachedDestination()
    {
        return this.GlobalPosition.DistanceSquaredTo(this.wanderController.GetTargetPosition()) < 1;
    }
}

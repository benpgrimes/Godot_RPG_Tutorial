using Godot;
using System;



public partial class Player : CharacterBody2D
{    enum ActionState
    {
        MOVE,
        ROLL,
        ATTACK
    }

    [Export]
    private int FRICTION = 800;
    [Export]
    private int WALL_FRICTION = 100;
    [Export]
    private int ACCELERATION = 1000;
    [Export]
    private int MAX_SPEED = 100;
    [Export]
    private int ROLL_SPEED = 100;
    [Export]
    private int BOUNCE = 1;

    bool wasHitThisFrame;
    ActionState actionState;
    Vector2 rollVector;

    AnimationPlayer animationPlayer;
    AnimationTree animationTree;
    AnimationNodeStateMachinePlayback animationPlayback;
    SwordHitbox swordHitbox;
    Stats playerStats;
    Hurtbox hurtbox;

    public override void _Ready()
    {
        this.wasHitThisFrame = false;
        this.Velocity = Vector2.Zero;
        this.actionState = ActionState.MOVE;
        this.MotionMode = MotionModeEnum.Floating;
        this.rollVector = Vector2.Down;

        this.animationPlayer = this.GetNode<AnimationPlayer>("AnimationPlayer");
        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
        this.swordHitbox = this.GetNode<SwordHitbox>("HitboxPivot/SwordHitbox");
        this.animationPlayback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        this.hurtbox = this.GetNode<Hurtbox>("Hurtbox");
        this.playerStats = GetNode<Stats>("/root/PlayerStats");

        this.animationTree.Active = true;
        this.swordHitbox.knockbackVector = rollVector;

        this.playerStats.Connect("NoHealth", Callable.From(() => this.QueueFree()));
    }

    public override void _PhysicsProcess(double delta)
    {
        wasHitThisFrame = false;
        float fdelta = (float)delta;

        switch(this.actionState)
        {
            case ActionState.MOVE:
                moveState(fdelta);
                break;

            case ActionState.ROLL:
                rollState(fdelta);
                break;

            case ActionState.ATTACK:
                attackState(fdelta);
                break;

            default:
                throw new Exception("Unrecognized action state");
        }
    }

    public void _OnHurtboxAreaEntered(Area2D area)
    {
        if(wasHitThisFrame == false)
        {
            hurtbox.startInvincibility(0.5);
            playerStats.setHealth(playerStats.getHealth() - 1);
            this.wasHitThisFrame = true;
        }
    }

    private void moveState(float delta)
    {

        Vector2 inputDirection = getInputDirection();

        this.move(inputDirection, delta);

        if (Input.IsActionJustPressed("attack"))
        {
            this.actionState = ActionState.ATTACK;
        }
        else if (Input.IsActionJustPressed("roll"))
        {
            hurtbox.startInvincibility(0.5);
            this.actionState = ActionState.ROLL;
        }

        this.SetAnimation(inputDirection);
    }

    private void attackState(float delta)
    {
        this.move(Vector2.Zero, delta);

        if (animationPlayback.GetCurrentNode() == "Idle") {
            this.actionState = ActionState.MOVE;
        }
    }

    private void rollState(float delta)
    {
        this.move(this.rollVector, delta, true);

        if (animationPlayback.GetCurrentNode() == "Idle")
        {
            this.actionState = ActionState.MOVE;
        }
    }


    private static Vector2 getInputDirection()
    {
        Vector2 direction = Vector2.Zero;

        direction.X = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        direction.Y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

        return direction.LimitLength(1);
    }

    private void SetAnimation(Vector2 inputDirection)
    {
        if(this.actionState == ActionState.ATTACK)
        {
            this.animationPlayback.Travel("Attack");
        }
        else if(this.actionState == ActionState.ROLL)
        {
            this.animationPlayback.Travel("Roll");
        }
        else if (inputDirection != Vector2.Zero)
        {
            this.animationPlayback.Travel("Run");

            rollVector = inputDirection;
            animationTree.Set("parameters/Idle/blend_position", inputDirection);
            animationTree.Set("parameters/Run/blend_position", inputDirection);
            animationTree.Set("parameters/Attack/blend_position", inputDirection);
            animationTree.Set("parameters/Roll/blend_position", inputDirection);

            this.setKnockback(inputDirection);
        }
        else
        {
            this.animationPlayback.Travel("Idle");
        }
    }

    private void move(Vector2 inputDirection, float delta, bool isRoll = false)
    {
        if(isRoll)
        {
            Velocity = inputDirection * ROLL_SPEED;
        }
        else
        {
            calculateVelocity(inputDirection, delta);
        }
        

        if (MoveAndSlide())
        {
            BounceOffWall();
            ApplyWallFriction(delta);
        }

    }

    private void calculateVelocity(Vector2 inputDirection, float delta)
    {
        this.Velocity += inputDirection * ACCELERATION * delta;
        this.Velocity = this.Velocity.LimitLength(MAX_SPEED);
        this.Velocity = this.Velocity.MoveToward(Vector2.Zero, FRICTION * delta);
    }

    private void ApplyWallFriction(float delta)
    {
        this.Velocity = this.Velocity.MoveToward(Vector2.Zero, WALL_FRICTION / 500);
    }

    private void BounceOffWall()
    {
        Vector2 realVelocity = GetRealVelocity();
        float velocityDifference = Math.Abs(Velocity.X - realVelocity.X) + Math.Abs(Velocity.Y - realVelocity.Y);
        Vector2 oldVelocity = this.Velocity;
        this.Velocity = realVelocity;

        KinematicCollision2D lastCollision = this.GetLastSlideCollision();
        float lastCollisionAngleY = lastCollision.GetAngle();

        Vector2 adjustment = Vector2.FromAngle(lastCollisionAngleY);

        this.Velocity = this.Velocity.MoveToward(Vector2.Zero, velocityDifference / 4);
        this.Velocity = this.Velocity.MoveToward(oldVelocity.Reflect(adjustment.Normalized()), BOUNCE);
    }

    private void setKnockback(Vector2 inputDirection)
    {
        float distanceFromRight = inputDirection.DistanceTo(Vector2.Right);
        float distanceFromLeft = inputDirection.DistanceTo(Vector2.Left);
        float distanceFromUp = inputDirection.DistanceTo(Vector2.Up * 1.1f);
        float distanceFromDown = inputDirection.DistanceTo(Vector2.Down * 1.1f);

        if (distanceFromRight < distanceFromLeft
            && distanceFromRight < distanceFromUp
            && distanceFromRight < distanceFromDown)
        {
            this.swordHitbox.knockbackVector = Vector2.Right;
        }
        else if (distanceFromLeft < distanceFromUp
            && distanceFromLeft < distanceFromDown)
        {
            this.swordHitbox.knockbackVector = Vector2.Left;
        }
        else if (distanceFromUp < distanceFromDown)
        {
            this.swordHitbox.knockbackVector = Vector2.Up;
        }
        else
        {
            this.swordHitbox.knockbackVector = Vector2.Down;
        }
    }
}

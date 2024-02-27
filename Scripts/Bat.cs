using Godot;
using System;

public partial class Bat : CharacterBody2D
{
    [Export]
    private int KNOCKBACK = 150;
    [Export]
    const int FRICTION = 200;

    Stats stats;

    public override void _Ready()
    {
        stats = this.GetNode<Stats>("Stats");
    }

    public override void _PhysicsProcess(double delta)
    {
        float fdelta = (float)delta;

        this.MoveAndSlide();
        this.Velocity = Velocity.MoveToward(Vector2.Zero, FRICTION * fdelta);
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
}

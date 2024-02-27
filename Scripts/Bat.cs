using Godot;
using System;

public partial class Bat : CharacterBody2D
{
    const int KNOCKBACK = 150;
    const int FRICTION = 200;

    public override void _PhysicsProcess(double delta)
    {
        float fdelta = (float)delta;

        this.MoveAndSlide();
        this.Velocity = Velocity.MoveToward(Vector2.Zero, FRICTION * fdelta);
    }

    public void _OnHurtboxAreaEntered(SwordHitbox area)
    {
        this.Velocity = KNOCKBACK * area.knockbackVector;
    }
}

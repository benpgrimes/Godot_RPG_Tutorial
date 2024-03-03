using Godot;
using System;

public partial class Hurtbox : Area2D
{
    bool wasHitThisFrame;
    Timer invincibiltyTimer;

    public override void _Ready()
    {
        this.wasHitThisFrame = false;
        this.invincibiltyTimer = this.GetNode<Timer>("InvincibilityTimer");
    }

    public override void _PhysicsProcess(double delta)
    {
        wasHitThisFrame = false;
    }

    public void startInvincibility(double duration)
    {
        SetDeferred("monitoring", false);
        this.invincibiltyTimer.WaitTime = duration;
        this.invincibiltyTimer.Start();
    }

    public void _OnAreaEntered(Area2D area)
    {
        if(wasHitThisFrame)
        SharedMethods.createEffect(this, "res://Scenes/HitEffect.tscn");
        wasHitThisFrame = true;
    }

    public void  _OnInvincibiltyTimeout()
    {
        SetDeferred("monitoring", true);
}
}

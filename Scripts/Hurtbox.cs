using Godot;
using System;

public partial class Hurtbox : Area2D
{
    [Signal]
    public delegate void InvincibilityStartedEventHandler();

    [Signal]
    public delegate void InvincibilityEndedEventHandler();

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
        EmitSignal("InvincibilityStarted");
        this.SetDeferred("monitoring", false);
        this.invincibiltyTimer.WaitTime = duration;
        this.invincibiltyTimer.Start();
    }

    public void _OnAreaEntered(Area2D area)
    {
        if(wasHitThisFrame == false)
        SharedMethods.createEffect(this, "res://Scenes/HitEffect.tscn");
        wasHitThisFrame = true;
    }

    public void  _OnInvincibiltyTimeout()
    {
        EmitSignal("InvincibilityEnded");
        this.SetDeferred("monitoring", true);

}
}

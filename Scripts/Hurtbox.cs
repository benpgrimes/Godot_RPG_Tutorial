using Godot;
using System;

public partial class Hurtbox : Area2D
{

    Timer invincibiltyTimer;

    public override void _Ready()
    {
        this.invincibiltyTimer = this.GetNode<Timer>("InvincibilityTimer");
    }

    public void startInvincibility(double duration)
    {
        SetDeferred("monitoring", false);
        //EmitSignal("InvincibilityStarted");
        this.invincibiltyTimer.WaitTime = duration;
        this.invincibiltyTimer.Start();
    }

    public void _OnAreaEntered(Area2D area)
    {
        SharedMethods.createEffect(this, "res://Scenes/HitEffect.tscn");
    }

    public void  _OnInvincibiltyTimeout()
    {
        //EmitSignal("InvincibiltyEnded");
        SetDeferred("monitoring", true);
}
}

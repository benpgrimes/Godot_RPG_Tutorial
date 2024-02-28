using Godot;
using System;

public partial class Hurtbox : Area2D
{

    public void _OnAreaEntered(Area2D area)
    {
        SharedMethods.createEffect(this, "res://Scenes/HitEffect.tscn");
    }
}

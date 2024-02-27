using Godot;
using System;
using static Godot.HttpClient;

public partial class Grass : Node2D
{
	public void _OnHurtboxAreaEntered(Area2D area)
	{
        SharedMethods.createEffect(this, "res://Scenes/GrassEffect.tscn");
		this.QueueFree();
	}
}

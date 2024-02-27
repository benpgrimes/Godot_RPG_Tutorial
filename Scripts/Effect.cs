using Godot;
using System;

public partial class Effect : AnimatedSprite2D
{

	public override void _Ready()
	{
		this.Connect("animation_finished", new Callable(this, "_OnAnimationFinished"));
		this.Play("Animate");
	}

	public void _OnAnimationFinished()
	{
		this.QueueFree();
	}
}

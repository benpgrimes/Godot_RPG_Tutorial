using Godot;
using System;

public partial class GrassEffect : Node2D
{
	AnimatedSprite2D animatedSprite;

	public override void _Ready()
	{
		this.animatedSprite = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Play("Animate");
	}

	public void _OnAnimationFinished()
	{
		this.QueueFree();
	}
}

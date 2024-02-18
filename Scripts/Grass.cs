using Godot;
using System;
using static Godot.HttpClient;

public partial class Grass : Node2D
{
	GrassEffect grassEffect;

	public void _OnHurtboxAreaEntered(Area2D area)
	{
        this.createGrassEffect();
		this.QueueFree();
	}

	private void createGrassEffect()
	{
        PackedScene grassEffectScene = GD.Load<PackedScene>("res://Scenes/GrassEffect.tscn");
        GrassEffect grassEffect = grassEffectScene.Instantiate<GrassEffect>();

        this.GetParent().AddChild(grassEffect);
        grassEffect.Position = this.Position;
    }
}

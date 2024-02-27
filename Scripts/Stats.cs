using Godot;
using System;

public partial class Stats : Node2D
{
	[Export]
	private int MAX_HEALTH = 1;

	[Signal]
	public delegate void NoHealthEventHandler();

	private int health;

	public override void _Ready()
	{
		this.health = MAX_HEALTH;
	}

	public int getHealth()
	{
		return this.health;
	}

	public void setHealth(int value)
	{
		this.health = value;
		if(health <= 0)
		{
			EmitSignal("NoHealth");
		}
	}
}

using Godot;
using System;

public partial class Stats : Node2D
{
	[Export]
	private int maxHealth = 1;

	[Signal]
	public delegate void NoHealthEventHandler();

	[Signal]
	public delegate void HealthChangedEventHandler(int health);

    [Signal]
    public delegate void MaxHealthChangedEventHandler(int health);

    private int health;

	public override void _Ready()
	{
		this.health = maxHealth;
	}

	public int getHealth()
	{
		return this.health;
	}

	public void setHealth(int value)
	{
		this.health = Math.Min(value, maxHealth);
		EmitSignal("HealthChanged", this.health);

		if(health <= 0)
		{
			EmitSignal("NoHealth");
		}
	}

	public void setMaxHealth(int value)
	{
		if(value > 0)
		{
			this.maxHealth = value;

			if(this.health > maxHealth)
			{
				this.health = maxHealth;
                EmitSignal("HealthChanged", this.health);
            }

            EmitSignal("MaxHealthChanged", maxHealth);
        }
		
	}

	public int getMaxHealth()
	{
		return maxHealth;
	}
}

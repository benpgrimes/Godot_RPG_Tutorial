using Godot;
using System;

public partial class WanderController : Node2D
{
	[Export]
	private int WANDER_RANGE = 64;

	private Vector2 startPosition;
	private Vector2 targetPosition;

	private Timer timer;

	public override void _Ready()
	{
		startPosition = this.GlobalPosition;
		targetPosition = this.GlobalPosition;
		updateTargetPosition();

		timer = this.GetNode<Timer>("Timer");
	}

	public void _OnTimerTimeout()
	{
		updateTargetPosition();
	}

	public void updateTargetPosition()
	{
		Vector2 targetVector = new Vector2(GD.RandRange(WANDER_RANGE * -1, WANDER_RANGE), GD.RandRange(WANDER_RANGE * -1, WANDER_RANGE));
		targetPosition = startPosition + targetVector;
	}

	public double GetTimeLeft()
	{
		return this.timer.TimeLeft;
	}

	public void SetTimer(double duration)
	{
		this.timer.Start(duration);
        updateTargetPosition();
    }

	public Vector2 GetTargetPosition()
	{
		return this.targetPosition;
	}
}

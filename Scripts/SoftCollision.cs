using Godot;
using System;

public partial class SoftCollision : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Vector2 GetPushVector()
	{
        Godot.Collections.Array<Area2D> areas = this.GetOverlappingAreas();
		Vector2 pushVector = Vector2.Zero;

		if(areas.Count > 0)
		{
			Area2D area = areas[0];
			pushVector = area.GlobalPosition.DirectionTo(this.GlobalPosition);
			pushVector = pushVector.Normalized();
		}

		return pushVector;
    }
}

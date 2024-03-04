using Godot;
using System;

public partial class PlayerCam : Camera2D
{
	Marker2D topLeft;
	Marker2D bottomRight;

	public override void _Ready()
	{
		this.topLeft = this.GetNode<Marker2D>("Limits/TopLeft");
		this.bottomRight = this.GetNode<Marker2D>("Limits/BottomRight");

		this.SetLimits();
    }

	private void SetLimits()
	{
        this.LimitTop = (int)topLeft.Position.Y;
        this.LimitLeft = (int)topLeft.Position.X;
        this.LimitBottom = (int)bottomRight.Position.Y;
        this.LimitRight = (int)bottomRight.Position.X;
    }
}

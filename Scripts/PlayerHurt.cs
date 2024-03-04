using Godot;
using System;

public partial class PlayerHurt : AudioStreamPlayer
{
    public void _OnFinished()
    {
        this.QueueFree();
    }
}

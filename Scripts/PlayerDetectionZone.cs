using Godot;
using System;

public partial class PlayerDetectionZone : Area2D
{
    private Node2D player;

    public bool CanSeePlayer()
    {
        return player != null;
    }

    public Vector2? getPlayerCoordinates()
    {
        if(player != null) {
            return player.Position;
        }
        else
        {
            return null;
        }
    }

    public void _OnBodyEntered(Node2D body)
    {
        player = body;
    }

    public void _OnBodyExited(Node2D body)
    {
        player = null;
    }
}

using Godot;

class SharedMethods
{

    public static void createEffect(Node2D spawner, string path, int offsetX = 0, int offsetY = 0)
    {
        Vector2 offset = new Vector2(offsetX, offsetY);

        PackedScene effectScene = GD.Load<PackedScene>(path);
        Effect effect = effectScene.Instantiate<Effect>();

        spawner.GetParent().AddChild(effect);
        effect.Position = spawner.Position + offset;
    }
}
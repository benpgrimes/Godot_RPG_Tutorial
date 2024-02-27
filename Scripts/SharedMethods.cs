using Godot;

class SharedMethods
{

    public static void createEffect(Node2D spawner, string path, int offsetX = 0, int offsetY = 0)
    {
        Vector2 offset = new Vector2(offsetX, offsetY);

        PackedScene grassEffectScene = GD.Load<PackedScene>(path);
        Effect grassEffect = grassEffectScene.Instantiate<Effect>();

        spawner.GetParent().AddChild(grassEffect);
        grassEffect.Position = spawner.Position + offset;
    }
}
using Godot;

public partial class HealthUI : Control
{
	private const int HEART_SIZE = 15;

	private TextureRect HeartUIFull;
	private TextureRect HeartUIEmpty;

	Stats playerStats;

	public override void _Ready()
	{
        HeartUIFull = this.GetNode<TextureRect>("HealthUIFull");
		HeartUIEmpty = this.GetNode<TextureRect>("HealthUIEmpty");

        this.playerStats = GetNode<Stats>("/root/PlayerStats");
		
        this.SetHearts(playerStats.getHealth());
		this.SetMaxHearts(playerStats.getMaxHealth());

        playerStats.HealthChanged += value => SetHearts(value);
		playerStats.MaxHealthChanged += value => SetMaxHearts(value);
    }

	public void SetHearts(int value)
	{
		value = Mathf.Clamp(value, 0, playerStats.getMaxHealth());

		this.HeartUIFull.SetSize(new Vector2(value * HEART_SIZE, this.HeartUIFull.Size.Y));
	}

	public void SetMaxHearts(int value)
	{
		if(value > 0)
		{
            this.HeartUIEmpty.SetSize(new Vector2(value * HEART_SIZE, this.HeartUIEmpty.Size.Y));
        }
    }
}

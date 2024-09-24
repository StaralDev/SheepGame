using Godot;
using System;
using System.Collections;

public partial class JumpscareGui : CanvasLayer
{
	public float Alpha = 1;
	public float Speed = 1f;
	public bool FreeOnCompletion;
	public Sprite2D CenterImage;
	public int offsetlimiter = 5;
	public bool offset = true;

	public Color ColorDefault = new Color("#000000");
	public bool flashColor = true;
	private ColorRect colorRect;

	public override void _Ready()
	{
		colorRect = GetNode<ColorRect>("ColorRect");
		CenterImage = GetNode<Sprite2D>("CenterImage");
	}

	public override void _Process(double delta)
	{
		Alpha = Mathf.Clamp(Alpha - (float)delta * Speed, 0, 1);

		colorRect.Color = new Color(ColorDefault, Alpha);
		string selectColor = "White";
		
		if (flashColor && GD.Randi()%10 == 0)
		{
			selectColor = "Black";
		}
		
		CenterImage.Modulate = new Color(selectColor, Mathf.Clamp(Mathf.Pow(Alpha+0.2f, 10f), 0, 1));

		if (offset) {
			CenterImage.Offset = new Vector2(
				Mathf.Clamp(((GD.Randi()%20)-10)*Alpha, -10, 10),
				Mathf.Clamp(((GD.Randi()%20)-10)*Alpha, -10, 10)
			)/offsetlimiter;
		}

		if (Alpha <= 0 && FreeOnCompletion)
		{
			QueueFree();
		}
	}
}

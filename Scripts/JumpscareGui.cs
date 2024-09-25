using Godot;
using System;
using System.Collections;

public partial class JumpscareGui : CanvasLayer
{
	public float Alpha = 1;
	public float Speed = 1f;
	public bool FreeOnCompletion;

	public Color ColorDefault = new Color("#000000");

	private ColorRect colorRect;

	public override void _Ready()
	{
		colorRect = GetNode<ColorRect>("ColorRect");
	}

	public override void _Process(double delta)
	{
		Alpha = Mathf.Clamp(Alpha - (float)delta * Speed, 0, 1);

		colorRect.Color = new Color(ColorDefault, Alpha);

		if (Alpha <= 0 && FreeOnCompletion)
		{
			QueueFree();
		}
	}
}

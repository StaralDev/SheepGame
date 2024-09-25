using Godot;
using System;
using System.Transactions;

public partial class ScreenColor : CanvasLayer
{
	public ColorRect ScreenCover;
	public Color Modulate;
	public float Transparency;

	public void SetColor(Color set)
	{
		Modulate = set;
	}

	public void SetColor(Color col, float transparency)
	{
		Modulate = col;
		Transparency = transparency;
	}

	public void SetTransparency(float transparency)
	{
		Transparency = transparency;
	}

	public override void _Ready()
	{
		ScreenCover = GetNode<ColorRect>("ColorRect");
		ScreenCover.Color = new Color(Modulate, Transparency);
	}

	public override void _Process(double delta)
	{
		ScreenCover.Color = new Color(Modulate, Transparency);
	}
}

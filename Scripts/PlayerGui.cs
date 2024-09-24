using Godot;
using System;

public partial class PlayerGui : CanvasLayer
{
	public Sprite2D[] Hearts = new Sprite2D[3];
	public struct Icons {
		public Sprite2D Green;
		public Sprite2D Blue;
		public Sprite2D Red;
		public Sprite2D Yellow;
	}

	public Icons IconLiterals;

	public override void _Ready()
	{
		Hearts[0] = GetNode<Sprite2D>("FullHeart1");
		Hearts[1] = GetNode<Sprite2D>("FullHeart2");
		Hearts[2] = GetNode<Sprite2D>("FullHeart3");

		IconLiterals = new Icons{
			Red = GetNode<Sprite2D>("RedIcon"),
			Yellow = GetNode<Sprite2D>("YellowIcon"),
			Green = GetNode<Sprite2D>("GreenIcon"),
			Blue = GetNode<Sprite2D>("BlueIcon")
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

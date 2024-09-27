using Godot;
using SheepGame;
using System;

public partial class Bilboard : Sprite2D
{
	public override void _Ready()
	{
		Overworld.GetGlobal(GetTree()).CreateBilboard(this, 0);
	}
}

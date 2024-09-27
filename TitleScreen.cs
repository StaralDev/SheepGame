using Godot;
using SheepGame;
using System;

public partial class TitleScreen : Node2D
{
	public Sprite2D startButton;
	public Sprite2D exitButton;
	public PackedScene simultaneousScene;
	public int selected = 1;

    public override void _Ready()
    {
        var global = Overworld.GetGlobal(GetTree());
		simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/TestScene.tscn");

		startButton = GetNode<Sprite2D>("startbutton");
		exitButton = GetNode<Sprite2D>("exitbutton");
    }

    public void play()
	{
		if (selected == 1)
		{
			var global = Overworld.GetGlobal(GetTree());
			simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/TestScene.tscn");
			global.SetScene(simultaneousScene);
			return;
		}
		else if (selected == 2);
		{
			GetTree().Quit();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("sg_interact"))
		{
			play();
		}
		if (Input.IsActionJustPressed("ui_left"))
		{
			selected = 1;
			startButton.Texture = GD.Load<Texture2D>("res://ArtistDropbox/PlayButtonOutline.png");
			exitButton.Texture = GD.Load<Texture2D>("res://ArtistDropbox/QuitButton.png");
		}
		else if (Input.IsActionJustPressed("ui_right"))
		{
			selected = 2;
			startButton.Texture = GD.Load<Texture2D>("res://ArtistDropbox/PlayButton.png");
			exitButton.Texture = GD.Load<Texture2D>("res://ArtistDropbox/QuitButtonOutline.png");
		}
	}
}

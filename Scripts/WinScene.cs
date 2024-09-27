using Godot;
using SheepGame;
using System;

public partial class WinScene : Node2D
{
	Timer ChangeTextTimer;
	Label label;
	Label goback;

	string[] creditTexts;

	private int counter = 0;
	private float showbottomcounter = 0;

	private bool docount = false;

	private float InterpolateQuadraticPolynomial(float x, float zeroa, float zerob, float constMultiplier = 1)
	{
		return (x+zeroa)*(x+zerob)*constMultiplier;
	}

	public override void _Ready()
	{
		label = GetNode<Label>("Label");
		ChangeTextTimer = GetNode<Timer>("ChangeTextTimer");
		goback = GetNode<Label>("GoBackText");
	
		creditTexts = new string[] {
			"Summer Job",
			"Created by\n3to1 Games",
			"ART\nMagnus Wiese",
			"DESIGN\nMikael Walker",
			"PROGRAMMING\nDillan Gray\nDeclan Mahoney",
			"THANKS FOR PLAYING"
		};

		ChangeTextTimer.Timeout += () => {
			
			docount = true;

			if (counter >= creditTexts.Length)
			{
				ChangeTextTimer.Stop();
				label.Visible = false;
			}
			else
			{
				label.Visible = true;
				label.Text = creditTexts[counter];
			}

			counter += 1;
		};

		ChangeTextTimer.Start();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustReleased("sg_interact"))
		{
			Overworld.ChangeScene(GD.Load<PackedScene>("res://Scenes/title_screen.tscn"), GetTree());
		}

		float transparency = ((float)Time.GetTicksMsec())/(1000/0.7f)%1;
		goback.Modulate = new Color(Colors.White, -InterpolateQuadraticPolynomial(transparency, 0, -1, 4));
	}
}

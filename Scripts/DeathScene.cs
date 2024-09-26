using Godot;
using SheepGame;
using System;

public partial class DeathScene : Node2D
{
	private Sprite2D paralyzedText;
	private Sprite2D tryAgainText;
	private const float hoverSpeed = 0.3f;
	private const float hoverSize = 40;
	private const float transparencySpeed = 0.7f;
	private const float flashTime = 8;

	private const int floatSnap = 1;

	private bool interacted = false;

	private Vector2 startPos;

	private Global global;

	private float InterpolateQuadraticPolynomial(float x, float zeroa, float zerob, float constMultiplier = 1)
	{
		return (x+zeroa)*(x+zerob)*constMultiplier;
	}

	public override void _Ready()
	{
		paralyzedText = GetNode<Sprite2D>("YouveBeenParalyzed");
		tryAgainText = GetNode<Sprite2D>("PressZprompt");

		startPos = paralyzedText.Position;

		global = Overworld.GetGlobal(GetTree());
	}

	public override void _Process(double delta)
	{
		float hover = Mathf.Sin(((float)Time.GetTicksMsec())/(1000/hoverSpeed)) * hoverSize;
		paralyzedText.Position = startPos + new Vector2(0, Mathf.Round(hover/floatSnap)*floatSnap);

		if (!interacted)
		{
			float transparency = ((float)Time.GetTicksMsec())/(1000/transparencySpeed)%1;
			tryAgainText.Modulate = new Color(Colors.White, -InterpolateQuadraticPolynomial(transparency, 0, -1, 4));
		}
		else
		{
			tryAgainText.Modulate = new Color(Colors.Yellow);
			float show = Mathf.Round((float)Time.GetTicksMsec()/(1000/flashTime));
			tryAgainText.Visible = show%2 == 0;
		}

		if (!interacted && Input.IsActionJustReleased("sg_interact"))
		{
			interacted = true;

			var SceneTransition = Overworld.InstantiateScene("res://Replicatables/SceneTransition.tscn") as SceneTransition;
			SceneTransition.NewScene = "res://Scenes/MainGame/StageFright.tscn";

			global.myData.Health = 3;
			global.myData.currentBalloon = null;
			global.myData.pasifiedData.Red = false;
			global.myData.pasifiedData.Yellow = false;
			global.myData.pasifiedData.Green = false;
			global.myData.pasifiedData.Blue = false;

			global.AddChild(SceneTransition);
		}
	}
}

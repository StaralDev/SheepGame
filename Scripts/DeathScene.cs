using Godot;
using System;

public partial class DeathScene : Node2D
{
	private Sprite2D paralyzedText;
	private Sprite2D tryAgainText;
	private const float hoverSpeed = 0.3f;
	private const float hoverSize = 40;
	private const float transparencySpeed = 0.7f;

	private const int floatSnap = 1;

	private Vector2 startPos;

	private float InterpolateQuadraticPolynomial(float x, float zeroa, float zerob, float constMultiplier = 1)
	{
		return (x+zeroa)*(x+zerob)*constMultiplier;
	}

	public override void _Ready()
	{
		paralyzedText = GetNode<Sprite2D>("YouveBeenParalyzed");
		tryAgainText = GetNode<Sprite2D>("PressZprompt");

		startPos = paralyzedText.Position;
	}

	public override void _Process(double delta)
	{
		float hover = Mathf.Sin(((float)Time.GetTicksMsec())/(1000/hoverSpeed)) * hoverSize;
		paralyzedText.Position = startPos + new Vector2(0, Mathf.Round(hover/floatSnap)*floatSnap);

		float transparency = ((float)Time.GetTicksMsec())/(1000/transparencySpeed)%1;
		tryAgainText.Modulate = new Color(Colors.White, -InterpolateQuadraticPolynomial(transparency, 0, -1, 4));
	}
}

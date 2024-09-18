using Godot;
using System;

public partial class Sparky : CharacterBody2D
{
	public const float Speed = 300.0f;

	private Vector2 lastDirection;


	private Sprite2D sprite;
	private CollisionShape2D sheepCollider;
	private Area2D sheepTouchbox;
	private CollisionShape2D sheepTouchboxCollider;
	private Camera2D camera;

	public bool HasRedBalloon = false;
	public bool HasBlueBalloon = false;
	public bool HasGreenBalloon = false;
	public bool HasYellowBalloon = false;

	private Vector2 lockDirection(Vector2 dir)
	{
		if ((dir.X == 0 || dir.Y == 0) && !(dir.X == 0 && dir.Y == 0))
		{
			if (dir.X != 0)
			{
				return new Vector2(Mathf.Sign(dir.X), 0);
			}
			else
			{
				return new Vector2(0, Mathf.Sign(dir.Y));
			}
		}

		return lastDirection;
	}

	

    public override void _Ready()
    {
		sprite = GetNode<Sprite2D>("Sprite");
		sheepCollider = GetNode<CollisionShape2D>("SheepCollider");
		sheepTouchbox = GetNode<Area2D>("SheepTouchbox");
		sheepTouchboxCollider = sheepTouchbox.GetNode<CollisionShape2D>("CollisionShape2D");

        lastDirection = new Vector2(1, 0);
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		lastDirection = lockDirection(direction);
		velocity = direction * Speed;

		sheepTouchboxCollider.Position = lastDirection * 67f;
		sheepTouchboxCollider.Rotation = Mathf.DegToRad(90f - (lastDirection.Y * 90f));

		Velocity = velocity;
		//MoveAndCollide(velocity*(float)delta);
		MoveAndSlide();
	}
}

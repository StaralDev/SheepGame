using Godot;
using System;

public partial class Sparky : CharacterBody2D
{
	public const float Speed = 300f;
	public const float SpeedBoost = 45f;

	private Vector2 lastDirection;

	private AnimatedSprite2D sprite;
	private CollisionShape2D sheepCollider;
	private Area2D sheepTouchbox;
	private CollisionShape2D sheepTouchboxCollider;
	private Camera2D camera;

	private Vector2 lastPosition;

	private string animation;

	public struct Persuance<T> {
		public T Red;
		public T Yellow;
		public T Green;
		public T Blue;
	}

	public Persuance<bool> sparkyPersuedConfig = new()
    {
		Red = false,
		Yellow = false,
		Green = false,
		Blue = false,
	};

	private Persuance<int> speedModifier = new()
	{
		Red = 0,
		Yellow = 0,
		Green = 0,
		Blue = 0
	};

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

	// For dinner we're having spaghetti
	public void Persue(string name, bool enable)
	{
		if (name == "Red")
		{
			sparkyPersuedConfig.Red = enable;
			if (enable) 
			{
				speedModifier.Red = 1;
			}
			else
			{
				speedModifier.Red = 0;
			}
		}
		else if (name == "Yellow")
		{
			sparkyPersuedConfig.Yellow = enable;
			if (enable) 
			{
				speedModifier.Yellow = 1;
			}
			else
			{
				speedModifier.Yellow = 0;
			}
		}
		else if (name == "Green")
		{
			sparkyPersuedConfig.Green = enable;
			if (enable) 
			{
				speedModifier.Green = 1;
			}
			else
			{
				speedModifier.Green = 0;
			}
		}
		else if (name == "Blue")
		{
			sparkyPersuedConfig.Blue = enable;
			if (enable) 
			{
				speedModifier.Blue = 1;
			}
			else
			{
				speedModifier.Blue = 0;
			}
		}
	}

	private string GetAnimationFromDirection(Vector2 dir)
	{
		if (dir.X == 0 && dir.Y == 0) { return animation; }
		
		if (Mathf.Abs(dir.X) > Mathf.Abs(dir.Y)) {
			
			if (dir.X > 0) 
			{
				return "WalkRight";
			}
			else
			{
				return "WalkLeft";
			}

		}
		else
		{

			if (dir.Y > 0) 
			{
				return "WalkDown";
			}
			else
			{
				return "WalkUp";
			}
		}
	}

    public override void _Ready()
    {
		animation = "WalkDown";
		sprite = GetNode<AnimatedSprite2D>("Sprite");
		sheepCollider = GetNode<CollisionShape2D>("SheepCollider");
		sheepTouchbox = GetNode<Area2D>("SheepTouchbox");
		sheepTouchboxCollider = sheepTouchbox.GetNode<CollisionShape2D>("CollisionShape2D");

        lastDirection = new Vector2(1, 0);
		lastPosition = Vector2.Zero;
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		lastDirection = lockDirection(direction);
		float mod = Speed + (SpeedBoost * (speedModifier.Red + speedModifier.Yellow + speedModifier.Green + speedModifier.Blue));
		velocity = direction * mod;

		sprite.SpeedScale = mod/Speed;

		animation = GetAnimationFromDirection(lastDirection);

		Vector2 magnitude = Position - lastPosition;

		if (magnitude.X != 0 || magnitude.Y != 0)
		{
			if (!sprite.IsPlaying())
			{
				sprite.Play();
			}
		}
		else
		{
			if (sprite.IsPlaying())
			{
				sprite.Stop();
			}
		}

		lastPosition = Position;

		sprite.Animation = animation;

		sheepTouchboxCollider.Position = lastDirection * 67f;
		sheepTouchboxCollider.Rotation = Mathf.DegToRad(90f - (lastDirection.Y * 90f));

		Velocity = velocity;
		//MoveAndCollide(velocity*(float)delta);
		MoveAndSlide();
	}
}

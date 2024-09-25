using Godot;
using SheepGame;
using System;
using System.Runtime.Serialization;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public float SearchSpeed { get; set; } = 2f;
	[Export]
	public float WalkSpeed { get; set; } = 3f;
	[Export]
	public float RunSpeed { get; set; } = 4.5f;
	[Export]
	public float focusDistance { get; set; } = 1000f;

	public Vector2 MapSize;
	public Vector2 MapCenter;

	[Export]
	public string ColorName { get; set; } = "Red";

	public bool PathfindingEnable = true;

	protected Sparky sparky;
	protected bool searching = true;

	protected AnimatedSprite2D ExclamationPoint;
	protected Timer sawSparkyTimer;

	protected float transparancyAmount = 0f;
	protected int transparencyDirection = 1;

	protected AnimatedSprite2D sprite;
	protected CollisionShape2D enemyCollider;
	protected Area2D enemySightbox;
	protected CollisionShape2D enemySightboxCollider;
	protected NavigationAgent2D navigationAgent;
	protected Area2D enemyKillbox;
	protected RayCast2D enemySightline;
	protected Timer enemyLostTimer;
	protected bool lostSinceSeenLast = true;
	protected Global globalObject;
	protected Timer newPathTimer;
	protected Sprite2D lightCone;

	protected bool lost = false;

	private bool allowProcess;

	protected float currentSpeed;

    public override void _Ready()
    {
		currentSpeed = SearchSpeed;

        sprite = GetNode<AnimatedSprite2D>("Sprite");
		enemyCollider = GetNode<CollisionShape2D>("EnemyCollider");
		enemySightbox = GetNode<Area2D>("EnemySightbox");
		enemySightboxCollider = enemySightbox.GetNode<CollisionShape2D>("EnemySightboxCollider");
		navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		enemyKillbox = GetNode<Area2D>("EnemyKillbox");
		enemySightline = GetNode<RayCast2D>("EnemySightline");
		enemyLostTimer = GetNode<Timer>("EnemyLostTimer");
		newPathTimer = GetNode<Timer>("NewPathTimer");
		lightCone = enemySightboxCollider.GetNode<Sprite2D>("LightCone");
		ExclamationPoint = GetNode<AnimatedSprite2D>("ExclamationPoint");
		sawSparkyTimer = GetNode<Timer>("ExclamationPointTimer");

		globalObject = Overworld.GetGlobal(GetTree());

		CallDeferred(MethodName.Setup);	

		sparky ??= Overworld.GetSparky(GetTree());

		enemySightline.AddException(sparky);

		enemySightbox.AreaEntered += (area) => {
			if (area.GetParent() == sparky && area.Name == "SheepHitbox")
			{
				bool canSeeSparky = !enemySightline.IsColliding();

				if (canSeeSparky)
				{
					if (searching)
					{
						ExclamationPoint.Visible = true;
						ExclamationPoint.Play();
						sawSparkyTimer.Start();
					}

					searching = false;
					transparencyDirection = -1;
				}
			}
		};
	}

    public override void _PhysicsProcess(double delta)
    {
		if (!allowProcess) { return; }

		if (PathfindingEnable)
		{
			enemySightline.Position = Position;
			if (sparky != null)
			{
				enemySightline.TargetPosition = sparky.Position - Position;
			}

			if (searching) 
			{
				if (!lost)
				{
					transparencyDirection = 1;
				}

				if ((Position-navigationAgent.TargetPosition).Length() <= 10)
				{
					navigationAgent.TargetPosition = new Vector2(
						(GD.Randi()%(MapSize.X*2)) + MapCenter.X - MapSize.X,
						(GD.Randi()%(MapSize.Y*2)) + MapCenter.Y - MapSize.Y
					);
				}
			}
			else
			{
				sparky ??= Overworld.GetSparky(GetTree());
				
				if (sparky == null) { return; }

				bool canSeeSparky = !enemySightline.IsColliding();

				if (canSeeSparky && (sparky.Position - Position).Length() <= focusDistance)
				{
					lost = false;
					lostSinceSeenLast = false;
					navigationAgent.TargetPosition = sparky.Position;
					sparky.Persue(ColorName, true);
					currentSpeed = WalkSpeed;
					sprite.SpeedScale = 1;
				}
				else
				{
					currentSpeed = RunSpeed;
					sprite.SpeedScale = 1.5f;
					sparky.Persue(ColorName, false);
				}
			}

			Vector2 nextPosition = navigationAgent.GetNextPathPosition();

			if ((nextPosition-Position).Length() <= 10)
			{
				return;
			}

			if (!lost) {
				var agentVelocity = Position.DirectionTo(nextPosition) * currentSpeed;
				Velocity = agentVelocity / (float)delta;

				MoveAndSlide();
			}
		}
	}

    public override void _Process(double delta)
    {
        
    }

	protected void timeout()
	{
		if (searching && PathfindingEnable) {
			navigationAgent.TargetPosition = new Vector2(
				(GD.Randi()%(MapSize.X*2)) + MapCenter.X - MapSize.X,
				(GD.Randi()%(MapSize.Y*2)) + MapCenter.Y - MapSize.Y
			);
			newPathTimer.WaitTime = 5+(GD.Randi()%4);
		}
	}

	public void Setup()
	{
		allowProcess = true;

		newPathTimer.Timeout += () => {
			timeout();
		};

		timeout();
	}
}


//hi
using Godot;
using SheepGame;
using System;
using System.Runtime.Serialization;

public partial class Enemy : CharacterBody2D
{
	public const float SearchSpeed = 2f;
	public const float WalkSpeed = 3f;
	public const float RunSpeed = 4.5f;
	public const float focusDistance = 1000f;

	public Vector2 MapSize;
	public Vector2 MapCenter;

	public bool PathfindingEnable = true;

	protected Sparky sparky;
	protected bool searching = true;

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
		currentSpeed = WalkSpeed;

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

		globalObject = Overworld.GetGlobal(GetTree());

		CallDeferred(MethodName.Setup);	

		sparky ??= Overworld.GetSparky(GetTree());

		enemySightline.AddException(sparky);

		enemySightbox.AreaEntered += (area) => {
			if (area.GetParent() == sparky && area.Name == "SheepHitbox")
			{
				searching = false;
				//Tween lightConeTween = GetTree().CreateTween();
				//lightConeTween.TweenProperty(lightCone, "modulate", new Godot.Color(Colors.LightYellow, 0f), 0.5);
			}
		};
	}

    public override void _PhysicsProcess(double delta)
    {
		if (!allowProcess) { return; }

		if (PathfindingEnable)
		{
			if (searching) 
			{
				if ((Position-navigationAgent.TargetPosition).Length() <= 10)
				{
					navigationAgent.TargetPosition = new Vector2(
						(GD.Randi()%MapSize.X) + MapCenter.X,
						(GD.Randi()%MapSize.Y) + MapCenter.Y
					);
				}
			}
			else
			{
				sparky ??= Overworld.GetSparky(GetTree());
				
				if (sparky == null) { return; }
				
				enemySightline.Position = Position;
				enemySightline.TargetPosition = sparky.Position - Position;

				bool canSeeSparky = !enemySightline.IsColliding();

				if (canSeeSparky && (sparky.Position - Position).Length() <= focusDistance)
				{
					lost = false;
					lostSinceSeenLast = false;
					navigationAgent.TargetPosition = sparky.Position;
					currentSpeed = WalkSpeed;
					sprite.SpeedScale = 1;
				}
				else
				{
					currentSpeed = RunSpeed;
					sprite.SpeedScale = 1.5f;
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

	public void Setup()
	{
		allowProcess = true;

		newPathTimer.Timeout += () => {
			if (searching && PathfindingEnable) {
				navigationAgent.TargetPosition = new Vector2(
					(GD.Randi()%MapSize.X) + MapCenter.X,
					(GD.Randi()%MapSize.Y) + MapCenter.Y
				);
			}
		};
	}
}

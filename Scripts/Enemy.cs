using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public const float Speed = 300.0f;

	public AnimatedSprite2D sprite;
	public CollisionShape2D enemyCollider;
	public Area2D enemySightbox;
	public CollisionShape2D enemySightboxCollider;
	public NavigationAgent2D navigationAgent;

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("Sprite");
		enemyCollider = GetNode<CollisionShape2D>("EnemyCollider");
		enemySightbox = GetNode<Area2D>("EnemySightbox");
		enemySightboxCollider = enemySightbox.GetNode<CollisionShape2D>("EnemySightboxCollider");
		navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
	}

    public override void _PhysicsProcess(double delta)
    {
        navigationAgent.TargetPosition = Position;
    }

    public override void _Process(double delta)
    {
        
    }
}

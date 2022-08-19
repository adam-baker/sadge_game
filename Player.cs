using Godot;
using System;

public class Player : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Signal]
	public delegate void Hit();
	[Export]
	public int Speed = 400; // player movespeed (pixel/sec)
	public Vector2 ScreenSize; // game window size
	
	private void _on_Player_body_entered(object body)
	{
		Hide(); //player disappears on hit
		EmitSignal(nameof(Hit));
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
	}

	public void Start(Vector2 pos)
	{
		Position = pos;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
	}
	
	public override void _Process(float delta)
	{
		var velocity = Vector2.Zero; // The player's movement vector.

		if (Input.IsActionPressed("move_right"))
		{
			velocity.x += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.x -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.y -= 1;
		}

		var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite.Play();
		}
		else
		{
			animatedSprite.Stop();
		}
		
		// update player position 
		// use clamp() to keep player inside game window
		Position += velocity * delta;
		Position = new Vector2(
		x: Mathf.Clamp(Position.x, 0, ScreenSize.x),
		y: Mathf.Clamp(Position.y, 0, ScreenSize.y)
		);
		
		if (velocity.x != 0)
		{
			animatedSprite.Animation = "walk";
			animatedSprite.FlipV = false;
			if (velocity.x < 0)
			{
				animatedSprite.FlipH = true;
			}
			else
			{
				animatedSprite.FlipH = false;
			}
		}
		else if (velocity.y != 0)
		{
			animatedSprite.Animation = "up";
			animatedSprite.FlipV = velocity.y > 0;
		}
	}
}


using Godot;
using System;

public class HUD : CanvasLayer
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Signal]
	public delegate void StartGame();

	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();
		
		GetNode<Timer>("MessageTimer").Start();
	}
	
	async public void ShowGameOver()
	{
		ShowMessage("GameOver");
		
		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, "timeout");
		
		var message = GetNode<Label>("Message");
		message.Text = "Dodge the\n Creeps!";
		message.Show();
		
		await ToSignal(GetTree().CreateTimer(1), "timeout");
		GetNode<Button>("StartButton").Show();
	}
	
	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	private void _on_StartGame_pressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal("StartGame");
	}


	private void _on_MessageTimer_timeout()
	{
		GetNode<Label>("Message").Hide();
	}
}

using Godot;
using System;

public class ProgressTrigger : Area2D
{
    public void BodyEntered(PhysicsBody2D b)
    {
        GetNode<KinematicBody2D>("/root/Game/HumanPlayer").Position = new Vector2(536, 529);
        GetNode("/root/Game").CallDeferred("NextLevel");
    }
}
using Godot;
using System;

public class ProgressTrigger : Area2D
{
    public void BodyEntered(PhysicsBody2D b)
    {
        GetNode<KinematicBody2D>("/root/root/SceneSwitcher/Game/HumanPlayer").Position = new Vector2(962, 957);
        GetNode<KinematicBody2D>("/root/root/SceneSwitcher/Game/DogPlayer").Position = new Vector2(962, 957);
        GetNode("/root/root/SceneSwitcher/Game").CallDeferred("NextLevel");
    }
}
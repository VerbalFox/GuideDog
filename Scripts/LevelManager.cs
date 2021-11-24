using Godot;
using System;
using System.Collections.Generic;

public class LevelManager : Node2D
{
    [Export]
    private PackedScene lvl1;

    [Export]
    private PackedScene lvl2;

    [Export]
    private PackedScene lvl3;

    private List<PackedScene> levels = new List<PackedScene>();

    private int currentLevel;

    private float cooldown;
    public override void _Ready()
    {
        AddChild(lvl1.Instance());

        currentLevel = 0;
        cooldown = 0;

        levels.Add(lvl1);
        levels.Add(lvl2);
        levels.Add(lvl3);
    }
    public void NextLevel()
    {
        if (cooldown < 0)
        {
            currentLevel++;
            if (currentLevel < levels.Count)
            {
                GetChild<Node2D>(3).QueueFree();
                AddChild(levels[currentLevel].Instance());
            }
            cooldown = 1;
        }
    }
    public override void _Process(float dt)
    {
        cooldown -= dt;

        if (Input.IsActionJustPressed("debugSwitch"))
        {
            GetNode<HumanPlayer>("HumanPlayer").isPlayer = !GetNode<HumanPlayer>("HumanPlayer").isPlayer;
            GetNode<DogPlayer>("DogPlayer").isDog = !GetNode<DogPlayer>("DogPlayer").isDog;
        }
    }
}

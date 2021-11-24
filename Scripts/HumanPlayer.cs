using Godot;
using System;

public class HumanPlayer : KinematicBody2D
{
  // Declare member variables here. Examples:
   public bool isPlayer;
   float speed;
   Vector2 velocity;

   bool hitCrate;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        isPlayer = true;
        setSpeed(150);
        velocity = new Vector2(0, 0);
    }

    public void setPlayerBool(string msg)
    {
        if(msg == "yes")
        {
            isPlayer = true;
        }
        else if(msg == "no")
        {
            isPlayer = false;
        }
    }

    public void setSpeed(float spd)
    {
        speed = spd;
    }

    public void movePlayer()
    {
        if(Input.IsActionPressed("ui_up"))
        {
            velocity.y = -speed;
        }  
        if(Input.IsActionPressed("ui_down"))
        {
            velocity.y = speed;
        } 
      
        if (Input.IsActionPressed("ui_down") != true && Input.IsActionPressed("ui_up") != true)
        {
            velocity.y=0;
        }


        if(Input.IsActionPressed("ui_left"))
        {
            velocity.x=-speed;
        }  
      
        if(Input.IsActionPressed("ui_right"))
        {
            velocity.x = speed;
        } 

        if ( Input.IsActionPressed("ui_left") != true && Input.IsActionPressed("ui_right") != true  )
        {
            velocity.x=0;
        }

      
      // normalise
        velocity = velocity.Normalized() * speed;
        MoveAndSlide(velocity);
    }

  
    public override void _PhysicsProcess(float delta)
    {
        if (isPlayer)
        movePlayer();
    }
}

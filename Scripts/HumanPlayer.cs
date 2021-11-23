using Godot;
using System;

public class HumanPlayer : KinematicBody2D
{
  // Declare member variables here. Examples:
   private bool isPlayer;
   float speed;
   Vector2 velocity;

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
        }else if(msg == "no")
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
        if(isPlayer == true && Input.IsActionPressed("ui_up"))
      {
          
         velocity.y = -speed;
      }  
        if(isPlayer == true && Input.IsActionPressed("ui_down"))
      {
         velocity.y = speed;
      } 
      
      if (Input.IsActionPressed("ui_down") != true && Input.IsActionPressed("ui_up") != true)
      {
          //if (Input.IsActionPressed("ui_up") != true)
          //{
              velocity.y=0;
          //}
      }


       if(isPlayer == true && Input.IsActionPressed("ui_left"))
      {
          velocity.x=-speed;
      }  
      
       if(isPlayer == true && Input.IsActionPressed("ui_right"))
      {
          velocity.x = speed;
      } 

      if ( Input.IsActionPressed("ui_left") != true && Input.IsActionPressed("ui_right") != true  )
      {
         // if (Input.IsActionPressed("ui_right") != true)
         // {
              velocity.x=0;
          //}
      }

     

      
      // normalise
     velocity = velocity.Normalized() * speed;
     MoveAndSlide(velocity);
    
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
 {
     movePlayer();
    // velocity = MoveAndSlide(velocity);
 }



}

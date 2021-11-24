using Godot;
using System;

public class DogPlayer : KinematicBody2D
{
    // Declare member variables here. Examples:
   public bool isDog;
   float speed;
   Vector2 velocity;

   private AnimatedSprite animationSprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        isDog = false;
        setSpeed(270);
        animationSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        velocity = new Vector2(0, 0);
    }

    public void setDogBool(string msg)
    {
        if(msg == "yes")
        {
            isDog = true;
        }
        else if(msg == "no")
        {
            isDog = false;
        }
    }

    public void setSpeed(float spd)
    {
        speed = spd;
    }

    public void moveDog()
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
           // GD.Print("right walk");
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

   public void animationHandler()
    {
        // if two buttons are pressed at the same time decide on animation
        if ( Input.IsActionPressed("ui_up") == true && Input.IsActionPressed("ui_right") == true  )
        {
            animationSprite.Play("walkRight");
            
        }
        else if ( Input.IsActionPressed("ui_up") == true && Input.IsActionPressed("ui_left") == true  )
        {
           animationSprite.Play("walkLeft");
        }
        else if ( Input.IsActionPressed("ui_down") == true && Input.IsActionPressed("ui_right") == true  )
        {
           animationSprite.Play("walkRight");
        }
        else if ( Input.IsActionPressed("ui_down") == true && Input.IsActionPressed("ui_left") == true  )
        {
           animationSprite.Play("walkLeft");
        }
        else if(Input.IsActionPressed("ui_left"))
        {
            animationSprite.Play("walkLeft");
           
        }  
        else if(Input.IsActionPressed("ui_right"))
        {
            animationSprite.Play("walkRight");
            GD.Print("right anim");
            
        } 
         else if(Input.IsActionPressed("ui_up"))
        {
            animationSprite.Play("walkRight");
            
        } 
         else if(Input.IsActionPressed("ui_down"))
        {
            animationSprite.Play("walkLeft");
            
        } 

         if (Input.IsActionPressed("ui_left") != true && Input.IsActionPressed("ui_right") != true && Input.IsActionPressed("ui_down") != true && Input.IsActionPressed("ui_up") != true)
         // if no keys are pressed at all
         {animationSprite.Play("idle");}
           

        
    }

    public override void _PhysicsProcess(float delta)
    {
        if (isDog)
       {
           moveDog();
           animationHandler();
       }
        
        
    }
}

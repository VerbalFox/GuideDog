using Godot;
using System;

public class HumanPlayer : KinematicBody2D
{
  // Declare member variables here. Examples:
   public bool isPlayer;
   public Vector2 desiredPosition;
   float speed;
   public Vector2 velocity;

   private AnimatedSprite animationSprite;

   bool hitCrate;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        isPlayer = true;
        setSpeed(150);
        animationSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        velocity = new Vector2(0, 0);

        desiredPosition = Position;
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
        // move up and down
        if(Input.IsActionPressed("ui_up"))
        {
          //  animationHandler("up");
            velocity.y = -speed;
        }  
        else if(Input.IsActionPressed("ui_down"))
        {
          ///  animationHandler("down");
            velocity.y = speed;
        } 
       // 
        if (Input.IsActionPressed("ui_down") != true && Input.IsActionPressed("ui_up") != true)
        {
         //   animationHandler("");
            velocity.y=0;
            
        }
        //

        // left and right
        if(Input.IsActionPressed("ui_left"))
        {
           // animationHandler("left");
            velocity.x=-speed;
        }  
        else if(Input.IsActionPressed("ui_right"))
        {
          //  animationHandler("right");
            velocity.x = speed;
        } 
        //
        if ( Input.IsActionPressed("ui_left") != true && Input.IsActionPressed("ui_right") != true  )
        {
           // animationHandler("");
            velocity.x=0;
        }
        //

        
      // normalise and move
        velocity = velocity.Normalized() * speed;
        MoveAndSlide(velocity);
    }

    void animationHandler()
    {
        // if two buttons are pressed at the same time decide on animation
        if ( Input.IsActionPressed("ui_up") == true && Input.IsActionPressed("ui_right") == true  )
        {
            animationSprite.Play("walkRight");
           // GD.Print("right mfs");
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
            
        } 
         else if(Input.IsActionPressed("ui_up"))
        {
            animationSprite.Play("walkUp");
            
        } 
         else if(Input.IsActionPressed("ui_down"))
        {
            animationSprite.Play("walkDown");
            
        } 

         if (Input.IsActionPressed("ui_left") != true && Input.IsActionPressed("ui_right") != true && Input.IsActionPressed("ui_down") != true && Input.IsActionPressed("ui_up") != true)
         // if no keys are pressed at all
         {animationSprite.Stop();}
           

        
    }

    public void networkedAnimationHandler(Vector2 velocity)
    {
        // if two buttons are pressed at the same time decide on animation
        if (velocity.x > 0 && velocity.y < 0) // moving right and up
        {
            animationSprite.Play("walkRight");
        }
        else if ( velocity.x < 0 && velocity.y < 0  ) // moving left and up
        {
           animationSprite.Play("walkLeft");
        }
        else if ( velocity.x > 0 && velocity.y > 0  ) // moving down and right
        {
           animationSprite.Play("walkRight");
        }
        else if ( velocity.x < 0 && velocity.y > 0  ) // moving down and left
        {
           animationSprite.Play("walkLeft");
        }
        else if( velocity.x < 0) // moving left
        {
            animationSprite.Play("walkLeft");
           
        }  
        else if(velocity.x > 0) // moving right
        {
            animationSprite.Play("walkRight");
            
        } 
         else if(velocity.y<0) // moving up
        {
            animationSprite.Play("walkUp");
            
        } 
         else if(velocity.y>0) // moving down
        {
            animationSprite.Play("walkDown");
            
        } 

         if (velocity.x == 0 && velocity.y == 0)
         // if no keys are pressed at all
         {animationSprite.Stop();}
           

        
    }

    public void NetworkPositionLerp() {
        Position = new Vector2(Mathf.Lerp(Position.x, desiredPosition.x, 0.5f), Mathf.Lerp(Position.y, desiredPosition.y, 0.5f));
    }
  
    public override void _PhysicsProcess(float delta)
    {
        if (isPlayer) {
            movePlayer();
            animationHandler();
        } else {
            NetworkPositionLerp();
        }
    }
}

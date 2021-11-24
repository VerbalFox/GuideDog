using Godot;
using System;

public class Crate : RigidBody2D
{
    private float timer;
    private bool schmooving;
    private bool playerCollision;

    private Vector2 prevPos;
    private Vector2 newPos;

    private float moveTime;

    private bool[] cantMove = new bool[4];

    public override void _Ready()
    {
        timer = 0;
        schmooving = false;
        moveTime = 0.5f;

        for (int i = 0; i < 4; i++)
        {
            cantMove[i] = false;
        }
    }
    public override void _Process(float dt)
    {
        if (playerCollision == true && !schmooving && Input.IsActionJustPressed("push"))
        {
            if (!(cantMove[0] && newPos.y < 0) && !(cantMove[2] && newPos.y > 0) &&  !(cantMove[1] && newPos.x > 0) && !(cantMove[3] && newPos.x < 0))
            {
                newPos += Position;
                schmooving = true;
            }
        }
        else if (playerCollision == true && !schmooving && Input.IsActionJustPressed("pull"))
        {
            newPos = -newPos;
            newPos += Position;
            schmooving = true;
        }

        if (schmooving)
        {
            timer += dt;
            Position = new Vector2(Mathf.Lerp(prevPos.x, newPos.x, timer/0.5f), Mathf.Lerp(prevPos.y, newPos.y, timer/moveTime));
            if (timer > moveTime)
            {
                schmooving = false;
                timer = 0;
            }
        }
    }

    private void Collision(Vector2 _newPos, Node _collision, int _side)
    {
        if (_collision.Name == "HumanPlayer")
        {
            if (!schmooving)
            {
                playerCollision = true;
                newPos = _newPos;
                prevPos = Position;
            }
        }
        else
        {
            GD.Print("I cannae move");
            cantMove[_side] = true;
        }
    }
    public void NorthCollision(Node b)
    {
        //if (!cantMove[2])
        Collision(new Vector2(0, 96), b, 0);
        //Position = new Vector2(Position.x, Position.y + 96);
    }

    public void EastCollision(Node b)
    {
        //if (!cantMove[3])
        Collision(new Vector2(-96, 0), b, 1);
        //Position = new Vector2(Position.x - 96, Position.y);
    }

    public void SouthCollision(Node b)
    {
        //if (!cantMove[0])
        Collision(new Vector2(0, -96), b, 2);
        //Position = new Vector2(Position.x, Position.y - 96);
    }

    public void WestCollision(Node b)
    {
        //if (!cantMove[1])
        Collision(new Vector2(96, 0), b, 3);
        //Position = new Vector2(Position.x + 96, Position.y);
    }

    public void ExitNorthCollision(Node b)
    {
        playerCollision = false;
        cantMove[0] = false;
    }
    public void ExitEastCollision(Node b)
    {
        playerCollision = false;
        cantMove[1] = false;
    }
    public void ExitSouthCollision(Node b)
    {
        playerCollision = false;
        cantMove[2] = false;
    }
    public void ExitWestCollision(Node b)
    {
        playerCollision = false;
        cantMove[3] = false;
    }
}

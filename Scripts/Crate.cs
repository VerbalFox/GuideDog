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
    private bool[] cantCrush = new bool[4];

    public override void _Ready()
    {
        timer = 0;
        schmooving = false;
        moveTime = 0.5f;

        for (int i = 0; i < 4; i++)
        {
            cantMove[i] = false;
            cantCrush[i] = false;
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
            if (!(cantCrush[0] && newPos.y > 0) && !(cantCrush[2] && newPos.y < 0) &&  !(cantCrush[1] && newPos.x < 0) && !(cantCrush[3] && newPos.x > 0))
            {
                newPos = -newPos;
                newPos += Position;
                schmooving = true;
            }
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

        if (GetNode<DogPlayer>("/root/root/SceneSwitcher/Game/DogPlayer").isDog)
        {
            GetNode<Sprite>("Sprite").Visible = false;
        }
        else
        {
            GetNode<Sprite>("Sprite").Visible = true;
        }
    }

    public override void _PhysicsProcess(float dt)
    {
        CheckCollision(new Vector2(Position.x, Position.y - 192), 0);
        CheckCollision(new Vector2(Position.x + 192, Position.y), 1);
        CheckCollision(new Vector2(Position.x, Position.y + 192), 2);
        CheckCollision(new Vector2(Position.x - 192, Position.y), 3);

        GD.Print($"{cantCrush[0]}, {cantCrush[1]}, {cantCrush[2]}, {cantCrush[3]}");
    }

    private void CheckCollision(Vector2 _centre, int _side)
    {
        Physics2DDirectSpaceState spaceState = GetWorld2d().DirectSpaceState;
        var hit = spaceState.IntersectRay(new Vector2(_centre.x - 20, _centre.y), new Vector2(new Vector2(_centre.x + 20, _centre.y)));
        var hit2 = spaceState.IntersectRay(new Vector2(_centre.x, _centre.y - 20), new Vector2(new Vector2(_centre.x, _centre.y + 20)));

        if(hit.Count > 0 || hit2.Count > 0)
        {
            cantCrush[_side] = true;
        }
        else
        {
            cantCrush[_side] = false;
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
            cantMove[_side] = true;
        }
    }
    public void NorthCollision(Node b)
    {
        //if (!cantMove[2])
        Collision(new Vector2(0, 128), b, 0);
    }

    public void EastCollision(Node b)
    {
        //if (!cantMove[3])
        Collision(new Vector2(-128, 0), b, 1);
    }

    public void SouthCollision(Node b)
    {
        //if (!cantMove[0])
        Collision(new Vector2(0, -128), b, 2);
    }

    public void WestCollision(Node b)
    {
        //if (!cantMove[1])
        Collision(new Vector2(128, 0), b, 3);
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

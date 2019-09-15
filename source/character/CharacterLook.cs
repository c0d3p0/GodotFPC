using Godot;


public class CharacterLook : Node
{
    private void ApplyLook()
    {
        if(mouseMove.Length() > 0)
        {
            look = head.GetRotationDegrees();
            look.x += Mathf.Deg2Rad(-mouseMove.y) * lookSpeed;
            look.y += Mathf.Deg2Rad(-mouseMove.x) * lookSpeed;

            if(look.x > lookUpMaxAngle)
                look.x = lookUpMaxAngle;
            else if(look.x < lookDownMaxAngle)
                look.x = lookDownMaxAngle;

            head.SetRotationDegrees(look);
        }
        
        mouseMove.Set(0f, 0f);
    }

    private void Initialize()
    {
        head = GetNode<Spatial>(headNP);
        Input.SetMouseMode(Input.MouseMode.Captured);
    }

    public override void _Input(InputEvent inputEvent)
    {
        InputEventMouseMotion mouseMotion = inputEvent as InputEventMouseMotion;

        if(mouseMotion != null)
            mouseMove = mouseMotion.Relative;
    }

    public override void _PhysicsProcess(float physicsDelta)
    {
        ApplyLook();
    }

    public override void _EnterTree()
    {
        Initialize();
    }



    [Export]
    private NodePath headNP;

    [Export]
    public float lookSpeed = 4.5f;

    [Export]
    public float lookUpMaxAngle = 80f;

    [Export]
    public float lookDownMaxAngle = -80f;


    private Spatial head;
    private Vector3 look;
    private Vector2 mouseMove;
}

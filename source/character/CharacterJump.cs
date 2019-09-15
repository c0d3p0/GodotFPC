using Godot;


public class CharacterJump : Node
{
    private void ApplyJump()
    {
        if(kinematicBody.IsOnFloor() &&
                Input.IsActionJustPressed(PlayerInput.P1_JUMP))
        {
            velocity = SignalUtil.Emit<Vector3>(this,
                    SignalKey.GET_MOVE_AND_SLIDE_VELOCITY);
            velocity.y += jumpSpeed;
            EmitSignal(SignalKey.SET_MOVE_AND_SLIDE_VELOCITY, velocity);
        }
    }

    private void Initialize()
    {
        kinematicBody = GetNode<KinematicBody>(kinematicBodyNP);
        AddUserSignal(SignalKey.GET_MOVE_AND_SLIDE_VELOCITY);
        AddUserSignal(SignalKey.SET_MOVE_AND_SLIDE_VELOCITY);
    }

    public override void _PhysicsProcess(float physicsDelta)
    {
        ApplyJump();
    }

    public override void _EnterTree()
    {
        Initialize();
    }



    [Export]
    private NodePath kinematicBodyNP;

    [Export]
    private float jumpSpeed = 5f;


    private KinematicBody kinematicBody;
    private Vector3 velocity;
}

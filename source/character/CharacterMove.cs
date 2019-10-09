using Godot;


public class CharacterMove : Node
{
    private void ApplyMovement(float delta)
    {
        targetVelocity = Vector3.Zero;
        direction = SignalUtil.Emit<Vector3>(this, SignalKey.GET_DIRECTION).
                Rotated(Vector3.Up, head.Rotation.y);

        if(direction.x != 0f && direction.z != 0f)
            targetVelocity = direction * GetMoveSpeed() * diagonalMoveFactor;
        else
            targetVelocity = direction * GetMoveSpeed();

        velocity = SignalUtil.Emit<Vector3>(this,
                SignalKey.GET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);
        velocity.y = 0;
        velocity = velocity.LinearInterpolate(targetVelocity, delta * GetAcceleration());
        velocity.y = 0;
        EmitSignal(SignalKey.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY, velocity);
    }

    private float GetMoveSpeed()
    {
        if(forcedMoveSpeed > 0f)
            return forcedMoveSpeed;
        
        if(Input.IsActionPressed(PlayerInput.P1_WALK))
            return walkSpeed;

        return runSpeed;
    }

    private float GetAcceleration()
    {
        if(direction.Dot(targetVelocity) > 0)
            return acceleration;
        else
            return deacceleration;
    }

    private void SetForcedMoveSpeed(float forcedMoveSpeed)
    {
        this.forcedMoveSpeed = forcedMoveSpeed;
    }

    private void Initialize()
    {
        head = GetNode<Spatial>(headNP);
        AddUserSignal(SignalKey.GET_DIRECTION);
        AddUserSignal(SignalKey.GET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);
        AddUserSignal(SignalKey.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);
    }

    public override void _PhysicsProcess(float physicsDelta)
    {
        ApplyMovement(physicsDelta);
    }

    public override void _EnterTree()
    {
        Initialize();
    }


    [Export]
    private NodePath headNP;

    [Export]    
    public float runSpeed = 8f;

    [Export]
    public float walkSpeed = 3f;

    [Export]    
    public float acceleration = 6f;

    [Export]
    public float deacceleration = 18f;


    private Spatial head;

    private Vector3 velocity;
    private Vector3 targetVelocity;
    private Vector3 direction;

    private float diagonalMoveFactor = 0.7071f;
    private float forcedMoveSpeed;
}

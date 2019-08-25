using Godot;


public class CharacterMove : Node
{
    private void ApplyMovement(float delta)
    {
        targetVelocity.Set(0f, 0f, 0f);
        direction = SignalUtil.Emit<Vector3>(this, SignalKey.GET_DIRECTION).
                Rotated(Vector3.Up, head.GetRotation().y);

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
        if(Input.IsActionPressed(PlayerInput.P1_WALK) || forceCrouchSpeed)
            return walkSpeed;
        else
            return runSpeed;
    }

    private float GetAcceleration()
    {
        if(direction.Dot(targetVelocity) > 0)
            return acceleration;
        else
            return deacceleration;
    }

    public override void _PhysicsProcess(float physicsDelta)
    {
        ApplyMovement(physicsDelta);
    }

    public override void _EnterTree()
    {
        head = GetNode<Spatial>(headNP);
    }

    public void SetForceCrouchSpeed(bool forceCrouchSpeed)
    {
        this.forceCrouchSpeed = forceCrouchSpeed;
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
    private bool forceCrouchSpeed;
}

using Godot;


public class CharacterLadder : Node
{
    private void ApplyLadderMovement(float physicsDelta)
    {
        if(ladder != null)
		{
            targetVelocity = Vector3.Zero;
            direction = SignalUtil.Emit<Vector3>(this, SignalKey.GET_DIRECTION);
            headBasis = head.GlobalTransform.basis;
            targetVelocity += direction.x * headBasis.x;
            targetVelocity += direction.z * headBasis.z;
            targetVelocity = targetVelocity.Normalized();
            targetVelocity.y = GetFixedMovementDirection();
            targetVelocity *= ladderSpeed;

            if(!kinematicBody.IsOnFloor())
            {
                targetVelocity.x = 0f;
                EmitSignal(SignalKey.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY,
                        Vector3.Zero);
            }
            
            velocity = SignalUtil.Emit<Vector3>(this,
                    SignalKey.GET_MOVE_AND_SLIDE_VELOCITY);
            velocity = velocity.LinearInterpolate(targetVelocity,
                    physicsDelta * GetAcceleration());
            EmitSignal(SignalKey.SET_MOVE_AND_SLIDE_VELOCITY, velocity);
            EmitSignal(SignalKey.SET_GRAVITY_ENABLED, false);
		}
        else
            EmitSignal(SignalKey.SET_GRAVITY_ENABLED, true);
    }

    private float GetAcceleration()
    {
        if(direction.Dot(targetVelocity) > 0)
            return acceleration;
        else
            return deacceleration;
    }

    private float GetFixedMovementDirection()
    {
        if(targetVelocity.y < 0)
            return Mathf.Floor(targetVelocity.y);
        else
            return Mathf.Ceil(targetVelocity.y);
    }

    private void AttachToLadder(Area area)
    {
        if(area.IsInGroup(NodeGroup.LADDER))
            this.ladder = area.GetParent<Spatial>();
    }

    private void DetachFromLadder(Area area)
    {
        if(ladder != null)
        {
            Spatial checkingLadder = area.GetParentOrNull<Spatial>();
        
            if(checkingLadder.GetInstanceId() == ladder.GetInstanceId())
                ladder = null;
        }
    }

    private void Initialize()
    {
        kinematicBody = GetNode<KinematicBody>(kinematicBodyNP);
        head = GetNode<Spatial>(headNP);
        AddUserSignal(SignalKey.GET_DIRECTION);
        AddUserSignal(SignalKey.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);
        AddUserSignal(SignalKey.GET_MOVE_AND_SLIDE_VELOCITY);
        AddUserSignal(SignalKey.SET_MOVE_AND_SLIDE_VELOCITY);
        AddUserSignal(SignalKey.SET_GRAVITY_ENABLED);
    }

    public override void _PhysicsProcess(float physicsDelta)
    {
        ApplyLadderMovement(physicsDelta);
    }
    
    public void OnAreaEntered(Area area)
    {
        AttachToLadder(area);
    }

    public void OnAreaExited(Area area)
    {
        DetachFromLadder(area);
    }

    public override void _EnterTree()
    {
        Initialize();
    }



    [Export]
    private NodePath kinematicBodyNP;

    [Export]
    private NodePath headNP;

    [Export]
    public float ladderSpeed = 3.5f;

    [Export]    
    public float acceleration = 6;

    [Export]
    public float deacceleration = 18;


    private KinematicBody kinematicBody;
    private Spatial head;

    private Spatial ladder;
    private Vector3 velocity;
    private Vector3 targetVelocity;
    private Vector3 direction;
    private Basis headBasis;
}

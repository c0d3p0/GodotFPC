using Godot;


public class CharacterPhysics : Node
{
    private void ApplyGravity(float delta)
    {
        if(!kinematicBody.IsOnFloor() && gravityEnabled)
            masDesiredVelocity.y += masVelocity.y + (gravity * delta);
    }

    private void ExecuteMoveAndSlideWithSnap()
    {
        if(maswsDesiredVelocity.Length() > 0)
        {
            maswsVelocity = kinematicBody.MoveAndSlideWithSnap(maswsDesiredVelocity,
                    new Vector3(0f, -0.5f, 0f), Vector3.Up, true, 1, slopeAngle);
            maswsDesiredVelocity.Set(0f, 0f, 0f);
        }
    }

    private void ExecuteMoveAndSlide()
    {
        if(masDesiredVelocity.Length() > 0)
        {
            masVelocity = kinematicBody.MoveAndSlide(masDesiredVelocity,
                    Vector3.Up, true, 1, slopeAngle);
            masDesiredVelocity.Set(0f, 0f, 0f);
        }
    }

    private void ExecuteMoveAndCollide()
    {
        if(desiredTranslation.Length() > 0)
        {
            kinematicBody.MoveAndCollide(desiredTranslation);
            desiredTranslation.Set(0f, 0f, 0f);
        }
    }

    public void SetMoveAndSlideWithSnapVelocity(Vector3 velocity)
    {
        this.maswsDesiredVelocity = velocity;
    }

    public void SetMoveAndSlideWithSnapVelocity(float x, float y, float z)
    {
        this.maswsDesiredVelocity.Set(x, y, z);
    }

    public void SetMoveAndSlideVelocity(Vector3 desiredVelocity)
    {
        this.masDesiredVelocity = desiredVelocity;
    }

    public void SetMoveAndSlideVelocity(float x, float y, float z)
    {
        this.masDesiredVelocity.Set(x, y, z);
    }

    public void SetDesiredTranslation(Vector3 desiredTranslation)
    {
        this.desiredTranslation = desiredTranslation;
    }

    public void SetDesiredTranslation(float x, float y, float z)
    {
        this.desiredTranslation.Set(x, y, z);
    }

    public void SetGravityEnabled(bool enabled)
    {
        this.gravityEnabled = enabled;
    }

    public void GetMoveAndSlideWithSnapVelocity(Godot.Object response)
    {
        response.EmitSignal(SignalKey.SET_DATA, maswsVelocity);
    }

    public void GetMoveAndSlideVelocity(Godot.Object response)
    {
        response.EmitSignal(SignalKey.SET_DATA, masVelocity);
    }

    public override void _PhysicsProcess(float physicsDelta)
    {
        ApplyGravity(physicsDelta);
        ExecuteMoveAndSlideWithSnap();
        ExecuteMoveAndSlide();
        ExecuteMoveAndCollide();
    }

    public override void _EnterTree()
    {
        slopeAngle = Mathf.Deg2Rad(slopeAngleDegree);
        kinematicBody = GetNode<KinematicBody>(kinematicBodyNP);
    }


    [Export]
    private NodePath kinematicBodyNP;

    [Export]
    public float gravity = -9.807f;

    [Export]
    public float slopeAngleDegree = 50f;


    private KinematicBody kinematicBody;

    private Vector3 maswsDesiredVelocity;
    private Vector3 masDesiredVelocity;
    private Vector3 desiredTranslation;

    private Vector3 maswsVelocity;
    private Vector3 masVelocity;

    private float slopeAngle;
    private bool gravityEnabled;
}
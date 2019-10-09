using Godot;


public class CharacterCrouch : Node
{
    private void SetShapesHeight(float height)
    {
        physicShape.Height = height;
        Vector3 extents = interactionShape.Extents;
        extents.y = height + (physicShape.Radius * 2);
        interactionShape.Extents = extents;
    }

    private void SetTranslationY(int direction)
    {
        float translationY = direction * (crouchHeight / 2f);
        Vector3 translation = head.Translation;
        translation.y += translationY;
        head.Translation = translation;
        translation = new Vector3(0f, translationY, 0f);
        EmitSignal(SignalKey.SET_DESIRED_TRANSLATION, translation);
    }

    private void Crouch(bool active)
    {
        if(active)
        {
            SetShapesHeight(crouchHeight);
            SetTranslationY(-1);
            EmitSignal(SignalKey.SET_FORCED_MOVE_SPEED, crouchSpeed);
        }
        else
        {
            SetTranslationY(1);
            SetShapesHeight(standHeight);
            EmitSignal(SignalKey.SET_FORCED_MOVE_SPEED, 0f);
        }

        crouched = active;
    }

    private void ExecuteCrouchOrUncrouch()
    {
        if(Input.IsActionPressed(PlayerInput.P1_CROUCH) && kinematicBody.IsOnFloor())
        {
            if(!crouched)
                Crouch(true);
        }
        else if(crouched && !headRayCast.IsColliding())
            Crouch(false);
    }

    private void Initialize()
    {
        AddUserSignal(SignalKey.SET_DESIRED_TRANSLATION);
        AddUserSignal(SignalKey.SET_FORCED_MOVE_SPEED);
        kinematicBody = GetNode<KinematicBody>(kinematicBodyNP);
        head = GetNode<Spatial>(headNP);
        headRayCast = GetNode<RayCast>(headRayCastNP);
        physicShape = GetNode<CollisionShape>(physicCollisionShapeNP).
                Shape as CapsuleShape;
        interactionShape = GetNode<CollisionShape>(interactionCollisionShapeNP).
                Shape as BoxShape;
    }


    public override void _PhysicsProcess(float physicsDelta)
    {
        ExecuteCrouchOrUncrouch();
    }

    public override void _EnterTree()
    {
        Initialize();
    }



    [Export]
    public NodePath kinematicBodyNP;

    [Export]
    public NodePath headNP;

    [Export]
    public NodePath headRayCastNP;

    [Export]
    public NodePath physicCollisionShapeNP;

    [Export]
    public NodePath interactionCollisionShapeNP;


    [Export]
    public float standHeight = 1f;

    [Export]
    public float crouchHeight = 0.4f;

    [Export]
    public float crouchSpeed = 3f;


    private KinematicBody kinematicBody;
    private Spatial head;
    private RayCast headRayCast;
    private CapsuleShape physicShape;
    private BoxShape interactionShape;

    private bool crouched;
}

using Godot;


public class CharacterCrouch : Node
{
    private void SetShapesHeight(float height)
    {
        physicShape.SetHeight(height);
        Vector3 extents = interactionShape.GetExtents();
        extents.y = height + (physicShape.Radius * 2);
        interactionShape.SetExtents(extents);
    }

    private void SetTranslationY(int direction)
    {
        float translationY = direction * (crouchHeight / 2f);
        Vector3 translation = head.GetTranslation();
        translation.y += translationY;
        head.SetTranslation(translation);
        translation.Set(0f, translationY, 0f);
        EmitSignal(SignalKey.SET_DESIRED_TRANSLATION, translation);
    }

    private void SetCrouchFlags(bool active)
    {
        crouched = active;
    }

    private void ExecuteCrouchOrUncrouch()
    {
        if(Input.IsActionPressed(PlayerInput.P1_CROUCH) && kinematicBody.IsOnFloor())
        {
            if(!crouched)
            {
                SetShapesHeight(crouchHeight);
                SetTranslationY(-1);
                SetCrouchFlags(true);
            }
        }
        else if(crouched && !headRayCast.IsColliding())
        {
            SetTranslationY(1);
            SetShapesHeight(standHeight);
            SetCrouchFlags(false);
        }
    }

    private void Initialize()
    {
        AddUserSignal(SignalKey.SET_DESIRED_TRANSLATION);
        kinematicBody = GetNode<KinematicBody>(kinematicBodyNP);
        head = GetNode<Spatial>(headNP);
        headRayCast = GetNode<RayCast>(headRayCastNP);
        physicShape = GetNode<CollisionShape>(physicCollisionShapeNP).
                Shape as CapsuleShape;
        interactionShape = GetNode<CollisionShape>(interactionCollisionShapeNP).
                Shape as BoxShape;
    }

    public void IsCrouched(Godot.Object response)
    {
        response.EmitSignal(SignalKey.SET_DATA, crouched);
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


    private KinematicBody kinematicBody;
    private Spatial head;
    private RayCast headRayCast;
    private CapsuleShape physicShape;
    private BoxShape interactionShape;

    private bool crouched;
}

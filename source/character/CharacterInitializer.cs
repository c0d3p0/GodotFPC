using Godot;


public class CharacterInitializer : Node
{
    private void InitializeNodes()
    {
        head = GetNode<Spatial>(headNP);
        kinematicBody = GetNode<KinematicBody>(kinematicBodyNP);
        characterInput = GetNode<CharacterInput>(characterInputNP);
        characterCrouch = GetNode<CharacterCrouch>(characterCrouchNP);
        characterLook = GetNode<CharacterLook>(characterLookNP);
        characterMove = GetNode<CharacterMove>(characterMoveNP);
        characterLadder = GetNode<CharacterLadder>(characterLadderNP);
        characterJump = GetNode<CharacterJump>(characterJumpNP);
        characterPhysics = GetNode<CharacterPhysics>(characterPhysicsNP);
    }

    private void InitializeCharacterMove()
    {
        characterLadder.AddUserSignal(SignalKey.GET_DIRECTION);
        characterLadder.AddUserSignal(SignalKey.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);
        characterLadder.AddUserSignal(SignalKey.GET_MOVE_AND_SLIDE_VELOCITY);
        characterLadder.AddUserSignal(SignalKey.SET_MOVE_AND_SLIDE_VELOCITY);
        characterLadder.AddUserSignal(SignalKey.SET_GRAVITY_ENABLED);

        characterLadder.Connect(SignalKey.GET_DIRECTION,
                characterInput, SignalMethod.GET_DIRECTION);
        characterLadder.Connect(SignalKey.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY,
                characterPhysics, SignalMethod.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);
        characterLadder.Connect(SignalKey.GET_MOVE_AND_SLIDE_VELOCITY,
                characterPhysics, SignalMethod.GET_MOVE_AND_SLIDE_VELOCITY);
        characterLadder.Connect(SignalKey.SET_MOVE_AND_SLIDE_VELOCITY,
                characterPhysics, SignalMethod.SET_MOVE_AND_SLIDE_VELOCITY);
        characterLadder.Connect(SignalKey.SET_GRAVITY_ENABLED,
                characterPhysics, SignalMethod.SET_GRAVITY_ENABLED);
    }

    private void InitializeCharacterLadder()
    {
        characterMove.AddUserSignal(SignalKey.GET_DIRECTION);
        characterMove.AddUserSignal(SignalKey.GET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);
        characterMove.AddUserSignal(SignalKey.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);

        characterMove.Connect(SignalKey.GET_DIRECTION,
                characterInput, SignalMethod.GET_DIRECTION);
        characterMove.Connect(SignalKey.GET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY,
                characterPhysics, SignalMethod.GET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);
        characterMove.Connect(SignalKey.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY,
                characterPhysics, SignalMethod.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);
    }

    private void InitializeCharacterJump()
    {
        characterJump.AddUserSignal(SignalKey.GET_MOVE_AND_SLIDE_VELOCITY);
        characterJump.AddUserSignal(SignalKey.SET_MOVE_AND_SLIDE_VELOCITY);

        characterJump.Connect(SignalKey.GET_MOVE_AND_SLIDE_VELOCITY,
                characterPhysics, SignalMethod.GET_MOVE_AND_SLIDE_VELOCITY);
        characterJump.Connect(SignalKey.SET_MOVE_AND_SLIDE_VELOCITY,
                characterPhysics, SignalMethod.SET_MOVE_AND_SLIDE_VELOCITY);
    }

    private void InitializeCharacterCrouch()
    {
        characterCrouch.AddUserSignal(SignalKey.SET_DESIRED_TRANSLATION);
        characterCrouch.AddUserSignal(SignalKey.SET_FORCE_CROUCH_SPEED);

        characterCrouch.Connect(SignalKey.SET_DESIRED_TRANSLATION,
                characterPhysics, SignalMethod.SET_DESIRED_TRANSLATION);
        characterCrouch.Connect(SignalKey.SET_FORCE_CROUCH_SPEED,
                characterMove, SignalMethod.SET_FORCE_CROUCH_SPEED);
    }

    public override void _EnterTree()
    {
        InitializeNodes();
        InitializeCharacterMove();
        InitializeCharacterLadder();
        InitializeCharacterJump();
        InitializeCharacterCrouch();
    }


    [Export]
    public NodePath characterPhysicsNP;

    [Export]
    public NodePath characterInputNP;

    [Export]
    public NodePath characterMoveNP;

    [Export]
    public NodePath characterLookNP;

    [Export]
    public NodePath characterLadderNP;

    [Export]
    public NodePath characterJumpNP;

    [Export]
    public NodePath characterCrouchNP;

    [Export]
    public NodePath kinematicBodyNP;
    
    [Export]
    public NodePath headNP;


    private CharacterPhysics characterPhysics; 
    private CharacterMove characterMove;
    private CharacterLook characterLook;
    private CharacterLadder characterLadder;
    private CharacterJump characterJump;
    private CharacterCrouch characterCrouch;
    private CharacterInput characterInput;

    private KinematicBody kinematicBody;
    private Spatial head;
}

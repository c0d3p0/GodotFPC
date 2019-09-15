using Godot;


public class CharacterInitializer : Node
{
    private void Initialize()
    {
        characterInput = GetNode(characterInputNP);
        characterCrouch = GetNode(characterCrouchNP);
        characterLook = GetNode(characterLookNP);
        characterMove = GetNode(characterMoveNP);
        characterLadder = GetNode(characterLadderNP);
        characterJump = GetNode(characterJumpNP);
        characterPhysics = GetNode(characterPhysicsNP);
    }

    private void InitializeCharacterLadder()
    {
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

    private void InitializeCharacterMove()
    {
        characterMove.Connect(SignalKey.GET_DIRECTION,
                characterInput, SignalMethod.GET_DIRECTION);
        characterMove.Connect(SignalKey.GET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY,
                characterPhysics, SignalMethod.GET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);
        characterMove.Connect(SignalKey.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY,
                characterPhysics, SignalMethod.SET_MOVE_AND_SLIDE_WITH_SNAP_VELOCITY);
        characterMove.Connect(SignalKey.IS_CROUCHED, characterCrouch,
				SignalMethod.IS_CROUCHED);
    }

    private void InitializeCharacterJump()
    {
        characterJump.Connect(SignalKey.GET_MOVE_AND_SLIDE_VELOCITY,
                characterPhysics, SignalMethod.GET_MOVE_AND_SLIDE_VELOCITY);
        characterJump.Connect(SignalKey.SET_MOVE_AND_SLIDE_VELOCITY,
                characterPhysics, SignalMethod.SET_MOVE_AND_SLIDE_VELOCITY);
    }

    private void InitializeCharacterCrouch()
    {
        characterCrouch.Connect(SignalKey.SET_DESIRED_TRANSLATION,
                characterPhysics, SignalMethod.SET_DESIRED_TRANSLATION);
    }

    public override void _EnterTree()
    {
        Initialize();
        InitializeCharacterLadder();
        InitializeCharacterMove();
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


    private Node characterPhysics; 
    private Node characterMove;
    private Node characterLook;
    private Node characterLadder;
    private Node characterJump;
    private Node characterCrouch;
    private Node characterInput;
}

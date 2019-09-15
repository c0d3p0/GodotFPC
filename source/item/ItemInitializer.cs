using Godot;


public class ItemInitializer : Node
{
    protected virtual void Initialize()
    {
        Node globalSignal = GetNode("/root/GlobalSignal");
        Node item = GetNode(itemNP);
        rootNode = GetNode(rootNodeNP);

        AddUserSignal(SignalKey.SET_GLOBAL_SIGNAL);
        Connect(SignalKey.SET_GLOBAL_SIGNAL, item,
                SignalMethod.SET_GLOBAL_SIGNAL, null, (int) ConnectFlags.Oneshot);
        EmitSignal(SignalKey.SET_GLOBAL_SIGNAL, globalSignal);

        rootNode.AddUserSignal(SignalKey.SET_EQUIPPED);
        rootNode.AddUserSignal(SignalKey.SET_CHARACTER_BODY_AND_HEAD);
        rootNode.Connect(SignalKey.SET_EQUIPPED, item, SignalMethod.SET_EQUIPPED);
        rootNode.Connect(SignalKey.SET_CHARACTER_BODY_AND_HEAD, item,
                SignalMethod.SET_CHARACTER_BODY_AND_HEAD);
    }

    public override void _EnterTree()
    {
        Initialize();
    }


    [Export]
    public NodePath rootNodeNP;

    [Export]
    public NodePath itemNP;


    protected Node rootNode;
}

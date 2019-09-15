using Godot;


public class PlayerHUDInitializer : Node
{
    private void Initialize()
    {
        Node playerHUD = GetNode(playerHUDNP);
        Node globalSignal = GetNode("/root/GlobalSignal");
        globalSignal.AddUserSignal(SignalKey.ON_WEAPON_DATA_CHANGED);
        globalSignal.Connect(SignalKey.ON_WEAPON_DATA_CHANGED, playerHUD,
                SignalMethod.ON_WEAPON_DATA_CHANGED);
    }

    public override void _EnterTree()
    {
        Initialize();
    }
    

    [Export]
    public NodePath playerHUDNP;
}

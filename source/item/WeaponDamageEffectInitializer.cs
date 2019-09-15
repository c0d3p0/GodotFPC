using Godot;


public class WeaponDamageEffectInitializer : Node
{
    private void Initialize()
    {
        Node rootNode = GetNode(rootNodeNP);
        Node weaponDamageEffect = GetNode(weaponDamageEffectNP);
        Node globalSignal = GetNode("/root/GlobalSignal");

        AddUserSignal(SignalKey.SET_WORLD);
        Connect(SignalKey.SET_WORLD, weaponDamageEffect, SignalMethod.SET_WORLD,
                null, (int) ConnectFlags.Oneshot);
        EmitSignal(SignalKey.SET_WORLD, GetNode(rootNodeNP).GetParent<Spatial>());

        globalSignal.AddUserSignal(SignalKey.ON_WEAPON_DAMAGE);
        globalSignal.Connect(SignalKey.ON_WEAPON_DAMAGE, weaponDamageEffect,
                SignalMethod.ON_WEAPON_DAMAGE);
    }

    public override void _EnterTree()
    {
        Initialize();
    }


    [Export]
    public NodePath rootNodeNP;

    [Export]
    public NodePath weaponDamageEffectNP;
}

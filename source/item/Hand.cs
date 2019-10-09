public class Hand : Item
{
    private void EmitWeaponDataChangedSignal()
    {
        paramMap.Clear();
        paramMap.Add(ParameterKey.WEAPON_NAME, rootNode.Name);
        paramMap.Add(ParameterKey.AIM_VISIBLE, false);
        globalSignal.EmitSignal(SignalKey.ON_WEAPON_DATA_CHANGED, paramMap);
    }

    protected override void OnEquipping()
    {
        EmitWeaponDataChangedSignal();
    }
}

using System;
using System.Text;

using Godot;
using Godot.Collections;


public class PlayerHUD : Node
{
    public void OnWeaponDataChanged(Dictionary<byte, object> paramMap)
    {
        if(paramMap != null)
        {
            weaponNameLabel.Text = GetText(paramMap, ParameterKey.WEAPON_NAME);
            aimTextureRect.Visible = GetBool(paramMap, ParameterKey.AIM_VISIBLE);
            ammoLabel.Text = GetAmmoText(paramMap);
            reloadingLabel.Visible = GetBool(paramMap, ParameterKey.RELOADING);
        }
    }

    private bool GetBool(Dictionary<byte, object> paramMap, byte key)
    {
        object value;

        if(paramMap.TryGetValue(key, out value))
            return (bool) value;
        else
            return false;
    }

    private string GetText(Dictionary<byte, object> paramMap, byte key)
    {
        object value;

        if(paramMap.TryGetValue(key, out value))
            return value as String;
        else
            return "";
    }

    private string GetAmmoText(Dictionary<byte, object> paramMap)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(GetText(paramMap, ParameterKey.CLIP_AMMO_AMOUNT));

        if(sb.Length > 0)
            sb.Append(" / ");

        sb.Append(GetText(paramMap, ParameterKey.AMMO_AMOUNT));
        return sb.ToString();
    }

    private void Initialize()
    {
        aimTextureRect = GetNode<TextureRect>(aimTextureRectNP);
        weaponNameLabel = GetNode<Label>(weaponNameLabelNP);
        reloadingLabel = GetNode<Label>(reloadingLabelNP);
        ammoLabel = GetNode<Label>(ammoLabelNP);
    }
    
    public override void _EnterTree()
    {
        Initialize();
    }
    

    [Export]
    public NodePath aimTextureRectNP;

    [Export]
    public NodePath weaponNameLabelNP;

    [Export]
    public NodePath ammoLabelNP;

    [Export]
    public NodePath reloadingLabelNP;


    private TextureRect aimTextureRect;
    private Label weaponNameLabel;
    private Label ammoLabel;
    private Label reloadingLabel;
}

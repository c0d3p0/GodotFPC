using Godot;
using Godot.Collections;


public class CharacterItem : Node
{
    private void EquipItem()
    {
        if(Input.IsActionJustPressed(PlayerInput.P1_EQUIP_ITEM_1))
            SetCurrentItemEquipped(itemList[0]);
        else if(Input.IsActionJustPressed(PlayerInput.P1_EQUIP_ITEM_2))
            SetCurrentItemEquipped(itemList[1]);
        else if(Input.IsActionJustPressed(PlayerInput.P1_UNEQUIP))
            SetCurrentItemEquipped(itemList[2]);
    }

    private void SetCurrentItemEquipped(Node item)
    {
        if(currentItem != null)
            currentItem.EmitSignal(SignalKey.SET_EQUIPPED, false);

        if(item != null)
            item.EmitSignal(SignalKey.SET_EQUIPPED, true);

        currentItem = item;
    }

    private void Initialize()
    {
        kinematicBody = GetNode<KinematicBody>(kinematicBodyNP);
        head = GetNode<Spatial>(headNP);
        items = GetNode<Spatial>(itemsNP);
    }

    private void AddWeapons()
    {
        itemList = new Array<Node>();
        PackedScene[] itemScenes = {assaultRiflePrefab, pistolPrefab, handPrefab};

        for(int i = 0; i < itemScenes.Length; i++)
        {
            Spatial item = itemScenes[i].Instance() as Spatial;
            items.AddChild(item);
            itemList.Add(item);
            item.EmitSignal(SignalKey.SET_CHARACTER_BODY_AND_HEAD, kinematicBody, head);
        }

        SetCurrentItemEquipped(itemList[2]);
    }

    public override void _Process(float delta)
    {
        EquipItem();
    }

    public override void _EnterTree()
    {
        Initialize();
    }

    public override void _Ready()
    {
        AddWeapons();
    }
    

    [Export]
    public NodePath itemsNP;

    [Export]
    public NodePath kinematicBodyNP;

    [Export]
    public NodePath headNP;
    
    [Export]
    public PackedScene assaultRiflePrefab;

    [Export]
    public PackedScene pistolPrefab;

    [Export]
    public PackedScene handPrefab;


    private KinematicBody kinematicBody;
    private Spatial head;
    private Spatial items;
    
    private Node currentItem;
    private Array<Node> itemList;
}

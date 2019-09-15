using Godot;
using Godot.Collections;


public abstract class Item : Node
{
    protected virtual void Initialize()
    {
        paramMap = new Dictionary<byte, object>();
        rootNode = GetNode<Spatial>(rootNodeNP);
    }

    protected virtual void SetEquipped(bool equipping)
    {
        if(equipping && !this.equipped)
            OnEquipping();
        
        this.equipped = equipping;
    }

    public void SetGlobalSignal(Node globalSignal)
    {
        this.globalSignal = globalSignal;
    }

    public void SetCharacterBodyAndHead(KinematicBody kinematicBody, Spatial head)
    {
        this.characterBody = kinematicBody;
        this.characterHead = head;
    }

    public override void _EnterTree()
    {
        Initialize();
    }
    
    protected abstract void OnEquipping();


    [Export]
    public NodePath rootNodeNP;


    protected Node globalSignal;
    protected Spatial rootNode;
    protected KinematicBody characterBody;
    protected Spatial characterHead;

    protected bool equipped;
    protected Dictionary<byte, object> paramMap;
}

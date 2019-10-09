using Godot;
using Godot.Collections;


public class WeaponDamageEffect : Node
{
    public void CleanOverLimitEffect()
    {
        if(damageEffectList.Count > effectPollSize)
        {
            Node bh = damageEffectList[0];
            bh.CallDeferred(GDMethod.QUEUE_FREE);
            damageEffectList.RemoveAt(0);
        }
    }

    public void OnWeaponDamage(Dictionary<byte, object> paramMap)
    {
        object spatialObj;

        if(paramMap.TryGetValue(ParameterKey.DAMAGED_NODE, out spatialObj))
        {
            Spatial damaged = spatialObj as Spatial;

            if(damaged.IsInGroup(NodeGroup.STATIC_OBJECT))
            {
                Spatial effect = weaponDamageEffectPrefab.Instance() as Spatial;
                effect.Name = effect.Name + effect.GetInstanceId();
                world.AddChild(effect);
                damageEffectList.Add(effect);
                collisionNormal = ((Vector3) paramMap[ParameterKey.COLLISION_NORMAL]);
                lookAtDirection = collisionNormal.y != -1 ? Vector3.Up : Vector3.Forward;
                effect.LookAt(-collisionNormal, lookAtDirection);
                effectTranslation = ((Vector3) paramMap[ParameterKey.COLLISION_POINT]);
                effectTranslation += collisionNormal * 0.001f;
                effect.Translation = effectTranslation;
            }
        }
    }

    private void Initialize()
    {
        damageEffectList = new Array<Node>();
    }

    public override void _EnterTree()
    {
        Initialize();
    }

    public override void _Process(float delta)
    {
        CleanOverLimitEffect();
    }

    public void SetWorld(Spatial world)
    {
        this.world = world;
    }


    [Export]
    public int effectPollSize = 8;

    [Export]
    private NodePath rootNodeNP;

    [Export]
    public PackedScene weaponDamageEffectPrefab;


    private Node globalSignal;
    private Spatial world;
    private Array<Node> damageEffectList;
    private Vector3 effectTranslation;
    private Vector3 collisionNormal;
    private Vector3 lookAtDirection;
}

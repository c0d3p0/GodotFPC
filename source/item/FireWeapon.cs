using Godot;


public class FireWeapon : Item
{
    private void Shoot(float delta)
    {
        if(!equipped)
            return;

        if(fireRateTimer.IsStopped() &&
                reloadTimer.IsStopped() && clipAmmoAmount > 0 &&
                (Input.IsActionJustPressed(PlayerInput.P1_ACTION) ||
                (automatic && Input.IsActionPressed(PlayerInput.P1_ACTION))))
        {
            CalculateTrajectoryAngle();
            ExecuteTrajectoryCast();
            CalculateRecoil();
            ComputeShot();
            EmitWeaponDamageSignal();
            EmitWeaponDataChangedSignal();
        }
        else
        {
            if(fireRateTimer.IsStopped())
                CalculateRecoilRecover(delta);
        }
    }

    private void Reload()
    {
        if(!equipped)
            return;

        if(Input.IsActionJustPressed(PlayerInput.P1_RELOAD) && 
                ammoAmount > clipAmmoAmount && clipAmmoAmount < clipSize)
        {
            reloadTimer.Start();
            EmitWeaponDataChangedSignal();
        }
    }

    public void OnReloadTimeout()
    {
        clipAmmoAmount = ammoAmount < clipSize ? ammoAmount : clipSize;
        ammoAmount -= clipAmmoAmount;
        EmitWeaponDataChangedSignal();
    }

    private void CalculateTrajectoryAngle()
    {
        rng.Randomize();
        rotationDegrees = characterBody.RotationDegrees;
        rotationDegrees += characterHead.RotationDegrees;
        rotationDegrees.y += rng.RandfRange(-actualRecoil.x, actualRecoil.x);
        rotationDegrees.x += actualRecoil.y + (actualRecoil.Normalized().y *
                rng.RandfRange(-recoilStep.y / 2, recoilStep.y / 2));
    }

    private void ExecuteTrajectoryCast()
    {
        trajectoryRayCast.Translation = characterHead.Translation;
        trajectoryRayCast.RotationDegrees = rotationDegrees;
        trajectoryRayCast.ForceRaycastUpdate();
    }

    private void CalculateRecoil()
    {
        actualRecoil.x = Mathf.Min(actualRecoil.x + recoilStep.x, recoilMaximum.x);
        actualRecoil.y = Mathf.Min(actualRecoil.y + recoilStep.y, recoilMaximum.y);
        actualRecoverRecoil.x = Mathf.Max(actualRecoverRecoil.x - recoilRecoverStep.x, 1f);
        actualRecoverRecoil.y = Mathf.Max(actualRecoverRecoil.y - recoilRecoverStep.y, 1f);
    }

    private void ComputeShot()
    {
        clipAmmoAmount--;
        fireRateTimer.Start();
    }

    private void CalculateRecoilRecover(float delta)
    {
        if(actualRecoil.x > 0f)
        {
            if(actualRecoil.x < 0.05f)
                actualRecoil.x = Mathf.Lerp(actualRecoil.x, 0, actualRecoverRecoil.x * delta);
            else
                actualRecoil.x = Mathf.Lerp(actualRecoil.x, 0, actualRecoverRecoil.x * delta);
            
            if(actualRecoil.x < 0.007f)
                actualRecoil.x = 0f;
        }
        
        if(actualRecoil.y > 0f)
        {
            if(actualRecoil.y < 0.05f)
                actualRecoil.y = Mathf.Lerp(actualRecoil.y, 0, actualRecoverRecoil.y * delta);
            else
                actualRecoil.y = Mathf.Lerp(actualRecoil.y, 0, actualRecoverRecoil.y * delta);
            
            if(actualRecoil.y < 0.007f)
                actualRecoil.y = 0f;
        }

        actualRecoverRecoil.x += Mathf.Min(recoilRecoverMaximum.x,
                recoilRecoverStep.x * delta);
        actualRecoverRecoil.y += Mathf.Min(recoilRecoverMaximum.y,
                recoilRecoverStep.y * delta);
    }

    private void ResetRecoil()
    {
        actualRecoverRecoil = recoilRecoverMaximum;
        actualRecoil = Vector2.Zero;
    }

    private void EmitWeaponDamageSignal()
    {
        paramMap.Clear();
        paramMap.Add(ParameterKey.FIRE_WEAPON, this);
        paramMap.Add(ParameterKey.COLLISION_POINT,
                trajectoryRayCast.GetCollisionPoint());
        paramMap.Add(ParameterKey.COLLISION_NORMAL,
                trajectoryRayCast.GetCollisionNormal());
        paramMap.Add(ParameterKey.DAMAGED_NODE,
                (trajectoryRayCast.GetCollider() as Node).GetParent());
        globalSignal.EmitSignal(SignalKey.ON_WEAPON_DAMAGE, paramMap);
    }

    private void EmitWeaponDataChangedSignal()
    {
        paramMap.Clear();
        paramMap.Add(ParameterKey.WEAPON_NAME, rootNode.Name);
        paramMap.Add(ParameterKey.AIM_VISIBLE, true);
        paramMap.Add(ParameterKey.CLIP_AMMO_AMOUNT, clipAmmoAmount.ToString());
        paramMap.Add(ParameterKey.AMMO_AMOUNT, ammoAmount.ToString());
        paramMap.Add(ParameterKey.RELOADING, !reloadTimer.IsStopped());
        globalSignal.EmitSignal(SignalKey.ON_WEAPON_DATA_CHANGED, paramMap);
    }

    protected override void Initialize()
    {
        base.Initialize();
        trajectoryRayCast = GetNode<RayCast>(trajectoryRayCastNP);
        fireRateTimer = GetNode<Timer>(fireRateTimerNP);
        reloadTimer = GetNode<Timer>(reloadTimerNP);
        fireRateTimer.WaitTime = fireRate;
        reloadTimer.WaitTime = reloadTime;
        ammoAmount = ammoAmount > maximumAmmoAmount ? maximumAmmoAmount : ammoAmount;
        clipAmmoAmount = clipAmmoAmount > clipSize ? clipSize : clipAmmoAmount;
        rng = new RandomNumberGenerator();
    }

    protected override void OnEquipping()
    {
        EmitWeaponDataChangedSignal();
        ResetRecoil();
    }

    public override void _PhysicsProcess(float physicsDelta)
    {
        Shoot(physicsDelta);
        Reload();
    }


    [Export]
    public Vector2 recoilMaximum = new Vector2(3f, 6f);

    [Export]
    public Vector2 recoilStep = new Vector2(0.35f, 0.8f);

    [Export]
    public Vector2 recoilRecoverMaximum = new Vector2(3.2f, 4f);

    [Export]
    public Vector2 recoilRecoverStep = new Vector2(0.35f, 0.8f);

    [Export]
    public float fireRate;

    [Export]
    public float reloadTime;

    [Export]
    public int clipSize;

    [Export]
    public int maximumAmmoAmount;

    [Export]
    public int clipAmmoAmount;

    [Export]
    public int ammoAmount;

    [Export]
    public bool automatic;

    [Export]
    public NodePath trajectoryRayCastNP;

    [Export]
    public NodePath fireRateTimerNP;

    [Export]
    public NodePath reloadTimerNP;

    
    private RayCast trajectoryRayCast;
    private Timer fireRateTimer;
    private Timer reloadTimer;

    private RandomNumberGenerator rng;
    private Vector2 actualRecoil;
    private Vector2 actualRecoverRecoil;
    private Vector3 rotationDegrees;
}

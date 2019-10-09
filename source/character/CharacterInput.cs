using Godot;


public class CharacterInput : Node
{
    private float ComputeDirection(string positiveKey, string negativeKey)
    {
        bool axisP = Input.IsActionPressed(positiveKey);
        bool axisN = Input.IsActionPressed(negativeKey);

        if(axisP && !axisN)
            return 1f;
        else if(axisN && !axisP)
            return -1f;
        else
            return 0;
    }

    private void UpdateDirection()
    {
        direction.x = ComputeDirection(PlayerInput.P1_RIGHT, PlayerInput.P1_LEFT);
        direction.y = 0f;
        direction.z = ComputeDirection(PlayerInput.P1_DOWN, PlayerInput.P1_UP);
    }

    public override void _PhysicsProcess(float delta)
    {
        UpdateDirection();
    }

    public void GetDirection(Godot.Object response)
    {
        response.EmitSignal(SignalKey.SET_DATA, direction);
    }


    private Vector3 direction;
}

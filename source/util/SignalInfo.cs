public class SignalInfo : Godot.Object
{
    public void SetData(object data)
    {
        this.data = data;
        this.dataReceived = true;
    }

    public T GetData<T>()
    {
        return (T) data;
    }

    public bool IsDataReceived()
    {
        return dataReceived;
    }

    public void Clear()
    {
        this.dataReceived = false;
        data = null;
    }

    public SignalInfo()
    {
        AddUserSignal("SetData");
        Connect("SetData", this, "SetData");
    }


    private object data;
    private bool dataReceived;
}

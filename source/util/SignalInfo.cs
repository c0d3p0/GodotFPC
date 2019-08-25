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

    public void SetDataReceived(bool dataReceived)
    {
        this.dataReceived = dataReceived;
    }

    public SignalInfo()
    {
        AddUserSignal("SetData");
        Connect("SetData", this, "SetData");
    }


    private object data;
    private bool dataReceived;
}

using Godot;


public class SignalUtil
{
    public static T Emit<T>(Node emitter, string signal, params object[] args)
    {
        T response = default(T);
        SignalInfo si = new SignalInfo();
        int length = args != null ? args.Length : 0;
        object[] newArgs = new object[length + 1];
        System.Array.Copy(args, 0, newArgs, 0, length);
        newArgs[length] = si;
        emitter.EmitSignal(signal, newArgs);

        if(!si.IsDataReceived())
        {
            GD.PushError(signal + " emitted by " + emitter.GetName() +
                    " could not return a value!");
        }
        else
            response = si.GetData<T>();

        si.Free();
        return response;
    }
}

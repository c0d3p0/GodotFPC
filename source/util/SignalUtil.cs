using System.Text;

using Godot;


public class SignalUtil
{
    public static T Emit<T>(Node emitter, string signal, params object[] args)
    {
        T response = default(T);
        int length = args != null ? args.Length : 0;
        object[] newArgs = new object[length + 1];
        System.Array.Copy(args, 0, newArgs, 0, length);
        SignalInfo signalInfo = new SignalInfo();
        newArgs[length] = signalInfo;
        emitter.EmitSignal(signal, newArgs);

        if(!signalInfo.IsDataReceived())
        {
            StringBuilder msg = new StringBuilder(signal).Append(" emitted by ");
            msg.Append(emitter.Name).Append(" could not return a value!");
            GD.PushError(msg.ToString());
        }
        else
            response = signalInfo.GetData<T>();

        return response;
    }
}

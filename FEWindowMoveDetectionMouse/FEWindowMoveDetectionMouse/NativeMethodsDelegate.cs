namespace FreeEcho.FEWindowMoveDetectionMouse;

/// <summary>
/// NativeMethodsDelegate
/// </summary>
internal class NativeMethodsDelegate
{
    public delegate System.IntPtr MouseHookCallback(int nCode, uint msg, ref MSLLHOOKSTRUCT msllhookstruct);
}

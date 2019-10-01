using System.Runtime.InteropServices;

namespace Dwscdv3.ima
{
    internal static class WinAPI
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool LockWorkStation();
    }
}

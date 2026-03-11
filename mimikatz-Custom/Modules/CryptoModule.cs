using System;
using System.Security.Cryptography.X509Certificates;

namespace MimikatzCustom.Modules
{
    public class CryptoModule
    {
        public static void EnumerateCertificates()
        {
            try
            {
                Console.WriteLine("[*] Enumerating certificates...");
                Console.WriteLine();

                string[] storeNames = { "My", "Root", "CA", "TrustedPublisher" };

                foreach (string storeName in storeNames)
                {
                    try
                    {
                        X509Store store = new X509Store(storeName, StoreLocation.CurrentUser);
                        store.Open(OpenFlags.ReadOnly);

                        Console.WriteLine($"[+] {storeName} Store ({store.Certificates.Count} certificates):");

                        foreach (X509Certificate2 cert in store.Certificates)
                        {
                            Console.WriteLine($"    Subject: {cert.Subject}");
                            Console.WriteLine($"    Issuer: {cert.Issuer}");
                            Console.WriteLine($"    Thumbprint: {cert.Thumbprint}");
                            Console.WriteLine($"    Valid From: {cert.NotBefore:yyyy-MM-dd}");
                            Console.WriteLine($"    Valid To: {cert.NotAfter:yyyy-MM-dd}");
                            Console.WriteLine($"    Has Private Key: {cert.HasPrivateKey}");
                            Console.WriteLine();
                        }

                        store.Close();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine($"[!] Access denied to {storeName} store");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[!] Error accessing {storeName} store: {ex.Message}");
                    }
                }

                // Also check Local Machine certificates if running as admin
                try
                {
                    X509Store machineStore = new X509Store("My", StoreLocation.LocalMachine);
                    machineStore.Open(OpenFlags.ReadOnly);

                    Console.WriteLine($"[+] Local Machine 'My' Store ({machineStore.Certificates.Count} certificates):");
                    foreach (X509Certificate2 cert in machineStore.Certificates)
                    {
                        Console.WriteLine($"    Subject: {cert.Subject}");
                        Console.WriteLine($"    Has Private Key: {cert.HasPrivateKey}");
                    }

                    machineStore.Close();
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("[!] Access denied to Local Machine certificate store (requires admin)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Error accessing Local Machine store: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error in EnumerateCertificates: {ex.Message}");
            }
        }
    }
}
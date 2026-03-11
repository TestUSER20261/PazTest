# Mimikatz-Custom v1.0

A C# reimplementation of mimikatz credential extraction tool with Windows security-focused modules.

## Overview

Mimikatz-Custom is an educational and security testing tool designed to demonstrate Windows credential extraction and authentication token manipulation. This C# version provides a cross-platform compatible alternative to the original C implementation while maintaining similar functionality.

## Features

### Core Modules

- **Privilege Management** (`privilege::`)
  - `privilege::debug` - Enable SeDebugPrivilege for process inspection
  
- **Security Accounts Manager** (`lsadump::`)
  - `lsadump::sam` - Dump SAM database (requires SYSTEM privileges)
  - `lsadump::secrets` - Extract LSA secrets (requires SYSTEM privileges)

- **Logon Session Enumeration** (`sekurlsa::`)
  - `sekurlsa::logonpasswords` - List active logon sessions and user information

- **Cryptography** (`crypto::`)
  - `crypto::certificates` - Enumerate user and machine certificates

- **Credential Vault** (`vault::`)
  - `vault::list` - List stored credentials from Windows Credential Manager

- **System Information**
  - `whoami` - Display current user information

## Requirements

- Windows 7 SP1 or later
- .NET 6.0 Runtime or later
- Administrator/SYSTEM privileges for full functionality (optional but recommended for sensitive operations)

## Building

### Prerequisites

- Visual Studio 2022 or later, OR
- .NET 6.0 SDK installed

### Build Instructions

```bash
dotnet build -c Release
```

### Output

```
bin/Release/net6.0/win-x64/publish/mimikatz-Custom.exe
```

## Usage

### Basic Syntax

```
mimikatz-Custom.exe <command>
```

### Examples

```bash
# Display help
mimikatz-Custom.exe

# Enable debug privilege
mimikatz-Custom.exe privilege::debug

# List local users
mimikatz-Custom.exe sekurlsa::logonpasswords

# Enumerate certificates
mimikatz-Custom.exe crypto::certificates

# List credential vault
mimikatz-Custom.exe vault::list

# Dump SAM (requires SYSTEM privilege)
mimikatz-Custom.exe lsadump::sam

# Exit
mimikatz-Custom.exe exit
```

## Privilege Requirements

| Command | Required Privilege |
|---------|-------------------|
| `privilege::debug` | Administrator |
| `sekurlsa::logonpasswords` | User (limited data) / SYSTEM (full data) |
| `lsadump::sam` | SYSTEM |
| `lsadump::secrets` | SYSTEM |
| `crypto::certificates` | User (own store) / Administrator (machine store) |
| `vault::list` | User (own vault) / Administrator (others' vaults) |

## Project Structure

```
mimikatz-Custom/
├── Program.cs                    # Main console interface
├── Modules/
│   ├── PrivilegeModule.cs       # Token privilege manipulation
│   ├── SecurlsaModule.cs        # Logon session enumeration
│   ├── LsadumpModule.cs         # SAM and LSA secret dumping
│   ├── CryptoModule.cs          # Certificate enumeration
│   └── VaultModule.cs           # Credential vault access
├── mimikatz-Custom.csproj       # Project configuration
└── README.md                     # This file
```

## Technical Implementation

### P/Invoke Usage

This tool uses Windows API calls via P/Invoke to interact with:
- `advapi32.dll` - Windows API library for token and privilege manipulation
- `vaultcli.dll` - Vault CLI API for credential management

### Security Considerations

**This tool is designed for authorized security testing and educational purposes only.** Unauthorized access to credentials is illegal. Usage is bound by laws including the Computer Fraud and Abuse Act (CFAA).

## Limitations

- Some operations require administrator or SYSTEM privileges
- Direct memory access features require kernel-mode drivers
- Modern Windows hardening (Credential Guard, etc.) may block certain operations
- DPAPI-protected credentials may not be extractable without proper keys

## Differences from Original Mimikatz

1. **C# Implementation** instead of C - easier to understand and modify
2. **Simplified Functionality** - educational focus rather than complete toolset
3. **Cross-Platform Compatible** - .NET can run on Windows, Linux (with limitations)
4. **Cleaner Code Structure** - modular design for easy extension

## Disclaimer

This software is provided for authorized security testing and educational purposes only. Unauthorized access to computer systems and credentials is illegal. The authors assume no liability for misuse or damages resulting from its use. Always obtain proper authorization before testing security systems.

## Future Enhancements

- [ ] Interactive command shell
- [ ] Direct LSA memory access
- [ ] DPAPI decryption capabilities
- [ ] Kerberos ticket extraction
- [ ] Pass-the-Hash attacks
- [ ] Credential delegation
- [ ] Multi-language support

## References

- [Windows API Documentation](https://docs.microsoft.com/en-us/windows/win32/)
- [Mimikatz GitHub Repository](https://github.com/gentilkiwi/mimikatz)
- [Windows Security Overview](https://docs.microsoft.com/en-us/windows/security/)

## Version History

### v1.0 (Initial Release)
- Basic privilege management
- SAM and LSA dumping stubs
- Certificate enumeration
- Credential vault listing
- User logon information extraction

## License

Educational use only. Not for commercial distribution.

---

**Last Updated:** March 11, 2026
**Author:** Mimikatz-Custom Development Team
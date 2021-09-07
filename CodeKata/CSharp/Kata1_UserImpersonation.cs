using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System;
using System.Security.Principal;


[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
static extern bool LogonUser(String Username, String Domain, String Password, int LogonType, int LogonProvider, out SafeAccessTokenHandle Token);

const int LOGON32_PROVIDER_DEFAULT = 0;
//This parameter causes LogonUser to create a primary token.     
const int LOGON32_LOGON_INTERACTIVE = 2;
// Call LogonUser to obtain a handle to an access token.     
SafeAccessTokenHandle safeAccessTokenHandle;

var currentUser = WindowsIdentity.GetCurrent().Name;

Console.WriteLine(currentUser.ToString());

var username = "[Your Username Goes Here]";
var domain = "[Your Domain Goes Here]";
var password = "[Your Password Goes Here]";

bool returnValue = LogonUser(username, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out safeAccessTokenHandle);
WindowsIdentity.RunImpersonated(safeAccessTokenHandle, () => {
    currentUser = WindowsIdentity.GetCurrent().Name;
    Console.WriteLine(currentUser.ToString());
});

Console.ReadLine();
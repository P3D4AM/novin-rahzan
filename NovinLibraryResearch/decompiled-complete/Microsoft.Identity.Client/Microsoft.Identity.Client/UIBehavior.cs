using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Microsoft.Identity.Client;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("UIBehavior struct is now obsolete.  Please use Prompt struct instead.See https://aka.ms/msal-net-3-breaking-changes", true)]
public struct UIBehavior
{
}

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.Identity.Client;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("Use IAccount instead (See https://aka.ms/msal-net-2-released)", true)]
public interface IUser
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	[Obsolete("Use IAccount.Username instead (See https://aka.ms/msal-net-2-released)", true)]
	string DisplayableId { get; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	[Obsolete("Use IAccount.Username instead (See https://aka.ms/msal-net-2-released)", true)]
	string Name { get; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	[Obsolete("Use IAccount.Environment instead to get the Identity Provider host (See https://aka.ms/msal-net-2-released)", true)]
	string IdentityProvider { get; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	[Obsolete("Use IAccount.HomeAccountId.Identifier instead to get the user identifier (See https://aka.ms/msal-net-2-released)", true)]
	string Identifier { get; }
}

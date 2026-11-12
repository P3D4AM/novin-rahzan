using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Client.Extensions.Msal;

public class CacheChangedEventArgs : EventArgs
{
	public readonly IEnumerable<string> AccountsAdded;

	public readonly IEnumerable<string> AccountsRemoved;

	public CacheChangedEventArgs(IEnumerable<string> added, IEnumerable<string> removed)
	{
		AccountsAdded = added;
		AccountsRemoved = removed;
	}
}

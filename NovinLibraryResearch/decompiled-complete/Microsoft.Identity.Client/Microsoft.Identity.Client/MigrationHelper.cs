using System;

namespace Microsoft.Identity.Client;

internal static class MigrationHelper
{
	public static NotImplementedException CreateMsalNet3BreakingChangesException()
	{
		return new NotImplementedException("See https://aka.ms/msal-net-3-breaking-changes");
	}
}

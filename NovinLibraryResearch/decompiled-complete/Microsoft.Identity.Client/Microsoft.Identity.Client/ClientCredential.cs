using System;

namespace Microsoft.Identity.Client;

[Obsolete("Use ConfidentialClientApplicationBuilder.WithCertificate or WithClientSecret instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
public sealed class ClientCredential
{
	internal ClientAssertionCertificate Certificate
	{
		get
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
	}

	internal string Assertion
	{
		get
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
		set
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
	}

	internal long ValidTo
	{
		get
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
		set
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
	}

	internal bool ContainsX5C
	{
		get
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
		set
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
	}

	internal string Audience
	{
		get
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
		set
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
	}

	internal string Secret
	{
		get
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
	}

	public ClientCredential(ClientAssertionCertificate certificate)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	public ClientCredential(string secret)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}
}

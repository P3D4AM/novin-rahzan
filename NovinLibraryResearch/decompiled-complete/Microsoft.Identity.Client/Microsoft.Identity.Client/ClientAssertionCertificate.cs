using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client;

[Obsolete("Use ConfidentialClientApplicationBuilder.WithCertificate instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
public sealed class ClientAssertionCertificate
{
	public static int MinKeySizeInBits => 2048;

	public X509Certificate2 Certificate
	{
		get
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
	}

	internal string Thumbprint
	{
		get
		{
			throw MigrationHelper.CreateMsalNet3BreakingChangesException();
		}
	}

	public ClientAssertionCertificate(X509Certificate2 certificate)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	internal byte[] Sign(ICryptographyManager cryptographyManager, string message)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}
}

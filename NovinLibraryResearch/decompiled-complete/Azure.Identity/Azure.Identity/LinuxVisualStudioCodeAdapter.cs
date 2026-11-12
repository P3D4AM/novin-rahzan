using System;
using System.IO;
using Azure.Core;

namespace Azure.Identity;

internal sealed class LinuxVisualStudioCodeAdapter : IVisualStudioCodeAdapter
{
	private static readonly string s_userSettingsJsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Code", "User", "settings.json");

	public string GetUserSettingsPath()
	{
		return s_userSettingsJsonPath;
	}

	public string GetCredentials(string serviceName, string accountName)
	{
		Azure.Core.Argument.AssertNotNullOrEmpty(serviceName, "serviceName");
		Azure.Core.Argument.AssertNotNullOrEmpty(accountName, "accountName");
		IntPtr schemaPtr = GetLibsecretSchema();
		try
		{
			return LookupPassword(in schemaPtr, serviceName, accountName);
		}
		finally
		{
			LinuxNativeMethods.secret_schema_unref(schemaPtr);
		}
	}

	private static string LookupPassword(in IntPtr schemaPtr, string serviceName, string accountName)
	{
		return LinuxNativeMethods.secret_password_lookup_sync(schemaPtr, IntPtr.Zero, "service", serviceName, "account", accountName);
	}

	private static IntPtr GetLibsecretSchema()
	{
		return LinuxNativeMethods.secret_schema_new("org.freedesktop.Secret.Generic", LinuxNativeMethods.SecretSchemaFlags.SECRET_SCHEMA_DONT_MATCH_NAME, "service", LinuxNativeMethods.SecretSchemaAttributeType.SECRET_SCHEMA_ATTRIBUTE_STRING, "account", LinuxNativeMethods.SecretSchemaAttributeType.SECRET_SCHEMA_ATTRIBUTE_STRING);
	}
}

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Identity.Client.Extensions.Msal;

internal static class Libsecret
{
	public enum SecretSchemaAttributeType
	{
		SECRET_SCHEMA_ATTRIBUTE_STRING,
		SECRET_SCHEMA_ATTRIBUTE_INTEGER,
		SECRET_SCHEMA_ATTRIBUTE_BOOLEAN
	}

	public enum SecretSchemaFlags
	{
		SECRET_SCHEMA_NONE = 0,
		SECRET_SCHEMA_DONT_MATCH_NAME = 2
	}

	[DllImport("libsecret-1.so.0", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr secret_schema_new(string name, int flags, string attribute1, int attribute1Type, string attribute2, int attribute2Type, IntPtr end);

	[DllImport("libsecret-1.so.0", CallingConvention = CallingConvention.StdCall)]
	public static extern int secret_password_store_sync(IntPtr schema, string collection, string label, string password, IntPtr cancellable, out IntPtr error, string attribute1Type, string attribute1Value, string attribute2Type, string attribute2Value, IntPtr end);

	[DllImport("libsecret-1.so.0", CallingConvention = CallingConvention.StdCall)]
	public static extern string secret_password_lookup_sync(IntPtr schema, IntPtr cancellable, out IntPtr error, string attribute1Type, string attribute1Value, string attribute2Type, string attribute2Value, IntPtr end);

	[DllImport("libsecret-1.so.0", CallingConvention = CallingConvention.StdCall)]
	public static extern int secret_password_clear_sync(IntPtr schema, IntPtr cancellable, out IntPtr error, string attribute1Type, string attribute1Value, string attribute2Type, string attribute2Value, IntPtr end);
}

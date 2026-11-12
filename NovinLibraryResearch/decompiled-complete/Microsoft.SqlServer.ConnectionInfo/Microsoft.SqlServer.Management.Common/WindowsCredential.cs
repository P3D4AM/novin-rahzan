using System;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.SqlServer.Management.Common;

[SuppressUnmanagedCodeSecurity]
public class WindowsCredential
{
	private enum CRED_TYPE
	{
		GENERIC = 1,
		DOMAIN_PASSWORD,
		DOMAIN_CERTIFICATE,
		DOMAIN_VISIBLE_PASSWORD
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct Credential
	{
		public uint Flags;

		public CRED_TYPE Type;

		public IntPtr TargetName;

		public IntPtr Comment;

		public FILETIME LastWritten;

		public uint CredentialBlobSize;

		public IntPtr CredentialBlob;

		public uint Persist;

		public uint AttributeCount;

		public IntPtr Attributes;

		public IntPtr TargetAlias;

		public IntPtr UserName;
	}

	[DllImport("Advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "CredReadW", SetLastError = true)]
	private static extern bool CredRead(string target, CRED_TYPE type, int reservedFlag, out IntPtr CredentialPtr);

	[DllImport("Advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "CredWriteW", SetLastError = true)]
	private static extern bool CredWrite([In] ref Credential userCredential, [In] uint flags);

	[DllImport("Advapi32.dll", SetLastError = true)]
	private static extern bool CredFree([In] IntPtr cred);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	private static extern bool CredDelete(string targetName, CRED_TYPE type, int flags);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	private static extern bool CredEnumerate(string targetName, int flags, out int count, out IntPtr pCredential);

	private static string GetKey(string repo, string instance, int authType, string user, Guid serverType, string version)
	{
		return $"Microsoft:{repo}:{version}:{instance}:{user}:{serverType}:{authType}";
	}

	private static string GetAdsKey(string instance, string database, string authType, string user)
	{
		return $"Microsoft.SqlTools|itemtype:Profile|id:providerName:MSSQL|applicationName:azdata|authenticationType:{authType}|database:{database ?? string.Empty}|server:{instance}|user:{user}";
	}

	private static void SetSqlCredential(string targetName, string user, SecureString password)
	{
		Credential credential = default(Credential);
		credential.TargetName = Marshal.StringToCoTaskMemUni(targetName);
		credential.UserName = Marshal.StringToCoTaskMemUni(user);
		credential.CredentialBlob = Marshal.SecureStringToCoTaskMemUnicode(password);
		credential.CredentialBlobSize = (uint)(password.Length * 2);
		credential.AttributeCount = 0u;
		credential.Attributes = IntPtr.Zero;
		credential.Comment = IntPtr.Zero;
		credential.TargetAlias = IntPtr.Zero;
		credential.Type = CRED_TYPE.GENERIC;
		credential.Persist = 2u;
		Credential userCredential = credential;
		try
		{
			if (!CredWrite(ref userCredential, 0u))
			{
				throw new Win32Exception(StringConnectionInfo.UnableToSavePasswordFormat(user));
			}
		}
		finally
		{
			Marshal.FreeCoTaskMem(userCredential.TargetName);
			Marshal.FreeCoTaskMem(userCredential.UserName);
			Marshal.ZeroFreeCoTaskMemUnicode(userCredential.CredentialBlob);
		}
	}

	private static SecureString GetSqlCredential(string targetName)
	{
		SecureString result = null;
		if (CredRead(targetName, CRED_TYPE.GENERIC, 0, out var CredentialPtr))
		{
			try
			{
				Credential credential = (Credential)Marshal.PtrToStructure(CredentialPtr, typeof(Credential));
				result = EncryptionUtility.EncryptString((credential.CredentialBlob == IntPtr.Zero) ? string.Empty : Marshal.PtrToStringUni(credential.CredentialBlob, (int)credential.CredentialBlobSize / 2));
			}
			finally
			{
				CredFree(CredentialPtr);
			}
		}
		return result;
	}

	private static void RemoveCredential(string targetName)
	{
		CredDelete(targetName, CRED_TYPE.GENERIC, 0);
	}

	[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
	public static void SetSqlSsmsCredential(string instance, int authType, string user, Guid serverType, SecureString password, string version)
	{
		SetSqlCredential(GetKey("SSMS", instance, authType, user, serverType, version), user, password);
	}

	[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
	public static void SetSqlRegSvrCredential(string instance, int authType, string user, Guid serverType, SecureString password, string version)
	{
		SetSqlCredential(GetKey("RegSvr", instance, authType, user, serverType, version), user, password);
	}

	[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
	public static SecureString GetSqlSsmsCredential(string instance, int authType, string user, Guid serverType, string version)
	{
		return GetSqlCredential(GetKey("SSMS", instance, authType, user, serverType, version));
	}

	[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
	public static SecureString GetSqlRegSvrCredential(string instance, int authType, string user, Guid serverType, string version)
	{
		return GetSqlCredential(GetKey("RegSvr", instance, authType, user, serverType, version));
	}

	[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
	public static void RemoveSsmsCredential(string instance, int authType, string user, Guid serverType, string version)
	{
		RemoveCredential(GetKey("SSMS", instance, authType, user, serverType, version));
	}

	[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
	public static void RemoveRegSvrCredential(string instance, int authType, string user, Guid serverType, string version)
	{
		RemoveCredential(GetKey("RegSvr", instance, authType, user, serverType, version));
	}

	[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
	public static void SetAzureDataStudioCredential(string instance, string database, string authType, string user, SecureString password)
	{
		SetSqlCredential(GetAdsKey(instance, database, authType, user), user, password);
	}

	[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
	public static SecureString GetAzureDataStudioCredential(string instance, string database, string authType, string user)
	{
		return GetSqlCredential(GetAdsKey(instance, database, authType, user));
	}
}

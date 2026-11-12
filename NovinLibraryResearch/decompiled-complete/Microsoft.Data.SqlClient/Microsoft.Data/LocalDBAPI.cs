using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using Microsoft.Data.SqlClient;

namespace Microsoft.Data;

internal static class LocalDBAPI
{
	[SuppressUnmanagedCodeSecurity]
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int LocalDBCreateInstanceDelegate([MarshalAs(UnmanagedType.LPWStr)] string version, [MarshalAs(UnmanagedType.LPWStr)] string instance, uint flags);

	[SuppressUnmanagedCodeSecurity]
	[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	private delegate int LocalDBFormatMessageDelegate(int hrLocalDB, uint dwFlags, uint dwLanguageId, StringBuilder buffer, ref uint buflen);

	private class InstanceInfo
	{
		internal readonly string version;

		internal bool created;

		internal InstanceInfo(string version)
		{
			this.version = version;
			created = false;
		}
	}

	private const string const_localDbPrefix = "(localdb)\\";

	private const string const_partialTrustFlagKey = "ALLOW_LOCALDB_IN_PARTIAL_TRUST";

	private static PermissionSet _fullTrust = null;

	private static bool _partialTrustFlagChecked = false;

	private static bool _partialTrustAllowed = false;

	private static IntPtr s_userInstanceDLLHandle = IntPtr.Zero;

	private static object s_dllLock = new object();

	private static LocalDBCreateInstanceDelegate s_localDBCreateInstance = null;

	private static LocalDBFormatMessageDelegate s_localDBFormatMessage = null;

	private const uint const_LOCALDB_TRUNCATE_ERR_MESSAGE = 1u;

	private const int const_ErrorMessageBufferSize = 1024;

	private static object s_configLock = new object();

	private static Dictionary<string, InstanceInfo> s_configurableInstances = null;

	private static IntPtr UserInstanceDLLHandle
	{
		get
		{
			if (s_userInstanceDLLHandle == IntPtr.Zero)
			{
				bool lockTaken = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(s_dllLock, ref lockTaken);
					if (s_userInstanceDLLHandle == IntPtr.Zero)
					{
						SNINativeMethodWrapper.SNIQueryInfo(SNINativeMethodWrapper.QTypes.SNI_QUERY_LOCALDB_HMODULE, ref s_userInstanceDLLHandle);
						if (!(s_userInstanceDLLHandle != IntPtr.Zero))
						{
							SNINativeMethodWrapper.SNI_Error pErrorStruct = default(SNINativeMethodWrapper.SNI_Error);
							SNINativeMethodWrapper.SNIGetLastError(out pErrorStruct);
							throw CreateLocalDBException(StringsHelper.GetString("LocalDB_FailedGetDLLHandle"), null, 0, (int)pErrorStruct.sniError);
						}
						SqlClientEventSource.Log.TryTraceEvent("<sc.LocalDBAPI.UserInstanceDLLHandle> LocalDB - handle obtained");
					}
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(s_dllLock);
					}
				}
			}
			return s_userInstanceDLLHandle;
		}
	}

	private static LocalDBCreateInstanceDelegate LocalDBCreateInstance
	{
		get
		{
			if (s_localDBCreateInstance == null)
			{
				bool lockTaken = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(s_dllLock, ref lockTaken);
					if (s_localDBCreateInstance == null)
					{
						IntPtr procAddress = SafeNativeMethods.GetProcAddress(UserInstanceDLLHandle, "LocalDBCreateInstance");
						if (procAddress == IntPtr.Zero)
						{
							int lastWin32Error = Marshal.GetLastWin32Error();
							SqlClientEventSource.Log.TryTraceEvent("<sc.LocalDBAPI.LocalDBCreateInstance> GetProcAddress for LocalDBCreateInstance error 0x{0}", lastWin32Error);
							throw CreateLocalDBException(StringsHelper.GetString("LocalDB_MethodNotFound"));
						}
						s_localDBCreateInstance = (LocalDBCreateInstanceDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(LocalDBCreateInstanceDelegate));
					}
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(s_dllLock);
					}
				}
			}
			return s_localDBCreateInstance;
		}
	}

	private static LocalDBFormatMessageDelegate LocalDBFormatMessage
	{
		get
		{
			if (s_localDBFormatMessage == null)
			{
				bool lockTaken = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(s_dllLock, ref lockTaken);
					if (s_localDBFormatMessage == null)
					{
						IntPtr procAddress = SafeNativeMethods.GetProcAddress(UserInstanceDLLHandle, "LocalDBFormatMessage");
						if (procAddress == IntPtr.Zero)
						{
							int lastWin32Error = Marshal.GetLastWin32Error();
							SqlClientEventSource.Log.TryTraceEvent("<sc.LocalDBAPI.LocalDBFormatMessage> GetProcAddress for LocalDBFormatMessage error 0x{0}", lastWin32Error);
							throw CreateLocalDBException(StringsHelper.GetString("LocalDB_MethodNotFound"));
						}
						s_localDBFormatMessage = (LocalDBFormatMessageDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(LocalDBFormatMessageDelegate));
					}
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(s_dllLock);
					}
				}
			}
			return s_localDBFormatMessage;
		}
	}

	internal static string GetLocalDbInstanceNameFromServerName(string serverName)
	{
		if (serverName == null)
		{
			return null;
		}
		serverName = serverName.TrimStart(Array.Empty<char>());
		if (!serverName.StartsWith("(localdb)\\", StringComparison.OrdinalIgnoreCase))
		{
			return null;
		}
		string text = serverName.Substring("(localdb)\\".Length).Trim();
		if (text.Length == 0)
		{
			return null;
		}
		return text;
	}

	internal static void ReleaseDLLHandles()
	{
		s_userInstanceDLLHandle = IntPtr.Zero;
		s_localDBFormatMessage = null;
		s_localDBCreateInstance = null;
	}

	internal static string GetLocalDBMessage(int hrCode)
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			uint buflen = (uint)stringBuilder.Capacity;
			int num = LocalDBFormatMessage(hrCode, 1u, (uint)CultureInfo.CurrentCulture.LCID, stringBuilder, ref buflen);
			if (num >= 0)
			{
				return stringBuilder.ToString();
			}
			stringBuilder = new StringBuilder(1024);
			buflen = (uint)stringBuilder.Capacity;
			num = LocalDBFormatMessage(hrCode, 1u, 0u, stringBuilder, ref buflen);
			if (num >= 0)
			{
				return stringBuilder.ToString();
			}
			return string.Format(CultureInfo.CurrentCulture, "{0} (0x{1:X}).", StringsHelper.GetString("LocalDB_UnobtainableMessage"), num);
		}
		catch (SqlException ex)
		{
			return string.Format(CultureInfo.CurrentCulture, "{0} ({1}).", StringsHelper.GetString("LocalDB_UnobtainableMessage"), ex.Message);
		}
	}

	private static SqlException CreateLocalDBException(string errorMessage, string instance = null, int localDbError = 0, int sniError = 0)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		int infoNumber = ((localDbError == 0) ? sniError : localDbError);
		if (sniError != 0)
		{
			string sNIErrorMessage = SQL.GetSNIErrorMessage(sniError);
			errorMessage = string.Format(null, "{0} (error: {1} - {2})", errorMessage, sniError, sNIErrorMessage);
		}
		sqlErrorCollection.Add(new SqlError(infoNumber, 0, 20, instance, errorMessage, null, 0));
		if (localDbError != 0)
		{
			sqlErrorCollection.Add(new SqlError(infoNumber, 0, 20, instance, GetLocalDBMessage(localDbError), null, 0));
		}
		SqlException ex = SqlException.CreateException(sqlErrorCollection, null);
		ex._doNotReconnect = true;
		return ex;
	}

	internal static void DemandLocalDBPermissions()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		if (_partialTrustAllowed)
		{
			return;
		}
		if (!_partialTrustFlagChecked)
		{
			object data = AppDomain.CurrentDomain.GetData("ALLOW_LOCALDB_IN_PARTIAL_TRUST");
			if (data != null && data is bool)
			{
				_partialTrustAllowed = (bool)data;
			}
			_partialTrustFlagChecked = true;
			if (_partialTrustAllowed)
			{
				return;
			}
		}
		if (_fullTrust == null)
		{
			_fullTrust = (PermissionSet)new NamedPermissionSet("FullTrust");
		}
		_fullTrust.Demand();
	}

	internal static void AssertLocalDBPermissions()
	{
		_partialTrustAllowed = true;
	}

	internal static void CreateLocalDBInstance(string instance)
	{
		DemandLocalDBPermissions();
		if (s_configurableInstances == null)
		{
			bool lockTaken = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.Enter(s_configLock, ref lockTaken);
				if (s_configurableInstances == null)
				{
					Dictionary<string, InstanceInfo> dictionary = new Dictionary<string, InstanceInfo>(StringComparer.OrdinalIgnoreCase);
					object section = ConfigurationManager.GetSection("system.data.localdb");
					if (section != null)
					{
						if (!(section is LocalDBConfigurationSection localDBConfigurationSection))
						{
							throw CreateLocalDBException(StringsHelper.GetString("LocalDB_BadConfigSectionType"));
						}
						foreach (LocalDBInstanceElement item in (ConfigurationElementCollection)localDBConfigurationSection.LocalDbInstances)
						{
							dictionary.Add(item.Name.Trim(), new InstanceInfo(item.Version.Trim()));
						}
					}
					else
					{
						SqlClientEventSource.Log.TryTraceEvent("<sc.LocalDBAPI.CreateLocalDBInstance> No system.data.localdb section found in configuration");
					}
					s_configurableInstances = dictionary;
				}
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(s_configLock);
				}
			}
		}
		InstanceInfo value = null;
		if (s_configurableInstances.TryGetValue(instance, out value) && !value.created)
		{
			if (value.version.Contains("\0"))
			{
				throw CreateLocalDBException(StringsHelper.GetString("LocalDB_InvalidVersion"), instance);
			}
			int num = LocalDBCreateInstance(value.version, instance, 0u);
			SqlClientEventSource.Log.TryTraceEvent("<sc.LocalDBAPI.CreateLocalDBInstance> Starting creation of instance {0} version {1}", instance, value.version);
			if (num < 0)
			{
				throw CreateLocalDBException(StringsHelper.GetString("LocalDB_CreateFailed"), instance, num);
			}
			SqlClientEventSource.Log.TryTraceEvent("<sc.LocalDBAPI.CreateLocalDBInstance> Finished creation of instance {0}", instance);
			value.created = true;
		}
	}
}

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Identity.Client.Extensions.Msal;

[ExcludeFromCodeCoverage]
internal static class MacKeyChain
{
	public static void WriteKey(string serviceName, string accountName, byte[] value)
	{
		IntPtr itemRef = IntPtr.Zero;
		IntPtr passwordData = IntPtr.Zero;
		try
		{
			int num = MacNativeMethods.SecKeychainFindGenericPassword(IntPtr.Zero, (uint)serviceName.Length, serviceName, (uint)accountName.Length, accountName, out var _, out passwordData, out itemRef);
			if (num != 0 && num != -25300)
			{
				throw new Exception(string.Format(CultureInfo.CurrentCulture, "SecKeychainFindGenericPassword failed with error code: {0}", new object[1] { num }));
			}
			if (itemRef != IntPtr.Zero)
			{
				num = MacNativeMethods.SecKeychainItemModifyAttributesAndData(itemRef, IntPtr.Zero, (uint)value.Length, value);
				if (num != 0)
				{
					throw new Exception(string.Format(CultureInfo.CurrentCulture, "SecKeychainItemModifyAttributesAndData failed with error code: {0}", new object[1] { num }));
				}
			}
			else
			{
				num = MacNativeMethods.SecKeychainAddGenericPassword(IntPtr.Zero, (uint)serviceName.Length, serviceName, (uint)accountName.Length, accountName, (uint)value.Length, value, out itemRef);
				if (num != 0)
				{
					throw new Exception(string.Format(CultureInfo.CurrentCulture, "SecKeychainAddGenericPassword failed with error code: {0}", new object[1] { num }));
				}
			}
		}
		finally
		{
			ReleaseItemRefAndValuePtr(ref itemRef, ref passwordData);
		}
	}

	public static byte[] RetrieveKey(string serviceName, string accountName, TraceSourceLogger logger = null)
	{
		IntPtr itemRef = IntPtr.Zero;
		IntPtr passwordData = IntPtr.Zero;
		byte[] array = null;
		StringBuilder stringBuilder = new StringBuilder();
		TraceEventType traceEventType = TraceEventType.Information;
		try
		{
			stringBuilder.AppendLine("SecKeychainFindGenericPassword, for serviceName " + serviceName + " and accountName " + accountName);
			uint passwordLength;
			int num = MacNativeMethods.SecKeychainFindGenericPassword(IntPtr.Zero, (uint)serviceName.Length, serviceName, (uint)accountName.Length, accountName, out passwordLength, out passwordData, out itemRef);
			stringBuilder.AppendLine($"Status: '{num}'");
			switch (num)
			{
			case -25300:
				traceEventType = TraceEventType.Error;
				stringBuilder.AppendLine("Failed, item not found");
				return null;
			default:
				traceEventType = TraceEventType.Error;
				stringBuilder.AppendLine($"Failed, other error {num}");
				throw new Exception(string.Format(CultureInfo.CurrentCulture, "SecKeychainFindGenericPassword failed with error code: {0}", new object[1] { num }));
			case 0:
				if (itemRef != IntPtr.Zero)
				{
					stringBuilder.AppendLine("SecKeychainFindGenericPassword succeeded");
					array = new byte[passwordLength];
					Marshal.Copy(passwordData, array, 0, array.Length);
				}
				break;
			}
		}
		finally
		{
			ReleaseItemRefAndValuePtr(ref itemRef, ref passwordData);
			if (logger != null)
			{
				if (traceEventType == TraceEventType.Information)
				{
					logger.LogInformation(stringBuilder.ToString());
				}
				else
				{
					logger.LogError(stringBuilder.ToString());
				}
			}
		}
		return array;
	}

	public static void DeleteKey(string serviceName, string accountName)
	{
		IntPtr itemRef = IntPtr.Zero;
		IntPtr passwordData = IntPtr.Zero;
		try
		{
			int num = MacNativeMethods.SecKeychainFindGenericPassword(IntPtr.Zero, (uint)serviceName.Length, serviceName, (uint)accountName.Length, accountName, out var _, out passwordData, out itemRef);
			switch (num)
			{
			case -25300:
				break;
			default:
				throw new Exception(string.Format(CultureInfo.CurrentCulture, "SecKeychainFindGenericPassword failed with error code: {0}", new object[1] { num }));
			case 0:
				if (!(itemRef == IntPtr.Zero))
				{
					num = MacNativeMethods.SecKeychainItemDelete(itemRef);
					if (num != 0)
					{
						throw new Exception(string.Format(CultureInfo.CurrentCulture, "SecKeychainItemDelete failed with error code: {0}", new object[1] { num }));
					}
				}
				break;
			}
		}
		finally
		{
			ReleaseItemRefAndValuePtr(ref itemRef, ref passwordData);
		}
	}

	private static void ReleaseItemRefAndValuePtr(ref IntPtr itemRef, ref IntPtr valuePtr)
	{
		ReleaseItemRef(ref itemRef);
		ReleaseValuePtr(ref valuePtr);
	}

	private static void ReleaseItemRef(ref IntPtr itemRef)
	{
		if (itemRef != IntPtr.Zero)
		{
			MacNativeMethods.CFRelease(itemRef);
			itemRef = IntPtr.Zero;
		}
	}

	private static void ReleaseValuePtr(ref IntPtr valuePtr)
	{
		if (valuePtr != IntPtr.Zero)
		{
			MacNativeMethods.SecKeychainItemFreeContent(IntPtr.Zero, valuePtr);
			valuePtr = IntPtr.Zero;
		}
	}
}

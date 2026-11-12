using System;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

internal sealed class TdsParserStaticMethods
{
	private TdsParserStaticMethods()
	{
	}

	internal static void AliasRegistryLookup(ref string host, ref string protocol)
	{
		if (ADP.IsEmpty(host))
		{
			return;
		}
		string text = (string)ADP.LocalMachineRegistryValue("SOFTWARE\\Microsoft\\MSSQLServer\\Client\\ConnectTo", host);
		if (ADP.IsEmpty(text))
		{
			return;
		}
		int num = text.IndexOf(',');
		if (-1 == num)
		{
			return;
		}
		string text2 = text.Substring(0, num).ToLower(CultureInfo.InvariantCulture);
		if (num + 1 >= text.Length)
		{
			return;
		}
		string text3 = text.Substring(num + 1);
		if ("dbnetlib" == text2)
		{
			num = text3.IndexOf(':');
			if (-1 != num && num + 1 < text3.Length)
			{
				text2 = text3.Substring(0, num);
				if (SqlConnectionString.ValidProtocol(text2))
				{
					protocol = text2;
					host = text3.Substring(num + 1);
				}
			}
		}
		else
		{
			protocol = (string)SqlConnectionString.NetlibMapping()[text2];
			if (protocol != null)
			{
				host = text3;
			}
		}
	}

	internal static byte[] ObfuscatePassword(string password)
	{
		byte[] array = new byte[password.Length << 1];
		for (int i = 0; i < password.Length; i++)
		{
			int num = password[i];
			byte b = (byte)((uint)num & 0xFFu);
			byte b2 = (byte)((uint)(num >> 8) & 0xFFu);
			array[i << 1] = (byte)((uint)(((b & 0xF) << 4) | (b >> 4)) ^ 0xA5u);
			array[(i << 1) + 1] = (byte)((uint)(((b2 & 0xF) << 4) | (b2 >> 4)) ^ 0xA5u);
		}
		return array;
	}

	internal static int GetCurrentProcessIdForTdsLoginOnly()
	{
		return Microsoft.Data.Common.SafeNativeMethods.GetCurrentProcessId();
	}

	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal static int GetCurrentThreadIdForTdsLoginOnly()
	{
		return AppDomain.GetCurrentThreadId();
	}

	internal static byte[] GetNetworkPhysicalAddressForTdsLoginOnly()
	{
		int num = 0;
		byte[] array = null;
		object obj = ADP.LocalMachineRegistryValue("SOFTWARE\\Description\\Microsoft\\Rpc\\UuidTemporaryData", "NetworkAddressLocal");
		if (obj is int)
		{
			num = (int)obj;
		}
		if (num <= 0)
		{
			obj = ADP.LocalMachineRegistryValue("SOFTWARE\\Description\\Microsoft\\Rpc\\UuidTemporaryData", "NetworkAddress");
			if (obj is byte[])
			{
				array = (byte[])obj;
			}
		}
		if (array == null)
		{
			array = new byte[6];
			Random random = new Random();
			random.NextBytes(array);
		}
		return array;
	}

	internal static int GetTimeoutMilliseconds(long timeoutTime)
	{
		if (long.MaxValue == timeoutTime)
		{
			return -1;
		}
		long num = ADP.TimerRemainingMilliseconds(timeoutTime);
		if (num < 0)
		{
			return 0;
		}
		if (num > int.MaxValue)
		{
			return int.MaxValue;
		}
		return (int)num;
	}

	internal static long GetTimeout(long timeoutMilliseconds)
	{
		if (timeoutMilliseconds <= 0)
		{
			return long.MaxValue;
		}
		try
		{
			return checked(ADP.TimerCurrent() + ADP.TimerFromMilliseconds(timeoutMilliseconds));
		}
		catch (OverflowException)
		{
			return long.MaxValue;
		}
	}

	internal static bool TimeoutHasExpired(long timeoutTime)
	{
		bool result = false;
		if (timeoutTime != 0L && long.MaxValue != timeoutTime)
		{
			result = ADP.TimerHasExpired(timeoutTime);
		}
		return result;
	}

	internal static int NullAwareStringLength(string str)
	{
		return str?.Length ?? 0;
	}

	internal static int GetRemainingTimeout(int timeout, long start)
	{
		if (timeout <= 0)
		{
			return timeout;
		}
		long num = ADP.TimerRemainingSeconds(start + ADP.TimerFromSeconds(timeout));
		if (num <= 0)
		{
			return 1;
		}
		return checked((int)num);
	}
}

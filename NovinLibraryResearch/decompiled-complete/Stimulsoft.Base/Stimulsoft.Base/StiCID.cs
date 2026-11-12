using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Stimulsoft.Base;

public class StiCID : StiObject
{
	private const string Key = "fjk2dpfko5epefko4prmk";

	private const string Undefined = "Undefined";

	private const string Prefix = "PL534950";

	public string MachineAddress { get; set; }

	public string MachineName { get; set; }

	public string MachineUserName { get; set; }

	public string MachineGuid { get; set; }

	public static string GetDefault()
	{
		return "PL534950" + StiEncryption.Encrypt(GetDeveloperCID().SaveToString(), "fjk2dpfko5epefko4prmk");
	}

	private static StiCID GetDeveloperCID()
	{
		return new StiCID(GetCurrentMachineName(), GetCurrentMachineAddress(), GetCurrentMachineUserName(), GetCurrentMachineGuid());
	}

	private static string GetCurrentMachineName()
	{
		try
		{
			return Environment.MachineName;
		}
		catch
		{
			return "Undefined";
		}
	}

	private static string GetCurrentMachineGuid()
	{
		try
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			RegistryKey val = RegistryKey.OpenBaseKey((RegistryHive)(-2147483646), (RegistryView)256);
			RegistryKey val2 = RegistryKey.OpenBaseKey((RegistryHive)(-2147483646), (RegistryView)512);
			RegistryKey val3 = val.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography", (RegistryKeyPermissionCheck)1);
			RegistryKey val4 = val2.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography", (RegistryKeyPermissionCheck)1);
			object value = val3.GetValue("MachineGuid", (object)"default");
			object value2 = val4.GetValue("MachineGuid", (object)"default");
			val3.Close();
			val4.Close();
			val.Close();
			val2.Close();
			val3.Dispose();
			val4.Dispose();
			val.Dispose();
			val2.Dispose();
			val3 = null;
			val4 = null;
			val = null;
			val2 = null;
			if (value != null && value.ToString() != "default")
			{
				return value.ToString();
			}
			if (value2 != null && value2.ToString() != "default")
			{
				return value2.ToString();
			}
		}
		catch (Exception)
		{
			return "Undefined";
		}
		return "Undefined";
	}

	private static string GetCurrentMachineAddress()
	{
		try
		{
			IEnumerable<NetworkInterface> enumerable = from n in NetworkInterface.GetAllNetworkInterfaces()
				where n.OperationalStatus == OperationalStatus.Up
				select n;
			if (enumerable == null)
			{
				return "Undefined";
			}
			NetworkInterface networkInterface = enumerable.FirstOrDefault((NetworkInterface n) => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet);
			if (networkInterface == null)
			{
				networkInterface = enumerable.FirstOrDefault((NetworkInterface n) => n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211);
			}
			if (networkInterface == null)
			{
				return "Undefined";
			}
			return string.Join("-", Regex.Matches(networkInterface.GetPhysicalAddress().ToString(), "\\w{2}").Cast<Match>());
		}
		catch
		{
			return "Undefined";
		}
	}

	private static string GetCurrentMachineUserName()
	{
		try
		{
			return Environment.UserName;
		}
		catch
		{
			return "Undefined";
		}
	}

	public static bool IsCID(string cid)
	{
		return cid?.StartsWith("PL534950") ?? false;
	}

	private StiCID()
	{
	}

	public StiCID(string cid)
	{
		if (IsCID(cid))
		{
			cid = cid.Substring("PL534950".Length);
			LoadFromString(StiEncryption.Decrypt(cid, "fjk2dpfko5epefko4prmk"));
		}
	}

	public StiCID(string machineName, string machineAddress, string machineUserName)
	{
		MachineName = machineName;
		MachineAddress = machineAddress;
		MachineUserName = machineUserName;
	}

	public StiCID(string machineName, string machineAddress, string machineUserName, string machineGuid)
	{
		MachineName = machineName;
		MachineAddress = machineAddress;
		MachineUserName = machineUserName;
		MachineGuid = machineGuid;
	}
}

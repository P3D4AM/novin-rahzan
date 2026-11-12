using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.PlatformsCommon.Factories;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.Internal;

internal static class MsalIdHelper
{
	private static readonly Lazy<string> s_msalVersion = new Lazy<string>(delegate
	{
		string fullName = typeof(MsalIdHelper).GetTypeInfo().Assembly.FullName;
		Match match = new Regex("Version=[\\d]+.[\\d+]+.[\\d]+.[\\d]+").Match(fullName);
		return (!match.Success) ? null : match.Groups[0].Value.Split(new char[1] { '=' }, StringSplitOptions.None)[1];
	});

	public static IDictionary<string, string> GetMsalIdParameters(ICoreLogger logger)
	{
		IPlatformProxy platformProxy = PlatformProxyFactory.CreatePlatformProxy(logger);
		if (platformProxy == null)
		{
			throw new MsalClientException("platform_not_supported", "Platform Not Supported");
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			["x-client-SKU"] = platformProxy.GetProductName(),
			["x-client-Ver"] = GetMsalVersion()
		};
		string processorArchitecture = platformProxy.GetProcessorArchitecture();
		if (processorArchitecture != null)
		{
			dictionary["x-client-CPU"] = processorArchitecture;
		}
		string operatingSystem = platformProxy.GetOperatingSystem();
		if (operatingSystem != null)
		{
			dictionary["x-client-OS"] = operatingSystem;
		}
		string deviceModel = platformProxy.GetDeviceModel();
		if (deviceModel != null)
		{
			dictionary["x-client-DM"] = deviceModel;
		}
		return dictionary;
	}

	public static string GetMsalVersion()
	{
		return s_msalVersion.Value;
	}
}

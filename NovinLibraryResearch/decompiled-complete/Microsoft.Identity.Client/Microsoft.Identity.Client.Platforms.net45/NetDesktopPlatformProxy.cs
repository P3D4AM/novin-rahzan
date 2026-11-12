using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client.AuthScheme.PoP;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Platforms.Features.Windows;
using Microsoft.Identity.Client.Platforms.net45.Http;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.PlatformsCommon.Shared;
using Microsoft.Identity.Client.TelemetryCore.Internal;
using Microsoft.Identity.Client.UI;
using Microsoft.Win32;

namespace Microsoft.Identity.Client.Platforms.net45;

internal class NetDesktopPlatformProxy : AbstractPlatformProxy
{
	private bool IsWindows
	{
		get
		{
			PlatformID platform = Environment.OSVersion.Platform;
			if ((uint)platform <= 3u)
			{
				return true;
			}
			return false;
		}
	}

	public override bool BrokerSupportsWamAccounts => true;

	public NetDesktopPlatformProxy(ICoreLogger logger)
		: base(logger)
	{
	}

	public override Task<string> GetUserPrincipalNameAsync()
	{
		return Task.FromResult(GetUserPrincipalName(8));
	}

	private string GetUserPrincipalName(int nameFormat)
	{
		uint userNameSize = 0u;
		WindowsNativeMethods.GetUserNameEx(nameFormat, null, ref userNameSize);
		if (userNameSize == 0)
		{
			throw new MsalClientException("get_user_name_failed", "Failed to get user name", new Win32Exception(Marshal.GetLastWin32Error()));
		}
		StringBuilder stringBuilder = new StringBuilder((int)userNameSize);
		if (!WindowsNativeMethods.GetUserNameEx(nameFormat, stringBuilder, ref userNameSize))
		{
			throw new MsalClientException("get_user_name_failed", "Failed to get user name", new Win32Exception(Marshal.GetLastWin32Error()));
		}
		return stringBuilder.ToString();
	}

	public override Task<bool> IsUserLocalAsync(RequestContext requestContext)
	{
		if (WindowsIdentity.GetCurrent() != null)
		{
			return Task.FromResult(((ClaimsIdentity)(object)WindowsIdentity.GetCurrent()).Name.Split(new char[1] { '\\' })[0].ToUpperInvariant().Equals(Environment.MachineName.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
		}
		return Task.FromResult(result: false);
	}

	public override bool IsDomainJoined()
	{
		if (!IsWindows)
		{
			return false;
		}
		bool result = false;
		try
		{
			IntPtr domain;
			WindowsNativeMethods.NetJoinStatus status;
			int num = WindowsNativeMethods.NetGetJoinInformation(null, out domain, out status);
			if (domain != IntPtr.Zero)
			{
				WindowsNativeMethods.NetApiBufferFree(domain);
			}
			result = num == 0 && status == WindowsNativeMethods.NetJoinStatus.NetSetupDomainName;
		}
		catch (Exception exWithPii)
		{
			base.Logger.WarningPii(exWithPii);
		}
		return result;
	}

	public override string GetEnvironmentVariable(string variable)
	{
		if (string.IsNullOrWhiteSpace(variable))
		{
			throw new ArgumentNullException("variable");
		}
		return Environment.GetEnvironmentVariable(variable);
	}

	public override string GetDefaultRedirectUri(string clientId, bool useRecommendedRedirectUri = false)
	{
		if (useRecommendedRedirectUri)
		{
			return "https://login.microsoftonline.com/common/oauth2/nativeclient";
		}
		return "urn:ietf:wg:oauth:2.0:oob";
	}

	public override ILegacyCachePersistence CreateLegacyCachePersistence()
	{
		return new InMemoryLegacyCachePersistance();
	}

	public override ITokenCacheAccessor CreateTokenCacheAccessor()
	{
		return new InMemoryTokenCacheAccessor(base.Logger);
	}

	protected override IWebUIFactory CreateWebUiFactory()
	{
		return new NetDesktopWebUIFactory();
	}

	protected override string InternalGetDeviceModel()
	{
		return null;
	}

	protected override string InternalGetOperatingSystem()
	{
		return Environment.OSVersion.ToString();
	}

	protected override string InternalGetProcessorArchitecture()
	{
		if (!IsWindows)
		{
			return null;
		}
		return WindowsNativeMethods.GetProcessorArchitecture();
	}

	protected override string InternalGetCallingApplicationName()
	{
		return Assembly.GetEntryAssembly()?.GetName()?.Name;
	}

	protected override string InternalGetCallingApplicationVersion()
	{
		return Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString();
	}

	protected override string InternalGetDeviceId()
	{
		try
		{
			return (from nic in NetworkInterface.GetAllNetworkInterfaces()
				where nic.OperationalStatus == OperationalStatus.Up
				select nic.GetPhysicalAddress()?.ToString()).FirstOrDefault();
		}
		catch (EntryPointNotFoundException)
		{
			return null;
		}
	}

	protected override string InternalGetProductName()
	{
		return "MSAL.Desktop";
	}

	protected override ICryptographyManager InternalGetCryptographyManager()
	{
		return new NetDesktopCryptographyManager();
	}

	public override string GetDeviceNetworkState()
	{
		return string.Empty;
	}

	public override string GetDevicePlatformTelemetryId()
	{
		object value = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\SQMClient", false).GetValue("MachineId");
		if (value == null)
		{
			return string.Empty;
		}
		string? text = value.ToString();
		string text2 = GetUserPrincipalName(2);
		int num = text2.IndexOf('@');
		if (num >= 0)
		{
			text2 = text2.Substring(0, num);
		}
		string input = text + text2;
		byte[] array = InternalGetCryptographyManager().CreateSha256HashBytes(input);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.Append($"{b:x2}");
		}
		return stringBuilder.ToString();
	}

	public override string GetMatsOsPlatform()
	{
		return MatsConverter.AsString(OsPlatform.Win32);
	}

	public override int GetMatsOsPlatformCode()
	{
		return MatsConverter.AsInt(OsPlatform.Win32);
	}

	protected override IPlatformLogger InternalGetPlatformLogger()
	{
		return new EventSourcePlatformLogger();
	}

	protected override IFeatureFlags CreateFeatureFlags()
	{
		return new NetDesktopFeatureFlags();
	}

	public override Task StartDefaultOsBrowserAsync(string url)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = url,
				UseShellExecute = true
			});
		}
		catch
		{
			url = url.Replace("&", "^&");
			Process.Start(new ProcessStartInfo("cmd", "/c start " + url)
			{
				CreateNoWindow = true
			});
		}
		return Task.FromResult(0);
	}

	public override IPoPCryptoProvider GetDefaultPoPCryptoProvider()
	{
		return PoPProviderFactory.GetOrCreateProvider();
	}

	public override IDeviceAuthManager CreateDeviceAuthManager()
	{
		return new NetDesktopDeviceAuthManager();
	}

	public override IMsalHttpClientFactory CreateDefaultHttpClientFactory()
	{
		return new NetDesktopHttpClientFactory();
	}
}

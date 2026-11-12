using System;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Instance;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.Internal.Logger;
using Microsoft.Identity.Client.OAuth2.Throttling;
using Microsoft.Identity.Client.PlatformsCommon.Factories;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.TelemetryCore;
using Microsoft.Identity.Client.TelemetryCore.Http;
using Microsoft.Identity.Client.WsTrust;

namespace Microsoft.Identity.Client.Internal;

internal class ServiceBundle : IServiceBundle
{
	public ICoreLogger DefaultLogger { get; }

	public IHttpManager HttpManager { get; }

	public IMatsTelemetryManager MatsTelemetryManager { get; }

	public IInstanceDiscoveryManager InstanceDiscoveryManager { get; }

	public IWsTrustWebRequestManager WsTrustWebRequestManager { get; }

	public IAuthorityEndpointResolutionManager AuthorityEndpointResolutionManager { get; }

	public IPlatformProxy PlatformProxy { get; private set; }

	public IApplicationConfiguration Config { get; }

	public ITelemetryClient Mats { get; }

	public IDeviceAuthManager DeviceAuthManager { get; }

	public IHttpTelemetryManager HttpTelemetryManager { get; }

	public IThrottlingProvider ThrottlingManager { get; }

	internal ServiceBundle(ApplicationConfiguration config, bool shouldClearCaches = false)
	{
		Config = config;
		DefaultLogger = new MsalLogger(Guid.Empty, config.ClientName, config.ClientVersion, config.LogLevel, config.EnablePiiLogging, config.IsDefaultPlatformLoggingEnabled, config.LoggingCallback);
		PlatformProxy = config.PlatformProxy ?? PlatformProxyFactory.CreatePlatformProxy(DefaultLogger);
		HttpManager = config.HttpManager ?? new HttpManager(config.HttpClientFactory ?? PlatformProxy.CreateDefaultHttpClientFactory());
		HttpTelemetryManager = new HttpTelemetryManager();
		if (config.TelemetryConfig != null)
		{
			Mats = TelemetryClient.CreateMats(config, PlatformProxy, config.TelemetryConfig);
			MatsTelemetryManager = Mats?.TelemetryManager ?? new TelemetryManager(config, PlatformProxy, config.TelemetryCallback);
		}
		else
		{
			MatsTelemetryManager = new TelemetryManager(config, PlatformProxy, config.TelemetryCallback);
		}
		InstanceDiscoveryManager = new InstanceDiscoveryManager(HttpManager, MatsTelemetryManager, shouldClearCaches, config.CustomInstanceDiscoveryMetadata, config.CustomInstanceDiscoveryMetadataUri);
		WsTrustWebRequestManager = new WsTrustWebRequestManager(HttpManager);
		ThrottlingManager = SingletonThrottlingManager.GetInstance();
		AuthorityEndpointResolutionManager = new AuthorityEndpointResolutionManager(this, shouldClearCaches);
		DeviceAuthManager = PlatformProxy.CreateDeviceAuthManager();
	}

	public static ServiceBundle Create(ApplicationConfiguration config)
	{
		return new ServiceBundle(config);
	}

	public void SetPlatformProxyForTest(IPlatformProxy platformProxy)
	{
		PlatformProxy = platformProxy;
	}
}

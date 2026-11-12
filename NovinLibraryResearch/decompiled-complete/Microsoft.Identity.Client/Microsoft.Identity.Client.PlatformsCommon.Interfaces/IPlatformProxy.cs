using System.Threading.Tasks;
using Microsoft.Identity.Client.AuthScheme.PoP;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Broker;
using Microsoft.Identity.Client.UI;

namespace Microsoft.Identity.Client.PlatformsCommon.Interfaces;

internal interface IPlatformProxy
{
	bool IsSystemWebViewAvailable { get; }

	bool UseEmbeddedWebViewDefault { get; }

	ICryptographyManager CryptographyManager { get; }

	IPlatformLogger PlatformLogger { get; }

	bool BrokerSupportsWamAccounts { get; }

	string GetDeviceModel();

	string GetEnvironmentVariable(string variable);

	string GetOperatingSystem();

	string GetProcessorArchitecture();

	Task<string> GetUserPrincipalNameAsync();

	bool IsDomainJoined();

	Task<bool> IsUserLocalAsync(RequestContext requestContext);

	string GetCallingApplicationName();

	string GetCallingApplicationVersion();

	string GetDeviceId();

	string GetDefaultRedirectUri(string clientId, bool useRecommendedRedirectUri = false);

	string GetProductName();

	ILegacyCachePersistence CreateLegacyCachePersistence();

	ITokenCacheAccessor CreateTokenCacheAccessor();

	ITokenCacheBlobStorage CreateTokenCacheBlobStorage();

	IWebUIFactory GetWebUiFactory();

	IPoPCryptoProvider GetDefaultPoPCryptoProvider();

	string GetDevicePlatformTelemetryId();

	string GetDeviceNetworkState();

	int GetMatsOsPlatformCode();

	string GetMatsOsPlatform();

	void SetWebUiFactory(IWebUIFactory webUiFactory);

	IFeatureFlags GetFeatureFlags();

	void SetFeatureFlags(IFeatureFlags featureFlags);

	Task StartDefaultOsBrowserAsync(string url);

	IBroker CreateBroker(CoreUIParent uiParent);

	IDeviceAuthManager CreateDeviceAuthManager();

	void SetBrokerForTest(IBroker broker);

	bool CanBrokerSupportSilentAuth();

	IMsalHttpClientFactory CreateDefaultHttpClientFactory();
}

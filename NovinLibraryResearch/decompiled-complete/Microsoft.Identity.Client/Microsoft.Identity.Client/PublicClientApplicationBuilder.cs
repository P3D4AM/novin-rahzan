using System;
using System.Windows.Forms;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.PlatformsCommon.Factories;

namespace Microsoft.Identity.Client;

public sealed class PublicClientApplicationBuilder : AbstractApplicationBuilder<PublicClientApplicationBuilder>
{
	internal PublicClientApplicationBuilder(ApplicationConfiguration configuration)
		: base(configuration)
	{
	}

	public static PublicClientApplicationBuilder CreateWithApplicationOptions(PublicClientApplicationOptions options)
	{
		return new PublicClientApplicationBuilder(new ApplicationConfiguration()).WithOptions(options);
	}

	public static PublicClientApplicationBuilder Create(string clientId)
	{
		return new PublicClientApplicationBuilder(new ApplicationConfiguration()).WithClientId(clientId);
	}

	internal PublicClientApplicationBuilder WithUserTokenLegacyCachePersistenceForTest(ILegacyCachePersistence legacyCachePersistence)
	{
		base.Config.UserTokenLegacyCachePersistenceForTest = legacyCachePersistence;
		return this;
	}

	public PublicClientApplicationBuilder WithDefaultRedirectUri()
	{
		base.Config.UseRecommendedDefaultRedirectUri = true;
		return this;
	}

	public PublicClientApplicationBuilder WithIosKeychainSecurityGroup(string keychainSecurityGroup)
	{
		return this;
	}

	public PublicClientApplicationBuilder WithBroker(bool enableBroker = true)
	{
		if (!base.Config.ExperimentalFeaturesEnabled)
		{
			throw new MsalClientException("experimental_feature", MsalErrorMessage.ExperimentalFeature("WithBroker"));
		}
		base.Config.IsBrokerEnabled = enableBroker;
		return this;
	}

	public PublicClientApplicationBuilder WithParentActivityOrWindow(Func<object> parentActivityOrWindowFunc)
	{
		return WithParentFunc(parentActivityOrWindowFunc);
	}

	private PublicClientApplicationBuilder WithParentFunc(Func<object> parentFunc)
	{
		base.Config.ParentActivityOrWindowFunc = parentFunc;
		return this;
	}

	[CLSCompliant(false)]
	public PublicClientApplicationBuilder WithParentActivityOrWindow(Func<IWin32Window> windowFunc)
	{
		if (windowFunc == null)
		{
			throw new ArgumentNullException("windowFunc");
		}
		return WithParentFunc(() => windowFunc());
	}

	[CLSCompliant(false)]
	public PublicClientApplicationBuilder WithParentActivityOrWindow(Func<IntPtr> windowFunc)
	{
		if (windowFunc == null)
		{
			throw new ArgumentNullException("windowFunc");
		}
		return WithParentFunc(() => windowFunc());
	}

	public IPublicClientApplication Build()
	{
		return BuildConcrete();
	}

	internal PublicClientApplication BuildConcrete()
	{
		return new PublicClientApplication(BuildConfiguration());
	}

	internal override void Validate()
	{
		base.Validate();
		if (string.IsNullOrWhiteSpace(base.Config.RedirectUri))
		{
			base.Config.RedirectUri = PlatformProxyFactory.CreatePlatformProxy(null).GetDefaultRedirectUri(base.Config.ClientId, base.Config.UseRecommendedDefaultRedirectUri);
		}
		if (!Uri.TryCreate(base.Config.RedirectUri, UriKind.Absolute, out Uri _))
		{
			throw new InvalidOperationException(MsalErrorMessage.InvalidRedirectUriReceived(base.Config.RedirectUri));
		}
	}
}

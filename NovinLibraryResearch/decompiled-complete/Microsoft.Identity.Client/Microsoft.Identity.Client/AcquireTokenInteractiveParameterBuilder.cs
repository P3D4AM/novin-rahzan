using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Extensibility;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client;

public sealed class AcquireTokenInteractiveParameterBuilder : AbstractPublicClientAcquireTokenParameterBuilder<AcquireTokenInteractiveParameterBuilder>
{
	private AcquireTokenInteractiveParameters Parameters { get; } = new AcquireTokenInteractiveParameters();


	internal override ApiTelemetryId ApiTelemetryId => ApiTelemetryId.AcquireTokenInteractive;

	internal AcquireTokenInteractiveParameterBuilder(IPublicClientApplicationExecutor publicClientApplicationExecutor)
		: base(publicClientApplicationExecutor)
	{
	}

	internal void SetCustomWebUi(ICustomWebUi customWebUi)
	{
		Parameters.CustomWebUi = customWebUi;
	}

	internal static AcquireTokenInteractiveParameterBuilder Create(IPublicClientApplicationExecutor publicClientApplicationExecutor, IEnumerable<string> scopes)
	{
		return new AcquireTokenInteractiveParameterBuilder(publicClientApplicationExecutor).WithCurrentSynchronizationContext().WithScopes(scopes);
	}

	internal AcquireTokenInteractiveParameterBuilder WithCurrentSynchronizationContext()
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithCurrentSynchronizationContext);
		Parameters.UiParent.SynchronizationContext = SynchronizationContext.Current;
		return this;
	}

	internal AcquireTokenInteractiveParameterBuilder WithParentActivityOrWindowFunc(Func<object> parentActivityOrWindowFunc)
	{
		if (parentActivityOrWindowFunc != null)
		{
			WithParentActivityOrWindow(parentActivityOrWindowFunc());
		}
		return this;
	}

	public AcquireTokenInteractiveParameterBuilder WithUseEmbeddedWebView(bool useEmbeddedWebView)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithEmbeddedWebView, useEmbeddedWebView);
		Parameters.UseEmbeddedWebView = (useEmbeddedWebView ? WebViewPreference.Embedded : WebViewPreference.System);
		return this;
	}

	public AcquireTokenInteractiveParameterBuilder WithSystemWebViewOptions(SystemWebViewOptions options)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithSystemBrowserOptions);
		Parameters.UiParent.SystemWebViewOptions = options;
		return this;
	}

	public AcquireTokenInteractiveParameterBuilder WithLoginHint(string loginHint)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithLoginHint);
		Parameters.LoginHint = loginHint;
		return this;
	}

	public AcquireTokenInteractiveParameterBuilder WithAccount(IAccount account)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAccount);
		Parameters.Account = account;
		return this;
	}

	public AcquireTokenInteractiveParameterBuilder WithExtraScopesToConsent(IEnumerable<string> extraScopesToConsent)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithExtraScopesToConsent);
		Parameters.ExtraScopesToConsent = extraScopesToConsent;
		return this;
	}

	public AcquireTokenInteractiveParameterBuilder WithPrompt(Prompt prompt)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithPrompt);
		Parameters.Prompt = prompt;
		return this;
	}

	public AcquireTokenInteractiveParameterBuilder WithParentActivityOrWindow(object parent)
	{
		return WithParentObject(parent);
	}

	private AcquireTokenInteractiveParameterBuilder WithParentObject(object parent)
	{
		base.CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithParent);
		IWin32Window val = (IWin32Window)((parent is IWin32Window) ? parent : null);
		if (val != null)
		{
			Parameters.UiParent.OwnerWindow = val;
		}
		else if (parent is IntPtr intPtr)
		{
			Parameters.UiParent.OwnerWindow = intPtr;
		}
		return this;
	}

	[CLSCompliant(false)]
	public AcquireTokenInteractiveParameterBuilder WithParentActivityOrWindow(IWin32Window window)
	{
		if (window == null)
		{
			throw new ArgumentNullException("window");
		}
		return WithParentObject(window);
	}

	[CLSCompliant(false)]
	public AcquireTokenInteractiveParameterBuilder WithParentActivityOrWindow(IntPtr window)
	{
		return WithParentObject(window);
	}

	protected override void Validate()
	{
		base.Validate();
		if (Parameters.UiParent.SystemWebViewOptions != null && Parameters.UseEmbeddedWebView == WebViewPreference.Embedded)
		{
			throw new MsalClientException("embedded_webview_not_compatible_default_browser", "You configured MSAL interactive authentication to use an embedded WebView and you also configured system WebView options. These are mutually exclusive. See https://aka.ms/msal-net-os-browser");
		}
		if (Parameters.UiParent.SystemWebViewOptions != null && Parameters.UseEmbeddedWebView == WebViewPreference.NotSpecified)
		{
			WithUseEmbeddedWebView(useEmbeddedWebView: false);
		}
		Parameters.LoginHint = ((!string.IsNullOrWhiteSpace(Parameters.LoginHint)) ? Parameters.LoginHint : Parameters.Account?.Username);
	}

	internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
	{
		return base.PublicClientApplicationExecutor.ExecuteAsync(base.CommonParameters, Parameters, cancellationToken);
	}

	internal override ApiEvent.ApiIds CalculateApiEventId()
	{
		return ApiEvent.ApiIds.AcquireTokenInteractive;
	}
}

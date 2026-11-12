using System.Collections.Generic;
using System.Text;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Extensibility;
using Microsoft.Identity.Client.UI;

namespace Microsoft.Identity.Client.ApiConfig.Parameters;

internal class AcquireTokenInteractiveParameters : IAcquireTokenParameters
{
	public Prompt Prompt { get; set; } = Prompt.NotSpecified;


	public CoreUIParent UiParent { get; } = new CoreUIParent();


	public IEnumerable<string> ExtraScopesToConsent { get; set; } = new List<string>();


	public WebViewPreference UseEmbeddedWebView { get; set; }

	public string LoginHint { get; set; }

	public IAccount Account { get; set; }

	public ICustomWebUi CustomWebUi { get; set; }

	public void LogParameters(ICoreLogger logger)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("=== InteractiveParameters Data ===");
		stringBuilder.AppendLine("LoginHint provided: " + !string.IsNullOrEmpty(LoginHint));
		stringBuilder.AppendLine("User provided: " + (Account != null));
		stringBuilder.AppendLine("UseEmbeddedWebView: " + UseEmbeddedWebView);
		stringBuilder.AppendLine("ExtraScopesToConsent: " + string.Join(";", ExtraScopesToConsent ?? new List<string>()));
		stringBuilder.AppendLine("Prompt: " + Prompt.PromptValue);
		stringBuilder.AppendLine("HasCustomWebUi: " + (CustomWebUi != null));
		UiParent.SystemWebViewOptions?.LogParameters(logger);
		logger.Info(stringBuilder.ToString());
	}
}

using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Client;

public abstract class ApplicationOptions
{
	public string ClientId { get; set; }

	public string TenantId { get; set; }

	public AadAuthorityAudience AadAuthorityAudience { get; set; }

	public string Instance { get; set; }

	public AzureCloudInstance AzureCloudInstance { get; set; }

	public string RedirectUri { get; set; }

	public LogLevel LogLevel { get; set; }

	public bool EnablePiiLogging { get; set; }

	public bool IsDefaultPlatformLoggingEnabled { get; set; }

	[Obsolete("Should use ClientName and ClientVersion properties instead of Component", true)]
	public string Component { get; set; }

	public string ClientName { get; set; }

	public string ClientVersion { get; set; }

	public IEnumerable<string> ClientCapabilities { get; set; }
}

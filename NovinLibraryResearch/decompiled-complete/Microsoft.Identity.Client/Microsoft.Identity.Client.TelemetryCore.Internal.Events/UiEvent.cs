namespace Microsoft.Identity.Client.TelemetryCore.Internal.Events;

internal class UiEvent : EventBase
{
	public const string UserCancelledKey = "msal.user_cancelled";

	public const string AccessDeniedKey = "msal.access_denied";

	public bool UserCancelled
	{
		set
		{
			base["msal.user_cancelled"] = value.ToString().ToLowerInvariant();
		}
	}

	public bool AccessDenied
	{
		set
		{
			base["msal.access_denied"] = value.ToString().ToLowerInvariant();
		}
	}

	public UiEvent(string correlationId)
		: base("msal.ui_event", correlationId)
	{
	}
}

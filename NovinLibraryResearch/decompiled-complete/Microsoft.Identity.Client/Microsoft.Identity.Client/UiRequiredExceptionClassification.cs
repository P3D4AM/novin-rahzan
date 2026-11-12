namespace Microsoft.Identity.Client;

public enum UiRequiredExceptionClassification
{
	None,
	MessageOnly,
	BasicAction,
	AdditionalAction,
	ConsentRequired,
	UserPasswordExpired,
	PromptNeverFailed,
	AcquireTokenSilentFailed
}

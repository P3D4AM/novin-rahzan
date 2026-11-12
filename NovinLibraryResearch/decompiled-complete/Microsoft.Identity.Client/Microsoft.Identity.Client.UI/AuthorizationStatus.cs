namespace Microsoft.Identity.Client.UI;

internal enum AuthorizationStatus
{
	Success,
	ErrorHttp,
	ProtocolError,
	UserCancel,
	UnknownError
}

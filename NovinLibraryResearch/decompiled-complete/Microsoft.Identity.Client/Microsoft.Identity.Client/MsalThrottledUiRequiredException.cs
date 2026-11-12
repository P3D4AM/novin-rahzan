namespace Microsoft.Identity.Client;

public class MsalThrottledUiRequiredException : MsalUiRequiredException
{
	public MsalUiRequiredException OriginalServiceException { get; }

	public MsalThrottledUiRequiredException(MsalUiRequiredException originalException)
		: base(originalException.ErrorCode, originalException.Message, originalException.InnerException, originalException.Classification)
	{
		base.SubError = originalException.SubError;
		base.StatusCode = originalException.StatusCode;
		base.Claims = originalException.Claims;
		base.CorrelationId = originalException.CorrelationId;
		base.ResponseBody = originalException.ResponseBody;
		base.Headers = originalException.Headers;
		OriginalServiceException = originalException;
	}
}

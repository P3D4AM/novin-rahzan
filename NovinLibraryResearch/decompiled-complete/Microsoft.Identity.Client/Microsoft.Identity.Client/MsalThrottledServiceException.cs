namespace Microsoft.Identity.Client;

public class MsalThrottledServiceException : MsalServiceException
{
	public MsalServiceException OriginalServiceException { get; }

	public MsalThrottledServiceException(MsalServiceException originalException)
		: base(originalException.ErrorCode, originalException.Message, originalException.InnerException)
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

using System;
using System.Globalization;
using System.Net.Http.Headers;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client;

public class MsalServiceException : MsalException
{
	private const string ClaimsKey = "claims";

	private const string ResponseBodyKey = "response_body";

	private const string CorrelationIdKey = "correlation_id";

	private const string SubErrorKey = "sub_error";

	public int StatusCode { get; internal set; }

	public string Claims { get; internal set; }

	public string ResponseBody { get; set; }

	public HttpResponseHeaders Headers { get; set; }

	public string CorrelationId { get; set; }

	internal string SubError { get; set; }

	public MsalServiceException(string errorCode, string errorMessage)
		: base(errorCode, errorMessage)
	{
		if (string.IsNullOrWhiteSpace(errorMessage))
		{
			throw new ArgumentNullException("errorMessage");
		}
	}

	public MsalServiceException(string errorCode, string errorMessage, int statusCode)
		: this(errorCode, errorMessage)
	{
		StatusCode = statusCode;
	}

	public MsalServiceException(string errorCode, string errorMessage, Exception innerException)
		: base(errorCode, errorMessage, innerException)
	{
	}

	public MsalServiceException(string errorCode, string errorMessage, int statusCode, Exception innerException)
		: base(errorCode, errorMessage, innerException)
	{
		StatusCode = statusCode;
	}

	public MsalServiceException(string errorCode, string errorMessage, int statusCode, string claims, Exception innerException)
		: this(errorCode, errorMessage, statusCode, innerException)
	{
		Claims = claims;
	}

	internal bool IsAadUnavailable()
	{
		if (StatusCode != 429 && StatusCode < 500)
		{
			return string.Equals(base.ErrorCode, "request_timeout", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	public override string ToString()
	{
		return base.ToString() + string.Format(CultureInfo.InvariantCulture, "\n\tStatusCode: {0} \n\tResponseBody: {1} \n\tHeaders: {2}", StatusCode, ResponseBody, Headers);
	}

	internal override void PopulateJson(JObject jobj)
	{
		base.PopulateJson(jobj);
		jobj["claims"] = Claims;
		jobj["response_body"] = ResponseBody;
		jobj["correlation_id"] = CorrelationId;
		jobj["sub_error"] = SubError;
	}

	internal override void PopulateObjectFromJson(JObject jobj)
	{
		base.PopulateObjectFromJson(jobj);
		Claims = JsonUtils.GetExistingOrEmptyString(jobj, "claims");
		ResponseBody = JsonUtils.GetExistingOrEmptyString(jobj, "response_body");
		CorrelationId = JsonUtils.GetExistingOrEmptyString(jobj, "correlation_id");
		SubError = JsonUtils.GetExistingOrEmptyString(jobj, "sub_error");
	}
}

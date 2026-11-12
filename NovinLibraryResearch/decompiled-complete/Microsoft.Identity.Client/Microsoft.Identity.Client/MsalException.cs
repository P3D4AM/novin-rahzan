using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.PlatformsCommon.Factories;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client;

public class MsalException : Exception
{
	private string _errorCode;

	private const string ExceptionTypeKey = "type";

	private const string ErrorCodeKey = "error_code";

	private const string ErrorDescriptionKey = "error_description";

	private static readonly Lazy<Dictionary<string, Type>> s_typeNameToType = new Lazy<Dictionary<string, Type>>(() => new Dictionary<string, Type>
	{
		{
			typeof(MsalException).Name,
			typeof(MsalException)
		},
		{
			typeof(MsalClientException).Name,
			typeof(MsalClientException)
		},
		{
			typeof(MsalServiceException).Name,
			typeof(MsalServiceException)
		},
		{
			typeof(MsalUiRequiredException).Name,
			typeof(MsalUiRequiredException)
		}
	});

	public string ErrorCode
	{
		get
		{
			return _errorCode;
		}
		private set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentNullException("ErrorCode");
			}
			_errorCode = value;
		}
	}

	public MsalException()
		: base("Unknown error")
	{
		ErrorCode = "unknown_error";
	}

	public MsalException(string errorCode)
	{
		ErrorCode = errorCode;
	}

	public MsalException(string errorCode, string errorMessage)
		: base(errorMessage)
	{
		if (string.IsNullOrWhiteSpace(Message))
		{
			throw new ArgumentNullException("errorMessage");
		}
		ErrorCode = errorCode;
	}

	public MsalException(string errorCode, string errorMessage, Exception innerException)
		: base(errorMessage, innerException)
	{
		if (string.IsNullOrWhiteSpace(Message))
		{
			throw new ArgumentNullException("errorMessage");
		}
		ErrorCode = errorCode;
	}

	public override string ToString()
	{
		string productName = PlatformProxyFactory.CreatePlatformProxy(null).GetProductName();
		string msalVersion = MsalIdHelper.GetMsalVersion();
		string text = ((base.InnerException == null) ? string.Empty : string.Format(CultureInfo.InvariantCulture, "\nInner Excception: {0}", base.InnerException.ToString()));
		return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}: \n\tErrorCode: {3}\n{4}{5}", productName, msalVersion, GetType().Name, ErrorCode, base.ToString(), text);
	}

	internal virtual void PopulateJson(JObject jobj)
	{
		jobj["type"] = GetType().Name;
		jobj["error_code"] = ErrorCode;
		jobj["error_description"] = Message;
	}

	internal virtual void PopulateObjectFromJson(JObject jobj)
	{
	}

	public string ToJsonString()
	{
		JObject jObject = new JObject();
		PopulateJson(jObject);
		return jObject.ToString();
	}

	public static MsalException FromJsonString(string json)
	{
		JObject jObject = JObject.Parse(json);
		string key = jObject.Value<string>("type");
		if (s_typeNameToType.Value.TryGetValue(key, out var value))
		{
			string existingOrEmptyString = JsonUtils.GetExistingOrEmptyString(jObject, "error_code");
			string existingOrEmptyString2 = JsonUtils.GetExistingOrEmptyString(jObject, "error_description");
			MsalException obj = Activator.CreateInstance(value, existingOrEmptyString, existingOrEmptyString2) as MsalException;
			obj.PopulateObjectFromJson(jObject);
			return obj;
		}
		throw new MsalClientException("json_parse_failed", "Attempted to deserialize an MsalException but the type was unknown.");
	}
}

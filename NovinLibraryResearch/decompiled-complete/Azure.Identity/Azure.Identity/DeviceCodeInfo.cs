using System;
using System.Collections.Generic;
using Microsoft.Identity.Client;

namespace Azure.Identity;

public struct DeviceCodeInfo
{
	public string UserCode { get; private set; }

	public string DeviceCode { get; private set; }

	public Uri VerificationUri { get; private set; }

	public DateTimeOffset ExpiresOn { get; private set; }

	public string Message { get; private set; }

	public string ClientId { get; private set; }

	public IReadOnlyCollection<string> Scopes { get; private set; }

	internal DeviceCodeInfo(DeviceCodeResult deviceCode)
		: this(deviceCode.UserCode, deviceCode.DeviceCode, new Uri(deviceCode.VerificationUrl), deviceCode.ExpiresOn, deviceCode.Message, deviceCode.ClientId, deviceCode.Scopes)
	{
	}

	internal DeviceCodeInfo(string userCode, string deviceCode, Uri verificationUri, DateTimeOffset expiresOn, string message, string clientId, IReadOnlyCollection<string> scopes)
	{
		UserCode = userCode;
		DeviceCode = deviceCode;
		VerificationUri = verificationUri;
		ExpiresOn = expiresOn;
		Message = message;
		ClientId = clientId;
		Scopes = scopes;
	}
}

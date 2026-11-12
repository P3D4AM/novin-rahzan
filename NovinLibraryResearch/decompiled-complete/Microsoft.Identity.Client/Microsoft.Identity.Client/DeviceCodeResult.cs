using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.Identity.Client;

public class DeviceCodeResult
{
	public string UserCode { get; }

	public string DeviceCode { get; }

	public string VerificationUrl { get; }

	public DateTimeOffset ExpiresOn { get; }

	public long Interval { get; }

	public string Message { get; }

	public string ClientId { get; }

	public IReadOnlyCollection<string> Scopes { get; }

	internal DeviceCodeResult(string userCode, string deviceCode, string verificationUrl, DateTimeOffset expiresOn, long interval, string message, string clientId, ISet<string> scopes)
	{
		UserCode = userCode;
		DeviceCode = deviceCode;
		VerificationUrl = verificationUrl;
		ExpiresOn = expiresOn;
		Interval = interval;
		Message = message;
		ClientId = clientId;
		Scopes = new ReadOnlyCollection<string>(scopes.AsEnumerable().ToList());
	}
}

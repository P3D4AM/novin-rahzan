using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.TelemetryCore.Internal.Events;

[DebuggerDisplay("Name = {this[EventNameKey]} InProgress = {this[ElapsedTimeKey] == \"-1\" }")]
internal abstract class EventBase : Dictionary<string, string>
{
	protected const string EventNamePrefix = "msal.";

	public const string EventNameKey = "msal.event_name";

	public const string StartTimeKey = "msal.start_time";

	public const string ElapsedTimeKey = "msal.elapsed_time";

	private readonly long _startTimestamp;

	public const string TenantPlaceHolder = "<tenant>";

	public string EventId { get; }

	public string CorrelationId
	{
		get
		{
			return base["Microsoft.MSAL.correlation_id"];
		}
		set
		{
			base["Microsoft.MSAL.correlation_id"] = value;
		}
	}

	public EventBase(string eventName, string correlationId)
	{
		base["msal.event_name"] = eventName;
		_startTimestamp = CurrentUnixTimeMilliseconds();
		base["msal.start_time"] = _startTimestamp.ToString(CultureInfo.InvariantCulture);
		base["msal.elapsed_time"] = "-1";
		CorrelationId = correlationId;
		EventId = Guid.NewGuid().AsMatsCorrelationId();
	}

	protected static long CurrentUnixTimeMilliseconds()
	{
		return CoreHelpers.DateTimeToUnixTimestampMilliseconds(DateTimeOffset.Now);
	}

	public void Stop()
	{
		base["msal.elapsed_time"] = (CurrentUnixTimeMilliseconds() - _startTimestamp).ToString(CultureInfo.InvariantCulture);
	}

	public static string ScrubTenant(Uri uri)
	{
		if (!uri.IsAbsoluteUri)
		{
			throw new ArgumentException("Requires an absolute uri");
		}
		if (!KnownMetadataProvider.IsKnownEnvironment(uri.Host))
		{
			return null;
		}
		string[] array = uri.AbsolutePath.Split(new char[1] { '/' });
		if (array.Length >= 2)
		{
			int num = ((!(array[1] == "tfp")) ? 1 : 2);
			if (num < array.Length)
			{
				array[num] = "<tenant>";
			}
		}
		string text = string.Join("/", array);
		return uri.Scheme + "://" + uri.Authority + text;
	}

	public string HashPersonalIdentifier(ICryptographyManager cryptographyManager, string valueToHash)
	{
		return cryptographyManager.CreateBase64UrlEncodedSha256Hash(valueToHash);
	}
}

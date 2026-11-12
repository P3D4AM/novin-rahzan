using System;

namespace Microsoft.Identity.Client.Cache.Keys;

internal class MsalAppMetadataCacheKey : IiOSKey
{
	private readonly string _clientId;

	private readonly string _environment;

	public string iOSService => ("AppMetadata-" + _clientId).ToLowerInvariant();

	public string iOSGeneric => "1";

	public string iOSAccount => (_environment ?? "").ToLowerInvariant();

	public int iOSType => 3001;

	public MsalAppMetadataCacheKey(string clientId, string environment)
	{
		_clientId = clientId ?? throw new ArgumentNullException("clientId");
		_environment = environment ?? throw new ArgumentNullException("environment");
	}

	public override string ToString()
	{
		return ("appmetadata-" + _environment + "-" + _clientId).ToLowerInvariant();
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		MsalAppMetadataCacheKey msalAppMetadataCacheKey = obj as MsalAppMetadataCacheKey;
		return string.Equals(ToString(), msalAppMetadataCacheKey.ToString(), StringComparison.OrdinalIgnoreCase);
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}
}

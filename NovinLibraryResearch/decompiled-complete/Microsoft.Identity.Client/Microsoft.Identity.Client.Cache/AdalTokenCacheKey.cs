using System;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.Identity.Client.Cache;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
internal sealed class AdalTokenCacheKey : IEquatable<AdalTokenCacheKey>
{
	public string Authority { get; }

	public string Resource { get; }

	public string ClientId { get; }

	public string UniqueId { get; }

	public string DisplayableId { get; }

	public TokenSubjectType TokenSubjectType { get; }

	private string DebuggerDisplay => string.Format(CultureInfo.InvariantCulture, "AdalTokenCacheKey: {0} {1} {2} {3} {4}", Authority, Resource, ClientId, UniqueId, DisplayableId);

	internal AdalTokenCacheKey(string authority, string resource, string clientId, TokenSubjectType tokenSubjectType, AdalUserInfo adalUserInfo)
		: this(authority, resource, clientId, tokenSubjectType, adalUserInfo?.UniqueId, adalUserInfo?.DisplayableId)
	{
	}

	internal AdalTokenCacheKey(string authority, string resource, string clientId, TokenSubjectType tokenSubjectType, string uniqueId, string displayableId)
	{
		Authority = authority;
		Resource = resource;
		ClientId = clientId;
		TokenSubjectType = tokenSubjectType;
		UniqueId = uniqueId;
		DisplayableId = displayableId;
	}

	public override bool Equals(object obj)
	{
		if (obj is AdalTokenCacheKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public bool Equals(AdalTokenCacheKey other)
	{
		if (this == other)
		{
			return true;
		}
		if (other != null && other.Authority == Authority && ClientIdEquals(other.ClientId) && other.UniqueId == UniqueId && DisplayableIdEquals(other.DisplayableId))
		{
			return other.TokenSubjectType == TokenSubjectType;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (Authority + ":::" + ClientId + ":::" + UniqueId + ":::" + DisplayableId + ":::" + (int)TokenSubjectType).GetHashCode();
	}

	private bool ClientIdEquals(string otherClientId)
	{
		return string.Equals(otherClientId, ClientId, StringComparison.OrdinalIgnoreCase);
	}

	private bool DisplayableIdEquals(string otherDisplayableId)
	{
		return string.Equals(otherDisplayableId, DisplayableId, StringComparison.OrdinalIgnoreCase);
	}
}

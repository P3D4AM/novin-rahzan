using System;
using System.Diagnostics;

namespace Microsoft.Identity.Client;

public class AccountId
{
	public string Identifier { get; }

	public string ObjectId { get; }

	public string TenantId { get; }

	public AccountId(string identifier, string objectId, string tenantId)
	{
		Identifier = identifier ?? throw new ArgumentNullException("identifier");
		ObjectId = objectId;
		TenantId = tenantId;
	}

	public AccountId(string adfsIdentifier)
		: this(adfsIdentifier, adfsIdentifier, null)
	{
	}

	internal static AccountId ParseFromString(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}
		string[] array = str.Split(new char[1] { '.' });
		if (array.Length == 1)
		{
			return new AccountId(str);
		}
		return new AccountId(str, array[0], array[1]);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is AccountId accountId))
		{
			return false;
		}
		return string.Equals(Identifier, accountId.Identifier, StringComparison.OrdinalIgnoreCase);
	}

	public override int GetHashCode()
	{
		return Identifier.GetHashCode();
	}

	public override string ToString()
	{
		return "AccountId: " + Identifier;
	}

	[Conditional("DEBUG")]
	private void ValidateId()
	{
		if (!string.Equals((TenantId == null) ? ObjectId : (ObjectId + "." + TenantId), Identifier, StringComparison.Ordinal))
		{
			throw new InvalidOperationException("Internal Error (debug only) - Expecting Identifier = ObjectId.TenantId but have " + ToString());
		}
	}
}

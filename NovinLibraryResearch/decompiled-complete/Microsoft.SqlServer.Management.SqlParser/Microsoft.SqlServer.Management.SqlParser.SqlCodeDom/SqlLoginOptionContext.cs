using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlLoginOptionContext
{
	private readonly SqlLoginOptionContextItem[] items;

	internal static readonly SqlLoginOptionContext FromCertificateContext = new SqlLoginOptionContext(new SqlLoginOptionContextItem[1] { SqlLoginOptionContextItem.Credential });

	internal static readonly SqlLoginOptionContext FromWindowsContext = new SqlLoginOptionContext(new SqlLoginOptionContextItem[2]
	{
		SqlLoginOptionContextItem.DefaultDatabase,
		SqlLoginOptionContextItem.DefaultLanguage
	});

	internal static readonly SqlLoginOptionContext WithPasswordContext = new SqlLoginOptionContext(new SqlLoginOptionContextItem[6]
	{
		SqlLoginOptionContextItem.Sid,
		SqlLoginOptionContextItem.DefaultDatabase,
		SqlLoginOptionContextItem.DefaultLanguage,
		SqlLoginOptionContextItem.Credential,
		SqlLoginOptionContextItem.CheckExpiration,
		SqlLoginOptionContextItem.CheckPolicy
	});

	internal static readonly SqlLoginOptionContext PasswordContext = new SqlLoginOptionContext(new SqlLoginOptionContextItem[2]
	{
		SqlLoginOptionContextItem.Hashed,
		SqlLoginOptionContextItem.MustChange
	});

	internal static readonly SqlLoginOptionContext AlterOptionsContext = new SqlLoginOptionContext(new SqlLoginOptionContextItem[6]
	{
		SqlLoginOptionContextItem.DefaultDatabase,
		SqlLoginOptionContextItem.DefaultLanguage,
		SqlLoginOptionContextItem.Name,
		SqlLoginOptionContextItem.CheckExpiration,
		SqlLoginOptionContextItem.CheckPolicy,
		SqlLoginOptionContextItem.Credential
	});

	private SqlLoginOptionContext(SqlLoginOptionContextItem[] items)
	{
		this.items = items;
	}

	public static string GetOptionString(SqlLoginOptionType type)
	{
		return type switch
		{
			SqlLoginOptionType.Hashed => "HASHED", 
			SqlLoginOptionType.MustChange => "MUST_CHANGE", 
			SqlLoginOptionType.Sid => "SID", 
			SqlLoginOptionType.DefaultDatabase => "DEFAULT_DATABASE", 
			SqlLoginOptionType.DefaultLanguage => "DEFAULT_LANGUAGE", 
			SqlLoginOptionType.Credential => "CREDENTIAL", 
			SqlLoginOptionType.CheckExpiration => "CHECK_EXPIRATION", 
			SqlLoginOptionType.CheckPolicy => "CHECK_POLICY", 
			SqlLoginOptionType.Name => "NAME", 
			SqlLoginOptionType.NoCredential => "NO CREDENTIAL", 
			_ => null, 
		};
	}

	public StringToken GetOptionStringToken(SqlLoginOptionType type)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].type.Equals(type))
			{
				return items[i].stringToken;
			}
		}
		return StringToken.None;
	}

	public SqlLoginOptionType GetOptionType(StringToken stringToken)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].stringToken.Equals(stringToken))
			{
				return items[i].type;
			}
		}
		return SqlLoginOptionType.Invalid;
	}

	public SqlLoginOptionType GetOptionType(string stringValue)
	{
		StringToken stringToken = StringTokenLookup.Find(stringValue);
		if (stringToken != 0)
		{
			return GetOptionType(stringToken);
		}
		return SqlLoginOptionType.Invalid;
	}
}

using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlLoginOptionContextItem
{
	public readonly StringToken stringToken;

	public readonly SqlLoginOptionType type;

	public static readonly SqlLoginOptionContextItem Hashed = new SqlLoginOptionContextItem(SqlLoginOptionType.Hashed);

	public static readonly SqlLoginOptionContextItem MustChange = new SqlLoginOptionContextItem(SqlLoginOptionType.MustChange);

	public static readonly SqlLoginOptionContextItem Sid = new SqlLoginOptionContextItem(SqlLoginOptionType.Sid);

	public static readonly SqlLoginOptionContextItem DefaultDatabase = new SqlLoginOptionContextItem(SqlLoginOptionType.DefaultDatabase);

	public static readonly SqlLoginOptionContextItem DefaultLanguage = new SqlLoginOptionContextItem(SqlLoginOptionType.DefaultLanguage);

	public static readonly SqlLoginOptionContextItem Credential = new SqlLoginOptionContextItem(SqlLoginOptionType.Credential);

	public static readonly SqlLoginOptionContextItem CheckExpiration = new SqlLoginOptionContextItem(SqlLoginOptionType.CheckExpiration);

	public static readonly SqlLoginOptionContextItem CheckPolicy = new SqlLoginOptionContextItem(SqlLoginOptionType.CheckPolicy);

	public static readonly SqlLoginOptionContextItem Name = new SqlLoginOptionContextItem(SqlLoginOptionType.Name);

	public SqlLoginOptionContextItem(SqlLoginOptionType type)
	{
		this.type = type;
		stringToken = StringTokenLookup.Find(SqlLoginOptionContext.GetOptionString(type));
	}
}

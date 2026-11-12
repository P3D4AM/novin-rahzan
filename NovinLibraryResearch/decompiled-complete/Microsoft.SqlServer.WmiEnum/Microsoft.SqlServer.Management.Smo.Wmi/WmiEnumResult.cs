using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class WmiEnumResult : EnumResult
{
	private string m_scope;

	private string m_whereClause;

	public string Scope
	{
		get
		{
			return m_scope;
		}
		set
		{
			m_scope = value;
		}
	}

	public string WhereClause
	{
		get
		{
			return m_whereClause;
		}
		set
		{
			m_whereClause = value;
		}
	}

	public WmiEnumResult(string scope)
		: base(null, ResultType.Reserved1)
	{
		Scope = scope;
	}
}

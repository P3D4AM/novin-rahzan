using System.Data;
using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal abstract class WmiChildObjectBase : WmiBase
{
	private DataTable m_dataParent;

	protected abstract string[] GetParentFields();

	protected abstract string GetWhereClauseTemplate();

	public WmiChildObjectBase(string className)
		: base(className)
	{
	}

	public override Request RetrieveParentRequest()
	{
		return new WmiRequest
		{
			Fields = GetParentFields()
		};
	}

	protected override void BuildStatementBuilder()
	{
		base.BuildStatementBuilder();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataRow row in m_dataParent.Rows)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" OR ");
			}
			stringBuilder.AppendFormat(GetWhereClauseTemplate(), row.ItemArray);
		}
		m_dataParent = null;
		base.StatementBuilder.AddWhere(stringBuilder.ToString());
	}

	public override EnumResult GetData(EnumResult erParent)
	{
		m_dataParent = erParent;
		return base.GetData(erParent);
	}
}

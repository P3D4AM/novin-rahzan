using System.Globalization;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

internal class ServerTargetProvider : ITargetProvider
{
	private Target target;

	private ServerConnection ServerConnection => (ServerConnection)target.Parent.Parent.SfcConnection;

	public ServerTargetProvider(Target parent)
	{
		target = parent;
	}

	public string GetCreateScript()
	{
		StringBuilder stringBuilder = new StringBuilder(128);
		stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "ADD TARGET {0}", target.ScriptName));
		if (target.HasCustomizableField())
		{
			stringBuilder.Append("(");
			stringBuilder.Append("SET ");
			foreach (TargetField targetField in target.TargetFields)
			{
				if (targetField.Value != null)
				{
					BaseXEStore parent = target.Parent.Parent;
					TargetColumnInfo targetColumnInfo = parent.ObjectInfoSet.Get<TargetInfo>(target.Name).TargetColumnInfoSet[targetField.Name];
					stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0}={1},", targetField.Name, parent.FormatFieldValue(targetField.Value.ToString(), targetColumnInfo.TypePackageID, targetColumnInfo.TypeName)));
				}
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append(")");
		}
		return stringBuilder.ToString();
	}

	public string GetDropScript()
	{
		return string.Format(CultureInfo.InvariantCulture, "DROP TARGET {0}", target.ScriptName);
	}

	public string GetTargetData()
	{
		Session parent = target.Parent;
		if (parent == null)
		{
			throw new XEventException(ExceptionTemplates.ParentNull);
		}
		string value = target.Name.Substring(target.Name.LastIndexOf('.') + 1);
		string sqlCommand = string.Format(CultureInfo.InvariantCulture, "SELECT target_data FROM sys.dm_xe_session_targets AS t JOIN sys.dm_xe_sessions AS s ON t.event_session_address=s.address WHERE s.name='{0}' AND t.target_name='{1}'", SfcTsqlProcFormatter.SqlString(parent.Name), SfcTsqlProcFormatter.SqlString(value));
		return (ServerConnection.ExecuteScalar(sqlCommand) ?? throw new XEventException(ExceptionTemplates.CannotReadTargetData)) as string;
	}
}

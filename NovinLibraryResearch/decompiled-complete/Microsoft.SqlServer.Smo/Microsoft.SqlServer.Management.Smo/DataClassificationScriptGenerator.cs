using System;
using System.Text;

namespace Microsoft.SqlServer.Management.Smo;

internal abstract class DataClassificationScriptGenerator
{
	internal sealed class V7 : DataClassificationScriptGenerator
	{
		internal V7(Column column, ScriptingPreferences sp)
			: base(column, sp)
		{
		}

		internal override string Add()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Table table = Column.ParentColl.ParentInstance as Table;
			Tuple<string, string>[] array = new Tuple<string, string>[4]
			{
				new Tuple<string, string>("sys_sensitivity_label_name", (string)Column.Properties.Get("SensitivityLabelName").Value),
				new Tuple<string, string>("sys_sensitivity_label_id", (string)Column.Properties.Get("SensitivityLabelId").Value),
				new Tuple<string, string>("sys_information_type_name", (string)Column.Properties.Get("SensitivityInformationTypeName").Value),
				new Tuple<string, string>("sys_information_type_id", (string)Column.Properties.Get("SensitivityInformationTypeId").Value)
			};
			foreach (Tuple<string, string> tuple in array)
			{
				if (!string.IsNullOrEmpty(tuple.Item2))
				{
					stringBuilder.Append("exec sp_addextendedproperty @level0type = N'schema', @level0name = N'" + SqlSmoObject.SqlString(table.Schema) + "', @level1type = N'table', @level1name = N'" + SqlSmoObject.SqlString(table.Name) + "', @level2type = N'column', @level2name = N'" + SqlSmoObject.SqlString(Column.Name) + "', @name = N'" + SqlSmoObject.SqlString(tuple.Item1) + "', @value = N'" + SqlSmoObject.SqlString(tuple.Item2) + "'");
					stringBuilder.Append(ScriptingPreferences.NewLine);
				}
			}
			return stringBuilder.ToString();
		}

		internal override string Update()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Table table = Column.ParentColl.ParentInstance as Table;
			Tuple<string, Property>[] array = new Tuple<string, Property>[4]
			{
				new Tuple<string, Property>("sys_sensitivity_label_name", Column.Properties.Get("SensitivityLabelName")),
				new Tuple<string, Property>("sys_sensitivity_label_id", Column.Properties.Get("SensitivityLabelId")),
				new Tuple<string, Property>("sys_information_type_name", Column.Properties.Get("SensitivityInformationTypeName")),
				new Tuple<string, Property>("sys_information_type_id", Column.Properties.Get("SensitivityInformationTypeId"))
			};
			foreach (Tuple<string, Property> tuple in array)
			{
				if (tuple.Item2.Dirty)
				{
					string text = (string)tuple.Item2.Value;
					if (string.IsNullOrEmpty(text))
					{
						stringBuilder.Append(("if exists(\r\nSELECT s.name AS schema_name\r\nFROM sys.columns c\r\n\tLEFT JOIN sys.tables t ON t.object_id = c.object_id\r\n\tLEFT JOIN sys.schemas s ON s.schema_id = t.schema_id\r\n\tLEFT JOIN sys.extended_properties EP ON c.object_id = EP.major_id AND c.column_id = EP.minor_id and EP.name = N'" + SqlSmoObject.SqlString(tuple.Item1) + "'\r\n\tWHERE s.name = N'" + SqlSmoObject.SqlString(table.Schema) + "' AND t.name = N'" + SqlSmoObject.SqlString(table.Name) + "' AND c.name = N'" + SqlSmoObject.SqlString(Column.Name) + "' AND EP.value IS NOT NULL\r\n)\r\n  exec sp_dropextendedproperty @name = N'" + SqlSmoObject.SqlString(tuple.Item1) + "', @level0type = N'schema', @level0name = N'" + SqlSmoObject.SqlString(table.Schema) + "', @level1type = N'table', @level1name = N'" + SqlSmoObject.SqlString(table.Name) + "', @level2type = N'column', @level2name = N'" + SqlSmoObject.SqlString(Column.Name) + "'").FixNewLines());
					}
					else
					{
						stringBuilder.Append(("if exists(\r\nSELECT s.name AS schema_name\r\nFROM sys.columns c\r\n\tLEFT JOIN sys.tables t ON t.object_id = c.object_id\r\n\tLEFT JOIN sys.schemas s ON s.schema_id = t.schema_id\r\n\tLEFT JOIN sys.extended_properties EP ON c.object_id = EP.major_id AND c.column_id = EP.minor_id and EP.name = N'" + SqlSmoObject.SqlString(tuple.Item1) + "'\r\n\tWHERE s.name = N'" + SqlSmoObject.SqlString(table.Schema) + "' AND t.name = N'" + SqlSmoObject.SqlString(table.Name) + "' AND c.name = N'" + SqlSmoObject.SqlString(Column.Name) + "' AND EP.value IS NOT NULL\r\n)\r\n  exec sp_dropextendedproperty @name = N'" + SqlSmoObject.SqlString(tuple.Item1) + "', @level0type = N'schema', @level0name = N'" + SqlSmoObject.SqlString(table.Schema) + "', @level1type = N'table', @level1name = N'" + SqlSmoObject.SqlString(table.Name) + "', @level2type = N'column', @level2name = N'" + SqlSmoObject.SqlString(Column.Name) + "'\r\nexec sp_addextendedproperty @level0type = N'schema', @level0name = N'" + SqlSmoObject.SqlString(table.Schema) + "', @level1type = N'table', @level1name = N'" + SqlSmoObject.SqlString(table.Name) + "', @level2type = N'column', @level2name = N'" + SqlSmoObject.SqlString(Column.Name) + "', @name = N'" + SqlSmoObject.SqlString(tuple.Item1) + "', @value = N'" + SqlSmoObject.SqlString(text) + "'").FixNewLines());
					}
					stringBuilder.Append(ScriptingPreferences.NewLine);
				}
			}
			return stringBuilder.ToString();
		}

		internal override string Drop()
		{
			return Update();
		}
	}

	internal sealed class V15 : DataClassificationScriptGenerator
	{
		internal V15(Column column, ScriptingPreferences sp)
			: base(column, sp)
		{
		}

		internal override string Add()
		{
			ScriptStringBuilder scriptStringBuilder = new ScriptStringBuilder("ADD SENSITIVITY CLASSIFICATION TO " + Column.ParentColl.ParentInstance.FullQualifiedName + "." + Column.FullQualifiedName + " WITH");
			Tuple<string, Property>[] array = new Tuple<string, Property>[5]
			{
				new Tuple<string, Property>("label", Column.Properties.Get("SensitivityLabelName")),
				new Tuple<string, Property>("label_id", Column.Properties.Get("SensitivityLabelId")),
				new Tuple<string, Property>("information_type", Column.Properties.Get("SensitivityInformationTypeName")),
				new Tuple<string, Property>("information_type_id", Column.Properties.Get("SensitivityInformationTypeId")),
				new Tuple<string, Property>("rank", Column.IsSupportedProperty("SensitivityRank") ? Column.Properties.Get("SensitivityRank") : null)
			};
			foreach (Tuple<string, Property> tuple in array)
			{
				Property item = tuple.Item2;
				if (item != null)
				{
					string value = item.Value as string;
					SensitivityRank? sensitivityRank = item.Value as SensitivityRank?;
					if (!string.IsNullOrEmpty(value))
					{
						scriptStringBuilder.SetParameter(tuple.Item1, Util.EscapeString(value, '\''));
					}
					else if (sensitivityRank.HasValue && sensitivityRank.Value != SensitivityRank.Undefined)
					{
						scriptStringBuilder.SetParameter(tuple.Item1, sensitivityRank.Value.ToString(), ParameterValueFormat.NotString);
					}
				}
			}
			return scriptStringBuilder.ToString();
		}

		internal override string Update()
		{
			return Add();
		}

		internal override string Drop()
		{
			return "DROP SENSITIVITY CLASSIFICATION FROM " + Column.ParentColl.ParentInstance.FullQualifiedName + "." + Column.FullQualifiedName;
		}
	}

	protected Column Column;

	protected ScriptingPreferences ScriptingPreferences;

	internal static DataClassificationScriptGenerator Create(Column column, ScriptingPreferences sp)
	{
		if (!((sp == null) ? VersionUtils.IsSql15Azure12OrLater(column.DatabaseEngineType, column.ServerVersion) : VersionUtils.IsTargetVersionSql15Azure12OrLater(sp.TargetDatabaseEngineType, sp.TargetServerVersionInternal)))
		{
			return new V7(column, sp);
		}
		return new V15(column, sp);
	}

	internal abstract string Add();

	internal abstract string Update();

	internal abstract string Drop();

	private DataClassificationScriptGenerator(Column column, ScriptingPreferences sp)
	{
		Column = column;
		ScriptingPreferences = sp;
	}
}

using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal static class BindUtils
{
	public static class Column
	{
		public enum LookupResult
		{
			None,
			Ambiguous,
			EmptyName
		}

		public static bool BindColumnIdentitySpec(string columnName, IScalarDataType scalarType, SqlLiteralExpression seed, SqlLiteralExpression increment, SqlCodeObject reportErrorObject)
		{
			ISystemDataType systemDataType = scalarType?.BaseSystemDataType;
			bool flag = true;
			bool flag2 = true;
			bool flag3;
			if (systemDataType != null)
			{
				flag3 = true;
				bool flag4 = seed != null;
				switch (systemDataType.TypeSpec.SqlDataType)
				{
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.TinyInt:
					if (flag4)
					{
						flag = IsValidInt(seed, 0L, 255L, zeroAllowed: true);
						flag2 = IsValidInt(increment, 0L, 255L, zeroAllowed: false);
					}
					break;
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.SmallInt:
					if (flag4)
					{
						flag = IsValidInt(seed, -32768L, 32767L, zeroAllowed: true);
						flag2 = IsValidInt(increment, -32768L, 32767L, zeroAllowed: false);
					}
					break;
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Int:
					if (flag4)
					{
						flag = IsValidInt(seed, -2147483648L, 2147483647L, zeroAllowed: true);
						flag2 = IsValidInt(increment, -2147483648L, 2147483647L, zeroAllowed: false);
					}
					break;
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.BigInt:
					if (flag4)
					{
						flag = IsValidInt(seed, long.MinValue, long.MaxValue, zeroAllowed: true);
						flag2 = IsValidInt(increment, long.MinValue, long.MaxValue, zeroAllowed: false);
					}
					break;
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Decimal:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Numeric:
					if (systemDataType.NumericScale != 0)
					{
						flag3 = false;
					}
					else if (flag4)
					{
						int numericPrecision = systemDataType.NumericPrecision;
						flag = IsValidNumeric(seed, numericPrecision, zeroAllowed: true);
						flag2 = IsValidNumeric(increment, numericPrecision, zeroAllowed: false);
					}
					break;
				default:
					flag3 = false;
					break;
				}
			}
			else
			{
				flag3 = false;
			}
			if (!flag3)
			{
				reportErrorObject.ReportBindError(SR.IdentityColumnInvalidType(columnName));
			}
			else
			{
				if (!flag)
				{
					seed.ReportBindError(SR.InvalidSeed(columnName));
				}
				if (!flag2)
				{
					increment.ReportBindError(SR.InvalidIncrement(columnName));
				}
			}
			return flag3;
		}

		private static bool IsValidInt(SqlLiteralExpression expr, long min, long max, bool zeroAllowed)
		{
			long? num = expr.AsInt64();
			if (num.HasValue && num.Value >= min && num.Value <= max)
			{
				if (!zeroAllowed)
				{
					return num.Value != 0;
				}
				return true;
			}
			return false;
		}

		private static bool IsValidNumeric(SqlLiteralExpression expr, int precision, bool zeroAllowed)
		{
			if (expr.HasDecimalPoint())
			{
				return false;
			}
			if (expr.EffectiveNumberOfDigits() > precision)
			{
				return false;
			}
			if (!zeroAllowed)
			{
				int? num = expr.AsInt32();
				if (num.HasValue && num.Value == 0)
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsIdentity(IColumn column)
		{
			return column.IdentityColumnInfo != null;
		}

		public static bool IsComputed(IColumn column)
		{
			return column.ComputedColumnInfo != null;
		}

		public static LookupResult FindColumn(IBindingContext bindingContext, SqlIdentifier columnName, out IColumn boundColumn)
		{
			LookupResult result = LookupResult.None;
			boundColumn = null;
			if (columnName.IsEmpty)
			{
				result = LookupResult.EmptyName;
			}
			else
			{
				boundColumn = bindingContext.FindColumn(columnName.Value, out var isAmbiguous);
				if (isAmbiguous)
				{
					result = LookupResult.Ambiguous;
				}
			}
			return result;
		}

		public static LookupResult FindColumn(IBindingContext bindingContext, SqlMultipartIdentifier objectIdentifier, int columnIndex, out ITabular boundTable, out IColumn boundColumn)
		{
			LookupResult result = LookupResult.None;
			boundTable = null;
			boundColumn = null;
			MultipartObjectName multipartObjectName = new MultipartObjectName(objectIdentifier, columnIndex);
			string databaseName = multipartObjectName.DatabaseName;
			string schemaName = multipartObjectName.SchemaName;
			string objectName = multipartObjectName.ObjectName;
			if (objectName.Length == 0)
			{
				result = LookupResult.EmptyName;
			}
			else
			{
				boundTable = bindingContext.FindLocalTableExpression(databaseName, schemaName, objectName);
				SqlIdentifier sqlIdentifier = objectIdentifier[columnIndex];
				if (sqlIdentifier.IsEmpty)
				{
					result = LookupResult.EmptyName;
				}
				else if (boundTable != null)
				{
					boundColumn = boundTable.Columns[sqlIdentifier.Value];
				}
			}
			return result;
		}

		public static LookupResult LookupColumnPrefix(IBindingContext bindingContext, SqlMultipartIdentifier multipartIdentifier, out int columnIndex, out ITabular boundTable, out IColumn boundColumn)
		{
			bool flag = false;
			LookupResult lookupResult = LookupResult.None;
			int count = multipartIdentifier.Count;
			columnIndex = -1;
			boundColumn = null;
			boundTable = null;
			if (count >= 2 && count <= 4)
			{
				columnIndex = count - 1;
				lookupResult = FindColumn(bindingContext, multipartIdentifier, columnIndex, out boundTable, out boundColumn);
				flag = boundColumn != null;
			}
			else
			{
				columnIndex = 1;
			}
			if (!flag)
			{
				LookupResult lookupResult2 = FindColumn(bindingContext, multipartIdentifier[0], out boundColumn);
				if (boundColumn != null)
				{
					if (IsUdtColumn(boundColumn))
					{
						columnIndex = 0;
						lookupResult = LookupResult.None;
						boundTable = null;
						flag = true;
					}
					else
					{
						boundColumn = null;
					}
				}
				else if (lookupResult2 == LookupResult.Ambiguous)
				{
					foreach (IColumn item in bindingContext.FindAllColumns(multipartIdentifier[0].Value))
					{
						if (IsUdtColumn(item))
						{
							columnIndex = 0;
							lookupResult = LookupResult.Ambiguous;
							flag = true;
						}
					}
				}
			}
			if (!flag && count > 2)
			{
				ITabular tabular = boundTable;
				LookupResult lookupResult3 = lookupResult;
				lookupResult = FindColumn(bindingContext, multipartIdentifier, 1, out boundTable, out boundColumn);
				if (boundColumn == null)
				{
					if (tabular == null && boundTable != null)
					{
						columnIndex = 1;
					}
					else
					{
						boundTable = tabular;
						lookupResult = lookupResult3;
					}
				}
				else
				{
					columnIndex = 1;
				}
			}
			return lookupResult;
		}

		private static bool IsUdtColumn(IColumn column)
		{
			IScalarDataType scalarDataType = DataType.AsScalarDataType(column.DataType);
			if (!scalarDataType.IsClr)
			{
				return scalarDataType.IsXml;
			}
			return true;
		}
	}

	public static class DataType
	{
		public static bool IsNullOrUnknown(IDataType dataType)
		{
			return dataType?.IsUnknown ?? true;
		}

		public static bool IsUniqueIdentifier(IScalarDataType dataType)
		{
			return IsOfSqlType(dataType, Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.UniqueIdentifier);
		}

		public static bool IsOfSqlType(IScalarDataType dataType, Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType sqlDataType)
		{
			ISystemDataType baseSystemDataType = dataType.BaseSystemDataType;
			if (baseSystemDataType != null)
			{
				return baseSystemDataType.TypeSpec.SqlDataType == sqlDataType;
			}
			return false;
		}

		public static bool IsValidForCollate(IScalarDataType dataType)
		{
			ISystemDataType systemDataType = AsSystemDataType(dataType);
			if (systemDataType != null)
			{
				switch (systemDataType.TypeSpec.SqlDataType)
				{
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Char:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.NChar:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.NText:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.NVarChar:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.NVarCharMax:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.SysName:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Text:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.VarChar:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.VarCharMax:
					return true;
				default:
					return false;
				}
			}
			return false;
		}

		public static IScalarDataType AsScalarDataType(IDataType dataType)
		{
			return dataType as IScalarDataType;
		}

		public static ITableDataType AsTableDataType(IDataType dataType)
		{
			return dataType as ITableDataType;
		}

		public static IUserDefinedType AsUserDefinedType(IDataType dataType)
		{
			return dataType as IUserDefinedType;
		}

		public static bool IsUserDefinedType(IDataType dataType)
		{
			return dataType is IUserDefinedType;
		}

		public static ISystemDataType AsSystemDataType(IDataType dataType)
		{
			return dataType as ISystemDataType;
		}

		public static bool isSystemDataType(IDataType dataType)
		{
			return dataType is ISystemDataType;
		}
	}

	public static class Function
	{
		public static BindingState BindArguments(SqlUserDefinedScalarFunctionCallExpression expression, IBindingContext bindingContext, IFunction boundFunction)
		{
			return BindArguments(expression, expression.ObjectIdentifier, expression.Arguments, bindingContext, boundFunction);
		}

		public static BindingState BindArguments(SqlTableValuedFunctionRefExpression expression, IBindingContext bindingContext, IFunction boundFunction)
		{
			return BindArguments(expression, expression.ObjectIdentifier, expression.Arguments, bindingContext, boundFunction);
		}

		public static BindingState BindArguments(SqlUdtMemberExpression expression, SqlScalarExpressionCollection arguments, IBindingContext bindingContext, IFunction boundFunction)
		{
			return BindArguments(expression, expression.MemberName, arguments, bindingContext, boundFunction);
		}

		public static BindingState BindArguments(SqlTableUdtMemberExpression expression, SqlScalarExpressionCollection arguments, IBindingContext bindingContext, IFunction boundFunction)
		{
			return BindArguments(expression, expression.MemberName, arguments, bindingContext, boundFunction);
		}

		private static BindingState BindArguments(SqlCodeObject codeObject, SqlCodeObject functionIdentifier, SqlScalarExpressionCollection arguments, IBindingContext bindingContext, IFunction boundFunction)
		{
			BindingState bindingState = BindingState.Unbound;
			int num = 0;
			int count = boundFunction.Parameters.Count;
			if (arguments != null)
			{
				for (int i = 0; i < arguments.Count; i++)
				{
					if (i < count)
					{
						IParameter parameter = boundFunction.Parameters[i];
						SqlScalarExpression sqlScalarExpression = arguments[i];
						Module.BindArgument(sqlScalarExpression, parameter, bindingContext);
						bindingState |= sqlScalarExpression.BindingState;
					}
				}
				num = arguments.Count;
			}
			if (num != count)
			{
				bindingState |= BindingState.BindFailed;
				string message = ((num >= count) ? SR.TooManyArguments(functionIdentifier.ToString()) : SR.InsufficientArguments(functionIdentifier.ToString()));
				codeObject.ReportBindError(message);
			}
			return bindingState;
		}
	}

	public static class Index
	{
		public static IRelationalIndex AsRelationalIndex(IIndex index)
		{
			return index as IRelationalIndex;
		}
	}

	public static class ScalarDataType
	{
		public static IClrDataType AsClrDataType(IScalarDataType scalarDataType)
		{
			return scalarDataType as IClrDataType;
		}

		public static IXmlDataType AsXmlDataType(IScalarDataType scalarDataType)
		{
			return scalarDataType as IXmlDataType;
		}
	}

	public static class Scalar
	{
		public static IColumn AsColumn(IScalar scalar)
		{
			return scalar as IColumn;
		}
	}

	public static class ScalarFunction
	{
		public static IUserDefinedFunction AsUserDefinedFunction(IScalarFunction function)
		{
			return function as IUserDefinedFunction;
		}

		public static bool IsSchemaBound(IScalarFunction function)
		{
			return AsUserDefinedFunction(function)?.IsSchemaBound ?? false;
		}
	}

	public static class Module
	{
		public static bool IsExtendedStoredProcedure(ICallableModule module)
		{
			return module.ModuleType == CallableModuleType.ExtendedStoredProcedure;
		}

		public static bool IsScalarFunction(ICallableModule module)
		{
			return module.ModuleType == CallableModuleType.ScalarFunction;
		}

		public static bool IsStoredProcedure(ICallableModule module)
		{
			return module.ModuleType == CallableModuleType.StoredProcedure;
		}

		public static void BindArgument(SqlScalarExpression value, IParameter parameter, IBindingContext bindingContext)
		{
			if (!parameter.DataType.IsScalar)
			{
				IDataType dataType = null;
				if (value.IsVariableReference)
				{
					SqlScalarVariableRefExpression sqlScalarVariableRefExpression = value as SqlScalarVariableRefExpression;
					sqlScalarVariableRefExpression.Bind(bindingContext, allowTableVariables: true);
					if (sqlScalarVariableRefExpression.BoundVariable != null)
					{
						dataType = sqlScalarVariableRefExpression.BoundVariable.DataType;
					}
				}
				else
				{
					value.Bind(bindingContext);
					if (value.BoundScalar != null)
					{
						dataType = value.BoundScalar.DataType;
					}
				}
				if (!DataType.IsNullOrUnknown(dataType) && dataType != parameter.DataType)
				{
					value.ReportBindError(SR.OperandTypeClash(dataType.Name, parameter.DataType.Name));
				}
			}
			else
			{
				value.Bind(bindingContext);
			}
		}
	}

	public static class Table
	{
		public enum ColumnAliasOption
		{
			ColumnAlias,
			ScalarAlias
		}

		public static bool IsCte(ITabular tabular)
		{
			return tabular.TabularType == TabularType.CommonTableExpression;
		}

		public static bool IsDerivedTable(ITabular tabular)
		{
			return tabular.TabularType == TabularType.DerivedTable;
		}

		public static bool IsDmlInsertedDeleted(ITabular tabular)
		{
			return tabular.TabularType == TabularType.DmlInsertedDeleted;
		}

		public static bool IsDmlTableSource(ITabular tabular)
		{
			return tabular.TabularType == TabularType.DmlTableSource;
		}

		public static bool IsPivotTable(ITabular tabular)
		{
			return tabular.TabularType == TabularType.PivotTable;
		}

		public static bool IsTable(ITabular tabular)
		{
			return tabular.TabularType == TabularType.Table;
		}

		public static bool IsTableAlias(ITabular tabular)
		{
			return tabular.TabularType == TabularType.TableAlias;
		}

		public static bool IsTableValuedFunction(ITabular tabular)
		{
			return tabular.TabularType == TabularType.TableValuedFunction;
		}

		public static bool IsTableVariable(ITabular tabular)
		{
			return tabular.TabularType == TabularType.TableVariable;
		}

		public static bool IsTemporaryTable(ITabular tabular)
		{
			return tabular.TabularType == TabularType.TemporaryTable;
		}

		public static bool IsUnpivotTable(ITabular tabular)
		{
			return tabular.TabularType == TabularType.UnpivotTable;
		}

		public static bool IsView(ITabular tabular)
		{
			return tabular.TabularType == TabularType.View;
		}

		public static bool IsDatabaseTable(ITabular tabular)
		{
			TabularType tabularType = tabular.TabularType;
			if (tabularType == TabularType.Table || tabularType == TabularType.TableValuedFunction || tabularType == TabularType.View)
			{
				return true;
			}
			return false;
		}

		public static bool IsTempTableName(string name)
		{
			if (name.Length <= 0)
			{
				return false;
			}
			return name[0] == '#';
		}

		public static ITable AsTable(ITableViewBase tableOrView)
		{
			return tableOrView as ITable;
		}

		public static IView AsView(ITableViewBase tableOrView)
		{
			return tableOrView as IView;
		}

		public static void PopulateVirtualTableColumns(SqlIdentifier tableName, SqlIdentifierCollection columnList, IMetadataOrderedCollection<IColumn> queryColumns, IMutableTabular table, IColumnFactory columnFactory, bool hasAnyBindSkipped)
		{
			PopulateTabularColumns(tableName, columnList, queryColumns, table, columnFactory, ColumnAliasOption.ColumnAlias, hasAnyBindSkipped);
		}

		public static void PopulateTabularColumns(SqlIdentifier tableName, SqlIdentifierCollection columnList, SqlQueryExpression queryExpression, BatchBindingContext bindingContext, IMutableTabular table, ColumnAliasOption aliasOption)
		{
			PopulateTabularColumns(tableName, columnList, queryExpression.LocalBindingContext.LocalColumnExpressions, table, bindingContext.MetadataFactory.Column, aliasOption, queryExpression.HasAnyBindSkipped);
		}

		private static void PopulateTabularColumns(SqlIdentifier tableName, SqlIdentifierCollection columnList, IMetadataOrderedCollection<IColumn> queryColumns, IMutableTabular table, IColumnFactory columnFactory, ColumnAliasOption aliasOption, bool hasAnyBindSkipped)
		{
			if (columnList != null)
			{
				if (!hasAnyBindSkipped)
				{
					if (queryColumns.Count > columnList.Count)
					{
						tableName.ReportBindError(SR.MoreColumns(tableName.Value));
					}
					else if (queryColumns.Count < columnList.Count)
					{
						tableName.ReportBindError(SR.FewerColumns(tableName.Value));
					}
				}
				for (int i = 0; i < columnList.Count; i++)
				{
					SqlIdentifier sqlIdentifier = columnList[i];
					if (!sqlIdentifier.IsEmpty)
					{
						if (table.Columns.Contains(sqlIdentifier.Value))
						{
							sqlIdentifier.ReportBindError(SR.ColumnSpecifiedMultipleTimes(sqlIdentifier.Value, tableName.Value));
							continue;
						}
						IColumn aliasedColumn = ((i < queryColumns.Count) ? queryColumns[i] : columnFactory.Null);
						IColumn item = CreateColumnAlias(table, columnFactory, aliasedColumn, sqlIdentifier.Value, aliasOption);
						table.Columns.Add(item);
					}
				}
				return;
			}
			for (int j = 0; j < queryColumns.Count; j++)
			{
				IColumn column = queryColumns[j];
				if (column != columnFactory.Null)
				{
					if (column.Name.Length == 0)
					{
						tableName.ReportBindError(SR.MissingColumn(j + 1, tableName.Value));
						continue;
					}
					if (table.Columns.Contains(column.Name))
					{
						tableName.ReportBindError(SR.ColumnSpecifiedMultipleTimes(column.Name, tableName.Value));
						continue;
					}
					IColumn item2 = CreateColumnAlias(table, columnFactory, column, null, aliasOption);
					table.Columns.Add(item2);
				}
			}
		}

		private static IColumn CreateColumnAlias(IMutableTabular table, IColumnFactory columnFactory, IColumn aliasedColumn, string columnName, ColumnAliasOption aliasOption)
		{
			if (columnName == null)
			{
				columnName = aliasedColumn.Name;
			}
			return aliasOption switch
			{
				ColumnAliasOption.ColumnAlias => columnFactory.CreateColumnAlias(table, aliasedColumn, columnName), 
				ColumnAliasOption.ScalarAlias => columnFactory.CreateScalarAlias(table, aliasedColumn, columnName), 
				_ => null, 
			};
		}
	}

	public static class Constraint
	{
		public static string GenerateCheckName(IDatabaseTable boundTable, string constraintText)
		{
			return GenerateConstraintName("CK", boundTable, constraintText);
		}

		public static string GenerateDefaultName(IDatabaseTable boundTable, string columnName)
		{
			return GenerateConstraintName("DF", boundTable, columnName);
		}

		public static string GenerateForeignKeyName(IDatabaseTable boundTable, List<string> columnNames)
		{
			return GenerateKeyName("FK", boundTable, columnNames);
		}

		public static string GeneratePrimaryKeyName(IDatabaseTable boundTable, List<string> columnNames)
		{
			return GenerateKeyName("PK", boundTable, columnNames);
		}

		public static string GenerateUniqueName(IDatabaseTable boundTable, List<string> columnNames)
		{
			return GenerateKeyName("UQ", boundTable, columnNames);
		}

		private static string GenerateKeyName(string prefix, IDatabaseTable boundTable, List<string> columnNames)
		{
			string content = string.Join(",", columnNames.ToArray());
			return GenerateConstraintName(prefix, boundTable, content);
		}

		private static string GenerateConstraintName(string prefix, IDatabaseTable boundTable, string content)
		{
			string text = null;
			string text2 = null;
			int num = 0;
			content = content + boundTable.Name + prefix;
			while (true)
			{
				text2 = GetHashValue(content + num);
				text = GenerateName(prefix, boundTable.Name, text2);
				if (!boundTable.Constraints.Contains(text))
				{
					break;
				}
				num++;
			}
			return text;
		}

		private static string GenerateName(string prefix, string tableName, string content)
		{
			int num = prefix.Length + content.Length + 4;
			tableName = ((num + tableName.Length < 128) ? tableName : tableName.Substring(0, 128 - num));
			return string.Format(CultureInfo.InvariantCulture, "{0}__{1}__{2}", prefix, tableName, content);
		}

		private static string GetHashValue(string content)
		{
			return GetHexFormatString(CoalesceBytes(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(content))));
		}

		private static byte[] CoalesceBytes(byte[] byteArray)
		{
			int num = byteArray.Length / 2;
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (byte)(byteArray[2 * i] ^ byteArray[2 * i + 1]);
			}
			return array;
		}

		private static string GetHexFormatString(byte[] byteArray)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < byteArray.Length; i++)
			{
				stringBuilder.Append(byteArray[i].ToString("x2", CultureInfo.InvariantCulture));
			}
			return stringBuilder.ToString();
		}
	}

	public static class DatabaseObject
	{
		private static readonly NameCollationVisitor nameCollationVisitor = new NameCollationVisitor();

		public static bool NameMatches(IDatabaseObject dbObject, string name)
		{
			return dbObject.Accept(nameCollationVisitor).EqualityComparer.Equals(dbObject.Name, name);
		}
	}

	public static class XmlDataType
	{
		public static bool IsValidMethodForXmlNode(IFunction method)
		{
			switch (method.Name)
			{
			case "query":
			case "exists":
			case "value":
			case "nodes":
				return true;
			default:
				return false;
			}
		}

		public static void PopulateTabularColumns(IBindingContext bindingContext, SqlIdentifier tableName, SqlIdentifierCollection columnList, IMutableTabular table)
		{
			IXmlDataType xmlNode = bindingContext.MetadataFactory.DataType.XmlNode;
			for (int i = 0; i < columnList.Count; i++)
			{
				SqlIdentifier sqlIdentifier = columnList[i];
				if (!sqlIdentifier.IsEmpty)
				{
					if (table.Columns.Contains(sqlIdentifier.Value))
					{
						sqlIdentifier.ReportBindError(SR.ColumnSpecifiedMultipleTimes(sqlIdentifier.Value, (tableName != null) ? tableName.Value : string.Empty));
						continue;
					}
					IColumn item = bindingContext.MetadataFactory.Column.CreateSimpleColumn(table, sqlIdentifier.Value, xmlNode, nullable: true);
					table.Columns.Add(item);
				}
			}
		}
	}

	public static ISchemaOwnedObject AsSchemaOwnedObject(IMetadataObject metadataObject)
	{
		return metadataObject as ISchemaOwnedObject;
	}
}

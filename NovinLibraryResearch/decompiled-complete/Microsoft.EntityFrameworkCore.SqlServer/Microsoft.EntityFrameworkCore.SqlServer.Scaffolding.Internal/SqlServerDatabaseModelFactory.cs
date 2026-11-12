using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;

public class SqlServerDatabaseModelFactory : DatabaseModelFactory
{
	private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

	private static readonly ISet<string> _dateTimePrecisionTypes = new HashSet<string> { "datetimeoffset", "datetime2", "time" };

	private static readonly ISet<string> _maxLengthRequiredTypes = new HashSet<string> { "binary", "varbinary", "char", "varchar", "nchar", "nvarchar" };

	private const string NamePartRegex = "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))";

	private static readonly Regex _partExtractor = new Regex(string.Format(CultureInfo.InvariantCulture, "^{0}(?:\\.{1})?$", string.Format(CultureInfo.InvariantCulture, "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))", 1), string.Format(CultureInfo.InvariantCulture, "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))", 2)), RegexOptions.Compiled, TimeSpan.FromMilliseconds(1000.0));

	private static readonly Dictionary<string, long[]> _defaultSequenceMinMax = new Dictionary<string, long[]>(StringComparer.OrdinalIgnoreCase)
	{
		{
			"tinyint",
			new long[2] { 0L, 255L }
		},
		{
			"smallint",
			new long[2] { -32768L, 32767L }
		},
		{
			"int",
			new long[2] { -2147483648L, 2147483647L }
		},
		{
			"bigint",
			new long[2] { -9223372036854775808L, 9223372036854775807L }
		}
	};

	public SqlServerDatabaseModelFactory([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logger, "logger");
		_logger = logger;
	}

	public override DatabaseModel Create(string connectionString, DatabaseModelFactoryOptions options)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(connectionString, "connectionString");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(options, "options");
		using SqlConnection connection = new SqlConnection(connectionString);
		return Create(connection, options);
	}

	public override DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(options, "options");
		DatabaseModel databaseModel = new DatabaseModel();
		bool flag = connection.State == ConnectionState.Open;
		if (!flag)
		{
			connection.Open();
		}
		try
		{
			databaseModel.DatabaseName = connection.Database;
			databaseModel.DefaultSchema = GetDefaultSchema(connection);
			IReadOnlyDictionary<string, (string, string)> typeAliases = GetTypeAliases(connection);
			List<string> list = options.Schemas.ToList();
			Func<string, string> schemaFilter = GenerateSchemaFilter(list);
			List<string> list2 = options.Tables.ToList();
			Func<string, string, string> tableFilter = GenerateTableFilter(list2.Select(Parse).ToList(), schemaFilter);
			if (SupportsSequences(connection))
			{
				foreach (DatabaseSequence sequence in GetSequences(connection, schemaFilter, typeAliases))
				{
					sequence.Database = databaseModel;
					databaseModel.Sequences.Add(sequence);
				}
			}
			foreach (DatabaseTable table in GetTables(connection, tableFilter, typeAliases))
			{
				table.Database = databaseModel;
				databaseModel.Tables.Add(table);
			}
			foreach (string item in list.Except(databaseModel.Sequences.Select((DatabaseSequence s) => s.Schema).Concat(databaseModel.Tables.Select((DatabaseTable t) => t.Schema))))
			{
				_logger.MissingSchemaWarning(item);
			}
			foreach (string item2 in list2)
			{
				var (parsedSchema, parsedTableName) = Parse(item2);
				if (!databaseModel.Tables.Any((DatabaseTable t) => (!string.IsNullOrEmpty(parsedSchema) && t.Schema == parsedSchema) || t.Name == parsedTableName))
				{
					_logger.MissingTableWarning(item2);
				}
			}
			return databaseModel;
		}
		finally
		{
			if (!flag)
			{
				connection.Close();
			}
		}
	}

	private string GetDefaultSchema(DbConnection connection)
	{
		using DbCommand dbCommand = connection.CreateCommand();
		dbCommand.CommandText = "SELECT SCHEMA_NAME()";
		if (dbCommand.ExecuteScalar() is string text)
		{
			_logger.DefaultSchemaFound(text);
			return text;
		}
		return null;
	}

	private static Func<string, string> GenerateSchemaFilter(IReadOnlyList<string> schemas)
	{
		if (schemas.Count <= 0)
		{
			return null;
		}
		return delegate(string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(s);
			stringBuilder.Append(" IN (");
			stringBuilder.Append(string.Join(", ", schemas.Select(EscapeLiteral)));
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		};
	}

	private static (string Schema, string Table) Parse(string table)
	{
		Match match = _partExtractor.Match(table.Trim());
		if (!match.Success)
		{
			throw new InvalidOperationException(SqlServerStrings.InvalidTableToIncludeInScaffolding(table));
		}
		string text = match.Groups["part1"].Value.Replace("]]", "]");
		string text2 = match.Groups["part2"].Value.Replace("]]", "]");
		if (!string.IsNullOrEmpty(text2))
		{
			return (Schema: text, Table: text2);
		}
		return (Schema: null, Table: text);
	}

	private static Func<string, string, string> GenerateTableFilter(IReadOnlyList<(string Schema, string Table)> tables, Func<string, string> schemaFilter)
	{
		if (schemaFilter == null && tables.Count <= 0)
		{
			return null;
		}
		return delegate(string s, string t)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			if (schemaFilter != null)
			{
				stringBuilder.Append("(").Append(schemaFilter(s));
				flag = true;
			}
			if (tables.Count > 0)
			{
				if (flag)
				{
					stringBuilder.AppendLine().Append("OR ");
				}
				else
				{
					stringBuilder.Append("(");
					flag = true;
				}
				List<(string, string)> list = tables.Where(((string Schema, string Table) e) => string.IsNullOrEmpty(e.Schema)).ToList();
				if (list.Count > 0)
				{
					stringBuilder.Append(t);
					stringBuilder.Append(" IN (");
					stringBuilder.Append(string.Join(", ", list.Select<(string, string), string>(((string Schema, string Table) e) => EscapeLiteral(e.Table))));
					stringBuilder.Append(")");
				}
				List<(string, string)> list2 = tables.Where(((string Schema, string Table) e) => !string.IsNullOrEmpty(e.Schema)).ToList();
				if (list2.Count > 0)
				{
					if (list.Count > 0)
					{
						stringBuilder.Append(" OR ");
					}
					stringBuilder.Append(t);
					stringBuilder.Append(" IN (");
					stringBuilder.Append(string.Join(", ", list2.Select<(string, string), string>(((string Schema, string Table) e) => EscapeLiteral(e.Table))));
					stringBuilder.Append(") AND (");
					stringBuilder.Append(s);
					stringBuilder.Append(" + N'.' + ");
					stringBuilder.Append(t);
					stringBuilder.Append(") IN (");
					stringBuilder.Append(string.Join(", ", list2.Select<(string, string), string>(((string Schema, string Table) e) => EscapeLiteral(e.Schema + "." + e.Table))));
					stringBuilder.Append(")");
				}
			}
			if (flag)
			{
				stringBuilder.Append(")");
			}
			return stringBuilder.ToString();
		};
	}

	private static string EscapeLiteral(string s)
	{
		return "N'" + s + "'";
	}

	private IReadOnlyDictionary<string, (string, string)> GetTypeAliases(DbConnection connection)
	{
		using DbCommand dbCommand = connection.CreateCommand();
		Dictionary<string, (string, string)> dictionary = new Dictionary<string, (string, string)>(StringComparer.OrdinalIgnoreCase);
		dbCommand.CommandText = "\r\nSELECT\r\n    SCHEMA_NAME([t].[schema_id]) AS [schema_name],\r\n    [t].[name] AS [type_name],\r\n    [t2].[name] AS [underlying_system_type],\r\n    CAST([t].[max_length] AS int) AS [max_length],\r\n    CAST([t].[precision] AS int) AS [precision],\r\n    CAST([t].[scale] AS int) AS [scale]\r\nFROM [sys].[types] AS [t]\r\nJOIN [sys].[types] AS [t2] ON [t].[system_type_id] = [t2].[user_type_id]\r\nWHERE [t].[is_user_defined] = 1 OR [t].[system_type_id] <> [t].[user_type_id]";
		using (DbDataReader dbDataReader = dbCommand.ExecuteReader())
		{
			while (dbDataReader.Read())
			{
				string valueOrDefault = dbDataReader.GetValueOrDefault<string>("schema_name");
				string valueOrDefault2 = dbDataReader.GetValueOrDefault<string>("type_name");
				string valueOrDefault3 = dbDataReader.GetValueOrDefault<string>("underlying_system_type");
				int valueOrDefault4 = dbDataReader.GetValueOrDefault<int>("max_length");
				int valueOrDefault5 = dbDataReader.GetValueOrDefault<int>("precision");
				int valueOrDefault6 = dbDataReader.GetValueOrDefault<int>("scale");
				string storeType = GetStoreType(valueOrDefault3, valueOrDefault4, valueOrDefault5, valueOrDefault6);
				_logger.TypeAliasFound(DisplayName(valueOrDefault, valueOrDefault2), storeType);
				dictionary.Add("[" + valueOrDefault + "].[" + valueOrDefault2 + "]", (storeType, valueOrDefault3));
			}
		}
		return dictionary;
	}

	private IEnumerable<DatabaseSequence> GetSequences(DbConnection connection, Func<string, string> schemaFilter, IReadOnlyDictionary<string, (string storeType, string)> typeAliases)
	{
		using DbCommand command = connection.CreateCommand();
		command.CommandText = "\r\nSELECT\r\n    OBJECT_SCHEMA_NAME([s].[object_id]) AS [schema_name],\r\n    [s].[name],\r\n    SCHEMA_NAME([t].[schema_id]) AS [type_schema],\r\n    TYPE_NAME([s].[user_type_id]) AS [type_name],\r\n    CAST([s].[precision] AS int) AS [precision],\r\n    CAST([s].[scale] AS int) AS [scale],\r\n    [s].[is_cycling],\r\n    CAST([s].[increment] AS int) AS [increment],\r\n    CAST([s].[start_value] AS bigint) AS [start_value],\r\n    CAST([s].[minimum_value] AS bigint) AS [minimum_value],\r\n    CAST([s].[maximum_value] AS bigint) AS [maximum_value]\r\nFROM [sys].[sequences] AS [s]\r\nJOIN [sys].[types] AS [t] ON [s].[user_type_id] = [t].[user_type_id]";
		if (schemaFilter != null)
		{
			command.CommandText = command.CommandText + "\r\nWHERE " + schemaFilter("OBJECT_SCHEMA_NAME([s].[object_id])");
		}
		using DbDataReader reader = command.ExecuteReader();
		while (reader.Read())
		{
			string valueOrDefault = reader.GetValueOrDefault<string>("schema_name");
			string valueOrDefault2 = reader.GetValueOrDefault<string>("name");
			string valueOrDefault3 = reader.GetValueOrDefault<string>("type_schema");
			string text = reader.GetValueOrDefault<string>("type_name");
			int valueOrDefault4 = reader.GetValueOrDefault<int>("precision");
			int valueOrDefault5 = reader.GetValueOrDefault<int>("scale");
			bool valueOrDefault6 = reader.GetValueOrDefault<bool>("is_cycling");
			int valueOrDefault7 = reader.GetValueOrDefault<int>("increment");
			long valueOrDefault8 = reader.GetValueOrDefault<long>("start_value");
			long valueOrDefault9 = reader.GetValueOrDefault<long>("minimum_value");
			long valueOrDefault10 = reader.GetValueOrDefault<long>("maximum_value");
			if (typeAliases.TryGetValue("[" + valueOrDefault3 + "].[" + text + "]", out (string, string) value))
			{
				(text, _) = value;
			}
			text = GetStoreType(text, 0, valueOrDefault4, valueOrDefault5);
			_logger.SequenceFound(DisplayName(valueOrDefault, valueOrDefault2), text, valueOrDefault6, valueOrDefault7, valueOrDefault8, valueOrDefault9, valueOrDefault10);
			DatabaseSequence databaseSequence = new DatabaseSequence
			{
				Schema = valueOrDefault,
				Name = valueOrDefault2,
				StoreType = text,
				IsCyclic = valueOrDefault6,
				IncrementBy = valueOrDefault7,
				StartValue = valueOrDefault8,
				MinValue = valueOrDefault9,
				MaxValue = valueOrDefault10
			};
			if (_defaultSequenceMinMax.ContainsKey(text))
			{
				long num = _defaultSequenceMinMax[text][0];
				databaseSequence.MinValue = ((databaseSequence.MinValue == num) ? null : databaseSequence.MinValue);
				databaseSequence.StartValue = ((databaseSequence.StartValue == num) ? null : databaseSequence.StartValue);
				databaseSequence.MaxValue = ((databaseSequence.MaxValue == _defaultSequenceMinMax[databaseSequence.StoreType][1]) ? null : databaseSequence.MaxValue);
			}
			yield return databaseSequence;
		}
	}

	private IEnumerable<DatabaseTable> GetTables(DbConnection connection, Func<string, string, string> tableFilter, IReadOnlyDictionary<string, (string, string)> typeAliases)
	{
		using DbCommand dbCommand = connection.CreateCommand();
		List<DatabaseTable> list = new List<DatabaseTable>();
		bool flag = SupportsMemoryOptimizedTable(connection);
		bool flag2 = SupportsTemporalTable(connection);
		string text = "\r\nSELECT\r\n    SCHEMA_NAME([t].[schema_id]) AS [schema],\r\n    [t].[name],\r\n    CAST([e].[value] AS nvarchar(MAX)) AS [comment],\r\n    'table' AS [type]";
		if (flag)
		{
			text += ",\r\n    [t].[is_memory_optimized]";
		}
		text += "\r\nFROM [sys].[tables] AS [t]\r\nLEFT JOIN [sys].[extended_properties] AS [e] ON [e].[major_id] = [t].[object_id] AND [e].[minor_id] = 0 AND [e].[class] = 1 AND [e].[name] = 'MS_Description'";
		string text2 = "[t].[is_ms_shipped] = 0\r\nAND NOT EXISTS (SELECT *\r\n    FROM [sys].[extended_properties] AS [ep]\r\n    WHERE [ep].[major_id] = [t].[object_id]\r\n        AND [ep].[minor_id] = 0\r\n        AND [ep].[class] = 1\r\n        AND [ep].[name] = N'microsoft_database_tools_support'\r\n    )\r\nAND [t].[name] <> '__EFMigrationsHistory'";
		if (flag2)
		{
			text2 += "\r\nAND [t].[temporal_type] <> 1";
		}
		if (tableFilter != null)
		{
			text2 = text2 + "\r\nAND " + tableFilter("SCHEMA_NAME([t].[schema_id])", "[t].[name]");
		}
		text = text + "\r\nWHERE " + text2;
		string text3 = "\r\nUNION\r\nSELECT\r\n    SCHEMA_NAME([v].[schema_id]) AS [schema],\r\n    [v].[name],\r\n    CAST([e].[value] AS nvarchar(MAX)) AS [comment],\r\n    'view' AS [type]";
		if (flag)
		{
			text3 += ",\r\n    CAST(0 AS bit) AS [is_memory_optimized]";
		}
		text3 += "\r\nFROM [sys].[views] AS [v]\r\nLEFT JOIN [sys].[extended_properties] AS [e] ON [e].[major_id] = [v].[object_id] AND [e].[minor_id] = 0 AND [e].[class] = 1 AND [e].[name] = 'MS_Description'";
		string text4 = "[v].[is_ms_shipped] = 0\r\nAND [v].[is_date_correlation_view] = 0 ";
		if (tableFilter != null)
		{
			text4 = text4 + "\r\nAND " + tableFilter("SCHEMA_NAME([v].[schema_id])", "[v].[name]");
		}
		text3 = text3 + "\r\nWHERE " + text4;
		dbCommand.CommandText = text + text3;
		using (DbDataReader dbDataReader = dbCommand.ExecuteReader())
		{
			while (dbDataReader.Read())
			{
				string valueOrDefault = dbDataReader.GetValueOrDefault<string>("schema");
				string valueOrDefault2 = dbDataReader.GetValueOrDefault<string>("name");
				string valueOrDefault3 = dbDataReader.GetValueOrDefault<string>("comment");
				string valueOrDefault4 = dbDataReader.GetValueOrDefault<string>("type");
				_logger.TableFound(DisplayName(valueOrDefault, valueOrDefault2));
				DatabaseTable databaseTable = ((valueOrDefault4 == "table") ? new DatabaseTable() : new DatabaseView());
				databaseTable.Schema = valueOrDefault;
				databaseTable.Name = valueOrDefault2;
				databaseTable.Comment = valueOrDefault3;
				if (flag && dbDataReader.GetValueOrDefault<bool>("is_memory_optimized"))
				{
					databaseTable["SqlServer:MemoryOptimized"] = true;
				}
				list.Add(databaseTable);
			}
		}
		GetColumns(connection, list, text2, text4, typeAliases);
		GetIndexes(connection, list, text2);
		GetForeignKeys(connection, list, text2);
		return list;
	}

	private void GetColumns(DbConnection connection, IReadOnlyList<DatabaseTable> tables, string tableFilter, string viewFilter, IReadOnlyDictionary<string, (string storeType, string typeName)> typeAliases)
	{
		using DbCommand dbCommand = connection.CreateCommand();
		string text = "\r\nSELECT\r\n    SCHEMA_NAME([o].[schema_id]) AS [table_schema],\r\n    [o].[name] AS [table_name],\r\n    [c].[name] AS [column_name],\r\n    [c].[column_id] AS [ordinal],\r\n    SCHEMA_NAME([tp].[schema_id]) AS [type_schema],\r\n    [tp].[name] AS [type_name],\r\n    CAST([c].[max_length] AS int) AS [max_length],\r\n    CAST([c].[precision] AS int) AS [precision],\r\n    CAST([c].[scale] AS int) AS [scale],\r\n    [c].[is_nullable],\r\n    [c].[is_identity],\r\n    [dc].[definition] AS [default_sql],\r\n    [cc].[definition] AS [computed_sql],\r\n    CAST([e].[value] AS nvarchar(MAX)) AS [comment]\r\nFROM\r\n(\r\n    SELECT[v].[name], [v].[object_id], [v].[schema_id]\r\n    FROM [sys].[views] v WHERE ";
		text += viewFilter;
		text += "\r\nUNION ALL\r\n    SELECT [t].[name], [t].[object_id], [t].[schema_id]\r\n    FROM [sys].[tables] t WHERE ";
		text += tableFilter;
		text += "\r\n) o\r\nJOIN [sys].[columns] AS [c] ON [o].[object_id] = [c].[object_id]\r\nJOIN [sys].[types] AS [tp] ON [c].[user_type_id] = [tp].[user_type_id]\r\nLEFT JOIN [sys].[extended_properties] AS [e] ON [e].[major_id] = [o].[object_id] AND [e].[minor_id] = [c].[column_id] AND [e].[class] = 1 AND [e].[name] = 'MS_Description'\r\nLEFT JOIN [sys].[computed_columns] AS [cc] ON [c].[object_id] = [cc].[object_id] AND [c].[column_id] = [cc].[column_id]\r\nLEFT JOIN [sys].[default_constraints] AS [dc] ON [c].[object_id] = [dc].[parent_object_id] AND [c].[column_id] = [dc].[parent_column_id]";
		if (SupportsTemporalTable(connection))
		{
			text += " WHERE [c].[is_hidden] = 0";
		}
		text += "\r\nORDER BY [table_schema], [table_name], [c].[column_id]";
		dbCommand.CommandText = text;
		using DbDataReader source = dbCommand.ExecuteReader();
		foreach (IGrouping<(string, string), DbDataRecord> item in from DbDataRecord ddr in source
			group ddr by (tableSchema: ddr.GetValueOrDefault<string>("table_schema"), tableName: ddr.GetValueOrDefault<string>("table_name")))
		{
			string tableSchema = item.Key.Item1;
			string tableName = item.Key.Item2;
			DatabaseTable databaseTable = tables.Single((DatabaseTable t) => t.Schema == tableSchema && t.Name == tableName);
			foreach (DbDataRecord item2 in item)
			{
				string valueOrDefault = item2.GetValueOrDefault<string>("column_name");
				int valueOrDefault2 = item2.GetValueOrDefault<int>("ordinal");
				string valueOrDefault3 = item2.GetValueOrDefault<string>("type_schema");
				string valueOrDefault4 = item2.GetValueOrDefault<string>("type_name");
				int valueOrDefault5 = item2.GetValueOrDefault<int>("max_length");
				int valueOrDefault6 = item2.GetValueOrDefault<int>("precision");
				int valueOrDefault7 = item2.GetValueOrDefault<int>("scale");
				bool valueOrDefault8 = item2.GetValueOrDefault<bool>("is_nullable");
				bool valueOrDefault9 = item2.GetValueOrDefault<bool>("is_identity");
				string valueOrDefault10 = item2.GetValueOrDefault<string>("default_sql");
				string valueOrDefault11 = item2.GetValueOrDefault<string>("computed_sql");
				string valueOrDefault12 = item2.GetValueOrDefault<string>("comment");
				_logger.ColumnFound(DisplayName(tableSchema, tableName), valueOrDefault, valueOrDefault2, DisplayName(valueOrDefault3, valueOrDefault4), valueOrDefault5, valueOrDefault6, valueOrDefault7, valueOrDefault8, valueOrDefault9, valueOrDefault10, valueOrDefault11);
				string text2;
				string dataTypeName;
				if (typeAliases.TryGetValue("[" + valueOrDefault3 + "].[" + valueOrDefault4 + "]", out (string, string) value))
				{
					(text2, dataTypeName) = value;
				}
				else
				{
					text2 = GetStoreType(valueOrDefault4, valueOrDefault5, valueOrDefault6, valueOrDefault7);
					dataTypeName = valueOrDefault4;
				}
				valueOrDefault10 = FilterClrDefaults(dataTypeName, valueOrDefault8, valueOrDefault10);
				DatabaseColumn databaseColumn = new DatabaseColumn
				{
					Table = databaseTable,
					Name = valueOrDefault,
					StoreType = text2,
					IsNullable = valueOrDefault8,
					DefaultValueSql = valueOrDefault10,
					ComputedColumnSql = valueOrDefault11,
					Comment = valueOrDefault12,
					ValueGenerated = (valueOrDefault9 ? new ValueGenerated?(ValueGenerated.OnAdd) : ((text2 == "rowversion") ? new ValueGenerated?(ValueGenerated.OnAddOrUpdate) : null))
				};
				if (text2 == "rowversion")
				{
					databaseColumn["ConcurrencyToken"] = true;
				}
				databaseTable.Columns.Add(databaseColumn);
			}
		}
	}

	private static string FilterClrDefaults(string dataTypeName, bool nullable, string defaultValue)
	{
		if (defaultValue == null || defaultValue == "(NULL)")
		{
			return null;
		}
		if (nullable)
		{
			return defaultValue;
		}
		switch (defaultValue)
		{
		case "((0))":
			switch (dataTypeName)
			{
			case "bigint":
			case "bit":
			case "decimal":
			case "float":
			case "int":
			case "money":
			case "numeric":
			case "real":
			case "smallint":
			case "smallmoney":
			case "tinyint":
				return null;
			}
			break;
		case "((0.0))":
			switch (dataTypeName)
			{
			case "decimal":
			case "float":
			case "money":
			case "numeric":
			case "real":
			case "smallmoney":
				return null;
			}
			break;
		case "(CONVERT([real],(0)))":
			if (!(dataTypeName == "real"))
			{
				goto default;
			}
			goto IL_0204;
		default:
			{
				if ((!(defaultValue == "((0.0000000000000000e+000))") || !(dataTypeName == "float")) && (!(defaultValue == "('0001-01-01')") || !(dataTypeName == "date")) && (!(defaultValue == "('1900-01-01T00:00:00.000')") || (!(dataTypeName == "datetime") && !(dataTypeName == "smalldatetime"))) && (!(defaultValue == "('0001-01-01T00:00:00.000')") || !(dataTypeName == "datetime2")) && (!(defaultValue == "('0001-01-01T00:00:00.000+00:00')") || !(dataTypeName == "datetimeoffset")) && (!(defaultValue == "('00:00:00')") || !(dataTypeName == "time")) && (!(defaultValue == "('00000000-0000-0000-0000-000000000000')") || !(dataTypeName == "uniqueidentifier")))
				{
					break;
				}
				goto IL_0204;
			}
			IL_0204:
			return null;
		}
		return defaultValue;
	}

	private static string GetStoreType(string dataTypeName, int maxLength, int precision, int scale)
	{
		switch (dataTypeName)
		{
		case "timestamp":
			return "rowversion";
		case "decimal":
		case "numeric":
			return $"{dataTypeName}({precision}, {scale})";
		default:
			if (_dateTimePrecisionTypes.Contains(dataTypeName) && scale != 7)
			{
				return $"{dataTypeName}({scale})";
			}
			if (_maxLengthRequiredTypes.Contains(dataTypeName))
			{
				if (maxLength == -1)
				{
					return dataTypeName + "(max)";
				}
				if (dataTypeName == "nvarchar" || dataTypeName == "nchar")
				{
					maxLength /= 2;
				}
				return $"{dataTypeName}({maxLength})";
			}
			return dataTypeName;
		}
	}

	private void GetIndexes(DbConnection connection, IReadOnlyList<DatabaseTable> tables, string tableFilter)
	{
		using DbCommand dbCommand = connection.CreateCommand();
		string text = "\r\nSELECT\r\n    SCHEMA_NAME([t].[schema_id]) AS [table_schema],\r\n    [t].[name] AS [table_name],\r\n    [i].[name] AS [index_name],\r\n    [i].[type_desc],\r\n    [i].[is_primary_key],\r\n    [i].[is_unique_constraint],\r\n    [i].[is_unique],\r\n    [i].[has_filter],\r\n    [i].[filter_definition],\r\n    COL_NAME([ic].[object_id], [ic].[column_id]) AS [column_name]\r\nFROM [sys].[indexes] AS [i]\r\nJOIN [sys].[tables] AS [t] ON [i].[object_id] = [t].[object_id]\r\nJOIN [sys].[index_columns] AS [ic] ON [i].[object_id] = [ic].[object_id] AND [i].[index_id] = [ic].[index_id]\r\nJOIN [sys].[columns] AS [c] ON [ic].[object_id] = [c].[object_id] AND [ic].[column_id] = [c].[column_id]\r\nWHERE " + tableFilter;
		if (SupportsTemporalTable(connection))
		{
			text = text + "\r\nAND CAST([i].[object_id] AS nvarchar(12)) + '#' + CAST([i].[index_id] AS nvarchar(12)) NOT IN\r\n(\r\n   SELECT CAST([i].[object_id] AS nvarchar(12)) + '#' + CAST([i].[index_id] AS nvarchar(12))\r\n   FROM [sys].[indexes] i\r\n   JOIN [sys].[tables] AS [t] ON [i].[object_id] = [t].[object_id]\r\n   JOIN [sys].[index_columns] AS [ic] ON [i].[object_id] = [ic].[object_id] AND [i].[index_id] = [ic].[index_id]\r\n   JOIN [sys].[columns] AS [c] ON [ic].[object_id] = [c].[object_id] AND [ic].[column_id] = [c].[column_id]\r\n   WHERE " + tableFilter;
			text += "\r\n   AND [c].[is_hidden] = 1\r\n)";
		}
		text += "\r\nORDER BY [table_schema], [table_name], [index_name], [ic].[key_ordinal]";
		dbCommand.CommandText = text;
		using DbDataReader source = dbCommand.ExecuteReader();
		foreach (IGrouping<(string, string), DbDataRecord> item4 in from DbDataRecord ddr in source
			group ddr by (tableSchema: ddr.GetValueOrDefault<string>("table_schema"), tableName: ddr.GetValueOrDefault<string>("table_name")))
		{
			string tableSchema = item4.Key.Item1;
			string tableName = item4.Key.Item2;
			DatabaseTable databaseTable = tables.Single((DatabaseTable t) => t.Schema == tableSchema && t.Name == tableName);
			IGrouping<(string, string), DbDataRecord>[] array = (from ddr in item4
				where ddr.GetValueOrDefault<bool>("is_primary_key")
				group ddr by (Name: ddr.GetValueOrDefault<string>("index_name"), TypeDesc: ddr.GetValueOrDefault<string>("type_desc"))).ToArray();
			if (array.Length == 1)
			{
				IGrouping<(string, string), DbDataRecord> grouping = array[0];
				_logger.PrimaryKeyFound(grouping.Key.Item1, DisplayName(tableSchema, tableName));
				DatabasePrimaryKey databasePrimaryKey = new DatabasePrimaryKey
				{
					Table = databaseTable,
					Name = grouping.Key.Item1
				};
				if (grouping.Key.Item2 == "NONCLUSTERED")
				{
					databasePrimaryKey["SqlServer:Clustered"] = false;
				}
				foreach (DbDataRecord item5 in grouping)
				{
					string columnName3 = item5.GetValueOrDefault<string>("column_name");
					DatabaseColumn item = databaseTable.Columns.FirstOrDefault((DatabaseColumn c) => c.Name == columnName3) ?? databaseTable.Columns.FirstOrDefault((DatabaseColumn c) => c.Name.Equals(columnName3, StringComparison.OrdinalIgnoreCase));
					databasePrimaryKey.Columns.Add(item);
				}
				databaseTable.PrimaryKey = databasePrimaryKey;
			}
			IGrouping<(string, string), DbDataRecord>[] array2 = (from ddr in item4
				where ddr.GetValueOrDefault<bool>("is_unique_constraint")
				group ddr by (Name: ddr.GetValueOrDefault<string>("index_name"), TypeDesc: ddr.GetValueOrDefault<string>("type_desc"))).ToArray();
			foreach (IGrouping<(string, string), DbDataRecord> grouping2 in array2)
			{
				_logger.UniqueConstraintFound(grouping2.Key.Item1, DisplayName(tableSchema, tableName));
				DatabaseUniqueConstraint databaseUniqueConstraint = new DatabaseUniqueConstraint
				{
					Table = databaseTable,
					Name = grouping2.Key.Item1
				};
				if (grouping2.Key.Item2 == "CLUSTERED")
				{
					databaseUniqueConstraint["SqlServer:Clustered"] = true;
				}
				foreach (DbDataRecord item6 in grouping2)
				{
					string columnName2 = item6.GetValueOrDefault<string>("column_name");
					DatabaseColumn item2 = databaseTable.Columns.FirstOrDefault((DatabaseColumn c) => c.Name == columnName2) ?? databaseTable.Columns.FirstOrDefault((DatabaseColumn c) => c.Name.Equals(columnName2, StringComparison.OrdinalIgnoreCase));
					databaseUniqueConstraint.Columns.Add(item2);
				}
				databaseTable.UniqueConstraints.Add(databaseUniqueConstraint);
			}
			IGrouping<(string, string, bool, bool, string), DbDataRecord>[] array3 = (from ddr in item4
				where !ddr.GetValueOrDefault<bool>("is_primary_key") && !ddr.GetValueOrDefault<bool>("is_unique_constraint")
				group ddr by (Name: ddr.GetValueOrDefault<string>("index_name"), TypeDesc: ddr.GetValueOrDefault<string>("type_desc"), IsUnique: ddr.GetValueOrDefault<bool>("is_unique"), HasFilter: ddr.GetValueOrDefault<bool>("has_filter"), FilterDefinition: ddr.GetValueOrDefault<string>("filter_definition"))).ToArray();
			foreach (IGrouping<(string, string, bool, bool, string), DbDataRecord> grouping3 in array3)
			{
				_logger.IndexFound(grouping3.Key.Item1, DisplayName(tableSchema, tableName), grouping3.Key.Item3);
				DatabaseIndex databaseIndex = new DatabaseIndex
				{
					Table = databaseTable,
					Name = grouping3.Key.Item1,
					IsUnique = grouping3.Key.Item3,
					Filter = (grouping3.Key.Item4 ? grouping3.Key.Item5 : null)
				};
				if (grouping3.Key.Item2 == "CLUSTERED")
				{
					databaseIndex["SqlServer:Clustered"] = true;
				}
				foreach (DbDataRecord item7 in grouping3)
				{
					string columnName = item7.GetValueOrDefault<string>("column_name");
					DatabaseColumn item3 = databaseTable.Columns.FirstOrDefault((DatabaseColumn c) => c.Name == columnName) ?? databaseTable.Columns.FirstOrDefault((DatabaseColumn c) => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
					databaseIndex.Columns.Add(item3);
				}
				databaseTable.Indexes.Add(databaseIndex);
			}
		}
	}

	private void GetForeignKeys(DbConnection connection, IReadOnlyList<DatabaseTable> tables, string tableFilter)
	{
		using DbCommand dbCommand = connection.CreateCommand();
		dbCommand.CommandText = "\r\nSELECT\r\n    SCHEMA_NAME([t].[schema_id]) AS [table_schema],\r\n    [t].[name] AS [table_name],\r\n    [f].[name],\r\n    OBJECT_SCHEMA_NAME([f].[referenced_object_id]) AS [principal_table_schema],\r\n    OBJECT_NAME([f].[referenced_object_id]) AS [principal_table_name],\r\n    [f].[delete_referential_action_desc],\r\n    col_name([fc].[parent_object_id], [fc].[parent_column_id]) AS [column_name],\r\n    col_name([fc].[referenced_object_id], [fc].[referenced_column_id]) AS [referenced_column_name]\r\nFROM [sys].[foreign_keys] AS [f]\r\nJOIN [sys].[tables] AS [t] ON [f].[parent_object_id] = [t].[object_id]\r\nJOIN [sys].[foreign_key_columns] AS [fc] ON [f].[object_id] = [fc].[constraint_object_id]\r\nWHERE " + tableFilter + "\r\nORDER BY [table_schema], [table_name], [f].[name], [fc].[constraint_column_id]";
		using DbDataReader source = dbCommand.ExecuteReader();
		foreach (IGrouping<(string, string), DbDataRecord> item4 in from DbDataRecord ddr in source
			group ddr by (tableSchema: ddr.GetValueOrDefault<string>("table_schema"), tableName: ddr.GetValueOrDefault<string>("table_name")))
		{
			string tableSchema = item4.Key.Item1;
			string tableName = item4.Key.Item2;
			DatabaseTable databaseTable = tables.Single((DatabaseTable t) => t.Schema == tableSchema && t.Name == tableName);
			foreach (IGrouping<(string, string, string, string), DbDataRecord> item5 in from c in item4
				group c by (Name: c.GetValueOrDefault<string>("name"), PrincipalTableSchema: c.GetValueOrDefault<string>("principal_table_schema"), PrincipalTableName: c.GetValueOrDefault<string>("principal_table_name"), OnDeleteAction: c.GetValueOrDefault<string>("delete_referential_action_desc")))
			{
				string item = item5.Key.Item1;
				string principalTableSchema = item5.Key.Item2;
				string principalTableName = item5.Key.Item3;
				string item2 = item5.Key.Item4;
				_logger.ForeignKeyFound(item, DisplayName(databaseTable.Schema, databaseTable.Name), DisplayName(principalTableSchema, principalTableName), item2);
				DatabaseTable databaseTable2 = tables.FirstOrDefault((DatabaseTable t) => t.Schema == principalTableSchema && t.Name == principalTableName) ?? tables.FirstOrDefault((DatabaseTable t) => t.Schema.Equals(principalTableSchema, StringComparison.OrdinalIgnoreCase) && t.Name.Equals(principalTableName, StringComparison.OrdinalIgnoreCase));
				if (databaseTable2 == null)
				{
					_logger.ForeignKeyReferencesMissingPrincipalTableWarning(item, DisplayName(databaseTable.Schema, databaseTable.Name), DisplayName(principalTableSchema, principalTableName));
					continue;
				}
				DatabaseForeignKey databaseForeignKey = new DatabaseForeignKey
				{
					Name = item,
					Table = databaseTable,
					PrincipalTable = databaseTable2,
					OnDelete = ConvertToReferentialAction(item2)
				};
				bool flag = false;
				foreach (DbDataRecord item6 in item5)
				{
					string columnName = item6.GetValueOrDefault<string>("column_name");
					DatabaseColumn item3 = databaseTable.Columns.FirstOrDefault((DatabaseColumn c) => c.Name == columnName) ?? databaseTable.Columns.FirstOrDefault((DatabaseColumn c) => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
					string principalColumnName = item6.GetValueOrDefault<string>("referenced_column_name");
					DatabaseColumn databaseColumn = databaseForeignKey.PrincipalTable.Columns.FirstOrDefault((DatabaseColumn c) => c.Name == principalColumnName) ?? databaseForeignKey.PrincipalTable.Columns.FirstOrDefault((DatabaseColumn c) => c.Name.Equals(principalColumnName, StringComparison.OrdinalIgnoreCase));
					if (databaseColumn == null)
					{
						flag = true;
						_logger.ForeignKeyPrincipalColumnMissingWarning(item, DisplayName(databaseTable.Schema, databaseTable.Name), principalColumnName, DisplayName(principalTableSchema, principalTableName));
						break;
					}
					databaseForeignKey.Columns.Add(item3);
					databaseForeignKey.PrincipalColumns.Add(databaseColumn);
				}
				if (!flag)
				{
					if (databaseForeignKey.Columns.SequenceEqual(databaseForeignKey.PrincipalColumns))
					{
						_logger.ReflexiveConstraintIgnored(databaseForeignKey.Name, DisplayName(databaseTable.Schema, databaseTable.Name));
					}
					else
					{
						databaseTable.ForeignKeys.Add(databaseForeignKey);
					}
				}
			}
		}
	}

	private bool SupportsTemporalTable(DbConnection connection)
	{
		if (CompatibilityLevel(connection) >= 130)
		{
			return EngineEdition(connection) != 6;
		}
		return false;
	}

	private bool SupportsMemoryOptimizedTable(DbConnection connection)
	{
		if (CompatibilityLevel(connection) >= 120)
		{
			return EngineEdition(connection) != 6;
		}
		return false;
	}

	private bool SupportsSequences(DbConnection connection)
	{
		if (CompatibilityLevel(connection) >= 110)
		{
			return EngineEdition(connection) != 6;
		}
		return false;
	}

	private int EngineEdition(DbConnection connection)
	{
		using DbCommand dbCommand = connection.CreateCommand();
		dbCommand.CommandText = "\r\nSELECT SERVERPROPERTY('EngineEdition');";
		return (int)dbCommand.ExecuteScalar();
	}

	private byte CompatibilityLevel(DbConnection connection)
	{
		using DbCommand dbCommand = connection.CreateCommand();
		dbCommand.CommandText = "\r\nSELECT compatibility_level\r\nFROM sys.databases\r\nWHERE name = '" + connection.Database + "';";
		object obj = dbCommand.ExecuteScalar();
		return (byte)((obj != null) ? Convert.ToByte(obj) : 0);
	}

	private static string DisplayName(string schema, string name)
	{
		return ((!string.IsNullOrEmpty(schema)) ? (schema + ".") : "") + name;
	}

	private static ReferentialAction? ConvertToReferentialAction(string onDeleteAction)
	{
		return onDeleteAction switch
		{
			"NO_ACTION" => ReferentialAction.NoAction, 
			"CASCADE" => ReferentialAction.Cascade, 
			"SET_NULL" => ReferentialAction.SetNull, 
			"SET_DEFAULT" => ReferentialAction.SetDefault, 
			_ => null, 
		};
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Stimulsoft.Base;

public class StiOracleDevartConnector : StiOracleConnector
{
	public override Type SqlType => typeof(StiDbType.Devart.Oracle);

	public override int DefaultSqlType => 18;

	public override string[] NuGetPackages => new string[1] { "dotConnect.Express.for.Oracle" };

	public override string NuGetVersion => "9.3.230";

	public override DbConnection CreateConnection()
	{
		string dbSchemaName;
		DbConnection dbConnection = base.AssemblyHelper.CreateConnection(base.TypeConnection, GetConvertedConnectionStringToDotConnect(base.ConnectionString, out dbSchemaName));
		PropertyInfo property = dbConnection.GetType().GetProperty("Direct");
		property.SetValue(dbConnection, true, null);
		return dbConnection;
	}

	public override void DeriveParameters(DbCommand command)
	{
		PropertyInfo property = command.GetType().GetProperty("ParameterCheck");
		property.SetValue(command, true, null);
	}

	public override StiDataSchema RetrieveSchema(bool allowException = false)
	{
		if (string.IsNullOrEmpty(base.ConnectionString))
		{
			return null;
		}
		StiDataSchema stiDataSchema = new StiDataSchema(ConnectionIdent);
		try
		{
			using (DbConnection dbConnection = CreateConnection())
			{
				dbConnection.Open();
				string dbSchemaName = null;
				GetConvertedConnectionStringToDotConnect(base.ConnectionString, out dbSchemaName);
				List<StiDataTableSchema> list = new List<StiDataTableSchema>();
				try
				{
					DataTable schema = dbConnection.GetSchema("Tables", new string[2] { dbSchemaName, null });
					foreach (DataRow row in schema.Rows)
					{
						if (row["SCHEMA"] != DBNull.Value && (string)row["SCHEMA"] != "dbo")
						{
							string name = StiTableName.GetName("", row["NAME"] as string);
							string selectQuery = StiTableQuery.Get(this).GetSelectQuery(row["SCHEMA"] as string, row["NAME"] as string);
							StiDataTableSchema item = StiDataTableSchema.NewTableOrView(name, this, selectQuery);
							list.Add(item);
							stiDataSchema.Tables.Add(item);
						}
					}
				}
				catch (Exception)
				{
				}
				try
				{
					DataTable schema2 = dbConnection.GetSchema("Views", new string[2] { dbSchemaName, null });
					foreach (StiSchemaRow item5 in StiSchemaRow.All(schema2))
					{
						string name2 = StiTableName.GetName(item5.OWNER, item5.TABLE_NAME);
						string selectQuery2 = StiTableQuery.Get(this).GetSelectQuery(item5.OWNER, item5.TABLE_NAME);
						StiDataTableSchema item2 = StiDataTableSchema.NewTableOrView(name2, this, selectQuery2);
						list.Add(item2);
						stiDataSchema.Views.Add(item2);
					}
				}
				catch
				{
				}
				try
				{
					foreach (StiDataTableSchema item6 in list)
					{
						DataTable schema3 = dbConnection.GetSchema("Columns", new string[3] { dbSchemaName, item6.Name, null });
						foreach (DataRow row2 in schema3.Rows)
						{
							string empty = string.Empty;
							if (row2["SCHEMA"] != DBNull.Value && (string)row2["SCHEMA"] != "sys" && (string)row2["SCHEMA"] != "dbo")
							{
								empty = string.Format("{1}.{0}", row2["TABLE"] as string, row2["SCHEMA"] as string);
							}
							else
							{
								empty = row2["TABLE"] as string;
							}
							StiDataColumnSchema item3 = new StiDataColumnSchema(row2["NAME"] as string, GetNetType(row2["DATATYPE"] as string));
							item6.Columns.Add(item3);
						}
					}
				}
				catch (Exception)
				{
				}
				Hashtable hashtable = new Hashtable();
				try
				{
					DataTable schema4 = dbConnection.GetSchema("Procedures", new string[2] { dbSchemaName, null });
					foreach (StiSchemaRow item7 in StiSchemaRow.All(schema4))
					{
						string name3 = StiTableName.GetName(item7.OWNER, item7.OBJECT_NAME);
						string procQuery = StiTableQuery.Get(this).GetProcQuery(item7.OWNER, item7.OBJECT_NAME);
						StiDataTableSchema item4 = (StiDataTableSchema)(hashtable[name3] = StiDataTableSchema.NewProcedure(name3, this, procQuery));
						stiDataSchema.StoredProcedures.Add(item4);
					}
				}
				catch
				{
				}
				foreach (StiDataTableSchema storedProcedure in stiDataSchema.StoredProcedures)
				{
					try
					{
						using DbCommand dbCommand = CreateCommand(storedProcedure.Query, dbConnection, CommandType.StoredProcedure);
						DeriveParameters(dbCommand);
						using DbDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SchemaOnly);
						using DataTable dataTable = new DataTable(storedProcedure.Name);
						dataTable.Load(reader);
						foreach (DataColumn column in dataTable.Columns)
						{
							storedProcedure.Columns.Add(new StiDataColumnSchema
							{
								Name = column.ColumnName,
								Type = column.DataType
							});
						}
						foreach (DbParameter parameter in dbCommand.Parameters)
						{
							if (parameter.Direction == ParameterDirection.Input)
							{
								storedProcedure.Parameters.Add(new StiDataParameterSchema
								{
									Name = parameter.ParameterName,
									Type = StiDbTypeConversion.GetNetType(parameter.DbType)
								});
							}
						}
					}
					catch
					{
					}
				}
				try
				{
					string query = "\r\n                            SELECT a.table_name, a.column_name, a.constraint_name, c.owner, \r\n                                   c.r_owner, c_pk.table_name r_table_name, c_pk.constraint_name r_pk\r\n                            FROM all_cons_columns a\r\n                            JOIN all_constraints c ON a.owner = c.owner\r\n                                 AND a.constraint_name = c.constraint_name\r\n                            JOIN all_constraints c_pk ON c.r_owner = c_pk.owner\r\n                                 AND c.r_constraint_name = c_pk.constraint_name\r\n                            WHERE c.constraint_type = 'R'";
					using DataSet dataSet = new DataSet();
					using DbDataAdapter dbDataAdapter = CreateAdapter(query, dbConnection);
					dbDataAdapter.Fill(dataSet);
					DataTable dataTable2 = ((dataSet.Tables.Count > 0) ? dataSet.Tables[0] : null);
					if (dataTable2 != null)
					{
						foreach (StiSchemaRow item8 in StiSchemaRow.All(dataTable2))
						{
							stiDataSchema.Relations.Add(new StiDataRelationSchema
							{
								Name = item8.CONSTRAINT_NAME,
								ParentSourceName = $"{item8.OWNER}.{item8.TABLE_NAME}",
								ChildSourceName = $"{item8.R_OWNER}.{item8.R_TABLE_NAME}",
								ParentColumns = new List<string> { item8.COLUMN_NAME },
								ChildColumns = new List<string> { item8.R_PK }
							});
						}
					}
				}
				catch
				{
				}
				dbConnection.Close();
			}
			return stiDataSchema.Sort();
		}
		catch (Exception)
		{
			if (allowException)
			{
				throw;
			}
			return null;
		}
	}

	public override int GetSqlType(Type type)
	{
		if (type == typeof(DateTime))
		{
			return 8;
		}
		if (type == typeof(TimeSpan))
		{
			return 25;
		}
		if (type == typeof(long))
		{
			return 19;
		}
		if (type == typeof(int))
		{
			return 11;
		}
		if (type == typeof(short))
		{
			return 11;
		}
		if (type == typeof(byte))
		{
			return 11;
		}
		if (type == typeof(ulong))
		{
			return 19;
		}
		if (type == typeof(uint))
		{
			return 11;
		}
		if (type == typeof(ushort))
		{
			return 11;
		}
		if (type == typeof(sbyte))
		{
			return 11;
		}
		if (type == typeof(float))
		{
			return 10;
		}
		if (type == typeof(double))
		{
			return 9;
		}
		if (type == typeof(decimal))
		{
			return 19;
		}
		if (type == typeof(string))
		{
			return 28;
		}
		if (type == typeof(bool))
		{
			return 4;
		}
		if (type == typeof(char))
		{
			return 5;
		}
		if (type == typeof(byte[]))
		{
			return 3;
		}
		if (type == typeof(Guid))
		{
			return 22;
		}
		return 11;
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.Devart.Oracle)dbType)
		{
		case StiDbType.Devart.Oracle.Date:
			return typeof(byte[]);
		case StiDbType.Devart.Oracle.BFile:
		case StiDbType.Devart.Oracle.Blob:
		case StiDbType.Devart.Oracle.LongRaw:
		case StiDbType.Devart.Oracle.Raw:
			return typeof(byte[]);
		case StiDbType.Devart.Oracle.Float:
			return typeof(float);
		case StiDbType.Devart.Oracle.Double:
			return typeof(double);
		case StiDbType.Devart.Oracle.Number:
			return typeof(decimal);
		case StiDbType.Devart.Oracle.Byte:
			return typeof(byte);
		case StiDbType.Devart.Oracle.Int16:
			return typeof(short);
		case StiDbType.Devart.Oracle.Integer:
			return typeof(int);
		case StiDbType.Devart.Oracle.IntervalDS:
		case StiDbType.Devart.Oracle.IntervalYM:
		case StiDbType.Devart.Oracle.Long:
		case StiDbType.Devart.Oracle.Int64:
			return typeof(long);
		default:
			return typeof(string);
		}
	}

	public override Type GetNetType(string dbType)
	{
		switch (dbType.ToLowerInvariant())
		{
		case "bigint":
		case "int":
		case "uniqueidentifier":
		case "smallint":
		case "tinyint":
			return typeof(long);
		case "decimal":
		case "money":
		case "smallmoney":
		case "number":
			return typeof(decimal);
		case "float":
		case "real":
			return typeof(double);
		case "datetime":
		case "smalldatetime":
		case "timestamp":
		case "date":
			return typeof(DateTime);
		default:
			return typeof(string);
		}
	}

	private string GetConvertedConnectionStringToDotConnect(string connectionString, out string dbSchemaName)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		string text5 = string.Empty;
		Regex regex = new Regex("\\(HOST\\s*=\\s*(?<val>[0-9a-zA-Z_-]+)\\)");
		foreach (Match item in from Match match in regex.Matches(connectionString)
			where match.Success
			select match)
		{
			text = item.Groups["val"].Value;
		}
		regex = new Regex("Server\\s*=\\s*(?<val>[\\d\\.]+)|Server\\s*=\\s*(?<val>[0-9a-zA-Z_-]+)");
		foreach (Match item2 in from Match match in regex.Matches(connectionString)
			where match.Success
			select match)
		{
			text = item2.Groups["val"].Value;
		}
		regex = new Regex("Data Source\\s*=\\s*(?<val>[0-9a-zA-Z_-]+)");
		foreach (Match item3 in from Match match in regex.Matches(connectionString)
			where match.Success
			select match)
		{
			text = item3.Groups["val"].Value;
		}
		regex = new Regex("Sid\\s*=\\s*(?<val>[0-9a-zA-Z_-]+)");
		foreach (Match item4 in from Match match in regex.Matches(connectionString)
			where match.Success
			select match)
		{
			text4 = item4.Groups["val"].Value;
		}
		regex = new Regex("User Id\\s*=\\s*(?<val>[0-9a-zA-Z_-]+)");
		foreach (Match item5 in from Match match in regex.Matches(connectionString)
			where match.Success
			select match)
		{
			text2 = item5.Groups["val"].Value;
		}
		regex = new Regex("uid\\s*=\\s*(?<val>[0-9a-zA-Z_-]+)");
		foreach (Match item6 in from Match match in regex.Matches(connectionString)
			where match.Success
			select match)
		{
			text2 = item6.Groups["val"].Value;
		}
		regex = new Regex("Password\\s*=\\s*(?<val>[0-9a-zA-Z_-]+)");
		foreach (Match item7 in from Match match in regex.Matches(connectionString)
			where match.Success
			select match)
		{
			text3 = item7.Groups["val"].Value;
		}
		regex = new Regex("pwd\\s*=\\s*(?<val>[0-9a-zA-Z_-]+)");
		foreach (Match item8 in from Match match in regex.Matches(connectionString)
			where match.Success
			select match)
		{
			text3 = item8.Groups["val"].Value;
		}
		regex = new Regex("Direct\\s*=\\s*(?<val>[0-9a-zA-Z_-]+)");
		foreach (Match item9 in from Match match in regex.Matches(connectionString)
			where match.Success
			select match)
		{
			text5 = item9.Groups["val"].Value;
		}
		dbSchemaName = text2;
		return $"User Id={text2};Password={text3};Server={text};Direct={text5};SID={text4};";
	}

	public StiOracleDevartConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "Devart.Data.Oracle.dll";
		base.TypeConnection = "Devart.Data.Oracle.OracleConnection";
		base.TypeDataAdapter = "Devart.Data.Oracle.OracleDataAdapter";
		base.TypeCommand = "Devart.Data.Oracle.OracleCommand";
		base.TypeParameter = "Devart.Data.Oracle.OracleParameter";
		base.TypeDbType = "Devart.Data.Oracle.OracleDbType";
		base.TypeCommandBuilder = "Devart.Data.Oracle.CommandBuilder";
		base.TypeConnectionStringBuilder = "Devart.Data.Oracle.OracleConnectionStringBuilder";
		base.TypeDataSourceEnumerator = "Devart.Data.Oracle.OracleDataSourceEnumerator";
	}
}

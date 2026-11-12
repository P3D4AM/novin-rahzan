using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Stimulsoft.Base;

public class StiMySqlDevartConnector : StiMySqlConnector
{
	public override Type SqlType => typeof(StiDbType.Devart.MySql);

	public override int DefaultSqlType => 13;

	public override string[] NuGetPackages => new string[1] { "dotConnect.Express.for.MySQL" };

	public override string NuGetVersion => "8.8.862";

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
				List<StiDataTableSchema> list = new List<StiDataTableSchema>();
				try
				{
					DataTable schema = dbConnection.GetSchema("Tables", new string[2] { dbConnection.Database, null });
					foreach (StiSchemaRow item4 in StiSchemaRow.All(schema))
					{
						StiDataTableSchema item = StiDataTableSchema.NewTable(item4.NAME, this);
						list.Add(item);
						stiDataSchema.Tables.Add(item);
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema2 = dbConnection.GetSchema("Views", new string[2] { dbConnection.Database, null });
					foreach (StiSchemaRow item5 in StiSchemaRow.All(schema2))
					{
						StiDataTableSchema item2 = StiDataTableSchema.NewView(item5.NAME, this);
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
						DataTable schema3 = dbConnection.GetSchema("Columns", new string[3] { dbConnection.Database, item6.Name, null });
						foreach (StiSchemaRow item7 in StiSchemaRow.All(schema3))
						{
							StiDataColumnSchema item3 = new StiDataColumnSchema(item7.NAME, GetNetType(item7.DATATYPE));
							item6.Columns.Add(item3);
						}
					}
				}
				catch
				{
				}
				Hashtable hashtable = new Hashtable();
				try
				{
					DataTable schema4 = dbConnection.GetSchema("Procedures", new string[2] { dbConnection.Database, null });
					foreach (StiSchemaRow item8 in StiSchemaRow.All(schema4))
					{
						StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewProcedure(item8.NAME, this);
						hashtable[stiDataTableSchema.Name] = stiDataTableSchema;
						stiDataSchema.StoredProcedures.Add(stiDataTableSchema);
					}
				}
				catch
				{
				}
				foreach (StiDataTableSchema storedProcedure in stiDataSchema.StoredProcedures)
				{
					try
					{
						using DbCommand dbCommand = CreateCommand(storedProcedure.Name, dbConnection, CommandType.StoredProcedure);
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
						if (dbCommand.Parameters.Count <= 0)
						{
							continue;
						}
						StringBuilder stringBuilder = new StringBuilder();
						foreach (DbParameter parameter in dbCommand.Parameters)
						{
							if (parameter.Direction == ParameterDirection.Input)
							{
								storedProcedure.Parameters.Add(new StiDataParameterSchema
								{
									Name = parameter.ParameterName,
									Type = StiDbTypeConversion.GetNetType(parameter.DbType)
								});
								stringBuilder = ((stringBuilder.Length == 0) ? stringBuilder.Append(parameter.ParameterName) : stringBuilder.AppendFormat("{0},", parameter.ParameterName));
							}
						}
						if (stringBuilder.Length > 0)
						{
							storedProcedure.Query = $"{storedProcedure.Query} ({stringBuilder})";
						}
					}
					catch
					{
					}
				}
				try
				{
					foreach (StiDataTableSchema table in stiDataSchema.Tables)
					{
						using DataSet dataSet = new DataSet();
						string format = "SELECT * FROM information_schema.TABLE_CONSTRAINTS i LEFT JOIN information_schema.KEY_COLUMN_USAGE k ON i.CONSTRAINT_NAME = k.CONSTRAINT_NAME WHERE i.CONSTRAINT_TYPE = 'FOREIGN KEY' and i.TABLE_NAME = '{0}'";
						using DbDataAdapter dbDataAdapter = CreateAdapter(string.Format(format, table.Name), dbConnection);
						dbDataAdapter.Fill(dataSet, table.Name);
						DataTable dataTable2 = ((dataSet.Tables.Count > 0) ? dataSet.Tables[0] : null);
						if (dataTable2 == null)
						{
							continue;
						}
						foreach (StiSchemaRow item9 in StiSchemaRow.All(dataTable2))
						{
							stiDataSchema.Relations.Add(new StiDataRelationSchema
							{
								Name = item9.CONSTRAINT_NAME,
								ParentSourceName = item9.TABLE_NAME,
								ChildSourceName = item9.REFERENCED_TABLE_NAME,
								ParentColumns = new List<string> { item9.COLUMN_NAME },
								ChildColumns = new List<string> { item9.REFERENCED_COLUMN_NAME }
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

	public override DbConnection CreateConnection()
	{
		DbConnection dbConnection = base.CreateConnection();
		dbConnection.GetType().GetProperty("Charset")?.SetValue(dbConnection, "auto", null);
		dbConnection.GetType().GetProperty("Unicode")?.SetValue(dbConnection, true, null);
		return dbConnection;
	}

	public override void DeriveParameters(DbCommand command)
	{
		PropertyInfo property = command.GetType().GetProperty("ParameterCheck");
		property.SetValue(command, true, null);
	}

	public override int GetSqlType(Type type)
	{
		if (type == typeof(DateTime))
		{
			return 7;
		}
		if (type == typeof(TimeSpan))
		{
			return 15;
		}
		if (type == typeof(long))
		{
			return 1;
		}
		if (type == typeof(int))
		{
			return 11;
		}
		if (type == typeof(short))
		{
			return 12;
		}
		if (type == typeof(sbyte))
		{
			return 16;
		}
		if (type == typeof(ulong))
		{
			return 1;
		}
		if (type == typeof(uint))
		{
			return 11;
		}
		if (type == typeof(ushort))
		{
			return 12;
		}
		if (type == typeof(byte))
		{
			return 16;
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
			return 8;
		}
		if (type == typeof(string))
		{
			return 18;
		}
		if (type == typeof(bool))
		{
			return 12;
		}
		if (type == typeof(char))
		{
			return 5;
		}
		if (type == typeof(byte[]))
		{
			return 2;
		}
		if (type == typeof(Guid))
		{
			return 20;
		}
		return 11;
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.Devart.MySql)dbType)
		{
		case StiDbType.Devart.MySql.BigInt:
		case StiDbType.Devart.MySql.Year:
			return typeof(long);
		case StiDbType.Devart.MySql.Int:
			return typeof(int);
		case StiDbType.Devart.MySql.SmallInt:
			return typeof(short);
		case StiDbType.Devart.MySql.TinyInt:
			return typeof(sbyte);
		case StiDbType.Devart.MySql.Decimal:
			return typeof(decimal);
		case StiDbType.Devart.MySql.Float:
			return typeof(float);
		case StiDbType.Devart.MySql.Double:
			return typeof(double);
		case StiDbType.Devart.MySql.Bit:
			return typeof(bool);
		case StiDbType.Devart.MySql.Date:
		case StiDbType.Devart.MySql.DateTime:
		case StiDbType.Devart.MySql.TimeStamp:
			return typeof(DateTime);
		case StiDbType.Devart.MySql.Time:
			return typeof(TimeSpan);
		default:
			return typeof(string);
		}
	}

	public override string GetSampleConnectionString()
	{
		return "User ID=root;Password=myPassword;Host=localhost;Port=3306;Database=myDataBase;" + Environment.NewLine + "Direct=true;Protocol=TCP;Compress=false;Pooling=true;Min Pool Size=0;" + Environment.NewLine + "Max Pool Size=100;Connection Lifetime=0;";
	}

	public StiMySqlDevartConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "Devart.Data.MySql.dll";
		base.TypeConnection = "Devart.Data.MySql.MySqlConnection";
		base.TypeDataAdapter = "Devart.Data.MySql.MySqlDataAdapter";
		base.TypeCommand = "Devart.Data.MySql.MySqlCommand";
		base.TypeParameter = "Devart.Data.MySql.MySqlParameter";
		base.TypeDbType = "Devart.Data.MySql.MySqlType";
		base.TypeCommandBuilder = "Devart.Data.MySql.MySqlCommandBuilder";
	}
}

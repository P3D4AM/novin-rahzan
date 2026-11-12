using System;
using System.Data;
using System.Data.Common;

namespace Stimulsoft.Base;

public abstract class StiSqlDataConnector : StiDataConnector
{
	private StiDataAssemblyHelper assemblyHelper;

	public string NameAssembly { get; set; }

	public string FolderAssembly { get; set; }

	public string TypeConnection { get; set; }

	public string TypeDataAdapter { get; set; }

	public string TypeCommand { get; set; }

	public string TypeParameter { get; set; }

	public string TypeDbType { get; set; }

	public string TypeCommandBuilder { get; set; }

	public string TypeConnectionStringBuilder { get; set; }

	public string TypeDataSourceEnumerator { get; set; }

	public string MethodDeriveParameters { get; set; } = "DeriveParameters";


	public string ConnectionString { get; set; }

	protected StiDataAssemblyHelper AssemblyHelper
	{
		get
		{
			if (NameAssembly == null)
			{
				throw new NotSupportedException("NameAssembly is not specified!");
			}
			return assemblyHelper ?? (assemblyHelper = new StiDataAssemblyHelper(NameAssembly, FolderAssembly));
		}
	}

	public override bool IsAvailable
	{
		get
		{
			if (AssemblyHelper != null)
			{
				return AssemblyHelper.IsAllowed;
			}
			return false;
		}
	}

	public virtual Type ConnectionType => AssemblyHelper.GetType(TypeConnection);

	public abstract Type SqlType { get; }

	public abstract int DefaultSqlType { get; }

	public virtual void SetTimeout(DbCommand command, int timeOut)
	{
		command.CommandTimeout = timeOut;
	}

	public virtual void SetTimeout(IDbCommand command, int timeOut)
	{
		SetTimeout(command as DbCommand, timeOut);
	}

	public virtual StiTestConnectionResult TestConnection()
	{
		try
		{
			using DbConnection dbConnection = CreateConnection();
			dbConnection.Open();
			dbConnection.Close();
		}
		catch (Exception ex)
		{
			return StiTestConnectionResult.MakeWrong(ex.Message);
		}
		return StiTestConnectionResult.MakeFine();
	}

	public virtual DbConnection CreateConnection()
	{
		return AssemblyHelper.CreateConnection(TypeConnection, ConnectionString);
	}

	public virtual DbDataAdapter CreateAdapter(string query, DbConnection connection, CommandType commandType = CommandType.Text)
	{
		DbDataAdapter dbDataAdapter = AssemblyHelper.CreateAdapter(TypeDataAdapter, query, connection);
		dbDataAdapter.SelectCommand.CommandType = commandType;
		return dbDataAdapter;
	}

	public virtual DbCommand CreateCommand(string query, DbConnection connection, CommandType commandType = CommandType.Text)
	{
		DbCommand dbCommand = AssemblyHelper.CreateCommand(TypeCommand, query, connection);
		dbCommand.CommandType = commandType;
		return dbCommand;
	}

	public virtual DbParameter CreateParameter(string parameterName, object value, int size = 0)
	{
		return AssemblyHelper.CreateParameterWithValueAndSize(TypeParameter, parameterName, value, size);
	}

	public virtual DbParameter CreateParameter(string parameterName, int type, int size = 0)
	{
		Type dbType = GetDbType();
		return AssemblyHelper.CreateParameterWithTypeAndSize(TypeParameter, parameterName, type, size, dbType ?? typeof(int));
	}

	public virtual DbCommandBuilder CreateCommandBuilder()
	{
		return AssemblyHelper.CreateCommandBuilder(TypeCommandBuilder);
	}

	public virtual DbConnectionStringBuilder CreateConnectionStringBuilder()
	{
		return AssemblyHelper.CreateConnectionStringBuilder(TypeConnectionStringBuilder, ConnectionString);
	}

	public virtual DbDataSourceEnumerator CreateDataSourceEnumerator()
	{
		return AssemblyHelper.CreateDataSourceEnumerator(TypeDataSourceEnumerator);
	}

	public virtual void DeriveParameters(DbCommand command)
	{
		try
		{
			if (TypeCommandBuilder == null || MethodDeriveParameters == null)
			{
				throw new NotSupportedException();
			}
			AssemblyHelper.DeriveParameters(TypeCommandBuilder, MethodDeriveParameters, command);
		}
		catch
		{
		}
	}

	public abstract StiDataSchema RetrieveSchema(bool allowException = false);

	public abstract int GetSqlType(Type type);

	public abstract Type GetNetType(string dbType);

	public abstract Type GetNetType(int dbType);

	public virtual string GetDatabaseSpecificName(string name)
	{
		return name;
	}

	public DataTable GetSchemaTable(DbCommand command, bool isStoredProc = false)
	{
		DataTable dataTable = new DataTable("Schema");
		using DbDataReader reader = command.ExecuteReader((StiDataOptions.RetrieveColumnsMode == StiRetrieveColumnsMode.KeyInfo) ? CommandBehavior.KeyInfo : CommandBehavior.SchemaOnly);
		dataTable.Load(reader);
		return dataTable;
	}

	public DataTable GetDataTable(DbCommand command, int index, int count)
	{
		DataTable dataTable = new DataTable("Data");
		using (DbDataReader dbDataReader = command.ExecuteReader())
		{
			for (int i = 0; i < dbDataReader.FieldCount; i++)
			{
				string name = dbDataReader.GetName(i);
				dataTable.Columns.Add(name);
			}
			for (int j = 0; j < index; j++)
			{
				if (!dbDataReader.Read())
				{
					break;
				}
			}
			for (int k = 0; k < count; k++)
			{
				if (!dbDataReader.Read())
				{
					break;
				}
				object[] values = new object[dbDataReader.FieldCount];
				int values2 = dbDataReader.GetValues(values);
				if (values2 > 0)
				{
					dataTable.Rows.Add(values);
				}
			}
		}
		return dataTable;
	}

	public DataTable GetParametersTable(DbCommand command)
	{
		DataTable dataTable = new DataTable("Parameters");
		dataTable.Columns.Add("Name");
		dataTable.Columns.Add("Type");
		dataTable.Columns.Add("Direction");
		foreach (DbParameter parameter in command.Parameters)
		{
			if (parameter.Direction == ParameterDirection.Input)
			{
				dataTable.Rows.Add(parameter.ParameterName, StiDbTypeConversion.GetNetType(parameter.DbType).ToString(), parameter.Direction.ToString());
			}
		}
		return dataTable;
	}

	protected virtual DataTable GetRelationsTable(DbConnection connection, string tableName)
	{
		return null;
	}

	public abstract string GetSampleConnectionString();

	public virtual Type GetDbType()
	{
		return AssemblyHelper.GetType(TypeDbType);
	}

	public static StiSqlDataConnector Get(StiConnectionIdent ident, string connectionString)
	{
		return ident switch
		{
			StiConnectionIdent.Db2DataSource => StiDb2Connector.Get(connectionString), 
			StiConnectionIdent.FirebirdDataSource => StiFirebirdConnector.Get(connectionString), 
			StiConnectionIdent.InformixDataSource => StiInformixConnector.Get(connectionString), 
			StiConnectionIdent.MsAccessDataSource => StiMsAccessConnector.Get(connectionString), 
			StiConnectionIdent.MsSqlDataSource => StiMsSqlConnector.Get(connectionString), 
			StiConnectionIdent.MySqlDataSource => StiMySqlConnector.Get(connectionString), 
			StiConnectionIdent.OdbcDataSource => StiOdbcConnector.Get(connectionString), 
			StiConnectionIdent.OleDbDataSource => StiOleDbConnector.Get(connectionString), 
			StiConnectionIdent.OracleDataSource => StiOracleConnector.Get(connectionString), 
			StiConnectionIdent.PostgreSqlDataSource => StiPostgreSqlConnector.Get(connectionString), 
			StiConnectionIdent.SqlCeDataSource => StiSqlCeConnector.Get(connectionString), 
			StiConnectionIdent.SqLiteDataSource => StiSqLiteConnector.Get(connectionString), 
			StiConnectionIdent.SybaseDataSource => StiSybaseConnector.Get(connectionString), 
			StiConnectionIdent.TeradataDataSource => StiTeradataConnector.Get(connectionString), 
			StiConnectionIdent.VistaDbDataSource => StiVistaDbConnector.Get(connectionString), 
			StiConnectionIdent.UniversalDevartDataSource => StiUniversalDevartConnector.Get(connectionString), 
			StiConnectionIdent.ODataDataSource => StiODataConnector.Get(connectionString), 
			_ => null, 
		};
	}

	protected StiSqlDataConnector(string connectionString)
	{
		ConnectionString = connectionString;
	}
}

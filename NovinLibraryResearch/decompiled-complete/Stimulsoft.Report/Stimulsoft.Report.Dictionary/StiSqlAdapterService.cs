using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dictionary;

public class StiSqlAdapterService : StiDataStoreAdapterService
{
	public override string ServiceName => string.Format(StiLocalization.Get("Adapters", "AdapterConnection"), ((StiDataConnector)CreateConnector()).Name);

	public override string GetDataCategoryName(StiData data)
	{
		return data.Name;
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Invalid comparison between Unknown and I4
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		StiSqlSource stiSqlSource = dataSource as StiSqlSource;
		try
		{
			if (!string.IsNullOrEmpty(stiSqlSource.SqlCommand))
			{
				DbConnection dbConnection = data.ViewData as DbConnection;
				StiSqlDataConnector val = CreateConnector(data);
				if (data.Data != null && data.Data.GetType() == val.ConnectionType)
				{
					OpenConnection(dbConnection, data, dataSource.Dictionary);
					bool flag = stiSqlSource.Type == StiSqlSourceType.StoredProcedure;
					if (flag)
					{
						try
						{
							using DbCommand dbCommand = val.CreateCommand(stiSqlSource.SqlCommand, dbConnection, CommandType.StoredProcedure);
							val.SetTimeout(dbCommand, stiSqlSource.CommandTimeout);
							if (dataSource.Parameters.Count > 0 && dbCommand.Parameters.Count == 0)
							{
								foreach (StiDataParameter parameter in dataSource.Parameters)
								{
									dbCommand.Parameters.Add(val.CreateParameter(parameter.Name, (parameter.Value == "") ? parameter.ParameterValue : parameter.Value, parameter.Size));
								}
							}
							else
							{
								val.DeriveParameters(dbCommand);
								if (retrieveMode == CommandBehavior.SchemaOnly)
								{
									foreach (DbParameter parameter2 in dbCommand.Parameters)
									{
										if (parameter2.Direction == ParameterDirection.Input)
										{
											dataSource.Parameters.Add(new StiDataParameter(parameter2.ParameterName, val.GetSqlType(StiDbTypeConversion.GetNetType(parameter2.DbType)), parameter2.Size));
										}
									}
								}
							}
							using DbDataReader reader = dbCommand.ExecuteReader(retrieveMode);
							using DataTable dataTable = new DataTable();
							dataTable.Load(reader);
							foreach (DataColumn column in dataTable.Columns)
							{
								stiDataColumnsCollection.Add(new StiDataColumn(column.ColumnName, column.Caption, column.DataType));
							}
						}
						catch
						{
						}
					}
					if (!flag)
					{
						if ((int)StiOptions.Engine.RetrieveColumnsMode == 2)
						{
							using DbDataAdapter dbDataAdapter = val.CreateAdapter(stiSqlSource.SqlCommand, dbConnection, CommandType.Text);
							using DataTable dataTable2 = new DataTable();
							val.SetTimeout(dbDataAdapter.SelectCommand, stiSqlSource.CommandTimeout);
							foreach (StiDataParameter parameter3 in dataSource.Parameters)
							{
								dbDataAdapter.SelectCommand.Parameters.Add(val.CreateParameter(parameter3.Name, parameter3.Value ?? parameter3.ParameterValue, parameter3.Size));
							}
							dbDataAdapter.FillSchema(dataTable2, SchemaType.Source);
							foreach (DataColumn column2 in dataTable2.Columns)
							{
								Type type = Type.GetType(column2.DataType.ToString());
								if (type == null)
								{
									type = typeof(object);
								}
								stiDataColumnsCollection.Add(new StiDataColumn(column2.ColumnName, column2.ColumnName, type));
							}
						}
						else
						{
							using DbCommand dbCommand2 = val.CreateCommand(stiSqlSource.SqlCommand, dbConnection, CommandType.Text);
							val.SetTimeout(dbCommand2, stiSqlSource.CommandTimeout);
							foreach (StiDataParameter parameter4 in dataSource.Parameters)
							{
								dbCommand2.Parameters.Add(val.CreateParameter(parameter4.Name, (parameter4.Value == "") ? parameter4.ParameterValue : parameter4.Value, parameter4.Size));
							}
							List<StiVariable> list = StiVariableAsParameterHelper.FetchAll(stiSqlSource.SqlCommand, stiSqlSource.Parameters.ToList(), stiSqlSource.Dictionary.Report);
							if (list != null && list.Count > 0)
							{
								foreach (StiVariable item in list)
								{
									try
									{
										object obj2 = stiSqlSource.Dictionary.Report[item.Name];
										if (obj2 == null)
										{
											obj2 = item.Eval(stiSqlSource.Dictionary.Report);
										}
										DbParameter value = val.CreateParameter("@" + item.Name, obj2, 0);
										dbCommand2.Parameters.Add(value);
									}
									catch
									{
									}
								}
							}
							using DbDataReader dbDataReader = dbCommand2.ExecuteReader(retrieveMode);
							using DataTable dataTable3 = dbDataReader.GetSchemaTable();
							if (dataTable3 != null)
							{
								foreach (DataRow row in dataTable3.Rows)
								{
									Type type2 = Type.GetType(row["DataType"].ToString());
									if (type2 == null)
									{
										type2 = typeof(object);
									}
									stiDataColumnsCollection.Add(new StiDataColumn(row["ColumnName"].ToString(), row["ColumnName"].ToString(), type2));
								}
							}
						}
					}
					CloseConnection(data, dbConnection);
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)this).GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return stiDataColumnsCollection;
	}

	public override StiDataParametersCollection GetParametersFromData(StiData data, StiDataSource dataSource)
	{
		StiDataParametersCollection stiDataParametersCollection = new StiDataParametersCollection();
		StiSqlSource stiSqlSource = dataSource as StiSqlSource;
		if (stiSqlSource.Type != StiSqlSourceType.StoredProcedure)
		{
			return stiDataParametersCollection;
		}
		try
		{
			if (!string.IsNullOrEmpty(stiSqlSource.SqlCommand))
			{
				DbConnection dbConnection = data.ViewData as DbConnection;
				StiSqlDataConnector val = CreateConnector(data);
				if (data.Data != null && data.Data.GetType() == val.ConnectionType)
				{
					OpenConnection(dbConnection, data, dataSource.Dictionary);
					try
					{
						using DbCommand dbCommand = val.CreateCommand(stiSqlSource.SqlCommand, dbConnection, CommandType.StoredProcedure);
						val.SetTimeout(dbCommand, stiSqlSource.CommandTimeout);
						val.DeriveParameters(dbCommand);
						if (dbCommand.Parameters.Count > 0)
						{
							foreach (DbParameter parameter in dbCommand.Parameters)
							{
								if (parameter.Direction == ParameterDirection.Input)
								{
									stiDataParametersCollection.Add(new StiDataParameter(parameter.ParameterName, val.GetSqlType(StiDbTypeConversion.GetNetType(parameter.DbType)), parameter.Size));
								}
							}
						}
					}
					catch
					{
					}
					CloseConnection(data, dbConnection);
				}
			}
		}
		catch
		{
		}
		return stiDataParametersCollection;
	}

	public override void SetDataSourceNames(StiData data, StiDataSource dataSource)
	{
		base.SetDataSourceNames(data, dataSource);
		StiSqlDataConnector val = CreateConnector(data);
		string name = (dataSource.Alias = ((StiDataConnector)val).Name.Replace(" ", "") + "Source");
		dataSource.Name = name;
	}

	public override Type GetDataSourceType()
	{
		return typeof(StiSqlSource);
	}

	public override Type[] GetDataTypes()
	{
		try
		{
			return new Type[1] { CreateConnector().CreateConnection().GetType() };
		}
		catch
		{
			return null;
		}
	}

	public override bool IsAdapterDataType(Type type)
	{
		if (type == null)
		{
			return false;
		}
		StiSqlDataConnector val = CreateConnector();
		if (val == null)
		{
			return false;
		}
		return val.TypeConnection == type.ToString();
	}

	public override void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		StiDataLeader.Disconnect(dataSource);
		if (!loadData)
		{
			dataSource.DataTable = new DataTable();
			return;
		}
		StiSqlSource stiSqlSource = dataSource as StiSqlSource;
		string nameInSource = stiSqlSource.NameInSource.ToLowerInvariant();
		IEnumerable<StiData> enumerable = from d in dataSource.Dictionary.DataStore.ToList()
			where d.Name.ToLowerInvariant() == nameInSource
			select d;
		foreach (StiData item in enumerable)
		{
			try
			{
				StiSqlDataConnector val = CreateConnector(item);
				if (item.Data == null || item.Data.GetType() != val.ConnectionType)
				{
					continue;
				}
				DbConnection dbConnection = item.ViewData as DbConnection;
				OpenConnection(dbConnection, item, dataSource.Dictionary);
				CommandType commandType = ((stiSqlSource.Type != StiSqlSourceType.StoredProcedure) ? CommandType.Text : CommandType.StoredProcedure);
				stiSqlSource.DataAdapter = val.CreateAdapter(stiSqlSource.SqlCommand, dbConnection, commandType);
				foreach (StiDataParameter parameter in stiSqlSource.Parameters)
				{
					IDbDataParameter dbDataParameter = val.CreateParameter(parameter.Name, parameter.Type, parameter.Size);
					if (dbDataParameter.DbType == DbType.Decimal)
					{
						dbDataParameter.Precision = (byte)parameter.Size;
					}
					stiSqlSource.DataAdapter.SelectCommand.Parameters.Add(dbDataParameter);
				}
				List<StiVariable> list = StiVariableAsParameterHelper.FetchAll(stiSqlSource.SqlCommand, stiSqlSource.Parameters.ToList(), dictionary.Report);
				if (list != null && list.Count > 0)
				{
					foreach (StiVariable item2 in list)
					{
						try
						{
							object obj = dictionary.Report[item2.Name];
							if (obj == null)
							{
								obj = item2.Eval(dictionary.Report);
							}
							DbParameter value = val.CreateParameter("@" + item2.Name, obj, 0);
							stiSqlSource.DataAdapter.SelectCommand.Parameters.Add(value);
						}
						catch
						{
						}
					}
				}
				if (stiSqlSource.Type == StiSqlSourceType.StoredProcedure && this is StiOleDbAdapterService)
				{
					using DbCommand dbCommand = val.CreateCommand("set nocount on", dbConnection, CommandType.Text);
					dbCommand.ExecuteNonQuery();
				}
				StiDataLeader.RetrieveData(stiSqlSource, !loadData);
				break;
			}
			catch (Exception e)
			{
				StiLogService.Write(((object)this).GetType(), e);
				if (!StiOptions.Engine.HideExceptions)
				{
					throw;
				}
			}
		}
	}

	public virtual StiSqlDataConnector CreateConnector(StiData data)
	{
		if (data == null)
		{
			return CreateConnector();
		}
		return CreateConnector((data.ViewData as DbConnection)?.ConnectionString);
	}

	public virtual StiSqlDataConnector CreateConnector(string connectionString = "")
	{
		return (StiSqlDataConnector)(object)StiMsSqlConnector.Get(connectionString);
	}

	protected virtual void OpenConnection(IDbConnection connection)
	{
		OpenConnection(connection, null, null);
	}

	protected virtual void OpenConnection(IDbConnection connection, StiData data, StiDictionary dictionary)
	{
		StiSqlDatabase stiSqlDatabase = ((data != null && dictionary != null) ? (dictionary.Databases.ToList().FirstOrDefault((StiDatabase d) => d.Name.ToLowerInvariant() == data.Name.ToLowerInvariant()) as StiSqlDatabase) : null);
		if (stiSqlDatabase != null && stiSqlDatabase.PromptUserNameAndPassword)
		{
			string connectionString = connection.ConnectionString;
			string text = null;
			string text2 = null;
			if (StiDictionary.CacheUserNamesAndPasswords != null && StiDictionary.CacheUserNamesAndPasswords[connection.ConnectionString] != null)
			{
				lock (StiDictionary.CacheUserNamesAndPasswords)
				{
					StiUserNameAndPassword stiUserNameAndPassword = StiDictionary.CacheUserNamesAndPasswords[connection.ConnectionString] as StiUserNameAndPassword;
					text = stiUserNameAndPassword.UserName;
					text2 = stiUserNameAndPassword.Password;
				}
			}
			else if (!dictionary.Report.IsDesigning)
			{
				StiUserNameAndPassword stiUserNameAndPassword2 = StiDataEditorsHelper.Get().PromptUserNameAndPassword();
				if (stiUserNameAndPassword2 != null)
				{
					text = stiUserNameAndPassword2.UserName;
					text2 = stiUserNameAndPassword2.Password;
					if (stiUserNameAndPassword2.CacheData)
					{
						if (StiDictionary.CacheUserNamesAndPasswords == null)
						{
							StiDictionary.CacheUserNamesAndPasswords = new Hashtable();
						}
						lock (StiDictionary.CacheUserNamesAndPasswords)
						{
							StiDictionary.CacheUserNamesAndPasswords[connection.ConnectionString] = stiUserNameAndPassword2;
						}
					}
				}
			}
			if (text != null && text2 != null)
			{
				connectionString += stiSqlDatabase.MapUserNameAndPassword(text, text2);
				if (connection.State != 0)
				{
					connection.Close();
				}
				connection.ConnectionString = connectionString;
			}
		}
		if (connection.State == ConnectionState.Closed)
		{
			connection.Open();
		}
	}

	protected virtual void CloseConnection(StiData data, IDbConnection connection)
	{
		if (data.OriginalConnectionState is ConnectionState)
		{
			if ((ConnectionState)data.OriginalConnectionState != ConnectionState.Open)
			{
				connection.Close();
			}
		}
		else if (connection.State != 0)
		{
			connection.Close();
		}
	}

	public virtual string TestConnection(string connectionString)
	{
		try
		{
			StiSqlDataConnector val = CreateConnector(connectionString);
			StiTestConnectionResult val2 = val.TestConnection();
			return val2.Success ? Loc.Get("DesignerFx", "ConnectionSuccessfull") : val2.Notice;
		}
		catch (Exception ex)
		{
			return Loc.Get("DesignerFx", "ConnectionError") + ": " + ex.Message;
		}
	}

	public virtual void CreateConnectionInDataStore(StiDictionary dictionary, StiSqlDatabase database)
	{
		try
		{
			if (database.Name != null)
			{
				StiData stiData = dictionary.DataStore.ToList().FirstOrDefault((StiData d) => d.Name != null && d.Name.ToLowerInvariant() == database.Name.ToLowerInvariant());
				if (stiData != null)
				{
					dictionary.DataStore.Remove(stiData);
				}
				StiSqlDataConnector val = CreateConnector(database.ConnectionString);
				dictionary.DataStore.Add(new StiData(database.Name, val.CreateConnection())
				{
					IsReportData = true
				});
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)this).GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}
}

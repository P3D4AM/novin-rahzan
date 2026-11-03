using System;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dictionary;

public abstract class StiNoSqlAdapterService : StiDataStoreAdapterService
{
	public override string GetDataCategoryName(StiData data)
	{
		return data.Name;
	}

	public override Type[] GetDataTypes()
	{
		try
		{
			return new Type[1] { ((object)CreateConnector()).GetType() };
		}
		catch
		{
			return null;
		}
	}

	public abstract StiNoSqlDataConnector CreateConnector(string connectionString = null);

	public virtual string TestConnection(string connectionString)
	{
		try
		{
			StiNoSqlDataConnector val = CreateConnector(connectionString);
			StiTestConnectionResult val2 = val.TestConnection();
			if (val2.Success)
			{
				return StiLocalization.Get("DesignerFx", "ConnectionSuccessfull");
			}
			return val2.Notice;
		}
		catch (Exception ex)
		{
			return StiLocalization.Get("DesignerFx", "ConnectionError") + ": " + ex.Message;
		}
	}

	public virtual void CreateConnectionInDataStore(StiDictionary dictionary, StiNoSqlDatabase database)
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
				StiNoSqlDataConnector data = CreateConnector(database.ConnectionString);
				dictionary.DataStore.Add(new StiData(database.Name, data)
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

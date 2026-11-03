using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiExcelDatabaseConverter))]
public class StiExcelDatabase : StiFileDatabase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiExcelDatabase;

	[StiSerializable]
	[StiOrder(350)]
	[DefaultValue(true)]
	public bool FirstRowIsHeader { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "FirstRowIsHeader", FirstRowIsHeader, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "FirstRowIsHeader")
			{
				FirstRowIsHeader = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public override StiDatabase CreateNew()
	{
		return new StiExcelDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiExcelConnector.Get();
	}

	public override StiDatabaseInformation GetDatabaseInformation(StiReport report)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		DataSet dataSet = null;
		try
		{
			Data val = StiUniversalDataLoader.LoadSingle(report, base.PathData);
			if (val != null && val.Array != null)
			{
				StiDataSchema val2 = StiDataLeader.RetrieveSchema(CreateFileConnector(), (StiFileDataOptions)new StiExcelOptions(val.Array, FirstRowIsHeader));
				if (val2 != null)
				{
					dataSet = val2.GetDataSet();
				}
			}
		}
		catch (Exception)
		{
			if (base.AllowException)
			{
				throw;
			}
		}
		if (dataSet != null)
		{
			return new StiDatabaseInformation(dataSet.Tables);
		}
		return null;
	}

	public override DialogResult Edit(StiDictionary dictionary, bool newDatabase)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return StiDataEditorsHelper.Get().ExcelDatabaseEdit(this, dictionary, newDatabase);
	}

	public override Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		DialogResult dialogResult = StiDataEditorsHelper.Get().ExcelDatabaseEdit(this, dictionary, newDatabase);
		return Task<DialogResult>.Factory.StartNew(() => dialogResult);
	}

	public override void RegData(StiDictionary dictionary, bool loadData)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		DataSet dataSet = null;
		try
		{
			Data val = StiUniversalDataLoader.LoadSingle(dictionary.Report, ParsePath(base.PathData, dictionary.Report));
			if (val != null && val.Array != null)
			{
				dataSet = StiDataAdapterHelper.Fill(dictionary, StiDataLeader.GetDataSet(CreateFileConnector(), (StiFileDataOptions)new StiExcelOptions(val.Array, FirstRowIsHeader)), this);
			}
		}
		catch (Exception e)
		{
			if (!StiRenderingMessagesHelper.WriteConnectionException(dictionary, base.Name, e))
			{
				throw;
			}
		}
		RegDataSetInDataStore(dictionary, dataSet);
	}

	public StiExcelDatabase()
	{
		FirstRowIsHeader = true;
	}

	public StiExcelDatabase(string name, string pathData)
		: base(name, pathData)
	{
		base.PathData = pathData;
		FirstRowIsHeader = true;
	}

	public StiExcelDatabase(string name, string pathData, string key)
		: base(name, pathData, key)
	{
		base.PathData = pathData;
		FirstRowIsHeader = true;
	}

	public StiExcelDatabase(string name, string pathData, string key, bool firstRowIsHeader)
		: base(name, pathData, key)
	{
		base.PathData = pathData;
		FirstRowIsHeader = firstRowIsHeader;
	}
}

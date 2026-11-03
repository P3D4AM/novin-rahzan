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

[TypeConverter(typeof(StiGisDatabaseConverter))]
public class StiGisDatabase : StiFileDatabase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiGisDatabase;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a list separator.")]
	[DefaultValue("|")]
	[Browsable(false)]
	public string Separator { get; set; } = "|";


	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public StiGisDataType DataType { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Separator", Separator);
		JsonExtensions.AddPropertyEnum(val, "DataType", (Enum)(object)DataType, (Enum)(object)(StiGisDataType)0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Separator"))
			{
				if (name == "DataType")
				{
					DataType = StiJsonReportObjectHelper.DeserializeEnum<StiGisDataType>(item);
				}
			}
			else
			{
				Separator = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public override StiDatabase CreateNew()
	{
		return new StiGisDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiGisConnector.Get();
	}

	public override StiDatabaseInformation GetDatabaseInformation(StiReport report)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		DataSet dataSet = null;
		try
		{
			Data val = StiUniversalDataLoader.LoadSingle(report, ParsePath(base.PathData, report));
			if (val != null && val.Array != null)
			{
				dataSet = (((int)DataType == 0) ? StiGisToDataSetConverter.GetDataSetFromWkt(val.Array, Separator) : StiGisToDataSetConverter.GetDataSetFromGeoJson(val.Array));
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
		return StiDataEditorsHelper.Get().GisDatabaseEdit(this, dictionary, newDatabase);
	}

	public override Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		DialogResult dialogResult = StiDataEditorsHelper.Get().GisDatabaseEdit(this, dictionary, newDatabase);
		return Task<DialogResult>.Factory.StartNew(() => dialogResult);
	}

	public override void RegData(StiDictionary dictionary, bool loadData)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		DataSet dataSet = null;
		try
		{
			Data val = StiUniversalDataLoader.LoadSingle(dictionary.Report, ParsePath(base.PathData, dictionary.Report));
			if (val != null && val.Array != null)
			{
				dataSet = (((int)DataType == 0) ? StiGisToDataSetConverter.GetDataSetFromWkt(val.Array, Separator) : StiGisToDataSetConverter.GetDataSetFromGeoJson(val.Array));
				dataSet = StiDataAdapterHelper.Fill(dictionary, dataSet, this);
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

	public StiGisDatabase()
	{
	}

	public StiGisDatabase(string name, string pathData)
		: base(name, pathData)
	{
		base.PathData = pathData;
	}

	public StiGisDatabase(string name, string pathData, string key)
		: base(name, pathData, key)
	{
		base.PathData = pathData;
	}

	public StiGisDatabase(string name, string pathData, string key, StiGisDataType dataType)
		: base(name, pathData, key)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		base.PathData = pathData;
		DataType = dataType;
	}
}

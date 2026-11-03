using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiJsonDatabaseConverter))]
public class StiJsonDatabase : StiFileDatabase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiJsonDatabase;

	[StiSerializable]
	[Description("Specifies direction of the relation processing.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public StiRelationDirection RelationDirection { get; set; }

	[Description("List of headers used for http requests to load data.")]
	public NameValueCollection Headers { get; protected set; } = new NameValueCollection();


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "RelationDirection", (Enum)(object)RelationDirection, (Enum)(object)(StiRelationDirection)0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "RelationDirection")
			{
				RelationDirection = StiJsonReportObjectHelper.DeserializeEnum<StiRelationDirection>(item);
			}
		}
	}

	public override StiDatabase CreateNew()
	{
		return new StiJsonDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiJsonConnector.Get();
	}

	public override StiDatabaseInformation GetDatabaseInformation(StiReport report)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		DataSet dataSet = null;
		try
		{
			Data val = StiUniversalDataLoader.LoadSingle(report, ParsePath(base.PathData, report), Headers);
			if (val != null && val.Array != null)
			{
				dataSet = (((int)StiBaseOptions.DefaultJsonConverterVersion == 1) ? StiJsonToDataSetConverterV2.GetDataSet(val.Array, RelationDirection) : StiJsonToDataSetConverter.GetDataSet(val.Array));
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
			return new StiDatabaseInformation(dataSet.Tables)
			{
				Relations = dataSet.Relations.Cast<DataRelation>().ToList()
			};
		}
		return null;
	}

	public override DialogResult Edit(StiDictionary dictionary, bool newDatabase)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return StiDataEditorsHelper.Get().JsonDatabaseEdit(this, dictionary, newDatabase);
	}

	public override Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		DialogResult dialogResult = StiDataEditorsHelper.Get().JsonDatabaseEdit(this, dictionary, newDatabase);
		return Task<DialogResult>.Factory.StartNew(() => dialogResult);
	}

	public override void RegData(StiDictionary dictionary, bool loadData)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		DataSet dataSet = null;
		try
		{
			Data val = StiUniversalDataLoader.LoadSingle(dictionary.Report, ParsePath(base.PathData, dictionary.Report), Headers);
			if (val != null && val.Array != null)
			{
				dataSet = StiDataAdapterHelper.Fill(dictionary, ((int)StiBaseOptions.DefaultJsonConverterVersion == 1) ? StiJsonToDataSetConverterV2.GetDataSet(val.Array, RelationDirection) : StiJsonToDataSetConverter.GetDataSet(val.Array), this);
			}
		}
		catch (Exception e)
		{
			if (!StiRenderingMessagesHelper.WriteConnectionException(dictionary, base.Name, e) || base.AllowException)
			{
				throw;
			}
		}
		RegDataSetInDataStore(dictionary, dataSet);
	}

	public StiJsonDatabase()
	{
	}

	public StiJsonDatabase(string name, string pathData)
		: base(name, pathData)
	{
		base.PathData = pathData;
	}

	public StiJsonDatabase(string name, string pathData, string key)
		: base(name, pathData, key)
	{
		base.PathData = pathData;
	}
}

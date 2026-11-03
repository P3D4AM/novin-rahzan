using System;
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
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiXmlDatabaseConverter))]
public class StiXmlDatabase : StiFileDatabase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiXmlDatabase;

	[StiSerializable]
	[Description("Specifies direction of the relation processing.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public StiRelationDirection RelationDirection { get; set; }

	[StiSerializable]
	[Description("Gets or sets a path to the xml schema.")]
	[StiOrder(400)]
	public string PathSchema { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(typeof(StiXmlType), "AdoNetXml")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiOrder(600)]
	public StiXmlType XmlType { get; set; }

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[5]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.PathData(),
			propertiesHelper.PathSchema(),
			propertiesHelper.XmlType()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "PathSchema", PathSchema);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "PathData", base.PathData);
		JsonExtensions.AddPropertyEnum(val, "XmlType", (Enum)XmlType, (Enum)StiXmlType.AdoNetXml);
		JsonExtensions.AddPropertyEnum(val, "RelationDirection", (Enum)(object)RelationDirection, (Enum)(object)(StiRelationDirection)0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "PathSchema":
				PathSchema = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "PathData":
				base.PathData = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "XmlType":
				XmlType = StiJsonReportObjectHelper.DeserializeEnum<StiXmlType>(item);
				break;
			case "RelationDirection":
				RelationDirection = StiJsonReportObjectHelper.DeserializeEnum<StiRelationDirection>(item);
				break;
			}
		}
	}

	public override StiDatabase CreateNew()
	{
		return new StiXmlDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiXmlConnector.Get();
	}

	public override StiDatabaseInformation GetDatabaseInformation(StiReport report)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		DataSet dataSet = null;
		try
		{
			if (XmlType == StiXmlType.AdoNetXml)
			{
				Data val = StiUniversalDataLoader.LoadSingle(report, PathSchema);
				Data val2 = StiUniversalDataLoader.LoadSingle(report, base.PathData);
				dataSet = new DataSet
				{
					EnforceConstraints = false
				};
				DataSetExtensions.Read(dataSet, (val != null) ? val.Array : null, (val2 != null) ? val2.Array : null);
			}
			else
			{
				Data val3 = StiUniversalDataLoader.LoadSingle(report, base.PathData);
				if (val3 != null && val3.Array != null)
				{
					dataSet = (((int)StiBaseOptions.DefaultJsonConverterVersion == 1) ? StiJsonToDataSetConverterV2.GetDataSetFromXml(val3.Array, RelationDirection) : StiJsonToDataSetConverter.GetDataSetFromXml(val3.Array));
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
		return StiDataEditorsHelper.Get().XmlDatabaseEdit(this, dictionary, newDatabase);
	}

	public override Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		DialogResult dialogResult = StiDataEditorsHelper.Get().XmlDatabaseEdit(this, dictionary, newDatabase);
		return Task<DialogResult>.Factory.StartNew(() => dialogResult);
	}

	public override void RegData(StiDictionary dictionary, bool loadData)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Invalid comparison between Unknown and I4
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		DataSet dataSet = null;
		try
		{
			if (XmlType == StiXmlType.AdoNetXml)
			{
				Data val = StiUniversalDataLoader.LoadSingle(dictionary.Report, ParsePath(PathSchema, dictionary.Report));
				Data val2 = StiUniversalDataLoader.LoadSingle(dictionary.Report, ParsePath(base.PathData, dictionary.Report));
				dataSet = new DataSet
				{
					EnforceConstraints = false
				};
				DataSetExtensions.Read(dataSet, (val != null) ? val.Array : null, (val2 != null) ? val2.Array : null);
				dataSet = StiDataAdapterHelper.Fill(dictionary, dataSet, this);
			}
			else
			{
				Data val3 = StiUniversalDataLoader.LoadSingle(dictionary.Report, ParsePath(base.PathData, dictionary.Report));
				if (val3 != null && val3.Array != null)
				{
					dataSet = (((int)StiBaseOptions.DefaultJsonConverterVersion == 1) ? StiJsonToDataSetConverterV2.GetDataSetFromXml(val3.Array, RelationDirection) : StiJsonToDataSetConverter.GetDataSetFromXml(val3.Array));
					dataSet = StiDataAdapterHelper.Fill(dictionary, dataSet, this);
				}
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

	public StiXmlDatabase()
	{
	}

	public StiXmlDatabase(string name, string pathData)
		: this(name, null, pathData)
	{
		XmlType = StiXmlType.Xml;
	}

	public StiXmlDatabase(string name, string pathSchema, string pathData)
		: base(name, pathData)
	{
		PathSchema = pathSchema;
	}

	public StiXmlDatabase(string name, string pathSchema, string pathData, string key)
		: base(name, pathData, key)
	{
		PathSchema = pathSchema;
	}

	public StiXmlDatabase(string name, string pathSchema, string pathData, string key, StiXmlType xmlType)
		: base(name, pathData, key)
	{
		PathSchema = pathSchema;
		XmlType = xmlType;
	}
}

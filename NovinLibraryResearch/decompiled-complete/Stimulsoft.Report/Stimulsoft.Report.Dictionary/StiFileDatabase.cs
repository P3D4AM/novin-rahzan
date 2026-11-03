using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiCsvDatabaseConverter))]
public abstract class StiFileDatabase : StiDatabase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFileDatabase;

	[StiSerializable]
	[Description("Gets or sets a path to the xml data.")]
	[StiOrder(500)]
	public string PathData { get; set; }

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.PathData()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "PathData", PathData);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "PathData")
			{
				PathData = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public async Task CreateDataSourcesAsync(StiDictionary dictionary)
	{
		await Task.Run(delegate
		{
			CreateDataSources(dictionary);
		});
	}

	public void CreateDataSources(StiDictionary dictionary)
	{
		StiDatabaseInformation databaseInformation = GetDatabaseInformation(dictionary.Report);
		List<StiDataTableSource> list = databaseInformation.Tables.Select((DataTable t) => CreateDataTableSource(dictionary, t)).ToList();
		list.ForEach(dictionary.DataSources.Add);
	}

	private StiDataTableSource CreateDataTableSource(StiDictionary dictionary, DataTable table)
	{
		string text = StiNameCreation.CreateDataSourcesName(dictionary.Report, table.TableName);
		return new StiDataTableSource
		{
			Dictionary = dictionary,
			NameInSource = base.Name + "." + table.TableName,
			Name = text,
			Alias = text,
			Columns = new StiDataColumnsCollection(table.Columns.Cast<DataColumn>().ToList())
		};
	}

	protected string ParsePath(string path, StiReport report)
	{
		if (string.IsNullOrEmpty(path) || !path.Contains("{") || !path.Contains("}"))
		{
			return path;
		}
		try
		{
			StiPage stiPage = new StiPage(report);
			StiText component = new StiText();
			stiPage.Components.Add(component);
			return StiParser.ParseTextValue(path, component) as string;
		}
		catch
		{
			return path;
		}
	}

	protected virtual void RegDataSetInDataStore(StiDictionary dictionary, DataSet dataSet)
	{
		if (dataSet == null)
		{
			return;
		}
		dataSet.DataSetName = base.Name;
		foreach (DataTable table in dataSet.Tables)
		{
			StiData stiData = new StiData(base.Name + "." + table.TableName, table)
			{
				IsReportData = true
			};
			dictionary.DataStore[stiData.Name] = stiData;
		}
	}

	public StiFileDataConnector CreateFileConnector()
	{
		StiDataConnector obj = CreateConnector();
		return (StiFileDataConnector)(object)((obj is StiFileDataConnector) ? obj : null);
	}

	public override void ApplyDatabaseInformation(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll)
	{
		Hashtable hashtable = new Hashtable();
		foreach (DataTable table in information.Tables)
		{
			StiDataTableSource stiDataTableSource = new StiDataTableSource(base.Name + "." + table.TableName, StiNameCreation.CreateName(report, table.TableName, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true));
			table.TableName = stiDataTableSource.Name;
			foreach (DataColumn column in table.Columns)
			{
				stiDataTableSource.Columns.Add(new StiDataColumn(column.ColumnName, column.DataType));
			}
			hashtable[table.TableName] = stiDataTableSource.Name;
			report.Dictionary.DataSources.Add(stiDataTableSource);
		}
		foreach (DataRelation relation in information.Relations)
		{
			IEnumerable<string> source = relation.ParentColumns.Select((DataColumn r) => r.ColumnName);
			IEnumerable<string> source2 = relation.ChildColumns.Select((DataColumn r) => r.ColumnName);
			if (!(hashtable[relation.ParentTable.TableName] is string text) || !(hashtable[relation.ChildTable.TableName] is string text2))
			{
				continue;
			}
			StiDataSource parentSource = ((!string.IsNullOrEmpty(text) && report.Dictionary.DataSources.Contains(text)) ? report.Dictionary.DataSources[text] : null);
			if (parentSource != null)
			{
				StiDataSource childSource = ((!string.IsNullOrEmpty(text2) && report.Dictionary.DataSources.Contains(text2)) ? report.Dictionary.DataSources[text2] : null);
				if (childSource != null && source.All((string c) => parentSource.Columns[c] != null) && source2.All((string c) => childSource.Columns[c] != null))
				{
					StiDataRelation stiDataRelation = new StiDataRelation(relation.RelationName, parentSource, childSource, source.ToArray(), source2.ToArray());
					report.Dictionary.Relations.Add(stiDataRelation);
					StiDataRelationSetName.SetName(stiDataRelation, report, relation.DataSet, relation.RelationName);
				}
			}
		}
	}

	public override void ApplyDatabaseInformation(StiDatabaseInformation information, StiReport report)
	{
		ApplyDatabaseInformation(information, report, null);
	}

	public StiFileDatabase()
		: this(string.Empty, string.Empty, string.Empty)
	{
	}

	public StiFileDatabase(string name, string pathData)
		: base(name)
	{
		PathData = pathData;
	}

	public StiFileDatabase(string name, string pathData, string key)
		: base(name, name, key)
	{
		PathData = pathData;
	}
}

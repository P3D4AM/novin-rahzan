using System;
using System.Collections.Generic;
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

[TypeConverter(typeof(StiCsvDatabaseConverter))]
public class StiCsvDatabase : StiFileDatabase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCsvDatabase;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(null)]
	[Description("Gets or sets a list separator.")]
	public string Separator { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0)]
	[Description("Gets or sets a code page.")]
	public int CodePage { get; set; }

	public override StiDatabase CreateNew()
	{
		return new StiCsvDatabase();
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Separator", Separator);
		JsonExtensions.AddPropertyInt(val, "CodePage", CodePage, 0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Separator"))
			{
				if (name == "CodePage")
				{
					CodePage = StiJsonReportObjectHelper.DeserializeInt(item);
				}
			}
			else
			{
				Separator = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiCsvConnector.Get();
	}

	public override StiDatabaseInformation GetDatabaseInformation(StiReport report)
	{
		DataSet dataSet = null;
		try
		{
			List<Data> list = StiUniversalDataLoader.LoadMutiple(report, base.PathData, "*.csv");
			if (list == null)
			{
				return null;
			}
			foreach (Data item in list)
			{
				try
				{
					DataTable table = StiCsvHelper.GetTable(item.Array, CodePage, Separator, 1);
					if (table != null)
					{
						if (dataSet == null)
						{
							dataSet = new DataSet
							{
								EnforceConstraints = true
							};
						}
						table.TableName = item.Name;
						table.Rows.Clear();
						dataSet.Tables.Add(table);
					}
				}
				catch
				{
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
		return StiDataEditorsHelper.Get().CsvDatabaseEdit(this, dictionary, newDatabase);
	}

	public override Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		DialogResult dialogResult = StiDataEditorsHelper.Get().CsvDatabaseEdit(this, dictionary, newDatabase);
		return Task<DialogResult>.Factory.StartNew(() => dialogResult);
	}

	public override void RegData(StiDictionary dictionary, bool loadData)
	{
		DataSet dataSet = null;
		string path = ParsePath(base.PathData, dictionary.Report);
		List<Data> list = StiUniversalDataLoader.LoadMutiple(dictionary.Report, path, "*.csv");
		if (list != null)
		{
			int num = 0;
			foreach (Data item in list)
			{
				try
				{
					DataTable dataTable = StiDataAdapterHelper.Fill(dictionary, StiCsvHelper.GetTable(item.Array, CodePage, Separator, num));
					if (dataTable != null)
					{
						if (dataSet == null)
						{
							dataSet = new DataSet
							{
								EnforceConstraints = false
							};
						}
						dataTable.TableName = item.Name;
						dataSet.Tables.Add(dataTable);
					}
				}
				catch (Exception e)
				{
					if (!StiRenderingMessagesHelper.WriteConnectionException(dictionary, base.Name, e))
					{
						throw;
					}
				}
			}
		}
		RegDataSetInDataStore(dictionary, dataSet);
	}

	public StiCsvDatabase()
	{
	}

	public StiCsvDatabase(string name, string pathData)
		: this(name, pathData, 0, null)
	{
	}

	public StiCsvDatabase(string name, string pathData, int codePage, string separator)
		: base(name, pathData)
	{
		CodePage = codePage;
		Separator = separator;
	}

	public StiCsvDatabase(string name, string pathData, int codePage, string separator, string key)
		: base(name, pathData, key)
	{
		CodePage = codePage;
		Separator = separator;
	}
}

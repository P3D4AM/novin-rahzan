using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiDBaseDatabaseConverter))]
public class StiDBaseDatabase : StiFileDatabase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiDBaseDatabase;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0)]
	[Description("Gets or sets a code page.")]
	public int CodePage { get; set; }

	public override StiDatabase CreateNew()
	{
		return new StiDBaseDatabase();
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiDBaseConnector.Get();
	}

	public override StiDatabaseInformation GetDatabaseInformation(StiReport report)
	{
		DataSet dataSet = null;
		try
		{
			List<Data> list = StiUniversalDataLoader.LoadMutiple(report, base.PathData, "*.dbf");
			if (list == null)
			{
				return null;
			}
			foreach (Data item in list)
			{
				try
				{
					DataTable table = StiDBaseHelper.GetTable(item.Array, CodePage, 1);
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
		return StiDataEditorsHelper.Get().DBaseDatabaseEdit(this, dictionary, newDatabase);
	}

	public override Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		DialogResult dialogResult = StiDataEditorsHelper.Get().DBaseDatabaseEdit(this, dictionary, newDatabase);
		return Task<DialogResult>.Factory.StartNew(() => dialogResult);
	}

	public override void RegData(StiDictionary dictionary, bool loadData)
	{
		DataSet dataSet = null;
		string path = ParsePath(base.PathData, dictionary.Report);
		List<Data> list = StiUniversalDataLoader.LoadMutiple(dictionary.Report, path, "*.dbf");
		if (list != null)
		{
			int num = 0;
			foreach (Data item in list)
			{
				try
				{
					DataTable dataTable = StiDataAdapterHelper.Fill(dictionary, StiDBaseHelper.GetTable(item.Array, CodePage, num));
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

	public StiDBaseDatabase()
	{
	}

	public StiDBaseDatabase(string name, string pathData)
		: this(name, pathData, 0)
	{
	}

	public StiDBaseDatabase(string name, string pathData, int codePage)
		: base(name, pathData)
	{
		CodePage = codePage;
	}

	public StiDBaseDatabase(string name, string pathData, int codePage, string key)
		: base(name, pathData, key)
	{
		CodePage = codePage;
	}
}

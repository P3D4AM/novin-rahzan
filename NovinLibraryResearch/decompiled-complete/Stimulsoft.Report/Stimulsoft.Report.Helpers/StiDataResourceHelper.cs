using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Helpers;

public class StiDataResourceHelper
{
	public static async Task SaveSnapshotAsync(IStiApp app)
	{
		await Task.Run(delegate
		{
			SaveSnapshot(app);
		});
	}

	public static void SaveSnapshot(IStiApp app)
	{
		try
		{
			if (app.GetDictionary() is StiDictionary stiDictionary)
			{
				List<StiResource> listResourceSnapshot = GetListResourceSnapshot(app, stiDictionary);
				listResourceSnapshot.ForEach(stiDictionary.Resources.Add);
				Hashtable relationInformation = GetRelationInformation(stiDictionary);
				ReplaceDatabase(stiDictionary, listResourceSnapshot);
				SaveRelationSnapshot(stiDictionary, relationInformation);
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private static Hashtable GetRelationInformation(StiDictionary dictionary)
	{
		Hashtable hashtable = new Hashtable();
		foreach (StiDataRelation relation in dictionary.Relations)
		{
			if (relation.ParentSource != null && relation.ChildSource != null && !DatabaseFromRecource(relation.ParentSource.GetConnection() as StiDatabase) && !DatabaseFromRecource(relation.ChildSource.GetConnection() as StiDatabase))
			{
				Tuple<string, string> key = new Tuple<string, string>(relation.ParentSource.Name, relation.ChildSource.Name);
				StiDataRelation stiDataRelation2 = relation.Clone() as StiDataRelation;
				stiDataRelation2.ParentSource = null;
				stiDataRelation2.ChildSource = null;
				hashtable[key] = stiDataRelation2;
			}
		}
		return hashtable;
	}

	private static void SaveRelationSnapshot(StiDictionary dictionary, Hashtable hashRelation)
	{
		foreach (Tuple<string, string> key in hashRelation.Keys)
		{
			string parentSourceName = key.Item1;
			string childSourceName = key.Item2;
			StiDataRelation stiDataRelation = hashRelation[key] as StiDataRelation;
			stiDataRelation.ParentSource = dictionary.DataSources.Items.FirstOrDefault((StiDataSource x) => x.Name == parentSourceName);
			stiDataRelation.ChildSource = dictionary.DataSources.Items.FirstOrDefault((StiDataSource x) => x.Name == childSourceName);
			dictionary.Relations.Add(stiDataRelation);
		}
	}

	private static void ReplaceDatabase(StiDictionary dictionary, List<StiResource> listResource)
	{
		Hashtable hashtable = new Hashtable();
		List<StiDatabase> list = new List<StiDatabase>();
		foreach (StiDatabase database in dictionary.Databases)
		{
			if (!DatabaseFromRecource(database))
			{
				list.Add(database);
			}
		}
		List<StiDataSource> list2 = new List<StiDataSource>();
		foreach (StiDataSource dataSource in dictionary.DataSources)
		{
			string categoryName = dataSource.GetCategoryName();
			if (list.Any((StiDatabase x) => x.Name == categoryName))
			{
				IEnumerable<IStiAppDataColumn> enumerable = from c in dataSource.FetchColumns()
					where c is IStiAppCalcDataColumn
					select c;
				if (enumerable.Count() > 0)
				{
					hashtable.Add(dataSource.Name, enumerable);
				}
				list2.Add(dataSource);
			}
		}
		foreach (StiDatabase item in list)
		{
			dictionary.Databases.Remove(item);
		}
		foreach (StiDataSource item2 in list2)
		{
			dictionary.DataSources.Remove(item2);
		}
		foreach (StiResource item3 in listResource)
		{
			StiFileDatabase stiFileDatabase = item3.CreateFileDatabase();
			stiFileDatabase.Name = item3.Alias;
			dictionary.Databases.Add(stiFileDatabase);
			StiDatabaseInformation databaseInformation = stiFileDatabase.GetDatabaseInformation(dictionary.Report);
			stiFileDatabase.ApplyDatabaseInformation(databaseInformation, dictionary.Report);
		}
		foreach (StiDataSource dataSource2 in dictionary.DataSources)
		{
			if (!hashtable.ContainsKey(dataSource2.Name))
			{
				continue;
			}
			IEnumerable enumerable2 = hashtable[dataSource2.Name] as IEnumerable;
			foreach (object item4 in enumerable2)
			{
				dataSource2.Columns.Add(item4 as StiCalcDataColumn);
			}
		}
	}

	private static bool DatabaseFromRecource(StiDatabase database)
	{
		if (database is StiFileDatabase stiFileDatabase)
		{
			return stiFileDatabase.PathData.StartsWith("resource://");
		}
		return false;
	}

	private static List<StiResource> GetListResourceSnapshot(IStiApp app, StiDictionary dictionary)
	{
		Hashtable hashtable = new Hashtable();
		dictionary.Connect();
		foreach (StiDataSource dataSource in dictionary.DataSources)
		{
			if (DatabaseFromRecource(dataSource.GetConnection() as StiDatabase) || dataSource is StiDataTransformation)
			{
				continue;
			}
			string categoryName = dataSource.GetCategoryName();
			DataSet dataSet = hashtable[categoryName.ToLowerInvariant()] as DataSet;
			if (dataSet == null)
			{
				dataSet = new DataSet(categoryName);
				hashtable[categoryName.ToLowerInvariant()] = dataSet;
			}
			DataTable dataTable = StiDataPicker.GetDataTable((StiDataRequestOption)3, (IStiAppDataSource)(object)dataSource);
			if (dataTable == null)
			{
				continue;
			}
			List<IStiAppDataColumn> list = (from c in dataSource.FetchColumns()
				where c is IStiAppCalcDataColumn
				select c).ToList();
			foreach (IStiAppDataColumn item in list)
			{
				dataTable.Columns.Remove(((StiCalcDataColumn)(object)item).Name);
			}
			dataSet.Tables.Add(dataTable);
		}
		dictionary.Disconnect();
		List<StiResource> list2 = new List<StiResource>();
		foreach (DataSet value in hashtable.Values)
		{
			list2.Add(DataSetToResourceXml(app, value));
		}
		return list2;
	}

	private static StiResource DataSetToResourceXml(IStiApp app, DataSet dataSet)
	{
		StiDictionary stiDictionary = app.GetDictionary() as StiDictionary;
		using MemoryStream memoryStream = new MemoryStream();
		dataSet.WriteXml(memoryStream, XmlWriteMode.WriteSchema);
		byte[] content = memoryStream.ToArray();
		return new StiResource(GetNewResourceName(dataSet.DataSetName, stiDictionary.Report), dataSet.DataSetName, StiResourceType.Xml, content);
	}

	private static string GetNewResourceName(string name, StiReport report)
	{
		if (StiNameCreation.IsResourceNameExists(report, name))
		{
			name = StiNameCreation.CreateResourceName(report, name);
			return GetNewResourceName(name, report);
		}
		return name;
	}
}

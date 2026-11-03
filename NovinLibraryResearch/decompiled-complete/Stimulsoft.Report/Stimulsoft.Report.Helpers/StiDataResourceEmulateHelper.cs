using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Helpers;

public class StiDataResourceEmulateHelper
{
	private const int rowsCount = 5;

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
			if (!DatabaseFromRecource(dataSource.GetConnection() as StiDatabase))
			{
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
		foreach (StiDataSource item4 in list2)
		{
			StiDataSource stiDataSource2 = dictionary.DataSources[item4.Name];
			if (stiDataSource2 == null)
			{
				continue;
			}
			foreach (object column in item4.Columns)
			{
				if (column is StiCalcDataColumn stiCalcDataColumn)
				{
					stiDataSource2.Columns.Add(new StiCalcDataColumn(stiCalcDataColumn.Name, stiCalcDataColumn.Alias, stiCalcDataColumn.Type, stiCalcDataColumn.Value));
				}
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
		foreach (StiDataSource dataSource in dictionary.DataSources)
		{
			if (!DatabaseFromRecource(dataSource.GetConnection() as StiDatabase))
			{
				string categoryName = dataSource.GetCategoryName();
				DataSet dataSet = hashtable[categoryName.ToLowerInvariant()] as DataSet;
				if (dataSet == null)
				{
					dataSet = new DataSet(categoryName);
					hashtable[categoryName.ToLowerInvariant()] = dataSet;
				}
				DataTable dataTable = CreateDataTableFromDataSource(dataSource);
				if (dataTable != null)
				{
					dataSet.Tables.Add(dataTable);
				}
			}
		}
		List<StiResource> list = new List<StiResource>();
		foreach (DataSet value in hashtable.Values)
		{
			list.Add(DataSetToResourceXml(app, value));
		}
		return list;
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

	private static DataTable CreateDataTableFromDataSource(StiDataSource dataSource)
	{
		DataTable dataTable = new DataTable(dataSource.Name);
		foreach (StiDataColumn column2 in dataSource.Columns)
		{
			if (!(column2 is StiCalcDataColumn) && !string.IsNullOrWhiteSpace(column2.Name) && !dataTable.Columns.Contains(column2.Name))
			{
				Type type = column2.Type;
				try
				{
					Assembly assembly = column2.Type.Assembly;
				}
				catch
				{
					type = typeof(object);
				}
				if (type == typeof(Image))
				{
					type = typeof(byte[]);
				}
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					type = Nullable.GetUnderlyingType(type);
				}
				DataColumn column = new DataColumn(column2.Name, type);
				dataTable.Columns.Add(column);
			}
		}
		for (int i = 0; i < 5; i++)
		{
			DataRow dataRow = dataTable.NewRow();
			foreach (DataColumn column3 in dataTable.Columns)
			{
				try
				{
					if (DataColumnExt.IsIntegerType(column3))
					{
						dataRow[column3] = i;
					}
					else if (DataColumnExt.IsNumericType(column3))
					{
						dataRow[column3] = 1100 * i;
					}
					else if (DataColumnExt.IsDateType(column3))
					{
						dataRow[column3] = new DateTime(2021 + i, i + 1, i + 1, i + 1, i + 1, i + 1);
					}
					else if (column3.DataType == typeof(string))
					{
						dataRow[column3] = $"String {i}";
					}
					else if (column3.DataType == typeof(char))
					{
						dataRow[column3] = (char)(65 + i);
					}
					else if (column3.DataType == typeof(bool))
					{
						dataRow[column3] = i % 2 == 1;
					}
					else if (column3.DataType == typeof(Guid))
					{
						dataRow[column3] = Guid.NewGuid().ToString().Substring(0, 32) + i.ToString("D4");
					}
					else if (column3.DataType == typeof(byte[]))
					{
						dataRow[column3] = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAASklEQVR42mI0yj7KQC5gYqAAjGqmheazU6yAiBzNEG3GOceI0oxsDx6dQMCINYUhOxKXTpzOhmvAoxMIWHBJ4Nc2msKGmGaAAAMAeJgWiHx8Cd8AAAAASUVORK5CYII=");
					}
					else if (column3.DataType == typeof(string[]))
					{
						dataRow[column3] = new string[5] { "s1", "s2", "s3", "s4", "s5" };
					}
					else if (column3.DataType == typeof(DateTime[]))
					{
						dataRow[column3] = new DateTime[3]
						{
							DateTime.Today,
							DateTime.Today.AddDays(1.0),
							DateTime.Today.AddDays(2.0)
						};
					}
					else if (column3.DataType == typeof(long[]))
					{
						dataRow[column3] = new long[5] { 1L, 2L, 3L, 4L, 5L };
					}
					else if (column3.DataType == typeof(float[]))
					{
						dataRow[column3] = new float[5] { 1.1f, 2.2f, 3.3f, 4.4f, 5.5f };
					}
					else if (column3.DataType == typeof(double[]))
					{
						dataRow[column3] = new double[5] { 1.1, 2.2, 3.3, 4.4, 5.5 };
					}
					else if (column3.DataType == typeof(decimal[]))
					{
						dataRow[column3] = new decimal[5] { 1.1m, 2.2m, 3.3m, 4.4m, 5.5m };
					}
					else if (column3.DataType == typeof(object))
					{
						dataRow[column3] = $"Object {i}";
					}
				}
				catch
				{
				}
			}
			dataTable.Rows.Add(dataRow);
		}
		return dataTable;
	}
}

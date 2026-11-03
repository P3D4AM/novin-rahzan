using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Dictionary;

[StiServiceBitmap(typeof(StiDataAdapterService), "Stimulsoft.Report.Bmp.DataAdapter.bmp")]
[StiServiceCategoryBitmap(typeof(StiDataAdapterService), "Stimulsoft.Report.Bmp.DataAdapter.bmp")]
public abstract class StiDataAdapterService : StiService
{
	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryDictionary");

	public sealed override Type ServiceType => typeof(StiDataAdapterService);

	public virtual bool IsObjectAdapter => false;

	public virtual object GetSyntaxProvider()
	{
		return null;
	}

	public virtual object GetMetadataProvider(IDbConnection connection)
	{
		return null;
	}

	public abstract string GetDataCategoryName(StiData data);

	public static StiDataAdapterService GetDataAdapter(StiDataSource dataSource)
	{
		IEnumerable<StiDataAdapterService> source = StiOptions.Services.DataAdapters.Where((StiDataAdapterService s) => ((StiService)s).ServiceEnabled);
		StiDataAdapterService stiDataAdapterService = ((dataSource is StiBusinessObjectSource) ? source.FirstOrDefault((StiDataAdapterService a) => a is StiBusinessObjectAdapterService) : null);
		if (stiDataAdapterService != null)
		{
			return stiDataAdapterService;
		}
		return source.FirstOrDefault((StiDataAdapterService a) => a.GetDataSourceType() == dataSource.GetType());
	}

	public static StiDataAdapterService GetDataAdapter(StiData data)
	{
		if (data == null || data.Data == null)
		{
			return null;
		}
		Type type = data.Data.GetType();
		if (type.BaseType != null && type.BaseType.FullName.StartsWith("System.Data.TypedTableBase"))
		{
			type = typeof(DataTable);
		}
		foreach (StiDataAdapterService item in StiOptions.Services.DataAdapters.Where((StiDataAdapterService s) => ((StiService)s).ServiceEnabled))
		{
			if (item is StiBusinessObjectAdapterService)
			{
				if (!(data.Data is DataSet) && !(data.Data is DataTable) && !(data.Data is DataView) && !(data.Data is IDbConnection) && !(data.Data is StiUserData))
				{
					return item;
				}
				continue;
			}
			try
			{
				if (item.IsAdapterDataType(type))
				{
					return item;
				}
			}
			catch
			{
			}
		}
		return null;
	}

	public static TreeNode GetDataAdapterCategories(StiDataAdapterService dataAdapter, StiDataCollection dataStore)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		SortedList<string, List<StiData>> sortedList = new SortedList<string, List<StiData>>();
		Hashtable hashtable = new Hashtable();
		TreeNode val = new TreeNode();
		foreach (StiData item in dataStore)
		{
			if (dataAdapter is StiBusinessObjectAdapterService && (item.Data is DataSet || item.Data is DataTable || item.Data is DataView || item.Data is IDbConnection || item.Data is StiUserData))
			{
				continue;
			}
			Type[] dataTypes = dataAdapter.GetDataTypes();
			if (dataTypes == null)
			{
				continue;
			}
			Type[] array = dataTypes;
			foreach (Type type in array)
			{
				if (StiTypeFinder.FindInterface(item.Data.GetType(), type) || StiTypeFinder.FindType(item.Data.GetType(), type))
				{
					string dataCategoryName = dataAdapter.GetDataCategoryName(item);
					if (!sortedList.ContainsKey(dataCategoryName))
					{
						sortedList[dataCategoryName] = new List<StiData>();
					}
					List<StiData> list = sortedList[dataCategoryName];
					if (!list.Contains(item))
					{
						list.Add(item);
					}
					hashtable[dataCategoryName] = dataAdapter;
				}
			}
		}
		foreach (string key in sortedList.Keys)
		{
			List<StiData> list2 = sortedList[key];
			if (list2.Count <= 0)
			{
				continue;
			}
			TreeNode val2 = new TreeNode(key, 0, 0)
			{
				Tag = hashtable[key]
			};
			val.Nodes.Add(val2);
			foreach (StiData item2 in list2)
			{
				TreeNode val3 = new TreeNode(item2.Name, 1, 1)
				{
					Tag = item2
				};
				val2.Nodes.Add(val3);
			}
		}
		return val;
	}

	public abstract StiDataSource Create(StiDictionary dictionary, bool addToDictionary);

	public StiDataSource Create(StiDictionary dictionary)
	{
		return Create(dictionary, addToDictionary: true);
	}

	public abstract bool New(StiDictionary dictionary, StiDataSource dataSource);

	public abstract bool Edit(StiDictionary dictionary, StiDataSource dataSource);

	public abstract Type GetDataSourceType();

	public abstract Type[] GetDataTypes();

	public virtual bool IsAdapterDataType(Type type)
	{
		if (type == null)
		{
			return false;
		}
		return GetDataTypes()?.Any((Type t) => t == type) ?? false;
	}

	public abstract StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource);

	public abstract StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode);

	public abstract StiDataParametersCollection GetParametersFromData(StiData data, StiDataSource dataSource);

	public abstract void SetDataSourceNames(StiData data, StiDataSource dataSource);

	public abstract void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData);

	public override string ToString()
	{
		return ((StiService)this).ServiceName;
	}
}

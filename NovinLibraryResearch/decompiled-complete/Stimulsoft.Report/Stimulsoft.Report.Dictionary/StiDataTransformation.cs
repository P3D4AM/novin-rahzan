using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiDataTransformationConverter))]
public class StiDataTransformation : StiDataStoreSource, IStiQueryObject, IStiRetrieval
{
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public List<StiDataSortRule> Sorts { get; set; } = new List<StiDataSortRule>();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public List<StiDataFilterRule> Filters { get; set; } = new List<StiDataFilterRule>();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public List<StiDataActionRule> Actions { get; set; } = new List<StiDataActionRule>();


	public override StiComponentId ComponentId => StiComponentId.StiDataTransformation;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "Sorts", Serialize.JObjectCollection((IEnumerable<IStiJsonReportObject>)Sorts, mode));
		JsonExtensions.AddPropertyJObject(val, "Filters", Serialize.JObjectCollection((IEnumerable<IStiJsonReportObject>)Filters, mode));
		JsonExtensions.AddPropertyJObject(val, "Actions", Serialize.JObjectCollection((IEnumerable<IStiJsonReportObject>)Actions, mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Sorts":
				Sorts.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataSortRule.LoadFromJson((JObject)p.Value));
				break;
			case "Filters":
				Filters.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataFilterRule.LoadFromJson((JObject)p.Value));
				break;
			case "Actions":
				Actions.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataActionRule.LoadFromJson((JObject)p.Value));
				break;
			}
		}
	}

	public List<string> RetrieveUsedDataNames(string group)
	{
		return StiUsedDataHelper.GetMany((IEnumerable<IStiMeter>)GetMeters());
	}

	public IEnumerable<IStiAppDataSource> GetDataSources(IEnumerable<string> dataNames)
	{
		IStiAppDictionary dictionary = GetDictionary();
		if (dictionary == null)
		{
			return null;
		}
		IEnumerable<IStiAppDataSource> enumerable = from d in dictionary.FetchDataSources()
			where !(d is StiDataTransformation) && (((IStiAppCell)d).GetKey() != GetKey() || GetKey() == null)
			select d;
		return StiDataSourcePicker.Fetch((IStiQueryObject)(object)this, (string)null, dataNames, enumerable);
	}

	public string GetKey()
	{
		return base.Key;
	}

	public override DataTable GetDataTable(bool allowConnectToData)
	{
		return RetrieveDataTable((StiDataRequestOption)(allowConnectToData ? 1 : 0));
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiDataTransformationAdapterService);
	}

	public DataTable RetrieveDataTable(StiDataRequestOption option)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (Columns.Count == 0)
		{
			return new DataTable(base.Name);
		}
		StiDataTable val = StiDataAnalyzer.Analyze((IStiQueryObject)(object)this, (string)null, (IEnumerable<IStiMeter>)GetMeters(), option, (IEnumerable<StiDataSortRule>)Sorts, (IEnumerable<StiDataFilterRule>)Filters, (IEnumerable<StiDataFilterRule>)null, (IEnumerable<StiDataActionRule>)Actions, (IEnumerable<StiDataSortRule>)null, (IEnumerable<StiDataFilterRule>)null, (IEnumerable<StiDataActionRule>)null, (IEnumerable<StiDataFilterRule>)null);
		Type[] array = (from c in Columns.ToList()
			select c.Type).ToArray();
		DataTable dataTable = StiDataTableConverter.ToNetTable(val, array);
		dataTable.TableName = base.Name;
		return dataTable;
	}

	internal void ConnectToData()
	{
		base.DataTable = RetrieveDataTable((StiDataRequestOption)0);
	}

	public List<IStiMeter> GetMeters(string group = null)
	{
		return (from c in Columns.ToList()
			where c is StiDataTransformationColumn
			select c).Cast<StiDataTransformationColumn>().Select(GetMeter).ToList();
	}

	internal IStiMeter GetMeter(StiDataTransformationColumn column)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		return (IStiMeter)(column.Mode switch
		{
			StiDataTransformationMode.Dimension => new StiDimensionTransformationMeter(column.Expression, column.Name, column.Key), 
			StiDataTransformationMode.Measure => new StiMeasureTransformationMeter(column.Expression, column.Name, column.Key), 
			_ => throw new StiTypeNotRecognizedException((object)column.Mode), 
		});
	}

	public override StiDataSource CreateNew()
	{
		return new StiDataTransformation();
	}

	public StiDataTransformation()
		: this(null, null)
	{
	}

	public StiDataTransformation(string nameInSource, string name)
		: this(nameInSource, name, StiKeyHelper.GenerateKey())
	{
		ConnectionOrder = 0;
	}

	public StiDataTransformation(string nameInSource, string name, string key)
		: base(nameInSource, name, name, key)
	{
		ConnectionOrder = 0;
		base.Key = StiKeyHelper.GetOrGeneratedKey(key);
	}
}

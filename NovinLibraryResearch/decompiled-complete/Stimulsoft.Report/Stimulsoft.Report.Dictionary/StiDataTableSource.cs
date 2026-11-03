using System;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Dictionary;

public class StiDataTableSource : StiDataStoreSource
{
	public override StiComponentId ComponentId => StiComponentId.StiDataTableSource;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "NameInSource", NameInSource);
		return val;
	}

	public override string GetCategoryName()
	{
		string nameInSourceStr = NameInSource;
		if (string.IsNullOrEmpty(nameInSourceStr))
		{
			return base.GetCategoryName();
		}
		nameInSourceStr = nameInSourceStr.ToLowerInvariant();
		if (base.Dictionary != null)
		{
			StiDatabase stiDatabase = base.Dictionary.Databases.ToList().FirstOrDefault((StiDatabase d) => d.Name.ToLowerInvariant() == nameInSourceStr);
			if (stiDatabase != null)
			{
				return stiDatabase.Name;
			}
		}
		if (StiDataAdapterService.GetDataAdapter(this) is StiDataTableAdapterService stiDataTableAdapterService)
		{
			StiData dataFromDataSource = stiDataTableAdapterService.GetDataFromDataSource(base.Dictionary, this);
			if (dataFromDataSource == null)
			{
				return base.GetCategoryName();
			}
			return stiDataTableAdapterService.GetDataCategoryName(dataFromDataSource);
		}
		return base.GetCategoryName();
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiDataTableAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiDataTableSource();
	}

	public StiDataTableSource()
		: this("", "", "")
	{
	}

	public StiDataTableSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiDataTableSource(string nameInSource, string name, string alias)
		: base(nameInSource, name, alias)
	{
	}

	public StiDataTableSource(string nameInSource, string name, string alias, string key)
		: base(nameInSource, name, alias, key)
	{
	}
}

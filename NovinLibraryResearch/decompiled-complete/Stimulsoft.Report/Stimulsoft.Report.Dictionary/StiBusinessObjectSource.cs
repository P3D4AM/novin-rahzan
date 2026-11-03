using System;

namespace Stimulsoft.Report.Dictionary;

public class StiBusinessObjectSource : StiDataTableSource
{
	public override StiComponentId ComponentId => StiComponentId.StiBusinessObjectSource;

	public override string GetCategoryName()
	{
		StiDataAdapterService dataAdapter = StiDataAdapterService.GetDataAdapter(this);
		StiData stiData = base.Dictionary.DataStore[NameInSource];
		if (stiData != null)
		{
			return dataAdapter.GetDataCategoryName(stiData);
		}
		return base.GetCategoryName();
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiBusinessObjectAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiBusinessObjectSource();
	}

	public StiBusinessObjectSource()
		: this("", "", "")
	{
	}

	public StiBusinessObjectSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiBusinessObjectSource(string nameInSource, string name, string alias)
		: base(nameInSource, name, alias)
	{
	}

	public StiBusinessObjectSource(string nameInSource, string name, string alias, string key)
		: base(nameInSource, name, alias, key)
	{
	}
}

using System;

namespace Stimulsoft.Report.Dictionary;

public class StiUserSource : StiDataStoreSource
{
	public override StiComponentId ComponentId => StiComponentId.StiUserSource;

	protected override Type GetDataAdapterType()
	{
		return typeof(StiUserAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiUserSource();
	}

	public StiUserSource()
		: this("", "", "")
	{
	}

	public StiUserSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiUserSource(string nameInSource, string name, string alias)
		: base(nameInSource, name, alias)
	{
	}

	public StiUserSource(string nameInSource, string name, string alias, string key)
		: base(nameInSource, name, alias, key)
	{
	}
}

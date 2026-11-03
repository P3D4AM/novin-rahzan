using System;

namespace Stimulsoft.Report.Dictionary;

public class StiDataViewSource : StiDataStoreSource
{
	public override StiComponentId ComponentId => StiComponentId.StiDataViewSource;

	protected override Type GetDataAdapterType()
	{
		return typeof(StiDataViewAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiDataViewSource();
	}

	public StiDataViewSource()
		: this("", "", "")
	{
	}

	public StiDataViewSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiDataViewSource(string nameInSource, string name, string alias)
		: base(nameInSource, name, alias)
	{
	}

	public StiDataViewSource(string nameInSource, string name, string alias, string key)
		: base(nameInSource, name, alias, key)
	{
	}
}

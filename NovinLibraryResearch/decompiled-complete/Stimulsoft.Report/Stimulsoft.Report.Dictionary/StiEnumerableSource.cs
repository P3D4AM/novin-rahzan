namespace Stimulsoft.Report.Dictionary;

public class StiEnumerableSource : StiBusinessObjectSource
{
	public override StiComponentId ComponentId => StiComponentId.StiEnumerableSource;

	public override StiDataSource CreateNew()
	{
		return new StiEnumerableSource();
	}

	public StiEnumerableSource()
	{
	}

	public StiEnumerableSource(string nameInSource, string name)
		: base(nameInSource, name)
	{
	}

	public StiEnumerableSource(string nameInSource, string name, string alias)
		: base(nameInSource, name, alias)
	{
	}

	public StiEnumerableSource(string nameInSource, string name, string alias, string key)
		: base(nameInSource, name, alias, key)
	{
	}
}

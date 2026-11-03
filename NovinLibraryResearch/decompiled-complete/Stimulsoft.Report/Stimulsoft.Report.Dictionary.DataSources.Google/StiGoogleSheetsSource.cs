using System;
using Stimulsoft.Report.Dictionary.Adapters.Google;

namespace Stimulsoft.Report.Dictionary.DataSources.Google;

public class StiGoogleSheetsSource : StiDataStoreSource
{
	protected override Type GetDataAdapterType()
	{
		return typeof(StiGoogleSheetsAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiGoogleSheetsSource();
	}

	public StiGoogleSheetsSource()
		: base("", "", "", null)
	{
	}

	public StiGoogleSheetsSource(string nameInSource, string name)
		: base(nameInSource, name)
	{
	}

	public StiGoogleSheetsSource(string nameInSource, string name, string alias, string key)
		: base(nameInSource, name, alias, key)
	{
	}
}

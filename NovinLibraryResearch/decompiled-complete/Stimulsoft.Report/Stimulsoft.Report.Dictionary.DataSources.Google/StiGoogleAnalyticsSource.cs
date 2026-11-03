using System;
using System.ComponentModel;
using Stimulsoft.Report.Dictionary.Adapters.Google;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary.DataSources.Google;

[TypeConverter(typeof(StiGoogleAnalyticsSourceConverter))]
public class StiGoogleAnalyticsSource : StiNoSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiGoogleAnalyticsDatabase;

	protected override Type GetDataAdapterType()
	{
		return typeof(StiGoogleAnalyticsAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiGoogleAnalyticsSource();
	}

	public StiGoogleAnalyticsSource()
		: base("", "", "", null)
	{
	}

	public StiGoogleAnalyticsSource(string nameInSource, string name)
		: base(nameInSource, name)
	{
	}

	public StiGoogleAnalyticsSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiGoogleAnalyticsSource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiGoogleAnalyticsSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiGoogleAnalyticsSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiGoogleAnalyticsSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiGoogleAnalyticsSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}

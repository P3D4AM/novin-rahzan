using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components;

public interface IStiDataSource : IStiEnumerator
{
	StiDataSource DataSource { get; }

	string DataSourceName { get; set; }

	bool IsDataSourceEmpty { get; }
}

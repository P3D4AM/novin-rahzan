using System.ComponentModel;

namespace Stimulsoft.Report.Design;

[ToolboxItem(false)]
[TypeConverter(typeof(StiReportDataSourceConverter))]
[DesignTimeVisible(false)]
public class StiReportDataSource : Component
{
	public object Item { get; set; }

	public string Name { get; set; }

	public StiReportDataSource()
		: this(string.Empty, null)
	{
	}

	public StiReportDataSource(string name, object item)
	{
		Name = name;
		Item = item;
	}
}

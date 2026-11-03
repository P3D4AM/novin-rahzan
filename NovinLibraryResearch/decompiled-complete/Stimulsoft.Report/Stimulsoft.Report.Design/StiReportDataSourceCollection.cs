using System.Collections;
using System.Globalization;

namespace Stimulsoft.Report.Design;

public class StiReportDataSourceCollection : CollectionBase
{
	public StiReportDataSource this[string name]
	{
		get
		{
			name = name.ToLower(CultureInfo.InvariantCulture);
			foreach (StiReportDataSource item in base.List)
			{
				if (item.Name.ToLower(CultureInfo.InvariantCulture) == name)
				{
					return item;
				}
			}
			return null;
		}
		set
		{
			name = name.ToLower(CultureInfo.InvariantCulture);
			for (int i = 0; i < base.List.Count; i++)
			{
				StiReportDataSource stiReportDataSource = base.List[i] as StiReportDataSource;
				if (stiReportDataSource.Name.ToLower(CultureInfo.InvariantCulture) == name)
				{
					base.List[i] = value;
					return;
				}
			}
			Add(value);
		}
	}

	public StiReport Report { get; }

	public void Add(string name, object value)
	{
		Add(new StiReportDataSource(name, value));
	}

	public void Add(StiReportDataSource item)
	{
		base.List.Add(item);
	}

	public void Remove(string name)
	{
		foreach (StiReportDataSource item in base.List)
		{
			if (item.Name == name)
			{
				base.List.Remove(item);
				break;
			}
		}
	}

	public StiReportDataSourceCollection(StiReport report)
	{
		Report = report;
	}
}

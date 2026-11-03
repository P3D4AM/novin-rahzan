using System.ComponentModel;

namespace Stimulsoft.Report.Dictionary;

public class StiBusinessObjectCategory
{
	[Browsable(false)]
	public string Category { get; private set; }

	public StiBusinessObjectCategory(string category)
	{
		Category = category;
	}
}

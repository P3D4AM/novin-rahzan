using System.Collections.Generic;

namespace Stimulsoft.Report.Dictionary;

internal class StiConnectionOrderComparer : IComparer<StiDataSource>
{
	int IComparer<StiDataSource>.Compare(StiDataSource x, StiDataSource y)
	{
		return x.ConnectionOrder.CompareTo(y.ConnectionOrder);
	}
}

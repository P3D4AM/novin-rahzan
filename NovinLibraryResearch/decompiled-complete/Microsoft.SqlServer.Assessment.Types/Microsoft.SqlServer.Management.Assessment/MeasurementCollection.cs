using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment;

public class MeasurementCollection : List<IDataRow>, IDataView, IReadOnlyList<IDataRow>, IReadOnlyCollection<IDataRow>, IEnumerable<IDataRow>, IEnumerable
{
	public MeasurementCollection()
	{
	}

	public MeasurementCollection(IEnumerable<Measurement> rows)
		: base((IEnumerable<IDataRow>)rows)
	{
	}

	public MeasurementCollection(params Measurement[] rep)
		: base((IEnumerable<IDataRow>)rep)
	{
	}
}

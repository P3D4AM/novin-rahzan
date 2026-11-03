using System.Collections.Generic;

namespace Stimulsoft.Report.Chart;

internal struct StiErrorFormatEntity
{
	public static HashSet<StiErrorFormatEntity> Hash { get; private set; } = new HashSet<StiErrorFormatEntity>();


	public string Format { get; set; }

	public object Value { get; set; }
}

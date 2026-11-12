using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal abstract class CheckRule : CheckInPlaceTransform
{
	public SqlObjectLocatorPattern Target { get; set; }

	protected CheckRule(SqlObjectLocatorPattern target, IEnumerable<IItemChange<Check>> changes)
		: base(changes)
	{
		Target = target;
	}
}

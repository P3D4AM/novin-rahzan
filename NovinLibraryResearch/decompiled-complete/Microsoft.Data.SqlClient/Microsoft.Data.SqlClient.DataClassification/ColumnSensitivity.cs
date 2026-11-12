using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Data.SqlClient.DataClassification;

public sealed class ColumnSensitivity
{
	public ReadOnlyCollection<SensitivityProperty> SensitivityProperties { get; private set; }

	public ColumnSensitivity(IList<SensitivityProperty> sensitivityProperties)
	{
		SensitivityProperties = new ReadOnlyCollection<SensitivityProperty>(sensitivityProperties);
	}
}

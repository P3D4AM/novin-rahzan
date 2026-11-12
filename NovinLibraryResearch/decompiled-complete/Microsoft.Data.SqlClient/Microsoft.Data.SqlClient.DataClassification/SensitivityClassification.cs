using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Data.SqlClient.DataClassification;

public sealed class SensitivityClassification
{
	public ReadOnlyCollection<Label> Labels { get; private set; }

	public ReadOnlyCollection<InformationType> InformationTypes { get; private set; }

	public SensitivityRank SensitivityRank { get; private set; }

	public ReadOnlyCollection<ColumnSensitivity> ColumnSensitivities { get; private set; }

	public SensitivityClassification(IList<Label> labels, IList<InformationType> informationTypes, IList<ColumnSensitivity> columnSensitivity, SensitivityRank sensitivityRank = SensitivityRank.NOT_DEFINED)
	{
		Labels = new ReadOnlyCollection<Label>(labels);
		InformationTypes = new ReadOnlyCollection<InformationType>(informationTypes);
		ColumnSensitivities = new ReadOnlyCollection<ColumnSensitivity>(columnSensitivity);
		SensitivityRank = sensitivityRank;
	}
}

namespace Microsoft.Data.SqlClient.DataClassification;

public sealed class SensitivityProperty
{
	public Label Label { get; private set; }

	public InformationType InformationType { get; private set; }

	public SensitivityRank SensitivityRank { get; private set; }

	public SensitivityProperty(Label label, InformationType informationType, SensitivityRank sensitivityRank = SensitivityRank.NOT_DEFINED)
	{
		Label = label;
		InformationType = informationType;
		SensitivityRank = sensitivityRank;
	}
}

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

public class OrderedScalarDifference : ScalarDifference
{
	private readonly int sourceIndex;

	private readonly int targetIndex;

	public int SourceIndex => sourceIndex;

	public int TargetIndex => targetIndex;

	internal OrderedScalarDifference(string propertyName, object sourceValue, int sourceIndex, object targetValue, int targetIndex)
		: base(propertyName, sourceValue, targetValue)
	{
		this.sourceIndex = sourceIndex;
		this.targetIndex = targetIndex;
	}
}

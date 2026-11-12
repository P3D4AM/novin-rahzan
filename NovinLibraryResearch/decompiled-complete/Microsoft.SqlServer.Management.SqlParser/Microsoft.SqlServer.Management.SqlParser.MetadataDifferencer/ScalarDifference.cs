namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

public class ScalarDifference : PropertyDifference
{
	private readonly object sourceValue;

	private readonly object targetValue;

	public object SourceValue => sourceValue;

	public object TargetValue => targetValue;

	internal ScalarDifference(string propertyName, object sourceValue, object targetValue)
		: base(propertyName)
	{
		this.sourceValue = sourceValue;
		this.targetValue = targetValue;
	}
}

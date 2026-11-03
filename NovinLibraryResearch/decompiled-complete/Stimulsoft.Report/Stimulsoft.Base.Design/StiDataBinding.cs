namespace Stimulsoft.Base.Design;

public class StiDataBinding
{
	public StiDataBindingsCollection Collection { get; }

	public string PropertyName { get; }

	public string DisplayName { get; }

	public StiDataBinding(StiDataBindingsCollection collection, string propertyName, string displayName)
	{
		Collection = collection;
		PropertyName = propertyName;
		DisplayName = displayName;
	}
}

namespace Stimulsoft.Report;

public class StiResourceItem
{
	public object Component { get; }

	public byte[] Resource { get; }

	public string ResourceName { get; }

	public string Property { get; }

	public StiResourceItem(object component, byte[] resource, string resourceName, string property)
	{
		Component = component;
		Resource = resource;
		ResourceName = resourceName;
		Property = property;
	}
}

using System.Reflection;

namespace Stimulsoft.Base.Serializing;

public class StiReference
{
	public StiPropertyInfo PropInfo { get; set; }

	public object Object { get; set; }

	public PropertyInfo PropertyInfo { get; set; }

	public StiReference(StiPropertyInfo propInfo)
		: this(propInfo, null, null)
	{
	}

	public StiReference(StiPropertyInfo propInfo, object obj, PropertyInfo propertyInfo)
	{
		PropInfo = propInfo;
		Object = obj;
		PropertyInfo = propertyInfo;
	}
}

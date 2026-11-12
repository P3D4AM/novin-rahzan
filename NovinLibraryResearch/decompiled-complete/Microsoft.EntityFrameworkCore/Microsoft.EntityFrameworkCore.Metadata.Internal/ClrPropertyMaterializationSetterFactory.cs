namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ClrPropertyMaterializationSetterFactory : ClrPropertySetterFactory
{
	public override IClrPropertySetter Create(IPropertyBase property)
	{
		return (property as IClrPropertySetter) ?? Create(property.GetMemberInfo(forMaterialization: true, forSet: true), property);
	}
}

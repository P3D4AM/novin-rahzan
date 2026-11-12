using System;
using System.ComponentModel;
using System.Linq;

namespace Stimulsoft.Base;

[AttributeUsage(AttributeTargets.Property)]
public sealed class StiVisualStatePermissionAttribute : Attribute
{
	public StiVisualStatePermissionKind Kinds { get; set; }

	public static bool IsFromDefaultStateAllowed(ITypeDescriptorContext context)
	{
		StiVisualStatePermissionKind stiVisualStatePermissionKind = ((context.PropertyDescriptor.Attributes.Cast<Attribute>().FirstOrDefault((Attribute a) => a is StiVisualStatePermissionAttribute) is StiVisualStatePermissionAttribute stiVisualStatePermissionAttribute) ? stiVisualStatePermissionAttribute.Kinds : StiVisualStatePermissionKind.Deny);
		return stiVisualStatePermissionKind == StiVisualStatePermissionKind.AllowDefaultAndStyle;
	}

	public StiVisualStatePermissionAttribute(StiVisualStatePermissionKind kinds)
	{
		Kinds = kinds;
	}
}

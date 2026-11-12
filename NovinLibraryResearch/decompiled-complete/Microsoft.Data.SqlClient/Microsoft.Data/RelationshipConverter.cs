using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace Microsoft.Data;

internal sealed class RelationshipConverter : ExpandableObjectConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}
}

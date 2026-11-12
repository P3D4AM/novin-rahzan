using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class OriginalPropertyValues : EntryPropertyValues
{
	public OriginalPropertyValues([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry)
		: base(internalEntry)
	{
	}

	public override TValue GetValue<TValue>(string propertyName)
	{
		return InternalEntry.GetOriginalValue<TValue>(InternalEntry.EntityType.GetProperty(propertyName));
	}

	public override TValue GetValue<TValue>(IProperty property)
	{
		return InternalEntry.GetOriginalValue<TValue>(InternalEntry.EntityType.CheckPropertyBelongsToType(property));
	}

	protected override void SetValueInternal(IProperty property, object value)
	{
		InternalEntry.SetOriginalValue(property, value);
	}

	protected override object GetValueInternal(IProperty property)
	{
		return InternalEntry.GetOriginalValue(property);
	}
}

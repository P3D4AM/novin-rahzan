using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

public class MetadataTracker : IReferenceRoot<ForeignKey>
{
	private readonly Dictionary<ForeignKey, Reference<ForeignKey>> _trackedForeignKeys = new Dictionary<ForeignKey, Reference<ForeignKey>>();

	public virtual void Update([JetBrains.Annotations.NotNull] ForeignKey oldForeignKey, [JetBrains.Annotations.NotNull] ForeignKey newForeignKey)
	{
		if (_trackedForeignKeys.TryGetValue(oldForeignKey, out var value))
		{
			_trackedForeignKeys.Remove(oldForeignKey);
			value.Object = newForeignKey;
			_trackedForeignKeys.Add(newForeignKey, value);
		}
	}

	public virtual Reference<ForeignKey> Track(ForeignKey foreignKey)
	{
		if (_trackedForeignKeys.TryGetValue(foreignKey, out var value))
		{
			value.IncreaseReferenceCount();
			return value;
		}
		value = new Reference<ForeignKey>(foreignKey, this);
		_trackedForeignKeys.Add(foreignKey, value);
		return value;
	}

	void IReferenceRoot<ForeignKey>.Release(Reference<ForeignKey> foreignKeyReference)
	{
		_trackedForeignKeys.Remove(foreignKeyReference.Object);
	}
}

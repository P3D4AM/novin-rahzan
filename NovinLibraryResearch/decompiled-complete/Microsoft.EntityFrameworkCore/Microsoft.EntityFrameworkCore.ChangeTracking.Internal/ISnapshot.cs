using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface ISnapshot
{
	object this[int index]
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	T GetValue<T>(int index);
}

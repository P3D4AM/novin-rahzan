using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public readonly struct MultiSnapshot : ISnapshot
{
	private readonly ISnapshot[] _snapshots;

	internal static readonly ConstructorInfo Constructor = typeof(MultiSnapshot).GetDeclaredConstructor(new Type[1] { typeof(ISnapshot[]) });

	public object this[int index]
	{
		get
		{
			return _snapshots[index / 30][index % 30];
		}
		set
		{
			_snapshots[index / 30][index % 30] = value;
		}
	}

	public MultiSnapshot([JetBrains.Annotations.NotNull] ISnapshot[] snapshots)
	{
		_snapshots = snapshots;
	}

	public T GetValue<T>(int index)
	{
		return _snapshots[index / 30].GetValue<T>(index % 30);
	}
}

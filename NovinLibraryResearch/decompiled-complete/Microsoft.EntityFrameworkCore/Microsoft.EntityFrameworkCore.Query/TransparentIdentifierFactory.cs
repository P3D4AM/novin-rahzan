using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query;

public static class TransparentIdentifierFactory
{
	private readonly struct TransparentIdentifier<TOuter, TInner>
	{
		[JetBrains.Annotations.UsedImplicitly]
		public readonly TOuter Outer;

		[JetBrains.Annotations.UsedImplicitly]
		public readonly TInner Inner;

		[JetBrains.Annotations.UsedImplicitly]
		public TransparentIdentifier(TOuter outer, TInner inner)
		{
			Outer = outer;
			Inner = inner;
		}
	}

	public static Type Create(Type outerType, Type innerType)
	{
		return typeof(TransparentIdentifier<, >).MakeGenericType(outerType, innerType);
	}
}

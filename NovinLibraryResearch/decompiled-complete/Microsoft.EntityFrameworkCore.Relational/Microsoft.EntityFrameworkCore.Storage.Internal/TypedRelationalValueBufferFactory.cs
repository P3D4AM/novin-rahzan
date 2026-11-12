using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public sealed class TypedRelationalValueBufferFactory : IRelationalValueBufferFactory
{
	private readonly Func<DbDataReader, object[]> _valueFactory;

	public TypedRelationalValueBufferFactory([JetBrains.Annotations.NotNull] RelationalValueBufferFactoryDependencies dependencies, [JetBrains.Annotations.NotNull] Func<DbDataReader, object[]> valueFactory)
	{
		_valueFactory = valueFactory;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ValueBuffer Create(DbDataReader dataReader)
	{
		object[] array = _valueFactory(dataReader);
		if (array.Length != 0)
		{
			return new ValueBuffer(array);
		}
		return ValueBuffer.Empty;
	}
}

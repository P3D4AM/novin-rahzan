using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public abstract class ReaderColumn
{
	private static readonly ConcurrentDictionary<Type, ConstructorInfo> _constructors = new ConcurrentDictionary<Type, ConstructorInfo>();

	public virtual Type Type { get; }

	public virtual bool IsNullable { get; }

	public virtual string Name { get; }

	protected ReaderColumn([JetBrains.Annotations.NotNull] Type type, bool nullable, [JetBrains.Annotations.CanBeNull] string name)
	{
		Type = type;
		IsNullable = nullable;
		Name = name;
	}

	public static ReaderColumn Create([JetBrains.Annotations.NotNull] Type type, bool nullable, [JetBrains.Annotations.CanBeNull] string columnName, [JetBrains.Annotations.NotNull] object readFunc)
	{
		return (ReaderColumn)GetConstructor(type).Invoke(new object[3] { nullable, columnName, readFunc });
	}

	private static ConstructorInfo GetConstructor(Type type)
	{
		return _constructors.GetOrAdd(type, (Type t) => typeof(ReaderColumn<>).MakeGenericType(t).GetConstructors()[0]);
	}
}
public class ReaderColumn<T> : ReaderColumn
{
	public virtual Func<DbDataReader, int[], T> GetFieldValue { get; }

	public ReaderColumn(bool nullable, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.NotNull] Func<DbDataReader, int[], T> getFieldValue)
		: base(typeof(T), nullable, name)
	{
		GetFieldValue = getFieldValue;
	}
}

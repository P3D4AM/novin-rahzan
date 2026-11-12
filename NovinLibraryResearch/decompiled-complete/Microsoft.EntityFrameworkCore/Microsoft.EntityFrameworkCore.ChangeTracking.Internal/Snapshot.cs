using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public sealed class Snapshot : ISnapshot
{
	public const int MaxGenericTypes = 30;

	public static ISnapshot Empty = new Snapshot();

	public object this[int index]
	{
		get
		{
			throw new IndexOutOfRangeException();
		}
		set
		{
			throw new IndexOutOfRangeException();
		}
	}

	private Snapshot()
	{
	}

	public T GetValue<T>(int index)
	{
		throw new IndexOutOfRangeException();
	}

	public static Delegate[] CreateReaders<TSnapshot>()
	{
		Type[] genericTypeArguments = typeof(TSnapshot).GetTypeInfo().GenericTypeArguments;
		Delegate[] array = new Delegate[genericTypeArguments.Length];
		for (int i = 0; i < genericTypeArguments.Length; i++)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(TSnapshot), "snapshot");
			array[i] = Expression.Lambda(typeof(Func<, >).MakeGenericType(typeof(TSnapshot), genericTypeArguments[i]), Expression.Field(parameterExpression, "_value" + i), parameterExpression).Compile();
		}
		return array;
	}

	public static Type CreateSnapshotType([JetBrains.Annotations.NotNull] Type[] types)
	{
		return types.Length switch
		{
			1 => typeof(Snapshot<>).MakeGenericType(types), 
			2 => typeof(Snapshot<, >).MakeGenericType(types), 
			3 => typeof(Snapshot<, , >).MakeGenericType(types), 
			4 => typeof(Snapshot<, , , >).MakeGenericType(types), 
			5 => typeof(Snapshot<, , , , >).MakeGenericType(types), 
			6 => typeof(Snapshot<, , , , , >).MakeGenericType(types), 
			7 => typeof(Snapshot<, , , , , , >).MakeGenericType(types), 
			8 => typeof(Snapshot<, , , , , , , >).MakeGenericType(types), 
			9 => typeof(Snapshot<, , , , , , , , >).MakeGenericType(types), 
			10 => typeof(Snapshot<, , , , , , , , , >).MakeGenericType(types), 
			11 => typeof(Snapshot<, , , , , , , , , , >).MakeGenericType(types), 
			12 => typeof(Snapshot<, , , , , , , , , , , >).MakeGenericType(types), 
			13 => typeof(Snapshot<, , , , , , , , , , , , >).MakeGenericType(types), 
			14 => typeof(Snapshot<, , , , , , , , , , , , , >).MakeGenericType(types), 
			15 => typeof(Snapshot<, , , , , , , , , , , , , , >).MakeGenericType(types), 
			16 => typeof(Snapshot<, , , , , , , , , , , , , , , >).MakeGenericType(types), 
			17 => typeof(Snapshot<, , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			18 => typeof(Snapshot<, , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			19 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			20 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			21 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			22 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			23 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			24 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			25 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			26 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			27 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			28 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			29 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			30 => typeof(Snapshot<, , , , , , , , , , , , , , , , , , , , , , , , , , , , , >).MakeGenericType(types), 
			_ => throw new IndexOutOfRangeException(), 
		};
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	private T19 _value19;

	private T20 _value20;

	private T21 _value21;

	private T22 _value22;

	private T23 _value23;

	private T24 _value24;

	private T25 _value25;

	private T26 _value26;

	private T27 _value27;

	private T28 _value28;

	private T29 _value29;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				19 => _value19, 
				20 => _value20, 
				21 => _value21, 
				22 => _value22, 
				23 => _value23, 
				24 => _value24, 
				25 => _value25, 
				26 => _value26, 
				27 => _value27, 
				28 => _value28, 
				29 => _value29, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			case 19:
				_value19 = (T19)value;
				break;
			case 20:
				_value20 = (T20)value;
				break;
			case 21:
				_value21 = (T21)value;
				break;
			case 22:
				_value22 = (T22)value;
				break;
			case 23:
				_value23 = (T23)value;
				break;
			case 24:
				_value24 = (T24)value;
				break;
			case 25:
				_value25 = (T25)value;
				break;
			case 26:
				_value26 = (T26)value;
				break;
			case 27:
				_value27 = (T27)value;
				break;
			case 28:
				_value28 = (T28)value;
				break;
			case 29:
				_value29 = (T29)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18, [JetBrains.Annotations.CanBeNull] T19 value19, [JetBrains.Annotations.CanBeNull] T20 value20, [JetBrains.Annotations.CanBeNull] T21 value21, [JetBrains.Annotations.CanBeNull] T22 value22, [JetBrains.Annotations.CanBeNull] T23 value23, [JetBrains.Annotations.CanBeNull] T24 value24, [JetBrains.Annotations.CanBeNull] T25 value25, [JetBrains.Annotations.CanBeNull] T26 value26, [JetBrains.Annotations.CanBeNull] T27 value27, [JetBrains.Annotations.CanBeNull] T28 value28, [JetBrains.Annotations.CanBeNull] T29 value29)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
		_value19 = value19;
		_value20 = value20;
		_value21 = value21;
		_value22 = value22;
		_value23 = value23;
		_value24 = value24;
		_value25 = value25;
		_value26 = value26;
		_value27 = value27;
		_value28 = value28;
		_value29 = value29;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	private T19 _value19;

	private T20 _value20;

	private T21 _value21;

	private T22 _value22;

	private T23 _value23;

	private T24 _value24;

	private T25 _value25;

	private T26 _value26;

	private T27 _value27;

	private T28 _value28;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				19 => _value19, 
				20 => _value20, 
				21 => _value21, 
				22 => _value22, 
				23 => _value23, 
				24 => _value24, 
				25 => _value25, 
				26 => _value26, 
				27 => _value27, 
				28 => _value28, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			case 19:
				_value19 = (T19)value;
				break;
			case 20:
				_value20 = (T20)value;
				break;
			case 21:
				_value21 = (T21)value;
				break;
			case 22:
				_value22 = (T22)value;
				break;
			case 23:
				_value23 = (T23)value;
				break;
			case 24:
				_value24 = (T24)value;
				break;
			case 25:
				_value25 = (T25)value;
				break;
			case 26:
				_value26 = (T26)value;
				break;
			case 27:
				_value27 = (T27)value;
				break;
			case 28:
				_value28 = (T28)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18, [JetBrains.Annotations.CanBeNull] T19 value19, [JetBrains.Annotations.CanBeNull] T20 value20, [JetBrains.Annotations.CanBeNull] T21 value21, [JetBrains.Annotations.CanBeNull] T22 value22, [JetBrains.Annotations.CanBeNull] T23 value23, [JetBrains.Annotations.CanBeNull] T24 value24, [JetBrains.Annotations.CanBeNull] T25 value25, [JetBrains.Annotations.CanBeNull] T26 value26, [JetBrains.Annotations.CanBeNull] T27 value27, [JetBrains.Annotations.CanBeNull] T28 value28)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
		_value19 = value19;
		_value20 = value20;
		_value21 = value21;
		_value22 = value22;
		_value23 = value23;
		_value24 = value24;
		_value25 = value25;
		_value26 = value26;
		_value27 = value27;
		_value28 = value28;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	private T19 _value19;

	private T20 _value20;

	private T21 _value21;

	private T22 _value22;

	private T23 _value23;

	private T24 _value24;

	private T25 _value25;

	private T26 _value26;

	private T27 _value27;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				19 => _value19, 
				20 => _value20, 
				21 => _value21, 
				22 => _value22, 
				23 => _value23, 
				24 => _value24, 
				25 => _value25, 
				26 => _value26, 
				27 => _value27, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			case 19:
				_value19 = (T19)value;
				break;
			case 20:
				_value20 = (T20)value;
				break;
			case 21:
				_value21 = (T21)value;
				break;
			case 22:
				_value22 = (T22)value;
				break;
			case 23:
				_value23 = (T23)value;
				break;
			case 24:
				_value24 = (T24)value;
				break;
			case 25:
				_value25 = (T25)value;
				break;
			case 26:
				_value26 = (T26)value;
				break;
			case 27:
				_value27 = (T27)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18, [JetBrains.Annotations.CanBeNull] T19 value19, [JetBrains.Annotations.CanBeNull] T20 value20, [JetBrains.Annotations.CanBeNull] T21 value21, [JetBrains.Annotations.CanBeNull] T22 value22, [JetBrains.Annotations.CanBeNull] T23 value23, [JetBrains.Annotations.CanBeNull] T24 value24, [JetBrains.Annotations.CanBeNull] T25 value25, [JetBrains.Annotations.CanBeNull] T26 value26, [JetBrains.Annotations.CanBeNull] T27 value27)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
		_value19 = value19;
		_value20 = value20;
		_value21 = value21;
		_value22 = value22;
		_value23 = value23;
		_value24 = value24;
		_value25 = value25;
		_value26 = value26;
		_value27 = value27;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	private T19 _value19;

	private T20 _value20;

	private T21 _value21;

	private T22 _value22;

	private T23 _value23;

	private T24 _value24;

	private T25 _value25;

	private T26 _value26;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				19 => _value19, 
				20 => _value20, 
				21 => _value21, 
				22 => _value22, 
				23 => _value23, 
				24 => _value24, 
				25 => _value25, 
				26 => _value26, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			case 19:
				_value19 = (T19)value;
				break;
			case 20:
				_value20 = (T20)value;
				break;
			case 21:
				_value21 = (T21)value;
				break;
			case 22:
				_value22 = (T22)value;
				break;
			case 23:
				_value23 = (T23)value;
				break;
			case 24:
				_value24 = (T24)value;
				break;
			case 25:
				_value25 = (T25)value;
				break;
			case 26:
				_value26 = (T26)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18, [JetBrains.Annotations.CanBeNull] T19 value19, [JetBrains.Annotations.CanBeNull] T20 value20, [JetBrains.Annotations.CanBeNull] T21 value21, [JetBrains.Annotations.CanBeNull] T22 value22, [JetBrains.Annotations.CanBeNull] T23 value23, [JetBrains.Annotations.CanBeNull] T24 value24, [JetBrains.Annotations.CanBeNull] T25 value25, [JetBrains.Annotations.CanBeNull] T26 value26)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
		_value19 = value19;
		_value20 = value20;
		_value21 = value21;
		_value22 = value22;
		_value23 = value23;
		_value24 = value24;
		_value25 = value25;
		_value26 = value26;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	private T19 _value19;

	private T20 _value20;

	private T21 _value21;

	private T22 _value22;

	private T23 _value23;

	private T24 _value24;

	private T25 _value25;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				19 => _value19, 
				20 => _value20, 
				21 => _value21, 
				22 => _value22, 
				23 => _value23, 
				24 => _value24, 
				25 => _value25, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			case 19:
				_value19 = (T19)value;
				break;
			case 20:
				_value20 = (T20)value;
				break;
			case 21:
				_value21 = (T21)value;
				break;
			case 22:
				_value22 = (T22)value;
				break;
			case 23:
				_value23 = (T23)value;
				break;
			case 24:
				_value24 = (T24)value;
				break;
			case 25:
				_value25 = (T25)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18, [JetBrains.Annotations.CanBeNull] T19 value19, [JetBrains.Annotations.CanBeNull] T20 value20, [JetBrains.Annotations.CanBeNull] T21 value21, [JetBrains.Annotations.CanBeNull] T22 value22, [JetBrains.Annotations.CanBeNull] T23 value23, [JetBrains.Annotations.CanBeNull] T24 value24, [JetBrains.Annotations.CanBeNull] T25 value25)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
		_value19 = value19;
		_value20 = value20;
		_value21 = value21;
		_value22 = value22;
		_value23 = value23;
		_value24 = value24;
		_value25 = value25;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	private T19 _value19;

	private T20 _value20;

	private T21 _value21;

	private T22 _value22;

	private T23 _value23;

	private T24 _value24;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				19 => _value19, 
				20 => _value20, 
				21 => _value21, 
				22 => _value22, 
				23 => _value23, 
				24 => _value24, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			case 19:
				_value19 = (T19)value;
				break;
			case 20:
				_value20 = (T20)value;
				break;
			case 21:
				_value21 = (T21)value;
				break;
			case 22:
				_value22 = (T22)value;
				break;
			case 23:
				_value23 = (T23)value;
				break;
			case 24:
				_value24 = (T24)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18, [JetBrains.Annotations.CanBeNull] T19 value19, [JetBrains.Annotations.CanBeNull] T20 value20, [JetBrains.Annotations.CanBeNull] T21 value21, [JetBrains.Annotations.CanBeNull] T22 value22, [JetBrains.Annotations.CanBeNull] T23 value23, [JetBrains.Annotations.CanBeNull] T24 value24)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
		_value19 = value19;
		_value20 = value20;
		_value21 = value21;
		_value22 = value22;
		_value23 = value23;
		_value24 = value24;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	private T19 _value19;

	private T20 _value20;

	private T21 _value21;

	private T22 _value22;

	private T23 _value23;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				19 => _value19, 
				20 => _value20, 
				21 => _value21, 
				22 => _value22, 
				23 => _value23, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			case 19:
				_value19 = (T19)value;
				break;
			case 20:
				_value20 = (T20)value;
				break;
			case 21:
				_value21 = (T21)value;
				break;
			case 22:
				_value22 = (T22)value;
				break;
			case 23:
				_value23 = (T23)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18, [JetBrains.Annotations.CanBeNull] T19 value19, [JetBrains.Annotations.CanBeNull] T20 value20, [JetBrains.Annotations.CanBeNull] T21 value21, [JetBrains.Annotations.CanBeNull] T22 value22, [JetBrains.Annotations.CanBeNull] T23 value23)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
		_value19 = value19;
		_value20 = value20;
		_value21 = value21;
		_value22 = value22;
		_value23 = value23;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	private T19 _value19;

	private T20 _value20;

	private T21 _value21;

	private T22 _value22;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				19 => _value19, 
				20 => _value20, 
				21 => _value21, 
				22 => _value22, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			case 19:
				_value19 = (T19)value;
				break;
			case 20:
				_value20 = (T20)value;
				break;
			case 21:
				_value21 = (T21)value;
				break;
			case 22:
				_value22 = (T22)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18, [JetBrains.Annotations.CanBeNull] T19 value19, [JetBrains.Annotations.CanBeNull] T20 value20, [JetBrains.Annotations.CanBeNull] T21 value21, [JetBrains.Annotations.CanBeNull] T22 value22)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
		_value19 = value19;
		_value20 = value20;
		_value21 = value21;
		_value22 = value22;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	private T19 _value19;

	private T20 _value20;

	private T21 _value21;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				19 => _value19, 
				20 => _value20, 
				21 => _value21, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			case 19:
				_value19 = (T19)value;
				break;
			case 20:
				_value20 = (T20)value;
				break;
			case 21:
				_value21 = (T21)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18, [JetBrains.Annotations.CanBeNull] T19 value19, [JetBrains.Annotations.CanBeNull] T20 value20, [JetBrains.Annotations.CanBeNull] T21 value21)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
		_value19 = value19;
		_value20 = value20;
		_value21 = value21;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	private T19 _value19;

	private T20 _value20;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				19 => _value19, 
				20 => _value20, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			case 19:
				_value19 = (T19)value;
				break;
			case 20:
				_value20 = (T20)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18, [JetBrains.Annotations.CanBeNull] T19 value19, [JetBrains.Annotations.CanBeNull] T20 value20)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
		_value19 = value19;
		_value20 = value20;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	private T19 _value19;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				19 => _value19, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			case 19:
				_value19 = (T19)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18, [JetBrains.Annotations.CanBeNull] T19 value19)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
		_value19 = value19;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	private T18 _value18;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				18 => _value18, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			case 18:
				_value18 = (T18)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17, [JetBrains.Annotations.CanBeNull] T18 value18)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
		_value18 = value18;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	private T17 _value17;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				17 => _value17, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			case 17:
				_value17 = (T17)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16, [JetBrains.Annotations.CanBeNull] T17 value17)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
		_value17 = value17;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	private T16 _value16;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				16 => _value16, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			case 16:
				_value16 = (T16)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15, [JetBrains.Annotations.CanBeNull] T16 value16)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
		_value16 = value16;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	private T15 _value15;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				15 => _value15, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			case 15:
				_value15 = (T15)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14, [JetBrains.Annotations.CanBeNull] T15 value15)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
		_value15 = value15;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	private T14 _value14;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				14 => _value14, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			case 14:
				_value14 = (T14)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13, [JetBrains.Annotations.CanBeNull] T14 value14)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
		_value14 = value14;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	private T13 _value13;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				13 => _value13, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			case 13:
				_value13 = (T13)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12, [JetBrains.Annotations.CanBeNull] T13 value13)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
		_value13 = value13;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	private T12 _value12;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				12 => _value12, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			case 12:
				_value12 = (T12)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11, [JetBrains.Annotations.CanBeNull] T12 value12)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
		_value12 = value12;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	private T11 _value11;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				11 => _value11, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			case 11:
				_value11 = (T11)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10, [JetBrains.Annotations.CanBeNull] T11 value11)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
		_value11 = value11;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	private T10 _value10;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				10 => _value10, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			case 10:
				_value10 = (T10)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9, [JetBrains.Annotations.CanBeNull] T10 value10)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
		_value10 = value10;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	private T9 _value9;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				9 => _value9, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			case 9:
				_value9 = (T9)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8, [JetBrains.Annotations.CanBeNull] T9 value9)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
		_value9 = value9;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	private T8 _value8;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				8 => _value8, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			case 8:
				_value8 = (T8)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7, [JetBrains.Annotations.CanBeNull] T8 value8)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
		_value8 = value8;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7, T8>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6, T7> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	private T7 _value7;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				7 => _value7, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			case 7:
				_value7 = (T7)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6, [JetBrains.Annotations.CanBeNull] T7 value7)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
		_value7 = value7;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6, T7>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5, T6> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5, T6>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	private T6 _value6;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				6 => _value6, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			case 6:
				_value6 = (T6)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5, [JetBrains.Annotations.CanBeNull] T6 value6)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
		_value6 = value6;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5, T6>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4, T5> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4, T5>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	private T5 _value5;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				5 => _value5, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			case 5:
				_value5 = (T5)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4, [JetBrains.Annotations.CanBeNull] T5 value5)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
		_value5 = value5;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4, T5>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3, T4> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3, T4>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	private T4 _value4;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				4 => _value4, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			case 4:
				_value4 = (T4)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3, [JetBrains.Annotations.CanBeNull] T4 value4)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
		_value4 = value4;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3, T4>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2, T3> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2, T3>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	private T3 _value3;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				3 => _value3, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			case 3:
				_value3 = (T3)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2, [JetBrains.Annotations.CanBeNull] T3 value3)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
		_value3 = value3;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2, T3>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1, T2> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1, T2>>();

	private T0 _value0;

	private T1 _value1;

	private T2 _value2;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				2 => _value2, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			case 2:
				_value2 = (T2)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1, [JetBrains.Annotations.CanBeNull] T2 value2)
	{
		_value0 = value0;
		_value1 = value1;
		_value2 = value2;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1, T2>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0, T1> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0, T1>>();

	private T0 _value0;

	private T1 _value1;

	public object this[int index]
	{
		get
		{
			return index switch
			{
				0 => _value0, 
				1 => _value1, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				_value0 = (T0)value;
				break;
			case 1:
				_value1 = (T1)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0, [JetBrains.Annotations.CanBeNull] T1 value1)
	{
		_value0 = value0;
		_value1 = value1;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0, T1>, T>)_valueReaders[index])(this);
	}
}
public sealed class Snapshot<T0> : ISnapshot
{
	private static readonly Delegate[] _valueReaders = Snapshot.CreateReaders<Snapshot<T0>>();

	private T0 _value0;

	public object this[int index]
	{
		get
		{
			if (index == 0)
			{
				return _value0;
			}
			throw new IndexOutOfRangeException();
		}
		set
		{
			if (index == 0)
			{
				_value0 = (T0)value;
				return;
			}
			throw new IndexOutOfRangeException();
		}
	}

	public Snapshot()
	{
	}

	public Snapshot([JetBrains.Annotations.CanBeNull] T0 value0)
	{
		_value0 = value0;
	}

	public T GetValue<T>(int index)
	{
		return ((Func<Snapshot<T0>, T>)_valueReaders[index])(this);
	}
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System;

internal static class EnumUtil
{
	[CompilerGenerated]
	private sealed class _003CGetFlags_003Ed__9<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IDisposable, IEnumerator where T : struct, IConvertible
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private T value;

		public T _003C_003E3__value;

		private IEnumerator _003C_003E7__wrap1;

		T IEnumerator<T>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetFlags_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					CheckIsEnum<T>(checkHasFlags: true);
					_003C_003E7__wrap1 = Enum.GetValues(typeof(T)).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					T flag = (T)_003C_003E7__wrap1.Current;
					if (value.IsFlagSet(flag))
					{
						_003C_003E2__current = flag;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CGetFlags_003Ed__9<T> _003CGetFlags_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFlags_003Ed__ = this;
			}
			else
			{
				_003CGetFlags_003Ed__ = new _003CGetFlags_003Ed__9<T>(0);
			}
			_003CGetFlags_003Ed__.value = _003C_003E3__value;
			return _003CGetFlags_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	public static void CheckIsEnum<T>(bool checkHasFlags = false)
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException("Type '" + typeof(T).FullName + "' is not an enum");
		}
		if (checkHasFlags && !IsFlags<T>())
		{
			throw new ArgumentException("Type '" + typeof(T).FullName + "' doesn't have the 'Flags' attribute");
		}
	}

	public static bool IsFlags<T>()
	{
		return Attribute.IsDefined(typeof(T), typeof(FlagsAttribute));
	}

	public static void CheckHasValue<T>(T value, string argName = null)
	{
		CheckIsEnum<T>();
		if (IsFlags<T>())
		{
			long num = 0L;
			foreach (T value2 in Enum.GetValues(typeof(T)))
			{
				num |= Convert.ToInt64(value2);
			}
			if ((num & Convert.ToInt64(value)) != 0L)
			{
				return;
			}
		}
		else if (Enum.IsDefined(typeof(T), value))
		{
			return;
		}
		throw new InvalidEnumArgumentException(argName ?? "value", Convert.ToInt32(value), typeof(T));
	}

	public static byte BitPosition<T>(this T flags) where T : struct, IConvertible
	{
		CheckIsEnum<T>(checkHasFlags: true);
		long num = Convert.ToInt64(flags);
		if (num == 0L)
		{
			throw new ArgumentException("The flag value is zero and has no bit position.");
		}
		double num2 = Math.Log(num, 2.0);
		if (num2 % 1.0 > 0.0)
		{
			throw new ArithmeticException("The flag value has more than a single bit set.");
		}
		return Convert.ToByte(num2);
	}

	public static bool IsFlagSet<T>(this T flags, T flag) where T : struct, IConvertible
	{
		CheckIsEnum<T>(checkHasFlags: true);
		long num = Convert.ToInt64(flag);
		return (Convert.ToInt64(flags) & num) == num;
	}

	public static bool IsValidFlagValue<T>(this T flags) where T : struct, IConvertible
	{
		CheckIsEnum<T>(checkHasFlags: true);
		long num = 0L;
		foreach (T value in Enum.GetValues(typeof(T)))
		{
			if (flags.IsFlagSet(value))
			{
				num |= Convert.ToInt64(value);
			}
		}
		return num == Convert.ToInt64(flags);
	}

	public static void SetFlags<T>(ref T flags, T flag, bool set = true) where T : struct, IConvertible
	{
		CheckIsEnum<T>(checkHasFlags: true);
		long num = Convert.ToInt64(flags);
		long num2 = Convert.ToInt64(flag);
		num = ((!set) ? (num & ~num2) : (num | num2));
		flags = (T)Enum.ToObject(typeof(T), num);
	}

	public static T SetFlags<T>(this T flags, T flag, bool set = true) where T : struct, IConvertible
	{
		T flags2 = flags;
		SetFlags(ref flags2, flag, set);
		return flags2;
	}

	public static T ClearFlags<T>(this T flags, T flag) where T : struct, IConvertible
	{
		return flags.SetFlags(flag, set: false);
	}

	[IteratorStateMachine(typeof(_003CGetFlags_003Ed__9<>))]
	public static IEnumerable<T> GetFlags<T>(this T value) where T : struct, IConvertible
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFlags_003Ed__9<T>(-2)
		{
			_003C_003E3__value = value
		};
	}

	public static T CombineFlags<T>(this IEnumerable<T> flags) where T : struct, IConvertible
	{
		CheckIsEnum<T>(checkHasFlags: true);
		long num = 0L;
		foreach (T flag in flags)
		{
			long num2 = Convert.ToInt64(flag);
			num |= num2;
		}
		return (T)Enum.ToObject(typeof(T), num);
	}

	public static string GetDescription<T>(this T value) where T : struct, IConvertible
	{
		CheckIsEnum<T>();
		string name = Enum.GetName(typeof(T), value);
		if (name != null)
		{
			FieldInfo field = typeof(T).GetField(name);
			if (field != null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)
			{
				return descriptionAttribute.Description;
			}
		}
		return null;
	}

	public static TEnum TryParse<TEnum>(string value, bool ignoreCase = false, TEnum defaultVal = default(TEnum)) where TEnum : struct, IConvertible
	{
		CheckIsEnum<TEnum>();
		try
		{
			return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
		}
		catch
		{
		}
		if (!Enum.IsDefined(typeof(TEnum), defaultVal))
		{
			Array values = Enum.GetValues(typeof(TEnum));
			if (values != null && values.Length > 0)
			{
				return (TEnum)values.GetValue(0);
			}
		}
		return defaultVal;
	}
}

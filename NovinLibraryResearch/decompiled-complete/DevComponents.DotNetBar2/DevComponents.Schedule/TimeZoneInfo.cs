using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using DevComponents.AdvTree;
using Microsoft.Win32;

namespace DevComponents.Schedule;

[Serializable]
[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public sealed class TimeZoneInfo : IEquatable<TimeZoneInfo>, ISerializable, IDeserializationCallback
{
	[Serializable]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class AdjustmentRule : IEquatable<AdjustmentRule>, ISerializable, IDeserializationCallback
	{
		private DateTime m_dateEnd;

		private DateTime m_dateStart;

		private TimeSpan m_daylightDelta;

		private TransitionTime m_daylightTransitionEnd;

		private TransitionTime m_daylightTransitionStart;

		public DateTime DateEnd => m_dateEnd;

		public DateTime DateStart => m_dateStart;

		public TimeSpan DaylightDelta => m_daylightDelta;

		public TransitionTime DaylightTransitionEnd => m_daylightTransitionEnd;

		public TransitionTime DaylightTransitionStart => m_daylightTransitionStart;

		private AdjustmentRule()
		{
		}

		private AdjustmentRule(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			m_dateStart = (DateTime)info.GetValue("DateStart", typeof(DateTime));
			m_dateEnd = (DateTime)info.GetValue("DateEnd", typeof(DateTime));
			m_daylightDelta = (TimeSpan)info.GetValue("DaylightDelta", typeof(TimeSpan));
			m_daylightTransitionStart = (TransitionTime)info.GetValue("DaylightTransitionStart", typeof(TransitionTime));
			m_daylightTransitionEnd = (TransitionTime)info.GetValue("DaylightTransitionEnd", typeof(TransitionTime));
		}

		public static AdjustmentRule CreateAdjustmentRule(DateTime dateStart, DateTime dateEnd, TimeSpan daylightDelta, TransitionTime daylightTransitionStart, TransitionTime daylightTransitionEnd)
		{
			ValidateAdjustmentRule(dateStart, dateEnd, daylightDelta, daylightTransitionStart, daylightTransitionEnd);
			AdjustmentRule adjustmentRule = new AdjustmentRule();
			adjustmentRule.m_dateStart = dateStart;
			adjustmentRule.m_dateEnd = dateEnd;
			adjustmentRule.m_daylightDelta = daylightDelta;
			adjustmentRule.m_daylightTransitionStart = daylightTransitionStart;
			adjustmentRule.m_daylightTransitionEnd = daylightTransitionEnd;
			return adjustmentRule;
		}

		public bool Equals(AdjustmentRule other)
		{
			if (other != null && m_dateStart == other.m_dateStart && m_dateEnd == other.m_dateEnd && m_daylightDelta == other.m_daylightDelta && m_daylightTransitionEnd.Equals(other.m_daylightTransitionEnd))
			{
				return m_daylightTransitionStart.Equals(other.m_daylightTransitionStart);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return m_dateStart.GetHashCode();
		}

		void IDeserializationCallback.OnDeserialization(object sender)
		{
			try
			{
				ValidateAdjustmentRule(m_dateStart, m_dateEnd, m_daylightDelta, m_daylightTransitionStart, m_daylightTransitionEnd);
			}
			catch (ArgumentException innerException)
			{
				throw new SerializationException("Serialization_InvalidData", innerException);
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("DateStart", m_dateStart);
			info.AddValue("DateEnd", m_dateEnd);
			info.AddValue("DaylightDelta", m_daylightDelta);
			info.AddValue("DaylightTransitionStart", m_daylightTransitionStart);
			info.AddValue("DaylightTransitionEnd", m_daylightTransitionEnd);
		}

		private static void ValidateAdjustmentRule(DateTime dateStart, DateTime dateEnd, TimeSpan daylightDelta, TransitionTime daylightTransitionStart, TransitionTime daylightTransitionEnd)
		{
			if (dateStart.Kind != 0)
			{
				throw new ArgumentException("Argument_DateTimeKindMustBeUnspecified", "dateStart");
			}
			if (dateEnd.Kind != 0)
			{
				throw new ArgumentException("Argument_DateTimeKindMustBeUnspecified", "dateEnd");
			}
			if (daylightTransitionStart.Equals(daylightTransitionEnd))
			{
				throw new ArgumentException("Argument_TransitionTimesAreIdentical", "daylightTransitionEnd");
			}
			if (dateStart > dateEnd)
			{
				throw new ArgumentException("Argument_OutOfOrderDateTimes", "dateStart");
			}
			if (UtcOffsetOutOfRange(daylightDelta))
			{
				throw new ArgumentOutOfRangeException("daylightDelta", daylightDelta, "ArgumentOutOfRange_UtcOffset");
			}
			if (daylightDelta.Ticks % 600000000 != 0)
			{
				throw new ArgumentException("Argument_TimeSpanHasSeconds", "daylightDelta");
			}
			if (dateStart.TimeOfDay != TimeSpan.Zero)
			{
				throw new ArgumentException("Argument_DateTimeHasTimeOfDay", "dateStart");
			}
			if (dateEnd.TimeOfDay != TimeSpan.Zero)
			{
				throw new ArgumentException("Argument_DateTimeHasTimeOfDay", "dateEnd");
			}
		}
	}

	[Serializable]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public struct TransitionTime : IEquatable<TransitionTime>, ISerializable, IDeserializationCallback
	{
		private DateTime m_timeOfDay;

		private byte m_month;

		private byte m_week;

		private byte m_day;

		private DayOfWeek m_dayOfWeek;

		private bool m_isFixedDateRule;

		public DateTime TimeOfDay => m_timeOfDay;

		public int Month => m_month;

		public int Week => m_week;

		public int Day => m_day;

		public DayOfWeek DayOfWeek => m_dayOfWeek;

		public bool IsFixedDateRule => m_isFixedDateRule;

		public override bool Equals(object obj)
		{
			if (obj is TransitionTime)
			{
				return Equals((TransitionTime)obj);
			}
			return false;
		}

		public static bool operator ==(TransitionTime left, TransitionTime right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(TransitionTime left, TransitionTime right)
		{
			return !left.Equals(right);
		}

		public bool Equals(TransitionTime other)
		{
			bool flag = m_isFixedDateRule == other.m_isFixedDateRule && m_timeOfDay == other.m_timeOfDay && m_month == other.m_month;
			if (!flag)
			{
				return flag;
			}
			if (other.m_isFixedDateRule)
			{
				return m_day == other.m_day;
			}
			if (m_week == other.m_week)
			{
				return m_dayOfWeek == other.m_dayOfWeek;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return m_month ^ (m_week << 8);
		}

		public static TransitionTime CreateFixedDateRule(DateTime timeOfDay, int month, int day)
		{
			return CreateTransitionTime(timeOfDay, month, 1, day, DayOfWeek.Sunday, isFixedDateRule: true);
		}

		public static TransitionTime CreateFloatingDateRule(DateTime timeOfDay, int month, int week, DayOfWeek dayOfWeek)
		{
			return CreateTransitionTime(timeOfDay, month, week, 1, dayOfWeek, isFixedDateRule: false);
		}

		private static TransitionTime CreateTransitionTime(DateTime timeOfDay, int month, int week, int day, DayOfWeek dayOfWeek, bool isFixedDateRule)
		{
			ValidateTransitionTime(timeOfDay, month, week, day, dayOfWeek);
			TransitionTime result = default(TransitionTime);
			result.m_isFixedDateRule = isFixedDateRule;
			result.m_timeOfDay = timeOfDay;
			result.m_dayOfWeek = dayOfWeek;
			result.m_day = (byte)day;
			result.m_week = (byte)week;
			result.m_month = (byte)month;
			return result;
		}

		private static void ValidateTransitionTime(DateTime timeOfDay, int month, int week, int day, DayOfWeek dayOfWeek)
		{
			if (timeOfDay.Kind != 0)
			{
				throw new ArgumentException("Argument_DateTimeKindMustBeUnspecified", "timeOfDay");
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", "ArgumentOutOfRange_Month");
			}
			if (day < 1 || day > 31)
			{
				throw new ArgumentOutOfRangeException("day", "ArgumentOutOfRange_Day");
			}
			if (week < 1 || week > 5)
			{
				throw new ArgumentOutOfRangeException("week", "ArgumentOutOfRange_Week");
			}
			if (dayOfWeek < DayOfWeek.Sunday || dayOfWeek > DayOfWeek.Saturday)
			{
				throw new ArgumentOutOfRangeException("dayOfWeek", "ArgumentOutOfRange_DayOfWeek");
			}
			if (timeOfDay.Year != 1 || timeOfDay.Month != 1 || timeOfDay.Day != 1 || timeOfDay.Ticks % 10000 != 0)
			{
				throw new ArgumentException("Argument_DateTimeHasTicks", "timeOfDay");
			}
		}

		void IDeserializationCallback.OnDeserialization(object sender)
		{
			try
			{
				ValidateTransitionTime(m_timeOfDay, m_month, m_week, m_day, m_dayOfWeek);
			}
			catch (ArgumentException innerException)
			{
				throw new SerializationException("Serialization_InvalidData", innerException);
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("TimeOfDay", m_timeOfDay);
			info.AddValue("Month", m_month);
			info.AddValue("Week", m_week);
			info.AddValue("Day", m_day);
			info.AddValue("RelativeDayOfWeek", m_dayOfWeek);
			info.AddValue("IsFixedDateRule", m_isFixedDateRule);
		}

		private TransitionTime(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			m_timeOfDay = (DateTime)info.GetValue("TimeOfDay", typeof(DateTime));
			m_month = (byte)info.GetValue("Month", typeof(byte));
			m_week = (byte)info.GetValue("Week", typeof(byte));
			m_day = (byte)info.GetValue("Day", typeof(byte));
			m_dayOfWeek = (DayOfWeek)info.GetValue("RelativeDayOfWeek", typeof(DayOfWeek));
			m_isFixedDateRule = (bool)info.GetValue("IsFixedDateRule", typeof(bool));
		}
	}

	private enum _2A45
	{
		_2A46,
		_2A47,
		_2A48,
		_2A49
	}

	private sealed class _2A4A
	{
		private enum _2A6F
		{
			_2A70,
			_2A71,
			_2A72,
			_2A73
		}

		private const string _2A4B = "MM:dd:yyyy";

		private const char _2A4C = '\\';

		private const string _2A4D = "\\\\";

		private const string _2A4E = "\\[";

		private const string _2A4F = "\\]";

		private const string _2A50 = "\\;";

		private const string _2A51 = "\\";

		private const int _2A52 = 64;

		private const char _2A53 = '[';

		private const string _2A54 = "[";

		private const char _2A55 = ']';

		private const string _2A56 = "]";

		private const char _2A57 = ';';

		private const string _2A58 = ";";

		private const string _2A59 = "HH:mm:ss.FFF";

		private int _2A5A;

		private string _2A5B;

		private _2A6F _2A5C;

		private _2A4A(string str)
		{
			_2A5B = str;
			_2A5C = _2A6F._2A72;
		}

		public static TimeZoneInfo _2A5D(string ᎂ)
		{
			_2A4A _2A4A = new _2A4A(ᎂ);
			string id = _2A4A._2A63(_2A5F: false);
			TimeSpan baseUtcOffset = _2A4A._2A64(_2A5F: false);
			string displayName = _2A4A._2A63(_2A5F: false);
			string standardDisplayName = _2A4A._2A63(_2A5F: false);
			string daylightDisplayName = _2A4A._2A63(_2A5F: false);
			AdjustmentRule[] adjustmentRules = _2A4A._2A5E(_2A5F: false);
			try
			{
				return CreateCustomTimeZone(id, baseUtcOffset, displayName, standardDisplayName, daylightDisplayName, adjustmentRules);
			}
			catch (ArgumentException innerException)
			{
				throw new SerializationException("Serialization_InvalidData", innerException);
			}
			catch (InvalidTimeZoneException innerException2)
			{
				throw new SerializationException("Serialization_InvalidData", innerException2);
			}
		}

		private AdjustmentRule[] _2A5E(bool _2A5F)
		{
			List<AdjustmentRule> list = new List<AdjustmentRule>(1);
			int num = 0;
			for (AdjustmentRule adjustmentRule = _2A60(_2A5F: true); adjustmentRule != null; adjustmentRule = _2A60(_2A5F: true))
			{
				list.Add(adjustmentRule);
				num++;
			}
			if (!_2A5F)
			{
				if (_2A5C == _2A6F._2A73)
				{
					throw new SerializationException("Serialization_InvalidData");
				}
				if (_2A5A < 0 || _2A5A >= _2A5B.Length)
				{
					throw new SerializationException("Serialization_InvalidData");
				}
			}
			if (num == 0)
			{
				return null;
			}
			return list.ToArray();
		}

		private AdjustmentRule _2A60(bool _2A5F)
		{
			if (_2A5C == _2A6F._2A73)
			{
				if (!_2A5F)
				{
					throw new SerializationException("Serialization_InvalidData");
				}
				return null;
			}
			if (_2A5A < 0 || _2A5A >= _2A5B.Length)
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			if (_2A5B[_2A5A] == ';')
			{
				return null;
			}
			if (_2A5B[_2A5A] != '[')
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			_2A5A++;
			DateTime dateStart = _2A61(_2A5F: false, "MM:dd:yyyy");
			DateTime dateEnd = _2A61(_2A5F: false, "MM:dd:yyyy");
			TimeSpan daylightDelta = _2A64(_2A5F: false);
			TransitionTime daylightTransitionStart = _2A65(_2A5F: false);
			TransitionTime daylightTransitionEnd = _2A65(_2A5F: false);
			if (_2A5C == _2A6F._2A73 || _2A5A >= _2A5B.Length)
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			if (_2A5B[_2A5A] != ']')
			{
				_2A6C(1);
			}
			else
			{
				_2A5A++;
			}
			AdjustmentRule result;
			try
			{
				result = AdjustmentRule.CreateAdjustmentRule(dateStart, dateEnd, daylightDelta, daylightTransitionStart, daylightTransitionEnd);
			}
			catch (ArgumentException innerException)
			{
				throw new SerializationException("Serialization_InvalidData", innerException);
			}
			if (_2A5A >= _2A5B.Length)
			{
				_2A5C = _2A6F._2A73;
				return result;
			}
			_2A5C = _2A6F._2A72;
			return result;
		}

		private DateTime _2A61(bool _2A5F, string _0EE9)
		{
			if (!DateTime.TryParseExact(_2A63(_2A5F), _0EE9, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out var result))
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			return result;
		}

		private int _2A62(bool _2A5F)
		{
			if (!int.TryParse(_2A63(_2A5F), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var result))
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			return result;
		}

		private string _2A63(bool _2A5F)
		{
			if (_2A5C == _2A6F._2A73)
			{
				if (!_2A5F)
				{
					throw new SerializationException("Serialization_InvalidData");
				}
				return null;
			}
			if (_2A5A < 0 || _2A5A >= _2A5B.Length)
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			_2A6F _2A6F = _2A6F._2A71;
			StringBuilder stringBuilder = new StringBuilder(64);
			for (int i = _2A5A; i < _2A5B.Length; i++)
			{
				switch (_2A6F)
				{
				case _2A6F._2A70:
					_2A6E(_2A5B[i]);
					stringBuilder.Append(_2A5B[i]);
					_2A6F = _2A6F._2A71;
					break;
				case _2A6F._2A71:
					switch (_2A5B[i])
					{
					case '[':
						throw new SerializationException("Serialization_InvalidData");
					case '\\':
						_2A6F = _2A6F._2A70;
						break;
					case ']':
						if (!_2A5F)
						{
							throw new SerializationException("Serialization_InvalidData");
						}
						_2A5A = i;
						_2A5C = _2A6F._2A72;
						return stringBuilder.ToString();
					case ';':
						_2A5A = i + 1;
						if (_2A5A >= _2A5B.Length)
						{
							_2A5C = _2A6F._2A73;
						}
						else
						{
							_2A5C = _2A6F._2A72;
						}
						return stringBuilder.ToString();
					case '\0':
						throw new SerializationException("Serialization_InvalidData");
					default:
						stringBuilder.Append(_2A5B[i]);
						break;
					}
					break;
				}
			}
			if (_2A6F == _2A6F._2A70)
			{
				throw new SerializationException("Serialization_InvalidEscapeSequence");
			}
			if (!_2A5F)
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			_2A5A = _2A5B.Length;
			_2A5C = _2A6F._2A73;
			return stringBuilder.ToString();
		}

		private TimeSpan _2A64(bool _2A5F)
		{
			int minutes = _2A62(_2A5F);
			try
			{
				return new TimeSpan(0, minutes, 0);
			}
			catch (ArgumentOutOfRangeException innerException)
			{
				throw new SerializationException("Serialization_InvalidData", innerException);
			}
		}

		private TransitionTime _2A65(bool _2A5F)
		{
			if (_2A5C == _2A6F._2A73 || (_2A5A < _2A5B.Length && _2A5B[_2A5A] == ']'))
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			if (_2A5A < 0 || _2A5A >= _2A5B.Length)
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			if (_2A5B[_2A5A] != '[')
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			_2A5A++;
			int num = _2A62(_2A5F: false);
			if (num != 0 && num != 1)
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			DateTime dateTime = _2A61(_2A5F: false, "HH:mm:ss.FFF");
			dateTime = new DateTime(1, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
			int month = _2A62(_2A5F: false);
			TransitionTime result;
			if (num == 1)
			{
				int day = _2A62(_2A5F: false);
				try
				{
					result = TransitionTime.CreateFixedDateRule(dateTime, month, day);
				}
				catch (ArgumentException innerException)
				{
					throw new SerializationException("Serialization_InvalidData", innerException);
				}
			}
			else
			{
				int week = _2A62(_2A5F: false);
				int dayOfWeek = _2A62(_2A5F: false);
				try
				{
					result = TransitionTime.CreateFloatingDateRule(dateTime, month, week, (DayOfWeek)dayOfWeek);
				}
				catch (ArgumentException innerException2)
				{
					throw new SerializationException("Serialization_InvalidData", innerException2);
				}
			}
			if (_2A5C == _2A6F._2A73 || _2A5A >= _2A5B.Length)
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			if (_2A5B[_2A5A] != ']')
			{
				_2A6C(1);
			}
			else
			{
				_2A5A++;
			}
			bool flag = false;
			if (_2A5A < _2A5B.Length && _2A5B[_2A5A] == ';')
			{
				_2A5A++;
				flag = true;
			}
			if (!flag && !_2A5F)
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			if (_2A5A >= _2A5B.Length)
			{
				_2A5C = _2A6F._2A73;
				return result;
			}
			_2A5C = _2A6F._2A72;
			return result;
		}

		public static string _2A66(TimeZoneInfo _2A67)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(_2A68(_2A67.Id));
			stringBuilder.Append(';');
			stringBuilder.Append(_2A68(_2A67.BaseUtcOffset.TotalMinutes.ToString(CultureInfo.InvariantCulture)));
			stringBuilder.Append(';');
			stringBuilder.Append(_2A68(_2A67.DisplayName));
			stringBuilder.Append(';');
			stringBuilder.Append(_2A68(_2A67.StandardName));
			stringBuilder.Append(';');
			stringBuilder.Append(_2A68(_2A67.DaylightName));
			stringBuilder.Append(';');
			AdjustmentRule[] adjustmentRules = _2A67.GetAdjustmentRules();
			if (adjustmentRules != null && adjustmentRules.Length > 0)
			{
				foreach (AdjustmentRule adjustmentRule in adjustmentRules)
				{
					stringBuilder.Append('[');
					stringBuilder.Append(_2A68(adjustmentRule.DateStart.ToString("MM:dd:yyyy", DateTimeFormatInfo.InvariantInfo)));
					stringBuilder.Append(';');
					stringBuilder.Append(_2A68(adjustmentRule.DateEnd.ToString("MM:dd:yyyy", DateTimeFormatInfo.InvariantInfo)));
					stringBuilder.Append(';');
					stringBuilder.Append(_2A68(adjustmentRule.DaylightDelta.TotalMinutes.ToString(CultureInfo.InvariantCulture)));
					stringBuilder.Append(';');
					_2A69(adjustmentRule.DaylightTransitionStart, stringBuilder);
					stringBuilder.Append(';');
					_2A69(adjustmentRule.DaylightTransitionEnd, stringBuilder);
					stringBuilder.Append(';');
					stringBuilder.Append(']');
				}
			}
			stringBuilder.Append(';');
			return stringBuilder.ToString();
		}

		private static string _2A68(string _06EA)
		{
			_06EA = _06EA.Replace("\\", "\\\\");
			_06EA = _06EA.Replace("[", "\\[");
			_06EA = _06EA.Replace("]", "\\]");
			return _06EA.Replace(";", "\\;");
		}

		private static void _2A69(TransitionTime _2A6A, StringBuilder _2A6B)
		{
			_2A6B.Append('[');
			_2A6B.Append((_2A6A.IsFixedDateRule ? 1 : 0).ToString(CultureInfo.InvariantCulture));
			_2A6B.Append(';');
			if (_2A6A.IsFixedDateRule)
			{
				_2A6B.Append(_2A68(_2A6A.TimeOfDay.ToString("HH:mm:ss.FFF", DateTimeFormatInfo.InvariantInfo)));
				_2A6B.Append(';');
				_2A6B.Append(_2A68(_2A6A.Month.ToString(CultureInfo.InvariantCulture)));
				_2A6B.Append(';');
				_2A6B.Append(_2A68(_2A6A.Day.ToString(CultureInfo.InvariantCulture)));
				_2A6B.Append(';');
			}
			else
			{
				_2A6B.Append(_2A68(_2A6A.TimeOfDay.ToString("HH:mm:ss.FFF", DateTimeFormatInfo.InvariantInfo)));
				_2A6B.Append(';');
				_2A6B.Append(_2A68(_2A6A.Month.ToString(CultureInfo.InvariantCulture)));
				_2A6B.Append(';');
				_2A6B.Append(_2A68(_2A6A.Week.ToString(CultureInfo.InvariantCulture)));
				_2A6B.Append(';');
				_2A6B.Append(_2A68(((int)_2A6A.DayOfWeek).ToString(CultureInfo.InvariantCulture)));
				_2A6B.Append(';');
			}
			_2A6B.Append(']');
		}

		private void _2A6C(int _2A6D)
		{
			if (_2A5A < 0 || _2A5A >= _2A5B.Length)
			{
				throw new SerializationException("Serialization_InvalidData");
			}
			_2A6F _2A6F = _2A6F._2A71;
			for (int i = _2A5A; i < _2A5B.Length; i++)
			{
				switch (_2A6F)
				{
				case _2A6F._2A70:
					_2A6E(_2A5B[i]);
					_2A6F = _2A6F._2A71;
					break;
				case _2A6F._2A71:
					switch (_2A5B[i])
					{
					case '[':
						_2A6D++;
						break;
					case '\\':
						_2A6F = _2A6F._2A70;
						break;
					case ']':
						_2A6D--;
						if (_2A6D == 0)
						{
							_2A5A = i + 1;
							if (_2A5A >= _2A5B.Length)
							{
								_2A5C = _2A6F._2A73;
							}
							else
							{
								_2A5C = _2A6F._2A72;
							}
							return;
						}
						break;
					case '\0':
						throw new SerializationException("Serialization_InvalidData");
					}
					break;
				}
			}
			throw new SerializationException("Serialization_InvalidData");
		}

		private static void _2A6E(char _0BDE)
		{
			if (_0BDE != '\\' && _0BDE != ';' && _0BDE != '[' && _0BDE != ']')
			{
				throw new SerializationException("Serialization_InvalidEscapeSequence " + _0BDE);
			}
		}
	}

	private class _2A74 : IComparer<TimeZoneInfo>
	{
		private int _2A75(TimeZoneInfo و, TimeZoneInfo ى)
		{
			return Utilities._17D5(و.Id, ى.Id);
		}

		int IComparer<TimeZoneInfo>.Compare(TimeZoneInfo و, TimeZoneInfo ى)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ⩵
			return this._2A75(و, ى);
		}
	}

	private const string c_daylightValue = "Dlt";

	private const string c_disableDST = "DisableAutoDaylightTimeSet";

	private const string c_disableDynamicDST = "DynamicDaylightTimeDisabled";

	private const string c_displayValue = "Display";

	private const string c_firstEntryValue = "FirstEntry";

	private const string c_lastEntryValue = "LastEntry";

	private const string c_localId = "Local";

	private const int c_maxKeyLength = 255;

	private const string c_muiDaylightValue = "MUI_Dlt";

	private const string c_muiDisplayValue = "MUI_Display";

	private const string c_muiStandardValue = "MUI_Std";

	private const string c_standardValue = "Std";

	private const long c_ticksPerDay = 864000000000L;

	private const long c_ticksPerDayRange = 863999990000L;

	private const long c_ticksPerHour = 36000000000L;

	private const long c_ticksPerMillisecond = 10000L;

	private const long c_ticksPerMinute = 600000000L;

	private const long c_ticksPerSecond = 10000000L;

	private const string c_timeZoneInfoRegistryHive = "SYSTEM\\CurrentControlSet\\Control\\TimeZoneInformation";

	private const string c_timeZoneInfoRegistryHivePermissionList = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\TimeZoneInformation";

	private const string c_timeZoneInfoValue = "TZI";

	private const string c_timeZonesRegistryHive = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones";

	private const string c_timeZonesRegistryHivePermissionList = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones";

	private const string c_utcId = "UTC";

	private AdjustmentRule[] m_adjustmentRules;

	private TimeSpan m_baseUtcOffset;

	private string m_daylightDisplayName;

	private string m_displayName;

	private string m_id;

	private string m_standardDisplayName;

	private bool m_supportsDaylightSavingTime;

	private static bool s_allSystemTimeZonesRead = false;

	private static object s_hiddenInternalSyncObject;

	private static Dictionary<string, TimeZoneInfo> s_hiddenSystemTimeZones;

	private static TimeZoneInfo s_localTimeZone;

	private static List<TimeZoneInfo> s_readOnlySystemTimeZones;

	private static TimeZoneInfo s_utcTimeZone;

	public TimeSpan BaseUtcOffset => m_baseUtcOffset;

	public string DaylightName
	{
		get
		{
			if (m_daylightDisplayName != null)
			{
				return m_daylightDisplayName;
			}
			return string.Empty;
		}
	}

	public string DisplayName
	{
		get
		{
			if (m_displayName != null)
			{
				return m_displayName;
			}
			return string.Empty;
		}
	}

	public string Id => m_id;

	public static TimeZoneInfo Local
	{
		[SecurityCritical]
		get
		{
			TimeZoneInfo timeZoneInfo = s_localTimeZone;
			if (timeZoneInfo != null)
			{
				return timeZoneInfo;
			}
			lock (s_internalSyncObject)
			{
				if (s_localTimeZone == null)
				{
					TimeZoneInfo localTimeZone = GetLocalTimeZone();
					s_localTimeZone = new TimeZoneInfo(localTimeZone.m_id, localTimeZone.m_baseUtcOffset, localTimeZone.m_displayName, localTimeZone.m_standardDisplayName, localTimeZone.m_daylightDisplayName, localTimeZone.m_adjustmentRules, disableDaylightSavingTime: false);
				}
				return s_localTimeZone;
			}
		}
	}

	private static object s_internalSyncObject
	{
		get
		{
			if (s_hiddenInternalSyncObject == null)
			{
				object value = new object();
				Interlocked.CompareExchange(ref s_hiddenInternalSyncObject, value, null);
			}
			return s_hiddenInternalSyncObject;
		}
	}

	private static Dictionary<string, TimeZoneInfo> s_systemTimeZones
	{
		get
		{
			if (s_hiddenSystemTimeZones == null)
			{
				s_hiddenSystemTimeZones = new Dictionary<string, TimeZoneInfo>();
			}
			return s_hiddenSystemTimeZones;
		}
		set
		{
			s_hiddenSystemTimeZones = value;
		}
	}

	public string StandardName
	{
		get
		{
			if (m_standardDisplayName != null)
			{
				return m_standardDisplayName;
			}
			return string.Empty;
		}
	}

	public bool SupportsDaylightSavingTime => m_supportsDaylightSavingTime;

	public static TimeZoneInfo Utc
	{
		get
		{
			TimeZoneInfo timeZoneInfo = s_utcTimeZone;
			if (timeZoneInfo != null)
			{
				return timeZoneInfo;
			}
			lock (s_internalSyncObject)
			{
				if (s_utcTimeZone == null)
				{
					s_utcTimeZone = CreateCustomTimeZone("UTC", TimeSpan.Zero, "UTC", "UTC");
				}
				return s_utcTimeZone;
			}
		}
	}

	private TimeZoneInfo(_2A34._2A2C zone, bool dstDisabled)
	{
		if (string.IsNullOrEmpty(zone._2A2E))
		{
			m_id = "Local";
		}
		else
		{
			m_id = zone._2A2E;
		}
		m_baseUtcOffset = new TimeSpan(0, -zone._2A2D, 0);
		if (!dstDisabled)
		{
			_2A34._2A40 timeZoneInformation = new _2A34._2A40(zone);
			DateTime minValue = DateTime.MinValue;
			DateTime date = minValue.Date;
			DateTime maxValue = DateTime.MaxValue;
			AdjustmentRule adjustmentRule = CreateAdjustmentRuleFromTimeZoneInformation(timeZoneInformation, date, maxValue.Date);
			if (adjustmentRule != null)
			{
				m_adjustmentRules = new AdjustmentRule[1] { adjustmentRule };
			}
		}
		ValidateTimeZoneInfo(m_id, m_baseUtcOffset, m_adjustmentRules, out m_supportsDaylightSavingTime);
		m_displayName = zone._2A2E;
		m_standardDisplayName = zone._2A2E;
		m_daylightDisplayName = zone._2A31;
	}

	private TimeZoneInfo(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		m_id = (string)info.GetValue("Id", typeof(string));
		m_displayName = (string)info.GetValue("DisplayName", typeof(string));
		m_standardDisplayName = (string)info.GetValue("StandardName", typeof(string));
		m_daylightDisplayName = (string)info.GetValue("DaylightName", typeof(string));
		m_baseUtcOffset = (TimeSpan)info.GetValue("BaseUtcOffset", typeof(TimeSpan));
		m_adjustmentRules = (AdjustmentRule[])info.GetValue("AdjustmentRules", typeof(AdjustmentRule[]));
		m_supportsDaylightSavingTime = (bool)info.GetValue("SupportsDaylightSavingTime", typeof(bool));
	}

	private TimeZoneInfo(string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName, string daylightDisplayName, AdjustmentRule[] adjustmentRules, bool disableDaylightSavingTime)
	{
		ValidateTimeZoneInfo(id, baseUtcOffset, adjustmentRules, out var adjustmentRulesSupportDst);
		if (!disableDaylightSavingTime && adjustmentRules != null && adjustmentRules.Length > 0)
		{
			m_adjustmentRules = (AdjustmentRule[])adjustmentRules.Clone();
		}
		m_id = id;
		m_baseUtcOffset = baseUtcOffset;
		m_displayName = displayName;
		m_standardDisplayName = standardDisplayName;
		m_daylightDisplayName = (disableDaylightSavingTime ? null : daylightDisplayName);
		m_supportsDaylightSavingTime = adjustmentRulesSupportDst && !disableDaylightSavingTime;
	}

	[SecurityTreatAsSafe]
	[SecurityCritical]
	private static bool CheckDaylightSavingTimeDisabled()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		try
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission((IPermission?)new RegistryPermission((RegistryPermissionAccess)1, "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\TimeZoneInformation"));
			permissionSet.Assert();
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\TimeZoneInformation", RegistryKeyPermissionCheck.Default, RegistryRights.ExecuteKey);
			if (registryKey == null)
			{
				return false;
			}
			int num = 0;
			try
			{
				num = (int)registryKey.GetValue("DisableAutoDaylightTimeSet", 0, RegistryValueOptions.None);
			}
			catch (InvalidCastException)
			{
			}
			if (num == 1)
			{
				goto IL_0082;
			}
			try
			{
				num = (int)registryKey.GetValue("DynamicDaylightTimeDisabled", 0, RegistryValueOptions.None);
			}
			catch (InvalidCastException)
			{
			}
			if (num == 1)
			{
				goto IL_0082;
			}
			goto end_IL_0035;
			IL_0082:
			return true;
			end_IL_0035:;
		}
		finally
		{
			PermissionSet.RevertAssert();
		}
		return false;
	}

	private static bool CheckDaylightSavingTimeNotSupported(_2A34._2A2C timeZone)
	{
		if (timeZone._2A32._2A36 == timeZone._2A2F._2A36 && timeZone._2A32._2A37 == timeZone._2A2F._2A37 && timeZone._2A32._2A38 == timeZone._2A2F._2A38 && timeZone._2A32._2A39 == timeZone._2A2F._2A39 && timeZone._2A32._2A3A == timeZone._2A2F._2A3A && timeZone._2A32._2A3B == timeZone._2A2F._2A3B && timeZone._2A32._2A3C == timeZone._2A2F._2A3C)
		{
			return timeZone._2A32._2A3D == timeZone._2A2F._2A3D;
		}
		return false;
	}

	private static bool CheckIsDst(DateTime startTime, DateTime time, DateTime endTime)
	{
		if (startTime.Year != endTime.Year)
		{
			endTime = endTime.AddYears(startTime.Year - endTime.Year);
		}
		if (startTime.Year != time.Year)
		{
			time = time.AddYears(startTime.Year - time.Year);
		}
		if (startTime > endTime)
		{
			if (!(time < endTime))
			{
				return time >= startTime;
			}
			return true;
		}
		if (time >= startTime)
		{
			return time < endTime;
		}
		return false;
	}

	public static void ClearCachedData()
	{
		lock (s_internalSyncObject)
		{
			s_localTimeZone = null;
			s_utcTimeZone = null;
			s_systemTimeZones = null;
			s_readOnlySystemTimeZones = null;
			s_allSystemTimeZonesRead = false;
		}
	}

	public static DateTime ConvertTime(DateTime dateTime, TimeZoneInfo destinationTimeZone)
	{
		if (destinationTimeZone == null)
		{
			throw new ArgumentNullException("destinationTimeZone");
		}
		if (dateTime.Kind == DateTimeKind.Utc)
		{
			lock (s_internalSyncObject)
			{
				return ConvertTime(dateTime, Utc, destinationTimeZone);
			}
		}
		lock (s_internalSyncObject)
		{
			return ConvertTime(dateTime, Local, destinationTimeZone);
		}
	}

	public static DateTimeOffset ConvertTime(DateTimeOffset dateTimeOffset, TimeZoneInfo destinationTimeZone)
	{
		if (destinationTimeZone == null)
		{
			throw new ArgumentNullException("destinationTimeZone");
		}
		DateTime utcDateTime = dateTimeOffset.UtcDateTime;
		TimeSpan utcOffsetFromUtc = GetUtcOffsetFromUtc(utcDateTime, destinationTimeZone);
		long num = utcDateTime.Ticks + utcOffsetFromUtc.Ticks;
		DateTimeOffset maxValue = DateTimeOffset.MaxValue;
		if (num > maxValue.Ticks)
		{
			return DateTimeOffset.MaxValue;
		}
		DateTimeOffset minValue = DateTimeOffset.MinValue;
		if (num < minValue.Ticks)
		{
			return DateTimeOffset.MinValue;
		}
		return new DateTimeOffset(num, utcOffsetFromUtc);
	}

	public static DateTime ConvertTime(DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
	{
		return ConvertTime(dateTime, sourceTimeZone, destinationTimeZone, _2A41.ᘶ);
	}

	internal static DateTime ConvertTime(DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone, _2A41 flags)
	{
		if (sourceTimeZone == null)
		{
			throw new ArgumentNullException("sourceTimeZone");
		}
		if (destinationTimeZone == null)
		{
			throw new ArgumentNullException("destinationTimeZone");
		}
		DateTimeKind correspondingKind = sourceTimeZone.GetCorrespondingKind();
		if ((flags & _2A41._2A42) == 0 && dateTime.Kind != 0 && dateTime.Kind != correspondingKind)
		{
			throw new ArgumentException("Argument_ConvertMismatch", "sourceTimeZone");
		}
		AdjustmentRule adjustmentRuleForTime = sourceTimeZone.GetAdjustmentRuleForTime(dateTime);
		TimeSpan baseUtcOffset = sourceTimeZone.BaseUtcOffset;
		if (adjustmentRuleForTime != null)
		{
			bool flag = false;
			DaylightTime daylightTime = GetDaylightTime(dateTime.Year, adjustmentRuleForTime);
			if ((flags & _2A41._2A42) == 0 && GetIsInvalidTime(dateTime, adjustmentRuleForTime, daylightTime))
			{
				throw new ArgumentException("Argument_DateTimeIsInvalid", "dateTime");
			}
			flag = GetIsDaylightSavings(dateTime, adjustmentRuleForTime, daylightTime);
			baseUtcOffset += (flag ? adjustmentRuleForTime.DaylightDelta : TimeSpan.Zero);
		}
		DateTimeKind dateTimeKind = destinationTimeZone.GetCorrespondingKind();
		if (dateTime.Kind != 0 && correspondingKind != 0 && correspondingKind == dateTimeKind)
		{
			return dateTime;
		}
		long ticks = dateTime.Ticks - baseUtcOffset.Ticks;
		DateTime dateTime2 = ConvertUtcToTimeZone(ticks, destinationTimeZone);
		if (dateTimeKind == DateTimeKind.Local)
		{
			dateTimeKind = DateTimeKind.Unspecified;
		}
		return new DateTime(dateTime2.Ticks, dateTimeKind);
	}

	public static DateTime ConvertTimeBySystemTimeZoneId(DateTime dateTime, string destinationTimeZoneId)
	{
		return ConvertTime(dateTime, FindSystemTimeZoneById(destinationTimeZoneId));
	}

	public static DateTimeOffset ConvertTimeBySystemTimeZoneId(DateTimeOffset dateTimeOffset, string destinationTimeZoneId)
	{
		return ConvertTime(dateTimeOffset, FindSystemTimeZoneById(destinationTimeZoneId));
	}

	public static DateTime ConvertTimeBySystemTimeZoneId(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId)
	{
		if (dateTime.Kind == DateTimeKind.Local && string.Compare(sourceTimeZoneId, Local.Id, StringComparison.OrdinalIgnoreCase) == 0)
		{
			lock (s_internalSyncObject)
			{
				return ConvertTime(dateTime, Local, FindSystemTimeZoneById(destinationTimeZoneId));
			}
		}
		if (dateTime.Kind == DateTimeKind.Utc && string.Compare(sourceTimeZoneId, Utc.Id, StringComparison.OrdinalIgnoreCase) == 0)
		{
			lock (s_internalSyncObject)
			{
				return ConvertTime(dateTime, Utc, FindSystemTimeZoneById(destinationTimeZoneId));
			}
		}
		return ConvertTime(dateTime, FindSystemTimeZoneById(sourceTimeZoneId), FindSystemTimeZoneById(destinationTimeZoneId));
	}

	public static DateTime ConvertTimeFromUtc(DateTime dateTime, TimeZoneInfo destinationTimeZone)
	{
		lock (s_internalSyncObject)
		{
			return ConvertTime(dateTime, Utc, destinationTimeZone);
		}
	}

	public static DateTime ConvertTimeToUtc(DateTime dateTime)
	{
		if (dateTime.Kind == DateTimeKind.Utc)
		{
			return dateTime;
		}
		lock (s_internalSyncObject)
		{
			return ConvertTime(dateTime, Local, Utc);
		}
	}

	public static DateTime ConvertTimeToUtc(DateTime dateTime, TimeZoneInfo sourceTimeZone)
	{
		lock (s_internalSyncObject)
		{
			return ConvertTime(dateTime, sourceTimeZone, Utc);
		}
	}

	private static DateTime ConvertUtcToTimeZone(long ticks, TimeZoneInfo destinationTimeZone)
	{
		long num = ticks;
		DateTime maxValue = DateTime.MaxValue;
		DateTime time;
		if (num > maxValue.Ticks)
		{
			time = DateTime.MaxValue;
		}
		else
		{
			long num2 = ticks;
			DateTime minValue = DateTime.MinValue;
			time = ((num2 >= minValue.Ticks) ? new DateTime(ticks) : DateTime.MinValue);
		}
		ticks += GetUtcOffsetFromUtc(time, destinationTimeZone).Ticks;
		long num3 = ticks;
		DateTime maxValue2 = DateTime.MaxValue;
		if (num3 > maxValue2.Ticks)
		{
			return DateTime.MaxValue;
		}
		long num4 = ticks;
		DateTime minValue2 = DateTime.MinValue;
		if (num4 < minValue2.Ticks)
		{
			return DateTime.MinValue;
		}
		return new DateTime(ticks);
	}

	private static AdjustmentRule CreateAdjustmentRuleFromTimeZoneInformation(_2A34._2A40 timeZoneInformation, DateTime startDate, DateTime endDate)
	{
		if (timeZoneInformation._2A2F._2A37 == 0)
		{
			return null;
		}
		TransitionTime? transitionTime = TransitionTimeFromTimeZoneInformation(timeZoneInformation, readStartDate: true);
		if (!transitionTime.HasValue)
		{
			return null;
		}
		TransitionTime? transitionTime2 = TransitionTimeFromTimeZoneInformation(timeZoneInformation, readStartDate: false);
		if (!transitionTime2.HasValue)
		{
			return null;
		}
		if (transitionTime.Equals(transitionTime2))
		{
			return null;
		}
		return AdjustmentRule.CreateAdjustmentRule(startDate, endDate, new TimeSpan(0, -timeZoneInformation._2A33, 0), transitionTime.Value, transitionTime2.Value);
	}

	public static TimeZoneInfo CreateCustomTimeZone(string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName)
	{
		return new TimeZoneInfo(id, baseUtcOffset, displayName, standardDisplayName, standardDisplayName, null, disableDaylightSavingTime: false);
	}

	public static TimeZoneInfo CreateCustomTimeZone(string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName, string daylightDisplayName, AdjustmentRule[] adjustmentRules)
	{
		return new TimeZoneInfo(id, baseUtcOffset, displayName, standardDisplayName, daylightDisplayName, adjustmentRules, disableDaylightSavingTime: false);
	}

	public static TimeZoneInfo CreateCustomTimeZone(string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName, string daylightDisplayName, AdjustmentRule[] adjustmentRules, bool disableDaylightSavingTime)
	{
		return new TimeZoneInfo(id, baseUtcOffset, displayName, standardDisplayName, daylightDisplayName, adjustmentRules, disableDaylightSavingTime);
	}

	public bool Equals(TimeZoneInfo other)
	{
		if (other != null && string.Compare(m_id, other.m_id, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return HasSameRules(other);
		}
		return false;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	private static string FindIdFromTimeZoneInformation(_2A34._2A2C timeZone, out bool dstDisabled)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		dstDisabled = false;
		try
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission((IPermission?)new RegistryPermission((RegistryPermissionAccess)1, "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones"));
			permissionSet.Assert();
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", RegistryKeyPermissionCheck.Default, RegistryRights.ExecuteKey);
			if (registryKey == null)
			{
				return null;
			}
			string[] subKeyNames = registryKey.GetSubKeyNames();
			foreach (string text in subKeyNames)
			{
				if (TryCompareTimeZoneInformationToRegistry(timeZone, text, out dstDisabled))
				{
					return text;
				}
			}
		}
		finally
		{
			PermissionSet.RevertAssert();
		}
		return null;
	}

	public static TimeZoneInfo FindSystemTimeZoneById(string id)
	{
		if (string.Compare(id, "UTC", StringComparison.OrdinalIgnoreCase) == 0)
		{
			return Utc;
		}
		lock (s_internalSyncObject)
		{
			return GetTimeZone(id);
		}
	}

	public static TimeZoneInfo FromSerializedString(string source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source.Length == 0)
		{
			throw new ArgumentException("Argument_InvalidSerializedString source");
		}
		return _2A4A._2A5D(source);
	}

	private AdjustmentRule GetAdjustmentRuleForTime(DateTime dateTime)
	{
		if (m_adjustmentRules != null && m_adjustmentRules.Length != 0)
		{
			DateTime date = dateTime.Date;
			for (int i = 0; i < m_adjustmentRules.Length; i++)
			{
				if (m_adjustmentRules[i].DateStart <= date && m_adjustmentRules[i].DateEnd >= date)
				{
					return m_adjustmentRules[i];
				}
			}
		}
		return null;
	}

	public AdjustmentRule[] GetAdjustmentRules()
	{
		if (m_adjustmentRules == null)
		{
			return new AdjustmentRule[0];
		}
		return (AdjustmentRule[])m_adjustmentRules.Clone();
	}

	public TimeSpan[] GetAmbiguousTimeOffsets(DateTime dateTime)
	{
		if (!m_supportsDaylightSavingTime)
		{
			throw new ArgumentException("Argument_DateTimeIsNotAmbiguous", "dateTime");
		}
		DateTime dateTime2;
		if (dateTime.Kind == DateTimeKind.Local)
		{
			lock (s_internalSyncObject)
			{
				dateTime2 = ConvertTime(dateTime, Local, this, _2A41._2A42);
			}
		}
		else if (dateTime.Kind == DateTimeKind.Utc)
		{
			lock (s_internalSyncObject)
			{
				dateTime2 = ConvertTime(dateTime, Utc, this, _2A41._2A42);
			}
		}
		else
		{
			dateTime2 = dateTime;
		}
		bool flag = false;
		AdjustmentRule adjustmentRuleForTime = GetAdjustmentRuleForTime(dateTime2);
		if (adjustmentRuleForTime != null)
		{
			DaylightTime daylightTime = GetDaylightTime(dateTime2.Year, adjustmentRuleForTime);
			flag = GetIsAmbiguousTime(dateTime2, adjustmentRuleForTime, daylightTime);
		}
		if (!flag)
		{
			throw new ArgumentException("Argument_DateTimeIsNotAmbiguous", "dateTime");
		}
		TimeSpan[] array = new TimeSpan[2];
		if (adjustmentRuleForTime.DaylightDelta > TimeSpan.Zero)
		{
			ref TimeSpan reference = ref array[0];
			reference = m_baseUtcOffset;
			ref TimeSpan reference2 = ref array[1];
			reference2 = m_baseUtcOffset + adjustmentRuleForTime.DaylightDelta;
			return array;
		}
		ref TimeSpan reference3 = ref array[0];
		reference3 = m_baseUtcOffset + adjustmentRuleForTime.DaylightDelta;
		ref TimeSpan reference4 = ref array[1];
		reference4 = m_baseUtcOffset;
		return array;
	}

	public TimeSpan[] GetAmbiguousTimeOffsets(DateTimeOffset dateTimeOffset)
	{
		if (!m_supportsDaylightSavingTime)
		{
			throw new ArgumentException("Argument_DateTimeOffsetIsNotAmbiguous", "dateTimeOffset");
		}
		DateTime dateTime = ConvertTime(dateTimeOffset, this).DateTime;
		bool flag = false;
		AdjustmentRule adjustmentRuleForTime = GetAdjustmentRuleForTime(dateTime);
		if (adjustmentRuleForTime != null)
		{
			DaylightTime daylightTime = GetDaylightTime(dateTime.Year, adjustmentRuleForTime);
			flag = GetIsAmbiguousTime(dateTime, adjustmentRuleForTime, daylightTime);
		}
		if (!flag)
		{
			throw new ArgumentException("Argument_DateTimeOffsetIsNotAmbiguous", "dateTimeOffset");
		}
		TimeSpan[] array = new TimeSpan[2];
		if (adjustmentRuleForTime.DaylightDelta > TimeSpan.Zero)
		{
			ref TimeSpan reference = ref array[0];
			reference = m_baseUtcOffset;
			ref TimeSpan reference2 = ref array[1];
			reference2 = m_baseUtcOffset + adjustmentRuleForTime.DaylightDelta;
			return array;
		}
		ref TimeSpan reference3 = ref array[0];
		reference3 = m_baseUtcOffset + adjustmentRuleForTime.DaylightDelta;
		ref TimeSpan reference4 = ref array[1];
		reference4 = m_baseUtcOffset;
		return array;
	}

	private DateTimeKind GetCorrespondingKind()
	{
		if (this == s_utcTimeZone)
		{
			return DateTimeKind.Utc;
		}
		if (this == s_localTimeZone)
		{
			return DateTimeKind.Local;
		}
		return DateTimeKind.Unspecified;
	}

	private static DaylightTime GetDaylightTime(int year, AdjustmentRule rule)
	{
		TimeSpan daylightDelta = rule.DaylightDelta;
		DateTime start = TransitionTimeToDateTime(year, rule.DaylightTransitionStart);
		return new DaylightTime(start, TransitionTimeToDateTime(year, rule.DaylightTransitionEnd), daylightDelta);
	}

	public override int GetHashCode()
	{
		return m_id.ToUpperInvariant().GetHashCode();
	}

	private static bool GetIsAmbiguousTime(DateTime time, AdjustmentRule rule, DaylightTime daylightTime)
	{
		bool flag = false;
		if (rule != null && rule.DaylightDelta != TimeSpan.Zero)
		{
			DateTime dateTime;
			DateTime dateTime2;
			if (rule.DaylightDelta > TimeSpan.Zero)
			{
				dateTime = daylightTime.End;
				dateTime2 = daylightTime.End - rule.DaylightDelta;
			}
			else
			{
				dateTime = daylightTime.Start;
				dateTime2 = daylightTime.Start + rule.DaylightDelta;
			}
			flag = time >= dateTime2 && time < dateTime;
			if (flag || dateTime.Year == dateTime2.Year)
			{
				return flag;
			}
			try
			{
				DateTime dateTime3 = dateTime.AddYears(1);
				DateTime dateTime4 = dateTime2.AddYears(1);
				flag = time >= dateTime4 && time < dateTime3;
			}
			catch (ArgumentOutOfRangeException)
			{
			}
			if (flag)
			{
				return flag;
			}
			try
			{
				DateTime dateTime3 = dateTime.AddYears(-1);
				DateTime dateTime4 = dateTime2.AddYears(-1);
				flag = time >= dateTime4 && time < dateTime3;
			}
			catch (ArgumentOutOfRangeException)
			{
			}
		}
		return flag;
	}

	private static bool GetIsDaylightSavings(DateTime time, AdjustmentRule rule, DaylightTime daylightTime)
	{
		if (rule == null)
		{
			return false;
		}
		bool flag = rule.DaylightDelta > TimeSpan.Zero;
		DateTime startTime = daylightTime.Start + (flag ? rule.DaylightDelta : TimeSpan.Zero);
		DateTime endTime = daylightTime.End + (flag ? (-rule.DaylightDelta) : TimeSpan.Zero);
		return CheckIsDst(startTime, time, endTime);
	}

	private static bool GetIsDaylightSavingsFromUtc(DateTime time, int Year, TimeSpan utc, AdjustmentRule rule)
	{
		if (rule == null)
		{
			return false;
		}
		DaylightTime daylightTime = GetDaylightTime(Year, rule);
		DateTime startTime = daylightTime.Start - utc;
		DateTime endTime = daylightTime.End - utc - rule.DaylightDelta;
		return CheckIsDst(startTime, time, endTime);
	}

	private static bool GetIsInvalidTime(DateTime time, AdjustmentRule rule, DaylightTime daylightTime)
	{
		bool flag = false;
		if (rule != null && rule.DaylightDelta != TimeSpan.Zero)
		{
			DateTime dateTime;
			DateTime dateTime2;
			if (rule.DaylightDelta < TimeSpan.Zero)
			{
				dateTime = daylightTime.End;
				dateTime2 = daylightTime.End - rule.DaylightDelta;
			}
			else
			{
				dateTime = daylightTime.Start;
				dateTime2 = daylightTime.Start + rule.DaylightDelta;
			}
			flag = time >= dateTime && time < dateTime2;
			if (flag || dateTime.Year == dateTime2.Year)
			{
				return flag;
			}
			try
			{
				DateTime dateTime3 = dateTime.AddYears(1);
				DateTime dateTime4 = dateTime2.AddYears(1);
				flag = time >= dateTime3 && time < dateTime4;
			}
			catch (ArgumentOutOfRangeException)
			{
			}
			if (flag)
			{
				return flag;
			}
			try
			{
				DateTime dateTime3 = dateTime.AddYears(-1);
				DateTime dateTime4 = dateTime2.AddYears(-1);
				flag = time >= dateTime3 && time < dateTime4;
			}
			catch (ArgumentOutOfRangeException)
			{
			}
		}
		return flag;
	}

	[SecurityCritical]
	private static TimeZoneInfo GetLocalTimeZone()
	{
		string text = null;
		try
		{
			_2A34._2A3E lpDynamicTimeZoneInformation = default(_2A34._2A3E);
			long num = _2A43.GetDynamicTimeZoneInformation(out lpDynamicTimeZoneInformation);
			if (num == -1)
			{
				return CreateCustomTimeZone("Local", TimeSpan.Zero, "Local", "Local");
			}
			_2A34._2A2C _2A2C = new _2A34._2A2C(lpDynamicTimeZoneInformation);
			bool dstDisabled = CheckDaylightSavingTimeDisabled();
			if (!string.IsNullOrEmpty(lpDynamicTimeZoneInformation._2A3F) && TryGetTimeZone(lpDynamicTimeZoneInformation._2A3F, dstDisabled, out var value, out var _) == _2A45._2A46)
			{
				return value;
			}
			text = FindIdFromTimeZoneInformation(_2A2C, out dstDisabled);
			if (text != null && TryGetTimeZone(text, dstDisabled, out var value2, out var _) == _2A45._2A46)
			{
				return value2;
			}
			return GetLocalTimeZoneFromWin32Data(_2A2C, dstDisabled);
		}
		catch (EntryPointNotFoundException)
		{
			_2A34._2A2C lpTimeZoneInformation = default(_2A34._2A2C);
			long num2 = _2A43.GetTimeZoneInformation(out lpTimeZoneInformation);
			if (num2 == -1)
			{
				return CreateCustomTimeZone("Local", TimeSpan.Zero, "Local", "Local");
			}
			text = FindIdFromTimeZoneInformation(lpTimeZoneInformation, out var dstDisabled2);
			if (text != null && TryGetTimeZone(text, dstDisabled2, out var value3, out var _) == _2A45._2A46)
			{
				return value3;
			}
			return GetLocalTimeZoneFromWin32Data(lpTimeZoneInformation, dstDisabled2);
		}
	}

	private static TimeZoneInfo GetLocalTimeZoneFromWin32Data(_2A34._2A2C timeZoneInformation, bool dstDisabled)
	{
		TimeZoneInfo result = null;
		try
		{
			result = new TimeZoneInfo(timeZoneInformation, dstDisabled);
		}
		catch (ArgumentException)
		{
		}
		catch (InvalidTimeZoneException)
		{
		}
		return result;
	}

	[SecurityTreatAsSafe]
	[SecurityCritical]
	public static List<TimeZoneInfo> GetSystemTimeZones()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		lock (s_internalSyncObject)
		{
			if (!s_allSystemTimeZonesRead)
			{
				PermissionSet permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.AddPermission((IPermission?)new RegistryPermission((RegistryPermissionAccess)1, "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones"));
				permissionSet.Assert();
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", RegistryKeyPermissionCheck.Default, RegistryRights.ExecuteKey))
				{
					if (registryKey == null)
					{
						List<TimeZoneInfo> collection = ((s_systemTimeZones == null) ? new List<TimeZoneInfo>() : new List<TimeZoneInfo>(s_systemTimeZones.Values));
						s_readOnlySystemTimeZones = new List<TimeZoneInfo>(collection);
						s_allSystemTimeZonesRead = true;
						return s_readOnlySystemTimeZones;
					}
					string[] subKeyNames = registryKey.GetSubKeyNames();
					foreach (string id in subKeyNames)
					{
						TryGetTimeZone(id, dstDisabled: false, out var _, out var _);
					}
				}
				IComparer<TimeZoneInfo> comparer = new _2A74();
				List<TimeZoneInfo> list = new List<TimeZoneInfo>(s_systemTimeZones.Values);
				list.Sort(comparer);
				s_readOnlySystemTimeZones = new List<TimeZoneInfo>(list);
				s_allSystemTimeZonesRead = true;
			}
			return s_readOnlySystemTimeZones;
		}
	}

	private static TimeZoneInfo GetTimeZone(string id)
	{
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if (id.Length == 0 || id.Length > 255 || id.Contains("\0"))
		{
			throw new TimeZoneNotFoundException("TimeZoneNotFound_MissingRegistryData, id");
		}
		TimeZoneInfo value;
		Exception e;
		return TryGetTimeZone(id, dstDisabled: false, out value, out e) switch
		{
			_2A45._2A46 => value, 
			_2A45._2A48 => throw new InvalidTimeZoneException("InvalidTimeZone_InvalidRegistryData, id", e), 
			_2A45._2A49 => throw new SecurityException("Security_CannotReadRegistryData, id", e), 
			_ => throw new TimeZoneNotFoundException("TimeZoneNotFound_MissingRegistryData, id", e), 
		};
	}

	public TimeSpan GetUtcOffset(DateTime dateTime)
	{
		if (dateTime.Kind == DateTimeKind.Local)
		{
			DateTime time;
			lock (s_internalSyncObject)
			{
				if (GetCorrespondingKind() == DateTimeKind.Local)
				{
					return GetUtcOffset(dateTime, this);
				}
				time = ConvertTime(dateTime, Local, Utc, _2A41._2A42);
			}
			return GetUtcOffsetFromUtc(time, this);
		}
		if (dateTime.Kind != DateTimeKind.Utc)
		{
			return GetUtcOffset(dateTime, this);
		}
		if (GetCorrespondingKind() == DateTimeKind.Utc)
		{
			return m_baseUtcOffset;
		}
		return GetUtcOffsetFromUtc(dateTime, this);
	}

	public TimeSpan GetUtcOffset(DateTimeOffset dateTimeOffset)
	{
		return GetUtcOffsetFromUtc(dateTimeOffset.UtcDateTime, this);
	}

	private static TimeSpan GetUtcOffset(DateTime time, TimeZoneInfo zone)
	{
		TimeSpan baseUtcOffset = zone.BaseUtcOffset;
		AdjustmentRule adjustmentRuleForTime = zone.GetAdjustmentRuleForTime(time);
		if (adjustmentRuleForTime != null)
		{
			DaylightTime daylightTime = GetDaylightTime(time.Year, adjustmentRuleForTime);
			bool isDaylightSavings = GetIsDaylightSavings(time, adjustmentRuleForTime, daylightTime);
			baseUtcOffset += (isDaylightSavings ? adjustmentRuleForTime.DaylightDelta : TimeSpan.Zero);
		}
		return baseUtcOffset;
	}

	private static TimeSpan GetUtcOffsetFromUtc(DateTime time, TimeZoneInfo zone)
	{
		bool isDaylightSavings;
		return GetUtcOffsetFromUtc(time, zone, out isDaylightSavings);
	}

	private static TimeSpan GetUtcOffsetFromUtc(DateTime time, TimeZoneInfo zone, out bool isDaylightSavings)
	{
		isDaylightSavings = false;
		TimeSpan baseUtcOffset = zone.BaseUtcOffset;
		AdjustmentRule adjustmentRuleForTime;
		int year;
		if (time > new DateTime(9999, 12, 31))
		{
			adjustmentRuleForTime = zone.GetAdjustmentRuleForTime(DateTime.MaxValue);
			year = 9999;
		}
		else if (time < new DateTime(1, 1, 2))
		{
			adjustmentRuleForTime = zone.GetAdjustmentRuleForTime(DateTime.MinValue);
			year = 1;
		}
		else
		{
			DateTime dateTime = time + baseUtcOffset;
			year = time.Year;
			adjustmentRuleForTime = zone.GetAdjustmentRuleForTime(dateTime);
		}
		if (adjustmentRuleForTime != null)
		{
			isDaylightSavings = GetIsDaylightSavingsFromUtc(time, year, zone.m_baseUtcOffset, adjustmentRuleForTime);
			baseUtcOffset += (isDaylightSavings ? adjustmentRuleForTime.DaylightDelta : TimeSpan.Zero);
		}
		return baseUtcOffset;
	}

	public bool HasSameRules(TimeZoneInfo other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (m_baseUtcOffset != other.m_baseUtcOffset || m_supportsDaylightSavingTime != other.m_supportsDaylightSavingTime)
		{
			return false;
		}
		AdjustmentRule[] adjustmentRules = m_adjustmentRules;
		AdjustmentRule[] adjustmentRules2 = other.m_adjustmentRules;
		bool flag = (adjustmentRules == null && adjustmentRules2 == null) || (adjustmentRules != null && adjustmentRules2 != null);
		if (!flag)
		{
			return false;
		}
		if (adjustmentRules != null)
		{
			if (adjustmentRules.Length != adjustmentRules2.Length)
			{
				return false;
			}
			for (int i = 0; i < adjustmentRules.Length; i++)
			{
				if (!adjustmentRules[i].Equals(adjustmentRules2[i]))
				{
					return false;
				}
			}
		}
		return flag;
	}

	public bool IsAmbiguousTime(DateTime dateTime)
	{
		if (!m_supportsDaylightSavingTime)
		{
			return false;
		}
		DateTime dateTime2;
		if (dateTime.Kind == DateTimeKind.Local)
		{
			lock (s_internalSyncObject)
			{
				dateTime2 = ConvertTime(dateTime, Local, this, _2A41._2A42);
			}
		}
		else if (dateTime.Kind == DateTimeKind.Utc)
		{
			lock (s_internalSyncObject)
			{
				dateTime2 = ConvertTime(dateTime, Utc, this, _2A41._2A42);
			}
		}
		else
		{
			dateTime2 = dateTime;
		}
		AdjustmentRule adjustmentRuleForTime = GetAdjustmentRuleForTime(dateTime2);
		if (adjustmentRuleForTime != null)
		{
			DaylightTime daylightTime = GetDaylightTime(dateTime2.Year, adjustmentRuleForTime);
			return GetIsAmbiguousTime(dateTime2, adjustmentRuleForTime, daylightTime);
		}
		return false;
	}

	public bool IsAmbiguousTime(DateTimeOffset dateTimeOffset)
	{
		if (!m_supportsDaylightSavingTime)
		{
			return false;
		}
		return IsAmbiguousTime(ConvertTime(dateTimeOffset, this).DateTime);
	}

	public bool IsDaylightSavingTime(DateTime dateTime)
	{
		if (!m_supportsDaylightSavingTime || m_adjustmentRules == null)
		{
			return false;
		}
		DateTime dateTime2;
		if (dateTime.Kind == DateTimeKind.Local)
		{
			lock (s_internalSyncObject)
			{
				dateTime2 = ConvertTime(dateTime, Local, this, _2A41._2A42);
			}
		}
		else
		{
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				if (GetCorrespondingKind() == DateTimeKind.Utc)
				{
					return false;
				}
				GetUtcOffsetFromUtc(dateTime, this, out var isDaylightSavings);
				return isDaylightSavings;
			}
			dateTime2 = dateTime;
		}
		AdjustmentRule adjustmentRuleForTime = GetAdjustmentRuleForTime(dateTime2);
		if (adjustmentRuleForTime != null)
		{
			DaylightTime daylightTime = GetDaylightTime(dateTime2.Year, adjustmentRuleForTime);
			return GetIsDaylightSavings(dateTime2, adjustmentRuleForTime, daylightTime);
		}
		return false;
	}

	public bool IsDaylightSavingTime(DateTimeOffset dateTimeOffset)
	{
		GetUtcOffsetFromUtc(dateTimeOffset.UtcDateTime, this, out var isDaylightSavings);
		return isDaylightSavings;
	}

	public bool IsInvalidTime(DateTime dateTime)
	{
		bool result = false;
		if (dateTime.Kind != 0 && (dateTime.Kind != DateTimeKind.Local || GetCorrespondingKind() != DateTimeKind.Local))
		{
			return result;
		}
		AdjustmentRule adjustmentRuleForTime = GetAdjustmentRuleForTime(dateTime);
		if (adjustmentRuleForTime != null)
		{
			DaylightTime daylightTime = GetDaylightTime(dateTime.Year, adjustmentRuleForTime);
			return GetIsInvalidTime(dateTime, adjustmentRuleForTime, daylightTime);
		}
		return false;
	}

	void IDeserializationCallback.OnDeserialization(object sender)
	{
		try
		{
			ValidateTimeZoneInfo(m_id, m_baseUtcOffset, m_adjustmentRules, out var adjustmentRulesSupportDst);
			if (adjustmentRulesSupportDst != m_supportsDaylightSavingTime)
			{
				throw new SerializationException("Serialization_CorruptField, SupportsDaylightSavingTime");
			}
		}
		catch (ArgumentException innerException)
		{
			throw new SerializationException("Serialization_InvalidData", innerException);
		}
		catch (InvalidTimeZoneException innerException2)
		{
			throw new SerializationException("Serialization_InvalidData", innerException2);
		}
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("Id", m_id);
		info.AddValue("DisplayName", m_displayName);
		info.AddValue("StandardName", m_standardDisplayName);
		info.AddValue("DaylightName", m_daylightDisplayName);
		info.AddValue("BaseUtcOffset", m_baseUtcOffset);
		info.AddValue("AdjustmentRules", m_adjustmentRules);
		info.AddValue("SupportsDaylightSavingTime", m_supportsDaylightSavingTime);
	}

	public string ToSerializedString()
	{
		return _2A4A._2A66(this);
	}

	public override string ToString()
	{
		return DisplayName;
	}

	private static TransitionTime? TransitionTimeFromTimeZoneInformation(_2A34._2A40 timeZoneInformation, bool readStartDate)
	{
		if (timeZoneInformation._2A2F._2A37 == 0)
		{
			return null;
		}
		return readStartDate ? ((timeZoneInformation._2A32._2A36 != 0) ? TransitionTime.CreateFixedDateRule(new DateTime(1, 1, 1, timeZoneInformation._2A32._2A3A, timeZoneInformation._2A32._2A3B, timeZoneInformation._2A32._2A3C, timeZoneInformation._2A32._2A3D), timeZoneInformation._2A32._2A37, timeZoneInformation._2A32._2A39) : TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, timeZoneInformation._2A32._2A3A, timeZoneInformation._2A32._2A3B, timeZoneInformation._2A32._2A3C, timeZoneInformation._2A32._2A3D), timeZoneInformation._2A32._2A37, timeZoneInformation._2A32._2A39, (DayOfWeek)timeZoneInformation._2A32._2A38)) : ((timeZoneInformation._2A2F._2A36 != 0) ? TransitionTime.CreateFixedDateRule(new DateTime(1, 1, 1, timeZoneInformation._2A2F._2A3A, timeZoneInformation._2A2F._2A3B, timeZoneInformation._2A2F._2A3C, timeZoneInformation._2A2F._2A3D), timeZoneInformation._2A2F._2A37, timeZoneInformation._2A2F._2A39) : TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, timeZoneInformation._2A2F._2A3A, timeZoneInformation._2A2F._2A3B, timeZoneInformation._2A2F._2A3C, timeZoneInformation._2A2F._2A3D), timeZoneInformation._2A2F._2A37, timeZoneInformation._2A2F._2A39, (DayOfWeek)timeZoneInformation._2A2F._2A38));
	}

	private static DateTime TransitionTimeToDateTime(int year, TransitionTime transitionTime)
	{
		DateTime timeOfDay = transitionTime.TimeOfDay;
		if (transitionTime.IsFixedDateRule)
		{
			int num = DateTime.DaysInMonth(year, transitionTime.Month);
			return new DateTime(year, transitionTime.Month, (num < transitionTime.Day) ? num : transitionTime.Day, timeOfDay.Hour, timeOfDay.Minute, timeOfDay.Second, timeOfDay.Millisecond);
		}
		DateTime result;
		if (transitionTime.Week <= 4)
		{
			result = new DateTime(year, transitionTime.Month, 1, timeOfDay.Hour, timeOfDay.Minute, timeOfDay.Second, timeOfDay.Millisecond);
			int dayOfWeek = (int)result.DayOfWeek;
			int num2 = (int)(transitionTime.DayOfWeek - dayOfWeek);
			if (num2 < 0)
			{
				num2 += 7;
			}
			num2 += 7 * (transitionTime.Week - 1);
			if (num2 > 0)
			{
				result = result.AddDays(num2);
			}
			return result;
		}
		int day = DateTime.DaysInMonth(year, transitionTime.Month);
		result = new DateTime(year, transitionTime.Month, day, timeOfDay.Hour, timeOfDay.Minute, timeOfDay.Second, timeOfDay.Millisecond);
		int num3 = result.DayOfWeek - transitionTime.DayOfWeek;
		if (num3 < 0)
		{
			num3 += 7;
		}
		if (num3 > 0)
		{
			result = result.AddDays(-num3);
		}
		return result;
	}

	private static bool TryCompareStandardDate(_2A34._2A2C timeZone, _2A34._2A40 registryTimeZoneInfo)
	{
		if (timeZone._2A2D == registryTimeZoneInfo._2A2D && timeZone._2A30 == registryTimeZoneInfo._2A30 && timeZone._2A2F._2A36 == registryTimeZoneInfo._2A2F._2A36 && timeZone._2A2F._2A37 == registryTimeZoneInfo._2A2F._2A37 && timeZone._2A2F._2A38 == registryTimeZoneInfo._2A2F._2A38 && timeZone._2A2F._2A39 == registryTimeZoneInfo._2A2F._2A39 && timeZone._2A2F._2A3A == registryTimeZoneInfo._2A2F._2A3A && timeZone._2A2F._2A3B == registryTimeZoneInfo._2A2F._2A3B && timeZone._2A2F._2A3C == registryTimeZoneInfo._2A2F._2A3C)
		{
			return timeZone._2A2F._2A3D == registryTimeZoneInfo._2A2F._2A3D;
		}
		return false;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	private static bool TryCompareTimeZoneInformationToRegistry(_2A34._2A2C timeZone, string id, out bool dstDisabled)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		dstDisabled = false;
		try
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission((IPermission?)new RegistryPermission((RegistryPermissionAccess)1, "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones"));
			permissionSet.Assert();
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", new object[2] { "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", id }), RegistryKeyPermissionCheck.Default, RegistryRights.ExecuteKey);
			if (registryKey == null)
			{
				return false;
			}
			_2A34._2A40 registryTimeZoneInfo;
			try
			{
				registryTimeZoneInfo = new _2A34._2A40((byte[])registryKey.GetValue("TZI", null, RegistryValueOptions.None));
			}
			catch (InvalidCastException)
			{
				return false;
			}
			catch (ArgumentException)
			{
				return false;
			}
			if (!TryCompareStandardDate(timeZone, registryTimeZoneInfo))
			{
				return false;
			}
			dstDisabled = CheckDaylightSavingTimeDisabled();
			bool flag = dstDisabled || CheckDaylightSavingTimeNotSupported(timeZone) || (timeZone._2A33 == registryTimeZoneInfo._2A33 && timeZone._2A32._2A36 == registryTimeZoneInfo._2A32._2A36 && timeZone._2A32._2A37 == registryTimeZoneInfo._2A32._2A37 && timeZone._2A32._2A38 == registryTimeZoneInfo._2A32._2A38 && timeZone._2A32._2A39 == registryTimeZoneInfo._2A32._2A39 && timeZone._2A32._2A3A == registryTimeZoneInfo._2A32._2A3A && timeZone._2A32._2A3B == registryTimeZoneInfo._2A32._2A3B && timeZone._2A32._2A3C == registryTimeZoneInfo._2A32._2A3C && timeZone._2A32._2A3D == registryTimeZoneInfo._2A32._2A3D);
			if (flag)
			{
				string strA = registryKey.GetValue("Std", string.Empty, RegistryValueOptions.None) as string;
				flag = string.Compare(strA, timeZone._2A2E, StringComparison.Ordinal) == 0;
			}
			return flag;
		}
		finally
		{
			PermissionSet.RevertAssert();
		}
	}

	private static bool TryCreateAdjustmentRules(string id, _2A34._2A40 defaultTimeZoneInformation, out AdjustmentRule[] rules, out Exception e)
	{
		e = null;
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(string.Format(CultureInfo.InvariantCulture, "{0}\\{1}\\Dynamic DST", new object[2] { "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", id }), RegistryKeyPermissionCheck.Default, RegistryRights.ExecuteKey);
			if (registryKey == null)
			{
				DateTime minValue = DateTime.MinValue;
				DateTime date = minValue.Date;
				DateTime maxValue = DateTime.MaxValue;
				AdjustmentRule adjustmentRule = CreateAdjustmentRuleFromTimeZoneInformation(defaultTimeZoneInformation, date, maxValue.Date);
				if (adjustmentRule == null)
				{
					rules = null;
				}
				else
				{
					rules = new AdjustmentRule[1] { adjustmentRule };
				}
				return true;
			}
			int num = (int)registryKey.GetValue("FirstEntry", -1, RegistryValueOptions.None);
			int num2 = (int)registryKey.GetValue("LastEntry", -1, RegistryValueOptions.None);
			if (num == -1 || num2 == -1 || num > num2)
			{
				rules = null;
				return false;
			}
			_2A34._2A40 _2A40 = new _2A34._2A40((byte[])registryKey.GetValue(num.ToString(CultureInfo.InvariantCulture), null, RegistryValueOptions.None));
			if (num == num2)
			{
				_2A34._2A40 timeZoneInformation = _2A40;
				DateTime minValue2 = DateTime.MinValue;
				DateTime date2 = minValue2.Date;
				DateTime maxValue2 = DateTime.MaxValue;
				AdjustmentRule adjustmentRule2 = CreateAdjustmentRuleFromTimeZoneInformation(timeZoneInformation, date2, maxValue2.Date);
				if (adjustmentRule2 == null)
				{
					rules = null;
				}
				else
				{
					rules = new AdjustmentRule[1] { adjustmentRule2 };
				}
				return true;
			}
			List<AdjustmentRule> list = new List<AdjustmentRule>(1);
			_2A34._2A40 timeZoneInformation2 = _2A40;
			DateTime minValue3 = DateTime.MinValue;
			AdjustmentRule adjustmentRule3 = CreateAdjustmentRuleFromTimeZoneInformation(timeZoneInformation2, minValue3.Date, new DateTime(num, 12, 31));
			if (adjustmentRule3 != null)
			{
				list.Add(adjustmentRule3);
			}
			for (int i = num + 1; i < num2; i++)
			{
				_2A40 = new _2A34._2A40((byte[])registryKey.GetValue(i.ToString(CultureInfo.InvariantCulture), null, RegistryValueOptions.None));
				AdjustmentRule adjustmentRule4 = CreateAdjustmentRuleFromTimeZoneInformation(_2A40, new DateTime(i, 1, 1), new DateTime(i, 12, 31));
				if (adjustmentRule4 != null)
				{
					list.Add(adjustmentRule4);
				}
			}
			_2A40 = new _2A34._2A40((byte[])registryKey.GetValue(num2.ToString(CultureInfo.InvariantCulture), null, RegistryValueOptions.None));
			_2A34._2A40 timeZoneInformation3 = _2A40;
			DateTime startDate = new DateTime(num2, 1, 1);
			DateTime maxValue3 = DateTime.MaxValue;
			AdjustmentRule adjustmentRule5 = CreateAdjustmentRuleFromTimeZoneInformation(timeZoneInformation3, startDate, maxValue3.Date);
			if (adjustmentRule5 != null)
			{
				list.Add(adjustmentRule5);
			}
			rules = list.ToArray();
			if (rules != null && rules.Length == 0)
			{
				rules = null;
			}
		}
		catch (InvalidCastException ex)
		{
			rules = null;
			e = ex;
			return false;
		}
		catch (ArgumentOutOfRangeException ex2)
		{
			rules = null;
			e = ex2;
			return false;
		}
		catch (ArgumentException ex3)
		{
			rules = null;
			e = ex3;
			return false;
		}
		return true;
	}

	[SecurityTreatAsSafe]
	[SecurityCritical]
	private static string TryGetLocalizedNameByMuiNativeResource(string resource)
	{
		if (string.IsNullOrEmpty(resource))
		{
			return string.Empty;
		}
		string[] array = resource.Split(new char[1] { ',' }, StringSplitOptions.None);
		if (array.Length != 2)
		{
			return string.Empty;
		}
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
		string path = array[0].TrimStart(new char[1] { '@' });
		string filePath;
		try
		{
			filePath = Path.Combine(folderPath, path);
		}
		catch (ArgumentException)
		{
			return string.Empty;
		}
		if (!int.TryParse(array[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return string.Empty;
		}
		result = -result;
		try
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			stringBuilder.Length = 260;
			int fileMuiPathLength = 260;
			int languageLength = 0;
			long enumerator = 0L;
			if (!_2A43.GetFileMUIPath(16, filePath, null, ref languageLength, stringBuilder, ref fileMuiPathLength, ref enumerator))
			{
				return string.Empty;
			}
			return TryGetLocalizedNameByNativeResource(stringBuilder.ToString(), result);
		}
		catch (EntryPointNotFoundException)
		{
			return string.Empty;
		}
	}

	[SecurityCritical]
	private static string TryGetLocalizedNameByNativeResource(string filePath, int resource)
	{
		if (File.Exists(filePath))
		{
			using _2A44 _2A442 = _2A43.LoadLibraryEx(filePath, IntPtr.Zero, 2);
			if (!_2A442.IsInvalid)
			{
				StringBuilder stringBuilder = new StringBuilder(500);
				stringBuilder.Length = 500;
				if (_2A43.LoadString(_2A442, resource, stringBuilder, stringBuilder.Length) != 0)
				{
					return stringBuilder.ToString();
				}
			}
		}
		return string.Empty;
	}

	private static bool TryGetLocalizedNamesByRegistryKey(RegistryKey key, out string displayName, out string standardName, out string daylightName)
	{
		displayName = string.Empty;
		standardName = string.Empty;
		daylightName = string.Empty;
		string text = key.GetValue("MUI_Display", string.Empty, RegistryValueOptions.None) as string;
		string text2 = key.GetValue("MUI_Std", string.Empty, RegistryValueOptions.None) as string;
		string text3 = key.GetValue("MUI_Dlt", string.Empty, RegistryValueOptions.None) as string;
		if (!string.IsNullOrEmpty(text))
		{
			displayName = TryGetLocalizedNameByMuiNativeResource(text);
		}
		if (!string.IsNullOrEmpty(text2))
		{
			standardName = TryGetLocalizedNameByMuiNativeResource(text2);
		}
		if (!string.IsNullOrEmpty(text3))
		{
			daylightName = TryGetLocalizedNameByMuiNativeResource(text3);
		}
		if (string.IsNullOrEmpty(displayName))
		{
			displayName = key.GetValue("Display", string.Empty, RegistryValueOptions.None) as string;
		}
		if (string.IsNullOrEmpty(standardName))
		{
			standardName = key.GetValue("Std", string.Empty, RegistryValueOptions.None) as string;
		}
		if (string.IsNullOrEmpty(daylightName))
		{
			daylightName = key.GetValue("Dlt", string.Empty, RegistryValueOptions.None) as string;
		}
		return true;
	}

	private static _2A45 TryGetTimeZone(string id, bool dstDisabled, out TimeZoneInfo value, out Exception e)
	{
		_2A45 result = _2A45._2A46;
		e = null;
		TimeZoneInfo value2 = null;
		if (s_systemTimeZones.TryGetValue(id, out value2))
		{
			if (dstDisabled && value2.m_supportsDaylightSavingTime)
			{
				value = CreateCustomTimeZone(value2.m_id, value2.m_baseUtcOffset, value2.m_displayName, value2.m_standardDisplayName);
				return result;
			}
			value = new TimeZoneInfo(value2.m_id, value2.m_baseUtcOffset, value2.m_displayName, value2.m_standardDisplayName, value2.m_daylightDisplayName, value2.m_adjustmentRules, disableDaylightSavingTime: false);
			return result;
		}
		if (!s_allSystemTimeZonesRead)
		{
			result = TryGetTimeZoneByRegistryKey(id, out value2, out e);
			if (result == _2A45._2A46)
			{
				s_systemTimeZones.Add(id, value2);
				if (dstDisabled && value2.m_supportsDaylightSavingTime)
				{
					value = CreateCustomTimeZone(value2.m_id, value2.m_baseUtcOffset, value2.m_displayName, value2.m_standardDisplayName);
					return result;
				}
				value = new TimeZoneInfo(value2.m_id, value2.m_baseUtcOffset, value2.m_displayName, value2.m_standardDisplayName, value2.m_daylightDisplayName, value2.m_adjustmentRules, disableDaylightSavingTime: false);
				return result;
			}
			value = null;
			return result;
		}
		result = _2A45._2A47;
		value = null;
		return result;
	}

	[SecurityTreatAsSafe]
	[SecurityCritical]
	private static _2A45 TryGetTimeZoneByRegistryKey(string id, out TimeZoneInfo value, out Exception e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		e = null;
		try
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission((IPermission?)new RegistryPermission((RegistryPermissionAccess)1, "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones"));
			permissionSet.Assert();
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", new object[2] { "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", id }), RegistryKeyPermissionCheck.Default, RegistryRights.ExecuteKey);
			if (registryKey == null)
			{
				value = null;
				return _2A45._2A47;
			}
			_2A34._2A40 defaultTimeZoneInformation;
			try
			{
				defaultTimeZoneInformation = new _2A34._2A40((byte[])registryKey.GetValue("TZI", null, RegistryValueOptions.None));
			}
			catch (InvalidCastException ex)
			{
				value = null;
				e = ex;
				return _2A45._2A48;
			}
			catch (ArgumentException ex2)
			{
				value = null;
				e = ex2;
				return _2A45._2A48;
			}
			if (!TryCreateAdjustmentRules(id, defaultTimeZoneInformation, out var rules, out e))
			{
				value = null;
				return _2A45._2A48;
			}
			if (!TryGetLocalizedNamesByRegistryKey(registryKey, out var displayName, out var standardName, out var daylightName))
			{
				value = null;
				return _2A45._2A48;
			}
			try
			{
				value = new TimeZoneInfo(id, new TimeSpan(0, -defaultTimeZoneInformation._2A2D, 0), displayName, standardName, daylightName, rules, disableDaylightSavingTime: false);
				return _2A45._2A46;
			}
			catch (ArgumentException ex3)
			{
				value = null;
				e = ex3;
				return _2A45._2A48;
			}
			catch (InvalidTimeZoneException ex4)
			{
				value = null;
				e = ex4;
				return _2A45._2A48;
			}
		}
		finally
		{
			PermissionSet.RevertAssert();
		}
	}

	internal static bool UtcOffsetOutOfRange(TimeSpan offset)
	{
		if (offset.TotalHours >= -14.0)
		{
			return offset.TotalHours > 14.0;
		}
		return true;
	}

	private static void ValidateTimeZoneInfo(string id, TimeSpan baseUtcOffset, AdjustmentRule[] adjustmentRules, out bool adjustmentRulesSupportDst)
	{
		adjustmentRulesSupportDst = false;
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if (id.Length == 0)
		{
			throw new ArgumentException("Argument_InvalidId , id");
		}
		if (UtcOffsetOutOfRange(baseUtcOffset))
		{
			throw new ArgumentOutOfRangeException("baseUtcOffset", "ArgumentOutOfRange_UtcOffset");
		}
		if (baseUtcOffset.Ticks % 600000000 != 0)
		{
			throw new ArgumentException("Argument_TimeSpanHasSeconds", "baseUtcOffset");
		}
		if (adjustmentRules == null || adjustmentRules.Length == 0)
		{
			return;
		}
		adjustmentRulesSupportDst = true;
		AdjustmentRule adjustmentRule = null;
		AdjustmentRule adjustmentRule2 = null;
		for (int i = 0; i < adjustmentRules.Length; i++)
		{
			adjustmentRule = adjustmentRule2;
			adjustmentRule2 = adjustmentRules[i];
			if (adjustmentRule2 == null)
			{
				throw new InvalidTimeZoneException("Argument_AdjustmentRulesNoNulls");
			}
			if (UtcOffsetOutOfRange(baseUtcOffset + adjustmentRule2.DaylightDelta))
			{
				throw new InvalidTimeZoneException("ArgumentOutOfRange_UtcOffsetAndDaylightDelta");
			}
			if (adjustmentRule != null && adjustmentRule2.DateStart <= adjustmentRule.DateEnd)
			{
				throw new InvalidTimeZoneException("Argument_AdjustmentRulesOutOfOrder");
			}
		}
	}
}

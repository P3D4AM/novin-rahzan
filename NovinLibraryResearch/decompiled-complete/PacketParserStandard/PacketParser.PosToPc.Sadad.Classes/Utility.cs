using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace PacketParser.PosToPc.Sadad.Classes;

public static class Utility
{
	[CompilerGenerated]
	private sealed class _003CTakeWhileFooter_003Ed__40<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<T> elements;

		public IEnumerable<T> _003C_003E3__elements;

		private Func<T, bool> predicate;

		public Func<T, bool> _003C_003E3__predicate;

		private List<T> _003Cts_003E5__2;

		private int _003Cnum1_003E5__3;

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
		public _003CTakeWhileFooter_003Ed__40(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cts_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			checked
			{
				T[] array3;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Cts_003E5__2 = new List<T>();
					_ = new byte[50];
					_003Cnum1_003E5__3 = 0;
					_003Cts_003E5__2.AddRange(elements);
					if (_003Cts_003E5__2.Count == 0)
					{
						return false;
					}
					goto IL_00a0;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00a0;
				case 2:
				{
					_003C_003E1__state = -1;
					T[] array = _003Cts_003E5__2.ToArray();
					_003C_003E2__current = (T)array.GetValue(_003Cnum1_003E5__3++);
					_003C_003E1__state = 3;
					return true;
				}
				case 3:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_00a0:
					if (predicate((T)_003Cts_003E5__2.ToArray().GetValue(_003Cnum1_003E5__3)))
					{
						T[] array2 = _003Cts_003E5__2.ToArray();
						_003C_003E2__current = (T)array2.GetValue(_003Cnum1_003E5__3++);
						_003C_003E1__state = 1;
						return true;
					}
					array3 = _003Cts_003E5__2.ToArray();
					_003C_003E2__current = (T)array3.GetValue(_003Cnum1_003E5__3++);
					_003C_003E1__state = 2;
					return true;
				}
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CTakeWhileFooter_003Ed__40<T> _003CTakeWhileFooter_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CTakeWhileFooter_003Ed__ = this;
			}
			else
			{
				_003CTakeWhileFooter_003Ed__ = new _003CTakeWhileFooter_003Ed__40<T>(0);
			}
			_003CTakeWhileFooter_003Ed__.elements = _003C_003E3__elements;
			_003CTakeWhileFooter_003Ed__.predicate = _003C_003E3__predicate;
			return _003CTakeWhileFooter_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	private static StreamWriter streamWriter;

	private static DateTime epoch;

	private const int MUST_BE_LESS_THAN = 100000000;

	public static string CommonProgramFilesx86
	{
		get
		{
			if (8 != IntPtr.Size && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))
			{
				return Environment.GetEnvironmentVariable("ProgramFiles") + "\\Common Files";
			}
			return Environment.GetEnvironmentVariable("ProgramFiles(x86)") + "\\Common Files";
		}
	}

	internal static StreamWriter LogFileWriter
	{
		get
		{
			if (streamWriter == null || streamWriter.BaseStream == null || !streamWriter.BaseStream.CanWrite)
			{
				FileInfo fileInfo = new FileInfo(LogPath);
				if (fileInfo.Exists && fileInfo.Length >= 10000000)
				{
					File.Copy(LogPath, LogPath.Replace(".log", $"{DateTime.Now}.log"));
					fileInfo.Delete();
				}
				streamWriter = File.AppendText(LogPath);
			}
			return streamWriter;
		}
	}

	public static string LogPath
	{
		get
		{
			try
			{
				string text = CommonProgramFilesx86 + "\\Sadad\\PcPos\\PcPosLog.log";
				if (!Directory.Exists(text + "\\..\\"))
				{
					Directory.CreateDirectory(text + "\\..\\");
				}
				return text;
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
	}

	public static string ProgramFilesx86
	{
		get
		{
			if (8 != IntPtr.Size && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))
			{
				return Environment.GetEnvironmentVariable("ProgramFiles");
			}
			return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
		}
	}

	static Utility()
	{
		epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	}

	public static string BCDToString(byte[] b, int offset, int len, bool padLeft)
	{
		StringBuilder stringBuilder = new StringBuilder(len);
		int num = (((len & 1) == 1 && padLeft) ? 1 : 0);
		for (int i = num; i < checked(len + num); i = checked(i + 1))
		{
			int num2 = (((i & 1) != 1) ? 4 : 0);
			char c = (char)checked((ushort)(((b[offset + (i >> 1)] >> num2) & 0xF) + 48));
			if (c == 'd')
			{
				c = '=';
			}
			stringBuilder.Append(char.ToUpper(c));
		}
		return stringBuilder.ToString();
	}

	public static string BCDToString2(byte[] b, int offset, int len, bool padLeft)
	{
		StringBuilder stringBuilder = new StringBuilder(len);
		int num = (((len & 1) == 1 && padLeft) ? 1 : 0);
		for (int i = num; i < checked(len + num); i = checked(i + 1))
		{
			int num2 = (((i & 1) != 1) ? 4 : 0);
			char c = (char)checked((ushort)(((b[offset + (i >> 1)] >> num2) & 0xF) + 48));
			stringBuilder.Append(char.ToUpper(c));
		}
		return stringBuilder.ToString();
	}

	public static short[] ByteArrayToInt16Array(byte[] bytes)
	{
		try
		{
			short[] array = new short[bytes.Length / 2];
			for (int i = 0; i < bytes.Length; i = checked(i + 2))
			{
				array[i / 2] = BitConverter.ToInt16(bytes, i);
			}
			return array;
		}
		catch (Exception)
		{
			throw;
		}
	}

	internal static byte[] ComputeHashOfFile(string fileName)
	{
		try
		{
			_ = new byte[1];
			HashAlgorithm hashAlgorithm = HashAlgorithm.Create();
			using FileStream inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			return hashAlgorithm.ComputeHash(inputStream);
		}
		catch (Exception exception)
		{
			LogPcPos(exception);
			return new byte[1];
		}
	}

	public static string ConvertToBinaryCodedDecimal(bool isLittleEndian, params byte[] bytes)
	{
		checked
		{
			StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
			if (!isLittleEndian)
			{
				for (int i = 0; i < bytes.Length; i++)
				{
					byte b = bytes[i];
					int num = b >> 4;
					int num2 = b & 0xF;
					if (num > 9 || num2 > 9)
					{
						throw new ArgumentException($"One of the argument bytes was not in binary-coded decimal format: byte[{i}] = 0x{b:X2}.");
					}
					stringBuilder.Append($"{num}{num2}");
				}
			}
			else
			{
				for (int num3 = bytes.Length - 1; num3 >= 0; num3--)
				{
					byte b2 = bytes[num3];
					int num4 = b2 >> 4;
					int num5 = b2 & 0xF;
					if (num4 > 9 || num5 > 9)
					{
						throw new ArgumentException($"One of the argument bytes was not in binary-coded decimal format: byte[{num3}] = 0x{b2:X2}.");
					}
					stringBuilder.Append($"{num4}{num5}");
				}
			}
			return stringBuilder.ToString();
		}
	}

	public static byte[] ConvertToBinaryCodedDecimal(bool isLittleEndian, string bcdString)
	{
		bool num = !string.IsNullOrEmpty(bcdString);
		if (!Regex.IsMatch(bcdString, "^([0-9]{2})+$"))
		{
			bcdString = "0" + bcdString;
		}
		if (!num || !Regex.IsMatch(bcdString, "^([0-9]{2})+$"))
		{
			throw new ArgumentException($"Input string ({bcdString}) was invalid.");
		}
		char[] array = bcdString.ToCharArray();
		int num2 = array.Length / 2;
		byte[] array2 = new byte[num2];
		checked
		{
			if (!isLittleEndian)
			{
				for (int i = 0; i < num2; i++)
				{
					byte b = byte.Parse(array[2 * i].ToString());
					byte b2 = byte.Parse(array[2 * i + 1].ToString());
					array2[i] = (byte)((byte)(b << 4) | b2);
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					byte b3 = byte.Parse(array[2 * (num2 - 1) - 2 * j].ToString());
					byte b4 = byte.Parse(array[2 * (num2 - 1) - 2 * j + 1].ToString());
					array2[j] = (byte)((byte)(b3 << 4) | b4);
				}
			}
			return array2;
		}
	}

	public static IEnumerable<T> Distinct<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer)
	{
		try
		{
			List<T> list = new List<T>();
			if (collection != null)
			{
				object obj = comparer;
				if (obj == null)
				{
					obj = EqualityComparer<T>.Default;
				}
				comparer = (IEqualityComparer<T>)obj;
				foreach (T item in collection)
				{
					bool flag = false;
					foreach (T item2 in list)
					{
						if (comparer.Equals(item, item2))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						list.Add(item);
					}
				}
				return list;
			}
			return list;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static IEnumerable<T> Distinct<T>(IEnumerable<T> collection)
	{
		return Distinct(collection, null);
	}

	public static IEnumerable<T> Empty<T>()
	{
		return new List<T>();
	}

	public static string ErrorMessage(int errorNo)
	{
		return errorNo switch
		{
			0 => "پاسخی دریافت نشد", 
			1 => "خطا حداکثر دفعات تکرار", 
			2 => "لغو تراکنش توسط پی سی پوز", 
			3 => "لغو تراکنش توسط کاربر پوز", 
			4 => "ارتباط موفق با دستگاه", 
			5 => "خطای داخلی", 
			6 => "خطای درخواست همزمان", 
			_ => "خطای ناشناخته", 
		};
	}

	public static DateTime FromUnixTime(double timestamp)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp).ToLocalTime();
	}

	public static double GetCurrentTimeStamp()
	{
		return ToUnixTime(DateTime.Now);
	}

	public static byte GetDigit(byte b)
	{
		checked
		{
			if (b >= 48 && b <= 57)
			{
				return (byte)(b - 48);
			}
			if (b < 65 || b > 70)
			{
				throw new Exception("Invalid Hex String");
			}
			return (byte)(10 + b - 65);
		}
	}

	internal static byte GetLRC(byte[] input)
	{
		int num = input.Length;
		int num2 = 0;
		checked
		{
			for (int i = 0; i < num; i++)
			{
				num2 ^= (byte)input.GetValue(i);
			}
			num2 ^= 3;
			return (byte)(num2 & 0xFF);
		}
	}

	public static string GetPersianPcDate()
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		return persianCalendar.GetYear(DateTime.Now).ToString("D4") + "/" + persianCalendar.GetMonth(DateTime.Now).ToString("D2") + "/" + persianCalendar.GetDayOfMonth(DateTime.Now).ToString("D2");
	}

	public static string GetPersianPcDate(DateTime date)
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		return persianCalendar.GetYear(date).ToString("D4") + "/" + persianCalendar.GetMonth(date).ToString("D2") + "/" + persianCalendar.GetDayOfMonth(date).ToString("D2");
	}

	public static string GetPersianPcTime()
	{
		PersianCalendar persianCalendar = new PersianCalendar();
		return persianCalendar.GetHour(DateTime.Now).ToString("D2") + ":" + persianCalendar.GetMinute(DateTime.Now).ToString("D2") + ":" + persianCalendar.GetSecond(DateTime.Now).ToString("D2");
	}

	public static string GetResponseCodeMessage(string pResponseCode)
	{
		if (string.IsNullOrEmpty(pResponseCode))
		{
			return string.Empty;
		}
		int result = -1;
		int.TryParse(pResponseCode, out result);
		return pResponseCode switch
		{
			"460" => "موفق: تراکنش به درستي انجام شده است", 
			"461" => "ناموفق: تراکنش به دليل کد پاسخ غير صفر از مرکز، ناموفق بوده است", 
			"462" => "ناموفق: اشکال در ارتباط.", 
			"463" => "ناموفق: خط تلفن اشغال است.", 
			"464" => "ناموفق: اشکال در خط تلفن.", 
			"465" => "ناموفق: شماره تلفن تنظيم نيست.", 
			"466" => "ناموفق: تراکنش کنسل شد.", 
			"467" => "ناموفق: اشکال در خواندن کارت.", 
			"468" => "ناموفق: پاسخ از مرکز دريافت نشد. اصلاحيه تراکنش ارسال مي شود.", 
			"469" => "ناموفق: غيره.", 
			"470" => "ناموفق: کد ترمينال وارد شده معتبر نمي باشد.", 
			"471" => "ناموفق: در استعلام تراکنش بصورت offline تراکنش پيدا نشد.", 
			"472" => "دريافت اطلاعات نامعتبر از دستگاه POS از طريق پورت سريال.", 
			"473" => "وقوع Timeout و عدم دريافت پاسخ از POS در مهلت زماني مشخص (حداکثر 150 ثانيه).", 
			"475" => "اصلاحيه تراکنش قبلا انجام شده است. (در پاسخ به ارسال اصلاحيه).", 
			"476" => "پذيرنده کارت شناخته شده نيست. تراکنش بازکردن سازمان را انجام دهيد.", 
			"477" => "اشکال در سيستم مرکزي.", 
			"478" => "پيام نادرست است. يکي از دلايل اين خطا عدم مجوز پايانه در انجام عمليات مورد نظر مي باشد.", 
			"479" => "مقدار ريالي غلط است. (مبلغ تراکنش صحيح نمي باشد).", 
			"480" => "شماره کارت شناخته شده نيست. (کارت در سيستم خدمات کارت نا معتبر است).", 
			"481" => "در پاسخ به تراکنش باز کردن سازمان: قبلا باز شده است.", 
			"482" => "در پاسخ به تراکنش بستن سازمان: قبلا بسته شده است.", 
			"483" => "در پاسخ به تراکنش تراز جاري پايانه: مجمور تراکنش هاي پايانه با مجمور تراکنش هاي مرکز برابر نيست.", 
			"484" => "داده اي از تراکنش ارسالي غلط است و اين داده در مرکز پيدا نشد. يکي از اين موارد شماره پايانه است.", 
			"485" => "فرمت اشتباه است.", 
			"486" => "تاريخ انقضاي کارت سپري شده است.", 
			"487" => "تراکنش مالي تاييد نشده است.", 
			"488" => "کارت به علت مفقود شدن مسدود موقت است.", 
			"489" => "کارت به علت دزديده شدن دايما مسدود است.", 
			"490" => "کارت فاقد موجودي کافي جهت انجام تراکنش مالي است.", 
			"491" => "رمز نامعتبر.", 
			"492" => "کارت نامعتبر.", 
			"493" => "تراکنش غير مجاز.", 
			"494" => "مبلغ تراکنش هاي اين کارت از حد مجاز گذشته است.", 
			"495" => "مبلغ تراکنش روز اين کارت از حد مجاز گذشته است.", 
			"496" => "تعداد تراکنش هاي مربوط به اين کارت از حد مجاز گذشته است.", 
			"497" => "حساب مسدود است.", 
			"498" => "تعداد رمز نا معتبر از حد مجاز گذشته است.", 
			"499" => "روز مالي صحيح نيست.", 
			"500" => "کارت فعال نيست.", 
			"501" => "حساب تعريف نشده است.", 
			"502" => "به دليل اشکال در حساب تراکنش پذيرفته نيست.", 
			"503" => "کارت قبلا باطل شده است.", 
			"504" => "سيستم مرکزي آماده نيست.", 
			"505" => "سيستم مرکزي فعال نيست.", 
			"506" => "خطاي دروني سيستم مرکزي.", 
			"507" => "تراکنش روي اين دستگاه مجاز نيست.", 
			"508" => "خطاي دروني سيستم مرکزي.", 
			"509" => "خطاي دروني سيستم مرکزي.", 
			"510" => "خطاي دروني سيستم مرکزي.", 
			"511" => "ارتباط در حين پردازش قطع شده است.", 
			"512" => "عدم دريافت پاسخ به علت وقوع Timeout در مرکز و اعلام آن توسط مرکز به دستگاه.", 
			"513" => "صادر کننده نا معتبر است ", 
			"514" => "به علت ترافيک تراکنشها صف تراکنشها در سيستم مرکزي پر شده است.", 
			"515" => "شماره تراکنش تکراري است.", 
			"516" => "اشکال در سيستم مرکزي.", 
			"517" => "طول شماره پایانه مجاز نمیباشد.", 
			"518" => "طول شماره پذیرنده مجاز نمیباشد.", 
			"519" => "طول شماره قبض مجاز نمیباشد.", 
			"520" => "طول مبلغ مجاز نمیباشد.", 
			"521" => "طول تاریخ ورودی مجاز نمیباشد.", 
			"522" => "طول زمان ورودی مجاز نمیباشد.", 
			"523" => "خطا در ارسال اطلاعات به دستگاه پوز.", 
			"524" => "پورت سریال بسته است.", 
			"525" => "پورت سریال اشغال است.", 
			"526" => "نام پورت سریال وارد نشده است.", 
			"527" => "خطا در درسترسی غیرمجاز به پورت های ورودی و خروجی سیستم.", 
			"528" => "خطا در باز کردن پورت سریال.", 
			"529" => "پارامترهای ورودی باز کردن پورت سریال اشتباه است.", 
			"530" => "خطا در باز کردن پورت سریال.", 
			_ => "خطا ناشناخته", 
		};
	}

	internal static int GetStableHash(string input)
	{
		uint num = 0u;
		byte[] bytes = Encoding.Unicode.GetBytes(input);
		checked
		{
			for (int i = 0; i < bytes.Length; i++)
			{
				num += bytes[i];
				num += num << 10;
				num ^= num >> 6;
			}
			num += num << 3;
			num ^= num >> 11;
			num += num << 15;
			return (int)unchecked(num % 100000000);
		}
	}

	public static bool IsNullOrWhiteSpace(string value)
	{
		if (value == null)
		{
			return true;
		}
		for (int i = 0; i < value.Length; i = checked(i + 1))
		{
			if (!char.IsWhiteSpace(value[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsNumericType(object o)
	{
		if (checked(Type.GetTypeCode(o.GetType()) - 5) <= TypeCode.UInt32)
		{
			return true;
		}
		return false;
	}

	public static void LogPcPos(string logMessage)
	{
	}

	public static void LogPcPos(Exception exception)
	{
	}

	internal static T RequestPcPosWebService<T>(string url, string data) where T : new()
	{
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		httpWebRequest.Method = "POST";
		httpWebRequest.ContentType = "application/json";
		httpWebRequest.ContentLength = data.Length;
		using (Stream stream = httpWebRequest.GetRequestStream())
		{
			using StreamWriter streamWriter = new StreamWriter(stream, Encoding.ASCII);
			streamWriter.Write(data);
		}
		try
		{
			using (Stream stream2 = httpWebRequest.GetResponse().GetResponseStream())
			{
				if (stream2 != null)
				{
					using (StreamReader streamReader = new StreamReader(stream2))
					{
						return JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd());
					}
				}
			}
			return new T();
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static byte[] StringToBCD(string s, bool padLeft)
	{
		byte[] d = new byte[checked(s.Length + 1) >> 1];
		return StringToBCD(s, padLeft, d, 0);
	}

	public static byte[] StringToBCD(string s, bool padLeft, int outputLen)
	{
		int length = s.Length;
		checked
		{
			if (outputLen < length + 1 >> 1)
			{
				outputLen = length + 1 >> 1;
			}
			byte[] d = new byte[length + 1 >> 1];
			byte[] array = new byte[outputLen];
			d = StringToBCD(s, padLeft, d, 0);
			Array.Copy(d, 0, array, outputLen - d.Length, d.Length);
			return array;
		}
	}

	public static byte[] StringToBCD(string s, bool padLeft, byte[] d, int offset)
	{
		int length = s.Length;
		int num = (((length & 1) == 1 && padLeft) ? 1 : 0);
		checked
		{
			for (int i = num; i < length + num; i++)
			{
				d[offset + (i >> 1)] = (byte)(d[offset + (i >> 1)] | (byte)(GetDigit((byte)s[i - num]) << (((i & 1) != 1) ? 4 : 0)));
			}
			return d;
		}
	}

	public static byte[] StringToBCD2(string s, bool padLeft, byte[] d, int offset)
	{
		int length = s.Length;
		int num = (((length & 1) == 1 && padLeft) ? 1 : 0);
		checked
		{
			for (int i = num; i < length + num; i++)
			{
				int num2 = offset + (i >> 1);
				byte b = d[num2];
				byte digit = GetDigit((byte)s[i - num]);
				byte b2 = (((i & 1) != 1) ? ((byte)(digit << 4)) : digit);
				d[num2] = (byte)(b | b2);
			}
			return d;
		}
	}

	[IteratorStateMachine(typeof(_003CTakeWhileFooter_003Ed__40<>))]
	internal static IEnumerable<T> TakeWhileFooter<T>(IEnumerable<T> elements, Func<T, bool> predicate)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CTakeWhileFooter_003Ed__40<T>(-2)
		{
			_003C_003E3__elements = elements,
			_003C_003E3__predicate = predicate
		};
	}

	public static string ToN16Hash(string input, short digitForRound)
	{
		if (string.IsNullOrEmpty(input))
		{
			return GetStableHash(input).ToString();
		}
		int num = Convert.ToInt32(Math.Round((double)input.Length / (double)digitForRound));
		int stableHash = GetStableHash(input.Substring(0, num));
		int stableHash2 = GetStableHash(input.Substring(num));
		return stableHash.ToString() + stableHash2;
	}

	public static string ToPersianDate(string date)
	{
		Match match = Regex.Match(date, "\\d+", RegexOptions.Compiled);
		PersianCalendar persianCalendar = new PersianCalendar();
		DateTime result = DateTime.Now;
		StringBuilder stringBuilder = new StringBuilder(100000);
		switch (match.Value.Length)
		{
		case 6:
			stringBuilder.Append(match.Value.Substring(0, 2));
			stringBuilder.Append("/");
			stringBuilder.Append(match.Value.Substring(2, 2));
			stringBuilder.Append("/");
			stringBuilder.Append(match.Value.Substring(4, 2));
			break;
		case 7:
		case 9:
			return persianCalendar.GetYear(result).ToString("D4") + "/" + persianCalendar.GetMonth(result).ToString("D2") + "/" + persianCalendar.GetDayOfMonth(result).ToString("D2");
		case 8:
			stringBuilder.Append(match.Value.Substring(0, 4));
			stringBuilder.Append("/");
			stringBuilder.Append(match.Value.Substring(4, 2));
			stringBuilder.Append("/");
			stringBuilder.Append(match.Value.Substring(6, 2));
			break;
		case 10:
			stringBuilder.Append(match.Value.Substring(0, 4));
			stringBuilder.Append("/");
			stringBuilder.Append(match.Value.Substring(6, 2));
			stringBuilder.Append("/");
			stringBuilder.Append(match.Value.Substring(8, 2));
			break;
		default:
			return persianCalendar.GetYear(result).ToString("D4") + "/" + persianCalendar.GetMonth(result).ToString("D2") + "/" + persianCalendar.GetDayOfMonth(result).ToString("D2");
		}
		DateTime.TryParse(stringBuilder.ToString(), out result);
		return persianCalendar.GetYear(result).ToString("D4") + "/" + persianCalendar.GetMonth(result).ToString("D2") + "/" + persianCalendar.GetDayOfMonth(result).ToString("D2");
	}

	public static double ToUnixTime(DateTime dateTime)
	{
		return Math.Round((dateTime.ToUniversalTime() - epoch).TotalSeconds);
	}

	public static bool ValidateIpAddress(string pIp)
	{
		if (string.IsNullOrEmpty(pIp))
		{
			return false;
		}
		IPAddress address;
		return IPAddress.TryParse(pIp, out address);
	}
}

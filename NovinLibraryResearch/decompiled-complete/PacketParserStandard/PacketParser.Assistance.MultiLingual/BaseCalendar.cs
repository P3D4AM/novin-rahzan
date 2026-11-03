using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.Assistance.MultiLingual;

public abstract class BaseCalendar : ICalendar
{
	private SemaphoreSlim _fileReadSemaphore = new SemaphoreSlim(1, 1);

	public ConcurrentDictionary<string, string> LstLangauge { get; set; }

	public abstract DayOfWeek StartDayOfWeek { get; }

	public string Content { get; }

	internal string GetPartday_(string date, EnPartDate type)
	{
		try
		{
			string text = date.Substring(0, 4);
			string text2 = date.Substring(5, 2);
			string text3 = date.Substring(8, 2);
			return type switch
			{
				EnPartDate.Year => text, 
				EnPartDate.Mounth => text2, 
				EnPartDate.Day => text3, 
				_ => "0", 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPartday_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 37);
			return "0";
		}
	}

	internal int GetDayOfDate_(string dateSh)
	{
		int num = 0;
		try
		{
			if (dateSh == "")
			{
				return 0;
			}
			num = 1;
			List<string> list = dateSh.SplitString();
			num = 2;
			return (list.Count > 2) ? list[2].ParseToInt() : 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"The Error Line IS: {num} ", "GetDayOfDate_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 54);
			return 0;
		}
	}

	internal string Minutes2TimeFormat_(int min)
	{
		try
		{
			min = Math.Abs(min);
			int num = min / 60;
			int num2 = min % 60;
			string text = num.ToString();
			if (num < 10)
			{
				text = "0" + num;
			}
			text += ":";
			return (num2 <= 9) ? (text + "0" + num2) : (text + num2);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Minutes2TimeFormat_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 77);
			return "00:00";
		}
	}

	internal int GetYearOfDate_(string dateSh)
	{
		try
		{
			string text = "";
			if (string.IsNullOrEmpty(dateSh))
			{
				return 0;
			}
			if (dateSh.Split(new char[1] { '/' }).Length <= 2)
			{
				return 0;
			}
			text = dateSh.Split(new char[1] { '/' })[0];
			if (text == "00")
			{
				text = "1400";
			}
			return int.Parse(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetYearOfDate_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 95);
			return 0;
		}
	}

	internal int GetMonthOfDate_(string date)
	{
		try
		{
			if (string.IsNullOrEmpty(date))
			{
				return 0;
			}
			if (date.Split(new char[1] { '/' }).Length <= 2)
			{
				return 0;
			}
			return int.Parse(date.Split(new char[1] { '/' })[1]);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMonthOfDate_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 110);
			return 0;
		}
	}

	public string ToLongTimeString_(DateTime dateM)
	{
		try
		{
			return dateM.ToLongTimeString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToLongTimeString_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 123);
			return "00:00";
		}
	}

	public string DateToFormatTime_(DateTime dateM)
	{
		checked
		{
			try
			{
				int hour = dateM.Hour;
				int minute = dateM.Minute;
				if (dateM.Second <= 30)
				{
					return hour.ToString("00") + ":" + minute.ToString("00");
				}
				minute++;
				if (minute != 60)
				{
					return hour.ToString("00") + ":" + minute.ToString("00");
				}
				return (hour + 1).ToString("00") + ":" + 0.ToString("00");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DateToFormatTime_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\PacketParser\\Calendar.cs", 645);
				return "";
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<ConcurrentDictionary<string, string>>> GetAll(string content)
	{
		ReturnedSaveFuncInfoWithValue<ConcurrentDictionary<string, string>> ret = new ReturnedSaveFuncInfoWithValue<ConcurrentDictionary<string, string>>();
		try
		{
			await _fileReadSemaphore.WaitAsync();
			ReturnedSaveFuncInfoWithValue<ConcurrentDictionary<string, string>> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await LoadFromContentAsync(Content), ReturnedState.None, "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 153);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 157);
			ret.AddReturnedValue(ex, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 158);
		}
		finally
		{
			_fileReadSemaphore.Release();
		}
		return ret;
	}

	private async Task GetAllKey()
	{
		try
		{
			if (LstLangauge == null)
			{
				ReturnedSaveFuncInfoWithValue<ConcurrentDictionary<string, string>> returnedSaveFuncInfoWithValue = await GetAll(Content);
				if (!returnedSaveFuncInfoWithValue.HasError)
				{
					LstLangauge = returnedSaveFuncInfoWithValue.value;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllKey", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 172);
		}
	}

	public string GetValue(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return "";
		}
		if (LstLangauge == null)
		{
			return key;
		}
		try
		{
			string value;
			return LstLangauge.TryGetValue(key, out value) ? value : key;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 185);
			return key;
		}
	}

	private Task<ReturnedSaveFuncInfoWithValue<ConcurrentDictionary<string, string>>> LoadFromContentAsync(string Content)
	{
		ReturnedSaveFuncInfoWithValue<ConcurrentDictionary<string, string>> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<ConcurrentDictionary<string, string>>
		{
			value = new ConcurrentDictionary<string, string>()
		};
		try
		{
			string[] array = Content.Split(new char[1] { '\n' });
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim(new char[1] { '\r' });
				if (!string.IsNullOrEmpty(text))
				{
					Language language = new Language(text);
					if (!string.IsNullOrEmpty(language.KeyLang))
					{
						returnedSaveFuncInfoWithValue.value.TryAdd(language.KeyLang, language.ValueLang);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadFromContentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 208);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "LoadFromContentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 209);
		}
		return Task.FromResult(returnedSaveFuncInfoWithValue);
	}

	public DateTime GetFirstDayOfWeek(DateTime dateM, string time)
	{
		checked
		{
			try
			{
				time = time?.KeepCharacters("01234567890:");
				DateTime dateTime = dateM;
				if (dateM.DayOfWeek > StartDayOfWeek)
				{
					dateTime = dateM.AddDays(dateM.DayOfWeek - StartDayOfWeek);
				}
				else if (dateM.DayOfWeek < StartDayOfWeek)
				{
					dateTime = dateM.AddDays(7 - StartDayOfWeek - dateM.DayOfWeek);
				}
				short result = 0;
				short result2 = 0;
				short result3 = 0;
				if (!string.IsNullOrEmpty(time) && time.Contains(":"))
				{
					string[] array = time.Split(new char[1] { ':' });
					short.TryParse(array[0], out result);
					short.TryParse(array[1], out result3);
					if (array.Length >= 3)
					{
						short.TryParse(array[2], out result2);
					}
				}
				else if ((time?.Length ?? 0) == 4)
				{
					result = time.Substring(0, 2).ParseToShort(0);
					result3 = time.Substring(2, 2).ParseToShort(0);
				}
				return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, result, result3, result2);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFirstDayOfWeek", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 240);
				return dateM;
			}
		}
	}

	public DateTime GetEndDayOfWeek(DateTime dateM, string time)
	{
		return GetFirstDayOfWeek(dateM, time).AddDays(7.0);
	}

	public virtual int GetWeekNumber(DateTime dateM, CalendarWeekRule role = CalendarWeekRule.FirstFullWeek)
	{
		try
		{
			return new PersianCalendar().GetWeekOfYear(dateM, role, StartDayOfWeek);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetWeekNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\BaseCalendar.cs", 254);
			return 0;
		}
	}

	public BaseCalendar(string content)
	{
		Content = content;
		Task.Run((Func<Task?>)GetAllKey);
	}

	public abstract string GetDayNameOfWeek(DateTime dateM);

	public abstract string[] GetListOfMonth();

	public abstract DateTime CalendarToMiladi(string dateParam);

	public abstract bool CheckDate(string dateSh);

	public abstract string MiladiToCalendar(DateTime dateM);

	public abstract string ToShortDate(DateTime dateM);

	public abstract int EndDayOfMonthInt(short civilMonth, int civilyear);

	public abstract string GetMonthName(string month);

	public abstract string GetMonthName(DateTime date);

	public abstract string ToLongDateWithoutYear(DateTime dateM);

	public abstract string ToLongTimeString(DateTime dateM);

	public abstract DateTime EndDayOfMonth(string Date, string time);

	public abstract DateTime EndDayOfMonth(short Month, int Year, string time);

	public abstract DateTime EndDayOfMonth();

	public abstract DateTime StartDayOfMonth();

	public abstract DateTime StartDayOfMonth(string Date, string time);

	public abstract DateTime StartDayOfMonth(short Month, int Year, string time);

	public abstract string Today();

	public abstract string AddToDate(int count, string date, EnPartDate part);

	public abstract string AddYear(string Date, int yearCount);

	public abstract string GetPartday(string date, EnPartDate type);

	public abstract int GetDayOfDate(string dateSh);

	public abstract string Minutes2TimeFormat(int min);

	public abstract int GetYearOfDate(string dateSh);

	public abstract int GetMonthOfDate(string date);

	public abstract string AddMonth(string Date, int Count);

	public abstract string MiladitoCalenderText(DateTime dateM);

	public abstract string DateToFormatTime(DateTime dateM);

	public virtual void Dispose()
	{
		throw new NotImplementedException();
	}
}

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Force.Crc32;
using Microsoft.Win32;
using PacketParser.Assistance;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.CoreChangeLogger;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;

namespace PacketParser;

public static class Utilities
{
	private const int _chunckSizeKB = 100000;

	public static readonly DateTime MinValidDateTime = DateTime.Parse("1900/01/01 00:00:00");

	private static ConcurrentDictionary<string, int> cach = new ConcurrentDictionary<string, int>();

	public static string CompareObject<T>(this T oldObj, T newObj)
	{
		return ObjectComparer.CompareObjects(oldObj, newObj);
	}

	public static LanguageTypes GetLanguage()
	{
		int num = 0;
		try
		{
			num = short.Parse((Registry.GetValue("HKEY_CURRENT_USER\\Software\\Novin\\LanguageSetting", "Language", "0") ?? "0").ToString());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLanguage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 49);
		}
		return (LanguageTypes)checked((short)num);
	}

	public static void SetLanguage(LanguageTypes language)
	{
		try
		{
			short num = (short)language;
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin\\LanguageSetting", "Language", num.ToString());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetLanguage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 61);
		}
	}

	public static string GetUserFriendlyTextForSendSmsErrorCode(this long errorCode)
	{
		try
		{
			return errorCode switch
			{
				-1L => " -نام کاربری یا رمز عبور اشتباه است", 
				-2L => " -خطا در دریافت داده ه", 
				-3L => "شماره اختصاصی اشتباه است", 
				-4L => "فرمت داده های ورودی اشتباه است", 
				-5L => "شناسه پیامک اشتباه است", 
				-6L => "آرایه ورودی شناسه ها خالی است", 
				-7L => "فرمت تاریخ اشتباه است", 
				-8L => "تاریخ یا ساعت ورودی مربوط به گذشته است", 
				-9L => "وب سرویس برای این کاربر فعال نمی باشد", 
				-10L => "تعداد آرایه ورودی موبایل اشتباه است", 
				-11L => "متن پیام خالی است", 
				-18L => "ن خطا به هنگام چک کردن وضعیت پیام های ارسال درآینده می باشد و زمانی که smsidخالی باشد", 
				-110L => "خطا در ارتباط با سرور بک اند.", 
				-111L => "خطا در پارس کردن اطلاعات دریافت شده.", 
				_ => "کد خطا تعریف نشده است", 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetUserFriendlyTextForSendSmsErrorCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 102);
			return "Error";
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FileTryReplceadeAsync(string FullFilePath, byte[] data, short tryCount = 10)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (File.Exists(FullFilePath))
			{
				File.Delete(FullFilePath);
			}
			File.WriteAllBytes(FullFilePath, data);
			return returnedSaveFuncInfo;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FileTryReplceadeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 120);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "FileTryReplceadeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 121);
		}
		try
		{
			File.WriteAllBytes(FullFilePath, data);
			return new ReturnedSaveFuncInfo();
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "FileTryReplceadeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 133);
			returnedSaveFuncInfo.AddReturnedValue(ex2, "", "FileTryReplceadeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 134);
		}
		if (tryCount > 0 && returnedSaveFuncInfo.HasError)
		{
			await Task.Delay(5000);
			short num = checked((short)(tryCount - 1));
			tryCount = num;
			return await FileTryReplceadeAsync(FullFilePath, data, num);
		}
		return returnedSaveFuncInfo;
	}

	public static string ToBase64(this string data)
	{
		try
		{
			return Convert.ToBase64String(Encoding.Unicode.GetBytes(data));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToBase64", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 152);
			return "";
		}
	}

	public static string FromBase64(this string data)
	{
		try
		{
			return Encoding.Unicode.GetString(Convert.FromBase64String(data));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FromBase64", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 164);
			return "";
		}
	}

	public static string AccountFormated(this decimal data, bool shortFormat)
	{
		try
		{
			if (data > 0m)
			{
				return data.NpToString("N0", 0) + " " + (shortFormat ? "بس" : " بستانکار");
			}
			if (data < 0m)
			{
				return (-data).NpToString("N0", 0) + " " + (shortFormat ? "بد" : " بدهکار");
			}
			return "بی حساب".GetTranslate();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AccountFormated", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 179);
			return "";
		}
	}

	public static string NpToString(this decimal data, string format = "", short decimalRound = 0)
	{
		try
		{
			string text = data.ToString().Replace(".000000", "").Replace(".00000", "")
				.Replace(".000", "")
				.Replace(".0", "")
				.Replace(".", "/");
			if (string.IsNullOrEmpty(format) && decimalRound <= 0)
			{
				text = data.ToString().Replace(".000000", "").Replace(".00000", "")
					.Replace(".000", "")
					.Replace(".0", "")
					.Replace(".", "/");
			}
			else if (!string.IsNullOrEmpty(format) && decimalRound <= 0)
			{
				text = data.ToString().Replace(".000000", "").Replace(".00000", "")
					.Replace(".000", "")
					.Replace(".0", "")
					.Replace(".", "/");
				if (!text.Contains("/"))
				{
					text = data.ToString(format);
				}
			}
			else if (string.IsNullOrEmpty(format) && decimalRound > 0)
			{
				text = data.ToString().Replace(".000000", "").Replace(".00000", "")
					.Replace(".000", "")
					.Replace(".0", "")
					.Replace(".", "/");
				if (!text.Contains("/"))
				{
					text = data.ToString(decimalRound);
				}
			}
			if (data - Math.Floor(data) > 0m)
			{
				while (text.EndsWith(".0") || text.EndsWith("/0"))
				{
					text = text.Substring(0, checked(text.Length - 1));
				}
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NpToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 211);
			throw;
		}
	}

	public static string NpToString(this double data, string format = "", short decimalRound = 0)
	{
		try
		{
			string text = data.ToString().Replace(".000000", "").Replace(".000", "")
				.Replace(".", "/");
			if (string.IsNullOrEmpty(format) && decimalRound <= 0)
			{
				text = data.ToString().Replace(".000000", "").Replace(".000", "")
					.Replace(".", "/");
			}
			else if (!string.IsNullOrEmpty(format) && decimalRound <= 0)
			{
				text = data.ToString().Replace(".000000", "").Replace(".000", "")
					.Replace(".", "/");
				if (!text.Contains("/"))
				{
					text = data.ToString(format);
				}
			}
			else if (string.IsNullOrEmpty(format) && decimalRound > 0)
			{
				text = data.ToString().Replace(".000000", "").Replace(".000", "")
					.Replace(".", "/");
				if (!text.Contains("/"))
				{
					text = data.ToString(decimalRound);
				}
			}
			while (text.EndsWith(".0") || text.EndsWith("/0"))
			{
				text = text.Substring(0, checked(text.Length - 1));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NpToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 241);
			throw;
		}
	}

	public static string NpToString(this long data, string format = "")
	{
		try
		{
			string text = data.ToString().Replace(".000000", "").Replace(".000", "")
				.Replace(".", "/");
			if (string.IsNullOrEmpty(format))
			{
				text = data.ToString().Replace(".000000", "").Replace(".000", "")
					.Replace(".", "/");
			}
			else if (!string.IsNullOrEmpty(format))
			{
				text = data.ToString().Replace(".000000", "").Replace(".000", "")
					.Replace(".", "/");
				if (!text.Contains("/"))
				{
					text = data.ToString(format);
				}
			}
			while (text.EndsWith(".0") || text.EndsWith("/0"))
			{
				text = text.Substring(0, checked(text.Length - 1));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NpToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 265);
			throw;
		}
	}

	public static void CopyPropertiesTo<T, U>(this T source, U dest)
	{
		try
		{
			if (source is IList)
			{
				foreach (object item in (IEnumerable)(object)source)
				{
					object obj = Activator.CreateInstance(dest.GetType().GetGenericArguments().Single());
					item.CopyPropertiesTo_(obj);
					((IList)(object)dest).Add(obj);
				}
				return;
			}
			source.CopyPropertiesTo_(dest);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CopyPropertiesTo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 293);
		}
	}

	public static ReturnedSaveFuncInfoWithValue<WebPageContainer<T>> ProgressPages<T>(this IEnumerable<T> items, int eachPageCount, int currentPageNumber) where T : class
	{
		return WebPageContainer<T>.GeneratePageItems(items?.ToList(), eachPageCount, currentPageNumber);
	}

	private static void CopyPropertiesTo_<T, U>(this T source, U dest)
	{
		try
		{
			IEnumerable<PropertyInfo> enumerable = from x in source.GetType().GetProperties()
				where x.CanRead
				select x;
			IEnumerable<PropertyInfo> enumerable2 = from x in dest.GetType().GetProperties()
				where x.CanWrite
				select x;
			foreach (PropertyInfo item in enumerable)
			{
				foreach (PropertyInfo item2 in enumerable2)
				{
					try
					{
						if (!(item.Name == item2.Name))
						{
							continue;
						}
						if (item.PropertyType.IsEnum && item2.PropertyType.IsEnum)
						{
							string name = Enum.GetName(item.GetValue(source).GetType(), item.GetValue(source));
							source.GetType();
							foreach (object value in Enum.GetValues(item2.PropertyType))
							{
								if (value.ToString() == name)
								{
									item2.SetValue(dest, value);
									break;
								}
							}
						}
						else if (item.PropertyType == item2.PropertyType)
						{
							item2.SetValue(dest, item.GetValue(source));
							break;
						}
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "CopyPropertiesTo_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 337);
						Console.WriteLine(ex);
						throw;
					}
				}
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CopyPropertiesTo_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 346);
		}
	}

	public static Task CancelTask(CancellationToken token)
	{
		try
		{
			return Task.Run(async delegate
			{
				while (!token.IsCancellationRequested)
				{
					await Task.Delay(100);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CancelTask", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 364);
			return null;
		}
	}

	public static bool ParsIP(string ip)
	{
		try
		{
			Regex regex = new Regex("((([2][5][0-5])|([2][0-4][0-9])|([1][0-9][0-9])|([0-9][0-9])|([0-9]))[.]){3}((([2][5][0-5])|([2][0-4][0-9])|([1][0-9][0-9])|([0-9][0-9])|([0-9])))");
			Match match = null;
			match = regex.Match(ip);
			if (match.Success && match.Value.ToString().Length == ip.Length)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParsIP", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 381);
			return false;
		}
	}

	public static bool ParseToBoolean(this string value, bool def = false)
	{
		bool result = def;
		try
		{
			if (string.IsNullOrEmpty(value?.Trim()?.FixString()))
			{
				return def;
			}
			if (!bool.TryParse(value, out result))
			{
				return def;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParseToBoolean", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 396);
		}
		return result;
	}

	public static DateTime ParseToDate(this string value, DateTime? def = null)
	{
		try
		{
			if (string.IsNullOrEmpty(value?.Trim()?.FixString()))
			{
				return def ?? DateTime.Now;
			}
			DateTime result = def ?? DateTime.Now;
			if (DateTime.TryParse(value, out result))
			{
				return result;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParseToDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 417);
		}
		return def ?? DateTime.Now;
	}

	public static DateTime ParsToDateFromShOrMiDate(this string strDateTimeMiladiOrShamsi, DateTime? def = null)
	{
		DateTime result = new DateTime(0L);
		if (def.HasValue)
		{
			result = def.Value;
		}
		string text = "strDateTime first= " + strDateTimeMiladiOrShamsi;
		int num = 0;
		string text2 = "";
		string text3 = "";
		string text4 = "";
		int result2 = 1400;
		int result3 = 1;
		int result4 = 1;
		int result5 = 0;
		int result6 = 0;
		int result7 = 0;
		string text5 = "";
		string text6 = "";
		checked
		{
			try
			{
				string text7 = strDateTimeMiladiOrShamsi.Trim();
				if (string.IsNullOrEmpty(text7) || text7.Length < 6)
				{
					return result;
				}
				string[] array = text7.Split(new char[1] { ' ' });
				if (array.Length != 0)
				{
					text5 = array[0];
				}
				if (array.Length > 1)
				{
					text6 = array[1];
				}
				num = 1;
				text5 = text5.Replace("-", "/").Replace(".", "/").Trim();
				text = text + "datePart=" + text5;
				num = 2;
				if (text5.Contains("/"))
				{
					string[] array2 = text5.Split(new char[1] { '/' });
					if (array2.Length != 3)
					{
						return result;
					}
					text2 = array2[0].RemoveNoNumbers();
					if (text2.Length < 2)
					{
						return result;
					}
					text3 = array2[1].RemoveNoNumbers();
					if (text3.Length < 1)
					{
						return result;
					}
					text4 = array2[2].RemoveNoNumbers();
					if (text4.Length < 1)
					{
						return result;
					}
					int.TryParse(text2, out result2);
					int.TryParse(text3, out result3);
					int.TryParse(text4, out result4);
				}
				else
				{
					num = 3;
					text5 = text5.RemoveNoNumbers();
					if (text5.Length < 4 || text5.Length > 8)
					{
						return result;
					}
					num = 5;
					switch (text5.Length)
					{
					case 4:
						text2 = text5.Substring(0, 2);
						text3 = text5.Substring(2, 1);
						text4 = text5.Substring(3, 1);
						int.TryParse(text2, out result2);
						int.TryParse(text3, out result3);
						int.TryParse(text4, out result4);
						if (result3 == 0 || result4 == 0)
						{
							return result;
						}
						break;
					case 5:
						text2 = text5.Substring(0, 2);
						text3 = text5.Substring(2, 1);
						text4 = text5.Substring(3, 2);
						int.TryParse(text2, out result2);
						int.TryParse(text3, out result3);
						int.TryParse(text4, out result4);
						if (result4 == 0)
						{
							return result;
						}
						if (result3 == 0)
						{
							text3 = text5.Substring(2, 2);
							text4 = text5.Substring(4, 1);
							int.TryParse(text3, out result3);
							int.TryParse(text4, out result4);
							if (result3 == 0 || result4 == 0)
							{
								return result;
							}
						}
						break;
					case 6:
						text2 = text5.Substring(0, 2);
						text3 = text5.Substring(2, 2);
						text4 = text5.Substring(4, 2);
						int.TryParse(text2, out result2);
						int.TryParse(text3, out result3);
						int.TryParse(text4, out result4);
						if (result4 == 0)
						{
							return result;
						}
						if (result3 == 0)
						{
							text2 = text5.Substring(0, 4);
							text3 = text5.Substring(4, 1);
							text4 = text5.Substring(5, 1);
							int.TryParse(text2, out result2);
							int.TryParse(text3, out result3);
							int.TryParse(text4, out result4);
							if (result3 == 0 || result4 == 0)
							{
								return result;
							}
						}
						break;
					case 7:
						text2 = text5.Substring(0, 4);
						text3 = text5.Substring(4, 1);
						text4 = text5.Substring(5, 2);
						int.TryParse(text2, out result2);
						int.TryParse(text3, out result3);
						int.TryParse(text4, out result4);
						if (result4 == 0)
						{
							return result;
						}
						if (result3 == 0)
						{
							text3 = text5.Substring(4, 2);
							text4 = text5.Substring(6, 1);
							int.TryParse(text3, out result3);
							int.TryParse(text4, out result4);
							if (result3 == 0 || result4 == 0)
							{
								return result;
							}
						}
						break;
					case 8:
						text2 = text5.Substring(0, 4);
						text3 = text5.Substring(4, 2);
						text4 = text5.Substring(6, 2);
						int.TryParse(text2, out result2);
						int.TryParse(text3, out result3);
						int.TryParse(text4, out result4);
						if (result4 == 0 || result3 == 0)
						{
							return result;
						}
						break;
					}
				}
				bool flag = ((result4 > 31 || result4 == 0) ? true : false);
				bool flag2 = flag;
				if (!flag2)
				{
					bool flag3 = ((result3 > 12 || result3 == 0) ? true : false);
					flag2 = flag3;
				}
				if (flag2)
				{
					return result;
				}
				if (result2 < 1300)
				{
					if (result2 <= 25)
					{
						result2 = ((result2 >= 5) ? (result2 + 2000) : (result2 + 1400));
					}
					else
					{
						if (result2 > 99)
						{
							return result;
						}
						result2 += 1300;
					}
				}
				else if (result2 > 2050)
				{
					return result;
				}
				if (text6.Contains(":"))
				{
					string[] array3 = text6.Split(new char[1] { ':' });
					if (array3.Length != 0)
					{
						int.TryParse(array3[0].RemoveNoNumbers(), out result5);
					}
					if (array3.Length > 1)
					{
						int.TryParse(array3[1].RemoveNoNumbers(), out result6);
					}
					if (array3.Length > 2)
					{
						int.TryParse(array3[2].RemoveNoNumbers(), out result7);
					}
				}
				if (result5 > 24 || result6 > 59 || result7 > 59)
				{
					result5 = (result6 = (result7 = 0));
				}
				string text8 = $"{result2}/{result3}/{result4} {result5}:{result6}:{result7}";
				DateTime result8;
				if (result2 > 1500)
				{
					DateTime.TryParse(text8, out result8);
				}
				else
				{
					result8 = text8.ShamsiToMiladi();
				}
				num = 18;
				return result8;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"msg {text} line {num}", "ParsToDateFromShOrMiDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 619);
				return result;
			}
		}
	}

	public static TimeSpan ParsToTimeSpan(this string StrTime)
	{
		TimeSpan timeOfDay = new DateTime(0L).TimeOfDay;
		string text = "StrTime first " + StrTime;
		int num = 0;
		try
		{
			StrTime = StrTime.Trim();
			num = 1;
			if (string.IsNullOrEmpty(StrTime) || StrTime.Length < 3 || !StrTime.Contains(":"))
			{
				return timeOfDay;
			}
			num = 2;
			string[] array = StrTime.Split(new char[1] { ':' });
			string text2 = array[0].RemoveNoNumbers();
			text = text + " str_hh=" + text2;
			num = 3;
			string text3 = array[1].RemoveNoNumbers();
			text = text + " str_mm=" + text3;
			num = 4;
			string value = "00";
			if (array.Length > 2)
			{
				value = array[2].RemoveNoNumbers();
			}
			int hours = Convert.ToInt32(text2);
			text = text + " hh=" + hours;
			num = 5;
			int minutes = Convert.ToInt32(text3);
			text = text + " mm=" + minutes;
			num = 6;
			int seconds = Convert.ToInt32(value);
			text = text + " ss=" + seconds;
			num = 6;
			return new TimeSpan(hours, minutes, seconds);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"{text} line {num}", "ParsToTimeSpan", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 657);
			return timeOfDay;
		}
	}

	public static decimal ParseToDecimal(this string value, decimal def = 0m, int DigitAfterPoint = 0)
	{
		try
		{
			bool flag = false;
			value = value?.Trim()?.FixString();
			if (string.IsNullOrEmpty(value))
			{
				return def;
			}
			if (value.Length > 1 && value.StartsWith("-"))
			{
				flag = true;
			}
			value = value.Replace(Text.decimalSeprator, "").Replace(",", "").Replace(".", Text.DecimalPoint)
				.Replace("/", Text.DecimalPoint);
			value = value.KeepCharacters("0123456789" + Text.DecimalPoint);
			if (value.EndsWith(Text.DecimalPoint))
			{
				value = value.Substring(0, checked(value.Length - 1));
			}
			decimal result = (decimal.TryParse(value, out result) ? result : def);
			if (flag)
			{
				return -Math.Round(result, DigitAfterPoint);
			}
			return Math.Round(result, DigitAfterPoint);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "Value:" + value, "ParseToDecimal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 677);
			return def;
		}
	}

	public static string ToString(this float value, int roundDecimals)
	{
		try
		{
			if (value == 0f)
			{
				return "";
			}
			string text = "";
			for (int i = 0; i < roundDecimals; i = checked(i + 1))
			{
				text += "0";
			}
			return Math.Round(value, roundDecimals).ToString().Replace(Text.DecimalPoint, "/")
				.Replace("/" + text, "");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 692);
			return "";
		}
	}

	public static string ToString(this decimal value, int roundDecimals)
	{
		try
		{
			if (value == 0m)
			{
				return "";
			}
			string text = "";
			for (int i = 0; i < roundDecimals; i = checked(i + 1))
			{
				text += "0";
			}
			string text2 = Math.Round(value, roundDecimals).ToString().Replace(Text.DecimalPoint, "/")
				.Replace("/" + text, "");
			if (text2.EndsWith("/0"))
			{
				text2 = text2.Replace("/0", "");
			}
			return text2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 710);
			return "";
		}
	}

	public static string ToString(this double value, int roundDecimals)
	{
		try
		{
			if (value == 0.0)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder(100000);
			for (int i = 0; i < roundDecimals; i = checked(i + 1))
			{
				stringBuilder.Append("0");
			}
			return Math.Round(value, roundDecimals).ToString().Replace(Text.DecimalPoint, "/")
				.Replace($"/{stringBuilder}", "");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 727);
			return "";
		}
	}

	public static double ParseToDouble(this string value, double def = 0.0, short DigitAfterPoint = 3)
	{
		return (double)value.ParseToDecimal((decimal)def, DigitAfterPoint);
	}

	public static float ParseTofloat(this string value, float def = 0f, short DigitAfterPoint = 3)
	{
		return (float)value.ParseToDecimal((decimal)def, DigitAfterPoint);
	}

	public static int ParseToInt(this string value, int def = 0)
	{
		try
		{
			if (string.IsNullOrEmpty(value))
			{
				return 0;
			}
			decimal num = value.ParseToDecimal();
			if (num > 2147483647m || num < -2147483648m)
			{
				return 0;
			}
			return (int)num;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "value:(" + value + ")", "ParseToInt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 765);
			return 0;
		}
	}

	public static long ParseToLong(this string value, long def = 0L)
	{
		try
		{
			if (string.IsNullOrEmpty(value))
			{
				return 0L;
			}
			decimal num = value.ParseToDecimal();
			if (num > 9223372036854775807m || num < -9223372036854775808m)
			{
				return 0L;
			}
			return (long)num;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "value:(" + value + ")", "ParseToLong", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 781);
			return 0L;
		}
	}

	public static short ParseToShort(this string value, short def = 0)
	{
		try
		{
			if (string.IsNullOrEmpty(value))
			{
				return 0;
			}
			decimal num = value.ParseToDecimal();
			if (num > 32767m || num < -32768m)
			{
				return 0;
			}
			return (short)num;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "value:(" + value + ")", "ParseToShort", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 796);
			return 0;
		}
	}

	public static string RemoveCharactersForLinkSearch(this string value, string validChars = "(1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ/-+()×@#_[].$&ابپتثجچحخدذرزژسشصضطظعغفقکگلمنوهئیءآا\u064bه\u0654ة )ـ,აბგდევზთიკლმნოპრსტუფქღყშჩცძწჭხჯჰ")
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		try
		{
			if (string.IsNullOrEmpty(value))
			{
				return "";
			}
			value = value.FixString();
			char[] array = value.ToCharArray();
			foreach (char value2 in array)
			{
				if (Enumerable.Contains(validChars, value2))
				{
					stringBuilder.Append(value2);
				}
				else
				{
					stringBuilder.Append(" ");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveCharactersForLinkSearch", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 817);
		}
		return stringBuilder.ToString();
	}

	public static string KeepPersianCharacters(this string value, string validChars = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZآئبپتثحخدذرزژسشصضطظعغفقکگلمنوهیءاაბგდევზთიკლმნოპრსტუფქღყშჩცძწჭხჯჰ")
	{
		return value.KeepCharacters(validChars);
	}

	public static string KeepCharactersForSmsParser(this string value, string validChars = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZآئبپتثحخدذرزژسشصضطظعغفقکگلمنوهیءا\r\n\t -_+,* \\/[]{}@$#%^()|!.:;<>?\"'")
	{
		return value.KeepCharacters(validChars);
	}

	public static string KeepCharacters(this string value, string validChars = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_")
	{
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			if (string.IsNullOrEmpty(value))
			{
				return "";
			}
			string text = "sdfsdfgdfrgxcvbgf";
			value = value.Replace("*", text).FixString().Replace(text, "*");
			char[] array = value.ToCharArray();
			foreach (char value2 in array)
			{
				if (Enumerable.Contains(validChars, value2))
				{
					stringBuilder.Append(value2);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "KeepCharacters", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 840);
		}
		return stringBuilder.ToString();
	}

	public static string GenerateDebugDescription(this IEnumerable<string> data, short max = 5)
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		checked
		{
			try
			{
				stringBuilder.Append("Count(" + (data?.Count().ToString() ?? "Null!!") + ")");
				if (data != null && data.Any())
				{
					int num = 1;
					foreach (string datum in data)
					{
						if (num > max - 1)
						{
							stringBuilder.Append($",({data.Count() - num})OtherRecords");
							break;
						}
						stringBuilder.AppendLine("[" + datum + "]");
						num++;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDebugDescription", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 865);
			}
			return stringBuilder.ToString();
		}
	}

	public static List<Packet> GeneratePackets<T>(this IEnumerable<T> items, PacketType packetType, short Action, string[] args = null, bool isSync = false, Guid? packetReferenceId = null, int partitionId = 0, bool deliveryRequst = true, [CallerMemberName] string extraDataForDebug = "")
	{
		using (new JaegerMonitor("GeneratePackets", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs"))
		{
			try
			{
				List<Packet> list = new List<Packet>();
				if (!packetReferenceId.HasValue)
				{
					packetReferenceId = Guid.NewGuid();
				}
				list.Add(new Packet(packetType, Action, Json.ToBytesJson(items), args, 0, 1, packetReferenceId.Value, isSync, partitionId, extraDataForDebug)
				{
					DeliveryRequested = deliveryRequst
				});
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GeneratePackets", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 900);
				return null;
			}
		}
	}

	public static Packet GeneratePacket<T>(this T item, PacketType packetType, short Action, string[] args = null, bool isSync = false, Guid? packetReferenceId = null, int partitionId = 0, bool deliveryRequst = true, [CallerMemberName] string extraDataForDebug = "")
	{
		try
		{
			return new List<T> { item }.GeneratePackets(packetType, Action, args, isSync, packetReferenceId, partitionId, deliveryRequst, extraDataForDebug)?.FirstOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GeneratePacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 914);
			throw;
		}
	}

	public static bool Compare(this List<byte> item1, List<byte> item2)
	{
		using (new JaegerMonitor("Compare", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs"))
		{
			try
			{
				if (item1 == null && item2 == null)
				{
					return true;
				}
				if (item1 == null || item2 == null)
				{
					return false;
				}
				if (item1.Count() != item2.Count())
				{
					return false;
				}
				for (int i = 0; i < item1.Count(); i = checked(i + 1))
				{
					if (item1[i] != item2[i])
					{
						return false;
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Compare", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 935);
				return false;
			}
		}
	}

	public static async Task RaiseEvent<T>(Func<object, T, Task> handler, object sender, T args, CancellationToken token, int TimeOutSeconds = 60, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		try
		{
			if (handler == null)
			{
				return;
			}
			Delegate[] invocationList = handler.GetInvocationList();
			List<Task> list = new List<Task>();
			Delegate[] array = invocationList;
			foreach (Delegate invocationItem in array)
			{
				if ((object)invocationItem == null)
				{
					continue;
				}
				list.Add(Task.Run(async delegate
				{
					try
					{
						Task task = ((Func<object, T, Task>)invocationItem)(sender, args);
						if (task != null)
						{
							Task timeOutTask = Task.Delay(checked(TimeOutSeconds * 30 * 1000));
							if (await Task.WhenAny(new Task[2] { task, timeOutTask }) == timeOutTask)
							{
								Console.WriteLine("Caller:" + memberName + ",class:" + sourceFilePath);
							}
						}
					}
					catch (Exception ex3)
					{
						WebErrorLog.Instance.StartLog(ex3, "Callsr:" + memberName + ",class:" + sourceFilePath, "RaiseEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 966);
					}
				}));
			}
			await Task.WhenAll(list);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			if (!ex2.Message.ToLower().Contains("The operation has timed out".ToLower()))
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RaiseEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 974);
			}
		}
	}

	private static int GetObjectSize(object TestObject)
	{
		checked
		{
			try
			{
				if (TestObject is byte)
				{
					_ = (byte)TestObject;
					return 1;
				}
				if (TestObject is string text)
				{
					return text.Length * 2;
				}
				if (TestObject == null)
				{
					return 200;
				}
				if (cach.TryGetValue(TestObject.GetType().ToString(), out var value))
				{
					return value;
				}
				int num = Json.ToBytesJson(TestObject).Length * 2;
				cach.TryAdd(TestObject.GetType().ToString(), num);
				return num;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetObjectSize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1007);
				return 200;
			}
		}
	}

	public static List<List<T>> ChunkList<T>(this IEnumerable<T> lst, decimal ChunkSizeKBytes, int MaxChunkCount = 1000)
	{
		try
		{
			ChunkSizeKBytes = 999999999m;
			MaxChunkCount = 999999999;
			if (lst == null || !lst.Any())
			{
				return null;
			}
			T val = lst.FirstOrDefault();
			int num = 0;
			if (!(val is WebCus))
			{
				if (!(val is WebCusGroup))
				{
					if (!(val is WebCusTitle))
					{
						if (!(val is WebHesabTree))
						{
							if (!(val is WebProduct))
							{
								if (!(val is WebPrdGroup))
								{
									if (!(val is WebPrdVahed))
									{
										if (val is WebApplicationSetting)
										{
											num = 100;
											if (MaxChunkCount > 1000)
											{
												MaxChunkCount = 1000;
											}
										}
										else
										{
											num = GetObjectSize(lst.FirstOrDefault());
										}
									}
									else
									{
										num = 100;
									}
								}
								else
								{
									num = 100;
								}
							}
							else
							{
								num = 1000;
								if (MaxChunkCount > 100)
								{
									MaxChunkCount = 100;
								}
							}
						}
						else
						{
							num = 5000;
							if (MaxChunkCount > 100)
							{
								MaxChunkCount = 100;
							}
						}
					}
					else
					{
						num = 100;
					}
				}
				else
				{
					num = 100;
				}
			}
			else
			{
				num = 1000;
				if (MaxChunkCount > 100)
				{
					MaxChunkCount = 100;
				}
			}
			ChunkSizeKBytes *= 1024m;
			List<List<T>> list = new List<List<T>>();
			List<T> list2 = new List<T>();
			foreach (T item in lst)
			{
				list2.Add(item);
				if (list2.Count > MaxChunkCount || (decimal)checked(num * list2.Count) > ChunkSizeKBytes)
				{
					list.Add(list2);
					list2 = new List<T>();
				}
			}
			if (list2.Count > 0)
			{
				list.Add(list2);
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChunkList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1082);
			return null;
		}
	}

	public static List<List<T>> ChunkListBySpecialLenght<T>(this IEnumerable<T> lst, decimal chunkSizeBytes)
	{
		try
		{
			if (lst == null || !lst.Any())
			{
				return null;
			}
			lst.FirstOrDefault();
			List<List<T>> list = new List<List<T>>();
			List<T> list2 = new List<T>();
			foreach (T item in lst)
			{
				list2.Add(item);
				if ((decimal)list2.Count == chunkSizeBytes)
				{
					list.Add(list2);
					list2 = new List<T>();
				}
			}
			if (list2.Count > 0)
			{
				list.Add(list2);
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChunkListBySpecialLenght", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1111);
			return null;
		}
	}

	public static Task RaiseEventOnNewThread<T>(Func<object, T, Task> handler, object sender, T args)
	{
		return RaiseEventOnNewThread(handler, sender, args, default(CancellationToken));
	}

	public static Task RaiseEventOnNewThread<T>(Func<object, T, Task> handler, object sender, T args, CancellationToken token)
	{
		try
		{
			if (handler == null)
			{
				return Task.CompletedTask;
			}
			Delegate[] invocationList = handler.GetInvocationList();
			List<Task> list = new List<Task>();
			for (int i = 0; i < invocationList.Length; i = checked(i + 1))
			{
				Task task = ((Func<object, T, Task>)invocationList[i])(sender, args);
				if (task != null)
				{
					list.Add(task);
				}
			}
			return Task.WhenAll(list).ContinueWith(delegate(Task res)
			{
				try
				{
					if (res?.Exception != null)
					{
						WebErrorLog.Instance.StartLog(res.Exception, "RaiseEventOnNewThread", "RaiseEventOnNewThread", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1142);
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "RaiseEventOnNewThread", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1145);
				}
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "RaiseEventOnNewThread", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1149);
		}
		return Task.CompletedTask;
	}

	public static string GetKey<TEnum>(this TEnum EnumItem) where TEnum : Enum
	{
		try
		{
			LanguageAndCalendar instance = LanguageAndCalendar.Instance;
			TEnum val = EnumItem;
			return instance.GetValue("[" + val?.ToString() + "]");
		}
		catch (Exception)
		{
			return EnumItem.ToString();
		}
	}

	public static string GetTranslate(this string str)
	{
		try
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			if (str.Contains("[") || str.Contains("]"))
			{
				return LanguageAndCalendar.Instance.GetValue(str);
			}
			return LanguageAndCalendar.Instance.GetValue("[" + str + "]");
		}
		catch (ThreadAbortException)
		{
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "Error when translate Str =>" + str, "GetTranslate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1226);
		}
		return str;
	}

	public static IEnumerable<string> GetTranslate(this IEnumerable<string> strList)
	{
		try
		{
			List<string> list = new List<string>();
			foreach (string str in strList)
			{
				list.Add(LanguageAndCalendar.Instance.GetValue(str));
			}
			return list;
		}
		catch (ThreadAbortException)
		{
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetTranslate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1240);
		}
		return strList;
	}

	public static string GetDisplay<T>(this T EnumItem) where T : Enum
	{
		try
		{
			if (typeof(T).IsEnum)
			{
				return ((PersianNameAttribute.PersianName)(typeof(T).GetFields()?.SingleOrDefault((FieldInfo rec) => rec.Name == Enum.GetName(typeof(T), EnumItem))?.GetCustomAttributes(inherit: false)?.FirstOrDefault((object rec) => rec is PersianNameAttribute.PersianName)))?.Text ?? EnumItem.ToString();
			}
			return EnumItem.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "#type:" + typeof(T).FullName + "\r\n" + $"#Value:{EnumItem}", "GetDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1261);
			return EnumItem.ToString();
		}
	}

	public static string GetDescription<T>(this T EnumItem) where T : Enum
	{
		try
		{
			if (typeof(T).IsEnum)
			{
				return ((DescriptionAttribute)(typeof(T).GetFields()?.Single((FieldInfo rec) => rec.Name == Enum.GetName(typeof(T), EnumItem))?.GetCustomAttributes(inherit: false)?.FirstOrDefault((object rec) => rec is DescriptionAttribute)))?.Description ?? EnumItem.ToString();
			}
			return EnumItem.ToString();
		}
		catch (Exception)
		{
			return EnumItem.ToString();
		}
	}

	public static string GetTelegramDisplay<T>(this T EnumItem) where T : Enum
	{
		try
		{
			using (new JaegerMonitor("GetTelegramDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs"))
			{
				if (typeof(T).IsEnum)
				{
					return ((TelegramNameAttribute.TelegramName)(typeof(T).GetFields()?.Single((FieldInfo rec) => rec.Name == Enum.GetName(typeof(T), EnumItem))?.GetCustomAttributes(inherit: false)?.FirstOrDefault((object rec) => rec is TelegramNameAttribute.TelegramName)))?.Text ?? EnumItem.ToString();
				}
				return EnumItem.ToString();
			}
		}
		catch (Exception)
		{
			return EnumItem.ToString();
		}
	}

	public static string GetLocalFileAttribute<T>(this T EnumItem) where T : Enum
	{
		using (new JaegerMonitor("GetLocalFileAttribute", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs"))
		{
			try
			{
				if (typeof(T).IsEnum)
				{
					return ((LocalFilePathAttribute.LocalFilePath)typeof(T).GetFields().Single((FieldInfo rec) => rec.Name == Enum.GetName(typeof(T), EnumItem)).GetCustomAttributes(inherit: false)
						.First((object rec) => rec is LocalFilePathAttribute.LocalFilePath)).Text;
				}
				return EnumItem.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetLocalFileAttribute", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1321);
				return EnumItem.ToString();
			}
		}
	}

	public static string GetRemoteFileAttribute<T>(this T EnumItem) where T : Enum
	{
		try
		{
			using (new JaegerMonitor("GetRemoteFileAttribute", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs"))
			{
				if (typeof(T).IsEnum)
				{
					return ((RemoteFilePathAttribute.RemoteFilePath)typeof(T).GetFields().Single((FieldInfo rec) => rec.Name == Enum.GetName(typeof(T), EnumItem)).GetCustomAttributes(inherit: false)
						.First((object rec) => rec is RemoteFilePathAttribute.RemoteFilePath)).Text;
				}
				return EnumItem.ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetRemoteFileAttribute", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1345);
			return EnumItem.ToString();
		}
	}

	public static async Task RetryOnFault<T>(Func<T> function, short maxTries, int MilisecondsDelayBetweenRetries)
	{
		checked
		{
			try
			{
				using (new JaegerMonitor("RetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs"))
				{
					for (int i = 0; i < maxTries; i++)
					{
						try
						{
							function();
						}
						catch
						{
							if (i == maxTries - 1)
							{
								throw;
							}
							await Task.Delay(MilisecondsDelayBetweenRetries);
						}
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1371);
				throw;
			}
		}
	}

	public static async Task<T> RetryOnFault<T>(Func<Task<T>> function, int maxTries)
	{
		checked
		{
			try
			{
				using (new JaegerMonitor("RetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs"))
				{
					for (int i = 0; i < maxTries; i++)
					{
						try
						{
							return await function();
						}
						catch
						{
							if (i == maxTries - 1)
							{
								throw;
							}
						}
					}
					return default(T);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1398);
				return default(T);
			}
		}
	}

	public static async Task RetryOnFault(Func<Task> function, int maxTries)
	{
		checked
		{
			using (new JaegerMonitor("RetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs"))
			{
				try
				{
					for (int i = 0; i < maxTries; i++)
					{
						try
						{
							await function();
						}
						catch (Exception ex)
						{
							WebErrorLog.Instance.StartLog(ex, "", "RetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1413);
							if (i == maxTries - 1)
							{
								throw;
							}
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "RetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1423);
				}
			}
		}
	}

	public static async Task<T> TimedRetryOnFault<T>(Func<Task<T>> function, int maxTries, Func<Task> retryWhen)
	{
		_ = 1;
		checked
		{
			try
			{
				using (new JaegerMonitor("TimedRetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs"))
				{
					for (int i = 0; i < maxTries; i++)
					{
						try
						{
							return await function();
						}
						catch (Exception ex)
						{
							WebErrorLog.Instance.StartLog(ex, "", "TimedRetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1437);
							if (i == maxTries - 1)
							{
								throw;
							}
						}
						await retryWhen();
					}
					return default(T);
				}
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "TimedRetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1449);
				return default(T);
			}
		}
	}

	public static async Task TimedRetryOnFault(Func<Task> function, int maxTries, Func<Task> retryWhen)
	{
		checked
		{
			using (new JaegerMonitor("TimedRetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs"))
			{
				_ = 1;
				try
				{
					for (int i = 0; i < maxTries; i++)
					{
						try
						{
							await function();
						}
						catch (Exception ex)
						{
							WebErrorLog.Instance.StartLog(ex, "", "TimedRetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1464);
							if (i == maxTries - 1)
							{
								throw;
							}
						}
						await retryWhen();
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "TimedRetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1475);
				}
			}
		}
	}

	public static AttHelpURL GetExtentionHelpURLAttr<T>(this T pcls, string pName)
	{
		AttHelpURL result = new AttHelpURL(" ");
		PropertyInfo[] properties = pcls.GetType().GetProperties();
		foreach (PropertyInfo obj in properties)
		{
			object[] customAttributes = obj.GetCustomAttributes(inherit: true);
			string name = obj.Name;
			object[] array = customAttributes;
			foreach (object obj2 in array)
			{
				if (obj2 is AttHelpURL)
				{
					result = obj2 as AttHelpURL;
					break;
				}
			}
			if (name == pName)
			{
				break;
			}
		}
		return result;
	}

	public static string GetAttReportFileName<T>(this T EnumItem) where T : Enum
	{
		if (typeof(T).IsEnum)
		{
			try
			{
				return ((AttReportFileName)typeof(T).GetFields().Single((FieldInfo rec) => rec.Name == Enum.GetName(typeof(T), EnumItem)).GetCustomAttributes(inherit: false)
					.First((object rec) => rec is AttReportFileName)).Text;
			}
			catch
			{
				return EnumItem.ToString();
			}
		}
		return EnumItem.ToString();
	}

	public static string GetMonthName(this int month)
	{
		string text = "";
		return month switch
		{
			1 => "فروردین", 
			2 => "اردیبهشت", 
			3 => "خرداد", 
			4 => "تیر", 
			5 => "مرداد", 
			6 => "شهریور", 
			7 => "مهر", 
			8 => "آبان", 
			9 => "آذر", 
			10 => "دی", 
			11 => "بهمن", 
			12 => "اسفند", 
			_ => "", 
		};
	}

	public static DateTime GotoEndYear(this DateTime pdate)
	{
		try
		{
			int year = new PersianCalendar().GetYear(pdate);
			int num = Calendar.EndDayOfMonth(12, year);
			return $"{year}/{12}/{num}".ShamsiToMiladi().Date.AddDays(1.0).AddSeconds(-1.0);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GotoEndYear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1561);
		}
		return DateTime.Now;
	}

	public static int GetYearSh(this DateTime pdate)
	{
		try
		{
			return new PersianCalendar().GetYear(pdate);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetYearSh", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1576);
		}
		return 0;
	}

	public static string AccountTashkhis(this decimal pValue)
	{
		try
		{
			if (pValue > 0m)
			{
				return "بس".GetTranslate();
			}
			if (pValue < 0m)
			{
				return "بد".GetTranslate();
			}
			return "بی حساب".GetTranslate();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AccountTashkhis", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1591);
			return "";
		}
	}

	public static string AccountForReport(this decimal pValue)
	{
		try
		{
			if (pValue > 0m)
			{
				return "بس".GetTranslate();
			}
			if (pValue < 0m)
			{
				return "بد".GetTranslate();
			}
			return "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AccountForReport", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1605);
			return "";
		}
	}

	public static string GetTimeFormat(this string pTime)
	{
		pTime = pTime.Trim();
		int num = 0;
		int num2 = 0;
		try
		{
			pTime = pTime.RemoveNoNumbers(":").Replace("::", ":");
			if (string.IsNullOrEmpty(pTime))
			{
				return null;
			}
			string[] array = pTime.Split(new char[1] { ':' });
			if (array.Length == 1)
			{
				if (array[0].Length == 4)
				{
					num = array[0].Substring(0, 2).ParseToShort(0);
					num2 = array[0].Substring(2, 2).ParseToShort(0);
					if (num > 24 || num2 > 59 || (num == 24 && num2 > 0))
					{
						return null;
					}
					return num.ToString("00") + ":" + num2.ToString("00");
				}
				if (array[0].ParseToShort(0) > 0 && array[0].ParseToShort(0) <= 24)
				{
					return array[0].ParseToShort(0).ToString("00") + ":00";
				}
				return null;
			}
			if (array.Length != 0)
			{
				if (array[0].Trim().ParseToShort(0) < 0 || array[0].Trim().ParseToShort(0) > 24)
				{
					return null;
				}
				num = array[0].ParseToShort(0);
			}
			if (array.Length > 1)
			{
				if (array[1].Trim().ParseToShort(0) < 0 || array[1].Trim().ParseToShort(0) >= 60)
				{
					return null;
				}
				num2 = array[1].ParseToShort(0);
			}
			if (num == 24 && num2 > 0)
			{
				return null;
			}
			return num.ToString("00") + ":" + num2.ToString("00");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "V:6.4.8//مقداری که باعث شده خطا ایجاد شود " + pTime, "GetTimeFormat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1656);
			return null;
		}
	}

	public static int GetNthIndex(this string src, string sch, int n)
	{
		int num = 1;
		int num2 = 0;
		checked
		{
			while (num2 < src.Length)
			{
				int num3 = src.IndexOf(sch, num2, StringComparison.Ordinal);
				if (num3 == -1 || n < 1 || num == n)
				{
					return num3;
				}
				num++;
				num2 = num3 + 1;
			}
			return -1;
		}
	}

	public static void UpLoadFrom(ref object desItem, object srcItem)
	{
		try
		{
			PropertyInfo[] properties = desItem.GetType().GetProperties();
			if (properties.Length == 0)
			{
				desItem = properties;
				return;
			}
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				if (propertyInfo.CanWrite)
				{
					object desItem2 = propertyInfo.GetValue(desItem, null);
					object value = propertyInfo.GetValue(srcItem, null);
					UpLoadFrom(ref desItem2, value);
				}
			}
		}
		catch (Exception value2)
		{
			Console.WriteLine(value2);
			throw;
		}
	}

	public static string UntilMaxLen(this string src, int len)
	{
		try
		{
			return (string.IsNullOrEmpty(src) || len >= src.Length) ? src : src.Substring(0, len);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UntilMaxLen", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1715);
		}
		return "";
	}

	public static string GetShortDateSh(this string datesh)
	{
		try
		{
			if (string.IsNullOrEmpty(datesh))
			{
				return "";
			}
			datesh = datesh.Replace("/", "");
			if (datesh.Length > 6)
			{
				datesh = datesh.Substring(2);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetShortDateSh", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1739);
		}
		return datesh;
	}

	public static string GetShortFromatedDateSh(this string datesh)
	{
		try
		{
			if (string.IsNullOrEmpty(datesh))
			{
				return "";
			}
			datesh.Split("/".ToArray());
			if (datesh.Length > 2)
			{
				datesh = datesh.Substring(5);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetShortFromatedDateSh", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1760);
		}
		return datesh;
	}

	public static string PersianDayName(this string persianDate)
	{
		try
		{
			return persianDate.ShamsiToMiladi().DayOfWeek switch
			{
				DayOfWeek.Saturday => "شنبه", 
				DayOfWeek.Sunday => "یکشنبه", 
				DayOfWeek.Monday => "دوشنبه", 
				DayOfWeek.Tuesday => "سه شنبه", 
				DayOfWeek.Wednesday => "چهار شنبه", 
				DayOfWeek.Thursday => "پنج شنبه", 
				DayOfWeek.Friday => "جمعه", 
				_ => "", 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PersianDayName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1784);
		}
		return "";
	}

	public static string Reverse(this string str)
	{
		try
		{
			char[] array = str.ToCharArray();
			Array.Reverse((Array)array);
			return new string(array);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Reverse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1798);
		}
		return "";
	}

	public static string TimeDifferenceToString(this DateTime information, int fromTotal, int currentItem)
	{
		string text = "";
		try
		{
			if (currentItem < fromTotal)
			{
				text = " " + ((decimal)currentItem / (decimal)fromTotal * 100m).ToString(2) + "%";
			}
			text = information.TimeDifferenceToString() + text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TimeDifferenceToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1812);
		}
		return text;
	}

	public static string TimeDifferenceToString(this DateTime information)
	{
		try
		{
			if (DateTime.Now.Subtract(information).TotalSeconds < 30.0)
			{
				return "هم اکنون";
			}
			if (DateTime.Now.Subtract(information).TotalSeconds < 100.0)
			{
				return "لحظاتی پیش";
			}
			if (DateTime.Now.Subtract(information).TotalMinutes < 60.0)
			{
				return DateTime.Now.Subtract(information).TotalMinutes.ToString("N0") + "دقیقه قبل";
			}
			if (DateTime.Now.Subtract(information).TotalHours <= 24.0)
			{
				return DateTime.Now.Subtract(information).TotalHours.ToString("N0") + "ساعت پیش";
			}
			if (DateTime.Now.Subtract(information).TotalDays == 1.0)
			{
				return $"دیروز {information.Hour}:{information.Minute}";
			}
			if (DateTime.Now.Subtract(information).TotalDays == 2.0)
			{
				return $"پریروز {information.Hour}:{information.Minute}";
			}
			if (DateTime.Now.Subtract(information).TotalDays < 7.0)
			{
				return DateTime.Now.Subtract(information).TotalDays.ToString("N0") + "روز پیش";
			}
			if (DateTime.Now.Subtract(information).TotalDays < 1825.0)
			{
				return information.MiladiToCalture() ?? "";
			}
			return "هرگز";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TimeDifferenceToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1841);
			return "Error";
		}
	}

	public static string CalcSafeHashBase64(this string Password, string salt)
	{
		return Convert.ToBase64String(Password.CalcHashSafe(new UnicodeEncoding().GetBytes(salt)));
	}

	public static string CalcHashBase64(this string Password)
	{
		return Convert.ToBase64String(Password.CalcHash());
	}

	public static byte[] CalcHash(this string Password)
	{
		try
		{
			return new SHA256CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(Password));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcHash", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1870);
			return null;
		}
	}

	public static long CalcCrc(this IEnumerable<byte> data)
	{
		try
		{
			return Crc32Algorithm.Compute(data?.ToArray());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcCrc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1884);
			return 0L;
		}
	}

	public static long CalcCrc(this string data)
	{
		return Encoding.ASCII.GetBytes(data).CalcCrc();
	}

	public static byte[] CalcHashSafe(this string Password, byte[] salt)
	{
		try
		{
			if (Password == null)
			{
				Password = "";
			}
			return new Rfc2898DeriveBytes(Password, salt, 10000).GetBytes(32);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, string.Format("password is null or empty:{0}//data:{1}", string.IsNullOrEmpty(Password), Password ?? "Null!!"), "CalcHashSafe", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1902);
			return null;
		}
	}

	public static string TimeDifferenceToHexColor(this DateTime information)
	{
		try
		{
			if (DateTime.Now.Subtract(information).TotalMinutes < 1.0)
			{
				return "#008000";
			}
			if (DateTime.Now.Subtract(information).TotalSeconds < 30.0)
			{
				return "#00FF00";
			}
			if (DateTime.Now.Subtract(information).TotalMinutes < 60.0)
			{
				return "#00BFFF";
			}
			if (DateTime.Now.Subtract(information).TotalDays > 365.0)
			{
				return "#FF0000";
			}
			if (DateTime.Now.Subtract(information).TotalDays > 180.0)
			{
				return "#DC143C";
			}
			if (DateTime.Now.Subtract(information).TotalDays > 90.0)
			{
				return "#DB7093";
			}
			if (DateTime.Now.Subtract(information).TotalDays > 30.0)
			{
				return "#D2691E";
			}
			if (DateTime.Now.Subtract(information).TotalDays > 7.0)
			{
				return "#FF7F50";
			}
			if (DateTime.Now.Subtract(information).TotalHours > 1.0 && DateTime.Now.Subtract(information).TotalHours <= 24.0)
			{
				return "#DAA520";
			}
			return "#00008B";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TimeDifferenceToHexColor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 1935);
			return "#000000";
		}
	}

	public static long VersionStringToLong(this string version, bool removeMinorPart = false)
	{
		try
		{
			if (string.IsNullOrEmpty(version) || version == "0")
			{
				return 0L;
			}
			List<string> list = version.SplitString();
			string text = "";
			if (list.Count == 1)
			{
				return FixAccV5Version(version).VersionStringToLong();
			}
			if (list != null && list.Any())
			{
				for (int i = 0; i < list.Count; i = checked(i + 1))
				{
					if (i > 3)
					{
						throw new Exception("version(" + version + ") parts is not valid!");
					}
					if (i == 3 && removeMinorPart)
					{
						break;
					}
					string text2 = list[i];
					while (text2.Length < 2)
					{
						text2 = "0" + text2;
					}
					text += text2;
				}
			}
			while (text.Length < 9)
			{
				text += "0";
			}
			if (text.Length > 9)
			{
				text = text.Substring(0, 9);
			}
			return text.ParseToLong(0L);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Version:{version}//removeMinorPart:{removeMinorPart}", "VersionStringToLong", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2005);
			return 0L;
		}
	}

	private static string FixAccV5Version(string version)
	{
		if (string.IsNullOrEmpty(version))
		{
			throw new ArgumentException("version is null or empty!");
		}
		if (version == "0")
		{
			throw new ArgumentException("version(0) is not valid!");
		}
		try
		{
			if (version == "40205090")
			{
				return "4.2.5.0009";
			}
			if (version == "424002")
			{
				return "4.2.4.0002";
			}
			if (version == "424002")
			{
				return "4.2.4.0002";
			}
			if (version == "40205000")
			{
				return "4.2.5";
			}
			if (version == "40206020")
			{
				return "4.2.6.0002";
			}
			if (version == "30078")
			{
				return "3.0.78";
			}
			if (version == "20057")
			{
				return "2.5.7";
			}
			if (version == "20053")
			{
				return "2.5.3";
			}
			if (version == "423005")
			{
				return "4.2.3.0005";
			}
			if (version == "423000")
			{
				return "4.2.3";
			}
			if (version == "40206030")
			{
				return "4.2.6.3";
			}
			if (version == "40206040")
			{
				return "4.2.6.4";
			}
			if (version == "40205060")
			{
				return "4.2.5.6";
			}
			if (version == "422007")
			{
				return "4.2.2.7";
			}
			if (version == "40104")
			{
				return "4.1.4";
			}
			if (version == "40206090")
			{
				return "4.2.6.9";
			}
			if (version == "422004")
			{
				return "4.2.2.4";
			}
			if (version == "40206090")
			{
				return "4.2.6.9";
			}
			if (version == "40207010")
			{
				return "4.2.7.1";
			}
			if (version == "40206070")
			{
				return "4.2.6.7";
			}
			if (version == "40207000")
			{
				return "4.2.7";
			}
			if (version == "423003")
			{
				return "4.2.3.3";
			}
			if (version == "423009")
			{
				return "4.2.3.9";
			}
			if (version == "423007")
			{
				return "4.2.3.7";
			}
			if (version == "40204070")
			{
				return "4.2.4.7";
			}
			if (version == "20058")
			{
				return "2.5.8";
			}
			if (version == "423007")
			{
				return "4.2.3.7";
			}
			if (version == "40205010")
			{
				return "4.2.5.1";
			}
			if (version == "30081")
			{
				return "3.8.1";
			}
			if (version == "40207020")
			{
				return "4.2.7.2";
			}
			switch (version)
			{
			case "30081":
				return "3.0.81";
			case "40207020":
				return "4.2.7.2";
			case "423001":
				return "4.2.3.1";
			case "40205040":
				return "4.2.5.4";
			case "40107":
				return "4.1.7";
			case "30037":
				return "3.0.37";
			case "41000":
				return "4.1.0";
			case "40205050":
				return "4.2.5.5";
			case "20042":
				return "2.0.42";
			case "60030":
				return "6.0.3";
			case "600":
				return "6.0.0";
			case "20056":
				return "2.0.56";
			default:
				if (version.Length != 4)
				{
					throw new ArgumentException("یک نسخه جدید مشکل دار دیگه پیدا کردم!".GetTranslate() + " " + version);
				}
				return $"{version[0]}.{version[1]}.{version[2]}.{version[3]}";
			}
		}
		catch (ArgumentException ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixAccV5Version", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2064);
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "FixAccV5Version", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2067);
			return version;
		}
	}

	public static decimal Round(this decimal value, decimal round = 0m)
	{
		try
		{
			if (round <= 0m)
			{
				return value;
			}
			return value - value % round;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Round", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2082);
			return value;
		}
	}

	public static string AdadToHoroof(this decimal number)
	{
		return NumberManager.AdadToHoroof(number);
	}

	public static string AdadToHoroof(this long number)
	{
		return NumberManager.AdadToHoroof(number);
	}

	public static int YearOfDateCalendar(this string dateString)
	{
		return LanguageAndCalendar.Instance.CaltureCalender.GetYearOfDate(dateString);
	}

	public static int MonthOfDateCalendar(this string dateString)
	{
		return LanguageAndCalendar.Instance.CaltureCalender.GetMonthOfDate(dateString);
	}

	public static DateTime EndDayOfMonthtCalendar(this string dateString, string time = "23:59:59")
	{
		return LanguageAndCalendar.Instance.CaltureCalender.EndDayOfMonth(dateString, time);
	}

	public static DateTime CalendarToMiladi(this string dateString)
	{
		if (LanguageAndCalendar.Instance == null || LanguageAndCalendar.Instance.CaltureCalender == null)
		{
			return DateTime.Now;
		}
		return LanguageAndCalendar.Instance.CaltureCalender.CalendarToMiladi(dateString);
	}

	public static string AddMonthCalendar(this string dateString, int count)
	{
		return LanguageAndCalendar.Instance.CaltureCalender.AddMonth(dateString, count);
	}

	public static DateTime StartDayOfMonthtCalendar(this string dateString, string time = "00:00:00")
	{
		return LanguageAndCalendar.Instance.CaltureCalender.StartDayOfMonth(dateString, time);
	}

	public static bool CheckCalendarDate(this string dateString)
	{
		if (LanguageAndCalendar.Instance == null || LanguageAndCalendar.Instance.CaltureCalender == null)
		{
			return false;
		}
		return LanguageAndCalendar.Instance.CaltureCalender.CheckDate(dateString);
	}

	public static string ReturnResponseError(this HttpResponseMessage StatusMessage)
	{
		try
		{
			return StatusMessage.StatusCode switch
			{
				HttpStatusCode.NotFound => "ارتباط با سرور برقرار نیست".GetTranslate(), 
				HttpStatusCode.InternalServerError => "خطا در سرور", 
				HttpStatusCode.BadRequest => "عدم ارسال صحیح پارامترها به سرور".GetTranslate(), 
				_ => "خطا در سرور".GetTranslate(), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReturnResponseError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2139);
			return "خطا نامشخص";
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<DownLoadStatus>> DownnLoadFile(this Uri url, string downloadPath, CancellationToken Token, long crc = 0L, bool FileDamgeHasError = true)
	{
		string text = downloadPath + ".temp";
		FileInfo fileInfo = new FileInfo(downloadPath);
		WebClient webClient = new WebClient();
		ReturnedSaveFuncInfoWithValue<DownLoadStatus> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<DownLoadStatus>
		{
			value = DownLoadStatus.CancelDownLoad
		};
		try
		{
			if (Token.IsCancellationRequested)
			{
				return returnedSaveFuncInfoWithValue.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "DownnLoadFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2151);
			}
			if (File.Exists(downloadPath))
			{
				if (crc != 0L)
				{
					if (crc == Crc32Algorithm.Compute(File.ReadAllBytes(downloadPath)))
					{
						ReturnedSaveFuncInfoWithValue<DownLoadStatus> obj = new ReturnedSaveFuncInfoWithValue<DownLoadStatus>
						{
							value = DownLoadStatus.FileExsistOnLocal
						};
						returnedSaveFuncInfoWithValue = obj;
						return obj;
					}
					File.Delete(downloadPath);
				}
				else
				{
					File.Delete(downloadPath);
				}
			}
			if (Token.IsCancellationRequested)
			{
				return returnedSaveFuncInfoWithValue.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "DownnLoadFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2163);
			}
			FileUtility.CreateDirectory(fileInfo.DirectoryName);
			webClient.DownloadFile(url, text);
			if (!webClient.ResponseHeaders.AllKeys.Any((string x) => x == "Last-Modified"))
			{
				ReturnedSaveFuncInfoWithValue<DownLoadStatus> obj2 = new ReturnedSaveFuncInfoWithValue<DownLoadStatus>
				{
					value = DownLoadStatus.NotFound
				};
				returnedSaveFuncInfoWithValue = obj2;
				return obj2;
			}
			if (crc == 0L)
			{
				ReturnedSaveFuncInfoWithValue<DownLoadStatus> obj3 = new ReturnedSaveFuncInfoWithValue<DownLoadStatus>
				{
					value = DownLoadStatus.Complate
				};
				returnedSaveFuncInfoWithValue = obj3;
				return obj3;
			}
			if (crc == Crc32Algorithm.Compute(File.ReadAllBytes(text)))
			{
				File.Move(text, downloadPath);
				ReturnedSaveFuncInfoWithValue<DownLoadStatus> obj4 = new ReturnedSaveFuncInfoWithValue<DownLoadStatus>
				{
					value = DownLoadStatus.Complate
				};
				returnedSaveFuncInfoWithValue = obj4;
				return obj4;
			}
			File.Delete(downloadPath);
			if (FileDamgeHasError)
			{
				returnedSaveFuncInfoWithValue.AddError(EnSystem.None, "DownLoaded File Not Correct Download(crc)=>" + downloadPath + " ", "DownnLoadFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2180);
			}
			returnedSaveFuncInfoWithValue.value = DownLoadStatus.Damaged;
			return returnedSaveFuncInfoWithValue;
		}
		catch (Exception exception)
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(exception, "", "DownnLoadFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2187);
			return returnedSaveFuncInfoWithValue;
		}
		finally
		{
			webClient.Dispose();
			if (File.Exists(text))
			{
				File.Delete(text);
			}
		}
	}

	public static T GetAttribute<T>(this MemberInfo member, bool isRequired) where T : Attribute
	{
		object? obj = member.GetCustomAttributes(typeof(T), inherit: false).SingleOrDefault();
		if (obj == null && isRequired)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The {0} attribute must be defined on member {1}", typeof(T).Name, member.Name));
		}
		return (T)obj;
	}

	public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> propertyExpression)
	{
		MemberInfo propertyInformation = GetPropertyInformation(propertyExpression.Body);
		if (propertyInformation == null)
		{
			throw new ArgumentException("No property reference expression was found.", "propertyExpression");
		}
		DisplayNameAttribute attribute = propertyInformation.GetAttribute<DisplayNameAttribute>(isRequired: false);
		if (attribute == null)
		{
			return propertyInformation.Name;
		}
		return attribute.DisplayName;
	}

	public static MemberInfo GetPropertyInformation(Expression propertyExpression)
	{
		try
		{
			MemberExpression memberExpression = propertyExpression as MemberExpression;
			if (memberExpression == null && propertyExpression is UnaryExpression { NodeType: ExpressionType.Convert } unaryExpression)
			{
				memberExpression = unaryExpression.Operand as MemberExpression;
			}
			if (memberExpression != null && memberExpression.Member.MemberType == MemberTypes.Property)
			{
				return memberExpression.Member;
			}
			return null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPropertyInformation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2251);
			return null;
		}
	}

	public static async Task<SemaphoreUtil> GaurdAsync(this SemaphoreSlim sem, bool awaitOnSemaphore, string callerName, int timeout = 60000, params CancellationToken[] tokens)
	{
		SemaphoreUtil ret = new SemaphoreUtil(sem, awaitOnSemaphore, timeout, callerName, tokens);
		await ret.Start();
		return ret;
	}

	[DebuggerHidden]
	public static async Task<bool> CheckWebAddressExsistAsync(this string UrlText, bool IsSSl, bool throwExceptin = false)
	{
		try
		{
			if (string.IsNullOrEmpty(UrlText))
			{
				return false;
			}
			if (IsSSl && !UrlText.ToLower().Contains("https"))
			{
				return false;
			}
			try
			{
				HttpWebResponse obj = (HttpWebResponse)(await WebRequest.Create(new Uri(UrlText)).GetResponseAsync());
				obj.GetResponseStream();
				obj.Close();
				return true;
			}
			catch (UriFormatException)
			{
				if (throwExceptin)
				{
					throw;
				}
				return false;
			}
			catch (IOException)
			{
				if (throwExceptin)
				{
					throw;
				}
				return false;
			}
			catch (WebException ex3)
			{
				if (throwExceptin)
				{
					throw;
				}
				if (ex3.Status == WebExceptionStatus.ProtocolError)
				{
					HttpStatusCode statusCode = ((HttpWebResponse)ex3.Response).StatusCode;
					if (statusCode == HttpStatusCode.Forbidden || statusCode == HttpStatusCode.Unauthorized)
					{
						return true;
					}
				}
				return false;
			}
		}
		catch
		{
			if (throwExceptin)
			{
				throw;
			}
			return false;
		}
	}

	public static Dictionary<T, U> CloneDictionary<T, U>(this Dictionary<T, U> dic)
	{
		Dictionary<T, U> dictionary = new Dictionary<T, U>();
		try
		{
			if (dic != null && dic.Any())
			{
				foreach (T key in dic.Keys)
				{
					if (key != null)
					{
						dic.TryGetValue(key, out var value);
						dictionary.Add(key, value);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CloneDictionary", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2376);
		}
		return dictionary;
	}

	public static int Checksum_EAN13Barcode(this string barcode)
	{
		try
		{
			if (barcode.Length > 12)
			{
				barcode = barcode.Substring(0, 12);
			}
			int num = 0;
			int num2 = 0;
			bool flag = true;
			char[] array = barcode.ToCharArray();
			checked
			{
				for (int i = 0; i < array.Length; i = unchecked(i + 1))
				{
					char c = array[i];
					if (flag)
					{
						num2 += c.ToString().ParseToShort(0);
					}
					else
					{
						num += c.ToString().ParseToShort(0);
					}
					flag = !flag;
				}
				num *= 3;
			}
			int num3 = checked(num2 + num) % 10;
			if (num3 == 0)
			{
				return 0;
			}
			return checked(10 - num3);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Checksum_EAN13Barcode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2402);
			return 0;
		}
	}

	public static Scope WeakSubscribe<TSource, TTarget, TEventArgs>(this EventInfo<TSource, EventHandler<TEventArgs>> info, TSource source, TTarget target, Action<TTarget, object, TEventArgs> action) where TTarget : class where TEventArgs : System.EventArgs
	{
		WeakReference weakTarget = new WeakReference(target, trackResurrection: false);
		EventHandler<TEventArgs> handler = null;
		handler = delegate(object s, TEventArgs e)
		{
			TTarget val = (TTarget)weakTarget.Target;
			if (val != null)
			{
				action(val, s, e);
			}
			else
			{
				info.RemoveHandler(source, handler);
			}
		};
		return info.Subscribe(source, handler);
	}

	public static bool IsEqual(this IEnumerable<ulong> lst1, IEnumerable<ulong> lst2)
	{
		if ((lst1?.Count() ?? 0) != (lst2?.Count() ?? 0))
		{
			return false;
		}
		foreach (ulong item in lst1)
		{
			if (!lst2.Contains(item))
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsEqual(this IEnumerable<uint> lst1, IEnumerable<uint> lst2)
	{
		if ((lst1?.Count() ?? 0) != (lst2?.Count() ?? 0))
		{
			return false;
		}
		foreach (uint item in lst1)
		{
			if (!lst2.Contains(item))
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsEqual(this IEnumerable<string> lst1, IEnumerable<string> lst2)
	{
		if ((lst1?.Count() ?? 0) != (lst2?.Count() ?? 0))
		{
			return false;
		}
		foreach (string item in lst1)
		{
			if (!lst2.Contains(item))
			{
				return false;
			}
		}
		return true;
	}

	public static string PublicKey(this string pubKey)
	{
		try
		{
			if (string.IsNullOrEmpty(pubKey))
			{
				return "";
			}
			int num = pubKey.IndexOf("-----BEGIN PUBLIC KEY-----\r\n");
			int num2 = pubKey.IndexOf("-----END PUBLIC KEY-----");
			pubKey = checked(pubKey.Substring(num + "-----BEGIN PUBLIC KEY-----".Length, num2 - num - "\r\n-----BEGIN PUBLIC KEY-----".Length));
			return pubKey;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PublicKey", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2463);
			return pubKey;
		}
	}

	public static string PrivateKey(this string privKey)
	{
		try
		{
			if (string.IsNullOrEmpty(privKey))
			{
				return "";
			}
			int num = privKey.IndexOf("-----BEGIN PRIVATE KEY-----\r\n");
			int num2 = privKey.IndexOf("-----END PRIVATE KEY-----");
			privKey = checked(privKey.Substring(num + "-----BEGIN PRIVATE KEY-----".Length, num2 - num - "\r\n-----BEGIN PRIVATE KEY-----".Length));
			return privKey;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PrivateKey", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2479);
			return privKey;
		}
	}

	public static DateTime ChangeYear(this DateTime date, int year)
	{
		return new DateTime(year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
	}

	public static decimal ConvertPriceStringToDecimal(this string str, short digitAfterPoint, decimal defValue = 0m)
	{
		try
		{
			Match match = new Regex("\\D*\\(\\)\\D*").Match(str);
			if (match.Success)
			{
				return match.ToString().RemoveNoNumbers().ParseToDecimal();
			}
			if (defValue != 0m)
			{
				return defValue;
			}
			return str.ParseToDecimal(0m, digitAfterPoint);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ConvertPriceStringToDecimal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2502);
		}
		return 0m;
	}

	public static EnPasswordScore CheckPasswordStrength(this string password)
	{
		try
		{
			int num = 0;
			checked
			{
				if (password.Length >= 8 && password.Length <= 10)
				{
					num++;
				}
				if (password.Length >= 11 && password.Length <= 15)
				{
					num++;
				}
				if (password.Length >= 16)
				{
					num++;
				}
				if (Regex.IsMatch(password, "[a-z]"))
				{
					num++;
				}
				if (Regex.IsMatch(password, "[A-Z]"))
				{
					num++;
				}
				if (Regex.IsMatch(password, "\\d"))
				{
					num++;
				}
				if (Regex.IsMatch(password, "[^\\w]"))
				{
					num++;
				}
			}
			return (EnPasswordScore)checked((short)num);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckPasswordStrength", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2521);
			throw;
		}
	}

	public static long ConvertToMB(object bytes)
	{
		if (bytes is long num)
		{
			return num / 1048576;
		}
		return 0L;
	}

	public static Dictionary<string, object> GetCPUInfo(ManagementObject obj)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		try
		{
			dictionary["CPU Name"] = ((ManagementBaseObject)obj)["Name"] ?? "Unknown";
			dictionary["Cores"] = ((ManagementBaseObject)obj)["NumberOfCores"] ?? ((object)0);
			dictionary["Logical Processors"] = ((ManagementBaseObject)obj)["NumberOfLogicalProcessors"] ?? ((object)0);
			dictionary["Max Clock Speed"] = ((((ManagementBaseObject)obj)["MaxClockSpeed"] != null) ? (((ManagementBaseObject)obj)["MaxClockSpeed"]?.ToString() + " MHz") : "Unknown");
			dictionary["CPU Serial Number"] = ((ManagementBaseObject)obj)["ProcessorId"] ?? "Unknown";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "Error retrieving CPU information.", "GetCPUInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2541);
		}
		return dictionary;
	}

	public static Dictionary<string, object> GetRAMInfo(ManagementObject obj)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		try
		{
			dictionary["RAM Capacity"] = ConvertToMB(((ManagementBaseObject)obj)["Capacity"]) + " MB";
			dictionary["RAM Serial Number"] = ((ManagementBaseObject)obj)["SerialNumber"] ?? "Unknown";
			dictionary["RAM Part Number"] = ((ManagementBaseObject)obj)["PartNumber"] ?? "Unknown";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "Error retrieving RAM information.", "GetRAMInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2553);
		}
		return dictionary;
	}

	public static Dictionary<string, object> GetGPUInfo(ManagementObject obj)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		try
		{
			dictionary["GPU Name"] = ((ManagementBaseObject)obj)["Name"] ?? "Unknown";
			dictionary["GPU Memory"] = ConvertToMB(((ManagementBaseObject)obj)["AdapterRAM"]) + " MB";
			dictionary["GPU Serial Number"] = ((ManagementBaseObject)obj)["DeviceID"] ?? "Unknown";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "Error retrieving GPU information.", "GetGPUInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2566);
		}
		return dictionary;
	}

	public static (long totalSize, long totalUsedSpace, long totalFreeSpace, int partitionCount) GetDiskInfo(ManagementObjectSearcher searcher)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		long num2 = 0L;
		long num3 = 0L;
		long num4 = 0L;
		checked
		{
			try
			{
				ManagementObjectEnumerator enumerator = searcher.Get().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ManagementObject val = (ManagementObject)enumerator.Current;
						long num5 = Convert.ToInt64(((ManagementBaseObject)val)["Size"]);
						long num6 = Convert.ToInt64(((ManagementBaseObject)val)["FreeSpace"]);
						long num7 = num5 - num6;
						num3 += num5;
						num2 += num7;
						num4 += num6;
						num++;
					}
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetDiskInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2587);
			}
			return (totalSize: num3, totalUsedSpace: num2, totalFreeSpace: num4, partitionCount: num);
		}
	}

	public static (long totalRAM, long usedRAM, long freeRAM) GetRAMUsage()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		long num = 0L;
		long num2 = 0L;
		long num3 = 0L;
		checked
		{
			try
			{
				ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ManagementObject val = (ManagementObject)enumerator.Current;
						num = Convert.ToInt64(((ManagementBaseObject)val)["TotalVisibleMemorySize"]);
						num3 = Convert.ToInt64(((ManagementBaseObject)val)["FreePhysicalMemory"]);
						num2 = num - num3;
					}
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetRAMUsage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2604);
			}
			return (totalRAM: ConvertToMB(num * 1024), usedRAM: ConvertToMB(num2 * 1024), freeRAM: ConvertToMB(num3 * 1024));
		}
	}

	public static string GetMotherboardSerialNumber()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ManagementObjectSearcher val = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
			try
			{
				ManagementObjectEnumerator enumerator = val.Get().GetEnumerator();
				try
				{
					if (enumerator.MoveNext())
					{
						return ((ManagementBaseObject)(ManagementObject)enumerator.Current)["SerialNumber"]?.ToString() ?? "Unknown";
					}
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMotherboardSerialNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2615);
		}
		return "Unknown";
	}

	public static string GetCpuSerialNumber()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ManagementObjectSearcher val = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
			try
			{
				ManagementObjectEnumerator enumerator = val.Get().GetEnumerator();
				try
				{
					if (enumerator.MoveNext())
					{
						return ((ManagementBaseObject)(ManagementObject)enumerator.Current)["ProcessorId"]?.ToString() ?? "Unknown";
					}
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCpuSerialNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2626);
		}
		return "Unknown";
	}

	public static Dictionary<string, string> GetHardDriveInfo()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		try
		{
			ManagementObjectSearcher val = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
			try
			{
				ManagementObjectSearcher val2 = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
				try
				{
					ManagementObjectCollection val3 = val2.Get();
					ManagementObjectEnumerator enumerator = val.Get().GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string value = ((ManagementBaseObject)(ManagementObject)enumerator.Current)["Model"]?.ToString() ?? "Unknown";
							string value2 = "None";
							ManagementObjectEnumerator enumerator2 = val3.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									ManagementObject val4 = (ManagementObject)enumerator2.Current;
									if (((ManagementBaseObject)val4)["SerialNumber"] != null)
									{
										value2 = ((ManagementBaseObject)val4)["SerialNumber"]?.ToString() ?? "None";
										break;
									}
								}
							}
							finally
							{
								((IDisposable)enumerator2)?.Dispose();
							}
							dictionary["Hard Drive Model"] = value;
							dictionary["Serial Number"] = value2;
						}
					}
					finally
					{
						((IDisposable)enumerator)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetHardDriveInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2654);
		}
		return dictionary;
	}

	public static string GetHardSerial()
	{
		string result = "";
		try
		{
			result = GetMotherboardSerialNumber() + GetCpuSerialNumber();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetHardSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2664);
		}
		return result;
	}

	public static string UniqueIdentifier()
	{
		string text = "";
		try
		{
			Dictionary<string, string> hardDriveInfo = GetHardDriveInfo();
			text = GetMotherboardSerialNumber() + "_" + GetCpuSerialNumber();
			if (hardDriveInfo != null && hardDriveInfo.Any())
			{
				foreach (KeyValuePair<string, string> item in hardDriveInfo)
				{
					text = text + "_" + item.Value.Trim().FixString().Replace(" ", "");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UniqueIdentifier", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2678);
		}
		return text;
	}

	public static EnDeviceGroup GetDeviceGroup(this DeviceModelEnumType type)
	{
		try
		{
			switch (type)
			{
			case DeviceModelEnumType.NP290:
				return EnDeviceGroup._290;
			default:
				throw new NotImplementedException();
			case DeviceModelEnumType.EC160:
			case DeviceModelEnumType.ETC132:
			case DeviceModelEnumType.N1:
			case DeviceModelEnumType.RETC132:
			case DeviceModelEnumType.RN1:
			case DeviceModelEnumType.NP2815:
			case DeviceModelEnumType.NP302:
			case DeviceModelEnumType.NP261:
			case DeviceModelEnumType.NP550:
			case DeviceModelEnumType.NP395:
			case DeviceModelEnumType.NP586:
			case DeviceModelEnumType.NP20:
			case DeviceModelEnumType.NP587:
			case DeviceModelEnumType.NP255:
			case DeviceModelEnumType.NP770:
			case DeviceModelEnumType.NP756P:
			case DeviceModelEnumType.NP761P:
			case DeviceModelEnumType.ZKT:
			case DeviceModelEnumType.NP620:
				return EnDeviceGroup._290;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDeviceGroup", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2732);
			throw;
		}
	}

	public static List<DeviceModelEnumType> GetDeviceModelList(this EnDeviceGroup group)
	{
		try
		{
			List<DeviceModelEnumType> list = new List<DeviceModelEnumType>();
			switch (group)
			{
			case EnDeviceGroup._290:
				list.Add(DeviceModelEnumType.NP290);
				break;
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDeviceModelList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Utilities.cs", 2750);
			throw;
		}
	}
}

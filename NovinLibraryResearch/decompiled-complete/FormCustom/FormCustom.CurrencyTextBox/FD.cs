using System;
using System.Windows.Forms;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.CurrencyTextBox;

public class FD
{
	public const Keys AddYearWithStepKey = 89;

	public const Keys AddMonthWithStepKey = 77;

	public const Keys AddDayWithStepKey = 68;

	public const Keys ClearKey = 32;

	public const Keys TodayKey = 27;

	public static short Year;

	public static short Month;

	public static short Day;

	public static bool AllowNull;

	public static int GetSelStart(int selStart)
	{
		try
		{
			if (selStart <= 4)
			{
				return 0;
			}
			if (selStart >= 5 && selStart <= 7)
			{
				return 5;
			}
			if (selStart >= 8)
			{
				return 8;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSelStart", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FD.cs", 27);
		}
		return 0;
	}

	public static int GetSelLen(int selStart)
	{
		try
		{
			if (selStart <= 4)
			{
				return 4;
			}
			if (selStart >= 5 && selStart <= 7)
			{
				return 2;
			}
			if (selStart >= 8)
			{
				return 2;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSelLen", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FD.cs", 39);
		}
		return 0;
	}

	public static string ProgressKey(ref KeyEventArgs e, string txt, ref int selStart, ref int sellen)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		string result = "";
		try
		{
			result = ChangeDate(ref e, txt, ref selStart, ref sellen);
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 32)
			{
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 32)
					{
						result = "";
					}
				}
				else
				{
					result = DateTime.Now.MiladiToCalture();
				}
			}
			else if ((int)keyCode != 68)
			{
				if ((int)keyCode != 77)
				{
					if ((int)keyCode == 89)
					{
						result = Calendar.AddToShamsiDate(Year, txt, EnPartDate.Year);
					}
				}
				else
				{
					result = Calendar.AddToShamsiDate(Month, txt, EnPartDate.Mounth);
				}
			}
			else
			{
				result = Calendar.AddToShamsiDate(Day, txt, EnPartDate.Day);
			}
			e = new KeyEventArgs((Keys)0)
			{
				Handled = true
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ProgressKey", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FD.cs", 69);
		}
		return result;
	}

	private static string ChangeDate(ref KeyEventArgs e, string txt, ref int selStart, ref int sellen)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Invalid comparison between Unknown and I4
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Invalid comparison between Unknown and I4
		string result = "";
		try
		{
			short count = 0;
			if ((int)e.KeyCode == 38)
			{
				count = 1;
			}
			if ((int)e.KeyCode == 40)
			{
				count = -1;
			}
			switch (selStart)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			{
				Keys keyCode = e.KeyCode;
				if ((int)keyCode != 37)
				{
					if ((int)keyCode == 39)
					{
						selStart = 5;
						sellen = 2;
					}
					else
					{
						selStart = 0;
						sellen = 4;
					}
				}
				else
				{
					selStart = 8;
					sellen = 2;
				}
				result = Calendar.AddToShamsiDate(count, txt, EnPartDate.Year);
				break;
			}
			case 5:
			case 6:
			{
				Keys keyCode = e.KeyCode;
				if ((int)keyCode != 37)
				{
					if ((int)keyCode == 39)
					{
						selStart = 8;
						sellen = 2;
					}
					else
					{
						selStart = 5;
						sellen = 4;
					}
				}
				else
				{
					selStart = 0;
					sellen = 4;
				}
				result = Calendar.AddToShamsiDate(count, txt, EnPartDate.Mounth);
				break;
			}
			case 8:
			case 9:
			{
				Keys keyCode = e.KeyCode;
				if ((int)keyCode != 37)
				{
					if ((int)keyCode == 39)
					{
						selStart = 0;
						sellen = 4;
					}
					else
					{
						selStart = 8;
						sellen = 2;
					}
				}
				else
				{
					selStart = 5;
					sellen = 2;
				}
				result = Calendar.AddToShamsiDate(count, txt, EnPartDate.Day);
				break;
			}
			default:
				result = txt;
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FD.cs", 117);
		}
		return result;
	}
}

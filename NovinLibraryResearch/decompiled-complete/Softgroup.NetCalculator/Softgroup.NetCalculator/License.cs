using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using _4FaFXdbWCf2EwSeyTf;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using mKbaIlVHVypdZRXAVh;
using og5v0aGw6yPMOuFqdk;

namespace Softgroup.NetCalculator;

public sealed class License
{
	private enum pLZE7k67JKGWADp2KH
	{

	}

	private const int cPEjmYIMH = 19;

	private const string oRvbwAa78 = "Softgroup s.r.l.";

	private const int wT2cpGf42 = 24;

	private const int DCw5sVMLH = 6;

	private const int RMlLYiEXP = 10;

	private const int kQlhEvf9t = 2;

	private const int vTal0unf9 = 14;

	private const int qtvxFY3hu = 18;

	private const string C5WEEhbTS = ".Net Calculator Control 2.0.0.0";

	private const string GJZXBpJ5f = "Software\\Softgroup\\NetCalculator";

	private const int OWGopKWYU = 25;

	private const string tYiabx6l5 = "ABCDEFGHIJKLMNOPQRSTUVWYZ0123456789";

	private static bool _2hwiDuqXu;

	private static string cmetaw0OP;

	private static string c8QkQfBmJ;

	private static string fy3OdaBhA;

	[Description("Gets/Sets the license key for the product")]
	public static string LicenseKey
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return cmetaw0OP;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			cmetaw0OP = value;
		}
	}

	[Description("Gets/Sets the license company name for the product")]
	public static string LicenseName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return c8QkQfBmJ;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			c8QkQfBmJ = value;
		}
	}

	[Description("Gets/Sets the license user name (usually and email address) for the product")]
	public static string LicenseUser
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return fy3OdaBhA;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			fy3OdaBhA = value;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static License()
	{
		eOWyXJHApIkypx1lmV.hB8jVZbTXcmOk();
		_2hwiDuqXu = false;
		cmetaw0OP = "";
		c8QkQfBmJ = "";
		fy3OdaBhA = "";
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private License()
	{
		eOWyXJHApIkypx1lmV.hB8jVZbTXcmOk();
		base._002Ector();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string kZ44kdRMh(pLZE7k67JKGWADp2KH P_0)
	{
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
		byte[] array = new byte[0];
		string text = "";
		int num = 0;
		checked
		{
			try
			{
				string text2 = "";
				if (ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(968).Length == 0)
				{
					throw new Exception(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1004));
				}
				if (ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1048).Length == 0)
				{
					throw new Exception(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1114));
				}
				if (c8QkQfBmJ.Length == 0)
				{
					throw new Exception(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1158));
				}
				if (fy3OdaBhA.Length == 0)
				{
					throw new Exception(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1202));
				}
				text = ((P_0 != 0) ? (ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1338) + c8QkQfBmJ + ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1436)) : (ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1240) + c8QkQfBmJ + fy3OdaBhA));
				array = unicodeEncoding.GetBytes(text);
				array = mD5CryptoServiceProvider.ComputeHash(array);
				text = Convert.ToBase64String(array);
				text = text.ToUpper();
				if (text.Length > 19)
				{
					text = text.Substring(0, 19);
				}
				else if (text.Length < 19)
				{
					array = new byte[19 - text.Length - 1 + 1];
					int lowerBound = array.GetLowerBound(0);
					int upperBound = array.GetUpperBound(0);
					for (num = lowerBound; num <= upperBound; num++)
					{
						array[num] = 88;
					}
					text += Encoding.ASCII.GetString(array);
				}
				int length = text.Length;
				for (num = 1; num <= length; num++)
				{
					if (!ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1450).Contains(text.Substring(num - 1, 1)))
					{
						StringType.MidStmtStr(ref text, num, 1, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1524));
					}
				}
				return text;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				throw ex2;
			}
			finally
			{
				if (unicodeEncoding != null)
				{
					unicodeEncoding = null;
				}
				if (mD5CryptoServiceProvider != null)
				{
					mD5CryptoServiceProvider = null;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int YdHKfVQaA(string P_0)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		num2 = 3;
		num3 = 0;
		num4 = P_0.Length;
		checked
		{
			for (num5 = num4; num5 >= 1; num5 += -1)
			{
				num3 += Strings.Asc(P_0.Substring(num5 - 1, 1)) * num2;
				num2 = 4 - num2;
			}
			num = unchecked(num3 % 10);
			if (num == 0)
			{
				return 0;
			}
			return 10 - num;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string hoNe6CCpP(string P_0)
	{
		string text = "";
		int num = 0;
		if (P_0.Length == 25)
		{
			int num2 = checked(P_0.Length - 1);
			for (num = 0; num <= num2; num = checked(num + 1))
			{
				if (!(num == 2 || num == 6 || num == 10 || num == 14 || num == 18 || num == 24))
				{
					text += P_0.Substring(num, 1);
				}
			}
		}
		else
		{
			text = P_0;
		}
		return text;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string FoMC4pDWf(pLZE7k67JKGWADp2KH P_0)
	{
		string text = "";
		string text2 = "";
		checked
		{
			try
			{
				string text3 = "";
				text = kZ44kdRMh(P_0);
				switch (P_0)
				{
				case (pLZE7k67JKGWADp2KH)0:
					text = text.Insert(2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1530).Substring(0, 1));
					break;
				case (pLZE7k67JKGWADp2KH)1:
					text = text.Insert(2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1604).Substring(1, 1));
					break;
				case (pLZE7k67JKGWADp2KH)2:
					text = text.Insert(2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1678).Substring(2, 1));
					break;
				case (pLZE7k67JKGWADp2KH)3:
					text = text.Insert(2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1752).Substring(3, 1));
					break;
				case (pLZE7k67JKGWADp2KH)4:
					text = text.Insert(2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1826).Substring(4, 1));
					break;
				case (pLZE7k67JKGWADp2KH)5:
					text = text.Insert(2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1900).Substring(5, 1));
					break;
				}
				text = text.Insert(6, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(1974).Substring(DateAndTime.Now.Day - 1, 1));
				text = text.Insert(10, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2048).Substring(DateAndTime.Now.Month - 1, 1));
				text2 = osDYVuQCk(DateAndTime.Now.Year);
				text = text.Insert(14, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2122).Substring((int)Math.Round(Conversion.Val(text2.Substring(0, 1))), 1));
				text = text.Insert(18, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2196).Substring((int)Math.Round(Conversion.Val(text2.Substring(1, 1))), 1));
				return text.Insert(24, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2270).Substring(YdHKfVQaA(text), 1));
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				throw ex2;
			}
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool YBgmbmdmP(string P_0, ref pLZE7k67JKGWADp2KH P_1, ref bool P_2, ref int P_3)
	{
		DateTime dateTime = default(DateTime);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		pLZE7k67JKGWADp2KH pLZE7k67JKGWADp2KH = (pLZE7k67JKGWADp2KH)0;
		bool flag = false;
		bool flag2 = false;
		checked
		{
			bool result;
			try
			{
				result = false;
				P_1 = (pLZE7k67JKGWADp2KH)0;
				P_2 = false;
				P_3 = 0;
				if (P_0.Length == 25 && Operators.CompareString(P_0.Substring(24, 1), ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2344).Substring(YdHKfVQaA(P_0.Substring(0, P_0.Length - 1)), 1), false) == 0)
				{
					string text = P_0.Substring(2, 1);
					if (Operators.CompareString(text, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2418).Substring(0, 1), false) == 0)
					{
						pLZE7k67JKGWADp2KH = (pLZE7k67JKGWADp2KH)0;
						goto IL_0173;
					}
					if (Operators.CompareString(text, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2492).Substring(1, 1), false) == 0)
					{
						pLZE7k67JKGWADp2KH = (pLZE7k67JKGWADp2KH)1;
						goto IL_0173;
					}
					if (Operators.CompareString(text, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2566).Substring(2, 1), false) == 0)
					{
						pLZE7k67JKGWADp2KH = (pLZE7k67JKGWADp2KH)2;
						goto IL_0173;
					}
					if (Operators.CompareString(text, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2640).Substring(3, 1), false) == 0)
					{
						pLZE7k67JKGWADp2KH = (pLZE7k67JKGWADp2KH)3;
						goto IL_0173;
					}
					if (Operators.CompareString(text, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2714).Substring(4, 1), false) == 0)
					{
						pLZE7k67JKGWADp2KH = (pLZE7k67JKGWADp2KH)4;
						goto IL_0173;
					}
					if (Operators.CompareString(text, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2788).Substring(5, 1), false) == 0)
					{
						pLZE7k67JKGWADp2KH = (pLZE7k67JKGWADp2KH)5;
						goto IL_0173;
					}
				}
				goto end_IL_001f;
				IL_0173:
				num = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2862).IndexOf(P_0.Substring(6, 1)) + 1;
				num2 = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(2936).IndexOf(P_0.Substring(10, 1)) + 1;
				num3 = gMb6BRaRL(Conversions.ToString(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3010).IndexOf(P_0.Substring(14, 1))) + Conversions.ToString(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3084).IndexOf(P_0.Substring(18, 1))));
				if (unchecked(num >= 1 && num <= 31 && num2 >= 1 && num2 <= 12))
				{
					dateTime = DateAndTime.DateSerial(num3, num2, num);
					switch (pLZE7k67JKGWADp2KH)
					{
					case (pLZE7k67JKGWADp2KH)1:
						P_3 = (int)DateAndTime.DateDiff((DateInterval)4, dateTime, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1);
						if (P_3 > 15)
						{
							flag2 = true;
						}
						else
						{
							P_3 = 15 - P_3;
						}
						break;
					case (pLZE7k67JKGWADp2KH)2:
						P_3 = (int)DateAndTime.DateDiff((DateInterval)4, dateTime, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1);
						if (P_3 > 30)
						{
							flag2 = true;
						}
						else
						{
							P_3 = 30 - P_3;
						}
						break;
					case (pLZE7k67JKGWADp2KH)3:
						P_3 = (int)DateAndTime.DateDiff((DateInterval)4, dateTime, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1);
						if (P_3 > 60)
						{
							flag2 = true;
						}
						else
						{
							P_3 = 60 - P_3;
						}
						break;
					case (pLZE7k67JKGWADp2KH)4:
						P_3 = (int)DateAndTime.DateDiff((DateInterval)4, dateTime, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1);
						if (P_3 > 90)
						{
							flag2 = true;
						}
						else
						{
							P_3 = 90 - P_3;
						}
						break;
					case (pLZE7k67JKGWADp2KH)5:
						P_3 = (int)DateAndTime.DateDiff((DateInterval)4, dateTime, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1);
						if (P_3 > 366)
						{
							flag2 = true;
						}
						else
						{
							P_3 = 366 - P_3;
						}
						break;
					default:
						P_3 = -1;
						flag2 = false;
						break;
					}
					if (Operators.CompareString(kZ44kdRMh(pLZE7k67JKGWADp2KH), hoNe6CCpP(P_0), false) == 0)
					{
						P_1 = pLZE7k67JKGWADp2KH;
						P_2 = flag2;
						result = true;
					}
					else
					{
						result = false;
					}
				}
				end_IL_001f:;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				throw ex2;
			}
			return result;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int gMb6BRaRL(string P_0)
	{
		return checked((int)Math.Round(Conversion.Val(P_0) + 2000.0));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string osDYVuQCk(int P_0)
	{
		string text = (P_0 % 100).ToString();
		if (text.Length == 1)
		{
			text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3158) + text;
		}
		return text;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void q2lZj53xp()
	{
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		RegistryKey val = null;
		_5ed0Svg0OwdWJaf5UT _5ed0Svg0OwdWJaf5UT = null;
		pLZE7k67JKGWADp2KH pLZE7k67JKGWADp2KH = (pLZE7k67JKGWADp2KH)0;
		bool flag = false;
		int num = 0;
		try
		{
			if (_2hwiDuqXu)
			{
				return;
			}
			if ((c8QkQfBmJ.Length == 0) & (fy3OdaBhA.Length == 0) & (cmetaw0OP.Length == 0))
			{
				val = Registry.CurrentUser.OpenSubKey(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3164), false);
				if (val != null)
				{
					c8QkQfBmJ = val.GetValue(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3232), (object)"").ToString();
					fy3OdaBhA = val.GetValue(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3258), (object)"").ToString();
					cmetaw0OP = val.GetValue(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3284), (object)"").ToString();
				}
			}
			if ((c8QkQfBmJ.Length == 0) | (fy3OdaBhA.Length == 0) | (cmetaw0OP.Length == 0))
			{
				_5ed0Svg0OwdWJaf5UT = new _5ed0Svg0OwdWJaf5UT();
				_5ed0Svg0OwdWJaf5UT.LabMessage.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3308);
				((Form)_5ed0Svg0OwdWJaf5UT).ShowDialog();
				_2hwiDuqXu = true;
			}
			else if (YBgmbmdmP(cmetaw0OP, ref pLZE7k67JKGWADp2KH, ref flag, ref num))
			{
				if (flag)
				{
					_5ed0Svg0OwdWJaf5UT = new _5ed0Svg0OwdWJaf5UT();
					_5ed0Svg0OwdWJaf5UT.LabMessage.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3370);
					((Form)_5ed0Svg0OwdWJaf5UT).ShowDialog();
					_2hwiDuqXu = true;
				}
				else if (pLZE7k67JKGWADp2KH != 0 && pLZE7k67JKGWADp2KH != (pLZE7k67JKGWADp2KH)5)
				{
					_5ed0Svg0OwdWJaf5UT = new _5ed0Svg0OwdWJaf5UT();
					_5ed0Svg0OwdWJaf5UT.LabMessage.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3416) + num + ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3476);
					((Form)_5ed0Svg0OwdWJaf5UT).ShowDialog();
					_2hwiDuqXu = true;
				}
			}
			else
			{
				_5ed0Svg0OwdWJaf5UT = new _5ed0Svg0OwdWJaf5UT();
				_5ed0Svg0OwdWJaf5UT.LabMessage.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3492);
				((Form)_5ed0Svg0OwdWJaf5UT).ShowDialog();
				_2hwiDuqXu = true;
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			_2hwiDuqXu = false;
			throw ex2;
		}
		finally
		{
			if (val != null)
			{
				val.Close();
				val = null;
			}
			_5ed0Svg0OwdWJaf5UT = null;
		}
	}
}

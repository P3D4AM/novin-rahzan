using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using Microsoft.Win32;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Logger;
using TimeAttendanceDAL.Properties;

namespace TimeAttendanceDAL.Utility;

public static class Utility
{
	public class MyNotif
	{
		public string Message { get; set; } = "";


		public string Title { get; set; } = "";


		public Image Image { get; set; }

		public short DurationTime { get; set; } = 5;

	}

	private class Gdi32
	{
		public const int Srccopy = 13369376;

		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteDC(IntPtr hDC);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hDc, IntPtr hObject);
	}

	private class User32
	{
		public struct Rect
		{
			public int left;

			public int top;

			public int right;

			public int bottom;
		}

		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
	}

	public static TaskCompletionSource<IWin32Window> OwnerForm = new TaskCompletionSource<IWin32Window>();

	public static TaskCompletionSource<DataGridViewX> GridLastTaradod = new TaskCompletionSource<DataGridViewX>();

	public static TaskCompletionSource<DataGridView> GridReportDevices = new TaskCompletionSource<DataGridView>();

	private static SemaphoreSlim _writeToFileSemaphore = new SemaphoreSlim(1, 1);

	private static string _lastMessage = "";

	private static Task _showNotificationTask = null;

	private static CancellationTokenSource _showNotificationToken = null;

	private static readonly BufferBlock<MyNotif> QueueNotifs = new BufferBlock<MyNotif>();

	private static CancellationTokenSource _showNotifTokenSource = null;

	private static SemaphoreSlim _showNotifSemaphore = new SemaphoreSlim(1, 1);

	public static Image CaptureScreen()
	{
		return CaptureWindow(User32.GetDesktopWindow());
	}

	public static Image CaptureWindow(IntPtr handle)
	{
		IntPtr windowDC = User32.GetWindowDC(handle);
		User32.Rect rect = default(User32.Rect);
		User32.GetWindowRect(handle, ref rect);
		int nWidth = rect.right - rect.left;
		int nHeight = rect.bottom - rect.top;
		IntPtr intPtr = Gdi32.CreateCompatibleDC(windowDC);
		IntPtr intPtr2 = Gdi32.CreateCompatibleBitmap(windowDC, nWidth, nHeight);
		IntPtr hObject = Gdi32.SelectObject(intPtr, intPtr2);
		Gdi32.BitBlt(intPtr, 0, 0, nWidth, nHeight, windowDC, 0, 0, 13369376);
		Gdi32.SelectObject(intPtr, hObject);
		Gdi32.DeleteDC(intPtr);
		User32.ReleaseDC(handle, windowDC);
		Bitmap result = Image.FromHbitmap(intPtr2);
		Gdi32.DeleteObject(intPtr2);
		return (Image)(object)result;
	}

	public static void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
	{
		CaptureWindow(handle).Save(filename, format);
	}

	public static void CaptureScreenToFile(string filename, ImageFormat format)
	{
		CaptureScreen().Save(filename, format);
	}

	public static string CaptureScreenToString()
	{
		return ImageToString(CaptureWindow(User32.GetDesktopWindow()));
	}

	public static string ImageToString(Image imageIn)
	{
		MemoryStream memoryStream = new MemoryStream();
		imageIn.Save((Stream)memoryStream, ImageFormat.Gif);
		return Convert.ToBase64String(memoryStream.ToArray());
	}

	public static Image StringToImage(string imgStr)
	{
		using MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(imgStr));
		return Image.FromStream((Stream)memoryStream);
	}

	public static string GetOsInfo()
	{
		string text = "";
		try
		{
			OperatingSystem oSVersion = Environment.OSVersion;
			Version version = oSVersion.Version;
			switch (oSVersion.Platform)
			{
			case PlatformID.Win32Windows:
				switch (version.Minor)
				{
				case 0:
					text = "95";
					break;
				case 10:
					text = ((version.Revision.ToString() == "2222A") ? "98SE" : "98");
					break;
				case 90:
					text = "Me";
					break;
				}
				break;
			case PlatformID.Win32NT:
				text = version.Major switch
				{
					3 => "NT3.51", 
					4 => "NT4.0", 
					5 => (version.Minor == 0) ? "2000" : "XP", 
					6 => version.Minor switch
					{
						0 => "Vista", 
						1 => "7", 
						2 => "8", 
						_ => "8.1", 
					}, 
					10 => "10", 
					_ => text, 
				};
				break;
			}
			if (text != "")
			{
				text = "Windows" + text;
				if (oSVersion.ServicePack != "")
				{
					text += oSVersion.ServicePack;
				}
			}
		}
		catch
		{
		}
		return text;
	}

	public static List<IPAddress> GetLocalIps(bool onlyIpV4 = false)
	{
		try
		{
			List<IPAddress> list = Dns.GetHostAddresses(Dns.GetHostName()).ToList();
			return onlyIpV4 ? list.Where((IPAddress ip) => ip.AddressFamily == AddressFamily.InterNetwork).ToList() : list;
		}
		catch
		{
			return new List<IPAddress>();
		}
	}

	public static string GetInternetIp()
	{
		try
		{
			foreach (IPAddress localIp in GetLocalIps())
			{
				if (localIp.AddressFamily == AddressFamily.InterNetwork)
				{
					return localIp.MapToIPv4().ToString();
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return "";
	}

	public static void SendTelegramMessage(string contextGuid, string message, Guid guid = default(Guid), string contactInfo = "[REDACTED]", EnTelegramType contactType = 1, string apiKey = "[REDACTED]")
	{
		// [SANITIZED] تله‌متری حذف شد: این متد پیش‌تر نسخهٔ برنامه، سیستم‌عامل، سریال قفل،
		// [SANITIZED] نام کاربر ویندوز و IPهای محلی را به ربات تلگرام سازنده ارسال می‌کرد؛
		// [SANITIZED] اکنون عمداً هیچ داده‌ای ارسال نمی‌کند.
	}

	public static Calendar GetCalendar(CalendarType ct)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected I4, but got Unknown
		CultureInfo cultureInfo = new CultureInfo("fa-Ir");
		object obj;
		switch ((int)ct)
		{
		case 0:
			obj = new PersianCalendar();
			cultureInfo = new CultureInfo("fa-Ir");
			break;
		case 1:
			obj = new HijriCalendar();
			cultureInfo = new CultureInfo("ar-sa");
			break;
		case 2:
			obj = new GregorianCalendar();
			cultureInfo = new CultureInfo("es-ES");
			break;
		default:
			obj = new PersianCalendar();
			break;
		}
		cultureInfo.DateTimeFormat.Calendar = (Calendar)obj;
		Thread.CurrentThread.CurrentCulture = cultureInfo;
		return (Calendar)obj;
	}

	public static ReturnedSaveFuncInfo Ping(string ip)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ReturnedSaveFuncInfo val = new ReturnedSaveFuncInfo();
		try
		{
			if (!IsValidIp(ip))
			{
				string text = "آی پی معتبر نمی باشد \r\n ip=" + ip;
				val.AddError((EnSystem)2, text, "", "Ping", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 321, (StringBuilder)null);
				return val;
			}
			PingReply pingReply = new Ping().Send(ip);
			if (pingReply == null || pingReply.Status.ToString() != "Success")
			{
				string text = "پینگ ناموفق است \r\n ip=" + ip;
				val.AddError((EnSystem)2, text, "", "Ping", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 329, (StringBuilder)null);
				return val;
			}
			return val;
		}
		catch (PingException ex)
		{
			if (!ex.Message.ToLower().Contains("during a ping request".ToLower()))
			{
				WebErrorLog.ErrorLogInstance.StartLog((Exception)ex, "", "Ping", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 337);
			}
			string text2 = "خطا در هنگام پینگ آی پی \r\n ip=" + ip;
			val.AddError((EnSystem)2, text2 + "\r\n " + ex.Message, "", "Ping", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 339, (StringBuilder)null);
			return val;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> PingAsync(string ip)
	{
		string ip2 = ip;
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!IsValidIp(ip2))
			{
				string text = "آی پی معتبر نمی باشد \r\n ip=" + ip2;
				res.AddError((EnSystem)2, text, "", "PingAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 353, (StringBuilder)null);
				return res;
			}
			Ping ping = new Ping();
			PingReply pingResult = null;
			await Task.Run(delegate
			{
				pingResult = ping.Send(ip2);
			});
			if (pingResult == null || pingResult.Status.ToString() != "Success")
			{
				string text = "پینگ ناموفق است \r\n ip=" + ip2;
				res.AddError((EnSystem)2, text, "", "PingAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 365, (StringBuilder)null);
				return res;
			}
		}
		catch (PingException ex)
		{
			if (!ex.Message.ToLower().Contains("during a ping request".ToLower()))
			{
				WebErrorLog.ErrorLogInstance.StartLog((Exception)ex, "", "PingAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 373);
			}
			string text2 = "خطا در هنگام پینگ آی پی \r\n ip=" + ip2;
			res.AddError((EnSystem)2, text2 + "\r\n " + ex.Message, "", "PingAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 375, (StringBuilder)null);
			return res;
		}
		return res;
	}

	private static bool IsValidIp(string ip)
	{
		try
		{
			string[] array = TextManager.Instance.RemoveNoNumbers(ip, ".").Split(new char[1] { '.' });
			return array.Length == 4 && array.All((string part) => part.Length > 0 && int.Parse(part) <= 255);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "IsValidIp", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 390);
		}
		return false;
	}

	public static decimal RoundPrice(this decimal decimalNumber, int belowNumber = 1)
	{
		try
		{
			return (belowNumber <= 0) ? Math.Round(decimalNumber, 0) : Math.Round(decimalNumber - decimalNumber % (decimal)belowNumber, 0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RoundPrice", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 404);
		}
		return Math.Round(decimalNumber, 0);
	}

	public static string WintoDosString(this string unicodeStr)
	{
		try
		{
			return new ConvertWindowsPersianToDos().GetUnicodeFromIranSystem(unicodeStr);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "WintoDosString", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 419);
		}
		return "";
	}

	public static string DosToWinString(this string dosStr)
	{
		try
		{
			return new ConvertDosPersianToWindows().GetUnicodeStr(dosStr);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DosToWinString", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 432);
		}
		return "";
	}

	private static string DosToWinString2(this string dosStr)
	{
		try
		{
			string[] array = dosStr.Split(new char[1] { ',' });
			ConvertDosPersianToWindows convertDosPersianToWindows = new ConvertDosPersianToWindows();
			for (int i = 0; i < array.Length; i++)
			{
				byte[] bytes = Encoding.GetEncoding("windows-1256").GetBytes(array[i]);
				if (!convertDosPersianToWindows.IsLatinWord(bytes))
				{
					array[i] = convertDosPersianToWindows.GetUnicodeStr(array[i]);
				}
			}
			return string.Join(",", array);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DosToWinString2", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 452);
		}
		return "";
	}

	public static async Task CreateUnicodeFileDosFileAsync(string contextGuid, string dosFilePath, string unicodeFilePath)
	{
		try
		{
			if (!File.Exists(dosFilePath))
			{
				Show(new ReturnedSaveFuncInfo((ReturnedState)2, "[تولید فایل فارسی بیمه در مسیر زیر امکان پذیر نیست]\r\n" + dosFilePath, "", "CreateUnicodeFileDosFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 464), contextGuid);
				return;
			}
			string directoryName = new FileInfo(unicodeFilePath).DirectoryName;
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (!Directory.Exists(directoryName))
			{
				Show(new ReturnedSaveFuncInfo((ReturnedState)2, "[فایل مبدا جهت تولید فایل فارسی بیمه یافت نشد]\r\n" + directoryName, "", "CreateUnicodeFileDosFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 474), contextGuid);
				return;
			}
			List<string> values = (from line in File.ReadLines(dosFilePath, Encoding.Default)
				select line.DosToWinString2()).ToList();
			using StreamWriter file = new StreamWriter(unicodeFilePath);
			await file.WriteLineAsync(string.Join("\r\n", values));
			file.Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateUnicodeFileDosFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 484);
		}
	}

	public static DialogResult Show(this ReturnedSaveFuncInfo returnedSaveFuncInfo, string contextGuid)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = "خطا";
			string text2 = "هشدار";
			string text3 = "*اطلاعات*";
			text = Utilities.GetTranslate("[خطا]");
			text2 = Utilities.GetTranslate("[هشدار]");
			text3 = Utilities.GetTranslate("[اطلاعات]");
			if (returnedSaveFuncInfo.HasError || returnedSaveFuncInfo.HasWarning || returnedSaveFuncInfo.HasInformation)
			{
				int num = 10;
				while (num-- > 0 && !(text3 != "*اطلاعات*"))
				{
					Thread.Sleep(200);
				}
				string text4 = "";
				if (returnedSaveFuncInfo.HasError)
				{
					text4 = text;
				}
				else if (returnedSaveFuncInfo.HasWarning)
				{
					text4 = text2;
				}
				else if (returnedSaveFuncInfo.HasInformation)
				{
					text4 = text3;
				}
				FrmShowErrorMessage val = new FrmShowErrorMessage(contextGuid, returnedSaveFuncInfo, text4, (MessageBoxButtons)0, false, LanguageAndCalendar.Instance.SelectedLangauge, "Show", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs");
				((Form)val).ShowDialog();
				return ((Form)val).DialogResult;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Show", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 520);
		}
		return (DialogResult)2;
	}

	public static Encoding GetEncoding(string filename)
	{
		byte[] array = new byte[4];
		using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
		{
			fileStream.Read(array, 0, 4);
		}
		if (array[0] == 43 && array[1] == 47 && array[2] == 118)
		{
			return Encoding.UTF7;
		}
		if (array[0] == 239 && array[1] == 187 && array[2] == 191)
		{
			return Encoding.UTF8;
		}
		if (array[0] == byte.MaxValue && array[1] == 254 && array[2] == 0 && array[3] == 0)
		{
			return Encoding.UTF32;
		}
		if (array[0] == byte.MaxValue && array[1] == 254)
		{
			return Encoding.Unicode;
		}
		if (array[0] == 254 && array[1] == byte.MaxValue)
		{
			return Encoding.BigEndianUnicode;
		}
		if (array[0] == 0 && array[1] == 0 && array[2] == 254 && array[3] == byte.MaxValue)
		{
			return new UTF32Encoding(bigEndian: true, byteOrderMark: true);
		}
		return Encoding.ASCII;
	}

	public static void SetTooltip(this Control control, string body, string header = "", string footer = "", eTooltipColor color = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			SuperTooltip val = new SuperTooltip
			{
				DefaultFont = new Font("Tahoma", 7f),
				ShowTooltipImmediately = true
			};
			SuperTooltipInfo val2 = new SuperTooltipInfo
			{
				BodyText = Utilities.GetTranslate(body)
			};
			if (!string.IsNullOrEmpty(header))
			{
				val2.HeaderText = Utilities.GetTranslate(header);
			}
			if (!string.IsNullOrEmpty(footer))
			{
				val2.FooterText = Utilities.GetTranslate(footer);
			}
			val2.Color = color;
			val.SetSuperTooltip((IComponent)control, val2);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTooltip", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 555);
		}
	}

	public static void MessageBoxShowByNotif(bool showPopUp, NotificationEnum notificationEnum, string msg, string title = "", short secondTimeForDisplay = 3)
	{
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected I4, but got Unknown
		int num = 0;
		try
		{
			num = 1;
			Bitmap image = (Bitmap)((notificationEnum - 1) switch
			{
				2 => Resources.information.ToBitmap(), 
				3 => Resources.error.ToBitmap(), 
				4 => Resources.exclamtion.ToBitmap(), 
				1 => Resources.cancel.ToBitmap(), 
				5 => Resources.question.ToBitmap(), 
				0 => Resources.ok.ToBitmap(), 
				6 => Resources.EndSync.ToBitmap(), 
				7 => Resources.Syncing.ToBitmap(), 
				8 => Resources.StartSync.ToBitmap(), 
				9 => Resources.Buffer.ToBitmap(), 
				_ => Resources.StartSync.ToBitmap(), 
			});
			num = 2;
			if (string.IsNullOrEmpty(title))
			{
				title = " ";
			}
			MessageBoxShowByNotifNew(msg, title, secondTimeForDisplay, (Image)(object)image);
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message + $":line= {num} in MessageBoxShowByNotif \r\n showPopUp:{showPopUp}\r\nnotificationEnum:{notificationEnum}\r\nmsg:{msg}\r\ntitle:{title}\r\nsecondTimeForDisplay:{secondTimeForDisplay}");
		}
	}

	public static void MessageBoxShowError(string msg, string contextGuid)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = ((msg.Contains("[") && msg.Contains("]")) ? Utilities.GetTranslate(msg) : msg);
			Show(new ReturnedSaveFuncInfo((ReturnedState)2, text, "", "MessageBoxShowError", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 593), contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MessageBoxShowError", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 595);
		}
	}

	public static void MessageBoxShowWarning(string msg, string contextGuid)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = ((msg.Contains("[") && msg.Contains("]")) ? Utilities.GetTranslate(msg) : msg);
			Show(new ReturnedSaveFuncInfo((ReturnedState)4, text, "", "MessageBoxShowWarning", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 603), contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MessageBoxShowWarning", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 605);
		}
	}

	public static void MessageBoxShow(string msg, string contextGuid)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = ((msg.Contains("[") && msg.Contains("]")) ? Utilities.GetTranslate(msg) : msg);
			Show(new ReturnedSaveFuncInfo((ReturnedState)1, text, "", "MessageBoxShow", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 613), contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MessageBoxShow", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 615);
		}
	}

	public static Dictionary<string, int> GetEnumPersianName(Type enumType)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		try
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("Only enum type is expected.");
			}
			foreach (object value2 in Enum.GetValues(enumType))
			{
				short value = (short)value2;
				string text = value2.ToString();
				object obj = value2.GetType().GetField(text)?.GetCustomAttributes(typeof(PersianName), inherit: false).First();
				if (obj != null)
				{
					text = ((PersianName)(((obj is PersianName) ? obj : null)?)).Text;
				}
				if (text != null)
				{
					dictionary.Add(text, value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetEnumPersianName", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 638);
		}
		return dictionary;
	}

	public static Dictionary<string, int> GetEnumDescriptions(Type enumType)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		try
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("Only enum type is expected.");
			}
			foreach (object value2 in Enum.GetValues(enumType))
			{
				int value = (int)value2;
				string text = value2.ToString();
				object obj = value2.GetType().GetField(text)?.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).First();
				if (obj != null)
				{
					text = (obj as DescriptionAttribute)?.Description;
				}
				if (text != null)
				{
					dictionary.Add(text, value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetEnumDescriptions", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 662);
		}
		return dictionary;
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetLanguage", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 676);
		}
		return (LanguageTypes)(short)num;
	}

	public static void SetLanguage(LanguageTypes language)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected I4, but got Unknown
		try
		{
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin\\LanguageSetting", "Language", ((short)(int)language).ToString());
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetLanguage", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 688);
		}
	}

	public static void StartMessageBoxByNotif()
	{
		try
		{
			MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, "", "", 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "StartMessageBoxByNotif", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 695);
		}
	}

	private static void MessageBoxShowByNotifNew(string msg, string title, short secondTimeForDisplay = 5, Image image = null)
	{
		if (_showNotificationTask == null)
		{
			_showNotificationToken?.Cancel();
			_showNotificationToken = new CancellationTokenSource();
			CancellationToken token = _showNotificationToken.Token;
			_showNotificationTask = Task.Delay(100);
			Task.Run(() => ShowNotificationAsync(token));
		}
		Image val = image;
		if (val == null)
		{
			val = (Image)(object)Resources.information.ToBitmap();
		}
		string text = msg.Trim();
		if (msg.Contains("[") && msg.Contains("]"))
		{
			text = Utilities.GetTranslate(msg).Trim();
		}
		if (text != "" && _lastMessage == text)
		{
			return;
		}
		try
		{
			if (text != "")
			{
				QueueNotifs.Post(new MyNotif
				{
					Message = text,
					Title = Utilities.GetTranslate(title).Trim(),
					Image = val,
					DurationTime = secondTimeForDisplay
				});
			}
		}
		catch (Exception ex)
		{
			if (!ex.Message.Contains("Adding the specified count to the semaphore would cause it to exceed"))
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MessageBoxShowByNotifNew", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 730);
			}
		}
	}

	private static async Task ShowNotificationAsync(CancellationToken token)
	{
		IWin32Window owner = await OwnerForm.Task;
		MyNotif lastSendedNotif = null;
		while (!token.IsCancellationRequested)
		{
			try
			{
				if (QueueNotifs.Count <= 0)
				{
					lastSendedNotif = null;
				}
				MyNotif notif = await QueueNotifs.ReceiveAsync();
				while (QueueNotifs.Count > 3)
				{
					notif = await QueueNotifs.ReceiveAsync();
				}
				if (notif == null || string.IsNullOrEmpty(notif?.Message) || (lastSendedNotif != null && lastSendedNotif.Message == notif?.Message))
				{
					continue;
				}
				lastSendedNotif = notif;
				do
				{
					await Task.Delay(500);
					if (token.IsCancellationRequested || ((Control)(Form)owner).IsDisposed)
					{
						return;
					}
				}
				while (!((Control)(Form)owner).IsHandleCreated);
				((Control)(Form)owner).BeginInvoke((Delegate)(Action)delegate
				{
					//IL_0069: Unknown result type (might be due to invalid IL or missing references)
					//IL_0078: Expected O, but got Unknown
					//IL_0073: Unknown result type (might be due to invalid IL or missing references)
					//IL_007d: Expected O, but got Unknown
					//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b3: Expected O, but got Unknown
					//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b8: Expected O, but got Unknown
					//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
					try
					{
						ClsNotif clsNotif = new ClsNotif
						{
							Image = notif?.Image,
							TitleText = notif?.Title,
							ContentText = notif?.Message,
							IsRightToLeft = true,
							ImageSize = new Size(50, 50),
							TitleFont = new Font(new FontFamily("Tahoma"), 7f),
							AnimationDuration = 1000,
							BodyColor = Color.Black,
							BorderColor = Color.Yellow,
							ContentFont = new Font(new FontFamily("Tahoma"), 10f),
							ContentPadding = new Padding(5, 5, 5, 5),
							HeaderColor = Color.Black,
							HeaderHeight = 15,
							ImagePadding = new Padding(10, 0, 0, 5),
							ShowCloseButton = true,
							ShowGrip = true,
							Size = new Size(300, 75 + (notif.Message.Length / 22 + 1) * 12),
							TitleColor = Color.White,
							ContentColor = Color.Yellow,
							Delay = notif.DurationTime * 1000,
							AnimationInterval = 10
						};
						if (clsNotif != null && clsNotif.TitleText?.Length < 2)
						{
							clsNotif.TitleText = "حضور و غیاب " + ResourceManagement.TextBrandResourceManagement.CompanyName;
						}
						clsNotif?.Popup();
						AttLogger.LoggerUi.Information<EnGroup>("title=>[" + notif.Title + "] and  message=>[" + notif.Message + "]", (EnGroup)13);
					}
					catch (Exception ex3)
					{
						Log.Error(ex3, "");
					}
				});
				await Task.Delay(notif.DurationTime * 1000);
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception ex2)
			{
				Log.Error(ex2, "");
			}
		}
	}

	public static async Task ReloadDgridReportDevicesAsync(CancellationToken token)
	{
		await Task.Delay(3000);
		if (token.IsCancellationRequested || !UserAttendance.SelectedUser.UserSetting.viewGridReportDevices)
		{
			return;
		}
		IWin32Window owner = await OwnerForm.Task;
		DataGridView dGridReportDevices = await GridReportDevices.Task;
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			List<DeviceEventsEntity> recordsOfDevice = DeviceEventsEntity.GetEventMessages()?.OrderByDescending((DeviceEventsEntity p) => p.ModifiedDate).ToList();
			do
			{
				await Task.Delay(500);
				if (token.IsCancellationRequested || ((Control)(Form)owner).IsDisposed)
				{
					return;
				}
			}
			while (!((Control)(Form)owner).IsHandleCreated);
			((Control)(Form)owner).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					dGridReportDevices.DataSource = recordsOfDevice;
					((Control)dGridReportDevices).Refresh();
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "ReloadDgridReportDevicesAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 829);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReloadDgridReportDevicesAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 833);
		}
	}

	public static async Task ReloadDgridLastTaradodsAsync(string contextGuid, CancellationToken token)
	{
		await Task.Delay(5000);
		if (token.IsCancellationRequested || UserAttendance.SelectedUser == null)
		{
			return;
		}
		UserAttSetting userSetting = UserAttendance.SelectedUser.UserSetting;
		if (userSetting != null && !userSetting.viewGridLastTaradods)
		{
			return;
		}
		IWin32Window owner = await OwnerForm.Task;
		DataGridViewX dGridLastTardods = await GridLastTaradod.Task;
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			List<LogOnPersonelTopLastTaradods> lastTaradods = await LogOnPersonelTopLastTaradods.GetAsync(contextGuid, (UserAttendance.SelectedUser?.UserSetting?.CountLastTaradods).GetValueOrDefault(10), token);
			do
			{
				await Task.Delay(500);
				if (token.IsCancellationRequested || ((Control)(Form)owner).IsDisposed)
				{
					return;
				}
			}
			while (!((Control)(Form)owner).IsHandleCreated);
			((Control)(Form)owner).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					((DataGridView)dGridLastTardods).DataSource = lastTaradods;
					((Control)dGridLastTardods).Refresh();
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "ReloadDgridLastTaradodsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 866);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReloadDgridLastTaradodsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 873);
		}
	}

	private static Icon GetToolTipIconForTaradod(NotificationEnum notificationEnum)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected I4, but got Unknown
		try
		{
			switch (notificationEnum - 7)
			{
			case 2:
				return Resources.StartSync;
			case 1:
				return Resources.Syncing;
			case 0:
				return Resources.EndSync;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetToolTipIconForTaradod", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Utility\\Utility.cs", 891);
		}
		return SystemIcons.Information;
	}

	public static void SendTelegramMessageTaradods(string message, long serialNumber)
	{
		// [SANITIZED] تله‌متری حذف شد: ترددهای دستگاه‌های خاص به کانال تلگرامی سازنده ارسال می‌شد.
	}

	public static void ExecutePowerShellCommandsFromFile(string filePath)
	{
		try
		{
			string[] array = File.ReadAllLines(filePath);
			foreach (string text in array)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					using Process process = new Process();
					process.StartInfo.FileName = "powershell.exe";
					process.StartInfo.Arguments = "-NoProfile -Command \"" + text + "\"";
					process.StartInfo.Verb = "runas";
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.RedirectStandardError = true;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;
					process.Start();
					process.WaitForExit();
				}
			}
		}
		catch (Exception)
		{
		}
	}
}

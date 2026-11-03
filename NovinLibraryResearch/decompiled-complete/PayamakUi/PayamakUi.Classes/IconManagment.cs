using System;
using System.IO;
using System.Windows.Forms;
using PacketParser.Entities;

namespace PayamakUi.Classes;

public class IconManagment
{
	private static byte[] _InBox_Read = null;

	private static byte[] _Inbox_UnRead = null;

	private static byte[] _InBox_Replyed = null;

	private static byte[] _SentItems_Normal = null;

	private static byte[] _SentItems_Failed = null;

	private static byte[] _SentItems_Success = null;

	private static string StartUp = Application.StartupPath;

	private static string FullPath = Application.StartupPath + "\\Icon";

	public static byte[] InBox_Read
	{
		get
		{
			try
			{
				if (_InBox_Read != null)
				{
					return _InBox_Read;
				}
				checkPictures();
				return _InBox_Read;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "InBox_Read", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\IconManagment.cs", 36);
				return null;
			}
		}
	}

	public static byte[] Inbox_UnRead
	{
		get
		{
			try
			{
				if (_Inbox_UnRead != null)
				{
					return _Inbox_UnRead;
				}
				checkPictures();
				return _Inbox_UnRead;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Inbox_UnRead", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\IconManagment.cs", 55);
				return null;
			}
		}
	}

	public static byte[] InBox_Replyed
	{
		get
		{
			try
			{
				if (_InBox_Replyed != null)
				{
					return _InBox_Replyed;
				}
				checkPictures();
				return _InBox_Replyed;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "InBox_Replyed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\IconManagment.cs", 74);
				return null;
			}
		}
	}

	public static byte[] SentItems_Normal
	{
		get
		{
			try
			{
				if (_SentItems_Normal != null)
				{
					return _SentItems_Normal;
				}
				checkPictures();
				return _SentItems_Normal;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SentItems_Normal", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\IconManagment.cs", 93);
				return null;
			}
		}
	}

	public static byte[] SentItems_Failed
	{
		get
		{
			try
			{
				if (_SentItems_Failed != null)
				{
					return _SentItems_Failed;
				}
				checkPictures();
				return _SentItems_Failed;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SentItems_Failed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\IconManagment.cs", 112);
				return null;
			}
		}
	}

	public static byte[] SentItems_Success
	{
		get
		{
			try
			{
				if (_SentItems_Success != null)
				{
					return _SentItems_Success;
				}
				checkPictures();
				return _SentItems_Success;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SentItems_Success", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\IconManagment.cs", 131);
				return null;
			}
		}
	}

	private static void checkPictures()
	{
	}

	private static byte[] IMGToBinary(string fileName)
	{
		try
		{
			if (!File.Exists(fileName))
			{
				return null;
			}
			BinaryReader binaryReader = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read));
			return binaryReader.ReadBytes(Convert.ToInt32(binaryReader.BaseStream.Length));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IMGToBinary", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\IconManagment.cs", 205);
			return null;
		}
	}
}

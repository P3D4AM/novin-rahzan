using System;
using System.IO;
using System.Runtime.InteropServices;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

internal static class util
{
	public static int gMachineNumber;

	public static string gstrNoDevice = "No Device";

	public static string gTempPhotoFile = "C:\\TempPhoto.jpg";

	public static int gCompressPhotoSize = 8192;

	public static short FK_Company = 1;

	public static short FK_328 = 101;

	public static short FK_338 = 102;

	public static short FK_528 = 201;

	public static short FK_526 = 202;

	[DllImport("user32.dll", CharSet = CharSet.Ansi)]
	public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

	[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
	public static extern void Sleep(int dwMilliseconds);

	public static string ErrorPrint(int aErrorCode)
	{
		return aErrorCode switch
		{
			0 => "SUCCESS", 
			1 => "ERR_COMPORT_ERROR", 
			2 => "ERR_WRITE_FAIL", 
			3 => "ERR_READ_FAIL", 
			4 => "ERR_INVALID_PARAM", 
			5 => "ERR_NON_CARRYOUT", 
			6 => "ERR_LOG_END", 
			7 => "ERR_MEMORY", 
			8 => "ERR_MULTIUSER", 
			_ => "NO_ERROR", 
		};
	}

	public static int pubIPAddrToLong(string txtIP)
	{
		int num = txtIP.IndexOf('.');
		if (num == 0)
		{
			return 0;
		}
		string value = txtIP.Substring(0, num);
		txtIP = txtIP.Remove(0, num + 1);
		long num2 = Convert.ToInt32(value);
		num = txtIP.IndexOf('.');
		if (num == 0)
		{
			return 0;
		}
		value = txtIP.Substring(0, num);
		txtIP = txtIP.Remove(0, num + 1);
		num2 = num2 * 256 + Convert.ToInt32(value);
		num = txtIP.IndexOf('.');
		if (num == 0)
		{
			return 0;
		}
		value = txtIP.Substring(0, num);
		txtIP = txtIP.Remove(0, num + 1);
		num2 = num2 * 256 + Convert.ToInt32(value);
		num2 = num2 * 256 + Convert.ToInt32(txtIP);
		if (num2 > int.MaxValue)
		{
			num2 -= int.MaxValue;
		}
		return (int)num2;
	}

	public static string pubLongToIPAddr(int vValue)
	{
		int num = vValue % 256;
		if (num < 0)
		{
			num = 256 + num;
		}
		string text = Convert.ToString(num);
		vValue = (vValue - num) / 256;
		num = vValue % 256;
		if (num < 0)
		{
			num = 256 + num;
		}
		text = Convert.ToString(num) + "." + text;
		vValue = (vValue - num) / 256;
		num = vValue % 256;
		if (num < 0)
		{
			num = 256 + num;
		}
		text = Convert.ToString(num) + "." + text;
		vValue = (vValue - num) / 256;
		num = vValue % 256;
		if (num < 0)
		{
			num = 256 + num;
		}
		return Convert.ToString(num) + "." + text;
	}

	public static bool MakeXMLRequestHeader(AxSBXPC bpc, ref string strXML, string strRequest)
	{
		bool flag = bpc.XML_AddString(ref strXML, "REQUEST", strRequest);
		flag = bpc.XML_AddString(ref strXML, "MSGTYPE", "request") && flag;
		return bpc.XML_AddInt(ref strXML, "MachineID", gMachineNumber) && flag;
	}

	public static bool SaveDeviceLongInfo(string fileName, int[] data)
	{
		FileStream fileStream = null;
		try
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
			fileStream = File.Create(fileName);
			byte[] array = new byte[4 * data.Length];
			IntPtr source = GCHandle.Alloc(data, GCHandleType.Pinned).AddrOfPinnedObject();
			Marshal.Copy(source, array, 0, 4 * data.Length);
			fileStream.Write(array, 0, array.Length);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
		finally
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream.Dispose();
			}
		}
	}

	public static bool LoadDeviceLongInfo(string fileName, int[] data)
	{
		FileStream fileStream = null;
		try
		{
			if (!File.Exists(fileName))
			{
				return false;
			}
			fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
			if (fileStream.Length != data.Length * 4)
			{
				return false;
			}
			byte[] array = new byte[data.Length * 4];
			fileStream.Read(array, 0, array.Length);
			IntPtr source = GCHandle.Alloc(array, GCHandleType.Pinned).AddrOfPinnedObject();
			Marshal.Copy(source, data, 0, data.Length);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
		finally
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream.Dispose();
			}
		}
	}
}

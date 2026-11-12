using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;
using Stimulsoft.Base.Crypto;

namespace Stimulsoft.Base;

public class StiMD5Helper
{
	public static byte[] ComputeHash(Stream stream)
	{
		Stimulsoft.Base.Crypto.MD5 mD = new Stimulsoft.Base.Crypto.MD5();
		byte[] array = new byte[4096];
		int num = array.Length;
		while (num == array.Length)
		{
			num = stream.Read(array, 0, array.Length);
			if (num > 0)
			{
				mD.BlockUpdate(array, 0, num);
			}
		}
		return mD.GetHash();
	}

	public static byte[] ComputeHash(byte[] buf)
	{
		return ComputeHash(buf, 0, buf.Length);
	}

	public static byte[] ComputeHash(byte[] buf, int offset, int count)
	{
		if (StiBaseOptions.FIPSCompliance)
		{
			Stimulsoft.Base.Crypto.MD5 mD = new Stimulsoft.Base.Crypto.MD5();
			mD.BlockUpdate(buf, offset, count);
			return mD.GetHash();
		}
		using MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		return mD5CryptoServiceProvider.ComputeHash(buf, offset, count);
	}

	public static string ComputeHash(string input)
	{
		byte[] array = ComputeHash(Encoding.UTF8.GetBytes(input));
		string text = string.Empty;
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			text += b.ToString("x2");
		}
		return text;
	}

	static StiMD5Helper()
	{
		try
		{
			RegistryKey val = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa\\FIPSAlgorithmPolicy");
			if (val != null)
			{
				object value = val.GetValue("Enabled");
				if (value != null && value is int && (int)value == 1)
				{
					StiBaseOptions.FIPSCompliance = true;
				}
			}
		}
		catch
		{
		}
	}
}

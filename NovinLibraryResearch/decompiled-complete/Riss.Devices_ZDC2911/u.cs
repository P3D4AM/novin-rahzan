using System;
using System.Runtime.InteropServices;

internal class u
{
	internal static ulong b(string A_0)
	{
		if (!string.IsNullOrEmpty(A_0))
		{
			return (ulong)Convert.ToInt64(A_0);
		}
		return 0uL;
	}

	internal static byte[] a(object A_0, int A_1)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(A_1);
		try
		{
			Marshal.StructureToPtr(A_0, intPtr, fDeleteOld: false);
			byte[] array = new byte[A_1];
			Marshal.Copy(intPtr, array, 0, A_1);
			return array;
		}
		catch (Exception ex)
		{
			throw new Exception("Error in StructToBytes ! " + Environment.NewLine + ex.Message);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	internal static T BytesToStruct<T>(byte[] bytes, int startIndex, int length)
	{
		if (bytes == null)
		{
			return default(T);
		}
		if (bytes.Length <= 0)
		{
			return default(T);
		}
		IntPtr intPtr = Marshal.AllocHGlobal(length);
		try
		{
			Marshal.Copy(bytes, startIndex, intPtr, length);
			return (T)Marshal.PtrToStructure(intPtr, typeof(T));
		}
		catch (Exception ex)
		{
			throw new Exception("Error in BytesToStruct ! " + Environment.NewLine + ex.Message);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	internal static int a(string A_0)
	{
		int length = A_0.Length;
		int num = 0;
		if (length > 8)
		{
			return 0;
		}
		for (int num2 = 0; num2 < length; num2++)
		{
			num <<= 4;
			byte b2;
			if (A_0[num2] >= '1' && A_0[num2] <= '9')
			{
				b2 = (byte)(A_0[num2] - 48);
			}
			else
			{
				if (A_0[num2] != '0')
				{
					return 0;
				}
				b2 = 10;
			}
			num += b2;
		}
		return num;
	}

	internal static string a(int A_0)
	{
		byte[] array = new byte[8];
		int num = 7;
		while ((A_0 & (15 << num * 4)) == 0)
		{
			num--;
		}
		while (num >= 0)
		{
			byte b2 = (byte)((uint)A_0 & 0xFu);
			if (b2 >= 1 && b2 <= 9)
			{
				array[num] = (byte)(48 + b2);
			}
			else if (b2 == 10)
			{
				array[num] = 48;
			}
			A_0 >>= 4;
			num--;
		}
		return BitConverter.ToString(array, 0);
	}
}

using System;

namespace Stimulsoft.Report.Helpers;

public static class StiArrayCloner
{
	public static byte[] Clone(byte[] array)
	{
		byte[] array2 = new byte[array.Length];
		Buffer.BlockCopy(array, 0, array2, 0, array.Length);
		return array2;
	}
}

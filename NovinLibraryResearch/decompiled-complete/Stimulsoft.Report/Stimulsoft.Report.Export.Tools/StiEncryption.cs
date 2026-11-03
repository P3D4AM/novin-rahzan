namespace Stimulsoft.Report.Export.Tools;

public class StiEncryption
{
	public static void RC4(ref byte[] bytes, byte[] key)
	{
		byte[] array = new byte[256];
		byte[] array2 = new byte[256];
		int length = key.GetLength(0);
		int i;
		for (i = 0; i < 256; i++)
		{
			array[i] = (byte)i;
			array2[i] = key[i % length];
		}
		int num = 0;
		for (i = 0; i < 256; i++)
		{
			num = (num + array[i] + array2[i]) % 256;
			byte b = array[i];
			array[i] = array[num];
			array[num] = b;
		}
		i = (num = 0);
		int length2 = bytes.GetLength(0);
		for (int j = 0; j < length2; j++)
		{
			i = (i + 1) % 256;
			num = (num + array[i]) % 256;
			byte b = array[i];
			array[i] = array[num];
			array[num] = b;
			int num2 = (array[i] + array[num]) % 256;
			bytes[j] ^= array[num2];
		}
	}
}

using System.Text;

namespace SharpCompress.Common.Rar.Headers;

internal static class FileNameDecoder
{
	internal static int GetChar(byte[] name, int pos)
	{
		return name[pos] & 0xFF;
	}

	internal static string Decode(byte[] name, int encPos)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int @char = GetChar(name, encPos++);
		StringBuilder stringBuilder = new StringBuilder();
		while (encPos < name.Length)
		{
			if (num3 == 0)
			{
				num2 = GetChar(name, encPos++);
				num3 = 8;
			}
			switch (num2 >> 6)
			{
			case 0:
				stringBuilder.Append((char)GetChar(name, encPos++));
				num++;
				break;
			case 1:
				stringBuilder.Append((char)(GetChar(name, encPos++) + (@char << 8)));
				num++;
				break;
			case 2:
				num4 = GetChar(name, encPos);
				num5 = GetChar(name, encPos + 1);
				stringBuilder.Append((char)((num5 << 8) + num4));
				num++;
				encPos += 2;
				break;
			case 3:
			{
				int char2 = GetChar(name, encPos++);
				if (((uint)char2 & 0x80u) != 0)
				{
					int char3 = GetChar(name, encPos++);
					char2 = (char2 & 0x7F) + 2;
					while (char2 > 0 && num < name.Length)
					{
						num4 = (GetChar(name, num) + char3) & 0xFF;
						stringBuilder.Append((char)((@char << 8) + num4));
						char2--;
						num++;
					}
				}
				else
				{
					char2 += 2;
					while (char2 > 0 && num < name.Length)
					{
						stringBuilder.Append((char)GetChar(name, num));
						char2--;
						num++;
					}
				}
				break;
			}
			}
			num2 = (num2 << 2) & 0xFF;
			num3 -= 2;
		}
		return stringBuilder.ToString();
	}
}

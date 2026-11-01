using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeAttendanceDAL.Utility;

public class ConvertDosPersianToWindows
{
	private Dictionary<byte, byte> MapFromDosToUnicode = new Dictionary<byte, byte>
	{
		{ 41, 40 },
		{ 40, 41 },
		{ 123, 125 },
		{ 125, 123 },
		{ 91, 93 },
		{ 93, 91 },
		{ 128, 48 },
		{ 129, 49 },
		{ 130, 50 },
		{ 131, 51 },
		{ 132, 52 },
		{ 133, 53 },
		{ 134, 54 },
		{ 135, 55 },
		{ 136, 56 },
		{ 137, 57 },
		{ 138, 161 },
		{ 139, 220 },
		{ 140, 191 },
		{ 141, 194 },
		{ 142, 198 },
		{ 143, 193 },
		{ 144, 199 },
		{ 145, 199 },
		{ 146, 200 },
		{ 147, 200 },
		{ 148, 129 },
		{ 149, 129 },
		{ 150, 202 },
		{ 151, 202 },
		{ 152, 203 },
		{ 153, 203 },
		{ 154, 204 },
		{ 155, 204 },
		{ 156, 141 },
		{ 157, 141 },
		{ 158, 205 },
		{ 159, 205 },
		{ 160, 206 },
		{ 161, 206 },
		{ 162, 207 },
		{ 163, 208 },
		{ 164, 209 },
		{ 165, 210 },
		{ 166, 142 },
		{ 167, 211 },
		{ 168, 211 },
		{ 169, 212 },
		{ 170, 212 },
		{ 171, 213 },
		{ 172, 213 },
		{ 173, 214 },
		{ 174, 214 },
		{ 175, 216 },
		{ 224, 217 },
		{ 225, 218 },
		{ 226, 218 },
		{ 227, 218 },
		{ 228, 218 },
		{ 229, 219 },
		{ 230, 219 },
		{ 231, 219 },
		{ 232, 219 },
		{ 233, 221 },
		{ 234, 221 },
		{ 235, 222 },
		{ 236, 222 },
		{ 237, 152 },
		{ 238, 152 },
		{ 239, 144 },
		{ 240, 144 },
		{ 241, 225 },
		{ 242, 225 },
		{ 243, 225 },
		{ 244, 227 },
		{ 245, 227 },
		{ 246, 228 },
		{ 247, 228 },
		{ 248, 230 },
		{ 249, 229 },
		{ 250, 229 },
		{ 251, 229 },
		{ 252, 236 },
		{ 253, 236 },
		{ 254, 237 }
	};

	public string GetUnicodeStr(string dosStr)
	{
		dosStr = new string(dosStr.ToCharArray().Reverse().ToArray());
		string s = " " + dosStr + " ";
		byte[] bytes = Encoding.GetEncoding("windows-1256").GetBytes(s);
		List<byte> list = new List<byte>();
		for (int i = 0; i < bytes.Length; i++)
		{
			if (bytes[i] == 242)
			{
				list.Add(225);
				list.Add(199);
			}
			else
			{
				list.Add(GetConvertedChar(bytes[i]));
			}
		}
		return new string(Encoding.GetEncoding("windows-1256").GetChars(list.ToArray())).Trim();
	}

	public bool IsLatinWord(byte[] byteArr)
	{
		foreach (byte b in byteArr)
		{
			if (b <= 254 && b >= 40 && MapFromDosToUnicode.ContainsKey(b))
			{
				return false;
			}
		}
		return true;
	}

	private byte GetConvertedChar(byte charByte)
	{
		if (MapFromDosToUnicode.ContainsKey(charByte))
		{
			return MapFromDosToUnicode[charByte];
		}
		return charByte;
	}
}

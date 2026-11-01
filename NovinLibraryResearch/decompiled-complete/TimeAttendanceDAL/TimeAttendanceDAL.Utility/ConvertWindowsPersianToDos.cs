using System.Collections.Generic;
using System.Text;

namespace TimeAttendanceDAL.Utility;

public class ConvertWindowsPersianToDos
{
	private Dictionary<byte, byte> MapSeparateChar = new Dictionary<byte, byte>
	{
		{ 48, 128 },
		{ 49, 129 },
		{ 50, 130 },
		{ 51, 131 },
		{ 52, 132 },
		{ 53, 133 },
		{ 54, 134 },
		{ 55, 135 },
		{ 56, 136 },
		{ 57, 137 },
		{ 161, 138 },
		{ 191, 140 },
		{ 193, 143 },
		{ 194, 141 },
		{ 195, 144 },
		{ 196, 248 },
		{ 197, 144 },
		{ 199, 144 },
		{ 200, 146 },
		{ 201, 249 },
		{ 202, 150 },
		{ 203, 152 },
		{ 204, 154 },
		{ 205, 158 },
		{ 206, 160 },
		{ 207, 162 },
		{ 208, 163 },
		{ 209, 164 },
		{ 210, 165 },
		{ 211, 167 },
		{ 212, 169 },
		{ 213, 171 },
		{ 214, 173 },
		{ 216, 175 },
		{ 217, 224 },
		{ 218, 225 },
		{ 219, 229 },
		{ 220, 139 },
		{ 221, 233 },
		{ 222, 235 },
		{ 223, 237 },
		{ 225, 241 },
		{ 227, 244 },
		{ 228, 246 },
		{ 229, 249 },
		{ 230, 248 },
		{ 236, 253 },
		{ 237, 253 },
		{ 129, 148 },
		{ 141, 156 },
		{ 142, 166 },
		{ 152, 237 },
		{ 144, 239 }
	};

	private Dictionary<byte, byte> MapFirstChar = new Dictionary<byte, byte>
	{
		{ 48, 128 },
		{ 49, 129 },
		{ 50, 130 },
		{ 51, 131 },
		{ 52, 132 },
		{ 53, 133 },
		{ 54, 134 },
		{ 55, 135 },
		{ 56, 136 },
		{ 57, 137 },
		{ 161, 138 },
		{ 191, 140 },
		{ 193, 143 },
		{ 194, 141 },
		{ 195, 144 },
		{ 196, 248 },
		{ 197, 144 },
		{ 198, 254 },
		{ 199, 144 },
		{ 200, 147 },
		{ 201, 251 },
		{ 202, 151 },
		{ 203, 153 },
		{ 204, 155 },
		{ 205, 159 },
		{ 206, 161 },
		{ 207, 162 },
		{ 208, 163 },
		{ 209, 164 },
		{ 210, 165 },
		{ 211, 168 },
		{ 212, 170 },
		{ 213, 172 },
		{ 214, 174 },
		{ 216, 175 },
		{ 217, 224 },
		{ 218, 228 },
		{ 219, 232 },
		{ 220, 139 },
		{ 221, 234 },
		{ 222, 236 },
		{ 223, 238 },
		{ 225, 243 },
		{ 227, 245 },
		{ 228, 247 },
		{ 229, 251 },
		{ 230, 248 },
		{ 236, 254 },
		{ 237, 254 },
		{ 129, 149 },
		{ 141, 157 },
		{ 142, 166 },
		{ 152, 238 },
		{ 144, 240 }
	};

	private Dictionary<byte, byte> MapEndChar = new Dictionary<byte, byte>
	{
		{ 48, 128 },
		{ 49, 129 },
		{ 50, 130 },
		{ 51, 131 },
		{ 52, 132 },
		{ 53, 133 },
		{ 54, 134 },
		{ 55, 135 },
		{ 56, 136 },
		{ 57, 137 },
		{ 161, 138 },
		{ 191, 140 },
		{ 193, 143 },
		{ 194, 141 },
		{ 195, 144 },
		{ 196, 248 },
		{ 197, 144 },
		{ 198, 253 },
		{ 199, 145 },
		{ 200, 146 },
		{ 201, 249 },
		{ 202, 150 },
		{ 203, 152 },
		{ 204, 154 },
		{ 205, 158 },
		{ 206, 160 },
		{ 207, 162 },
		{ 208, 163 },
		{ 209, 164 },
		{ 210, 165 },
		{ 211, 167 },
		{ 212, 169 },
		{ 213, 171 },
		{ 214, 173 },
		{ 216, 175 },
		{ 217, 224 },
		{ 218, 226 },
		{ 219, 230 },
		{ 220, 139 },
		{ 221, 233 },
		{ 222, 235 },
		{ 223, 237 },
		{ 225, 241 },
		{ 227, 244 },
		{ 228, 246 },
		{ 229, 249 },
		{ 230, 248 },
		{ 236, 252 },
		{ 237, 252 },
		{ 129, 148 },
		{ 141, 156 },
		{ 142, 166 },
		{ 152, 237 },
		{ 144, 239 }
	};

	private Dictionary<byte, byte> MapMiddleChar = new Dictionary<byte, byte>
	{
		{ 48, 128 },
		{ 49, 129 },
		{ 50, 130 },
		{ 51, 131 },
		{ 52, 132 },
		{ 53, 133 },
		{ 54, 134 },
		{ 55, 135 },
		{ 56, 136 },
		{ 57, 137 },
		{ 161, 138 },
		{ 191, 140 },
		{ 193, 143 },
		{ 194, 141 },
		{ 195, 145 },
		{ 196, 248 },
		{ 197, 145 },
		{ 198, 254 },
		{ 199, 145 },
		{ 200, 147 },
		{ 201, 250 },
		{ 202, 151 },
		{ 203, 153 },
		{ 204, 155 },
		{ 205, 159 },
		{ 206, 161 },
		{ 207, 162 },
		{ 208, 163 },
		{ 209, 164 },
		{ 210, 165 },
		{ 211, 168 },
		{ 212, 170 },
		{ 213, 172 },
		{ 214, 174 },
		{ 216, 175 },
		{ 217, 224 },
		{ 218, 227 },
		{ 219, 231 },
		{ 220, 139 },
		{ 221, 234 },
		{ 222, 236 },
		{ 223, 238 },
		{ 225, 243 },
		{ 227, 245 },
		{ 228, 247 },
		{ 229, 250 },
		{ 230, 248 },
		{ 236, 254 },
		{ 237, 254 },
		{ 129, 149 },
		{ 141, 157 },
		{ 142, 166 },
		{ 152, 238 },
		{ 144, 240 }
	};

	private bool IsLattinLetter(byte c)
	{
		if (c < 128)
		{
			return c > 31;
		}
		return false;
	}

	private byte GetLattinLetter(byte c)
	{
		if ("0123456789".IndexOf((char)c) >= 0)
		{
			return (byte)(c + 80);
		}
		return GetFarsiExceptions(c);
	}

	private byte GetFarsiExceptions(byte c)
	{
		return c switch
		{
			40 => 41, 
			123 => 125, 
			91 => 93, 
			41 => 40, 
			125 => 123, 
			93 => 91, 
			_ => c, 
		};
	}

	private bool IsFinalLetter(byte c)
	{
		switch (c)
		{
		case 142:
		case 193:
		case 194:
		case 195:
		case 196:
		case 197:
		case 199:
		case 207:
		case 208:
		case 209:
		case 210:
		case 230:
			return true;
		default:
			return false;
		}
	}

	private bool IsWhiteLetter(byte c)
	{
		if (c != 8 && c != 9 && c != 10 && c != 13 && c != 27 && c != 32)
		{
			return c == 0;
		}
		return true;
	}

	private bool Char_Cond(byte c)
	{
		if (!IsWhiteLetter(c) && !IsLattinLetter(c))
		{
			return c == 191;
		}
		return true;
	}

	public string GetUnicodeFromIranSystem(string unicodeText)
	{
		string s = " " + unicodeText + " ";
		Encoding encoding = Encoding.GetEncoding("windows-1256");
		Encoding unicode = Encoding.Unicode;
		byte[] bytes = unicode.GetBytes(s);
		byte[] array = Encoding.Convert(unicode, encoding, bytes);
		char[] chars = new char[encoding.GetCharCount(array, 0, array.Length)];
		encoding.GetChars(array, 0, array.Length, chars, 0);
		byte[] bytes2 = Encoding.GetEncoding("windows-1256").GetBytes(chars);
		byte b = 0;
		List<byte> list = new List<byte>();
		for (int i = 0; i < bytes2.Length; i++)
		{
			if (IsLattinLetter(bytes2[i]))
			{
				byte lattinLetter = GetLattinLetter(bytes2[i]);
				list.Add(lattinLetter);
				b = lattinLetter;
			}
			else
			{
				if (i == 0 || i == bytes2.Length - 1)
				{
					continue;
				}
				byte lattinLetter = GetUnicodeToIranSystemChar(bytes2[i - 1], bytes2[i], bytes2[i + 1]);
				if (lattinLetter == 145)
				{
					if (b == 243)
					{
						list.RemoveAt(list.Count - 1);
						list.Add(242);
					}
					else
					{
						list.Add(lattinLetter);
					}
				}
				else
				{
					list.Add(lattinLetter);
				}
				b = lattinLetter;
			}
		}
		list.Reverse();
		return new string(Encoding.GetEncoding("windows-1256").GetChars(list.ToArray())).Trim();
	}

	private byte GetUnicodeToIranSystemChar(byte previousChar, byte currentChar, byte nextChar)
	{
		bool flag = Char_Cond(previousChar) || IsFinalLetter(previousChar);
		bool flag2 = Char_Cond(nextChar);
		if (flag && flag2)
		{
			return MapUcToIsSeparateChar(currentChar);
		}
		if (flag)
		{
			return MapUcToIsFirstChar(currentChar);
		}
		if (flag2)
		{
			return MapUcToIsEndChar(currentChar);
		}
		return MapUcToIsMiddleChar(currentChar);
	}

	private byte MapUcToIsMiddleChar(byte charByte)
	{
		if (MapMiddleChar.ContainsKey(charByte))
		{
			return MapMiddleChar[charByte];
		}
		return charByte;
	}

	private byte MapUcToIsEndChar(byte charByte)
	{
		if (MapEndChar.ContainsKey(charByte))
		{
			return MapEndChar[charByte];
		}
		return charByte;
	}

	private byte MapUcToIsFirstChar(byte charByte)
	{
		if (MapFirstChar.ContainsKey(charByte))
		{
			return MapFirstChar[charByte];
		}
		return charByte;
	}

	private byte MapUcToIsSeparateChar(byte charByte)
	{
		if (MapSeparateChar.ContainsKey(charByte))
		{
			return MapSeparateChar[charByte];
		}
		return charByte;
	}
}

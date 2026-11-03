using System.Collections.Generic;
using System.Text;

namespace PacketParser.Attendance.Access.StringProcess;

public class ConvertWindowsPersianToDos2
{
	private readonly Dictionary<byte, byte> _charachtersMapperGroup1 = new Dictionary<byte, byte>
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

	private readonly Dictionary<byte, byte> _charachtersMapperGroup2 = new Dictionary<byte, byte>
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

	private readonly Dictionary<byte, byte> _charachtersMapperGroup3 = new Dictionary<byte, byte>
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
		{ 198, 252 },
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

	private readonly Dictionary<byte, byte> _charachtersMapperGroup4 = new Dictionary<byte, byte>
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

	private readonly Dictionary<byte, string> _charachtersMapperGroup5 = new Dictionary<byte, string>
	{
		{ 193, "ء" },
		{ 194, "آ" },
		{ 195, "أ" },
		{ 196, "ؤ" },
		{ 197, "إ" },
		{ 199, "ا" },
		{ 207, "د" },
		{ 208, "ذ" },
		{ 209, "ر" },
		{ 210, "ز" },
		{ 142, "ژ" }
	};

	private bool is_Lattin_Letter(byte c)
	{
		if (c < 128)
		{
			return c > 31;
		}
		return false;
	}

	private byte get_Lattin_Letter(byte c)
	{
		if ("0123456789".IndexOf((char)c) < 0)
		{
			return get_FarsiExceptions(c);
		}
		return checked((byte)(c + 80));
	}

	private byte get_FarsiExceptions(byte c)
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

	private bool is_Final_Letter(byte c)
	{
		return _charachtersMapperGroup5.ContainsKey(c);
	}

	private bool IS_White_Letter(byte c)
	{
		if (c != 8 && c != 9 && c != 10 && c != 13 && c != 27 && c != 32)
		{
			return c == 0;
		}
		return true;
	}

	private bool Char_Cond(byte c)
	{
		if (!IS_White_Letter(c) && !is_Lattin_Letter(c))
		{
			return c == 191;
		}
		return true;
	}

	public List<string> get_Unicode_To_IranSystem(string unicodeText)
	{
		string s = " " + unicodeText + " ";
		Encoding encoding = Encoding.GetEncoding("windows-1256");
		Encoding unicode = Encoding.Unicode;
		byte[] bytes = unicode.GetBytes(s);
		byte[] array = Encoding.Convert(unicode, encoding, bytes);
		char[] array2 = new char[encoding.GetCharCount(array, 0, array.Length)];
		encoding.GetChars(array, 0, array.Length, array2, 0);
		new string(array2);
		byte[] bytes2 = Encoding.GetEncoding("windows-1256").GetBytes(array2);
		int num = bytes2.Length;
		byte b = 0;
		byte b2 = 0;
		List<string> list = new List<string>();
		checked
		{
			for (int i = 0; i < num; i++)
			{
				if (is_Lattin_Letter(bytes2[i]))
				{
					b2 = get_Lattin_Letter(bytes2[i]);
					list.Add($"{b2:X}");
					b = b2;
				}
				else
				{
					if (i == 0 || i == bytes2.Length - 1)
					{
						continue;
					}
					b2 = get_Unicode_To_IranSystem_Char(bytes2[i - 1], bytes2[i], bytes2[i + 1]);
					if (b2 == 145)
					{
						if (b == 243)
						{
							list.RemoveAt(list.Count - 1);
							list.Add($"{242:X}");
						}
						else
						{
							list.Add($"{b2:X}");
						}
					}
					else
					{
						list.Add($"{b2:X}");
					}
					b = b2;
				}
			}
			return list;
		}
	}

	private byte get_Unicode_To_IranSystem_Char(byte previousChar, byte currentChar, byte nextChar)
	{
		bool flag = Char_Cond(previousChar) || is_Final_Letter(previousChar);
		bool flag2 = Char_Cond(nextChar);
		if (flag && flag2)
		{
			return UCTOIS_Group_1(currentChar);
		}
		if (flag)
		{
			return UCTOIS_Group_2(currentChar);
		}
		if (!flag2)
		{
			return UCTOIS_Group_4(currentChar);
		}
		return UCTOIS_Group_3(currentChar);
	}

	private byte UCTOIS_Group_4(byte currentChar)
	{
		if (!_charachtersMapperGroup4.ContainsKey(currentChar))
		{
			return currentChar;
		}
		return _charachtersMapperGroup4[currentChar];
	}

	private byte UCTOIS_Group_3(byte currentChar)
	{
		if (!_charachtersMapperGroup3.ContainsKey(currentChar))
		{
			return currentChar;
		}
		return _charachtersMapperGroup3[currentChar];
	}

	private byte UCTOIS_Group_2(byte currentChar)
	{
		if (!_charachtersMapperGroup2.ContainsKey(currentChar))
		{
			return currentChar;
		}
		return _charachtersMapperGroup2[currentChar];
	}

	private byte UCTOIS_Group_1(byte currentChar)
	{
		if (!_charachtersMapperGroup1.ContainsKey(currentChar))
		{
			return currentChar;
		}
		return _charachtersMapperGroup1[currentChar];
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacketParser.Entities;

namespace PacketParser;

public class IranSystem
{
	public Dictionary<byte, byte> CharachtersMapper_Group1 = new Dictionary<byte, byte>
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

	public Dictionary<byte, byte> CharachtersMapper_Group2 = new Dictionary<byte, byte>
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

	public Dictionary<byte, byte> CharachtersMapper_Group3 = new Dictionary<byte, byte>
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

	public Dictionary<byte, byte> CharachtersMapper_Group4 = new Dictionary<byte, byte>
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

	private static string NotPersian = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890+-=/-.,";

	public bool is_Lattin_Letter(byte c)
	{
		if (c < 128 && c > 31)
		{
			return true;
		}
		return false;
	}

	public byte get_Lattin_Letter(byte c)
	{
		if ("0123456789".IndexOf((char)c) >= 0)
		{
			return checked((byte)(c + 80));
		}
		return get_FarsiExceptions(c);
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

	public bool is_Final_Letter(byte c)
	{
		if ("ءآأؤإادذرزژو".ToString().IndexOf((char)c) >= 0)
		{
			return true;
		}
		return false;
	}

	public bool IS_White_Letter(byte c)
	{
		if (c == 8 || c == 9 || c == 10 || c == 13 || c == 27 || c == 32 || c == 0)
		{
			return true;
		}
		return false;
	}

	public bool Char_Cond(byte c)
	{
		if (!IS_White_Letter(c) && !is_Lattin_Letter(c))
		{
			return c == 191;
		}
		return true;
	}

	public static string InverthLatin(string Input)
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder(100000);
			Stack<string> stack = new Stack<string>();
			foreach (char item in Input?.ToList())
			{
				if (NotPersian.Contains(item.ToString().ToUpper()))
				{
					stack.Push(item.ToString());
					continue;
				}
				while (stack.Count > 0)
				{
					stringBuilder.Append(stack.Pop());
				}
				stringBuilder.Append(item);
			}
			return stringBuilder.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InverthLatin", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\IranSystem.cs", 340);
			return Input;
		}
	}

	public List<byte> get_Unicode_To_IranSystem(string Unicode_Text)
	{
		Unicode_Text = Unicode_Text.Replace("(", "azsxdc__+");
		Unicode_Text = Unicode_Text.Replace(")", "(");
		Unicode_Text = Unicode_Text.Replace("azsxdc__+", ")");
		Unicode_Text = Unicode_Text.Replace("ي", "ی");
		Unicode_Text = Unicode_Text.Replace("ئ", "ی");
		List<char> list = new List<char> { '\n' };
		List<string> list2 = Unicode_Text.Split(list.ToArray()).ToList();
		Unicode_Text = "";
		if (list2 != null)
		{
			foreach (string item in list2)
			{
				Unicode_Text = item + "\n" + Unicode_Text;
			}
		}
		_ = " " + Unicode_Text + " ";
		List<byte> list3 = new List<byte>();
		char ch = ' ';
		checked
		{
			for (int i = 0; i < Unicode_Text.Length; i++)
			{
				char c = Unicode_Text[i];
				char c2 = ' ';
				if (i + 1 < Unicode_Text.Length)
				{
					c2 = Unicode_Text[i + 1];
				}
				switch (c)
				{
				case '0':
					list3.Add(128);
					break;
				case '1':
					list3.Add(129);
					break;
				case '2':
					list3.Add(130);
					break;
				case '3':
					list3.Add(131);
					break;
				case '4':
					list3.Add(132);
					break;
				case '5':
					list3.Add(133);
					break;
				case '6':
					list3.Add(134);
					break;
				case '7':
					list3.Add(135);
					break;
				case '8':
					list3.Add(136);
					break;
				case '9':
					list3.Add(137);
					break;
				case '،':
					list3.Add(138);
					break;
				case '؟':
					list3.Add(140);
					break;
				case 'آ':
					list3.Add(141);
					break;
				case 'ک':
					if (Chasb2Before(c2))
					{
						list3.Add(238);
					}
					else
					{
						list3.Add(237);
					}
					break;
				case 'ء':
					list3.Add(143);
					break;
				case 'ئ':
					list3.Add(142);
					break;
				case 'ا':
					if (Chasb2After(ch))
					{
						list3.Add(145);
					}
					else
					{
						list3.Add(144);
					}
					break;
				case 'ب':
					if (Chasb2Before(c2))
					{
						list3.Add(147);
					}
					else
					{
						list3.Add(146);
					}
					break;
				case 'پ':
					if (Chasb2Before(c2))
					{
						list3.Add(149);
					}
					else
					{
						list3.Add(148);
					}
					break;
				case 'ت':
					if (Chasb2Before(c2))
					{
						list3.Add(151);
					}
					else
					{
						list3.Add(150);
					}
					break;
				case 'ث':
					if (Chasb2Before(c2))
					{
						list3.Add(153);
					}
					else
					{
						list3.Add(152);
					}
					break;
				case 'ج':
					if (Chasb2Before(c2))
					{
						list3.Add(155);
					}
					else
					{
						list3.Add(154);
					}
					break;
				case 'چ':
					if (Chasb2Before(c2))
					{
						list3.Add(157);
					}
					else
					{
						list3.Add(156);
					}
					break;
				case 'ح':
					if (Chasb2Before(c2))
					{
						list3.Add(159);
					}
					else
					{
						list3.Add(158);
					}
					break;
				case 'خ':
					if (Chasb2Before(c2))
					{
						list3.Add(161);
					}
					else
					{
						list3.Add(160);
					}
					break;
				case 'د':
					list3.Add(162);
					break;
				case 'ذ':
					list3.Add(163);
					break;
				case 'ر':
					list3.Add(164);
					break;
				case 'ز':
					list3.Add(165);
					break;
				case 'س':
					if (Chasb2Before(c2))
					{
						list3.Add(168);
					}
					else
					{
						list3.Add(167);
					}
					break;
				case 'ش':
					if (Chasb2Before(c2))
					{
						list3.Add(170);
					}
					else
					{
						list3.Add(169);
					}
					break;
				case 'ص':
					if (Chasb2Before(c2))
					{
						list3.Add(172);
					}
					else
					{
						list3.Add(171);
					}
					break;
				case 'ض':
					if (Chasb2Before(c2))
					{
						list3.Add(174);
					}
					else
					{
						list3.Add(173);
					}
					break;
				case 'ط':
					list3.Add(175);
					break;
				case 'ظ':
					list3.Add(224);
					break;
				case 'ع':
					if (Chasb2Before(c2) && Chasb2After(ch))
					{
						list3.Add(227);
					}
					else if (!Chasb2Before(c2) && !Chasb2After(ch))
					{
						list3.Add(225);
					}
					else if (Chasb2Before(c2))
					{
						list3.Add(228);
					}
					else if (Chasb2After(ch))
					{
						list3.Add(226);
					}
					break;
				case 'غ':
					if (Chasb2Before(c2) && Chasb2After(ch))
					{
						list3.Add(231);
					}
					else if (!Chasb2Before(c2) && !Chasb2After(ch))
					{
						list3.Add(229);
					}
					else if (Chasb2Before(c2))
					{
						list3.Add(232);
					}
					else if (Chasb2After(ch))
					{
						list3.Add(230);
					}
					break;
				case 'ف':
					if (Chasb2Before(c2))
					{
						list3.Add(234);
					}
					else
					{
						list3.Add(233);
					}
					break;
				case 'ق':
					if (Chasb2Before(c2))
					{
						list3.Add(236);
					}
					else
					{
						list3.Add(235);
					}
					break;
				case 'گ':
					if (Chasb2Before(c2))
					{
						list3.Add(240);
					}
					else
					{
						list3.Add(239);
					}
					break;
				case 'ل':
					if (c2 == 'ا')
					{
						list3.Add(242);
						c = c2;
						c2 = ' ';
						if (i + 1 < Unicode_Text.Length)
						{
							c2 = Unicode_Text[i + 1];
						}
						i++;
					}
					else if (Chasb2Before(c2))
					{
						list3.Add(243);
					}
					else
					{
						list3.Add(241);
					}
					break;
				case 'م':
					if (Chasb2Before(c2))
					{
						list3.Add(245);
					}
					else
					{
						list3.Add(244);
					}
					break;
				case 'ن':
					if (Chasb2Before(c2))
					{
						list3.Add(247);
					}
					else
					{
						list3.Add(246);
					}
					break;
				case 'و':
					if (Chasb2Before(c2))
					{
						list3.Add(248);
					}
					else
					{
						list3.Add(248);
					}
					break;
				case 'ه':
					if (Chasb2After(ch) && Chasb2Before(c2))
					{
						list3.Add(250);
					}
					else if (Chasb2Before(c2))
					{
						list3.Add(251);
					}
					else
					{
						list3.Add(249);
					}
					break;
				case 'ی':
					if (Chasb2Before(c2))
					{
						list3.Add(254);
					}
					else if (Chasb2After(ch))
					{
						list3.Add(253);
					}
					else
					{
						list3.Add(252);
					}
					break;
				case '░':
					list3.Add(176);
					break;
				case '▒':
					list3.Add(177);
					break;
				case '▓':
					list3.Add(178);
					break;
				case '│':
					list3.Add(179);
					break;
				case '┤':
					list3.Add(180);
					break;
				case '╡':
					list3.Add(181);
					break;
				case '╢':
					list3.Add(182);
					break;
				case '╖':
					list3.Add(183);
					break;
				case '╕':
					list3.Add(184);
					break;
				case '╣':
					list3.Add(185);
					break;
				case '║':
					list3.Add(186);
					break;
				case '╗':
					list3.Add(187);
					break;
				case '╝':
					list3.Add(188);
					break;
				case '╜':
					list3.Add(189);
					break;
				case '╛':
					list3.Add(190);
					break;
				case '┐':
					list3.Add(191);
					break;
				case '└':
					list3.Add(192);
					break;
				case '┴':
					list3.Add(193);
					break;
				case '┬':
					list3.Add(194);
					break;
				case '├':
					list3.Add(195);
					break;
				case '─':
					list3.Add(196);
					break;
				case '┼':
					list3.Add(197);
					break;
				case '╞':
					list3.Add(198);
					break;
				case '╟':
					list3.Add(199);
					break;
				case '╚':
					list3.Add(200);
					break;
				case '╔':
					list3.Add(201);
					break;
				case '╩':
					list3.Add(202);
					break;
				case '╦':
					list3.Add(203);
					break;
				case '╠':
					list3.Add(204);
					break;
				case '═':
					list3.Add(205);
					break;
				case '╬':
					list3.Add(206);
					break;
				case '╧':
					list3.Add(207);
					break;
				case '╨':
					list3.Add(208);
					break;
				case '╤':
					list3.Add(209);
					break;
				case '╥':
					list3.Add(210);
					break;
				case '╙':
					list3.Add(211);
					break;
				case '╘':
					list3.Add(212);
					break;
				case '╒':
					list3.Add(213);
					break;
				case '╓':
					list3.Add(214);
					break;
				case '╫':
					list3.Add(215);
					break;
				case '╪':
					list3.Add(216);
					break;
				case '┘':
					list3.Add(217);
					break;
				case '┌':
					list3.Add(218);
					break;
				case '█':
					list3.Add(219);
					break;
				case '▄':
					list3.Add(220);
					break;
				case '▌':
					list3.Add(221);
					break;
				case '▐':
					list3.Add(222);
					break;
				case '▀':
					list3.Add(223);
					break;
				default:
					list3.Add((byte)c);
					break;
				}
				ch = c;
			}
			return list3;
		}
	}

	private bool Chasb2After(char ch)
	{
		try
		{
			return ch switch
			{
				'ب' => true, 
				'پ' => true, 
				'ت' => true, 
				'ث' => true, 
				'ج' => true, 
				'چ' => true, 
				'ح' => true, 
				'خ' => true, 
				'س' => true, 
				'ش' => true, 
				'ص' => true, 
				'ض' => true, 
				'ط' => true, 
				'ظ' => true, 
				'ع' => true, 
				'غ' => true, 
				'ف' => true, 
				'ق' => true, 
				'ک' => true, 
				'گ' => true, 
				'ل' => true, 
				'م' => true, 
				'ن' => true, 
				'ه' => true, 
				'ی' => true, 
				_ => false, 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Chasb2After", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\IranSystem.cs", 724);
			return false;
		}
	}

	private bool Chasb2Before(char ch)
	{
		try
		{
			return ch switch
			{
				'ا' => true, 
				'ب' => true, 
				'پ' => true, 
				'ت' => true, 
				'ث' => true, 
				'ج' => true, 
				'چ' => true, 
				'ح' => true, 
				'خ' => true, 
				'د' => true, 
				'ذ' => true, 
				'ر' => true, 
				'ز' => true, 
				'س' => true, 
				'ش' => true, 
				'ص' => true, 
				'ض' => true, 
				'ط' => true, 
				'ظ' => true, 
				'ع' => true, 
				'غ' => true, 
				'ف' => true, 
				'ق' => true, 
				'ک' => true, 
				'گ' => true, 
				'ل' => true, 
				'م' => true, 
				'ن' => true, 
				'و' => true, 
				'ه' => true, 
				'ی' => true, 
				_ => false, 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Chasb2Before", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\IranSystem.cs", 772);
			return false;
		}
	}

	public byte get_Unicode_To_IranSystem_Char(byte PreviousChar, byte CurrentChar, byte NextChar)
	{
		try
		{
			bool flag = Char_Cond(PreviousChar) || is_Final_Letter(PreviousChar);
			bool flag2 = Char_Cond(NextChar);
			if (flag && flag2)
			{
				return UCTOIS_Group_1(CurrentChar);
			}
			if (flag)
			{
				return UCTOIS_Group_2(CurrentChar);
			}
			if (flag2)
			{
				return UCTOIS_Group_3(CurrentChar);
			}
			return UCTOIS_Group_4(CurrentChar);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "get_Unicode_To_IranSystem_Char", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\IranSystem.cs", 790);
			return 0;
		}
	}

	private byte UCTOIS_Group_4(byte CurrentChar)
	{
		try
		{
			if (CharachtersMapper_Group4.ContainsKey(CurrentChar))
			{
				return CharachtersMapper_Group4[CurrentChar];
			}
			return CurrentChar;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UCTOIS_Group_4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\IranSystem.cs", 807);
			return 0;
		}
	}

	private byte UCTOIS_Group_3(byte CurrentChar)
	{
		try
		{
			if (CharachtersMapper_Group3.ContainsKey(CurrentChar))
			{
				return CharachtersMapper_Group3[CurrentChar];
			}
			return CurrentChar;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UCTOIS_Group_3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\IranSystem.cs", 824);
			return 0;
		}
	}

	private byte UCTOIS_Group_2(byte CurrentChar)
	{
		try
		{
			if (CharachtersMapper_Group2.ContainsKey(CurrentChar))
			{
				return CharachtersMapper_Group2[CurrentChar];
			}
			return CurrentChar;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UCTOIS_Group_2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\IranSystem.cs", 841);
			return 0;
		}
	}

	private byte UCTOIS_Group_1(byte CurrentChar)
	{
		try
		{
			if (CharachtersMapper_Group1.ContainsKey(CurrentChar))
			{
				return CharachtersMapper_Group1[CurrentChar];
			}
			return CurrentChar;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UCTOIS_Group_1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\IranSystem.cs", 858);
			return 0;
		}
	}
}

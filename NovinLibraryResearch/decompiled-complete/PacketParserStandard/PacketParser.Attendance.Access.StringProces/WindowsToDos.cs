using System;
using System.Text;
using PacketParser.Entities;

namespace PacketParser.Attendance.Access.StringProces;

public static class WindowsToDos
{
	public static string WinToDosStr(this string winStr)
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		checked
		{
			try
			{
				bool flag = false;
				char c = ' ';
				winStr += " ";
				int length = winStr.Length;
				for (int i = 0; i < length; i++)
				{
					stringBuilder = stringBuilder.Append(Convert.ToChar(32));
				}
				int num = length - 1;
				string text = "";
				for (int j = 0; j < length; j++)
				{
					flag = false;
					switch (winStr[j])
					{
					case ' ':
						c = Convert.ToChar(32);
						break;
					case '.':
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						unchecked
						{
							c = (char)checked((ushort)(winStr[j] + 80));
							if (winStr[j] == '.')
							{
								c = Convert.ToChar(140);
							}
							text += c;
							flag = true;
							c = Convert.ToChar(0);
							break;
						}
					case 'A':
					case 'B':
					case 'C':
					case 'D':
					case 'E':
					case 'F':
					case 'G':
					case 'H':
					case 'I':
					case 'J':
					case 'K':
					case 'L':
					case 'M':
					case 'N':
					case 'O':
					case 'P':
					case 'Q':
					case 'R':
					case 'S':
					case 'T':
					case 'U':
					case 'V':
					case 'W':
					case 'X':
					case 'Y':
					case 'Z':
					case 'a':
					case 'b':
					case 'c':
					case 'd':
					case 'e':
					case 'f':
					case 'g':
					case 'h':
					case 'i':
					case 'j':
					case 'k':
					case 'l':
					case 'm':
					case 'n':
					case 'o':
					case 'p':
					case 'q':
					case 'r':
					case 's':
					case 't':
					case 'u':
					case 'v':
					case 'w':
					case 'x':
					case 'y':
					case 'z':
						text += winStr[j];
						flag = true;
						c = Convert.ToChar(0);
						break;
					case 'آ':
						c = Convert.ToChar(141);
						break;
					case 'ئ':
						c = Convert.ToChar(142);
						break;
					case 'ء':
						c = Convert.ToChar(143);
						break;
					case 'ا':
						c = Convert.ToChar(145);
						if (j == 0 || winStr[j - 1] == ' ' || winStr[j - 1] == Convert.ToChar(157) || winStr[j - 1] == 'ا' || winStr[j - 1] == 'و' || winStr[j - 1] == 'د' || winStr[j - 1] == 'ذ' || winStr[j - 1] == 'ر' || winStr[j - 1] == 'ز' || winStr[j - 1] == 'ژ')
						{
							c = Convert.ToChar(144);
						}
						break;
					case 'ب':
						c = Convert.ToChar(147);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(146);
						}
						break;
					case 'پ':
						c = Convert.ToChar(149);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(148);
						}
						break;
					case 'ت':
						c = Convert.ToChar(151);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(150);
						}
						break;
					case 'ث':
						c = Convert.ToChar(153);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(152);
						}
						break;
					case 'ج':
						c = Convert.ToChar(155);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(154);
						}
						break;
					case 'چ':
						c = Convert.ToChar(157);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(156);
						}
						break;
					case 'ح':
						c = Convert.ToChar(159);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(158);
						}
						break;
					case 'خ':
						c = Convert.ToChar(161);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(160);
						}
						break;
					case 'د':
						c = Convert.ToChar(162);
						break;
					case 'ذ':
						c = Convert.ToChar(163);
						break;
					case 'ر':
						c = Convert.ToChar(164);
						break;
					case 'ز':
						c = Convert.ToChar(165);
						break;
					case 'ژ':
						c = Convert.ToChar(166);
						break;
					case 'س':
						c = Convert.ToChar(168);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(167);
						}
						break;
					case 'ش':
						c = Convert.ToChar(170);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(169);
						}
						break;
					case 'ص':
						c = Convert.ToChar(172);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(171);
						}
						break;
					case 'ض':
						c = Convert.ToChar(174);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(173);
						}
						break;
					case 'ط':
						c = Convert.ToChar(175);
						break;
					case 'ظ':
						c = Convert.ToChar(224);
						break;
					case 'ع':
						c = Convert.ToChar(227);
						if (j == 0 || winStr[j - 1] == ' ' || winStr[j - 1] == Convert.ToChar(157) || winStr[j - 1] == 'ا' || winStr[j - 1] == 'و' || winStr[j - 1] == 'د' || winStr[j - 1] == 'ذ' || winStr[j - 1] == 'ر' || winStr[j - 1] == 'ز' || winStr[j - 1] == 'ژ')
						{
							c = Convert.ToChar(228);
						}
						if (winStr[j + 1] == ' ' || j == length)
						{
							c = Convert.ToChar((c == Convert.ToChar(228)) ? 225 : 226);
						}
						break;
					case 'غ':
						c = Convert.ToChar(231);
						if (j == 0 || winStr[j - 1] == ' ' || winStr[j - 1] == Convert.ToChar(157) || winStr[j - 1] == 'ا' || winStr[j - 1] == 'و' || winStr[j - 1] == 'د' || winStr[j - 1] == 'ذ' || winStr[j - 1] == 'ر' || winStr[j - 1] == 'ز' || winStr[j - 1] == 'ژ')
						{
							c = Convert.ToChar(232);
						}
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar((c == Convert.ToChar(232)) ? 229 : 230);
						}
						break;
					case 'ف':
						c = Convert.ToChar(234);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(233);
						}
						break;
					case 'ق':
						c = Convert.ToChar(236);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(235);
						}
						break;
					case 'ك':
						c = Convert.ToChar(238);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(237);
						}
						break;
					case 'گ':
						c = Convert.ToChar(240);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(239);
						}
						break;
					case 'ل':
						c = Convert.ToChar(243);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(241);
						}
						break;
					case 'م':
						c = Convert.ToChar(245);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(244);
						}
						break;
					case 'ن':
						c = Convert.ToChar(247);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(246);
						}
						break;
					case 'و':
						c = Convert.ToChar(248);
						break;
					case 'ه':
						c = Convert.ToChar(250);
						if (j == 0 || winStr[j - 1] == ' ' || winStr[j - 1] == Convert.ToChar(157) || winStr[j - 1] == 'ا' || winStr[j - 1] == 'و' || winStr[j - 1] == 'د' || winStr[j - 1] == 'ذ' || winStr[j - 1] == 'ر' || winStr[j - 1] == 'ز' || winStr[j - 1] == 'ژ')
						{
							c = Convert.ToChar(251);
						}
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(249);
						}
						break;
					case 'ي':
					case 'ی':
						c = Convert.ToChar(254);
						if (winStr[j + 1] == Convert.ToChar(157) || winStr[j + 1] == ' ')
						{
							c = Convert.ToChar(252);
						}
						if (winStr[j] == 'ì')
						{
							c = Convert.ToChar(254);
						}
						break;
					case 'ـ':
						c = Convert.ToChar(139);
						break;
					default:
						c = winStr[j];
						break;
					}
					if (flag || c == Convert.ToChar(0) || num <= 0)
					{
						continue;
					}
					for (int num2 = text.Length; num2 > 0; num2--)
					{
						if (winStr[j] != Convert.ToChar(157))
						{
							stringBuilder[num] = text[num2];
							num--;
						}
					}
					if (winStr[j] != Convert.ToChar(157))
					{
						stringBuilder[num] = c;
						num--;
					}
					text = "";
				}
				if (flag && c == Convert.ToChar(0) && num > 0)
				{
					for (int num3 = text.Length; num3 > 0; num3--)
					{
						stringBuilder[num] = text[num3];
						num--;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WinToDosStr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\StringProcess\\WindowsToDos.cs", 349);
			}
			return stringBuilder.ToString().Substring(1);
		}
	}
}

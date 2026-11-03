using System.Text;

namespace Stimulsoft.Report.Export;

public sealed class StiBidirectionalConvert
{
	public enum Mode
	{
		Pdf,
		Xps
	}

	private const int arabicTableSize = 97;

	private const int ligaturesTableSize = 40;

	private Mode mode;

	private static string[,] ligaturesTable;

	private static ushort[,] arabicTable;

	private static ushort[,] arabicTableArray;

	private const string stSeparator = "./:\\,-";

	private bool useStaticArrays = true;

	public StringBuilder Convert(StringBuilder inputString, bool useRightToLeft)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (inputString.Length > 0)
		{
			if (useRightToLeft)
			{
				int i = 0;
				StringBuilder stringBuilder2 = new StringBuilder();
				if (char.IsDigit(inputString[0]))
				{
					StringBuilder stringBuilder3 = new StringBuilder();
					int j;
					for (j = 0; j < inputString.Length && (char.IsDigit(inputString[j]) || "./:\\,-".IndexOf(inputString[j]) != -1); j++)
					{
						stringBuilder3.Append(inputString[j]);
					}
					if (inputString[j - 1] == '.')
					{
						stringBuilder3.Length--;
						stringBuilder3.Insert(0, '.');
					}
					while (j < inputString.Length && (inputString[j] == ' ' || inputString[j] == '\u00a0'))
					{
						j++;
						stringBuilder.Insert(0, ' ');
					}
					stringBuilder.Append((object?)stringBuilder3);
					i = j;
				}
				for (; i < inputString.Length; i++)
				{
					ushort num = inputString[i];
					if (SymbolIsArabicOrHebrew(num) || SymbolIsRTLMark(num))
					{
						if (!SymbolIsRTLMark(num))
						{
							int num2 = stringBuilder2.Length;
							int num3 = -1;
							int num4 = -1;
							while (i > 0 && num2 > 0 && ((inputString[i - 1] >= ' ' && inputString[i - 1] <= '@') || (inputString[i - 1] >= '[' && inputString[i - 1] <= '`') || (inputString[i - 1] >= '{' && inputString[i - 1] <= '~') || inputString[i - 1] == '\u00a0'))
							{
								i--;
								num2--;
								if (inputString[i] == ' ' || inputString[i] == '\u00a0')
								{
									num3 = i;
									num4 = num2;
								}
							}
							if (num3 != -1)
							{
								i = num3;
								num2 = num4;
							}
							stringBuilder2.Length = num2;
						}
						int num5 = i;
						int num6 = num5;
						int num7 = num5;
						num = inputString[i];
						while (!SymbolIsLTRMark(num) && (SymbolIsArabicOrHebrew(num) || (num >= 32 && num <= 64) || (num >= 91 && num <= 96) || (num >= 123 && num <= 126) || num == 160 || (num >= 8192 && num <= 8303)))
						{
							if (SymbolIsArabicOrHebrew(num))
							{
								num7 = i;
							}
							if (char.IsWhiteSpace((char)num))
							{
								num6 = i;
							}
							i++;
							num = ((i < inputString.Length) ? inputString[i] : '\0');
						}
						int num8 = i;
						if (num6 > num7)
						{
							num8 = num6 + 1;
						}
						StringBuilder stringBuilder4 = new StringBuilder();
						for (i = num5; i < num8; i++)
						{
							num = inputString[i];
							if (!SymbolIsBidiMark(num))
							{
								stringBuilder4.Append(inputString[i]);
							}
						}
						i--;
						stringBuilder.Insert(0, stringBuilder2);
						stringBuilder.Insert(0, ConvertArabic(stringBuilder4));
						stringBuilder2 = new StringBuilder();
					}
					else if (!SymbolIsBidiMark(num))
					{
						stringBuilder2.Append((char)num);
					}
				}
				stringBuilder.Insert(0, stringBuilder2);
				if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '.')
				{
					string text = stringBuilder.ToString();
					int num9 = text.Length - 1;
					while (num9 > 0 && text[num9 - 1] == '.')
					{
						num9--;
					}
					stringBuilder.Clear();
					stringBuilder.Append(text.Substring(num9));
					stringBuilder.Append(text.Substring(0, num9));
				}
			}
			else
			{
				for (int k = 0; k < inputString.Length; k++)
				{
					ushort num10 = inputString[k];
					if (SymbolIsArabicOrHebrew(num10) && !char.IsDigit((char)num10))
					{
						StringBuilder stringBuilder5 = new StringBuilder();
						while (SymbolIsArabicOrHebrew(num10) || (num10 >= 32 && num10 <= 64) || (num10 >= 91 && num10 <= 96) || (num10 >= 123 && num10 <= 126) || num10 == 160 || (num10 >= 8192 && num10 <= 8303))
						{
							if (!SymbolIsBidiMark(num10))
							{
								stringBuilder5.Append(inputString[k]);
							}
							k++;
							num10 = ((k < inputString.Length) ? inputString[k] : '\0');
						}
						k--;
						while (inputString[k] == ' ' || inputString[k] == '\u00a0')
						{
							k--;
							stringBuilder5.Length--;
						}
						stringBuilder.Append(ConvertArabic(stringBuilder5));
					}
					else if (!SymbolIsBidiMark(num10))
					{
						stringBuilder.Append((char)num10);
					}
				}
			}
			if (mode == Mode.Pdf && StiOptions.Export.Pdf.ConvertDigitsToArabic && useRightToLeft)
			{
				return StiExportUtils.ConvertDigitsToArabic(stringBuilder, StiOptions.Export.Pdf.ArabicDigitsType);
			}
			_ = mode;
			_ = 1;
		}
		return stringBuilder;
	}

	private string ConvertArabic(StringBuilder inputSB)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < inputSB.Length; i++)
		{
			switch (arabicTableArray[(uint)inputSB[i], 0])
			{
			case 3:
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(inputSB[i]);
				while (i + 1 < inputSB.Length && arabicTableArray[(uint)inputSB[i + 1], 0] == 3)
				{
					i++;
					stringBuilder2.Append(inputSB[i]);
				}
				if (i + 1 < inputSB.Length && arabicTableArray[(uint)inputSB[i + 1], 0] == 2)
				{
					i++;
					stringBuilder2.Append(inputSB[i]);
				}
				if (stringBuilder2.Length == 1)
				{
					stringBuilder.Append((char)arabicTableArray[(uint)stringBuilder2[0], 1]);
					break;
				}
				stringBuilder.Append((char)arabicTableArray[(uint)stringBuilder2[0], 3]);
				if (stringBuilder2.Length > 2)
				{
					for (int j = 1; j < stringBuilder2.Length - 1; j++)
					{
						stringBuilder.Append((char)arabicTableArray[(uint)stringBuilder2[j], 4]);
					}
				}
				stringBuilder.Append((char)arabicTableArray[(uint)stringBuilder2[stringBuilder2.Length - 1], 2]);
				break;
			}
			case 1:
			case 2:
				stringBuilder.Append((char)arabicTableArray[(uint)inputSB[i], 1]);
				break;
			default:
				stringBuilder.Append(inputSB[i]);
				break;
			}
		}
		StringBuilder stringBuilder3 = new StringBuilder();
		stringBuilder3 = stringBuilder;
		for (int k = 0; k < 40; k++)
		{
			stringBuilder3 = stringBuilder3.Replace(ligaturesTable[k, 0], ligaturesTable[k, 1]);
		}
		StringBuilder stringBuilder4 = new StringBuilder();
		for (int num = stringBuilder3.Length - 1; num >= 0; num--)
		{
			ushort num2 = stringBuilder3[num];
			switch (num2)
			{
			case 40:
				num2 = 41;
				break;
			case 41:
				num2 = 40;
				break;
			case 91:
				num2 = 93;
				break;
			case 93:
				num2 = 91;
				break;
			case 123:
				num2 = 125;
				break;
			case 125:
				num2 = 123;
				break;
			}
			if (char.IsDigit((char)num2))
			{
				int num3 = num;
				while (num3 > 0 && SymbolIsDigitOrDelimiter(stringBuilder3[num3 - 1]))
				{
					num3--;
				}
				for (int l = num3; l <= num; l++)
				{
					stringBuilder4.Append(stringBuilder3[l]);
				}
				num = num3;
			}
			else
			{
				stringBuilder4.Append((char)num2);
			}
		}
		return stringBuilder4.ToString();
	}

	private bool SymbolIsDigitOrDelimiter(ushort num)
	{
		if (!char.IsDigit((char)num) && num != 46 && num != 47 && num != 58 && num != 92 && num != 44)
		{
			return num == 45;
		}
		return true;
	}

	private static bool SymbolIsArabicOrHebrew(ushort num)
	{
		if ((num < 1536 || num > 1791) && (num < 1424 || num > 1535) && (num < 64285 || num > 64335) && (num < 64336 || num > 65023))
		{
			if (num >= 65136)
			{
				return num <= 65279;
			}
			return false;
		}
		return true;
	}

	public static bool StringContainArabicOrHebrew(string st)
	{
		if (string.IsNullOrWhiteSpace(st))
		{
			return false;
		}
		foreach (char num in st)
		{
			if (SymbolIsArabicOrHebrew(num))
			{
				return true;
			}
		}
		return false;
	}

	private bool SymbolIsBidiMark(ushort num)
	{
		if (num != 8206 && num != 8207)
		{
			if (num >= 8234)
			{
				return num <= 8238;
			}
			return false;
		}
		return true;
	}

	private bool SymbolIsLTRMark(ushort num)
	{
		if (num != 8206 && num != 8234)
		{
			return num == 8237;
		}
		return true;
	}

	private bool SymbolIsRTLMark(ushort num)
	{
		if (num != 8207 && num != 8235)
		{
			return num == 8238;
		}
		return true;
	}

	public void Clear()
	{
		if (!useStaticArrays)
		{
			arabicTable = null;
			ligaturesTable = null;
			arabicTableArray = null;
		}
	}

	public StiBidirectionalConvert(Mode mode)
	{
		this.mode = mode;
		if (arabicTable == null)
		{
			ushort[,] array = new ushort[97, 6]
			{
				{ 1569, 1, 65152, 0, 0, 0 },
				{ 1570, 2, 65153, 65154, 0, 0 },
				{ 1571, 2, 65155, 65156, 0, 0 },
				{ 1572, 2, 65157, 65158, 0, 0 },
				{ 1573, 2, 65159, 65160, 0, 0 },
				{ 1574, 3, 65161, 65162, 65163, 65164 },
				{ 1575, 2, 65165, 65166, 0, 0 },
				{ 1576, 3, 65167, 65168, 65169, 65170 },
				{ 1577, 2, 65171, 65172, 0, 0 },
				{ 1578, 3, 65173, 65174, 65175, 65176 },
				{ 1579, 3, 65177, 65178, 65179, 65180 },
				{ 1580, 3, 65181, 65182, 65183, 65184 },
				{ 1581, 3, 65185, 65186, 65187, 65188 },
				{ 1582, 3, 65189, 65190, 65191, 65192 },
				{ 1583, 2, 65193, 65194, 0, 0 },
				{ 1584, 2, 65195, 65196, 0, 0 },
				{ 1585, 2, 65197, 65198, 0, 0 },
				{ 1586, 2, 65199, 65200, 0, 0 },
				{ 1587, 3, 65201, 65202, 65203, 65204 },
				{ 1588, 3, 65205, 65206, 65207, 65208 },
				{ 1589, 3, 65209, 65210, 65211, 65212 },
				{ 1590, 3, 65213, 65214, 65215, 65216 },
				{ 1591, 3, 65217, 65218, 65219, 65220 },
				{ 1592, 3, 65221, 65222, 65223, 65224 },
				{ 1593, 3, 65225, 65226, 65227, 65228 },
				{ 1594, 3, 65229, 65230, 65231, 65232 },
				{ 1601, 3, 65233, 65234, 65235, 65236 },
				{ 1602, 3, 65237, 65238, 65239, 65240 },
				{ 1603, 3, 65241, 65242, 65243, 65244 },
				{ 1604, 3, 65245, 65246, 65247, 65248 },
				{ 1605, 3, 65249, 65250, 65251, 65252 },
				{ 1606, 3, 65253, 65254, 65255, 65256 },
				{ 1607, 3, 65257, 65258, 65259, 65260 },
				{ 1608, 2, 65261, 65262, 0, 0 },
				{ 1609, 3, 65263, 65264, 64488, 64489 },
				{ 1610, 3, 65265, 65266, 65267, 65268 },
				{ 1649, 2, 64336, 64337, 0, 0 },
				{ 1655, 2, 64477, 1655, 0, 0 },
				{ 1657, 3, 64358, 64359, 64360, 64361 },
				{ 1658, 3, 64350, 64351, 64352, 64353 },
				{ 1659, 3, 64338, 64339, 64340, 64341 },
				{ 1662, 3, 64342, 64343, 64344, 64345 },
				{ 1663, 3, 64354, 64355, 64356, 64357 },
				{ 1664, 3, 64346, 64347, 64348, 64349 },
				{ 1667, 3, 64374, 64375, 64376, 64377 },
				{ 1668, 3, 64370, 64371, 64372, 64373 },
				{ 1670, 3, 64378, 64379, 64380, 64381 },
				{ 1671, 3, 64382, 64383, 64384, 64385 },
				{ 1672, 2, 64392, 64393, 0, 0 },
				{ 1676, 2, 64388, 64389, 0, 0 },
				{ 1677, 2, 64386, 64387, 0, 0 },
				{ 1678, 2, 64390, 64391, 0, 0 },
				{ 1681, 2, 64396, 64397, 0, 0 },
				{ 1688, 2, 64394, 64395, 0, 0 },
				{ 1700, 3, 64362, 64363, 64364, 64365 },
				{ 1702, 3, 64366, 64367, 64368, 64369 },
				{ 1705, 3, 64398, 64399, 64400, 64401 },
				{ 1709, 3, 64467, 64468, 64469, 64470 },
				{ 1711, 3, 64402, 64403, 64404, 64405 },
				{ 1713, 3, 64410, 64411, 64412, 64413 },
				{ 1715, 3, 64406, 64407, 64408, 64409 },
				{ 1722, 2, 64414, 64415, 0, 0 },
				{ 1723, 3, 64416, 64417, 64418, 64419 },
				{ 1726, 3, 64426, 64427, 64428, 64429 },
				{ 1728, 2, 64420, 64421, 0, 0 },
				{ 1729, 3, 64422, 64423, 64424, 64425 },
				{ 1733, 2, 64480, 64481, 0, 0 },
				{ 1734, 2, 64473, 64474, 0, 0 },
				{ 1735, 2, 64471, 64472, 0, 0 },
				{ 1736, 2, 64475, 64476, 0, 0 },
				{ 1737, 2, 64482, 64483, 0, 0 },
				{ 1739, 2, 64478, 64479, 0, 0 },
				{ 1740, 3, 64508, 64509, 64510, 64511 },
				{ 1744, 3, 64484, 64485, 64486, 64487 },
				{ 1746, 2, 64430, 64431, 0, 0 },
				{ 1747, 2, 64432, 64433, 0, 0 },
				{ 1611, 3, 1611, 1611, 1611, 1611 },
				{ 1612, 3, 1612, 1612, 1612, 1612 },
				{ 1613, 3, 1613, 1613, 1613, 1613 },
				{ 1614, 3, 1614, 1614, 1614, 1614 },
				{ 1615, 3, 1615, 1615, 1615, 1615 },
				{ 1616, 3, 1616, 1616, 1616, 1616 },
				{ 1617, 3, 1617, 1617, 1617, 1617 },
				{ 1618, 3, 1618, 1618, 1618, 1618 },
				{ 1619, 3, 1619, 1619, 1619, 1619 },
				{ 1620, 3, 1620, 1620, 1620, 1620 },
				{ 1621, 3, 1621, 1621, 1621, 1621 },
				{ 1622, 3, 1622, 1622, 1622, 1622 },
				{ 1623, 3, 1623, 1623, 1623, 1623 },
				{ 1624, 3, 1624, 1624, 1624, 1624 },
				{ 1625, 3, 1625, 1625, 1625, 1625 },
				{ 1626, 3, 1626, 1626, 1626, 1626 },
				{ 1627, 3, 1627, 1627, 1627, 1627 },
				{ 1628, 3, 1628, 1628, 1628, 1628 },
				{ 1629, 3, 1629, 1629, 1629, 1629 },
				{ 1630, 3, 1630, 1630, 1630, 1630 },
				{ 1631, 3, 1631, 1631, 1631, 1631 }
			};
			arabicTable = array;
		}
		if (ligaturesTable == null)
		{
			string[,] array2 = new string[40, 2]
			{
				{ "ﻟﺂ", "ﻵ" },
				{ "ﻠﺂ", "ﻶ" },
				{ "ﻟﺄ", "ﻷ" },
				{ "ﻠﺄ", "ﻸ" },
				{ "ﻟﺈ", "ﻹ" },
				{ "ﻠﺈ", "ﻺ" },
				{ "ﻟﺎ", "ﻻ" },
				{ "ﻠﺎ", "ﻼ" },
				{ "ש\u05c1", "שׁ" },
				{ "ש\u05c2", "שׂ" },
				{ "שּ\u05c1", "שּׁ" },
				{ "שּ\u05c2", "שּׂ" },
				{ "א\u05b7", "אַ" },
				{ "א\u05b8", "אָ" },
				{ "א\u05bc", "אּ" },
				{ "ב\u05bc", "בּ" },
				{ "ג\u05bc", "גּ" },
				{ "ד\u05bc", "דּ" },
				{ "ה\u05bc", "הּ" },
				{ "ו\u05bc", "וּ" },
				{ "ז\u05bc", "זּ" },
				{ "ט\u05bc", "טּ" },
				{ "י\u05bc", "יּ" },
				{ "ך\u05bc", "ךּ" },
				{ "כ\u05bc", "כּ" },
				{ "ל\u05bc", "לּ" },
				{ "מ\u05bc", "מּ" },
				{ "נ\u05bc", "נּ" },
				{ "ס\u05bc", "סּ" },
				{ "ף\u05bc", "ףּ" },
				{ "פ\u05bc", "פּ" },
				{ "צ\u05bc", "צּ" },
				{ "ק\u05bc", "קּ" },
				{ "ר\u05bc", "רּ" },
				{ "ש\u05bc", "שּ" },
				{ "ת\u05bc", "תּ" },
				{ "ו\u05b9", "וֹ" },
				{ "ב\u05bf", "בֿ" },
				{ "כ\u05bf", "כֿ" },
				{ "פ\u05bf", "פֿ" }
			};
			ligaturesTable = array2;
		}
		if (arabicTableArray == null)
		{
			ushort[,] array3 = new ushort[65536, 5];
			for (int i = 0; i < 97; i++)
			{
				ushort num = arabicTable[i, 0];
				array3[num, 0] = arabicTable[i, 1];
				array3[num, 1] = arabicTable[i, 2];
				array3[num, 2] = arabicTable[i, 3];
				array3[num, 3] = arabicTable[i, 4];
				array3[num, 4] = arabicTable[i, 5];
			}
			for (ushort num2 = 1424; num2 <= 1535; num2++)
			{
				array3[num2, 0] = 1;
				array3[num2, 1] = num2;
			}
			for (ushort num3 = 64285; num3 <= 64335; num3++)
			{
				array3[num3, 0] = 1;
				array3[num3, 1] = num3;
			}
			arabicTableArray = array3;
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiMaxicodeBarCodeTypeConverter))]
public class StiMaxicodeBarCodeType : StiBarCodeTypeService
{
	protected class StiMaxicodeException : Exception
	{
		public StiMaxicodeException()
		{
		}

		public StiMaxicodeException(string message)
			: base(message)
		{
		}

		public StiMaxicodeException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}

	protected class StiMaxicode
	{
		public class ReedSolomon
		{
			private int logmod;

			private int rlen;

			private int[] log;

			private int[] alog;

			private int[] rspoly;

			public ReedSolomon(int poly, int nsym, int index)
			{
				int num = 0;
				int num2 = 0;
				for (num2 = 1; num2 <= poly; num2 <<= 1)
				{
					num++;
				}
				num2 >>= 1;
				num--;
				logmod = (1 << num) - 1;
				log = new int[logmod + 1];
				alog = new int[logmod];
				int num3 = 1;
				for (int i = 0; i < logmod; i++)
				{
					alog[i] = num3;
					log[num3] = i;
					num3 <<= 1;
					if ((num3 & num2) != 0)
					{
						num3 ^= poly;
					}
				}
				rlen = nsym;
				rspoly = new int[nsym + 1];
				rspoly[0] = 1;
				for (int j = 1; j <= nsym; j++)
				{
					rspoly[j] = 1;
					for (int num4 = j - 1; num4 > 0; num4--)
					{
						if (rspoly[num4] != 0)
						{
							rspoly[num4] = alog[(log[rspoly[num4]] + index) % logmod];
						}
						rspoly[num4] ^= rspoly[num4 - 1];
					}
					rspoly[0] = alog[(log[rspoly[0]] + index) % logmod];
					index++;
				}
			}

			public byte[] Encode(int len, byte[] data)
			{
				byte[] array = new byte[rlen];
				for (int i = 0; i < len; i++)
				{
					int num = array[rlen - 1] ^ data[i];
					for (int num2 = rlen - 1; num2 > 0; num2--)
					{
						if (num != 0 && rspoly[num2] != 0)
						{
							array[num2] = (byte)(array[num2 - 1] ^ alog[(log[num] + log[rspoly[num2]]) % logmod]);
						}
						else
						{
							array[num2] = array[num2 - 1];
						}
					}
					if (num != 0 && rspoly[0] != 0)
					{
						array[0] = (byte)alog[(log[num] + log[rspoly[0]]) % logmod];
					}
					else
					{
						array[0] = 0;
					}
				}
				return array;
			}
		}

		private const char com_RS = '\u001e';

		private const char com_GS = '\u001d';

		private static readonly string FormatHeader = "[)>\u001e01\u001d";

		private static readonly string FormatTrailer = "\u001e\u0004";

		private static readonly int[] Grid_Map = new int[990]
		{
			122, 121, 128, 127, 134, 133, 140, 139, 146, 145,
			152, 151, 158, 157, 164, 163, 170, 169, 176, 175,
			182, 181, 188, 187, 194, 193, 200, 199, 0, 0,
			124, 123, 130, 129, 136, 135, 142, 141, 148, 147,
			154, 153, 160, 159, 166, 165, 172, 171, 178, 177,
			184, 183, 190, 189, 196, 195, 202, 201, 817, 0,
			126, 125, 132, 131, 138, 137, 144, 143, 150, 149,
			156, 155, 162, 161, 168, 167, 174, 173, 180, 179,
			186, 185, 192, 191, 198, 197, 204, 203, 819, 818,
			284, 283, 278, 277, 272, 271, 266, 265, 260, 259,
			254, 253, 248, 247, 242, 241, 236, 235, 230, 229,
			224, 223, 218, 217, 212, 211, 206, 205, 820, 0,
			286, 285, 280, 279, 274, 273, 268, 267, 262, 261,
			256, 255, 250, 249, 244, 243, 238, 237, 232, 231,
			226, 225, 220, 219, 214, 213, 208, 207, 822, 821,
			288, 287, 282, 281, 276, 275, 270, 269, 264, 263,
			258, 257, 252, 251, 246, 245, 240, 239, 234, 233,
			228, 227, 222, 221, 216, 215, 210, 209, 823, 0,
			290, 289, 296, 295, 302, 301, 308, 307, 314, 313,
			320, 319, 326, 325, 332, 331, 338, 337, 344, 343,
			350, 349, 356, 355, 362, 361, 368, 367, 825, 824,
			292, 291, 298, 297, 304, 303, 310, 309, 316, 315,
			322, 321, 328, 327, 334, 333, 340, 339, 346, 345,
			352, 351, 358, 357, 364, 363, 370, 369, 826, 0,
			294, 293, 300, 299, 306, 305, 312, 311, 318, 317,
			324, 323, 330, 329, 336, 335, 342, 341, 348, 347,
			354, 353, 360, 359, 366, 365, 372, 371, 828, 827,
			410, 409, 404, 403, 398, 397, 392, 391, 80, 79,
			0, 0, 14, 13, 38, 37, 3, 0, 45, 44,
			110, 109, 386, 385, 380, 379, 374, 373, 829, 0,
			412, 411, 406, 405, 400, 399, 394, 393, 82, 81,
			41, 0, 16, 15, 40, 39, 4, 0, 0, 46,
			112, 111, 388, 387, 382, 381, 376, 375, 831, 830,
			414, 413, 408, 407, 402, 401, 396, 395, 84, 83,
			42, 0, 0, 0, 0, 0, 6, 5, 48, 47,
			114, 113, 390, 389, 384, 383, 378, 377, 832, 0,
			416, 415, 422, 421, 428, 427, 104, 103, 56, 55,
			17, 0, 0, 0, 0, 0, 0, 0, 21, 20,
			86, 85, 434, 433, 440, 439, 446, 445, 834, 833,
			418, 417, 424, 423, 430, 429, 106, 105, 58, 57,
			0, 0, 0, 0, 0, 0, 0, 0, 23, 22,
			88, 87, 436, 435, 442, 441, 448, 447, 835, 0,
			420, 419, 426, 425, 432, 431, 108, 107, 60, 59,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 24,
			90, 89, 438, 437, 444, 443, 450, 449, 837, 836,
			482, 481, 476, 475, 470, 469, 49, 0, 31, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			54, 53, 464, 463, 458, 457, 452, 451, 838, 0,
			484, 483, 478, 477, 472, 471, 50, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 466, 465, 460, 459, 454, 453, 840, 839,
			486, 485, 480, 479, 474, 473, 52, 51, 32, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
			0, 43, 468, 467, 462, 461, 456, 455, 841, 0,
			488, 487, 494, 493, 500, 499, 98, 97, 62, 61,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 27,
			92, 91, 506, 505, 512, 511, 518, 517, 843, 842,
			490, 489, 496, 495, 502, 501, 100, 99, 64, 63,
			0, 0, 0, 0, 0, 0, 0, 0, 29, 28,
			94, 93, 508, 507, 514, 513, 520, 519, 844, 0,
			492, 491, 498, 497, 504, 503, 102, 101, 66, 65,
			18, 0, 0, 0, 0, 0, 0, 0, 19, 30,
			96, 95, 510, 509, 516, 515, 522, 521, 846, 845,
			560, 559, 554, 553, 548, 547, 542, 541, 74, 73,
			33, 0, 0, 0, 0, 0, 0, 11, 68, 67,
			116, 115, 536, 535, 530, 529, 524, 523, 847, 0,
			562, 561, 556, 555, 550, 549, 544, 543, 76, 75,
			0, 0, 8, 7, 36, 35, 12, 0, 70, 69,
			118, 117, 538, 537, 532, 531, 526, 525, 849, 848,
			564, 563, 558, 557, 552, 551, 546, 545, 78, 77,
			0, 34, 10, 9, 26, 25, 0, 0, 72, 71,
			120, 119, 540, 539, 534, 533, 528, 527, 850, 0,
			566, 565, 572, 571, 578, 577, 584, 583, 590, 589,
			596, 595, 602, 601, 608, 607, 614, 613, 620, 619,
			626, 625, 632, 631, 638, 637, 644, 643, 852, 851,
			568, 567, 574, 573, 580, 579, 586, 585, 592, 591,
			598, 597, 604, 603, 610, 609, 616, 615, 622, 621,
			628, 627, 634, 633, 640, 639, 646, 645, 853, 0,
			570, 569, 576, 575, 582, 581, 588, 587, 594, 593,
			600, 599, 606, 605, 612, 611, 618, 617, 624, 623,
			630, 629, 636, 635, 642, 641, 648, 647, 855, 854,
			728, 727, 722, 721, 716, 715, 710, 709, 704, 703,
			698, 697, 692, 691, 686, 685, 680, 679, 674, 673,
			668, 667, 662, 661, 656, 655, 650, 649, 856, 0,
			730, 729, 724, 723, 718, 717, 712, 711, 706, 705,
			700, 699, 694, 693, 688, 687, 682, 681, 676, 675,
			670, 669, 664, 663, 658, 657, 652, 651, 858, 857,
			732, 731, 726, 725, 720, 719, 714, 713, 708, 707,
			702, 701, 696, 695, 690, 689, 684, 683, 678, 677,
			672, 671, 666, 665, 660, 659, 654, 653, 859, 0,
			734, 733, 740, 739, 746, 745, 752, 751, 758, 757,
			764, 763, 770, 769, 776, 775, 782, 781, 788, 787,
			794, 793, 800, 799, 806, 805, 812, 811, 861, 860,
			736, 735, 742, 741, 748, 747, 754, 753, 760, 759,
			766, 765, 772, 771, 778, 777, 784, 783, 790, 789,
			796, 795, 802, 801, 808, 807, 814, 813, 862, 0,
			738, 737, 744, 743, 750, 749, 756, 755, 762, 761,
			768, 767, 774, 773, 780, 779, 786, 785, 792, 791,
			798, 797, 804, 803, 810, 809, 816, 815, 864, 863
		};

		private static readonly int[] CodeSet_Map = new int[256]
		{
			5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 0, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 5, 5, 5, 5, 5, 0, 0,
			0, 5, 0, 2, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 0, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 0, 2,
			2, 2, 2, 2, 2, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 2, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 2, 2, 2, 2, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 4, 4,
			4, 4, 4, 4, 4, 4, 4, 4, 4, 5,
			5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 4, 5, 5, 5, 5, 5, 5, 4, 5,
			3, 4, 3, 5, 5, 4, 4, 3, 3, 3,
			4, 3, 5, 4, 4, 3, 3, 4, 3, 3,
			3, 4, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 4, 4, 4, 4, 4, 4,
			4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
			4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
			4, 4, 4, 4, 4, 4
		};

		private static readonly byte[] CharToSym_Map = new byte[256]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 30, 28, 29,
			30, 35, 32, 53, 34, 35, 36, 37, 38, 39,
			40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
			50, 51, 52, 53, 54, 55, 56, 57, 58, 37,
			38, 39, 40, 41, 52, 1, 2, 3, 4, 5,
			6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
			16, 17, 18, 19, 20, 21, 22, 23, 24, 25,
			26, 42, 43, 44, 45, 46, 0, 1, 2, 3,
			4, 5, 6, 7, 8, 9, 10, 11, 12, 13,
			14, 15, 16, 17, 18, 19, 20, 21, 22, 23,
			24, 25, 26, 32, 54, 34, 35, 36, 48, 49,
			50, 51, 52, 53, 54, 55, 56, 57, 47, 48,
			49, 50, 51, 52, 53, 54, 55, 56, 57, 48,
			49, 50, 51, 52, 53, 54, 55, 56, 57, 36,
			37, 37, 38, 39, 40, 41, 42, 43, 38, 44,
			37, 39, 38, 45, 46, 40, 41, 39, 40, 41,
			42, 42, 47, 43, 44, 43, 44, 45, 45, 46,
			47, 46, 0, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 16, 17,
			18, 19, 20, 21, 22, 23, 24, 25, 26, 32,
			33, 34, 35, 36, 0, 1, 2, 3, 4, 5,
			6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
			16, 17, 18, 19, 20, 21, 22, 23, 24, 25,
			26, 32, 33, 34, 35, 36
		};

		private StiMaxicodeMode mode;

		private int structuredAppendPosition = 1;

		private int structuredAppendTotal = 1;

		private bool trimExcessData = true;

		private string data_Postcode;

		private int data_Country;

		private int data_Service;

		private string content;

		private int eciMode = 3;

		private List<byte> codewords;

		private byte[] sourceBytes;

		private int[] set = new int[144];

		private byte[] character = new byte[144];

		private bool[,] grid = new bool[33, 30];

		private string errorMessage;

		private int offsetEndOfData;

		public byte[] Matrix => GridToMatrix();

		public string ErrorMessage => errorMessage;

		protected int GetEciEncoding()
		{
			if (TryCodePage("ISO-8859-1"))
			{
				return 3;
			}
			if (TryCodePage("ISO-8859-2"))
			{
				return 4;
			}
			if (TryCodePage("ISO-8859-3"))
			{
				return 5;
			}
			if (TryCodePage("ISO-8859-4"))
			{
				return 6;
			}
			if (TryCodePage("ISO-8859-5"))
			{
				return 7;
			}
			if (TryCodePage("ISO-8859-6"))
			{
				return 8;
			}
			if (TryCodePage("ISO-8859-7"))
			{
				return 9;
			}
			if (TryCodePage("ISO-8859-8"))
			{
				return 10;
			}
			if (TryCodePage("ISO-8859-9"))
			{
				return 11;
			}
			if (TryCodePage("ISO-8859-11"))
			{
				return 13;
			}
			if (TryCodePage("ISO-8859-13"))
			{
				return 15;
			}
			if (TryCodePage("ISO-8859-15"))
			{
				return 17;
			}
			if (TryCodePage("Windows-1250"))
			{
				return 21;
			}
			if (TryCodePage("Windows-1251"))
			{
				return 22;
			}
			if (TryCodePage("Windows-1252"))
			{
				return 23;
			}
			if (TryCodePage("Windows-1256"))
			{
				return 24;
			}
			if (TryCodePage("SJIS"))
			{
				return 20;
			}
			TryCodePage("UTF-8");
			return 26;
		}

		private bool TryCodePage(string charset)
		{
			try
			{
				Encoding encoding = Encoding.GetEncoding(charset);
				sourceBytes = encoding.GetBytes(content);
				string @string = encoding.GetString(sourceBytes);
				if (content == @string)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		private string UnpackTilde(string input)
		{
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			while (num < input.Length)
			{
				char c = input[num++];
				bool flag = false;
				if (c == '~' && num + 2 < input.Length)
				{
					string s = input.Substring(num, 3);
					int result = 0;
					if (int.TryParse(s, out result) && result >= 0 && result < 255)
					{
						stringBuilder.Append((char)result);
						flag = true;
						num += 3;
					}
				}
				if (!flag)
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		private List<string> ExtractPrimaryParts(string input)
		{
			List<string> list = new List<string>();
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			while (num < input.Length)
			{
				char c = input[num++];
				if (c == '\u001d')
				{
					list.Add(stringBuilder.ToString());
					if (list.Count == 3)
					{
						list.Add(input.Substring(num));
						stringBuilder.Clear();
						break;
					}
					stringBuilder = new StringBuilder();
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			if (stringBuilder.Length > 0)
			{
				list.Add(stringBuilder.ToString());
			}
			return list;
		}

		private byte[] GridToMatrix()
		{
			byte[] array = new byte[990];
			for (int i = 0; i < 33; i++)
			{
				for (int j = 0; j < 30; j++)
				{
					if (grid[i, j])
					{
						array[i * 30 + j] = 1;
					}
				}
			}
			return array;
		}

		private int FindBestSet(int index, int length, int set1, int set2, int set3 = -1, int set4 = -1, int set5 = -1)
		{
			int num = set[index - 1];
			bool flag = num == set1 || num == set2 || num == set3 || num == set4 || num == set5;
			if (index + 1 < length)
			{
				int num2 = set[index + 1];
				bool flag2 = num2 == set1 || num2 == set2 || num2 == set3 || num2 == set4 || num2 == set5;
				if (flag && flag2)
				{
					return Math.Min(num, num2);
				}
				if (flag)
				{
					return num;
				}
				if (flag2)
				{
					return num2;
				}
			}
			else if (flag)
			{
				return num;
			}
			return set1;
		}

		private void InsertSymbol(int position, byte c)
		{
			for (int num = 143; num > position; num--)
			{
				set[num] = set[num - 1];
				character[num] = character[num - 1];
			}
			character[position] = c;
		}

		private bool ProcessTextToSymbols()
		{
			int num = sourceBytes.Length;
			if (num > 138)
			{
				return false;
			}
			for (int i = 0; i < 144; i++)
			{
				set[i] = -1;
				character[i] = 0;
			}
			for (int j = 0; j < num; j++)
			{
				set[j] = CodeSet_Map[sourceBytes[j]];
				character[j] = CharToSym_Map[sourceBytes[j]];
			}
			if (set[0] == 0)
			{
				if (character[0] == 13)
				{
					character[0] = 0;
				}
				set[0] = 1;
			}
			int k;
			for (k = 1; k < num; k++)
			{
				if (set[k] != 0)
				{
					continue;
				}
				if (character[k] == 13)
				{
					set[k] = FindBestSet(k, num, 1, 5);
					if (set[k] == 5)
					{
						character[k] = 13;
					}
					else
					{
						character[k] = 0;
					}
				}
				else if (character[k] == 28)
				{
					set[k] = FindBestSet(k, num, 1, 2, 3, 4, 5);
					if (set[k] == 5)
					{
						character[k] = 32;
					}
				}
				else if (character[k] == 29)
				{
					set[k] = FindBestSet(k, num, 1, 2, 3, 4, 5);
					if (set[k] == 5)
					{
						character[k] = 33;
					}
				}
				else if (character[k] == 30)
				{
					set[k] = FindBestSet(k, num, 1, 2, 3, 4, 5);
					if (set[k] == 5)
					{
						character[k] = 34;
					}
				}
				else if (character[k] == 32)
				{
					set[k] = FindBestSet(k, num, 1, 2, 3, 4, 5);
					if (set[k] == 1)
					{
						character[k] = 32;
					}
					else if (set[k] == 2)
					{
						character[k] = 47;
					}
					else
					{
						character[k] = 59;
					}
				}
				else if (character[k] == 44)
				{
					set[k] = FindBestSet(k, num, 1, 2);
					if (set[k] == 2)
					{
						character[k] = 48;
					}
				}
				else if (character[k] == 46)
				{
					set[k] = FindBestSet(k, num, 1, 2);
					if (set[k] == 2)
					{
						character[k] = 49;
					}
				}
				else if (character[k] == 47)
				{
					set[k] = FindBestSet(k, num, 1, 2);
					if (set[k] == 2)
					{
						character[k] = 50;
					}
				}
				else if (character[k] == 58)
				{
					set[k] = FindBestSet(k, num, 1, 2);
					if (set[k] == 2)
					{
						character[k] = 51;
					}
				}
			}
			if (set[num - 1] == 5 && character[num - 1] == 4)
			{
				set[num] = 0;
				character[num] = 63;
				for (k = num + 1; k < set.Length; k++)
				{
					set[k] = 0;
					character[k] = 33;
				}
			}
			else
			{
				for (k = num; k < set.Length; k++)
				{
					if (set[num - 1] == 2)
					{
						set[k] = 2;
					}
					else
					{
						set[k] = 1;
					}
					character[k] = 33;
				}
			}
			int num2 = ((mode == StiMaxicodeMode.Mode2 || mode == StiMaxicodeMode.Mode3) ? 9 : 0);
			int num3 = 0;
			for (k = num2; k < 143; k++)
			{
				num3 = ((set[k] == 1 && character[k] >= 48 && character[k] <= 57) ? (num3 + 1) : 0);
				if (num3 == 9)
				{
					set[k] = 6;
					set[k - 1] = 6;
					set[k - 2] = 6;
					set[k - 3] = 6;
					set[k - 4] = 6;
					set[k - 5] = 6;
					set[k - 6] = 6;
					set[k - 7] = 6;
					set[k - 8] = 6;
					num3 = 0;
				}
			}
			int num4 = 1;
			k = 0;
			do
			{
				if (set[k] != num4 && set[k] != 6)
				{
					switch (set[k])
					{
					case 1:
						if (k + 1 < set.Length && set[k + 1] == 1)
						{
							if (k + 2 < set.Length && set[k + 2] == 1)
							{
								if (k + 3 < set.Length && set[k + 3] == 1)
								{
									InsertSymbol(k, 63);
									num4 = 1;
									num++;
									k += 3;
								}
								else
								{
									InsertSymbol(k, 57);
									num++;
									k += 2;
								}
							}
							else
							{
								InsertSymbol(k, 56);
								num++;
								k++;
							}
						}
						else
						{
							InsertSymbol(k, 59);
							num++;
						}
						break;
					case 2:
						if (k + 1 < set.Length && set[k + 1] == 2)
						{
							InsertSymbol(k, 63);
							num4 = 2;
							num++;
							k++;
						}
						else
						{
							InsertSymbol(k, 59);
							num++;
						}
						break;
					case 3:
						if (k + 3 < set.Length && set[k + 1] == 3 && set[k + 2] == 3 && set[k + 3] == 3)
						{
							InsertSymbol(k, 60);
							InsertSymbol(k, 60);
							num4 = 3;
							num++;
							k += 3;
						}
						else
						{
							InsertSymbol(k, 60);
							num++;
						}
						break;
					case 4:
						if (k + 3 < set.Length && set[k + 1] == 4 && set[k + 2] == 4 && set[k + 3] == 4)
						{
							InsertSymbol(k, 61);
							InsertSymbol(k, 61);
							num4 = 4;
							num++;
							k += 3;
						}
						else
						{
							InsertSymbol(k, 61);
							num++;
						}
						break;
					case 5:
						if (k + 3 < set.Length && set[k + 1] == 5 && set[k + 2] == 5 && set[k + 3] == 5)
						{
							InsertSymbol(k, 62);
							InsertSymbol(k, 62);
							num4 = 5;
							num++;
							k += 3;
						}
						else
						{
							InsertSymbol(k, 62);
							num++;
						}
						break;
					default:
						if (set[k] == 0 && (character[k] == 63 || character[k] == 33))
						{
							if (character[k] == 63)
							{
								num4 = ((num4 == 2) ? 1 : 2);
							}
							break;
						}
						throw new StiMaxicodeException("Encoding: unexpected set " + set[k] + " at index " + k + ".");
					}
					k++;
				}
				k++;
			}
			while (k < set.Length);
			k = 0;
			do
			{
				if (set[k] == 6)
				{
					int num5 = 0;
					for (num2 = 0; num2 < 9; num2++)
					{
						num5 *= 10;
						num5 += character[k + num2] - 48;
					}
					character[k] = 31;
					character[k + 1] = (byte)((num5 & 0x3F000000) >> 24);
					character[k + 2] = (byte)((num5 & 0xFC0000) >> 18);
					character[k + 3] = (byte)((num5 & 0x3F000) >> 12);
					character[k + 4] = (byte)((num5 & 0xFC0) >> 6);
					character[k + 5] = (byte)((uint)num5 & 0x3Fu);
					k += 6;
					for (num2 = k; num2 < 140; num2++)
					{
						set[num2] = set[num2 + 3];
						character[num2] = character[num2 + 3];
					}
					num -= 3;
				}
				else
				{
					k++;
				}
			}
			while (k < set.Length);
			if (eciMode != 3)
			{
				InsertSymbol(0, 27);
				InsertSymbol(1, (byte)((uint)eciMode & 0x1Fu));
				num += 2;
			}
			if (!trimExcessData)
			{
				if ((mode == StiMaxicodeMode.Mode2 || mode == StiMaxicodeMode.Mode3) && num > 84)
				{
					return false;
				}
				if ((mode == StiMaxicodeMode.Mode4 || mode == StiMaxicodeMode.Mode6) && num > 93)
				{
					return false;
				}
				if (mode == StiMaxicodeMode.Mode5 && num > 77)
				{
					return false;
				}
			}
			offsetEndOfData = num;
			return true;
		}

		private static byte[] GetErrorCorrectionCodewords(byte[] dataCodewords, int ecclen)
		{
			ReedSolomon reedSolomon = new ReedSolomon(67, ecclen, 1);
			byte[] array = reedSolomon.Encode(dataCodewords.Length, dataCodewords);
			byte[] array2 = new byte[ecclen];
			for (int i = 0; i < ecclen; i++)
			{
				array2[i] = array[ecclen - 1 - i];
			}
			return array2;
		}

		private byte[] GetPrimaryCodewords()
		{
			if (mode == StiMaxicodeMode.Mode2)
			{
				return getMode2PrimaryCodewords(data_Postcode, data_Country, data_Service);
			}
			return getMode3PrimaryCodewords(data_Postcode, data_Country, data_Service);
		}

		private static byte[] getMode2PrimaryCodewords(string postcode, int country, int service)
		{
			int.TryParse(postcode, out var result);
			return new byte[10]
			{
				(byte)((uint)((result & 3) << 4) | 2u),
				(byte)((result & 0xFC) >> 2),
				(byte)((result & 0x3F00) >> 8),
				(byte)((result & 0xFC000) >> 14),
				(byte)((result & 0x3F00000) >> 20),
				(byte)(((result & 0x3C000000) >> 26) | ((postcode.Length & 3) << 4)),
				(byte)(((postcode.Length & 0x3C) >> 2) | ((country & 3) << 4)),
				(byte)((country & 0xFC) >> 2),
				(byte)(((country & 0x300) >> 8) | ((service & 0xF) << 2)),
				(byte)((service & 0x3F0) >> 4)
			};
		}

		private static byte[] getMode3PrimaryCodewords(string postcode, int country, int service)
		{
			if (postcode.Length < 6)
			{
				postcode += new string(' ', 6 - postcode.Length);
			}
			int[] array = new int[postcode.Length];
			postcode = postcode.ToUpperInvariant();
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = postcode[i];
				if (postcode[i] >= 'A' && postcode[i] <= 'Z')
				{
					array[i] -= 64;
				}
				if (array[i] == 27 || array[i] == 31 || array[i] == 33 || array[i] >= 59)
				{
					array[i] = 32;
				}
			}
			return new byte[10]
			{
				(byte)((uint)((array[5] & 3) << 4) | 3u),
				(byte)(((array[4] & 3) << 4) | ((array[5] & 0x3C) >> 2)),
				(byte)(((array[3] & 3) << 4) | ((array[4] & 0x3C) >> 2)),
				(byte)(((array[2] & 3) << 4) | ((array[3] & 0x3C) >> 2)),
				(byte)(((array[1] & 3) << 4) | ((array[2] & 0x3C) >> 2)),
				(byte)(((array[0] & 3) << 4) | ((array[1] & 0x3C) >> 2)),
				(byte)(((array[0] & 0x3C) >> 2) | ((country & 3) << 4)),
				(byte)((country & 0xFC) >> 2),
				(byte)(((country & 0x300) >> 8) | ((service & 0xF) << 2)),
				(byte)((service & 0x3F0) >> 4)
			};
		}

		private void Encode()
		{
			eciMode = GetEciEncoding();
			if (!ProcessTextToSymbols())
			{
				throw new StiMaxicodeException("The input data is too long");
			}
			codewords = new List<byte>();
			codewords.AddRange(character);
			if (mode == StiMaxicodeMode.Mode2 || mode == StiMaxicodeMode.Mode3)
			{
				byte[] primaryCodewords = GetPrimaryCodewords();
				codewords.InsertRange(0, primaryCodewords);
				offsetEndOfData += primaryCodewords.Length;
			}
			else
			{
				codewords.Insert(0, (byte)mode);
				offsetEndOfData++;
			}
			if (structuredAppendTotal > 1)
			{
				byte[] array = new byte[2]
				{
					33,
					(byte)((structuredAppendPosition - 1 << 3) | (structuredAppendTotal - 1))
				};
				int index = ((mode != StiMaxicodeMode.Mode2 && mode != StiMaxicodeMode.Mode3) ? 1 : 10);
				codewords.InsertRange(index, array);
				offsetEndOfData += array.Length;
			}
			int num;
			int num2;
			if (mode == StiMaxicodeMode.Mode5)
			{
				num = 68;
				num2 = 56;
			}
			else
			{
				num = 84;
				num2 = 40;
			}
			int num3 = num + 10;
			if (codewords.Count > num3)
			{
				if (offsetEndOfData > num3 && content.EndsWith("\u0004"))
				{
					codewords[num3 - 1] = codewords[offsetEndOfData - 1];
					codewords[num3 - 2] = codewords[offsetEndOfData - 2];
					if (codewords[offsetEndOfData - 3] == 30)
					{
						codewords[num3 - 3] = codewords[offsetEndOfData - 3];
						if (codewords[offsetEndOfData - 4] == 29)
						{
							codewords[num3 - 4] = codewords[offsetEndOfData - 4];
						}
					}
				}
				codewords.RemoveRange(num3, codewords.Count - num3);
			}
			byte[] array2 = new byte[10];
			Array.Copy(codewords.ToArray(), 0, array2, 0, 10);
			codewords.InsertRange(10, GetErrorCorrectionCodewords(array2, 10));
			byte[] array3 = new byte[codewords.Count - 20];
			Array.Copy(codewords.ToArray(), 20, array3, 0, codewords.Count - 20);
			byte[] array4 = new byte[array3.Length / 2];
			byte[] array5 = new byte[array3.Length / 2];
			for (int i = 0; i < array3.Length; i++)
			{
				if (((uint)i & (true ? 1u : 0u)) != 0)
				{
					array4[(i - 1) / 2] = array3[i];
				}
				else
				{
					array5[i / 2] = array3[i];
				}
			}
			byte[] errorCorrectionCodewords = GetErrorCorrectionCodewords(array4, num2 / 2);
			byte[] errorCorrectionCodewords2 = GetErrorCorrectionCodewords(array5, num2 / 2);
			for (int j = 0; j < errorCorrectionCodewords2.Length; j++)
			{
				codewords.Add(errorCorrectionCodewords2[j]);
				codewords.Add(errorCorrectionCodewords[j]);
			}
			int[] array6 = new int[7];
			for (int k = 0; k < 33; k++)
			{
				for (int l = 0; l < 30; l++)
				{
					int num4 = (Grid_Map[k * 30 + l] + 5) / 6;
					int num5 = (Grid_Map[k * 30 + l] + 5) % 6;
					if (num4 != 0)
					{
						array6[0] = (codewords[num4 - 1] & 0x20) >> 5;
						array6[1] = (codewords[num4 - 1] & 0x10) >> 4;
						array6[2] = (codewords[num4 - 1] & 8) >> 3;
						array6[3] = (codewords[num4 - 1] & 4) >> 2;
						array6[4] = (codewords[num4 - 1] & 2) >> 1;
						array6[5] = codewords[num4 - 1] & 1;
						if (array6[num5] != 0)
						{
							grid[k, l] = true;
						}
						else
						{
							grid[k, l] = false;
						}
					}
				}
			}
			grid[0, 28] = true;
			grid[0, 29] = true;
			grid[9, 10] = true;
			grid[9, 11] = true;
			grid[10, 11] = true;
			grid[15, 7] = true;
			grid[16, 8] = true;
			grid[16, 20] = true;
			grid[17, 20] = true;
			grid[22, 10] = true;
			grid[23, 10] = true;
			grid[22, 17] = true;
			grid[23, 17] = true;
		}

		public StiMaxicode(string data, StiMaxicodeMode mode, int position, int total, bool processTilde, bool trimExcessData)
		{
			try
			{
				if (string.IsNullOrEmpty(data))
				{
					throw new StiMaxicodeException("Empty content!");
				}
				if (position < 1 || position > 8)
				{
					throw new StiMaxicodeException("Invalid structured append position: " + position);
				}
				if (total < 1 || total > 8)
				{
					throw new StiMaxicodeException("Invalid structured append total: " + total);
				}
				this.mode = mode;
				structuredAppendPosition = position;
				structuredAppendTotal = total;
				this.trimExcessData = trimExcessData;
				string text = (processTilde ? UnpackTilde(data) : data);
				if (mode == StiMaxicodeMode.Mode2 || mode == StiMaxicodeMode.Mode3)
				{
					string text2 = null;
					if (text.StartsWith(FormatHeader))
					{
						if (!text.EndsWith(FormatTrailer))
						{
							throw new StiMaxicodeException("Unexpected end of input data");
						}
						text2 = text.Substring(7, 2);
						text = text.Substring(9, text.Length - 11);
					}
					List<string> list = ExtractPrimaryParts(text);
					if (list.Count < 4)
					{
						throw new StiMaxicodeException("Invalid message format");
					}
					data_Postcode = list[0].Trim();
					if (!int.TryParse(list[1].Trim(), out data_Country))
					{
						throw new StiMaxicodeException("Invalid country data");
					}
					if (!int.TryParse(list[2].Trim(), out data_Service))
					{
						throw new StiMaxicodeException("Invalid service data");
					}
					content = list[3].Trim();
					if (text2 != null)
					{
						content = FormatHeader + text2 + content + FormatTrailer;
					}
					if (mode == StiMaxicodeMode.Mode2)
					{
						for (int i = 0; i < data_Postcode.Length; i++)
						{
							if (data_Postcode[i] < '0' || data_Postcode[i] > '9')
							{
								this.mode = StiMaxicodeMode.Mode3;
								break;
							}
						}
					}
				}
				else
				{
					content = text;
				}
				Encode();
			}
			catch (StiMaxicodeException ex)
			{
				errorMessage = ex.Message;
			}
		}
	}

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiMaxicodeBarCodeType;

	public override string ServiceName => "Maxicode";

	public override string DefaultCodeValue => "ABC abc 123";

	[Description("Gets or sets width of the most fine element of the bar code.")]
	[DefaultValue(10f)]
	[Browsable(false)]
	public override float Module
	{
		get
		{
			return 10f;
		}
		set
		{
		}
	}

	[Description("Gets os sets height factor of the bar code.")]
	[DefaultValue(1f)]
	[Browsable(false)]
	public override float Height
	{
		get
		{
			return 1f;
		}
		set
		{
		}
	}

	[Description("Gets or sets the mode of the Maxicode.")]
	[DefaultValue(StiMaxicodeMode.Mode4)]
	[StiOrder(100)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiMaxicodeMode Mode { get; set; } = StiMaxicodeMode.Mode4;


	[Description("Gets or sets the flag that indicates whether the data message supports character '~' as the escape.")]
	[DefaultValue(true)]
	[StiOrder(110)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool ProcessTilde { get; set; } = true;


	[Description("Gets or sets the structured append Position.")]
	[DefaultValue(1)]
	[StiOrder(140)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public int StructuredAppendPosition { get; set; } = 1;


	[Description("Gets or sets the structured append Total.")]
	[DefaultValue(1)]
	[StiOrder(150)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public int StructuredAppendTotal { get; set; } = 1;


	[Description("Gets or sets a value indicating whether it is necessary to trim excess data if the data length exceeds the capacity of the barcode.")]
	[DefaultValue(true)]
	[StiOrder(120)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool TrimExcessData { get; set; } = true;


	internal override float LabelFontHeight => 8f;

	public override bool[] VisibleProperties => new bool[27]
	{
		false, false, false, false, false, false, false, false, false, true,
		false, false, true, true, false, false, false, false, false, false,
		false, true, false, false, false, false, false
	};

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "Mode", (Enum)Mode, (Enum)StiMaxicodeMode.Mode4);
		JsonExtensions.AddPropertyInt(val, "StructuredAppendPosition", StructuredAppendPosition, 1);
		JsonExtensions.AddPropertyInt(val, "StructuredAppendTotal", StructuredAppendTotal, 1);
		JsonExtensions.AddPropertyBool(val, "ProcessTilde", ProcessTilde, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Mode":
				Mode = StiJsonReportObjectHelper.DeserializeEnum<StiMaxicodeMode>(item);
				break;
			case "StructuredAppendPosition":
				StructuredAppendPosition = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "StructuredAppendTotal":
				StructuredAppendTotal = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "ProcessTilde":
				ProcessTilde = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[5]
		{
			propertiesHelper.Mode(),
			propertiesHelper.ProcessTilde(),
			propertiesHelper.TrimExcessData(),
			propertiesHelper.StructuredAppendPosition(),
			propertiesHelper.StructuredAppendTotal()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		base.BarCodeData.Code = code;
		StiMaxicode stiMaxicode = new StiMaxicode(code, Mode, StructuredAppendPosition, StructuredAppendTotal, ProcessTilde, TrimExcessData);
		base.BarCodeData.MatrixGrid = stiMaxicode.Matrix;
		if (stiMaxicode.ErrorMessage == null)
		{
			DrawMaxicode(context, rect, barCode, zoom);
		}
		else
		{
			DrawBarCodeError(context, rect, barCode, stiMaxicode.ErrorMessage);
		}
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiMaxicodeBarCodeType();
	}

	public StiMaxicodeBarCodeType()
		: this(StiMaxicodeMode.Mode4, 1, 1, processTilde: true, trimExcessData: true)
	{
	}

	public StiMaxicodeBarCodeType(StiMaxicodeMode mode, int structuredAppendPosition, int structuredAppendTotal, bool processTilde)
		: this(mode, structuredAppendPosition, structuredAppendTotal, processTilde, trimExcessData: true)
	{
	}

	public StiMaxicodeBarCodeType(StiMaxicodeMode mode, int structuredAppendPosition, int structuredAppendTotal, bool processTilde, bool trimExcessData)
	{
		Mode = mode;
		StructuredAppendPosition = structuredAppendPosition;
		StructuredAppendTotal = structuredAppendTotal;
		ProcessTilde = processTilde;
		TrimExcessData = trimExcessData;
	}
}

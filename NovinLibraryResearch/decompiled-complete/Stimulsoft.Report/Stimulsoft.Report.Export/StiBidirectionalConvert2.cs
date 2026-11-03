using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Stimulsoft.Report.Export;

public static class StiBidirectionalConvert2
{
	private struct DirectionalStatus
	{
		internal byte paragraphEmbeddingLevel;

		internal byte directionalOverrideStatus;

		internal bool directionalIsolateStatus;
	}

	private class IsolatingRunSequence
	{
		public byte level;

		public BidiClass sos;

		public BidiClass eos;

		public int length;

		public int[] indexes;

		public byte[] types;

		public byte[] resolvedLevels;

		public IsolatingRunSequence(byte paragraphEmbeddingLevel, List<int> runIndexList, byte[] types, byte[] levels)
		{
			this.ComputeIsolatingRunSequence(paragraphEmbeddingLevel, runIndexList, types, levels);
		}
	}

	private enum BidiClass : byte
	{
		L,
		LRE,
		LRO,
		R,
		AL,
		RLE,
		RLO,
		PDF,
		EN,
		ES,
		ET,
		AN,
		CS,
		NSM,
		BN,
		B,
		S,
		WS,
		ON,
		LRI,
		RLI,
		FSI,
		PDI
	}

	private static class Bidi_Types
	{
		private const int arabicTableSize = 97;

		private const int ligaturesTableSize = 40;

		internal static byte[] BidiCharTypes;

		internal static ushort[] MirrorChars;

		internal static ushort[,] ArabicTable;

		internal static string[,] LigaturesTable;

		internal static ushort GetArabicTableValue(int sym, int column)
		{
			int num = 0;
			if ((sym & 0xFF00) == 1280 || (sym & 0xFF00) == 1536)
			{
				num = sym - 1280;
			}
			if ((sym & 0xFF00) == 64256)
			{
				num = sym - 63744;
			}
			return ArabicTable[num, column];
		}

		static Bidi_Types()
		{
			byte[] array = Convert.FromBase64String("eJzt3Yt6myAYgOF/022Rimm4/4udiijgATVq0vZ722XK6QdE6+qepHI+75+PexW43+s0Y5RSplGURVneemWbaGTWmvwuUDWrbMK38evyVdOZ263ZNDdZan4h7Kp8vLv+0JvAeN8vEmXW9MWCrovr3tXr8eJwy7yZ+cm6Y6K3yOov++1ayeYN+V7Uj1qzAnOl8jIPzodcJM/r77WmOti2b6mPj6ZQfoUu+ocbSZ1g7Mbw82uqnlSpDup5smGuNvKi5BLI1uubyLLmyuevgh/siQtPW3M4+jbBbQ+llgPsDz/EDxJGpaJUpRazN+jiu1fbkURzozLPx2/iDvM/LhWmhqOXJ+dfT8//lgE+0QGJaq9ryt74qCPuAdzY7SHoR6rT43c3ZM+IG56OFKWaI8bt2Xz+nWWYfC0nhtTi37C5s8m/Cp6rjx+vsmPDL7bVTLDeuXx1ILh8+dlRuWlDo3KJNmYX2l7xF4p2FcYZUZrXzkXDAC6yeUUPv5kQ4Nt7zGe96BTQevmkTecvZifzV4luAIabMi9VTV194uvLsN/8gmThaLzaEbO2g3Y3MkNKkNFMo3nba7VJmqmoXzTdlxvdjq7Q37V3eyvF52eXKAm2hl4d5jxD/9+hN2jodWZrC76u9nmmvx9dz9P78hhUVSXZ3M+Jx/3Xn3+//5bKPY73lGbWo32I3wazz/Nv9ZZ9/lIUdb4Mz2PsfkxNklmpEyFe8N0z4eBn+DCtdhBivyT85bEtYgu4La9SRzYyL1fEhxcjcoSF5s3htnciMFfdr5/KX3SbIIcy+HLCozY6hOJddffqG15eIWuWj7iGpC++b9nJfLUoIzj/lurNkR/OAEdx60nwVszEUTFTid9P6p9DZis51kIgc5Z18b8ymRWXenQbZk5fs1svXSuuMTlLszRFzvyfyic2fR2zguy1o+qW2Lu6ttB+Kt4buairySCpfAAAAAAAAAAAAFzMPEkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+C2UdwgPMncvgIWek/4vWlTJJc4qIwE3T3x75MF9GHfmCpOXhW5zt+FPdp2ckCS2W+Njc498fLGW0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACslemsV2Q+6V4l676d/HSpTkuOq4zfxzR/0t4WZKWgkgneuLQVv+9t/KamJqE0pdQvjjKmKOq/xShl+vg7x3g4kaoekKq71ijKoixvvW4QMmtdfqLQ2dqxKSWjjtjd/2GsGxo=");
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(array, 2, array.Length - 2);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress);
			BidiCharTypes = new byte[65535];
			deflateStream.Read(BidiCharTypes, 0, 65535);
			deflateStream.Close();
			ushort[,] array2 = new ushort[97, 6]
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
			ArabicTable = new ushort[768, 5];
			for (int i = 0; i < 97; i++)
			{
				uint num = (uint)(array2[i, 0] - 1280);
				ArabicTable[num, 0] = array2[i, 1];
				ArabicTable[num, 1] = array2[i, 2];
				ArabicTable[num, 2] = array2[i, 3];
				ArabicTable[num, 3] = array2[i, 4];
				ArabicTable[num, 4] = array2[i, 5];
			}
			for (ushort num2 = 1424; num2 <= 1535; num2++)
			{
				ArabicTable[num2 - 1280, 0] = 1;
				ArabicTable[num2 - 1280, 1] = num2;
			}
			for (ushort num3 = 64285; num3 <= 64335; num3++)
			{
				ArabicTable[num3 - 63744, 0] = 1;
				ArabicTable[num3 - 63744, 1] = num3;
			}
			LigaturesTable = new string[40, 2]
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
			MirrorChars = new ushort[65536];
			MirrorChars[40] = 41;
			MirrorChars[41] = 40;
			MirrorChars[60] = 62;
			MirrorChars[62] = 60;
			MirrorChars[91] = 93;
			MirrorChars[93] = 91;
			MirrorChars[123] = 125;
			MirrorChars[125] = 123;
			MirrorChars[171] = 187;
			MirrorChars[187] = 171;
			MirrorChars[8249] = 8250;
			MirrorChars[8250] = 8249;
			MirrorChars[8261] = 8262;
			MirrorChars[8262] = 8261;
			MirrorChars[8317] = 8318;
			MirrorChars[8318] = 8317;
			MirrorChars[8333] = 8334;
			MirrorChars[8334] = 8333;
			MirrorChars[8712] = 8715;
			MirrorChars[8713] = 8716;
			MirrorChars[8714] = 8717;
			MirrorChars[8715] = 8712;
			MirrorChars[8716] = 8713;
			MirrorChars[8717] = 8714;
			MirrorChars[8725] = 10741;
			MirrorChars[8764] = 8765;
			MirrorChars[8765] = 8764;
			MirrorChars[8771] = 8909;
			MirrorChars[8786] = 8787;
			MirrorChars[8787] = 8786;
			MirrorChars[8788] = 8789;
			MirrorChars[8789] = 8788;
			MirrorChars[8804] = 8805;
			MirrorChars[8805] = 8804;
			MirrorChars[8806] = 8807;
			MirrorChars[8807] = 8806;
			MirrorChars[8808] = 8809;
			MirrorChars[8809] = 8808;
			MirrorChars[8810] = 8811;
			MirrorChars[8811] = 8810;
			MirrorChars[8814] = 8815;
			MirrorChars[8815] = 8814;
			MirrorChars[8816] = 8817;
			MirrorChars[8817] = 8816;
			MirrorChars[8818] = 8819;
			MirrorChars[8819] = 8818;
			MirrorChars[8820] = 8821;
			MirrorChars[8821] = 8820;
			MirrorChars[8822] = 8823;
			MirrorChars[8823] = 8822;
			MirrorChars[8824] = 8825;
			MirrorChars[8825] = 8824;
			MirrorChars[8826] = 8827;
			MirrorChars[8827] = 8826;
			MirrorChars[8828] = 8829;
			MirrorChars[8829] = 8828;
			MirrorChars[8830] = 8831;
			MirrorChars[8831] = 8830;
			MirrorChars[8832] = 8833;
			MirrorChars[8833] = 8832;
			MirrorChars[8834] = 8835;
			MirrorChars[8835] = 8834;
			MirrorChars[8836] = 8837;
			MirrorChars[8837] = 8836;
			MirrorChars[8838] = 8839;
			MirrorChars[8839] = 8838;
			MirrorChars[8840] = 8841;
			MirrorChars[8841] = 8840;
			MirrorChars[8842] = 8843;
			MirrorChars[8843] = 8842;
			MirrorChars[8847] = 8848;
			MirrorChars[8848] = 8847;
			MirrorChars[8849] = 8850;
			MirrorChars[8850] = 8849;
			MirrorChars[8856] = 10680;
			MirrorChars[8866] = 8867;
			MirrorChars[8867] = 8866;
			MirrorChars[8870] = 10974;
			MirrorChars[8872] = 10980;
			MirrorChars[8873] = 10979;
			MirrorChars[8875] = 10981;
			MirrorChars[8880] = 8881;
			MirrorChars[8881] = 8880;
			MirrorChars[8882] = 8883;
			MirrorChars[8883] = 8882;
			MirrorChars[8884] = 8885;
			MirrorChars[8885] = 8884;
			MirrorChars[8886] = 8887;
			MirrorChars[8887] = 8886;
			MirrorChars[8905] = 8906;
			MirrorChars[8906] = 8905;
			MirrorChars[8907] = 8908;
			MirrorChars[8908] = 8907;
			MirrorChars[8909] = 8771;
			MirrorChars[8912] = 8913;
			MirrorChars[8913] = 8912;
			MirrorChars[8918] = 8919;
			MirrorChars[8919] = 8918;
			MirrorChars[8920] = 8921;
			MirrorChars[8921] = 8920;
			MirrorChars[8922] = 8923;
			MirrorChars[8923] = 8922;
			MirrorChars[8924] = 8925;
			MirrorChars[8925] = 8924;
			MirrorChars[8926] = 8927;
			MirrorChars[8927] = 8926;
			MirrorChars[8928] = 8929;
			MirrorChars[8929] = 8928;
			MirrorChars[8930] = 8931;
			MirrorChars[8931] = 8930;
			MirrorChars[8932] = 8933;
			MirrorChars[8933] = 8932;
			MirrorChars[8934] = 8935;
			MirrorChars[8935] = 8934;
			MirrorChars[8936] = 8937;
			MirrorChars[8937] = 8936;
			MirrorChars[8938] = 8939;
			MirrorChars[8939] = 8938;
			MirrorChars[8940] = 8941;
			MirrorChars[8941] = 8940;
			MirrorChars[8944] = 8945;
			MirrorChars[8945] = 8944;
			MirrorChars[8946] = 8954;
			MirrorChars[8947] = 8955;
			MirrorChars[8948] = 8956;
			MirrorChars[8950] = 8957;
			MirrorChars[8951] = 8958;
			MirrorChars[8954] = 8946;
			MirrorChars[8955] = 8947;
			MirrorChars[8956] = 8948;
			MirrorChars[8957] = 8950;
			MirrorChars[8958] = 8951;
			MirrorChars[8968] = 8969;
			MirrorChars[8969] = 8968;
			MirrorChars[8970] = 8971;
			MirrorChars[8971] = 8970;
			MirrorChars[9001] = 9002;
			MirrorChars[9002] = 9001;
			MirrorChars[10088] = 10089;
			MirrorChars[10089] = 10088;
			MirrorChars[10090] = 10091;
			MirrorChars[10091] = 10090;
			MirrorChars[10092] = 10093;
			MirrorChars[10093] = 10092;
			MirrorChars[10094] = 10095;
			MirrorChars[10095] = 10094;
			MirrorChars[10096] = 10097;
			MirrorChars[10097] = 10096;
			MirrorChars[10098] = 10099;
			MirrorChars[10099] = 10098;
			MirrorChars[10100] = 10101;
			MirrorChars[10101] = 10100;
			MirrorChars[10197] = 10198;
			MirrorChars[10198] = 10197;
			MirrorChars[10205] = 10206;
			MirrorChars[10206] = 10205;
			MirrorChars[10210] = 10211;
			MirrorChars[10211] = 10210;
			MirrorChars[10212] = 10213;
			MirrorChars[10213] = 10212;
			MirrorChars[10214] = 10215;
			MirrorChars[10215] = 10214;
			MirrorChars[10216] = 10217;
			MirrorChars[10217] = 10216;
			MirrorChars[10218] = 10219;
			MirrorChars[10219] = 10218;
			MirrorChars[10627] = 10628;
			MirrorChars[10628] = 10627;
			MirrorChars[10629] = 10630;
			MirrorChars[10630] = 10629;
			MirrorChars[10631] = 10632;
			MirrorChars[10632] = 10631;
			MirrorChars[10633] = 10634;
			MirrorChars[10634] = 10633;
			MirrorChars[10635] = 10636;
			MirrorChars[10636] = 10635;
			MirrorChars[10637] = 10640;
			MirrorChars[10638] = 10639;
			MirrorChars[10639] = 10638;
			MirrorChars[10640] = 10637;
			MirrorChars[10641] = 10642;
			MirrorChars[10642] = 10641;
			MirrorChars[10643] = 10644;
			MirrorChars[10644] = 10643;
			MirrorChars[10645] = 10646;
			MirrorChars[10646] = 10645;
			MirrorChars[10647] = 10648;
			MirrorChars[10648] = 10647;
			MirrorChars[10680] = 8856;
			MirrorChars[10688] = 10689;
			MirrorChars[10689] = 10688;
			MirrorChars[10692] = 10693;
			MirrorChars[10693] = 10692;
			MirrorChars[10703] = 10704;
			MirrorChars[10704] = 10703;
			MirrorChars[10705] = 10706;
			MirrorChars[10706] = 10705;
			MirrorChars[10708] = 10709;
			MirrorChars[10709] = 10708;
			MirrorChars[10712] = 10713;
			MirrorChars[10713] = 10712;
			MirrorChars[10714] = 10715;
			MirrorChars[10715] = 10714;
			MirrorChars[10741] = 8725;
			MirrorChars[10744] = 10745;
			MirrorChars[10745] = 10744;
			MirrorChars[10748] = 10749;
			MirrorChars[10749] = 10748;
			MirrorChars[10795] = 10796;
			MirrorChars[10796] = 10795;
			MirrorChars[10797] = 10796;
			MirrorChars[10798] = 10797;
			MirrorChars[10804] = 10805;
			MirrorChars[10805] = 10804;
			MirrorChars[10812] = 10813;
			MirrorChars[10813] = 10812;
			MirrorChars[10852] = 10853;
			MirrorChars[10853] = 10852;
			MirrorChars[10873] = 10874;
			MirrorChars[10874] = 10873;
			MirrorChars[10877] = 10878;
			MirrorChars[10878] = 10877;
			MirrorChars[10879] = 10880;
			MirrorChars[10880] = 10879;
			MirrorChars[10881] = 10882;
			MirrorChars[10882] = 10881;
			MirrorChars[10883] = 10884;
			MirrorChars[10884] = 10883;
			MirrorChars[10891] = 10892;
			MirrorChars[10892] = 10891;
			MirrorChars[10897] = 10898;
			MirrorChars[10898] = 10897;
			MirrorChars[10899] = 10900;
			MirrorChars[10900] = 10899;
			MirrorChars[10901] = 10902;
			MirrorChars[10902] = 10901;
			MirrorChars[10903] = 10904;
			MirrorChars[10904] = 10903;
			MirrorChars[10905] = 10906;
			MirrorChars[10906] = 10905;
			MirrorChars[10907] = 10908;
			MirrorChars[10908] = 10907;
			MirrorChars[10913] = 10914;
			MirrorChars[10914] = 10913;
			MirrorChars[10918] = 10919;
			MirrorChars[10919] = 10918;
			MirrorChars[10920] = 10921;
			MirrorChars[10921] = 10920;
			MirrorChars[10922] = 10923;
			MirrorChars[10923] = 10922;
			MirrorChars[10924] = 10925;
			MirrorChars[10925] = 10924;
			MirrorChars[10927] = 10928;
			MirrorChars[10928] = 10927;
			MirrorChars[10931] = 10932;
			MirrorChars[10932] = 10931;
			MirrorChars[10939] = 10940;
			MirrorChars[10940] = 10939;
			MirrorChars[10941] = 10942;
			MirrorChars[10942] = 10941;
			MirrorChars[10943] = 10944;
			MirrorChars[10944] = 10943;
			MirrorChars[10945] = 10946;
			MirrorChars[10946] = 10945;
			MirrorChars[10947] = 10948;
			MirrorChars[10948] = 10947;
			MirrorChars[10949] = 10950;
			MirrorChars[10950] = 10949;
			MirrorChars[10957] = 10958;
			MirrorChars[10958] = 10957;
			MirrorChars[10959] = 10960;
			MirrorChars[10960] = 10959;
			MirrorChars[10961] = 10962;
			MirrorChars[10962] = 10961;
			MirrorChars[10963] = 10964;
			MirrorChars[10964] = 10963;
			MirrorChars[10965] = 10966;
			MirrorChars[10966] = 10965;
			MirrorChars[10974] = 8870;
			MirrorChars[10979] = 8873;
			MirrorChars[10980] = 8872;
			MirrorChars[10981] = 8875;
			MirrorChars[10988] = 10989;
			MirrorChars[10989] = 10988;
			MirrorChars[10999] = 11000;
			MirrorChars[11000] = 10999;
			MirrorChars[11001] = 11002;
			MirrorChars[11002] = 11001;
			MirrorChars[12296] = 12297;
			MirrorChars[12297] = 12296;
			MirrorChars[12298] = 12299;
			MirrorChars[12299] = 12298;
			MirrorChars[12300] = 12301;
			MirrorChars[12301] = 12300;
			MirrorChars[12302] = 12303;
			MirrorChars[12303] = 12302;
			MirrorChars[12304] = 12305;
			MirrorChars[12305] = 12304;
			MirrorChars[12308] = 12309;
			MirrorChars[12309] = 12308;
			MirrorChars[12310] = 12311;
			MirrorChars[12311] = 12310;
			MirrorChars[12312] = 12313;
			MirrorChars[12313] = 12312;
			MirrorChars[12314] = 12315;
			MirrorChars[12315] = 12314;
			MirrorChars[65288] = 65289;
			MirrorChars[65289] = 65288;
			MirrorChars[65308] = 65310;
			MirrorChars[65310] = 65308;
			MirrorChars[65339] = 65341;
			MirrorChars[65341] = 65339;
			MirrorChars[65371] = 65373;
			MirrorChars[65373] = 65371;
			MirrorChars[65375] = 65376;
			MirrorChars[65376] = 65375;
			MirrorChars[65378] = 65379;
			MirrorChars[65379] = 65378;
		}
	}

	private const int MAX_DEPTH = 125;

	private const int MAX_NESTED_BRACKET_PAIRS = 63;

	private const string LeftBracketsList = "([{༺༼᚛⁅⁽₍⌈⌊〈❨❪❬❮❰❲❴⟅⟦⟨⟪⟬⟮⦃⦅⦇⦉⦋⦍⦏⦑⦓⦕⦗⧘⧚⧼⸢⸤⸦⸨〈《「『【〔〖〘〚﹙﹛﹝（［｛｟｢";

	private const string RightBracketsList = ")]}༻༽᚜⁆⁾₎⌉⌋〉❩❫❭❯❱❳❵⟆⟧⟩⟫⟭⟯⦄⦆⦈⦊⦌⦎⦐⦒⦔⦖⦘⧙⧛⧽⸣⸥⸧⸩〉》」』】〕〗〙〛﹚﹜﹞）］｝｠｣";

	public static StringBuilder ConvertStringBuilder(StringBuilder input, bool rightToLeft, bool modePdf = true)
	{
		return new StringBuilder(ConvertString(input.ToString(), rightToLeft, modePdf));
	}

	public static string ConvertString(string input, bool rightToLeft, bool modePdf = true)
	{
		if (input == null)
		{
			return string.Empty;
		}
		if (string.IsNullOrWhiteSpace(input))
		{
			return input;
		}
		input = ConvertArabicHebrew(input).ToString();
		string text = LogicalToVisual(input, rightToLeft);
		if (modePdf && StiOptions.Export.Pdf.ConvertDigitsToArabic && rightToLeft)
		{
			text = StiExportUtils.ConvertDigitsToArabic(text, StiOptions.Export.Pdf.ArabicDigitsType);
		}
		return text;
	}

	private static string LogicalToVisual(string input, bool rightToLeft, int[] lineBreaks = null)
	{
		bool flag = rightToLeft;
		if (!flag)
		{
			foreach (char c in input)
			{
				BidiClass bidiClass = (BidiClass)Bidi_Types.BidiCharTypes[(uint)c];
				if (bidiClass == BidiClass.R || bidiClass == BidiClass.AL || bidiClass == BidiClass.AN)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			return input;
		}
		int length = input.Length;
		byte[] typesList = new byte[input.Length];
		byte[] levels = new byte[input.Length];
		ClassifyCharacters(input, ref typesList);
		GetMatchingPDI(typesList, out var outMatchingPDI, out var outMatchingIsolateInitiator);
		byte b = GetParagraphEmbeddingLevel(typesList, outMatchingPDI);
		if (rightToLeft)
		{
			b = 1;
		}
		SetLevels(ref levels, b);
		GetExplicitEmbeddingLevels(b, typesList, ref levels, outMatchingPDI);
		RemoveX9Characters(ref typesList);
		List<List<int>> levelRuns = GetLevelRuns(levels);
		int[] runForCharacter = GetRunForCharacter(levelRuns, length);
		List<IsolatingRunSequence> isolatingRunSequences = GetIsolatingRunSequences(b, typesList, levels, levelRuns, outMatchingIsolateInitiator, outMatchingPDI, runForCharacter);
		foreach (IsolatingRunSequence item in isolatingRunSequences)
		{
			item.ResolveWeaks();
			item.ResolveNeutrals(input);
			item.ResolveImplicit();
			item.ApplyTypesAndLevels(ref typesList, ref levels);
		}
		int[] lineBreaks2 = ((lineBreaks != null) ? lineBreaks : new int[1] { typesList.Length });
		byte[] levels2;
		int[] reorderedIndexes = GetReorderedIndexes(b, typesList, levels, lineBreaks2, out levels2);
		return GetOrderedString(input, reorderedIndexes, levels2);
	}

	public static StringBuilder ConvertArabicHebrew(StringBuilder inputSB)
	{
		return ConvertArabicHebrew(inputSB.ToString());
	}

	public static StringBuilder ConvertArabicHebrew(string inputSB)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (inputSB == null)
		{
			return stringBuilder;
		}
		bool flag = false;
		bool flag2 = false;
		foreach (char c in inputSB)
		{
			byte b = (byte)((uint)((int)c >> 8) & 0xFFu);
			if (b == 5 || b == 6 || b == 251)
			{
				flag = true;
			}
			if (b == 5 || b == 251 || b == 254)
			{
				flag2 = true;
			}
		}
		if (flag)
		{
			for (int j = 0; j < inputSB.Length; j++)
			{
				switch (Bidi_Types.GetArabicTableValue(inputSB[j], 0))
				{
				case 3:
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.Append(inputSB[j]);
					while (j + 1 < inputSB.Length && Bidi_Types.GetArabicTableValue(inputSB[j + 1], 0) == 3)
					{
						j++;
						stringBuilder2.Append(inputSB[j]);
					}
					if (j + 1 < inputSB.Length && Bidi_Types.GetArabicTableValue(inputSB[j + 1], 0) == 2)
					{
						j++;
						stringBuilder2.Append(inputSB[j]);
					}
					if (stringBuilder2.Length == 1)
					{
						stringBuilder.Append((char)Bidi_Types.GetArabicTableValue(stringBuilder2[0], 1));
						break;
					}
					stringBuilder.Append((char)Bidi_Types.GetArabicTableValue(stringBuilder2[0], 3));
					if (stringBuilder2.Length > 2)
					{
						for (int k = 1; k < stringBuilder2.Length - 1; k++)
						{
							stringBuilder.Append((char)Bidi_Types.GetArabicTableValue(stringBuilder2[k], 4));
						}
					}
					stringBuilder.Append((char)Bidi_Types.GetArabicTableValue(stringBuilder2[stringBuilder2.Length - 1], 2));
					break;
				}
				case 1:
				case 2:
					stringBuilder.Append((char)Bidi_Types.GetArabicTableValue(inputSB[j], 1));
					break;
				default:
					stringBuilder.Append(inputSB[j]);
					break;
				}
			}
		}
		else
		{
			stringBuilder.Append(inputSB);
		}
		if (flag2)
		{
			for (int l = 0; l < Bidi_Types.LigaturesTable.GetLength(0); l++)
			{
				stringBuilder = stringBuilder.Replace(Bidi_Types.LigaturesTable[l, 0], Bidi_Types.LigaturesTable[l, 1]);
			}
		}
		return stringBuilder;
	}

	private static void ClassifyCharacters(string text, ref byte[] typesList)
	{
		typesList = new byte[text.Length];
		for (int i = 0; i < text.Length; i++)
		{
			int num = Convert.ToInt32(text[i]);
			typesList[i] = Bidi_Types.BidiCharTypes[num];
		}
	}

	private static byte GetParagraphEmbeddingLevel(byte[] types, int[] matchingPDI, int si = -1, int ei = -1)
	{
		int num = ((si != -1) ? si : 0);
		int num2 = ((ei != -1) ? ei : types.Length);
		for (int i = num; i < num2; i++)
		{
			BidiClass bidiClass = (BidiClass)types[i];
			switch (bidiClass)
			{
			case BidiClass.L:
			case BidiClass.R:
			case BidiClass.AL:
				if (bidiClass == BidiClass.L)
				{
					return 0;
				}
				return 1;
			case BidiClass.LRI:
			case BidiClass.RLI:
			case BidiClass.FSI:
				i = matchingPDI[i];
				break;
			}
		}
		return 0;
	}

	private static void GetExplicitEmbeddingLevels(byte level, byte[] types, ref byte[] levels, int[] matchingPDI)
	{
		Stack<DirectionalStatus> stack = new Stack<DirectionalStatus>(127);
		DirectionalStatus directionalStatus = default(DirectionalStatus);
		directionalStatus.paragraphEmbeddingLevel = level;
		directionalStatus.directionalOverrideStatus = 18;
		directionalStatus.directionalIsolateStatus = false;
		DirectionalStatus item = directionalStatus;
		stack.Push(item);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < types.Length; i++)
		{
			BidiClass bidiClass = (BidiClass)types[i];
			switch (bidiClass)
			{
			case BidiClass.LRE:
			case BidiClass.LRO:
			case BidiClass.RLE:
			case BidiClass.RLO:
			case BidiClass.LRI:
			case BidiClass.RLI:
			case BidiClass.FSI:
			{
				bool flag = bidiClass == BidiClass.RLI || bidiClass == BidiClass.LRI;
				if (flag)
				{
					levels[i] = stack.Peek().paragraphEmbeddingLevel;
				}
				if (bidiClass == BidiClass.FSI)
				{
					byte paragraphEmbeddingLevel = GetParagraphEmbeddingLevel(types, matchingPDI, i + 1, matchingPDI[i]);
					bidiClass = ((paragraphEmbeddingLevel == 1) ? BidiClass.RLI : BidiClass.LRI);
				}
				byte b = ((bidiClass != BidiClass.RLE && bidiClass != BidiClass.RLO && bidiClass != BidiClass.RLI) ? ((byte)LeastGreaterEven(stack.Peek().paragraphEmbeddingLevel)) : ((byte)LeastGreaterOdd(stack.Peek().paragraphEmbeddingLevel)));
				if (b <= 125 && num == 0 && num2 == 0)
				{
					if (flag)
					{
						num3++;
					}
					byte directionalOverrideStatus = bidiClass switch
					{
						BidiClass.LRO => 0, 
						BidiClass.RLO => 3, 
						_ => 18, 
					};
					stack.Push(new DirectionalStatus
					{
						paragraphEmbeddingLevel = b,
						directionalOverrideStatus = directionalOverrideStatus,
						directionalIsolateStatus = flag
					});
				}
				else if (num == 0)
				{
					num2++;
				}
				break;
			}
			case BidiClass.PDI:
				if (num > 0)
				{
					num--;
				}
				else if (num3 != 0)
				{
					num2 = 0;
					while (!stack.Peek().directionalIsolateStatus)
					{
						stack.Pop();
					}
					stack.Pop();
					num3--;
				}
				levels[i] = stack.Peek().paragraphEmbeddingLevel;
				break;
			case BidiClass.PDF:
				if (num <= 0)
				{
					if (num2 > 0)
					{
						num2--;
					}
					else if (!stack.Peek().directionalIsolateStatus && stack.Count > 1)
					{
						stack.Pop();
					}
				}
				break;
			case BidiClass.B:
				num2 = 0;
				num = 0;
				num3 = 0;
				stack.Clear();
				stack.Push(item);
				levels[i] = level;
				break;
			default:
				levels[i] = stack.Peek().paragraphEmbeddingLevel;
				if (stack.Peek().directionalOverrideStatus != 18)
				{
					types[i] = stack.Peek().directionalOverrideStatus;
				}
				break;
			}
		}
	}

	private static void ResolveWeaks(this IsolatingRunSequence sequence)
	{
		for (int i = 0; i < sequence.length; i++)
		{
			BidiClass bidiClass = (BidiClass)sequence.types[i];
			BidiClass bidiClass2 = ((i == 0) ? sequence.sos : ((BidiClass)sequence.types[i - 1]));
			if (bidiClass == BidiClass.NSM)
			{
				bool flag = bidiClass2 == BidiClass.LRI || bidiClass2 == BidiClass.RLI || bidiClass2 == BidiClass.FSI || bidiClass2 == BidiClass.PDI;
				sequence.types[i] = (byte)(flag ? BidiClass.ON : bidiClass2);
			}
		}
		for (int j = 0; j < sequence.length; j++)
		{
			BidiClass bidiClass3 = (BidiClass)sequence.types[j];
			if (bidiClass3 != BidiClass.EN)
			{
				continue;
			}
			for (int num = j - 1; num >= 0; num--)
			{
				BidiClass bidiClass4 = (BidiClass)sequence.types[num];
				if ((bidiClass4 == BidiClass.R || bidiClass4 == BidiClass.AL || bidiClass4 == BidiClass.L) && bidiClass4 == BidiClass.AL)
				{
					sequence.types[j] = 11;
					break;
				}
			}
		}
		for (int k = 0; k < sequence.length; k++)
		{
			if (sequence.types[k] == 4)
			{
				sequence.types[k] = 3;
			}
		}
		for (int l = 1; l < sequence.length - 1; l++)
		{
			BidiClass bidiClass5 = (BidiClass)sequence.types[l];
			BidiClass bidiClass6 = (BidiClass)sequence.types[l - 1];
			BidiClass bidiClass7 = (BidiClass)sequence.types[l + 1];
			if (bidiClass5 == BidiClass.ES && bidiClass6 == BidiClass.EN && bidiClass7 == BidiClass.EN)
			{
				sequence.types[l] = 8;
			}
			else if (bidiClass5 == BidiClass.CS && ((bidiClass6 == BidiClass.EN && bidiClass7 == BidiClass.EN) || (bidiClass6 == BidiClass.AN && bidiClass7 == BidiClass.AN)))
			{
				sequence.types[l] = (byte)bidiClass6;
			}
		}
		BidiClass[] typesSet = new BidiClass[1] { BidiClass.ET };
		for (int m = 0; m < sequence.length; m++)
		{
			if (sequence.types[m] == 10)
			{
				int num2 = m;
				int runLimit = sequence.GetRunLimit(num2, sequence.length, typesSet);
				BidiClass bidiClass8 = ((num2 > 0) ? ((BidiClass)sequence.types[num2 - 1]) : sequence.sos);
				if (bidiClass8 != BidiClass.EN)
				{
					bidiClass8 = ((runLimit < sequence.length) ? ((BidiClass)sequence.types[runLimit]) : sequence.eos);
				}
				if (bidiClass8 == BidiClass.EN)
				{
					sequence.SetRunTypes(num2, runLimit, BidiClass.EN);
				}
				m = runLimit;
			}
		}
		for (int n = 0; n < sequence.length; n++)
		{
			BidiClass bidiClass9 = (BidiClass)sequence.types[n];
			if (bidiClass9 == BidiClass.ET || bidiClass9 == BidiClass.ES || bidiClass9 == BidiClass.CS)
			{
				sequence.types[n] = 18;
			}
		}
		for (int num3 = 0; num3 < sequence.length; num3++)
		{
			if (sequence.types[num3] != 8)
			{
				continue;
			}
			BidiClass sos = sequence.sos;
			for (int num4 = num3 - 1; num4 >= 0; num4--)
			{
				BidiClass bidiClass10 = (BidiClass)sequence.types[num4];
				if (bidiClass10 == BidiClass.R || bidiClass10 == BidiClass.L || bidiClass10 == BidiClass.AL)
				{
					sos = bidiClass10;
					break;
				}
				if (sos == BidiClass.L)
				{
					sequence.types[num3] = 0;
				}
			}
		}
	}

	private static void ResolveNeutrals(this IsolatingRunSequence sequence, string input)
	{
		int[,] array = new int[63, 2];
		int num = 0;
		List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
		for (int i = 0; i < sequence.length; i++)
		{
			char c = input[sequence.indexes[i]];
			if (IsBracketTypeOpen(c))
			{
				if (num >= 63)
				{
					break;
				}
				array[num, 0] = c;
				array[num, 1] = i;
				num++;
			}
			else
			{
				if (!IsBracketTypeClose(c) || num == 0)
				{
					continue;
				}
				int num2 = num - 1;
				char c2 = (char)Bidi_Types.MirrorChars[(uint)c];
				while (num2 >= 0)
				{
					if (array[num2, 0] == c2)
					{
						num = num2;
						list.Add(new KeyValuePair<int, int>(array[num2, 1], i));
						break;
					}
					num2--;
				}
			}
		}
		foreach (KeyValuePair<int, int> item in list)
		{
			bool flag = false;
			BidiClass bidiClass = BidiClass.ON;
			for (int j = item.Key + 1; j < item.Value; j++)
			{
				BidiClass bidiClass2 = (BidiClass)sequence.types[j];
				if (bidiClass2 == BidiClass.AN || bidiClass2 == BidiClass.EN)
				{
					bidiClass2 = BidiClass.R;
				}
				if (bidiClass2 == BidiClass.L || bidiClass2 == BidiClass.R)
				{
					bidiClass = bidiClass2;
					if (sequence.sos == bidiClass2)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				for (int num3 = item.Key - 1; num3 >= 0; num3--)
				{
					BidiClass bidiClass3 = (BidiClass)sequence.types[num3];
					if (bidiClass3 == BidiClass.AN || bidiClass3 == BidiClass.EN)
					{
						bidiClass3 = BidiClass.R;
					}
					if ((bidiClass3 == BidiClass.L || bidiClass3 == BidiClass.R) && bidiClass == bidiClass3)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				bidiClass = sequence.sos;
			}
			sequence.types[item.Key] = (byte)bidiClass;
			sequence.types[item.Value] = (byte)bidiClass;
		}
		BidiClass[] typesSet = new BidiClass[8]
		{
			BidiClass.B,
			BidiClass.S,
			BidiClass.WS,
			BidiClass.ON,
			BidiClass.LRI,
			BidiClass.RLI,
			BidiClass.FSI,
			BidiClass.PDI
		};
		for (int k = 0; k < sequence.length; k++)
		{
			BidiClass bidiClass4 = (BidiClass)sequence.types[k];
			if (bidiClass4 != BidiClass.B && bidiClass4 != BidiClass.S && bidiClass4 != BidiClass.WS && bidiClass4 != BidiClass.ON && bidiClass4 != BidiClass.LRI && bidiClass4 != BidiClass.RLI && bidiClass4 != BidiClass.FSI && bidiClass4 != BidiClass.PDI)
			{
				continue;
			}
			BidiClass bidiClass5 = BidiClass.L;
			BidiClass bidiClass6 = BidiClass.L;
			int num4 = k;
			int runLimit = sequence.GetRunLimit(num4, sequence.length, typesSet);
			if (num4 == 0)
			{
				bidiClass5 = sequence.sos;
			}
			else
			{
				bidiClass5 = (BidiClass)sequence.types[num4 - 1];
				if (bidiClass5 == BidiClass.AN || bidiClass5 == BidiClass.EN)
				{
					bidiClass5 = BidiClass.R;
				}
			}
			if (runLimit == sequence.length)
			{
				bidiClass6 = sequence.eos;
			}
			else
			{
				bidiClass6 = (BidiClass)sequence.types[runLimit];
				if (bidiClass6 == BidiClass.AN || bidiClass6 == BidiClass.EN)
				{
					bidiClass6 = BidiClass.R;
				}
			}
			if (bidiClass5 == bidiClass6)
			{
				sequence.SetRunTypes(num4, runLimit, bidiClass5);
			}
			else
			{
				BidiClass typeForLevel = GetTypeForLevel(sequence.level);
				sequence.SetRunTypes(num4, runLimit, typeForLevel);
			}
			k = runLimit;
		}
	}

	private static void ResolveImplicit(this IsolatingRunSequence sequence)
	{
		byte level = sequence.level;
		sequence.resolvedLevels = new byte[sequence.length];
		SetLevels(ref sequence.resolvedLevels, sequence.level);
		for (int i = 0; i < sequence.length; i++)
		{
			BidiClass bidiClass = (BidiClass)sequence.types[i];
			if (!IsOdd(level))
			{
				switch (bidiClass)
				{
				case BidiClass.R:
					sequence.resolvedLevels[i]++;
					break;
				case BidiClass.EN:
				case BidiClass.AN:
					sequence.resolvedLevels[i] += 2;
					break;
				}
			}
			else if (bidiClass == BidiClass.L || bidiClass == BidiClass.AN || bidiClass == BidiClass.EN)
			{
				sequence.resolvedLevels[i]++;
			}
		}
	}

	private static void ApplyTypesAndLevels(this IsolatingRunSequence sequence, ref byte[] typesList, ref byte[] levelsList)
	{
		for (int i = 0; i < sequence.length; i++)
		{
			int num = sequence.indexes[i];
			typesList[num] = sequence.types[i];
			levelsList[num] = sequence.resolvedLevels[i];
		}
	}

	private static int[] GetReorderedIndexes(byte level, byte[] typesList, byte[] levelsList, int[] lineBreaks, out byte[] levels)
	{
		levels = GetTextLevels(level, typesList, levelsList, lineBreaks);
		return GetMultiLineReordered(levels, lineBreaks);
	}

	private static void GetMatchingPDI(byte[] types, out int[] outMatchingPDI, out int[] outMatchingIsolateInitiator)
	{
		int[] array = new int[types.Length];
		int[] array2 = new int[types.Length];
		for (int i = 0; i < types.Length; i++)
		{
			BidiClass bidiClass = (BidiClass)types[i];
			if (bidiClass == BidiClass.LRI || bidiClass == BidiClass.RLI || bidiClass == BidiClass.FSI)
			{
				int num = 1;
				bool flag = false;
				for (int j = i + 1; j < types.Length; j++)
				{
					switch ((BidiClass)types[j])
					{
					case BidiClass.LRI:
					case BidiClass.RLI:
					case BidiClass.FSI:
						num++;
						continue;
					case BidiClass.PDI:
						num--;
						if (num != 0)
						{
							continue;
						}
						break;
					default:
						continue;
					}
					flag = true;
					array[i] = j;
					array2[j] = i;
					break;
				}
				if (!flag)
				{
					array[i] = types.Length;
				}
			}
			else
			{
				array[i] = -1;
				array2[i] = -1;
			}
		}
		outMatchingPDI = array;
		outMatchingIsolateInitiator = array2;
	}

	private static void RemoveX9Characters(ref byte[] buffer)
	{
		for (int i = 0; i < buffer.Length; i++)
		{
			BidiClass bidiClass = (BidiClass)buffer[i];
			if (bidiClass == BidiClass.LRE || bidiClass == BidiClass.RLE || bidiClass == BidiClass.LRO || bidiClass == BidiClass.RLO)
			{
				buffer[i] = 14;
			}
		}
	}

	private static List<List<int>> GetLevelRuns(byte[] levels)
	{
		List<int> list = new List<int>();
		List<List<int>> list2 = new List<List<int>>();
		sbyte b = -1;
		for (int i = 0; i < levels.Length; i++)
		{
			if (levels[i] != b)
			{
				if (b >= 0)
				{
					list2.Add(list);
					list.Clear();
				}
				b = (sbyte)levels[i];
			}
			list.Add(i);
		}
		if (list.Count > 0)
		{
			list2.Add(list);
		}
		return list2;
	}

	private static int[] GetRunForCharacter(List<List<int>> levelRuns, int length)
	{
		int[] array = new int[length];
		for (int i = 0; i < levelRuns.Count; i++)
		{
			for (int j = 0; j < levelRuns[i].Count; j++)
			{
				int num = levelRuns[i][j];
				array[num] = num;
			}
		}
		return array;
	}

	private static List<IsolatingRunSequence> GetIsolatingRunSequences(byte pLevel, byte[] types, byte[] levels, List<List<int>> levelRuns, int[] matchingIsolateInitiator, int[] matchingPDI, int[] runCharsArray)
	{
		List<IsolatingRunSequence> list = new List<IsolatingRunSequence>(levelRuns.Count);
		foreach (List<int> levelRun in levelRuns)
		{
			int num = levelRun[0];
			if (types[num] != 22 || matchingIsolateInitiator[num] == -1)
			{
				List<int> list2 = new List<int>(levelRun);
				int num2 = list2[list2.Count - 1];
				BidiClass bidiClass = (BidiClass)types[num2];
				bool flag = bidiClass == BidiClass.RLI || bidiClass == BidiClass.LRI || bidiClass == BidiClass.FSI;
				int num3 = matchingPDI[num2];
				while (flag && num3 != types.Length)
				{
					int index = runCharsArray[num3];
					List<int> collection = levelRuns[index];
					list2.AddRange(collection);
				}
				list.Add(new IsolatingRunSequence(pLevel, list2, types, levels));
			}
		}
		return list;
	}

	private static void ComputeIsolatingRunSequence(this IsolatingRunSequence sequence, byte pLevel, List<int> indexList, byte[] typesList, byte[] levels)
	{
		sequence.length = indexList.Count;
		sequence.indexes = indexList.ToArray();
		sequence.types = new byte[indexList.Count];
		for (int i = 0; i < sequence.length; i++)
		{
			sequence.types[i] = typesList[indexList[i]];
		}
		byte val = (sequence.level = levels[indexList[0]]);
		int num = indexList[0] - 1;
		byte val2 = ((num >= 0) ? levels[num] : pLevel);
		sequence.sos = GetTypeForLevel(Math.Max(val, val2));
		BidiClass bidiClass = (BidiClass)sequence.types[sequence.length - 1];
		int num2 = indexList[sequence.length - 1];
		byte val3 = levels[num2];
		int num3 = indexList[sequence.length - 1] + 1;
		byte val4 = ((num3 < typesList.Length && bidiClass != BidiClass.PDI) ? levels[num2] : pLevel);
		sequence.eos = GetTypeForLevel(Math.Max(val3, val4));
	}

	private static void SetLevels(ref byte[] levels, byte newLevel)
	{
		for (int i = 0; i < levels.Length; i++)
		{
			levels[i] = newLevel;
		}
	}

	private static int GetRunLimit(this IsolatingRunSequence sequence, int index, int limit, BidiClass[] typesSet)
	{
		while (index < limit)
		{
			BidiClass bidiClass = (BidiClass)sequence.types[index];
			int num = 0;
			while (true)
			{
				if (num < typesSet.Length)
				{
					if (bidiClass == typesSet[num])
					{
						break;
					}
					num++;
					continue;
				}
				return index;
			}
			index++;
		}
		return limit;
	}

	private static void SetRunTypes(this IsolatingRunSequence sequence, int start, int limit, BidiClass newType)
	{
		for (int i = start; i < limit; i++)
		{
			sequence.types[i] = (byte)newType;
		}
	}

	private static int LeastGreaterOdd(int l)
	{
		if (!IsOdd(l))
		{
			return l + 1;
		}
		return l + 2;
	}

	private static int LeastGreaterEven(int l)
	{
		if (IsOdd(l))
		{
			return l + 1;
		}
		return l + 2;
	}

	private static bool IsOdd(int n)
	{
		return (n & 1) != 0;
	}

	private static BidiClass GetTypeForLevel(byte level)
	{
		if (((uint)level & (true ? 1u : 0u)) != 0)
		{
			return BidiClass.R;
		}
		return BidiClass.L;
	}

	private static byte[] GetTextLevels(byte paragraphEmbeddingLevel, byte[] typesList, byte[] levelsList, int[] lineBreaks)
	{
		for (int i = 0; i < levelsList.Length; i++)
		{
			BidiClass bidiClass = (BidiClass)typesList[i];
			if (bidiClass == BidiClass.S || bidiClass == BidiClass.B)
			{
				levelsList[i] = paragraphEmbeddingLevel;
			}
			for (int num = i - 1; num >= 0; num--)
			{
				bidiClass = (BidiClass)typesList[num];
				if (bidiClass != BidiClass.LRI && bidiClass != BidiClass.RLI && bidiClass != BidiClass.FSI && bidiClass != BidiClass.FSI)
				{
					break;
				}
				levelsList[num] = paragraphEmbeddingLevel;
			}
		}
		int num2 = 0;
		foreach (int num3 in lineBreaks)
		{
			for (int num4 = num3 - 1; num4 >= num2; num4--)
			{
				BidiClass bidiClass2 = (BidiClass)typesList[num4];
				if (bidiClass2 != BidiClass.LRI && bidiClass2 != BidiClass.RLI && bidiClass2 != BidiClass.FSI && bidiClass2 != BidiClass.FSI)
				{
					break;
				}
				levelsList[num4] = paragraphEmbeddingLevel;
			}
			num2 = num3;
		}
		return levelsList;
	}

	private static int[] GetMultiLineReordered(byte[] levels, int[] lineBreaks)
	{
		int[] array = new int[levels.Length];
		int num = 0;
		foreach (int num2 in lineBreaks)
		{
			byte[] array2 = new byte[num2 - num];
			Array.Copy(levels, num, array2, 0, array2.Length);
			int[] array3 = ComputeReorderingIndexes(array2);
			for (int j = 0; j < array3.Length; j++)
			{
				array[num + j] = array3[j] + num;
			}
			num = num2;
		}
		return array;
	}

	private static int[] ComputeReorderingIndexes(byte[] levels)
	{
		int num = levels.Length;
		int[] array = new int[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = i;
		}
		byte b = 0;
		byte b2 = 127;
		foreach (byte b3 in levels)
		{
			if (b3 > b)
			{
				b = b3;
			}
			if (IsOdd(b3) && b3 < b2)
			{
				b2 = b3;
			}
		}
		for (int num2 = b; num2 >= b2; num2--)
		{
			for (int k = 0; k < num; k++)
			{
				if (levels[k] >= num2)
				{
					int num3 = k;
					int l;
					for (l = k + 1; l < num && levels[l] >= num2; l++)
					{
					}
					int num4 = num3;
					int num5 = l - 1;
					while (num4 < num5)
					{
						int num6 = array[num4];
						array[num4] = array[num5];
						array[num5] = num6;
						num4++;
						num5--;
					}
					k = l;
				}
			}
		}
		return array;
	}

	private static string GetOrderedString(string input, int[] newIndexes, byte[] levels)
	{
		StringBuilder stringBuilder = new StringBuilder(input.Length);
		foreach (int num in newIndexes)
		{
			char c = input[num];
			if (levels[num] % 2 == 1)
			{
				char c2 = (char)Bidi_Types.MirrorChars[(uint)c];
				if (c2 != 0)
				{
					c = c2;
				}
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	private static bool IsBracketTypeOpen(char ch)
	{
		return "([{༺༼᚛⁅⁽₍⌈⌊〈❨❪❬❮❰❲❴⟅⟦⟨⟪⟬⟮⦃⦅⦇⦉⦋⦍⦏⦑⦓⦕⦗⧘⧚⧼⸢⸤⸦⸨〈《「『【〔〖〘〚﹙﹛﹝（［｛｟｢".IndexOf(ch) != -1;
	}

	private static bool IsBracketTypeClose(char ch)
	{
		return ")]}༻༽᚜⁆⁾₎⌉⌋〉❩❫❭❯❱❳❵⟆⟧⟩⟫⟭⟯⦄⦆⦈⦊⦌⦎⦐⦒⦔⦖⦘⧙⧛⧽⸣⸥⸧⸩〉》」』】〕〗〙〛﹚﹜﹞）］｝｠｣".IndexOf(ch) != -1;
	}
}

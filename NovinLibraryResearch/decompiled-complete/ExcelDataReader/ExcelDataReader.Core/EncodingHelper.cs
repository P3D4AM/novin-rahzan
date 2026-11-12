using System.Text;

namespace ExcelDataReader.Core;

internal class EncodingHelper
{
	public static Encoding GetEncoding(ushort codePage)
	{
		Encoding result = null;
		switch (codePage)
		{
		case 37:
			result = Encoding.GetEncoding("IBM037");
			break;
		case 437:
			result = Encoding.GetEncoding("IBM437");
			break;
		case 500:
			result = Encoding.GetEncoding("IBM500");
			break;
		case 708:
			result = Encoding.GetEncoding("ASMO-708");
			break;
		case 709:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 710:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 720:
			result = Encoding.GetEncoding("DOS-720");
			break;
		case 737:
			result = Encoding.GetEncoding("ibm737");
			break;
		case 775:
			result = Encoding.GetEncoding("ibm775");
			break;
		case 850:
			result = Encoding.GetEncoding("ibm850");
			break;
		case 852:
			result = Encoding.GetEncoding("ibm852");
			break;
		case 855:
			result = Encoding.GetEncoding("IBM855");
			break;
		case 857:
			result = Encoding.GetEncoding("ibm857");
			break;
		case 858:
			result = Encoding.GetEncoding("IBM00858");
			break;
		case 860:
			result = Encoding.GetEncoding("IBM860");
			break;
		case 861:
			result = Encoding.GetEncoding("ibm861");
			break;
		case 862:
			result = Encoding.GetEncoding("DOS-862");
			break;
		case 863:
			result = Encoding.GetEncoding("IBM863");
			break;
		case 864:
			result = Encoding.GetEncoding("IBM864");
			break;
		case 865:
			result = Encoding.GetEncoding("IBM865");
			break;
		case 866:
			result = Encoding.GetEncoding("cp866");
			break;
		case 869:
			result = Encoding.GetEncoding("ibm869");
			break;
		case 870:
			result = Encoding.GetEncoding("IBM870");
			break;
		case 874:
			result = Encoding.GetEncoding("windows-874");
			break;
		case 875:
			result = Encoding.GetEncoding("cp875");
			break;
		case 932:
			result = Encoding.GetEncoding("shift_jis");
			break;
		case 936:
			result = Encoding.GetEncoding("gb2312");
			break;
		case 949:
			result = Encoding.GetEncoding("ks_c_5601-1987");
			break;
		case 950:
			result = Encoding.GetEncoding("big5");
			break;
		case 1026:
			result = Encoding.GetEncoding("IBM1026");
			break;
		case 1047:
			result = Encoding.GetEncoding("IBM01047");
			break;
		case 1140:
			result = Encoding.GetEncoding("IBM01140");
			break;
		case 1141:
			result = Encoding.GetEncoding("IBM01141");
			break;
		case 1142:
			result = Encoding.GetEncoding("IBM01142");
			break;
		case 1143:
			result = Encoding.GetEncoding("IBM01143");
			break;
		case 1144:
			result = Encoding.GetEncoding("IBM01144");
			break;
		case 1145:
			result = Encoding.GetEncoding("IBM01145");
			break;
		case 1146:
			result = Encoding.GetEncoding("IBM01146");
			break;
		case 1147:
			result = Encoding.GetEncoding("IBM01147");
			break;
		case 1148:
			result = Encoding.GetEncoding("IBM01148");
			break;
		case 1149:
			result = Encoding.GetEncoding("IBM01149");
			break;
		case 1200:
			result = Encoding.GetEncoding("utf-16");
			break;
		case 1201:
			result = Encoding.GetEncoding("unicodeFFFE");
			break;
		case 1250:
			result = Encoding.GetEncoding("windows-1250");
			break;
		case 1251:
			result = Encoding.GetEncoding("windows-1251");
			break;
		case 1252:
			result = Encoding.GetEncoding("windows-1252");
			break;
		case 1253:
			result = Encoding.GetEncoding("windows-1253");
			break;
		case 1254:
			result = Encoding.GetEncoding("windows-1254");
			break;
		case 1255:
			result = Encoding.GetEncoding("windows-1255");
			break;
		case 1256:
			result = Encoding.GetEncoding("windows-1256");
			break;
		case 1257:
			result = Encoding.GetEncoding("windows-1257");
			break;
		case 1258:
			result = Encoding.GetEncoding("windows-1258");
			break;
		case 1361:
			result = Encoding.GetEncoding("Johab");
			break;
		case 10000:
			result = Encoding.GetEncoding("macintosh");
			break;
		case 10001:
			result = Encoding.GetEncoding("x-mac-japanese");
			break;
		case 10002:
			result = Encoding.GetEncoding("x-mac-chinesetrad");
			break;
		case 10003:
			result = Encoding.GetEncoding("x-mac-korean");
			break;
		case 10004:
			result = Encoding.GetEncoding("x-mac-arabic");
			break;
		case 10005:
			result = Encoding.GetEncoding("x-mac-hebrew");
			break;
		case 10006:
			result = Encoding.GetEncoding("x-mac-greek");
			break;
		case 10007:
			result = Encoding.GetEncoding("x-mac-cyrillic");
			break;
		case 10008:
			result = Encoding.GetEncoding("x-mac-chinesesimp");
			break;
		case 10010:
			result = Encoding.GetEncoding("x-mac-romanian");
			break;
		case 10017:
			result = Encoding.GetEncoding("x-mac-ukrainian");
			break;
		case 10021:
			result = Encoding.GetEncoding("x-mac-thai");
			break;
		case 10029:
			result = Encoding.GetEncoding("x-mac-ce");
			break;
		case 10079:
			result = Encoding.GetEncoding("x-mac-icelandic");
			break;
		case 10081:
			result = Encoding.GetEncoding("x-mac-turkish");
			break;
		case 10082:
			result = Encoding.GetEncoding("x-mac-croatian");
			break;
		case 12000:
			result = Encoding.GetEncoding("utf-32");
			break;
		case 12001:
			result = Encoding.GetEncoding("utf-32BE");
			break;
		case 20000:
			result = Encoding.GetEncoding("x-Chinese_CNS");
			break;
		case 20001:
			result = Encoding.GetEncoding("x-cp20001");
			break;
		case 20002:
			result = Encoding.GetEncoding("x_Chinese-Eten");
			break;
		case 20003:
			result = Encoding.GetEncoding("x-cp20003");
			break;
		case 20004:
			result = Encoding.GetEncoding("x-cp20004");
			break;
		case 20005:
			result = Encoding.GetEncoding("x-cp20005");
			break;
		case 20105:
			result = Encoding.GetEncoding("x-IA5");
			break;
		case 20106:
			result = Encoding.GetEncoding("x-IA5-German");
			break;
		case 20107:
			result = Encoding.GetEncoding("x-IA5-Swedish");
			break;
		case 20108:
			result = Encoding.GetEncoding("x-IA5-Norwegian");
			break;
		case 20127:
			result = Encoding.GetEncoding("us-ascii");
			break;
		case 20261:
			result = Encoding.GetEncoding("x-cp20261");
			break;
		case 20269:
			result = Encoding.GetEncoding("x-cp20269");
			break;
		case 20273:
			result = Encoding.GetEncoding("IBM273");
			break;
		case 20277:
			result = Encoding.GetEncoding("IBM277");
			break;
		case 20278:
			result = Encoding.GetEncoding("IBM278");
			break;
		case 20280:
			result = Encoding.GetEncoding("IBM280");
			break;
		case 20284:
			result = Encoding.GetEncoding("IBM284");
			break;
		case 20285:
			result = Encoding.GetEncoding("IBM285");
			break;
		case 20290:
			result = Encoding.GetEncoding("IBM290");
			break;
		case 20297:
			result = Encoding.GetEncoding("IBM297");
			break;
		case 20420:
			result = Encoding.GetEncoding("IBM420");
			break;
		case 20423:
			result = Encoding.GetEncoding("IBM423");
			break;
		case 20424:
			result = Encoding.GetEncoding("IBM424");
			break;
		case 20833:
			result = Encoding.GetEncoding("x-EBCDIC-KoreanExtended");
			break;
		case 20838:
			result = Encoding.GetEncoding("IBM-Thai");
			break;
		case 20866:
			result = Encoding.GetEncoding("koi8-r");
			break;
		case 20871:
			result = Encoding.GetEncoding("IBM871");
			break;
		case 20880:
			result = Encoding.GetEncoding("IBM880");
			break;
		case 20905:
			result = Encoding.GetEncoding("IBM905");
			break;
		case 20924:
			result = Encoding.GetEncoding("IBM00924");
			break;
		case 20932:
			result = Encoding.GetEncoding("EUC-JP");
			break;
		case 20936:
			result = Encoding.GetEncoding("x-cp20936");
			break;
		case 20949:
			result = Encoding.GetEncoding("x-cp20949");
			break;
		case 21025:
			result = Encoding.GetEncoding("cp1025");
			break;
		case 21027:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 21866:
			result = Encoding.GetEncoding("koi8-u");
			break;
		case 28591:
			result = Encoding.GetEncoding("iso-8859-1");
			break;
		case 28592:
			result = Encoding.GetEncoding("iso-8859-2");
			break;
		case 28593:
			result = Encoding.GetEncoding("iso-8859-3");
			break;
		case 28594:
			result = Encoding.GetEncoding("iso-8859-4");
			break;
		case 28595:
			result = Encoding.GetEncoding("iso-8859-5");
			break;
		case 28596:
			result = Encoding.GetEncoding("iso-8859-6");
			break;
		case 28597:
			result = Encoding.GetEncoding("iso-8859-7");
			break;
		case 28598:
			result = Encoding.GetEncoding("iso-8859-8");
			break;
		case 28599:
			result = Encoding.GetEncoding("iso-8859-9");
			break;
		case 28603:
			result = Encoding.GetEncoding("iso-8859-13");
			break;
		case 28605:
			result = Encoding.GetEncoding("iso-8859-15");
			break;
		case 29001:
			result = Encoding.GetEncoding("x-Europa");
			break;
		case 32768:
			result = Encoding.GetEncoding("macintosh");
			break;
		case 32769:
			result = Encoding.GetEncoding("windows-1252");
			break;
		case 38598:
			result = Encoding.GetEncoding("iso-8859-8-i");
			break;
		case 50220:
			result = Encoding.GetEncoding("iso-2022-jp");
			break;
		case 50221:
			result = Encoding.GetEncoding("csISO2022JP");
			break;
		case 50222:
			result = Encoding.GetEncoding("iso-2022-jp");
			break;
		case 50225:
			result = Encoding.GetEncoding("iso-2022-kr");
			break;
		case 50227:
			result = Encoding.GetEncoding("x-cp50227");
			break;
		case 50229:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 50930:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 50931:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 50933:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 50935:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 50936:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 50937:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 50939:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 51932:
			result = Encoding.GetEncoding("euc-jp");
			break;
		case 51936:
			result = Encoding.GetEncoding("EUC-CN");
			break;
		case 51949:
			result = Encoding.GetEncoding("euc-kr");
			break;
		case 51950:
			result = Encoding.GetEncoding(string.Empty);
			break;
		case 52936:
			result = Encoding.GetEncoding("hz-gb-2312");
			break;
		case 54936:
			result = Encoding.GetEncoding("GB18030");
			break;
		case 57002:
			result = Encoding.GetEncoding("x-iscii-de");
			break;
		case 57003:
			result = Encoding.GetEncoding("x-iscii-be");
			break;
		case 57004:
			result = Encoding.GetEncoding("x-iscii-ta");
			break;
		case 57005:
			result = Encoding.GetEncoding("x-iscii-te");
			break;
		case 57006:
			result = Encoding.GetEncoding("x-iscii-as");
			break;
		case 57007:
			result = Encoding.GetEncoding("x-iscii-or");
			break;
		case 57008:
			result = Encoding.GetEncoding("x-iscii-ka");
			break;
		case 57009:
			result = Encoding.GetEncoding("x-iscii-ma");
			break;
		case 57010:
			result = Encoding.GetEncoding("x-iscii-gu");
			break;
		case 57011:
			result = Encoding.GetEncoding("x-iscii-pa");
			break;
		case 65000:
			result = Encoding.GetEncoding("utf-7");
			break;
		case 65001:
			result = Encoding.GetEncoding("utf-8");
			break;
		}
		return result;
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Import;

namespace Stimulsoft.Report.Export;

[SuppressUnmanagedCodeSecurity]
public sealed class StiExportUtils
{
	private enum EmfToWmfBitsFlags
	{
		EmfToWmfBitsFlagsDefault = 0,
		EmfToWmfBitsFlagsEmbedEmf = 1,
		EmfToWmfBitsFlagsIncludePlaceable = 2,
		EmfToWmfBitsFlagsNoXORClip = 4
	}

	public sealed class ChecksumAdler32
	{
		private const uint MOD_ADLER = 65521u;

		private uint checksum;

		public long Value => checksum;

		public ChecksumAdler32()
		{
			checksum = 1u;
		}

		public void Update(byte[] buf, int off, int len)
		{
			uint num = checksum & 0xFFFFu;
			uint num2 = checksum >> 16;
			while (len > 0)
			{
				int num3 = ((len > 5550) ? 5550 : len);
				len -= num3;
				while (num3 > 0)
				{
					num += buf[off++];
					num2 += num;
					num3--;
				}
				num %= 65521;
				num2 %= 65521;
			}
			checksum = (num2 << 16) | num;
		}
	}

	private static string wrongUrlSymbols = " \"'*,;<>[^`{|}";

	private static string reportVersion = null;

	private static readonly Regex RegexRemoveWhitespace = new Regex("\\s+");

	private static string[] hexStrings = null;

	private const int MM_TEXT = 1;

	private const int MM_LOMETRIC = 2;

	private const int MM_HIMETRIC = 3;

	private const int MM_LOENGLISH = 4;

	private const int MM_HIENGLISH = 5;

	private const int MM_TWIPS = 6;

	private const int MM_ISOTROPIC = 7;

	private const int MM_ANISOTROPIC = 8;

	private static readonly uint[] _lookup32 = CreateLookup32();

	private static object lockClearType = new object();

	private static int processCounter = 0;

	private const uint SPI_GETFONTSMOOTHING = 74u;

	private const uint SPI_SETFONTSMOOTHING = 75u;

	private const uint SPI_UPDATEINIFILE = 1u;

	private const uint SPI_SENDCHANGE = 2u;

	private const uint SPI_GETFONTSMOOTHINGTYPE = 8202u;

	private const uint FE_FONTSMOOTHINGSTANDARD = 1u;

	private const uint FE_FONTSMOOTHINGCLEARTYPE = 2u;

	internal static string AdditionalData = "iVBORw0KGgoAAAANSUhEUgAAAd8AAAGNCAYAAABQRnukAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAhdEVYdENyZWF0aW9uIFRpbWUAMjAxNzowMjowNSAwMjoyODo0OXhS4H0AABHRSURBVHhe7d0LktvIsYbRkZcy+1/TbEVuWA1Pq0USIFCPzKxzIhzWDYct4hH/10Xp2j9+fvgLABjmP5//DAAMIr4AMJj4AsBg4gsAg4kvAAwmvgAwmPgCwGDiCwCDiS8ADCa+ADCY+ALAYOILAIOJLwAMJr4AMJj4AsBg4gsAg4kvAAwmvgAwmPgCfPPPP//87x/Qi/gCfPE1ugJML+IL8OlRbAWYHsQX4MOryAowrYkvsLwzcRVgWhJfYGnvRFWAaUV8gWVdiakA04L4Aku6E1EB5i7xBZbTIp4CzB3iCyylZTQFmKvEF1hGj1gKMFeIL7CEnpEUYN4lvkB5I+IowLxDfIHSRkZRgDlLfIGyZsRQgDlDfIGSZkZQgDkivkA5EeInwLwivkApkaInwDwjvkAZEWMnwDwivgCdCTDfiS9Qxt9///35q3gEmK/EFyhFgMlAfIFyBJjoxBcoSYCJTHyBsgSYqMQXKE2AiUh8gfIEmGjEF1iCABOJ+ALLEGCiEF9gKQJMBOILLEeAmU18gSUJMDOJL7AsAWYW8QWWJsDMIL7A8gSY0cQX4IMAM5L4AnwSYEYRX4AvBJgRxBfgGwGmN/EFeECA6Ul8AZ4QYHoRX4AXBJgexBfggADTmvgCnCDAtCS+ACcJMK2IL8AbBJgWxBfgTQLMXeILcIEAc4f4AlwkwFwlvgA3CDBXiC/ATQLMu8QXoAEB5h3iC9CIAHOW+AI0JMCcIb4AjQkwR8QXoAMB5hXxBehEgHlGfAE6EmAeEV+AzgSY78QXYAAB5ivxBRhEgNmJL8BAAsxGfAEGE2DEF2ACAV6b+AJMIsDrEl+AiQR4TeILMJkAr0d8AQIQ4LWIL0AQArwO8QUIRIDXIL4AwQhwfeILEJAA1ya+AEEJcF3iCxCYANckvgDBCXA94guQgADXIr4ASQhwHeILkIgA1yC+AMkIcH4/fn74/DVAKY9CEDlc74ocukr3uQfxBUq4GqLskRDgnMQXSKt1eLLGQoDzEV8glRGhyRgMAc5FfIEURsdFgNsS4N+JLxDezKhki4YA5yC+QFhRQiLA7QjwL/7/fIGQIgUkcsweiRy4bPeyFydfIBSntnbcy7jEFwgjy6koUzgEOCZfOwMhZPo6MtNn9RV0TE6+wFSZB9gJuI0VT8BOvsA02U8+TsBtrHgCFl9giiqDK8BtrBZg8QWGqza0AtzGSgEWX2CoqgMrwG2sEmDxBYapPqwC3MYKARZfYIhVTjQC3Eb190V8ge5WCe9OgNuo/N6IL9DVauHdCXAbVd8f8QW6WTW8OwFuo+J7JL5AF6uHdyfAbVR7n8QXaE54fyfAfCe+QFPC+5gA31PthwLxBZoR3tcE+JqKp3HxBZoQ3nME+D0Vw7sRX4DBBPicquHd+N/zBW6bGZNXAx09cpniMvpeVg7vRnyBW2YF7p1xjhxhAf5T9fBuxBe4bEbU7gxz1AgL8L9WCO/Gn/kCadwd5qjDHvlk/l3Pe7hKeDfiC1wyOhithlmA7+txD1cK70Z8gbdlDe9OgO9reQ9XC+9GfIGwtlHuNcwCfF+Le7hieDfiC7xlVBxWHeXNKgFe+RmLLxDOyqO8qx7g1Z+x+AKnZQrCkQzXUjXAfrgSXyAYw/y7agH2fH8RX+CUEREYNczZTvBVAiy8/xJfIATD/Fr2AHu+vxNf4FC2k+Irma8la4CF90/iC0xnnM/LFmDP9jHxhTdVOgWupsqz8w7mJ77whn30jF87TkbXeAdzE1846fvYrTJ+Va6z4vMS4LzEF054NnLGL4fKz8k7mJP4woGjcTN+zOYdzEd84YWzo2b84mr9bKL+GbV3MBfxhSfeHTPjF0+v8EYOsPcwB/GFB64OmOFbR+S/pe09jE984Zu7w2X4Ymj9HB7FVoC5Snzhi1aDZfjO63GvRoR3J8BcIb7wqfVQGb51RA+wdzEe8YUPvcbJ6I3X+p6fDWvkAG9EOBbxZXm9B8ngjTP7XkcP8Mb7GIP4srRRQ2TwXot6f67ENEuAvZNziS/LGj0+xq6v1vf3TkQzBHizR9i7OZ74sqRZY2PkHrsbK/f1PhEe68fPD5+/hiVEGJgsJ6Ndz3vW4l60/nytnk/2mGV7TzNx8mUpUcYw+yi3EjG8LWWOl/D2Jb4sQ/Di2IY96ri3/lwZIya8/YkvS4gW3mzj1vLztvzPav1cRcc9GEV8WUKkQVl13Lbrbnntmb7JyPLMV303ZxBflhFhWFYdtwzXvXp4hHcs8WUpMwdmxXHbrrnHdfu6ua3Vr38G8WU5M4amwri9ew29rtlfnGtLeOcQX5Y0cnBWHLdM1zzqs0a8Jyu+m1GIL8saMTzVxu3oerZ/vec1O/W2U+3dzEZ8WVrPAVpt3DJe76oBEt75xJfl9RiiyuP2/dq2/3vE9fpLVm2set3RiC98aDlIrcct8lethjwXzysO8YVPLYapV3ijBXi7zpFD7s967xPeWMQXvrgzUL3CuxOgdmaEaObzE954xBe+uTJUvcO7WzHAfuj49X5dfceENybxhQfeGaxR4d2J0T2ZY7R99pnvJu2ILzxxZrhaj9vZsK4SYD9oPHYmwsIbm/jCC68GbFZ4d8KUR69n9SzCwhuf+MKBEeN2dZwFmM3XCAtvDuILJ3wdtCjh3VUNcJXrunsd77xvwpuH+MJJ27BFC++uaoChKvGFSVoHU4Dj8Ux4Rnxhgl6jXGXse16HIBKB+MJgvcdfXGLwHHhFfGGgUYNs+Ody/zkivjDI6EEWgOey3Bt/e7ku8YUBZo29AD/X696455whvtDZ7DEWg+da3pvtP8u95izxhY6ijLEoPNfi3vS4v75yrk18oZNowRPg57Z7c+X+XP33wY+fHz5/DTQSeZAznKgi3L9X92nE53PyrU18oRMBvm7106Tw1udrZ+gk8oCuHjeYTXyhIwG+ZuWTn1PvGsQXOhNg4DvxhQEEmDOcetchvjCIAAM78YWBBPi81U6BTr1rEV8YTIAB8YUJBPicVU6DTr3rEV+YRIDZCO+axBcmEuBjleMkvOsSX5hMgGE9/rud4aTvIWodzcihi/ADQrUfBJx61ya+8MKrwe8xngL8WpUACy++doYHtpE/GvoeIYg8ylXCN5vwshFf+OJMdHsT4Oeyh0t42YkvfLoSll4xEuDnsgZMePlKfFneFpPZQXlEgJ/LFjLh5Tt/4YqltYpIz3GN+IPBbnZUIt+bjejyjJMvy4o+3LvIA+4E/Jzw8or4sqQs4d0J8HPbvYl2f4SXI752Zjm9YjFicCP/0BAhOE7iZCG+LKXnOI8aXgE+NvoeiS7v8rUzy+g9yKMGP/LQR/nBYOQ9El6ucPJlGSPCMHKIo4TukWhBan2vBJe7xJcljAyVAP8SNVBX75ng0pL4Ut7oQI0eaQGGfPyZLzRWPfbviPyDAcwkvpS2yvgLMOQivtDBjOAIMOQhvlCIAEMO4gudzIqNAEN84ktZKw+9AENs4gsdzQyNAENc4gsdzQ6gAENM/ks2KGvWuEcMXuTQRf4BAXoR3wXsw7vayI0MToZ7K8AQh6+di/s6uJHHN7Ms4Yj8Ob2brEZ8C3s0aEaunS1m2U5sAgwxiG9Rr4aswsjNvoZs0f1KgGE+8S3ozIBlHrn9sx9dQ6/IZA7vToBhLvEt5p3hyjhy3z/z6GuoEN6dAMM84lvIlcHKNHLPPuuoa6gU3p0AwxziW8Sdocowckefsfc1VAzvToBhPPEtoMVARR65s5/NUF8nwDCW+CbXcpgijty7n6nHNVQ+9X4lwDCO+CbWY5AijdzVz2KorxNgGEN8k+o5RBFG7u5n2P/9d2Oyyqn3KwGG/sQ3oREDNHPkWv3ehvo6AYa+xDeZkcMzY+Ra/56G+joBhn7EN5EKMXzFoMYjwNCH+CYxc2hG/N6GNC4BhvbEN4EIA9PzMxjQ+AQY2hLf4KIMS6/xNZx5CDC0I76BCe98Rv13UQMc+QcDeER8gxJeoooWOuElI/ENSHiJLkrwhJesxDeY6uHNyA8Lj81+R7yjZCa+gawSXqNZx6xn6R0iO/ENYrUTr/GsY/Sz9O5QgfgGsOpXzZlG1FfPr416lsJLFeI72arh3RnTOno/S+8KlYjvRKuHd5dlVJ1+j/V6lsJLNeI7ifD+ToDraP0shZeKxHcC4X3MyNbR6ll6J6hKfAcT3tcyjK3T7zl3n6XwUpn4DiS85whwHVefpfBSnfgOIrzvEeA63n2WwssKxHcA4b3GCNdx9ll65qxCfDsT3nuif26n3/OOnqXwshLx7Uh42xDgOp49S+FlNeLbifC2JcB1fH+WwsuKxLcD4e1DgOvYn6XwsqofPz98/poGhLe/6JETFOCIk29DwjtG9OtzAgaOiG8jwjuW0yWQmfg2ILxzRL5ep1/gFfG9SXjnEmAgI/G9QXhjEGAgG/G9SHhjEWAgE/G9QHhjEmAgC/F9k/DGJsBABuL7BuHNQYCB6MT3pEijacCPCTAQmfieEHEsDfgxAQaiEt8DkUfSgB8TYCAi8X0hwzga8GMCDEQjvk9kGkUDfkyAgUjE94GMY2jAjwkwEIX4fpN5BA34MQEGIhDfLyqMnwE/JsDAbOL7qdLoGfBjAgzMJL4fKo6dAT8mwMAsy8e38sgZ8GMCDMywdHxXGDcDfkyAgdGWje9Ko2bAjwkwMNKS8V1xzAz4MQEGRlkuviuPmAE/JsDACEvF13i5B2cIMNDbMvE1Wv9yL44JMNDTEvE1Vn9yT44JMNBL+fgaqefcm2MCDPRQOr7G6Zh7dEyAgdbKxjfKKEUe7p0BPybAQEsl4xstvAJcgwADrZSLb7Tw7gS4BgEGWigV36jh3QlwDQIM3FUmvtHDuxPgGgQYuKNEfLOEdyfANQgwcFX6+GYL706AaxBg4IrU8c0a3p0A1yDAwLvSxjd7eHcCXEOG5wjEkTK+VcK7E+AaIj5HPxRATOniWy28OwGuIdJzFF6IK1V8q4Z3J8A1RHiOwguxpYlv9fDuBLiGmc9ReCG+tH/haoZRoybANcx4jsILOaSJ7+xRGf37C3ANI5+j8EIeqU6+s8Zltd/3HQJ8bMRzFF7IJd3XzqNHZvaoCXANPZ+j8EI+Kf/Md9TYRBk1Aa6hx3MUXsgp7V+46j060UZNgGto+RyFF/JKG99Nr/GJOmoCXEOL5yi8kFvq+G5aj1D0URPgGu48R+GF/NLHd9NqjLKMmgDXcOU5Ci/UUCK+m7ujlG3UBLiGd56j8EIdZeK7uTpOWUdNgGs48xyFF2opFd/NuyOVfdQEuIZXz1F4oZ5y8d2cHasqoybANTx6jsILNZWM7+ZotKqNmgDX8PU5Ci/U9ePnh89fl/Ro8CuPWobAiQqwurIn3933oa8+/BmuzwkYWF35+G72IK1y4hJggNjKf+28Ml9BA8S0xMl3VU7AADGJb3ECDBCP+C5AgAFiEd9FCDBAHOK7EAEGiEF8FyPAAPOJ74IEGGAu8V2UAAPMI74LE2CAOcR3cQIMMJ74IsAAg4kv/yPAAOOIL/8nwABjiC+/EWCA/sSXPwgwQF/iy0MCDNCP+PKUAAP0Ib68JMAA7YkvhwQYoC3x5RQBBmhHfDlNgAHaEF/eIsAA94kvbxNggHvEl0sEGOA68eUyAQa4Rny5RYAB3ie+3CbAAO8RX5oQYIDzxJdmBBjgHPGlKQEGOCa+NCfAAK+JL10IMMBz4ks3AgzwmPjSVYYAA4wmvnQXOcB+OABmEF+GiBg54QVmEV+GiRQ74QVmEl+GihA94QVmE1+Gmxk/4QUiEF+mmBFB4QWiEF+mGRlD4QUiEV+mGhFF4QWiEV+m6xlH4QUiEl9C6BFJ4QWiEl/CaBlL4QUiE19CaRFN4QWiE1/CuRNP4QUyEF9CuhJR4QWyEF/CeiemwgtkIr6EdiaqwgtkI76E9yquwgtkJL6k8CiywgtkJb6k8TW2wgtk9uPnh89fAwADOPkCwGDiCwCDiS8ADCa+ADCY+ALAYOILAIOJLwAMJr4AMJj4AsBg4gsAg4kvAAwmvgAwmPgCwGDiCwCDiS8ADCa+ADCY+ALAYOILAIOJLwAMJr4AMJj4AsBQf/31X1H6k/ZbMBjlAAAAAElFTkSuQmCC";

	internal static string AdditionalData2 = "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";

	private static byte[] cachedAdditionalData2 = null;

	private static string[] positivePatterns = new string[4] { "$n", "n$", "$ n", "n $" };

	private static string[] negativePatterns = new string[16]
	{
		"($n)", "-$n", "$-n", "$n-", "(n$)", "-n$", "n-$", "n$-", "-n $", "-$ n",
		"n $-", "$ n-", "$ -n", "n- $", "($ n)", "(n $)"
	};

	public static string ConvertDigitsToArabic(string outputString, StiArabicDigitsType digitsType)
	{
		StringBuilder outputString2 = new StringBuilder(outputString);
		outputString2 = ConvertDigitsToArabic(outputString2, digitsType);
		return outputString2.ToString();
	}

	public static StringBuilder ConvertDigitsToArabic(StringBuilder outputString, StiArabicDigitsType digitsType)
	{
		for (int i = 0; i < outputString.Length; i++)
		{
			int num = outputString[i];
			if (num >= 48 && num <= 57)
			{
				num += 1584;
				if (digitsType == StiArabicDigitsType.Eastern)
				{
					num += 144;
				}
				outputString[i] = (char)num;
			}
		}
		return outputString;
	}

	public static string TrimEndWhiteSpace(string inputString)
	{
		return TrimEndWhiteSpace(inputString, removeControl: true);
	}

	public static string TrimEndWhiteSpace(string inputString, bool removeControl)
	{
		if (StiOptions.Engine.MeasureTrailingSpaces || !StiOptions.Export.Text.TrimTrailingSpaces)
		{
			return inputString;
		}
		string result = string.Empty;
		int num = inputString.Length;
		while (num > 0 && char.IsWhiteSpace(inputString[num - 1]) && (removeControl || inputString[num - 1] >= ' '))
		{
			num--;
		}
		if (num == inputString.Length)
		{
			result = inputString;
		}
		else if (num > 0)
		{
			result = inputString.Substring(0, num);
		}
		return result;
	}

	public static List<string> SplitString(string inputString, bool removeControl)
	{
		List<string> list = new List<string>();
		if (inputString == null)
		{
			inputString = string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		string text = inputString;
		foreach (char c in text)
		{
			if (c == '\n')
			{
				list.Add(TrimEndWhiteSpace(stringBuilder.ToString(), removeControl));
				stringBuilder.Length = 0;
			}
			else if (!removeControl || !char.IsControl(c) || c == '\t')
			{
				stringBuilder.Append(c);
			}
		}
		if (stringBuilder.Length > 0)
		{
			list.Add(TrimEndWhiteSpace(stringBuilder.ToString(), removeControl));
		}
		if (list.Count == 0)
		{
			list.Add(string.Empty);
		}
		return list;
	}

	public static string[] SplitString(string inputString, char delimiter)
	{
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		char c = ' ';
		foreach (char c2 in inputString)
		{
			if (flag)
			{
				if (c2 == c)
				{
					flag = false;
				}
				stringBuilder.Append(c2);
				continue;
			}
			if (c2 == delimiter)
			{
				list.Add(stringBuilder.ToString());
				stringBuilder = new StringBuilder();
				continue;
			}
			if (c2 == '"' || c2 == '\'')
			{
				c = c2;
				flag = true;
			}
			stringBuilder.Append(c2);
		}
		if (stringBuilder.Length > 0)
		{
			list.Add(stringBuilder.ToString());
		}
		return list.ToArray();
	}

	public static string StringToUrl(string input)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		byte[] bytes = uTF8Encoding.GetBytes(input);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = bytes;
		foreach (byte b in array)
		{
			if (b < 32 || b > 127 || wrongUrlSymbols.IndexOf((char)b) != -1)
			{
				stringBuilder.Append($"%{b:x2}");
			}
			else
			{
				stringBuilder.Append((char)b);
			}
		}
		return stringBuilder.ToString();
	}

	public static MemoryStream MakePdfDeflateStream(byte[] data)
	{
		MemoryStream memoryStream = new MemoryStream();
		DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream(memoryStream);
		deflaterOutputStream.IsStreamOwner = false;
		deflaterOutputStream.Write(data, 0, data.Length);
		deflaterOutputStream.Close();
		return memoryStream;
	}

	public static string GetReportVersion(StiReport report = null)
	{
		bool flag = false;
		if (report != null)
		{
			StiPagesCollection renderedPages = report.RenderedPages;
			if (renderedPages != null && renderedPages.Count > 0)
			{
				object tagValue = report.RenderedPages.GetPageWithoutCache(0).TagValue;
				flag = tagValue as string == "*Dashboards*" || tagValue is StiAdvancedWatermark;
			}
		}
		if (reportVersion == null)
		{
			Assembly assembly = Assembly.GetAssembly(typeof(StiReport));
			Version version = assembly.GetName().Version;
			reportVersion = string.Format("{0}.{1}.{2} from {3:D}, {4}", version.Major.ToString(), version.Minor.ToString(), version.Build.ToString(), "7 December 2021", GetFrameworkVersion());
		}
		return string.Format("Stimulsoft {0} {1}", flag ? "Dashboards" : "Reports", reportVersion);
	}

	private static string GetFrameworkVersion()
	{
		string text = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
		if (!string.IsNullOrEmpty(text) && text.IndexOf("=v") > 0)
		{
			string text2 = text.Substring(text.IndexOf("=v") + 2);
			return ".NET" + ((text.StartsWith(".NETCoreApp") && (text2.StartsWith("2") || text2.StartsWith("3"))) ? " Core" : "") + " " + text2;
		}
		return ".NET";
	}

	public static void CorrectRichTextForRiched20(StiRichText rich)
	{
		string rtfText = rich.RtfText;
		string text = CorrectRichTextImagesOrNull(rtfText);
		if (text != null)
		{
			string text2 = CorrectEncoding(text);
			if (text2 != null)
			{
				rich.RtfText = text2;
			}
			else
			{
				rich.RtfText = text;
			}
		}
		else
		{
			string text3 = CorrectEncoding(rtfText);
			if (text3 != null)
			{
				rich.RtfText = text3;
			}
		}
	}

	public static string CorrectRichTextForRiched20(string baseText)
	{
		string text = CorrectRichTextImagesOrNull(baseText);
		if (text != null)
		{
			string text2 = CorrectEncoding(text);
			if (text2 != null)
			{
				return text2;
			}
			return text;
		}
		string text3 = CorrectEncoding(baseText);
		if (text3 != null)
		{
			return text3;
		}
		return baseText;
	}

	private static string CorrectRichTextImagesOrNull(string baseText)
	{
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		try
		{
			int num = 0;
			int num2 = -1;
			int num3 = num;
			while ((num2 = baseText.IndexOf("{\\pict", num, StringComparison.InvariantCulture)) != -1)
			{
				num = num2 + 6;
				StringReader rtfReader = new StringReader(baseText.Substring(num, 1024));
				StiRtfCorrectionHelper stiRtfCorrectionHelper = new StiRtfCorrectionHelper(rtfReader);
				int num4 = 0;
				int num5 = -1;
				int num6 = -1;
				int num7 = -1;
				int num8 = -1;
				string text = null;
				int num9 = -1;
				int num10 = 0;
				StiRtfCorrectionHelper.StiRtfToken stiRtfToken = stiRtfCorrectionHelper.NextToken();
				while (stiRtfToken.Type != StiRtfCorrectionHelper.StiRtfTokenType.Eof)
				{
					if (stiRtfToken.Type == StiRtfCorrectionHelper.StiRtfTokenType.GroupStart)
					{
						num4++;
					}
					if (stiRtfToken.Type == StiRtfCorrectionHelper.StiRtfTokenType.GroupEnd)
					{
						num4--;
					}
					if (num4 == 0)
					{
						switch (stiRtfToken.Type)
						{
						case StiRtfCorrectionHelper.StiRtfTokenType.Keyword:
							if (stiRtfToken.Key == "picwgoal")
							{
								num5 = stiRtfToken.Parameter;
							}
							if (stiRtfToken.Key == "pichgoal")
							{
								num6 = stiRtfToken.Parameter;
							}
							if (stiRtfToken.Key == "picscalex")
							{
								num7 = stiRtfToken.Parameter;
							}
							if (stiRtfToken.Key == "picscaley")
							{
								num8 = stiRtfToken.Parameter;
							}
							if (stiRtfToken.Key == "bliptag")
							{
								num9 = stiRtfToken.Parameter;
							}
							if (stiRtfToken.Key.EndsWith("blip"))
							{
								text = stiRtfToken.Key;
							}
							break;
						case StiRtfCorrectionHelper.StiRtfTokenType.Text:
							num10 = num + stiRtfToken.Position;
							break;
						}
						if (stiRtfToken.Type == StiRtfCorrectionHelper.StiRtfTokenType.Text)
						{
							break;
						}
					}
					stiRtfToken = stiRtfCorrectionHelper.NextToken(num4 > 0);
				}
				if (num9 != -1 || (!(text == "jpegblip") && !(text == "pngblip")) || num5 == -1 || num6 == -1)
				{
					continue;
				}
				MemoryStream memoryStream = new MemoryStream();
				while (baseText[num10] != '}')
				{
					int num11 = baseText[num10];
					if ((num11 >= 48 && num11 <= 57) || (num11 >= 97 && num11 <= 102))
					{
						memoryStream.WriteByte((byte)ParseHexTwoCharToInt(baseText[num10], baseText[num10 + 1]));
						num10 += 2;
					}
					else
					{
						num10++;
					}
				}
				num = num10 + 1;
				memoryStream.Seek(0L, SeekOrigin.Begin);
				Bitmap val = new Bitmap((Stream)memoryStream);
				Metafile val2 = null;
				Bitmap val3 = new Bitmap(1, 1);
				try
				{
					Graphics val4 = Graphics.FromImage((Image)(object)val3);
					try
					{
						IntPtr hdc = val4.GetHdc();
						val2 = new Metafile(hdc, (EmfType)3);
						val4.ReleaseHdc(hdc);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				Graphics val5 = Graphics.FromImage((Image)(object)val2);
				try
				{
					val5.DrawImageUnscaledAndClipped((Image)(object)val, new Rectangle(0, 0, ((Image)val).Width - 1, ((Image)val).Height - 1));
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				((Image)val).Dispose();
				IntPtr henhmetafile = val2.GetHenhmetafile();
				uint num12 = GdipEmfToWmfBits(henhmetafile, 0u, null, 8, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
				byte[] array = new byte[num12];
				GdipEmfToWmfBits(henhmetafile, num12, array, 8, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
				DeleteEnhMetaFile(henhmetafile);
				((Image)val2).Dispose();
				if (num2 > 50)
				{
					int num13 = baseText.IndexOf("shppict", num2 - 50, 50);
					if (num13 > 0)
					{
						int num14 = baseText.LastIndexOf("{", num13, 20);
						if (num14 > 0)
						{
							string input = baseText.Substring(num14, num2 - num14);
							string text2 = RegexRemoveWhitespace.Replace(input, "");
							if (text2 == "{\\*\\shppict")
							{
								num2 = num14 + 1;
							}
						}
					}
				}
				stringBuilder.Append(baseText.Substring(num3, num2 - num3));
				stringBuilder.Append("{\\pict");
				if (num5 > 0)
				{
					stringBuilder.Append("\\picwgoal" + num5);
				}
				if (num6 > 0)
				{
					stringBuilder.Append("\\pichgoal" + num6);
				}
				if (num7 > 0)
				{
					stringBuilder.Append("\\picscalex" + num7);
				}
				if (num8 > 0)
				{
					stringBuilder.Append("\\picscaley" + num8);
				}
				stringBuilder.Append("\\wmetafile8\r\n");
				stringBuilder.Append(ByteArrayToHex(array));
				stringBuilder.Append("}\r\n");
				num3 = num;
				flag = true;
			}
			if (flag)
			{
				stringBuilder.Append(baseText.Substring(num3));
			}
		}
		catch
		{
			flag = false;
		}
		if (flag)
		{
			return stringBuilder.ToString();
		}
		return null;
	}

	public static string CorrectEncoding(string st)
	{
		if (string.IsNullOrEmpty(st))
		{
			return null;
		}
		Encoding encoding = Encoding.Default;
		try
		{
			int num = st.IndexOf("\\ansicpg", StringComparison.InvariantCulture);
			if (num > 0)
			{
				num += 8;
				int i;
				for (i = num; i < st.Length && char.IsDigit(st, i); i++)
				{
				}
				string s = st.Substring(num, i - num);
				int result = 0;
				int.TryParse(s, out result);
				if (result != 0)
				{
					encoding = Encoding.GetEncoding(result);
				}
			}
		}
		catch
		{
		}
		byte[] bytes = encoding.GetBytes(st);
		bool flag = false;
		byte[] array = bytes;
		foreach (byte b in array)
		{
			if (b > 127)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array2 = bytes;
		foreach (byte b2 in array2)
		{
			if (b2 > 127)
			{
				stringBuilder.AppendFormat("\\'{0:x2}", b2);
			}
			else
			{
				stringBuilder.Append((char)b2);
			}
		}
		return stringBuilder.ToString();
	}

	internal static string GetHexString(byte bb)
	{
		if (hexStrings == null)
		{
			hexStrings = new string[256];
			for (int i = 0; i < 256; i++)
			{
				hexStrings[i] = i.ToString("x2");
			}
		}
		return hexStrings[bb];
	}

	[DllImport("gdiplus.dll")]
	private static extern uint GdipEmfToWmfBits(IntPtr _hEmf, uint _bufferSize, byte[] _buffer, int _mappingMode, EmfToWmfBitsFlags _flags);

	[DllImport("gdi32.dll")]
	private static extern bool DeleteEnhMetaFile(IntPtr hEmf);

	private static uint[] CreateLookup32()
	{
		uint[] array = new uint[256];
		for (int i = 0; i < 256; i++)
		{
			string text = i.ToString("X2");
			array[i] = text[0] + ((uint)text[1] << 16);
		}
		return array;
	}

	public static string ByteArrayToHex(byte[] bytes)
	{
		int num = bytes.Length * 2 + bytes.Length / 120 * 2;
		char[] array = new char[num];
		int num2 = 0;
		for (int i = 0; i < bytes.Length; i++)
		{
			uint num3 = _lookup32[bytes[i]];
			array[num2++] = (char)num3;
			array[num2++] = (char)(num3 >> 16);
			if (i % 120 == 0 && i > 0)
			{
				array[num2++] = '\r';
				array[num2++] = '\n';
			}
		}
		return new string(array);
	}

	private static int ParseHexTwoCharToInt(char c1, char c2)
	{
		return ParseHexCharToInt(c1) * 16 + ParseHexCharToInt(c2);
	}

	private static int ParseHexCharToInt(char ch)
	{
		if (ch <= '9')
		{
			return ch - 48;
		}
		return ch switch
		{
			'a' => 10, 
			'b' => 11, 
			'c' => 12, 
			'd' => 13, 
			'e' => 14, 
			'f' => 15, 
			_ => 0, 
		};
	}

	internal static uint GetAdler32Checksum(byte[] buff)
	{
		Adler32 adler = new Adler32();
		adler.Update(buff);
		return (uint)adler.Value;
	}

	public static void DisableFontSmoothing(StiReport report)
	{
		if (StiOptions.Export.DisableClearTypeDuringExport && StiOptions.Engine.FullTrust && report != null && !report.FlagFontSmoothing)
		{
			report.FlagFontSmoothing = true;
			DisableFontSmoothingInternal();
		}
	}

	private static void DisableFontSmoothingInternal()
	{
		lock (lockClearType)
		{
			if (processCounter == 0)
			{
				try
				{
					if (GetFontSmoothing())
					{
						processCounter++;
						int pvParam = 0;
						bool flag = SystemParametersInfo(75u, 0u, ref pvParam, 3u);
					}
					return;
				}
				catch
				{
					if (processCounter == 0)
					{
						StiOptions.Export.DisableClearTypeDuringExport = false;
					}
					return;
				}
			}
			processCounter++;
		}
	}

	public static void EnableFontSmoothing(StiReport report)
	{
		if (StiOptions.Export.DisableClearTypeDuringExport && StiOptions.Engine.FullTrust && report != null && (report.FlagFontSmoothing || (report.CompiledReport != null && report.CompiledReport.FlagFontSmoothing)))
		{
			report.FlagFontSmoothing = false;
			EnableFontSmoothingInternal();
		}
	}

	public static void EnableFontSmoothingInternal()
	{
		lock (lockClearType)
		{
			if (processCounter == 0)
			{
				return;
			}
			processCounter--;
			if (processCounter != 0)
			{
				return;
			}
			try
			{
				int pvParam = 0;
				bool flag = SystemParametersInfo(75u, 1u, ref pvParam, 3u);
			}
			catch
			{
			}
		}
	}

	private static bool GetFontSmoothing()
	{
		int pvParam = 0;
		bool flag = SystemParametersInfo(74u, 0u, ref pvParam, 0u);
		int pvParam2 = 0;
		flag = SystemParametersInfo(8202u, 0u, ref pvParam2, 0u);
		if (pvParam > 0 && (long)pvParam2 == 2)
		{
			return true;
		}
		return false;
	}

	[DllImport("user32.dll", SetLastError = true)]
	internal static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int pvParam, uint fWinIni);

	public static string SaveComponentToString(StiComponent component)
	{
		return SaveComponentToString(component, ImageFormat.Png, 0.75f, 100f);
	}

	public static string SaveComponentToString(StiComponent component, ImageFormat imageFormat, float imageQuality, float imageResolution)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		Image val = null;
		float zoom = imageResolution / 100f;
		if (component is StiTextInCells)
		{
			val = (component as StiTextInCells).GetImage(ref zoom, StiExportFormat.ImagePng);
		}
		else if (component is StiRichText)
		{
			StiRichText stiRichText = ((StiService)(component as StiRichText)).Clone() as StiRichText;
			stiRichText.Border = new StiBorder();
			val = stiRichText.GetImage(ref zoom, StiExportFormat.ImagePng);
		}
		else if (component is StiText)
		{
			StiText stiText = ((StiService)(component as StiText)).Clone() as StiText;
			stiText.Border = new StiBorder();
			val = stiText.GetImage(ref zoom, StiExportFormat.Pdf);
		}
		else if (component is StiImage)
		{
			val = (component as StiImage).GetImage(ref zoom, StiExportFormat.ImagePng);
		}
		else if (component is StiBarCode)
		{
			StiBarCode stiBarCode = ((StiService)(component as StiBarCode)).Clone() as StiBarCode;
			stiBarCode.Border = new StiBorder();
			val = stiBarCode.GetImage(ref zoom, StiExportFormat.ImagePng);
		}
		else if (component is StiShape)
		{
			val = (component as StiShape).GetImage(ref zoom, StiExportFormat.ImagePng);
		}
		else if (component is StiCheckBox)
		{
			StiCheckBox stiCheckBox = ((StiService)(component as StiCheckBox)).Clone() as StiCheckBox;
			stiCheckBox.Border = new StiBorder();
			val = stiCheckBox.GetImage(ref zoom, StiExportFormat.ImagePng);
		}
		else if (component is StiZipCode)
		{
			StiZipCode stiZipCode = ((StiService)(component as StiZipCode)).Clone() as StiZipCode;
			stiZipCode.Border = new StiBorder();
			val = stiZipCode.GetImage(ref zoom, StiExportFormat.ImagePng);
		}
		else if (component is StiChart)
		{
			StiChart stiChart = ((StiService)(component as StiChart)).Clone() as StiChart;
			stiChart.Border = new StiBorder();
			val = stiChart.GetImage(ref zoom, StiExportFormat.ImagePng);
		}
		EnableFontSmoothing(component.Report);
		if (val == null)
		{
			return string.Empty;
		}
		MemoryStream memoryStream = new MemoryStream();
		if (imageFormat != ImageFormat.Jpeg)
		{
			val.Save((Stream)memoryStream, imageFormat);
		}
		else
		{
			ImageCodecInfo imageCodec = StiImageCodecInfo.GetImageCodec("image/jpeg");
			if (imageCodec == null)
			{
				val.Save((Stream)memoryStream, imageFormat);
			}
			else
			{
				EncoderParameters val2 = new EncoderParameters(1);
				val2.Param[0] = new EncoderParameter(Encoder.Quality, (long)(imageQuality * 100f));
				val.Save((Stream)memoryStream, imageCodec, val2);
			}
		}
		memoryStream.Seek(0L, SeekOrigin.Begin);
		return Convert.ToBase64String(memoryStream.ToArray());
	}

	internal static byte[] GetAdditionalData2(byte opacity)
	{
		if (cachedAdditionalData2 == null)
		{
			cachedAdditionalData2 = Convert.FromBase64String(AdditionalData2);
		}
		return cachedAdditionalData2;
	}

	public static string GetPositivePattern(int patternIndex)
	{
		return positivePatterns[patternIndex];
	}

	public static string GetNegativePattern(int patternIndex)
	{
		return negativePatterns[patternIndex];
	}
}

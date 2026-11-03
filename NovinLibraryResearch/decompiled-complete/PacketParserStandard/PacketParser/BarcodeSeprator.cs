using System;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser;

public class BarcodeSeprator
{
	private const string A = "شماره ترازو";

	private const string B = "شماره فاکتور";

	private const string C = "تعداد ردیف";

	private const string D = "وزن/تعداد";

	private const string E = "قیمت واحد";

	private const string F = "کد کالا";

	private const string G = "مبلغ کل";

	private const string H = "کاراکتر کنترل";

	private const string I = "نامشخص";

	private const string J = "ردیف حافظه";

	private const string T = "تاریخ";

	private const string K = "نوع فاکتور";

	private const string L = "کد تفصیلی";

	private const string N = "شناسه فاکتور";

	private const string O = "شناسه مشتری";

	private const string Y = "اینتر";

	public string ControlCharacter { get; private set; }

	public decimal PriceZarib { get; private set; }

	public decimal WeightZarib { get; private set; }

	public string Barcode { get; private set; }

	public string Pattern { get; private set; }

	public string Tarikh { get; private set; }

	public short ScaleNO { get; private set; }

	public int FactorNO { get; private set; }

	public short RadiCount { get; private set; }

	public decimal Weight { get; private set; }

	public decimal UnitPrice { get; private set; }

	public string Code { get; private set; }

	public string TafsilCode { get; private set; }

	public Guid TafsilGuid { get; private set; }

	public Guid OrderGuid { get; private set; }

	public decimal TotalPrice { get; private set; }

	public string Flag { get; private set; }

	public string Other { get; private set; }

	public bool IsValid { get; private set; }

	public int MemoryAddress { get; private set; }

	public Document OrderType { get; private set; }

	public BarcodeSeprator(string pattern, string barcode, decimal weightZarib, decimal priceZarib, string controlCharacter)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeSeprator.cs"))
		{
			try
			{
				if (weightZarib <= 0m)
				{
					weightZarib = 0.001m;
				}
				if (priceZarib <= 0m)
				{
					priceZarib = 1m;
				}
				Barcode = barcode.Trim();
				Pattern = pattern.Replace("Y", "\r\n").ToUpper().Trim();
				if (Pattern.Contains("O") || Pattern.Contains("N"))
				{
					if (((from q in Pattern.ToCharArray()
						where q == 'O'
						select q).ToList()?.Count() ?? 0) == 1)
					{
						Pattern = Pattern.Replace("O", "OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
					}
					if (((from q in Pattern.ToCharArray()
						where q == 'N'
						select q).ToList()?.Count() ?? 0) == 1)
					{
						Pattern = Pattern.Replace("N", "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN");
					}
				}
				WeightZarib = weightZarib;
				PriceZarib = priceZarib;
				ControlCharacter = controlCharacter;
				IsValid = CheckBarcodeWithPattern(Barcode, Pattern);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeSeprator.cs", 51);
			}
		}
	}

	private bool CheckBarcodeWithPattern(string barcode, string pattern)
	{
		using (new JaegerMonitor("CheckBarcodeWithPattern", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeSeprator.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(barcode) || string.IsNullOrEmpty(pattern) || barcode.Length != pattern.Length)
				{
					return false;
				}
				ScaleNO = GetSpecialParameter("A").Trim().ParseToShort(0);
				FactorNO = GetSpecialParameter("B").Trim().ParseToInt();
				RadiCount = GetSpecialParameter("C").Trim().ParseToShort(0);
				Weight = GetSpecialParameter("D").Trim().ParseToDecimal(0m, 3) * WeightZarib;
				UnitPrice = GetSpecialParameter("E").ParseToDecimal() * PriceZarib;
				Code = GetSpecialParameter("F").Trim();
				TotalPrice = GetSpecialParameter("G").ParseToDecimal() * PriceZarib;
				Other = GetSpecialParameter("I").Trim();
				Tarikh = GetSpecialParameter("T").Trim();
				Flag = GetSpecialParameter("H").Trim();
				MemoryAddress = GetSpecialParameter("J").Trim().ParseToInt();
				OrderType = (Document)GetSpecialParameter("K").Trim().ParseToShort(0);
				TafsilCode = GetSpecialParameter("L");
				string specialParameter = GetSpecialParameter("O");
				if (!string.IsNullOrEmpty(specialParameter))
				{
					TafsilGuid = Guid.Parse(specialParameter);
				}
				string specialParameter2 = GetSpecialParameter("N");
				if (!string.IsNullOrEmpty(specialParameter2))
				{
					OrderGuid = Guid.Parse(specialParameter2);
				}
				return ControlCharacter == null || string.IsNullOrEmpty(ControlCharacter.Trim()) || Flag == ControlCharacter;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckBarcodeWithPattern", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeSeprator.cs", 108);
				return false;
			}
		}
	}

	private string GetSpecialParameter(string ch)
	{
		using (new JaegerMonitor("GetSpecialParameter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeSeprator.cs"))
		{
			try
			{
				string text = Pattern.Trim().Replace("Y", "").Replace("\r\n", "");
				string text2 = Barcode.Trim().Replace("\r\n", "");
				int num = text.IndexOf(ch);
				int num2 = checked(text.Split(new char[1] { Convert.ToChar(ch) }).Length - 1);
				if (num < 0 || num2 == 0)
				{
					return "";
				}
				return text2.Substring(num, num2).Replace("\r\n", "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSpecialParameter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeSeprator.cs", 127);
				return "";
			}
		}
	}
}

using System;
using System.Text;
using PacketParser;
using PacketParser.Attendance.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;

namespace TimeAttendanceDAL.Tax_and_insurace;

public class ClsTaxPersonInfo
{
	private Person ClsPerson;

	public short Melliat { get; set; } = 1;


	public string NationalCode { get; set; }

	public string Name { get; set; }

	public string Family { get; set; }

	public string FatherName { get; set; }

	public string BirthDate { get; set; }

	public string ShenasnameNumber { get; set; }

	public string MahalTavalod { get; set; }

	public short MadrakTahsili { get; set; } = 2;


	public short BimeType { get; set; } = 2;


	public string BimeNumber { get; set; }

	public string BimeName { get; set; }

	public string MoafiatType { get; set; } = "1";


	public TaxEnums.EnCountry MahalTabeiat { get; set; } = TaxEnums.EnCountry.Iran;


	public TaxEnums.EnCountry MahalZendegi { get; set; } = TaxEnums.EnCountry.Iran;


	public string CodePosti { get; set; }

	public string Address { get; set; }

	public TaxEnums.EnRaste Raste { get; set; } = TaxEnums.EnRaste.EdiriMali;


	public string Semat { get; set; }

	public TaxEnums.EnEstekhdamType EstekhdamType { get; set; } = TaxEnums.EnEstekhdamType.Sayer;


	public string WorkStartDate { get; set; }

	public string WorkEndDate { get; set; }

	public string BazneshastegiDate { get; set; }

	public string ContextGuid { get; }

	public ClsTaxPersonInfo(string contextGuid, Person person)
	{
		try
		{
			ClsPerson = person;
			LoadPersonInfo();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private bool LoadPersonInfo()
	{
		try
		{
			if (ClsPerson == null)
			{
				return false;
			}
			Melliat = 1;
			NationalCode = ((PersonBase)ClsPerson).CodeMeli.Replace("-", "").Replace("_", "");
			Name = ((PersonBase)ClsPerson).Name;
			Family = ((PersonBase)ClsPerson).Family;
			FatherName = ((PersonBase)ClsPerson).FatherName;
			BirthDate = Text.RemoveNoNumbers(Calendar.MiladiToShamsi(((PersonBase)ClsPerson).DateMBirthday), "");
			ShenasnameNumber = ((PersonBase)ClsPerson).Shsh;
			MahalTavalod = ((PersonBase)ClsPerson).MTavalod;
			if (((PersonBase)ClsPerson).MadrakTahsili.Contains("فوق دکتر"))
			{
				MadrakTahsili = 7;
			}
			else if (((PersonBase)ClsPerson).MadrakTahsili.Contains("دکتر"))
			{
				MadrakTahsili = 6;
			}
			else if (((PersonBase)ClsPerson).MadrakTahsili.Contains("ارشد") || ((PersonBase)ClsPerson).MadrakTahsili.Contains("فوق لیسانس"))
			{
				MadrakTahsili = 5;
			}
			else if (((PersonBase)ClsPerson).MadrakTahsili.Contains("کارشناس") || ((PersonBase)ClsPerson).MadrakTahsili.Contains("لیسانس"))
			{
				MadrakTahsili = 4;
			}
			else if (((PersonBase)ClsPerson).MadrakTahsili.Contains("کاردان") || ((PersonBase)ClsPerson).MadrakTahsili.Contains("فوق دیپلم"))
			{
				MadrakTahsili = 3;
			}
			else
			{
				MadrakTahsili = (short)((!((PersonBase)ClsPerson).MadrakTahsili.Contains("دیپلم")) ? 1 : 2);
			}
			BimeType = 2;
			BimeNumber = ((PersonBase)ClsPerson).NoBimeh;
			BimeName = "اجباری";
			MoafiatType = 1.ToString();
			MahalTabeiat = TaxEnums.EnCountry.Iran;
			MahalZendegi = TaxEnums.EnCountry.Iran;
			CodePosti = ((PersonBase)ClsPerson).ZipCode;
			Address = ((PersonBase)ClsPerson).Address;
			Raste = TaxEnums.EnRaste.EdiriMali;
			Semat = "کارشناس";
			EstekhdamType = TaxEnums.EnEstekhdamType.Sayer;
			WorkStartDate = Text.RemoveNoNumbers(Calendar.ToShamsiDate(((PersonBase)ClsPerson).DateMStartWork), "");
			WorkEndDate = Text.RemoveNoNumbers(Calendar.ToShamsiDate(((PersonBase)ClsPerson).DateMFinishW), "");
			BazneshastegiDate = "";
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public string GetTaXRecoedString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			stringBuilder.Append(Melliat);
			stringBuilder.Append(',');
			stringBuilder.Append(NationalCode);
			stringBuilder.Append(",");
			stringBuilder.Append(Name);
			stringBuilder.Append(",");
			stringBuilder.Append(Family);
			stringBuilder.Append(",");
			stringBuilder.Append(FatherName);
			stringBuilder.Append(",");
			stringBuilder.Append(BirthDate);
			stringBuilder.Append(",");
			stringBuilder.Append(ShenasnameNumber);
			stringBuilder.Append(",");
			stringBuilder.Append(MahalTavalod);
			stringBuilder.Append(",");
			stringBuilder.Append(MadrakTahsili);
			stringBuilder.Append(",");
			stringBuilder.Append(BimeType);
			stringBuilder.Append(",");
			stringBuilder.Append(BimeNumber);
			stringBuilder.Append(",");
			stringBuilder.Append(BimeName);
			stringBuilder.Append(",");
			stringBuilder.Append(MoafiatType);
			stringBuilder.Append(",");
			stringBuilder.Append((short)MahalTabeiat);
			stringBuilder.Append(",");
			stringBuilder.Append((short)MahalZendegi);
			stringBuilder.Append(",");
			stringBuilder.Append(CodePosti);
			stringBuilder.Append(",");
			stringBuilder.Append(Address);
			stringBuilder.Append(",");
			stringBuilder.Append((short)Raste);
			stringBuilder.Append(",");
			stringBuilder.Append(Semat);
			stringBuilder.Append(",");
			stringBuilder.Append((short)EstekhdamType);
			stringBuilder.Append(",");
			stringBuilder.Append(WorkStartDate);
			stringBuilder.Append(",");
			stringBuilder.Append(WorkEndDate);
			stringBuilder.Append(",");
			stringBuilder.Append(BazneshastegiDate);
			return stringBuilder.ToString();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}
}

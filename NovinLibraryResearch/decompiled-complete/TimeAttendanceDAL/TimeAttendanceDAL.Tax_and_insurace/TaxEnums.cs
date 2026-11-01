namespace TimeAttendanceDAL.Tax_and_insurace;

public class TaxEnums
{
	public enum EnMelliatType : short
	{
		Irani = 1,
		Khareji
	}

	public enum EnCountry : short
	{
		Otrish = 1,
		Afghanestan = 14,
		Iran = 103
	}

	public enum EnRaste : short
	{
		AmuzeshiFarhangi = 1,
		EdiriMali,
		HeiatElmi,
		OmoorEjtemaei,
		AnbarDari,
		BazarYabi,
		BehdashtDarman,
		Tahghighat
	}

	public enum EnEstekhdamType : short
	{
		Gharardadi = 1,
		Sherkati,
		Rasmi,
		Peymani,
		Mamur,
		Karmozdi,
		KharidKhedmat,
		Saati,
		RoozMozd,
		RasmiAzmayeshi,
		RasmiFasli,
		Sayer
	}
}

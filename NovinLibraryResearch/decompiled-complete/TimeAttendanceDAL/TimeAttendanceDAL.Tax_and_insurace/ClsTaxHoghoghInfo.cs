using System;
using System.Text;
using PacketParser.Attendance.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendanceDAL.Tax_and_insurace;

public class ClsTaxHoghoghInfo
{
	public string NationalCode { get; set; }

	public short PardakhtType { get; set; } = 1;


	public short VazeiatMahalKhedmat { get; set; } = 1;


	public short EstethnaatGhanoonBoodje { get; set; } = 1;


	public int ArzType { get; set; } = 84;


	public int taseerArzType { get; set; } = 1;


	public double SumHoghoghMazayaNaghdi { get; set; }

	public double SumHoghoghMazayaMoavagh { get; set; }

	public int MaskanType { get; set; } = 1;


	public double MaskanCost { get; set; }

	public int CarType { get; set; } = 1;


	public double CarCost { get; set; }

	public double SumMazayaGheirNaghdi { get; set; }

	public double SumMazayaGheirNaghdiMoavvagh { get; set; }

	public double HaghMoshavereh { get; set; }

	public double GharardadPajuheshiCost { get; set; }

	public double EzafeKari { get; set; }

	public double TripCost { get; set; }

	public double ExtraTripCost { get; set; }

	public double Karaneh { get; set; }

	public double Padash { get; set; }

	public double PadashENdOfYear { get; set; }

	public double Eidi { get; set; }

	public double PadashPayanKhedmat { get; set; }

	public double KhesaratEkhraj { get; set; }

	public double BazkharidKhemmat { get; set; }

	public double HaghSanavat { get; set; }

	public double VacationNotUsed { get; set; }

	public double SayerHoghoghMazayaGherMostamar { get; set; }

	public double HoghoghMazayaGheirMostamarNaghdi { get; set; }

	public double HoghoghMazayaGheirMostamarNaghdMoavvagh { get; set; }

	public double GheymatTamamShodehMazayaGheirMostamarGheirNaghdi { get; set; }

	public double MazayaGheirMostamarGheirNaghdiMoavvaghNotMiliat { get; set; }

	public double HaghBimeDarman { get; set; }

	public double HaghBimeOmr { get; set; }

	public double KhalesHoghogh { get; set; }

	public double HaghTadris { get; set; }

	public double HaghKeshik { get; set; }

	public double RefahiAngizeshi { get; set; }

	public double HaghSaay { get; set; }

	public Fish ClsFish { get; set; }

	public ClsTaxHoghoghInfo(string contextGuid, Fish fish)
	{
		try
		{
			if (fish != null)
			{
				LoadPersonInfo();
			}
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
			if (ClsFish == null)
			{
				return false;
			}
			NationalCode = ((PersonBase)ClsFish.ClsPerson).CodeMeli.Replace("-", "").Replace("_", "");
			PardakhtType = 1;
			VazeiatMahalKhedmat = 1;
			EstethnaatGhanoonBoodje = 1;
			ArzType = 84;
			taseerArzType = 1;
			SumHoghoghMazayaNaghdi = (double)ClsFish.SumKarkerd;
			SumHoghoghMazayaMoavagh = 0.0;
			MaskanType = 1;
			MaskanCost = 0.0;
			CarType = 1;
			CarCost = 0.0;
			SumMazayaGheirNaghdi = 0.0;
			SumMazayaGheirNaghdiMoavvagh = 0.0;
			HaghMoshavereh = 0.0;
			GharardadPajuheshiCost = 0.0;
			EzafeKari = (double)ClsFish.SumEzafeKar;
			TripCost = 0.0;
			ExtraTripCost = 0.0;
			Karaneh = 0.0;
			Padash = 0.0;
			PadashENdOfYear = 0.0;
			Eidi = 0.0;
			PadashPayanKhedmat = 0.0;
			KhesaratEkhraj = 0.0;
			BazkharidKhemmat = 0.0;
			HaghSanavat = 0.0;
			VacationNotUsed = 0.0;
			SayerHoghoghMazayaGherMostamar = 0.0;
			HoghoghMazayaGheirMostamarNaghdi = 0.0;
			HoghoghMazayaGheirMostamarNaghdMoavvagh = 0.0;
			GheymatTamamShodehMazayaGheirMostamarGheirNaghdi = 0.0;
			MazayaGheirMostamarGheirNaghdiMoavvaghNotMiliat = 0.0;
			HaghBimeDarman = (double)ClsFish.GetValueEmployerinsurrance();
			HaghBimeOmr = 0.0;
			KhalesHoghogh = (double)ClsFish.SumKhales;
			HaghTadris = 0.0;
			HaghKeshik = 0.0;
			RefahiAngizeshi = 0.0;
			HaghSaay = 0.0;
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
			stringBuilder.Append(NationalCode);
			stringBuilder.Append(',');
			stringBuilder.Append(PardakhtType);
			stringBuilder.Append(",");
			stringBuilder.Append(VazeiatMahalKhedmat);
			stringBuilder.Append(",");
			stringBuilder.Append(EstethnaatGhanoonBoodje);
			stringBuilder.Append(",");
			stringBuilder.Append(ArzType);
			stringBuilder.Append(",");
			stringBuilder.Append(taseerArzType);
			stringBuilder.Append(",");
			stringBuilder.Append(SumHoghoghMazayaNaghdi);
			stringBuilder.Append(",");
			stringBuilder.Append(SumHoghoghMazayaMoavagh);
			stringBuilder.Append(",");
			stringBuilder.Append(MaskanType);
			stringBuilder.Append(",");
			stringBuilder.Append(MaskanCost);
			stringBuilder.Append(",");
			stringBuilder.Append(CarType);
			stringBuilder.Append(",");
			stringBuilder.Append(CarCost);
			stringBuilder.Append(",");
			stringBuilder.Append(SumMazayaGheirNaghdi);
			stringBuilder.Append(",");
			stringBuilder.Append(SumMazayaGheirNaghdiMoavvagh);
			stringBuilder.Append(",");
			stringBuilder.Append(HaghMoshavereh);
			stringBuilder.Append(",");
			stringBuilder.Append(GharardadPajuheshiCost);
			stringBuilder.Append(",");
			stringBuilder.Append(EzafeKari);
			stringBuilder.Append(",");
			stringBuilder.Append(TripCost);
			stringBuilder.Append(",");
			stringBuilder.Append(ExtraTripCost);
			stringBuilder.Append(",");
			stringBuilder.Append(Karaneh);
			stringBuilder.Append(",");
			stringBuilder.Append(Padash);
			stringBuilder.Append(",");
			stringBuilder.Append(PadashENdOfYear);
			stringBuilder.Append(",");
			stringBuilder.Append(Eidi);
			stringBuilder.Append(",");
			stringBuilder.Append(PadashPayanKhedmat);
			stringBuilder.Append(",");
			stringBuilder.Append(KhesaratEkhraj);
			stringBuilder.Append(",");
			stringBuilder.Append(BazkharidKhemmat);
			stringBuilder.Append(",");
			stringBuilder.Append(HaghSanavat);
			stringBuilder.Append(",");
			stringBuilder.Append(VacationNotUsed);
			stringBuilder.Append(",");
			stringBuilder.Append(SayerHoghoghMazayaGherMostamar);
			stringBuilder.Append(",");
			stringBuilder.Append(HoghoghMazayaGheirMostamarNaghdi);
			stringBuilder.Append(",");
			stringBuilder.Append(HoghoghMazayaGheirMostamarNaghdMoavvagh);
			stringBuilder.Append(",");
			stringBuilder.Append(GheymatTamamShodehMazayaGheirMostamarGheirNaghdi);
			stringBuilder.Append(",");
			stringBuilder.Append(MazayaGheirMostamarGheirNaghdiMoavvaghNotMiliat);
			stringBuilder.Append(",");
			stringBuilder.Append(HaghBimeDarman);
			stringBuilder.Append(",");
			stringBuilder.Append(HaghBimeOmr);
			stringBuilder.Append(",");
			stringBuilder.Append(KhalesHoghogh);
			stringBuilder.Append(",");
			stringBuilder.Append(HaghTadris);
			stringBuilder.Append(",");
			stringBuilder.Append(HaghKeshik);
			stringBuilder.Append(",");
			stringBuilder.Append(RefahiAngizeshi);
			stringBuilder.Append(",");
			stringBuilder.Append(HaghSaay);
			return stringBuilder.ToString();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}
}

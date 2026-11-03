using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.Attendance.LaborLaw;

public class WebLaborLawDet : Serializable<WebLaborLawDet>
{
	private static readonly string Url = HttpHelpers.ApiUrl + "LaborLawNew/";

	public short Sal { get; set; }

	public decimal DailyWage { get; set; }

	public decimal Bon { get; set; }

	public float CoefficientInsuranceEmployer { get; set; }

	public float CoefficientInsuranceUnemployment { get; set; }

	public float CoefficientInsuranceInsured { get; set; }

	public short DaysOnMonth { get; set; }

	public decimal OvertimeCoefficient { get; set; }

	public decimal HaghMaskan { get; set; }

	public decimal HaghTahol { get; set; }

	public decimal HaghSpouse { get; set; }

	public string RewardFormul { get; set; }

	public string SevrancePayFormul { get; set; }

	public static Task<ReturnedSaveFuncInfoWithValue<WebResponse<List<WebLaborLawDet>>>> GetFromApi(CancellationToken token)
	{
		return (Url + "GetDetNew").GetFromApi<List<WebLaborLawDet>>(token, 3);
	}
}

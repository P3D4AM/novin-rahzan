namespace TimeAttendanceDAL.Settings;

public class LaborLaw
{
	public int Sal { get; set; }

	public decimal DailyWage { get; set; } = 2388728m;


	public decimal HaghMaskan { get; set; } = 90000000m;


	public decimal HaghSpouse { get; set; } = 7166184m;


	public decimal HaghTahol { get; set; } = 5000000m;


	public decimal Bon { get; set; } = 14000000m;


	public float CoefficientInsuranceEmployer { get; set; } = 0.2f;


	public float CoefficientInsuranceUnemployment { get; set; } = 0.03f;


	public float CoefficientInsuranceInsured { get; set; } = 0.07f;


	public short DaysOnMonth { get; set; } = 30;


	public decimal OvertimeCoefficient { get; set; } = 1.4m;

}

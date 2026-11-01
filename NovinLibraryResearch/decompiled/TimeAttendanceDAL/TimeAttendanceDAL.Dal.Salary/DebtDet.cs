using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Salary;

[Serializable]
[DoNotPruneType]
public class DebtDet
{
	public string DateCaltureDesc { get; set; }

	public int FishId { get; set; }

	public int FishNo { get; set; }

	public string MonthYear { get; set; }

	public decimal Price { get; set; }

	public decimal Remnant { get; set; }

	public async Task<DebtDet> GetAsync(AgreedDeductionDet agreedDeductionDet, CancellationToken token)
	{
		_ = 1;
		try
		{
			DebtDet debtDet = new DebtDet();
			DebtDet debtDet2 = debtDet;
			debtDet2.DateCaltureDesc = await agreedDeductionDet.GetDateCaltureDescAsync(token);
			debtDet.FishId = agreedDeductionDet.FishId;
			DebtDet debtDet3 = debtDet;
			debtDet3.FishNo = await agreedDeductionDet.GetFishNoAsync(token);
			debtDet.MonthYear = agreedDeductionDet.MonthYear;
			debtDet.Price = agreedDeductionDet.Price;
			debtDet.Remnant = agreedDeductionDet.Remnant;
			return debtDet;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}

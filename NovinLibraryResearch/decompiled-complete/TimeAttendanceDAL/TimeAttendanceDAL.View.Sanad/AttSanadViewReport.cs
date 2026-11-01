using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRSanad;

namespace TimeAttendanceDAL.View.Sanad;

[Serializable]
[DoNotPruneType]
public class AttSanadViewReport
{
	public int ID { get; private set; }

	public int No { get; set; }

	public DateTime DateM { get; set; }

	public short UID { get; set; }

	public string Description { get; set; }

	public decimal Sum { get; private set; }

	public List<ATTSanadDetViewReport> lstSanadDets { get; set; }

	public string UserName { get; set; }

	public decimal Sum_Debit { get; set; }

	public decimal Sum_Credit { get; set; }

	public static async Task<AttSanadViewReport> ConvertToViewReportAsync(AttSanad pAttSanad, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			AttSanadViewReport xAttSanadViewReport = new AttSanadViewReport
			{
				ID = pAttSanad.Id,
				No = pAttSanad.No,
				DateM = pAttSanad.DateM,
				UID = pAttSanad.Uid,
				Description = pAttSanad.Description,
				Sum = pAttSanad.Sum
			};
			AttSanadViewReport attSanadViewReport = xAttSanadViewReport;
			attSanadViewReport.UserName = await pAttSanad.GetUserNameAsync(token);
			attSanadViewReport = xAttSanadViewReport;
			attSanadViewReport.Sum_Debit = await pAttSanad.GetSumDebitAsync(token);
			attSanadViewReport = xAttSanadViewReport;
			attSanadViewReport.Sum_Credit = await pAttSanad.GetSumCreditAsync(token);
			attSanadViewReport = xAttSanadViewReport;
			attSanadViewReport.lstSanadDets = (await pAttSanad.GetLstSanadDetsAsync(token)).ConvertAll<ATTSanadDetViewReport>(ATTSanadDetViewReport.ConvertToViewReport);
			return xAttSanadViewReport;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}

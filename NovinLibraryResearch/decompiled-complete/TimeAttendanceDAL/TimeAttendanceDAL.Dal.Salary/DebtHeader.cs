using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Salary;

[Serializable]
[DoNotPruneType]
public class DebtHeader : IHasContext
{
	public string FullName { get; set; }

	public string HesabTitle { get; set; }

	public decimal AmountPayable { get; set; }

	public decimal MonthlyInstallment { get; set; }

	public DateTime DateCreatedM { get; set; }

	public string DateCreatedCalture => Calendar.MiladiToCalture(DateCreatedM);

	public string ContextGuid { get; private set; }

	public DebtHeader(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<DebtHeader> GetAsync(string contextGuid, AgreedDeduction agreedDeduction, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			DebtHeader ret = new DebtHeader(contextGuid);
			DebtHeader debtHeader = ret;
			debtHeader.FullName = await agreedDeduction.GetFullNameAsync(token);
			debtHeader = ret;
			debtHeader.HesabTitle = await agreedDeduction.GetHesabTitleAsync(token);
			ret.AmountPayable = agreedDeduction.AmountPayable;
			ret.MonthlyInstallment = agreedDeduction.MonthlyInstallment;
			ret.DateCreatedM = agreedDeduction.DateCreatedM;
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}

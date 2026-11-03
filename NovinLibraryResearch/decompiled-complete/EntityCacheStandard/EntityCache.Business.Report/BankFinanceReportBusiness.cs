using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Filters;

namespace EntityCache.Business.Report;

public class BankFinanceReportBusiness
{
	public Guid Guid { get; set; }

	public long Code { get; set; }

	public string Name { get; set; }

	public decimal Account { get; set; }

	public string AccountTashkhis
	{
		get
		{
			if (Account > 0m)
			{
				return "بس";
			}
			if (Account < 0m)
			{
				return "بد";
			}
			return "";
		}
	}

	public int ReceptionChequeCount { get; set; }

	public decimal ReceptionChequeSum { get; set; }

	public int PardakhtChequeCount { get; set; }

	public decimal PardakhtChequeSum { get; set; }

	public decimal Remain => Account + ReceptionChequeSum + PardakhtChequeSum;

	public string RemainTashkhis
	{
		get
		{
			if (Remain > 0m)
			{
				return "بس";
			}
			if (Remain < 0m)
			{
				return "بد";
			}
			return "";
		}
	}

	public bool IsPos { get; set; }

	public string Shobe { get; set; }

	public string ShobeCode { get; set; }

	public string HesabNo { get; set; }

	public string HesabType { get; set; }

	public static async Task<IEnumerable<BankFinanceReportBusiness>> GetAllAsync(string contextGuid, DateTime dateM, EnSystem system, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<BankFinanceReportBusiness> list = new List<BankFinanceReportBusiness>();
			IEnumerable<BankBusiness> bankList = await BankBusiness.GetAllAsync(contextGuid, system, "", token, true);
			if (!(bankList?.Any() ?? false))
			{
				return null;
			}
			IEnumerable<ReceptionCheckReportBusiness> receptionChequeList = await ReceptionCheckBusiness.GetAllAsync(contextGuid, new ReceptionChequeFilter
			{
				Status = ENUMCheck.VagozarShode,
				DateTo = dateM
			}, token);
			IEnumerable<PardakhtCheckShReportBusiness> enumerable = await PardakhtCheckShBusiness.GetAllAsync(contextGuid, new PardakhtChequeFilter
			{
				Status = ENCheckSHStatus.Pardakhtani,
				DateTo = dateM
			}, token);
			foreach (BankBusiness bank in bankList)
			{
				BankFinanceReportBusiness bankFinanceReportBusiness = new BankFinanceReportBusiness
				{
					Account = bank.Account,
					Name = bank.Name,
					Code = bank.Code,
					Guid = bank.Guid,
					HesabNo = bank.HesabNo,
					HesabType = bank.HesabType,
					IsPos = bank.IsPos,
					Shobe = bank.Shobe,
					ShobeCode = bank.ShobeCode
				};
				IEnumerable<ReceptionCheckReportBusiness> enumerable2 = receptionChequeList?.Where((ReceptionCheckReportBusiness q) => (q.PlaceGuid ?? Guid.Empty) == bank.Guid);
				IEnumerable<PardakhtCheckShReportBusiness> enumerable3 = enumerable?.Where((PardakhtCheckShReportBusiness q) => q.BankTafsilGuid == bank.Guid);
				if (enumerable2 != null && enumerable2.Any())
				{
					bankFinanceReportBusiness.ReceptionChequeCount = enumerable2.Count();
					bankFinanceReportBusiness.ReceptionChequeSum = enumerable2.Sum((ReceptionCheckReportBusiness q) => q.Price);
				}
				if (enumerable3 != null && enumerable3.Any())
				{
					bankFinanceReportBusiness.PardakhtChequeSum = enumerable3.Sum((PardakhtCheckShReportBusiness q) => q.Price);
					bankFinanceReportBusiness.PardakhtChequeCount = enumerable3.Count();
				}
				list.Add(bankFinanceReportBusiness);
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\BankFinanceReportBusiness.cs", 92);
			throw;
		}
	}
}

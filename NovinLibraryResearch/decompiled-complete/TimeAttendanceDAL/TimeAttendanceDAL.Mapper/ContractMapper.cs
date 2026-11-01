using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;

namespace TimeAttendanceDAL.Mapper;

public class ContractMapper : IItemAsyncMapper<GharardadNew, WebAttContract>
{
	public static ContractMapper Instance { get; } = new ContractMapper();


	public async Task<WebAttContract> MapAsync(string contextGuid, GharardadNew source, WebAttContract destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebAttContract();
			}
			destination.ID = source.ID;
			destination.PersonId = source.PersonId;
			destination.DateStartM = source.DateStartM;
			destination.DateM = source.DateM;
			destination.DateFinishM = source.DateFinishM;
			destination.CountChild = source.CountChild;
			destination.JobId = source.JobId;
			destination.ShiftId = source.ShiftId;
			destination.CompanyId = source.CompanyId;
			destination.BankId = source.BankId;
			destination.BankNo = source.BankNo;
			destination.BankType = source.BankType;
			destination.Description = source.Description;
			destination.IsCheckoutVacation = source.IsCheckoutVacation;
			destination.IsMotahel = source.IsMotahel;
			destination.LeaveDays = source.LeaveDays;
			destination.LeaveMinutes = source.LeaveMinutes;
			destination.GharardadStatus = source.GharardadStatus;
			destination.GroupSanavat = source.GroupSanavat;
			destination.Sanavat = destination.Sanavat;
			destination.No = source.No;
			destination.Married = source.Married;
			destination.LeaveSaved = source.LeaveSaved;
			destination.MoeenEmployerInsurance = source.MoeenEmployerInsurance;
			destination.MoeenInsuredInsurance = source.MoeenInsuredInsurance;
			destination.MoeenUnemploymentInsurance = source.MoeenUnemploymentInsurance;
			destination.PartId = source.PartId;
			destination.Peyvast = source.Peyvast;
			destination.PlaceId = source.PlaceId;
			destination.PriceAbsent = source.PriceAbsent;
			destination.PriceLowTime = source.PriceLowTime;
			destination.PriceHoliday = source.PriceHoliday;
			destination.PriceJarime = source.PriceJarime;
			destination.PriceLowTime2 = source.PriceLowTime2;
			destination.PriceLowTime3 = source.PriceLowTime3;
			destination.PriceMission = source.PriceMission;
			destination.PriceNight = source.PriceNight;
			destination.PriceOverTime = source.PriceOverTime;
			destination.PriceOverTime2 = source.PriceOverTime2;
			destination.PriceOverTime3 = source.PriceOverTime3;
			destination.DateStartCalture = source.DateStartCalture;
			destination.DateFinishCalture = source.DateFinishCalture;
			destination.RecruitmentSt = source.RecruitmentSt;
			destination.RecruitmentType = source.RecruitmentType;
			destination.ShabaNo = source.ShabaNo;
			destination.SetSalaryType(source.SalaryType);
			destination.SalaryOneHour = source.SalaryOneHour;
			destination.SalaryOneDay = source.SalaryOneDay;
			destination.SetSalaryOneMonth(source.SalaryOneMonth);
			destination.SetSalaryPrice(source.SalaryPrice);
			destination.PriceNobatKari1 = source.PriceNobatKari1;
			destination.PriceNobatKari2 = source.PriceNobatKari2;
			destination.PriceNobatKari3 = source.PriceNobatKari3;
			destination.PriceTatil = source.PriceTatil;
			destination.PriceLeave = source.PriceLeave;
			destination.TafsilEmployerInsurance = source.TafsilEmployerInsurance;
			destination.ZaribEmployerInsurance = source.ZaribEmployerInsurance;
			destination.TafsilInsuredInsurance = source.TafsilInsuredInsurance;
			destination.ZaribInsuredInsurance = source.ZaribInsuredInsurance;
			destination.TafsilUnemploymentInsurance = source.TafsilUnemploymentInsurance;
			destination.ZaribUnemploymentInsurance = source.ZaribUnemploymentInsurance;
			destination.InsuranceOption = source.InsuranceOption;
			destination.TaxOption = source.TaxOption;
			destination.SumMazaya = source.SumMazaya;
			destination.SumKosorat = source.SumKosorat;
			destination.SetSumHoghMazaya(source.SumHoghMazaya);
			WebAttContract val = destination;
			val.DetailList = await ContractDetailMapper.Instance.MapListAsync(contextGuid, source.ListGharardadDet, new List<WebAttContractDetail>(), "", token);
			val = destination;
			CompanyMapper instance = CompanyMapper.Instance;
			val.Company = await instance.MapAsync(contextGuid, await source.GetCompanyAsync(contextGuid, token), new WebAttCompany(), "", token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return destination;
	}

	public Task<List<WebAttContract>> MapListAsync(string contextGuid, IEnumerable<GharardadNew> source, List<WebAttContract> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<GharardadNew, WebAttContract>(source, contextGuid, (IItemAsyncMapper<GharardadNew, WebAttContract>)this, destination, tranName, (short)20);
	}
}

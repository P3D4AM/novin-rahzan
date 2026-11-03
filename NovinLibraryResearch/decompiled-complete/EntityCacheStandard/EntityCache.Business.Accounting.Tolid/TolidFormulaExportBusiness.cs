using System;
using EntityCache.Business.Accounting.Tolid.Base;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Base;

namespace EntityCache.Business.Accounting.Tolid;

public class TolidFormulaExportBusiness : BaseExportBusiness, IBaseExportBusiness, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public TolidFormulaExportBusiness(string contextGuid)
		: base(contextGuid)
	{
	}

	public override ReturnedSaveFuncInfo CheckValidation(EnSystem system)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			returnedSaveFuncInfo.AddReturnedValue(base.CheckValidation(system), ReturnedState.None, "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaExportBusiness.cs", 18);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaExportBusiness.cs", 22);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\TolidFormulaExportBusiness.cs", 23);
		}
		return returnedSaveFuncInfo;
	}
}

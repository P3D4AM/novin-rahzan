using System;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.Entities.Interfaces;

namespace LK;

public class LockInformationManager : ILockInformationManager, IHasContext
{
	public string ContextGuid { get; set; }

	public LockInformationManager(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public override async Task<ReturnedSaveFuncInfoWithValue<LKInfo>> CheckLkWithWebServiceAsync(Package package, bool ignoreHttpExceptions, LKInfo lk = null)
	{
		ReturnedSaveFuncInfoWithValue<LKInfo> ret = new ReturnedSaveFuncInfoWithValue<LKInfo>();
		try
		{
			return await Demo.CheckLKWithWebServiceAsync(lk, package, ignoreHttpExceptions: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckLkWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LockInformationManager.cs", 27);
			ret.AddReturnedValue(ex, "", "CheckLkWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LockInformationManager.cs", 28);
		}
		return ret;
	}
}

using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;
using PacketParser.Payamak.Interfaces.Entities;

namespace EntityCache.Business.Accounting.Payamak;

public class SmsOutBoxBusiness : BaseSms, ISmsOutBox, IBaseSms, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public DateTime SchadulingM { get; set; } = DateTime.Now;


	public SMSSchadulingType SchadulingType { get; set; }

	public short Priority { get; set; }

	public int DeletedItemsId { get; set; }

	public int DraftId { get; set; }

	public int SentBoxId { get; set; }

	public bool Delivery { get; set; }

	public string RefrenceAccountId { get; set; }

	public string Rstr4 { get; set; }

	public string Rstr5 { get; set; }

	public string SenderNumber { get; set; }

	public int RefrencedId { get; set; }

	public int AccountId { get; set; }

	public DateTime Modified { get; set; }

	public bool Status
	{
		get
		{
			return base.ST;
		}
		set
		{
			base.ST = value;
		}
	}

	public string ContextGuid { get; set; }

	public SmsOutBoxBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	internal async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool trantionBeenCreate = false;
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autotran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Payamak\\SmsOutBoxBusiness.cs", 43), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Payamak\\SmsOutBoxBusiness.cs", 43);
				}
				trantionBeenCreate = true;
				if (res.HasError)
				{
					return res;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.SmsOutBox.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Payamak\\SmsOutBoxBusiness.cs", 46);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Payamak\\SmsOutBoxBusiness.cs", 50);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Payamak\\SmsOutBoxBusiness.cs", 51);
			}
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Payamak\\SmsOutBoxBusiness.cs", 56), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Payamak\\SmsOutBoxBusiness.cs", 56);
			}
		}
		return res;
	}
}

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Core;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Core;

public abstract class BaseBusiness<T> : Serializable<T>, IHasContext, IEditableDelivery, IHasChangeStatus
{
	private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 20);

	public abstract string ContextGuid { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public abstract IDatabaseManager DatabaseManager { get; }

	public virtual EntityChangeSt ChangeStatus { get; set; }

	public virtual async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		int line = 0;
		bool semaphoreHasBeenTaken = false;
		try
		{
			_ = 2;
			try
			{
				await _semaphore.WaitAsync();
				semaphoreHasBeenTaken = true;
				IEditableDelivery repository = GetRepository();
				line = 1;
				if (repository == null)
				{
					throw new NullReferenceException("repo is null!!");
				}
				line = 2;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					line = 3;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await DatabaseManager.BeginTransactionAsync(tranName, 60000, "SetDeliveryAsync", $"BaseBusiness_{repository.GetType()}", 40), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseBusiness.cs", 40);
					transactionHasBeenCreated = true;
				}
				line = 4;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await GetRepository().SetDeliveryAsync(del, token, tryCount, tranName), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseBusiness.cs", 44);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, $"#Line_{line}", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseBusiness.cs", 46);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "RepositoryType:" + GetRepository().GetType().ToString(), "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseBusiness.cs", 49);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseBusiness.cs", 50);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseBusiness.cs", 55), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Core\\BaseBusiness.cs", 55);
			}
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return ret;
	}

	public abstract IEditableDelivery GetRepository();
}

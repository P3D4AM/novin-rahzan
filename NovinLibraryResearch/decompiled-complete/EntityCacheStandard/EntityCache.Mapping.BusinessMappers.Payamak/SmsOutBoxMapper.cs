using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Payamak;
using PacketParser.Entities;
using PacketParser.Mapping;
using PacketParser.Payamak.Entities;

namespace EntityCache.Mapping.BusinessMappers.Payamak;

public class SmsOutBoxMapper : IItemAsyncMapper<SmsOutBoxBusiness, SmsOutBoxBusiness>, IItemAsyncMapper<SmsOutBoxBusiness, BaseSmsOutBox>
{
	public static SmsOutBoxMapper Instance { get; private set; } = new SmsOutBoxMapper();


	public Task<SmsOutBoxBusiness> MapAsync(string contextGuid, SmsOutBoxBusiness source, SmsOutBoxBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new SmsOutBoxBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.ID = source.ID;
			destination.Number = source.Number;
			destination.Message = source.Message;
			destination.DateM = source.DateM;
			destination.CustomerId = source.CustomerId;
			destination.ST = source.ST;
			destination.SchadulingM = source.SchadulingM;
			destination.SchadulingType = source.SchadulingType;
			destination.Priority = source.Priority;
			destination.DeletedItemsId = source.DeletedItemsId;
			destination.DraftId = source.DraftId;
			destination.SentBoxId = source.SentBoxId;
			destination.Delivery = source.Delivery;
			destination.RefrenceAccountId = source.RefrenceAccountId;
			destination.Rstr4 = source.Rstr4;
			destination.Rstr5 = source.Rstr5;
			destination.SenderNumber = source.SenderNumber;
			destination.RefrencedId = source.RefrencedId;
			destination.AccountId = source.AccountId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Payamak\\SmsOutBoxMapper.cs", 52);
			throw;
		}
	}

	public Task<BaseSmsOutBox> MapAsync(string contextGuid, SmsOutBoxBusiness source, BaseSmsOutBox destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new BaseSmsOutBox();
			}
			destination.ID = source.ID;
			destination.Number = source.Number;
			destination.Message = source.Message;
			destination.DateM = source.DateM;
			destination.CustomerId = source.CustomerId;
			destination.ST = source.ST;
			destination.SchadulingM = source.SchadulingM;
			destination.SchadulingType = source.SchadulingType;
			destination.Priority = source.Priority;
			destination.DeletedItemsId = source.DeletedItemsId;
			destination.DraftId = source.DraftId;
			destination.SentBoxId = source.SentBoxId;
			destination.Delivery = source.Delivery;
			destination.RefrenceAccountId = source.RefrenceAccountId;
			destination.Rstr4 = source.Rstr4;
			destination.Rstr5 = source.Rstr5;
			destination.SenderNumber = source.SenderNumber;
			destination.RefrencedId = source.RefrencedId;
			destination.AccountId = source.AccountId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Payamak\\SmsOutBoxMapper.cs", 89);
			throw;
		}
	}

	public Task<List<SmsOutBoxBusiness>> MapListAsync(string contextGuid, IEnumerable<SmsOutBoxBusiness> source, List<SmsOutBoxBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<BaseSmsOutBox>> MapListAsync(string contextGuid, IEnumerable<SmsOutBoxBusiness> source, List<BaseSmsOutBox> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

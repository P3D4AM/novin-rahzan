using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Mapping;
using PacketParser.Payamak.Entities;
using PayamakUi.Classes.Entities;

namespace PayamakUi.Mapper;

public class SmsOutBoxMapper : IItemAsyncMapper<BaseSmsOutBox, SmsOutBox>
{
	public static SmsOutBoxMapper Instance { get; private set; } = new SmsOutBoxMapper();


	public Task<SmsOutBox> MapAsync(string contextGuid, BaseSmsOutBox source, SmsOutBox destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SmsOutBox("", "", 0);
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
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Mapper\\SmsOutBoxMapper.cs", 49);
			throw;
		}
	}

	public Task<List<SmsOutBox>> MapListAsync(string contextGuid, IEnumerable<BaseSmsOutBox> source, List<SmsOutBox> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

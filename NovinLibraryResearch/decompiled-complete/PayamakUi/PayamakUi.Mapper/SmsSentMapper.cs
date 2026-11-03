using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Mapping;
using PacketParser.Payamak.Entities;
using PayamakUi.Classes.Entities;

namespace PayamakUi.Mapper;

public class SmsSentMapper : IItemAsyncMapper<BaseSmsSent, SmsSent>
{
	public static SmsSentMapper Instance { get; private set; } = new SmsSentMapper();


	public Task<SmsSent> MapAsync(string contextGuid, BaseSmsSent source, SmsSent destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SmsSent();
			}
			destination.ID = source.ID;
			destination.Number = source.Number;
			destination.Message = source.Message;
			destination.DateM = source.DateM;
			destination.CustomerId = source.CustomerId;
			destination.ST = source.ST;
			destination.SenderNumber = source.SenderNumber;
			destination.SendDateM = source.SendDateM;
			destination.DeliverySt = source.DeliverySt;
			destination.DeliveryRequested = source.DeliveryRequested;
			destination.RefrencedIds = source.RefrencedIds;
			destination.SenderNumber = source.SenderNumber;
			destination.RefrenseAccountId = source.RefrenseAccountId;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Mapper\\SmsSentMapper.cs", 43);
			throw;
		}
	}

	public Task<List<SmsSent>> MapListAsync(string contextGuid, IEnumerable<BaseSmsSent> source, List<SmsSent> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

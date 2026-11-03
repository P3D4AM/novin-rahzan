using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class Send2FriendMapper : IItemAsyncMapper<Send2FriendBusiness, WebSend2Friend>, IItemAsyncMapper<Send2FriendBusiness, Send2FriendBusiness>, IItemAsyncMapper<WebSend2Friend, Send2FriendBusiness>
{
	public static Send2FriendMapper Instance { get; private set; } = new Send2FriendMapper();


	public Task<WebSend2Friend> MapAsync(string contextGuid, Send2FriendBusiness source, WebSend2Friend destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebSend2Friend();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CustomerGuid = source.CustomerGuid;
			destination.DateM = source.DateM;
			destination.Email = source.Email;
			destination.Family = source.Family;
			destination.Gender = source.Gender;
			destination.Ip = source.Ip;
			destination.Message = source.Message;
			destination.Mob = source.Mob;
			destination.Name = source.Name;
			destination.SmsId = source.SmsId;
			destination.SmsSt = source.SmsSt;
			destination.Type = source.Type;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Send2FriendMapper.cs", 44);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebSend2Friend>> MapListAsync(string contextGuid, IEnumerable<Send2FriendBusiness> source, List<WebSend2Friend> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<Send2FriendBusiness> MapAsync(string contextGuid, Send2FriendBusiness source, Send2FriendBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Send2FriendBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CustomerGuid = source.CustomerGuid;
			destination.DateM = source.DateM;
			destination.Email = source.Email;
			destination.Family = source.Family;
			destination.Gender = source.Gender;
			destination.Ip = source.Ip;
			destination.Message = source.Message;
			destination.Mob = source.Mob;
			destination.Name = source.Name;
			destination.SmsId = source.SmsId;
			destination.SmsSt = source.SmsSt;
			destination.Type = source.Type;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Send2FriendMapper.cs", 81);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Send2FriendBusiness>> MapListAsync(string contextGuid, IEnumerable<Send2FriendBusiness> source, List<Send2FriendBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<Send2FriendBusiness> MapAsync(string contextGuid, WebSend2Friend source, Send2FriendBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Send2FriendBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CustomerGuid = source.CustomerGuid;
			destination.DateM = source.DateM;
			destination.Email = source.Email;
			destination.Family = source.Family;
			destination.Gender = source.Gender;
			destination.Ip = source.Ip;
			destination.Message = source.Message;
			destination.Mob = source.Mob;
			destination.Name = source.Name;
			destination.SmsId = source.SmsId;
			destination.SmsSt = source.SmsSt;
			destination.Type = source.Type;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Send2FriendMapper.cs", 118);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Send2FriendBusiness>> MapListAsync(string contextGuid, IEnumerable<WebSend2Friend> source, List<Send2FriendBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

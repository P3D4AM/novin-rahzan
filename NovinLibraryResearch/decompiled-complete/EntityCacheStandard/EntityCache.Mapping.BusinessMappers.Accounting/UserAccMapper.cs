using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.Access;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class UserAccMapper : IItemAsyncMapper<UserAccBusiness, WebUserAcc>, IItemAsyncMapper<UserAccBusiness, UserAccBusiness>, IItemAsyncMapper<WebUserAcc, UserAccBusiness>
{
	public static UserAccMapper Instance { get; private set; } = new UserAccMapper();


	public async Task<WebUserAcc> MapAsync(string contextGuid, UserAccBusiness source, WebUserAcc destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebUserAcc();
			}
			destination.Name = source.Name;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.Guid = source.Guid;
			destination.UserAccess = Json.ToStringJson(source.UserAccess).FromJson<AccessLevel>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\UserAccMapper.cs", 35);
			destination.UserTime = source.UserTime;
			WebUserAcc webUserAcc = destination;
			webUserAcc.SafePassword = (await CashierBusiness.GetAsync(contextGuid, source.Guid, token, tranName))?.HashedPassword;
			destination.ServerStatus = source.ServerStatus;
			IEnumerable<CashierBusiness> source2 = await CashierBusiness.GetAllAsync(contextGuid, source.Guid, null, token, tranName);
			webUserAcc = destination;
			webUserAcc.CashierList = await CashierMapper.Instance.MapListAsync(contextGuid, source2, new List<WebCashier>(), tranName, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\UserAccMapper.cs", 45);
			throw;
		}
		return destination;
	}

	public Task<UserAccBusiness> MapAsync(string contextGuid, UserAccBusiness source, UserAccBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new UserAccBusiness(contextGuid, source.Guid);
			}
			destination.ContextGuid = contextGuid;
			destination.Name = source.Name;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.UserAccess = source.UserAccess;
			destination.UserTime = source.UserTime;
			destination.ServerStatus = source.ServerStatus;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\UserAccMapper.cs", 71);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<UserAccBusiness> MapAsync(string contextGuid, WebUserAcc source, UserAccBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new UserAccBusiness(contextGuid, source.Guid);
			}
			destination.ContextGuid = contextGuid;
			destination.UserAccess = Json.ToStringJson(source.UserAccess).FromJson<AccessLevel>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\UserAccMapper.cs", 88);
			destination.Name = source.Name;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.UserTime = source.UserTime;
			destination.ServerStatus = source.ServerStatus;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\UserAccMapper.cs", 98);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebUserAcc>> MapListAsync(string contextGuid, IEnumerable<UserAccBusiness> source, List<WebUserAcc> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<UserAccBusiness>> MapListAsync(string contextGuid, IEnumerable<UserAccBusiness> source, List<UserAccBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<UserAccBusiness>> MapListAsync(string contextGuid, IEnumerable<WebUserAcc> source, List<UserAccBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

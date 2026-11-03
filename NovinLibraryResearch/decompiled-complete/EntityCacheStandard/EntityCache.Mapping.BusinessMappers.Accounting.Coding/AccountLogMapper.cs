using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Coding;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Coding;

public class AccountLogMapper : IItemAsyncMapper<AccountLogBusiness, AccountLogBusiness>
{
	public static AccountLogMapper Instance { get; private set; } = new AccountLogMapper();


	public Task<AccountLogBusiness> MapAsync(string contextGuid, AccountLogBusiness source, AccountLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AccountLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Date = source.Date;
			destination.NewAccount = source.NewAccount;
			destination.OldAccount = source.OldAccount;
			destination.DebuggerInfo = source.DebuggerInfo;
			destination.Action = source.Action;
			destination.Modified = source.Modified;
			destination.AsType = source.AsType;
			destination.AsGuid = source.AsGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\AccountLogMapper.cs", 41);
			throw;
		}
	}

	public Task<List<AccountLogBusiness>> MapListAsync(string contextGuid, IEnumerable<AccountLogBusiness> source, List<AccountLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

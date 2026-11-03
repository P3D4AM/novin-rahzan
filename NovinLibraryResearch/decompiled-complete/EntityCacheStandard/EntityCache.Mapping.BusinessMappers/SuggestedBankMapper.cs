using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class SuggestedBankMapper : IItemAsyncMapper<SuggestedBankBusiness, WebSuggestedBank>, IItemAsyncMapper<SuggestedBankBusiness, SuggestedBankBusiness>, IItemAsyncMapper<WebSuggestedBank, SuggestedBankBusiness>
{
	public static SuggestedBankMapper Instance { get; private set; } = new SuggestedBankMapper();


	public Task<WebSuggestedBank> MapAsync(string contextGuid, SuggestedBankBusiness source, WebSuggestedBank destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebSuggestedBank();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ShobeCode = source.ShobeCode;
			destination.AccountNo = source.AccountNo;
			destination.BankName = source.BankName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\SuggestedBankMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebSuggestedBank>> MapListAsync(string contextGuid, IEnumerable<SuggestedBankBusiness> source, List<WebSuggestedBank> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<SuggestedBankBusiness> MapAsync(string contextGuid, SuggestedBankBusiness source, SuggestedBankBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SuggestedBankBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ShobeCode = source.ShobeCode;
			destination.AccountNo = source.AccountNo;
			destination.BankName = source.BankName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\SuggestedBankMapper.cs", 63);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<SuggestedBankBusiness>> MapListAsync(string contextGuid, IEnumerable<SuggestedBankBusiness> source, List<SuggestedBankBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<SuggestedBankBusiness> MapAsync(string contextGuid, WebSuggestedBank source, SuggestedBankBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SuggestedBankBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ShobeCode = source.ShobeCode;
			destination.AccountNo = source.AccountNo;
			destination.BankName = source.BankName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\SuggestedBankMapper.cs", 91);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<SuggestedBankBusiness>> MapListAsync(string contextGuid, IEnumerable<WebSuggestedBank> source, List<SuggestedBankBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

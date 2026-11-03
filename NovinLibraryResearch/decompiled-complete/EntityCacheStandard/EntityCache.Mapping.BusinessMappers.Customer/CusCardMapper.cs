using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Customers;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Customer;

public class CusCardMapper : IItemAsyncMapper<CusCardBusiness, WebCusCard>, IItemAsyncMapper<CusCardBusiness, CusCardBusiness>, IItemAsyncMapper<WebCusCard, CusCardBusiness>
{
	public static CusCardMapper Instance { get; private set; } = new CusCardMapper();


	public Task<WebCusCard> MapAsync(string contextGuid, CusCardBusiness source, WebCusCard destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCusCard();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CusGuid = source.CusGuid;
			destination.CardName = source.CardName;
			destination.CardNo = source.CardNo;
			destination.Status = source.Status;
			destination.Pass = source.Pass;
			destination.DateStart = source.DateStart;
			destination.DateExpire = source.DateExpire;
			destination.Description = source.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CusCardMapper.cs", 40);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebCusCard>> MapListAsync(string contextGuid, IEnumerable<CusCardBusiness> source, List<WebCusCard> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<CusCardBusiness> MapAsync(string contextGuid, CusCardBusiness source, CusCardBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CusCardBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CusGuid = source.CusGuid;
			destination.CardName = source.CardName;
			destination.CardNo = source.CardNo;
			destination.Status = source.Status;
			destination.Pass = source.Pass;
			destination.DateStart = source.DateStart;
			destination.DateExpire = source.DateExpire;
			destination.Description = source.Description;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CusCardMapper.cs", 73);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CusCardBusiness>> MapListAsync(string contextGuid, IEnumerable<CusCardBusiness> source, List<CusCardBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<CusCardBusiness> MapAsync(string contextGuid, WebCusCard source, CusCardBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new CusCardBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.CusGuid = source.CusGuid;
			destination.CardName = source.CardName;
			destination.CardNo = source.CardNo;
			destination.Status = source.Status;
			destination.Pass = source.Pass;
			destination.DateStart = source.DateStart;
			destination.DateExpire = source.DateExpire;
			destination.Description = source.Description;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CusCardMapper.cs", 107);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CusCardBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCusCard> source, List<CusCardBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

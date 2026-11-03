using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Cheque;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Cheque;

public class ChequeBookMapper : IItemAsyncMapper<ChequeBookBusiness, WebChequeBook>, IItemAsyncMapper<ChequeBookBusiness, ChequeBookBusiness>, IItemAsyncMapper<WebChequeBook, ChequeBookBusiness>
{
	public static ChequeBookMapper Instance { get; private set; } = new ChequeBookMapper();


	public Task<WebChequeBook> MapAsync(string contextGuid, ChequeBookBusiness source, WebChequeBook destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebChequeBook();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.SerialNumber = source.SerialNumber;
			destination.BankGuid = source.BankGuid;
			destination.IsSaderShode = source.IsSaderShode;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Cheque\\ChequeBookMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<ChequeBookBusiness> MapAsync(string contextGuid, ChequeBookBusiness source, ChequeBookBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ChequeBookBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.SerialNumber = source.SerialNumber;
			destination.BankGuid = source.BankGuid;
			destination.IsSaderShode = source.IsSaderShode;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Cheque\\ChequeBookMapper.cs", 60);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<ChequeBookBusiness> MapAsync(string contextGuid, WebChequeBook source, ChequeBookBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ChequeBookBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.SerialNumber = source.SerialNumber;
			destination.BankGuid = source.BankGuid;
			destination.IsSaderShode = source.IsSaderShode;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Cheque\\ChequeBookMapper.cs", 85);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebChequeBook>> MapListAsync(string contextGuid, IEnumerable<ChequeBookBusiness> source, List<WebChequeBook> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ChequeBookBusiness>> MapListAsync(string contextGuid, IEnumerable<ChequeBookBusiness> source, List<ChequeBookBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ChequeBookBusiness>> MapListAsync(string contextGuid, IEnumerable<WebChequeBook> source, List<ChequeBookBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

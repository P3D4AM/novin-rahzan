using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class ResidMapper : IItemAsyncMapper<ResidBusiness, WebResid>, IItemAsyncMapper<ResidBusiness, ResidBusiness>, IItemAsyncMapper<WebResid, ResidBusiness>
{
	public static ResidMapper Instance { get; private set; } = new ResidMapper();


	public Task<WebResid> MapAsync(string contextGuid, ResidBusiness source, WebResid destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebResid();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ID2 = source.ID2;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.Date1 = source.Date1;
			destination.Date2 = source.Date2;
			destination.GuidVahed = source.GuidVahed;
			destination.Price = source.Price;
			destination.Qt = source.Qt;
			destination.Mandeh = source.Mandeh;
			destination.PrdGuid = source.GetPrdGuid();
			destination.PrdModel = source.PrdModel;
			destination.PrdColor = source.PrdColor;
			destination.PrdSerial = source.PrdSerial;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Problems = source.Problems;
			destination.Darkhast = source.Darkhast;
			destination.StatusId = source.StatusId;
			destination.CashierGuid = source.CashierGuid;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.OrderGuid = source.OrderGuid;
			destination.Reserve = source.Reserve;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ResidMapper.cs", 51);
			throw;
		}
		return Task.FromResult(destination);
	}

	public async Task<ResidBusiness> MapAsync(string contextGuid, ResidBusiness source, ResidBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ResidBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ID2 = source.ID2;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.Date1 = source.Date1;
			destination.Date2 = source.Date2;
			await destination.SetGuidVahedAsync(source.GuidVahed);
			destination.Price = source.Price;
			destination.Qt = source.Qt;
			destination.Mandeh = source.Mandeh;
			await destination.SetPrdGuidAsync(source.GetPrdGuid(), EnSystem.Mapper);
			destination.PrdModel = source.PrdModel;
			destination.PrdColor = source.PrdColor;
			destination.PrdSerial = source.PrdSerial;
			await destination.SetTafsilGuidAsync(source.TafsilGuid, EnSystem.Mapper);
			destination.Problems = source.Problems;
			destination.Darkhast = source.Darkhast;
			destination.StatusId = source.StatusId;
			destination.CashierGuid = source.CashierGuid;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.OrderGuid = source.OrderGuid;
			destination.Reserve = source.Reserve;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ResidMapper.cs", 93);
			throw;
		}
		return destination;
	}

	public async Task<ResidBusiness> MapAsync(string contextGuid, WebResid source, ResidBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ResidBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ID2 = source.ID2;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.Date1 = source.Date1;
			destination.Date2 = source.Date2;
			await destination.SetGuidVahedAsync(source.GuidVahed);
			destination.Price = source.Price;
			destination.Qt = source.Qt;
			destination.Mandeh = source.Mandeh;
			await destination.SetPrdGuidAsync(source.PrdGuid, EnSystem.Mapper);
			destination.PrdModel = source.PrdModel;
			destination.PrdColor = source.PrdColor;
			destination.PrdSerial = source.PrdSerial;
			await destination.SetTafsilGuidAsync(source.TafsilGuid, EnSystem.Mapper);
			destination.Problems = source.Problems;
			destination.Darkhast = source.Darkhast;
			destination.StatusId = source.StatusId;
			destination.CashierGuid = source.CashierGuid;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.OrderGuid = source.OrderGuid;
			destination.Reserve = source.Reserve;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ResidMapper.cs", 135);
			throw;
		}
		return destination;
	}

	public Task<List<WebResid>> MapListAsync(string contextGuid, IEnumerable<ResidBusiness> source, List<WebResid> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ResidBusiness>> MapListAsync(string contextGuid, IEnumerable<ResidBusiness> source, List<ResidBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ResidBusiness>> MapListAsync(string contextGuid, IEnumerable<WebResid> source, List<ResidBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

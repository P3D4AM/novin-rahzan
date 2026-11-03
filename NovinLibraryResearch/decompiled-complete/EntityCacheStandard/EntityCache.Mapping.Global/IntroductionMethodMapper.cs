using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class IntroductionMethodMapper : IItemAsyncMapper<IntroductionMethodBusiness, WebIntroductionMethod>, IItemAsyncMapper<IntroductionMethodBusiness, IntroductionMethodBusiness>, IItemAsyncMapper<WebIntroductionMethod, IntroductionMethodBusiness>
{
	public static IntroductionMethodMapper Instance { get; private set; } = new IntroductionMethodMapper();


	public Task<WebIntroductionMethod> MapAsync(string contextGuid, IntroductionMethodBusiness source, WebIntroductionMethod destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebIntroductionMethod();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\IntroductionMethodMapper.cs", 40);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<IntroductionMethodBusiness> MapAsync(string contextGuid, IntroductionMethodBusiness source, IntroductionMethodBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new IntroductionMethodBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\IntroductionMethodMapper.cs", 70);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<IntroductionMethodBusiness> MapAsync(string contextGuid, WebIntroductionMethod source, IntroductionMethodBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new IntroductionMethodBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\IntroductionMethodMapper.cs", 99);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebIntroductionMethod>> MapListAsync(string contextGuid, IEnumerable<IntroductionMethodBusiness> source, List<WebIntroductionMethod> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<IntroductionMethodBusiness>> MapListAsync(string contextGuid, IEnumerable<IntroductionMethodBusiness> source, List<IntroductionMethodBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<IntroductionMethodBusiness>> MapListAsync(string contextGuid, IEnumerable<WebIntroductionMethod> source, List<IntroductionMethodBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

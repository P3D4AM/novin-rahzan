using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class AdvDepartmentMapper : IItemAsyncMapper<AdvDepartmentBusiness, AdvDepartmentBusiness>
{
	public static AdvDepartmentMapper Instance { get; private set; } = new AdvDepartmentMapper();


	public async Task<AdvDepartmentBusiness> MapAsync(string contextGuid, AdvDepartmentBusiness source, AdvDepartmentBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new AdvDepartmentBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.ChatList = source.ChatList;
			destination.ChatData = source.ChatData;
			destination.IsSendChat = source.IsSendChat;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.ChatCount = source.ChatCount;
			destination.ChatCity = source?.ChatCity;
			destination.ChatCityData = source.ChatCityData;
			AdvDepartmentBusiness advDepartmentBusiness = destination;
			advDepartmentBusiness.SetVisitList(await source.GetVisitorListAsync(tranName));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\AdvDepartmentMapper.cs", 41);
			throw;
		}
		return destination;
	}

	public Task<List<AdvDepartmentBusiness>> MapListAsync(string contextGuid, IEnumerable<AdvDepartmentBusiness> source, List<AdvDepartmentBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

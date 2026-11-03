using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class PostMapper : IItemAsyncMapper<PostBusiness, PostBusiness>, IItemAsyncMapper<WebPost, PostBusiness>
{
	public static PostMapper Instance { get; set; } = new PostMapper();


	public Task<PostBusiness> MapAsync(string contextGuid, PostBusiness source, PostBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PostBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Startdate = source.Startdate;
			destination.Status = source.Status;
			destination.EndDate = source.EndDate;
			destination.Guid = destination.Guid;
			destination.Image = source.Image;
			destination.Link = source.Link;
			destination.Type = source.Type;
			destination.Title = source.Title;
			destination.Modified = source.Modified;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\PostMapper.cs", 40);
			throw;
		}
	}

	public Task<PostBusiness> MapAsync(string contextGuid, WebPost source, PostBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PostBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Startdate = source.Startdate;
			destination.Status = source.Status;
			destination.EndDate = source.EndDate;
			destination.Guid = destination.Guid;
			destination.Image = source.Image;
			destination.Link = source.Link;
			destination.Type = source.Type;
			destination.Title = source.Title;
			destination.Modified = source.Modified;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\PostMapper.cs", 70);
			throw;
		}
	}

	public Task<List<PostBusiness>> MapListAsync(string contextGuid, IEnumerable<PostBusiness> source, List<PostBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PostBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPost> source, List<PostBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

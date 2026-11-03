using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class NewsMapper : IItemAsyncMapper<NewsBusiness, WebNews>, IItemAsyncMapper<NewsBusiness, NewsBusiness>, IItemAsyncMapper<WebNews, NewsBusiness>
{
	public static NewsMapper Instance { get; private set; } = new NewsMapper();


	public Task<WebNews> MapAsync(string contextGuid, NewsBusiness source, WebNews destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebNews();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Category = source.Category;
			destination.DateM = source.DateM;
			destination.FullContent = source.FullContent;
			destination.KeyWords = source.KeyWords;
			destination.MetaDescription = source.MetaDescription;
			destination.PicUrl = source.PicUrl;
			destination.QuickText = source.QuickText;
			destination.Title = source.Title;
			destination.Type = source.Type;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\NewsMapper.cs", 41);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebNews>> MapListAsync(string contextGuid, IEnumerable<NewsBusiness> source, List<WebNews> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<NewsBusiness> MapAsync(string contextGuid, NewsBusiness source, NewsBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new NewsBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Category = source.Category;
			destination.DateM = source.DateM;
			destination.FullContent = source.FullContent;
			destination.KeyWords = source.KeyWords;
			destination.MetaDescription = source.MetaDescription;
			destination.PicUrl = source.PicUrl;
			destination.QuickText = source.QuickText;
			destination.Title = source.Title;
			destination.Type = source.Type;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\NewsMapper.cs", 76);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<NewsBusiness>> MapListAsync(string contextGuid, IEnumerable<NewsBusiness> source, List<NewsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<NewsBusiness> MapAsync(string contextGuid, WebNews source, NewsBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new NewsBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Category = source.Category;
			destination.DateM = source.DateM;
			destination.FullContent = source.FullContent;
			destination.KeyWords = source.KeyWords;
			destination.MetaDescription = source.MetaDescription;
			destination.PicUrl = source.PicUrl;
			destination.QuickText = source.QuickText;
			destination.Title = source.Title;
			destination.Type = source.Type;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\NewsMapper.cs", 111);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<NewsBusiness>> MapListAsync(string contextGuid, IEnumerable<WebNews> source, List<NewsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

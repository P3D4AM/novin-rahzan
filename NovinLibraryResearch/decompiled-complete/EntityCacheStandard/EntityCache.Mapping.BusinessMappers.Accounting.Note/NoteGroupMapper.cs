using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Note;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Note;

public class NoteGroupMapper : IItemAsyncMapper<NoteGroupBusiness, WebNoteGroup>, IItemAsyncMapper<NoteGroupBusiness, NoteGroupBusiness>, IItemAsyncMapper<WebNoteGroup, NoteGroupBusiness>
{
	public static NoteGroupMapper Instance { get; private set; } = new NoteGroupMapper();


	public Task<WebNoteGroup> MapAsync(string contextGuid, NoteGroupBusiness source, WebNoteGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebNoteGroup();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ParentGuid = source.ParentGuid;
			destination.Name = source.Name;
			destination.Status = source.Status;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Note\\NoteGroupMapper.cs", 36);
			throw;
		}
	}

	public Task<NoteGroupBusiness> MapAsync(string contextGuid, NoteGroupBusiness source, NoteGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new NoteGroupBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ParentGuid = source.ParentGuid;
			destination.Name = source.Name;
			destination.Status = source.Status;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Note\\NoteGroupMapper.cs", 61);
			throw;
		}
	}

	public Task<NoteGroupBusiness> MapAsync(string contextGuid, WebNoteGroup source, NoteGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new NoteGroupBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ParentGuid = source.ParentGuid;
			destination.Name = source.Name;
			destination.Status = source.Status;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Note\\NoteGroupMapper.cs", 88);
			throw;
		}
	}

	public Task<List<WebNoteGroup>> MapListAsync(string contextGuid, IEnumerable<NoteGroupBusiness> source, List<WebNoteGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<NoteGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<NoteGroupBusiness> source, List<NoteGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<NoteGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<WebNoteGroup> source, List<NoteGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

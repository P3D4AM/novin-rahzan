using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Note;
using EntityCache.Models;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Note;

public class NoteMapper : IItemAsyncMapper<NoteBusiness, WebNote>, IItemAsyncMapper<NoteBusiness, NoteBusiness>, IItemAsyncMapper<WebNote, NoteBusiness>
{
	public static NoteMapper Instance { get; private set; } = new NoteMapper();


	public async Task<WebNote> MapAsync(string contextGuid, NoteBusiness source, WebNote destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebNote();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.Text = source.Text;
			destination.DateM = source.DateM;
			destination.DateSarResidM = source.DateSarResidM;
			destination.Pirority = source.Pirority;
			destination.Status = source.Status;
			destination.CashierGuid = source.CashierGuid;
			destination.GroupGuid = source.GroupGuid;
			destination.EntityGuid = source.EntityGuid;
			destination.AsType = source.EntityType;
			destination.PirorityName = source.PirorityName;
			destination.StatusName = source.StatusName;
			destination.DisplayState = source.DisplayState;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DateUtc = source.DateUtc;
			destination.TafsilGuid = source.TafsilGuid;
			if (source.GroupGuid != Guid.Empty)
			{
				destination.GroupName = (await NoteGroupBusiness.GetAsync(contextGuid, source.GroupGuid, token, tranName))?.Name ?? "";
			}
			if (source.MetaData != null)
			{
				destination.MetaData = new WebNoteMetaData();
				if (source.MetaData.CheckList?.Any() ?? false)
				{
					destination.MetaData.CheckList = new List<PacketParser.BusinessLayer.Models.CheckListModel>();
					foreach (EntityCache.Models.CheckListModel check in source.MetaData.CheckList)
					{
						destination.MetaData.CheckList.Add(new PacketParser.BusinessLayer.Models.CheckListModel
						{
							Ordinal = check.Ordinal,
							Title = check.Title,
							IsChecked = check.IsChecked
						});
					}
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Note\\NoteMapper.cs", 78);
			throw;
		}
	}

	public Task<NoteBusiness> MapAsync(string contextGuid, NoteBusiness source, NoteBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new NoteBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.Text = source.Text;
			destination.DateM = source.DateM;
			destination.DateSarResidM = source.DateSarResidM;
			destination.Pirority = source.Pirority;
			destination.Status = source.Status;
			destination.CashierGuid = source.CashierGuid;
			destination.GroupGuid = source.GroupGuid;
			destination.DisplayState = source.DisplayState;
			destination.EntityGuid = source.EntityGuid;
			destination.EntityType = source.EntityType;
			destination.IsChecked = source.IsChecked;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DateUtc = source.DateUtc;
			destination.TafsilGuid = source.TafsilGuid;
			if (source.MetaData != null)
			{
				destination.MetaData = new NoteMetaData();
				List<EntityCache.Models.CheckListModel> checkList = source.MetaData.CheckList;
				if (checkList != null && checkList.Any())
				{
					destination.MetaData.CheckList = new List<EntityCache.Models.CheckListModel>();
					foreach (EntityCache.Models.CheckListModel check in source.MetaData.CheckList)
					{
						destination.MetaData.CheckList.Add(new EntityCache.Models.CheckListModel
						{
							Ordinal = check.Ordinal,
							Title = check.Title,
							IsChecked = check.IsChecked
						});
					}
				}
			}
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Note\\NoteMapper.cs", 136);
			throw;
		}
	}

	public Task<NoteBusiness> MapAsync(string contextGuid, WebNote source, NoteBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new NoteBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.Text = source.Text;
			destination.DateM = source.DateM;
			destination.DateSarResidM = source.DateSarResidM;
			destination.Pirority = source.Pirority;
			destination.Status = source.Status;
			destination.CashierGuid = source.CashierGuid;
			destination.GroupGuid = source.GroupGuid;
			destination.DisplayState = source.DisplayState;
			destination.EntityGuid = source.EntityGuid;
			destination.EntityType = source.AsType;
			destination.ContextGuid = contextGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.GroupName = source.GroupName;
			destination.DateUtc = source.DateM.ToUniversalTime();
			destination.TafsilGuid = source.TafsilGuid;
			if (source.MetaData != null)
			{
				destination.MetaData = new NoteMetaData();
				List<PacketParser.BusinessLayer.Models.CheckListModel> checkList = source.MetaData.CheckList;
				if (checkList != null && checkList.Any())
				{
					destination.MetaData.CheckList = new List<EntityCache.Models.CheckListModel>();
					foreach (PacketParser.BusinessLayer.Models.CheckListModel check in source.MetaData.CheckList)
					{
						destination.MetaData.CheckList.Add(new EntityCache.Models.CheckListModel
						{
							Ordinal = check.Ordinal,
							Title = check.Title,
							IsChecked = check.IsChecked
						});
					}
				}
			}
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Note\\NoteMapper.cs", 194);
			throw;
		}
	}

	public Task<List<WebNote>> MapListAsync(string contextGuid, IEnumerable<NoteBusiness> source, List<WebNote> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<NoteBusiness>> MapListAsync(string contextGuid, IEnumerable<NoteBusiness> source, List<NoteBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<NoteBusiness>> MapListAsync(string contextGuid, IEnumerable<WebNote> source, List<NoteBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Business.Accounting.Note;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting.Note;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class NoteWebServiceRepository : WebServiceRepository<NoteBusiness>
{
	public NoteWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<NoteBusiness>)cache.CacheRepository.NoteRepository, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.Note.OnReadResponse += CusRoleInstance_OnReadResponse;
			Cache.Context.Note.OnSaveRequest += CusRoleInstance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NoteWebServiceRepository.cs", 24);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				if (Cache.Context.Note != null)
				{
					Cache.Context.Note.OnReadResponse -= CusRoleInstance_OnReadResponse;
					Cache.Context.Note.OnSaveRequest -= CusRoleInstance_OnSaveRequest;
				}
				Cache = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NoteWebServiceRepository.cs", 41);
		}
	}

	private async Task CusRoleInstance_OnReadResponse(object sender, ConnectionEventArgs<WebNote> e)
	{
		using (new JaegerMonitor("CusRoleInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NoteWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("Note received but data is null!", "", "CusRoleInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NoteWebServiceRepository.cs", 50);
					return;
				}
				List<WebNote> source = e.ConnectionData.ToList();
				List<NoteBusiness> lst = await NoteMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				await NoteBusiness.SaveRangeAsync(EnSystem.SignalR, sendToServer: false, e.Connection.cancellationToken, lst);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CusRoleInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NoteWebServiceRepository.cs", 58);
			}
		}
	}

	private async Task CusRoleInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebNote> e)
	{
		using (new JaegerMonitor("CusRoleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NoteWebServiceRepository.cs"))
		{
			_ = 4;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					throw new Exception("فراخوانی سیو رکوئست یادداشت در محصولی بغیر از حسابداری".GetTranslate());
				}
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("NoteInstance_OnSaveRequest received but data is null or empty!", "", "CusRoleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NoteWebServiceRepository.cs", 68);
					return;
				}
				List<NoteBusiness> list = await NoteMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || !list.Any())
				{
					throw new Exception("data was exists but mapped data is empty!");
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (NoteBusiness note in list)
				{
					if ((note?.TafsilGuid ?? Guid.Empty) != Guid.Empty && (note?.EntityType).GetValueOrDefault() == Document.CustomerLog)
					{
						note.EntityType = null;
						note.EntityGuid = null;
					}
					if ((note?.EntityGuid ?? Guid.Empty) != Guid.Empty && (note?.TafsilGuid ?? Guid.Empty) == Guid.Empty && (note?.EntityType).GetValueOrDefault() == Document.CustomerLog)
					{
						note.TafsilGuid = note.EntityGuid;
					}
					ReturnedSaveFuncInfo ret = await note.SaveAsync(EnSystem.SignalR, send2Server: false, e.Connection.cancellationToken, awaitOnSemaphore: true, await4Send: false, Priority.Low, 10);
					if (!ret.HasError && (note?.EntityGuid ?? Guid.Empty) != Guid.Empty)
					{
						IEnumerable<AccountTableViewModel> enumerable = await AccountBusiness.GetAllAsync(base.ContextGuid, note.EntityGuid.Value, isLoadDefMoein: false, true);
						if (enumerable != null && enumerable.Any())
						{
							foreach (AccountTableViewModel item2 in enumerable)
							{
								AccountBusiness acc = await AccountBusiness.GetFromDatabaseAsync(base.ContextGuid, item2.Guid);
								if (acc != null)
								{
									Task.Run(() => acc.Send2ServerAsync(e.Connection.cancellationToken, EnSystem.SignalR));
								}
							}
						}
					}
					WebDelivery item = new WebDelivery
					{
						Guid = note.Guid,
						Message = ret.GetErrorMessage() + ret.GetInformationMessage(),
						DeliveryStatus = !ret.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = ret.GetErrorMessage() + ret.GetInformationMessage(),
						Part = EntityPart.Note
					};
					deliveryList.Add(item);
				}
				if (deliveryList?.Any() ?? false)
				{
					List<Packet> lstPacket = WebDelivery.GenerateReadResponse(deliveryList, e.PacketReferenceId);
					await Cache.SendData(lstPacket, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CusRoleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\NoteWebServiceRepository.cs", 114);
			}
		}
	}
}

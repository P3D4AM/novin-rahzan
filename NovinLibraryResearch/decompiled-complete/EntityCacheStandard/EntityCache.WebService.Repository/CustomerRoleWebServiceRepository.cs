using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business.Customers;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Customer;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.WordPress;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class CustomerRoleWebServiceRepository : WebServiceRepository<CustomerRoleBusiness>
{
	public CustomerRoleWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<CustomerRoleBusiness>)cache.CacheRepository.CustomerRole, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.CustomerRole.OnReadResponse += CusRoleInstance_OnReadResponse;
			Cache.Context.CustomerRole.OnSync += CusRoleInstance_OnSync;
			Cache.Context.CustomerRole.OnSaveRequest += CusRoleInstance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerRoleWebServiceRepository.cs", 27);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				if (Cache.Context.CustomerRole != null)
				{
					Cache.Context.CustomerRole.OnReadResponse -= CusRoleInstance_OnReadResponse;
					Cache.Context.CustomerRole.OnSync -= CusRoleInstance_OnSync;
					Cache.Context.CustomerRole.OnSaveRequest -= CusRoleInstance_OnSaveRequest;
				}
				Cache = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerRoleWebServiceRepository.cs", 44);
		}
	}

	private async Task CusRoleInstance_OnSync(object sender, ConnectionEventArgs<WebCustomerRole> e)
	{
		using (new JaegerMonitor("CusRoleInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerRoleWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					WebErrorLog.Instance.Warning("Customer Role sync received but data is empty!", "", "CusRoleInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerRoleWebServiceRepository.cs", 53);
					return;
				}
				List<WebCustomerRole> source = e.ConnectionData.ToList();
				bool send2Server = Cache.Source == ENSource.Accounting;
				List<CustomerRoleBusiness> lst = await CustomerRoleMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await CustomerRoleBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, lst, send2Server);
				if (!e.Connection.cancellationToken.IsCancellationRequested)
				{
					returnedSaveFuncInfo.throwExceptionIfError();
					if (!e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.CustomerRole, e.PartitionId, returnedSaveFuncInfo);
						await Cache.SendData(packetData, Priority.High);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "CusRoleInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerRoleWebServiceRepository.cs", 70);
			}
		}
	}

	private async Task CusRoleInstance_OnReadResponse(object sender, ConnectionEventArgs<WebCustomerRole> e)
	{
		using (new JaegerMonitor("CusRoleInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerRoleWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					WebErrorLog.Instance.Warning("Customer Role received but data is empty!", "", "CusRoleInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerRoleWebServiceRepository.cs", 79);
					return;
				}
				List<WebCustomerRole> source = e.ConnectionData.ToList();
				List<CustomerRoleBusiness> lst = await CustomerRoleMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				bool send2Server = Cache.Source == ENSource.Accounting;
				await CustomerRoleBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, lst, send2Server);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CusRoleInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerRoleWebServiceRepository.cs", 87);
			}
		}
	}

	private async Task CusRoleInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebCustomerRole> e)
	{
		using (new JaegerMonitor("CusRoleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerRoleWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (Cache.Source != ENSource.Accounting)
				{
					throw new Exception("فراخوانی سیو رکوئست عنوان اشخاص در محصولی بغیر از حسابداری".GetTranslate());
				}
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("CusRoleInstance_OnSaveRequest received but data is null or empty!", "", "CusRoleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerRoleWebServiceRepository.cs", 99);
					return;
				}
				List<CustomerRoleBusiness> list = await CustomerRoleMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData.ToList());
				if (list == null || !list.Any())
				{
					throw new Exception("data was exists but mapped data is empty!");
				}
				List<WebDelivery> deliveryList = new List<WebDelivery>();
				foreach (CustomerRoleBusiness role in list)
				{
					bool send2Server = e.Connection.Source == ENSource.Accounting;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await role.SaveAsync(EnSystem.SignalR, e.Connection.cancellationToken, send2Server);
					WebDelivery item = new WebDelivery
					{
						Guid = role.Guid,
						Message = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						DeliveryStatus = !returnedSaveFuncInfo.HasError,
						DeliveryDate = DateTime.Now,
						ShortMessage = returnedSaveFuncInfo.GetErrorMessage() + returnedSaveFuncInfo.GetInformationMessage(),
						Part = EntityPart.CustomerRole
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
				WebErrorLog.Instance.StartLog(ex, "", "CusRoleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CustomerRoleWebServiceRepository.cs", 127);
			}
		}
	}
}

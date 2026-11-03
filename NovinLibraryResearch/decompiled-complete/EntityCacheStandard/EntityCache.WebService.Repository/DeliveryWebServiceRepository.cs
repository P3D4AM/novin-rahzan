using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core.Repository;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class DeliveryWebServiceRepository : IDisposable
{
	private ICustomerRepository customerRepo;

	private IOrderRepository orderRepo;

	private IPardakhtRepository pardakhtRepo;

	private IReceptionRepository receptionRepo;

	private readonly ClsCache Cache;

	public bool IsDisposed { get; private set; }

	public DeliveryWebServiceRepository(ClsCache cache)
	{
		try
		{
			Cache = cache;
			customerRepo = cache.CacheRepository.ICustomer ?? throw new ArgumentNullException("customerRepo");
			orderRepo = cache.CacheRepository.IOrder ?? throw new ArgumentNullException("orderRepo");
			pardakhtRepo = cache.CacheRepository.IPardakht ?? throw new ArgumentNullException("pardakhtRepo");
			receptionRepo = cache.CacheRepository.IReception ?? throw new ArgumentNullException("receptionRepo");
			Cache.Context.WebDelivery.OnSync += DeliveryInstance_OnSync;
			Cache.Context.WebDelivery.OnReadResponse += DeliveryInstance_OnReadResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 39);
		}
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				customerRepo.Dispose();
				customerRepo = null;
				orderRepo.Dispose();
				orderRepo = null;
				pardakhtRepo.Dispose();
				pardakhtRepo = null;
				receptionRepo.Dispose();
				receptionRepo = null;
				Cache.Context.WebDelivery.OnSync -= DeliveryInstance_OnSync;
				Cache.Context.WebDelivery.OnReadResponse -= DeliveryInstance_OnReadResponse;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 64);
		}
	}

	private async Task<ReturnedSaveFuncInfo> SaveDeliveryAsync(ConnectionEventArgs<WebDelivery> e, CancellationToken token, string caller)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor($"SaveDeliveryAsync_part_{(e?.ConnectionData?.FirstOrDefault()?.Part).GetValueOrDefault()}", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				int maxParalelTask = 5;
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				line = 5;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (WebDelivery connectionDatum in e.ConnectionData)
				{
					if (e.Connection.IsDisposed || e.Connection.cancellationToken.IsCancellationRequested || token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SaveDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 78);
					}
					WebErrorLog.Instance.Information("Delivery received... \r\n" + $"part:{(short)connectionDatum.Part}{connectionDatum.Part.ToString()}\r\n" + "Delivery:" + Json.ToStringJson(connectionDatum), "", "SaveDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 79);
					switch (connectionDatum.Part)
					{
					case EntityPart.CustomerGroup:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.CusGroupBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.CustomerTitle:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.CusTitleBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Customer:
						line = 10;
						tasks.Add(Cache.BusinessAggregator.CusBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Product:
						line = 15;
						tasks.Add(Cache.BusinessAggregator.ProductBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.ProductGroup:
						line = 15;
						tasks.Add(Cache.BusinessAggregator.PrdGroupBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.ProductVahed:
						line = 15;
						tasks.Add(Cache.BusinessAggregator.PrdVahedBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Order:
						line = 20;
						if (Cache.Source == ENSource.Accounting)
						{
							continue;
						}
						tasks.Add(Cache.BusinessAggregator.OrderBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Pardakht:
						if (Cache.Source == ENSource.Accounting)
						{
							continue;
						}
						line = 25;
						tasks.Add(Cache.BusinessAggregator.PardakhtBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Reception:
						line = 30;
						if (Cache.Source == ENSource.Accounting)
						{
							continue;
						}
						tasks.Add(Cache.BusinessAggregator.ReceptionBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Desc:
						line = 50;
						tasks.Add(Cache.BusinessAggregator.DescBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Gps:
						line = 55;
						if (Cache.Source == ENSource.Accounting)
						{
							continue;
						}
						tasks.Add(Cache.BusinessAggregator.GpsBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Visit:
						line = 60;
						tasks.Add(VisitBusiness.SetDeliveryAsync(e.Connection.ContextGuid, connectionDatum, token, 10));
						break;
					case EntityPart.AghsatAshkhas:
						line = 60;
						tasks.Add(Cache.BusinessAggregator.AghsatMBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.AghsatShakhsi:
						line = 65;
						tasks.Add(Cache.BusinessAggregator.AghsatShBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.CallingLog:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.CallingLogBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Kol:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.KolBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Moein:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.MoeinBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Tafsil:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.TafsilBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.User:
					case EntityPart.TafsilGroup:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.UserAccBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.UserLog:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.CashiersLogBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.ChequeMOperation:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.ChequeMOperationBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.ChequeShOperation:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.ChequeShOperationBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.Sanad:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.SanadBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.PrdAnbarGardani:
						line = 70;
						tasks.Add(Cache.BusinessAggregator.PrdAnbarGardaniBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.EntitySync:
						throw new InvalidOperationException("server sent delivery type with part of entity sync!! this is server error!");
					case EntityPart.PrdSerial:
						tasks.Add(Cache.BusinessAggregator.PrdSerialBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.SanadPrint:
						tasks.Add(Cache.BusinessAggregator.SanadPrintBusiness.SetDeliveryAsync(connectionDatum, token, 10));
						break;
					case EntityPart.AttPersonLeave:
						if (Cache.BusinessAggregator.PersonLeave != null)
						{
							tasks.Add(Cache.BusinessAggregator.PersonLeave.SetDeliveryAsync(connectionDatum, token, 10));
						}
						break;
					default:
					{
						line = 75;
						Exception ex = new Exception($"DeliveryInstance_OnReadResponse part not configed!{connectionDatum.Part}/Calle:{caller}");
						ret.AddReturnedValue(ex, "", "SaveDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 200);
						WebErrorLog.Instance.StartLog(ex, $" The Dispose State IS:{IsDisposed},The Error Line IS :{line}/Calle:{caller}", "SaveDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 201);
						throw new InvalidOperationException($"server sent delivery type with part of {connectionDatum.Part}!! this is server error!");
					}
					}
					line = 80;
					if (tasks.Count >= maxParalelTask)
					{
						Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "SaveDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 209);
						tasks.Remove(t);
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SaveDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 213);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"IsDisposed:{IsDisposed}//Line:{line}/Calle:{caller}", "SaveDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 217);
				ret.AddReturnedValue(ex2, "", "SaveDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 218);
			}
			return ret;
		}
	}

	private async Task DeliveryInstance_OnReadResponse(object sender, ConnectionEventArgs<WebDelivery> e)
	{
		try
		{
			await SaveDeliveryAsync(e, e.Connection.cancellationToken, "DeliveryInstance_OnReadResponse");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeliveryInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 228);
		}
	}

	private async Task DeliveryInstance_OnSync(object sender, ConnectionEventArgs<WebDelivery> e)
	{
		using (new JaegerMonitor("DeliveryInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs"))
		{
			int line = 0;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await SaveDeliveryAsync(e, e.Connection.cancellationToken, "DeliveryInstance_OnSync");
				line = 11;
				if (!returnedSaveFuncInfo.HasError && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					line = 12;
					await Cache.SendData(Sync.GenerateSyncDone(EntityPart.Delivery, e.PartitionId, returnedSaveFuncInfo), Priority.High);
				}
				line = 3;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "DeliveryInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\DeliveryWebServiceRepository.cs", 245);
			}
		}
	}
}

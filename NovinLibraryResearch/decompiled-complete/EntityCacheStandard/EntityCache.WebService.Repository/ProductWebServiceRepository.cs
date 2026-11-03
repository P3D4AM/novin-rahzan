using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.EventArguments;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class ProductWebServiceRepository : WebServiceDetailRepository<ProductBusiness>
{
	private bool _isDisposed;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public event EventHandler<CountChangedEventArgs> CountChanged;

	public ProductWebServiceRepository(ClsCache cache)
		: base((IDetailRepository<ProductBusiness>)cache.CacheRepository.IProduct, cache)
	{
		try
		{
			Cache.Context.WebProduct.OnReadResponse += ProductInstance_OnReadResponse;
			Cache.Context.WebProduct.OnSync += ProductInstance_OnSync;
			Cache.Context.WebProduct.OnSaveResponse += ProductInstance_OnSaveResponse;
			Cache.Context.WebProduct.OnDeletedRequest += ProductInstance_OnDeletedRequest;
			if (cache.Source == ENSource.Accounting)
			{
				Cache.Context.WebProduct.OnSaveRequest += WebProduct_OnSaveRequest;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 36);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Cache.Context.WebProduct.OnReadResponse -= ProductInstance_OnReadResponse;
				Cache.Context.WebProduct.OnSync -= ProductInstance_OnSync;
				Cache.Context.WebProduct.OnSaveResponse -= ProductInstance_OnSaveResponse;
				Cache.Context.WebProduct.OnDeletedRequest -= ProductInstance_OnDeletedRequest;
				Cache.Context.WebProduct.OnSaveRequest -= WebProduct_OnSaveRequest;
				this.CountChanged = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 55);
		}
	}

	private async Task WebProduct_OnSaveRequest(object sender, ConnectionEventArgs<WebProduct> e)
	{
		try
		{
			if (Cache.Source != ENSource.Accounting)
			{
				throw new NotImplementedException();
			}
			bool updateAmount = Cache.Source != ENSource.Accounting;
			ReturnedSaveFuncInfoWithValue<List<SaveResponse>> returnedSaveFuncInfoWithValue = await ProductBusiness.SaveRangeAsync(base.ContextGuid, 10, e.ConnectionData, e.Connection.cancellationToken, updateAmount, 0, EnSystem.SignalR);
			if (!e.Connection.cancellationToken.IsCancellationRequested)
			{
				Cache.SendData(WebProduct.GenerateSaveResponse(returnedSaveFuncInfoWithValue.value), Priority.High);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebProduct_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 89);
		}
	}

	private async Task ProductInstance_OnDeletedRequest(object sender, ConnectionEventArgs<Guid> e)
	{
		using (new JaegerMonitor("ProductInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs"))
		{
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("ProductInstance Ondelete received but data is null!", "", "ProductInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 99);
					return;
				}
				List<Guid> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("ProductInstance Ondelete received but data is empty!", "", "ProductInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 104);
				}
				else if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateDeleteResponse(EntityPart.Product, e.PartitionId);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ProductInstance_OnDeletedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 118);
			}
		}
	}

	private async Task ProductInstance_OnSaveResponse(object sende, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("ProductInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e == null || !(e.SaveResponses?.Count > 0))
				{
					return;
				}
				foreach (SaveResponse resopnse in e.SaveResponses)
				{
					if (resopnse != null)
					{
						ProductBusiness productBusiness = await ProductBusiness.GetAsync(Cache.ContextGuid, resopnse.Guid);
						if (productBusiness != null)
						{
							await productBusiness.SetSaveResultAsync(resopnse, e.Connection.cancellationToken, 10);
						}
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ProductInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 136);
			}
		}
	}

	private async Task ProductInstance_OnSync(object sender, ConnectionEventArgs<WebProduct> e)
	{
		using (new JaegerMonitor("ProductInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				CancellationToken token = e.Connection.cancellationToken;
				if (token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("", "", "ProductInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 147);
					return;
				}
				if (e?.ConnectionData == null || e.ConnectionData.Count <= 0 || token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("", "", "ProductInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 149);
					return;
				}
				line = 2;
				line = 3;
				bool updateAmount = e.Connection.Source != ENSource.Accounting;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ProductBusiness.SaveRangeAsync(e.Connection.Context.Guid, 10, e.ConnectionData, token, updateAmount, 0, EnSystem.SignalR), ReturnedState.None, "ProductInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 154);
				if (token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("", "", "ProductInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 155);
					return;
				}
				if (!ret.HasError && this.CountChanged != null)
				{
					int count = await base.CacheRepository.DbCountAsync(true, token, "");
					Task.Run(delegate
					{
						this.CountChanged?.Invoke(this, new CountChangedEventArgs(count));
					});
				}
				line = 8;
				if (!ret.HasError && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					line = 12;
					Packet packetData = Sync.GenerateSyncDone(EntityPart.Product, e.PartitionId, ret);
					line = 13;
					await Cache.SendData(packetData, Priority.High);
					line = 14;
					await ProductMapper.Instance.MapListAsync(base.ContextGuid, e.ConnectionData);
				}
				else
				{
					ret.AddError("sync done packet not sent!!!!!!!!", "", "ProductInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 175);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "ProductInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 178);
			}
		}
	}

	private async Task ProductInstance_OnReadResponse(object sender, ConnectionEventArgs<WebProduct> e)
	{
		using (new JaegerMonitor("ProductInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				CancellationToken token = e.Connection.cancellationToken;
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Product received but data is null!", "", "ProductInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 345);
					return;
				}
				List<WebProduct> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Product received but data is empty!", "", "ProductInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 350);
				}
				else
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					bool updateAmount = e.Connection.Source != ENSource.Accounting;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ProductBusiness.SaveRangeAsync(e.Connection.Context.Guid, 10, e.ConnectionData, e.Connection.cancellationToken, updateAmount, 0, EnSystem.SignalR), ReturnedState.None, "ProductInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 375);
					if (!ret.HasError && this.CountChanged != null)
					{
						int count = await base.CacheRepository.DbCountAsync(true, token, "");
						Task.Run(delegate
						{
							this.CountChanged?.Invoke(this, new CountChangedEventArgs(count));
						});
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ProductInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ProductWebServiceRepository.cs", 385);
			}
		}
	}
}

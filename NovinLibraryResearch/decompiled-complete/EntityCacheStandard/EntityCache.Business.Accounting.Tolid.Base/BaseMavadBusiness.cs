using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Business.Accounting.Tolid.Base;

public abstract class BaseMavadBusiness : IHasContext, IHasJustGuid, INpDisposable, IDisposable
{
	protected KardexBusiness _lastKardexRow;

	private CancellationTokenSource _getLastKardexRecordToken;

	private Guid _productGuid;

	private Guid _depotGuid;

	private List<PrdSerialBusiness> _serialStr;

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; } = true;


	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public ProductCompactBusiness Prd { get; private set; }

	public decimal Quantity { get; set; }

	public string Serial { get; set; }

	public string Description { get; set; }

	public Guid? V2Guid { get; set; }

	public virtual decimal Price { get; }

	public DepotBusiness Depot => DepotBusiness.Get(ContextGuid, DepotGuid);

	public string ProductCode => Prd?.IdProduct;

	public string ProductName => Prd?.Name;

	public string DepotName => Depot?.Name;

	public string PrdVahed => Prd?.VahedName;

	public Guid ProductGuid => _productGuid;

	public Guid DepotGuid => _depotGuid;

	public bool IsDisposed { get; }

	public IReadOnlyCollection<PrdSerialBusiness> SerialStr => _serialStr ?? (_serialStr = new List<PrdSerialBusiness>());

	public BaseMavadBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public void Dispose()
	{
		try
		{
			_getLastKardexRecordToken?.Cancel();
			_getLastKardexRecordToken?.Dispose();
			_getLastKardexRecordToken = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseMavadBusiness.cs", 50);
		}
	}

	public async Task GetLastKardexRowAsync(CancellationToken token, string tranName = "")
	{
		try
		{
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				KardexFilter filter = new KardexFilter
				{
					PrdGuid = ProductGuid,
					DepotGuid = DepotGuid,
					CountForDisplay = (short)1
				};
				_lastKardexRow = (await KardexBusiness.GetAllAsync(ContextGuid, filter, token, tranName))?.value?.SingleOrDefault();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastKardexRowAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseMavadBusiness.cs", 67);
			throw;
		}
	}

	public async Task SetProductGuid(Guid value, EnSystem system, string tranName)
	{
		_ = 1;
		try
		{
			_productGuid = value;
			if (system != EnSystem.Transfer)
			{
				_getLastKardexRecordToken?.Cancel();
				_getLastKardexRecordToken = new CancellationTokenSource();
				CancellationToken token = _getLastKardexRecordToken.Token;
				Prd = await ProductCompactBusiness.GetAsync(ContextGuid, value, system, null, tranName, default(CancellationToken), true);
				await GetLastKardexRowAsync(token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProductGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseMavadBusiness.cs", 83);
		}
	}

	public async Task SetDepotGuid(Guid value, EnSystem system, string tranName)
	{
		try
		{
			_depotGuid = value;
			if (system != EnSystem.Transfer)
			{
				_getLastKardexRecordToken?.Cancel();
				_getLastKardexRecordToken = new CancellationTokenSource();
				CancellationToken token = _getLastKardexRecordToken.Token;
				await GetLastKardexRowAsync(token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDepotGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseMavadBusiness.cs", 96);
		}
	}

	public Task SetSerialAsync(PrdSerialBusiness serial, DateTime? sanadDate, EnSystem system)
	{
		try
		{
			if (serial == null)
			{
				return Task.CompletedTask;
			}
			if (_serialStr == null)
			{
				_serialStr = new List<PrdSerialBusiness>();
			}
			if (system == EnSystem.Mapper || system == EnSystem.Transfer)
			{
				_serialStr.Add(serial);
				return Task.CompletedTask;
			}
			if (serial.ProductGuid != ProductGuid)
			{
				return Task.CompletedTask;
			}
			PrdSerialBusiness prdSerialBusiness = _serialStr.FirstOrDefault((PrdSerialBusiness q) => q.Serial == serial.Serial);
			if (prdSerialBusiness != null)
			{
				_serialStr.Remove(prdSerialBusiness);
			}
			PrdSerialDetailBusiness prdSerialDetailBusiness = serial.DetList?.FirstOrDefault((PrdSerialDetailBusiness q) => q.AsType == Document.Tolid_Mavad && q.DetailGuid == Guid);
			if (prdSerialDetailBusiness == null)
			{
				prdSerialDetailBusiness = new PrdSerialDetailBusiness(ContextGuid)
				{
					AsType = Document.Tolid_Mavad,
					DetailGuid = Guid,
					Guid = Guid.NewGuid(),
					MasterGuid = serial.Guid,
					Modified = DateTime.Now,
					SanadDate = sanadDate,
					IsVorod = false
				};
			}
			serial.AddDet(prdSerialDetailBusiness);
			_serialStr.Add(serial);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseMavadBusiness.cs", 129);
		}
		return Task.CompletedTask;
	}

	public async Task SetSerialListAsync(List<PrdSerialBusiness> serialList, DateTime? sanadDate, EnSystem system, bool clearList = false)
	{
		try
		{
			if (!(serialList?.Any() ?? false))
			{
				return;
			}
			if (clearList)
			{
				_serialStr = new List<PrdSerialBusiness>();
			}
			foreach (PrdSerialBusiness serial in serialList)
			{
				if (serial != null)
				{
					await SetSerialAsync(serial, sanadDate, system);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSerialListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseMavadBusiness.cs", 146);
			throw;
		}
	}

	public void RemoveSerial(PrdSerialBusiness serial)
	{
		try
		{
			List<PrdSerialBusiness> serialStr = _serialStr;
			if (serialStr != null && serialStr.Any())
			{
				_serialStr.Remove(serial);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseMavadBusiness.cs", 157);
		}
	}

	public void ClearSerialList()
	{
		_serialStr?.Clear();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting.Tolid.Base;

public abstract class BaseExportBusiness : IHasContext, IHasJustGuid
{
	private Guid productGuid;

	private List<PrdSerialBusiness> _serialStr;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public string ContextGuid { get; set; }

	public bool Status { get; }

	public Guid MasterGuid { get; set; }

	public Guid ProductGuid => productGuid;

	public string PrdCode => Prd?.IdProduct;

	public string PrdName => Prd?.Name;

	public string PrdVahed => Prd?.VahedName;

	public Guid DepotGuid { get; set; }

	public string DepotName => Depot?.Name;

	public string Description { get; set; }

	public decimal Quantity { get; set; }

	public Guid? V2Guid { get; set; }

	public virtual decimal Price { get; }

	public ProductCompactBusiness Prd { get; private set; }

	public DepotBusiness Depot => DepotBusiness.Get(ContextGuid, DepotGuid);

	public IReadOnlyCollection<PrdSerialBusiness> SerialStr => _serialStr ?? (_serialStr = new List<PrdSerialBusiness>());

	public BaseExportBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public virtual ReturnedSaveFuncInfo CheckValidation(EnSystem system)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (Quantity <= 0m)
			{
				returnedSaveFuncInfo.AddError(system, "مقدار وارد شده صحیح نیست ، لطفا میزان تولید را وارد نمایید .".GetTranslate(), "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseExportBusiness.cs", 45);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseExportBusiness.cs", 49);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseExportBusiness.cs", 50);
		}
		return returnedSaveFuncInfo;
	}

	public async Task SetProductGuidAsync(Guid value, EnSystem system, CancellationToken token, string tranName)
	{
		try
		{
			productGuid = value;
			if (system != EnSystem.Transfer)
			{
				Prd = await ProductCompactBusiness.GetAsync(ContextGuid, value, system, null, tranName, token, true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseExportBusiness.cs", 64);
			throw;
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
			PrdSerialDetailBusiness prdSerialDetailBusiness = serial.DetList?.FirstOrDefault((PrdSerialDetailBusiness q) => q.AsType == Document.Tolid_Mahsul && q.DetailGuid == Guid);
			if (prdSerialDetailBusiness == null)
			{
				prdSerialDetailBusiness = new PrdSerialDetailBusiness(ContextGuid)
				{
					AsType = Document.Tolid_Mahsul,
					DetailGuid = Guid,
					Guid = Guid.NewGuid(),
					MasterGuid = serial.Guid,
					Modified = DateTime.Now,
					SanadDate = sanadDate,
					IsVorod = true
				};
			}
			serial.AddDet(prdSerialDetailBusiness);
			_serialStr.Add(serial);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseExportBusiness.cs", 99);
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
			WebErrorLog.Instance.StartLog(ex, "", "SetSerialListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseExportBusiness.cs", 116);
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
			WebErrorLog.Instance.StartLog(ex, "", "RemoveSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Base\\BaseExportBusiness.cs", 127);
		}
	}

	public void ClearSerialList()
	{
		_serialStr?.Clear();
	}
}

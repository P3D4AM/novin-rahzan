using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DepotReceipt;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Accounting.DepotReceipt;

public class DepotReceiptDetailBusiness : Serializable<DepotReceiptDetailBusiness>, IDepotReceiptDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext, INotifyPropertyChanged, IHasQuantity
{
	private ProductCompactBusiness _product;

	private DepotBusiness _depot;

	private PrdVahedBusiness _vahed2;

	private Guid _productGuid;

	private Guid _depotGuid;

	private Guid? _v2Guid;

	private bool _isDisposed;

	private string _prdCode;

	private string _strQt = "";

	private decimal _quantity;

	private decimal? _v2Qt = default(decimal);

	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid ProductGuid => _productGuid;

	[JsonIgnore]
	public string ProductCode
	{
		get
		{
			if (!string.IsNullOrEmpty(_product?.IdProduct))
			{
				return _product.IdProduct;
			}
			return _prdCode;
		}
	}

	[JsonIgnore]
	public string ProductName => _product?.Name;

	[JsonIgnore]
	public string ProductNameForDisplay
	{
		get
		{
			string text = ProductName;
			ProductCompactBusiness product = _product;
			if (product != null && !product.Status)
			{
				text += " (غیرفعال) ";
			}
			if (!string.IsNullOrEmpty(Description.FixString().Trim()))
			{
				text = text + " (شرح سطر: " + Description + " )";
			}
			return text;
		}
	}

	[JsonIgnore]
	public string ProductVahed => _product?.VahedName;

	[JsonIgnore]
	public string ProductDescription => _product?.Description;

	public Guid DepotGuid => _depotGuid;

	[JsonIgnore]
	public string DepotName => _depot?.Name;

	public decimal Quantity => _quantity;

	[JsonIgnore]
	public string QuantityString
	{
		get
		{
			try
			{
				if (!string.IsNullOrEmpty(_strQt) && (_strQt.EndsWith("/") || _strQt.EndsWith(".")))
				{
					return _strQt;
				}
				_strQt = ((double)Quantity).ToString(3);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "QuantityString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 67);
			}
			return _strQt;
		}
	}

	public string Description { get; set; }

	public Guid? V2Guid => _v2Guid;

	[JsonIgnore]
	public string Unit2 => _vahed2?.Name;

	[JsonIgnore]
	public string ProductVahed2 => _vahed2?.Name;

	public decimal? V2Quantity => _v2Qt;

	[JsonIgnore]
	public string Quantity2String => _v2Qt?.ToString(3) ?? "";

	public int Ordinal { get; set; }

	public Guid? ReferenceGuid { get; set; }

	public string ContextGuid { get; set; }

	[JsonIgnore]
	public byte[] EditImage => ImageResourceManager.DepotReceiptEdit;

	[JsonIgnore]
	public byte[] DeleteImage => ImageResourceManager.DepotReceiptDelete;

	[JsonIgnore]
	public DepotBusiness DepotInfo => _depot;

	public EntityChangeSt ChangeStatus { get; set; } = EntityChangeSt.New;


	[JsonIgnore]
	public DepotReceiptBusiness MasterItem { get; internal set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public event Func<object, EventArgs, Task> OnDisposed;

	public event PropertyChangedEventHandler PropertyChanged;

	private DepotReceiptDetailBusiness()
	{
	}

	public static async Task<DepotReceiptDetailBusiness> GetFactoryAsync(string contextGuid, DepotReceiptBusiness master, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs"))
		{
			DepotReceiptDetailBusiness item = new DepotReceiptDetailBusiness();
			if (master == null)
			{
				throw new NullReferenceException("master");
			}
			try
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
				item.ContextGuid = master.ContextGuid;
				item._depotGuid = cashierBusiness?.DepotGuid ?? Guid.Empty;
				item.MasterItem = master;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 102);
			}
			return item;
		}
	}

	public async Task SetProductGuidAsync(Guid prdGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			_productGuid = prdGuid;
			if (_productGuid == Guid.Empty)
			{
				_product = null;
			}
			else
			{
				_product = await ProductCompactBusiness.GetAsync(ContextGuid, _productGuid, system, null, tranName, token, true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 119);
			throw;
		}
		finally
		{
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductCode"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductName"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductVahed"));
				}, "SetProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs");
			}
		}
	}

	public async Task SetProductCodeAsync(string value, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			_prdCode = value;
			await SetProductGuidAsync((await ProductCompactBusiness.GetAsync(ContextGuid, value, system, true, "", null, token))?.Guid ?? Guid.Empty, system, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#IsDisposed_{base.IsDisposed}", "SetProductCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 141);
			throw;
		}
	}

	public async Task SetDepotGuidAsync(Guid depotGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			_depotGuid = depotGuid;
			if (_depotGuid == Guid.Empty)
			{
				_depot = null;
			}
			else
			{
				_depot = await DepotBusiness.GetAsync(ContextGuid, _depotGuid, token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 157);
			throw;
		}
	}

	public async Task SetV2GuidAsync(EnSystem system, Guid? v2Guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			_v2Guid = (((v2Guid ?? Guid.Empty) == Guid.Empty) ? null : v2Guid);
			if ((_v2Guid ?? Guid.Empty) == Guid.Empty)
			{
				_vahed2 = null;
				return;
			}
			if (system != EnSystem.Mapper)
			{
				if (_v2Guid.HasValue && _product != null && _product.GuidVahed == _v2Guid.Value)
				{
					_v2Guid = null;
					decimal value = _v2Qt ?? _quantity;
					_v2Qt = null;
					await SetQuantityAsync(system, value, token, tranName);
				}
				decimal? value2 = ((!(V2Quantity.GetValueOrDefault() == 0m)) ? V2Quantity : null);
				_v2Qt = null;
				await SetV2QtAsync(value2, token, tranName);
			}
			_vahed2 = await PrdVahedBusiness.GetAsync(ContextGuid, _v2Guid.Value, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetV2GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 194);
			throw;
		}
	}

	public async Task SetV2QtAsync(decimal? value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			_v2Qt = value;
			decimal num = await GetV2ZaribAsync(token, tranName);
			if (value.HasValue && num > 0m)
			{
				_quantity = num * value.GetValueOrDefault();
			}
			else if (value.HasValue && num == 0m)
			{
				_quantity = value.GetValueOrDefault();
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Quantity"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Quantity2String"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("QuantityString"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("V2Quantity"));
				}, "SetV2QtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetV2QtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 219);
			throw;
		}
	}

	public async Task SetQuantityAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			_quantity = value;
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token, tranName);
			decimal num = await GetV2ZaribAsync(token, tranName);
			if (num > 0m && setting.Products.ChandVahedi)
			{
				_v2Qt = _quantity / num;
			}
			else if (!(num > 0m) || setting.Products.ChandVahedi)
			{
				_v2Qt = null;
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Quantity"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Quantity2String"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("QuantityString"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("V2Quantity"));
				}, "SetQuantityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetQuantityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 246);
			throw;
		}
	}

	public async Task SetQuantityStringAsync(EnSystem system, string value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (value.EndsWith("/") || value.EndsWith("."))
			{
				_strQt = value;
			}
			else
			{
				_strQt = "";
			}
			await SetQuantityAsync(system, value.ParseToDecimal(0m, 3), token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetQuantityStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 258);
			throw;
		}
	}

	public async Task<decimal> GetV2ZaribAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 3;
		try
		{
			if (_product == null)
			{
				return default(decimal);
			}
			if ((_v2Guid ?? Guid.Empty) == Guid.Empty)
			{
				return default(decimal);
			}
			if ((_v2Guid ?? Guid.Empty) == (_product?.V2Guid ?? Guid.Empty))
			{
				return _product.V2Qt;
			}
			Guid guid = _v2Guid ?? Guid.Empty;
			if (guid == ((await _product.GetV3GuidAsync(token, tranName)) ?? Guid.Empty))
			{
				return await _product.GetV3QtAsync(token, tranName);
			}
			guid = _v2Guid ?? Guid.Empty;
			if (guid == ((await _product.GetV4GuidAsync(token, tranName)) ?? Guid.Empty))
			{
				return await _product.GetV4QtAsync(token, tranName);
			}
			return 1m;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetV2ZaribAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 275);
		}
		return 1m;
	}

	public async Task<bool> GetIsMultiUnitAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (_product == null)
			{
				return false;
			}
			bool flag = _product.V2Qt > 0m;
			if (!flag)
			{
				flag = await _product.GetV3QtAsync(token, tranName) > 0m;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				flag2 = await _product.GetV4QtAsync(token, tranName) > 0m;
			}
			return flag2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetIsMultiUnitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 287);
			throw;
		}
	}

	public async Task<ProductCompactBusiness> GetPrdInfoAsync(EnSystem system = EnSystem.None, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (ProductGuid == Guid.Empty)
			{
				_product = null;
				return _product;
			}
			if ((_product?.Guid ?? Guid.Empty) != ProductGuid && ProductGuid != Guid.Empty)
			{
				_product = await ProductCompactBusiness.GetAsync(ContextGuid, ProductGuid, system, null, tranName, token, null);
			}
			return _product;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPrdInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 306);
			throw;
		}
	}

	public async Task<List<OrderDetailBusiness.DisplayMultiVahed>> GetMultiUnitListAsync(EnSystem system = EnSystem.None, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetMultiUnitListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs"))
		{
			_ = 15;
			try
			{
				List<OrderDetailBusiness.DisplayMultiVahed> list = new List<OrderDetailBusiness.DisplayMultiVahed>();
				ProductCompactBusiness prd = await GetPrdInfoAsync(system, token);
				if (prd == null)
				{
					return list;
				}
				list.Add(new OrderDetailBusiness.DisplayMultiVahed
				{
					Guid = prd.GuidVahed,
					Quantity = 1m,
					Name = ProductVahed
				});
				if (prd.V2Qt > 0m && prd.V2Guid.HasValue)
				{
					Guid? v2Guid = prd.V2Guid;
					if (v2Guid.HasValue)
					{
						if (_vahed2 == null)
						{
							_vahed2 = await PrdVahedBusiness.GetAsync(ContextGuid, v2Guid.Value, token);
						}
						list.Add(new OrderDetailBusiness.DisplayMultiVahed
						{
							Guid = v2Guid.Value,
							Quantity = prd.V2Qt,
							Name = Unit2 + " - " + prd.V2Qt.ToString(3)
						});
					}
				}
				bool flag = await prd.GetV3QtAsync() > 0m;
				if (flag)
				{
					flag = ((await prd.GetV3GuidAsync()) ?? Guid.Empty) != Guid.Empty;
				}
				if (flag)
				{
					string contextGuid = ContextGuid;
					string v3Name2 = (await PrdVahedBusiness.GetAsync(contextGuid, (await prd.GetV3GuidAsync()).Value, token)).Name;
					List<OrderDetailBusiness.DisplayMultiVahed> list2 = list;
					OrderDetailBusiness.DisplayMultiVahed displayMultiVahed = new OrderDetailBusiness.DisplayMultiVahed();
					OrderDetailBusiness.DisplayMultiVahed displayMultiVahed2 = displayMultiVahed;
					displayMultiVahed2.Guid = (await prd.GetV3GuidAsync(token)).Value;
					OrderDetailBusiness.DisplayMultiVahed displayMultiVahed3 = displayMultiVahed;
					displayMultiVahed3.Quantity = await prd.GetV3QtAsync(token);
					OrderDetailBusiness.DisplayMultiVahed displayMultiVahed4 = displayMultiVahed;
					contextGuid = v3Name2;
					displayMultiVahed4.Name = contextGuid + " - " + (await prd.GetV3QtAsync()).ToString(3);
					list2.Add(displayMultiVahed);
				}
				flag = await prd.GetV4QtAsync() > 0m;
				if (flag)
				{
					flag = ((await prd.GetV4GuidAsync()) ?? Guid.Empty) != Guid.Empty;
				}
				if (flag)
				{
					string contextGuid = ContextGuid;
					string v3Name2 = (await PrdVahedBusiness.GetAsync(contextGuid, (await prd.GetV4GuidAsync()).Value, token)).Name;
					List<OrderDetailBusiness.DisplayMultiVahed> list2 = list;
					OrderDetailBusiness.DisplayMultiVahed displayMultiVahed2 = new OrderDetailBusiness.DisplayMultiVahed();
					OrderDetailBusiness.DisplayMultiVahed displayMultiVahed = displayMultiVahed2;
					displayMultiVahed.Guid = (await prd.GetV4GuidAsync()).Value;
					OrderDetailBusiness.DisplayMultiVahed displayMultiVahed4 = displayMultiVahed2;
					displayMultiVahed4.Quantity = await prd.GetV4QtAsync();
					OrderDetailBusiness.DisplayMultiVahed displayMultiVahed3 = displayMultiVahed2;
					contextGuid = v3Name2;
					displayMultiVahed3.Name = contextGuid + " - " + (await prd.GetV4QtAsync()).ToString(3);
					list2.Add(displayMultiVahed2);
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetMultiUnitListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 362);
				throw;
			}
		}
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (this.OnDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs");
				}
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 378);
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				ProductCompactBusiness productCompactBusiness = await GetPrdInfoAsync(system, token, tranName);
				_ = MasterItem.DetailList.ToList().IndexOf(this) + 1;
				if (productCompactBusiness == null)
				{
					res.AddError("کالا انتخاب نشده است.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 390);
				}
				if (system == EnSystem.UserInterface && MasterItem.IsDuplicate(this))
				{
					res.AddReturnedValue(setting.Factors.PrdTekrari, (productCompactBusiness?.Name ?? "").GetTranslate() + " " + " در ردیف".GetTranslate() + " " + $" {Ordinal}".GetTranslate() + " " + " در فاکتور تکراری است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 392);
				}
				if ((productCompactBusiness?.Guid ?? Guid.Empty) == Guid.Empty)
				{
					res.AddError("هیچ کالایی در ردیف".GetTranslate() + " " + $" {Ordinal}".GetTranslate() + " " + " انتخاب نشده".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 394);
				}
				if (Quantity <= 0m)
				{
					string[] obj = new string[8]
					{
						"مقدار کالای".GetTranslate(),
						" ",
						null,
						null,
						null,
						null,
						null,
						null
					};
					object obj2 = productCompactBusiness?.Name ?? "";
					if (obj2 == null)
					{
						obj2 = "";
					}
					obj[2] = ((string)obj2).GetTranslate();
					obj[3] = " ";
					obj[4] = "در ردیف".GetTranslate();
					obj[5] = " ";
					obj[6] = $"{Ordinal}".GetTranslate();
					obj[7] = " مشخص نشده".GetTranslate();
					res.AddError(string.Concat(obj), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 395);
				}
				if (DepotGuid == Guid.Empty)
				{
					string[] obj3 = new string[8]
					{
						"انبار کالای".GetTranslate(),
						" ",
						null,
						null,
						null,
						null,
						null,
						null
					};
					object obj4 = productCompactBusiness?.Name ?? "";
					if (obj4 == null)
					{
						obj4 = "";
					}
					obj3[2] = ((string)obj4).GetTranslate();
					obj3[3] = " ";
					obj3[4] = "در ردیف".GetTranslate();
					obj3[5] = " ";
					obj3[6] = $" {Ordinal} ".GetTranslate();
					obj3[7] = "مشخص نشده".GetTranslate();
					res.AddError(string.Concat(obj3), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 396);
				}
				if (isDeleteAction && (ReferenceGuid ?? Guid.Empty) != Guid.Empty)
				{
					string[] obj5 = new string[11]
					{
						"کالای".GetTranslate(),
						" ",
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						null
					};
					object obj6 = productCompactBusiness?.Name ?? "";
					if (obj6 == null)
					{
						obj6 = "";
					}
					obj5[2] = ((string)obj6).GetTranslate();
					obj5[3] = " ";
					obj5[4] = " در ردیف".GetTranslate();
					obj5[5] = " ";
					obj5[6] = $" {Ordinal}".GetTranslate();
					obj5[7] = " ";
					obj5[8] = " ریالی شده و قابل حذف نمی باشد ".GetTranslate();
					obj5[9] = "\r\n";
					obj5[10] = " لطفا ابتدا نسبت به حذف سند حسابداری اقدام نمایید".GetTranslate();
					res.AddError(string.Concat(obj5), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 398);
				}
				return res;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 404);
				res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepotReceipt\\DepotReceiptDetailBusiness.cs", 405);
			}
			return res;
		}
	}
}

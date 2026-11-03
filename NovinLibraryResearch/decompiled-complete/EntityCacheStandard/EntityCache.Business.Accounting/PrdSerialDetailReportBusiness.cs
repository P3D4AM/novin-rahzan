using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting;

public class PrdSerialDetailReportBusiness : INotifyPropertyChanged
{
	private string _cashierName = "";

	private string _productName = "";

	private string _productCode = "";

	private Guid _prdGuid = Guid.Empty;

	private Guid _cashierGuid = Guid.Empty;

	private CancellationTokenSource _prdToken = new CancellationTokenSource();

	private CancellationTokenSource _cashierToken = new CancellationTokenSource();

	public Guid Guid { get; set; }

	public string Serial { get; set; }

	public string GuarrantyCode { get; set; }

	public DateTime? StartDate { get; set; }

	public string StartDateSh => StartDate.MiladiToCalture();

	public DateTime? EndDate { get; set; }

	public string EndDateSh => EndDate.MiladiToCalture();

	public Guid ProductGuid
	{
		get
		{
			return _prdGuid;
		}
		set
		{
			if (!(_prdGuid == value))
			{
				_prdGuid = value;
				_prdToken?.Cancel();
				_prdToken = new CancellationTokenSource(60000);
				CancellationToken token = _prdToken.Token;
				Task.Run(() => UpdateProductInfoAsync(token));
			}
		}
	}

	public string ProductName => _productName;

	public string ProductCode => _productCode;

	public Guid DetailGuid { get; set; }

	public Document AsType { get; set; }

	public string TypeStr => AsType.GetDisplay().GetTranslate();

	public bool IsVorod { get; set; }

	public Guid? OrderGuid { get; set; }

	public DateTime OrderDate { get; set; }

	public string OrderDateSh => OrderDate.MiladiToCalture();

	public Guid CashierGuid
	{
		get
		{
			return _cashierGuid;
		}
		set
		{
			_cashierGuid = value;
			if (!(_cashierGuid == Guid.Empty))
			{
				_cashierToken?.Cancel();
				_cashierToken = new CancellationTokenSource(60000);
				CancellationToken token = _cashierToken.Token;
				Task.Run(() => UpdateCashierInfoAsync(token));
			}
		}
	}

	public string CashierName => _cashierName;

	public string OrderDescription { get; set; }

	public string ContextGuid { get; private set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public PrdSerialDetailReportBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	private async Task UpdateProductInfoAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (token.IsCancellationRequested)
			{
				return;
			}
			ProductCompactBusiness productCompactBusiness = await ProductCompactBusiness.GetAsync(ContextGuid, ProductGuid, EnSystem.None, null, "", token, true);
			_productName = productCompactBusiness?.Name;
			_productCode = productCompactBusiness?.IdProduct;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductCode"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductName"));
				}, "UpdateProductInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialDetailReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateProductInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialDetailReportBusiness.cs", 85);
		}
	}

	private async Task UpdateCashierInfoAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (token.IsCancellationRequested)
			{
				return;
			}
			_cashierName = (await CashierBusiness.GetAsync(ContextGuid, CashierGuid, token))?.Name;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CashierName"));
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "UpdateCashierInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialDetailReportBusiness.cs", 97);
				}
			}, "UpdateCashierInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialDetailReportBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateCashierInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialDetailReportBusiness.cs", 99);
		}
	}
}

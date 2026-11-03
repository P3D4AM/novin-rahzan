using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Gift;

namespace EntityCache.Business.Accounting.Gift;

public class GiftDetailBusiness : Serializable<GiftDetailBusiness>, IHasContext, IGiftDetail
{
	private Guid _productGuid;

	private ProductCompactBusiness _product;

	private CancellationTokenSource _loadData;

	private bool _isDisposed;

	[JsonIgnore]
	public string ContextGuid { get; set; }

	[JsonIgnore]
	public string Name => _product?.Name;

	[JsonIgnore]
	public string Code => _product?.Code.ToString();

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public Guid ProductGuid
	{
		get
		{
			return _productGuid;
		}
		set
		{
			_productGuid = value;
			if (!(_productGuid == Guid.Empty))
			{
				_loadData?.Cancel();
				_loadData = new CancellationTokenSource(60000);
				CancellationToken Token = _loadData.Token;
				Task.Run(() => LoadProductAsync(Token));
			}
		}
	}

	public decimal Count { get; set; } = 1m;


	[JsonIgnore]
	public decimal UserEnteredCount { get; set; }

	[JsonIgnore]
	public byte[] EditImage => ImageResourceManager.OpenInvoiceEdit;

	[JsonIgnore]
	public byte[] DeleteImage => ImageResourceManager.OpenInvoiceDelete;

	public event Func<object, EventArgs, Task> OnDisposed;

	public event PropertyChangedEventHandler PropertyChanged;

	public GiftDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	internal async Task LoadProductAsync(CancellationToken token)
	{
		try
		{
			if (ContextGuid == null || token.IsCancellationRequested)
			{
				return;
			}
			_product = await ProductCompactBusiness.GetAsync(ContextGuid, ProductGuid, EnSystem.UserInterface, null, "", default(CancellationToken), true);
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Name"));
					this.PropertyChanged(this, new PropertyChangedEventArgs("Code"));
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LoadProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftDetailBusiness.cs", 65);
				}
			}, "LoadProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftDetailBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftDetailBusiness.cs", 68);
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
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftDetailBusiness.cs");
				}
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftDetailBusiness.cs", 82);
		}
	}
}

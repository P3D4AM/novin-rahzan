using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.OrderDepot.OrderComparer;

public class OrderDetailComparerWrapper : Serializable<OrderDetailComparerWrapper>
{
	public OrderDetailBusiness OriginalDetail { get; set; }

	public OrderDetailBusiness CurrentDetail { get; set; }

	public decimal ComparedQuantity { get; set; }

	public string ComparedQuantityForDisplay => ((double)ComparedQuantity).ToString().Replace(".", "/");

	public byte[] EditImage => ImageResourceManager.OpenInvoiceEdit;

	public byte[] DeleteImage => ImageResourceManager.OpenInvoiceDelete;

	public byte[] MatchImage => ImageResourceManager.MatchCompare;

	public bool CurrentIsDisposed => CurrentDetail?.IsDisposed ?? false;

	public bool OriginalIsDisposed => OriginalDetail?.IsDisposed ?? false;

	public Guid OriginalGuid => OriginalDetail?.Guid ?? Guid.Empty;

	public int OriginalOrdinal => OriginalDetail?.Ordinal ?? 0;

	public Guid OriginalProductGuid => OriginalDetail?.ProductGuid ?? Guid.Empty;

	public string OriginalProductName => OriginalDetail?.ProductNameForDisplay ?? "";

	public string OriginalProductCode => OriginalDetail?.Code ?? "";

	public string OriginalQuantityUnit => OriginalDetail?.QuantityUnitForDisplay ?? "";

	public decimal OriginalTotalSum => OriginalDetail?.RowSum ?? 0m;

	public decimal OriginalQuantity => OriginalDetail?.Quantity ?? 0m;

	public int CurrentOrdinal => CurrentDetail?.Ordinal ?? 0;

	public Guid CurrentGuid => CurrentDetail?.Guid ?? Guid.Empty;

	public Guid CurrentProductGuid => CurrentDetail?.ProductGuid ?? Guid.Empty;

	public string CurrentProductName => CurrentDetail?.ProductNameForDisplay ?? "";

	public string CurrentQuantityUnit => CurrentDetail?.QuantityUnitForDisplay ?? "";

	public decimal CurrentTotalSum => CurrentDetail?.RowSum ?? 0m;

	public decimal CurrentQuantity => CurrentDetail?.Quantity ?? 0m;

	public event Func<object, EventArgs, Task> OnDisposed;

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			CurrentDetail?.Dispose();
			if (this.OnDisposed != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderDetailComparerWrapper.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderDetailComparerWrapper.cs", 49);
		}
	}
}

using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Order.OpenInvoices;

namespace EntityCache.Business.OrderDepot.OpenInvoices;

public class OpenInvoiceOrderRelationBusiness : Serializable<OpenInvoiceOrderRelationBusiness>, IOpenInvoiceOrderRelation, IHasContext, IDisposable
{
	private bool _isDisposed;

	public Guid InvoiceGuid { get; set; }

	public Guid OrderGuid { get; set; }

	public short Ordinal { get; set; }

	public string ContextGuid { get; set; }

	public event Func<object, EventArgs, Task> OnDisposed;

	public OpenInvoiceOrderRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
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
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceOrderRelationBusiness.cs");
				}
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OpenInvoices\\OpenInvoiceOrderRelationBusiness.cs", 36);
		}
	}
}

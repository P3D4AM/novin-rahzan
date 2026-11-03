using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.TozieList;

namespace EntityCache.Business.Accounting.TozieList;

public class TozieListDetailBusiness : Serializable<TozieListDetailBusiness>, ITozieListDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext, IDisposable
{
	private bool _isDisposed;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public Guid MasterGuid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public bool Status { get; set; } = true;


	public string ContextGuid { get; set; }

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public TozieListDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\TozieList\\TozieListDetailBusiness.cs", 33);
		}
	}
}

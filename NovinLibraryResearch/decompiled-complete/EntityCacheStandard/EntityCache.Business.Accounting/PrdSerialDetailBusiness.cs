using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business.Accounting;

public class PrdSerialDetailBusiness : Serializable<PrdSerialDetailBusiness>, IPrdSerialDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext
{
	private bool _isDisposed;

	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid DetailGuid { get; set; }

	public Document AsType { get; set; }

	public bool IsVorod { get; set; }

	public DateTime Modified { get; set; }

	public bool Status => true;

	public string ContextGuid { get; set; }

	public short Ordinal { get; set; }

	public DateTime? SanadDate { get; set; }

	[XmlIgnore]
	[JsonIgnore]
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

	public PrdSerialDetailBusiness(string contextGuid)
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
				Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductSerial\\PrdSerialDetailBusiness.cs", 37);
		}
	}
}

using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace EntityCache.Business.Accounting.Sanad;

public class SanadStatusReportUpdatesBusiness : BaseBusiness<SanadStatusReportUpdatesBusiness>, ISanadStatusReportUpdates, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, ISyncFromClient2Server<SanadStatusReportUpdatesBusiness>, IDeliverableEntity, IEditableDelivery
{
	public Guid Guid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public int StatusId { get; set; }

	public Guid CashierGuid { get; set; }

	public DateTime DateM { get; set; }

	public string Description { get; set; }

	public int Ordinal { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public override string ContextGuid { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public SanadStatusReportUpdatesBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public override IEditableDelivery GetRepository()
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		throw new NotImplementedException();
	}
}

using System;
using EntityCache.Business.Attendance;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Attendance;

public interface IAttPersonLeaveRepository : ISyncRepository<AttPersonLeaveBusiness>, IEditableDelivery, IRepository<AttPersonLeaveBusiness>, INoGuidRepository<AttPersonLeaveBusiness>, IDisposable
{
}

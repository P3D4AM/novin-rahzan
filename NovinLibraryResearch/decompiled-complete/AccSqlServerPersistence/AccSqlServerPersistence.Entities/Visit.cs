using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_Visit")]
public class Visit : Serializable<IVisit>, IVisit, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; }

	public DateTime ScheduledDate { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime CreationdDate { get; set; }

	[Required]
	public string Description { get; set; }

	public int VisitNo { get; set; }

	public bool VisitIsCompleted { get; set; }

	public string? SerializedTicketGuids { get; set; }

	public string? SerializedData { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public string? MetaData { get; set; }

	public T FromString<T>(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\Visit.cs"))
		{
			try
			{
				return data.FromJson<T>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\Visit.cs", 44);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\Visit.cs", 48);
				throw;
			}
		}
	}
}

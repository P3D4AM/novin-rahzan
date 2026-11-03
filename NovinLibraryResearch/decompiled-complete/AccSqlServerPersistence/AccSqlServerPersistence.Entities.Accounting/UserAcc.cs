using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser;
using PacketParser.Access;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_UserAcc")]
public class UserAcc : Serializable<IUserAcc>, IUserAcc, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[Required]
	public string Description { get; set; }

	[Required]
	public string Name { get; set; }

	public bool Status { get; set; }

	public string? UserAccessStr { get; set; } = "";


	public string? SerializedData { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public short Id { get; set; }

	[NotMapped]
	public new bool IsDisposed => base.IsDisposed;

	[NotMapped]
	public Guid? DefaultCusGroup { get; set; }

	[NotMapped]
	public AccessLevel UserAccess { get; set; }

	[NotMapped]
	public List<Tuple<DateTime, DateTime>> UserTime { get; set; }

	public ICollection<Cashier> Cashiers { get; set; }

	public T FromString<T>(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\Accounting\\UserAcc.cs"))
		{
			try
			{
				return data.FromJson<T>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\Accounting\\UserAcc.cs", 45);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\Accounting\\UserAcc.cs", 49);
				throw;
			}
		}
	}
}

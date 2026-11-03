using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Department;

namespace AccSqlServerPersistence.Entities.Department;

[Table("Dpt_LkInfo")]
public class LkInfo : ILkInfo, IHasModified, IHasStatus, IDeliverableEntity, IHasMetaData
{
	public Guid Guid { get; set; }

	[Key]
	public long LKSerial { get; set; }

	public AccountingPackage AccountingPackage { get; set; }

	public ENCode TypeAccounting { get; set; }

	public HozoorPackage HozoorType { get; set; }

	public Guid TafsilGuid { get; set; }

	public DateTime ExpireDate { get; set; }

	public DateTime StartDate { get; set; }

	public ushort DeActivateDays { get; set; }

	[Required]
	public string Description { get; set; }

	public bool Tolid { get; set; }

	public bool BarCode { get; set; }

	public bool Scale { get; set; }

	public bool SpecialAccountingPackage { get; set; }

	public bool IsOffline { get; set; }

	public TypeHozor Hozor_Hoghogh { get; set; }

	public short ScaleCount { get; set; }

	public short MaxActivationDays { get; set; }

	public short Net_Accounting { get; set; }

	public short Net_Hozor { get; set; }

	public short Net_Scales { get; set; }

	public bool CallerID { get; set; }

	public DateTime? FailedDate { get; set; }

	public string? FailedDescription { get; set; }

	public bool IsFailed => !string.IsNullOrEmpty(FailedDescription);

	[NotMapped]
	public string ExpireDateSh => ExpireDate.MiladiToCalture();

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public DateTime? DisableActivationCodeDate { get; set; }

	public string? MetaData { get; set; }

	public EnLK_SpecialFacilities SpecialFacilities { get; set; }
}

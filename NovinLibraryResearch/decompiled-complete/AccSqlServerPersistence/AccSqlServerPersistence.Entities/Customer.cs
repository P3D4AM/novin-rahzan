using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;
using SharedAccEntityFramworkPersistence.Entities.Customer;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_Customer")]
public class Customer : ICus, ICusCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string FullName
	{
		get
		{
			return Name + " " + Family;
		}
		set
		{
		}
	}

	[Required]
	public string Address1 { get; set; }

	[Required]
	public string Address2 { get; set; }

	[Required]
	public string CodeMeli { get; set; }

	[Required]
	public string Company { get; set; }

	public DateTime Date { get; set; }

	public EnPriceType DefPrice { get; set; }

	[Required]
	public string Description { get; set; }

	[Required]
	public string EghtesadiNo { get; set; }

	public decimal Etebar { get; set; }

	public decimal EtebarCheck { get; set; }

	[Required]
	public string Family { get; set; }

	[Obsolete("Use AreaGuid")]
	public int IdPath { get; set; }

	public Guid? AreaGuid { get; set; }

	public bool IsBazaryab { get; set; }

	public bool IsSupervisor { get; set; }

	public bool IsEtebari { get; set; }

	[Required]
	public string Job { get; set; }

	[Required]
	public string Location { get; set; }

	[Required]
	public string Name { get; set; }

	[StringLength(50)]
	[Required]
	public string? Password { get; set; }

	[StringLength(50)]
	public string? Email { get; set; }

	[Required]
	public string PicFace { get; set; }

	[Required]
	public string PicSign { get; set; }

	public Guid SupervisorGuid { get; set; }

	public bool Special { get; set; }

	public bool Status { get; set; }

	[StringLength(100)]
	public string? User { get; set; }

	[Required]
	public string Zip { get; set; }

	public CustomerType CustType { get; set; }

	public DateTime Modified { get; set; }

	public string? SerializedData { get; set; }

	public Guid GroupGuid { get; set; }

	public Guid VisitorGuid { get; set; }

	public decimal BPercent { get; set; }

	public bool InPlacePayment { get; set; }

	public Guid TitleGuid { get; set; }

	public short TafsilLevel { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public ReturnedState ChequeBargashtiValidation { get; set; }

	public ReturnedState OrderTasvieTimeValidation { get; set; }

	public TypeTax TaxType { get; set; }

	public DateTime? SendToServerDate { get; set; }

	public Guid? RoleGuid { get; set; }

	public string? PhoneNumbers { get; set; }

	public CustomerTitle CusTitle { get; set; }

	public CustomerGroup CusGroup { get; set; }

	public CustomerRole Role { get; set; }

	public Tafsil Tafsil { get; set; }

	public VisitorArea VisitorArea { get; set; }

	public ICollection<CusCard> CusCard { get; set; }

	public ICollection<CustomerReceiptRelation> CustomerReceipt { get; set; }
}

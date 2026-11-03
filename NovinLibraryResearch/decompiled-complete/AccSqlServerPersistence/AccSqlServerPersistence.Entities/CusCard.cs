using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Interfaces;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_CusCard")]
public class CusCard : ICusCard, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public Guid CusGuid { get; set; }

	[Required]
	public string CardName { get; set; }

	[Required]
	public string CardNo { get; set; }

	public bool Status { get; set; } = true;


	[Required]
	public string Pass { get; set; }

	public DateTime DateStart { get; set; }

	public DateTime DateExpire { get; set; }

	[Required]
	public string Description { get; set; }

	public Customer Customers { get; set; }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_SuggestedBank")]
public class SuggestedBank : ISuggestedBank, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	[Required]
	public string ShobeCode { get; set; }

	[Required]
	public string AccountNo { get; set; }

	[Required]
	public string BankName { get; set; }
}

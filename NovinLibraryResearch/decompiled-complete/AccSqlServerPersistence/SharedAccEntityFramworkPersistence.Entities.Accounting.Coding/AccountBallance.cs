using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Coding;

[Table("Acc_AccountBallance")]
public class AccountBallance
{
	public Guid MoeinGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	public byte[]? GardeshHesab { get; set; }
}

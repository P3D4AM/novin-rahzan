using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Details;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Khazane.Reception;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Reception;

[Table("Acc_ReceptionChequeEntezami")]
public class ReceptionChequeEntezami : IReceptionChequeEntezami, IHasModified, IHasMetaData
{
	private DateTime _modified;

	[Key]
	public Guid ChequeGuid { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now : value);
			_modified = modified;
		}
	}

	public Guid CashierGuid { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public int SanadNo { get; set; }

	public string Description { get; set; }

	public string MetaData { get; set; }

	public DateTime ReceptionDateM { get; set; }

	public Cashier CashierItem { get; set; }

	public ReceptionCheck ChequeItem { get; set; }

	public Moein DebitMoeinItem { get; set; }

	public Tafsil DebitTafsilItem { get; set; }

	public Moein CreditMoeinItem { get; set; }

	public Tafsil CreditTafsilItem { get; set; }
}

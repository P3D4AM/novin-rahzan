using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Khazane;

namespace AccSqlServerPersistence.Entities.Details;

[Table("Acc_ReceptionHavale")]
public class ReceptionHavale : IReceptionHavale, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasWage, IHasMasterGuid, IHasMetaData
{
	private DateTime _modified;

	private DateTime _date;

	private DateTime _dateTasviye;

	[Key]
	public Guid Guid { get; set; }

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

	[NotMapped]
	public bool Status { get; }

	public DateTime Date
	{
		get
		{
			return _date;
		}
		set
		{
			DateTime date = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now : value);
			_date = date;
		}
	}

	[Required]
	public string Description { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	[Required]
	public string Number { get; set; }

	public decimal Price { get; set; }

	public ENHavaleType Type { get; set; }

	public string? TerminalId { get; set; }

	public string? PAN { get; set; }

	public string? STAN { get; set; }

	public string? RRN { get; set; }

	public string? MerchantId { get; set; }

	public string? MessageId { get; set; }

	public DateTime DateTasviye
	{
		get
		{
			return _dateTasviye;
		}
		set
		{
			DateTime dateTasviye = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now : value);
			_dateTasviye = dateTasviye;
		}
	}

	public string? FullDescription { get; set; }

	public decimal Qt { get; set; }

	public Guid? DecimalGuid { get; set; }

	public Guid MasterGuid { get; set; }

	[NotMapped]
	public string ShortDescription { get; }

	public int Ordinal { get; set; }

	public decimal? BankRealAccount { get; set; }

	public string? MetaData { get; set; }

	public Guid? WageGuid { get; set; }

	public Guid? WageMoeinGuid { get; set; }

	public Guid? WageTafsilGuid { get; set; }

	public decimal WagePrice { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public Tafsil TafsilItem { get; set; }

	public Moein MoeinItem { get; set; }

	public Reception ReceptionItem { get; set; }

	public DecimalEntity DeciamlItem { get; set; }

	public Moein WageMoeinItem { get; set; }

	public Tafsil WageTafsilItem { get; set; }
}

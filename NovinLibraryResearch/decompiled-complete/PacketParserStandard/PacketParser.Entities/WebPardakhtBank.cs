using System;
using System.ComponentModel;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Khazane;

namespace PacketParser.Entities;

[Serializable]
public class WebPardakhtBank : IPardakhtBank, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasWage
{
	[Obsolete]
	public int Id { get; set; }

	public int BankId { get; set; }

	public decimal Price { get; set; }

	public HesabType HType { get; }

	public int HTreeId { get; }

	public DateTime DateM { get; set; }

	[Obsolete("", true)]
	public DateTime? DateMUtc { get; set; }

	public DateTime? DateUtc { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Number { get; set; }

	public string Description { get; set; }

	public int TafsilId { get; }

	public int MoeinId { get; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid? DecimalGuid { get; set; }

	[JsonIgnore]
	public Guid MasterGuid { get; set; }

	public int MasterId { get; set; }

	public string ShortDescription => "بانک";

	public int Ordinal { get; set; }

	public decimal? BankRealAccount { get; set; }

	public Guid? WageGuid { get; set; }

	public Guid? WageMoeinGuid { get; set; }

	public Guid? WageTafsilGuid { get; set; }

	public decimal WagePrice { get; set; }
}

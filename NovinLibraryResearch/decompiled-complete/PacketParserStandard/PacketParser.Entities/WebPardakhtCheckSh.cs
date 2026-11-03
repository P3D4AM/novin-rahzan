using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;

namespace PacketParser.Entities;

[Serializable]
public class WebPardakhtCheckSh : IPardakhtCheckSh, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public string Number { get; set; }

	public int Idbank { get; set; }

	public string Description { get; set; }

	public DateTime DateSaresid { get; set; }

	public decimal Price { get; set; }

	public Guid Guid { get; set; }

	[Obsolete]
	public int Id { get; set; }

	[Obsolete]
	public int? IDSanad { get; set; }

	public short Status { get; set; }

	public DateTime Modified { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	bool IHasStatus.Status => Status == 1;

	public Guid BankTafsilGuid { get; set; }

	[JsonIgnore]
	public Guid? MasterGuid { get; set; }

	[JsonIgnore]
	Guid IHasMasterGuid.MasterGuid { get; set; }

	public Guid? UserGuid { get; set; }

	public bool IsEnteghali { get; set; }

	public DateTime PardakhtDate { get; set; }

	public bool Aval2re { get; set; }

	public Guid? A2re_MoeinGuid { get; set; }

	public Guid? A2re_TafsilGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public string ShortDescription => "چک شخصی";

	public Guid DebitTafsilGuid { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? DebitA2reMoeinGuid { get; set; }

	public Guid? DebitA2reTafsiGuid { get; set; }

	public int? SanadNo { get; set; }

	public string SayadiNationalCode { get; set; }

	public string SayadiIdentity { get; set; }

	public string SayadiName { get; set; }

	public bool SayadiRegistered { get; set; }

	public PardakhtChequeShMetaData MetaData { get; set; }

	public int Ordinal { get; set; }

	public DateTime? DateUtc { get; set; }
}

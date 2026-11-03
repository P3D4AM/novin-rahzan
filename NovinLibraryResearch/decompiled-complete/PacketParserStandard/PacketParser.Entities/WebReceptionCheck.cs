using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;

namespace PacketParser.Entities;

[Serializable]
public class WebReceptionCheck : IReceptionCheck, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public string Number { get; set; }

	public bool Personal { get; set; }

	public string BankName { get; set; }

	public string Description { get; set; }

	public DateTime Date { get; set; }

	public decimal Price { get; set; }

	public string HesabNum { get; set; }

	public long PoshtNomreh { get; set; }

	[JsonIgnore]
	public string DateSh => Date.MiladiToCalture();

	public ENUMCheck Status { get; set; }

	[JsonIgnore]
	public Guid? MasterGuid { get; set; }

	[JsonIgnore]
	Guid IHasMasterGuid.MasterGuid { get; set; }

	public Guid? A2re_PardazandeTafsilGuid { get; set; }

	public Guid? A2re_PardazandeMoeinGuid { get; set; }

	public DateTime? A2re_ReceptionDate { get; set; }

	public Guid? VisitorGuid { get; set; }

	public bool Aval2re { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	bool IHasStatus.Status => Status == ENUMCheck.BatelShode;

	[JsonIgnore]
	public string ShortDescription => $"چک- {Price} ریال به تاریخ {DateSh}";

	[Obsolete]
	public int Id { get; set; }

	[Obsolete]
	public int SanadId { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public bool IsEnteghali { get; set; }

	public Guid? UserGuid { get; set; }

	public Guid? CreditA2reMoeinGuid { get; set; }

	public Guid? CreditA2reTafsilGuid { get; set; }

	public int? SanadNo { get; set; }

	public string SayadiNationalCode { get; set; }

	public string SayadiIdentity { get; set; }

	public string SayadiName { get; set; }

	public ReceptionChequeMetaData MetaData { get; set; }

	public bool SayadiRegistered { get; set; }

	public int Ordinal { get; set; }

	public DateTime? DateUtc { get; set; }
}

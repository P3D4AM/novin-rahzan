using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Details;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_Decimal")]
public class DecimalEntity : IDecimal, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	[MaxLength(200)]
	[Required]
	public string Name { get; set; }

	[MaxLength(200)]
	[Required]
	public string Country { get; set; }

	[Required]
	public string Description { get; set; }

	public bool Status { get; set; }

	[MaxLength(50)]
	[Required]
	public string Abstract { get; set; }

	public DateTime LastUpdate { get; set; }

	public decimal LastPrice { get; set; }

	public short IconIndex { get; set; }

	public DateTime Modified { get; set; }

	[MaxLength(50)]
	public string? Symbol { get; set; }

	public ICollection<OrderDetail> OrderDetailItem { get; set; }

	public ICollection<PardakhtBank> PardakhtBankItem { get; set; }

	public ICollection<PardakhtNaghd> PardakhtNaghdItem { get; set; }

	public ICollection<ReceptionHavale> ReceptionHavaleItem { get; set; }

	public ICollection<ReceptionNaghd> ReceptionNaghdItem { get; set; }

	public ICollection<DecimalUpdates> DecimalUpdateses { get; set; }

	public ICollection<GardeshTafsil> GardeshTafsils { get; set; }

	public ICollection<Sanad_Det> SanadDet { get; set; }

	public ICollection<ViewSanadDetail> SanadDetails { get; set; }

	public ICollection<ViewSanadDetailCover> ViewSanadDetailCover { get; set; }

	public ICollection<ProductDecimalRelation> ProductPrices { get; set; }
}

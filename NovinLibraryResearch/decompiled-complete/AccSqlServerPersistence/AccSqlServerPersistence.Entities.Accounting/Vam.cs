using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_Vam")]
public class Vam : IVam, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public int Id2 { get; set; }

	public decimal Zarib { get; set; }

	[Required]
	public string Description { get; set; }

	public DateTime DateM { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public Guid VamMoeinGuid { get; set; }

	public Guid VamTafsilGuid { get; set; }

	public Guid SepordeMoeinGuid { get; set; }

	public Guid SepordeTafsilGuid { get; set; }

	public bool Status { get; set; }

	public Guid? AghsatGuid { get; set; }

	public decimal Sum { get; set; }

	public Guid? ZamenGuid1 { get; set; }

	public Guid? ZamenGuid2 { get; set; }

	public Guid? ZamenGuid3 { get; set; }

	public Guid? ZamenGuid4 { get; set; }

	public DateTime Date1 { get; set; }

	public DateTime Date2 { get; set; }

	public short Average { get; set; }

	public ENMoadel AverageType { get; set; }

	public decimal AverageSum { get; set; }

	public decimal ProfitPercent { get; set; }

	public Tafsil VamSepordeTafsil { get; set; }

	public Moein VamSepordeMoein { get; set; }

	public Tafsil VamGirandeTafsil { get; set; }

	public Moein VamGirandeMoein { get; set; }

	public Tafsil Zamen1 { get; set; }

	public Tafsil Zamen2 { get; set; }

	public Tafsil Zamen3 { get; set; }

	public Tafsil Zamen4 { get; set; }

	public Pardakht Pardakht { get; set; }
}

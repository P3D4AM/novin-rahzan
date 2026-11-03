using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid;

namespace AccSqlServerPersistence.Entities.Accounting.Tolid;

[Table("Acc_Tolid_Formula")]
public class TolidFormula : ITolidFormula, IHasJustGuid, IHasModified, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public DateTime DateM { get; set; }

	[Required]
	public string Title { get; set; }

	[Required]
	public string Description { get; set; }

	public ICollection<TolidFormulaExport> TolidFormulaExport { get; set; }

	public ICollection<TolidFormulaMavad> TolidFormulaMavad { get; set; }

	public ICollection<TolidFormulaHazine> TolidFormulaHaizne { get; set; }

	public ICollection<Product> Products { get; set; }

	public string? MetaData { get; set; }
}

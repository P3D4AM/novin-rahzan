using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_MoeinTafsilLevelRelations")]
public class MoeinTafsilGroupRelations : IMoeinTafsilGroupRelations
{
	public Guid MoeinGuid { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public Moein MoeinItem { get; set; }

	public EnMoeinTafsilRelationLevel Level { get; set; }

	public TafsilGroup TafsilLevelItem { get; set; }
}

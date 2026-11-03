using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_LocalSetting")]
public class LocalSetting : Serializable<LocalSetting>
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[Required]
	public string Key { get; set; }

	[Required]
	public string Value { get; set; }
}

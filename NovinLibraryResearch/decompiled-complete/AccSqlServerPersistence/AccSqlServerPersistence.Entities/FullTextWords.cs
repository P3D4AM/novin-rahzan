using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Indexes;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_FullTextWords")]
public class FullTextWords : IFullTextWord
{
	[Required]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int Id { get; set; }

	[MaxLength(400)]
	[Required]
	[Key]
	public string Word { get; set; }

	public ICollection<IxProductT1Field> IxProductT1Field { get; set; }

	public ICollection<IxProductT2Field> IxProductT2Field { get; set; }

	public ICollection<IxProductT3Field> IxProductT3Field { get; set; }

	public ICollection<IxProductT4Field> IxProductT4Field { get; set; }

	public ICollection<IxProductT5Field> IxProductT5Field { get; set; }

	public ICollection<IxProductT6Field> IxProductT6Field { get; set; }

	public ICollection<IxProductT7Field> IxProductT7Field { get; set; }

	public ICollection<IxProductT8Field> IxProductT8Field { get; set; }

	public ICollection<IxCustomerAllFields> IxCustomerAllFields { get; set; }
}

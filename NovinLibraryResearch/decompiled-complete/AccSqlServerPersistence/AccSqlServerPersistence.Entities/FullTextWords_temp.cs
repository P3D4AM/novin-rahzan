using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_FullTextWords_temp")]
public class FullTextWords_temp
{
	[MaxLength(400)]
	[Required]
	public string Word { get; set; }
}

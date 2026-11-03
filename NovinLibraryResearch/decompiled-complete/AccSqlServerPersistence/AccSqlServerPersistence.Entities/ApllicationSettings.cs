using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_ApllicationSetting")]
public class ApllicationSettings
{
	[Key]
	[Required]
	public string Name { get; set; }

	[Required]
	public string val { get; set; }

	public DateTime Modified { get; set; }
}

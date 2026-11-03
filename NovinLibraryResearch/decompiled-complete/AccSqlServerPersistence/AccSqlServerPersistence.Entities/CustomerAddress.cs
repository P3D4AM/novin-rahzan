using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_CustomerAddress")]
public class CustomerAddress : ICustomerAddress, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public short State { get; set; }

	public short City { get; set; }

	[Required]
	public string Address { get; set; }

	[Required]
	public string PostalCode { get; set; }

	[Required]
	public string FullName { get; set; }

	[Required]
	public string Phone { get; set; }

	[Required]
	public string CellPhone { get; set; }

	[Required]
	public string NationalCode { get; set; }

	[Key]
	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;

}

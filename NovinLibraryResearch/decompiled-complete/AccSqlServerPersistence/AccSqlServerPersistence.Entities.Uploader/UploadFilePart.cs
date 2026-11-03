using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Uploader;

[Table("Acc_UploadFilePart")]
public class UploadFilePart : IUploadFilePart, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Order { get; set; }

	public Guid FileId { get; set; }

	public byte[] Bytes { get; set; }

	public short RetryCount { get; set; }

	public bool IsSent { get; set; }

	public DateTime NextTry { get; set; } = DateTime.Now;


	public byte[] GetBytes()
	{
		throw new NotImplementedException();
	}
}

using System;
using EntityCache.Utilities;

namespace EntityCache.Models.Accounting.Prd;

public record ProductTagsRecord
{
	public Guid Guid { get; set; }

	public string Tag { get; set; }

	public byte[] EditImage => ImageResourceManager.OpenInvoiceEdit;

	public byte[] DeleteImage => ImageResourceManager.OpenInvoiceDelete;
}

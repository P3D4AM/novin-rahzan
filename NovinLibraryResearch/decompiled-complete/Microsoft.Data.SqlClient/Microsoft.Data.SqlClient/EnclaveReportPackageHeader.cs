using System;

namespace Microsoft.Data.SqlClient;

internal class EnclaveReportPackageHeader
{
	public uint PackageSize { get; set; }

	public uint Version { get; set; }

	public uint SignatureScheme { get; set; }

	public uint SignedStatementSize { get; set; }

	public uint SignatureSize { get; set; }

	public uint Reserved { get; set; }

	public EnclaveReportPackageHeader(byte[] payload)
	{
		int num = 0;
		PackageSize = BitConverter.ToUInt32(payload, num);
		num += 4;
		Version = BitConverter.ToUInt32(payload, num);
		num += 4;
		SignatureScheme = BitConverter.ToUInt32(payload, num);
		num += 4;
		SignedStatementSize = BitConverter.ToUInt32(payload, num);
		num += 4;
		SignatureSize = BitConverter.ToUInt32(payload, num);
		num += 4;
		Reserved = BitConverter.ToUInt32(payload, num);
		num += 4;
	}

	public int GetSizeInPayload()
	{
		return 24;
	}
}

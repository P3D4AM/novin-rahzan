using System;

namespace Microsoft.Data.SqlClient;

internal class EnclaveReportModuleHeader
{
	public uint DataType { get; set; }

	public uint ModuleSize { get; set; }

	public EnclaveReportModuleHeader(byte[] payload)
	{
		int num = 0;
		DataType = BitConverter.ToUInt32(payload, num);
		num += 4;
		ModuleSize = BitConverter.ToUInt32(payload, num);
		num += 4;
	}

	public int GetSizeInPayload()
	{
		return 8;
	}
}

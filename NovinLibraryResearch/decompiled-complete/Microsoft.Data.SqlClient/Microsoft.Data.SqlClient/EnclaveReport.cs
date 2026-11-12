using System;
using System.Linq;

namespace Microsoft.Data.SqlClient;

internal class EnclaveReport
{
	private const int EnclaveDataLength = 64;

	private int Size { get; set; }

	public uint ReportSize { get; set; }

	public uint ReportVersion { get; set; }

	public byte[] EnclaveData { get; set; }

	public EnclaveIdentity Identity { get; set; }

	public EnclaveReport(byte[] payload)
	{
		Size = payload.Length;
		int num = 0;
		ReportSize = BitConverter.ToUInt32(payload, num);
		num += 4;
		ReportVersion = BitConverter.ToUInt32(payload, num);
		num += 4;
		EnclaveData = payload.Skip(num).Take(64).ToArray();
		num += 64;
		Identity = new EnclaveIdentity(payload.Skip(num).ToArray());
		num += Identity.GetSizeInPayload();
	}

	public int GetSizeInPayload()
	{
		return 72 + Identity.GetSizeInPayload();
	}
}

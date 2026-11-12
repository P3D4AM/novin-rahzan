using System;
using System.Linq;

namespace Microsoft.Data.SqlClient;

internal class AttestationInfo
{
	public uint TotalSize { get; set; }

	public EnclavePublicKey Identity { get; set; }

	public HealthReport HealthReport { get; set; }

	public EnclaveReportPackage EnclaveReportPackage { get; set; }

	public long SessionId { get; set; }

	public EnclaveDiffieHellmanInfo EnclaveDHInfo { get; set; }

	public AttestationInfo(byte[] attestationInfo)
	{
		int num = 0;
		TotalSize = BitConverter.ToUInt32(attestationInfo, num);
		num += 4;
		int num2 = BitConverter.ToInt32(attestationInfo, num);
		num += 4;
		int num3 = BitConverter.ToInt32(attestationInfo, num);
		num += 4;
		int count = BitConverter.ToInt32(attestationInfo, num);
		num += 4;
		byte[] payload = attestationInfo.Skip(num).Take(num2).ToArray();
		Identity = new EnclavePublicKey(payload);
		num += num2;
		byte[] payload2 = attestationInfo.Skip(num).Take(num3).ToArray();
		HealthReport = new HealthReport(payload2);
		num += num3;
		byte[] payload3 = attestationInfo.Skip(num).Take(count).ToArray();
		EnclaveReportPackage = new EnclaveReportPackage(payload3);
		num += EnclaveReportPackage.GetSizeInPayload();
		uint value = BitConverter.ToUInt32(attestationInfo, num);
		num += 4;
		SessionId = BitConverter.ToInt64(attestationInfo, num);
		num += 8;
		int count2 = Convert.ToInt32(value) - 4;
		byte[] payload4 = attestationInfo.Skip(num).Take(count2).ToArray();
		EnclaveDHInfo = new EnclaveDiffieHellmanInfo(payload4);
		num += Convert.ToInt32(EnclaveDHInfo.Size);
	}
}

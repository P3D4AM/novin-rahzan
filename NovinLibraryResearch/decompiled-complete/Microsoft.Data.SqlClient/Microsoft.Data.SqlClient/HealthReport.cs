using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Data.SqlClient;

internal class HealthReport
{
	private int Size { get; set; }

	public X509Certificate2 Certificate { get; set; }

	public HealthReport(byte[] payload)
	{
		Size = payload.Length;
		Certificate = new X509Certificate2(payload);
	}

	public int GetSizeInPayload()
	{
		return Size;
	}
}

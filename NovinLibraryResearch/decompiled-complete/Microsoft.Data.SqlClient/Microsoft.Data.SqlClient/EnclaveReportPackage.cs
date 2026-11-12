using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.SqlClient;

internal class EnclaveReportPackage
{
	private int Size { get; set; }

	public EnclaveReportPackageHeader PackageHeader { get; set; }

	public EnclaveReport Report { get; set; }

	public List<EnclaveReportModule> Modules { get; set; }

	public byte[] ReportAsBytes { get; set; }

	public byte[] SignatureBlob { get; set; }

	public EnclaveReportPackage(byte[] payload)
	{
		Size = payload.Length;
		int num = 0;
		PackageHeader = new EnclaveReportPackageHeader(payload.Skip(num).ToArray());
		num += PackageHeader.GetSizeInPayload();
		Report = new EnclaveReport(payload.Skip(num).ToArray());
		num += Report.GetSizeInPayload();
		num = PackageHeader.GetSizeInPayload();
		int num2 = Convert.ToInt32(PackageHeader.SignedStatementSize);
		ReportAsBytes = payload.Skip(num).Take(num2).ToArray();
		num += num2;
		int num3 = Convert.ToInt32(PackageHeader.SignatureSize);
		SignatureBlob = payload.Skip(num).Take(num3).ToArray();
		num += num3;
	}

	public int GetSizeInPayload()
	{
		return Size;
	}
}

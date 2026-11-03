using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiPdfExportSettings : StiPageRangeExportSettings
{
	[DefaultValue(0.75f)]
	public float ImageQuality { get; set; } = 0.75f;


	[DefaultValue(StiImageResolutionMode.Exactly)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiImageResolutionMode ImageResolutionMode { get; set; } = StiImageResolutionMode.Exactly;


	[DefaultValue(100f)]
	public float ImageResolution { get; set; } = 100f;


	[DefaultValue(true)]
	public bool EmbeddedFonts { get; set; } = true;


	[DefaultValue(false)]
	public bool StandardPdfFonts { get; set; }

	[DefaultValue(true)]
	public bool Compressed { get; set; } = true;


	[DefaultValue(false)]
	public bool ExportRtfTextAsImage { get; set; }

	[DefaultValue("")]
	public string PasswordInputUser { get; set; } = string.Empty;


	[DefaultValue("")]
	public string PasswordInputOwner { get; set; } = string.Empty;


	[DefaultValue(StiUserAccessPrivileges.All)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiUserAccessPrivileges UserAccessPrivileges { get; set; } = StiUserAccessPrivileges.All;


	[DefaultValue(StiPdfEncryptionKeyLength.Bit40)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiPdfEncryptionKeyLength KeyLength { get; set; } = StiPdfEncryptionKeyLength.Bit40;


	[DefaultValue(true)]
	public bool UseUnicode { get; set; } = true;


	[DefaultValue(false)]
	public bool UseDigitalSignature { get; set; }

	[DefaultValue(false)]
	public bool GetCertificateFromCryptoUI { get; set; }

	[DefaultValue("")]
	public string SubjectNameString { get; set; } = string.Empty;


	[DefaultValue(false)]
	public bool UseLocalMachineCertificates { get; set; }

	[DefaultValue(null)]
	public byte[] CertificateData { get; set; }

	[DefaultValue(null)]
	public string CertificateThumbprint { get; set; }

	[DefaultValue(null)]
	public string CertificatePassword { get; set; }

	[DefaultValue("")]
	public string CreatorString { get; set; } = string.Empty;


	[DefaultValue("")]
	public string KeywordsString { get; set; } = string.Empty;


	[DefaultValue(StiPdfImageCompressionMethod.Jpeg)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiPdfImageCompressionMethod ImageCompressionMethod { get; set; } = StiPdfImageCompressionMethod.Jpeg;


	[DefaultValue(32)]
	public int ImageIndexedColorPaletteSize { get; set; } = 96;


	[DefaultValue(StiImageFormat.Color)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiImageFormat ImageFormat { get; set; } = StiImageFormat.Color;


	[DefaultValue(StiMonochromeDitheringType.FloydSteinberg)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiMonochromeDitheringType DitheringType { get; set; } = StiMonochromeDitheringType.FloydSteinberg;


	[Obsolete("Please use PdfComplianceMode")]
	[DefaultValue(false)]
	public bool PdfACompliance
	{
		get
		{
			return PdfComplianceMode != StiPdfComplianceMode.None;
		}
		set
		{
			PdfComplianceMode = (value ? StiPdfComplianceMode.A1 : StiPdfComplianceMode.None);
		}
	}

	[DefaultValue(StiPdfComplianceMode.None)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiPdfComplianceMode PdfComplianceMode { get; set; }

	[DefaultValue(StiPdfAutoPrintMode.None)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiPdfAutoPrintMode AutoPrintMode { get; set; } = StiOptions.Export.Pdf.DefaultAutoPrintMode;


	[DefaultValue(StiPdfAllowEditable.No)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiPdfAllowEditable AllowEditable { get; set; } = StiPdfAllowEditable.No;


	[DefaultValue(null)]
	public string DigitalSignatureReason { get; set; } = StiOptions.Export.Pdf.DigitalSignatureReason;


	[DefaultValue(null)]
	public string DigitalSignatureLocation { get; set; } = StiOptions.Export.Pdf.DigitalSignatureLocation;


	[DefaultValue(null)]
	public string DigitalSignatureContactInfo { get; set; } = StiOptions.Export.Pdf.DigitalSignatureContactInfo;


	[DefaultValue(null)]
	public string DigitalSignatureSignedBy { get; set; } = StiOptions.Export.Pdf.DigitalSignatureSignedBy;


	[DefaultValue(null)]
	public List<StiPdfEmbeddedFileData> EmbeddedFiles { get; set; } = new List<StiPdfEmbeddedFileData>();


	[Obsolete("Please use ZUGFeRDComplianceMode")]
	[DefaultValue(false)]
	public bool ZUGFeRDCompliance
	{
		get
		{
			return ZUGFeRDComplianceMode != StiPdfZUGFeRDComplianceMode.None;
		}
		set
		{
			ZUGFeRDComplianceMode = (value ? StiPdfZUGFeRDComplianceMode.V1 : StiPdfZUGFeRDComplianceMode.None);
		}
	}

	[DefaultValue(StiPdfZUGFeRDComplianceMode.None)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiPdfZUGFeRDComplianceMode ZUGFeRDComplianceMode { get; set; }

	[DefaultValue("BASIC")]
	public string ZUGFeRDConformanceLevel { get; set; } = "BASIC";


	[DefaultValue(null)]
	public byte[] ZUGFeRDInvoiceData { get; set; }

	public override StiExportFormat GetExportFormat()
	{
		return StiExportFormat.Pdf;
	}
}

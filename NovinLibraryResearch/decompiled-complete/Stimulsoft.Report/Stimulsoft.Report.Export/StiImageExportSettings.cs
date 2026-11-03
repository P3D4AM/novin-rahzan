using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiImageExportSettings : StiPageRangeExportSettings
{
	private StiImageType imageType;

	[DefaultValue(StiImageType.Jpeg)]
	[JsonConverter(typeof(StringEnumConverter))]
	public virtual StiImageType ImageType
	{
		get
		{
			return imageType;
		}
		set
		{
			imageType = value;
		}
	}

	[DefaultValue(1.0)]
	public double ImageZoom { get; set; } = 1.0;


	[DefaultValue(100)]
	public int ImageResolution { get; set; } = 100;


	[DefaultValue(false)]
	public bool CutEdges { get; set; }

	[DefaultValue(StiImageFormat.Color)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiImageFormat ImageFormat { get; set; } = StiImageFormat.Color;


	[DefaultValue(false)]
	public virtual bool MultipleFiles { get; set; }

	[DefaultValue(StiMonochromeDitheringType.FloydSteinberg)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiMonochromeDitheringType DitheringType { get; set; } = StiMonochromeDitheringType.FloydSteinberg;


	[DefaultValue(StiTiffCompressionScheme.Default)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiTiffCompressionScheme TiffCompressionScheme { get; set; } = StiTiffCompressionScheme.Default;


	[DefaultValue(false)]
	public bool CompressToArchive { get; set; }

	public override StiExportFormat GetExportFormat()
	{
		return ImageType switch
		{
			StiImageType.Bmp => StiExportFormat.ImageBmp, 
			StiImageType.Emf => StiExportFormat.ImageEmf, 
			StiImageType.Gif => StiExportFormat.ImageGif, 
			StiImageType.Jpeg => StiExportFormat.ImageJpeg, 
			StiImageType.Pcx => StiExportFormat.ImagePcx, 
			StiImageType.Png => StiExportFormat.ImagePng, 
			StiImageType.Svg => StiExportFormat.ImageSvg, 
			StiImageType.Svgz => StiExportFormat.ImageSvgz, 
			_ => StiExportFormat.ImageTiff, 
		};
	}

	public StiImageExportSettings()
		: this(StiImageType.Jpeg)
	{
	}

	public StiImageExportSettings(StiImageType imageType)
	{
		this.imageType = imageType;
	}
}

using System;
using Stimulsoft.Report.Dashboard.Export;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Dashboard.Export.Settings;

public class StiImageDashboardExportSettings : StiDashboardExportSettings, IStiSizeExportSettings, IStiImageDashboardExportSettings, IStiDashboardExportSettings
{
	private double width;

	private double height;

	public override StiDashboardExportFormat Format => StiDashboardExportFormat.Image;

	public virtual StiImageType ImageType { get; set; } = StiImageType.Png;


	public double Width
	{
		get
		{
			return width;
		}
		set
		{
			width = Math.Min(value, MaxWidth);
		}
	}

	public double Height
	{
		get
		{
			return height;
		}
		set
		{
			height = Math.Min(value, MaxHeight);
		}
	}

	public double MaxWidth { get; set; } = 10000.0;


	public double MaxHeight { get; set; } = 10000.0;


	public int Scale { get; set; } = 100;


	public StiImageDashboardExportSettings()
	{
		base.RenderSinglePage = true;
	}

	public StiImageDashboardExportSettings(StiImageType imageType)
		: this()
	{
		ImageType = imageType;
	}
}

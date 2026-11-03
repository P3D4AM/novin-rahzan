using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export;

public class StiCell : ICloneable
{
	private StiComponent component;

	private int width;

	private int height;

	public StiExportFormat ExportFormat { get; }

	public virtual StiComponent Component
	{
		get
		{
			return component;
		}
		set
		{
			component = value;
			ExportImage = value as IStiExportImage;
			if (ExportImage != null && ExportImage is IStiExportImageExtended && !((IStiExportImageExtended)ExportImage).IsExportAsImage(ExportFormat) && !ForceExportAsImage(value))
			{
				ExportImage = null;
			}
		}
	}

	public virtual IStiExportImage ExportImage { get; set; }

	public virtual StiCellStyle CellStyle { get; set; }

	public int Left { get; set; }

	public int Top { get; set; }

	public int Width
	{
		get
		{
			return width;
		}
		set
		{
			width = Math.Max(value, 0);
		}
	}

	public int Height
	{
		get
		{
			return height;
		}
		set
		{
			height = Math.Max(value, 0);
		}
	}

	public string Text { get; set; } = string.Empty;


	public virtual object Clone()
	{
		return MemberwiseClone() as StiCell;
	}

	internal bool ForceExportAsImage(object exportImage)
	{
		if (ExportFormat == StiExportFormat.Html)
		{
			return false;
		}
		if (exportImage is IStiTextOptions stiTextOptions)
		{
			return stiTextOptions.TextOptions.Angle != 0f;
		}
		return false;
	}

	public StiCell()
		: this(StiExportFormat.None)
	{
	}

	public StiCell(StiExportFormat exportFormat)
	{
		ExportFormat = exportFormat;
	}
}

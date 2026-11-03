using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export;

public class StiCell2 : StiCell
{
	private bool isExportImageNull;

	internal int cellStyleId = -1;

	internal StiMatrix Matrix { get; set; }

	public override StiComponent Component
	{
		get
		{
			return Matrix.GetCellComponent(this);
		}
		set
		{
			Matrix.SetCellComponent(this, value, -1);
		}
	}

	public override IStiExportImage ExportImage
	{
		get
		{
			if (isExportImageNull)
			{
				return null;
			}
			IStiExportImage stiExportImage = Component as IStiExportImage;
			if (stiExportImage != null && stiExportImage is IStiExportImageExtended && !((IStiExportImageExtended)stiExportImage).IsExportAsImage(Matrix.exportFormat) && !ForceExportAsImage(Component))
			{
				stiExportImage = null;
			}
			return stiExportImage;
		}
		set
		{
			if (value == null)
			{
				isExportImageNull = true;
				return;
			}
			throw new Exception("Error in StiMatrix cache mode");
		}
	}

	internal int PageId { get; set; } = -1;


	internal int ComponentId { get; set; } = -1;


	public override StiCellStyle CellStyle
	{
		get
		{
			return Matrix.Styles[cellStyleId] as StiCellStyle;
		}
		set
		{
			cellStyleId = Matrix.Styles.IndexOf(value);
		}
	}

	public override object Clone()
	{
		return MemberwiseClone() as StiCell2;
	}

	public StiCell2()
		: base(StiExportFormat.None)
	{
	}

	public StiCell2(StiExportFormat exportFormat)
		: base(exportFormat)
	{
	}

	public StiCell2(StiMatrix matrix)
		: base(StiExportFormat.None)
	{
		Matrix = matrix;
	}
}

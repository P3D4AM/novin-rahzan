using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Design;

[Serializable]
public class StiDesignerInfo
{
	[DefaultValue(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ForceDesigningMode { get; set; }

	[DefaultValue(StiQuickInfoType.None)]
	[StiSerializable]
	public StiQuickInfoType QuickInfoType { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	public bool GenerateLocalizedName { get; set; }

	[DefaultValue(true)]
	[StiSerializable]
	public bool ShowDimensionLines { get; set; } = true;


	[DefaultValue(true)]
	[StiSerializable]
	public bool QuickInfoOverlay { get; set; } = true;


	public bool IsComponentsMoving { get; set; }

	internal bool IsComponentCreating { get; set; }

	internal bool IsDragDropComponent { get; set; }

	internal bool IsDragDropFromToolBox { get; set; }

	public StiComponent DraggingComponent { get; set; }

	public StiComponent DraggingLabelComponent { get; set; }

	public StiAction CurrentAction { get; set; }

	[StiSerializable]
	[DefaultValue(StiMarkersStyle.Corners)]
	public StiMarkersStyle MarkersStyle { get; set; } = StiMarkersStyle.Corners;


	public bool IsTableMode { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	public bool DrawEventMarkers { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	public bool DrawMarkersWhenMoving { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	public bool RunDesignerAfterInsert { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	public bool UseLastFormat { get; set; } = true;


	[StiSerializable]
	[DefaultValue(15)]
	public int AutoSaveInterval { get; set; } = 15;


	[StiSerializable]
	[DefaultValue(false)]
	public bool EnableAutoSaveMode { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	public bool ShowOrder { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	public bool AlignToGrid { get; set; } = true;


	[StiSerializable]
	[DefaultValue(false)]
	public bool AutoSaveReportToReportClass { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	public bool ShowHeaders { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	public bool ShowGrid { get; set; } = true;


	public bool ShowInteractive { get; set; } = true;


	[StiSerializable]
	public double Zoom { get; set; } = 0.75;


	[Browsable(false)]
	[StiSerializable]
	public bool ShowRulers { get; set; } = true;


	[DefaultValue(StiViewMode.Normal)]
	[StiSerializable]
	public StiViewMode ViewMode { get; set; }

	[StiSerializable]
	[DefaultValue(20.0)]
	public double GridSizePoints { get; set; } = 20.0;


	[StiSerializable]
	[DefaultValue(10.0)]
	public double GridSizeScreenPoints { get; set; } = 10.0;


	[StiSerializable]
	[DefaultValue(8.0)]
	public double GridSizePixels { get; set; } = 8.0;


	[StiSerializable]
	[DefaultValue(0.2)]
	public double GridSizeCentimetres { get; set; } = 0.2;


	[StiSerializable]
	[DefaultValue(10.0)]
	public double GridSizeHundredthsOfInch { get; set; } = 10.0;


	[StiSerializable]
	[DefaultValue(0.1)]
	public double GridSizeInch { get; set; } = 0.1;


	[StiSerializable]
	[DefaultValue(2.0)]
	public double GridSizeMillimeters { get; set; } = 2.0;


	public double GridSize
	{
		get
		{
			if (Report.Unit is StiMillimetersUnit)
			{
				return Report.Info.GridSizeMillimeters;
			}
			if (Report.Unit is StiCentimetersUnit)
			{
				return Report.Info.GridSizeCentimetres;
			}
			if (Report.Unit is StiHundredthsOfInchUnit)
			{
				return Report.Info.GridSizeHundredthsOfInch;
			}
			return Report.Info.GridSizeInch;
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	public bool FillBands { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	public bool FillCrossBands { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	public bool FillContainer { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	public bool FillComponent { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	public bool UseComponentColor { get; set; } = true;


	[StiSerializable]
	[DefaultValue(StiGridMode.Lines)]
	public StiGridMode GridMode { get; set; }

	public StiReport Report { get; set; }

	public Color GetFillColor(Color color)
	{
		if (!UseComponentColor)
		{
			Color baseColor = StiColorUtils.Dark(Color.White, (byte)40);
			return Color.FromArgb(100, baseColor);
		}
		return Color.FromArgb(40, color);
	}

	public StiDesignerInfo(StiReport report)
	{
		Report = report;
	}

	public StiDesignerInfo()
		: this(null)
	{
	}
}

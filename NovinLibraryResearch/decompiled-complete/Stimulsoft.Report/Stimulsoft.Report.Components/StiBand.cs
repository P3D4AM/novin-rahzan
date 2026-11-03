using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components;

[StiToolbox(false)]
[StiDesigner("Stimulsoft.Report.Components.Design.StiBandDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfBandDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiBandGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiBandWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiBandV1Builder))]
[StiV2Builder(typeof(StiBandV2Builder))]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.catBands.png")]
[StiEngine(/*Could not decode attribute arguments.*/)]
public abstract class StiBand : StiContainer, IStiResetPageNumber
{
	private StiBandInfoV2 bandInfoV2;

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(420)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Allows to reset page number on this band.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ResetPageNumber { get; set; }

	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[Description("Gets or sets value indicates that this object can grow.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
			base.CanGrow = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiAnchorMode Anchor
	{
		get
		{
			return base.Anchor;
		}
		set
		{
			base.Anchor = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool GrowToHeight
	{
		get
		{
			return base.GrowToHeight;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiShiftMode ShiftMode
	{
		get
		{
			if (!StiOptions.Engine.UseCheckSizeForContinuedContainers)
			{
				return StiShiftMode.None;
			}
			return base.ShiftMode;
		}
		set
		{
		}
	}

	public override string LocalizedCategory => StiLocalization.Get("Report", "Bands");

	public override StiComponentType ComponentType => StiComponentType.Master;

	[Browsable(false)]
	public StiBandInfoV2 BandInfoV2 => bandInfoV2 ?? (bandInfoV2 = new StiBandInfoV2());

	[Browsable(false)]
	[StiNonSerialized]
	public override bool Printable
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiDockStyle DockStyle
	{
		get
		{
			if (!StiOptions.Engine.DockPageFooterToBottom || !(this is StiPageFooterBand))
			{
				return StiDockStyle.Top;
			}
			return StiDockStyle.Bottom;
		}
		set
		{
		}
	}

	public override bool IsAutomaticDock => true;

	[Browsable(false)]
	[StiNonSerialized]
	public override SizeD MinSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.MinSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.MinSize = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override SizeD MaxSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.MaxSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.MaxSize = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(500)]
	[Description("Gets or sets minimal height of band.")]
	[DefaultValue(0.0)]
	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double MinHeight
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			SizeD minSize = base.MinSize;
			return ((SizeD)(ref minSize)).Height;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			SizeD minSize = base.MinSize;
			if (((SizeD)(ref minSize)).Height != value)
			{
				base.MinSize = new SizeD(0.0, value);
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(500)]
	[Description("Gets or sets maximal height of band.")]
	[DefaultValue(0.0)]
	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double MaxHeight
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			SizeD maxSize = base.MaxSize;
			return ((SizeD)(ref maxSize)).Height;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			SizeD maxSize = base.MaxSize;
			if (((SizeD)(ref maxSize)).Height != value)
			{
				base.MaxSize = new SizeD(0.0, value);
			}
		}
	}

	[Browsable(false)]
	public override double Left
	{
		get
		{
			return base.Left;
		}
		set
		{
			base.Left = value;
		}
	}

	[Browsable(false)]
	public override double Top
	{
		get
		{
			return base.Top;
		}
		set
		{
			base.Top = value;
		}
	}

	[Browsable(false)]
	public override double Width
	{
		get
		{
			return base.Width;
		}
		set
		{
			base.Width = value;
		}
	}

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 300.0, 30.0);

	public override RectangleD DisplayRectangle
	{
		get
		{
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			if (Report != null && Report.IsRendering)
			{
				return new RectangleD(Left, Top, Width, Height);
			}
			double num = 0.0;
			double num2 = 0.0;
			if (Page != null && (Report == null || Report.Info.ShowHeaders))
			{
				num = Page.Unit.ConvertFromHInches(HeaderSize);
				num2 = Page.Unit.ConvertFromHInches(FooterSize);
			}
			return new RectangleD(Left, Top - num, Width, Height + num + num2);
		}
		set
		{
			if (Report != null && Report.IsRendering)
			{
				Left = ((RectangleD)(ref value)).Left;
				Top = ((RectangleD)(ref value)).Top;
				Width = ((RectangleD)(ref value)).Width;
				Height = ((RectangleD)(ref value)).Height;
				return;
			}
			double num = Page.Unit.ConvertFromHInches(HeaderSize);
			double num2 = Page.Unit.ConvertFromHInches(FooterSize);
			if (Report != null && !Report.Info.ShowHeaders)
			{
				num = 0.0;
				num2 = 0.0;
			}
			Left = ((RectangleD)(ref value)).Left;
			Top = ((RectangleD)(ref value)).Top + num;
			Width = ((RectangleD)(ref value)).Width;
			Height = ((RectangleD)(ref value)).Height - num - num2;
		}
	}

	public override RectangleD SelectRectangle
	{
		get
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			double num = Page.Unit.ConvertFromHInches(HeaderSize);
			if (DockStyle == StiDockStyle.Left || DockStyle == StiDockStyle.Right)
			{
				return new RectangleD(Left - num, Top, Width + num, Height);
			}
			return new RectangleD(Left, Top - num, Width, Height + num);
		}
		set
		{
			double num = Page.Unit.ConvertFromHInches(HeaderSize);
			if (DockStyle == StiDockStyle.Left || DockStyle == StiDockStyle.Right)
			{
				Left = ((RectangleD)(ref value)).Left + num;
				Top = ((RectangleD)(ref value)).Top;
				Width = ((RectangleD)(ref value)).Width - num;
				Height = ((RectangleD)(ref value)).Height;
			}
			else
			{
				Left = ((RectangleD)(ref value)).Left;
				Top = ((RectangleD)(ref value)).Top + num;
				Width = ((RectangleD)(ref value)).Width;
				Height = ((RectangleD)(ref value)).Height - num;
			}
		}
	}

	[StiBrowsable(false)]
	internal int NestedLevel
	{
		get
		{
			if (this is StiPageHeaderBand stiPageHeaderBand)
			{
				int num = 1;
				{
					foreach (StiComponent component in Page.Components)
					{
						if (component == stiPageHeaderBand)
						{
							return num;
						}
						if (component is StiPageHeaderBand)
						{
							num++;
						}
					}
					return num;
				}
			}
			if (this is StiPageFooterBand stiPageFooterBand)
			{
				int num2 = 1;
				{
					foreach (StiComponent component2 in Page.Components)
					{
						if (component2 == stiPageFooterBand)
						{
							return num2;
						}
						if (component2 is StiPageFooterBand)
						{
							num2++;
						}
					}
					return num2;
				}
			}
			if (this is StiReportTitleBand stiReportTitleBand)
			{
				int num3 = 1;
				{
					foreach (StiComponent component3 in Page.Components)
					{
						if (component3 == stiReportTitleBand)
						{
							return num3;
						}
						if (component3 is StiReportTitleBand)
						{
							num3++;
						}
					}
					return num3;
				}
			}
			if (this is StiReportSummaryBand stiReportSummaryBand)
			{
				int num4 = 1;
				for (int num5 = Page.Components.Count - 1; num5 >= 0; num5--)
				{
					StiComponent stiComponent4 = Page.Components[num5];
					if (stiComponent4 == stiReportSummaryBand)
					{
						return num4;
					}
					if (stiComponent4 is StiReportSummaryBand)
					{
						num4++;
					}
				}
				return num4;
			}
			if (this is StiEmptyBand stiEmptyBand)
			{
				int num6 = 1;
				{
					foreach (StiComponent component4 in Page.Components)
					{
						if (component4 == stiEmptyBand)
						{
							return num6;
						}
						if (component4 is StiEmptyBand)
						{
							num6++;
						}
					}
					return num6;
				}
			}
			if (this is StiOverlayBand stiOverlayBand)
			{
				int num7 = 1;
				{
					foreach (StiComponent component5 in Page.Components)
					{
						if (component5 == stiOverlayBand)
						{
							return num7;
						}
						if (component5 is StiOverlayBand)
						{
							num7++;
						}
					}
					return num7;
				}
			}
			if (this is StiDataBand stiDataBand)
			{
				StiDataBand stiDataBand2 = stiDataBand.MasterComponent as StiDataBand;
				int num8 = 1;
				while (stiDataBand2 != null)
				{
					stiDataBand2 = stiDataBand2.MasterComponent as StiDataBand;
					num8++;
				}
				return num8;
			}
			if (this is StiHeaderBand masterHeaderBand)
			{
				return StiHeaderBandV2Builder.GetMaster(masterHeaderBand)?.NestedLevel ?? 1;
			}
			if (this is StiFooterBand masterFooterBand)
			{
				return StiFooterBandV2Builder.GetMaster(masterFooterBand)?.NestedLevel ?? 1;
			}
			if (this is StiGroupHeaderBand masterGroupHeaderBand)
			{
				return StiGroupHeaderBandV2Builder.GetMaster(masterGroupHeaderBand)?.NestedLevel ?? 1;
			}
			if (this is StiGroupFooterBand masterFooterBand2)
			{
				return StiGroupFooterBandV2Builder.GetMaster(masterFooterBand2)?.NestedLevel ?? 1;
			}
			if (this is StiChildBand stiChildBand)
			{
				return stiChildBand.GetMaster()?.NestedLevel ?? 1;
			}
			return 0;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public RectangleD? RectangleMoveComponent { get; set; }

	public abstract Color HeaderStartColor { get; }

	public abstract Color HeaderEndColor { get; }

	[Browsable(false)]
	public virtual double HeaderSize
	{
		get
		{
			if (Report == null || !Report.Info.ShowHeaders)
			{
				return 0.0;
			}
			return StiAlignValue.AlignToMaxGrid(15.0, Page.Unit.ConvertToHInches(Page.GridSize), true);
		}
	}

	[Browsable(false)]
	public virtual double FooterSize
	{
		get
		{
			if (Report == null || !Report.Info.ShowHeaders)
			{
				return 0.0;
			}
			return StiAlignValue.AlignToMaxGrid(15.0, Page.Unit.ConvertToHInches(Page.GridSize), true);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "CanGrow", CanGrow, true, false);
		JsonExtensions.RemoveProperty(val, "GrowToHeight");
		JsonExtensions.RemoveProperty(val, "ShiftMode");
		JsonExtensions.RemoveProperty(val, "Printable");
		JsonExtensions.RemoveProperty(val, "DockStyle");
		JsonExtensions.RemoveProperty(val, "MinSize");
		JsonExtensions.RemoveProperty(val, "MaxSize");
		JsonExtensions.AddPropertyBool(val, "ResetPageNumber", ResetPageNumber, false, false);
		JsonExtensions.AddPropertyDouble(val, "MinHeight", MinHeight, 0.0);
		JsonExtensions.AddPropertyDouble(val, "MaxHeight", MaxHeight, 0.0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ResetPageNumber":
				ResetPageNumber = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MinHeight":
				MinHeight = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "MaxHeight":
				MaxHeight = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			}
		}
	}

	public override string GetQuickInfo()
	{
		StiQuickInfoType quickInfoType = Report.Info.QuickInfoType;
		if (quickInfoType == StiQuickInfoType.ShowComponentsNames)
		{
			return string.Empty;
		}
		return base.GetQuickInfo();
	}

	public override bool CanContainIn(StiComponent component)
	{
		if (component is IStiReportControl)
		{
			return false;
		}
		if (IsCross && component.IsCross)
		{
			return false;
		}
		if (IsCross && component is StiBand)
		{
			return true;
		}
		if (this is StiTable && component is StiBand)
		{
			return true;
		}
		if (component is StiBand)
		{
			return false;
		}
		return base.CanContainIn(component);
	}

	public override object Clone(bool cloneProperties, bool cloneComponents)
	{
		StiBand stiBand = (StiBand)base.Clone(cloneProperties, cloneComponents);
		stiBand.bandInfoV2 = BandInfoV2.Clone() as StiBandInfoV2;
		return stiBand;
	}

	protected internal override void SetDirectDisplayRectangle(RectangleD rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		DisplayRectangle = rect;
	}

	public virtual string GetHeaderText()
	{
		return ((object)this).ToString();
	}

	public StiBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiBand(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		CanGrow = true;
		DockStyle = StiDockStyle.Top;
		PlaceOnToolbox = false;
	}
}

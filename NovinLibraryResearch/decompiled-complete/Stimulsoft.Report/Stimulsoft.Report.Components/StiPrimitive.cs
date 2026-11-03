using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Components;

[StiToolbox(true)]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiShape.png")]
public abstract class StiPrimitive : StiComponent
{
	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool Shift
	{
		get
		{
			return base.Shift;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool UseParentStyles
	{
		get
		{
			return base.UseParentStyles;
		}
		set
		{
			base.UseParentStyles = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiDockStyle DockStyle
	{
		get
		{
			return base.DockStyle;
		}
		set
		{
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

	public override string LocalizedCategory => StiLocalization.Get("Report", "Shapes");

	public override StiComponentType ComponentType => StiComponentType.Simple;

	[Browsable(false)]
	public override int Priority => 1500;

	[Browsable(false)]
	[StiSerializable]
	public override RectangleD ClientRectangle
	{
		get
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			return new RectangleD(Left, Top, Width, Height);
		}
		set
		{
			Left = Math.Round(((RectangleD)(ref value)).Left, 2);
			Top = Math.Round(((RectangleD)(ref value)).Top, 2);
			Width = Math.Round(((RectangleD)(ref value)).Width, 2);
			Height = Math.Round(((RectangleD)(ref value)).Height, 2);
		}
	}

	[Browsable(false)]
	public override RectangleD DisplayRectangle
	{
		get
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			return new RectangleD(Left, Top, Width, Height);
		}
		set
		{
			Left = Math.Round(((RectangleD)(ref value)).Left, 2);
			Top = Math.Round(((RectangleD)(ref value)).Top, 2);
			Width = Math.Round(((RectangleD)(ref value)).Width, 2);
			Height = Math.Round(((RectangleD)(ref value)).Height, 2);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.RemoveProperty(val, "Shift");
		JsonExtensions.RemoveProperty(val, "UseParentStyles");
		JsonExtensions.RemoveProperty(val, "DockStyle");
		JsonExtensions.RemoveProperty(val, "GrowToHeight");
		return val;
	}

	protected internal override void SetDirectDisplayRectangle(RectangleD rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		DisplayRectangle = rect;
	}

	public StiPrimitive()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiPrimitive(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}

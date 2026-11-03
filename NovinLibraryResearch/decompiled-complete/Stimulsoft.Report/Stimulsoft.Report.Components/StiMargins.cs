using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components.Design;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiMarginsTypeConverter))]
public class StiMargins : ICloneable
{
	public static StiMargins Empty = new StiMargins(0.0, 0.0, 0.0, 0.0);

	[Browsable(false)]
	public virtual bool IsDefault => IsEmpty;

	[Description("Gets or sets left margin size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(1)]
	public double Left { get; set; }

	[Description("Gets or sets right margin size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(2)]
	public double Right { get; set; }

	[Description("Gets or sets top margin size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(3)]
	public double Top { get; set; }

	[Description("Gets or sets bottom margin size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(4)]
	public double Bottom { get; set; }

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (Left == 0.0 && Right == 0.0 && Top == 0.0)
			{
				return Bottom == 0.0;
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(double defLeft, double defRight, double defTop, double defBotttom)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		if (Left == defLeft && Right == defRight && Top == defTop && Bottom == defBotttom)
		{
			return null;
		}
		JObject val = new JObject();
		JsonExtensions.AddPropertyDouble(val, "Left", Left);
		JsonExtensions.AddPropertyDouble(val, "Right", Right);
		JsonExtensions.AddPropertyDouble(val, "Top", Top);
		JsonExtensions.AddPropertyDouble(val, "Bottom", Bottom);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Left":
				Left = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "Right":
				Right = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "Top":
				Top = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "Bottom":
				Bottom = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override bool Equals(object obj)
	{
		StiMargins stiMargins = (StiMargins)obj;
		if (stiMargins != null && stiMargins.Left == Left && stiMargins.Right == Right && stiMargins.Top == Top)
		{
			return stiMargins.Bottom == Bottom;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public virtual RectangleD Apply(RectangleD rect)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + Left;
		((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y + Top;
		((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - (Left + Right);
		((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - (Top + Bottom);
		return rect;
	}

	public StiMargins()
		: this(0.0, 0.0, 0.0, 0.0)
	{
	}

	public StiMargins(double all)
	{
		Left = (Top = (Right = (Bottom = all)));
	}

	public StiMargins(double left, double right, double top, double bottom)
	{
		Left = left;
		Top = top;
		Right = right;
		Bottom = bottom;
	}
}

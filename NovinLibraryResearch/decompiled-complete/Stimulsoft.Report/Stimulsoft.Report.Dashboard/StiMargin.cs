using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard.Design;

namespace Stimulsoft.Report.Dashboard;

[TypeConverter(typeof(StiMarginTypeConverter))]
public class StiMargin : ICloneable
{
	public static StiMargin Empty = new StiMargin();

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (Left == 3.0 && Right == 3.0 && Top == 3.0)
			{
				return Bottom == 3.0;
			}
			return false;
		}
	}

	[Description("Gets or sets left margin size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue(3.0)]
	[StiOrder(1)]
	public double Left { get; set; }

	[Description("Gets or sets top margin size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue(3.0)]
	[StiOrder(2)]
	public double Top { get; set; }

	[Description("Gets or sets right margin size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue(3.0)]
	[StiOrder(3)]
	public double Right { get; set; }

	[Description("Gets or sets bottom margin size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue(3.0)]
	[StiOrder(4)]
	public double Bottom { get; set; }

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (Left == 0.0 && Top == 0.0 && Right == 0.0)
			{
				return Bottom == 0.0;
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode, double defLeft, double defTop, double defRight, double defBotttom)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (Left == defLeft && Top == defTop && Right == defRight && Bottom == defBotttom)
		{
			return null;
		}
		JObject val = new JObject();
		JsonExtensions.AddPropertyDouble(val, "Left", Left);
		JsonExtensions.AddPropertyDouble(val, "Top", Top);
		JsonExtensions.AddPropertyDouble(val, "Right", Right);
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
			case "Top":
				Top = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "Right":
				Right = StiJsonReportObjectHelper.DeserializeDouble(item);
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
		StiMargin stiMargin = (StiMargin)obj;
		if (stiMargin != null && stiMargin.Left == Left && stiMargin.Top == Top && stiMargin.Right == Right)
		{
			return stiMargin.Bottom == Bottom;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public StiMargin()
		: this(3.0)
	{
	}

	public StiMargin(double all)
	{
		Left = (Right = (Top = (Bottom = all)));
	}

	public StiMargin(double left, double top, double right, double bottom)
	{
		Left = left;
		Right = right;
		Top = top;
		Bottom = bottom;
	}
}

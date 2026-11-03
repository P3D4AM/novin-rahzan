using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard.Design;

namespace Stimulsoft.Report.Dashboard;

[TypeConverter(typeof(StiPaddingTypeConverter))]
public class StiPadding : ICloneable, IStiDefault
{
	public static StiPadding Empty = new StiPadding();

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (Left == 5.0 && Right == 5.0 && Top == 5.0)
			{
				return Bottom == 5.0;
			}
			return false;
		}
	}

	[Description("Gets or sets left padding size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue(5.0)]
	[StiOrder(1)]
	public double Left { get; set; }

	[Description("Gets or sets top padding size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue(5.0)]
	[StiOrder(2)]
	public double Top { get; set; }

	[Description("Gets or sets right padding size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue(5.0)]
	[StiOrder(3)]
	public double Right { get; set; }

	[Description("Gets or sets bottom padding size on the object.")]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue(5.0)]
	[StiOrder(4)]
	public double Bottom { get; set; }

	[Browsable(false)]
	public bool IsEmpty => IsAllEqual(0.0);

	[Browsable(false)]
	public bool IsAllSame => IsAllEqual(Left);

	public JObject SaveToJsonObject(StiJsonSaveMode mode, double defLeft, double defTop, double defRight, double defBotttom)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (Left == defLeft && Top == defTop && Right == defRight && Bottom == defBotttom)
		{
			return null;
		}
		JObject val = new JObject();
		JsonExtensions.AddPropertyDouble(val, "Left", Left, defLeft);
		JsonExtensions.AddPropertyDouble(val, "Top", Top, defTop);
		JsonExtensions.AddPropertyDouble(val, "Right", Right, defRight);
		JsonExtensions.AddPropertyDouble(val, "Bottom", Bottom, defBotttom);
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
		StiPadding stiPadding = (StiPadding)obj;
		if (stiPadding != null && stiPadding.Left == Left && stiPadding.Top == Top && stiPadding.Right == Right)
		{
			return stiPadding.Bottom == Bottom;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public bool IsAllEqual(double value)
	{
		if (Left == value && Right == value && Top == value)
		{
			return Bottom == value;
		}
		return false;
	}

	public StiPadding()
		: this(5.0)
	{
	}

	public StiPadding(double all)
	{
		Left = (Top = (Right = (Bottom = all)));
	}

	public StiPadding(double left, double top, double right, double bottom)
	{
		Left = left;
		Top = top;
		Right = right;
		Bottom = bottom;
	}
}

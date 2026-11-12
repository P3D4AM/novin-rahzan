using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

[TypeConverter(typeof(StiCapConverter))]
[RefreshProperties(RefreshProperties.All)]
[StiReferenceIgnore]
[JsonObject]
public class StiCap : ICloneable
{
	public enum CapOrder
	{
		Color = 100,
		Fill = 110,
		Width = 120,
		Height = 130,
		Style = 140
	}

	[Browsable(true)]
	[StiSerializable]
	[DefaultValue(10)]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(120)]
	[Description("Gets or sets width of the cap.")]
	public int Width { get; set; }

	[Browsable(true)]
	[StiSerializable]
	[DefaultValue(StiCapStyle.None)]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(140)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a cap style.")]
	[Editor("Stimulsoft.Base.Drawing.Design.StiCapStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiCapStyle Style { get; set; }

	[Browsable(true)]
	[StiSerializable]
	[DefaultValue(10)]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(130)]
	[Description("Gets or sets height of the cap.")]
	public int Height { get; set; }

	[Browsable(true)]
	[StiSerializable]
	[DefaultValue(true)]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(110)]
	[Description("Gets or sets fill mode of the cap.")]
	public bool Fill { get; set; }

	[Browsable(true)]
	[StiSerializable]
	[StiOrder(100)]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(StiColorConverter))]
	[Description("Gets or sets cap color.")]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color Color { get; set; }

	public virtual object Clone()
	{
		StiCap stiCap = (StiCap)MemberwiseClone();
		stiCap.Width = Width;
		stiCap.Height = Height;
		stiCap.Style = Style;
		stiCap.Fill = Fill;
		stiCap.Color = Color;
		return stiCap;
	}

	public StiCap()
		: this(10, StiCapStyle.None, 10, fill: true, Color.Black)
	{
	}

	public StiCap(int width, StiCapStyle style, int height, bool fill, Color color)
	{
		Width = width;
		Style = style;
		Height = height;
		Fill = fill;
		Color = color;
	}
}

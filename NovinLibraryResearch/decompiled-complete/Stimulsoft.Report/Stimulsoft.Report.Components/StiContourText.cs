using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiText), "Stimulsoft.Report.Images.Components.StiContourText.png")]
[StiToolbox(false)]
[StiDesigner("Stimulsoft.Report.Components.Design.StiTextDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfTextDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiContourTextGdiPainter))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiText))]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiGrowToHeight))]
[StiContextTool(typeof(IStiComponentDesigner))]
public class StiContourText : StiText
{
	public override StiComponentId ComponentId => StiComponentId.StiContourText;

	[Browsable(false)]
	[StiNonSerialized]
	public override StiPenStyle LinesOfUnderline
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.LinesOfUnderline;
		}
		set
		{
		}
	}

	public override int ToolboxPosition => 104;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedName => StiLocalization.Get("Components", "StiContourText");

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiExpressionColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a contour color.")]
	[StiExpressionAllowed]
	public Color ContourColor { get; set; } = Color.Black;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[DefaultValue(1.0)]
	[Description("Gets or sets a contour size.")]
	public double Size { get; set; } = 1.0;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "LinesOfUnderline");
		JsonExtensions.AddPropertyColor(val, "ContourColor", ContourColor, Color.Black);
		JsonExtensions.AddPropertyDouble(val, "Size", Size, 1.0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "ContourColor"))
			{
				if (text == "Size")
				{
					Size = StiJsonReportObjectHelper.DeserializeDouble(item);
				}
			}
			else
			{
				ContourColor = StiJsonReportObjectHelper.DeserializeColor(item);
			}
		}
	}

	private bool ShouldSerializeContourColor()
	{
		return ContourColor != Color.Black;
	}

	public StiContourText()
		: this(RectangleD.Empty, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiContourText(RectangleD rect)
		: this(rect, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiContourText(RectangleD rect, string text)
		: base(rect, text)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		TextBrush = (StiBrush)new StiSolidBrush(Color.White);
		PlaceOnToolbox = false;
	}
}

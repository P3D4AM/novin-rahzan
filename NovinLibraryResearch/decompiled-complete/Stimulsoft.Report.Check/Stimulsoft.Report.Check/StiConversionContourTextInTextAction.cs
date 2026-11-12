using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiConversionContourTextInTextAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Convert");

	public override string Description => StiLocalizationExt.Get("CheckActions", "Convert");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (!(element is StiContourText stiContourText))
		{
			return;
		}
		StiText stiText = new StiText();
		stiText.Text = stiContourText.Text;
		stiText.TextBrush = stiContourText.TextBrush;
		stiText.TextFormat = stiContourText.TextFormat;
		stiText.TextOptions = stiContourText.TextOptions;
		stiText.TextQuality = stiContourText.TextQuality;
		stiText.WordWrap = stiContourText.WordWrap;
		stiText.RenderTo = stiContourText.RenderTo;
		stiText.ProcessAt = stiContourText.ProcessAt;
		stiText.ProcessAtEnd = stiContourText.ProcessAtEnd;
		stiText.ProcessingDuplicates = stiContourText.ProcessingDuplicates;
		stiText.NullValue = stiContourText.NullValue;
		stiText.LinesOfUnderline = stiContourText.LinesOfUnderline;
		stiText.LinesOfUnderlining = stiContourText.LinesOfUnderlining;
		stiText.Format = stiContourText.Format;
		stiText.Font = stiContourText.Font;
		stiText.ExcelValue = stiContourText.ExcelValue;
		stiText.Angle = stiContourText.Angle;
		stiText.AllowHtmlTags = stiContourText.AllowHtmlTags;
		stiText.HorAlignment = stiContourText.HorAlignment;
		stiText.VertAlignment = stiContourText.VertAlignment;
		stiText.Parent = stiContourText.Parent;
		stiText.Page = stiContourText.Page;
		stiText.SetPaintRectangle(stiContourText.GetPaintRectangle());
		stiText.ClientRectangle = stiContourText.ClientRectangle;
		stiText.MinSize = stiContourText.MinSize;
		stiText.MaxSize = stiContourText.MaxSize;
		stiText.Border = stiContourText.Border;
		stiText.Brush = stiContourText.Brush;
		stiText.Conditions = stiContourText.Conditions;
		stiText.ComponentStyle = stiContourText.ComponentStyle;
		stiText.UseParentStyles = stiContourText.UseParentStyles;
		stiText.CanGrow = stiContourText.CanGrow;
		stiText.CanShrink = stiContourText.CanShrink;
		stiText.GrowToHeight = stiContourText.GrowToHeight;
		stiText.DockStyle = stiContourText.DockStyle;
		stiText.Enabled = stiContourText.Enabled;
		stiText.Interaction = stiContourText.Interaction;
		stiText.Printable = stiContourText.Printable;
		stiText.PrintOn = stiContourText.PrintOn;
		stiText.ShiftMode = stiContourText.ShiftMode;
		stiText.Name = stiContourText.Name;
		stiText.Alias = stiContourText.Alias;
		stiText.Restrictions = stiContourText.Restrictions;
		stiText.Locked = stiContourText.Locked;
		stiText.Linked = stiContourText.Linked;
		StiContainer parent = stiContourText.Parent;
		if (parent != null)
		{
			int num = parent.Components.IndexOf(stiContourText);
			if (num != -1)
			{
				parent.Components.RemoveAt(num);
				parent.Components.Insert(num, stiText);
			}
		}
	}
}

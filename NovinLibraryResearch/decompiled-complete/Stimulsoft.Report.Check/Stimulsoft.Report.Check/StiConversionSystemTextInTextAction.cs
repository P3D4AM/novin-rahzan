using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiConversionSystemTextInTextAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Convert");

	public override string Description => StiLocalizationExt.Get("CheckActions", "Convert");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (!(element is StiSystemText stiSystemText))
		{
			return;
		}
		StiText stiText = new StiText();
		stiText.Text = stiSystemText.Text;
		stiText.TextBrush = stiSystemText.TextBrush;
		stiText.TextFormat = stiSystemText.TextFormat;
		stiText.TextOptions = stiSystemText.TextOptions;
		stiText.TextQuality = stiSystemText.TextQuality;
		stiText.WordWrap = stiSystemText.WordWrap;
		stiText.RenderTo = stiSystemText.RenderTo;
		stiText.ProcessAt = stiSystemText.ProcessAt;
		stiText.ProcessAtEnd = stiSystemText.ProcessAtEnd;
		stiText.ProcessingDuplicates = stiSystemText.ProcessingDuplicates;
		stiText.NullValue = stiSystemText.NullValue;
		stiText.LinesOfUnderline = stiSystemText.LinesOfUnderline;
		stiText.LinesOfUnderlining = stiSystemText.LinesOfUnderlining;
		stiText.Format = stiSystemText.Format;
		stiText.Font = stiSystemText.Font;
		stiText.ExcelValue = stiSystemText.ExcelValue;
		stiText.Angle = stiSystemText.Angle;
		stiText.AllowHtmlTags = stiSystemText.AllowHtmlTags;
		stiText.HorAlignment = stiSystemText.HorAlignment;
		stiText.VertAlignment = stiSystemText.VertAlignment;
		stiText.Parent = stiSystemText.Parent;
		stiText.Page = stiSystemText.Page;
		stiText.SetPaintRectangle(stiSystemText.GetPaintRectangle());
		stiText.ClientRectangle = stiSystemText.ClientRectangle;
		stiText.MinSize = stiSystemText.MinSize;
		stiText.MaxSize = stiSystemText.MaxSize;
		stiText.Border = stiSystemText.Border;
		stiText.Brush = stiSystemText.Brush;
		stiText.Conditions = stiSystemText.Conditions;
		stiText.ComponentStyle = stiSystemText.ComponentStyle;
		stiText.UseParentStyles = stiSystemText.UseParentStyles;
		stiText.CanGrow = stiSystemText.CanGrow;
		stiText.CanShrink = stiSystemText.CanShrink;
		stiText.GrowToHeight = stiSystemText.GrowToHeight;
		stiText.DockStyle = stiSystemText.DockStyle;
		stiText.Enabled = stiSystemText.Enabled;
		stiText.Interaction = stiSystemText.Interaction;
		stiText.Printable = stiSystemText.Printable;
		stiText.PrintOn = stiSystemText.PrintOn;
		stiText.ShiftMode = stiSystemText.ShiftMode;
		stiText.Name = stiSystemText.Name;
		stiText.Alias = stiSystemText.Alias;
		stiText.Restrictions = stiSystemText.Restrictions;
		stiText.Locked = stiSystemText.Locked;
		stiText.Linked = stiSystemText.Linked;
		StiContainer parent = stiSystemText.Parent;
		if (parent != null)
		{
			int num = parent.Components.IndexOf(stiSystemText);
			if (num != -1)
			{
				parent.Components.RemoveAt(num);
				parent.Components.Insert(num, stiText);
			}
		}
	}
}

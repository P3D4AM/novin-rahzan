using System;
using System.Collections;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;

namespace Stimulsoft.Report.Engine;

public class StiComponentV2Builder : StiV2Builder
{
	public override void SetReportVariables(StiComponent masterComp)
	{
	}

	public override void Prepare(StiComponent masterComp)
	{
		StiComponentHelper.FillComponentPlacement(masterComp);
		if (StiOptions.Engine.AllowResetValuesAtComponent)
		{
			masterComp.TagValue = null;
			masterComp.ToolTipValue = null;
			masterComp.PointerValue = null;
			masterComp.BookmarkValue = null;
			masterComp.HyperlinkValue = null;
		}
	}

	public override void UnPrepare(StiComponent masterComp)
	{
	}

	public override StiComponent InternalRender(StiComponent masterComp)
	{
		return masterComp.Clone(cloneProperties: false) as StiComponent;
	}

	public override StiComponent Render(StiComponent masterComp)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		StiComponent stiComponent = null;
		IStiTextBrush stiTextBrush = masterComp as IStiTextBrush;
		IStiBrush stiBrush = masterComp as IStiBrush;
		IStiFont stiFont = masterComp as IStiFont;
		IStiBorder stiBorder = masterComp as IStiBorder;
		IStiTextHorAlignment stiTextHorAlignment = masterComp as IStiTextHorAlignment;
		StiBrush textBrush = null;
		StiBrush brush = null;
		Font font = null;
		StiBorderSides side = (StiBorderSides)0;
		StiTextHorAlignment horAlignment = (StiTextHorAlignment)0;
		if (stiTextBrush != null)
		{
			textBrush = stiTextBrush.TextBrush;
		}
		if (stiBrush != null)
		{
			brush = stiBrush.Brush;
		}
		if (stiFont != null)
		{
			font = stiFont.Font;
		}
		if (stiBorder != null && stiBorder.Border != null)
		{
			side = stiBorder.Border.Side;
		}
		bool enabled = masterComp.Enabled;
		if (stiTextHorAlignment != null)
		{
			horAlignment = stiTextHorAlignment.HorAlignment;
		}
		StiReport report = masterComp.Report;
		bool flag = false;
		StiBaseStyle stiBaseStyle = null;
		if (masterComp.UseParentStyles && masterComp.Parent != null)
		{
			if (report?.Engine?.HashParentStyles != null && report.Engine.HashParentStyles.Count > 0)
			{
				stiBaseStyle = report.Engine.HashParentStyles[masterComp.Parent] as StiBaseStyle;
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiBaseStyle.GetStyle(masterComp.Parent);
			}
			if (stiBaseStyle != null)
			{
				stiBaseStyle.SetStyleToComponent(masterComp);
				flag = true;
				masterComp.GetComponentStyle()?.SetStyleToComponent(masterComp);
			}
		}
		masterComp.InvokeBeforePrint(masterComp, EventArgs.Empty);
		if (report?.Engine != null && report.Engine.HashUseParentStyles.ContainsKey(masterComp))
		{
			StiBaseStyle style = StiBaseStyle.GetStyle(masterComp, stiBaseStyle);
			if (report.Engine.HashParentStyles == null)
			{
				report.Engine.HashParentStyles = new Hashtable();
			}
			report.Engine.HashParentStyles[masterComp] = style;
			flag = true;
		}
		if (masterComp.IsEnabled)
		{
			if (!(masterComp is StiEmptyBand))
			{
				bool flag2 = masterComp.DoBookmark();
				masterComp.DoPointer(!flag2);
			}
			stiComponent = masterComp.InternalRender();
			if (stiComponent != null)
			{
				if (stiComponent.Page == null)
				{
					stiComponent.Page = masterComp.Page;
					stiComponent.InvokeEvents();
					stiComponent.Page = null;
				}
				else
				{
					stiComponent.InvokeEvents();
				}
			}
		}
		else if (masterComp is StiCrossField)
		{
			(masterComp as StiCrossField).DisabledByCondition = true;
		}
		masterComp.InvokeAfterPrint(masterComp, EventArgs.Empty);
		if (flag && report.Engine.HashParentStyles != null)
		{
			report.Engine.HashParentStyles.Remove(masterComp);
		}
		if (stiTextBrush != null)
		{
			stiTextBrush.TextBrush = textBrush;
		}
		if (stiBrush != null)
		{
			stiBrush.Brush = brush;
		}
		if (stiFont != null)
		{
			stiFont.Font = font;
		}
		if (stiBorder != null && stiBorder.Border != null)
		{
			stiBorder.Border.Side = side;
		}
		if (!(masterComp is StiSubReport))
		{
			masterComp.Enabled = enabled;
		}
		if (stiTextHorAlignment != null)
		{
			stiTextHorAlignment.HorAlignment = horAlignment;
		}
		if (masterComp is IStiBorder stiBorder2 && stiComponent is IStiBorder stiBorder3 && stiBorder2.Border != stiBorder3.Border && ((object)stiBorder2.Border).Equals((object?)stiBorder3.Border))
		{
			stiBorder3.Border = stiBorder2.Border;
		}
		if (masterComp is IStiBrush stiBrush2 && stiComponent is IStiBrush stiBrush3 && stiBrush2.Brush != stiBrush3.Brush && ((object)stiBrush2.Brush).Equals((object?)stiBrush3.Brush))
		{
			stiBrush3.Brush = stiBrush2.Brush;
		}
		if (masterComp is IStiTextBrush stiTextBrush2 && stiComponent is IStiTextBrush stiTextBrush3 && stiTextBrush2.TextBrush != stiTextBrush3.TextBrush && ((object)stiTextBrush2.TextBrush).Equals((object?)stiTextBrush3.TextBrush))
		{
			stiTextBrush3.TextBrush = stiTextBrush2.TextBrush;
		}
		masterComp.IsRendered = true;
		return stiComponent;
	}
}

using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiComponentV1Builder : StiV1Builder
{
	public override void Prepare(StiComponent masterComp)
	{
		masterComp.IsRendered = false;
		masterComp.RenderedCount = 0;
		StiComponentHelper.FillComponentPlacement(masterComp);
	}

	public override void UnPrepare(StiComponent masterComp)
	{
	}

	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		renderedComponent = masterComp.Clone(cloneProperties: false) as StiComponent;
		outContainer.Components.Add(renderedComponent);
		renderedComponent.InvokeEvents();
		return true;
	}

	public override bool Render(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		IStiTextBrush stiTextBrush = masterComp as IStiTextBrush;
		IStiBrush stiBrush = masterComp as IStiBrush;
		IStiFont stiFont = masterComp as IStiFont;
		StiBrush textBrush = null;
		StiBrush brush = null;
		Font font = null;
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
		bool enabled = masterComp.Enabled;
		masterComp.InvokeBeforePrint(masterComp, EventArgs.Empty);
		bool result = true;
		if (masterComp.IsEnabled)
		{
			masterComp.RenderedCount++;
			bool flag = masterComp.DoBookmark();
			masterComp.DoPointer(!flag);
			result = masterComp.InternalRender(ref renderedComponent, outContainer);
		}
		masterComp.InvokeAfterPrint(masterComp, EventArgs.Empty);
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
		masterComp.Enabled = enabled;
		return result;
	}

	public override bool Render(StiComponent masterComp, StiContainer outContainer)
	{
		StiComponent renderedComponent = null;
		return masterComp.Render(ref renderedComponent, outContainer);
	}
}

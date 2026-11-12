using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Button;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiButtonElementGdiPainter : StiControlElementGdiPainter
{
	public override void PaintContent(Graphics g, RectangleD rect, IStiElement element)
	{
		StiErrorStack.SetOk(element.GetKey());
		GraphicsState val = g.Save();
		try
		{
			g.SetClip(rect.ToRectangleF());
			rect = StiElementGdiPainter.ApplyCornerRadiusAndSetClip(g, rect, element);
			PaintButtonContent(g, rect, element);
		}
		catch (Exception ex)
		{
			StiErrorStack.SetError(element.GetKey(), ex.Message);
			PaintError(g, rect.ToRectangleF(), element);
		}
		finally
		{
			g.Restore(val);
		}
	}

	public static void PaintButton(Graphics g, RectangleD rect, IStiElement element, StiButtonVisualState state, bool isChecked, float scale = 1f)
	{
		StiButtonElementGdiPainter stiButtonElementGdiPainter = new StiButtonElementGdiPainter();
		StiButtonElement stiButtonElement = element as StiButtonElement;
		bool isEnabled = stiButtonElement.IsEnabled;
		StiRoundedBackgroundGdiPainter.PaintBackground(g, rect.ToRectangleF(), state.Border, stiButtonElement.CornerRadius, state.Brush, stiButtonElement.ShapeType, stiButtonElement.IsEnabled, scale);
		StiRoundedBorderGdiPainter.PaintBorder(g, rect.ToRectangleF(), state.Border, stiButtonElement.CornerRadius, stiButtonElement.ShapeType, scale, stiButtonElement.IsDesigning);
		RectangleD borderContentRect = StiBorderElementHelper.GetBorderContentRect(rect, element, skipMinimalSize: true, scale);
		rect = StiPaddingHelper.ApplyPadding(element, borderContentRect, scale);
		stiButtonElementGdiPainter.PaintButtonContentInternal(g, rect, state, stiButtonElement, isEnabled, isChecked, scale);
	}

	private StiButtonVisualState GetVisualState(IStiElement element)
	{
		StiButtonElement stiButtonElement = element as StiButtonElement;
		if (StiInplaceEditorTitleElement.IsComponentFocused(stiButtonElement))
		{
			return StiButtonVisualStatesHelper.GetHovered(stiButtonElement);
		}
		if (stiButtonElement.IsChecked)
		{
			return StiButtonVisualStatesHelper.GetChecked(stiButtonElement);
		}
		return StiButtonVisualStatesHelper.GetDefault(stiButtonElement);
	}

	public override void PaintBackground(Graphics g, RectangleD rect, IStiElement element)
	{
		StiButtonElement stiButtonElement = element as StiButtonElement;
		StiButtonVisualState visualState = GetVisualState(element);
		float scale = (float)StiElementScale.Factor(element);
		StiRoundedBackgroundGdiPainter.PaintBackground(g, rect.ToRectangleF(), visualState.Border, stiButtonElement.CornerRadius, visualState.Brush, stiButtonElement.ShapeType, stiButtonElement.IsEnabled, scale);
	}

	public override void PaintNotEnabled(Graphics g, RectangleF rect, IStiElement element, double? zoom = null)
	{
		StiButtonElement element2 = element as StiButtonElement;
		PaintButtonContent(g, RectangleD.CreateFromRectangle(rect), element2);
	}

	private void PaintButtonContent(Graphics g, RectangleD rect, IStiElement element)
	{
		StiButtonElement stiButtonElement = element as StiButtonElement;
		StiButtonVisualState visualState = GetVisualState(element);
		float scale = (float)StiElementScale.Factor(element);
		PaintButtonContentInternal(g, rect, visualState, stiButtonElement, stiButtonElement.IsEnabled, stiButtonElement.IsChecked, scale);
	}

	private void PaintButtonContentInternal(Graphics g, RectangleD rect, StiButtonVisualState state, StiButtonElement button, bool isEnabled, bool isChecked, float scale = 1f)
	{
		RectangleF textRect = rect.ToRectangleF();
		RectangleF iconRect = rect.ToRectangleF();
		StiFontIcons? stiFontIcons = state.IconSet.Icon;
		if (button.Type != 0)
		{
			stiFontIcons = (isChecked ? state.IconSet.CheckedIcon : state.IconSet.UncheckedIcon);
		}
		if (button.IconAlignment != 0 && stiFontIcons.HasValue)
		{
			CalculateRectangles(rect.ToRectangleF(), button.IconAlignment, ref textRect, ref iconRect, StiScale.I(32f * scale));
			if (iconRect != RectangleF.Empty)
			{
				StiButtonDrawing.PaintIcon(g, iconRect, state, stiFontIcons.Value, isEnabled, scale);
			}
		}
		if (button.IconAlignment != StiIconAlignment.Center)
		{
			if (button.ShapeType == StiButtonShapeType.Circle)
			{
				textRect = StiRoundedBorderGdiPainter.FitRectangleInCircle(textRect);
			}
			StiButtonDrawing.PaintText(g, textRect, state, button, scale);
		}
	}

	private static void CalculateRectangles(RectangleF rect, StiIconAlignment iconAlignment, ref RectangleF textRect, ref RectangleF iconRect, int size)
	{
		SizeF sizeF = new SizeF(size, size);
		if (iconAlignment != StiIconAlignment.Center && ((double)sizeF.Width * 0.5 > (double)rect.Width || (double)sizeF.Height * 0.5 > (double)rect.Height))
		{
			iconRect = RectangleF.Empty;
			return;
		}
		switch (iconAlignment)
		{
		case StiIconAlignment.Left:
			iconRect = new RectangleF(rect.X, rect.Y, sizeF.Width, rect.Height);
			textRect = new RectangleF(iconRect.Right, rect.Y, rect.Width - iconRect.Width, rect.Height);
			break;
		case StiIconAlignment.Right:
			iconRect = new RectangleF(rect.Right - sizeF.Width, rect.Y, sizeF.Width, rect.Height);
			textRect = new RectangleF(rect.X, rect.Y, rect.Width - iconRect.Width, rect.Height);
			break;
		case StiIconAlignment.Top:
			iconRect = new RectangleF(rect.X, rect.Y, rect.Width, sizeF.Height);
			textRect = new RectangleF(rect.X, iconRect.Bottom, rect.Width, rect.Height - sizeF.Height);
			break;
		case StiIconAlignment.Bottom:
			iconRect = new RectangleF(rect.X, rect.Bottom - sizeF.Height, rect.Width, sizeF.Height);
			textRect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height - sizeF.Height);
			break;
		case StiIconAlignment.Center:
			iconRect = rect;
			break;
		}
	}
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Button;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiElementGdiPainter : StiComponentGdiPainter
{
	public virtual Color DefaultBackgroundColor => Color.FromArgb(153, Color.White);

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		IStiElement stiElement = component as IStiElement;
		Graphics graphics = e.Graphics;
		if (component.ClientRectangle.IsEmpty)
		{
			return;
		}
		RectangleD paintRectangle = ((StiComponent)stiElement).GetPaintRectangle();
		if (!(paintRectangle.Width <= 0.0) && !(paintRectangle.Height <= 0.0) && (e.ClipRectangle.IsEmpty || paintRectangle.IntersectsWith(e.ClipRectangle)))
		{
			RectangleD rectangleD = StiMarginHelper.ApplyMargin(stiElement, paintRectangle, isScale: true);
			RectangleD rect = rectangleD;
			PaintShadow(graphics, rectangleD, stiElement);
			PaintBackground(graphics, rectangleD, stiElement);
			RectangleD borderContentRect = StiBorderElementHelper.GetBorderContentRect(rectangleD, stiElement);
			rectangleD = PaintTitle(graphics, borderContentRect, stiElement);
			rectangleD = StiPaddingHelper.ApplyPadding(stiElement, rectangleD);
			StiProgressStatus stiProgressStatus = StiComponentProgressHelper.Contains(component);
			if (StiComponentProgressHelper.IsActiveProgress(stiProgressStatus))
			{
				StiComponentGdiPainter.PaintProgress(graphics, rectangleD, stiProgressStatus);
			}
			else if (stiElement.IsDefined && stiElement.IsEnabled)
			{
				PaintContent(graphics, rectangleD, stiElement);
			}
			if (!stiElement.IsDefined && stiElement.IsDesigning && stiElement.IsEnabled && !StiComponentProgressHelper.IsActiveProgress(stiProgressStatus))
			{
				PaintNotDefined(graphics, rectangleD.ToRectangleF(), stiElement);
			}
			if (!stiElement.IsEnabled && stiElement.IsDesigning)
			{
				PaintNotEnabled(graphics, rect.ToRectangleF(), stiElement);
			}
			if (!component.Report.Info.IsComponentsMoving && stiElement.IsDefined && stiElement.IsDesigning)
			{
				PaintElementMarkers(graphics, paintRectangle, stiElement);
			}
			PaintCheckBoxTitle(graphics, borderContentRect, stiElement);
			PaintBorder(graphics, rect, stiElement);
		}
	}

	public virtual RectangleD PaintTitle(Graphics g, RectangleD rect, IStiElement element)
	{
		IStiTitle stiTitle = (element as IStiTitleElement)?.Title;
		if (stiTitle == null)
		{
			return rect;
		}
		if (!stiTitle.Visible || string.IsNullOrEmpty(stiTitle.Text))
		{
			return rect;
		}
		string titleText = StiReportParser.Parse(stiTitle.Text, element as StiComponent);
		IStiTitleElement element2 = element as IStiTitleElement;
		return PaintTitle(g, rect, element2, titleText, StiElementScale.Factor(element));
	}

	public static RectangleD GetPaintTitleRect(Graphics g, RectangleD rect, IStiTitleElement element, string titleText)
	{
		IStiTitle stiTitle = element?.Title;
		if (stiTitle == null || !stiTitle.Visible || string.IsNullOrEmpty(titleText))
		{
			return RectangleD.Empty;
		}
		SizeD sizeD = StiTitleMeasureHelper.MeasureTitle(g, rect, stiTitle, titleText);
		if (sizeD.Height > rect.Height / 2.0)
		{
			sizeD.Height = rect.Height / 2.0;
		}
		if (stiTitle.HorAlignment == StiHorAlignment.Center)
		{
			return new RectangleD(rect.X + (rect.Width - sizeD.Width) / 2.0, rect.Y, sizeD.Width, Math.Ceiling(sizeD.Height));
		}
		if (stiTitle.HorAlignment == StiHorAlignment.Left)
		{
			return new RectangleD(rect.X, rect.Y, sizeD.Width, Math.Ceiling(sizeD.Height));
		}
		return new RectangleD(rect.Right - sizeD.Width, rect.Y, sizeD.Width, Math.Ceiling(sizeD.Height));
	}

	public static RectangleD PaintTitle(Graphics g, RectangleD rect, IStiTitleElement element, string titleText, double zoom, bool isViewer = false)
	{
		IStiTitle stiTitle = element?.Title;
		if (stiTitle == null || !stiTitle.Visible || string.IsNullOrEmpty(titleText))
		{
			return rect;
		}
		SizeD sizeD = StiTitleMeasureHelper.MeasureTitle(g, rect, stiTitle, titleText, isViewer ? 1.0 : StiElementScale.Factor(element as IStiElement));
		if (sizeD.Height > rect.Height / 2.0)
		{
			sizeD.Height = rect.Height / 2.0;
		}
		RectangleD rect2 = new RectangleD(rect.X, rect.Y, rect.Width, Math.Ceiling(sizeD.Height));
		StiCornerRadius stiCornerRadius = (element as IStiCornerRadius)?.CornerRadius;
		StiSimpleBorder border = (element as IStiSimpleBorder)?.Border;
		if (stiTitle.BackColor != Color.Transparent)
		{
			if (stiCornerRadius != null && !stiCornerRadius.IsDefault)
			{
				GraphicsState val = g.Save();
				try
				{
					g.SetClip(rect2.ToRectangleF());
					StiRoundedBackgroundGdiPainter.PaintBackground(g, rect.ToRectangleF(), border, stiCornerRadius, stiTitle.BackColor, StiButtonShapeType.Rectangle, isEnabled: true, (float)zoom);
				}
				finally
				{
					g.Restore(val);
				}
			}
			else
			{
				StiDrawing.FillRectangle(g, stiTitle.BackColor, rect2);
			}
		}
		if (stiCornerRadius != null && (stiCornerRadius.TopLeft > 0f || stiCornerRadius.TopRight > 0f))
		{
			rect2.X += StiScale.I((double)stiCornerRadius.TopLeft * zoom);
			rect2.Width -= StiScale.I((double)(stiCornerRadius.TopLeft + stiCornerRadius.TopRight) * zoom);
		}
		if (!string.IsNullOrWhiteSpace(titleText))
		{
			Color color = ((stiTitle.ForeColor == Color.Transparent) ? StiDashboardStyleHelper.GetTitleForeColor(element as IStiElement) : stiTitle.ForeColor);
			rect2.Inflate(-4.0 * zoom * StiScale.Factor, 0.0);
			Font val2 = ((zoom == 1.0) ? stiTitle.Font : StiFontUtils.ChangeFontSize(stiTitle.Font, (float)((double)stiTitle.Font.Size * zoom)));
			GraphicsState val3 = g.Save();
			try
			{
				g.SetClip(rect2.ToRectangleF());
				StiAutoSizeTextHelper.Draw(g, rect2, titleText, val2, color, stiTitle.HorAlignment, StiVertAlignment.Center, stiTitle.SizeMode);
			}
			finally
			{
				g.Restore(val3);
			}
			if (zoom != 1.0)
			{
				val2.Dispose();
			}
		}
		rect.Y += rect2.Height;
		rect.Height -= rect2.Height;
		return rect;
	}

	public virtual void PaintCheckBoxTitle(Graphics g, RectangleD rect, IStiElement element)
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		if (!(element is IStiTitleElement stiTitleElement))
		{
			return;
		}
		StiComponent stiComponent = element as StiComponent;
		if (!stiComponent.IsSelected || stiComponent.Report.Info.IsComponentsMoving || !element.IsDesigning || !StiInplaceEditorTitleElement.IsComponentFocused(stiComponent))
		{
			return;
		}
		Rectangle rectangle = StiInplaceEditorTitleElement.GetSimpleTitleRect(rect, stiComponent).ToRectangle();
		if (rectangle.IsEmpty)
		{
			return;
		}
		bool flag = StiDashboardStyleHelper.IsDarkStyle(element);
		if (StiInplaceEditorTitleElement.IsTitleFocused(stiComponent))
		{
			StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(element);
			StiDrawing.FillRectangle(g, controlStyle.HotBackColor, rectangle);
		}
		rectangle.Inflate(-StiScaleUI.I2, -StiScaleUI.I2);
		Color color = (flag ? Color.Gainsboro : Color.FromArgb(255, 171, 171, 171));
		StiDrawing.DrawRectangle(g, color, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
		rectangle.Inflate(-StiScale.I3, -StiScale.I3);
		if (!stiTitleElement.Title.Visible)
		{
			return;
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			StiDrawing.FillRectangle(g, (Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual void PaintContent(Graphics g, RectangleD rect, IStiElement element)
	{
	}

	public virtual void PaintElementMarkers(Graphics g, RectangleD rect, IStiElement element)
	{
	}

	public virtual void PaintBorder(Graphics g, RectangleD rect, IStiElement element)
	{
		StiComponent stiComponent = element as StiComponent;
		if (stiComponent.HighlightState == StiHighlightState.Hide)
		{
			double num = StiElementScale.Factor(element);
			StiCornerRadius stiCornerRadius = (element as IStiCornerRadius)?.CornerRadius;
			StiSimpleBorder border = (element as IStiSimpleBorder)?.Border;
			StiButtonShapeType stiButtonShapeType = ((element is StiButtonElement stiButtonElement) ? stiButtonElement.ShapeType : StiButtonShapeType.Rectangle);
			if ((stiCornerRadius != null && !stiCornerRadius.IsEmpty) || stiButtonShapeType == StiButtonShapeType.Circle)
			{
				StiRoundedBorderGdiPainter.PaintBorder(g, rect.ToRectangleF(), border, stiCornerRadius, stiButtonShapeType, (float)num, stiComponent.IsDesigning);
			}
			else if (IsDefaultBorder(element) && stiComponent.IsDesigning)
			{
				PaintDefaultBorder(g, rect, element);
			}
			else
			{
				PaintRectangleBorder(g, rect.ToRectangleF(), element, (float)num);
			}
		}
	}

	public virtual void PaintDefaultBorder(Graphics g, RectangleD rect, IStiElement element)
	{
		StiDashboard stiDashboard = (element as StiComponent).Page as StiDashboard;
		Color color = (StiDashboardStyleHelper.IsDarkStyle(stiDashboard.Style) ? Color.FromArgb(150, Color.Silver) : Color.Silver);
		StiDrawing.DrawRectangle(g, color, rect.Left, rect.Top, rect.Width, rect.Height);
	}

	public static void PaintRectangleBorder(Graphics g, RectangleF rect, IStiElement element, float zoom)
	{
		StiSimpleBorder border = ((element is IStiSimpleBorder stiSimpleBorder) ? stiSimpleBorder.Border : null);
		Color backColor = ((element is IStiBackColor stiBackColor) ? StiDashboardExpressionHelper.GetBackColor(element, stiBackColor.BackColor) : Color.Transparent);
		StiBorderGdiPainter.PaintBorder(g, rect, border, backColor, zoom, isTooLightBorderAdoptation: true);
	}

	public virtual void PaintShadow(Graphics g, RectangleD rect, IStiElement element)
	{
		StiSimpleShadow stiSimpleShadow = (element as IStiSimpleShadow)?.Shadow;
		if (stiSimpleShadow != null && stiSimpleShadow.Visible)
		{
			float scale = (float)StiElementScale.Factor(element);
			Size size = rect.Size.ToSize();
			StiCornerRadius cornerRadius = (element as IStiCornerRadius)?.CornerRadius;
			StiBorderSides borderSide = ((element as IStiSimpleBorder)?.Border)?.Side ?? StiBorderSides.All;
			bool isCircle = element is IStiButtonElement stiButtonElement && stiButtonElement.ShapeType == StiButtonShapeType.Circle;
			Bitmap val = StiSimpleShadowCache.GetFromCache(element.Report, stiSimpleShadow, cornerRadius, borderSide, isCircle, size, scale);
			if (val == null)
			{
				val = StiSimpleShadowPainter.GetShadowBitmap(rect, stiSimpleShadow, cornerRadius, isCircle, scale);
				StiSimpleShadowCache.AddToCache(element.Report, stiSimpleShadow, val, cornerRadius, borderSide, isCircle, size, scale);
			}
			Point shadowLocation = StiSimpleShadowPainter.GetShadowLocation(rect, stiSimpleShadow, isCircle: false, scale);
			g.DrawImage((Image)(object)val, shadowLocation);
		}
	}

	public virtual void PaintBackground(Graphics g, RectangleD rect, IStiElement element)
	{
		Color color = StiDashboardStyleHelper.GetBackColor(element, null, allowOpacity: true);
		if (color == Color.Transparent && element.Report != null && element.Report.Info.FillComponent && element.IsDesigning)
		{
			color = DefaultBackgroundColor;
		}
		double num = StiElementScale.Factor(element) * StiScale.Factor;
		StiCornerRadius cornerRadius = (element as IStiCornerRadius)?.CornerRadius;
		StiSimpleBorder border = (element as IStiSimpleBorder)?.Border;
		StiRoundedBackgroundGdiPainter.PaintBackground(g, rect.ToRectangleF(), border, cornerRadius, color, StiButtonShapeType.Rectangle, element.IsEnabled, (float)num);
		StiAdvancedWatermark stiAdvancedWatermark = (element as IStiDashboardWatermark)?.DashboardWatermark;
		if (stiAdvancedWatermark != null)
		{
			StiAdvancedWatermarkGdiPainter.PaintWatermark(g, stiAdvancedWatermark, cornerRadius, rect, num);
		}
	}

	public virtual void PaintNoResult(Graphics g, RectangleD rect, IStiElement element, double? zoom = null)
	{
		string text = ((StiScale.Factor >= 2.0) ? "_x2" : "");
		string fullImageName = "Stimulsoft.Dashboard.Images.NoResult.NoResult" + text + ".png";
		PaintStatus(g, rect.ToRectangleF(), element, Loc.Get("Dashboard", "NoResult"), fullImageName, zoom);
	}

	public virtual void PaintEmptyData(Graphics g, RectangleD rect, IStiElement element, double? zoom = null)
	{
		string text = ((StiScale.Factor >= 2.0) ? "_x2" : "");
		string fullImageName = "Stimulsoft.Dashboard.Images.NoResult.NoResult" + text + ".png";
		PaintStatus(g, rect.ToRectangleF(), element, Loc.Get("Dashboard", "NoResult"), fullImageName, zoom);
	}

	public virtual void PaintNotDefined(Graphics g, RectangleF rect, IStiElement element, double? zoom = null)
	{
		PaintNotDefinedStatus(g, rect, element, Loc.Get("Dashboard", "DragDropDataFromDictionary"), GetElementIdent(element), zoom);
	}

	public virtual void PaintNotEnabled(Graphics g, RectangleF rect, IStiElement element, double? zoom = null)
	{
		StiDrawing.FillRectangle(g, Color.FromArgb(17, Color.Gray), rect.Left, rect.Top, rect.Width, rect.Height);
		PaintNotDefinedStatus(g, rect, element, Loc.Get("PropertyMain", "Disabled"), GetElementIdent(element), zoom);
	}

	public virtual void PaintNotDefinedStatus(Graphics g, RectangleF rect, IStiElement element, string text, string imageName, double? zoom = null)
	{
		string text2 = ((StiScale.Factor >= 2.0) ? "_x2" : "");
		imageName = "Stimulsoft.Dashboard.Images.NotDefined.Sti" + imageName + text2 + ".png";
		PaintStatus(g, rect, element, text, imageName, zoom);
	}

	public virtual void PaintStatus(Graphics g, RectangleF rect, IStiElement element, string text, string fullImageName, double? zoom = null)
	{
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		rect.Inflate(-StiScale.I10, -StiScale.I10);
		bool flag = rect.Width > (float)StiScale.XXI(60.0) && rect.Height > (float)StiScale.YYI(60.0);
		bool flag2 = rect.Width > (float)StiScale.XXI(200.0) && rect.Height > (float)StiScale.YYI(60.0);
		Bitmap image = StiImageUtils.GetImage(typeof(StiDashboard), fullImageName);
		try
		{
			Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont(zoom.GetValueOrDefault(element.Zoom));
			try
			{
				int num = ((!(StiScale.Factor >= 2.0)) ? 1 : 2);
				int num2 = StiScale.XXI(((Image)image).Width) / num;
				int num3 = StiScale.YYI(((Image)image).Height) / num;
				float num4 = (float)num3 + gdiFont.GetHeight() + (float)StiScale.I4;
				Rectangle rectangle = new RectangleD(rect.X, rect.Y + (rect.Height - num4) / 2f, rect.Width, num4).ToRectangle();
				if (flag)
				{
					if (flag2)
					{
						g.DrawImage((Image)(object)image, rectangle.X + (rectangle.Width - num2) / 2, rectangle.Y, num2, num3);
					}
					else
					{
						g.DrawImage((Image)(object)image, rectangle.X + (rectangle.Width - num2) / 2, rectangle.Y + (rectangle.Height - num3) / 2, num2, num3);
					}
				}
				if (flag2)
				{
					StringFormat val = new StringFormat();
					try
					{
						val.LineAlignment = (StringAlignment)2;
						val.Alignment = (StringAlignment)1;
						Color foreColor = StiDashboardStyleHelper.GetForeColor(element);
						RectangleD rect2 = new RectangleD(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
						StiTextDrawing.DrawString(g, text, gdiFont, foreColor, rect2, val);
						return;
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)gdiFont)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)image)?.Dispose();
		}
	}

	public virtual void PaintError(Graphics g, RectangleF rect, IStiElement element)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		string error = StiErrorStack.GetError(element.GetKey());
		Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont(element.Zoom);
		try
		{
			StringFormat val = new StringFormat();
			try
			{
				val.LineAlignment = (StringAlignment)1;
				val.Alignment = (StringAlignment)1;
				StiTextDrawing.DrawString(g, error, gdiFont, Color.Red, RectangleD.CreateFromRectangle(rect), val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)gdiFont)?.Dispose();
		}
	}

	public override void PaintHighlight(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		if (component.HighlightState != StiHighlightState.Show && component.HighlightState != StiHighlightState.Active)
		{
			return;
		}
		RectangleD paintRectangle = component.GetPaintRectangle();
		paintRectangle.Inflate(1.0, 1.0);
		Color color = Color.FromArgb(200, StiDashboardStyleHelper.IsDarkStyle(component.Page as IStiDashboard) ? StiElementConsts.Highlight.DarkColor : StiElementConsts.Highlight.Color);
		Pen val = new Pen(color);
		try
		{
			switch (component.HighlightState)
			{
			case StiHighlightState.Show:
				StiDrawing.DrawRectangle(graphics, val, paintRectangle);
				break;
			case StiHighlightState.Active:
				val.Width = 2f;
				val.DashStyle = (DashStyle)1;
				StiDrawing.DrawRectangle(graphics, val, paintRectangle);
				break;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected virtual string GetElementIdent(object element)
	{
		if (!(element is StiElement { ComponentId: var componentId }))
		{
			return string.Empty;
		}
		string text = componentId.ToString();
		if (text.StartsWith("Sti"))
		{
			text = text.Substring(3);
		}
		if (text.EndsWith("Element"))
		{
			text = text.Replace("Element", "");
		}
		return text;
	}

	public override Color GetSelectionCornerColor(StiComponent comp)
	{
		if (comp.Locked)
		{
			return Color.Red;
		}
		StiDashboard stiDashboard = comp.Page as StiDashboard;
		if (!StiDashboardStyleHelper.IsDarkStyle(stiDashboard.Style))
		{
			return Color.Gray;
		}
		return Color.LightGray;
	}

	public static RectangleD ApplyCornerRadiusAndSetClip(Graphics g, RectangleD rect, IStiElement element)
	{
		StiCornerRadius stiCornerRadius = (element as IStiCornerRadius)?.CornerRadius;
		if (stiCornerRadius == null || stiCornerRadius.IsEmpty)
		{
			return rect;
		}
		double scale = StiElementScale.Factor(element);
		rect = StiCornerRadiusHelper.FitRectIntoCornerRadius(rect, stiCornerRadius, scale, element is IStiFixedHeightElement);
		g.SetClip(rect.ToRectangleF());
		return rect;
	}

	public virtual bool IsNoResult(IStiElement element, StiDataTable dataTable)
	{
		if (dataTable == null)
		{
			return element.IsQuerable;
		}
		return false;
	}

	public virtual bool IsEmptyData(IStiElement element, StiDataTable dataTable)
	{
		if (dataTable != null && dataTable.IsEmpty)
		{
			return element.IsQuerable;
		}
		return false;
	}

	public virtual bool IsDefaultBorder(IStiElement element)
	{
		StiSimpleBorder stiSimpleBorder2 = ((element is IStiSimpleBorder stiSimpleBorder) ? stiSimpleBorder.Border : null);
		if (stiSimpleBorder2 == null)
		{
			return false;
		}
		if (stiSimpleBorder2.Style != StiPenStyle.None)
		{
			return stiSimpleBorder2.Side == StiBorderSides.None;
		}
		return true;
	}

	public virtual bool IsDefined(StiComponent component)
	{
		if (component is IStiElement stiElement)
		{
			return stiElement.IsDefined;
		}
		return false;
	}
}

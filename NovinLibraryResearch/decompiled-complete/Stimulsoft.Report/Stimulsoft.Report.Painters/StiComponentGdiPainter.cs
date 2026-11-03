using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.QuickButtons;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Painters;

public class StiComponentGdiPainter : StiGdiPainter
{
	public virtual void PaintEvents(StiComponent component, Graphics g, RectangleD rect)
	{
		if (component.IsDesigning && !component.IsEventEmpty && component.Report.Info.DrawEventMarkers)
		{
			if (StiOptions.Viewer.Pins.EventsRightToLeft)
			{
				float num = (float)((RectangleD)(ref rect)).Right;
				float num2 = (float)((RectangleD)(ref rect)).Y;
				g.FillPolygon(Brushes.Red, new PointF[3]
				{
					new PointF(num, num2),
					new PointF(num - (float)StiScale.I4, num2),
					new PointF(num, num2 + (float)StiScale.I4)
				});
			}
			else
			{
				float num3 = (float)((RectangleD)(ref rect)).X;
				float num4 = (float)((RectangleD)(ref rect)).Y;
				g.FillPolygon(Brushes.Red, new PointF[3]
				{
					new PointF(num3, num4),
					new PointF(num3 + (float)StiScale.I4, num4),
					new PointF(num3, num4 + (float)StiScale.I4)
				});
			}
		}
	}

	public virtual void PaintConditions(StiComponent component, Graphics g, RectangleD rect)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		PaintConditionsAndFilter(component, g, rect);
	}

	public virtual void PaintConditionsAndFilter(StiComponent component, Graphics g, RectangleD rect)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		if (!component.IsDesigning || component.Page == null || component.Page.Zoom < 0.5)
		{
			return;
		}
		float num = 0f;
		float num2 = 0f;
		if (component.Conditions != null && component.Conditions.Count > 0)
		{
			RectangleF rectangleF = ((RectangleD)(ref rect)).ToRectangleF();
			float num3 = rectangleF.X + 1f;
			if (StiOptions.Viewer.Pins.ConditionsRightToLeft)
			{
				num3 = rectangleF.Right - 1f - (float)StiDataBuilder.ImageCondition.Width;
				num2 = 1 + StiDataBuilder.ImageCondition.Width;
			}
			else
			{
				num = 1 + StiDataBuilder.ImageCondition.Width;
			}
			RectangleD paintRectangle = component.GetPaintRectangle();
			RectangleF rectangleF2 = ((RectangleD)(ref paintRectangle)).ToRectangleF();
			float num4 = rectangleF.Bottom - (float)StiDataBuilder.ImageCondition.Height - 1f;
			if (num4 < rectangleF2.Top)
			{
				num4 = rectangleF2.Top - (float)StiScale.I1;
			}
			g.DrawImage(StiDataBuilder.ImageCondition, num3, num4, (float)StiDataBuilder.ImageCondition.Width, (float)StiDataBuilder.ImageCondition.Height);
		}
		if (component.IsDesigning)
		{
			StiInteraction interaction = component.Interaction;
			if (interaction != null && !interaction.IsDefaultExt)
			{
				RectangleF rectangleF3 = ((RectangleD)(ref rect)).ToRectangleF();
				float num5 = rectangleF3.X + 1f + num;
				if (StiOptions.Viewer.Pins.FiltersRightToLeft)
				{
					num5 = rectangleF3.Right - 1f - (float)StiDataBuilder.ImageInteractions.Width - num2;
					num2 += (float)(1 + StiDataBuilder.ImageInteractions.Width);
				}
				else
				{
					num += (float)(1 + StiDataBuilder.ImageInteractions.Width);
				}
				g.DrawImage(StiDataBuilder.ImageInteractions, num5, rectangleF3.Bottom - (float)StiDataBuilder.ImageInteractions.Height - 1f, (float)StiDataBuilder.ImageFilter.Width, (float)StiDataBuilder.ImageFilter.Height);
			}
		}
		IStiFilter stiFilter = component as IStiFilter;
		if (stiFilter?.Filters != null && stiFilter.Filters.Count > 0 && stiFilter.FilterOn)
		{
			RectangleF rectangleF4 = ((RectangleD)(ref rect)).ToRectangleF();
			float num6 = rectangleF4.X + 1f + num;
			if (StiOptions.Viewer.Pins.FiltersRightToLeft)
			{
				num6 = rectangleF4.Right - 1f - (float)StiDataBuilder.ImageFilter.Width - num2;
			}
			g.DrawImage(StiDataBuilder.ImageFilter, num6, rectangleF4.Bottom - (float)StiDataBuilder.ImageFilter.Height - 1f, (float)StiDataBuilder.ImageFilter.Width, (float)StiDataBuilder.ImageFilter.Height);
		}
	}

	public virtual void PaintInteraction(StiComponent component, Graphics g)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		if (component.IsDesigning || component.IsPrinting || component.Page == null || component.Page.Zoom < 0.5 || component?.Report?.Info == null || !component.Report.Info.ShowInteractive)
		{
			return;
		}
		if (((IStiInteraction)component).Interaction == null || ((IStiInteraction)component).Interaction.SortingDirection == StiInteractionSortDirection.None)
		{
			return;
		}
		RectangleD paintRectangle = component.GetPaintRectangle();
		RectangleF rectangleF = ((RectangleD)(ref paintRectangle)).ToRectangleF();
		Image val = ((((IStiInteraction)component).Interaction.SortingDirection == StiInteractionSortDirection.Ascending) ? StiDataBuilder.ImageSortAsc : StiDataBuilder.ImageSortDesc);
		float num = 2f;
		if (((IStiInteraction)component).Interaction.SortingIndex > 0)
		{
			string text = ((IStiInteraction)component).Interaction.SortingIndex.ToString();
			Font val2 = new Font("Arial", 6f);
			try
			{
				StringFormat val3 = new StringFormat();
				try
				{
					val3.LineAlignment = (StringAlignment)1;
					val3.Alignment = (StringAlignment)0;
					num = g.MeasureString(text, val2).Width + 4f;
					RectangleF rectangleF2 = new RectangleF(rectangleF.Right - num, rectangleF.Y, num, rectangleF.Height);
					if (StiOptions.Viewer.Pins.InteractionSortRightToLeft)
					{
						rectangleF2.X = rectangleF.X;
					}
					g.DrawString(text, val2, Brushes.Black, rectangleF2, val3);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		float num2 = rectangleF.Right - (float)val.Width - num;
		if (StiOptions.Viewer.Pins.InteractionSortRightToLeft)
		{
			num2 = rectangleF.X + num;
		}
		g.DrawImage(val, num2, rectangleF.Y + (rectangleF.Height - (float)val.Height) / 2f, (float)val.Width, (float)val.Height);
	}

	public virtual void PaintNoDefinedStatus(Graphics g, RectangleF rect, StiComponent comp, string imagePath, string defaultText)
	{
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		rect.Inflate(-StiScale.I10, -StiScale.I10);
		bool flag = rect.Width > (float)StiScale.XXI(60.0) && rect.Height > (float)StiScale.YYI(60.0);
		bool flag2 = rect.Width > (float)StiScale.XXI(200.0) && rect.Height > (float)StiScale.YYI(60.0);
		string text = ((StiScale.Factor > 1.0) ? "_x2" : "");
		Bitmap image = StiImageUtils.GetImage(typeof(StiReport), "Stimulsoft.Report.Images.NotDefined." + imagePath + text + ".png");
		try
		{
			Font gdiFont = DragDrop.Font.GetGdiFont(comp.Report.Info.Zoom, (float?)null, (Font)null);
			try
			{
				int width = ((Image)image).Width;
				int height = ((Image)image).Height;
				float num = (float)height + gdiFont.GetHeight() + (float)StiScale.I4;
				RectangleD val = new RectangleD((double)rect.X, (double)(rect.Y + (rect.Height - num) / 2f), (double)rect.Width, (double)num);
				Rectangle rectangle = ((RectangleD)(ref val)).ToRectangle();
				if (flag)
				{
					if (flag2)
					{
						g.DrawImage((Image)(object)image, rectangle.X + (rectangle.Width - width) / 2, rectangle.Y, width, height);
					}
					else
					{
						g.DrawImage((Image)(object)image, rectangle.X + (rectangle.Width - width) / 2, rectangle.Y + (rectangle.Height - height) / 2, width, height);
					}
				}
				if (flag2)
				{
					StringFormat val2 = new StringFormat();
					try
					{
						val2.LineAlignment = (StringAlignment)2;
						val2.Alignment = (StringAlignment)1;
						Color gray = Color.Gray;
						RectangleD val3 = default(RectangleD);
						((RectangleD)(ref val3))._002Ector((double)rectangle.X, (double)rectangle.Y, (double)rectangle.Width, (double)rectangle.Height);
						StiTextDrawing.DrawString(g, defaultText, gdiFont, gray, val3, val2);
						return;
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
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

	public virtual void PaintInheritedImage(StiComponent component, Graphics g)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (!component.IsDesigning || component.Report == null || component.Page.Zoom < 0.25)
		{
			return;
		}
		if (component.Inherited)
		{
			RectangleD paintRectangle = component.GetPaintRectangle();
			RectangleF rectangleF = ((RectangleD)(ref paintRectangle)).ToRectangleF();
			float num = (StiOptions.Viewer.Pins.InheritedRightToLeft ? (rectangleF.Right - (float)StiDataBuilder.ImageLocked.Width) : rectangleF.X);
			g.DrawImage(StiDataBuilder.ImageLocked, num, rectangleF.Y, (float)StiDataBuilder.ImageLocked.Width, (float)StiDataBuilder.ImageLocked.Height);
		}
		if (!(component is StiContainer stiContainer))
		{
			return;
		}
		foreach (StiComponent component2 in stiContainer.Components)
		{
			PaintInheritedImage(component2, g);
		}
	}

	public virtual void PaintOrderAndQuickInfo(StiComponent component, Graphics g, string number)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (!(component is StiPage) && !component.Report.Info.IsComponentsMoving)
		{
			RectangleD paintRectangle = component.GetPaintRectangle();
			RectangleF rectangleF = ((RectangleD)(ref paintRectangle)).ToRectangleF();
			SizeF sizeF = SizeF.Empty;
			if (component.Report.Info.ShowOrder)
			{
				number += component.Parent.Components.IndexOf(component);
				sizeF = g.MeasureString(number, StiOptions.Engine.QuickInfo.QuickInfoSystemFont);
				RectangleF rectangleF2 = new RectangleF(rectangleF.X, rectangleF.Y, sizeF.Width, sizeF.Height);
				if (StiOptions.Viewer.Pins.OrderAndQuickInfoRightToLeft)
				{
					rectangleF2.X = rectangleF.Right - sizeF.Width;
				}
				g.FillRectangle(Brushes.Blue, rectangleF2);
				g.DrawString(number, StiOptions.Engine.QuickInfo.QuickInfoSystemFont, Brushes.White, rectangleF2);
			}
			if (component.Report.Info.QuickInfoOverlay && component.Report.Info.QuickInfoType != 0)
			{
				string quickInfo = component.GetQuickInfo();
				SizeF sizeF2 = g.MeasureString(quickInfo, StiOptions.Engine.QuickInfo.QuickInfoSystemFont);
				RectangleF rectangleF3 = new RectangleF(rectangleF.X + sizeF.Width, rectangleF.Y, sizeF2.Width, sizeF2.Height);
				if (rectangleF3.Width > rectangleF.Width - sizeF.Width)
				{
					rectangleF3.Width = rectangleF.Width - sizeF.Width;
				}
				if (StiOptions.Viewer.Pins.OrderAndQuickInfoRightToLeft)
				{
					rectangleF3.X = rectangleF.Right - sizeF.Width - sizeF2.Width;
				}
				g.FillRectangle(Brushes.Green, rectangleF3);
				g.DrawString(quickInfo, StiOptions.Engine.QuickInfo.QuickInfoSystemFont, Brushes.White, rectangleF3, StiOptions.Engine.QuickInfo.QuickInfoStringFormat);
			}
		}
		if (!(component is StiContainer stiContainer))
		{
			return;
		}
		if (number.Length > 0)
		{
			number += ".";
		}
		foreach (StiComponent component2 in stiContainer.Components)
		{
			PaintOrderAndQuickInfo(component2, g, number);
		}
	}

	public virtual void PaintQuickButtons(StiComponent component, Graphics g)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		if (!component.IsDesigning || !component.ShowQuickButtons || component.Report.Info.IsComponentsMoving || !StiRestrictionsHelper.IsAllowChange(component))
		{
			return;
		}
		StiQuickButton[] quickButtons = component.Report.Designer.GetQuickButtons(component);
		if (quickButtons == null || quickButtons.Length == 0)
		{
			return;
		}
		RectangleD paintRectangle = component.GetPaintRectangle();
		if (component is StiBand stiBand)
		{
			((RectangleD)(ref paintRectangle)).Y = ((RectangleD)(ref paintRectangle)).Y - stiBand.HeaderSize * component.Page.Zoom * StiScale.Factor;
			((RectangleD)(ref paintRectangle)).Height = stiBand.HeaderSize * component.Page.Zoom * StiScale.Factor;
		}
		if (!StiQuickButtonHelper.CheckVisibleQuickButtons(paintRectangle))
		{
			return;
		}
		int num = 0;
		StiQuickButton[] array = quickButtons;
		foreach (StiQuickButton stiQuickButton in array)
		{
			if (stiQuickButton.Image != null && stiQuickButton.SelectedImage != null)
			{
				Image val = (stiQuickButton.IsSelected ? stiQuickButton.SelectedImage : stiQuickButton.Image);
				RectangleD quickButtonRect = StiQuickButtonHelper.GetQuickButtonRect(quickButtons, num, paintRectangle);
				Rectangle rectangle = ((RectangleD)(ref quickButtonRect)).ToRectangle();
				rectangle = new Rectangle(rectangle.X - (val.Width - rectangle.Width) / 2, rectangle.Y - (val.Height - rectangle.Height) / 2, val.Width, val.Height);
				g.DrawImage(val, rectangle);
			}
			num++;
		}
	}

	public virtual void PaintMarkers(StiComponent component, Graphics g, RectangleD rect, Color? color = null)
	{
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (!component.IsDesigning || component.Report.Info.MarkersStyle == StiMarkersStyle.None || (component.Report.Info.IsComponentsMoving && (!component.Report.Info.DrawMarkersWhenMoving || !component.Report.Info.IsComponentsMoving)))
		{
			return;
		}
		if (component.Report.Info.MarkersStyle == StiMarkersStyle.Corners)
		{
			int i = StiScale.I3;
			if (!color.HasValue)
			{
				color = Color.DimGray;
			}
			Pen val = new Pen(color.Value);
			try
			{
				StiDrawing.DrawLine(g, val, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y, ((RectangleD)(ref rect)).X + (double)i, ((RectangleD)(ref rect)).Y);
				StiDrawing.DrawLine(g, val, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y + (double)i);
				StiDrawing.DrawLine(g, val, ((RectangleD)(ref rect)).Right, ((RectangleD)(ref rect)).Y, ((RectangleD)(ref rect)).Right - (double)i, ((RectangleD)(ref rect)).Y);
				StiDrawing.DrawLine(g, val, ((RectangleD)(ref rect)).Right, ((RectangleD)(ref rect)).Y, ((RectangleD)(ref rect)).Right, ((RectangleD)(ref rect)).Y + (double)i);
				StiDrawing.DrawLine(g, val, ((RectangleD)(ref rect)).Right, ((RectangleD)(ref rect)).Bottom, ((RectangleD)(ref rect)).Right, ((RectangleD)(ref rect)).Bottom - (double)i);
				StiDrawing.DrawLine(g, val, ((RectangleD)(ref rect)).Right, ((RectangleD)(ref rect)).Bottom, ((RectangleD)(ref rect)).Right - (double)i, ((RectangleD)(ref rect)).Bottom);
				StiDrawing.DrawLine(g, val, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Bottom, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Bottom - (double)i);
				StiDrawing.DrawLine(g, val, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Bottom, ((RectangleD)(ref rect)).X + (double)i, ((RectangleD)(ref rect)).Bottom);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		StiDrawing.DrawRectangle(g, StiOptions.Engine.QuickInfo.DashStylePen, rect);
	}

	public virtual void PaintBorder(StiComponent component, Graphics g, RectangleD rect)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		PaintBorder(component, g, rect, drawBorderFormatting: true, drawBorderSides: true);
	}

	public virtual void PaintBorder(StiComponent component, Graphics g, RectangleD rect, bool drawBorderFormatting, bool drawBorderSides)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		PaintBorder(component, g, rect, component.Page?.Zoom ?? 1.0, drawBorderFormatting, drawBorderSides);
	}

	public virtual void PaintBorder(StiComponent component, Graphics g, RectangleD rect, double zoom, bool drawBorderFormatting, bool drawBorderSides)
	{
		PaintBorder(component, g, ((RectangleD)(ref rect)).ToRectangleF(), (float)zoom, drawBorderFormatting, drawBorderSides);
	}

	public virtual void PaintBorder(StiComponent component, Graphics g, RectangleF rect, float zoom, bool drawBorderFormatting, bool drawBorderSides)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		if (!(component is IStiBorder { Border: not null } stiBorder))
		{
			return;
		}
		if ((int)stiBorder.Border.Style == 5)
		{
			Color color = Color.White;
			if (component is IStiBrush { Brush: not null } stiBrush)
			{
				color = StiBrush.ToColor(stiBrush.Brush);
			}
			if (color == Color.Transparent)
			{
				color = Color.White;
			}
			stiBorder.Border.Draw(g, rect, zoom, color, drawBorderFormatting, drawBorderSides);
		}
		else
		{
			stiBorder.Border.Draw(g, rect, zoom, Color.White, drawBorderFormatting, drawBorderSides);
		}
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		return null;
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
	}

	public override void PaintSelection(StiComponent component, StiPaintEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		if (component.IsDesigning && component.IsSelected && !component.Report.Info.IsComponentsMoving)
		{
			RectangleD paintRectangle = component.GetPaintRectangle();
			int num = StiScale.I2;
			if (component.Linked)
			{
				num = StiScale.I3;
			}
			Color selectionCornerColor = GetSelectionCornerColor(component);
			SolidBrush val = new SolidBrush(selectionCornerColor);
			try
			{
				StiDrawing.DrawSelectedRectangle(graphics, num, (Brush)(object)val, paintRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public virtual Color GetSelectionCornerColor(StiComponent comp)
	{
		if (!comp.Locked)
		{
			return Color.DimGray;
		}
		return Color.Red;
	}

	public override void PaintHighlight(StiComponent component, StiPaintEventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		if (component.HighlightState == StiHighlightState.Show || component.HighlightState == StiHighlightState.Active)
		{
			RectangleD paintRectangle = component.GetPaintRectangle();
			((RectangleD)(ref paintRectangle)).Inflate((double)StiScale.I1, (double)StiScale.I1);
			Color color = StiOptions.Viewer.HighlightShowStateColor;
			if (color == Color.Red)
			{
				color = Color.FromArgb(255, 43, 87, 154);
			}
			Pen val = new Pen(color);
			try
			{
				if (component.HighlightState == StiHighlightState.Show)
				{
					StiDrawing.DrawRectangle(graphics, val, paintRectangle);
				}
				else if (component.HighlightState == StiHighlightState.Active)
				{
					val.Width = StiScale.I2;
					StiDrawing.DrawRectangle(graphics, val, paintRectangle);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (!(component is StiContainer stiContainer))
		{
			return;
		}
		foreach (StiComponent component2 in stiContainer.Components)
		{
			component2.PaintHighlight(e);
		}
	}

	public override Bitmap GetThumbnail(StiComponent component, int width, int height, bool isDesignTime)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		StiScale.Lock();
		StiUnit stiUnit = ((component is StiPage) ? ((StiPage)component).Unit : component.Page.Unit);
		double num = width;
		RectangleD displayRectangle = component.DisplayRectangle;
		double val = num / stiUnit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width);
		double num2 = height;
		displayRectangle = component.DisplayRectangle;
		double val2 = num2 / stiUnit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Height);
		double num3 = Math.Min(val, val2);
		double zoom = component.Report.Info.Zoom;
		bool forceDesigningMode = component.Report.Info.ForceDesigningMode;
		bool showGrid = component.Report.Info.ShowGrid;
		StiMarkersStyle markersStyle = component.Report.Info.MarkersStyle;
		bool drawEventMarkers = component.Report.Info.DrawEventMarkers;
		bool fillComponent = component.Report.Info.FillComponent;
		component.Report.Info.Zoom = num3;
		component.Report.Info.ForceDesigningMode = isDesignTime;
		component.Report.Info.ShowGrid = false;
		component.Report.Info.MarkersStyle = StiMarkersStyle.None;
		component.Report.Info.DrawEventMarkers = false;
		component.Report.Info.FillComponent = false;
		displayRectangle = component.DisplayRectangle;
		int num4 = (int)(stiUnit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width) * num3);
		displayRectangle = component.DisplayRectangle;
		int num5 = (int)(stiUnit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Height) * num3);
		if (num4 == 0)
		{
			num4 = 1;
		}
		if (num5 == 0)
		{
			num5 = 1;
		}
		Bitmap val5;
		if (component is StiForm)
		{
			displayRectangle = component.DisplayRectangle;
			int num6 = (int)((RectangleD)(ref displayRectangle)).Width;
			displayRectangle = component.DisplayRectangle;
			Bitmap val3 = new Bitmap(num6, (int)((RectangleD)(ref displayRectangle)).Height);
			Graphics val4 = Graphics.FromImage((Image)(object)val3);
			try
			{
				val4.Clear(Color.White);
				component.Paint(new StiPaintEventArgs(val4, RectangleD.Empty));
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			val5 = new Bitmap(num4, num5);
			Graphics val6 = Graphics.FromImage((Image)(object)val5);
			try
			{
				val6.DrawImage((Image)(object)val3, 0, 0, num4, num5);
				val6.ResetTransform();
				val6.DrawRectangle(Pens.Gray, 0, 0, num4 - 1, num5 - 1);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		else if (component is StiPage)
		{
			val5 = new Bitmap(num4, num5);
			Graphics val7 = Graphics.FromImage((Image)(object)val5);
			try
			{
				val7.Clear(Color.White);
				((StiPage)component).LockHighlight = true;
				component.Paint(new StiPaintEventArgs(val7, RectangleD.Empty));
				((StiPage)component).LockHighlight = false;
				val7.ResetTransform();
				val7.DrawRectangle(Pens.Gray, 0, 0, num4 - 1, num5 - 1);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		else
		{
			component.Report.Info.Zoom = 1.0;
			RectangleD paintRectangle = component.GetPaintRectangle();
			Bitmap val8 = new Bitmap((int)Math.Round(((RectangleD)(ref paintRectangle)).Width), (int)Math.Round(((RectangleD)(ref paintRectangle)).Height));
			try
			{
				Graphics val9 = Graphics.FromImage((Image)(object)val8);
				try
				{
					val9.Clear(Color.White);
					val9.TranslateTransform(0f - (float)((RectangleD)(ref paintRectangle)).X, 0f - (float)((RectangleD)(ref paintRectangle)).Y);
					component.Paint(new StiPaintEventArgs(val9, RectangleD.Empty));
					val9.ResetTransform();
					if (((Image)val8).Width > ((Image)val8).Height)
					{
						float num7 = ((Image)val8).Width / ((Image)val8).Height;
						Image thumbnailImage = ((Image)val8).GetThumbnailImage(width, (int)((float)height / num7), (GetThumbnailImageAbort)null, IntPtr.Zero);
						val5 = (Bitmap)(object)((thumbnailImage is Bitmap) ? thumbnailImage : null);
					}
					else
					{
						float num8 = ((Image)val8).Height / ((Image)val8).Width;
						Image thumbnailImage2 = ((Image)val8).GetThumbnailImage((int)((float)width / num8), height, (GetThumbnailImageAbort)null, IntPtr.Zero);
						val5 = (Bitmap)(object)((thumbnailImage2 is Bitmap) ? thumbnailImage2 : null);
					}
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		component.Report.Info.Zoom = zoom;
		component.Report.Info.ForceDesigningMode = forceDesigningMode;
		component.Report.Info.ShowGrid = showGrid;
		component.Report.Info.MarkersStyle = markersStyle;
		component.Report.Info.DrawEventMarkers = drawEventMarkers;
		component.Report.Info.FillComponent = fillComponent;
		StiScale.Unlock();
		return val5;
	}

	public static void PaintProgress(Graphics g, RectangleD rect, StiProgressStatus progressStatus, float? value = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if ((int)progressStatus == 2)
		{
			PaintShortProgress(g, rect);
		}
		else if ((int)progressStatus == 3)
		{
			PaintLongProgress(g, rect, value);
		}
	}

	private static void PaintShortProgress(Graphics g, RectangleD rect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		Font val = new Font("Arial", 8f);
		try
		{
			StringFormat val2 = new StringFormat();
			try
			{
				val2.LineAlignment = (StringAlignment)1;
				val2.Alignment = (StringAlignment)1;
				string text = Loc.Get("A_WebViewer", "Loading");
				SizeF sizeF = g.MeasureString(text, val);
				if (!((double)sizeF.Width > ((RectangleD)(ref rect)).Width) && !((double)sizeF.Height > ((RectangleD)(ref rect)).Height))
				{
					g.DrawString(text, val, Brushes.LightGray, ((RectangleD)(ref rect)).ToRectangleF(), val2);
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static void PaintLongProgress(Graphics g, RectangleD rect, float? value)
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		double num = Math.Min(((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height) * 0.800000011920929;
		if (num <= 0.0)
		{
			return;
		}
		if (num > 80.0 * StiScale.Factor)
		{
			num = 80.0 * StiScale.Factor;
		}
		int num2 = (int)(num * 0.1);
		RectangleF rectangleF = default(RectangleF);
		rectangleF.X = (float)(((RectangleD)(ref rect)).X + (((RectangleD)(ref rect)).Width - num) / 2.0);
		rectangleF.Y = (float)(((RectangleD)(ref rect)).Y + (((RectangleD)(ref rect)).Height - num) / 2.0);
		rectangleF.Width = (float)num;
		rectangleF.Height = (float)num;
		RectangleF rectangleF2 = rectangleF;
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)4;
		GraphicsPath val = new GraphicsPath();
		try
		{
			if (!value.HasValue)
			{
				value = StiComponentProgressHelper.CurrentValue;
			}
			val.AddArc(rectangleF2, value.Value, 65f);
			Pen val2 = new Pen(Color.FromArgb(230, 230, 230), (float)num2);
			try
			{
				g.DrawEllipse(val2, rectangleF2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			Pen val3 = new Pen(Color.FromArgb(200, 74, 125, 177), (float)num2);
			try
			{
				g.DrawPath(val3, val);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		g.SmoothingMode = smoothingMode;
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiTreeViewElementGdiPainter : StiControlElementGdiPainter
{
	public override void PaintContent(Graphics g, RectangleD rect, IStiElement element)
	{
		StiErrorStack.SetOk(element.GetKey());
		GraphicsState val = g.Save();
		try
		{
			g.SetClip(rect.ToRectangleF());
			rect = StiElementGdiPainter.ApplyCornerRadiusAndSetClip(g, rect, element);
			StiDataTable dataTable = (StiDbsThumbnaiModelHelper.AllowPage(element.Page) ? StiThumbnailDataHelper.Get(element) : StiElementDataCache.GetOrCreateWithProgress(element));
			if (IsNoResult(element, dataTable))
			{
				PaintNoResult(g, rect, element);
			}
			else
			{
				PaintTreeView(g, rect.ToRectangleF(), element as StiTreeViewElement, dataTable);
			}
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

	private void PaintTreeView(Graphics g, RectangleF rect, StiTreeViewElement element, StiDataTable dataTable)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return;
		}
		float num = (float)StiElementScale.Factor((IStiElement)element);
		double num2 = (double)num * StiScale.Factor;
		float num3 = (float)(num2 * 15.0);
		Color foreColor = GetForeColor(element);
		Font font = GetFont(element);
		StringFormat val = new StringFormat();
		try
		{
			Font val2 = StiFontUtils.ChangeFontSize(font, font.Size * num);
			try
			{
				val.FormatFlags = (StringFormatFlags)4096;
				val.LineAlignment = (StringAlignment)1;
				if (element.ShowAllValue)
				{
					RectangleF rect2 = new RectangleF(rect.X, rect.Y, rect.Width, num3 * 2f);
					if (element.SelectionMode == StiItemSelectionMode.One)
					{
						rect2.X += num3;
						rect2.Width -= num3;
					}
					PaintItem(g, rect2, element, StiLocHelper.LocAll, val2, foreColor, val, (float)num2);
					StiDrawing.DrawLine(g, GetSeparatorColor(element), rect.X, (float)((double)rect.Y + 30.0 * num2), rect.Right, (float)((double)rect.Y + 30.0 * num2));
					rect.Y += num3 * 2f;
					rect.Height -= num3 * 2f;
					if (rect.Height < 0f)
					{
						return;
					}
				}
				List<StiTreeItem> list = StiTreeViewHelper.FetchItems(dataTable, element.ShowBlanks, 10);
				if (list == null)
				{
					return;
				}
				foreach (StiTreeItem item in list)
				{
					string text = Format(element, (item.Key != null) ? item.Key : "");
					RectangleF rect3 = new RectangleF(rect.X + num3, rect.Y, rect.Width - num3, num3 * 2f);
					PaintItem(g, rect3, element, text, val2, foreColor, val, (float)num2, item.Items != null && item.Items.Count > 0);
					rect.Y += num3 * 2f;
					rect.Height -= num3 * 2f;
					if (rect.Height < 0f)
					{
						break;
					}
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

	private void PaintItem(Graphics g, RectangleF rect, IStiItemElement element, string text, Font font, Color color, StringFormat sf, float scale, bool drawExpanded = false)
	{
		float num = 16f * scale;
		RectangleF rect2 = new RectangleF(rect.X + 2f * scale, rect.Y + scale, rect.Width - 4f * scale, 30f * scale);
		if (element.SelectionMode == StiItemSelectionMode.Multi)
		{
			rect2.X += num + 6f * scale;
			rect2.Width -= num + 6f * scale;
		}
		StiDrawing.DrawString(g, text, font, color, rect2, sf);
		Color glyphColor = GetGlyphColor(element as IStiControlElement);
		if (element.SelectionMode == StiItemSelectionMode.Multi)
		{
			RectangleD rectD = new RectangleD(rect.X + 4f * scale, rect.Y + (30f * scale - num) / 2f, num, num);
			StiCheckPainter.PaintCheckBox(g, rectD, StiCheckState.Checked, glyphColor, scale);
		}
		if (drawExpanded)
		{
			PaintExpandedState(g, rect, scale, glyphColor);
		}
	}

	private void PaintExpandedState(Graphics g, RectangleF rect, float scale, Color glyphColor)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		int checkSize = StiCheckPainter.GetCheckSize();
		RectangleF rectangleF = new RectangleF(rect.X - (float)checkSize - (float)StiScale.I2, rect.Y + (rect.Height - (float)checkSize) / 2f, checkSize, checkSize);
		float num = rectangleF.X + rectangleF.Width / 2f;
		float num2 = rectangleF.Y + rectangleF.Height / 2f;
		GraphicsPath val = new GraphicsPath();
		try
		{
			SolidBrush val2 = new SolidBrush(glyphColor);
			try
			{
				float num3 = scale * 2f;
				float num4 = scale * 4f;
				val.AddPolygon(new PointF[3]
				{
					new PointF(num - num3, num2 - num4),
					new PointF(num + num3, num2),
					new PointF(num - num3, num2 + num4)
				});
				SmoothingMode smoothingMode = g.SmoothingMode;
				g.SmoothingMode = (SmoothingMode)4;
				g.FillPath((Brush)(object)val2, val);
				g.SmoothingMode = smoothingMode;
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
}

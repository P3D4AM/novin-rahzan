using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiContainerGdiPainter : StiComponentGdiPainter
{
	internal const string TopmostToken = "#drawtopmost";

	public virtual void PaintColumns(StiContainer container, Graphics g)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		if (!(container is StiPanel stiPanel) || !container.IsDesigning || stiPanel.Columns < 2)
		{
			return;
		}
		double num = container.Page.Zoom * StiScale.Factor;
		RectangleD paintRectangle = container.GetPaintRectangle();
		double num2 = container.Page.Unit.ConvertToHInches(stiPanel.GetColumnWidth()) * num;
		double num3 = container.Page.Unit.ConvertToHInches(stiPanel.ColumnGaps) * num;
		double num4 = num2 + ((RectangleD)(ref paintRectangle)).Left;
		Pen val = new Pen(Color.Red);
		try
		{
			val.DashStyle = (DashStyle)1;
			int num5 = stiPanel.Columns;
			if (stiPanel.ColumnWidth == 0.0 && stiPanel.ColumnGaps == 0.0)
			{
				num5--;
			}
			for (int i = 1; i < num5; i++)
			{
				g.DrawLine(val, (float)num4, (float)((RectangleD)(ref paintRectangle)).Top, (float)num4, (float)(((RectangleD)(ref paintRectangle)).Top + ((RectangleD)(ref paintRectangle)).Height));
				g.DrawLine(val, (float)(num4 + num3), (float)((RectangleD)(ref paintRectangle)).Top, (float)(num4 + num3), (float)(((RectangleD)(ref paintRectangle)).Top + ((RectangleD)(ref paintRectangle)).Height));
				num4 += num2 + num3;
			}
			g.DrawLine(val, (float)num4, (float)((RectangleD)(ref paintRectangle)).Top, (float)num4, (float)(((RectangleD)(ref paintRectangle)).Top + ((RectangleD)(ref paintRectangle)).Height));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		((RectangleD)(ref paintRectangle)).X = ((RectangleD)(ref paintRectangle)).X + (num2 + num3);
		StiComponentsCollection components = container.GetComponents();
		foreach (StiComponent item in components)
		{
			RectangleD paintRectangle2 = item.GetPaintRectangle();
			if (!(((RectangleD)(ref paintRectangle2)).Width > 0.0) || !(((RectangleD)(ref paintRectangle2)).Height > 0.0))
			{
				continue;
			}
			Pen val2 = new Pen(Color.Blue, 1f);
			try
			{
				val2.DashStyle = (DashStyle)1;
				for (int j = 0; j < stiPanel.Columns - 1; j++)
				{
					((RectangleD)(ref paintRectangle2)).X = ((RectangleD)(ref paintRectangle2)).X + (num2 + num3);
					StiDrawing.FillRectangle(g, Color.FromArgb(20, Color.Blue), paintRectangle2);
					StiDrawing.DrawRectangle(g, val2, paintRectangle2);
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}

	public virtual void PaintBandInteraction(StiComponent component, Graphics g)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (component is StiContainer && !(component.Report.Info.Zoom < 0.5) && component.Report.Info.ShowInteractive && (int)component.Report.EngineVersion != 1)
		{
			StiContainer masterDataBand = component as StiContainer;
			if (((IStiInteraction)component).Interaction is StiBandInteraction && ((StiBandInteraction)((IStiInteraction)component).Interaction).CollapsingEnabled)
			{
				RectangleD paintRectangle = component.GetPaintRectangle();
				RectangleF rectangleF = ((RectangleD)(ref paintRectangle)).ToRectangleF();
				Image val = (StiDataBandV2Builder.IsCollapsed(masterDataBand, isRendering: false) ? StiDataBuilder.ImageCollapsed : StiDataBuilder.ImageExpanded);
				int num = (StiOptions.Viewer.Pins.EventsRightToLeft ? ((int)rectangleF.Right - StiScale.I3 - val.Width) : ((int)rectangleF.Left + StiScale.I3));
				g.DrawImage(val, num, (int)rectangleF.Y + StiScale.I3, val.Width, val.Height);
			}
		}
	}

	public virtual void PaintComponents(StiContainer container, StiPaintEventArgs e)
	{
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		if (!e.DrawChilds)
		{
			return;
		}
		if (container.Page.IsDashboard)
		{
			foreach (StiComponent component2 in container.Components)
			{
				if (!component2.IsSelected)
				{
					component2.Paint(e);
				}
			}
			foreach (StiComponent component3 in container.Components)
			{
				if (component3.IsSelected)
				{
					component3.Paint(e);
				}
			}
		}
		else if (container.Page.IsForm)
		{
			foreach (StiComponent component4 in container.Components)
			{
				component4.Paint(e);
			}
		}
		else if (container.IsPrinting)
		{
			StiPaintEventArgs stiPaintEventArgs = e.Clone() as StiPaintEventArgs;
			List<StiComponent> list = new List<StiComponent>();
			stiPaintEventArgs.DrawBorderFormatting = true;
			stiPaintEventArgs.DrawTopmostBorderSides = false;
			foreach (StiComponent component5 in container.Components)
			{
				if (!(component5 is StiPrimitive) && !(component5 is StiBand) && component5.Printable)
				{
					if (component5.TagValue != null && component5.TagValue.ToString().ToLowerInvariant() == "#drawtopmost")
					{
						list.Add(component5);
					}
					else
					{
						component5.Paint(stiPaintEventArgs);
					}
				}
			}
			foreach (StiComponent component6 in container.Components)
			{
				if (component6.Printable && component6 is StiBand)
				{
					component6.Paint(stiPaintEventArgs);
				}
			}
			stiPaintEventArgs.DrawBorderFormatting = false;
			stiPaintEventArgs.DrawTopmostBorderSides = true;
			foreach (StiComponent component7 in container.Components)
			{
				if (!(component7 is StiPrimitive) && !(component7 is StiBand) && component7.Printable)
				{
					component7.Paint(stiPaintEventArgs);
				}
			}
			foreach (StiComponent component8 in container.Components)
			{
				if (component8.Printable && component8 is StiBand)
				{
					component8.Paint(stiPaintEventArgs);
				}
			}
			stiPaintEventArgs.DrawBorderFormatting = true;
			stiPaintEventArgs.DrawTopmostBorderSides = true;
			foreach (StiComponent component9 in container.Components)
			{
				if (component9.Printable && component9 is StiPrimitive)
				{
					component9.Paint(stiPaintEventArgs);
				}
			}
			foreach (StiComponent item in list)
			{
				item.Paint(stiPaintEventArgs);
			}
			list.Clear();
		}
		else
		{
			StiPaintEventArgs stiPaintEventArgs2 = e.Clone() as StiPaintEventArgs;
			bool isDesigning = container.IsDesigning;
			RectangleD offsetRectangle = container.Page.OffsetRectangle;
			bool flag = !((RectangleD)(ref offsetRectangle)).IsEmpty;
			List<StiComponent> list2 = new List<StiComponent>();
			stiPaintEventArgs2.DrawBorderFormatting = true;
			stiPaintEventArgs2.DrawTopmostBorderSides = false;
			stiPaintEventArgs2.AnimationEngine = base.AnimationEngine;
			foreach (StiComponent component10 in container.Components)
			{
				if (component10 is StiText { ExceedMargins: not StiExceedMargins.None })
				{
					RectangleD paintRectangle = component10.GetPaintRectangle(convertToHInches: true, convertByZoom: true);
					if (component10 is StiText)
					{
						new StiTextGdiPainter().PaintBackground(component10 as StiText, stiPaintEventArgs2.Graphics, paintRectangle, paintExtendedMargins: true);
					}
				}
			}
			foreach (StiComponent component11 in container.Components)
			{
				if (!(component11 is StiPrimitive) && !(component11 is StiBand))
				{
					if (component11.IsSelected && isDesigning && flag)
					{
						container.Page.SelectedComponents.Add(component11);
					}
					else if (component11.TagValue != null && component11.TagValue.ToString().ToLowerInvariant() == "#drawtopmost")
					{
						list2.Add(component11);
					}
					else
					{
						component11.Paint(stiPaintEventArgs2);
					}
				}
			}
			foreach (StiComponent component12 in container.Components)
			{
				if (component12 is StiBand)
				{
					if (component12.IsSelected && isDesigning && flag)
					{
						container.Page.SelectedComponents.Add(component12);
					}
					else
					{
						component12.Paint(stiPaintEventArgs2);
					}
				}
			}
			stiPaintEventArgs2.DrawBorderFormatting = false;
			stiPaintEventArgs2.DrawTopmostBorderSides = true;
			foreach (StiComponent component13 in container.Components)
			{
				if (!(component13 is StiPrimitive) && !(component13 is StiBand) && !(component13.IsSelected && isDesigning && flag))
				{
					component13.Paint(stiPaintEventArgs2);
				}
			}
			foreach (StiComponent component14 in container.Components)
			{
				if (component14 is StiBand && !(component14.IsSelected && isDesigning && flag))
				{
					component14.Paint(stiPaintEventArgs2);
				}
			}
			stiPaintEventArgs2.DrawBorderFormatting = true;
			stiPaintEventArgs2.DrawTopmostBorderSides = true;
			foreach (StiComponent component15 in container.Components)
			{
				if (component15 is StiPrimitive)
				{
					if (component15.IsSelected && isDesigning && flag)
					{
						container.Page.SelectedComponents.Add(component15);
					}
					else
					{
						component15.Paint(stiPaintEventArgs2);
					}
				}
			}
			foreach (StiComponent item2 in list2)
			{
				item2.Paint(stiPaintEventArgs2);
			}
			list2.Clear();
			if (!container.IsDesigning)
			{
				foreach (StiComponent component16 in container.Components)
				{
					PaintBandInteraction(component16, e.Graphics);
				}
			}
		}
		PaintSelectionComponents(container, e.Graphics);
	}

	public virtual void PaintSelectionComponents(StiContainer container, Graphics g)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (!container.IsDesigning || container.Report.Info.IsComponentsMoving)
		{
			return;
		}
		if (!StiTableHelper.IsTableMode(container.Report.Designer))
		{
			foreach (StiComponent component in container.Components)
			{
				if (component is IStiElement)
				{
					RectangleD clientRectangle = component.ClientRectangle;
					if (((RectangleD)(ref clientRectangle)).IsEmpty)
					{
						continue;
					}
				}
				component.PaintSelection(new StiPaintEventArgs(g, RectangleD.Empty));
			}
			return;
		}
		RectangleD selectedRectangle = StiTableHelper.GetSelectedRectangle(container.Report.Designer, isPaintRect: true);
		if (((RectangleD)(ref selectedRectangle)).IsEmpty && container.Report.GetCurrentPage().IsDashboard)
		{
			StiDrawing.DrawSelectedRectangle(g, 2, Brushes.DimGray, selectedRectangle);
		}
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		StiContainer stiContainer = component as StiContainer;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiContainer.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			component.InvokePainting(component, e);
		}
		object tagValue = component.TagValue;
		StiAdvancedWatermark val = (StiAdvancedWatermark)((tagValue is StiAdvancedWatermark) ? tagValue : null);
		if (val != null)
		{
			RectangleD paintRectangle = component.GetPaintRectangle();
			double num = component.Page.Zoom * StiScale.Factor;
			StiAdvancedWatermarkGdiPainter.PaintWatermark(e.Graphics, val, (StiCornerRadius)null, paintRectangle, num);
		}
		if (!e.Cancel && (component.Enabled || component.IsDesigning))
		{
			RectangleD paintRectangle2 = component.GetPaintRectangle();
			RectangleD clipRectangle = e.ClipRectangle;
			if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle2)).IntersectsWith(e.ClipRectangle))
			{
				PaintContainerContent(e, paintRectangle2, stiContainer);
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			component.InvokePainted(component, e);
			PaintColumns(component as StiContainer, e.Graphics);
		}
		PaintComponents(component as StiContainer, e);
	}

	internal void PaintContainerContent(StiPaintEventArgs e, RectangleD rect, StiContainer container)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Invalid comparison between Unknown and I4
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		if (!(((RectangleD)(ref rect)).Width > 0.0) || !(((RectangleD)(ref rect)).Height > 0.0))
		{
			return;
		}
		Graphics graphics = e.Graphics;
		if (e.DrawBorderFormatting)
		{
			if (StiBrush.IsTransparent(container.Brush) && container.Report != null && container.Report.Info.FillContainer && container.IsDesigning)
			{
				Color color = Color.FromArgb(150, Color.White);
				StiDrawing.FillRectangle(graphics, color, ((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
			}
			else
			{
				StiDrawing.FillRectangle(graphics, container.Brush, rect);
			}
		}
		if (e.DrawBorderFormatting && container.IsDesigning)
		{
			StringFormat val = new StringFormat();
			try
			{
				Font val2 = new Font("Segoe UI", (float)(15.0 * container.Page.Zoom));
				try
				{
					SolidBrush val3 = new SolidBrush(Color.FromArgb(146, 146, 146));
					try
					{
						val.LineAlignment = (StringAlignment)1;
						val.Alignment = (StringAlignment)1;
						StiTextDrawing.DrawString(graphics, container.Name, val2, (Brush)(object)val3, new RectangleD(((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height), val);
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
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (e.DrawBorderFormatting && container.HighlightState == StiHighlightState.Hide && (int)container.Border.Side != 15)
		{
			PaintMarkers(container, graphics, rect);
		}
		if (e.DrawBorderFormatting && (int)container.Border.Side == 0 && container.IsDesigning)
		{
			Pen val4 = new Pen(Color.FromArgb(128, 128, 128));
			try
			{
				val4.DashStyle = (DashStyle)1;
				StiDrawing.DrawRectangle(graphics, val4, ((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		if (container.HighlightState == StiHighlightState.Hide)
		{
			double zoom = ((container.Page != null) ? container.Page.Zoom : 1.0);
			PaintBorder(container, graphics, rect, zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !container.Border.Topmost);
		}
		if (e.DrawBorderFormatting)
		{
			PaintEvents(container, graphics, rect);
			PaintConditions(container, graphics, rect);
		}
	}
}

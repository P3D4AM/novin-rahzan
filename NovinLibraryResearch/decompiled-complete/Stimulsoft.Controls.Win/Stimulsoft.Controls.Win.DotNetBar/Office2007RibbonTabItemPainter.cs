using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007RibbonTabItemPainter : Office2007ButtonItemPainter
{
	protected override Rectangle GetTextRectangle(ButtonItem button, ItemPaintArgs pa, eTextFormat stringFormat, CompositeImage image)
	{
		Rectangle textRectangle = base.GetTextRectangle(button, pa, stringFormat, image);
		if (image == null)
		{
			textRectangle.Inflate(-3, 0);
		}
		return textRectangle;
	}

	public override eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		eTextFormat stringFormat = base.GetStringFormat(button, pa, image);
		return stringFormat & ~(stringFormat & eTextFormat.EndEllipsis);
	}

	protected override void PaintState(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r, bool isMouseDown)
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Expected O, but got Unknown
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		if (r.IsEmpty || !ButtonItemPainter.IsItemEnabled(button, pa) || r.Width == 0 || r.Height == 0)
		{
			return;
		}
		if (!(button is RibbonTabItem ribbonTabItem) || IsOnMenu(button, pa))
		{
			base.PaintState(button, pa, image, r, isMouseDown);
			return;
		}
		bool isOnMenu = pa.IsOnMenu;
		Office2007RibbonTabItemColorTable colorTable = GetColorTable(ribbonTabItem);
		if (colorTable == null)
		{
			return;
		}
		bool controlExpanded = pa.ControlExpanded;
		Office2007RibbonTabItemStateColorTable stateColorTable = GetStateColorTable(colorTable, ribbonTabItem, controlExpanded);
		if (stateColorTable == null)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		int cornerSize = colorTable.CornerSize;
		Region clip = graphics.Clip;
		try
		{
			Rectangle rectangle = r;
			rectangle.Inflate(1, 0);
			graphics.SetClip(rectangle, (CombineMode)0);
			if (stateColorTable != null)
			{
				eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
				GraphicsPath tabPath = GetTabPath(r, cornerSize, getBottomPart: true, effectiveStyle);
				try
				{
					DisplayHelp.FillPath(graphics, tabPath, stateColorTable.Background);
					DisplayHelp.DrawGradientPathBorder(graphics, tabPath, stateColorTable.OuterBorder, 1);
				}
				finally
				{
					((IDisposable)tabPath)?.Dispose();
				}
				if (effectiveStyle != eDotNetBarStyle.Metro && ribbonTabItem.Checked && controlExpanded && ribbonTabItem.RenderTabState)
				{
					SmoothingMode smoothingMode = graphics.SmoothingMode;
					graphics.SmoothingMode = (SmoothingMode)0;
					if ((double)base.ColorTable.RibbonControl.TabsBackground.Start.GetBrightness() > 0.5 && !stateColorTable.OuterBorder.IsEmpty)
					{
						GraphicsPath val = new GraphicsPath();
						try
						{
							val.AddRectangle(new Rectangle(r.Right - 1, r.Y + cornerSize + 1, 1, r.Height - cornerSize - 3));
							DisplayHelp.FillPath(graphics, val, Color.FromArgb(96, stateColorTable.OuterBorder.Start), Color.FromArgb(32, stateColorTable.OuterBorder.End), 90);
						}
						finally
						{
							((IDisposable)val)?.Dispose();
						}
						GraphicsPath val2 = new GraphicsPath();
						try
						{
							val2.AddRectangle(new Rectangle(r.X + 1, r.Y + cornerSize + 1, 1, r.Height - cornerSize - 3));
							DisplayHelp.FillPath(graphics, val2, Color.FromArgb(32, stateColorTable.OuterBorder.Start), Color.FromArgb(8, stateColorTable.OuterBorder.End), 90);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
					graphics.SmoothingMode = smoothingMode;
				}
				Rectangle r2 = r;
				r2.Inflate(-1, 0);
				r2.Height--;
				r2.Y++;
				GraphicsPath tabPath2 = GetTabPath(r2, cornerSize, getBottomPart: true, effectiveStyle);
				try
				{
					DisplayHelp.DrawGradientPathBorder(graphics, tabPath2, stateColorTable.InnerBorder, 1);
				}
				finally
				{
					((IDisposable)tabPath2)?.Dispose();
				}
				if (ribbonTabItem.Checked && controlExpanded && ribbonTabItem.RenderTabState)
				{
					SolidBrush val3 = new SolidBrush(stateColorTable.InnerBorder.Start);
					try
					{
						SmoothingMode smoothingMode2 = graphics.SmoothingMode;
						graphics.SmoothingMode = (SmoothingMode)3;
						graphics.FillRectangle((Brush)(object)val3, new Rectangle(r2.X + cornerSize, r2.Y + 1, r2.Width - cornerSize * 2, 2));
						graphics.SmoothingMode = smoothingMode2;
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				float num = 0.6f;
				float num2 = 0.4f;
				Rectangle rectangle2 = r;
				Rectangle rectangle3 = new Rectangle(rectangle2.X, rectangle2.Y + (int)((float)rectangle2.Height * num), rectangle2.Width, (int)((float)rectangle2.Height * num2));
				if (!stateColorTable.BackgroundHighlight.IsEmpty)
				{
					Rectangle rectangle4 = new Rectangle(rectangle3.X, rectangle3.Y, rectangle2.Width, rectangle2.Height);
					GraphicsPath val4 = new GraphicsPath();
					val4.AddEllipse(rectangle4);
					PathGradientBrush val5 = new PathGradientBrush(val4);
					val5.CenterColor = stateColorTable.BackgroundHighlight.Start;
					val5.SurroundColors = new Color[1] { stateColorTable.BackgroundHighlight.End };
					val5.CenterPoint = new PointF(rectangle4.X + rectangle4.Width / 2, rectangle2.Bottom + 2);
					Blend val6 = new Blend();
					val6.Factors = new float[3] { 0f, 0.8f, 1f };
					val6.Positions = new float[3] { 0f, 0.55f, 1f };
					val5.Blend = val6;
					graphics.FillRectangle((Brush)(object)val5, rectangle3);
					((Brush)val5).Dispose();
					val4.Dispose();
				}
			}
			if (ribbonTabItem.ReducedSize && !ribbonTabItem.Checked && !ribbonTabItem.IsMouseOver && colorTable != null && colorTable.Selected != null)
			{
				Color start = base.ColorTable.RibbonControl.OuterBorder.Start;
				if (!start.IsEmpty)
				{
					DisplayHelp.DrawGradientLine(graphics, new Point(r.Right - 1, r.Y), new Point(r.Right - 1, r.Bottom - 1), Color.Transparent, start, 90, 1, new float[3] { 0f, 0.8f, 1f }, new float[3] { 0f, 0.5f, 1f });
				}
			}
			graphics.Clip = clip;
		}
		finally
		{
			if (clip != null)
			{
				clip.Dispose();
			}
		}
	}

	private GraphicsPath GetTabPath(Rectangle r, int cornerSize, bool getBottomPart, eDotNetBarStyle style)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		int num = ((style == eDotNetBarStyle.Windows7 || style == eDotNetBarStyle.Office2010) ? 1 : 2);
		if (style == eDotNetBarStyle.Metro)
		{
			num = 0;
		}
		if (getBottomPart)
		{
			val.AddLine(r.X, r.Bottom, r.X + num, r.Bottom - num);
		}
		else
		{
			val.AddLine(r.X, r.Bottom, r.X, r.Y + cornerSize);
		}
		r.Inflate(-num, 0);
		ElementStyleDisplay.AddCornerArc(val, r, cornerSize, eCornerArc.TopLeft);
		ElementStyleDisplay.AddCornerArc(val, r, cornerSize, eCornerArc.TopRight);
		if (getBottomPart)
		{
			val.AddLine(r.Right, r.Bottom - num, r.Right + num, r.Bottom);
		}
		else
		{
			val.AddLine(r.Right, r.Y + cornerSize, r.Right, r.Bottom);
		}
		return val;
	}

	protected override Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
	{
		if (!ButtonItemPainter.IsItemEnabled(button, pa) || !(button is RibbonTabItem))
		{
			return base.GetTextColor(button, pa);
		}
		RibbonTabItem tab = button as RibbonTabItem;
		Color result = Color.Empty;
		Office2007RibbonTabItemStateColorTable stateColorTable = GetStateColorTable(GetColorTable(tab), tab, pa.ControlExpanded);
		if (stateColorTable != null)
		{
			if (pa.GlassEnabled && !stateColorTable.GlassText.IsEmpty)
			{
				return stateColorTable.GlassText;
			}
			result = stateColorTable.Text;
		}
		if (result.IsEmpty)
		{
			return base.GetTextColor(button, pa);
		}
		return result;
	}

	private Office2007RibbonTabItemColorTable GetColorTable(RibbonTabItem tab)
	{
		return GetColorTable(base.ColorTable, tab);
	}

	internal static Office2007RibbonTabItemColorTable GetColorTable(Office2007ColorTable table, RibbonTabItem tab)
	{
		if (table == null)
		{
			return null;
		}
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = table.RibbonTabItemColors[tab.GetColorTableName()];
		if (office2007RibbonTabItemColorTable == null && table.RibbonTabItemColors.Count > 0)
		{
			office2007RibbonTabItemColorTable = table.RibbonTabItemColors[0];
		}
		return office2007RibbonTabItemColorTable;
	}

	internal static Office2007RibbonTabItemColorTable GetColorTable(Office2007ColorTable table, ButtonItem button)
	{
		if (table == null)
		{
			return null;
		}
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = table.RibbonTabItemColors[button.CustomColorName];
		if (office2007RibbonTabItemColorTable == null && table.RibbonTabItemColors.Count > 0)
		{
			office2007RibbonTabItemColorTable = table.RibbonTabItemColors[0];
		}
		return office2007RibbonTabItemColorTable;
	}

	internal static Office2007RibbonTabItemColorTable GetColorTable(Office2007ColorTable table)
	{
		if (table == null)
		{
			return null;
		}
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = table.RibbonTabItemColors["Default"];
		if (office2007RibbonTabItemColorTable == null && table.RibbonTabItemColors.Count > 0)
		{
			office2007RibbonTabItemColorTable = table.RibbonTabItemColors[0];
		}
		return office2007RibbonTabItemColorTable;
	}

	internal static Office2007RibbonTabItemStateColorTable GetStateColorTable(Office2007RibbonTabItemColorTable tabColorTable, RibbonTabItem tab, bool ribbonExpanded)
	{
		if (tabColorTable == null)
		{
			return null;
		}
		Office2007RibbonTabItemStateColorTable result = null;
		if (!tab.RenderTabState)
		{
			result = tabColorTable.Default;
		}
		else if (tab.Checked && tab.IsMouseOver && ribbonExpanded)
		{
			result = tabColorTable.SelectedMouseOver;
		}
		else if (tab.Checked && ribbonExpanded)
		{
			result = tabColorTable.Selected;
		}
		else if (tab.IsMouseOver)
		{
			result = tabColorTable.MouseOver;
		}
		else if (tab.GetEnabled())
		{
			result = tabColorTable.Default;
		}
		return result;
	}

	public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, CompositeImage image)
	{
		eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
		if ((effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Metro) || !pa.GlassEnabled)
		{
			base.PaintButtonText(button, pa, textColor, image);
			return;
		}
		Rectangle textRectangle = GetTextRectangle(button, pa, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter, image);
		ThemeTextFormat tf = (ThemeTextFormat)1048613;
		bool renderGlow = true;
		Office2007RibbonControlPainter.PaintTextOnGlass(pa.Graphics, button.Text, pa.Font, textRectangle, tf, textColor, copySourceBackground: true, renderGlow, 10);
	}
}

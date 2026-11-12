using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class RibbonPredefinedColorSchemes
{
	public static void ApplyGrayColorScheme(RibbonBar b)
	{
		b.BackgroundStyle.Reset();
		b.BackgroundMouseOverStyle.Reset();
		b.TitleStyle.Reset();
		b.TitleStyleMouseOver.Reset();
		TypeDescriptor.GetProperties(b.BackgroundStyle)["BackColor"].SetValue(b.BackgroundStyle, ColorScheme.GetColor("CFD5E2"));
		TypeDescriptor.GetProperties(b.BackgroundStyle)["BackColor2"].SetValue(b.BackgroundStyle, ColorScheme.GetColor("C2C9D9"));
		TypeDescriptor.GetProperties(b.BackgroundStyle)["BackColorGradientAngle"].SetValue(b.BackgroundStyle, 90);
		TypeDescriptor.GetProperties(b.BackgroundMouseOverStyle)["BackColor"].SetValue(b.BackgroundMouseOverStyle, ColorScheme.GetColor("EFEFEF"));
		TypeDescriptor.GetProperties(b.BackgroundMouseOverStyle)["BackColor2"].SetValue(b.BackgroundMouseOverStyle, ColorScheme.GetColor("D2DAED"));
		TypeDescriptor.GetProperties(b.BackgroundMouseOverStyle)["BackColorGradientAngle"].SetValue(b.BackgroundMouseOverStyle, 90);
		TypeDescriptor.GetProperties(b.TitleStyle)["BackColor"].SetValue(b.TitleStyle, ColorScheme.GetColor("6A7080"));
		TypeDescriptor.GetProperties(b.TitleStyle)["BackColor2"].SetValue(b.TitleStyle, ColorScheme.GetColor("A8B2CC"));
		TypeDescriptor.GetProperties(b.TitleStyle)["BackColorGradientAngle"].SetValue(b.TitleStyle, 90);
		TypeDescriptor.GetProperties(b.TitleStyle)["TextColor"].SetValue(b.TitleStyle, Color.White);
		TypeDescriptor.GetProperties(b.TitleStyle)["PaddingTop"].SetValue(b.TitleStyle, 3);
		TypeDescriptor.GetProperties(b.TitleStyle)["PaddingBottom"].SetValue(b.TitleStyle, 2);
		TypeDescriptor.GetProperties(b.TitleStyle)["PaddingLeft"].SetValue(b.TitleStyle, 2);
		TypeDescriptor.GetProperties(b.TitleStyle)["PaddingRight"].SetValue(b.TitleStyle, 2);
		TypeDescriptor.GetProperties(b.TitleStyle)["TextShadowColor"].SetValue(b.TitleStyle, Color.Black);
		TypeDescriptor.GetProperties(b.TitleStyle)["TextShadowOffset"].SetValue(b.TitleStyle, new Point(1, 1));
		TypeDescriptor.GetProperties(b.TitleStyle)["Border"].SetValue(b.TitleStyle, eStyleBorderType.None);
		TypeDescriptor.GetProperties(b.TitleStyleMouseOver)["BackColor"].SetValue(b.TitleStyleMouseOver, ColorScheme.GetColor("53607D"));
		TypeDescriptor.GetProperties(b.TitleStyleMouseOver)["BackColor2"].SetValue(b.TitleStyleMouseOver, ColorScheme.GetColor("8BA1CA"));
		((Control)b).Invalidate();
	}

	public static void ApplyOrangeColorScheme(RibbonBar b)
	{
		b.BackgroundStyle.Reset();
		b.BackgroundMouseOverStyle.Reset();
		b.TitleStyle.Reset();
		b.TitleStyleMouseOver.Reset();
		TypeDescriptor.GetProperties(b.BackgroundStyle)["BackColor"].SetValue(b.BackgroundStyle, ColorScheme.GetColor("CFD5E2"));
		TypeDescriptor.GetProperties(b.BackgroundStyle)["BackColor2"].SetValue(b.BackgroundStyle, ColorScheme.GetColor("C2C9D9"));
		TypeDescriptor.GetProperties(b.BackgroundStyle)["BackColorGradientAngle"].SetValue(b.BackgroundStyle, 90);
		TypeDescriptor.GetProperties(b.TitleStyle)["BackColor"].SetValue(b.TitleStyle, ColorScheme.GetColor("CBA950"));
		TypeDescriptor.GetProperties(b.TitleStyle)["BackColor2"].SetValue(b.TitleStyle, ColorScheme.GetColor("FED665"));
		TypeDescriptor.GetProperties(b.TitleStyle)["BackColorGradientAngle"].SetValue(b.TitleStyle, 90);
		TypeDescriptor.GetProperties(b.TitleStyle)["TextColor"].SetValue(b.TitleStyle, Color.White);
		TypeDescriptor.GetProperties(b.TitleStyle)["PaddingTop"].SetValue(b.TitleStyle, 3);
		TypeDescriptor.GetProperties(b.TitleStyle)["PaddingBottom"].SetValue(b.TitleStyle, 2);
		TypeDescriptor.GetProperties(b.TitleStyle)["PaddingLeft"].SetValue(b.TitleStyle, 2);
		TypeDescriptor.GetProperties(b.TitleStyle)["PaddingRight"].SetValue(b.TitleStyle, 2);
		TypeDescriptor.GetProperties(b.TitleStyle)["TextShadowColor"].SetValue(b.TitleStyle, Color.Black);
		TypeDescriptor.GetProperties(b.TitleStyle)["TextShadowOffset"].SetValue(b.TitleStyle, new Point(1, 1));
		TypeDescriptor.GetProperties(b.TitleStyle)["Border"].SetValue(b.TitleStyle, eStyleBorderType.None);
		((Control)b).Invalidate();
	}

	public static void ApplyOffice2003ColorScheme(RibbonBar b)
	{
		((Control)b).Invalidate();
	}

	internal static void ApplyRibbonBarOffice2003ElementStyle(ElementStyle backStyle, ElementStyle backMouseOverStyle, ElementStyle titleStyle, ElementStyle titleMouseOverStyle)
	{
		backStyle.Reset();
		backMouseOverStyle.Reset();
		titleStyle.Reset();
		titleMouseOverStyle.Reset();
		backStyle.BackColorSchemePart = eColorSchemePart.BarBackground2;
		backStyle.BackColor2SchemePart = eColorSchemePart.BarBackground;
		backStyle.BackColorGradientAngle = 90;
		titleStyle.BackColorSchemePart = eColorSchemePart.PanelBackground;
		titleStyle.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		titleStyle.BackColorGradientAngle = 90;
		titleStyle.TextColorSchemePart = eColorSchemePart.PanelText;
		titleStyle.PaddingTop = 1;
		titleStyle.PaddingBottom = 1;
		titleStyle.PaddingLeft = 2;
		titleStyle.PaddingRight = 2;
		titleStyle.TextShadowColor = Color.Black;
		titleStyle.TextShadowOffset = new Point(1, 1);
		titleStyle.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		titleStyle.Border = eStyleBorderType.Solid;
		titleStyle.BorderWidth = 1;
	}

	public static void ApplyOffice2003ColorScheme(RibbonControl b)
	{
		ApplyRibbonOffice2003ElementStyle(b.BackgroundStyle);
		((Control)b).Invalidate(true);
	}

	public static void ApplyGrayColorScheme(RibbonControl b)
	{
		b.BackgroundStyle.Reset();
		TypeDescriptor.GetProperties(b.BackgroundStyle)["BackColor"].SetValue(b.BackgroundStyle, ColorScheme.GetColor("F4F4F4"));
		TypeDescriptor.GetProperties(b.BackgroundStyle)["BackColor2"].SetValue(b.BackgroundStyle, ColorScheme.GetColor("CFCFCF"));
		TypeDescriptor.GetProperties(b.BackgroundStyle)["BackColorGradientAngle"].SetValue(b.BackgroundStyle, 0);
		TypeDescriptor.GetProperties(b.BackgroundStyle)["BackColorGradientType"].SetValue(b.BackgroundStyle, eGradientType.Radial);
		((Control)b).Invalidate(true);
	}

	public static void ApplyOffice2007ColorScheme(RibbonControl b)
	{
		((Control)b).Invalidate(true);
	}

	internal static void ApplyRibbonElementStyle(ElementStyle style, RibbonControl rc)
	{
		eDotNetBarStyle eDotNetBarStyle2 = rc.Style;
		if (eDotNetBarStyle2 == eDotNetBarStyle.StyleManagerControlled)
		{
			eDotNetBarStyle2 = StyleManager.GetEffectiveStyle();
		}
		if (rc != null && eDotNetBarStyle2 == eDotNetBarStyle.Office2007)
		{
			ApplyRibbonOffice2007ElementStyle(style, GetOffice2007ColorTable(rc));
		}
		else if (rc != null && (eDotNetBarStyle2 == eDotNetBarStyle.Office2010 || eDotNetBarStyle2 == eDotNetBarStyle.Metro))
		{
			ApplyRibbonOffice2010ElementStyle(style, GetOffice2007ColorTable(rc), rc.RibbonStrip.IsGlassEnabled);
		}
		else if (rc != null && eDotNetBarStyle2 == eDotNetBarStyle.Windows7)
		{
			ApplyRibbonWindows7ElementStyle(style, GetOffice2007ColorTable(rc));
		}
		else
		{
			ApplyRibbonOffice2003ElementStyle(style);
		}
	}

	internal static void ApplyRibbonOffice2007ElementStyle(ElementStyle backStyle, Office2007ColorTable ct)
	{
		backStyle.Reset();
		backStyle.BackColor = ct.RibbonControl.TabsBackground.Start;
		backStyle.BackColor2 = ct.RibbonControl.TabsBackground.End;
		backStyle.BackColorGradientAngle = ct.RibbonControl.TabsBackground.GradientAngle;
		backStyle.BackColorGradientType = eGradientType.Linear;
	}

	internal static void ApplyRibbonOffice2010ElementStyle(ElementStyle backStyle, Office2007ColorTable ct, bool isGlassEnabled)
	{
		backStyle.Reset();
		backStyle.BackColor = ct.RibbonControl.TabsBackground.Start;
		backStyle.BackColor2 = ct.RibbonControl.TabsBackground.End;
		backStyle.BackColorGradientAngle = ct.RibbonControl.TabsBackground.GradientAngle;
		backStyle.BackColorGradientType = eGradientType.Linear;
		if (isGlassEnabled)
		{
			backStyle.BackColor = ct.RibbonControl.TabsGlassBackground.End;
			backStyle.BackColor2 = ct.RibbonControl.TabsGlassBackground.Start;
			backStyle.BackColorGradientAngle = ct.RibbonControl.TabsGlassBackground.GradientAngle;
			backStyle.BackColorGradientType = eGradientType.Radial;
		}
	}

	internal static void ApplyRibbonWindows7ElementStyle(ElementStyle backStyle, Office2007ColorTable ct)
	{
		backStyle.Reset();
		backStyle.BackColor = ct.RibbonControl.TabsBackground.Start;
		backStyle.BackColor2 = ct.RibbonControl.TabsBackground.End;
		backStyle.BackColorGradientAngle = ct.RibbonControl.TabsBackground.GradientAngle;
		backStyle.BackColorGradientType = eGradientType.Linear;
	}

	internal static void ApplyRibbonOffice2003ElementStyle(ElementStyle backStyle)
	{
		backStyle.Reset();
		backStyle.BackColorSchemePart = eColorSchemePart.BarBackground2;
		backStyle.BackColor2SchemePart = eColorSchemePart.BarBackground;
		backStyle.BackColorGradientAngle = 90;
	}

	public static void ApplyOffice2007ColorScheme(RibbonBar b)
	{
		((Control)b).Invalidate();
	}

	internal static Office2007ColorTable GetOffice2007ColorTable(RibbonBar b)
	{
		Office2007ColorTable office2007ColorTable = null;
		if ((b.RenderMode == eRenderMode.Custom || b.RenderMode == eRenderMode.Instance) && b.Renderer != null)
		{
			if (b.Renderer is Office2007Renderer office2007Renderer)
			{
				office2007ColorTable = office2007Renderer.ColorTable;
			}
		}
		else if (GlobalManager.Renderer is Office2007Renderer office2007Renderer2)
		{
			office2007ColorTable = office2007Renderer2.ColorTable;
		}
		if (office2007ColorTable == null)
		{
			office2007ColorTable = new Office2007ColorTable();
		}
		return office2007ColorTable;
	}

	internal static Office2007ColorTable GetOffice2007ColorTable(RibbonControl b)
	{
		Office2007ColorTable office2007ColorTable = null;
		if ((b.RibbonStrip.RenderMode == eRenderMode.Custom || b.RibbonStrip.RenderMode == eRenderMode.Instance) && b.RibbonStrip.Renderer != null)
		{
			if (b.RibbonStrip.Renderer is Office2007Renderer office2007Renderer)
			{
				office2007ColorTable = office2007Renderer.ColorTable;
			}
		}
		else if (GlobalManager.Renderer is Office2007Renderer office2007Renderer2)
		{
			office2007ColorTable = office2007Renderer2.ColorTable;
		}
		if (office2007ColorTable == null)
		{
			office2007ColorTable = new Office2007ColorTable();
		}
		return office2007ColorTable;
	}

	internal static void ApplyRibbonBarElementStyle(ElementStyle backStyle, ElementStyle backMouseOverStyle, ElementStyle titleStyle, ElementStyle titleMouseOverStyle, RibbonBar b)
	{
		if (b.EffectiveStyle == eDotNetBarStyle.Office2007)
		{
			Office2007ColorTable office2007ColorTable = GetOffice2007ColorTable(b);
			ApplyRibbonBarOffice2007ElementStyle(backStyle, backMouseOverStyle, titleStyle, titleMouseOverStyle, office2007ColorTable);
		}
		else if (b.EffectiveStyle == eDotNetBarStyle.Office2010)
		{
			Office2007ColorTable office2007ColorTable2 = GetOffice2007ColorTable(b);
			ApplyRibbonBarOffice2010ElementStyle(backStyle, backMouseOverStyle, titleStyle, titleMouseOverStyle, office2007ColorTable2);
		}
		else if (b.EffectiveStyle == eDotNetBarStyle.Metro)
		{
			Office2007ColorTable office2007ColorTable3 = GetOffice2007ColorTable(b);
			ApplyRibbonBarMetroElementStyle(backStyle, backMouseOverStyle, titleStyle, titleMouseOverStyle, office2007ColorTable3);
		}
		else if (b.EffectiveStyle == eDotNetBarStyle.Windows7)
		{
			Office2007ColorTable office2007ColorTable4 = GetOffice2007ColorTable(b);
			ApplyRibbonBarWindows7ElementStyle(backStyle, backMouseOverStyle, titleStyle, titleMouseOverStyle, office2007ColorTable4);
		}
		else
		{
			ApplyRibbonBarOffice2003ElementStyle(backStyle, backMouseOverStyle, titleStyle, titleMouseOverStyle);
		}
	}

	internal static void ApplyRibbonBarOffice2010ElementStyle(ElementStyle backStyle, ElementStyle backMouseOverStyle, ElementStyle titleStyle, ElementStyle titleMouseOverStyle, Office2007ColorTable c)
	{
		backStyle.Reset();
		backMouseOverStyle.Reset();
		titleStyle.Reset();
		titleMouseOverStyle.Reset();
		backStyle.BorderRight = eStyleBorderType.Etched;
		backStyle.BorderRightWidth = 1;
		backStyle.CornerType = eCornerType.Square;
		backStyle.BorderColor = c.RibbonBar.Default.OuterBorder.Start;
		backStyle.BorderColor2 = c.RibbonBar.Default.OuterBorder.End;
		backStyle.BorderColorLight = c.RibbonBar.Default.InnerBorder.Start;
		backStyle.BorderColorLight2 = c.RibbonBar.Default.InnerBorder.End;
		backStyle.BackColorGradientAngle = 90;
		if (c.RibbonBar.Default.BottomBackground == null)
		{
			backStyle.BackColor = c.RibbonBar.Default.TopBackground.Start;
			backStyle.BackColor2 = c.RibbonBar.Default.TopBackground.End;
		}
		else
		{
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.TopBackground.Start, 0f));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.TopBackground.End, c.RibbonBar.Default.TopBackgroundHeight));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.BottomBackground.Start, c.RibbonBar.Default.TopBackgroundHeight));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.BottomBackground.End, 1f));
		}
		backStyle.PaddingTop = 2;
		backStyle.PaddingLeft = 2;
		backStyle.PaddingRight = 2;
		backMouseOverStyle.BorderColor = c.RibbonBar.MouseOver.OuterBorder.Start;
		backMouseOverStyle.BorderColor2 = c.RibbonBar.MouseOver.OuterBorder.End;
		backMouseOverStyle.BorderColorLight = c.RibbonBar.MouseOver.InnerBorder.Start;
		backMouseOverStyle.BorderColorLight2 = c.RibbonBar.MouseOver.InnerBorder.End;
		backMouseOverStyle.BackColorGradientAngle = 90;
		if (c.RibbonBar.MouseOver.BottomBackground == null)
		{
			backStyle.BackColor = c.RibbonBar.MouseOver.TopBackground.Start;
			backStyle.BackColor2 = c.RibbonBar.MouseOver.TopBackground.End;
		}
		else
		{
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.TopBackground.Start, 0f));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.TopBackground.End, c.RibbonBar.MouseOver.TopBackgroundHeight));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.BottomBackground.Start, c.RibbonBar.MouseOver.TopBackgroundHeight));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.BottomBackground.End, 1f));
		}
		if (c.RibbonBar.Default.TitleBackground != null)
		{
			titleStyle.BackColor = c.RibbonBar.Default.TitleBackground.Start;
			titleStyle.BackColor2 = c.RibbonBar.Default.TitleBackground.End;
			titleStyle.BackColorGradientAngle = 90;
		}
		titleStyle.TextAlignment = eStyleTextAlignment.Center;
		titleStyle.TextColor = c.RibbonBar.Default.TitleText;
		titleStyle.TextShadowOffset = Point.Empty;
		titleStyle.PaddingTop = 1;
		titleStyle.PaddingBottom = 1;
		if (c.RibbonBar.MouseOver.TitleBackground != null)
		{
			titleMouseOverStyle.BackColor = c.RibbonBar.MouseOver.TitleBackground.Start;
			titleMouseOverStyle.BackColor2 = c.RibbonBar.MouseOver.TitleBackground.End;
			titleMouseOverStyle.BackColorGradientAngle = 90;
			titleMouseOverStyle.BackColorGradientType = eGradientType.Radial;
		}
		titleMouseOverStyle.TextAlignment = eStyleTextAlignment.Center;
		titleMouseOverStyle.TextColor = c.RibbonBar.MouseOver.TitleText;
		titleStyle.HideMnemonic = true;
		titleMouseOverStyle.HideMnemonic = true;
	}

	internal static void ApplyRibbonBarMetroElementStyle(ElementStyle backStyle, ElementStyle backMouseOverStyle, ElementStyle titleStyle, ElementStyle titleMouseOverStyle, Office2007ColorTable c)
	{
		backStyle.Reset();
		backMouseOverStyle.Reset();
		titleStyle.Reset();
		titleMouseOverStyle.Reset();
		backStyle.BorderRight = eStyleBorderType.Solid;
		backStyle.BorderRightWidth = 1;
		backStyle.CornerType = eCornerType.Square;
		backStyle.BorderColor = c.RibbonBar.Default.OuterBorder.Start;
		backStyle.BorderColor2 = c.RibbonBar.Default.OuterBorder.End;
		backStyle.BackColorGradientAngle = 90;
		if (c.RibbonBar.Default.BottomBackground == null)
		{
			backStyle.BackColor = c.RibbonBar.Default.TopBackground.Start;
			backStyle.BackColor2 = c.RibbonBar.Default.TopBackground.End;
		}
		else
		{
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.TopBackground.Start, 0f));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.TopBackground.End, c.RibbonBar.Default.TopBackgroundHeight));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.BottomBackground.Start, c.RibbonBar.Default.TopBackgroundHeight));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.BottomBackground.End, 1f));
		}
		backStyle.PaddingTop = 2;
		backStyle.PaddingLeft = 2;
		backStyle.PaddingRight = 2;
		backMouseOverStyle.BorderColor = c.RibbonBar.MouseOver.OuterBorder.Start;
		backMouseOverStyle.BorderColor2 = c.RibbonBar.MouseOver.OuterBorder.End;
		backMouseOverStyle.BackColorGradientAngle = 90;
		if (c.RibbonBar.MouseOver.BottomBackground == null)
		{
			backStyle.BackColor = c.RibbonBar.MouseOver.TopBackground.Start;
			backStyle.BackColor2 = c.RibbonBar.MouseOver.TopBackground.End;
		}
		else
		{
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.TopBackground.Start, 0f));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.TopBackground.End, c.RibbonBar.MouseOver.TopBackgroundHeight));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.BottomBackground.Start, c.RibbonBar.MouseOver.TopBackgroundHeight));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.BottomBackground.End, 1f));
		}
		if (c.RibbonBar.Default.TitleBackground != null)
		{
			titleStyle.BackColor = c.RibbonBar.Default.TitleBackground.Start;
			titleStyle.BackColor2 = c.RibbonBar.Default.TitleBackground.End;
			titleStyle.BackColorGradientAngle = 90;
		}
		titleStyle.TextAlignment = eStyleTextAlignment.Center;
		titleStyle.TextColor = c.RibbonBar.Default.TitleText;
		titleStyle.TextShadowOffset = Point.Empty;
		titleStyle.PaddingTop = 1;
		titleStyle.PaddingBottom = 1;
		if (c.RibbonBar.MouseOver.TitleBackground != null)
		{
			titleMouseOverStyle.BackColor = c.RibbonBar.MouseOver.TitleBackground.Start;
			titleMouseOverStyle.BackColor2 = c.RibbonBar.MouseOver.TitleBackground.End;
			titleMouseOverStyle.BackColorGradientAngle = 90;
			titleMouseOverStyle.BackColorGradientType = eGradientType.Radial;
		}
		titleMouseOverStyle.TextAlignment = eStyleTextAlignment.Center;
		titleMouseOverStyle.TextColor = c.RibbonBar.MouseOver.TitleText;
		titleStyle.HideMnemonic = true;
		titleMouseOverStyle.HideMnemonic = true;
	}

	internal static void ApplyRibbonBarWindows7ElementStyle(ElementStyle backStyle, ElementStyle backMouseOverStyle, ElementStyle titleStyle, ElementStyle titleMouseOverStyle, Office2007ColorTable c)
	{
		backStyle.Reset();
		backMouseOverStyle.Reset();
		titleStyle.Reset();
		titleMouseOverStyle.Reset();
		backStyle.BorderRight = eStyleBorderType.Etched;
		backStyle.BorderRightWidth = 1;
		backStyle.CornerType = eCornerType.Square;
		backStyle.BorderColor = c.RibbonBar.Default.OuterBorder.Start;
		backStyle.BorderColor2 = c.RibbonBar.Default.OuterBorder.End;
		backStyle.BorderColorLight = c.RibbonBar.Default.InnerBorder.Start;
		backStyle.BorderColorLight2 = c.RibbonBar.Default.InnerBorder.End;
		backStyle.BackColorGradientAngle = 90;
		if (c.RibbonBar.Default.BottomBackground == null)
		{
			backStyle.BackColor = c.RibbonBar.Default.TopBackground.Start;
			backStyle.BackColor2 = c.RibbonBar.Default.TopBackground.End;
		}
		else
		{
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.TopBackground.Start, 0f));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.TopBackground.End, c.RibbonBar.Default.TopBackgroundHeight));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.BottomBackground.Start, c.RibbonBar.Default.TopBackgroundHeight));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.BottomBackground.End, 1f));
		}
		backStyle.PaddingTop = 2;
		backStyle.PaddingLeft = 2;
		backStyle.PaddingRight = 2;
		backMouseOverStyle.BorderColor = c.RibbonBar.MouseOver.OuterBorder.Start;
		backMouseOverStyle.BorderColor2 = c.RibbonBar.MouseOver.OuterBorder.End;
		backMouseOverStyle.BorderColorLight = c.RibbonBar.MouseOver.InnerBorder.Start;
		backMouseOverStyle.BorderColorLight2 = c.RibbonBar.MouseOver.InnerBorder.End;
		backMouseOverStyle.BackColorGradientAngle = 90;
		if (c.RibbonBar.MouseOver.BottomBackground == null)
		{
			backStyle.BackColor = c.RibbonBar.MouseOver.TopBackground.Start;
			backStyle.BackColor2 = c.RibbonBar.MouseOver.TopBackground.End;
		}
		else
		{
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.TopBackground.Start, 0f));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.TopBackground.End, c.RibbonBar.MouseOver.TopBackgroundHeight));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.BottomBackground.Start, c.RibbonBar.MouseOver.TopBackgroundHeight));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.BottomBackground.End, 1f));
		}
		if (c.RibbonBar.Default.TitleBackground != null)
		{
			titleStyle.BackColor = c.RibbonBar.Default.TitleBackground.Start;
			titleStyle.BackColor2 = c.RibbonBar.Default.TitleBackground.End;
			titleStyle.BackColorGradientAngle = 90;
		}
		titleStyle.TextAlignment = eStyleTextAlignment.Center;
		titleStyle.TextColor = c.RibbonBar.Default.TitleText;
		titleStyle.TextShadowOffset = Point.Empty;
		titleStyle.PaddingTop = 1;
		titleStyle.PaddingBottom = 1;
		if (c.RibbonBar.MouseOver.TitleBackground != null)
		{
			titleMouseOverStyle.BackColor = c.RibbonBar.MouseOver.TitleBackground.Start;
			titleMouseOverStyle.BackColor2 = c.RibbonBar.MouseOver.TitleBackground.End;
			titleMouseOverStyle.BackColorGradientAngle = 90;
			titleMouseOverStyle.BackColorGradientType = eGradientType.Radial;
		}
		titleMouseOverStyle.TextAlignment = eStyleTextAlignment.Center;
		titleMouseOverStyle.TextColor = c.RibbonBar.MouseOver.TitleText;
		titleStyle.HideMnemonic = true;
		titleMouseOverStyle.HideMnemonic = true;
	}

	internal static void ApplyRibbonBarOffice2007ElementStyle(ElementStyle backStyle, ElementStyle backMouseOverStyle, ElementStyle titleStyle, ElementStyle titleMouseOverStyle, Office2007ColorTable c)
	{
		backStyle.Reset();
		backMouseOverStyle.Reset();
		titleStyle.Reset();
		titleMouseOverStyle.Reset();
		backStyle.Border = eStyleBorderType.Etched;
		backStyle.BorderWidth = 1;
		backStyle.CornerType = eCornerType.Rounded;
		backStyle.CornerDiameter = 2;
		backStyle.BorderColor = c.RibbonBar.Default.OuterBorder.Start;
		backStyle.BorderColor2 = c.RibbonBar.Default.OuterBorder.End;
		backStyle.BorderColorLight = c.RibbonBar.Default.InnerBorder.Start;
		backStyle.BorderColorLight2 = c.RibbonBar.Default.InnerBorder.End;
		backStyle.BackColorGradientAngle = 90;
		if (c.RibbonBar.Default.TopBackground != null && c.RibbonBar.Default.BottomBackground != null)
		{
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.TopBackground.Start, 0f));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.TopBackground.End, c.RibbonBar.Default.TopBackgroundHeight));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.BottomBackground.Start, c.RibbonBar.Default.TopBackgroundHeight));
			backStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.Default.BottomBackground.End, 1f));
		}
		backStyle.PaddingLeft = 2;
		backStyle.PaddingRight = 2;
		backMouseOverStyle.BorderColor = c.RibbonBar.MouseOver.OuterBorder.Start;
		backMouseOverStyle.BorderColor2 = c.RibbonBar.MouseOver.OuterBorder.End;
		backMouseOverStyle.BorderColorLight = c.RibbonBar.MouseOver.InnerBorder.Start;
		backMouseOverStyle.BorderColorLight2 = c.RibbonBar.MouseOver.InnerBorder.End;
		backMouseOverStyle.BackColorGradientAngle = 90;
		if (c.RibbonBar.MouseOver.TopBackground != null && c.RibbonBar.MouseOver.BottomBackground != null)
		{
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.TopBackground.Start, 0f));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.TopBackground.End, c.RibbonBar.MouseOver.TopBackgroundHeight));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.BottomBackground.Start, c.RibbonBar.MouseOver.TopBackgroundHeight));
			backMouseOverStyle.BackColorBlend.Add(new BackgroundColorBlend(c.RibbonBar.MouseOver.BottomBackground.End, 1f));
		}
		if (c.RibbonBar.Default.TitleBackground != null)
		{
			titleStyle.BackColor = c.RibbonBar.Default.TitleBackground.Start;
			titleStyle.BackColor2 = c.RibbonBar.Default.TitleBackground.End;
		}
		titleStyle.BackColorGradientAngle = 90;
		titleStyle.TextAlignment = eStyleTextAlignment.Center;
		titleStyle.TextColor = c.RibbonBar.Default.TitleText;
		titleStyle.TextShadowOffset = Point.Empty;
		titleStyle.PaddingTop = 1;
		titleStyle.PaddingBottom = 1;
		if (c.RibbonBar.MouseOver != null)
		{
			if (c.RibbonBar.MouseOver.TitleBackground != null)
			{
				titleMouseOverStyle.BackColor = c.RibbonBar.MouseOver.TitleBackground.Start;
				titleMouseOverStyle.BackColor2 = c.RibbonBar.MouseOver.TitleBackground.End;
			}
			titleMouseOverStyle.BackColorGradientAngle = 90;
			titleMouseOverStyle.TextAlignment = eStyleTextAlignment.Center;
			titleMouseOverStyle.TextColor = c.RibbonBar.MouseOver.TitleText;
		}
		titleStyle.HideMnemonic = true;
		titleMouseOverStyle.HideMnemonic = true;
	}

	internal static ElementStyle StyleFromRibbonBarStateColorTable(Office2007RibbonBarStateColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Border = eStyleBorderType.Etched;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.CornerDiameter = 3;
		elementStyle.BorderColor = table.OuterBorder.Start;
		elementStyle.BorderColor2 = table.OuterBorder.End;
		elementStyle.BorderColorLight = table.InnerBorder.Start;
		elementStyle.BorderColorLight2 = table.InnerBorder.End;
		elementStyle.BackColorGradientAngle = 90;
		if (table.BottomBackground != null && !table.BottomBackground.IsEmpty)
		{
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(table.TopBackground.Start, 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(table.TopBackground.End, table.TopBackgroundHeight));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(table.BottomBackground.Start, table.TopBackgroundHeight));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(table.BottomBackground.End, 1f));
		}
		else if (table.TopBackground != null && !table.TopBackground.IsEmpty)
		{
			elementStyle.BackColor = table.TopBackground.Start;
			elementStyle.BackColor2 = table.TopBackground.End;
		}
		return elementStyle;
	}

	private static ColorScheme GetColorScheme()
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
		}
		return new ColorScheme();
	}

	public static void SetRibbonControlStyle(RibbonControl r, eDotNetBarStyle style)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		r.BackgroundStyle.Reset();
		if (style == eDotNetBarStyle.Office2010 || style == eDotNetBarStyle.Windows7 || style == eDotNetBarStyle.Metro || style != r.Style)
		{
			ChangeStyle((Control)(object)r, style, r.IsDesignMode, GetColorScheme());
		}
		else
		{
			r.Style = style;
			foreach (Control item in (ArrangedElementCollection)((Control)r).Controls)
			{
				Control val = item;
				if (val is RibbonPanel)
				{
					RibbonPanel ribbonPanel = val as RibbonPanel;
					ribbonPanel.Style.Reset();
					ribbonPanel.StyleMouseDown.Reset();
					ribbonPanel.StyleMouseOver.Reset();
					ribbonPanel.ColorSchemeStyle = style;
				}
				else if (val is RibbonBar)
				{
					SetRibbonBarStyle(val as RibbonBar, style);
				}
				foreach (Control item2 in (ArrangedElementCollection)val.Controls)
				{
					Control val2 = item2;
					if (val2 is RibbonBar)
					{
						SetRibbonBarStyle(val2 as RibbonBar, style);
					}
				}
			}
		}
		((Control)r).Invalidate(true);
	}

	internal static void ApplyQatElementStyle(ElementStyle style, QatToolbar qat)
	{
		if (((Control)qat).Parent is RibbonControl)
		{
			style.Reset();
			switch (qat.EffectiveStyle)
			{
			case eDotNetBarStyle.Office2010:
			case eDotNetBarStyle.Metro:
			{
				Office2007ColorTable office2007ColorTable3 = GetOffice2007ColorTable(((Control)qat).Parent as RibbonControl);
				Office2007QuickAccessToolbarStateColorTable standalone3 = office2007ColorTable3.QuickAccessToolbar.Standalone;
				style.BackColor = standalone3.BottomBackground.Start;
				style.BackColor2 = standalone3.BottomBackground.End;
				style.BackColorGradientAngle = standalone3.BottomBackground.GradientAngle;
				style.Border = eStyleBorderType.None;
				style.BorderWidth = 0;
				style.CornerType = eCornerType.Square;
				style.BorderColor = standalone3.OutterBorderColor;
				style.BorderColorLight = standalone3.InnerBorderColor;
				break;
			}
			case eDotNetBarStyle.Windows7:
			{
				Office2007ColorTable office2007ColorTable2 = GetOffice2007ColorTable(((Control)qat).Parent as RibbonControl);
				Office2007QuickAccessToolbarStateColorTable standalone2 = office2007ColorTable2.QuickAccessToolbar.Standalone;
				style.BackColor = standalone2.BottomBackground.Start;
				style.BackColor2 = standalone2.BottomBackground.End;
				style.BackColorGradientAngle = standalone2.BottomBackground.GradientAngle;
				style.Border = eStyleBorderType.Solid;
				style.BorderWidth = 1;
				style.CornerType = eCornerType.Square;
				style.BorderColor = standalone2.OutterBorderColor;
				style.BorderColorLight = standalone2.InnerBorderColor;
				break;
			}
			case eDotNetBarStyle.Office2007:
			{
				Office2007ColorTable office2007ColorTable = GetOffice2007ColorTable(((Control)qat).Parent as RibbonControl);
				Office2007QuickAccessToolbarStateColorTable standalone = office2007ColorTable.QuickAccessToolbar.Standalone;
				style.BackColor = standalone.BottomBackground.Start;
				style.BackColor2 = standalone.BottomBackground.End;
				style.BackColorGradientAngle = standalone.BottomBackground.GradientAngle;
				style.Border = eStyleBorderType.Etched;
				style.BorderWidth = 1;
				style.CornerType = eCornerType.Rounded;
				style.CornerDiameter = 2;
				style.BorderColor = standalone.OutterBorderColor;
				style.BorderColorLight = standalone.InnerBorderColor;
				break;
			}
			default:
				style.BackColorSchemePart = eColorSchemePart.BarBackground;
				style.BackColor2SchemePart = eColorSchemePart.BarBackground2;
				style.BackColorGradientAngle = 90;
				break;
			}
		}
	}

	public static void SetRibbonBarStyle(RibbonBar bar, eDotNetBarStyle style)
	{
		bar.BackgroundStyle.Reset();
		bar.BackgroundMouseOverStyle.Reset();
		bar.TitleStyle.Reset();
		bar.TitleStyleMouseOver.Reset();
		bar.Style = style;
		((Control)bar).Invalidate();
	}

	public static void SetupFileMenuContainer(ItemContainer topContainer)
	{
		Office2007MenuColorTable office2007MenuColorTable = GetOffice2007MenuColorTable();
		if (office2007MenuColorTable != null)
		{
			topContainer.LayoutOrientation = eOrientation.Vertical;
			topContainer.BackgroundStyle.PaddingBottom = 3;
			topContainer.BackgroundStyle.PaddingLeft = 3;
			topContainer.BackgroundStyle.PaddingRight = 3;
			topContainer.BackgroundStyle.PaddingTop = 8;
			BackgroundColorBlend[] array = new BackgroundColorBlend[office2007MenuColorTable.FileBackgroundBlend.Count];
			office2007MenuColorTable.FileBackgroundBlend.CopyTo(array);
			topContainer.BackgroundStyle.BackColorBlend.Clear();
			topContainer.BackgroundStyle.BackColorBlend.AddRange(array);
		}
	}

	private static Office2007MenuColorTable GetOffice2007MenuColorTable()
	{
		Office2007MenuColorTable result = null;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			result = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Menu;
		}
		return result;
	}

	public static void SetupTwoColumnMenuContainer(ItemContainer twoColumnMenu)
	{
		Office2007MenuColorTable office2007MenuColorTable = GetOffice2007MenuColorTable();
		if (office2007MenuColorTable != null)
		{
			twoColumnMenu.BackgroundStyle.BorderBottom = eStyleBorderType.Double;
			twoColumnMenu.BackgroundStyle.BorderBottomWidth = 1;
			twoColumnMenu.BackgroundStyle.BorderColor = office2007MenuColorTable.FileContainerBorder;
			twoColumnMenu.BackgroundStyle.BorderColorLight = office2007MenuColorTable.FileContainerBorderLight;
			twoColumnMenu.BackgroundStyle.BorderLeft = eStyleBorderType.Double;
			twoColumnMenu.BackgroundStyle.BorderLeftWidth = 1;
			twoColumnMenu.BackgroundStyle.BorderRight = eStyleBorderType.Double;
			twoColumnMenu.BackgroundStyle.BorderRightWidth = 1;
			twoColumnMenu.BackgroundStyle.BorderTop = eStyleBorderType.Double;
			twoColumnMenu.BackgroundStyle.BorderTopWidth = 1;
			twoColumnMenu.BackgroundStyle.PaddingBottom = 2;
			twoColumnMenu.BackgroundStyle.PaddingLeft = 2;
			twoColumnMenu.BackgroundStyle.PaddingRight = 2;
			twoColumnMenu.BackgroundStyle.PaddingTop = 2;
			twoColumnMenu.ItemSpacing = 0;
		}
	}

	public static void SetupMenuColumnOneContainer(ItemContainer menuColumn)
	{
		Office2007MenuColorTable office2007MenuColorTable = GetOffice2007MenuColorTable();
		if (office2007MenuColorTable != null)
		{
			menuColumn.BackgroundStyle.BackColor = office2007MenuColorTable.FileColumnOneBackground;
			menuColumn.BackgroundStyle.BorderRight = eStyleBorderType.Solid;
			menuColumn.BackgroundStyle.BorderRightColor = office2007MenuColorTable.FileColumnOneBorder;
			menuColumn.BackgroundStyle.BorderRightWidth = 1;
			menuColumn.BackgroundStyle.PaddingRight = 1;
			menuColumn.LayoutOrientation = eOrientation.Vertical;
			menuColumn.MinimumSize = new Size(120, 0);
		}
	}

	public static void SetupMenuColumnTwoContainer(ItemContainer columnTwo)
	{
		Office2007MenuColorTable office2007MenuColorTable = GetOffice2007MenuColorTable();
		if (office2007MenuColorTable != null)
		{
			columnTwo.BackgroundStyle.BackColor = office2007MenuColorTable.FileColumnTwoBackground;
			columnTwo.LayoutOrientation = eOrientation.Vertical;
			columnTwo.MinimumSize = new Size(180, 0);
		}
	}

	public static void SetupMenuBottomContainer(ItemContainer bottomContainer)
	{
		Office2007MenuColorTable office2007MenuColorTable = GetOffice2007MenuColorTable();
		if (office2007MenuColorTable != null)
		{
			BackgroundColorBlend[] array = new BackgroundColorBlend[office2007MenuColorTable.FileBottomContainerBackgroundBlend.Count];
			office2007MenuColorTable.FileBottomContainerBackgroundBlend.CopyTo(array);
			bottomContainer.BackgroundStyle.BackColorBlend.Clear();
			bottomContainer.BackgroundStyle.BackColorBlend.AddRange(array);
			bottomContainer.BackgroundStyle.BackColorGradientAngle = 90;
			bottomContainer.HorizontalItemAlignment = eHorizontalItemsAlignment.Right;
		}
	}

	public static void ChangeOffice2007ColorTable(eOffice2007ColorScheme colorTable)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable.Dispose();
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(colorTable);
			ApplyOffice2007ColorTable();
			return;
		}
		throw new InvalidOperationException("GlobalManager.Renderer is not Office2007Renderer. Cannot change the color table. Make sure that renderer is set to Office2007Renderer");
	}

	public static void ApplyOffice2007ColorTable()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		foreach (Form item in (ReadOnlyCollectionBase)(object)Application.OpenForms)
		{
			Form form = item;
			ApplyOffice2007ColorTable((Control)(object)form);
		}
	}

	public static void ChangeOffice2007ColorTable(Control form, eOffice2007ColorScheme colorTable)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable.Dispose();
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(colorTable);
			ApplyOffice2007ColorTable(form);
			return;
		}
		throw new InvalidOperationException("GlobalManager.Renderer is not Office2007Renderer. Cannot change the color table. Make sure that renderer is set to Office2007Renderer");
	}

	public static void ChangeOffice2007ColorTable(Control form, eOffice2007ColorScheme colorTable, Color baseSchemeColor)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable.Dispose();
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(colorTable, baseSchemeColor);
			ApplyOffice2007ColorTable(form);
			return;
		}
		throw new InvalidOperationException("GlobalManager.Renderer is not Office2007Renderer. Cannot change the color table. Make sure that renderer is set to Office2007Renderer");
	}

	public static void ChangeOffice2007ColorTable(eOffice2007ColorScheme colorTable, Color baseSchemeColor)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable.Dispose();
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(colorTable, baseSchemeColor);
			ApplyOffice2007ColorTable();
			return;
		}
		throw new InvalidOperationException("GlobalManager.Renderer is not Office2007Renderer. Cannot change the color table. Make sure that renderer is set to Office2007Renderer");
	}

	public static void ApplyOffice2007ColorTable(Control form)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		if (form == null)
		{
			return;
		}
		if (!(GlobalManager.Renderer is Office2007Renderer))
		{
			throw new InvalidOperationException("GlobalManager.Renderer is not Office2007Renderer. Cannot change the color table. Make sure that renderer is set to Office2007Renderer");
		}
		ColorScheme legacyColors = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
		foreach (Control item in (ArrangedElementCollection)form.Controls)
		{
			Control cont = item;
			ChangeOffice2007ColorTable(cont, legacyColors);
		}
		if (form is RibbonForm)
		{
			form.BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.BackColor;
		}
		if (form is OfficeForm)
		{
			((OfficeForm)(object)form).InvalidateNonClient(invalidateForm: true);
		}
		form.Invalidate(true);
	}

	private static void ChangeOffice2007ColorTable(Control cont, ColorScheme cs)
	{
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Invalid comparison between Unknown and I4
		bool flag = true;
		if (cont is PanelEx && ((PanelEx)(object)cont).ColorSchemeStyle == eDotNetBarStyle.Office2007)
		{
			PanelEx panelEx = cont as PanelEx;
			panelEx.ColorSchemeStyle = eDotNetBarStyle.Office2007;
			cont.Invalidate();
		}
		if (cont is PanelControl && ((PanelControl)(object)cont).EffectiveColorSchemeStyle == eDotNetBarStyle.Office2007)
		{
			PanelControl panelControl = cont as PanelControl;
			panelControl.RefreshStyleSystemColors();
			((Control)panelControl).Invalidate();
		}
		else if (cont is ExpandablePanel && ((ExpandablePanel)(object)cont).ColorSchemeStyle == eDotNetBarStyle.Office2007)
		{
			cont.Invalidate();
		}
		else if (cont is RibbonControl)
		{
			((RibbonControl)(object)cont).RibbonStrip.InitDefaultStyles();
			cont.Invalidate(true);
		}
		else if (cont is RibbonBar)
		{
			cont.Invalidate();
		}
		else if (cont is TabControl)
		{
			TabControl tabControl = cont as TabControl;
			if (tabControl.Style == eTabStripStyle.Office2007Dock || tabControl.Style == eTabStripStyle.Office2007Document)
			{
				tabControl.Style = tabControl.Style;
			}
		}
		else if (cont is TabStrip)
		{
			TabStrip tabStrip = cont as TabStrip;
			if (tabStrip.Style == eTabStripStyle.Office2007Dock || tabStrip.Style == eTabStripStyle.Office2007Document)
			{
				tabStrip.Style = tabStrip.Style;
			}
		}
		else if (cont is SideBar)
		{
			SideBar sideBar = cont as SideBar;
			if (sideBar.Style == eDotNetBarStyle.Office2007)
			{
				((Control)sideBar).Invalidate();
			}
		}
		else if (cont is RibbonForm)
		{
			cont.BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.BackColor;
			if (cont is OfficeForm)
			{
				((OfficeForm)(object)cont).InvalidateNonClient(invalidateForm: true);
			}
		}
		else if (cont is DockSite && (int)cont.Dock == 1)
		{
			DockSite dockSite = cont as DockSite;
			if (dockSite.Owner != null)
			{
				foreach (Bar bar in dockSite.Owner.Bars)
				{
					if (bar.DockSide == eDockSide.None)
					{
						((Control)bar).Invalidate(true);
						ChangeOffice2007ColorTable((Control)(object)bar, cs);
					}
				}
			}
		}
		else if (cont is ComboBoxEx)
		{
			((ComboBoxEx)(object)cont).Style = ((ComboBoxEx)(object)cont).Style;
		}
		else if (cont is NavigationPane)
		{
			((NavigationPane)(object)cont).Office2007ColorTableChanged();
		}
		else if (cont is BaseItemControl && ((BaseItemControl)(object)cont).Style == eDotNetBarStyle.Office2007)
		{
			cont.Invalidate();
		}
		else if (cont is ListViewEx)
		{
			ListViewEx listViewEx = cont as ListViewEx;
			((Control)listViewEx).Invalidate(true);
			listViewEx.ResetCachedColorTableReference();
		}
		else if (cont is DataGridViewX)
		{
			cont.Invalidate(true);
		}
		else if (cont is DGHScrollBar || cont is DGVScrollBar)
		{
			cont.Invalidate();
		}
		else if (cont is WarningBox)
		{
			((WarningBox)(object)cont).UpdateColorScheme();
		}
		if (!flag)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)cont.Controls)
		{
			Control cont2 = item;
			ChangeOffice2007ColorTable(cont2, cs);
		}
	}

	public static void ChangeStyle(eDotNetBarStyle newStyle)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		foreach (Form item in (ReadOnlyCollectionBase)(object)Application.OpenForms)
		{
			Form parentForm = item;
			ChangeStyle(newStyle, (Control)(object)parentForm);
		}
	}

	public static void ChangeStyle(eDotNetBarStyle newStyle, Color blendColor)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		foreach (Form item in (ReadOnlyCollectionBase)(object)Application.OpenForms)
		{
			Form parentForm = item;
			ChangeStyle(newStyle, (Control)(object)parentForm, designMode: false, blendColor);
		}
	}

	public static void ChangeStyle(eDotNetBarStyle newStyle, Control parentForm)
	{
		ChangeStyle(newStyle, parentForm, designMode: false, Color.Empty);
	}

	public static void ChangeStyle(eDotNetBarStyle newStyle, Control parentForm, Color blendColor)
	{
		ChangeStyle(newStyle, parentForm, designMode: false, blendColor);
	}

	private static void ChangeStyle(eDotNetBarStyle newStyle, Control parentForm, bool designMode, Color blendColor)
	{
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		switch (newStyle)
		{
		case eDotNetBarStyle.Office2010:
		case eDotNetBarStyle.Metro:
			if (blendColor.IsEmpty)
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable();
			}
			else
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.Silver, blendColor);
			}
			break;
		case eDotNetBarStyle.Windows7:
			if (blendColor.IsEmpty)
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable();
			}
			else
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable(eWindows7ColorScheme.Blue, blendColor);
			}
			break;
		case eDotNetBarStyle.Office2007:
			if (blendColor.IsEmpty)
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable();
			}
			else
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(eOffice2007ColorScheme.Blue, blendColor);
			}
			break;
		}
		ColorScheme legacyColors = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
		foreach (Control item in (ArrangedElementCollection)parentForm.Controls)
		{
			Control cont = item;
			ChangeStyle(cont, newStyle, designMode, legacyColors);
		}
	}

	private static BaseItem GetAppButton(RibbonControl ribbon)
	{
		BaseItem result = null;
		for (int i = 0; i < ribbon.QuickToolbarItems.Count; i++)
		{
			if (ribbon.QuickToolbarItems[i] is ApplicationButton)
			{
				result = ribbon.QuickToolbarItems[i];
				break;
			}
		}
		return result;
	}

	private static eTabStripStyle GetTabControlStyle(TabStrip tab, eDotNetBarStyle style)
	{
		switch (style)
		{
		case eDotNetBarStyle.Office2007:
		case eDotNetBarStyle.Office2010:
		case eDotNetBarStyle.Windows7:
		case eDotNetBarStyle.Metro:
			if (tab.Style != eTabStripStyle.Office2007Dock || tab.Style != eTabStripStyle.Office2007Document)
			{
				return eTabStripStyle.Office2007Document;
			}
			break;
		case eDotNetBarStyle.VS2005:
			return eTabStripStyle.VS2005;
		}
		return tab.Style;
	}

	private static void ChangeStyle(Control cont, eDotNetBarStyle style, bool designMode, ColorScheme cs)
	{
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Invalid comparison between Unknown and I4
		eDotNetBarStyle eDotNetBarStyle2 = style;
		if (style == eDotNetBarStyle.StyleManagerControlled)
		{
			eDotNetBarStyle2 = StyleManager.GetEffectiveStyle();
		}
		bool flag = true;
		if (cont is PanelEx && ((PanelEx)(object)cont).ColorSchemeStyle == eDotNetBarStyle.Office2007)
		{
			PanelEx panelEx = cont as PanelEx;
			panelEx.ColorSchemeStyle = style;
			cont.Invalidate();
		}
		if (cont is PanelControl && ((PanelControl)(object)cont).ColorSchemeStyle == eDotNetBarStyle.Office2007)
		{
			PanelControl panelControl = cont as PanelControl;
			panelControl.ColorSchemeStyle = style;
			panelControl.RefreshStyleSystemColors();
			((Control)panelControl).Invalidate();
		}
		else if (cont is ExpandablePanel && ((ExpandablePanel)(object)cont).ColorSchemeStyle == eDotNetBarStyle.Office2007)
		{
			ExpandablePanel expandablePanel = cont as ExpandablePanel;
			expandablePanel.ColorSchemeStyle = style;
			cont.Invalidate();
		}
		else if (cont is RibbonControl)
		{
			RibbonControl ribbonControl = (RibbonControl)(object)cont;
			switch (eDotNetBarStyle2)
			{
			case eDotNetBarStyle.Office2010:
			case eDotNetBarStyle.Windows7:
			case eDotNetBarStyle.Metro:
			{
				BaseItem appButton = GetAppButton(ribbonControl);
				if (appButton != null)
				{
					if (appButton is ButtonItem && ((ButtonItem)appButton).ImageFixedSize.IsEmpty)
					{
						((ButtonItem)appButton).ImageFixedSize = new Size(16, 16);
					}
					ribbonControl.QuickToolbarItems.Remove(appButton);
					ribbonControl.Items.Insert(0, appButton);
				}
				break;
			}
			case eDotNetBarStyle.Office2007:
				if (ribbonControl.Items.Count > 0 && ribbonControl.Items[0] is ApplicationButton)
				{
					ApplicationButton applicationButton = (ApplicationButton)ribbonControl.Items[0];
					if (!applicationButton.ImageFixedSize.IsEmpty)
					{
						applicationButton.ImageFixedSize = Size.Empty;
					}
					ribbonControl.Items.Remove(applicationButton);
					ribbonControl.QuickToolbarItems.Insert(0, applicationButton);
				}
				break;
			}
			if ((style == eDotNetBarStyle.StyleManagerControlled && ribbonControl.Style == style) || style != eDotNetBarStyle.StyleManagerControlled)
			{
				ribbonControl.Style = style;
			}
			ribbonControl.RibbonStrip.InitDefaultStyles();
			((Control)ribbonControl).Invalidate(true);
			if (((Control)ribbonControl).IsHandleCreated && ribbonControl.SelectedRibbonTabItem != null && ribbonControl.SelectedRibbonTabItem.Panel != null)
			{
				((Control)ribbonControl.SelectedRibbonTabItem.Panel).PerformLayout();
			}
			flag = false;
		}
		else if (cont is RibbonBar)
		{
			RibbonBar ribbonBar = cont as RibbonBar;
			ribbonBar.Style = style;
			cont.Invalidate();
		}
		else if (cont is TabControl)
		{
			TabControl tabControl = cont as TabControl;
			tabControl.Style = GetTabControlStyle(tabControl.TabStrip, style);
		}
		else if (cont is TabStrip)
		{
			TabStrip tabStrip = cont as TabStrip;
			tabStrip.Style = GetTabControlStyle(tabStrip, style);
		}
		else if (cont is SideBar)
		{
			SideBar sideBar = cont as SideBar;
			sideBar.Style = style;
			((Control)sideBar).Invalidate();
		}
		else if (cont is RibbonForm)
		{
			cont.BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.BackColor;
			if (cont is OfficeForm)
			{
				((OfficeForm)(object)cont).InvalidateNonClient(invalidateForm: true);
			}
		}
		else if (cont is DockSite && (int)cont.Dock == 1)
		{
			DockSite dockSite = cont as DockSite;
			if (dockSite.Owner != null)
			{
				foreach (Bar bar in dockSite.Owner.Bars)
				{
					if (bar.DockSide == eDockSide.None)
					{
						((Control)bar).Invalidate(true);
						ChangeOffice2007ColorTable((Control)(object)bar, cs);
					}
				}
			}
		}
		else if (cont is ComboBoxEx)
		{
			((ComboBoxEx)(object)cont).Style = style;
		}
		else if (cont is NavigationPane)
		{
			NavigationPane navigationPane = (NavigationPane)(object)cont;
			navigationPane.Style = style;
			if (BarFunctions.IsOffice2007Style(style))
			{
				((NavigationPane)(object)cont).Office2007ColorTableChanged();
			}
		}
		else if (cont is BaseItemControl)
		{
			((BaseItemControl)(object)cont).Style = style;
			cont.Invalidate();
		}
		else if (cont is ListViewEx)
		{
			ListViewEx listViewEx = cont as ListViewEx;
			((Control)listViewEx).Invalidate(true);
			listViewEx.ResetCachedColorTableReference();
		}
		else if (cont is DataGridViewX)
		{
			cont.Invalidate(true);
		}
		else if (cont is DGHScrollBar || cont is DGVScrollBar)
		{
			cont.Invalidate();
		}
		else if (cont is WarningBox)
		{
			((WarningBox)(object)cont).UpdateColorScheme();
		}
		if (!flag)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)cont.Controls)
		{
			Control cont2 = item;
			ChangeOffice2007ColorTable(cont2, cs);
		}
	}

	public static void ChangeOffice2010ColorTable(Control form, eOffice2010ColorScheme colorTable)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable.Dispose();
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(colorTable);
			ApplyOffice2007ColorTable(form);
			return;
		}
		throw new InvalidOperationException("GlobalManager.Renderer is not Office2007Renderer. Cannot change the color table. Make sure that renderer is set to Office2007Renderer");
	}

	public static void ChangeOffice2010ColorTable(Control form, eOffice2010ColorScheme colorTable, Color baseSchemeColor)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable.Dispose();
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(colorTable, baseSchemeColor);
			ApplyOffice2007ColorTable(form);
			return;
		}
		throw new InvalidOperationException("GlobalManager.Renderer is not Office2007Renderer. Cannot change the color table. Make sure that renderer is set to Office2007Renderer");
	}

	public static void ChangeOffice2010ColorTable(eOffice2010ColorScheme colorTable, Color baseSchemeColor)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable.Dispose();
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(colorTable, baseSchemeColor);
			ApplyOffice2007ColorTable();
			return;
		}
		throw new InvalidOperationException("GlobalManager.Renderer is not Office2007Renderer. Cannot change the color table. Make sure that renderer is set to Office2007Renderer");
	}

	public static void ChangeWindows7ColorTable(Control form, eWindows7ColorScheme colorTable)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable.Dispose();
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable(colorTable);
			ApplyOffice2007ColorTable(form);
			return;
		}
		throw new InvalidOperationException("GlobalManager.Renderer is not Office2007Renderer. Cannot change the color table. Make sure that renderer is set to Office2007Renderer");
	}

	public static void ChangeWindows7ColorTable(Control form, eWindows7ColorScheme colorTable, Color baseSchemeColor)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable.Dispose();
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable(colorTable, baseSchemeColor);
			ApplyOffice2007ColorTable(form);
			return;
		}
		throw new InvalidOperationException("GlobalManager.Renderer is not Office2007Renderer. Cannot change the color table. Make sure that renderer is set to Office2007Renderer");
	}

	public static void ChangeWindows7ColorTable(eWindows7ColorScheme colorTable, Color baseSchemeColor)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable.Dispose();
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable(colorTable, baseSchemeColor);
			ApplyOffice2007ColorTable();
			return;
		}
		throw new InvalidOperationException("GlobalManager.Renderer is not Office2007Renderer. Cannot change the color table. Make sure that renderer is set to Office2007Renderer");
	}
}

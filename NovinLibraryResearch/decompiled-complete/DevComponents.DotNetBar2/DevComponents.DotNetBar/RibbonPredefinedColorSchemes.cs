using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Ribbon;

namespace DevComponents.DotNetBar;

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

	internal static void _20F4(ElementStyle _20F5, ElementStyle _20F6, ElementStyle _20F7, ElementStyle _20F8)
	{
		_20F5.Reset();
		_20F6.Reset();
		_20F7.Reset();
		_20F8.Reset();
		_20F5.BackColorSchemePart = eColorSchemePart.BarBackground2;
		_20F5.BackColor2SchemePart = eColorSchemePart.BarBackground;
		_20F5.BackColorGradientAngle = 90;
		_20F7.BackColorSchemePart = eColorSchemePart.PanelBackground;
		_20F7.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		_20F7.BackColorGradientAngle = 90;
		_20F7.TextColorSchemePart = eColorSchemePart.PanelText;
		_20F7.PaddingTop = 1;
		_20F7.PaddingBottom = 1;
		_20F7.PaddingLeft = 2;
		_20F7.PaddingRight = 2;
		_20F7.TextShadowColor = Color.Black;
		_20F7.TextShadowOffset = new Point(1, 1);
		_20F7.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		_20F7.Border = eStyleBorderType.Solid;
		_20F7.BorderWidth = 1;
	}

	public static void ApplyOffice2003ColorScheme(RibbonControl b)
	{
		_20FE(b.BackgroundStyle);
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

	internal static void _20F9(ElementStyle _0657, RibbonControl ᑠ)
	{
		eDotNetBarStyle eDotNetBarStyle2 = ᑠ.Style;
		if (eDotNetBarStyle2 == eDotNetBarStyle.StyleManagerControlled)
		{
			eDotNetBarStyle2 = StyleManager.GetEffectiveStyle();
		}
		if (ᑠ != null && eDotNetBarStyle2 == eDotNetBarStyle.Office2007)
		{
			_20FA(_0657, ጪ(ᑠ));
		}
		else if (ᑠ != null && (eDotNetBarStyle2 == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(eDotNetBarStyle2)))
		{
			_20FB(_0657, ጪ(ᑠ), ᑠ.RibbonStrip.ষ);
		}
		else if (ᑠ != null && eDotNetBarStyle2 == eDotNetBarStyle.Windows7)
		{
			_20FD(_0657, ጪ(ᑠ));
		}
		else
		{
			_20FE(_0657);
		}
	}

	internal static void _20FA(ElementStyle _20F5, Office2007ColorTable ዅ)
	{
		_20F5.Reset();
		_20F5.BackColor = ዅ.RibbonControl.TabsBackground.Start;
		_20F5.BackColor2 = ዅ.RibbonControl.TabsBackground.End;
		_20F5.BackColorGradientAngle = ዅ.RibbonControl.TabsBackground.GradientAngle;
		_20F5.BackColorGradientType = eGradientType.Linear;
	}

	internal static void _20FB(ElementStyle _20F5, Office2007ColorTable ዅ, bool _20FC)
	{
		_20F5.Reset();
		_20F5.BackColor = ዅ.RibbonControl.TabsBackground.Start;
		_20F5.BackColor2 = ዅ.RibbonControl.TabsBackground.End;
		_20F5.BackColorGradientAngle = ዅ.RibbonControl.TabsBackground.GradientAngle;
		_20F5.BackColorGradientType = eGradientType.Linear;
		if (_20FC)
		{
			_20F5.BackColor = ዅ.RibbonControl.TabsGlassBackground.End;
			_20F5.BackColor2 = ዅ.RibbonControl.TabsGlassBackground.Start;
			_20F5.BackColorGradientAngle = ዅ.RibbonControl.TabsGlassBackground.GradientAngle;
			_20F5.BackColorGradientType = eGradientType.Radial;
		}
	}

	internal static void _20FD(ElementStyle _20F5, Office2007ColorTable ዅ)
	{
		_20F5.Reset();
		_20F5.BackColor = ዅ.RibbonControl.TabsBackground.Start;
		_20F5.BackColor2 = ዅ.RibbonControl.TabsBackground.End;
		_20F5.BackColorGradientAngle = ዅ.RibbonControl.TabsBackground.GradientAngle;
		_20F5.BackColorGradientType = eGradientType.Linear;
	}

	internal static void _20FE(ElementStyle _20F5)
	{
		_20F5.Reset();
		_20F5.BackColorSchemePart = eColorSchemePart.BarBackground2;
		_20F5.BackColor2SchemePart = eColorSchemePart.BarBackground;
		_20F5.BackColorGradientAngle = 90;
	}

	public static void ApplyOffice2007ColorScheme(RibbonBar b)
	{
		((Control)b).Invalidate();
	}

	internal static Office2007ColorTable ጪ(RibbonBar _0BCF)
	{
		Office2007ColorTable office2007ColorTable = null;
		if ((_0BCF.RenderMode == eRenderMode.Custom || _0BCF.RenderMode == eRenderMode.Instance) && _0BCF.Renderer != null)
		{
			if (_0BCF.Renderer is Office2007Renderer office2007Renderer)
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

	internal static Office2007ColorTable ጪ(RibbonControl _0BCF)
	{
		Office2007ColorTable office2007ColorTable = null;
		if ((_0BCF.RibbonStrip.RenderMode == eRenderMode.Custom || _0BCF.RibbonStrip.RenderMode == eRenderMode.Instance) && _0BCF.RibbonStrip.Renderer != null)
		{
			if (_0BCF.RibbonStrip.Renderer is Office2007Renderer office2007Renderer)
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

	internal static void _20FF(ElementStyle _20F5, ElementStyle _20F6, ElementStyle _20F7, ElementStyle _20F8, RibbonBar _0BCF)
	{
		if (_0BCF._൨ == eDotNetBarStyle.Office2007)
		{
			Office2007ColorTable office2007ColorTable = ጪ(_0BCF);
			_2103(_20F5, _20F6, _20F7, _20F8, office2007ColorTable);
		}
		else if (_0BCF._൨ == eDotNetBarStyle.Office2010)
		{
			Office2007ColorTable office2007ColorTable2 = ጪ(_0BCF);
			_2100(_20F5, _20F6, _20F7, _20F8, office2007ColorTable2);
		}
		else if (StyleManager.IsMetro(_0BCF._൨))
		{
			Office2007ColorTable office2007ColorTable3 = ጪ(_0BCF);
			_2101(_20F5, _20F6, _20F7, _20F8, office2007ColorTable3);
		}
		else if (_0BCF._൨ == eDotNetBarStyle.Windows7)
		{
			Office2007ColorTable office2007ColorTable4 = ጪ(_0BCF);
			ℂ(_20F5, _20F6, _20F7, _20F8, office2007ColorTable4);
		}
		else
		{
			_20F4(_20F5, _20F6, _20F7, _20F8);
		}
	}

	internal static void _2100(ElementStyle _20F5, ElementStyle _20F6, ElementStyle _20F7, ElementStyle _20F8, Office2007ColorTable _0BDE)
	{
		_20F5.Reset();
		_20F6.Reset();
		_20F7.Reset();
		_20F8.Reset();
		_20F5.BorderRight = eStyleBorderType.Etched;
		_20F5.BorderRightWidth = 1;
		_20F5.CornerType = eCornerType.Square;
		_20F5.BorderColor = _0BDE.RibbonBar.Default.OuterBorder.Start;
		_20F5.BorderColor2 = _0BDE.RibbonBar.Default.OuterBorder.End;
		_20F5.BorderColorLight = _0BDE.RibbonBar.Default.InnerBorder.Start;
		_20F5.BorderColorLight2 = _0BDE.RibbonBar.Default.InnerBorder.End;
		_20F5.BackColorGradientAngle = 90;
		if (_0BDE.RibbonBar.Default.BottomBackground == null)
		{
			_20F5.BackColor = _0BDE.RibbonBar.Default.TopBackground.Start;
			_20F5.BackColor2 = _0BDE.RibbonBar.Default.TopBackground.End;
		}
		else
		{
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.TopBackground.Start, 0f));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.TopBackground.End, _0BDE.RibbonBar.Default.TopBackgroundHeight));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.BottomBackground.Start, _0BDE.RibbonBar.Default.TopBackgroundHeight));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.BottomBackground.End, 1f));
		}
		_20F5.PaddingTop = 2;
		_20F5.PaddingLeft = 2;
		_20F5.PaddingRight = 2;
		_20F6.BorderColor = _0BDE.RibbonBar.MouseOver.OuterBorder.Start;
		_20F6.BorderColor2 = _0BDE.RibbonBar.MouseOver.OuterBorder.End;
		_20F6.BorderColorLight = _0BDE.RibbonBar.MouseOver.InnerBorder.Start;
		_20F6.BorderColorLight2 = _0BDE.RibbonBar.MouseOver.InnerBorder.End;
		_20F6.BackColorGradientAngle = 90;
		if (_0BDE.RibbonBar.MouseOver.BottomBackground == null)
		{
			_20F5.BackColor = _0BDE.RibbonBar.MouseOver.TopBackground.Start;
			_20F5.BackColor2 = _0BDE.RibbonBar.MouseOver.TopBackground.End;
		}
		else
		{
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.TopBackground.Start, 0f));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.TopBackground.End, _0BDE.RibbonBar.MouseOver.TopBackgroundHeight));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.BottomBackground.Start, _0BDE.RibbonBar.MouseOver.TopBackgroundHeight));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.BottomBackground.End, 1f));
		}
		if (_0BDE.RibbonBar.Default.TitleBackground != null)
		{
			_20F7.BackColor = _0BDE.RibbonBar.Default.TitleBackground.Start;
			_20F7.BackColor2 = _0BDE.RibbonBar.Default.TitleBackground.End;
			_20F7.BackColorGradientAngle = 90;
		}
		_20F7.TextAlignment = eStyleTextAlignment.Center;
		_20F7.TextColor = _0BDE.RibbonBar.Default.TitleText;
		_20F7.TextShadowOffset = Point.Empty;
		_20F7.PaddingTop = 1;
		_20F7.PaddingBottom = 1;
		if (_0BDE.RibbonBar.MouseOver.TitleBackground != null)
		{
			_20F8.BackColor = _0BDE.RibbonBar.MouseOver.TitleBackground.Start;
			_20F8.BackColor2 = _0BDE.RibbonBar.MouseOver.TitleBackground.End;
			_20F8.BackColorGradientAngle = 90;
			_20F8.BackColorGradientType = eGradientType.Radial;
		}
		_20F8.TextAlignment = eStyleTextAlignment.Center;
		_20F8.TextColor = _0BDE.RibbonBar.MouseOver.TitleText;
		_20F7.HideMnemonic = true;
		_20F8.HideMnemonic = true;
	}

	internal static void _2101(ElementStyle _20F5, ElementStyle _20F6, ElementStyle _20F7, ElementStyle _20F8, Office2007ColorTable _0BDE)
	{
		_20F5.Reset();
		_20F6.Reset();
		_20F7.Reset();
		_20F8.Reset();
		_20F5.BorderRight = eStyleBorderType.Solid;
		_20F5.BorderRightWidth = 1;
		_20F5.CornerType = eCornerType.Square;
		_20F5.BorderColor = _0BDE.RibbonBar.Default.OuterBorder.Start;
		_20F5.BorderColor2 = _0BDE.RibbonBar.Default.OuterBorder.End;
		_20F5.BackColorGradientAngle = 90;
		if (_0BDE.RibbonBar.Default.BottomBackground == null)
		{
			_20F5.BackColor = _0BDE.RibbonBar.Default.TopBackground.Start;
			_20F5.BackColor2 = _0BDE.RibbonBar.Default.TopBackground.End;
		}
		else
		{
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.TopBackground.Start, 0f));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.TopBackground.End, _0BDE.RibbonBar.Default.TopBackgroundHeight));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.BottomBackground.Start, _0BDE.RibbonBar.Default.TopBackgroundHeight));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.BottomBackground.End, 1f));
		}
		_20F5.PaddingTop = 2;
		_20F5.PaddingLeft = 2;
		_20F5.PaddingRight = 2;
		_20F6.BorderColor = _0BDE.RibbonBar.MouseOver.OuterBorder.Start;
		_20F6.BorderColor2 = _0BDE.RibbonBar.MouseOver.OuterBorder.End;
		_20F6.BackColorGradientAngle = 90;
		if (_0BDE.RibbonBar.MouseOver.BottomBackground == null)
		{
			_20F6.BackColor = _0BDE.RibbonBar.MouseOver.TopBackground.Start;
			_20F6.BackColor2 = _0BDE.RibbonBar.MouseOver.TopBackground.End;
		}
		else
		{
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.TopBackground.Start, 0f));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.TopBackground.End, _0BDE.RibbonBar.MouseOver.TopBackgroundHeight));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.BottomBackground.Start, _0BDE.RibbonBar.MouseOver.TopBackgroundHeight));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.BottomBackground.End, 1f));
		}
		if (_0BDE.RibbonBar.Default.TitleBackground != null)
		{
			_20F7.BackColor = _0BDE.RibbonBar.Default.TitleBackground.Start;
			_20F7.BackColor2 = _0BDE.RibbonBar.Default.TitleBackground.End;
			_20F7.BackColorGradientAngle = 90;
		}
		_20F7.TextAlignment = eStyleTextAlignment.Center;
		_20F7.TextColor = _0BDE.RibbonBar.Default.TitleText;
		_20F7.TextShadowOffset = Point.Empty;
		_20F7.PaddingTop = 1;
		_20F7.PaddingBottom = 1;
		if (_0BDE.RibbonBar.MouseOver.TitleBackground != null)
		{
			_20F8.BackColor = _0BDE.RibbonBar.MouseOver.TitleBackground.Start;
			_20F8.BackColor2 = _0BDE.RibbonBar.MouseOver.TitleBackground.End;
			_20F8.BackColorGradientAngle = 90;
			_20F8.BackColorGradientType = eGradientType.Radial;
		}
		_20F8.TextAlignment = eStyleTextAlignment.Center;
		_20F8.TextColor = _0BDE.RibbonBar.MouseOver.TitleText;
		_20F7.HideMnemonic = true;
		_20F8.HideMnemonic = true;
	}

	internal static void ℂ(ElementStyle _20F5, ElementStyle _20F6, ElementStyle _20F7, ElementStyle _20F8, Office2007ColorTable _0BDE)
	{
		_20F5.Reset();
		_20F6.Reset();
		_20F7.Reset();
		_20F8.Reset();
		_20F5.BorderRight = eStyleBorderType.Etched;
		_20F5.BorderRightWidth = 1;
		_20F5.CornerType = eCornerType.Square;
		_20F5.BorderColor = _0BDE.RibbonBar.Default.OuterBorder.Start;
		_20F5.BorderColor2 = _0BDE.RibbonBar.Default.OuterBorder.End;
		_20F5.BorderColorLight = _0BDE.RibbonBar.Default.InnerBorder.Start;
		_20F5.BorderColorLight2 = _0BDE.RibbonBar.Default.InnerBorder.End;
		_20F5.BackColorGradientAngle = 90;
		if (_0BDE.RibbonBar.Default.BottomBackground == null)
		{
			_20F5.BackColor = _0BDE.RibbonBar.Default.TopBackground.Start;
			_20F5.BackColor2 = _0BDE.RibbonBar.Default.TopBackground.End;
		}
		else
		{
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.TopBackground.Start, 0f));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.TopBackground.End, _0BDE.RibbonBar.Default.TopBackgroundHeight));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.BottomBackground.Start, _0BDE.RibbonBar.Default.TopBackgroundHeight));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.BottomBackground.End, 1f));
		}
		_20F5.PaddingTop = 2;
		_20F5.PaddingLeft = 2;
		_20F5.PaddingRight = 2;
		_20F6.BorderColor = _0BDE.RibbonBar.MouseOver.OuterBorder.Start;
		_20F6.BorderColor2 = _0BDE.RibbonBar.MouseOver.OuterBorder.End;
		_20F6.BorderColorLight = _0BDE.RibbonBar.MouseOver.InnerBorder.Start;
		_20F6.BorderColorLight2 = _0BDE.RibbonBar.MouseOver.InnerBorder.End;
		_20F6.BackColorGradientAngle = 90;
		if (_0BDE.RibbonBar.MouseOver.BottomBackground == null)
		{
			_20F5.BackColor = _0BDE.RibbonBar.MouseOver.TopBackground.Start;
			_20F5.BackColor2 = _0BDE.RibbonBar.MouseOver.TopBackground.End;
		}
		else
		{
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.TopBackground.Start, 0f));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.TopBackground.End, _0BDE.RibbonBar.MouseOver.TopBackgroundHeight));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.BottomBackground.Start, _0BDE.RibbonBar.MouseOver.TopBackgroundHeight));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.BottomBackground.End, 1f));
		}
		if (_0BDE.RibbonBar.Default.TitleBackground != null)
		{
			_20F7.BackColor = _0BDE.RibbonBar.Default.TitleBackground.Start;
			_20F7.BackColor2 = _0BDE.RibbonBar.Default.TitleBackground.End;
			_20F7.BackColorGradientAngle = 90;
		}
		_20F7.TextAlignment = eStyleTextAlignment.Center;
		_20F7.TextColor = _0BDE.RibbonBar.Default.TitleText;
		_20F7.TextShadowOffset = Point.Empty;
		_20F7.PaddingTop = 1;
		_20F7.PaddingBottom = 1;
		if (_0BDE.RibbonBar.MouseOver.TitleBackground != null)
		{
			_20F8.BackColor = _0BDE.RibbonBar.MouseOver.TitleBackground.Start;
			_20F8.BackColor2 = _0BDE.RibbonBar.MouseOver.TitleBackground.End;
			_20F8.BackColorGradientAngle = 90;
			_20F8.BackColorGradientType = eGradientType.Radial;
		}
		_20F8.TextAlignment = eStyleTextAlignment.Center;
		_20F8.TextColor = _0BDE.RibbonBar.MouseOver.TitleText;
		_20F7.HideMnemonic = true;
		_20F8.HideMnemonic = true;
	}

	internal static void _2103(ElementStyle _20F5, ElementStyle _20F6, ElementStyle _20F7, ElementStyle _20F8, Office2007ColorTable _0BDE)
	{
		_20F5.Reset();
		_20F6.Reset();
		_20F7.Reset();
		_20F8.Reset();
		_20F5.Border = eStyleBorderType.Etched;
		_20F5.BorderWidth = 1;
		_20F5.CornerType = eCornerType.Rounded;
		_20F5.CornerDiameter = 2;
		_20F5.BorderColor = _0BDE.RibbonBar.Default.OuterBorder.Start;
		_20F5.BorderColor2 = _0BDE.RibbonBar.Default.OuterBorder.End;
		_20F5.BorderColorLight = _0BDE.RibbonBar.Default.InnerBorder.Start;
		_20F5.BorderColorLight2 = _0BDE.RibbonBar.Default.InnerBorder.End;
		_20F5.BackColorGradientAngle = 90;
		if (_0BDE.RibbonBar.Default.TopBackground != null && _0BDE.RibbonBar.Default.BottomBackground != null)
		{
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.TopBackground.Start, 0f));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.TopBackground.End, _0BDE.RibbonBar.Default.TopBackgroundHeight));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.BottomBackground.Start, _0BDE.RibbonBar.Default.TopBackgroundHeight));
			_20F5.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.Default.BottomBackground.End, 1f));
		}
		_20F5.PaddingLeft = 2;
		_20F5.PaddingRight = 2;
		_20F6.BorderColor = _0BDE.RibbonBar.MouseOver.OuterBorder.Start;
		_20F6.BorderColor2 = _0BDE.RibbonBar.MouseOver.OuterBorder.End;
		_20F6.BorderColorLight = _0BDE.RibbonBar.MouseOver.InnerBorder.Start;
		_20F6.BorderColorLight2 = _0BDE.RibbonBar.MouseOver.InnerBorder.End;
		_20F6.BackColorGradientAngle = 90;
		if (_0BDE.RibbonBar.MouseOver.TopBackground != null && _0BDE.RibbonBar.MouseOver.BottomBackground != null)
		{
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.TopBackground.Start, 0f));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.TopBackground.End, _0BDE.RibbonBar.MouseOver.TopBackgroundHeight));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.BottomBackground.Start, _0BDE.RibbonBar.MouseOver.TopBackgroundHeight));
			_20F6.BackColorBlend.Add(new BackgroundColorBlend(_0BDE.RibbonBar.MouseOver.BottomBackground.End, 1f));
		}
		if (_0BDE.RibbonBar.Default.TitleBackground != null)
		{
			_20F7.BackColor = _0BDE.RibbonBar.Default.TitleBackground.Start;
			_20F7.BackColor2 = _0BDE.RibbonBar.Default.TitleBackground.End;
		}
		_20F7.BackColorGradientAngle = 90;
		_20F7.TextAlignment = eStyleTextAlignment.Center;
		_20F7.TextColor = _0BDE.RibbonBar.Default.TitleText;
		_20F7.TextShadowOffset = Point.Empty;
		_20F7.PaddingTop = 1;
		_20F7.PaddingBottom = 1;
		if (_0BDE.RibbonBar.MouseOver != null)
		{
			if (_0BDE.RibbonBar.MouseOver.TitleBackground != null)
			{
				_20F8.BackColor = _0BDE.RibbonBar.MouseOver.TitleBackground.Start;
				_20F8.BackColor2 = _0BDE.RibbonBar.MouseOver.TitleBackground.End;
			}
			_20F8.BackColorGradientAngle = 90;
			_20F8.TextAlignment = eStyleTextAlignment.Center;
			_20F8.TextColor = _0BDE.RibbonBar.MouseOver.TitleText;
		}
		_20F7.HideMnemonic = true;
		_20F8.HideMnemonic = true;
	}

	internal static ElementStyle _2104(Office2007RibbonBarStateColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Border = eStyleBorderType.Etched;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.CornerDiameter = 3;
		elementStyle.BorderColor = ቯ.OuterBorder.Start;
		elementStyle.BorderColor2 = ቯ.OuterBorder.End;
		elementStyle.BorderColorLight = ቯ.InnerBorder.Start;
		elementStyle.BorderColorLight2 = ቯ.InnerBorder.End;
		elementStyle.BackColorGradientAngle = 90;
		if (ቯ.BottomBackground != null && !ቯ.BottomBackground.IsEmpty)
		{
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ቯ.TopBackground.Start, 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ቯ.TopBackground.End, ቯ.TopBackgroundHeight));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ቯ.BottomBackground.Start, ቯ.TopBackgroundHeight));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ቯ.BottomBackground.End, 1f));
		}
		else if (ቯ.TopBackground != null && !ቯ.TopBackground.IsEmpty)
		{
			elementStyle.BackColor = ቯ.TopBackground.Start;
			elementStyle.BackColor2 = ቯ.TopBackground.End;
		}
		return elementStyle;
	}

	private static ColorScheme _2105()
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
		}
		return new ColorScheme();
	}

	public static void SetRibbonControlStyle(RibbonControl r, eDotNetBarStyle style)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		r.BackgroundStyle.Reset();
		if (style == eDotNetBarStyle.Office2010 || style == eDotNetBarStyle.Windows7 || StyleManager.IsMetro(style) || style != r.Style)
		{
			ℊ((Control)(object)r, style, r.Ꮬ, _2105());
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

	internal static void _2106(ElementStyle _0657, Ꮮ ℇ)
	{
		if (!(((Control)ℇ).Parent is RibbonControl))
		{
			return;
		}
		_0657.Reset();
		eDotNetBarStyle _൨ = ℇ._൨;
		if (_൨ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(_൨))
		{
			Office2007ColorTable office2007ColorTable = ጪ(((Control)ℇ).Parent as RibbonControl);
			Office2007QuickAccessToolbarStateColorTable standalone = office2007ColorTable.QuickAccessToolbar.Standalone;
			_0657.BackColor = standalone.BottomBackground.Start;
			_0657.BackColor2 = standalone.BottomBackground.End;
			_0657.BackColorGradientAngle = standalone.BottomBackground.GradientAngle;
			_0657.Border = eStyleBorderType.Solid;
			_0657.BorderWidth = 1;
			_0657.CornerType = eCornerType.Square;
			_0657.BorderColor = standalone.OutterBorderColor;
			_0657.BorderColorLight = standalone.InnerBorderColor;
			return;
		}
		switch (_൨)
		{
		case eDotNetBarStyle.Windows7:
		{
			Office2007ColorTable office2007ColorTable3 = ጪ(((Control)ℇ).Parent as RibbonControl);
			Office2007QuickAccessToolbarStateColorTable standalone3 = office2007ColorTable3.QuickAccessToolbar.Standalone;
			_0657.BackColor = standalone3.BottomBackground.Start;
			_0657.BackColor2 = standalone3.BottomBackground.End;
			_0657.BackColorGradientAngle = standalone3.BottomBackground.GradientAngle;
			_0657.Border = eStyleBorderType.Solid;
			_0657.BorderWidth = 1;
			_0657.CornerType = eCornerType.Square;
			_0657.BorderColor = standalone3.OutterBorderColor;
			_0657.BorderColorLight = standalone3.InnerBorderColor;
			break;
		}
		case eDotNetBarStyle.Office2007:
		{
			Office2007ColorTable office2007ColorTable2 = ጪ(((Control)ℇ).Parent as RibbonControl);
			Office2007QuickAccessToolbarStateColorTable standalone2 = office2007ColorTable2.QuickAccessToolbar.Standalone;
			_0657.BackColor = standalone2.BottomBackground.Start;
			_0657.BackColor2 = standalone2.BottomBackground.End;
			_0657.BackColorGradientAngle = standalone2.BottomBackground.GradientAngle;
			_0657.Border = eStyleBorderType.Etched;
			_0657.BorderWidth = 1;
			_0657.CornerType = eCornerType.Rounded;
			_0657.CornerDiameter = 2;
			_0657.BorderColor = standalone2.OutterBorderColor;
			_0657.BorderColorLight = standalone2.InnerBorderColor;
			break;
		}
		default:
			_0657.BackColorSchemePart = eColorSchemePart.BarBackground;
			_0657.BackColor2SchemePart = eColorSchemePart.BarBackground2;
			_0657.BackColorGradientAngle = 90;
			break;
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
		Office2007MenuColorTable office2007MenuColorTable = _2108();
		if (office2007MenuColorTable != null)
		{
			topContainer.LayoutOrientation = eOrientation.Vertical;
			topContainer.BackgroundStyle.PaddingBottom = 3;
			topContainer.BackgroundStyle.PaddingLeft = 3;
			topContainer.BackgroundStyle.PaddingRight = 3;
			topContainer.BackgroundStyle.PaddingTop = 8;
			BackgroundColorBlend[] array = new BackgroundColorBlend[office2007MenuColorTable.FileBackgroundBlend.Count];
			office2007MenuColorTable.FileBackgroundBlend._0CD0(array);
			topContainer.BackgroundStyle.BackColorBlend.Clear();
			topContainer.BackgroundStyle.BackColorBlend.AddRange(array);
		}
	}

	private static Office2007MenuColorTable _2108()
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
		Office2007MenuColorTable office2007MenuColorTable = _2108();
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
		Office2007MenuColorTable office2007MenuColorTable = _2108();
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
		Office2007MenuColorTable office2007MenuColorTable = _2108();
		if (office2007MenuColorTable != null)
		{
			columnTwo.BackgroundStyle.BackColor = office2007MenuColorTable.FileColumnTwoBackground;
			columnTwo.LayoutOrientation = eOrientation.Vertical;
			columnTwo.MinimumSize = new Size(180, 0);
		}
	}

	public static void SetupMenuBottomContainer(ItemContainer bottomContainer)
	{
		Office2007MenuColorTable office2007MenuColorTable = _2108();
		if (office2007MenuColorTable != null)
		{
			BackgroundColorBlend[] array = new BackgroundColorBlend[office2007MenuColorTable.FileBottomContainerBackgroundBlend.Count];
			office2007MenuColorTable.FileBottomContainerBackgroundBlend._0CD0(array);
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
			Control ᅈ = item;
			_2109(ᅈ, legacyColors);
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

	private static void _2109(Control ᅈ, ColorScheme ጎ)
	{
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Invalid comparison between Unknown and I4
		bool flag = true;
		if (ᅈ is PanelEx && ((PanelEx)(object)ᅈ).ColorSchemeStyle == eDotNetBarStyle.Office2007)
		{
			PanelEx panelEx = ᅈ as PanelEx;
			panelEx.ColorSchemeStyle = eDotNetBarStyle.Office2007;
			ᅈ.Invalidate();
		}
		if (ᅈ is PanelControl && ((PanelControl)(object)ᅈ).ᑦ == eDotNetBarStyle.Office2007)
		{
			PanelControl panelControl = ᅈ as PanelControl;
			panelControl.RefreshStyleSystemColors();
			((Control)panelControl).Invalidate();
		}
		else if (ᅈ is ExpandablePanel && ((ExpandablePanel)(object)ᅈ).ColorSchemeStyle == eDotNetBarStyle.Office2007)
		{
			ᅈ.Invalidate();
		}
		else if (ᅈ is RibbonControl)
		{
			((RibbonControl)(object)ᅈ).RibbonStrip.ඕ();
			ᅈ.Invalidate(true);
		}
		else if (ᅈ is RibbonBar)
		{
			ᅈ.Invalidate();
		}
		else if (ᅈ is TabControl)
		{
			TabControl tabControl = ᅈ as TabControl;
			if (tabControl.Style == eTabStripStyle.Office2007Dock || tabControl.Style == eTabStripStyle.Office2007Document)
			{
				tabControl.Style = tabControl.Style;
			}
		}
		else if (ᅈ is TabStrip)
		{
			TabStrip tabStrip = ᅈ as TabStrip;
			if (tabStrip.Style == eTabStripStyle.Office2007Dock || tabStrip.Style == eTabStripStyle.Office2007Document)
			{
				tabStrip.Style = tabStrip.Style;
			}
		}
		else if (ᅈ is SideBar)
		{
			SideBar sideBar = ᅈ as SideBar;
			if (sideBar.Style == eDotNetBarStyle.Office2007)
			{
				((Control)sideBar).Invalidate();
			}
		}
		else if (ᅈ is RibbonForm)
		{
			ᅈ.BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.BackColor;
			if (ᅈ is OfficeForm)
			{
				((OfficeForm)(object)ᅈ).InvalidateNonClient(invalidateForm: true);
			}
		}
		else if (ᅈ is DockSite && (int)ᅈ.Dock == 1)
		{
			DockSite dockSite = ᅈ as DockSite;
			if (dockSite.Owner != null)
			{
				foreach (Bar bar in dockSite.Owner.Bars)
				{
					if (bar.DockSide == eDockSide.None)
					{
						((Control)bar).Invalidate(true);
						_2109((Control)(object)bar, ጎ);
					}
				}
			}
		}
		else if (ᅈ is ComboBoxEx)
		{
			((ComboBoxEx)(object)ᅈ).Style = ((ComboBoxEx)(object)ᅈ).Style;
		}
		else if (ᅈ is NavigationPane)
		{
			((NavigationPane)(object)ᅈ)._215D();
		}
		else if (ᅈ is BaseItemControl && ((BaseItemControl)(object)ᅈ).Style == eDotNetBarStyle.Office2007)
		{
			ᅈ.Invalidate();
		}
		else if (ᅈ is ListViewEx)
		{
			ListViewEx listViewEx = ᅈ as ListViewEx;
			((Control)listViewEx).Invalidate(true);
			listViewEx.ResetCachedColorTableReference();
		}
		else if (ᅈ is DataGridViewX)
		{
			ᅈ.Invalidate(true);
		}
		else if (ᅈ is _2272 || ᅈ is _225D)
		{
			ᅈ.Invalidate();
		}
		else if (ᅈ is WarningBox)
		{
			((WarningBox)(object)ᅈ).UpdateColorScheme();
		}
		else if (ᅈ is DevComponents.AdvTree.AdvTree)
		{
			ᅈ.Invalidate();
		}
		if (!flag)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)ᅈ.Controls)
		{
			Control ᅈ2 = item;
			_2109(ᅈ2, ጎ);
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
			Form ℋ = item;
			ℊ(newStyle, (Control)(object)ℋ, ℌ: false, blendColor);
		}
	}

	public static void ChangeStyle(eDotNetBarStyle newStyle, Control parentForm)
	{
		ℊ(newStyle, parentForm, ℌ: false, Color.Empty);
	}

	public static void ChangeStyle(eDotNetBarStyle newStyle, Control parentForm, Color blendColor)
	{
		ℊ(newStyle, parentForm, ℌ: false, blendColor);
	}

	private static void ℊ(eDotNetBarStyle ᥙ, Control ℋ, bool ℌ, Color ℍ)
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		if (ᥙ == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(ᥙ))
		{
			if (ℍ.IsEmpty)
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable();
			}
			else
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.Silver, ℍ);
			}
		}
		else
		{
			switch (ᥙ)
			{
			case eDotNetBarStyle.Windows7:
				if (ℍ.IsEmpty)
				{
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable();
				}
				else
				{
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable(eWindows7ColorScheme.Blue, ℍ);
				}
				break;
			case eDotNetBarStyle.Office2007:
				if (ℍ.IsEmpty)
				{
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable();
				}
				else
				{
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(eOffice2007ColorScheme.Blue, ℍ);
				}
				break;
			}
		}
		ColorScheme legacyColors = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
		foreach (Control item in (ArrangedElementCollection)ℋ.Controls)
		{
			Control ᅈ = item;
			ℊ(ᅈ, ᥙ, ℌ, legacyColors);
		}
	}

	private static BaseItem ᐃ(RibbonControl ᤓ)
	{
		BaseItem result = null;
		for (int i = 0; i < ᤓ.QuickToolbarItems.Count; i++)
		{
			if (ᤓ.QuickToolbarItems[i] is ApplicationButton)
			{
				result = ᤓ.QuickToolbarItems[i];
				break;
			}
		}
		return result;
	}

	private static eTabStripStyle ℎ(TabStrip ᆰ, eDotNetBarStyle _0657)
	{
		if (_0657 == eDotNetBarStyle.Office2007 || _0657 == eDotNetBarStyle.Office2010 || _0657 == eDotNetBarStyle.Windows7 || StyleManager.IsMetro(_0657))
		{
			if (ᆰ.Style != eTabStripStyle.Office2007Dock || ᆰ.Style != eTabStripStyle.Office2007Document)
			{
				return eTabStripStyle.Office2007Document;
			}
		}
		else if (_0657 == eDotNetBarStyle.VS2005)
		{
			return eTabStripStyle.VS2005;
		}
		return ᆰ.Style;
	}

	private static void ℊ(Control ᅈ, eDotNetBarStyle _0657, bool ℌ, ColorScheme ጎ)
	{
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Expected O, but got Unknown
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Invalid comparison between Unknown and I4
		eDotNetBarStyle eDotNetBarStyle2 = _0657;
		if (_0657 == eDotNetBarStyle.StyleManagerControlled)
		{
			eDotNetBarStyle2 = StyleManager.GetEffectiveStyle();
		}
		bool flag = true;
		if (ᅈ is PanelEx && ((PanelEx)(object)ᅈ).ColorSchemeStyle == eDotNetBarStyle.Office2007)
		{
			PanelEx panelEx = ᅈ as PanelEx;
			panelEx.ColorSchemeStyle = _0657;
			ᅈ.Invalidate();
		}
		if (ᅈ is PanelControl && ((PanelControl)(object)ᅈ).ColorSchemeStyle == eDotNetBarStyle.Office2007)
		{
			PanelControl panelControl = ᅈ as PanelControl;
			panelControl.ColorSchemeStyle = _0657;
			panelControl.RefreshStyleSystemColors();
			((Control)panelControl).Invalidate();
		}
		else if (ᅈ is ExpandablePanel && ((ExpandablePanel)(object)ᅈ).ColorSchemeStyle == eDotNetBarStyle.Office2007)
		{
			ExpandablePanel expandablePanel = ᅈ as ExpandablePanel;
			expandablePanel.ColorSchemeStyle = _0657;
			ᅈ.Invalidate();
		}
		else if (ᅈ is RibbonControl)
		{
			RibbonControl ribbonControl = (RibbonControl)(object)ᅈ;
			if (eDotNetBarStyle2 == eDotNetBarStyle.Office2010 || eDotNetBarStyle2 == eDotNetBarStyle.Windows7 || StyleManager.IsMetro(eDotNetBarStyle2))
			{
				BaseItem baseItem = ᐃ(ribbonControl);
				if (baseItem != null)
				{
					if (baseItem is ButtonItem && ((ButtonItem)baseItem).ImageFixedSize.IsEmpty)
					{
						((ButtonItem)baseItem).ImageFixedSize = new Size(16, 16);
					}
					ribbonControl.QuickToolbarItems.Remove(baseItem);
					ribbonControl.Items.Insert(0, baseItem);
				}
			}
			else if (eDotNetBarStyle2 == eDotNetBarStyle.Office2007 && ribbonControl.Items.Count > 0 && ribbonControl.Items[0] is ApplicationButton)
			{
				ApplicationButton applicationButton = (ApplicationButton)ribbonControl.Items[0];
				if (!applicationButton.ImageFixedSize.IsEmpty)
				{
					applicationButton.ImageFixedSize = Size.Empty;
				}
				ribbonControl.Items.Remove(applicationButton);
				ribbonControl.QuickToolbarItems.Insert(0, applicationButton);
			}
			if ((_0657 == eDotNetBarStyle.StyleManagerControlled && ribbonControl.Style == _0657) || _0657 != eDotNetBarStyle.StyleManagerControlled)
			{
				ribbonControl.Style = _0657;
			}
			ribbonControl.RibbonStrip.ඕ();
			((Control)ribbonControl).Invalidate(true);
			if (((Control)ribbonControl).IsHandleCreated && ribbonControl.SelectedRibbonTabItem != null && ribbonControl.SelectedRibbonTabItem.Panel != null)
			{
				((Control)ribbonControl.SelectedRibbonTabItem.Panel).PerformLayout();
			}
			flag = false;
		}
		else if (ᅈ is RibbonBar)
		{
			RibbonBar ribbonBar = ᅈ as RibbonBar;
			ribbonBar.Style = _0657;
			ᅈ.Invalidate();
		}
		else if (ᅈ is TabControl)
		{
			TabControl tabControl = ᅈ as TabControl;
			tabControl.Style = ℎ(tabControl.TabStrip, _0657);
		}
		else if (ᅈ is TabStrip)
		{
			TabStrip tabStrip = ᅈ as TabStrip;
			tabStrip.Style = ℎ(tabStrip, _0657);
		}
		else if (ᅈ is SideBar)
		{
			SideBar sideBar = ᅈ as SideBar;
			sideBar.Style = _0657;
			((Control)sideBar).Invalidate();
		}
		else if (ᅈ is RibbonForm)
		{
			ᅈ.BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.BackColor;
			if (ᅈ is OfficeForm)
			{
				((OfficeForm)(object)ᅈ).InvalidateNonClient(invalidateForm: true);
			}
		}
		else if (ᅈ is DockSite && (int)ᅈ.Dock == 1)
		{
			DockSite dockSite = ᅈ as DockSite;
			if (dockSite.Owner != null)
			{
				foreach (Bar bar in dockSite.Owner.Bars)
				{
					if (bar.DockSide == eDockSide.None)
					{
						((Control)bar).Invalidate(true);
						_2109((Control)(object)bar, ጎ);
					}
				}
			}
		}
		else if (ᅈ is ComboBoxEx)
		{
			((ComboBoxEx)(object)ᅈ).Style = _0657;
		}
		else if (ᅈ is NavigationPane)
		{
			NavigationPane navigationPane = (NavigationPane)(object)ᅈ;
			navigationPane.Style = _0657;
			if (BarFunctions.IsOffice2007Style(_0657))
			{
				((NavigationPane)(object)ᅈ)._215D();
			}
		}
		else if (ᅈ is BaseItemControl)
		{
			((BaseItemControl)(object)ᅈ).Style = _0657;
			ᅈ.Invalidate();
		}
		else if (ᅈ is ListViewEx)
		{
			ListViewEx listViewEx = ᅈ as ListViewEx;
			((Control)listViewEx).Invalidate(true);
			listViewEx.ResetCachedColorTableReference();
		}
		else if (ᅈ is DataGridViewX)
		{
			ᅈ.Invalidate(true);
		}
		else if (ᅈ is _2272 || ᅈ is _225D)
		{
			ᅈ.Invalidate();
		}
		else if (ᅈ is WarningBox)
		{
			((WarningBox)(object)ᅈ).UpdateColorScheme();
		}
		else if (ᅈ is DevComponents.AdvTree.AdvTree)
		{
			ᅈ.Invalidate();
		}
		if (!flag)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)ᅈ.Controls)
		{
			Control ᅈ2 = item;
			_2109(ᅈ2, ጎ);
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

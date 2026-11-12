using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal static class SuperTabStyleColorFactory
{
	public static SuperTabColorTable GetColorTable(eSuperTabStyle style, ColorFactory factory)
	{
		return style switch
		{
			eSuperTabStyle.Office2007 => GetOffice2007ColorTable(), 
			eSuperTabStyle.Office2010BackstageBlue => GetOffice2010BackstageColorTable(), 
			eSuperTabStyle.OneNote2007 => GetOneNote2007ColorTable(factory), 
			eSuperTabStyle.VisualStudio2008Dock => GetVisualStudio2008DockColorTable(factory), 
			eSuperTabStyle.VisualStudio2008Document => GetVisualStudio2008DocumentColorTable(factory), 
			eSuperTabStyle.WinMediaPlayer12 => GetWinMediaPlayer12ColorTable(factory), 
			_ => GetOffice2007ColorTable(), 
		};
	}

	public static SuperTabItemColorTable GetItemColorTable(eSuperTabStyle style, ColorFactory factory)
	{
		return style switch
		{
			eSuperTabStyle.Office2007 => GetOffice2007ItemColorTable(), 
			eSuperTabStyle.Office2010BackstageBlue => GetOffice2010BackstageItemColorTable(), 
			eSuperTabStyle.OneNote2007 => GetOneNote2007ItemColorTable(factory), 
			eSuperTabStyle.VisualStudio2008Dock => GetVisualStudio2008DockItemColorTable(factory), 
			eSuperTabStyle.VisualStudio2008Document => GetVisualStudio2008DocumentItemColorTable(factory), 
			eSuperTabStyle.WinMediaPlayer12 => GetWinMediaPlayer12ItemColorTable(factory), 
			_ => GetOffice2007ItemColorTable(), 
		};
	}

	public static SuperTabPanelColorTable GetPanelColorTable(eSuperTabStyle style, ColorFactory factory)
	{
		return style switch
		{
			eSuperTabStyle.Office2007 => GetOffice2007PanelColorTable(), 
			eSuperTabStyle.Office2010BackstageBlue => GetOffice2010BackstagePanelColorTable(), 
			eSuperTabStyle.OneNote2007 => GetOneNote2007PanelColorTable(factory), 
			eSuperTabStyle.VisualStudio2008Dock => GetVisualStudio2008DockPanelColorTable(factory), 
			eSuperTabStyle.VisualStudio2008Document => GetVisualStudio2008DocumentPanelColorTable(factory), 
			eSuperTabStyle.WinMediaPlayer12 => GetWinMediaPlayer12PanelColorTable(factory), 
			_ => GetOffice2007PanelColorTable(), 
		};
	}

	private static SuperTabColorTable GetOffice2007ColorTable()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabColorTable();
		}
		return office2007Renderer.ColorTable.SuperTab;
	}

	private static SuperTabItemColorTable GetOffice2007ItemColorTable()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabItemColorTable();
		}
		return office2007Renderer.ColorTable.SuperTabItem;
	}

	private static SuperTabPanelColorTable GetOffice2007PanelColorTable()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabPanelColorTable();
		}
		return office2007Renderer.ColorTable.SuperTabPanel;
	}

	private static SuperTabColorTable GetOffice2010BackstageColorTable()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabColorTable();
		}
		return office2007Renderer.ColorTable.Backstage;
	}

	internal static SuperTabColorTable GetOffice2010BackstageBlueColorTable(SuperTabColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabColorTable();
		}
		ct.Background = new SuperTabLinearGradientColorTable(factory.GetColor(15003384), factory.GetColor(12308714));
		ct.InnerBorder = factory.GetColor(11254989);
		ct.OuterBorder = factory.GetColor(13293028);
		ct.ControlBoxDefault.Image = factory.GetColor(0);
		ct.ControlBoxMouseOver.Background = factory.GetColor(13561338);
		ct.ControlBoxMouseOver.Border = factory.GetColor(3381759);
		ct.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		ct.ControlBoxPressed.Background = factory.GetColor(12045024);
		ct.ControlBoxPressed.Border = factory.GetColor(3381759);
		ct.ControlBoxPressed.Image = factory.GetColor(255, 0);
		ct.InsertMarker = factory.GetColor(255, 0);
		ct.SelectionMarker = factory.GetColor(255, 0);
		return ct;
	}

	internal static SuperTabColorTable GetVS2010BackstageBlueColorTable(SuperTabColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabColorTable();
		}
		ct.Background = new SuperTabLinearGradientColorTable(factory.GetColor(13292514), factory.GetColor(11450829));
		ct.InnerBorder = factory.GetColor(11254989);
		ct.OuterBorder = factory.GetColor(13293028);
		ct.ControlBoxDefault.Image = factory.GetColor(0);
		ct.ControlBoxMouseOver.Background = factory.GetColor(13561338);
		ct.ControlBoxMouseOver.Border = factory.GetColor(3381759);
		ct.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		ct.ControlBoxPressed.Background = factory.GetColor(12045024);
		ct.ControlBoxPressed.Border = factory.GetColor(3381759);
		ct.ControlBoxPressed.Image = factory.GetColor(255, 0);
		ct.InsertMarker = factory.GetColor(255, 0);
		ct.SelectionMarker = factory.GetColor(255, 0);
		return ct;
	}

	internal static SuperTabColorTable GetOffice2010BackstageBlackColorTable(SuperTabColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabColorTable();
		}
		ct.Background = new SuperTabLinearGradientColorTable(factory.GetColor(7434609), factory.GetColor(3815994));
		ct.InnerBorder = factory.GetColor(2894892);
		ct.OuterBorder = factory.GetColor(4473924);
		ct.ControlBoxDefault.Image = factory.GetColor(16777215);
		ct.ControlBoxMouseOver.Background = factory.GetColor(13561338);
		ct.ControlBoxMouseOver.Border = factory.GetColor(3381759);
		ct.ControlBoxMouseOver.Image = factory.GetColor(0);
		ct.ControlBoxPressed.Background = factory.GetColor(12045024);
		ct.ControlBoxPressed.Border = factory.GetColor(3381759);
		ct.ControlBoxPressed.Image = factory.GetColor(0);
		ct.InsertMarker = factory.GetColor(16777215);
		ct.SelectionMarker = factory.GetColor(16777215);
		return ct;
	}

	internal static SuperTabColorTable GetOffice2010BackstageSilverColorTable(SuperTabColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabColorTable();
		}
		ct.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16514301), factory.GetColor(14738407));
		ct.InnerBorder = factory.GetColor(13422030);
		ct.OuterBorder = factory.GetColor(15001320);
		ct.ControlBoxDefault.Image = factory.GetColor(0);
		ct.ControlBoxMouseOver.Background = factory.GetColor(13561338);
		ct.ControlBoxMouseOver.Border = factory.GetColor(3381759);
		ct.ControlBoxMouseOver.Image = factory.GetColor(0);
		ct.ControlBoxPressed.Background = factory.GetColor(12045024);
		ct.ControlBoxPressed.Border = factory.GetColor(3381759);
		ct.ControlBoxPressed.Image = factory.GetColor(0);
		ct.InsertMarker = factory.GetColor(0);
		ct.SelectionMarker = factory.GetColor(0);
		return ct;
	}

	internal static SuperTabColorTable GetOffice2007BackstageVistaGlassColorTable(SuperTabColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabColorTable();
		}
		ct.Background = new SuperTabLinearGradientColorTable(factory.GetColor(3095119), factory.GetColor(4149102));
		ct.InnerBorder = Color.Empty;
		ct.OuterBorder = factory.GetColor(3885671);
		ct.ControlBoxDefault.Image = factory.GetColor(16777215);
		ct.ControlBoxMouseOver.Background = factory.GetColor(13561338);
		ct.ControlBoxMouseOver.Border = factory.GetColor(3381759);
		ct.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		ct.ControlBoxPressed.Background = factory.GetColor(12045024);
		ct.ControlBoxPressed.Border = factory.GetColor(3381759);
		ct.ControlBoxPressed.Image = factory.GetColor(255, 0);
		ct.InsertMarker = factory.GetColor(255, 16777215);
		ct.SelectionMarker = factory.GetColor(255, 16777215);
		return ct;
	}

	internal static SuperTabColorTable GetMetroBackstageColorTable(SuperTabColorTable ct, ColorFactory factory, MetroPartColors metroColors)
	{
		if (ct == null)
		{
			ct = new SuperTabColorTable();
		}
		ct.Background = new SuperTabLinearGradientColorTable(factory.GetColor(metroColors.BaseColor));
		ct.InnerBorder = Color.Empty;
		ct.OuterBorder = Color.Empty;
		ct.ControlBoxDefault.Image = factory.GetColor(metroColors.BaseTextColor);
		ct.ControlBoxMouseOver.Background = factory.GetColor(metroColors.BaseColorDark);
		ct.ControlBoxMouseOver.Border = factory.GetColor(metroColors.BaseColorDark);
		ct.ControlBoxMouseOver.Image = factory.GetColor(metroColors.BaseTextColor);
		ct.ControlBoxPressed.Background = factory.GetColor(metroColors.BaseColorDark);
		ct.ControlBoxPressed.Border = factory.GetColor(metroColors.BaseColorDark);
		ct.ControlBoxPressed.Image = factory.GetColor(metroColors.BaseTextColor);
		ct.InsertMarker = factory.GetColor(metroColors.BaseTextColor);
		ct.SelectionMarker = Color.Empty;
		return ct;
	}

	private static SuperTabItemColorTable GetOffice2010BackstageItemColorTable()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabItemColorTable();
		}
		return office2007Renderer.ColorTable.BackstageItem;
	}

	internal static SuperTabItemColorTable GetOffice2010BackstageBlueItemColorTable(SuperTabItemColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabItemColorTable();
		}
		ct.Default.Normal.Text = factory.GetColor(0);
		ct.Default.Normal.Background.AdaptiveGradient = false;
		ct.Default.Normal.CloseMarker = factory.GetColor(0);
		ct.Default.Selected.Background = new SuperTabLinearGradientColorTable(factory.GetColor(609709));
		ct.Default.Selected.Background.AdaptiveGradient = false;
		ct.Default.Selected.InnerBorder = Color.Empty;
		ct.Default.Selected.OuterBorder = factory.GetColor(475031);
		ct.Default.Selected.Text = factory.GetColor(16777215);
		ct.Default.Selected.CloseMarker = factory.GetColor(16777215);
		ct.Default.Selected.SelectionMarker = factory.GetColor(16777215);
		ct.Default.SelectedMouseOver = ct.Default.Selected;
		ct.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(14543866));
		ct.Default.MouseOver.Background.AdaptiveGradient = false;
		ct.Default.MouseOver.InnerBorder = factory.GetColor(15791612);
		ct.Default.MouseOver.OuterBorder = factory.GetColor(5406176);
		ct.Default.MouseOver.Text = factory.GetColor(0);
		ct.Default.MouseOver.CloseMarker = factory.GetColor(0);
		ct.Default.Disabled.Text = factory.GetColor(14737632);
		ct.Default.Disabled.Background.AdaptiveGradient = false;
		ct.Default.Disabled.CloseMarker = factory.GetColor(14737632);
		ct.Left = ct.Default;
		ct.Bottom = ct.Default;
		ct.Right = ct.Right;
		return ct;
	}

	internal static SuperTabItemColorTable GetOffice2010BackstageBlackItemColorTable(SuperTabItemColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabItemColorTable();
		}
		ct.Default.Normal.Text = factory.GetColor(16777215);
		ct.Default.Normal.Background.AdaptiveGradient = false;
		ct.Default.Normal.CloseMarker = factory.GetColor(16777215);
		ct.Default.Selected.Background = new SuperTabLinearGradientColorTable(factory.GetColor(609709));
		ct.Default.Selected.Background.AdaptiveGradient = false;
		ct.Default.Selected.InnerBorder = Color.Empty;
		ct.Default.Selected.OuterBorder = factory.GetColor(475031);
		ct.Default.Selected.Text = factory.GetColor(16777215);
		ct.Default.Selected.CloseMarker = factory.GetColor(16777215);
		ct.Default.Selected.SelectionMarker = factory.GetColor(16777215);
		ct.Default.SelectedMouseOver = ct.Default.Selected;
		ct.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(5531005));
		ct.Default.MouseOver.Background.AdaptiveGradient = false;
		ct.Default.MouseOver.InnerBorder = factory.GetColor(5596798);
		ct.Default.MouseOver.OuterBorder = factory.GetColor(4821493);
		ct.Default.MouseOver.Text = factory.GetColor(16777215);
		ct.Default.MouseOver.CloseMarker = factory.GetColor(16777215);
		ct.Left = ct.Default;
		ct.Bottom = ct.Default;
		ct.Right = ct.Right;
		return ct;
	}

	internal static SuperTabItemColorTable GetOffice2010BackstageSilverItemColorTable(SuperTabItemColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabItemColorTable();
		}
		ct.Default.Normal.Text = factory.GetColor(0);
		ct.Default.Normal.Background.AdaptiveGradient = false;
		ct.Default.Normal.CloseMarker = factory.GetColor(0);
		ct.Default.Selected.Background = new SuperTabLinearGradientColorTable(factory.GetColor(609709));
		ct.Default.Selected.Background.AdaptiveGradient = false;
		ct.Default.Selected.InnerBorder = Color.Empty;
		ct.Default.Selected.OuterBorder = factory.GetColor(475031);
		ct.Default.Selected.Text = factory.GetColor(16777215);
		ct.Default.Selected.CloseMarker = factory.GetColor(16777215);
		ct.Default.Selected.SelectionMarker = factory.GetColor(16777215);
		ct.Default.SelectedMouseOver = ct.Default.Selected;
		ct.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(14543866));
		ct.Default.MouseOver.Background.AdaptiveGradient = false;
		ct.Default.MouseOver.InnerBorder = factory.GetColor(15791612);
		ct.Default.MouseOver.OuterBorder = factory.GetColor(5406176);
		ct.Default.MouseOver.Text = factory.GetColor(0);
		ct.Default.MouseOver.CloseMarker = factory.GetColor(0);
		ct.Left = ct.Default;
		ct.Bottom = ct.Default;
		ct.Right = ct.Right;
		return ct;
	}

	internal static SuperTabItemColorTable GetOffice2007BackstageVistaGlassItemColorTable(SuperTabItemColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabItemColorTable();
		}
		ct.Default.Normal.Text = factory.GetColor(16777215);
		ct.Default.Normal.Background.AdaptiveGradient = false;
		ct.Default.Normal.CloseMarker = factory.GetColor(16777215);
		ct.Default.Selected.Background = new SuperTabLinearGradientColorTable(factory.GetColor(5865439), factory.GetColor(3037103));
		ct.Default.Selected.Background.AdaptiveGradient = false;
		ct.Default.Selected.InnerBorder = factory.GetColor(6851052);
		ct.Default.Selected.OuterBorder = factory.GetColor(2370877);
		ct.Default.Selected.Text = factory.GetColor(16777215);
		ct.Default.Selected.CloseMarker = factory.GetColor(16777215);
		ct.Default.Selected.SelectionMarker = factory.GetColor(16777215);
		ct.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(5865439), factory.GetColor(3037103));
		ct.Default.SelectedMouseOver.Background.AdaptiveGradient = false;
		ct.Default.SelectedMouseOver.InnerBorder = factory.GetColor(6851052);
		ct.Default.SelectedMouseOver.OuterBorder = factory.GetColor(2370877);
		ct.Default.SelectedMouseOver.Text = factory.GetColor(16777215);
		ct.Default.SelectedMouseOver.CloseMarker = factory.GetColor(16777215);
		ct.Default.SelectedMouseOver.SelectionMarker = factory.GetColor(16777215);
		ct.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			factory.GetColor(3886972),
			factory.GetColor(3557490),
			factory.GetColor(3557490),
			factory.GetColor(2701399)
		}, new float[4] { 0f, 95f, 0.95f, 1f });
		ct.Default.MouseOver.Background.AdaptiveGradient = false;
		ct.Default.MouseOver.InnerBorder = factory.GetColor(7576319);
		ct.Default.MouseOver.OuterBorder = factory.GetColor(728660);
		ct.Default.MouseOver.Text = factory.GetColor(16777215);
		ct.Default.MouseOver.CloseMarker = factory.GetColor(16777215);
		ct.Left = ct.Default;
		ct.Bottom = ct.Default;
		ct.Right = ct.Right;
		return ct;
	}

	internal static SuperTabItemColorTable GetMetroBackstageItemColorTable(SuperTabItemColorTable ct, ColorFactory factory, MetroPartColors metroColors)
	{
		if (ct == null)
		{
			ct = new SuperTabItemColorTable();
		}
		ct.Default.Normal.Text = factory.GetColor(metroColors.BaseTextColor);
		ct.Default.Normal.Background.AdaptiveGradient = false;
		ct.Default.Normal.CloseMarker = factory.GetColor(metroColors.BaseTextColor);
		ct.Default.Selected.Background = new SuperTabLinearGradientColorTable(factory.GetColor(metroColors.BaseColorLight1));
		ct.Default.Selected.Background.AdaptiveGradient = false;
		ct.Default.Selected.InnerBorder = Color.Empty;
		ct.Default.Selected.OuterBorder = Color.Empty;
		ct.Default.Selected.Text = factory.GetColor(metroColors.BaseTextColor);
		ct.Default.Selected.CloseMarker = factory.GetColor(metroColors.BaseTextColor);
		ct.Default.Selected.SelectionMarker = Color.Empty;
		ct.Default.SelectedMouseOver = new SuperTabItemStateColorTable();
		ct.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		ct.Default.SelectedMouseOver.Background.AdaptiveGradient = false;
		ct.Default.SelectedMouseOver.InnerBorder = Color.Empty;
		ct.Default.SelectedMouseOver.OuterBorder = Color.Empty;
		ct.Default.SelectedMouseOver.Text = factory.GetColor(metroColors.BaseTextColor);
		ct.Default.SelectedMouseOver.CloseMarker = factory.GetColor(metroColors.BaseTextColor);
		ct.Default.SelectedMouseOver.SelectionMarker = Color.Empty;
		ct.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(metroColors.BaseColorDark));
		ct.Default.MouseOver.Background.AdaptiveGradient = false;
		ct.Default.MouseOver.InnerBorder = Color.Empty;
		ct.Default.MouseOver.OuterBorder = Color.Empty;
		ct.Default.MouseOver.Text = factory.GetColor(metroColors.BaseTextColor);
		ct.Default.MouseOver.CloseMarker = factory.GetColor(metroColors.BaseTextColor);
		ct.Default.Disabled.Text = factory.GetColor(metroColors.TextDisabledColor);
		ct.Default.Disabled.Background.AdaptiveGradient = false;
		ct.Default.Disabled.CloseMarker = factory.GetColor(metroColors.TextDisabledColor);
		ct.Left = ct.Default;
		ct.Bottom = ct.Default;
		ct.Right = ct.Right;
		return ct;
	}

	private static SuperTabPanelColorTable GetOffice2010BackstagePanelColorTable()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabPanelColorTable();
		}
		return office2007Renderer.ColorTable.BackstagePanel;
	}

	internal static SuperTabPanelColorTable GetOffice2010BackstageBluePanelColorTable(SuperTabPanelColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabPanelColorTable();
		}
		ct.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(16777215));
		ct.Default.InnerBorder = Color.Empty;
		ct.Default.OuterBorder = factory.GetColor(11254989);
		ct.Default.Background.AdaptiveGradient = false;
		ct.Left = ct.Default;
		ct.Bottom = ct.Default;
		ct.Right = ct.Default;
		return ct;
	}

	internal static SuperTabPanelColorTable GetOffice2010BackstageBlackPanelColorTable(SuperTabPanelColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabPanelColorTable();
		}
		ct.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(16777215));
		ct.Default.InnerBorder = Color.Empty;
		ct.Default.OuterBorder = factory.GetColor(2894892);
		ct.Default.Background.AdaptiveGradient = false;
		ct.Left = ct.Default;
		ct.Bottom = ct.Default;
		ct.Right = ct.Default;
		return ct;
	}

	internal static SuperTabPanelColorTable GetOffice2010BackstageSilverPanelColorTable(SuperTabPanelColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabPanelColorTable();
		}
		ct.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(16777215));
		ct.Default.InnerBorder = Color.Empty;
		ct.Default.OuterBorder = factory.GetColor(13422030);
		ct.Default.Background.AdaptiveGradient = false;
		ct.Left = ct.Default;
		ct.Bottom = ct.Default;
		ct.Right = ct.Default;
		return ct;
	}

	internal static SuperTabPanelColorTable GetOffice2007BackstageVistaGlassPanelColorTable(SuperTabPanelColorTable ct, ColorFactory factory)
	{
		if (ct == null)
		{
			ct = new SuperTabPanelColorTable();
		}
		ct.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(16777215));
		ct.Default.OuterBorder = factory.GetColor(4149102);
		ct.Default.InnerBorder = factory.GetColor(4149102);
		ct.Default.Background.AdaptiveGradient = false;
		ct.Left = ct.Default;
		ct.Bottom = ct.Default;
		ct.Right = ct.Default;
		return ct;
	}

	internal static SuperTabPanelColorTable GetMetroBackstagePanelColorTable(SuperTabPanelColorTable ct, ColorFactory factory, MetroPartColors metroColors)
	{
		if (ct == null)
		{
			ct = new SuperTabPanelColorTable();
		}
		ct.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(metroColors.CanvasColor), factory.GetColor(metroColors.CanvasColor));
		ct.Default.InnerBorder = Color.Empty;
		ct.Default.OuterBorder = factory.GetColor(metroColors.CanvasColor);
		ct.Default.Background.AdaptiveGradient = false;
		ct.Left = ct.Default;
		ct.Bottom = ct.Default;
		ct.Right = ct.Default;
		return ct;
	}

	private static SuperTabColorTable GetOneNote2007ColorTable(ColorFactory factory)
	{
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.Background = new SuperTabLinearGradientColorTable(Color.White);
		superTabColorTable.InnerBorder = factory.GetColor(16777215);
		superTabColorTable.OuterBorder = factory.GetColor(8158356);
		superTabColorTable.ControlBoxDefault.Image = factory.GetColor(255, 0);
		superTabColorTable.ControlBoxMouseOver.Background = factory.GetColor(16770978);
		superTabColorTable.ControlBoxMouseOver.Border = factory.GetColor(16760169);
		superTabColorTable.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		superTabColorTable.ControlBoxPressed.Background = factory.GetColor(16485436);
		superTabColorTable.ControlBoxPressed.Border = factory.GetColor(16760169);
		superTabColorTable.ControlBoxPressed.Image = factory.GetColor(255, 0);
		superTabColorTable.InsertMarker = factory.GetColor(255, 128);
		return superTabColorTable;
	}

	private static SuperTabItemColorTable GetOneNote2007ItemColorTable(ColorFactory factory)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default.Normal = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Normal.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			factory.GetColor(14805497),
			factory.GetColor(13162227),
			factory.GetColor(9875943),
			factory.GetColor(9875943)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		superTabItemColorTable.Default.Normal.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Default.Normal.OuterBorder = factory.GetColor(7763090);
		superTabItemColorTable.Default.Normal.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Default.Normal.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Default.Selected = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			factory.GetColor(15791355),
			factory.GetColor(14410999),
			factory.GetColor(12965106),
			factory.GetColor(12965106)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		superTabItemColorTable.Default.Selected.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Default.Selected.OuterBorder = factory.GetColor(8158356);
		superTabItemColorTable.Default.Selected.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Default.Selected.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Default.SelectedMouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16776176), factory.GetColor(16773320));
		superTabItemColorTable.Default.SelectedMouseOver.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Default.SelectedMouseOver.OuterBorder = factory.GetColor(4934511);
		superTabItemColorTable.Default.SelectedMouseOver.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Default.SelectedMouseOver.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Default.MouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16776176), factory.GetColor(16773320));
		superTabItemColorTable.Default.MouseOver.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Default.MouseOver.OuterBorder = factory.GetColor(4934511);
		superTabItemColorTable.Default.MouseOver.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Default.MouseOver.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Left.Normal = new SuperTabItemStateColorTable();
		superTabItemColorTable.Left.Normal.Background = new SuperTabLinearGradientColorTable(factory.GetColor(14805497), factory.GetColor(9875943));
		superTabItemColorTable.Left.Normal.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Left.Normal.OuterBorder = factory.GetColor(7763090);
		superTabItemColorTable.Left.Normal.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Left.Normal.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Left.Selected = new SuperTabItemStateColorTable();
		superTabItemColorTable.Left.Selected.Background = new SuperTabLinearGradientColorTable(factory.GetColor(15791355), factory.GetColor(12965106));
		superTabItemColorTable.Left.Selected.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Left.Selected.OuterBorder = factory.GetColor(8158356);
		superTabItemColorTable.Left.Selected.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Left.Selected.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Left.SelectedMouseOver = superTabItemColorTable.Default.SelectedMouseOver;
		superTabItemColorTable.Left.MouseOver = superTabItemColorTable.Default.MouseOver;
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		superTabItemColorTable.Right = superTabItemColorTable.Left;
		return superTabItemColorTable;
	}

	private static SuperTabPanelColorTable GetOneNote2007PanelColorTable(ColorFactory factory)
	{
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			factory.GetColor(12899313),
			factory.GetColor(9087204),
			factory.GetColor(9087204),
			factory.GetColor(12899313)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		superTabPanelColorTable.Default.InnerBorder = factory.GetColor(16777215);
		superTabPanelColorTable.Default.OuterBorder = factory.GetColor(8158356);
		superTabPanelColorTable.Left.Background = new SuperTabLinearGradientColorTable(factory.GetColor(12899313), factory.GetColor(12899313));
		superTabPanelColorTable.Left.InnerBorder = factory.GetColor(16777215);
		superTabPanelColorTable.Left.OuterBorder = factory.GetColor(8158356);
		superTabPanelColorTable.Bottom = superTabPanelColorTable.Default;
		superTabPanelColorTable.Right = superTabPanelColorTable.Left;
		return superTabPanelColorTable;
	}

	private static SuperTabColorTable GetVisualStudio2008DockColorTable(ColorFactory factory)
	{
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.Background = new SuperTabLinearGradientColorTable(Color.White);
		superTabColorTable.InnerBorder = factory.GetColor(16777215);
		superTabColorTable.OuterBorder = factory.GetColor(8158356);
		superTabColorTable.ControlBoxDefault.Image = factory.GetColor(255, 0);
		superTabColorTable.ControlBoxMouseOver.Background = factory.GetColor(13561338);
		superTabColorTable.ControlBoxMouseOver.Border = factory.GetColor(3381759);
		superTabColorTable.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		superTabColorTable.ControlBoxPressed.Background = factory.GetColor(12045024);
		superTabColorTable.ControlBoxPressed.Border = factory.GetColor(3381759);
		superTabColorTable.ControlBoxPressed.Image = factory.GetColor(255, 0);
		superTabColorTable.InsertMarker = factory.GetColor(255, 128);
		return superTabColorTable;
	}

	private static SuperTabItemColorTable GetVisualStudio2008DockItemColorTable(ColorFactory factory)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default.Normal = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Normal.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			factory.GetColor(15921906),
			factory.GetColor(15461355),
			factory.GetColor(14540253),
			factory.GetColor(13619151)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		superTabItemColorTable.Default.Normal.InnerBorder = factory.GetColor(16579836);
		superTabItemColorTable.Default.Normal.OuterBorder = factory.GetColor(9014421);
		superTabItemColorTable.Default.Normal.Text = factory.GetColor(5592420);
		superTabItemColorTable.Default.Normal.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Default.Selected = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Selected.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16777215));
		superTabItemColorTable.Default.Selected.InnerBorder = Color.Empty;
		superTabItemColorTable.Default.Selected.OuterBorder = factory.GetColor(9014421);
		superTabItemColorTable.Default.Selected.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Default.Selected.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Default.SelectedMouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16777215));
		superTabItemColorTable.Default.SelectedMouseOver.InnerBorder = Color.Empty;
		superTabItemColorTable.Default.SelectedMouseOver.OuterBorder = factory.GetColor(9014421);
		superTabItemColorTable.Default.SelectedMouseOver.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Default.SelectedMouseOver.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Default.MouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			factory.GetColor(15398653),
			factory.GetColor(14283004),
			factory.GetColor(12510973),
			factory.GetColor(11000309)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		superTabItemColorTable.Default.MouseOver.InnerBorder = factory.GetColor(16449022);
		superTabItemColorTable.Default.MouseOver.OuterBorder = factory.GetColor(3964849);
		superTabItemColorTable.Default.MouseOver.Text = factory.GetColor(5592420);
		superTabItemColorTable.Default.MouseOver.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		superTabItemColorTable.Left = (SuperTabColorStates)superTabItemColorTable.Default.Clone();
		superTabItemColorTable.Left.Normal.Background.AdaptiveGradient = false;
		superTabItemColorTable.Left.MouseOver.Background.AdaptiveGradient = false;
		superTabItemColorTable.Left.Selected.Background.AdaptiveGradient = false;
		superTabItemColorTable.Left.SelectedMouseOver.Background.AdaptiveGradient = false;
		superTabItemColorTable.Right = superTabItemColorTable.Left;
		return superTabItemColorTable;
	}

	private static SuperTabPanelColorTable GetVisualStudio2008DockPanelColorTable(ColorFactory factory)
	{
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16777215));
		superTabPanelColorTable.Default.InnerBorder = factory.GetColor(16777215);
		superTabPanelColorTable.Default.OuterBorder = factory.GetColor(9014421);
		superTabPanelColorTable.Left = superTabPanelColorTable.Default;
		superTabPanelColorTable.Bottom = superTabPanelColorTable.Default;
		superTabPanelColorTable.Right = superTabPanelColorTable.Default;
		return superTabPanelColorTable;
	}

	private static SuperTabColorTable GetVisualStudio2008DocumentColorTable(ColorFactory factory)
	{
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.Background = new SuperTabLinearGradientColorTable(Color.White);
		superTabColorTable.InnerBorder = factory.GetColor(16777215);
		superTabColorTable.OuterBorder = factory.GetColor(6922687);
		superTabColorTable.ControlBoxDefault.Image = factory.GetColor(255, 0);
		superTabColorTable.ControlBoxMouseOver.Background = factory.GetColor(13561338);
		superTabColorTable.ControlBoxMouseOver.Border = factory.GetColor(3381759);
		superTabColorTable.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		superTabColorTable.ControlBoxPressed.Background = factory.GetColor(12045024);
		superTabColorTable.ControlBoxPressed.Border = factory.GetColor(3381759);
		superTabColorTable.ControlBoxPressed.Image = factory.GetColor(255, 0);
		superTabColorTable.InsertMarker = factory.GetColor(255, 128);
		return superTabColorTable;
	}

	private static SuperTabItemColorTable GetVisualStudio2008DocumentItemColorTable(ColorFactory factory)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default.Normal = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Normal.Background = new SuperTabLinearGradientColorTable(factory.GetColor(15135225), factory.GetColor(10007762));
		superTabItemColorTable.Default.Normal.InnerBorder = factory.GetColor(15923967);
		superTabItemColorTable.Default.Normal.OuterBorder = factory.GetColor(9541282);
		superTabItemColorTable.Default.Normal.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Default.Normal.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Default.Selected = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Selected.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16580094), factory.GetColor(13821690));
		superTabItemColorTable.Default.Selected.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Default.Selected.OuterBorder = factory.GetColor(6922687);
		superTabItemColorTable.Default.Selected.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Default.Selected.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Default.SelectedMouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16580094), factory.GetColor(13821690));
		superTabItemColorTable.Default.SelectedMouseOver.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Default.SelectedMouseOver.OuterBorder = factory.GetColor(6922687);
		superTabItemColorTable.Default.SelectedMouseOver.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Default.SelectedMouseOver.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Default.MouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16252158), factory.GetColor(8507633));
		superTabItemColorTable.Default.MouseOver.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Default.MouseOver.OuterBorder = factory.GetColor(6922687);
		superTabItemColorTable.Default.MouseOver.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Default.MouseOver.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Left = superTabItemColorTable.Default;
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		superTabItemColorTable.Right = superTabItemColorTable.Default;
		return superTabItemColorTable;
	}

	private static SuperTabPanelColorTable GetVisualStudio2008DocumentPanelColorTable(ColorFactory factory)
	{
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(13821690));
		superTabPanelColorTable.Default.InnerBorder = factory.GetColor(16777215);
		superTabPanelColorTable.Default.OuterBorder = factory.GetColor(6922687);
		superTabPanelColorTable.Left = superTabPanelColorTable.Default;
		superTabPanelColorTable.Bottom = superTabPanelColorTable.Default;
		superTabPanelColorTable.Right = superTabPanelColorTable.Default;
		return superTabPanelColorTable;
	}

	private static SuperTabColorTable GetWinMediaPlayer12ColorTable(ColorFactory factory)
	{
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.Background = new SuperTabLinearGradientColorTable(factory.GetColor(14806007));
		superTabColorTable.InnerBorder = factory.GetColor(16777215);
		superTabColorTable.OuterBorder = factory.GetColor(12372957);
		superTabColorTable.ControlBoxDefault.Image = factory.GetColor(255, 0);
		superTabColorTable.ControlBoxMouseOver.Background = factory.GetColor(13561338);
		superTabColorTable.ControlBoxMouseOver.Border = factory.GetColor(3381759);
		superTabColorTable.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		superTabColorTable.ControlBoxPressed.Background = factory.GetColor(12045024);
		superTabColorTable.ControlBoxPressed.Border = factory.GetColor(3381759);
		superTabColorTable.ControlBoxPressed.Image = factory.GetColor(255, 0);
		superTabColorTable.InsertMarker = factory.GetColor(255, 128);
		return superTabColorTable;
	}

	private static SuperTabItemColorTable GetWinMediaPlayer12ItemColorTable(ColorFactory factory)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default.Normal = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Normal.Background = new SuperTabLinearGradientColorTable(factory.GetColor(14278888), factory.GetColor(15397626));
		superTabItemColorTable.Default.Normal.Background.AdaptiveGradient = false;
		superTabItemColorTable.Default.Normal.InnerBorder = factory.GetColor(15658735);
		superTabItemColorTable.Default.Normal.OuterBorder = factory.GetColor(10926800);
		superTabItemColorTable.Default.Normal.Text = factory.GetColor(255, 0);
		superTabItemColorTable.Default.Normal.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Default.Selected = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Selected.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16054783), factory.GetColor(15726333));
		superTabItemColorTable.Default.Selected.Background.AdaptiveGradient = false;
		superTabItemColorTable.Default.Selected.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Default.Selected.OuterBorder = factory.GetColor(12372957);
		superTabItemColorTable.Default.Selected.Text = factory.GetColor(10595);
		superTabItemColorTable.Default.Selected.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Default.SelectedMouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(15923453));
		superTabItemColorTable.Default.SelectedMouseOver.Background.AdaptiveGradient = false;
		superTabItemColorTable.Default.SelectedMouseOver.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Default.SelectedMouseOver.OuterBorder = factory.GetColor(12372957);
		superTabItemColorTable.Default.SelectedMouseOver.Text = factory.GetColor(10595);
		superTabItemColorTable.Default.SelectedMouseOver.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Default.MouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(15923453));
		superTabItemColorTable.Default.MouseOver.Background.AdaptiveGradient = false;
		superTabItemColorTable.Default.MouseOver.InnerBorder = factory.GetColor(16777215);
		superTabItemColorTable.Default.MouseOver.OuterBorder = factory.GetColor(12372957);
		superTabItemColorTable.Default.MouseOver.Text = factory.GetColor(10595);
		superTabItemColorTable.Default.MouseOver.CloseMarker = factory.GetColor(1722485);
		superTabItemColorTable.Left = superTabItemColorTable.Default;
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		superTabItemColorTable.Right = superTabItemColorTable.Default;
		return superTabItemColorTable;
	}

	private static SuperTabPanelColorTable GetWinMediaPlayer12PanelColorTable(ColorFactory factory)
	{
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(15726333));
		superTabPanelColorTable.Default.Background.AdaptiveGradient = false;
		superTabPanelColorTable.Default.InnerBorder = factory.GetColor(16777215);
		superTabPanelColorTable.Default.OuterBorder = factory.GetColor(12372957);
		superTabPanelColorTable.Left = superTabPanelColorTable.Default;
		superTabPanelColorTable.Bottom = superTabPanelColorTable.Default;
		superTabPanelColorTable.Right = superTabPanelColorTable.Default;
		return superTabPanelColorTable;
	}

	public static SuperTabItemColorTable GetPredefinedTabColors(eTabItemColor c, ColorFactory factory)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default = new SuperTabColorStates();
		superTabItemColorTable.Default.Normal.Text = Color.Black;
		superTabItemColorTable.Default.Normal.InnerBorder = Color.White;
		superTabItemColorTable.Default.Normal.OuterBorder = Color.DimGray;
		superTabItemColorTable.Default.Normal.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Default.Normal.Background = GetPredefinedDefaultBackground(c, factory);
		superTabItemColorTable.Default.Selected.Text = Color.Black;
		superTabItemColorTable.Default.Selected.InnerBorder = Color.White;
		superTabItemColorTable.Default.Selected.OuterBorder = Color.DimGray;
		superTabItemColorTable.Default.Selected.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Default.Selected.SelectionMarker = factory.GetColor(255, 16777215);
		superTabItemColorTable.Default.Selected.Background = GetPredefinedSelectedBackground(c, factory);
		superTabItemColorTable.Default.MouseOver.Text = Color.Black;
		superTabItemColorTable.Default.MouseOver.InnerBorder = Color.White;
		superTabItemColorTable.Default.MouseOver.OuterBorder = Color.DimGray;
		superTabItemColorTable.Default.MouseOver.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16776176), factory.GetColor(16773320));
		superTabItemColorTable.Default.SelectedMouseOver.Text = Color.Black;
		superTabItemColorTable.Default.SelectedMouseOver.InnerBorder = Color.White;
		superTabItemColorTable.Default.SelectedMouseOver.OuterBorder = Color.DimGray;
		superTabItemColorTable.Default.SelectedMouseOver.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Default.SelectedMouseOver.SelectionMarker = factory.GetColor(255, 16777215);
		superTabItemColorTable.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(16776176), factory.GetColor(16773320));
		superTabItemColorTable.Left = superTabItemColorTable.Default;
		superTabItemColorTable.Right = superTabItemColorTable.Default;
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		return superTabItemColorTable;
	}

	public static SuperTabPanelItemColorTable GetPredefinedPanelColors(eTabItemColor c, ColorFactory factory)
	{
		SuperTabPanelItemColorTable superTabPanelItemColorTable = new SuperTabPanelItemColorTable();
		SuperTabLinearGradientColorTable predefinedSelectedBackground = GetPredefinedSelectedBackground(c, factory);
		superTabPanelItemColorTable.Background = new SuperTabLinearGradientColorTable(predefinedSelectedBackground.Colors[1]);
		superTabPanelItemColorTable.InnerBorder = Color.White;
		superTabPanelItemColorTable.OuterBorder = Color.DimGray;
		return superTabPanelItemColorTable;
	}

	private static SuperTabLinearGradientColorTable GetPredefinedDefaultBackground(eTabItemColor c, ColorFactory factory)
	{
		return c switch
		{
			eTabItemColor.Apple => new SuperTabLinearGradientColorTable(factory.GetColor(14349517), factory.GetColor(11659416)), 
			eTabItemColor.Blue => new SuperTabLinearGradientColorTable(factory.GetColor(14542583), factory.GetColor(9087204)), 
			eTabItemColor.BlueMist => new SuperTabLinearGradientColorTable(factory.GetColor(13820139), factory.GetColor(10600404)), 
			eTabItemColor.Cyan => new SuperTabLinearGradientColorTable(factory.GetColor(13492187), factory.GetColor(9944754)), 
			eTabItemColor.Green => new SuperTabLinearGradientColorTable(factory.GetColor(14608079), factory.GetColor(12307613)), 
			eTabItemColor.Lemon => new SuperTabLinearGradientColorTable(factory.GetColor(16514493), factory.GetColor(16120183)), 
			eTabItemColor.Magenta => new SuperTabLinearGradientColorTable(factory.GetColor(15521504), factory.GetColor(14133695)), 
			eTabItemColor.Orange => new SuperTabLinearGradientColorTable(factory.GetColor(16505794), factory.GetColor(16168063)), 
			eTabItemColor.Purple => new SuperTabLinearGradientColorTable(factory.GetColor(14537713), factory.GetColor(12100576)), 
			eTabItemColor.PurpleMist => new SuperTabLinearGradientColorTable(factory.GetColor(14275294), factory.GetColor(11510203)), 
			eTabItemColor.Red => new SuperTabLinearGradientColorTable(factory.GetColor(16240591), factory.GetColor(15702941)), 
			eTabItemColor.Silver => new SuperTabLinearGradientColorTable(factory.GetColor(13619161), factory.GetColor(10197934)), 
			eTabItemColor.Tan => new SuperTabLinearGradientColorTable(factory.GetColor(16116688), factory.GetColor(15324319)), 
			eTabItemColor.Teal => new SuperTabLinearGradientColorTable(factory.GetColor(13495536), factory.GetColor(5160138)), 
			_ => new SuperTabLinearGradientColorTable(factory.GetColor(16774617), factory.GetColor(16768120)), 
		};
	}

	private static SuperTabLinearGradientColorTable GetPredefinedSelectedBackground(eTabItemColor c, ColorFactory factory)
	{
		return c switch
		{
			eTabItemColor.Apple => new SuperTabLinearGradientColorTable(factory.GetColor(15596263), factory.GetColor(14087112)), 
			eTabItemColor.Blue => new SuperTabLinearGradientColorTable(factory.GetColor(14542583), factory.GetColor(9087204)), 
			eTabItemColor.BlueMist => new SuperTabLinearGradientColorTable(factory.GetColor(15331573), factory.GetColor(13491688)), 
			eTabItemColor.Cyan => new SuperTabLinearGradientColorTable(factory.GetColor(15200494), factory.GetColor(13163734)), 
			eTabItemColor.Green => new SuperTabLinearGradientColorTable(factory.GetColor(15791080), factory.GetColor(14410955)), 
			eTabItemColor.Lemon => new SuperTabLinearGradientColorTable(factory.GetColor(16645855), factory.GetColor(16448695)), 
			eTabItemColor.Magenta => new SuperTabLinearGradientColorTable(factory.GetColor(16182256), factory.GetColor(15389149)), 
			eTabItemColor.Orange => new SuperTabLinearGradientColorTable(factory.GetColor(16641761), factory.GetColor(16439227)), 
			eTabItemColor.Purple => new SuperTabLinearGradientColorTable(factory.GetColor(15723255), factory.GetColor(14274286)), 
			eTabItemColor.PurpleMist => new SuperTabLinearGradientColorTable(factory.GetColor(15526384), factory.GetColor(14011867)), 
			eTabItemColor.Red => new SuperTabLinearGradientColorTable(factory.GetColor(16574696), factory.GetColor(16173771)), 
			eTabItemColor.Silver => new SuperTabLinearGradientColorTable(factory.GetColor(15263980), factory.GetColor(13290196)), 
			eTabItemColor.Tan => new SuperTabLinearGradientColorTable(factory.GetColor(16446953), factory.GetColor(15984844)), 
			eTabItemColor.Teal => new SuperTabLinearGradientColorTable(factory.GetColor(13495536), factory.GetColor(5160138)), 
			_ => new SuperTabLinearGradientColorTable(factory.GetColor(16775916), factory.GetColor(16771763)), 
		};
	}
}

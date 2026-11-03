using System.Drawing;
using DevComponents.DotNetBar.Metro.ColorTables;

namespace DevComponents.DotNetBar.Rendering;

internal static class ጦ
{
	public static SuperTabColorTable ጧ(eSuperTabStyle _0657, ColorFactory ቖ)
	{
		return _0657 switch
		{
			eSuperTabStyle.Office2007 => ጪ(), 
			eSuperTabStyle.Office2010BackstageBlue => ጭ(), 
			eSuperTabStyle.OneNote2007 => ፁ(ቖ), 
			eSuperTabStyle.VisualStudio2008Dock => ፇ(ቖ), 
			eSuperTabStyle.VisualStudio2008Document => ፊ(ቖ), 
			eSuperTabStyle.WinMediaPlayer12 => ፍ(ቖ), 
			eSuperTabStyle.OfficeMobile2014 => ፄ(ቖ), 
			_ => ጪ(), 
		};
	}

	public static SuperTabItemColorTable ጨ(eSuperTabStyle _0657, ColorFactory ቖ)
	{
		return _0657 switch
		{
			eSuperTabStyle.Office2007 => ጫ(), 
			eSuperTabStyle.Office2010BackstageBlue => ጵ(), 
			eSuperTabStyle.OneNote2007 => ፂ(ቖ), 
			eSuperTabStyle.VisualStudio2008Dock => ፈ(ቖ), 
			eSuperTabStyle.VisualStudio2008Document => ፋ(ቖ), 
			eSuperTabStyle.WinMediaPlayer12 => ፎ(ቖ), 
			eSuperTabStyle.OfficeMobile2014 => ፅ(ቖ), 
			_ => ጫ(), 
		};
	}

	public static SuperTabPanelColorTable ጩ(eSuperTabStyle _0657, ColorFactory ቖ)
	{
		return _0657 switch
		{
			eSuperTabStyle.Office2007 => ጬ(), 
			eSuperTabStyle.Office2010BackstageBlue => ጻ(), 
			eSuperTabStyle.OneNote2007 => ፃ(ቖ), 
			eSuperTabStyle.VisualStudio2008Dock => ፉ(ቖ), 
			eSuperTabStyle.VisualStudio2008Document => ፌ(ቖ), 
			eSuperTabStyle.WinMediaPlayer12 => ፏ(ቖ), 
			eSuperTabStyle.OfficeMobile2014 => ፆ(ቖ), 
			_ => ጬ(), 
		};
	}

	private static SuperTabColorTable ጪ()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabColorTable();
		}
		return office2007Renderer.ColorTable.SuperTab;
	}

	private static SuperTabItemColorTable ጫ()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabItemColorTable();
		}
		return office2007Renderer.ColorTable.SuperTabItem;
	}

	private static SuperTabPanelColorTable ጬ()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabPanelColorTable();
		}
		return office2007Renderer.ColorTable.SuperTabPanel;
	}

	private static SuperTabColorTable ጭ()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabColorTable();
		}
		return office2007Renderer.ColorTable.Backstage;
	}

	internal static SuperTabColorTable ጮ(SuperTabColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabColorTable();
		}
		ዅ.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(15003384), ቖ.GetColor(12308714));
		ዅ.InnerBorder = ቖ.GetColor(11254989);
		ዅ.OuterBorder = ቖ.GetColor(13293028);
		ዅ.ControlBoxDefault.Image = ቖ.GetColor(0);
		ዅ.ControlBoxMouseOver.Background = ቖ.GetColor(13561338);
		ዅ.ControlBoxMouseOver.Border = ቖ.GetColor(3381759);
		ዅ.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		ዅ.ControlBoxPressed.Background = ቖ.GetColor(12045024);
		ዅ.ControlBoxPressed.Border = ቖ.GetColor(3381759);
		ዅ.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		ዅ.InsertMarker = ቖ.GetColor(255, 0);
		ዅ.SelectionMarker = ቖ.GetColor(255, 0);
		return ዅ;
	}

	internal static SuperTabColorTable ጯ(SuperTabColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabColorTable();
		}
		ዅ.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(13292514), ቖ.GetColor(11450829));
		ዅ.InnerBorder = ቖ.GetColor(11254989);
		ዅ.OuterBorder = ቖ.GetColor(13293028);
		ዅ.ControlBoxDefault.Image = ቖ.GetColor(0);
		ዅ.ControlBoxMouseOver.Background = ቖ.GetColor(13561338);
		ዅ.ControlBoxMouseOver.Border = ቖ.GetColor(3381759);
		ዅ.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		ዅ.ControlBoxPressed.Background = ቖ.GetColor(12045024);
		ዅ.ControlBoxPressed.Border = ቖ.GetColor(3381759);
		ዅ.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		ዅ.InsertMarker = ቖ.GetColor(255, 0);
		ዅ.SelectionMarker = ቖ.GetColor(255, 0);
		return ዅ;
	}

	internal static SuperTabColorTable ጰ(SuperTabColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabColorTable();
		}
		ዅ.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(7434609), ቖ.GetColor(3815994));
		ዅ.InnerBorder = ቖ.GetColor(2894892);
		ዅ.OuterBorder = ቖ.GetColor(4473924);
		ዅ.ControlBoxDefault.Image = ቖ.GetColor(16777215);
		ዅ.ControlBoxMouseOver.Background = ቖ.GetColor(13561338);
		ዅ.ControlBoxMouseOver.Border = ቖ.GetColor(3381759);
		ዅ.ControlBoxMouseOver.Image = ቖ.GetColor(0);
		ዅ.ControlBoxPressed.Background = ቖ.GetColor(12045024);
		ዅ.ControlBoxPressed.Border = ቖ.GetColor(3381759);
		ዅ.ControlBoxPressed.Image = ቖ.GetColor(0);
		ዅ.InsertMarker = ቖ.GetColor(16777215);
		ዅ.SelectionMarker = ቖ.GetColor(16777215);
		return ዅ;
	}

	internal static SuperTabColorTable ጱ(SuperTabColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabColorTable();
		}
		ዅ.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16514301), ቖ.GetColor(14738407));
		ዅ.InnerBorder = ቖ.GetColor(13422030);
		ዅ.OuterBorder = ቖ.GetColor(15001320);
		ዅ.ControlBoxDefault.Image = ቖ.GetColor(0);
		ዅ.ControlBoxMouseOver.Background = ቖ.GetColor(13561338);
		ዅ.ControlBoxMouseOver.Border = ቖ.GetColor(3381759);
		ዅ.ControlBoxMouseOver.Image = ቖ.GetColor(0);
		ዅ.ControlBoxPressed.Background = ቖ.GetColor(12045024);
		ዅ.ControlBoxPressed.Border = ቖ.GetColor(3381759);
		ዅ.ControlBoxPressed.Image = ቖ.GetColor(0);
		ዅ.InsertMarker = ቖ.GetColor(0);
		ዅ.SelectionMarker = ቖ.GetColor(0);
		return ዅ;
	}

	internal static SuperTabColorTable ጲ(SuperTabColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabColorTable();
		}
		ዅ.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(3095119), ቖ.GetColor(4149102));
		ዅ.InnerBorder = Color.Empty;
		ዅ.OuterBorder = ቖ.GetColor(3885671);
		ዅ.ControlBoxDefault.Image = ቖ.GetColor(16777215);
		ዅ.ControlBoxMouseOver.Background = ቖ.GetColor(13561338);
		ዅ.ControlBoxMouseOver.Border = ቖ.GetColor(3381759);
		ዅ.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		ዅ.ControlBoxPressed.Background = ቖ.GetColor(12045024);
		ዅ.ControlBoxPressed.Border = ቖ.GetColor(3381759);
		ዅ.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		ዅ.InsertMarker = ቖ.GetColor(255, 16777215);
		ዅ.SelectionMarker = ቖ.GetColor(255, 16777215);
		return ዅ;
	}

	internal static SuperTabColorTable ጳ(SuperTabColorTable ዅ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabColorTable();
		}
		ዅ.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.BaseColor));
		ዅ.InnerBorder = Color.Empty;
		ዅ.OuterBorder = Color.Empty;
		ዅ.ControlBoxDefault.Image = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.ControlBoxMouseOver.Background = ቖ.GetColor(ጴ.BaseColorDark);
		ዅ.ControlBoxMouseOver.Border = ቖ.GetColor(ጴ.BaseColorDark);
		ዅ.ControlBoxMouseOver.Image = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.ControlBoxPressed.Background = ቖ.GetColor(ጴ.BaseColorDark);
		ዅ.ControlBoxPressed.Border = ቖ.GetColor(ጴ.BaseColorDark);
		ዅ.ControlBoxPressed.Image = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.InsertMarker = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.SelectionMarker = Color.Empty;
		return ዅ;
	}

	private static SuperTabItemColorTable ጵ()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabItemColorTable();
		}
		return office2007Renderer.ColorTable.BackstageItem;
	}

	internal static SuperTabItemColorTable ጶ(SuperTabItemColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabItemColorTable();
		}
		ዅ.Default.Normal.Text = ቖ.GetColor(0);
		ዅ.Default.Normal.Background.AdaptiveGradient = false;
		ዅ.Default.Normal.CloseMarker = ቖ.GetColor(0);
		ዅ.Default.Selected.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(609709));
		ዅ.Default.Selected.Background.AdaptiveGradient = false;
		ዅ.Default.Selected.InnerBorder = Color.Empty;
		ዅ.Default.Selected.OuterBorder = ቖ.GetColor(475031);
		ዅ.Default.Selected.Text = ቖ.GetColor(16777215);
		ዅ.Default.Selected.CloseMarker = ቖ.GetColor(16777215);
		ዅ.Default.Selected.SelectionMarker = ቖ.GetColor(16777215);
		ዅ.Default.SelectedMouseOver = ዅ.Default.Selected;
		ዅ.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(14543866));
		ዅ.Default.MouseOver.Background.AdaptiveGradient = false;
		ዅ.Default.MouseOver.InnerBorder = ቖ.GetColor(15791612);
		ዅ.Default.MouseOver.OuterBorder = ቖ.GetColor(5406176);
		ዅ.Default.MouseOver.Text = ቖ.GetColor(0);
		ዅ.Default.MouseOver.CloseMarker = ቖ.GetColor(0);
		ዅ.Default.Disabled.Text = ቖ.GetColor(14737632);
		ዅ.Default.Disabled.Background.AdaptiveGradient = false;
		ዅ.Default.Disabled.CloseMarker = ቖ.GetColor(14737632);
		ዅ.Left = ዅ.Default;
		ዅ.Bottom = ዅ.Default;
		ዅ.Right = ዅ.Right;
		return ዅ;
	}

	internal static SuperTabItemColorTable ጷ(SuperTabItemColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabItemColorTable();
		}
		ዅ.Default.Normal.Text = ቖ.GetColor(16777215);
		ዅ.Default.Normal.Background.AdaptiveGradient = false;
		ዅ.Default.Normal.CloseMarker = ቖ.GetColor(16777215);
		ዅ.Default.Selected.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(609709));
		ዅ.Default.Selected.Background.AdaptiveGradient = false;
		ዅ.Default.Selected.InnerBorder = Color.Empty;
		ዅ.Default.Selected.OuterBorder = ቖ.GetColor(475031);
		ዅ.Default.Selected.Text = ቖ.GetColor(16777215);
		ዅ.Default.Selected.CloseMarker = ቖ.GetColor(16777215);
		ዅ.Default.Selected.SelectionMarker = ቖ.GetColor(16777215);
		ዅ.Default.SelectedMouseOver = ዅ.Default.Selected;
		ዅ.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(5531005));
		ዅ.Default.MouseOver.Background.AdaptiveGradient = false;
		ዅ.Default.MouseOver.InnerBorder = ቖ.GetColor(5596798);
		ዅ.Default.MouseOver.OuterBorder = ቖ.GetColor(4821493);
		ዅ.Default.MouseOver.Text = ቖ.GetColor(16777215);
		ዅ.Default.MouseOver.CloseMarker = ቖ.GetColor(16777215);
		ዅ.Left = ዅ.Default;
		ዅ.Bottom = ዅ.Default;
		ዅ.Right = ዅ.Right;
		return ዅ;
	}

	internal static SuperTabItemColorTable ጸ(SuperTabItemColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabItemColorTable();
		}
		ዅ.Default.Normal.Text = ቖ.GetColor(0);
		ዅ.Default.Normal.Background.AdaptiveGradient = false;
		ዅ.Default.Normal.CloseMarker = ቖ.GetColor(0);
		ዅ.Default.Selected.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(609709));
		ዅ.Default.Selected.Background.AdaptiveGradient = false;
		ዅ.Default.Selected.InnerBorder = Color.Empty;
		ዅ.Default.Selected.OuterBorder = ቖ.GetColor(475031);
		ዅ.Default.Selected.Text = ቖ.GetColor(16777215);
		ዅ.Default.Selected.CloseMarker = ቖ.GetColor(16777215);
		ዅ.Default.Selected.SelectionMarker = ቖ.GetColor(16777215);
		ዅ.Default.SelectedMouseOver = ዅ.Default.Selected;
		ዅ.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(14543866));
		ዅ.Default.MouseOver.Background.AdaptiveGradient = false;
		ዅ.Default.MouseOver.InnerBorder = ቖ.GetColor(15791612);
		ዅ.Default.MouseOver.OuterBorder = ቖ.GetColor(5406176);
		ዅ.Default.MouseOver.Text = ቖ.GetColor(0);
		ዅ.Default.MouseOver.CloseMarker = ቖ.GetColor(0);
		ዅ.Left = ዅ.Default;
		ዅ.Bottom = ዅ.Default;
		ዅ.Right = ዅ.Right;
		return ዅ;
	}

	internal static SuperTabItemColorTable ጹ(SuperTabItemColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabItemColorTable();
		}
		ዅ.Default.Normal.Text = ቖ.GetColor(16777215);
		ዅ.Default.Normal.Background.AdaptiveGradient = false;
		ዅ.Default.Normal.CloseMarker = ቖ.GetColor(16777215);
		ዅ.Default.Selected.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(5865439), ቖ.GetColor(3037103));
		ዅ.Default.Selected.Background.AdaptiveGradient = false;
		ዅ.Default.Selected.InnerBorder = ቖ.GetColor(6851052);
		ዅ.Default.Selected.OuterBorder = ቖ.GetColor(2370877);
		ዅ.Default.Selected.Text = ቖ.GetColor(16777215);
		ዅ.Default.Selected.CloseMarker = ቖ.GetColor(16777215);
		ዅ.Default.Selected.SelectionMarker = ቖ.GetColor(16777215);
		ዅ.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(5865439), ቖ.GetColor(3037103));
		ዅ.Default.SelectedMouseOver.Background.AdaptiveGradient = false;
		ዅ.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(6851052);
		ዅ.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(2370877);
		ዅ.Default.SelectedMouseOver.Text = ቖ.GetColor(16777215);
		ዅ.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(16777215);
		ዅ.Default.SelectedMouseOver.SelectionMarker = ቖ.GetColor(16777215);
		ዅ.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(3886972),
			ቖ.GetColor(3557490),
			ቖ.GetColor(3557490),
			ቖ.GetColor(2701399)
		}, new float[4] { 0f, 95f, 0.95f, 1f });
		ዅ.Default.MouseOver.Background.AdaptiveGradient = false;
		ዅ.Default.MouseOver.InnerBorder = ቖ.GetColor(7576319);
		ዅ.Default.MouseOver.OuterBorder = ቖ.GetColor(728660);
		ዅ.Default.MouseOver.Text = ቖ.GetColor(16777215);
		ዅ.Default.MouseOver.CloseMarker = ቖ.GetColor(16777215);
		ዅ.Left = ዅ.Default;
		ዅ.Bottom = ዅ.Default;
		ዅ.Right = ዅ.Right;
		return ዅ;
	}

	internal static SuperTabItemColorTable ጺ(SuperTabItemColorTable ዅ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabItemColorTable();
		}
		ዅ.Default.Normal.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.Default.Normal.Background.AdaptiveGradient = false;
		ዅ.Default.Normal.CloseMarker = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.Default.Selected.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorLight1));
		ዅ.Default.Selected.Background.AdaptiveGradient = false;
		ዅ.Default.Selected.InnerBorder = Color.Empty;
		ዅ.Default.Selected.OuterBorder = Color.Empty;
		ዅ.Default.Selected.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.Default.Selected.CloseMarker = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.Default.Selected.SelectionMarker = Color.Empty;
		ዅ.Default.SelectedMouseOver = new SuperTabItemStateColorTable();
		ዅ.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ዅ.Default.SelectedMouseOver.Background.AdaptiveGradient = false;
		ዅ.Default.SelectedMouseOver.InnerBorder = Color.Empty;
		ዅ.Default.SelectedMouseOver.OuterBorder = Color.Empty;
		ዅ.Default.SelectedMouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.Default.SelectedMouseOver.SelectionMarker = Color.Empty;
		ዅ.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.BaseColorDark));
		ዅ.Default.MouseOver.Background.AdaptiveGradient = false;
		ዅ.Default.MouseOver.InnerBorder = Color.Empty;
		ዅ.Default.MouseOver.OuterBorder = Color.Empty;
		ዅ.Default.MouseOver.Text = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.Default.MouseOver.CloseMarker = ቖ.GetColor(ጴ.BaseTextColor);
		ዅ.Default.Disabled.Text = ቖ.GetColor(ጴ.TextDisabledColor);
		ዅ.Default.Disabled.Background.AdaptiveGradient = false;
		ዅ.Default.Disabled.CloseMarker = ቖ.GetColor(ጴ.TextDisabledColor);
		ዅ.Left = ዅ.Default;
		ዅ.Bottom = ዅ.Default;
		ዅ.Right = ዅ.Right;
		return ዅ;
	}

	private static SuperTabPanelColorTable ጻ()
	{
		if (!(GlobalManager.Renderer is Office2007Renderer office2007Renderer))
		{
			return new SuperTabPanelColorTable();
		}
		return office2007Renderer.ColorTable.BackstagePanel;
	}

	internal static SuperTabPanelColorTable ጼ(SuperTabPanelColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabPanelColorTable();
		}
		ዅ.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16777215));
		ዅ.Default.InnerBorder = Color.Empty;
		ዅ.Default.OuterBorder = ቖ.GetColor(11254989);
		ዅ.Default.Background.AdaptiveGradient = false;
		ዅ.Left = ዅ.Default;
		ዅ.Bottom = ዅ.Default;
		ዅ.Right = ዅ.Default;
		return ዅ;
	}

	internal static SuperTabPanelColorTable ጽ(SuperTabPanelColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabPanelColorTable();
		}
		ዅ.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16777215));
		ዅ.Default.InnerBorder = Color.Empty;
		ዅ.Default.OuterBorder = ቖ.GetColor(2894892);
		ዅ.Default.Background.AdaptiveGradient = false;
		ዅ.Left = ዅ.Default;
		ዅ.Bottom = ዅ.Default;
		ዅ.Right = ዅ.Default;
		return ዅ;
	}

	internal static SuperTabPanelColorTable ጾ(SuperTabPanelColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabPanelColorTable();
		}
		ዅ.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16777215));
		ዅ.Default.InnerBorder = Color.Empty;
		ዅ.Default.OuterBorder = ቖ.GetColor(13422030);
		ዅ.Default.Background.AdaptiveGradient = false;
		ዅ.Left = ዅ.Default;
		ዅ.Bottom = ዅ.Default;
		ዅ.Right = ዅ.Default;
		return ዅ;
	}

	internal static SuperTabPanelColorTable ጿ(SuperTabPanelColorTable ዅ, ColorFactory ቖ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabPanelColorTable();
		}
		ዅ.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16777215));
		ዅ.Default.OuterBorder = ቖ.GetColor(4149102);
		ዅ.Default.InnerBorder = ቖ.GetColor(4149102);
		ዅ.Default.Background.AdaptiveGradient = false;
		ዅ.Left = ዅ.Default;
		ዅ.Bottom = ዅ.Default;
		ዅ.Right = ዅ.Default;
		return ዅ;
	}

	internal static SuperTabPanelColorTable ፀ(SuperTabPanelColorTable ዅ, ColorFactory ቖ, MetroPartColors ጴ)
	{
		if (ዅ == null)
		{
			ዅ = new SuperTabPanelColorTable();
		}
		ዅ.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(ጴ.CanvasColor), ቖ.GetColor(ጴ.CanvasColor));
		ዅ.Default.InnerBorder = Color.Empty;
		ዅ.Default.OuterBorder = ቖ.GetColor(ጴ.CanvasColor);
		ዅ.Default.Background.AdaptiveGradient = false;
		ዅ.Left = ዅ.Default;
		ዅ.Bottom = ዅ.Default;
		ዅ.Right = ዅ.Default;
		return ዅ;
	}

	private static SuperTabColorTable ፁ(ColorFactory ቖ)
	{
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.Background = new SuperTabLinearGradientColorTable(Color.White);
		superTabColorTable.InnerBorder = ቖ.GetColor(16777215);
		superTabColorTable.OuterBorder = ቖ.GetColor(8158356);
		superTabColorTable.ControlBoxDefault.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxMouseOver.Background = ቖ.GetColor(16770978);
		superTabColorTable.ControlBoxMouseOver.Border = ቖ.GetColor(16760169);
		superTabColorTable.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxPressed.Background = ቖ.GetColor(16485436);
		superTabColorTable.ControlBoxPressed.Border = ቖ.GetColor(16760169);
		superTabColorTable.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		superTabColorTable.InsertMarker = ቖ.GetColor(255, 128);
		return superTabColorTable;
	}

	private static SuperTabItemColorTable ፂ(ColorFactory ቖ)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default.Normal = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Normal.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(14805497),
			ቖ.GetColor(13162227),
			ቖ.GetColor(9875943),
			ቖ.GetColor(9875943)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		superTabItemColorTable.Default.Normal.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Default.Normal.OuterBorder = ቖ.GetColor(7763090);
		superTabItemColorTable.Default.Normal.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Default.Normal.CloseMarker = ቖ.GetColor(4222879);
		superTabItemColorTable.Default.Selected = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(15791355),
			ቖ.GetColor(14410999),
			ቖ.GetColor(12965106),
			ቖ.GetColor(12965106)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		superTabItemColorTable.Default.Selected.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Default.Selected.OuterBorder = ቖ.GetColor(8158356);
		superTabItemColorTable.Default.Selected.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Default.Selected.CloseMarker = ቖ.GetColor(4222879);
		superTabItemColorTable.Default.SelectedMouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16776176), ቖ.GetColor(16773320));
		superTabItemColorTable.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(4934511);
		superTabItemColorTable.Default.SelectedMouseOver.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(4222879);
		superTabItemColorTable.Default.MouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16776176), ቖ.GetColor(16773320));
		superTabItemColorTable.Default.MouseOver.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Default.MouseOver.OuterBorder = ቖ.GetColor(4934511);
		superTabItemColorTable.Default.MouseOver.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Default.MouseOver.CloseMarker = ቖ.GetColor(4222879);
		superTabItemColorTable.Left.Normal = new SuperTabItemStateColorTable();
		superTabItemColorTable.Left.Normal.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(14805497), ቖ.GetColor(9875943));
		superTabItemColorTable.Left.Normal.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Left.Normal.OuterBorder = ቖ.GetColor(7763090);
		superTabItemColorTable.Left.Normal.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Left.Normal.CloseMarker = ቖ.GetColor(4222879);
		superTabItemColorTable.Left.Selected = new SuperTabItemStateColorTable();
		superTabItemColorTable.Left.Selected.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(15791355), ቖ.GetColor(12965106));
		superTabItemColorTable.Left.Selected.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Left.Selected.OuterBorder = ቖ.GetColor(8158356);
		superTabItemColorTable.Left.Selected.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Left.Selected.CloseMarker = ቖ.GetColor(4222879);
		superTabItemColorTable.Left.SelectedMouseOver = superTabItemColorTable.Default.SelectedMouseOver;
		superTabItemColorTable.Left.MouseOver = superTabItemColorTable.Default.MouseOver;
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		superTabItemColorTable.Right = superTabItemColorTable.Left;
		return superTabItemColorTable;
	}

	private static SuperTabPanelColorTable ፃ(ColorFactory ቖ)
	{
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(12899313),
			ቖ.GetColor(9087204),
			ቖ.GetColor(9087204),
			ቖ.GetColor(12899313)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		superTabPanelColorTable.Default.InnerBorder = ቖ.GetColor(16777215);
		superTabPanelColorTable.Default.OuterBorder = ቖ.GetColor(8158356);
		superTabPanelColorTable.Left.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(12899313), ቖ.GetColor(12899313));
		superTabPanelColorTable.Left.InnerBorder = ቖ.GetColor(16777215);
		superTabPanelColorTable.Left.OuterBorder = ቖ.GetColor(8158356);
		superTabPanelColorTable.Bottom = superTabPanelColorTable.Default;
		superTabPanelColorTable.Right = superTabPanelColorTable.Left;
		return superTabPanelColorTable;
	}

	private static SuperTabColorTable ፄ(ColorFactory ቖ)
	{
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.InnerBorder = Color.Empty;
		superTabColorTable.OuterBorder = Color.Empty;
		superTabColorTable.Background = new SuperTabLinearGradientColorTable(Color.White);
		superTabColorTable.ControlBoxDefault.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxMouseOver.Background = ቖ.GetColor(16770978);
		superTabColorTable.ControlBoxMouseOver.Border = ቖ.GetColor(16760169);
		superTabColorTable.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxPressed.Background = ቖ.GetColor(16485436);
		superTabColorTable.ControlBoxPressed.Border = ቖ.GetColor(16760169);
		superTabColorTable.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		superTabColorTable.InsertMarker = ቖ.GetColor(255, 128);
		return superTabColorTable;
	}

	private static SuperTabItemColorTable ፅ(ColorFactory ቖ)
	{
		return ፐ(eTabItemColor.OfficeMobile2014Teal, ቖ);
	}

	private static SuperTabPanelColorTable ፆ(ColorFactory ቖ)
	{
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		SuperTabLinearGradientColorTable superTabLinearGradientColorTable = ፗ(eTabItemColor.OfficeMobile2014Teal, ቖ);
		Color color = ((superTabLinearGradientColorTable.Colors != null && superTabLinearGradientColorTable.Colors.Length > 0) ? superTabLinearGradientColorTable.Colors[superTabLinearGradientColorTable.Colors.Length - 1] : Color.DimGray);
		superTabPanelColorTable.Default.InnerBorder = color;
		superTabPanelColorTable.Default.OuterBorder = color;
		superTabPanelColorTable.Default.Background = new SuperTabLinearGradientColorTable(Color.White);
		superTabPanelColorTable.Left = superTabPanelColorTable.Default;
		superTabPanelColorTable.Bottom = superTabPanelColorTable.Default;
		superTabPanelColorTable.Right = superTabPanelColorTable.Left;
		return superTabPanelColorTable;
	}

	private static SuperTabColorTable ፇ(ColorFactory ቖ)
	{
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.Background = new SuperTabLinearGradientColorTable(Color.White);
		superTabColorTable.InnerBorder = ቖ.GetColor(16777215);
		superTabColorTable.OuterBorder = ቖ.GetColor(8158356);
		superTabColorTable.ControlBoxDefault.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxMouseOver.Background = ቖ.GetColor(13561338);
		superTabColorTable.ControlBoxMouseOver.Border = ቖ.GetColor(3381759);
		superTabColorTable.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxPressed.Background = ቖ.GetColor(12045024);
		superTabColorTable.ControlBoxPressed.Border = ቖ.GetColor(3381759);
		superTabColorTable.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		superTabColorTable.InsertMarker = ቖ.GetColor(255, 128);
		return superTabColorTable;
	}

	private static SuperTabItemColorTable ፈ(ColorFactory ቖ)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default.Normal = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Normal.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(15921906),
			ቖ.GetColor(15461355),
			ቖ.GetColor(14540253),
			ቖ.GetColor(13619151)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		superTabItemColorTable.Default.Normal.InnerBorder = ቖ.GetColor(16579836);
		superTabItemColorTable.Default.Normal.OuterBorder = ቖ.GetColor(9014421);
		superTabItemColorTable.Default.Normal.Text = ቖ.GetColor(5592420);
		superTabItemColorTable.Default.Normal.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Default.Selected = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Selected.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16777215));
		superTabItemColorTable.Default.Selected.InnerBorder = Color.Empty;
		superTabItemColorTable.Default.Selected.OuterBorder = ቖ.GetColor(9014421);
		superTabItemColorTable.Default.Selected.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Default.Selected.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Default.SelectedMouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16777215));
		superTabItemColorTable.Default.SelectedMouseOver.InnerBorder = Color.Empty;
		superTabItemColorTable.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(9014421);
		superTabItemColorTable.Default.SelectedMouseOver.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Default.MouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(15398653),
			ቖ.GetColor(14283004),
			ቖ.GetColor(12510973),
			ቖ.GetColor(11000309)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		superTabItemColorTable.Default.MouseOver.InnerBorder = ቖ.GetColor(16449022);
		superTabItemColorTable.Default.MouseOver.OuterBorder = ቖ.GetColor(3964849);
		superTabItemColorTable.Default.MouseOver.Text = ቖ.GetColor(5592420);
		superTabItemColorTable.Default.MouseOver.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		superTabItemColorTable.Left = (SuperTabColorStates)superTabItemColorTable.Default.Clone();
		superTabItemColorTable.Left.Normal.Background.AdaptiveGradient = false;
		superTabItemColorTable.Left.MouseOver.Background.AdaptiveGradient = false;
		superTabItemColorTable.Left.Selected.Background.AdaptiveGradient = false;
		superTabItemColorTable.Left.SelectedMouseOver.Background.AdaptiveGradient = false;
		superTabItemColorTable.Right = superTabItemColorTable.Left;
		return superTabItemColorTable;
	}

	private static SuperTabPanelColorTable ፉ(ColorFactory ቖ)
	{
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16777215));
		superTabPanelColorTable.Default.InnerBorder = ቖ.GetColor(16777215);
		superTabPanelColorTable.Default.OuterBorder = ቖ.GetColor(9014421);
		superTabPanelColorTable.Left = superTabPanelColorTable.Default;
		superTabPanelColorTable.Bottom = superTabPanelColorTable.Default;
		superTabPanelColorTable.Right = superTabPanelColorTable.Default;
		return superTabPanelColorTable;
	}

	private static SuperTabColorTable ፊ(ColorFactory ቖ)
	{
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.Background = new SuperTabLinearGradientColorTable(Color.White);
		superTabColorTable.InnerBorder = ቖ.GetColor(16777215);
		superTabColorTable.OuterBorder = ቖ.GetColor(6922687);
		superTabColorTable.ControlBoxDefault.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxMouseOver.Background = ቖ.GetColor(13561338);
		superTabColorTable.ControlBoxMouseOver.Border = ቖ.GetColor(3381759);
		superTabColorTable.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxPressed.Background = ቖ.GetColor(12045024);
		superTabColorTable.ControlBoxPressed.Border = ቖ.GetColor(3381759);
		superTabColorTable.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		superTabColorTable.InsertMarker = ቖ.GetColor(255, 128);
		return superTabColorTable;
	}

	private static SuperTabItemColorTable ፋ(ColorFactory ቖ)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default.Normal = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Normal.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(15135225), ቖ.GetColor(10007762));
		superTabItemColorTable.Default.Normal.InnerBorder = ቖ.GetColor(15923967);
		superTabItemColorTable.Default.Normal.OuterBorder = ቖ.GetColor(9541282);
		superTabItemColorTable.Default.Normal.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Default.Normal.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Default.Selected = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Selected.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16580094), ቖ.GetColor(13821690));
		superTabItemColorTable.Default.Selected.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Default.Selected.OuterBorder = ቖ.GetColor(6922687);
		superTabItemColorTable.Default.Selected.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Default.Selected.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Default.SelectedMouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16580094), ቖ.GetColor(13821690));
		superTabItemColorTable.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(6922687);
		superTabItemColorTable.Default.SelectedMouseOver.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Default.MouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16252158), ቖ.GetColor(8507633));
		superTabItemColorTable.Default.MouseOver.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Default.MouseOver.OuterBorder = ቖ.GetColor(6922687);
		superTabItemColorTable.Default.MouseOver.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Default.MouseOver.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Left = superTabItemColorTable.Default;
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		superTabItemColorTable.Right = superTabItemColorTable.Default;
		return superTabItemColorTable;
	}

	private static SuperTabPanelColorTable ፌ(ColorFactory ቖ)
	{
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(13821690));
		superTabPanelColorTable.Default.InnerBorder = ቖ.GetColor(16777215);
		superTabPanelColorTable.Default.OuterBorder = ቖ.GetColor(6922687);
		superTabPanelColorTable.Left = superTabPanelColorTable.Default;
		superTabPanelColorTable.Bottom = superTabPanelColorTable.Default;
		superTabPanelColorTable.Right = superTabPanelColorTable.Default;
		return superTabPanelColorTable;
	}

	private static SuperTabColorTable ፍ(ColorFactory ቖ)
	{
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(14806007));
		superTabColorTable.InnerBorder = ቖ.GetColor(16777215);
		superTabColorTable.OuterBorder = ቖ.GetColor(12372957);
		superTabColorTable.ControlBoxDefault.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxMouseOver.Background = ቖ.GetColor(13561338);
		superTabColorTable.ControlBoxMouseOver.Border = ቖ.GetColor(3381759);
		superTabColorTable.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxPressed.Background = ቖ.GetColor(12045024);
		superTabColorTable.ControlBoxPressed.Border = ቖ.GetColor(3381759);
		superTabColorTable.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		superTabColorTable.InsertMarker = ቖ.GetColor(255, 128);
		return superTabColorTable;
	}

	private static SuperTabItemColorTable ፎ(ColorFactory ቖ)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default.Normal = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Normal.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(14278888), ቖ.GetColor(15397626));
		superTabItemColorTable.Default.Normal.Background.AdaptiveGradient = false;
		superTabItemColorTable.Default.Normal.InnerBorder = ቖ.GetColor(15658735);
		superTabItemColorTable.Default.Normal.OuterBorder = ቖ.GetColor(10926800);
		superTabItemColorTable.Default.Normal.Text = ቖ.GetColor(255, 0);
		superTabItemColorTable.Default.Normal.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Default.Selected = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.Selected.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16054783), ቖ.GetColor(15726333));
		superTabItemColorTable.Default.Selected.Background.AdaptiveGradient = false;
		superTabItemColorTable.Default.Selected.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Default.Selected.OuterBorder = ቖ.GetColor(12372957);
		superTabItemColorTable.Default.Selected.Text = ቖ.GetColor(10595);
		superTabItemColorTable.Default.Selected.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Default.SelectedMouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(15923453));
		superTabItemColorTable.Default.SelectedMouseOver.Background.AdaptiveGradient = false;
		superTabItemColorTable.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(12372957);
		superTabItemColorTable.Default.SelectedMouseOver.Text = ቖ.GetColor(10595);
		superTabItemColorTable.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Default.MouseOver = new SuperTabItemStateColorTable();
		superTabItemColorTable.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(15923453));
		superTabItemColorTable.Default.MouseOver.Background.AdaptiveGradient = false;
		superTabItemColorTable.Default.MouseOver.InnerBorder = ቖ.GetColor(16777215);
		superTabItemColorTable.Default.MouseOver.OuterBorder = ቖ.GetColor(12372957);
		superTabItemColorTable.Default.MouseOver.Text = ቖ.GetColor(10595);
		superTabItemColorTable.Default.MouseOver.CloseMarker = ቖ.GetColor(1722485);
		superTabItemColorTable.Left = superTabItemColorTable.Default;
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		superTabItemColorTable.Right = superTabItemColorTable.Default;
		return superTabItemColorTable;
	}

	private static SuperTabPanelColorTable ፏ(ColorFactory ቖ)
	{
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(15726333));
		superTabPanelColorTable.Default.Background.AdaptiveGradient = false;
		superTabPanelColorTable.Default.InnerBorder = ቖ.GetColor(16777215);
		superTabPanelColorTable.Default.OuterBorder = ቖ.GetColor(12372957);
		superTabPanelColorTable.Left = superTabPanelColorTable.Default;
		superTabPanelColorTable.Bottom = superTabPanelColorTable.Default;
		superTabPanelColorTable.Right = superTabPanelColorTable.Default;
		return superTabPanelColorTable;
	}

	public static SuperTabItemColorTable ፐ(eTabItemColor _0BDE, ColorFactory ቖ)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default = new SuperTabColorStates();
		superTabItemColorTable.Default.Normal = ፑ(_0BDE, ቖ);
		superTabItemColorTable.Default.Selected = ፒ(_0BDE, ቖ);
		superTabItemColorTable.Default.MouseOver = ፓ(_0BDE, ቖ);
		superTabItemColorTable.Default.SelectedMouseOver = ፔ(_0BDE, ቖ);
		superTabItemColorTable.Left = superTabItemColorTable.Default;
		superTabItemColorTable.Right = superTabItemColorTable.Default;
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		return superTabItemColorTable;
	}

	internal static SuperTabItemStateColorTable ፑ(eTabItemColor _0BDE, ColorFactory ቖ)
	{
		SuperTabItemStateColorTable superTabItemStateColorTable = new SuperTabItemStateColorTable();
		superTabItemStateColorTable.InnerBorder = Color.Empty;
		superTabItemStateColorTable.OuterBorder = Color.Empty;
		superTabItemStateColorTable.CloseMarker = ቖ.GetColor(4222879);
		superTabItemStateColorTable.Background = ፖ(_0BDE, ቖ);
		switch (_0BDE)
		{
		case eTabItemColor.OfficeMobile2014Teal:
			superTabItemStateColorTable.Text = ቖ.GetColor(20845);
			break;
		case eTabItemColor.OfficeMobile2014Blue:
			superTabItemStateColorTable.Text = ቖ.GetColor(476800);
			break;
		case eTabItemColor.OfficeMobile2014Gold:
			superTabItemStateColorTable.Text = ቖ.GetColor(10312195);
			break;
		case eTabItemColor.OfficeMobile2014Green:
			superTabItemStateColorTable.Text = ቖ.GetColor(4491599);
			break;
		case eTabItemColor.OfficeMobile2014Coral:
			superTabItemStateColorTable.Text = ቖ.GetColor(12275267);
			break;
		case eTabItemColor.OfficeMobile2014Lilac:
			superTabItemStateColorTable.Text = ቖ.GetColor(8932741);
			break;
		case eTabItemColor.OfficeMobile2014Orange:
			superTabItemStateColorTable.Text = ቖ.GetColor(10303744);
			break;
		case eTabItemColor.OfficeMobile2014Pink:
			superTabItemStateColorTable.Text = ቖ.GetColor(8986714);
			break;
		default:
			superTabItemStateColorTable.Text = Color.Black;
			superTabItemStateColorTable.InnerBorder = Color.White;
			superTabItemStateColorTable.OuterBorder = Color.DimGray;
			break;
		}
		return superTabItemStateColorTable;
	}

	internal static SuperTabItemStateColorTable ፒ(eTabItemColor _0BDE, ColorFactory ቖ)
	{
		SuperTabItemStateColorTable superTabItemStateColorTable = new SuperTabItemStateColorTable();
		superTabItemStateColorTable.CloseMarker = ቖ.GetColor(4222879);
		superTabItemStateColorTable.SelectionMarker = ቖ.GetColor(255, 16777215);
		superTabItemStateColorTable.Background = ፗ(_0BDE, ቖ);
		switch (_0BDE)
		{
		case eTabItemColor.OfficeMobile2014Teal:
		case eTabItemColor.OfficeMobile2014Blue:
		case eTabItemColor.OfficeMobile2014Gold:
		case eTabItemColor.OfficeMobile2014Green:
		case eTabItemColor.OfficeMobile2014Coral:
		case eTabItemColor.OfficeMobile2014Lilac:
		case eTabItemColor.OfficeMobile2014Orange:
		case eTabItemColor.OfficeMobile2014Pink:
			superTabItemStateColorTable.Text = ቖ.GetColor(Color.White);
			superTabItemStateColorTable.InnerBorder = Color.Empty;
			superTabItemStateColorTable.OuterBorder = Color.Empty;
			break;
		default:
			superTabItemStateColorTable.Text = Color.Black;
			superTabItemStateColorTable.InnerBorder = Color.White;
			superTabItemStateColorTable.OuterBorder = Color.DimGray;
			break;
		}
		return superTabItemStateColorTable;
	}

	internal static SuperTabItemStateColorTable ፓ(eTabItemColor _0BDE, ColorFactory ቖ)
	{
		switch (_0BDE)
		{
		case eTabItemColor.OfficeMobile2014Teal:
		case eTabItemColor.OfficeMobile2014Blue:
		case eTabItemColor.OfficeMobile2014Gold:
		case eTabItemColor.OfficeMobile2014Green:
		case eTabItemColor.OfficeMobile2014Coral:
		case eTabItemColor.OfficeMobile2014Lilac:
		case eTabItemColor.OfficeMobile2014Orange:
		case eTabItemColor.OfficeMobile2014Pink:
			return ፑ(_0BDE, ቖ);
		default:
		{
			SuperTabItemStateColorTable superTabItemStateColorTable = new SuperTabItemStateColorTable();
			superTabItemStateColorTable.Text = Color.Black;
			superTabItemStateColorTable.InnerBorder = Color.White;
			superTabItemStateColorTable.OuterBorder = Color.DimGray;
			superTabItemStateColorTable.CloseMarker = ቖ.GetColor(4222879);
			superTabItemStateColorTable.SelectionMarker = ቖ.GetColor(255, 16777215);
			superTabItemStateColorTable.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16776176), ቖ.GetColor(16773320));
			return superTabItemStateColorTable;
		}
		}
	}

	internal static SuperTabItemStateColorTable ፔ(eTabItemColor _0BDE, ColorFactory ቖ)
	{
		switch (_0BDE)
		{
		case eTabItemColor.OfficeMobile2014Teal:
		case eTabItemColor.OfficeMobile2014Blue:
		case eTabItemColor.OfficeMobile2014Gold:
		case eTabItemColor.OfficeMobile2014Green:
		case eTabItemColor.OfficeMobile2014Coral:
		case eTabItemColor.OfficeMobile2014Lilac:
		case eTabItemColor.OfficeMobile2014Orange:
		case eTabItemColor.OfficeMobile2014Pink:
			return ፒ(_0BDE, ቖ);
		default:
		{
			SuperTabItemStateColorTable superTabItemStateColorTable = new SuperTabItemStateColorTable();
			superTabItemStateColorTable.Text = Color.Black;
			superTabItemStateColorTable.InnerBorder = Color.White;
			superTabItemStateColorTable.OuterBorder = Color.DimGray;
			superTabItemStateColorTable.CloseMarker = ቖ.GetColor(4222879);
			superTabItemStateColorTable.SelectionMarker = ቖ.GetColor(255, 16777215);
			superTabItemStateColorTable.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16776176), ቖ.GetColor(16773320));
			return superTabItemStateColorTable;
		}
		}
	}

	public static SuperTabPanelItemColorTable ፕ(eTabItemColor _0BDE, ColorFactory ቖ)
	{
		SuperTabPanelItemColorTable superTabPanelItemColorTable = new SuperTabPanelItemColorTable();
		SuperTabItemColorTable superTabItemColorTable = ፐ(_0BDE, ቖ);
		SuperTabLinearGradientColorTable background = superTabItemColorTable.Default.Selected.Background;
		Color color = ((background.Colors != null && background.Colors.Length > 0) ? background.Colors[background.Colors.Length - 1] : Color.DimGray);
		switch (_0BDE)
		{
		case eTabItemColor.OfficeMobile2014Teal:
		case eTabItemColor.OfficeMobile2014Blue:
		case eTabItemColor.OfficeMobile2014Gold:
		case eTabItemColor.OfficeMobile2014Green:
		case eTabItemColor.OfficeMobile2014Coral:
		case eTabItemColor.OfficeMobile2014Lilac:
		case eTabItemColor.OfficeMobile2014Orange:
		case eTabItemColor.OfficeMobile2014Pink:
			superTabPanelItemColorTable.InnerBorder = color;
			superTabPanelItemColorTable.OuterBorder = color;
			superTabPanelItemColorTable.Background = new SuperTabLinearGradientColorTable(Color.White);
			break;
		default:
			superTabPanelItemColorTable.InnerBorder = superTabItemColorTable.Default.Selected.InnerBorder;
			superTabPanelItemColorTable.OuterBorder = superTabItemColorTable.Default.Selected.OuterBorder;
			superTabPanelItemColorTable.Background = new SuperTabLinearGradientColorTable(color);
			break;
		}
		return superTabPanelItemColorTable;
	}

	internal static SuperTabLinearGradientColorTable ፖ(eTabItemColor _0BDE, ColorFactory ቖ)
	{
		return _0BDE switch
		{
			eTabItemColor.Apple => new SuperTabLinearGradientColorTable(ቖ.GetColor(14349517), ቖ.GetColor(11659416)), 
			eTabItemColor.Blue => new SuperTabLinearGradientColorTable(ቖ.GetColor(14542583), ቖ.GetColor(9087204)), 
			eTabItemColor.BlueMist => new SuperTabLinearGradientColorTable(ቖ.GetColor(13820139), ቖ.GetColor(10600404)), 
			eTabItemColor.Cyan => new SuperTabLinearGradientColorTable(ቖ.GetColor(13492187), ቖ.GetColor(9944754)), 
			eTabItemColor.Green => new SuperTabLinearGradientColorTable(ቖ.GetColor(14608079), ቖ.GetColor(12307613)), 
			eTabItemColor.Lemon => new SuperTabLinearGradientColorTable(ቖ.GetColor(16514493), ቖ.GetColor(16120183)), 
			eTabItemColor.Magenta => new SuperTabLinearGradientColorTable(ቖ.GetColor(15521504), ቖ.GetColor(14133695)), 
			eTabItemColor.Orange => new SuperTabLinearGradientColorTable(ቖ.GetColor(16505794), ቖ.GetColor(16168063)), 
			eTabItemColor.Purple => new SuperTabLinearGradientColorTable(ቖ.GetColor(14537713), ቖ.GetColor(12100576)), 
			eTabItemColor.PurpleMist => new SuperTabLinearGradientColorTable(ቖ.GetColor(14275294), ቖ.GetColor(11510203)), 
			eTabItemColor.Red => new SuperTabLinearGradientColorTable(ቖ.GetColor(16240591), ቖ.GetColor(15702941)), 
			eTabItemColor.Silver => new SuperTabLinearGradientColorTable(ቖ.GetColor(13619161), ቖ.GetColor(10197934)), 
			eTabItemColor.Tan => new SuperTabLinearGradientColorTable(ቖ.GetColor(16116688), ቖ.GetColor(15324319)), 
			eTabItemColor.Teal => new SuperTabLinearGradientColorTable(ቖ.GetColor(13495536), ቖ.GetColor(5160138)), 
			eTabItemColor.OfficeMobile2014Teal => new SuperTabLinearGradientColorTable(ቖ.GetColor(10276314)), 
			eTabItemColor.OfficeMobile2014Blue => new SuperTabLinearGradientColorTable(ቖ.GetColor(11126755)), 
			eTabItemColor.OfficeMobile2014Gold => new SuperTabLinearGradientColorTable(ቖ.GetColor(15782026)), 
			eTabItemColor.OfficeMobile2014Green => new SuperTabLinearGradientColorTable(ቖ.GetColor(10932397)), 
			eTabItemColor.OfficeMobile2014Coral => new SuperTabLinearGradientColorTable(ቖ.GetColor(15773352)), 
			eTabItemColor.OfficeMobile2014Lilac => new SuperTabLinearGradientColorTable(ቖ.GetColor(14268888)), 
			eTabItemColor.OfficeMobile2014Orange => new SuperTabLinearGradientColorTable(ቖ.GetColor(15843734)), 
			eTabItemColor.OfficeMobile2014Pink => new SuperTabLinearGradientColorTable(ቖ.GetColor(15250897)), 
			_ => new SuperTabLinearGradientColorTable(ቖ.GetColor(16774617), ቖ.GetColor(16768120)), 
		};
	}

	internal static SuperTabLinearGradientColorTable ፗ(eTabItemColor _0BDE, ColorFactory ቖ)
	{
		return _0BDE switch
		{
			eTabItemColor.Apple => new SuperTabLinearGradientColorTable(ቖ.GetColor(15596263), ቖ.GetColor(14087112)), 
			eTabItemColor.Blue => new SuperTabLinearGradientColorTable(ቖ.GetColor(14542583), ቖ.GetColor(9087204)), 
			eTabItemColor.BlueMist => new SuperTabLinearGradientColorTable(ቖ.GetColor(15331573), ቖ.GetColor(13491688)), 
			eTabItemColor.Cyan => new SuperTabLinearGradientColorTable(ቖ.GetColor(15200494), ቖ.GetColor(13163734)), 
			eTabItemColor.Green => new SuperTabLinearGradientColorTable(ቖ.GetColor(15791080), ቖ.GetColor(14410955)), 
			eTabItemColor.Lemon => new SuperTabLinearGradientColorTable(ቖ.GetColor(16645855), ቖ.GetColor(16448695)), 
			eTabItemColor.Magenta => new SuperTabLinearGradientColorTable(ቖ.GetColor(16182256), ቖ.GetColor(15389149)), 
			eTabItemColor.Orange => new SuperTabLinearGradientColorTable(ቖ.GetColor(16641761), ቖ.GetColor(16439227)), 
			eTabItemColor.Purple => new SuperTabLinearGradientColorTable(ቖ.GetColor(15723255), ቖ.GetColor(14274286)), 
			eTabItemColor.PurpleMist => new SuperTabLinearGradientColorTable(ቖ.GetColor(15526384), ቖ.GetColor(14011867)), 
			eTabItemColor.Red => new SuperTabLinearGradientColorTable(ቖ.GetColor(16574696), ቖ.GetColor(16173771)), 
			eTabItemColor.Silver => new SuperTabLinearGradientColorTable(ቖ.GetColor(15263980), ቖ.GetColor(13290196)), 
			eTabItemColor.Tan => new SuperTabLinearGradientColorTable(ቖ.GetColor(16446953), ቖ.GetColor(15984844)), 
			eTabItemColor.Teal => new SuperTabLinearGradientColorTable(ቖ.GetColor(13495536), ቖ.GetColor(5160138)), 
			eTabItemColor.OfficeMobile2014Teal => new SuperTabLinearGradientColorTable(ቖ.GetColor(3249842)), 
			eTabItemColor.OfficeMobile2014Blue => new SuperTabLinearGradientColorTable(ቖ.GetColor(5016517)), 
			eTabItemColor.OfficeMobile2014Gold => new SuperTabLinearGradientColorTable(ቖ.GetColor(14720268)), 
			eTabItemColor.OfficeMobile2014Green => new SuperTabLinearGradientColorTable(ቖ.GetColor(4627540)), 
			eTabItemColor.OfficeMobile2014Coral => new SuperTabLinearGradientColorTable(ቖ.GetColor(14636873)), 
			eTabItemColor.OfficeMobile2014Lilac => new SuperTabLinearGradientColorTable(ቖ.GetColor(11627949)), 
			eTabItemColor.OfficeMobile2014Orange => new SuperTabLinearGradientColorTable(ቖ.GetColor(14843428)), 
			eTabItemColor.OfficeMobile2014Pink => new SuperTabLinearGradientColorTable(ቖ.GetColor(13526431)), 
			_ => new SuperTabLinearGradientColorTable(ቖ.GetColor(16775916), ቖ.GetColor(16771763)), 
		};
	}

	public static bool ፘ(eTabItemColor ፙ)
	{
		switch (ፙ)
		{
		case eTabItemColor.OfficeMobile2014Teal:
		case eTabItemColor.OfficeMobile2014Blue:
		case eTabItemColor.OfficeMobile2014Gold:
		case eTabItemColor.OfficeMobile2014Green:
		case eTabItemColor.OfficeMobile2014Coral:
		case eTabItemColor.OfficeMobile2014Lilac:
		case eTabItemColor.OfficeMobile2014Orange:
		case eTabItemColor.OfficeMobile2014Pink:
			return true;
		default:
			return false;
		}
	}
}

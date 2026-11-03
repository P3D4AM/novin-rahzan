using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro.ColorTables;

namespace DevComponents.DotNetBar.Metro.Rendering;

public static class MetroRender
{
	private static MetroRenderer[] __1398;

	private static MetroRendererInfo __1399;

	private static MetroColorTable __137C;

	static MetroRender()
	{
		__1399 = new MetroRendererInfo();
		__137C = null;
		__1398 = new MetroRenderer[Enum.GetNames(typeof(Renderers)).Length];
		__1398[0] = new Ꭰ();
		__1398[1] = new _285B();
		__1398[2] = new _285C();
		__1398[3] = new _285E();
		__1398[4] = new _285F();
		__1398[5] = new _2860();
	}

	public static void Paint(MetroTileItem item, ItemPaintArgs pa)
	{
		MetroRenderer metroRenderer = ဣ(Renderers.MetroTileItem);
		metroRenderer.Render(_139C((object)item, pa, GetColorTable()));
	}

	public static void Paint(MetroAppForm form, PaintEventArgs e)
	{
		MetroRenderer metroRenderer = ဣ(Renderers.MetroForm);
		metroRenderer.Render(_139C((Control)(object)form, e, GetColorTable()));
	}

	internal static void _139A(_285A _139B, PaintEventArgs ؿ)
	{
		MetroRenderer metroRenderer = ဣ(Renderers.MetroForm);
		metroRenderer.Render(_139C((Control)(object)_139B, ؿ, GetColorTable()));
	}

	public static void Paint(MetroTabItem tab, ItemPaintArgs pa)
	{
		MetroRenderer metroRenderer = ဣ(Renderers.MetroTabItem);
		metroRenderer.Render(_139C((object)tab, pa, GetColorTable()));
	}

	public static void Paint(MetroTabStrip tabStrip, ItemPaintArgs pa)
	{
		MetroRenderer metroRenderer = ဣ(Renderers.MetroTabStrip);
		MetroRendererInfo renderingInfo = _139C(tabStrip, pa, GetColorTable());
		metroRenderer.Render(renderingInfo);
	}

	public static void Paint(MetroStatusBar bar, ItemPaintArgs pa)
	{
		MetroRenderer metroRenderer = ဣ(Renderers.MetroStatusBar);
		metroRenderer.Render(_139C((Control)(object)bar, pa, GetColorTable()));
	}

	public static void Paint(MetroToolbar bar, ItemPaintArgs pa)
	{
		MetroRenderer metroRenderer = ဣ(Renderers.MetroToolbar);
		metroRenderer.Render(_139C((Control)(object)bar, pa, GetColorTable()));
	}

	private static MetroRenderer ဣ(Renderers ઝ)
	{
		return __1398[(int)ઝ];
	}

	private static MetroRendererInfo _139C(Control ॼ, PaintEventArgs ؿ, MetroColorTable _139D)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		__1399.Control = ॼ;
		__1399.PaintEventArgs = ؿ;
		__1399.ColorTable = _139D;
		__1399.DefaultFont = ॼ.Font;
		__1399.RightToLeft = (int)ॼ.RightToLeft == 1;
		return __1399;
	}

	private static MetroRendererInfo _139C(Control ॼ, ItemPaintArgs ؿ, MetroColorTable _139D)
	{
		__1399.Control = ॼ;
		__1399.PaintEventArgs = ؿ.PaintEventArgs;
		__1399.DefaultFont = ؿ.Font;
		__1399.ColorTable = _139D;
		__1399.RightToLeft = ؿ.RightToLeft;
		__1399.ItemPaintArgs = ؿ;
		return __1399;
	}

	private static MetroRendererInfo _139C(MetroTabStrip ॼ, ItemPaintArgs ؿ, MetroColorTable _139D)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		__1399.Control = ॼ;
		__1399.PaintEventArgs = ؿ.PaintEventArgs;
		__1399.DefaultFont = ؿ.Font;
		if (ؿ.Font.Bold)
		{
			__1399.DefaultPlainFont = new Font(ؿ.Font, (FontStyle)0);
		}
		__1399.ColorTable = _139D;
		__1399.RightToLeft = ؿ.RightToLeft;
		__1399.ItemPaintArgs = ؿ;
		return __1399;
	}

	private static MetroRendererInfo _139C(object ॼ, ItemPaintArgs ؿ, MetroColorTable _139D)
	{
		__1399.Control = ॼ;
		__1399.PaintEventArgs = ؿ.PaintEventArgs;
		__1399.DefaultFont = ؿ.Font;
		__1399.ColorTable = _139D;
		__1399.RightToLeft = ؿ.RightToLeft;
		__1399.ItemPaintArgs = ؿ;
		return __1399;
	}

	public static MetroColorTable GetColorTable()
	{
		if (__137C == null)
		{
			_139E(MetroColorGeneratorParameters.Default);
		}
		return __137C;
	}

	internal static void _139E(MetroColorGeneratorParameters _139F)
	{
		if (StyleManager.IsVisualStudio2012(StyleManager.Style))
		{
			__137C = _228D._2226(_139F);
		}
		else if (StyleManager.Style == eStyle.OfficeMobile2014)
		{
			__137C = _228F._2226(_139F);
		}
		else if (StyleManager.Style == eStyle.Office2016)
		{
			__137C = _2291._2226(_139F);
		}
		else
		{
			__137C = _2224._2226(_139F);
		}
	}
}

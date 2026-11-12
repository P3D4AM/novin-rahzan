using System;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

public static class MetroRender
{
	private static MetroRenderer[] _Renderers;

	private static MetroRendererInfo _RenderingInfo;

	private static MetroColorTable _ColorTable;

	static MetroRender()
	{
		_RenderingInfo = new MetroRendererInfo();
		_ColorTable = null;
		_Renderers = new MetroRenderer[Enum.GetNames(typeof(Renderers)).Length];
		_Renderers[0] = new MetroFormRenderer();
		_Renderers[1] = new MetroTabItemPainter();
		_Renderers[2] = new MetroTabStripPainter();
		_Renderers[3] = new MetroStatusBarPainter();
		_Renderers[4] = new MetroToolbarPainter();
		_Renderers[5] = new MetroTileItemPainter();
	}

	public static void Paint(MetroTileItem item, ItemPaintArgs pa)
	{
		MetroRenderer renderer = GetRenderer(Renderers.MetroTileItem);
		renderer.Render(GetRenderingInfo((object)item, pa, GetColorTable()));
	}

	public static void Paint(MetroAppForm form, PaintEventArgs e)
	{
		MetroRenderer renderer = GetRenderer(Renderers.MetroForm);
		renderer.Render(GetRenderingInfo((Control)(object)form, e, GetColorTable()));
	}

	internal static void Paint(BorderOverlay formOverlay, PaintEventArgs e)
	{
		MetroRenderer renderer = GetRenderer(Renderers.MetroForm);
		renderer.Render(GetRenderingInfo((Control)(object)formOverlay, e, GetColorTable()));
	}

	public static void Paint(MetroTabItem tab, ItemPaintArgs pa)
	{
		MetroRenderer renderer = GetRenderer(Renderers.MetroTabItem);
		renderer.Render(GetRenderingInfo((object)tab, pa, GetColorTable()));
	}

	public static void Paint(MetroTabStrip tabStrip, ItemPaintArgs pa)
	{
		MetroRenderer renderer = GetRenderer(Renderers.MetroTabStrip);
		renderer.Render(GetRenderingInfo((Control)(object)tabStrip, pa, GetColorTable()));
	}

	public static void Paint(MetroStatusBar bar, ItemPaintArgs pa)
	{
		MetroRenderer renderer = GetRenderer(Renderers.MetroStatusBar);
		renderer.Render(GetRenderingInfo((Control)(object)bar, pa, GetColorTable()));
	}

	public static void Paint(MetroToolbar bar, ItemPaintArgs pa)
	{
		MetroRenderer renderer = GetRenderer(Renderers.MetroToolbar);
		renderer.Render(GetRenderingInfo((Control)(object)bar, pa, GetColorTable()));
	}

	private static MetroRenderer GetRenderer(Renderers renderer)
	{
		return _Renderers[(int)renderer];
	}

	private static MetroRendererInfo GetRenderingInfo(Control control, PaintEventArgs e, MetroColorTable colorTable)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		_RenderingInfo.Control = control;
		_RenderingInfo.PaintEventArgs = e;
		_RenderingInfo.ColorTable = colorTable;
		_RenderingInfo.DefaultFont = control.Font;
		_RenderingInfo.RightToLeft = (int)control.RightToLeft == 1;
		return _RenderingInfo;
	}

	private static MetroRendererInfo GetRenderingInfo(Control control, ItemPaintArgs e, MetroColorTable colorTable)
	{
		_RenderingInfo.Control = control;
		_RenderingInfo.PaintEventArgs = e.PaintEventArgs;
		_RenderingInfo.DefaultFont = e.Font;
		_RenderingInfo.ColorTable = colorTable;
		_RenderingInfo.RightToLeft = e.RightToLeft;
		_RenderingInfo.ItemPaintArgs = e;
		return _RenderingInfo;
	}

	private static MetroRendererInfo GetRenderingInfo(object control, ItemPaintArgs e, MetroColorTable colorTable)
	{
		_RenderingInfo.Control = control;
		_RenderingInfo.PaintEventArgs = e.PaintEventArgs;
		_RenderingInfo.DefaultFont = e.Font;
		_RenderingInfo.ColorTable = colorTable;
		_RenderingInfo.RightToLeft = e.RightToLeft;
		_RenderingInfo.ItemPaintArgs = e;
		return _RenderingInfo;
	}

	public static MetroColorTable GetColorTable()
	{
		if (_ColorTable == null)
		{
			UpdateColorTable(MetroColorGeneratorParameters.Default);
		}
		return _ColorTable;
	}

	internal static void UpdateColorTable(MetroColorGeneratorParameters colorParams)
	{
		_ColorTable = MetroColorTableInitializer.CreateColorTable(colorParams);
	}
}

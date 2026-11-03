using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.AdvTree.Layout;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;
using DevComponents.WinForms.Drawing;

namespace DevComponents.AdvTree.Display;

internal class _135B
{
	public static void _135C(TreeColorTable ዅ, ColorFactory ቖ)
	{
		TreeSelectionColors treeSelectionColors = (ዅ.Selection = new TreeSelectionColors());
		SelectionColorTable selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(10997232));
		treeSelectionColors.FullRowSelect = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(15066597));
		treeSelectionColors.FullRowSelectInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(64, 3238597));
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(96, 3238597), 1);
		treeSelectionColors.NodeMarker = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(64, 15066597));
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(96, 0), 1);
		treeSelectionColors.NodeMarkerInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(new ColorStop[4]
		{
			new ColorStop(ቖ.GetColor(16776409), 0f),
			new ColorStop(ቖ.GetColor(16770957), 0.4f),
			new ColorStop(ቖ.GetColor(16766792), 0.4f),
			new ColorStop(ቖ.GetColor(16770963), 1f)
		});
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(14536603), 1);
		selectionColorTable.BorderCornerRadius = 2;
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(192, 16773822), 1);
		treeSelectionColors.HighlightCells = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(ቖ.GetColor(16448251), ቖ.GetColor(15066597), 90f);
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(14277081), 1);
		selectionColorTable.BorderCornerRadius = 2;
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(228, 16777215), 1);
		treeSelectionColors.HighlightCellsInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(ቖ.GetColor(16448251), ቖ.GetColor(15066597), 90f);
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(14277081), 1);
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(228, 16777215), 1);
		treeSelectionColors.NodeHotTracking = selectionColorTable;
		TreeExpandColorTable treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(0), 1);
		treeExpandColorTable.CollapseFill = new SolidFill(ቖ.GetColor(5855577));
		treeExpandColorTable.CollapseMouseOverBorder = new SolidBorder(ቖ.GetColor(1885431), 1);
		treeExpandColorTable.CollapseMouseOverFill = new SolidFill(ቖ.GetColor(8577019));
		treeExpandColorTable.ExpandBorder = new SolidBorder(ቖ.GetColor(8684676), 1);
		treeExpandColorTable.ExpandFill = new SolidFill(ቖ.GetColor(16777215));
		treeExpandColorTable.ExpandMouseOverBorder = new SolidBorder(ቖ.GetColor(1885431), 1);
		treeExpandColorTable.ExpandMouseOverFill = new SolidFill(ቖ.GetColor(13430266));
		ዅ.ExpandTriangle = treeExpandColorTable;
		treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseForeground = new SolidFill(ቖ.GetColor(0));
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(9868950), 1);
		treeExpandColorTable.CollapseFill = new GradientFill(new ColorStop[3]
		{
			new ColorStop(ቖ.GetColor(16777215), 0f),
			new ColorStop(ቖ.GetColor(16777215), 0.4f),
			new ColorStop(ቖ.GetColor(11974326), 1f)
		}, 45);
		treeExpandColorTable.CollapseMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.CollapseMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.CollapseMouseOverFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandMouseOverFill = treeExpandColorTable.CollapseFill;
		ዅ.ExpandRectangle = treeExpandColorTable;
		ዅ.ExpandEllipse = treeExpandColorTable;
		ዅ.GridLines = ቖ.GetColor(14803425);
	}

	public static void _135D(TreeColorTable ዅ, ColorFactory ቖ)
	{
		TreeSelectionColors treeSelectionColors = (ዅ.Selection = new TreeSelectionColors());
		SelectionColorTable selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(10997232));
		treeSelectionColors.FullRowSelect = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(15066597));
		treeSelectionColors.FullRowSelectInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(64, 3238597));
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(96, 3238597), 1);
		treeSelectionColors.NodeMarker = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(64, 15066597));
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(96, 0), 1);
		treeSelectionColors.NodeMarkerInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(new ColorStop[4]
		{
			new ColorStop(ቖ.GetColor(16776409), 0f),
			new ColorStop(ቖ.GetColor(16770957), 0.4f),
			new ColorStop(ቖ.GetColor(16766792), 0.4f),
			new ColorStop(ቖ.GetColor(16770963), 1f)
		});
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(14536603), 1);
		selectionColorTable.BorderCornerRadius = 2;
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(192, 16773822), 1);
		treeSelectionColors.HighlightCells = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(ቖ.GetColor(16448251), ቖ.GetColor(15066597), 90f);
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(14277081), 1);
		selectionColorTable.BorderCornerRadius = 2;
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(228, 16777215), 1);
		treeSelectionColors.HighlightCellsInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(ቖ.GetColor(16448251), ቖ.GetColor(15066597), 90f);
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(14277081), 1);
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(228, 16777215), 1);
		treeSelectionColors.NodeHotTracking = selectionColorTable;
		TreeExpandColorTable treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(0), 1);
		treeExpandColorTable.CollapseFill = new SolidFill(ቖ.GetColor(5855577));
		treeExpandColorTable.CollapseMouseOverBorder = new SolidBorder(ቖ.GetColor(1885431), 1);
		treeExpandColorTable.CollapseMouseOverFill = new SolidFill(ቖ.GetColor(8577019));
		treeExpandColorTable.ExpandBorder = new SolidBorder(ቖ.GetColor(8684676), 1);
		treeExpandColorTable.ExpandFill = new SolidFill(ቖ.GetColor(16777215));
		treeExpandColorTable.ExpandMouseOverBorder = new SolidBorder(ቖ.GetColor(1885431), 1);
		treeExpandColorTable.ExpandMouseOverFill = new SolidFill(ቖ.GetColor(13430266));
		ዅ.ExpandTriangle = treeExpandColorTable;
		treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseForeground = new SolidFill(ቖ.GetColor(0));
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(9868950), 1);
		treeExpandColorTable.CollapseFill = new GradientFill(new ColorStop[3]
		{
			new ColorStop(ቖ.GetColor(16777215), 0f),
			new ColorStop(ቖ.GetColor(16777215), 0.4f),
			new ColorStop(ቖ.GetColor(11974326), 1f)
		}, 45);
		treeExpandColorTable.CollapseMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.CollapseMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.CollapseMouseOverFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandMouseOverFill = treeExpandColorTable.CollapseFill;
		ዅ.ExpandRectangle = treeExpandColorTable;
		ዅ.ExpandEllipse = treeExpandColorTable;
		ዅ.GridLines = ቖ.GetColor(14803425);
	}

	public static void _135E(TreeColorTable ዅ, ColorFactory ቖ)
	{
		TreeSelectionColors treeSelectionColors = (ዅ.Selection = new TreeSelectionColors());
		SelectionColorTable selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(10997232));
		treeSelectionColors.FullRowSelect = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(15066597));
		treeSelectionColors.FullRowSelectInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(64, 3238597));
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(96, 3238597), 1);
		treeSelectionColors.NodeMarker = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(64, 15066597));
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(96, 0), 1);
		treeSelectionColors.NodeMarkerInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(new ColorStop[4]
		{
			new ColorStop(ቖ.GetColor(16776409), 0f),
			new ColorStop(ቖ.GetColor(16770957), 0.4f),
			new ColorStop(ቖ.GetColor(16766792), 0.4f),
			new ColorStop(ቖ.GetColor(16770963), 1f)
		});
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(14536603), 1);
		selectionColorTable.BorderCornerRadius = 2;
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(192, 16773822), 1);
		treeSelectionColors.HighlightCells = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(ቖ.GetColor(16448251), ቖ.GetColor(15066597), 90f);
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(14277081), 1);
		selectionColorTable.BorderCornerRadius = 2;
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(228, 16777215), 1);
		treeSelectionColors.HighlightCellsInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(ቖ.GetColor(16448251), ቖ.GetColor(15066597), 90f);
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(14277081), 1);
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(228, 16777215), 1);
		treeSelectionColors.NodeHotTracking = selectionColorTable;
		TreeExpandColorTable treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(0), 1);
		treeExpandColorTable.CollapseFill = new SolidFill(ቖ.GetColor(5855577));
		treeExpandColorTable.CollapseMouseOverBorder = new SolidBorder(ቖ.GetColor(1885431), 1);
		treeExpandColorTable.CollapseMouseOverFill = new SolidFill(ቖ.GetColor(8577019));
		treeExpandColorTable.ExpandBorder = new SolidBorder(ቖ.GetColor(8684676), 1);
		treeExpandColorTable.ExpandFill = new SolidFill(ቖ.GetColor(16777215));
		treeExpandColorTable.ExpandMouseOverBorder = new SolidBorder(ቖ.GetColor(1885431), 1);
		treeExpandColorTable.ExpandMouseOverFill = new SolidFill(ቖ.GetColor(13430266));
		ዅ.ExpandTriangle = treeExpandColorTable;
		treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseForeground = new SolidFill(ቖ.GetColor(0));
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(9868950), 1);
		treeExpandColorTable.CollapseFill = new GradientFill(new ColorStop[3]
		{
			new ColorStop(ቖ.GetColor(16777215), 0f),
			new ColorStop(ቖ.GetColor(16777215), 0.4f),
			new ColorStop(ቖ.GetColor(11974326), 1f)
		}, 45);
		treeExpandColorTable.CollapseMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.CollapseMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.CollapseMouseOverFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandMouseOverFill = treeExpandColorTable.CollapseFill;
		ዅ.ExpandRectangle = treeExpandColorTable;
		ዅ.ExpandEllipse = treeExpandColorTable;
		ዅ.GridLines = ቖ.GetColor(14803425);
	}

	public static void _135F(TreeColorTable ዅ, ColorFactory ቖ)
	{
		TreeSelectionColors treeSelectionColors = (ዅ.Selection = new TreeSelectionColors());
		SelectionColorTable selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(12904698));
		treeSelectionColors.FullRowSelect = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(15066597));
		treeSelectionColors.FullRowSelectInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(64, 3238597));
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(96, 3238597), 1);
		treeSelectionColors.NodeMarker = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new SolidFill(ቖ.GetColor(64, 15066597));
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(96, 0), 1);
		treeSelectionColors.NodeMarkerInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(new ColorStop[2]
		{
			new ColorStop(ቖ.GetColor(15857917), 0f),
			new ColorStop(ቖ.GetColor(14020604), 1f)
		});
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(10084093), 1);
		selectionColorTable.BorderCornerRadius = 2;
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(192, 16186365), 1);
		treeSelectionColors.HighlightCells = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(ቖ.GetColor(16316664), ቖ.GetColor(15066597), 90f);
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(14277081), 1);
		selectionColorTable.BorderCornerRadius = 2;
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(228, 16448251), 1);
		treeSelectionColors.HighlightCellsInactive = selectionColorTable;
		selectionColorTable = new SelectionColorTable();
		selectionColorTable.Fill = new GradientFill(ቖ.GetColor(16120573), ቖ.GetColor(15267325), 90f);
		selectionColorTable.Border = new SolidBorder(ቖ.GetColor(14217466), 1);
		selectionColorTable.InnerBorder = new SolidBorder(ቖ.GetColor(228, 16317694), 1);
		treeSelectionColors.NodeHotTracking = selectionColorTable;
		TreeExpandColorTable treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(0), 1);
		treeExpandColorTable.CollapseFill = new SolidFill(ቖ.GetColor(5855577));
		treeExpandColorTable.CollapseMouseOverBorder = new SolidBorder(ቖ.GetColor(1885431), 1);
		treeExpandColorTable.CollapseMouseOverFill = new SolidFill(ቖ.GetColor(8577019));
		treeExpandColorTable.ExpandBorder = new SolidBorder(ቖ.GetColor(8684676), 1);
		treeExpandColorTable.ExpandFill = new SolidFill(ቖ.GetColor(16777215));
		treeExpandColorTable.ExpandMouseOverBorder = new SolidBorder(ቖ.GetColor(1885431), 1);
		treeExpandColorTable.ExpandMouseOverFill = new SolidFill(ቖ.GetColor(13430266));
		ዅ.ExpandTriangle = treeExpandColorTable;
		treeExpandColorTable = new TreeExpandColorTable();
		treeExpandColorTable.CollapseForeground = new SolidFill(ቖ.GetColor(0));
		treeExpandColorTable.CollapseBorder = new SolidBorder(ቖ.GetColor(9868950), 1);
		treeExpandColorTable.CollapseFill = new GradientFill(new ColorStop[3]
		{
			new ColorStop(ቖ.GetColor(16777215), 0f),
			new ColorStop(ቖ.GetColor(16777215), 0.4f),
			new ColorStop(ቖ.GetColor(11974326), 1f)
		}, 45);
		treeExpandColorTable.CollapseMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.CollapseMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.CollapseMouseOverFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandFill = treeExpandColorTable.CollapseFill;
		treeExpandColorTable.ExpandMouseOverForeground = treeExpandColorTable.CollapseForeground;
		treeExpandColorTable.ExpandMouseOverBorder = treeExpandColorTable.CollapseBorder;
		treeExpandColorTable.ExpandMouseOverFill = treeExpandColorTable.CollapseFill;
		ዅ.ExpandRectangle = treeExpandColorTable;
		ዅ.ExpandEllipse = treeExpandColorTable;
		ዅ.GridLines = ቖ.GetColor(15592941);
	}
}
internal class _2771 : NodeExpandDisplay
{
	public override void DrawExpandButton(NodeExpandPartRendererEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		if (!e.ExpandPartBounds.IsEmpty)
		{
			SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
			e.Graphics.SmoothingMode = (SmoothingMode)4;
			int width = Dpi.Width5;
			Rectangle rectangle = new Rectangle(e.ExpandPartBounds.X, e.ExpandPartBounds.Y + (e.ExpandPartBounds.Height - 8) / 2, width, Dpi.Height8);
			GraphicsPath val = null;
			if (e.Node.Expanded)
			{
				val = new GraphicsPath();
				val.AddLine(rectangle.X, rectangle.Y + width, rectangle.X + width, rectangle.Y);
				val.AddLine(rectangle.X + width, rectangle.Y, rectangle.X + width, rectangle.Y + width);
				val.CloseAllFigures();
			}
			else
			{
				val = new GraphicsPath();
				val.AddLine(rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom);
				val.AddLine(rectangle.X, rectangle.Bottom, rectangle.X + Dpi.Width4, rectangle.Y + rectangle.Height / 2);
				val.CloseAllFigures();
			}
			Brush backgroundBrush = GetBackgroundBrush(e);
			if (backgroundBrush != null)
			{
				e.Graphics.FillPath(backgroundBrush, val);
				backgroundBrush.Dispose();
			}
			Pen borderPen = GetBorderPen(e);
			if (borderPen != null)
			{
				e.Graphics.DrawPath(borderPen, val);
				borderPen.Dispose();
			}
			e.Graphics.SmoothingMode = smoothingMode;
			if (val != null)
			{
				val.Dispose();
			}
		}
	}
}
internal class _2772
{
	private ᙝ[] __2774;

	private ᙝ[] __2775;

	private ᙝ[] __2776;

	private TreeSelectionColors __2777;

	public TreeSelectionColors _2773
	{
		get
		{
			return __2777;
		}
		set
		{
			__2777 = value;
		}
	}

	public void _273E(SelectionRendererEventArgs _108F)
	{
		if (_108F.SelectionBoxStyle == eSelectionStyle.HighlightCells)
		{
			_277B(_108F);
		}
		else if (_108F.SelectionBoxStyle == eSelectionStyle.FullRowSelect)
		{
			_2778(_108F);
		}
		else if (_108F.SelectionBoxStyle == eSelectionStyle.NodeMarker)
		{
			_277E(_108F);
		}
	}

	public void _2740(SelectionRendererEventArgs _108F)
	{
		ᙝ[] array = _277D();
		Graphics graphics = _108F.Graphics;
		Rectangle bounds = _108F.Bounds;
		bounds.Width--;
		bounds.Height--;
		ᙝ[] array2 = array;
		foreach (ᙝ ᙝ in array2)
		{
			ᙝ.Paint(graphics, bounds);
		}
	}

	private void _2778(SelectionRendererEventArgs _108F)
	{
		ᙝ[] array = _2779(_108F.TreeActive);
		Graphics graphics = _108F.Graphics;
		Rectangle bounds = _108F.Bounds;
		ᙝ[] array2 = array;
		foreach (ᙝ ᙝ in array2)
		{
			ᙝ.Paint(graphics, bounds);
		}
	}

	private ᙝ[] _2779(bool _277A)
	{
		if (__2774 == null)
		{
			__2774 = new ᙝ[1];
			__2774[0] = new _2783();
		}
		_2783 _2783 = (_2783)__2774[0];
		SelectionColorTable selectionColorTable = (_277A ? __2777.FullRowSelect : __2777.FullRowSelectInactive);
		_2783.ᙵ = selectionColorTable.Fill;
		_2783.ᙴ = selectionColorTable.Border;
		return __2774;
	}

	private void _277B(SelectionRendererEventArgs _108F)
	{
		ᙝ[] array = _277C(_108F.TreeActive);
		Graphics graphics = _108F.Graphics;
		Rectangle bounds = _108F.Bounds;
		bounds.Width--;
		bounds.Height--;
		ᙝ[] array2 = array;
		foreach (ᙝ ᙝ in array2)
		{
			ᙝ.Paint(graphics, bounds);
		}
	}

	private ᙝ[] _277C(bool _277A)
	{
		SelectionColorTable selectionColorTable = (_277A ? __2777.HighlightCells : __2777.HighlightCellsInactive);
		if (__2775 != null)
		{
			_ = ((_2783)__2775[0])._2784;
			if (((_2783)__2775[0])._2784.TopLeft == selectionColorTable.BorderCornerRadius)
			{
				goto IL_009c;
			}
		}
		__2775 = new ᙝ[1];
		_2783 _2783 = new _2783();
		if (selectionColorTable.BorderCornerRadius > 0)
		{
			_2783._2784 = new CornerRadius(selectionColorTable.BorderCornerRadius);
			_2783 _277F = new _2783();
			_2783._277F = _277F;
		}
		__2775[0] = _2783;
		goto IL_009c;
		IL_009c:
		_2783 _27832 = (_2783)__2775[0];
		_27832.ᙵ = selectionColorTable.Fill;
		_27832.ᙴ = selectionColorTable.Border;
		if (_27832._277F != null)
		{
			_27832 = (_2783)_27832._277F;
			_27832.ᙴ = selectionColorTable.InnerBorder;
		}
		return __2775;
	}

	private ᙝ[] _277D()
	{
		if (__2776 == null)
		{
			__2776 = new ᙝ[1];
			_2783 _2783 = new _2783();
			_2783._2784 = new CornerRadius(2);
			_2783 _277F = new _2783();
			_2783._277F = _277F;
			__2776[0] = _2783;
		}
		SelectionColorTable nodeHotTracking = __2777.NodeHotTracking;
		_2783 _27832 = (_2783)__2776[0];
		_27832.ᙵ = nodeHotTracking.Fill;
		_27832.ᙴ = nodeHotTracking.Border;
		_27832 = (_2783)_27832._277F;
		_27832.ᙴ = nodeHotTracking.InnerBorder;
		return __2776;
	}

	private void _277E(SelectionRendererEventArgs _108F)
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		Rectangle bounds = _108F.Bounds;
		int num = 4;
		bounds.Inflate(1, 1);
		bounds.Width--;
		bounds.Height--;
		Rectangle bounds2 = _108F.Bounds;
		bounds2.Inflate(num, num);
		bounds2.Width--;
		bounds2.Height--;
		SelectionColorTable selectionColorTable = (_108F.TreeActive ? __2777.NodeMarker : __2777.NodeMarkerInactive);
		if (selectionColorTable.Border != null)
		{
			Pen val = selectionColorTable.Border.CreatePen();
			if (val != null)
			{
				_108F.Graphics.DrawRectangle(val, bounds);
				_108F.Graphics.DrawRectangle(val, bounds2);
				val.Dispose();
			}
		}
		if (selectionColorTable.Fill != null)
		{
			Brush val2 = selectionColorTable.Fill.CreateBrush(bounds2);
			if (val2 != null)
			{
				Region val3 = new Region(bounds2);
				val3.Exclude(bounds);
				_108F.Graphics.FillRegion(val2, val3);
				val2.Dispose();
			}
		}
	}
}
internal class _278A
{
	private Color __278C;

	public Color _278B
	{
		get
		{
			return __278C;
		}
		set
		{
			__278C = value;
		}
	}

	public void _278D(DragDropMarkerRendererEventArgs ؿ)
	{
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		Graphics graphics = ؿ.Graphics;
		Rectangle bounds = ؿ.Bounds;
		if (bounds.IsEmpty || __278C.IsEmpty)
		{
			return;
		}
		if (bounds.Width == AdvTree.ؠ)
		{
			SolidBrush val = new SolidBrush(__278C);
			try
			{
				Pen val2 = new Pen((Brush)(object)val, 1f);
				try
				{
					Point point = new Point(bounds.X + 4, bounds.Y);
					graphics.DrawLine(val2, point.X, point.Y, point.X, bounds.Bottom - 1);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				GraphicsPath val3 = new GraphicsPath();
				try
				{
					val3.AddLine(bounds.X, bounds.Y, bounds.X + 8, bounds.Y);
					val3.AddLine(bounds.X + 8, bounds.Y, bounds.X + 4, bounds.Y + 4);
					val3.CloseAllFigures();
					graphics.FillPath((Brush)(object)val, val3);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				GraphicsPath val4 = new GraphicsPath();
				try
				{
					val4.AddLine(bounds.X, bounds.Bottom, bounds.X + 8, bounds.Bottom);
					val4.AddLine(bounds.X + 8, bounds.Bottom, bounds.X + 4, bounds.Bottom - 4);
					val4.CloseAllFigures();
					graphics.FillPath((Brush)(object)val, val4);
					return;
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		SolidBrush val5 = new SolidBrush(__278C);
		try
		{
			Pen val6 = new Pen((Brush)(object)val5, 1f);
			try
			{
				Point point2 = new Point(bounds.X, bounds.Y + 4);
				graphics.DrawLine(val6, point2.X, point2.Y, bounds.Right - 1, point2.Y);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
			GraphicsPath val7 = new GraphicsPath();
			try
			{
				val7.AddLine(bounds.X, bounds.Y, bounds.X, bounds.Y + 8);
				val7.AddLine(bounds.X, bounds.Y + 8, bounds.X + 4, bounds.Y + 4);
				val7.CloseAllFigures();
				graphics.FillPath((Brush)(object)val5, val7);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
			GraphicsPath val8 = new GraphicsPath();
			try
			{
				val8.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Y + 8);
				val8.AddLine(bounds.Right, bounds.Y + 8, bounds.Right - 4, bounds.Y + 4);
				val8.CloseAllFigures();
				graphics.FillPath((Brush)(object)val5, val8);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
	}
}
internal class _278E
{
	private static readonly int _278F = 4;

	public void _2790(NodeRendererEventArgs ؿ)
	{
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		Color color = ؿ.Color;
		Node node = ؿ.Node;
		if (color.IsEmpty || color.A == 0 || node.Cells.Count == 0)
		{
			return;
		}
		Graphics graphics = ؿ.Graphics;
		Rectangle bounds = node.Bounds;
		Cell cell = node.Cells[node.Cells.Count - 1];
		if (cell.CheckBoxVisible && ܥ._073A(cell.CheckBoxAlignment, _0737: true, eView.Tile) == eHorizontalAlign.Right)
		{
			bounds.Width -= cell.CheckBoxBounds.Right - bounds.X + _278F;
			bounds.X = cell.CheckBoxBounds.Right + _278F;
		}
		else if (!cell.ImageBoundsRelative.IsEmpty && ܥ._0735(cell.ImageAlignment, _0737: true, eView.Tile) == eHorizontalAlign.Right)
		{
			bounds.Width -= cell.ImageBounds.Right - bounds.X + _278F;
			bounds.X = cell.ImageBounds.Right + _278F;
		}
		else
		{
			if (ؿ.Style.TextAlignment != 0)
			{
				return;
			}
			Rectangle textBounds = cell.TextBounds;
			if (cell.TextMarkupBody == null)
			{
				textBounds.Width = TextDrawing.MeasureString(graphics, cell.Text, ؿ.Style.Font).Width;
			}
			bounds.Width -= textBounds.Right - bounds.X + _278F;
			bounds.X = textBounds.Right + _278F;
		}
		Pen val = new Pen(color, 1f);
		try
		{
			graphics.DrawLine(val, bounds.X, bounds.Y + bounds.Height / 2, bounds.Right, bounds.Y + bounds.Height / 2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
internal class _2791
{
	private static ᢏ __2461;

	public static Office2007CheckBoxColorTable ᘽ = null;

	public static ᢏ _2460
	{
		get
		{
			return __2461;
		}
		set
		{
			__2461 = value;
		}
	}

	public static void _2792(NodeCellRendererEventArgs _2793)
	{
		if (_2793.Cell.CheckBoxVisible)
		{
			_2794(_2793);
		}
		if (!_2793.Cell.Images._26EE.IsEmpty)
		{
			_2795(_2793);
		}
		_2422(_2793);
	}

	public static void _2794(NodeCellRendererEventArgs _2793)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		if (!_2793.Cell.CheckBoxVisible)
		{
			return;
		}
		Cell cell = _2793.Cell;
		Rectangle checkBoxBoundsRelative = cell.CheckBoxBoundsRelative;
		checkBoxBoundsRelative.Offset(_2793._2750);
		if (_2793._2751 != null)
		{
			Image val = _2793._2751;
			if ((int)cell.CheckState == 0)
			{
				val = _2793._2752;
			}
			else if ((int)cell.CheckState == 2)
			{
				val = _2793._2753;
			}
			if (val != null)
			{
				_2793.Graphics.DrawImage(val, checkBoxBoundsRelative);
			}
		}
		else if (__2461 != null)
		{
			Office2007CheckBoxStateColorTable ዅ = ᢗ(_2793);
			if (cell.CheckBoxStyle == eCheckBoxStyle.CheckBox)
			{
				__2461.ᢓ(_2793.Graphics, checkBoxBoundsRelative, ዅ, cell.CheckState);
			}
			else
			{
				__2461.ᢐ(_2793.Graphics, checkBoxBoundsRelative, ዅ, cell.Checked);
			}
		}
		else
		{
			ButtonState val2 = (ButtonState)0;
			if (_2793.Cell.Checked)
			{
				val2 = (ButtonState)1024;
			}
			ControlPaint.DrawCheckBox(_2793.Graphics, checkBoxBoundsRelative, val2);
		}
	}

	private static Office2007CheckBoxStateColorTable ᢗ(NodeCellRendererEventArgs ؿ)
	{
		Cell cell = ؿ.Cell;
		if (ᘽ != null && BarFunctions.IsOffice2007Style(ؿ.ԫ.ߕ))
		{
			Office2007CheckBoxColorTable office2007CheckBoxColorTable = ᘽ;
			if (!cell.GetEnabled())
			{
				return office2007CheckBoxColorTable.Disabled;
			}
			return office2007CheckBoxColorTable.Default;
		}
		ColorScheme ԫ = ؿ.ԫ;
		Office2007CheckBoxStateColorTable office2007CheckBoxStateColorTable = new Office2007CheckBoxStateColorTable();
		if (!cell.GetEnabled())
		{
			office2007CheckBoxStateColorTable.CheckBackground = new LinearGradientColorTable(ԫ.MenuBackground, Color.Empty);
			office2007CheckBoxStateColorTable.CheckBorder = ԫ.ItemDisabledText;
			office2007CheckBoxStateColorTable.CheckInnerBorder = ԫ.ItemDisabledText;
			office2007CheckBoxStateColorTable.CheckInnerBackground = new LinearGradientColorTable();
			office2007CheckBoxStateColorTable.CheckSign = new LinearGradientColorTable(ԫ.ItemDisabledText, Color.Empty);
			office2007CheckBoxStateColorTable.Text = ԫ.ItemDisabledText;
		}
		else
		{
			office2007CheckBoxStateColorTable.CheckBackground = new LinearGradientColorTable(ԫ.MenuBackground, Color.Empty);
			office2007CheckBoxStateColorTable.CheckBorder = ԫ.PanelBorder;
			office2007CheckBoxStateColorTable.CheckInnerBorder = ColorBlendFactory.ዀ(ԫ.PanelBorder, Color.White);
			office2007CheckBoxStateColorTable.CheckInnerBackground = new LinearGradientColorTable(ԫ.MenuBackground, Color.Empty);
			office2007CheckBoxStateColorTable.CheckSign = new LinearGradientColorTable(ԫ.ItemText, Color.Empty);
			office2007CheckBoxStateColorTable.Text = ԫ.ItemText;
		}
		return office2007CheckBoxStateColorTable;
	}

	public static void _2795(NodeCellRendererEventArgs _2793)
	{
		if (!_2793.Cell.Images._26EE.IsEmpty)
		{
			Rectangle imageBoundsRelative = _2793.Cell.ImageBoundsRelative;
			imageBoundsRelative.Offset(_2793._2750);
			Image val = _2796(_2793.Cell);
			if (val != null)
			{
				Size size = Dpi.ImageSize(val.Size);
				_2793.Graphics.DrawImage(val, imageBoundsRelative.X + (imageBoundsRelative.Width - size.Width) / 2, imageBoundsRelative.Y + (imageBoundsRelative.Height - size.Height) / 2, size.Width, size.Height);
			}
		}
	}

	public static void _2422(NodeCellRendererEventArgs _2793)
	{
		Cell cell = _2793.Cell;
		if ((cell.HostedControl == null && cell.HostedItem == null && (cell.DisplayText == "" || _2793.Style.TextColor.IsEmpty)) || cell.TextContentBounds.IsEmpty)
		{
			return;
		}
		Rectangle textContentBounds = _2793.Cell.TextContentBounds;
		textContentBounds.Offset(_2793._2750);
		Graphics graphics = _2793.Graphics;
		if (cell.HostedControl != null)
		{
			if (!cell.HostedControl.Visible)
			{
				cell.HostedControl.Visible = true;
			}
			return;
		}
		if (cell.HostedItem != null)
		{
			BaseItem hostedItem = cell.HostedItem;
			if (hostedItem.ItemAlignment == eItemAlignment.Near)
			{
				hostedItem.LeftInternal = textContentBounds.X;
			}
			else if (hostedItem.ItemAlignment == eItemAlignment.Far)
			{
				hostedItem.LeftInternal = textContentBounds.X + (textContentBounds.Width - hostedItem.WidthInternal);
			}
			else if (hostedItem.ItemAlignment == eItemAlignment.Center)
			{
				hostedItem.LeftInternal = textContentBounds.X + (textContentBounds.Width - hostedItem.WidthInternal) / 2;
			}
			if (hostedItem.DisplayRectangle.Height < textContentBounds.Height)
			{
				hostedItem.TopInternal = textContentBounds.Y + (textContentBounds.Height - hostedItem.DisplayRectangle.Height) / 2;
			}
			else
			{
				hostedItem.TopInternal = textContentBounds.Y;
			}
			hostedItem.Displayed = true;
			Region clip = graphics.Clip;
			Rectangle rectangle = textContentBounds;
			rectangle.Inflate(2, 1);
			graphics.SetClip(rectangle, (CombineMode)1);
			hostedItem.Paint(_2793.က);
			if (clip != null)
			{
				graphics.Clip = clip;
				clip.Dispose();
			}
			return;
		}
		Font font = _2793.Style.Font;
		if (textContentBounds.Width > 1 && textContentBounds.Height > 1)
		{
			if (cell.TextMarkupBody == null)
			{
				TextDrawing.DrawString(graphics, cell.DisplayText, font, _2793.Style.TextColor, textContentBounds, _2793.Style.ݽ);
				return;
			}
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, _2793.Style.TextColor, rightToLeft: false);
			markupDrawContext.HotKeyPrefixVisible = (_2793.Style.ݽ & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
			Rectangle bounds = Rectangle.Empty;
			bounds = _2793.Style.TextLineAlignment switch
			{
				eStyleTextAlignment.Center => new Rectangle(textContentBounds.X, textContentBounds.Y + (textContentBounds.Height - cell.TextMarkupBody.Bounds.Height) / 2, cell.TextMarkupBody.Bounds.Width, cell.TextMarkupBody.Bounds.Height), 
				eStyleTextAlignment.Near => new Rectangle(textContentBounds.X, textContentBounds.Y, cell.TextMarkupBody.Bounds.Width, cell.TextMarkupBody.Bounds.Height), 
				_ => new Rectangle(textContentBounds.X, textContentBounds.Y + (textContentBounds.Height - cell.TextMarkupBody.Bounds.Height), cell.TextMarkupBody.Bounds.Width, cell.TextMarkupBody.Bounds.Height), 
			};
			cell.TextMarkupBody.Bounds = bounds;
			cell.TextMarkupBody.Render(markupDrawContext);
		}
	}

	private static Image _2796(Cell ڂ)
	{
		Image val = ڂ.Images.Image;
		bool enabled = ڂ.GetEnabled();
		if (!enabled && (ڂ.Images.ImageDisabled != null || ڂ.Images.ImageDisabledIndex >= 0 || ڂ.Images._26EC != null))
		{
			if (ڂ.Images._26EC != null)
			{
				return ڂ.Images._26EC;
			}
			if (ڂ.Images.ImageDisabled != null)
			{
				return ڂ.Images.ImageDisabled;
			}
			if (ڂ.Images.ImageDisabledIndex >= 0)
			{
				return ڂ.Images._2702(ڂ.Images.ImageDisabledIndex);
			}
		}
		if (val == null && !string.IsNullOrEmpty(ڂ.Images.ImageKey))
		{
			val = ڂ.Images._2704(ڂ.Images.ImageKey);
		}
		if (val == null && ڂ.Images.ImageIndex >= 0)
		{
			val = ڂ.Images._2702(ڂ.Images.ImageIndex);
		}
		if (!enabled && val is Bitmap)
		{
			ڂ.Images._2700();
			ڂ.Images._26EC = (Image)(object)ᶣ.ᶧ((val is Bitmap) ? val : null);
			if (ڂ.Images._26EC != null)
			{
				return ڂ.Images._26EC;
			}
			return val;
		}
		if (ڂ.IsMouseOver && (ڂ.Images.ImageMouseOver != null || ڂ.Images.ImageMouseOverIndex >= 0 || !string.IsNullOrEmpty(ڂ.Images.ImageMouseOverKey)))
		{
			val = ((ڂ.Images.ImageMouseOver != null) ? ڂ.Images.ImageMouseOver : ((ڂ.Images.ImageMouseOverIndex < 0) ? ڂ.Images._2704(ڂ.Images.ImageMouseOverKey) : ڂ.Images._2702(ڂ.Images.ImageMouseOverIndex)));
		}
		else if (ڂ.Parent.Expanded && (ڂ.Images.ImageExpanded != null || ڂ.Images.ImageExpandedIndex >= 0 || !string.IsNullOrEmpty(ڂ.Images.ImageExpandedKey)))
		{
			val = ((ڂ.Images.ImageExpanded != null) ? ڂ.Images.ImageExpanded : ((ڂ.Images.ImageExpandedIndex < 0) ? ڂ.Images._2704(ڂ.Images.ImageExpandedKey) : ڂ.Images._2702(ڂ.Images.ImageExpandedIndex)));
		}
		return val;
	}

	public static Font _2797(AdvTree ᝢ, Cell ڂ)
	{
		Font font = ((Control)ᝢ).Font;
		ElementStyle elementStyle = null;
		if (ڂ.StyleNormal == null)
		{
			elementStyle = ((ᝢ.NodeStyle == null) ? new ElementStyle() : ᝢ.NodeStyle);
			elementStyle = ((ᝢ.CellStyleDefault == null) ? ElementStyle.GetDefaultCellStyle(elementStyle) : ᝢ.CellStyleDefault);
		}
		else
		{
			elementStyle = ڂ.StyleNormal;
		}
		if (elementStyle != null && elementStyle.Font != null)
		{
			font = elementStyle.Font;
		}
		return font;
	}
}
internal class _29DB
{
	public ElementStyle ߕ;

	public Graphics ܓ;

	public Cell _0746;

	public Point _2750 = Point.Empty;

	public _29DB()
	{
	}

	public _29DB(ElementStyle style, Graphics g, Cell cell, Point cellOffset)
	{
		ߕ = style;
		ܓ = g;
		_0746 = cell;
		_2750 = cellOffset;
	}
}
internal class _29DF
{
	public Node _27B0;

	public Graphics ܓ;

	public Rectangle ঋ = Rectangle.Empty;

	public Color _29E0 = Color.Empty;

	public Color _29E1 = Color.Empty;

	public int ܐ = 4;
}

using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.AdvTree.Display;

public class NodeSystemRenderer : TreeRenderer
{
	private NodeExpandEllipseDisplay _2754 = new NodeExpandEllipseDisplay();

	private NodeExpandRectDisplay _2755 = new NodeExpandRectDisplay();

	private _2771 _2756 = new _2771();

	private NodeExpandImageDisplay _2757 = new NodeExpandImageDisplay();

	private ElementStyleDisplayInfo _2732 = new ElementStyleDisplayInfo();

	private _2772 _2758 = new _2772();

	private LineConnectorDisplay _2759;

	private _278A _275A = new _278A();

	private ស _275B = new ស();

	private ᢏ _275C = new ᢏ();

	private _278E __275D = new _278E();

	protected ElementStyleDisplayInfo GetElementStyleDisplayInfo(ElementStyle style, Graphics g, Rectangle bounds)
	{
		_2732.Style = style;
		_2732.Graphics = g;
		_2732.Bounds = bounds;
		return _2732;
	}

	private NodeConnectorDisplay _275E(NodeConnector _0BDE)
	{
		NodeConnectorDisplay result = null;
		if (_0BDE == null)
		{
			return null;
		}
		if (_0BDE.ConnectorType == eNodeConnectorType.Line)
		{
			if (_2759 == null)
			{
				_2759 = new LineConnectorDisplay();
			}
			result = _2759;
		}
		return result;
	}

	public override void DrawNodeBackground(NodeRendererEventArgs e)
	{
		ElementStyleDisplayInfo elementStyleDisplayInfo = GetElementStyleDisplayInfo(e.Style, e.Graphics, e.NodeBounds);
		ElementStyleDisplay.Paint(elementStyleDisplayInfo);
		base.DrawNodeBackground(e);
	}

	public override void DrawNodeExpandPart(NodeExpandPartRendererEventArgs e)
	{
		NodeExpandDisplay nodeExpandDisplay = _275F(e._276F);
		nodeExpandDisplay.ColorTable = base.ColorTable;
		nodeExpandDisplay.DrawExpandButton(e);
		nodeExpandDisplay.ColorTable = null;
		base.DrawNodeExpandPart(e);
	}

	private NodeExpandDisplay _275F(eExpandButtonType ؿ)
	{
		NodeExpandDisplay result = null;
		switch (ؿ)
		{
		case eExpandButtonType.Rectangle:
			result = _2755;
			break;
		case eExpandButtonType.Triangle:
			result = _2756;
			break;
		case eExpandButtonType.Ellipse:
			result = _2754;
			break;
		case eExpandButtonType.Image:
			result = _2757;
			break;
		}
		return result;
	}

	public override void DrawCellBackground(NodeCellRendererEventArgs e)
	{
		ElementStyleDisplayInfo elementStyleDisplayInfo = GetElementStyleDisplayInfo(e.Style, e.Graphics, DisplayHelp.GetDrawRectangle(e.CellBounds));
		ElementStyleDisplay.Paint(elementStyleDisplayInfo);
		base.DrawCellBackground(e);
	}

	public override void DrawCellCheckBox(NodeCellRendererEventArgs e)
	{
		_2791._2460 = _275C;
		if (base._2760 != null)
		{
			_2791.ᘽ = base._2760.CheckBoxItem;
		}
		_2791._2794(e);
		base.DrawCellCheckBox(e);
		_2791._2460 = null;
	}

	public override void DrawCellImage(NodeCellRendererEventArgs e)
	{
		_2791._2795(e);
		base.DrawCellImage(e);
	}

	public override void DrawCellText(NodeCellRendererEventArgs e)
	{
		_2791._2422(e);
		base.DrawCellText(e);
	}

	public override void DrawSelection(SelectionRendererEventArgs e)
	{
		_2758._2773 = base.ColorTable.Selection;
		_2758._273E(e);
		base.DrawSelection(e);
	}

	public override void DrawHotTracking(SelectionRendererEventArgs e)
	{
		_2758._2773 = base.ColorTable.Selection;
		_2758._2740(e);
		base.DrawHotTracking(e);
	}

	public override void DrawConnector(ConnectorRendererEventArgs e)
	{
		_275E(e.NodeConnector)?.DrawConnector(e);
		base.DrawConnector(e);
	}

	public override void DrawTreeBackground(TreeBackgroundRendererEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		AdvTree advTree = e.AdvTree;
		Graphics graphics = e.Graphics;
		if (!((Control)advTree).BackColor.IsEmpty)
		{
			SolidBrush val = new SolidBrush(((Control)advTree).BackColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val, ((Control)advTree).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo();
		elementStyleDisplayInfo.Bounds = ((Control)advTree).DisplayRectangle;
		elementStyleDisplayInfo.Graphics = graphics;
		elementStyleDisplayInfo.Style = advTree.BackgroundStyle;
		ElementStyleDisplay.Paint(elementStyleDisplayInfo);
		base.DrawTreeBackground(e);
	}

	public override void DrawDragDropMarker(DragDropMarkerRendererEventArgs e)
	{
		_275A._278B = base.ColorTable.DragDropMarker;
		_275A._278D(e);
		base.DrawDragDropMarker(e);
	}

	public override void DrawColumnHeader(ColumnHeaderRendererEventArgs e)
	{
		ElementStyleDisplayInfo elementStyleDisplayInfo = GetElementStyleDisplayInfo(e.Style, e.Graphics, e.Bounds);
		_275B.ហ(e, elementStyleDisplayInfo);
		base.DrawColumnHeader(e);
	}

	public override void DrawTileGroupLine(NodeRendererEventArgs e)
	{
		__275D._2790(e);
		OnRenderTileGroupLine(e);
	}
}

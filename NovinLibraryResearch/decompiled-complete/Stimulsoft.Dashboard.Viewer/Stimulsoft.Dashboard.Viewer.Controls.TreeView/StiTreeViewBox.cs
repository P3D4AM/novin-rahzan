using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Components.TreeViewBox;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Viewer.Controls.TreeView;

[ToolboxItem(false)]
public class StiTreeViewBox : Panel
{
	private DateTime dropDownClosedTime;

	private StiControlColorCollection colors;

	private bool isHot;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiControlColorCollection Colors
	{
		get
		{
			return colors;
		}
		set
		{
			if (value != colors)
			{
				colors = value;
				colors.Control = (Control)(object)this;
				((Control)this).Invalidate();
			}
		}
	}

	private StiTreeView TreeView { get; } = new StiTreeView();


	internal StiKeyTreeViewBoxMeter KeyMeter { get; set; }

	public StiFormatService TextFormat { get; set; }

	internal bool CheckBoxes
	{
		get
		{
			return ((TreeView)TreeView).CheckBoxes;
		}
		set
		{
			((TreeView)TreeView).CheckBoxes = value;
		}
	}

	private StiDropDown DropDownPopup { get; set; }

	private bool IsHot
	{
		get
		{
			return isHot;
		}
		set
		{
			if (isHot != value)
			{
				isHot = value;
				((Control)this).Invalidate();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public StiTreeNode SelectedNode
	{
		get
		{
			return ((TreeView)TreeView).SelectedNode as StiTreeNode;
		}
		set
		{
			((TreeView)TreeView).SelectedNode = (TreeNode)(object)value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public TreeNodeCollection Nodes => ((TreeView)TreeView).Nodes;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool DroppedDown
	{
		get
		{
			if (DropDownPopup != null)
			{
				return ((ToolStripDropDown)DropDownPopup).Visible;
			}
			return false;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool AllowCloseDropDownOnDoubleClick { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool AllowShowCheckedZones { get; set; } = true;


	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsTransparent { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Color BackColor
	{
		get
		{
			if (!IsTransparent)
			{
				return ((Control)this).BackColor;
			}
			return Color.Transparent;
		}
		set
		{
			((Control)this).BackColor = (IsTransparent ? Color.Transparent : value);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Color? DropDownBackColor { get; set; }

	public event TreeViewEventHandler AfterSelect
	{
		add
		{
			((TreeView)TreeView).AfterSelect += value;
		}
		remove
		{
			((TreeView)TreeView).AfterSelect -= value;
		}
	}

	public event TreeViewCancelEventHandler BeforeSelect
	{
		add
		{
			((TreeView)TreeView).BeforeSelect += value;
		}
		remove
		{
			((TreeView)TreeView).BeforeSelect -= value;
		}
	}

	public event TreeViewEventHandler AfterCheck
	{
		add
		{
			((TreeView)TreeView).AfterCheck += value;
		}
		remove
		{
			((TreeView)TreeView).AfterCheck -= value;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		DrawItem(graphics);
		if (AllowShowCheckedZones)
		{
			DrawItemZones(graphics);
		}
		DrawButton(graphics);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		IsHot = true;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		IsHot = false;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		IsHot = ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));
		((Control)this).Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		StiTreeNodeZone mouseOverZone = GetMouseOverZone();
		if (mouseOverZone != null)
		{
			if (mouseOverZone.IsAll)
			{
				mouseOverZone.Node.State = StiCheckState.Unchecked;
				mouseOverZone.Node.UpdateStates();
				InvokeSelectedIndexChanged();
				return;
			}
			if (mouseOverZone.IsStacked)
			{
				GetAllNodes().ForEach(delegate(StiTreeNode n)
				{
					n.State = StiCheckState.Unchecked;
					n.UpdateStates();
				});
				InvokeSelectedIndexChanged();
				return;
			}
			if (mouseOverZone.Node != null)
			{
				mouseOverZone.Node.State = StiCheckState.Unchecked;
				mouseOverZone.Node.UpdateStates();
				InvokeSelectedIndexChanged();
				return;
			}
		}
		if (!DroppedDown && (DateTime.Now - dropDownClosedTime).TotalSeconds > 0.5)
		{
			ShowDropDown();
		}
	}

	private void DrawItem(Graphics g)
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Color color = Colors.ForeColor;
		if (IsHot)
		{
			Color hotBackColor = Colors.HotBackColor;
			if (AllowShowCheckedZones)
			{
				List<StiTreeNodeZone> itemZones = GetItemZones();
				if (itemZones != null && itemZones.Any())
				{
					int num = itemZones.Last().Rect.Right + StiScale.I4;
					rectangle.X += num;
					rectangle.Width -= num;
				}
			}
			if (!rectangle.Contains(((Control)this).PointToClient(Cursor.Position)))
			{
				return;
			}
			StiDrawing.FillRectangle(g, hotBackColor, rectangle);
			color = Colors.HotForeColor;
		}
		if (!AllowShowCheckedZones)
		{
			TextFormatFlags val = (TextFormatFlags)32804;
			StiTreeNode selectedNode = SelectedNode;
			string text = ((selectedNode != null) ? ((TreeNode)selectedNode).Text : null);
			if (((Control)this).Text != null)
			{
				TextRenderer.DrawText((IDeviceContext)(object)g, text, ((Control)this).Font, rectangle, color, val);
			}
		}
	}

	private void DrawItemZones(Graphics g)
	{
		List<StiTreeNodeZone> itemZones = GetItemZones();
		if (itemZones == null || !itemZones.Any())
		{
			return;
		}
		foreach (StiTreeNodeZone item in itemZones)
		{
			bool flag = item.Rect.Contains(((Control)this).PointToClient(Cursor.Position));
			bool isHotClose = StiZonePainter.GetCloseRect(item.Rect).Contains(((Control)this).PointToClient(Cursor.Position));
			StiZonePainter.DrawItemZone(g, item.Text, ((Control)this).Font, flag, isHotClose, Colors.SelectedForeColor, Colors.SelectedBackColor, item.Rect);
		}
	}

	private void DrawButton(Graphics g)
	{
		Rectangle rect = new Rectangle(((Control)this).Width - GetButtonWidth(), 0, GetButtonWidth(), ((Control)this).Height);
		StiButtonPainter.DrawButton(g, rect, IsHot, Colors.GlyphColor, colors.HotGlyphColor);
	}

	private List<StiTreeNode> GetAllCheckedNodes()
	{
		if (Nodes.Count > 0 && Nodes[0].Checked && Nodes[0].Text == StiLocHelper.LocAll)
		{
			List<StiTreeNode> list = new List<StiTreeNode>();
			list.Add(Nodes[0] as StiTreeNode);
			return list;
		}
		return (from n in GetAllNodes()
			where n.State == StiCheckState.Checked && ((TreeNode)n).Tag is StiTreeItem && ((StiTreeItem)((TreeNode)n).Tag).Meter == KeyMeter
			select n).ToList();
	}

	internal List<StiTreeNode> GetAllNodes(TreeNodeCollection nodes = null)
	{
		if (nodes == null)
		{
			nodes = ((TreeView)TreeView).Nodes;
		}
		List<StiTreeNode> list = new List<StiTreeNode>();
		foreach (StiTreeNode node in nodes)
		{
			if (((TreeNode)node).Tag != null)
			{
				list.Add(node);
			}
			list.AddRange(GetAllNodes(((TreeNode)node).Nodes));
		}
		return list;
	}

	public void BeginUpdate()
	{
		((TreeView)TreeView).BeginUpdate();
	}

	public void EndUpdate()
	{
		((TreeView)TreeView).EndUpdate();
	}

	private void InvokeSelectedIndexChanged()
	{
		if (SelectedNode != null)
		{
			SelectedNode = null;
		}
		else if (Nodes.Count > 0)
		{
			SelectedNode = Nodes[0] as StiTreeNode;
		}
	}

	private StiTreeNodeZone GetMouseOverZone()
	{
		List<StiTreeNodeZone> itemZones = GetItemZones();
		foreach (StiTreeNodeZone item in itemZones)
		{
			if (GetCloseRect(item.Rect).Contains(((Control)this).PointToClient(Cursor.Position)))
			{
				return item;
			}
		}
		return null;
	}

	private Rectangle GetCloseRect(Rectangle rect)
	{
		return new Rectangle(rect.Right - (int)(13.0 * StiScale.Factor), rect.Y, (int)(8.0 * StiScale.Factor), rect.Height);
	}

	private List<StiTreeNodeZone> GetItemZones()
	{
		int x = 0;
		int index = 0;
		List<StiTreeNode> nodes = GetAllCheckedNodes();
		List<StiTreeNodeZone> list = nodes.Select(GetNodeZoneSize).Select(delegate(Size i)
		{
			Rectangle rect = new Rectangle(x, 0, i.Width + (int)(StiScale.Factor * 14.0), ((Control)this).Height);
			x += rect.Width + StiScale.I4;
			if (index == 0 && ((object)nodes[index]).ToString() == StiLocHelper.LocAll)
			{
				return StiTreeNodeZone.MakeAll(rect);
			}
			StiTreeNodeZone result = StiTreeNodeZone.Make(GetItemText(nodes[index]), rect, nodes[index]);
			index++;
			return result;
		}).ToList();
		if (!list.Any())
		{
			return list;
		}
		if (list.Last().Rect.Right <= ((Control)this).Width - GetButtonWidth())
		{
			return list;
		}
		string text = string.Format(Loc.Get("Dashboard", "NSelected"), list.Count);
		int width = TextRenderer.MeasureText(text, ((Control)this).Font).Width;
		return new List<StiTreeNodeZone> { StiTreeNodeZone.MakeStacked(text, new Rectangle(0, 0, width + (int)(14.0 * StiScale.Factor), ((Control)this).Height)) };
	}

	private Size GetNodeZoneSize(StiTreeNode node)
	{
		return TextRenderer.MeasureText(GetItemText(node), ((Control)this).Font);
	}

	private string GetItemText(StiTreeNode node)
	{
		if (!(((TreeNode)node).Text == StiLocHelper.LocAll))
		{
			return ((TreeNode)node).Text;
		}
		return StiLocHelper.LocAll;
	}

	protected virtual int GetButtonWidth()
	{
		return SystemInformation.HorizontalScrollBarArrowWidth;
	}

	protected virtual void CloseDropDown()
	{
		StiDropDown dropDownPopup = DropDownPopup;
		if (dropDownPopup != null)
		{
			((ToolStripDropDown)dropDownPopup).Close();
		}
	}

	protected virtual void ShowDropDown()
	{
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		int val = StiScale.YYI(StiElementConsts.TreeView.ItemHeight) * Math.Max(Nodes.Count, 10) + StiScale.I6;
		val = Math.Min(Screen.FromControl((Control)(object)this).WorkingArea.Height - StiScale.I8, val);
		if (DropDownPopup == null)
		{
			StiDropDown stiDropDown = new StiDropDown();
			((Control)stiDropDown).AutoSize = false;
			((Control)stiDropDown).Width = ((Control)this).Width;
			((Control)stiDropDown).Height = val;
			((ToolStrip)stiDropDown).BackColor = (DropDownBackColor.HasValue ? DropDownBackColor.Value : ((Control)this).BackColor);
			DropDownPopup = stiDropDown;
			((ToolStripDropDown)DropDownPopup).Closed += new ToolStripDropDownClosedEventHandler(DropDownPopupClosed);
		}
		((Control)DropDownPopup).Width = ((Control)this).Width;
		((Control)DropDownPopup).Height = val;
		((Control)TreeView).Location = new Point(0, 0);
		((Control)TreeView).Size = new Size(((Control)this).Width - StiScale.I2, val - StiScale.I4);
		TreeView.Colors = Colors;
		((Control)TreeView).Font = ((Control)this).Font;
		ToolStripControlHost val2 = new ToolStripControlHost((Control)(object)TreeView);
		((ToolStripItem)val2).AutoSize = false;
		((ToolStrip)DropDownPopup).Items.Clear();
		((ToolStrip)DropDownPopup).Items.Add((ToolStripItem)(object)val2);
		if (DropDownBackColor.HasValue)
		{
			Color backColor = (((ToolStrip)DropDownPopup).BackColor = DropDownBackColor.Value);
			((ToolStripItem)val2).BackColor = backColor;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Point point = ((Control)this).PointToScreen(new Point(clientRectangle.Left, clientRectangle.Top + clientRectangle.Height));
		Rectangle workingArea = Screen.FromControl((Control)(object)this).WorkingArea;
		if (point.X + ((Control)this).Size.Width > workingArea.Left + workingArea.Width)
		{
			point.X = workingArea.Left + workingArea.Width - ((Control)this).Size.Width;
		}
		if (point.Y + ((Control)this).Size.Height > workingArea.Top + workingArea.Height)
		{
			point.Y -= ((Control)this).Size.Height + clientRectangle.Height;
		}
		point = ((Control)this).PointToClient(point);
		((ToolStripDropDown)DropDownPopup).Show((Control)(object)this, point);
	}

	private void DropDownPopupClosed(object sender, ToolStripDropDownClosedEventArgs e)
	{
		dropDownClosedTime = DateTime.Now;
	}

	protected override void Dispose(bool disposing)
	{
		((Control)this).Dispose(disposing);
		if (TreeView != null && !((Control)TreeView).IsDisposed)
		{
			((Component)(object)TreeView).Dispose();
		}
	}

	public StiTreeViewBox()
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		Colors = new StiControlColorCollection((Control)(object)this);
		((Control)this).Height = (int)(24.0 * StiScale.Factor);
		dropDownClosedTime = DateTime.Now;
		((TreeView)TreeView).AfterCheck += (TreeViewEventHandler)delegate
		{
			((Control)this).Invalidate();
		};
		((Control)TreeView).MouseDoubleClick += (MouseEventHandler)delegate
		{
			if (AllowCloseDropDownOnDoubleClick)
			{
				CloseDropDown();
			}
		};
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

internal class StiTreeViewBoxPopupForm : StiPopupForm
{
	private Container components;

	internal int SelectedIndex { get; set; } = -1;


	internal StiTreeView TreeNodes { get; set; }

	private void InitializeComponent()
	{
		TreeNodes = new StiTreeView();
		((Control)this).SuspendLayout();
		((TreeView)TreeNodes).BorderStyle = (BorderStyle)0;
		((Control)TreeNodes).Dock = (DockStyle)5;
		((TreeView)TreeNodes).HideSelection = false;
		((TreeView)TreeNodes).ImageIndex = -1;
		((Control)TreeNodes).Location = new Point(2, 2);
		((Control)TreeNodes).Name = "tvNodes";
		((TreeView)TreeNodes).SelectedImageIndex = -1;
		((Control)TreeNodes).Size = new Size(296, 296);
		((Control)TreeNodes).TabIndex = 0;
		((Control)TreeNodes).DoubleClick += OnTvNodesDoubleClick;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Control)this).BackColor = SystemColors.Window;
		((Form)this).ClientSize = new Size(300, 300);
		((Control)this).Controls.AddRange((Control[])(object)new Control[1] { (Control)TreeNodes });
		((Control)this).Name = "StiTreeViewBoxPopupForm";
		((Control)this).Text = "StiComboBoxPopupForm";
		((Control)this).ResumeLayout(false);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Form)this).OnPaint(e);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Color color = (StiUX.IsDark ? StiUX.InputBackground : SystemColors.Window);
		Color color2 = (StiUX.IsDark ? StiUX.ButtonBorder : SystemColors.ControlDark);
		StiDrawing.FillRectangle(e.Graphics, color, clientRectangle.X, clientRectangle.Y, clientRectangle.Width, clientRectangle.Height);
		StiDrawing.DrawRectangle(e.Graphics, color2, clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
	}

	private void OnTvNodesDoubleClick(object sender, EventArgs e)
	{
		StiTreeViewBox stiTreeViewBox = base.ParentControl as StiTreeViewBox;
		TreeNode selectedNode = ((TreeView)TreeNodes).SelectedNode;
		if (selectedNode != null)
		{
			if (selectedNode.Tag != null || !stiTreeViewBox.SelectOnlyTagNotNull)
			{
				stiTreeViewBox.SelectedItem = selectedNode.Tag;
				StiValueChangedEventArgs stiValueChangedEventArgs = new StiValueChangedEventArgs(TreeNodes, selectedNode.Text);
				stiTreeViewBox.InvokeValueChanged(stiValueChangedEventArgs);
				((Control)stiTreeViewBox).Text = stiValueChangedEventArgs.Text;
			}
		}
		else if (!stiTreeViewBox.SelectOnlyTagNotNull)
		{
			StiValueChangedEventArgs stiValueChangedEventArgs2 = new StiValueChangedEventArgs(TreeNodes, string.Empty);
			stiTreeViewBox.InvokeValueChanged(stiValueChangedEventArgs2);
			stiTreeViewBox.SelectedItem = null;
			((Control)stiTreeViewBox).Text = stiValueChangedEventArgs2.Text;
		}
		ClosePopupForm();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((TreeView)TreeNodes).ImageList = null;
			if (components != null)
			{
				components.Dispose();
			}
		}
		base.Dispose(disposing);
	}

	public override void SetLocation(int x, int y, int width, int height)
	{
		base.SetLocation(x, y, width, height);
		DockPaddingEdges dockPadding = ((ScrollableControl)this).DockPadding;
		dockPadding.Left += 2;
		DockPaddingEdges dockPadding2 = ((ScrollableControl)this).DockPadding;
		dockPadding2.Top += 2;
		DockPaddingEdges dockPadding3 = ((ScrollableControl)this).DockPadding;
		dockPadding3.Right += 2;
		DockPaddingEdges dockPadding4 = ((ScrollableControl)this).DockPadding;
		dockPadding4.Bottom += 2;
	}

	public StiTreeViewBoxPopupForm()
		: this(null)
	{
	}

	public StiTreeViewBoxPopupForm(Control parentControl)
		: base(parentControl)
	{
		InitializeComponent();
	}
}

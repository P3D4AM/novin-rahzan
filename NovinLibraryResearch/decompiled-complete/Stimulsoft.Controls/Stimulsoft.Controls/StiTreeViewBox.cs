using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiTreeViewBox), "Toolbox.StiTreeViewBox.bmp")]
public class StiTreeViewBox : StiButtonEditBase, IStiPopupParentControl
{
	public delegate void ValueChangedEventHandler(object sender, StiValueChangedEventArgs e);

	public delegate void TreeNodesFillEventHandler(object sender, StiTreeNodesFillEventArgs e);

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool FixedHeight { get; set; } = true;


	internal static Bitmap DropdownBitmap { get; set; }

	internal StiTreeViewBoxPopupForm TreeViewBoxPopupForm { get; set; }

	[DefaultValue(true)]
	public override bool ReadOnly
	{
		get
		{
			return !((Control)base.TextBox).Visible;
		}
		set
		{
			((Control)base.TextBox).Visible = !value;
			((Control)this).BackColor = SystemColors.Window;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual object SelectedItem { get; set; }

	[Category("Behavior")]
	[DefaultValue(0)]
	public virtual int DropDownWidth { get; set; }

	[Category("Behavior")]
	[DefaultValue(false)]
	public virtual bool SelectOnlyTagNotNull { get; set; }

	[Category("Behavior")]
	[DefaultValue(200)]
	public virtual int MaxDropDownHeight { get; set; } = 200;


	[Category("Behavior")]
	public event EventHandler DropDown;

	[Category("Behavior")]
	public event EventHandler DropUp;

	[Category("Behavior")]
	public event ValueChangedEventHandler ValueChanged;

	[Category("Behavior")]
	public event TreeNodesFillEventHandler TreeNodesFill;

	protected virtual void OnDropDown(EventArgs e)
	{
		this.DropDown?.Invoke(this, e);
	}

	public void InvokeDropDown(EventArgs e)
	{
		OnDropDown(e);
	}

	protected virtual void OnDropUp(EventArgs e)
	{
		this.DropUp?.Invoke(this, e);
	}

	public void InvokeDropUp(EventArgs e)
	{
		OnDropUp(e);
	}

	protected virtual void OnValueChanged(StiValueChangedEventArgs e)
	{
		this.ValueChanged?.Invoke(this, e);
	}

	public void InvokeValueChanged(StiValueChangedEventArgs e)
	{
		OnValueChanged(e);
	}

	protected virtual void OnTreeNodesFill(StiTreeNodesFillEventArgs e)
	{
		this.TreeNodesFill?.Invoke(this, e);
	}

	public void InvokeTreeNodesFill(StiTreeNodesFillEventArgs e)
	{
		OnTreeNodesFill(e);
	}

	public void RefreshTreeNodesFill()
	{
		StiTreeNodesFillEventArgs e = new StiTreeNodesFillEventArgs(((TreeView)TreeViewBoxPopupForm.TreeNodes).Nodes, TreeViewBoxPopupForm.TreeNodes);
		InvokeTreeNodesFill(e);
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		base.OnEnabledChanged(e);
		((Control)base.TextBox).Visible = false;
		((Control)this).Invalidate();
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		base.OnPaint(p);
		if (!((Control)this).Enabled)
		{
			return;
		}
		Graphics graphics = p.Graphics;
		Rectangle contentRect = GetContentRect();
		if (((Control)this).Focused)
		{
			graphics.FillRectangle(SystemBrushes.Highlight, contentRect);
		}
		StringFormat val = new StringFormat();
		try
		{
			val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 0x1000);
			val.Trimming = (StringTrimming)3;
			val.LineAlignment = (StringAlignment)1;
			Color color = ((Control)this).ForeColor;
			if (!((Control)this).Enabled)
			{
				color = SystemColors.ControlDark;
			}
			else if (((Control)this).Focused)
			{
				color = SystemColors.HighlightText;
			}
			SolidBrush val2 = new SolidBrush(color);
			try
			{
				graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val2, (RectangleF)contentRect, val);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		base.OnMouseDown(e);
		if ((e.Button & 0x100000) <= 0)
		{
			return;
		}
		if (!LockPopupInvoke && TreeViewBoxPopupForm == null)
		{
			InvokeDropDown(EventArgs.Empty);
			TreeViewBoxPopupForm = new StiTreeViewBoxPopupForm((Control)(object)this);
			TreeViewBoxPopupForm.ClosePopup += TreeViewBox_ClosePopup;
			Rectangle rectangle = ((Control)this).RectangleToScreen(((Control)this).ClientRectangle);
			int num = StiScaleUI.XXI(DropDownWidth);
			if (num < ((Control)this).Width)
			{
				num = ((Control)this).Width;
			}
			TreeViewBoxPopupForm.SetLocation(rectangle.Left, rectangle.Top + ((Control)this).Height + StiScaleUI.I1, num, StiScaleUI.YYI(MaxDropDownHeight));
			StiTreeNodesFillEventArgs e2 = new StiTreeNodesFillEventArgs(((TreeView)TreeViewBoxPopupForm.TreeNodes).Nodes, TreeViewBoxPopupForm.TreeNodes);
			InvokeTreeNodesFill(e2);
			TreeViewBoxPopupForm.ShowPopupForm();
			InvokeDropDown(EventArgs.Empty);
		}
		else
		{
			LockPopupInvoke = false;
		}
	}

	private void TreeViewBox_ClosePopup(object sender, EventArgs e)
	{
		if (TreeViewBoxPopupForm != null)
		{
			InvokeDropUp(e);
			((Component)(object)TreeViewBoxPopupForm).Dispose();
			TreeViewBoxPopupForm = null;
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		base.OnTextChanged(e);
		((Control)this).Invalidate();
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (FixedHeight)
		{
			height = StiScaleUI.I(21);
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	static StiTreeViewBox()
	{
		DropdownBitmap = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.ComboDown.bmp");
	}

	public StiTreeViewBox()
	{
		ReadOnly = true;
	}
}

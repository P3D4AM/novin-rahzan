using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[Designer(typeof(StiDockingControlDesigner))]
[ToolboxItem(false)]
public class StiDockingControl : Panel
{
	private string guid;

	private bool isSelected;

	private Image image;

	[Category("Behavior")]
	[Browsable(false)]
	public string Guid
	{
		get
		{
			return guid;
		}
		set
		{
			guid = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsSelected
	{
		get
		{
			return isSelected;
		}
		set
		{
			isSelected = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	public Image Image
	{
		get
		{
			return image;
		}
		set
		{
			if (value is Bitmap)
			{
				StiImageUtils.MakeImageBackgroundAlphaZero((Bitmap)(object)((value is Bitmap) ? value : null));
			}
			image = value;
			((Control)this).Invalidate();
			if (((Control)this).Parent != null)
			{
				((Control)this).Parent.Invalidate();
			}
		}
	}

	[Browsable(false)]
	public bool IsVisible
	{
		get
		{
			if (((Control)this).Parent != null)
			{
				return ((StiDockingPanel)(object)((Control)this).Parent).Manager.ClosedControls.IndexOf(this) == -1;
			}
			return false;
		}
	}

	[Browsable(true)]
	public override string Text
	{
		get
		{
			return ((Panel)this).Text;
		}
		set
		{
			((Panel)this).Text = value;
			if (((Control)this).Parent == null)
			{
				return;
			}
			StiDockingPanel stiDockingPanel = (StiDockingPanel)(object)((Control)this).Parent;
			if (stiDockingPanel.SelectedControl == this)
			{
				stiDockingPanel.InvalidateTitle();
				stiDockingPanel.InvalidateTabs();
				if (stiDockingPanel.FloatingForm != null)
				{
					((Control)stiDockingPanel.FloatingForm).Text = value;
				}
			}
		}
	}

	[Category("Behavior")]
	public event CancelEventHandler Closing;

	[Category("Behavior")]
	public event EventHandler Closed;

	protected virtual void OnClosing(CancelEventArgs e)
	{
		if (this.Closing != null)
		{
			this.Closing(this, e);
		}
	}

	public void InvokeClosing(CancelEventArgs e)
	{
		OnClosing(e);
	}

	protected virtual void OnClosed(EventArgs e)
	{
		if (this.Closed != null)
		{
			this.Closed(this, e);
		}
	}

	public void InvokeClosed(EventArgs e)
	{
		OnClosed(e);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		((Control)(StiDockingPanel)(object)((Control)this).Parent).Invalidate();
	}

	public StiDockingControl()
	{
		((ScrollableControl)this).DockPadding.All = 2;
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).Text = "StiDockingControl";
		guid = System.Guid.NewGuid().ToString();
	}
}

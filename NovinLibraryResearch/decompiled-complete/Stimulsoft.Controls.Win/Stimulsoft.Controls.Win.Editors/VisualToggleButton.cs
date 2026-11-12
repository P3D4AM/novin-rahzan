using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualToggleButton : VisualItem
{
	private bool _Checked;

	private bool _IsMouseDown;

	private bool _IsMouseOver;

	[DefaultValue(false)]
	public virtual bool Checked
	{
		get
		{
			return _Checked;
		}
		set
		{
			if (_Checked != value)
			{
				_Checked = value;
				InvalidateRender();
				OnCheckedChanged(new EventArgs());
			}
		}
	}

	[Browsable(false)]
	public bool IsMouseDown
	{
		get
		{
			return _IsMouseDown;
		}
		internal set
		{
			if (value != _IsMouseDown)
			{
				_IsMouseDown = value;
				InvalidateRender();
			}
		}
	}

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			return _IsMouseOver;
		}
		internal set
		{
			if (value != _IsMouseOver)
			{
				_IsMouseOver = value;
				InvalidateRender();
			}
		}
	}

	public event EventHandler CheckedChanged;

	public VisualToggleButton()
	{
		base.Focusable = true;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && GetIsEnabled())
		{
			ToggleChecked();
			IsMouseDown = true;
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			IsMouseDown = false;
		}
		base.OnMouseUp(e);
	}

	protected override void OnMouseEnter()
	{
		IsMouseOver = true;
		base.OnMouseEnter();
	}

	protected override void OnMouseLeave()
	{
		IsMouseOver = false;
		base.OnMouseLeave();
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && GetIsEnabled())
		{
			ToggleChecked();
			e.Handled = true;
		}
		base.OnKeyDown(e);
	}

	private void ToggleChecked()
	{
		Checked = !Checked;
	}

	protected virtual void OnCheckedChanged(EventArgs e)
	{
		if (this.CheckedChanged != null)
		{
			this.CheckedChanged(this, e);
		}
	}
}

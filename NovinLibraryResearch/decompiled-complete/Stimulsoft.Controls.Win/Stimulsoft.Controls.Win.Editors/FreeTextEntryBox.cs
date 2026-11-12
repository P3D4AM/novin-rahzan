using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.Editors;

[ToolboxItem(false)]
internal class FreeTextEntryBox : TextBox
{
	private bool _HideOnLostFocus;

	public event EventHandler RevertValue;

	public event EventHandler ApplyValue;

	public event EventHandler IncreaseValue;

	public event EventHandler DecreaseValue;

	protected virtual void OnDecreaseValue(EventArgs e)
	{
		this.DecreaseValue?.Invoke(this, e);
	}

	protected virtual void OnIncreaseValue(EventArgs e)
	{
		this.IncreaseValue?.Invoke(this, e);
	}

	private void OnApplyValue(EventArgs e)
	{
		if (this.ApplyValue != null)
		{
			this.ApplyValue(this, e);
		}
	}

	private void OnRevertValue(EventArgs e)
	{
		if (this.RevertValue != null)
		{
			this.RevertValue(this, e);
		}
	}

	public void HideOnLostFocus()
	{
		if (((Control)this).Focused)
		{
			_HideOnLostFocus = true;
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		if (_HideOnLostFocus)
		{
			((Control)this).Visible = false;
			_HideOnLostFocus = false;
		}
		((Control)this).OnLostFocus(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			OnApplyValue((EventArgs)(object)e);
		}
		else if ((int)e.KeyCode == 27)
		{
			OnRevertValue((EventArgs)(object)e);
		}
		else if ((int)e.KeyCode == 38)
		{
			OnIncreaseValue((EventArgs)(object)e);
		}
		else if ((int)e.KeyCode == 40)
		{
			OnDecreaseValue((EventArgs)(object)e);
		}
		((Control)this).OnKeyDown(e);
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (!((TextBoxBase)this).ReadOnly && (!(((Control)this).Parent is VisualControlBase) || ((VisualControlBase)(object)((Control)this).Parent).InputMouseWheelEnabled))
		{
			if (e.Delta > 0)
			{
				OnIncreaseValue(EventArgs.Empty);
			}
			else
			{
				OnDecreaseValue(EventArgs.Empty);
			}
		}
		((Control)this).OnMouseWheel(e);
	}

	protected override void OnLeave(EventArgs e)
	{
		OnApplyValue(e);
		((Control)this).OnLeave(e);
	}
}

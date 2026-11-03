using System;
using System.Windows.Forms;

namespace DevComponents.Editors;

internal enum _1885
{
	ሬ,
	_1886
}
internal class _232C : TextBox
{
	private bool __2331;

	public event EventHandler _232D;

	public event EventHandler _232E;

	public event EventHandler _232F;

	public event EventHandler _2330;

	protected virtual void OnDecreaseValue(EventArgs e)
	{
		this._2330?.Invoke(this, e);
	}

	protected virtual void OnIncreaseValue(EventArgs e)
	{
		this._232F?.Invoke(this, e);
	}

	private void _2332(EventArgs ؿ)
	{
		if (this._232E != null)
		{
			this._232E(this, ؿ);
		}
	}

	private void _2333(EventArgs ؿ)
	{
		if (this._232D != null)
		{
			this._232D(this, ؿ);
		}
	}

	public void _2334()
	{
		if (((Control)this).Focused)
		{
			__2331 = true;
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		if (__2331)
		{
			((Control)this).Visible = false;
			__2331 = false;
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
			_2332((EventArgs)(object)e);
		}
		else if ((int)e.KeyCode == 27)
		{
			_2333((EventArgs)(object)e);
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
		_2332(e);
		((Control)this).OnLeave(e);
	}
}

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
internal class RichTextBoxScrollEx : RichTextBox, IScrollBarOverrideSupport
{
	public event EventHandler NonClientSizeChanged;

	public event ScrollValueChangedHandler ScrollBarValueChanged;

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 277 || ((Message)(ref m)).Msg == 276 || ((Message)(ref m)).Msg == 522)
		{
			((RichTextBox)this).WndProc(ref m);
			OnScrollBarValueChanged(new ScrollValueChangedEventArgs(ScrollbarControl.MapMessageToScrollChange(((Message)(ref m)).Msg)));
		}
		else if (((Message)(ref m)).Msg == 131)
		{
			((RichTextBox)this).WndProc(ref m);
			OnNonClientSizeChanged(EventArgs.Empty);
		}
		else if (((Message)(ref m)).Msg == 206 || ((Message)(ref m)).Msg == 8270)
		{
			((RichTextBox)this).WndProc(ref m);
			OnScrollBarValueChanged(new ScrollValueChangedEventArgs(eScrollBarScrollChange.Vertical | eScrollBarScrollChange.Horizontal));
		}
		else
		{
			((RichTextBox)this).WndProc(ref m);
		}
	}

	protected virtual void OnNonClientSizeChanged(EventArgs e)
	{
		this.NonClientSizeChanged?.Invoke(this, e);
	}

	protected virtual void OnScrollBarValueChanged(ScrollValueChangedEventArgs e)
	{
		this.ScrollBarValueChanged?.Invoke(this, e);
	}
}

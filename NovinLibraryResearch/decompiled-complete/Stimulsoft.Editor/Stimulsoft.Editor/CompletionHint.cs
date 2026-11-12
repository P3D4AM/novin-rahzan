using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class CompletionHint : Control
{
	private ICodeCompletionProvider provider;

	private IPainter painter;

	private HintSyntaxPaint syntaxPaint;

	private Size hintSize;

	private EventHandler updateSize;

	private int selectedIndex = 1;

	protected internal ISyntaxPaint SyntaxPaint => syntaxPaint;

	public ICodeCompletionProvider Provider
	{
		get
		{
			return provider;
		}
		set
		{
			if (provider != value)
			{
				provider = value;
				ProviderChanged();
			}
		}
	}

	public int SelectedIndex
	{
		get
		{
			return selectedIndex;
		}
		set
		{
			if (selectedIndex != value)
			{
				selectedIndex = value;
				StringChanged();
			}
		}
	}

	public Size HintSize => hintSize;

	public EventHandler UpdateSize
	{
		get
		{
			return updateSize;
		}
		set
		{
			updateSize = value;
		}
	}

	public CompletionHint()
	{
		painter = new GdiPainter();
		painter.Font = ((Control)this).Font;
		syntaxPaint = new HintSyntaxPaint(painter, (Control)(object)this);
		hintSize = ((Control)this).Size;
		((Control)this).SetStyle((ControlStyles)12550, true);
	}

	protected internal void UpdateControlSize()
	{
		hintSize = syntaxPaint.UpdateSize();
		if (UpdateSize != null)
		{
			UpdateSize(this, EventArgs.Empty);
		}
		((Control)this).Invalidate();
	}

	protected override bool IsInputKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Keys value = (Keys)(keyData & 0xFFFF);
		if (Array.IndexOf(EditConsts.NavKeys, value) >= 0)
		{
			return true;
		}
		return ((Control)this).IsInputKey(keyData);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		painter.Clear();
		painter.Font = ((Control)this).Font;
		UpdateControlSize();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((Control)this).OnMouseDown(e);
		if ((int)e.Button == 1048576 && NeedArrows())
		{
			ChangeSelection(!syntaxPaint.GetLeftArrowRect().Contains(e.X, e.Y));
		}
	}

	protected override void OnPaint(PaintEventArgs pe)
	{
		painter.BeginPaint(pe.Graphics);
		try
		{
			syntaxPaint.PaintSyntax(painter, 0, syntaxPaint.Strings.Count - 1, new Point(0, 0), ((Control)this).ClientRectangle, specialPaint: false);
		}
		finally
		{
			painter.EndPaint();
		}
	}

	protected void StringChanged()
	{
		syntaxPaint.ProviderChanged(provider, selectedIndex);
		UpdateHint();
		UpdateControlSize();
	}

	protected virtual void ProviderChanged()
	{
		selectedIndex = 0;
		StringChanged();
	}

	protected internal bool NeedArrows()
	{
		return syntaxPaint.NeedArrows();
	}

	protected internal bool UpdateHint()
	{
		if (provider != null && selectedIndex < provider.Count && syntaxPaint.UpdateHint(provider, selectedIndex))
		{
			UpdateControlSize();
			return true;
		}
		return false;
	}

	public void ResetContent()
	{
		ProviderChanged();
	}

	public void ChangeSelection(bool inc)
	{
		if (provider == null)
		{
			return;
		}
		int num = selectedIndex;
		if (inc)
		{
			num++;
			if (num >= provider.Count)
			{
				num = 0;
			}
		}
		else
		{
			num--;
			if (num < 0)
			{
				num = Math.Max(provider.Count - 1, 0);
			}
		}
		SelectedIndex = num;
	}
}

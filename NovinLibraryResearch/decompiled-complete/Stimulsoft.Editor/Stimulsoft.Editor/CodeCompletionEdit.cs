using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class CodeCompletionEdit : CodeCompletionWindow, ICodeCompletionEdit, ICodeCompletionWindow, IControl
{
	private CodeCompletionWindow parent;

	protected internal CompletionEdit Edit => (CompletionEdit)(object)base.PopupControl;

	public string EditField
	{
		get
		{
			return ((Control)Edit.Label).Text;
		}
		set
		{
			((Control)Edit.Label).Text = value;
		}
	}

	public string EditText
	{
		get
		{
			return ((Control)Edit.EditBox).Text;
		}
		set
		{
			((Control)Edit.EditBox).Text = value;
		}
	}

	public string EditPath
	{
		get
		{
			return ((Control)Edit.PathLabel).Text;
		}
		set
		{
			((Control)Edit.PathLabel).Text = value;
			Edit.UpdateSize();
		}
	}

	public CodeCompletionEdit(SyntaxEdit owner, CodeCompletionWindow parent)
		: base(owner)
	{
		this.parent = parent;
		((Form)this).FormBorderStyle = (FormBorderStyle)5;
		((Control)this).Width = 300;
		((Control)this).Height = ((Control)this).Font.Height + 2;
		if (parent != null)
		{
			base.CompletionFlags = parent.CompletionFlags;
		}
		base.CompletionFlags &= ~(CodeCompletionFlags.CloseOnEscape | CodeCompletionFlags.AcceptOnEnter | CodeCompletionFlags.AcceptOnDblClick | CodeCompletionFlags.AcceptOnDelimiter);
		base.CompletionFlags |= CodeCompletionFlags.KeepActive;
		((Control)Edit.EditBox).TextChanged += DoTextChanged;
	}

	protected override void Dispose(bool disposing)
	{
		if (parent != null)
		{
			parent.CodeEditDisposed();
		}
		((Form)this).Dispose(disposing);
	}

	protected internal override bool ContainsControl(Control control)
	{
		if (!base.ContainsControl(control))
		{
			if (control != null)
			{
				if (control != parent)
				{
					return control == parent.PopupControl;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	protected override Control CreatePopupControl()
	{
		return (Control)(object)new CompletionEdit();
	}

	protected override void WndProc(ref Message m)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		int msg = ((Message)(ref m)).Msg;
		if (msg == 133)
		{
			base.WndProc(ref m);
			if ((int)((Form)this).FormBorderStyle == 5 && XPThemes.CurrentTheme != 0)
			{
				IntPtr windowDC = OSUtils.GetWindowDC(((Control)this).Handle);
				try
				{
					OSUtils.ExcludeClipRect(windowDC, 2, 2, ((Control)this).Width - 2, ((Control)this).Height - 2);
					XPThemes.DrawEditBorder(windowDC, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
				}
				finally
				{
					OSUtils.ReleaseDC(((Control)this).Handle, windowDC);
				}
			}
		}
		else
		{
			base.WndProc(ref m);
		}
	}

	protected virtual void DoTextChanged(object sender, EventArgs e)
	{
		if (parent != null)
		{
			parent.PerformSearch();
		}
	}

	protected override void DoProcessKeyMessage(ref Message m)
	{
	}

	protected override bool ProcessKeyPreview(ref Message m)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Invalid comparison between Unknown and I4
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		if (OnProcessKeyPreview(ref m))
		{
			return true;
		}
		Keys val = (Keys)0;
		if (((Message)(ref m)).Msg == 256 || ((Message)(ref m)).Msg == 260)
		{
			val = (Keys)(((Message)(ref m)).WParam.ToInt32() & 0xFFFF);
			if ((int)val <= 27)
			{
				if ((int)val == 13 || (int)val == 27)
				{
					goto IL_005e;
				}
			}
			else if (val - 33 <= 1 || (int)val == 38 || (int)val == 40)
			{
				goto IL_005e;
			}
		}
		goto IL_00a2;
		IL_005e:
		if (parent != null && parent.PopupControl.IsHandleCreated)
		{
			OSUtils.SendMessage(parent.PopupControl.Handle, ((Message)(ref m)).Msg, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
			return true;
		}
		goto IL_00a2;
		IL_00a2:
		return base.ProcessKeyPreview(ref m);
	}

	public override bool IsFocused()
	{
		if (!((Control)this).Focused && !((Control)Edit).Focused && !((Control)Edit.EditBox).Focused)
		{
			return ((Control)Edit.PathLabel).Focused;
		}
		return true;
	}

	void IControl.Invalidate()
	{
		((Control)this).Invalidate();
	}

	void IControl.Invalidate(Rectangle rect)
	{
		((Control)this).Invalidate(rect);
	}

	void IControl.Update()
	{
		((Control)this).Update();
	}

	Point IControl.PointToClient(Point p)
	{
		return ((Control)this).PointToClient(p);
	}

	Point IControl.PointToScreen(Point p)
	{
		return ((Control)this).PointToScreen(p);
	}

	bool IControl.Focus()
	{
		return ((Control)this).Focus();
	}

	bool IControl.get_CanFocus()
	{
		return ((Control)this).CanFocus;
	}

	bool IControl.get_Visible()
	{
		return ((Control)this).Visible;
	}

	void IControl.set_Visible(bool value)
	{
		((Control)this).Visible = value;
	}

	Point IControl.get_Location()
	{
		return ((Form)this).Location;
	}

	void IControl.set_Location(Point value)
	{
		((Form)this).Location = value;
	}

	int IControl.get_Left()
	{
		return ((Control)this).Left;
	}

	void IControl.set_Left(int value)
	{
		((Control)this).Left = value;
	}

	int IControl.get_Top()
	{
		return ((Control)this).Top;
	}

	void IControl.set_Top(int value)
	{
		((Control)this).Top = value;
	}

	int IControl.get_Width()
	{
		return ((Control)this).Width;
	}

	void IControl.set_Width(int value)
	{
		((Control)this).Width = value;
	}

	int IControl.get_Height()
	{
		return ((Control)this).Height;
	}

	void IControl.set_Height(int value)
	{
		((Control)this).Height = value;
	}

	void IControl.set_Bounds(Rectangle value)
	{
		((Control)this).Bounds = value;
	}

	Rectangle IControl.get_ClientRectangle()
	{
		return ((Control)this).ClientRectangle;
	}

	Control IControl.get_Parent()
	{
		return ((Control)this).Parent;
	}

	void IControl.set_Parent(Control value)
	{
		((Control)this).Parent = value;
	}

	bool IControl.get_Enabled()
	{
		return ((Control)this).Enabled;
	}

	bool IControl.get_IsHandleCreated()
	{
		return ((Control)this).IsHandleCreated;
	}

	void IControl.add_Click(EventHandler value)
	{
		((Control)this).Click += value;
	}

	void IControl.remove_Click(EventHandler value)
	{
		((Control)this).Click -= value;
	}
}

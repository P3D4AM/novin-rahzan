using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

public abstract class StiPopupBaseForm : Form, IMessageFilter
{
	private class FormHandler : NativeWindow
	{
		protected const int WM_NCACTIVATE = 134;

		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).Msg == 134 && (int)((Message)(ref m)).WParam == 0)
			{
				((Message)(ref m)).Result = (IntPtr)1;
			}
			else
			{
				((NativeWindow)this).WndProc(ref m);
			}
		}

		public FormHandler(Control control)
		{
			if (control != null && control.TopLevelControl != null)
			{
				((NativeWindow)this).AssignHandle(control.TopLevelControl.Handle);
			}
		}
	}

	private FormHandler activetedHandler;

	protected static bool is64Bit;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((Control)this).ForeColor;
			}
			return StiUX.InputForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((Form)this).BackColor;
			}
			return StiUX.InputBackground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Form)this).BackColor = value;
			}
		}
	}

	protected Control ParentControl { get; set; }

	protected bool IsClosed { get; set; }

	protected virtual bool HookMouseButtonsMessages => true;

	protected virtual bool Layered => true;

	protected virtual bool UseCreateParams => true;

	protected override CreateParams CreateParams
	{
		get
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			if (!UseCreateParams)
			{
				return ((Form)this).CreateParams;
			}
			uint style = 2147483648u;
			int num = 8;
			int num2 = 128;
			int num3 = 524288;
			CreateParams val = new CreateParams
			{
				X = ((Form)this).Location.X,
				Y = ((Form)this).Location.Y,
				Height = ((Form)this).Size.Height,
				Width = ((Form)this).Size.Width,
				Parent = IntPtr.Zero,
				Style = (int)style
			};
			val.ExStyle |= num;
			val.ExStyle |= num2;
			if (Layered)
			{
				val.ExStyle |= num3;
			}
			return val;
		}
	}

	[Category("Behavior")]
	public event EventHandler ClosePopup;

	[Category("Behavior")]
	public event EventHandler ShowPopup;

	public virtual void SetLocation(int x, int y, int width, int height)
	{
		if (Screen.AllScreens.Length == 1)
		{
			if (y + height > Screen.PrimaryScreen.WorkingArea.Bottom)
			{
				y = Screen.PrimaryScreen.WorkingArea.Bottom - height - 1;
			}
			if (x + width > Screen.PrimaryScreen.WorkingArea.Right)
			{
				x = Screen.PrimaryScreen.WorkingArea.Right - width - 1;
			}
		}
		((Control)this).CreateControl();
		((Control)this).SetBoundsCore(x, y, width, height, (BoundsSpecified)15);
	}

	public virtual void ClosePopupForm()
	{
		if (!((Control)this).Disposing && !((Control)this).IsDisposed)
		{
			Win32.ShowWindow(((Control)this).Handle, 0);
		}
		IsClosed = true;
		InvokeClosePopup(EventArgs.Empty);
	}

	public virtual void ShowPopupForm()
	{
		Win32.ShowWindow(((Control)this).Handle, 4);
		InvokeShowPopup(EventArgs.Empty);
	}

	protected virtual void OnClosePopup(EventArgs e)
	{
		this.ClosePopup?.Invoke(this, e);
	}

	protected virtual void OnShowPopup(EventArgs e)
	{
		this.ShowPopup?.Invoke(this, e);
	}

	public void InvokeClosePopup(EventArgs e)
	{
		OnClosePopup(e);
	}

	public void InvokeShowPopup(EventArgs e)
	{
		OnShowPopup(e);
	}

	protected override void WndProc(ref Message m)
	{
		switch (((Message)(ref m)).Msg)
		{
		case 28:
			ClosePopupForm();
			break;
		case 33:
			((Message)(ref m)).Result = (IntPtr)3;
			break;
		default:
			((Form)this).WndProc(ref m);
			break;
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Form)this).OnHandleCreated(e);
		if (!((Component)this).DesignMode && HookMouseButtonsMessages)
		{
			Application.AddMessageFilter((IMessageFilter)(object)this);
		}
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		((Form)this).OnHandleDestroyed(e);
		if (!((Component)this).DesignMode && HookMouseButtonsMessages)
		{
			Application.RemoveMessageFilter((IMessageFilter)(object)this);
		}
	}

	public virtual bool PreFilterMessage(ref Message m)
	{
		try
		{
			if (((Message)(ref m)).Msg == 513 || ((Message)(ref m)).Msg == 519 || ((Message)(ref m)).Msg == 516 || ((Message)(ref m)).Msg == 523 || ((Message)(ref m)).Msg == 161 || ((Message)(ref m)).Msg == 167 || ((Message)(ref m)).Msg == 164 || ((Message)(ref m)).Msg == 171)
			{
				Point position = Cursor.Position;
				if (position.X < ((Control)this).Left || position.X > ((Control)this).Right || position.Y < ((Control)this).Top || position.Y > ((Control)this).Bottom)
				{
					if (ParentControl is IStiPopupParentControl stiPopupParentControl)
					{
						position = ParentControl.PointToClient(position);
						if (position.X >= 0 && position.X <= ParentControl.Width && position.Y >= 0 && position.Y <= ParentControl.Height && ((Control)this).Visible)
						{
							stiPopupParentControl.LockPopupInvoke = true;
						}
					}
					ClosePopupForm();
					Application.DoEvents();
				}
			}
		}
		catch
		{
		}
		return false;
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Form)this).OnVisibleChanged(e);
		if (((Control)this).Visible)
		{
			if (activetedHandler == null)
			{
				activetedHandler = new FormHandler(ParentControl);
			}
		}
		else if (activetedHandler != null)
		{
			((NativeWindow)activetedHandler).ReleaseHandle();
		}
	}

	internal void UpdateZOrder(IntPtr after)
	{
		int num = 1;
		int num2 = 2;
		int num3 = 16;
		int uFlags = num3 | num | num2;
		Win32.SetWindowPos(((Control)this).Handle, after, ((Control)this).Left, ((Control)this).Top, ((Control)this).Width, ((Control)this).Height, uFlags);
	}

	static StiPopupBaseForm()
	{
		is64Bit = IntPtr.Size == 8;
	}

	public StiPopupBaseForm()
		: this(null)
	{
	}

	public StiPopupBaseForm(Control parentControl)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).SuspendLayout();
		ParentControl = parentControl;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)0;
		((Form)this).TopMost = false;
		((Control)this).ResumeLayout();
		if (parentControl != null)
		{
			Form val = parentControl.FindForm();
			if (val != null)
			{
				val.AddOwnedForm((Form)(object)this);
			}
		}
	}
}

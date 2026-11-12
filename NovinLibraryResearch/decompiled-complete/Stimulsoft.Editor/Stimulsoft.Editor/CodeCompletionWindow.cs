using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class CodeCompletionWindow : Form, ICodeCompletionWindow, IControl
{
	private CodeCompletionFlags completionFlags = EditConsts.DefaultCodeCompletionFlags;

	private ImageList images;

	private bool sizeable;

	private bool autoSize = true;

	private ICodeCompletionProvider provider;

	private Point startPos;

	private Point endPos;

	private Point displayPos;

	private Control ownerControl;

	private Control popupControl;

	private Control prevFocused;

	protected internal bool showing;

	protected override CreateParams CreateParams
	{
		get
		{
			uint num = 2147483648u;
			CreateParams createParams = ((Form)this).CreateParams;
			createParams.Style |= (int)num;
			return createParams;
		}
	}

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
				SetProvider(value);
			}
		}
	}

	public bool AutoSize
	{
		get
		{
			return autoSize;
		}
		set
		{
			if (autoSize != value)
			{
				autoSize = value;
				if (value)
				{
					UpdateAutoSize();
				}
			}
		}
	}

	public ImageList Images
	{
		get
		{
			return images;
		}
		set
		{
			images = value;
		}
	}

	public CodeCompletionFlags CompletionFlags
	{
		get
		{
			return completionFlags;
		}
		set
		{
			completionFlags = value;
		}
	}

	public bool Sizeable
	{
		get
		{
			return sizeable;
		}
		set
		{
			if (sizeable != value)
			{
				sizeable = value;
				if (value)
				{
					((Form)this).FormBorderStyle = (FormBorderStyle)4;
				}
				else
				{
					((Form)this).FormBorderStyle = (FormBorderStyle)5;
				}
			}
		}
	}

	public Control OwnerControl
	{
		get
		{
			return ownerControl;
		}
		set
		{
			ownerControl = value;
		}
	}

	public Control PopupControl => popupControl;

	public Point StartPos
	{
		get
		{
			return startPos;
		}
		set
		{
			startPos = value;
		}
	}

	public Point EndPos
	{
		get
		{
			return endPos;
		}
		set
		{
			endPos = value;
		}
	}

	public Point DisplayPos
	{
		get
		{
			return displayPos;
		}
		set
		{
			displayPos = value;
		}
	}

	public event ClosePopupEvent ClosePopup;

	public event ShowPopupEvent ShowPopup;

	public event KeyPreviewEvent KeyPreviewEvent;

	public CodeCompletionWindow(SyntaxEdit owner)
	{
		ownerControl = (Control)(object)owner;
		startPos = new Point(0, 0);
		endPos = new Point(0, 0);
		displayPos = new Point(0, 0);
		((Control)this).MinimumSize = new Size(((Control)this).Font.Height + 2, ((Control)this).Font.Height + 2);
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)0;
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Control)this).Width = 300;
		((Control)this).Height = 600;
		((Form)this).ShowInTaskbar = false;
		((Form)this).ControlBox = false;
		((Form)this).Owner = GetParentForm((Control)(object)owner);
		((Control)this).BackColor = Consts.DefaultControlBackColor;
		((Control)this).SetStyle((ControlStyles)16, true);
		((Form)this).TopLevel = true;
		((Control)this).DoubleClick += DoDoubleClick;
		((Control)this).Click += DoClick;
		((Control)this).LostFocus += DoLostFocus;
		popupControl = CreatePopupControl();
		popupControl.DoubleClick += DoDoubleClick;
		popupControl.Click += DoClick;
		popupControl.LostFocus += DoLostFocus;
		((Control)this).Controls.Clear();
		((Control)this).Controls.Add(popupControl);
		popupControl.Dock = (DockStyle)5;
		UpdateAutoSize();
	}

	~CodeCompletionWindow()
	{
		try
		{
			popupControl.DoubleClick -= DoDoubleClick;
			popupControl.Click -= DoClick;
			popupControl.LostFocus -= DoLostFocus;
			((Form)this).Owner = null;
		}
		finally
		{
			((Component)this).Finalize();
		}
	}

	private Form GetParentForm(Control c)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		while (c != null)
		{
			if (c is Form)
			{
				return (Form)c;
			}
			c = c.Parent;
		}
		return null;
	}

	private bool IsPositionValid()
	{
		bool flag = true;
		SyntaxEdit syntaxEdit = GetSyntaxEdit();
		if (syntaxEdit != null)
		{
			Point position = syntaxEdit.Position;
			if (startPos.Y >= 0)
			{
				flag = position.Y > startPos.Y || (position.Y == startPos.Y && position.X >= startPos.X);
			}
			if (flag && endPos.Y >= 0)
			{
				flag = position.Y < endPos.Y || (position.Y == endPos.Y && position.X <= endPos.X);
			}
		}
		return flag;
	}

	protected internal void EnsureVisible(ref Point position)
	{
		Rectangle workingArea = Screen.GetWorkingArea(position);
		Rectangle rect = new Rectangle(position, ((Control)this).Bounds.Size);
		if (!workingArea.Contains(rect))
		{
			if (rect.X < workingArea.Left)
			{
				position.X = workingArea.Left;
			}
			else if (rect.Right > workingArea.Right)
			{
				position.X = workingArea.Right - rect.Width;
			}
			if (rect.Y < workingArea.Top)
			{
				position.Y = workingArea.Top;
			}
			else if (rect.Bottom > workingArea.Bottom)
			{
				int num = ((ISyntaxEdit)GetSyntaxEdit())?.Painter.FontHeight ?? ((Control)this).FontHeight;
				position.Y = Math.Min(position.Y - rect.Height - num, workingArea.Bottom - rect.Height);
			}
		}
	}

	protected internal virtual void CodeEditDisposed()
	{
	}

	protected internal virtual bool ContainsControl(Control control)
	{
		if (control != null)
		{
			return ((Control)this).Contains(control);
		}
		return false;
	}

	protected internal virtual void PositionChanged(int x, int y, int deltaX, int deltaY)
	{
		if (((Control)this).Visible)
		{
			SortList.UpdatePos(x, y, deltaX, deltaY, ref endPos, endPos: true);
		}
	}

	protected SyntaxEdit GetSyntaxEdit()
	{
		if (!(ownerControl is SyntaxEdit))
		{
			return null;
		}
		return (SyntaxEdit)(object)ownerControl;
	}

	protected virtual void UpdateAutoSize()
	{
	}

	protected virtual void SetProvider(ICodeCompletionProvider provider)
	{
	}

	protected virtual bool OnProcessKeyPreview(ref Message m)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (this.KeyPreviewEvent != null)
		{
			KeyPreviewEventArgs keyPreviewEventArgs = new KeyPreviewEventArgs(m);
			this.KeyPreviewEvent(this, keyPreviewEventArgs);
			return keyPreviewEventArgs.Handled;
		}
		return false;
	}

	protected virtual bool IsPopupControlFocused()
	{
		return PopupControl.Focused;
	}

	protected virtual bool NeedCloseOnLostFocus()
	{
		return PopupHookManager.NeedCloseOnLostFocus(this);
	}

	protected virtual void DoProcessKeyMessage(ref Message m)
	{
		GetSyntaxEdit()?.DoProcessKeyMessage(ref m);
	}

	protected void DoLostFocus(object sender, EventArgs e)
	{
		if (!showing && NeedCloseOnLostFocus())
		{
			Close(accept: false, setFocus: false);
		}
	}

	protected void DoClick(object sender, EventArgs e)
	{
		if ((CompletionFlags & CodeCompletionFlags.AcceptOnClick) != 0)
		{
			Close(accept: true);
		}
	}

	protected virtual Control CreatePopupControl()
	{
		return null;
	}

	protected virtual int GetSelectedIndex()
	{
		return -1;
	}

	protected void DoDoubleClick(object sender, EventArgs e)
	{
		if ((CompletionFlags & CodeCompletionFlags.AcceptOnDblClick) != 0)
		{
			Close(accept: true);
		}
	}

	protected override bool ProcessKeyPreview(ref Message m)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Invalid comparison between Unknown and I4
		Keys val = (Keys)0;
		if (((Message)(ref m)).Msg == 256 || ((Message)(ref m)).Msg == 260)
		{
			val = (Keys)(((Message)(ref m)).WParam.ToInt32() & 0xFFFF);
			if ((int)val <= 13)
			{
				if ((int)val != 9)
				{
					if ((int)val == 13 && (CompletionFlags & CodeCompletionFlags.AcceptOnEnter) != 0)
					{
						Close(accept: true);
						return true;
					}
				}
				else if ((CompletionFlags & CodeCompletionFlags.AcceptOnTab) != 0)
				{
					Close(accept: true);
					return true;
				}
			}
			else if ((int)val != 27)
			{
				if ((int)val == 32 && (CompletionFlags & CodeCompletionFlags.AcceptOnSpace) != 0)
				{
					Close(accept: true);
					return true;
				}
			}
			else if ((CompletionFlags & CodeCompletionFlags.CloseOnEscape) != 0)
			{
				Close(accept: false);
				return true;
			}
		}
		if (((Message)(ref m)).Msg == 258 && ((Message)(ref m)).WParam.ToInt32() == 32 && (CompletionFlags & CodeCompletionFlags.AcceptOnSpace) != 0)
		{
			Close(accept: true);
			return true;
		}
		DoProcessKeyMessage(ref m);
		CheckPositionValid();
		if (((((Message)(ref m)).Msg == 258 && ((Message)(ref m)).WParam.ToInt32() != 27 && ((Message)(ref m)).WParam.ToInt32() != 13) || (int)val == 8) && PerformSearch())
		{
			return true;
		}
		return ((Form)this).ProcessKeyPreview(ref m);
	}

	protected virtual void CheckPositionValid()
	{
		if ((CompletionFlags & CodeCompletionFlags.KeepActive) == 0 && !IsPositionValid())
		{
			Close(accept: false);
		}
	}

	protected virtual void DoShow(Point position)
	{
		prevFocused = null;
		if (ownerControl is SyntaxEdit)
		{
			if ((ownerControl.Focused || ((SyntaxEdit)(object)ownerControl).IsCodeCompletionWindowFocused(out prevFocused)) && prevFocused == null)
			{
				prevFocused = ownerControl;
			}
		}
		else
		{
			prevFocused = ((ownerControl != null && ownerControl.Focused) ? ownerControl : null);
		}
		((Form)this).Location = position;
		OSUtils.ShowWindowTopMost(((Control)this).Handle, position.X, position.Y, ((Control)this).Width, ((Control)this).Height);
		((Control)this).Visible = true;
		if (popupControl.CanFocus)
		{
			popupControl.Focus();
		}
	}

	protected virtual void DoHide()
	{
		((Control)this).Visible = false;
	}

	protected override void WndProc(ref Message m)
	{
		int msg = ((Message)(ref m)).Msg;
		if (msg == 1025)
		{
			Close(((Message)(ref m)).WParam != IntPtr.Zero, setFocus: false);
		}
		((Form)this).WndProc(ref m);
	}

	protected virtual void OnShowPopup(ShowingEventArgs args)
	{
		if (this.ShowPopup != null)
		{
			this.ShowPopup(this, args);
		}
	}

	protected virtual void OnClosePopup(ClosingEventArgs args)
	{
		if (Provider != null)
		{
			((CodeCompletionProvider)Provider).OnClosePopup(this, args);
			if (args.Accepted && args.Handled)
			{
				Provider = args.Provider;
				return;
			}
		}
		if (this.ClosePopup != null)
		{
			this.ClosePopup(this, args);
		}
	}

	protected internal virtual bool PerformSearch()
	{
		return false;
	}

	protected internal virtual void CheckMouseLeave(object sender, EventArgs e)
	{
		if (((Control)this).Visible && (CompletionFlags & CodeCompletionFlags.CloseOnMouseLeave) != 0 && (ownerControl == null || !ownerControl.Bounds.Contains(Control.MousePosition) || (sender == ownerControl && !((Control)this).Bounds.Contains(Control.MousePosition))))
		{
			CloseDelayed(accept: false);
		}
	}

	public virtual void ResetContent()
	{
	}

	public virtual void CloseDelayed(bool accept)
	{
		if (((Control)this).IsHandleCreated)
		{
			OSUtils.PostMessage(((Control)this).Handle, 1025, accept ? new IntPtr(1) : IntPtr.Zero, IntPtr.Zero);
		}
		else
		{
			Close(accept);
		}
	}

	public virtual void Close(bool accept)
	{
		Close(accept, setFocus: true);
	}

	public virtual void Close(bool accept, bool setFocus)
	{
		if (!((Control)this).Visible)
		{
			return;
		}
		if (Provider != null)
		{
			Provider.SelIndex = GetSelectedIndex();
		}
		ClosingEventArgs closingEventArgs = new ClosingEventArgs(accept, Provider);
		closingEventArgs.UseIndent = Provider != null && Provider.UseIndent;
		closingEventArgs.Text = ((Provider != null && Provider.ColumnCount != 0 && Provider.SelIndex >= 0 && Provider.SelIndex < Provider.Count) ? Provider.GetText(Provider.SelIndex) : string.Empty);
		OnClosePopup(closingEventArgs);
		if (!closingEventArgs.Handled)
		{
			PopupHookManager.PopupClosed(this);
			DoHide();
			if (setFocus && prevFocused != null && prevFocused.CanFocus)
			{
				prevFocused.Focus();
			}
		}
	}

	public void Popup()
	{
		PopupAt(Control.MousePosition);
	}

	public virtual void PopupAt(int x, int y)
	{
		PopupAt(new Point(x, y));
	}

	public void PopupAt(Point position)
	{
		if (Provider != null)
		{
			CodeCompletionProvider codeCompletionProvider = Provider as CodeCompletionProvider;
			ShowingEventArgs showingEventArgs = new ShowingEventArgs(Provider);
			codeCompletionProvider.OnShowPopup(this, showingEventArgs);
			OnShowPopup(showingEventArgs);
			if (!showingEventArgs.NeedShow)
			{
				return;
			}
		}
		showing = true;
		try
		{
			if ((completionFlags & CodeCompletionFlags.FeetToScreen) > CodeCompletionFlags.None)
			{
				EnsureVisible(ref position);
			}
			PopupHookManager.PopupClosed(this);
			PopupHookManager.PopupShowing(this);
			DoShow(position);
		}
		finally
		{
			showing = false;
		}
	}

	public virtual bool IsFocused()
	{
		if (!((Control)this).Focused)
		{
			return popupControl.Focused;
		}
		return true;
	}

	public virtual void ResetAutoSize()
	{
		AutoSize = true;
	}

	public virtual void ResetCodeCompletionFlags()
	{
		CompletionFlags = EditConsts.DefaultCodeCompletionFlags;
	}

	public virtual void ResetSizeable()
	{
		Sizeable = false;
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

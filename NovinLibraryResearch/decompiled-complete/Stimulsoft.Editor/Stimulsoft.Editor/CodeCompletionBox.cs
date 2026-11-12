using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class CodeCompletionBox : CodeCompletionWindow, ICodeCompletionBox, ICodeCompletionWindow, IControl
{
	private int dropDownCount = EditConsts.DefaultDropDownCount;

	private CodeCompletionHint codeHint;

	private CodeCompletionEdit codeEdit;

	private Timer codeHintTimer;

	private bool sorted = true;

	protected internal CompletionListBox ListBox => (CompletionListBox)(object)base.PopupControl;

	protected internal Timer CodeHintTimer
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (codeHintTimer == null)
			{
				codeHintTimer = new Timer();
				codeHintTimer.Enabled = false;
				codeHintTimer.Interval = SyntaxConsts.DefaultHintDelay;
				codeHintTimer.Tick += OnShowCodeHint;
			}
			return codeHintTimer;
		}
	}

	protected internal virtual CodeCompletionHint CodeHint
	{
		get
		{
			if (codeHint == null)
			{
				codeHint = new CodeCompletionHint(GetSyntaxEdit());
				((Control)codeHint).Enabled = false;
				codeHint.Provider = new QuickInfo();
				codeHint.CompletionFlags &= ~CodeCompletionFlags.CloseOnMouseLeave;
			}
			return codeHint;
		}
	}

	protected internal virtual CodeCompletionEdit CodeEdit
	{
		get
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			if (codeEdit == null)
			{
				codeEdit = new CodeCompletionEdit(GetSyntaxEdit(), this);
				((Control)codeEdit.Edit.EditBox).KeyPress += new KeyPressEventHandler(CodeEditKeyPress);
				((Control)codeEdit.Edit.EditBox).LostFocus += base.DoLostFocus;
				((Control)codeEdit.Edit.PathLabel).Click += PathLabelClick;
			}
			return codeEdit;
		}
	}

	public int DropDownCount
	{
		get
		{
			return dropDownCount;
		}
		set
		{
			if (dropDownCount != value)
			{
				dropDownCount = value;
				UpdateDropDown();
			}
		}
	}

	public bool Filtered
	{
		get
		{
			return ListBox.Filtered;
		}
		set
		{
			ListBox.Filtered = value;
		}
	}

	public string Filter
	{
		get
		{
			return ListBox.Filter;
		}
		set
		{
			ListBox.Filter = value;
		}
	}

	public bool Sorted
	{
		get
		{
			return sorted;
		}
		set
		{
			sorted = value;
			((ListBox)ListBox).Sorted = value;
		}
	}

	public ICodeCompletionColumn[] Columns
	{
		get
		{
			ICodeCompletionColumn[] array = new ICodeCompletionColumn[ListBox.ColumnCount];
			((CodeCompletionColumns)ListBox.Columns).CopyTo(array);
			return array;
		}
	}

	public event EventHandler SelectionChanged
	{
		add
		{
			((ListBox)ListBox).SelectedIndexChanged += value;
		}
		remove
		{
			((ListBox)ListBox).SelectedIndexChanged -= value;
		}
	}

	public CodeCompletionBox(SyntaxEdit owner)
		: base(owner)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		base.CompletionFlags &= ~CodeCompletionFlags.CloseOnMouseLeave;
		base.Sizeable = true;
		UpdateDropDown();
		CompletionListBox listBox = ListBox;
		listBox.UpdateSize = (EventHandler)Delegate.Combine(listBox.UpdateSize, new EventHandler(DoUpdateSize));
		((Control)ListBox).TabStop = false;
		((Control)this).Font = new Font(EditConsts.DefaultCodeCompletionFont, EditConsts.DefaultCodeCompletionFontSize);
	}

	~CodeCompletionBox()
	{
		if (codeHintTimer != null)
		{
			((Component)(object)codeHintTimer).Dispose();
		}
	}

	private bool IndexMatches(string text, int index)
	{
		if (index >= 0 && index < ((ListBox)ListBox).Items.Count)
		{
			string @string = ((ListboxItem)((ListBox)ListBox).Items[index]).String;
			@string = @string.Substring(0, Math.Min(@string.Length, text.Length));
			return string.Compare(text, @string, ignoreCase: true) == 0;
		}
		return false;
	}

	private int IndexOfString(string text)
	{
		if (IndexMatches(text, ((ListControl)ListBox).SelectedIndex))
		{
			return ((ListControl)ListBox).SelectedIndex;
		}
		for (int i = 0; i < ((ListBox)ListBox).Items.Count; i++)
		{
			if (IndexMatches(text, i))
			{
				return i;
			}
		}
		return -1;
	}

	private void UpdateDropDown()
	{
		if (base.AutoSize)
		{
			((Form)this).ClientSize = new Size(((Control)this).ClientRectangle.Width, Math.Max(Math.Min(dropDownCount, ((ListBox)ListBox).Items.Count) * ((ListBox)ListBox).ItemHeight + EditConsts.DefaultRowSeparator, EditConsts.DefaultMinListBoxHeight));
		}
	}

	protected internal override void CodeEditDisposed()
	{
		if (((Control)this).Visible)
		{
			Close(accept: false);
		}
		codeEdit = null;
	}

	protected internal override bool ContainsControl(Control control)
	{
		if (!base.ContainsControl(control))
		{
			if (codeEdit != null)
			{
				if (control != codeEdit && control != codeEdit.Edit && control != codeEdit.Edit.EditBox)
				{
					return control == codeEdit.Edit.PathLabel;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	protected override void DoProcessKeyMessage(ref Message m)
	{
		if (codeEdit == null || !((Control)codeEdit).Visible)
		{
			base.DoProcessKeyMessage(ref m);
		}
	}

	protected override bool ProcessKeyPreview(ref Message m)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Invalid comparison between Unknown and I4
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Invalid comparison between Unknown and I4
		if (((Message)(ref m)).Msg == 256 || ((Message)(ref m)).Msg == 260)
		{
			Keys val = (Keys)(((Message)(ref m)).WParam.ToInt32() & 0xFFFF);
			if ((((int)val == 9 && (base.CompletionFlags & CodeCompletionFlags.AcceptOnTab) != 0) || ((int)val == 32 && (base.CompletionFlags & CodeCompletionFlags.AcceptOnSpace) != 0)) && ((int)Control.ModifierKeys == 0 || (int)Control.ModifierKeys == 65536))
			{
				Close(accept: true);
				return true;
			}
			if (Array.IndexOf(ListBox.NavKeys, val) >= 0)
			{
				return false;
			}
		}
		if (OnProcessKeyPreview(ref m))
		{
			return true;
		}
		SyntaxEdit syntaxEdit = GetSyntaxEdit();
		if (((Message)(ref m)).Msg == 258 && syntaxEdit != null && ((int)Control.ModifierKeys == 0 || (int)Control.ModifierKeys == 65536))
		{
			char c = (char)((Message)(ref m)).WParam.ToInt32();
			if ((base.CompletionFlags & CodeCompletionFlags.AcceptOnDelimiter) != 0 && c >= ' ' && syntaxEdit.Lines.IsDelimiter(c))
			{
				Close(accept: true);
			}
			if (codeEdit != null && ((Control)codeEdit).Visible && !((Control)codeEdit.Edit.EditBox).Focused)
			{
				OSUtils.SendMessage(((Control)codeEdit.Edit.EditBox).Handle, ((Message)(ref m)).Msg, ((Message)(ref m)).WParam, ((Message)(ref m)).LParam);
			}
		}
		return base.ProcessKeyPreview(ref m);
	}

	protected override void UpdateAutoSize()
	{
		if (base.AutoSize)
		{
			int val = ((ListBox)ListBox).ItemHeight * Math.Min(((ListBox)ListBox).Items.Count, EditConsts.MaxDropDownCount) + EditConsts.DefaultRowSeparator;
			int num = ListBox.ItemWidth + EditConsts.DefaultColumnSeparator * 4;
			if (ListBox.Images != null)
			{
				num += ListBox.Images.ImageSize.Width + EditConsts.DefaultColumnSeparator;
			}
			num += OSUtils.GetScrollSize(isVertical: false);
			((Form)this).ClientSize = new Size(Math.Max(num, EditConsts.DefaultMinListBoxWidth), Math.Max(val, EditConsts.DefaultMinListBoxHeight));
		}
	}

	protected override void DoShow(Point position)
	{
		Filter = string.Empty;
		ListBox.Images = ((base.Provider != null && base.Provider.Images != null) ? base.Provider.Images : base.Images);
		if (base.Provider != null && base.Provider.SelIndex >= 0)
		{
			if (ListBox.GetIndex(base.Provider.SelIndex) == base.Provider.SelIndex)
			{
				((ListControl)ListBox).SelectedIndex = base.Provider.SelIndex;
			}
			else
			{
				for (int i = 0; i < ((ListBox)ListBox).Items.Count; i++)
				{
					if (ListBox.GetIndex(i) == base.Provider.SelIndex)
					{
						((ListControl)ListBox).SelectedIndex = i;
						break;
					}
				}
			}
		}
		else
		{
			((ListControl)ListBox).SelectedIndex = -1;
		}
		showing = true;
		try
		{
			ShowCodeEdit(ref position);
			DoShowCodeHint();
			base.DoShow(position);
			if (codeEdit != null && ((Control)codeEdit).Visible)
			{
				((Control)codeEdit).Focus();
			}
		}
		finally
		{
			showing = false;
		}
	}

	protected override Control CreatePopupControl()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		ListBox val = (ListBox)(object)new CompletionListBox();
		val.SelectedIndexChanged += DoSelectionChanged;
		((Control)val).MouseWheel += new MouseEventHandler(DoMouseWheel);
		((Control)val).KeyDown += new KeyEventHandler(DoKeyDown);
		return (Control)(object)val;
	}

	protected override void DoHide()
	{
		base.DoHide();
		HideCodeHint();
		HideCodeEdit();
	}

	protected virtual void HideCodeHint()
	{
		if (codeHint != null)
		{
			((Control)codeHint).Visible = false;
		}
	}

	protected virtual void HideCodeEdit()
	{
		if (codeEdit != null)
		{
			((Control)codeEdit).Visible = false;
		}
	}

	protected virtual void DoShowCodeHint()
	{
		HideCodeHint();
		CodeHintTimer.Enabled = false;
		CodeHintTimer.Enabled = true;
	}

	protected virtual bool UpdateCodeEdit(ref Point position)
	{
		if (base.Provider != null && base.Provider.EditField != null && base.Provider.EditField != string.Empty)
		{
			CodeCompletionEdit codeCompletionEdit = CodeEdit;
			codeCompletionEdit.Provider = base.Provider;
			codeCompletionEdit.EditField = base.Provider.EditField;
			codeCompletionEdit.EditPath = base.Provider.EditPath;
			codeCompletionEdit.EditText = string.Empty;
			position.X = ((Form)codeCompletionEdit).Location.X + ((Control)codeCompletionEdit.Edit.EditBox).Left;
			return true;
		}
		HideCodeEdit();
		return false;
	}

	protected virtual void ShowCodeEdit(ref Point position)
	{
		if (base.Provider != null && base.Provider.EditField != null && base.Provider.EditField != string.Empty)
		{
			CodeCompletionEdit codeCompletionEdit = CodeEdit;
			codeCompletionEdit.PopupAt(new Point(position.X, position.Y - ((Control)codeCompletionEdit).Height));
			UpdateCodeEdit(ref position);
		}
	}

	protected virtual void OnShowCodeHint(object sender, EventArgs e)
	{
		ShowCodeHint(ListBox.GetIndex());
		CodeHintTimer.Enabled = false;
	}

	protected virtual void ShowCodeHint(int index)
	{
		if (((Control)this).Visible && index >= 0 && base.Provider != null && base.Provider.ShowDescriptions && index < base.Provider.Count)
		{
			string text = base.Provider.Descriptions[index];
			if (text != string.Empty)
			{
				CodeCompletionHint codeCompletionHint = CodeHint;
				ICodeCompletionProvider codeCompletionProvider = codeCompletionHint.Provider;
				codeCompletionProvider.UseHtmlFormatting = base.Provider.UseHtmlFormatting;
				codeCompletionHint.Provider = null;
				((QuickInfo)codeCompletionProvider).Text = text;
				codeCompletionHint.Provider = codeCompletionProvider;
				codeCompletionHint.ResetContent();
				Point position = ((Control)this).PointToScreen(new Point(((Control)this).Width, (((ListControl)ListBox).SelectedIndex - ((ListBox)ListBox).TopIndex) * ((ListBox)ListBox).ItemHeight));
				codeCompletionHint.EnsureVisible(ref position);
				Rectangle rectangle = new Rectangle(position.X, position.Y, ((Control)codeCompletionHint).Width, ((Control)codeCompletionHint).Height);
				rectangle.Intersect(((Control)this).Bounds);
				if (!rectangle.IsEmpty)
				{
					position.X = ((Control)this).Left - ((Control)codeCompletionHint).Width;
				}
				codeCompletionHint.PopupAt(position);
			}
		}
		else
		{
			HideCodeHint();
		}
	}

	protected virtual void DoSelectionChanged(object sender, EventArgs e)
	{
		DoShowCodeHint();
	}

	protected override int GetSelectedIndex()
	{
		return ListBox.GetIndex();
	}

	protected virtual void DoKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && (base.CompletionFlags & CodeCompletionFlags.AcceptOnDelimiter) != 0)
		{
			e.Handled = true;
		}
	}

	protected virtual void DoMouseWheel(object sender, MouseEventArgs e)
	{
		HideCodeHint();
	}

	protected virtual void CodeEditKeyPress(object sender, KeyPressEventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (e.KeyChar == '\b' && sender is TextBox && ((TextBoxBase)(TextBox)sender).SelectionStart == 0)
		{
			if (base.Provider.GetParent() != null)
			{
				base.Provider = base.Provider.GetParent();
				((ListControl)ListBox).SelectedIndex = base.Provider.SelIndex;
			}
			e.Handled = true;
		}
	}

	protected virtual void PathLabelClick(object sender, EventArgs e)
	{
		if (base.Provider.GetParent() != null)
		{
			base.Provider = base.Provider.GetParent();
			((ListControl)ListBox).SelectedIndex = base.Provider.SelIndex;
		}
	}

	protected internal override bool PerformSearch()
	{
		if (base.Provider == null)
		{
			return false;
		}
		string text = null;
		if (codeEdit != null && ((Control)codeEdit).Visible)
		{
			text = codeEdit.EditText;
		}
		else
		{
			SyntaxEdit syntaxEdit = GetSyntaxEdit();
			if (syntaxEdit != null)
			{
				Point position = syntaxEdit.Position;
				if (position.Y == base.StartPos.Y && position.X > base.StartPos.X)
				{
					text = syntaxEdit.Lines[position.Y];
					if (base.StartPos.X < text.Length)
					{
						text = ((position.X < text.Length) ? text.Substring(base.StartPos.X, position.X - base.StartPos.X) : text.Substring(base.StartPos.X));
					}
				}
			}
		}
		if (text != null)
		{
			if (Filtered)
			{
				Filter = text.Trim();
			}
			else
			{
				int num = IndexOfString(text);
				if (num >= 0)
				{
					((ListControl)ListBox).SelectedIndex = num;
				}
				else
				{
					((ListControl)ListBox).SelectedIndex = -1;
				}
			}
		}
		else if (Filtered)
		{
			Filter = string.Empty;
		}
		else
		{
			((ListControl)ListBox).SelectedIndex = -1;
		}
		return text != null;
	}

	protected override void SetProvider(ICodeCompletionProvider provider)
	{
		((ListBox)ListBox).Sorted = sorted && provider != null && provider.Count > 1;
		ListBox.Provider = provider;
		ListBox.Images = ((provider != null && provider.Images != null) ? provider.Images : base.Images);
		HideCodeHint();
		Point position = ((Form)this).Location;
		if (UpdateCodeEdit(ref position))
		{
			((Form)this).Location = position;
		}
	}

	protected void DoUpdateSize(object sender, EventArgs e)
	{
		UpdateAutoSize();
		UpdateDropDown();
	}

	public override void ResetContent()
	{
		ListBox.ResetContent();
	}

	public void ClearColumns()
	{
		ListBox.ClearColumns();
	}

	public ICodeCompletionColumn AddColumn()
	{
		return ListBox.AddColumn();
	}

	public ICodeCompletionColumn InsertColumn(int index)
	{
		return ListBox.InsertColumn(index);
	}

	public void RemoveColumnAt(int index)
	{
		ListBox.RemoveColumnAt(index);
	}

	public override void ResetSizeable()
	{
		base.Sizeable = true;
	}

	public virtual void ResetDropDownCount()
	{
		DropDownCount = EditConsts.DefaultDropDownCount;
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

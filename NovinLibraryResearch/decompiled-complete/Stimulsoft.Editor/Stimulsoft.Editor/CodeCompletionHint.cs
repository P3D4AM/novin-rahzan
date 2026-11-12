using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class CodeCompletionHint : CodeCompletionWindow, ICodeCompletionHint, ICodeCompletionWindow, IControl, ISyntaxPaint
{
	private Point curPos;

	private bool autoHide;

	private Timer hideTimer;

	protected override CreateParams CreateParams
	{
		get
		{
			OSUtils.InitCommonControls();
			CreateParams createParams = base.CreateParams;
			createParams.Parent = IntPtr.Zero;
			createParams.ClassName = "tooltips_class32";
			createParams.Style |= 1;
			createParams.ExStyle = 0;
			return createParams;
		}
	}

	protected internal CompletionHint Hint => (CompletionHint)(object)base.PopupControl;

	protected internal Timer HideTimer
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (hideTimer == null)
			{
				hideTimer = new Timer();
				hideTimer.Enabled = false;
				hideTimer.Interval = EditConsts.DefaultHideHintDelay;
				hideTimer.Tick += OnHideHint;
			}
			return hideTimer;
		}
	}

	public bool AutoHide
	{
		get
		{
			return autoHide;
		}
		set
		{
			if (autoHide != value)
			{
				autoHide = value;
				if (value && ((Control)this).Visible)
				{
					HideTimer.Enabled = true;
				}
			}
		}
	}

	public int AutoHidePause
	{
		get
		{
			if (hideTimer == null)
			{
				return EditConsts.DefaultHideHintDelay;
			}
			return hideTimer.Interval;
		}
		set
		{
			HideTimer.Interval = value;
		}
	}

	public bool DisableColorPaint
	{
		get
		{
			return Hint.SyntaxPaint.DisableColorPaint;
		}
		set
		{
			Hint.SyntaxPaint.DisableColorPaint = value;
		}
	}

	public bool DisableSyntaxPaint
	{
		get
		{
			return Hint.SyntaxPaint.DisableSyntaxPaint;
		}
		set
		{
			Hint.SyntaxPaint.DisableSyntaxPaint = value;
		}
	}

	public ILexer Lexer
	{
		get
		{
			return Hint.SyntaxPaint.Lexer;
		}
		set
		{
			Hint.SyntaxPaint.Lexer = value;
		}
	}

	public event CustomDrawEvent CustomDraw
	{
		add
		{
			Hint.SyntaxPaint.CustomDraw += value;
		}
		remove
		{
			Hint.SyntaxPaint.CustomDraw -= value;
		}
	}

	public CodeCompletionHint(SyntaxEdit owner)
		: base(owner)
	{
		base.Sizeable = false;
		((Form)this).FormBorderStyle = (FormBorderStyle)5;
		curPos = new Point(0, 0);
		((Control)this).SetStyle((ControlStyles)12550, true);
		CompletionHint hint = Hint;
		hint.UpdateSize = (EventHandler)Delegate.Combine(hint.UpdateSize, new EventHandler(DoUpdateSize));
		((Control)Hint).MouseLeave += CheckMouseLeave;
		base.CompletionFlags &= ~(CodeCompletionFlags.AcceptOnEnter | CodeCompletionFlags.AcceptOnDblClick | CodeCompletionFlags.AcceptOnDelimiter);
	}

	~CodeCompletionHint()
	{
		if (hideTimer != null)
		{
			((Component)(object)hideTimer).Dispose();
		}
	}

	protected void UpdateProviderParamIndex(ICodeCompletionProvider provider)
	{
		foreach (IListMember item in provider)
		{
			item.CurrentParamIndex = -1;
		}
	}

	protected bool UpdateProviderContent(ICodeCompletionProvider pr1, ICodeCompletionProvider pr2)
	{
		bool flag = pr1 is IListMembers && pr2 is IListMembers;
		if (flag)
		{
			flag = pr1.Count == pr2.Count;
			if (flag)
			{
				for (int i = 0; i < pr1.Count; i++)
				{
					IListMember listMember = ((IListMembers)pr1).GetListMember(i);
					IListMember listMember2 = ((IListMembers)pr2).GetListMember(i);
					flag = listMember.Name == listMember2.Name && listMember.GetParamText(useFormatting: false) == listMember2.GetParamText(useFormatting: false);
					if (!flag)
					{
						break;
					}
				}
			}
		}
		if (flag)
		{
			for (int j = 0; j < pr1.Count; j++)
			{
				IListMember listMember3 = ((IListMembers)pr1).GetListMember(j);
				IListMember listMember4 = ((IListMembers)pr2).GetListMember(j);
				listMember3.CurrentParamIndex = listMember4.CurrentParamIndex;
			}
		}
		return flag;
	}

	protected void PositionChanged()
	{
		SyntaxEdit syntaxEdit = GetSyntaxEdit();
		if (syntaxEdit == null || base.StartPos.X < 0 || base.StartPos.Y < 0 || curPos.Equals((object?)syntaxEdit.Position))
		{
			return;
		}
		curPos = syntaxEdit.Position;
		if (base.Provider is IParameterInfo)
		{
			CodeCompletionArgs codeCompletionArgs = new CodeCompletionArgs();
			codeCompletionArgs.CompletionType = CodeCompletionType.ParameterInfo;
			codeCompletionArgs.NeedReparse = true;
			syntaxEdit.ProcessCodeCompletion(codeCompletionArgs, syntaxEdit.RemovePlainText(curPos.Y));
			if (!codeCompletionArgs.NeedShow)
			{
				codeCompletionArgs.Provider = null;
			}
			Point startPosition = codeCompletionArgs.StartPosition;
			if (base.StartPos.Equals((object?)startPosition))
			{
				if (!UpdateProviderContent(base.Provider, codeCompletionArgs.Provider))
				{
					base.Provider = codeCompletionArgs.Provider;
				}
			}
			else if (base.StartPos.Y > startPosition.Y || (base.StartPos.Y == startPosition.Y && base.StartPos.X > startPosition.X))
			{
				base.Provider = codeCompletionArgs.Provider;
				base.StartPos = startPosition;
				base.DisplayPos = startPosition;
				base.EndPos = codeCompletionArgs.EndPosition;
			}
			else
			{
				UpdateProviderParamIndex(base.Provider);
			}
		}
		if (base.Provider == null || base.Provider.Count == 0)
		{
			Close(accept: false);
		}
		else
		{
			Hint.UpdateHint();
		}
	}

	protected void DoUpdateSize(object sender, EventArgs e)
	{
		((Form)this).Size = Hint.HintSize;
	}

	protected override void DoShow(Point position)
	{
		base.DoShow(position);
		if (base.Provider != null && base.Provider.SelIndex >= 0)
		{
			Hint.SelectedIndex = base.Provider.SelIndex;
		}
		Hint.UpdateHint();
		((Control)this).Update();
		if (autoHide)
		{
			HideTimer.Enabled = true;
		}
	}

	protected override void DoHide()
	{
		base.DoHide();
		if (hideTimer != null)
		{
			hideTimer.Enabled = false;
		}
	}

	protected void OnHideHint(object sender, EventArgs e)
	{
		Close(accept: false);
	}

	protected override bool ProcessKeyPreview(ref Message m)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected I4, but got Unknown
		if (OnProcessKeyPreview(ref m))
		{
			return true;
		}
		if (((Message)(ref m)).Msg == 256 || ((Message)(ref m)).Msg == 260)
		{
			Keys val = (Keys)(((Message)(ref m)).WParam.ToInt32() & 0xFFFF);
			switch (val - 33)
			{
			case 5:
				if (Hint.NeedArrows())
				{
					Hint.ChangeSelection(inc: false);
					return true;
				}
				Close(accept: false);
				break;
			case 7:
				if (Hint.NeedArrows())
				{
					Hint.ChangeSelection(inc: true);
					return true;
				}
				Close(accept: false);
				break;
			case 0:
			case 1:
			case 2:
			case 3:
				Close(accept: false);
				break;
			}
		}
		if (((Message)(ref m)).Msg == 258)
		{
			char value = (char)((Message)(ref m)).WParam.ToInt32();
			SyntaxEdit syntaxEdit = GetSyntaxEdit();
			if (syntaxEdit != null && syntaxEdit.Lexer is ISyntaxParser && Array.IndexOf(((ISyntaxParser)syntaxEdit.Lexer).CodeCompletionStopChars, value) >= 0)
			{
				Close(accept: false);
			}
		}
		PositionChanged();
		base.CheckPositionValid();
		return base.ProcessKeyPreview(ref m);
	}

	protected override void CheckPositionValid()
	{
	}

	protected override Control CreatePopupControl()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		CompletionHint completionHint = new CompletionHint();
		((Control)completionHint).MouseWheel += new MouseEventHandler(DoMouseWheel);
		return (Control)(object)completionHint;
	}

	protected override int GetSelectedIndex()
	{
		return Hint.SelectedIndex;
	}

	protected override void SetProvider(ICodeCompletionProvider provider)
	{
		Hint.Provider = provider;
	}

	protected virtual void DoMouseWheel(object sender, MouseEventArgs e)
	{
		SyntaxEdit syntaxEdit = GetSyntaxEdit();
		if (syntaxEdit != null)
		{
			syntaxEdit.DoMouseWheel(e.Delta);
			if (base.DisplayPos.Y >= 0 && base.DisplayPos.X >= 0)
			{
				Point point = syntaxEdit.TextToScreen(base.DisplayPos);
				point.Y = Math.Min(Math.Max(point.Y + syntaxEdit.Painter.FontHeight + EditConsts.DefaultHintOffsetY, syntaxEdit.ClientRect.Top), syntaxEdit.ClientRect.Bottom - ((Control)this).Height);
				((Control)this).Top = ((Control)syntaxEdit).PointToScreen(point).Y;
			}
		}
	}

	public override void ResetContent()
	{
		Hint.ResetContent();
	}

	public virtual void ResetAutoHide()
	{
		AutoHide = false;
	}

	public virtual void ResetAutoHidePause()
	{
		AutoHidePause = EditConsts.DefaultHideHintDelay;
	}

	public void PaintSyntax(IPainter painter, int startLine, int endLine, Point position, Rectangle rect, bool specialPaint)
	{
		Hint.SyntaxPaint.PaintSyntax(painter, startLine, endLine, position, rect, specialPaint);
	}

	public void DrawLine(int index, Point position, Rectangle clipRect)
	{
		Hint.SyntaxPaint.DrawLine(index, position, clipRect);
	}

	public void DrawLine(int index, string line, short[] colorData, Point position, Rectangle clipRect)
	{
		Hint.SyntaxPaint.DrawLine(index, line, colorData, position, clipRect);
	}

	public int MeasureLine(int index, int pos, int len)
	{
		return Hint.SyntaxPaint.MeasureLine(index, pos, len);
	}

	public int MeasureLine(int index, int pos, int len, int width, out int chars, bool exact)
	{
		return Hint.SyntaxPaint.MeasureLine(index, pos, len, width, out chars, exact);
	}

	public int MeasureLine(string line, short[] colorData, int pos, int len)
	{
		return Hint.SyntaxPaint.MeasureLine(line, colorData, pos, len);
	}

	public int MeasureLine(string line, short[] colorData, int pos, int len, int width, out int chars, bool exact)
	{
		return Hint.SyntaxPaint.MeasureLine(line, colorData, pos, len, width, out chars, exact);
	}

	public void ResetDisableColorPaint()
	{
		Hint.SyntaxPaint.ResetDisableColorPaint();
	}

	public void ResetDisableSyntaxPaint()
	{
		Hint.SyntaxPaint.ResetDisableSyntaxPaint();
	}

	public bool OnCustomDraw(IPainter painter, Rectangle rect, DrawStage stage, DrawState state, DrawInfo info)
	{
		return Hint.SyntaxPaint.OnCustomDraw(painter, rect, stage, state, info);
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

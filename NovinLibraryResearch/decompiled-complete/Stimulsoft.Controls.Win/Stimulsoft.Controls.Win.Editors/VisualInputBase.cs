using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualInputBase : VisualItem
{
	private int _InputPosition;

	private string _InputStack = "";

	private bool _InsertMode = true;

	private string _UndoInputStack = "";

	private bool _IsEmpty = true;

	private bool _AutoOverwrite = true;

	private string _WatermarkText = "";

	private bool _WatermarkEnabled = true;

	private bool _IsReadOnly;

	protected string InputStack => _InputStack;

	public int InputPosition => _InputPosition;

	protected bool IsInputStackEmpty => _InputStack.Length == 0;

	protected virtual bool DrawWatermark
	{
		get
		{
			if (WatermarkEnabled && IsEmpty && WatermarkText.Length > 0)
			{
				return !base.IsFocused;
			}
			return false;
		}
	}

	public virtual bool IsEmpty
	{
		get
		{
			return _IsEmpty;
		}
		set
		{
			if (value != _IsEmpty)
			{
				_IsEmpty = value;
				InvalidateArrange();
				if (base.Parent is VisualInputGroup)
				{
					((VisualInputGroup)base.Parent).UpdateIsEmpty();
				}
				if (value)
				{
					ResetValue();
				}
				OnIsEmptyChanged();
			}
		}
	}

	public bool AutoOverwrite
	{
		get
		{
			return _AutoOverwrite;
		}
		set
		{
			_AutoOverwrite = value;
		}
	}

	[Localizable(true)]
	public string WatermarkText
	{
		get
		{
			return _WatermarkText;
		}
		set
		{
			if (value != null)
			{
				_WatermarkText = value;
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed if set.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _WatermarkEnabled;
		}
		set
		{
			_WatermarkEnabled = value;
		}
	}

	public virtual bool IsReadOnly
	{
		get
		{
			return _IsReadOnly;
		}
		set
		{
			_IsReadOnly = value;
		}
	}

	public event EventHandler InputChanged;

	public event InputValidationEventHandler ValidateInput;

	public event EventHandler IsEmptyChanged;

	public VisualInputBase()
	{
		base.Focusable = true;
	}

	internal override void ProcessKeyDown(KeyEventArgs e)
	{
		if (!_IsReadOnly)
		{
			base.ProcessKeyDown(e);
		}
	}

	internal override void ProcessKeyPress(KeyPressEventArgs e)
	{
		if (!_IsReadOnly)
		{
			base.ProcessKeyPress(e);
		}
	}

	internal override void ProcessKeyUp(KeyEventArgs e)
	{
		if (!_IsReadOnly)
		{
			base.ProcessKeyUp(e);
		}
	}

	internal override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (!_IsReadOnly)
		{
			return base.ProcessCmdKey(ref msg, keyData);
		}
		return false;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		OnInputKeyDown(e);
		base.OnKeyDown(e);
	}

	protected virtual void OnInputKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Invalid comparison between Unknown and I4
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Invalid comparison between Unknown and I4
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 8)
		{
			ProcessBackSpace(e);
		}
		else if ((int)e.KeyCode == 46)
		{
			ProcessDelete(e);
		}
		else if ((int)e.KeyCode == 67 && (int)e.Modifiers == 131072)
		{
			ProcessClipboardCopy();
			e.Handled = true;
		}
		else if ((int)e.KeyCode == 88 && (int)e.Modifiers == 131072)
		{
			ProcessClipboardCut();
			e.Handled = true;
		}
		else if ((int)e.KeyCode == 86 && (int)e.Modifiers == 131072)
		{
			ProcessClipboardPaste();
			e.Handled = true;
		}
		else if ((int)e.KeyCode == 90 && (int)e.Modifiers == 131072)
		{
			ProcessUndo();
			e.Handled = true;
		}
	}

	internal virtual void ProcessClear()
	{
		OnClear();
	}

	internal virtual void ProcessClipboardCopy()
	{
		OnClipboardCopy();
	}

	internal virtual void ProcessClipboardCut()
	{
		OnClipboardCut();
	}

	internal virtual void ProcessClipboardPaste()
	{
		OnClipboardPaste();
	}

	internal virtual void ProcessUndo()
	{
		OnUndo();
	}

	protected virtual void OnUndo()
	{
		if (SetInputStack(_UndoInputStack))
		{
			_InputPosition = _InputStack.Length;
			OnInputKeyAccepted();
		}
	}

	public void UndoInput()
	{
		ProcessUndo();
	}

	protected virtual void OnClear()
	{
		if (SetInputStack(""))
		{
			_InputPosition = 0;
		}
	}

	protected virtual void OnClipboardCut()
	{
		OnClipboardCopy();
		OnClear();
	}

	protected virtual void OnClipboardPaste()
	{
		if (Clipboard.ContainsText() && SetInputStack(Clipboard.GetText()))
		{
			_InputPosition = _InputStack.Length;
			OnInputKeyAccepted();
		}
	}

	protected virtual void OnClipboardCopy()
	{
		Clipboard.SetText(GetInputStringValue());
	}

	protected virtual string GetInputStringValue()
	{
		return _InputStack;
	}

	protected virtual void ProcessDelete(KeyEventArgs e)
	{
		OnClear();
		e.Handled = true;
	}

	protected virtual void ProcessBackSpace(KeyEventArgs e)
	{
		if (_InputStack.Length > 0 && _InputPosition > 0)
		{
			string inputStack = _InputStack.Substring(0, _InputPosition - 1) + _InputStack.Substring(_InputPosition);
			if (SetInputStack(inputStack))
			{
				_InputPosition = Math.Max(0, _InputPosition - 1);
			}
		}
		else
		{
			OnClear();
		}
		e.Handled = true;
	}

	protected virtual void ResetValue()
	{
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		OnInputKeyPress(e);
		base.OnKeyPress(e);
	}

	protected virtual void OnInputKeyPress(KeyPressEventArgs e)
	{
		if (AcceptKeyPress(e))
		{
			UpdateInputStack(e.KeyChar);
		}
	}

	protected virtual void UpdateInputStack(char c)
	{
		string text = _InputStack;
		if (_InputPosition >= _InputStack.Length)
		{
			if (_InputPosition > _InputStack.Length)
			{
				_InputPosition = _InputStack.Length;
			}
			text += c;
		}
		else if (_InsertMode)
		{
			text.Insert(_InputPosition, c.ToString());
		}
		else
		{
			text = _InputStack.Substring(0, _InputPosition - 1) + c + _InputStack.Substring(_InputPosition + 1);
		}
		if (SetInputStack(text))
		{
			_InputPosition++;
			OnInputKeyAccepted();
		}
	}

	protected virtual void OnInputKeyAccepted()
	{
	}

	protected virtual bool SetInputStack(string s)
	{
		if (ValidateNewInputStack(s))
		{
			_InputStack = s;
			OnInputStackChanged();
			return true;
		}
		return false;
	}

	protected virtual bool ValidateNewInputStack(string s)
	{
		if (this.ValidateInput != null)
		{
			InputValidationEventArgs inputValidationEventArgs = new InputValidationEventArgs(s);
			this.ValidateInput(this, inputValidationEventArgs);
			return inputValidationEventArgs.AcceptInput;
		}
		return true;
	}

	protected virtual void OnInputStackChanged()
	{
		OnInputChanged();
	}

	protected virtual void OnInputChanged()
	{
		if (base.Parent != null)
		{
			base.Parent.ProcessInputChanged(this);
		}
		if (this.InputChanged != null)
		{
			this.InputChanged(this, new EventArgs());
		}
	}

	protected virtual bool AcceptKeyPress(KeyPressEventArgs e)
	{
		e.Handled = true;
		return true;
	}

	protected override void OnGotFocus()
	{
		OnInputGotFocus();
		base.OnGotFocus();
	}

	protected override void OnLostFocus()
	{
		OnInputLostFocus();
		base.OnLostFocus();
	}

	protected virtual void OnInputGotFocus()
	{
		ResetInputStack();
		_UndoInputStack = GetInputStringValue();
		InvalidateArrange();
	}

	protected virtual void OnInputLostFocus()
	{
		InvalidateArrange();
	}

	protected void SetInputPosition(int position)
	{
		_InputPosition = position;
	}

	protected virtual void InputComplete(bool sendNotification)
	{
		if (_AutoOverwrite)
		{
			ResetInputStack();
		}
		if (sendNotification && base.Parent != null)
		{
			base.Parent.ProcessInputComplete();
		}
	}

	public void ResetInputPosition()
	{
		ResetInputStack();
	}

	protected virtual void ResetInputStack()
	{
		_InputStack = "";
		_InputPosition = 0;
	}

	public override void PerformLayout(PaintInfo p)
	{
		if (p.WatermarkEnabled && DrawWatermark)
		{
			Font font = p.DefaultFont;
			if (p.WatermarkFont != null)
			{
				font = p.WatermarkFont;
			}
			Size size = TextDrawing.MeasureString(p.Graphics, WatermarkText, font, 0, eTextFormat.NoPadding);
			base.Size = size;
		}
		base.PerformLayout(p);
	}

	protected override void OnPaint(PaintInfo p)
	{
		if (p.WatermarkEnabled && DrawWatermark)
		{
			Font font = p.DefaultFont;
			if (p.WatermarkFont != null)
			{
				font = p.WatermarkFont;
			}
			TextDrawing.DrawString(p.Graphics, WatermarkText, font, p.WatermarkColor, base.RenderBounds, eTextFormat.Default);
		}
		base.OnPaint(p);
	}

	protected virtual void OnIsEmptyChanged()
	{
		if (this.IsEmptyChanged != null)
		{
			this.IsEmptyChanged(this, new EventArgs());
		}
	}
}

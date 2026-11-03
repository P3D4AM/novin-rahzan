using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors;

public class VisualInputBase : VisualItem
{
	private int __22D9;

	private string __22DA = "";

	private bool __22DB = true;

	private string __22DC = "";

	private EventHandler _22DD;

	private InputValidationEventHandler _22DE;

	private EventHandler _22DF;

	private bool __22CE = true;

	private bool __22D0 = true;

	private string _ᠥ = "";

	private bool _ᚿ = true;

	private bool _ᣇ;

	protected string InputStack => __22DA;

	public int InputPosition => __22D9;

	protected bool IsInputStackEmpty => __22DA.Length == 0;

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
			return __22CE;
		}
		set
		{
			if (value != __22CE)
			{
				__22CE = value;
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
			return __22D0;
		}
		set
		{
			__22D0 = value;
		}
	}

	[Localizable(true)]
	public string WatermarkText
	{
		get
		{
			return _ᠥ;
		}
		set
		{
			if (value != null)
			{
				_ᠥ = value;
			}
		}
	}

	[Description("Indicates whether watermark text is displayed if set.")]
	[DefaultValue(true)]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _ᚿ;
		}
		set
		{
			_ᚿ = value;
		}
	}

	public virtual bool IsReadOnly
	{
		get
		{
			return _ᣇ;
		}
		set
		{
			_ᣇ = value;
		}
	}

	public event EventHandler InputChanged
	{
		add
		{
			EventHandler eventHandler = _22DD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _22DD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _22DD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _22DD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event InputValidationEventHandler ValidateInput
	{
		add
		{
			InputValidationEventHandler inputValidationEventHandler = _22DE;
			InputValidationEventHandler inputValidationEventHandler2;
			do
			{
				inputValidationEventHandler2 = inputValidationEventHandler;
				InputValidationEventHandler value2 = (InputValidationEventHandler)Delegate.Combine(inputValidationEventHandler2, value);
				inputValidationEventHandler = Interlocked.CompareExchange(ref _22DE, value2, inputValidationEventHandler2);
			}
			while ((object)inputValidationEventHandler != inputValidationEventHandler2);
		}
		remove
		{
			InputValidationEventHandler inputValidationEventHandler = _22DE;
			InputValidationEventHandler inputValidationEventHandler2;
			do
			{
				inputValidationEventHandler2 = inputValidationEventHandler;
				InputValidationEventHandler value2 = (InputValidationEventHandler)Delegate.Remove(inputValidationEventHandler2, value);
				inputValidationEventHandler = Interlocked.CompareExchange(ref _22DE, value2, inputValidationEventHandler2);
			}
			while ((object)inputValidationEventHandler != inputValidationEventHandler2);
		}
	}

	public event EventHandler IsEmptyChanged
	{
		add
		{
			EventHandler eventHandler = _22DF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _22DF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _22DF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _22DF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public VisualInputBase()
	{
		base.Focusable = true;
	}

	internal override void ᡟ(KeyEventArgs ؿ)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		if (!_ᣇ || ((int)ؿ.KeyCode == 67 && (int)ؿ.Modifiers == 131072))
		{
			base.ᡟ(ؿ);
		}
	}

	internal override void ᡡ(KeyPressEventArgs ؿ)
	{
		if (!_ᣇ)
		{
			base.ᡡ(ؿ);
		}
	}

	internal override void ᡠ(KeyEventArgs ؿ)
	{
		if (!_ᣇ)
		{
			base.ᡠ(ؿ);
		}
	}

	internal override bool ᘭ(ref Message ᘮ, Keys ᘯ)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (!_ᣇ)
		{
			return base.ᘭ(ref ᘮ, ᘯ);
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
			_22E1();
			e.Handled = true;
		}
		else if ((int)e.KeyCode == 88 && (int)e.Modifiers == 131072)
		{
			_22E2();
			e.Handled = true;
		}
		else if ((int)e.KeyCode == 86 && (int)e.Modifiers == 131072)
		{
			_22E3();
			e.Handled = true;
		}
		else if ((int)e.KeyCode == 90 && (int)e.Modifiers == 131072)
		{
			_22E4();
			e.Handled = true;
		}
	}

	internal virtual void _22E0()
	{
		OnClear();
	}

	internal virtual void _22E1()
	{
		OnClipboardCopy();
	}

	internal virtual void _22E2()
	{
		OnClipboardCut();
	}

	internal virtual void _22E3()
	{
		OnClipboardPaste();
	}

	internal virtual void _22E4()
	{
		OnUndo();
	}

	protected virtual void OnUndo()
	{
		if (SetInputStack(__22DC))
		{
			__22D9 = __22DA.Length;
			OnInputKeyAccepted();
		}
	}

	public void UndoInput()
	{
		_22E4();
	}

	protected virtual void OnClear()
	{
		if (SetInputStack(""))
		{
			__22D9 = 0;
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
			__22D9 = __22DA.Length;
			OnInputKeyAccepted();
		}
	}

	protected virtual void OnClipboardCopy()
	{
		Clipboard.SetText(GetInputStringValue());
	}

	protected virtual string GetInputStringValue()
	{
		return __22DA;
	}

	protected virtual void ProcessDelete(KeyEventArgs e)
	{
		OnClear();
		e.Handled = true;
	}

	protected virtual void ProcessBackSpace(KeyEventArgs e)
	{
		if (__22DA.Length > 0 && __22D9 > 0)
		{
			string inputStack = __22DA.Substring(0, __22D9 - 1) + __22DA.Substring(__22D9);
			if (SetInputStack(inputStack))
			{
				__22D9 = Math.Max(0, __22D9 - 1);
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
		string text = __22DA;
		if (__22D9 >= __22DA.Length)
		{
			if (__22D9 > __22DA.Length)
			{
				__22D9 = __22DA.Length;
			}
			text += c;
		}
		else if (__22DB)
		{
			text.Insert(__22D9, c.ToString());
		}
		else
		{
			text = __22DA.Substring(0, __22D9 - 1) + c + __22DA.Substring(__22D9 + 1);
		}
		if (SetInputStack(text))
		{
			__22D9++;
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
			__22DA = s;
			OnInputStackChanged();
			return true;
		}
		return false;
	}

	protected virtual bool ValidateNewInputStack(string s)
	{
		if (_22DE != null)
		{
			InputValidationEventArgs inputValidationEventArgs = new InputValidationEventArgs(s);
			_22DE(this, inputValidationEventArgs);
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
			base.Parent.ᡶ(this);
		}
		if (_22DD != null)
		{
			_22DD(this, new EventArgs());
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
		__22DC = GetInputStringValue();
		InvalidateArrange();
	}

	protected virtual void OnInputLostFocus()
	{
		InvalidateArrange();
	}

	protected void SetInputPosition(int position)
	{
		__22D9 = position;
	}

	protected virtual void InputComplete(bool sendNotification)
	{
		if (__22D0)
		{
			ResetInputStack();
		}
		if (sendNotification && base.Parent != null)
		{
			base.Parent.ᡵ();
		}
	}

	public void ResetInputPosition()
	{
		ResetInputStack();
	}

	protected virtual void ResetInputStack()
	{
		__22DA = "";
		__22D9 = 0;
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
		if (_22DF != null)
		{
			_22DF(this, new EventArgs());
		}
	}
}

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Editors.DateTimeAdv;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualIntegerInput : VisualNumericInput
{
	private int _Value;

	private int _MinValue = int.MinValue;

	private int _MaxValue = int.MaxValue;

	private int _Increment = 1;

	private string _DisplayFormat = "";

	public int MinValue
	{
		get
		{
			return _MinValue;
		}
		set
		{
			if (_MinValue == value)
			{
				return;
			}
			_MinValue = value;
			if (_MinValue >= 0)
			{
				base.AllowsNegativeValue = false;
			}
			else
			{
				base.AllowsNegativeValue = true;
			}
			if (_Value < _MinValue && (!IsEmpty || (IsEmpty && !base.AllowEmptyState)))
			{
				if (base.IsFocused)
				{
					SetValueDirect(FormatNumber(_MinValue));
				}
				else
				{
					SetValue(_MinValue);
				}
			}
		}
	}

	public int MaxValue
	{
		get
		{
			return _MaxValue;
		}
		set
		{
			if (_MaxValue == value)
			{
				return;
			}
			_MaxValue = value;
			if (_Value > _MaxValue && (!IsEmpty || (IsEmpty && !base.AllowEmptyState)))
			{
				if (base.IsFocused)
				{
					SetValueDirect(FormatNumber(_MaxValue));
				}
				else
				{
					SetValue(_MaxValue);
				}
			}
		}
	}

	private string NumberDecimalSeparator => DateTimeInput.GetActiveCulture().NumberFormat.NumberDecimalSeparator;

	private string NumberThousandsSeparator => DateTimeInput.GetActiveCulture().NumberFormat.NumberGroupSeparator;

	[DefaultValue(1)]
	public int Increment
	{
		get
		{
			return _Increment;
		}
		set
		{
			value = Math.Abs(value);
			if (_Increment != value)
			{
				_Increment = value;
			}
		}
	}

	[Browsable(false)]
	public virtual string Text
	{
		get
		{
			if (IsEmpty && base.AllowEmptyState)
			{
				return "";
			}
			return ConvertToString(_Value, useFormat: true, forceFormat: true);
		}
	}

	public virtual int Value
	{
		get
		{
			if (_Value < _MinValue)
			{
				return _MinValue;
			}
			if (_Value > _MaxValue)
			{
				return _MaxValue;
			}
			return _Value;
		}
		set
		{
			if (value < _MinValue)
			{
				value = _MinValue;
			}
			if (value > _MaxValue)
			{
				value = _MaxValue;
			}
			SetValueDirect(ConvertToString(value));
			if (base.IsFocused)
			{
				InputComplete(sendNotification: false);
			}
		}
	}

	[Description("Indicates Numeric String Format that is used to format the numeric value entered for display purpose.")]
	public string DisplayFormat
	{
		get
		{
			return _DisplayFormat;
		}
		set
		{
			if (value != null)
			{
				_DisplayFormat = value;
			}
		}
	}

	protected override void OnInputStackChanged()
	{
		if (base.InputStack.Length > 0)
		{
			IsEmpty = false;
			if (base.InputStack == "-")
			{
				SetValue(0, raiseValueChanged: true);
				return;
			}
			int i = int.Parse(base.InputStack);
			SetValue(i, raiseValueChanged: true);
		}
		else
		{
			if (base.AllowEmptyState)
			{
				IsEmpty = true;
			}
			SetValue(0, raiseValueChanged: true);
		}
		base.OnInputStackChanged();
	}

	protected override void OnInputKeyAccepted()
	{
		CheckInputComplete(sendNotification: true);
		base.OnInputKeyAccepted();
	}

	private void CheckInputComplete(bool sendNotification)
	{
		int nextPredictedValue = GetNextPredictedValue(_Value);
		if (_Value == _MaxValue || (ConvertToString(_Value).Length >= ConvertToString(_MaxValue).Length && ConvertToString(_Value).Length >= ConvertToString(_MinValue).Length) || nextPredictedValue > _MaxValue)
		{
			InputComplete(sendNotification);
		}
		else if (IsLeadingZero && ConvertToString(_Value).Length + 1 >= ConvertToString(_MaxValue).Length)
		{
			InputComplete(sendNotification);
		}
	}

	private int GetNextPredictedValue(int value)
	{
		string text = "";
		text = ((value != 0) ? (ConvertToString(value) + "0") : "1");
		int result = value;
		int.TryParse(text, out result);
		return result;
	}

	protected override void OnLostFocus()
	{
		ValidateValue();
		base.OnLostFocus();
	}

	protected virtual void ValidateValue()
	{
		if (_Value < _MinValue && !IsEmpty)
		{
			Value = _MinValue;
		}
		else if (_Value > _MaxValue && !IsEmpty)
		{
			Value = _MaxValue;
		}
	}

	private void SetValue(int i, bool raiseValueChanged)
	{
		bool flag = _Value != i || raiseValueChanged;
		_Value = i;
		if (flag)
		{
			OnValueChanged();
		}
		InvalidateArrange();
	}

	private void SetValue(int i)
	{
		SetValue(i, raiseValueChanged: false);
	}

	protected override string GetMeasureString()
	{
		string result = ConvertToString(_Value, useFormat: true);
		if (IsEmpty && base.AllowEmptyState)
		{
			result = "T";
		}
		else if (base.InputStack == "-" && _Value == 0)
		{
			result = "-";
		}
		return result;
	}

	protected override string GetRenderString()
	{
		if (base.InputStack == "-" && _Value == 0)
		{
			return "-";
		}
		if (IsEmpty && base.AllowEmptyState)
		{
			return "";
		}
		return ConvertToString(_Value, useFormat: true);
	}

	protected override void NegateValue()
	{
		if (!IsEmpty && _MaxValue >= 0)
		{
			int i = -_Value;
			SetValueDirect(FormatNumber(i));
		}
	}

	protected override void ResetValue()
	{
		int num = 0;
		if (_MinValue > num)
		{
			num = _MinValue;
		}
		if (num > _MaxValue)
		{
			num = _MaxValue;
		}
		SetValue(num);
		InvalidateArrange();
	}

	private string FormatNumber(int i)
	{
		CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
		if (activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider formatProvider)
		{
			return i.ToString(formatProvider);
		}
		return i.ToString();
	}

	protected override void OnClipboardPaste()
	{
		if (Clipboard.ContainsText())
		{
			string s = StripNonNumeric(Clipboard.GetText().Replace(" ", ""));
			int result = 0;
			if (int.TryParse(s, out result) && result <= _MaxValue && result >= _MinValue && SetInputStack(result.ToString()))
			{
				SetInputPosition(base.InputStack.Length);
				OnInputKeyAccepted();
			}
		}
		else
		{
			base.OnClipboardPaste();
		}
	}

	private string StripNonNumeric(string p)
	{
		string text = "";
		for (int i = 0; i < p.Length && !(p[i].ToString() == NumberDecimalSeparator); i++)
		{
			if ((p[i] >= '0' && p[i] <= '9') || (i == 0 && p[i] == '-'))
			{
				text += p[i];
			}
			else if (text.Length > 0 && p[i].ToString() != NumberThousandsSeparator)
			{
				break;
			}
		}
		return text;
	}

	protected override bool ValidateNewInputStack(string s)
	{
		if (s.Length > 0)
		{
			if (s == "-" && base.AllowsNegativeValue)
			{
				return true;
			}
			int result = 0;
			if (int.TryParse(s, out result))
			{
				if ((result > _MaxValue && _MaxValue >= 0) || (result < _MinValue && _MinValue < 0))
				{
					return false;
				}
				return true;
			}
			return false;
		}
		return base.ValidateNewInputStack(s);
	}

	public override void IncreaseValue()
	{
		int num = _Value + _Increment;
		if (num > _MaxValue)
		{
			num = _MaxValue;
		}
		else if (num < _MinValue)
		{
			num = _MinValue;
		}
		if (_Value < _MaxValue && num > _MaxValue)
		{
			num = _MaxValue;
		}
		if (num <= _MaxValue)
		{
			if (IsEmpty && base.AllowEmptyState)
			{
				num = Math.Max(0, _MinValue);
			}
			SetValueDirect(FormatNumber(num));
			CheckInputComplete(sendNotification: false);
		}
		base.IncreaseValue();
	}

	public override void DecreaseValue()
	{
		int num = _Value - _Increment;
		if (num > _MaxValue)
		{
			num = _MaxValue;
		}
		else if (num < _MinValue)
		{
			num = _MinValue;
		}
		if (_Value > _MinValue && num < _MinValue)
		{
			num = _MinValue;
		}
		if (num >= _MinValue)
		{
			SetValueDirect(FormatNumber(num));
			CheckInputComplete(sendNotification: false);
		}
		base.DecreaseValue();
	}

	private void SetValueDirect(string s)
	{
		if (SetInputStack(s))
		{
			SetInputPosition(base.InputStack.Length);
		}
	}

	private string ConvertToString(int i)
	{
		return ConvertToString(i, useFormat: false);
	}

	private string ConvertToString(int i, bool useFormat)
	{
		return ConvertToString(i, useFormat, forceFormat: false);
	}

	private string ConvertToString(int i, bool useFormat, bool forceFormat)
	{
		if ((!forceFormat && (!useFormat || base.IsFocused)) || _DisplayFormat.Length == 0)
		{
			return FormatNumber(i);
		}
		CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
		if (activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider formatProvider)
		{
			return i.ToString(_DisplayFormat, formatProvider);
		}
		return i.ToString(_DisplayFormat);
	}

	protected override string GetInputStringValue()
	{
		return ConvertToString(_Value);
	}
}

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Editors.DateTimeAdv;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualDoubleInput : VisualNumericInput
{
	private double _Value;

	private double _MinValue = double.MinValue;

	private double _MaxValue = double.MaxValue;

	private double _Increment = 1.0;

	private string _DisplayFormat = "";

	public double MinValue
	{
		get
		{
			return _MinValue;
		}
		set
		{
			if (_MinValue != value)
			{
				_MinValue = value;
				if (_MinValue >= 0.0)
				{
					base.AllowsNegativeValue = false;
				}
				else
				{
					base.AllowsNegativeValue = true;
				}
				ValidateValue();
			}
		}
	}

	public double MaxValue
	{
		get
		{
			return _MaxValue;
		}
		set
		{
			if (_MaxValue != value)
			{
				_MaxValue = value;
				ValidateValue();
			}
		}
	}

	[DefaultValue(1)]
	public double Increment
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

	public double Value
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
			if (base.IsFocused)
			{
				SetValueDirect(StripNonNumeric(FormatNumber(value)));
				if (base.IsFocused)
				{
					InputComplete(sendNotification: false);
				}
			}
			else
			{
				IsEmpty = false;
				SetValue(value, raiseValueChanged: true);
			}
		}
	}

	[DefaultValue("")]
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

	private string NumberDecimalSeparator => DateTimeInput.GetActiveCulture().NumberFormat.NumberDecimalSeparator;

	private string NumberThousandsSeparator => DateTimeInput.GetActiveCulture().NumberFormat.NumberGroupSeparator;

	[Browsable(false)]
	public virtual string Text
	{
		get
		{
			if (IsEmpty && base.AllowEmptyState)
			{
				return "";
			}
			return ConvertToString(_Value, useFormat: true, forceDisplayFormat: true);
		}
	}

	protected override void OnLostFocus()
	{
		ValidateValue();
		base.OnLostFocus();
	}

	protected virtual void ValidateValue()
	{
		if (_Value < _MinValue)
		{
			Value = _MinValue;
		}
		else if (_Value > _MaxValue)
		{
			Value = _MaxValue;
		}
	}

	protected override bool AcceptKeyPress(KeyPressEventArgs e)
	{
		string numberDecimalSeparator = NumberDecimalSeparator;
		if (e.KeyChar.ToString() == numberDecimalSeparator)
		{
			if (!base.InputStack.Contains(numberDecimalSeparator))
			{
				return true;
			}
			return false;
		}
		return base.AcceptKeyPress(e);
	}

	protected override string ProcessNewInputStack(string s)
	{
		if (base.InputStack == "0" && s != "0" && s.StartsWith("0") && s.EndsWith(NumberDecimalSeparator))
		{
			return s;
		}
		return base.ProcessNewInputStack(s);
	}

	protected override void OnInputStackChanged()
	{
		if (base.InputStack.Length > 0)
		{
			IsEmpty = false;
			if (base.InputStack == "-")
			{
				SetValue(0.0, raiseValueChanged: true);
				return;
			}
			if (base.InputStack == NumberDecimalSeparator || base.InputStack == "-" + NumberDecimalSeparator)
			{
				SetValue(0.0, raiseValueChanged: true);
				return;
			}
			SetValue(Parse(base.InputStack), raiseValueChanged: true);
		}
		else
		{
			if (base.AllowEmptyState)
			{
				IsEmpty = true;
			}
			SetValue(0.0);
		}
		base.OnInputStackChanged();
	}

	private double Parse(string s)
	{
		CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
		if (activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider provider)
		{
			return double.Parse(s, provider);
		}
		return double.Parse(s);
	}

	protected override void OnInputKeyAccepted()
	{
		CheckInputComplete(sendNotification: true);
		base.OnInputKeyAccepted();
	}

	private void CheckInputComplete(bool sendNotification)
	{
		string text = "";
		string s = "";
		if (base.InputStack.Length > 0)
		{
			if (base.InputStack.Contains(NumberDecimalSeparator))
			{
				text = base.InputStack + "1";
				s = text;
			}
			else
			{
				text = base.InputStack + NumberDecimalSeparator;
				s = base.InputStack + "0";
			}
		}
		if (!(base.InputStack == "-") && !base.InputStack.StartsWith(NumberDecimalSeparator) && !(base.InputStack == "-" + NumberDecimalSeparator) && (_Value == _MaxValue || (!ValidateNewInputStack(text) && !ValidateNewInputStack(s))))
		{
			InputComplete(sendNotification);
		}
	}

	private void SetValue(double i, bool raiseValueChanged)
	{
		bool flag = _Value != i || raiseValueChanged;
		_Value = i;
		if (flag)
		{
			OnValueChanged();
		}
		InvalidateArrange();
	}

	private void SetValue(double i)
	{
		SetValue(i, raiseValueChanged: false);
	}

	protected override string GetMeasureString()
	{
		string result = GetRenderString();
		if (IsEmpty && base.AllowEmptyState)
		{
			result = "T";
		}
		else if (base.InputStack == "-" && _Value == 0.0)
		{
			result = "-";
		}
		else if ((base.InputStack == NumberDecimalSeparator || base.InputStack == "-" + NumberDecimalSeparator) && _Value == 0.0)
		{
			return base.InputStack;
		}
		return result;
	}

	protected override string GetRenderString()
	{
		if (base.InputStack == "-" && _Value == 0.0)
		{
			return "-";
		}
		if ((base.InputStack == NumberDecimalSeparator || base.InputStack == "-" + NumberDecimalSeparator) && _Value == 0.0)
		{
			return base.InputStack;
		}
		if (base.IsFocused && base.InputStack.Length > 0)
		{
			return base.InputStack;
		}
		string text = "";
		return ConvertToString(_Value, useFormat: true);
	}

	protected override void NegateValue()
	{
		if (!(_MaxValue < 0.0))
		{
			double d = 0.0 - _Value;
			SetValueDirect(FormatNumber(d));
		}
	}

	protected override void ResetValue()
	{
		double num = 0.0;
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

	private bool TryParse(string s, out double value)
	{
		CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
		if (activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider provider)
		{
			return double.TryParse(s, NumberStyles.Number, provider, out value);
		}
		return double.TryParse(s, out value);
	}

	protected override void OnClipboardPaste()
	{
		if (Clipboard.ContainsText())
		{
			string p = Clipboard.GetText().Replace(" ", "");
			p = StripNonNumeric(p);
			double value = 0.0;
			if (TryParse(p, out value) && !(value > _MaxValue) && !(value < _MinValue) && SetInputStack(value.ToString()))
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

	protected override bool ValidateNewInputStack(string s)
	{
		if (s.Length > 0)
		{
			if (s == "-" && base.AllowsNegativeValue)
			{
				return true;
			}
			if (s == NumberDecimalSeparator || s == "-" + NumberDecimalSeparator)
			{
				return true;
			}
			double value = 0.0;
			if (TryParse(s, out value))
			{
				if ((value > _MaxValue && _MaxValue >= 0.0) || (value < _MinValue && _MinValue < 0.0))
				{
					return false;
				}
				string text = ConvertToString(value, useFormat: true, forceDisplayFormat: true);
				if (s.Contains(NumberDecimalSeparator) && text.Contains(NumberDecimalSeparator))
				{
					int numberOfDecimalPlaces = GetNumberOfDecimalPlaces();
					int length = s.Substring(s.IndexOf(NumberDecimalSeparator) + 1).Length;
					if (numberOfDecimalPlaces > 0 && length > numberOfDecimalPlaces)
					{
						return false;
					}
				}
				else if (s.Contains(NumberDecimalSeparator) && GetNumberOfDecimalPlaces() == 0)
				{
					return false;
				}
				return true;
			}
			return false;
		}
		return base.ValidateNewInputStack(s);
	}

	private int GetNumberOfDecimalPlaces()
	{
		if (DisplayFormat == null || DisplayFormat.Length == 0)
		{
			CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
			if (activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider formatProvider && formatProvider is NumberFormatInfo)
			{
				NumberFormatInfo numberFormatInfo = (NumberFormatInfo)formatProvider;
				return numberFormatInfo.NumberDecimalDigits;
			}
			return 2;
		}
		string text = ConvertToString(11.111111111111, useFormat: true, forceDisplayFormat: true);
		int num = text.IndexOf(NumberDecimalSeparator);
		if (num < 0)
		{
			return 0;
		}
		return text.Substring(num + 1).Length;
	}

	public override void IncreaseValue()
	{
		double num = _Value + _Increment;
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
				num = Math.Max(0.0, _MinValue);
			}
			SetValueDirect(StripNonNumeric(FormatNumber(num)));
			CheckInputComplete(sendNotification: false);
		}
		base.IncreaseValue();
	}

	public override void DecreaseValue()
	{
		double num = _Value - _Increment;
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
			SetValueDirect(StripNonNumeric(FormatNumber(num)));
			CheckInputComplete(sendNotification: false);
		}
		base.DecreaseValue();
	}

	private void SetValueDirect(string s)
	{
		string text = s;
		if (s.Contains(NumberDecimalSeparator) && text.Contains(NumberDecimalSeparator))
		{
			int numberOfDecimalPlaces = GetNumberOfDecimalPlaces();
			int length = s.Substring(s.IndexOf(NumberDecimalSeparator) + 1).Length;
			if (numberOfDecimalPlaces > 0 && length > numberOfDecimalPlaces)
			{
				s = s.Substring(0, s.IndexOf(NumberDecimalSeparator) + 1 + numberOfDecimalPlaces);
			}
		}
		if (SetInputStack(s))
		{
			SetInputPosition(base.InputStack.Length);
		}
	}

	private string ConvertToString(double d)
	{
		return ConvertToString(d, useFormat: false, forceDisplayFormat: false);
	}

	private string ConvertToString(double d, bool useFormat)
	{
		return ConvertToString(d, useFormat, forceDisplayFormat: false);
	}

	private string ConvertToString(double d, bool useFormat, bool forceDisplayFormat)
	{
		string text = FormatNumber(d);
		if (!forceDisplayFormat && (_DisplayFormat.Length == 0 || base.IsFocused || !useFormat))
		{
			if (base.InputStack.Contains(NumberDecimalSeparator))
			{
				if (!text.Contains(NumberDecimalSeparator))
				{
					text += base.InputStack.Substring(base.InputStack.IndexOf(NumberDecimalSeparator));
				}
				else if (text.Substring(text.IndexOf(NumberDecimalSeparator)) != base.InputStack.Substring(base.InputStack.IndexOf(NumberDecimalSeparator)))
				{
					text = text.Substring(0, text.IndexOf(NumberDecimalSeparator)) + base.InputStack.Substring(base.InputStack.IndexOf(NumberDecimalSeparator));
				}
			}
		}
		else if (_DisplayFormat.Length > 0)
		{
			try
			{
				CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
				text = ((!(activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider formatProvider)) ? d.ToString(_DisplayFormat) : d.ToString(_DisplayFormat, formatProvider));
			}
			catch
			{
				text = d.ToString();
			}
		}
		return text;
	}

	private string FormatNumber(double d)
	{
		string text = null;
		CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
		if (!string.IsNullOrEmpty(DisplayFormat) && DisplayFormat.ToLower() != "p" && base.IsFocused)
		{
			text = DisplayFormat;
		}
		else
		{
			text = "n";
			int numberDecimalDigits = activeCulture.NumberFormat.NumberDecimalDigits;
			if (numberDecimalDigits > 0)
			{
				text += numberDecimalDigits;
			}
		}
		if (activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider formatProvider)
		{
			if (text != null)
			{
				return d.ToString(text, formatProvider);
			}
			return d.ToString(formatProvider);
		}
		if (text != null)
		{
			return d.ToString(text);
		}
		return d.ToString();
	}

	private int GetDecimalPlaces(double d)
	{
		double num = d - Math.Truncate(d);
		if (num == 0.0)
		{
			return 0;
		}
		int num2 = 0;
		while (num < 1.0)
		{
			num *= 10.0;
			num2++;
		}
		return num2;
	}

	private string StripNonNumeric(string p)
	{
		string text = "";
		for (int i = 0; i < p.Length; i++)
		{
			if (p[i].ToString() == NumberDecimalSeparator || (p[i] >= '0' && p[i] <= '9') || (i == 0 && p[i] == '-'))
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

	protected override string GetInputStringValue()
	{
		return ConvertToString(_Value);
	}
}

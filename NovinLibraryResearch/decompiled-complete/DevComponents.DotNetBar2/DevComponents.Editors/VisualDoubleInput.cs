using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.Editors;

public class VisualDoubleInput : VisualNumericInput
{
	private double _ᗸ;

	private double __234A = double.MinValue;

	private double __234B = double.MaxValue;

	private double __234C = 1.0;

	private string __22BD = "";

	public double MinValue
	{
		get
		{
			return __234A;
		}
		set
		{
			if (__234A != value)
			{
				__234A = value;
				if (__234A >= 0.0)
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
			return __234B;
		}
		set
		{
			if (__234B != value)
			{
				__234B = value;
				ValidateValue();
			}
		}
	}

	[DefaultValue(1)]
	public double Increment
	{
		get
		{
			return __234C;
		}
		set
		{
			value = Math.Abs(value);
			if (__234C != value)
			{
				__234C = value;
			}
		}
	}

	public double Value
	{
		get
		{
			if (_ᗸ < __234A)
			{
				return __234A;
			}
			if (_ᗸ > __234B)
			{
				return __234B;
			}
			return _ᗸ;
		}
		set
		{
			if (value < __234A)
			{
				value = __234A;
			}
			if (value > __234B)
			{
				value = __234B;
			}
			if (base.IsFocused)
			{
				_2353(ៗ(_2352(value)));
				if (base.IsFocused)
				{
					InputComplete(sendNotification: false);
				}
			}
			else
			{
				IsEmpty = false;
				ᘩ(value, _2351: true);
			}
		}
	}

	[DefaultValue("")]
	public string DisplayFormat
	{
		get
		{
			return __22BD;
		}
		set
		{
			if (value != null)
			{
				__22BD = value;
			}
		}
	}

	private string _17CE => DateTimeInput.GetActiveCulture().NumberFormat.NumberDecimalSeparator;

	private string _2349 => DateTimeInput.GetActiveCulture().NumberFormat.NumberGroupSeparator;

	[Browsable(false)]
	public virtual string Text
	{
		get
		{
			if (IsEmpty && base.AllowEmptyState)
			{
				return "";
			}
			return _2354(_ᗸ, _2355: true, _29C8: true);
		}
	}

	protected override void OnLostFocus()
	{
		ValidateValue();
		base.OnLostFocus();
	}

	protected virtual void ValidateValue()
	{
		if (_ᗸ < __234A)
		{
			Value = __234A;
		}
		else if (_ᗸ > __234B)
		{
			Value = __234B;
		}
	}

	protected override bool AcceptKeyPress(KeyPressEventArgs e)
	{
		string text = _17CE;
		if (e.KeyChar.ToString() == text)
		{
			if (!base.InputStack.Contains(text))
			{
				return true;
			}
			return false;
		}
		return base.AcceptKeyPress(e);
	}

	protected override string ProcessNewInputStack(string s)
	{
		if (base.InputStack == "0" && s != "0" && s.StartsWith("0") && s.EndsWith(_17CE))
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
				ᘩ(0.0, _2351: true);
				return;
			}
			if (base.InputStack == _17CE || base.InputStack == "-" + _17CE)
			{
				ᘩ(0.0, _2351: true);
				return;
			}
			ᘩ(ሕ(base.InputStack), _2351: true);
		}
		else
		{
			if (base.AllowEmptyState)
			{
				IsEmpty = true;
			}
			ᘩ(0.0);
		}
		base.OnInputStackChanged();
	}

	private double ሕ(string ٮ)
	{
		CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
		if (activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider provider)
		{
			return double.Parse(ٮ, provider);
		}
		return double.Parse(ٮ);
	}

	protected override void OnInputKeyAccepted()
	{
		_234D(_234E: true);
		base.OnInputKeyAccepted();
	}

	private void _234D(bool _234E)
	{
		string text = "";
		string s = "";
		if (base.InputStack.Length > 0)
		{
			if (base.InputStack.Contains(_17CE))
			{
				text = base.InputStack + "1";
				s = text;
			}
			else
			{
				text = base.InputStack + _17CE;
				s = base.InputStack + "0";
			}
		}
		if (!(base.InputStack == "-") && !base.InputStack.StartsWith(_17CE) && !(base.InputStack == "-" + _17CE) && (_ᗸ == __234B || (!ValidateNewInputStack(text) && !ValidateNewInputStack(s))))
		{
			InputComplete(_234E);
		}
	}

	private void ᘩ(double _2350, bool _2351)
	{
		bool flag = _ᗸ != _2350 || _2351;
		_ᗸ = _2350;
		if (flag)
		{
			OnValueChanged();
		}
		InvalidateArrange();
	}

	private void ᘩ(double _2350)
	{
		ᘩ(_2350, _2351: false);
	}

	protected override string GetMeasureString()
	{
		string result = GetRenderString();
		if (IsEmpty && base.AllowEmptyState)
		{
			result = "T";
		}
		else if (base.InputStack == "-" && _ᗸ == 0.0)
		{
			result = "-";
		}
		else if ((base.InputStack == _17CE || base.InputStack == "-" + _17CE) && _ᗸ == 0.0)
		{
			return base.InputStack;
		}
		return result;
	}

	protected override string GetRenderString()
	{
		if (base.InputStack == "-" && _ᗸ == 0.0)
		{
			return "-";
		}
		if ((base.InputStack == _17CE || base.InputStack == "-" + _17CE) && _ᗸ == 0.0)
		{
			return base.InputStack;
		}
		if (base.IsFocused && base.InputStack.Length > 0)
		{
			return base.InputStack;
		}
		string text = "";
		return _2354(_ᗸ, _2355: true);
	}

	protected override void NegateValue()
	{
		if (!(__234B < 0.0))
		{
			double num = 0.0 - _ᗸ;
			_2353(_2352(num));
		}
	}

	protected override void ResetValue()
	{
		ResetInputStack();
		double num = 0.0;
		if (__234A > num)
		{
			num = __234A;
		}
		if (num > __234B)
		{
			num = __234B;
		}
		ᘩ(num);
		InvalidateArrange();
	}

	private bool _29C6(string ٮ, out double ԝ)
	{
		CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
		if (activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider provider)
		{
			return double.TryParse(ٮ, NumberStyles.Number, provider, out ԝ);
		}
		return double.TryParse(ٮ, out ԝ);
	}

	protected override void OnClipboardPaste()
	{
		if (Clipboard.ContainsText())
		{
			string text = Clipboard.GetText().Replace(" ", "");
			text = ៗ(text);
			double ԝ = 0.0;
			if (_29C6(text, out ԝ) && !(ԝ > __234B) && !(ԝ < __234A) && SetInputStack(ԝ.ToString()))
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
			if (s == _17CE || s == "-" + _17CE)
			{
				return true;
			}
			double ԝ = 0.0;
			if (_29C6(s, out ԝ))
			{
				if ((ԝ > __234B && __234B >= 0.0) || (ԝ < __234A && __234A < 0.0))
				{
					return false;
				}
				string text = _2354(ԝ, _2355: true, _29C8: true);
				if (s.Contains(_17CE) && text.Contains(_17CE))
				{
					int num = _29C7();
					int length = s.Substring(s.IndexOf(_17CE) + 1).Length;
					if (num > 0 && length > num)
					{
						return false;
					}
				}
				else if (s.Contains(_17CE) && _29C7() == 0)
				{
					return false;
				}
				return true;
			}
			return false;
		}
		return base.ValidateNewInputStack(s);
	}

	private int _29C7()
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
		string text = _2354(11.111111111111, _2355: true, _29C8: true);
		int num = text.IndexOf(_17CE);
		if (num < 0)
		{
			return 0;
		}
		return text.Substring(num + 1).Length;
	}

	public override void IncreaseValue()
	{
		if (__234C > 0.0)
		{
			double num = _ᗸ + __234C;
			if (num > __234B)
			{
				num = __234B;
			}
			else if (num < __234A)
			{
				num = __234A;
			}
			if (_ᗸ < __234B && num > __234B)
			{
				num = __234B;
			}
			if (num <= __234B)
			{
				if (IsEmpty && base.AllowEmptyState)
				{
					num = Math.Max(0.0, __234A);
				}
				_2353(ៗ(_2352(num)));
				_234D(_234E: false);
			}
		}
		base.IncreaseValue();
	}

	public override void DecreaseValue()
	{
		if (__234C > 0.0)
		{
			double num = _ᗸ - __234C;
			if (num > __234B)
			{
				num = __234B;
			}
			else if (num < __234A)
			{
				num = __234A;
			}
			if (_ᗸ > __234A && num < __234A)
			{
				num = __234A;
			}
			if (num >= __234A)
			{
				_2353(ៗ(_2352(num)));
				_234D(_234E: false);
			}
		}
		base.DecreaseValue();
	}

	private void _2353(string ٮ)
	{
		string text = ٮ;
		if (ٮ.Contains(_17CE) && text.Contains(_17CE))
		{
			int num = _29C7();
			int length = ٮ.Substring(ٮ.IndexOf(_17CE) + 1).Length;
			if (num > 0 && length > num)
			{
				ٮ = ٮ.Substring(0, ٮ.IndexOf(_17CE) + 1 + num);
			}
		}
		if (SetInputStack(ٮ))
		{
			SetInputPosition(base.InputStack.Length);
		}
	}

	private string _2354(double _0CC8)
	{
		return _2354(_0CC8, _2355: false, _29C8: false);
	}

	private string _2354(double _0CC8, bool _2355)
	{
		return _2354(_0CC8, _2355, _29C8: false);
	}

	private string _2354(double _0CC8, bool _2355, bool _29C8)
	{
		string text = _2352(_0CC8);
		if (!_29C8 && (__22BD.Length == 0 || base.IsFocused || !_2355))
		{
			if (base.InputStack.Contains(_17CE))
			{
				if (!text.Contains(_17CE))
				{
					text += base.InputStack.Substring(base.InputStack.IndexOf(_17CE));
				}
				else if (text.Substring(text.IndexOf(_17CE)) != base.InputStack.Substring(base.InputStack.IndexOf(_17CE)))
				{
					text = text.Substring(0, text.IndexOf(_17CE)) + base.InputStack.Substring(base.InputStack.IndexOf(_17CE));
				}
			}
		}
		else if (__22BD.Length > 0)
		{
			try
			{
				CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
				text = ((!(activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider formatProvider)) ? _0CC8.ToString(__22BD) : _0CC8.ToString(__22BD, formatProvider));
			}
			catch
			{
				text = _0CC8.ToString();
			}
		}
		return text;
	}

	private string _2352(double _0CC8)
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
				return _0CC8.ToString(text, formatProvider);
			}
			return _0CC8.ToString(formatProvider);
		}
		if (text != null)
		{
			return _0CC8.ToString(text);
		}
		return _0CC8.ToString();
	}

	private int _29C9(double _0CC8)
	{
		double num = _0CC8 - Math.Truncate(_0CC8);
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

	private string ៗ(string _065C)
	{
		string text = "";
		for (int i = 0; i < _065C.Length; i++)
		{
			if (_065C[i].ToString() == _17CE || (_065C[i] >= '0' && _065C[i] <= '9') || (i == 0 && _065C[i] == '-'))
			{
				text += _065C[i];
			}
			else if (text.Length > 0 && _065C[i].ToString() != _2349)
			{
				break;
			}
		}
		return text;
	}

	protected override string GetInputStringValue()
	{
		return _2354(_ᗸ);
	}
}

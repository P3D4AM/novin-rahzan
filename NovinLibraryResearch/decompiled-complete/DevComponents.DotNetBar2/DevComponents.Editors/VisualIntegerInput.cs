using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.Editors;

public class VisualIntegerInput : VisualNumericInput
{
	private int _ᗸ;

	private int __234A = int.MinValue;

	private int __234B = int.MaxValue;

	private int __234C = 1;

	private string __22BD = "";

	public int MinValue
	{
		get
		{
			return __234A;
		}
		set
		{
			if (__234A == value)
			{
				return;
			}
			__234A = value;
			if (__234A >= 0)
			{
				base.AllowsNegativeValue = false;
			}
			else
			{
				base.AllowsNegativeValue = true;
			}
			if (_ᗸ < __234A && (!IsEmpty || (IsEmpty && !base.AllowEmptyState)))
			{
				if (base.IsFocused)
				{
					_2353(_2352(__234A));
				}
				else
				{
					ᘩ(__234A);
				}
			}
		}
	}

	public int MaxValue
	{
		get
		{
			return __234B;
		}
		set
		{
			if (__234B == value)
			{
				return;
			}
			__234B = value;
			if (_ᗸ > __234B && (!IsEmpty || (IsEmpty && !base.AllowEmptyState)))
			{
				if (base.IsFocused)
				{
					_2353(_2352(__234B));
				}
				else
				{
					ᘩ(__234B);
				}
			}
		}
	}

	private string _17CE => DateTimeInput.GetActiveCulture().NumberFormat.NumberDecimalSeparator;

	private string _2349 => DateTimeInput.GetActiveCulture().NumberFormat.NumberGroupSeparator;

	[DefaultValue(1)]
	public int Increment
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

	[Browsable(false)]
	public virtual string Text
	{
		get
		{
			if (IsEmpty && base.AllowEmptyState)
			{
				return "";
			}
			return _2354(_ᗸ, _2355: true, _2356: true);
		}
	}

	public virtual int Value
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
			_2353(_2354(value));
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

	protected override void OnInputStackChanged()
	{
		if (base.InputStack.Length > 0)
		{
			IsEmpty = false;
			if (base.InputStack == "-")
			{
				ᘩ(0, _2351: true);
				return;
			}
			int _2350 = int.Parse(base.InputStack);
			ᘩ(_2350, _2351: true);
		}
		else
		{
			if (base.AllowEmptyState)
			{
				IsEmpty = true;
			}
			ᘩ(0, _2351: true);
		}
		base.OnInputStackChanged();
	}

	protected override void OnInputKeyAccepted()
	{
		_234D(_234E: true);
		base.OnInputKeyAccepted();
	}

	private void _234D(bool _234E)
	{
		int num = _234F(_ᗸ);
		if (_ᗸ == __234B || (_2354(_ᗸ).Length >= _2354(__234B).Length && _2354(_ᗸ).Length >= _2354(__234A).Length) || num > __234B)
		{
			InputComplete(_234E);
		}
		else if (IsLeadingZero && _2354(_ᗸ).Length + 1 >= _2354(__234B).Length)
		{
			InputComplete(_234E);
		}
	}

	private int _234F(int ԝ)
	{
		string text = "";
		text = ((ԝ != 0) ? (_2354(ԝ) + "0") : "1");
		int result = ԝ;
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
		if (_ᗸ < __234A && !IsEmpty)
		{
			Value = __234A;
		}
		else if (_ᗸ > __234B && !IsEmpty)
		{
			Value = __234B;
		}
	}

	private void ᘩ(int _2350, bool _2351)
	{
		bool flag = _ᗸ != _2350 || _2351;
		_ᗸ = _2350;
		if (flag)
		{
			OnValueChanged();
		}
		InvalidateArrange();
	}

	private void ᘩ(int _2350)
	{
		ᘩ(_2350, _2351: false);
	}

	protected override string GetMeasureString()
	{
		string result = _2354(_ᗸ, _2355: true);
		if (IsEmpty && base.AllowEmptyState)
		{
			result = "T";
		}
		else if (base.InputStack == "-" && _ᗸ == 0)
		{
			result = "-";
		}
		return result;
	}

	protected override string GetRenderString()
	{
		if (base.InputStack == "-" && _ᗸ == 0)
		{
			return "-";
		}
		if (IsEmpty && base.AllowEmptyState)
		{
			return "";
		}
		return _2354(_ᗸ, _2355: true);
	}

	protected override void NegateValue()
	{
		if (!IsEmpty && __234B >= 0)
		{
			int _2350 = -_ᗸ;
			_2353(_2352(_2350));
		}
	}

	protected override void ResetValue()
	{
		ResetInputStack();
		int num = 0;
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

	private string _2352(int _2350)
	{
		CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
		if (activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider formatProvider)
		{
			return _2350.ToString(formatProvider);
		}
		return _2350.ToString();
	}

	protected override void OnClipboardPaste()
	{
		if (Clipboard.ContainsText())
		{
			string s = ៗ(Clipboard.GetText().Replace(" ", ""));
			int result = 0;
			if (int.TryParse(s, out result) && result <= __234B && result >= __234A && SetInputStack(result.ToString()))
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

	private string ៗ(string _065C)
	{
		string text = "";
		for (int i = 0; i < _065C.Length && !(_065C[i].ToString() == _17CE); i++)
		{
			if ((_065C[i] >= '0' && _065C[i] <= '9') || (i == 0 && _065C[i] == '-'))
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
				if ((result > __234B && __234B >= 0) || (result < __234A && __234A < 0))
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
		if (__234C > 0)
		{
			int num = _ᗸ + __234C;
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
					num = Math.Max(0, __234A);
				}
				_2353(_2352(num));
				_234D(_234E: false);
			}
		}
		base.IncreaseValue();
	}

	public override void DecreaseValue()
	{
		if (__234C > 0)
		{
			int num = _ᗸ - __234C;
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
				_2353(_2352(num));
				_234D(_234E: false);
			}
		}
		base.DecreaseValue();
	}

	private void _2353(string ٮ)
	{
		if (SetInputStack(ٮ))
		{
			SetInputPosition(base.InputStack.Length);
		}
	}

	private string _2354(int _2350)
	{
		return _2354(_2350, _2355: false);
	}

	private string _2354(int _2350, bool _2355)
	{
		return _2354(_2350, _2355, _2356: false);
	}

	private string _2354(int _2350, bool _2355, bool _2356)
	{
		if ((!_2356 && (!_2355 || base.IsFocused)) || __22BD.Length == 0)
		{
			return _2352(_2350);
		}
		CultureInfo activeCulture = DateTimeInput.GetActiveCulture();
		if (activeCulture.GetFormat(typeof(NumberFormatInfo)) is IFormatProvider formatProvider)
		{
			return _2350.ToString(__22BD, formatProvider);
		}
		return _2350.ToString(__22BD);
	}

	protected override string GetInputStringValue()
	{
		return _2354(_ᗸ);
	}
}

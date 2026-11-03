using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.CurrencyTextBox;

public class TextBoxFa : TextBox
{
	public enum ENShowHorofi
	{
		AdadiFormatN0,
		Horofi,
		Adadi,
		HorofiBaRial,
		None
	}

	public delegate void CurrencyKeyDownEventHandler(object Sender, KeyEventArgs e);

	public delegate void TextChangeEventHandler(object Sender, EventArgs e);

	private Font _toolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);

	private bool _allowEnpty;

	private bool _getColoresFromSettingDLL;

	private bool _sendTabAfterEnter;

	private bool _sendTabAfterUP;

	private bool _sendShiftTabAfterDown;

	private bool _sendShiftTabAfterEnter;

	private bool _handleKeydown;

	private bool _showToolTipText;

	private bool _mouiseIn;

	private ENShowHorofi _showHorofi;

	private int _tootTipDelay;

	private Color _backcolorTooltip;

	private Color _foreColorTooltip;

	private Color _backColorTextBoxWhenFocus;

	private Color _backColorTextBoxWhenLostFocus;

	private string _currencyFormat;

	private string _toolTipPostFix = "ریال";

	[field: AccessedThroughProperty("Label1")]
	internal Label Label1
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	[field: AccessedThroughProperty("TT")]
	private ToolTip TT
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	public Font ToolTipFont
	{
		get
		{
			return _toolTipFont;
		}
		set
		{
			_toolTipFont = value;
		}
	}

	public string ToolTipPostFix
	{
		get
		{
			return _toolTipPostFix;
		}
		set
		{
			_toolTipPostFix = value;
		}
	}

	public bool AllowEmpty
	{
		get
		{
			return _allowEnpty;
		}
		set
		{
			_allowEnpty = value;
		}
	}

	public override string Text
	{
		get
		{
			return ((TextBox)this).Text;
		}
		set
		{
			decimal num = value?.ParseToDecimal(0m, DigitAfterPoint) ?? 0m;
			if (num > 1000000000000000m || num < -1000000000000000m)
			{
				((TextBox)this).Text = "OverFlow!";
			}
			else if (!value.EndsWith(PacketParser.TextManager.Instance.DecimalPoint) && !value.Contains(PacketParser.TextManager.Instance.DecimalPoint) && !value.Contains("/") && !value.EndsWith("/"))
			{
				((TextBox)this).Text = num.NpToString("N0", 0);
			}
			else
			{
				((TextBox)this).Text = num.ToString();
			}
		}
	}

	public bool ShowToolTipText
	{
		get
		{
			return _showToolTipText;
		}
		set
		{
			_showToolTipText = value;
		}
	}

	public string CurrencyFormat
	{
		get
		{
			return _currencyFormat;
		}
		set
		{
			_currencyFormat = value;
		}
	}

	public bool GetColoresFromSettingDLL
	{
		get
		{
			return _getColoresFromSettingDLL;
		}
		set
		{
			_getColoresFromSettingDLL = value;
		}
	}

	public bool HandleKeydown
	{
		get
		{
			return _handleKeydown;
		}
		set
		{
			_handleKeydown = value;
		}
	}

	public Color BackColorTextBoxWhenLostFocus
	{
		get
		{
			return _backColorTextBoxWhenLostFocus;
		}
		set
		{
			_backColorTextBoxWhenLostFocus = value;
		}
	}

	public Color BackColorTextBoxWhenFocus
	{
		get
		{
			return _backColorTextBoxWhenFocus;
		}
		set
		{
			_backColorTextBoxWhenFocus = value;
		}
	}

	public Color BackcolorTooltip
	{
		get
		{
			return _backcolorTooltip;
		}
		set
		{
			_backcolorTooltip = value;
		}
	}

	public Color ForeColorTooltip
	{
		get
		{
			return _foreColorTooltip;
		}
		set
		{
			_foreColorTooltip = value;
		}
	}

	public int TootTipDelay
	{
		get
		{
			return _tootTipDelay;
		}
		set
		{
			_tootTipDelay = value;
		}
	}

	public bool SendTabAfterEnter
	{
		get
		{
			return _sendTabAfterEnter;
		}
		set
		{
			_sendTabAfterEnter = value;
		}
	}

	public bool SendTabAfterUP
	{
		get
		{
			return _sendTabAfterUP;
		}
		set
		{
			_sendTabAfterUP = value;
		}
	}

	public bool SendShiftTabAfterDown
	{
		get
		{
			return _sendShiftTabAfterDown;
		}
		set
		{
			_sendShiftTabAfterDown = value;
		}
	}

	public bool SendShiftTabAfterEnter
	{
		get
		{
			return _sendShiftTabAfterEnter;
		}
		set
		{
			_sendShiftTabAfterEnter = value;
		}
	}

	public ENShowHorofi ShowHorofi
	{
		get
		{
			return _showHorofi;
		}
		set
		{
			_showHorofi = value;
		}
	}

	public decimal Value
	{
		get
		{
			return ((TextBox)this).Text?.ParseToDecimal(0m, DigitAfterPoint) ?? 0m;
		}
		set
		{
			if (value > 1000000000000000m)
			{
				throw new IndexOutOfRangeException();
			}
			if (value < -1000000000000000m)
			{
				throw new IndexOutOfRangeException();
			}
			((Control)this).Text = value.NpToString("", 0);
		}
	}

	public static short DigitAfterPoint { get; set; }

	public event CurrencyKeyDownEventHandler CurrencyKeyDown;

	public event EventHandler ValueChanged;

	private void ShowTolltip()
	{
		try
		{
			TT.BackColor = _backcolorTooltip;
			TT.Active = true;
			string text = _showHorofi switch
			{
				ENShowHorofi.Adadi => Value + ToolTipPostFix, 
				ENShowHorofi.AdadiFormatN0 => Value.NpToString("N0", 0) + ToolTipPostFix, 
				ENShowHorofi.Horofi => Value.AdadToHoroof() + ToolTipPostFix, 
				ENShowHorofi.HorofiBaRial => Value.AdadToHoroof() + ToolTipPostFix, 
				_ => "", 
			};
			TT.RemoveAll();
			if (_showToolTipText && ((Control)this).Focused)
			{
				TT.Show(text, (IWin32Window)(object)this, 0, ((Control)this).Height, _tootTipDelay);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowTolltip", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 124);
		}
	}

	private void DisplayBig()
	{
		try
		{
			((Control)Label1).Text = ((TextBox)this).Text;
			((Control)Label1).Refresh();
			((Control)Label1).Left = ((Control)this).Left;
			((Control)Label1).Top = checked(((Control)this).Top - ((Control)Label1).Height + 10);
			((Control)this).Parent.Controls.Add((Control)(object)Label1);
			if (((Control)Label1).Top < 0)
			{
				((Control)Label1).Top = ((Control)this).Bottom;
			}
			((Control)Label1).Visible = true;
			((Control)Label1).Show();
			((Control)Label1).BringToFront();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisplayBig", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 140);
		}
	}

	private void HideBig()
	{
		((Control)Label1).Visible = false;
	}

	private int GetDecimalPlaces()
	{
		try
		{
			if (((Control)this).Text.Contains("."))
			{
				return ((Control)this).Text.Split(new char[1] { '.' })[1].Length;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDecimalPlaces", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 150);
		}
		return 0;
	}

	public TextBoxFa()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Expected O, but got Unknown
		try
		{
			((Control)this).GotFocus += TextBox1_GotFocus;
			((Control)this).KeyDown += new KeyEventHandler(TextBox1_KeyDown);
			((Control)this).LostFocus += TextBox1_LostFocus;
			((Control)this).MouseEnter += TextBox1_MouseEnter;
			((Control)this).MouseLeave += TextBox1_MouseLeave;
			((Control)this).TextChanged += TextBox1_TextChanged;
			_mouiseIn = false;
			Label1 = new Label();
			_allowEnpty = true;
			_getColoresFromSettingDLL = true;
			_sendTabAfterEnter = true;
			_sendTabAfterUP = true;
			_sendShiftTabAfterDown = true;
			_sendShiftTabAfterEnter = true;
			_showHorofi = ENShowHorofi.HorofiBaRial;
			_tootTipDelay = 2000;
			_backcolorTooltip = Color.Red;
			_foreColorTooltip = Color.Black;
			_backColorTextBoxWhenFocus = Color.Yellow;
			_backColorTextBoxWhenLostFocus = Color.White;
			_handleKeydown = true;
			_currencyFormat = "";
			_showToolTipText = true;
			_toolTipPostFix = "";
			((Control)Label1).Font = ToolTipFont;
			((Control)Label1).BackColor = Color.Transparent;
			((Control)Label1).ForeColor = Color.Red;
			((Control)Label1).AutoSize = true;
			TT = new ToolTip();
			((TextBoxBase)this).MaxLength = 19;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 190);
		}
	}

	private void TextBox1_GotFocus(object sender, EventArgs e)
	{
		try
		{
			ShowTolltip();
			if (_getColoresFromSettingDLL)
			{
				((Control)this).BackColor = Color.Yellow;
			}
			else
			{
				((Control)this).BackColor = _backColorTextBoxWhenFocus;
			}
			((TextBoxBase)this).SelectAll();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TextBox1_GotFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 202);
		}
	}

	private void TextBox1_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Invalid comparison between Unknown and I4
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Invalid comparison between Unknown and I4
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Invalid comparison between Unknown and I4
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Invalid comparison between Unknown and I4
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Invalid comparison between Unknown and I4
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Invalid comparison between Unknown and I4
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Invalid comparison between Unknown and I4
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Invalid comparison between Unknown and I4
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Invalid comparison between Unknown and I4
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Invalid comparison between Unknown and I4
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Invalid comparison between Unknown and I4
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Invalid comparison between Unknown and I4
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Invalid comparison between Unknown and I4
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Invalid comparison between Unknown and I4
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Invalid comparison between Unknown and I4
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Invalid comparison between Unknown and I4
		try
		{
			if ((e.Control && (int)e.KeyCode == 86) || (e.Control && (int)e.KeyCode == 67) || (e.Control && (int)e.KeyCode == 88))
			{
				return;
			}
			if ((((int)e.KeyCode >= 48) & ((int)e.KeyCode <= 57)) | (((int)e.KeyCode >= 96) & ((int)e.KeyCode <= 105)))
			{
				e.SuppressKeyPress = false;
			}
			else if ((int)e.KeyCode == 36)
			{
				((TextBoxBase)this).SelectionStart = 0;
			}
			else if ((int)e.KeyCode == 35)
			{
				((TextBoxBase)this).SelectionStart = 999;
			}
			else
			{
				if ((int)e.KeyCode == 46)
				{
					if (((Control)this).Text.Length > ((TextBoxBase)this).SelectionStart && ((Control)this).Text.Substring(((TextBoxBase)this).SelectionStart, 1) == ",")
					{
						int selectionStart = ((TextBoxBase)this).SelectionStart;
						((TextBoxBase)this).SelectionStart = selectionStart + 1;
					}
					e.SuppressKeyPress = false;
					return;
				}
				if ((int)e.KeyCode == 8)
				{
					if (((TextBoxBase)this).SelectionStart > 0 && ((Control)this).Text.Substring(((TextBoxBase)this).SelectionStart - 1, 1) == ",")
					{
						int selectionStart = ((TextBoxBase)this).SelectionStart;
						((TextBoxBase)this).SelectionStart = selectionStart - 1;
					}
					e.SuppressKeyPress = false;
					return;
				}
				if ((int)e.KeyCode == 38)
				{
					if (_sendTabAfterUP)
					{
						SendKeys.Send("{Tab}");
					}
				}
				else if ((int)e.KeyCode == 40)
				{
					if (_sendShiftTabAfterDown)
					{
						SendKeys.Send("+{Tab}");
					}
				}
				else if ((int)e.KeyCode == 13)
				{
					if (_sendShiftTabAfterEnter && ((Convert.ToDouble(Value) != 0.0) | _allowEnpty))
					{
						SendKeys.Send("{Tab}");
					}
				}
				else if ((int)e.KeyCode == 37)
				{
					if (((TextBoxBase)this).SelectionStart > 0)
					{
						int selectionStart = ((TextBoxBase)this).SelectionStart;
						((TextBoxBase)this).SelectionStart = selectionStart - 1;
					}
					e.SuppressKeyPress = false;
				}
				else if ((int)e.KeyCode == 39)
				{
					int selectionStart = ((TextBoxBase)this).SelectionStart;
					((TextBoxBase)this).SelectionStart = selectionStart + 1;
					e.SuppressKeyPress = false;
				}
				else if ((int)e.KeyCode == 189 || (int)e.KeyCode == 109)
				{
					((Control)this).Text = ((Control)this).Text + ",000";
					((TextBoxBase)this).SelectionStart = 999;
				}
				else if ((int)e.KeyCode == 187 || (int)e.KeyCode == 107)
				{
					((Control)this).Text = ((Control)this).Text + ",00";
					((TextBoxBase)this).SelectionStart = 999;
				}
				else if ((int)e.KeyCode == 106)
				{
					((Control)this).Text = ((Control)this).Text + ",000,000";
					((TextBoxBase)this).SelectionStart = 999;
				}
				else if ((int)e.KeyCode == 189)
				{
					if (((Control)this).Text.Length > 4 && ((Control)this).Text.LastIndexOf(",000") == ((Control)this).Text.Length - 4)
					{
						((Control)this).Text = ((Control)this).Text.Substring(0, ((Control)this).Text.LastIndexOf(",000"));
					}
					((TextBoxBase)this).SelectionStart = 999;
				}
				else if ((int)e.KeyCode == 110 && DigitAfterPoint > 0)
				{
					((Control)this).Text = $"{Value}{PacketParser.TextManager.Instance.DecimalPoint}";
					((TextBoxBase)this).SelectionStart = 999;
				}
				else
				{
					e.SuppressKeyPress = true;
				}
			}
			e.Handled = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TextBox1_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 287);
		}
	}

	private void TextBox1_LostFocus(object sender, EventArgs e)
	{
		try
		{
			if (_getColoresFromSettingDLL)
			{
				((Control)this).BackColor = Color.White;
			}
			else
			{
				((Control)this).BackColor = _backColorTextBoxWhenLostFocus;
			}
			if (((Control)this).Text.EndsWith(PacketParser.TextManager.Instance.DecimalPoint))
			{
				((Control)this).Text = ((Control)this).Text.Substring(0, ((Control)this).Text.Length - 1);
			}
			TT.Hide((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TextBox1_LostFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 301);
		}
	}

	private void TextBox1_MouseEnter(object sender, EventArgs e)
	{
		try
		{
			_mouiseIn = true;
			DisplayBig();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TextBox1_MouseEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 310);
		}
	}

	private void TextBox1_MouseLeave(object sender, EventArgs e)
	{
		try
		{
			_mouiseIn = false;
			TT.Hide((IWin32Window)(object)this);
			HideBig();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TextBox1_MouseLeave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 320);
		}
	}

	private void TextBox1_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)this).TextChanged -= TextBox1_TextChanged;
			int selectionStart = ((TextBoxBase)this).SelectionStart;
			int length = ((Control)this).Text.Length;
			if (!((Control)this).Text.EndsWith(PacketParser.TextManager.Instance.DecimalPoint) && !((Control)this).Text.Contains(PacketParser.TextManager.Instance.DecimalPoint))
			{
				((Control)this).Text = Value.NpToString("N0", 0);
			}
			else if (GetDecimalPlaces() > DigitAfterPoint)
			{
				((Control)this).Text = ((Control)this).Text.Substring(0, ((Control)this).Text.Length - 1);
			}
			int length2 = ((Control)this).Text.Length;
			int num = selectionStart + length2 - length;
			if (num < 0)
			{
				num = 0;
			}
			((TextBoxBase)this).SelectionStart = num;
			if (_showToolTipText)
			{
				ShowTolltip();
			}
			((Control)this).TextChanged += TextBox1_TextChanged;
			if (!_showToolTipText & ((Control)this).Focused)
			{
				DisplayBig();
				this.ValueChanged?.Invoke(sender, e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TextBox1_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 348);
		}
	}
}

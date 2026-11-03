using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.CurrencyTextBox;

public class DateBoxFa : TextBoxX
{
	public enum ENFarsyDateStyle : short
	{
		none,
		DisplayToolTip
	}

	private bool ALT;

	private bool CTRL;

	private bool ETC = true;

	private bool ETT = true;

	private Color FBC = Color.Yellow;

	private Font FF = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3);

	private Font _font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3);

	private Color FFC = Color.Blue;

	private bool FTS = true;

	private int i;

	private Color LFBC;

	private Font LFF;

	private Color LFFC;

	private string ME = "";

	private bool MEIFS;

	private bool MER = true;

	private bool MESD;

	public string NumberToChar = "صفر";

	private bool SHIFT;

	private bool _AllowNull;

	private DateTime? _miladiDate;

	private string DefaultShamsiDate = DateTime.Now.Date.ToString().Substring(0, 10);

	private bool _ShowToolTipText;

	private bool _ShowNameOfDayOnToolTip;

	private ToolTip TT
	{
		get; [MethodImpl(MethodImplOptions.Synchronized)]
		set;
	}

	public override string Text
	{
		get
		{
			if (!string.IsNullOrEmpty(((TextBox)this).Text))
			{
				return ((TextBox)this).Text;
			}
			if (!AllowNull)
			{
				return DefaultShamsiDate;
			}
			return ((TextBox)this).Text;
		}
		set
		{
			if (AllowNull && string.IsNullOrEmpty(value))
			{
				((TextBox)this).Text = value;
			}
			else if (value.CheckCalendarDate())
			{
				((TextBox)this).Text = value;
			}
		}
	}

	public bool AllowNull
	{
		get
		{
			return _AllowNull;
		}
		set
		{
			_AllowNull = value;
			FD.AllowNull = value;
		}
	}

	public int Year
	{
		get
		{
			short.TryParse(((Control)this).Text.Substring(0, 4), out var result);
			return result;
		}
	}

	public override Font Font
	{
		get
		{
			return _font;
		}
		set
		{
			_font = value;
			((Control)this).Font = value;
		}
	}

	public ENFarsyDateStyle FarsyDateStyle { get; set; }

	public short Month
	{
		get
		{
			short.TryParse(((Control)this).Text.Substring(((Control)this).Text.IndexOf('/') + 1, 2), out var result);
			return result;
		}
	}

	public short Day
	{
		get
		{
			short.TryParse(((Control)this).Text.Substring(((Control)this).Text.LastIndexOf('/') + 1, 2), out var result);
			return result;
		}
	}

	public DateTime MiladiDate
	{
		get
		{
			try
			{
				if (_miladiDate.HasValue)
				{
					return ((Control)this).Text.CalendarToMiladi().AddHours(_miladiDate.Value.Hour).AddMinutes(_miladiDate.Value.Minute)
						.AddSeconds(_miladiDate.Value.Second);
				}
				return ((Control)this).Text.CalendarToMiladi().AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute)
					.AddSeconds(DateTime.Now.Second);
			}
			catch (Exception ex)
			{
				WebTelegramMessage.GetNp_UnseenErrors_bot().Sent2web("❌❌❌❌❌❌❌❌\r\nException:" + ex.Message + "\r\n#MiladiDate get\r\n" + $"_miladiDate:{_miladiDate}\r\n" + "Text:" + ((Control)this).Text + "\r\n" + $"DateTime.Now:{DateTime.Now}", null, "MiladiDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 458);
				WebErrorLog.Instance.StartLog(ex, "", "MiladiDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 464);
				return DateTime.Now;
			}
		}
		set
		{
			try
			{
				_miladiDate = value;
				((Control)this).Text = value.MiladiToCalture();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MiladiDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 475);
			}
		}
	}

	public Color BackcolorTooltip { get; set; }

	public int TootTipDelay { get; set; }

	public bool EnterToTab
	{
		get
		{
			return ETT;
		}
		set
		{
			ETT = value;
		}
	}

	public bool EscToClose
	{
		get
		{
			return ETC;
		}
		set
		{
			ETC = value;
		}
	}

	public Color FocusBackColor
	{
		get
		{
			return FBC;
		}
		set
		{
			FBC = value;
		}
	}

	public Font FocusFont
	{
		get
		{
			return FF;
		}
		set
		{
			FF = value;
		}
	}

	public Color FocusForeColor
	{
		get
		{
			return FFC;
		}
		set
		{
			FFC = value;
		}
	}

	public bool FocusTextSelect
	{
		get
		{
			return FTS;
		}
		set
		{
			FTS = value;
		}
	}

	public bool MessageEmptyShowDialog
	{
		get
		{
			return MESD;
		}
		set
		{
			MESD = value;
		}
	}

	public string MessegeEmpty
	{
		get
		{
			return ME;
		}
		set
		{
			ME = value;
		}
	}

	public bool MessegeEmptyInFormRight
	{
		get
		{
			return MER;
		}
		set
		{
			MER = value;
		}
	}

	public bool MessegeEmptyShowInForm
	{
		get
		{
			return MEIFS;
		}
		set
		{
			MEIFS = value;
		}
	}

	public bool ShowToolTipText
	{
		get
		{
			return _ShowToolTipText;
		}
		set
		{
			_ShowToolTipText = value;
		}
	}

	public bool ShowNameOfDayOnToolTip
	{
		get
		{
			return _ShowNameOfDayOnToolTip;
		}
		set
		{
			_ShowNameOfDayOnToolTip = value;
		}
	}

	private void MSGE()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		try
		{
			if (!(MessegeEmpty != "") || !MessegeEmptyShowInForm)
			{
				return;
			}
			if (((Control)this).Text == "" || ((Control)this).Text.Contains("_"))
			{
				Label val = new Label
				{
					Name = ((Control)this).Name + "1",
					Text = MessegeEmpty,
					AutoSize = true,
					Font = LFF,
					Top = ((Control)this).Top + Convert.ToInt16(((Control)this).Height / 2) - 3,
					ForeColor = Color.Red
				};
				Form obj = ((Control)this).FindForm();
				if (obj != null)
				{
					((Control)obj).Controls.Add((Control)(object)val);
				}
				if (MessegeEmptyInFormRight)
				{
					((Control)val).Left = ((Control)this).Left + ((Control)this).Width + 5;
				}
				else
				{
					((Control)val).Left = ((Control)this).Left - ((Control)val).Width - 5;
				}
				return;
			}
			try
			{
				Form obj2 = ((Control)this).FindForm();
				ControlCollection val2 = ((obj2 != null) ? ((Control)obj2).Controls : null);
				if (val2 == null)
				{
					return;
				}
				int count = ((ArrangedElementCollection)val2).Count;
				for (int i = 0; i < count; i++)
				{
					if (val2[i] is Label && val2[i].Name == ((Control)this).Name + "1" && MessegeEmpty != "" && ((Control)this).Text != "")
					{
						val2[i].Visible = false;
						val2[i].Text = "";
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MSGE", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 522);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "MSGE", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 526);
		}
	}

	protected override void OnCreateControl()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			((Control)this).RightToLeft = (RightToLeft)0;
			((TextBox)this).TextAlign = (HorizontalAlignment)2;
			((Control)this).Font = ((Control)this).Font;
			((TextBox)this).Text = ((Control)this).Text;
			TT = new ToolTip();
			((Control)this).OnCreateControl();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnCreateControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 540);
		}
	}

	protected override void OnEnter(EventArgs e)
	{
		try
		{
			ShowTolltip();
			LFBC = ((Control)this).BackColor;
			LFFC = ((Control)this).ForeColor;
			LFF = ((Control)this).Font;
			((Control)this).Font = FocusFont;
			((Control)this).BackColor = FocusBackColor;
			((Control)this).ForeColor = FocusForeColor;
			if (FocusTextSelect)
			{
				((TextBoxBase)this).SelectionStart = 10;
				((TextBoxBase)this).SelectionLength = ((Control)this).Text.Length;
			}
			((Control)this).OnEnter(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 562);
		}
	}

	protected override void OnLeave(EventArgs e)
	{
		TT.Hide((IWin32Window)(object)this);
		((Control)this).OnLeave(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Invalid comparison between Unknown and I4
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected I4, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Invalid comparison between Unknown and I4
		try
		{
			CTRL = e.Control;
			SHIFT = e.Shift;
			ALT = e.Alt;
			int selectionStart = ((TextBoxBase)this).SelectionStart;
			if (((Control)this).Text == "13__/__/__")
			{
				((Control)this).Text = DateTime.Now.MiladiToCalture();
				((TextBoxBase)this).SelectionStart = 10;
			}
			else
			{
				Keys keyCode = e.KeyCode;
				if ((int)keyCode != 13)
				{
					switch (keyCode - 32)
					{
					default:
						if ((int)keyCode == 46)
						{
							e.SuppressKeyPress = true;
							if (((TextBoxBase)this).SelectionStart == 4 || ((TextBoxBase)this).SelectionStart == 7)
							{
								int selectionStart2 = ((TextBoxBase)this).SelectionStart;
								((TextBoxBase)this).SelectionStart = selectionStart2 + 1;
							}
							else if (((TextBoxBase)this).SelectionStart < 10)
							{
								((Control)this).Text = ((Control)this).Text.Substring(0, ((TextBoxBase)this).SelectionStart) + "_" + ((Control)this).Text.Substring(((TextBoxBase)this).SelectionStart + 1);
								((TextBoxBase)this).SelectionStart = selectionStart + 1;
							}
						}
						break;
					case 6:
					{
						e.SuppressKeyPress = true;
						string text2 = ((Control)this).Text;
						if (text2 != null && text2.Length == 10 && !((Control)this).Text.Contains("_"))
						{
							if (((TextBoxBase)this).SelectionStart <= 4)
							{
								((Control)this).Text = LanguageAndCalendar.Instance.CaltureCalender.AddToDate(1, ((Control)this).Text, EnPartDate.Year);
							}
							else if (((TextBoxBase)this).SelectionStart >= 5 && ((TextBoxBase)this).SelectionStart <= 7)
							{
								((Control)this).Text = LanguageAndCalendar.Instance.CaltureCalender.AddToDate(1, ((Control)this).Text, EnPartDate.Mounth);
							}
							else if (((TextBoxBase)this).SelectionStart >= 8 && ((TextBoxBase)this).SelectionStart <= 10)
							{
								((Control)this).Text = LanguageAndCalendar.Instance.CaltureCalender.AddToDate(1, ((Control)this).Text, EnPartDate.Day);
							}
							((TextBoxBase)this).SelectionStart = selectionStart;
						}
						break;
					}
					case 8:
					{
						e.SuppressKeyPress = true;
						string text = ((Control)this).Text;
						if (text != null && text.Length == 10 && !((Control)this).Text.Contains("_"))
						{
							if (((TextBoxBase)this).SelectionStart <= 4)
							{
								((Control)this).Text = LanguageAndCalendar.Instance.CaltureCalender.AddToDate(-1, ((Control)this).Text, EnPartDate.Year);
							}
							else if (((TextBoxBase)this).SelectionStart >= 5 && ((TextBoxBase)this).SelectionStart <= 7)
							{
								((Control)this).Text = LanguageAndCalendar.Instance.CaltureCalender.AddToDate(-1, ((Control)this).Text, EnPartDate.Mounth);
							}
							else if (((TextBoxBase)this).SelectionStart >= 8 && ((TextBoxBase)this).SelectionStart <= 10)
							{
								((Control)this).Text = LanguageAndCalendar.Instance.CaltureCalender.AddToDate(-1, ((Control)this).Text, EnPartDate.Day);
							}
							((TextBoxBase)this).SelectionStart = selectionStart;
						}
						break;
					}
					case 0:
						e.SuppressKeyPress = true;
						MiladiDate = DateTime.Now;
						((TextBoxBase)this).SelectionStart = 999;
						break;
					case 1:
						e.SuppressKeyPress = true;
						((Control)this).Text = LanguageAndCalendar.Instance.CaltureCalender.EndDayOfMonth(((Control)this).Text, $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}").MiladiToCalture();
						((TextBoxBase)this).SelectionStart = 999;
						break;
					case 2:
						e.SuppressKeyPress = true;
						((Control)this).Text = ((Control)this).Text.Substring(0, 8) + "01";
						((TextBoxBase)this).SelectionStart = 999;
						break;
					case 3:
					case 4:
					case 5:
					case 7:
						break;
					}
				}
				else
				{
					e.SuppressKeyPress = true;
					if (EnterToTab)
					{
						if (!((TextBoxBase)this).Multiline)
						{
							e.Handled = true;
							Control parent = ((Control)this).Parent;
							if (parent != null)
							{
								parent.SelectNextControl((Control)(object)this, true, false, true, true);
							}
						}
						else if (!e.Control)
						{
							e.Handled = true;
							((Control)this).Parent.SelectNextControl((Control)(object)this, true, false, true, true);
						}
						((TextBoxBase)this).SelectionStart = selectionStart;
					}
				}
			}
			base.OnKeyDown(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 743);
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		try
		{
			if (((TextBoxBase)this).Multiline && e.KeyChar == '\n')
			{
				return;
			}
			string source = "";
			if (Convert.ToInt16(e.KeyChar) == 8)
			{
				if (((TextBoxBase)this).SelectionStart == 5 || ((TextBoxBase)this).SelectionStart == 8)
				{
					int selectionStart = ((TextBoxBase)this).SelectionStart;
					((TextBoxBase)this).SelectionStart = selectionStart - 1;
				}
				else if (((TextBoxBase)this).SelectionStart > 0)
				{
					int selectionStart2 = ((TextBoxBase)this).SelectionStart;
					((Control)this).Text = ((Control)this).Text.Substring(0, ((TextBoxBase)this).SelectionStart - 1) + "_" + ((Control)this).Text.Substring(((TextBoxBase)this).SelectionStart);
					((TextBoxBase)this).SelectionStart = selectionStart2 - 1;
				}
				e.KeyChar = Convert.ToChar(0);
				base.OnKeyPress(e);
				return;
			}
			if (((TextBoxBase)this).SelectionStart < 10)
			{
				((TextBoxBase)this).SelectionLength = 1;
			}
			if (((TextBoxBase)this).SelectionStart == 0)
			{
				source = "12";
			}
			if (((TextBoxBase)this).SelectionStart == 1 || ((TextBoxBase)this).SelectionStart == 2 || ((TextBoxBase)this).SelectionStart == 3)
			{
				source = "0123456789";
			}
			if (((TextBoxBase)this).SelectionStart == 5)
			{
				source = "01";
			}
			if (((TextBoxBase)this).SelectionStart == 6)
			{
				if (((Control)this).Text.Substring(5, 1) == "0")
				{
					source = "123456789";
				}
				if (((Control)this).Text.Substring(5, 1) == "1")
				{
					source = "012";
				}
			}
			if (((TextBoxBase)this).SelectionStart == 4 || ((TextBoxBase)this).SelectionStart == 7)
			{
				source = "/";
				if (!Enumerable.Contains(source, e.KeyChar))
				{
					int selectionStart = ((TextBoxBase)this).SelectionStart;
					((TextBoxBase)this).SelectionStart = selectionStart + 1;
				}
			}
			if (((TextBoxBase)this).SelectionStart == 8)
			{
				source = "0123";
			}
			if (((TextBoxBase)this).SelectionStart == 9)
			{
				if (((Control)this).Text.Substring(8, 1) == "1" || ((Control)this).Text.Substring(8, 1) == "2")
				{
					source = "1234567890";
				}
				if (((Control)this).Text.Substring(8, 1) == "0")
				{
					source = "123456789";
				}
				if (((Control)this).Text.Substring(8, 1) == "3")
				{
					source = ((!(((Control)this).Text.Substring(5, 2) == "01") && !(((Control)this).Text.Substring(5, 2) == "02") && !(((Control)this).Text.Substring(5, 2) == "03") && !(((Control)this).Text.Substring(5, 2) == "04") && !(((Control)this).Text.Substring(5, 2) == "05") && !(((Control)this).Text.Substring(5, 2) == "06")) ? "0" : "01");
				}
			}
			if (!Enumerable.Contains(source, e.KeyChar))
			{
				e.KeyChar = Convert.ToChar(0);
			}
			base.OnKeyPress(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnKeyPress", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 810);
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		try
		{
			ShowTolltip();
			if (((Control)this).Text == "" || ((Control)this).Text.Length != 10)
			{
				((Control)this).Text = "___/__/__";
			}
			MSGE();
			if (!string.IsNullOrEmpty(((Control)this).Text) && ((Control)this).Text.Substring(5, 1) == "1" && ((Control)this).Text.Substring(6, 1) != "0" && ((Control)this).Text.Substring(6, 1) != "1" && ((Control)this).Text.Substring(6, 1) != "2")
			{
				int selectionStart = ((TextBoxBase)this).SelectionStart;
				((Control)this).Text = ((Control)this).Text.Substring(0, 6) + "_" + ((Control)this).Text.Substring(7);
				((TextBoxBase)this).SelectionStart = selectionStart;
			}
			if (!string.IsNullOrEmpty(((Control)this).Text) && (((Control)this).Text.Substring(5, 2) == "07" || ((Control)this).Text.Substring(5, 2) == "08" || ((Control)this).Text.Substring(5, 2) == "09" || ((Control)this).Text.Substring(5, 2) == "10" || ((Control)this).Text.Substring(5, 2) == "11" || ((Control)this).Text.Substring(5, 2) == "12") && ((Control)this).Text.Substring(8) == "31")
			{
				int selectionStart2 = ((TextBoxBase)this).SelectionStart;
				((Control)this).Text = ((Control)this).Text.Substring(0, 9) + "_";
				((TextBoxBase)this).SelectionStart = selectionStart2;
			}
			if (((Control)this).Text.Length == 10 && ((Control)this).Text.Substring(8, 1) == "3" && ((Control)this).Text.Substring(9, 1) != "0" && ((Control)this).Text.Substring(9, 1) != "1")
			{
				int selectionStart3 = ((TextBoxBase)this).SelectionStart;
				((Control)this).Text = ((Control)this).Text.Substring(0, 9) + "_";
				((TextBoxBase)this).SelectionStart = selectionStart3;
			}
			if (((TextBoxBase)this).SelectionStart == 4 || ((TextBoxBase)this).SelectionStart == 7)
			{
				int selectionStart4 = ((TextBoxBase)this).SelectionStart;
				((TextBoxBase)this).SelectionStart = selectionStart4 + 1;
			}
			base.OnTextChanged(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnTextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 853);
		}
	}

	protected override void OnValidated(EventArgs e)
	{
		try
		{
			((Control)this).BackColor = LFBC;
			((Control)this).ForeColor = LFFC;
			((Control)this).Font = LFF;
			MSGE();
			((Control)this).OnValidated(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnValidated", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 866);
		}
	}

	protected override void OnValidating(CancelEventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!((Control)this).Text.CheckCalendarDate() && !AllowNull && !string.IsNullOrEmpty(MessegeEmpty))
			{
				MessageBox.Show(MessegeEmpty, "هشدار".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
			((Control)this).OnValidating(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnValidating", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 876);
		}
	}

	public void GotoEndOfMonthInThisYear(enMonth shamsiMonth)
	{
		try
		{
			string dateSh = DateTime.Now.MiladiToCalture();
			GotoEndOfMonth(shamsiMonth, Calendar.GetYearOfDateSh(dateSh));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GotoEndOfMonthInThisYear", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 885);
		}
	}

	public void GotoEndOfMonth(enMonth shamsiMonth, int year)
	{
		try
		{
			int num = Calendar.EndDayOfMonth((short)shamsiMonth, Year);
			string[] obj = new string[5]
			{
				Year.ToString(),
				"/",
				null,
				null,
				null
			};
			short num2 = (short)shamsiMonth;
			obj[2] = num2.ToString();
			obj[3] = "/";
			obj[4] = num.ToString();
			MiladiDate = string.Concat(obj).ShamsiToMiladi();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GotoEndOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 894);
		}
	}

	public void GotoStartOfMonthInThisYear(enMonth shamsiMonth)
	{
		try
		{
			string dateSh = DateTime.Now.MiladiToCalture();
			GotoStartOfMonth(shamsiMonth, Calendar.GetYearOfDateSh(dateSh));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GotoStartOfMonthInThisYear", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 903);
		}
	}

	public void GotoStartOfMonth(enMonth shamsiMonth, int year)
	{
		try
		{
			string text = Year.ToString();
			short num = (short)shamsiMonth;
			string dateParam = text + "/" + num + "/01";
			MiladiDate = dateParam.ShamsiToMiladi();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GotoStartOfMonth", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 912);
		}
	}

	public void Today()
	{
		MiladiDate = DateTime.Now;
	}

	private void InitializeComponent()
	{
		try
		{
			((Control)this).SuspendLayout();
			base.Border.CornerType = eCornerType.Rounded;
			base.Border.BorderBottom = eStyleBorderType.Dash;
			base.Border.BorderLeft = eStyleBorderType.Dash;
			base.Border.BorderRight = eStyleBorderType.Dash;
			base.Border.BorderTop = eStyleBorderType.Dash;
			base.Border.CornerDiameter = 3;
			((Control)this).Size = new Size(250, 23);
			((Control)this).ResumeLayout(false);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeComponent", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 929);
		}
	}

	private void ShowTolltip()
	{
		try
		{
			if (ShowToolTipText && TT != null)
			{
				TT.BackColor = Color.Red;
				TT.Active = true;
				string text = "";
				if (_ShowNameOfDayOnToolTip)
				{
					text = Calendar.GetDayNameOfWeek(MiladiDate);
				}
				TT.RemoveAll();
				if (((Control)this).Focused && !string.IsNullOrEmpty(text))
				{
					TT.Show(text, (IWin32Window)(object)this, 0, ((Control)this).Height, 2000);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowTolltip", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\CurrencyTextBox\\FaTextBox.cs", 945);
		}
	}
}

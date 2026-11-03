using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Cache;
using FormCustom.UC;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.Forms;

public class frmKeybord : AccountingBaseForm
{
	private bool _isDisposed;

	private IContainer components;

	private RadioButton rbAmount;

	private RadioButton rbPercent;

	private UcNumberPanel ucNumberPanel1;

	private UcGroupBox ucGroupBox1;

	private TextBox txtPrice;

	private UcButtons btnCancel;

	private UcButtons btnOk;

	public decimal InputPrice { get; set; }

	public EnWageCalculationMethod? Type { get; private set; }

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	private void SetInput(string input)
	{
		string text = ((Control)txtPrice).Text;
		try
		{
			if (text.Length > 10)
			{
				text = "";
			}
			if ((input == "." || input == "/") && Type.HasValue && Type == EnWageCalculationMethod.Percent)
			{
				if (!text.Contains(".") || text.Contains("/"))
				{
					text = ((Control)txtPrice).Text + "/";
				}
				((Control)txtPrice).Text = text;
				return;
			}
			text = ((Control)txtPrice).Text + input;
			decimal num = text.ParseToDecimal(0m, 3);
			if (!Type.HasValue)
			{
				((Control)txtPrice).Text = $"{num:N0}";
				return;
			}
			switch (Type)
			{
			case EnWageCalculationMethod.Price:
				((Control)txtPrice).Text = num.NpToString("N0", 0);
				break;
			case EnWageCalculationMethod.Percent:
				((Control)txtPrice).Text = text;
				break;
			default:
				throw new NotImplementedException();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetInput", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 44);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected I4, but got Unknown
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected I4, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected I4, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		try
		{
			if ((int)keyData <= 27)
			{
				if ((int)keyData != 8)
				{
					if ((int)keyData != 13)
					{
						if ((int)keyData == 27)
						{
							btnCancel.PerformClick();
						}
					}
					else
					{
						btnOk.PerformClick();
					}
				}
				else
				{
					ucNumberPanel1_OnClear(null, null);
				}
			}
			else
			{
				switch (keyData - 38)
				{
				default:
					switch (keyData - 96)
					{
					default:
						switch (keyData - 187)
						{
						case 0:
							break;
						case 2:
							goto IL_0134;
						case 3:
						case 4:
							goto IL_0225;
						default:
							goto end_IL_0025;
						}
						goto case 11;
					case 11:
						ucNumberPanel1_OnButtonClick("00", null);
						goto end_IL_0025;
					case 13:
						goto IL_0134;
					case 20:
						btnOk.PerformClick();
						goto end_IL_0025;
					case 1:
						break;
					case 2:
						goto IL_0178;
					case 3:
						goto IL_018a;
					case 4:
						goto IL_019c;
					case 5:
						goto IL_01ae;
					case 6:
						goto IL_01c0;
					case 7:
						goto IL_01cf;
					case 8:
						goto IL_01de;
					case 9:
						goto IL_01ed;
					case 0:
						goto IL_01fc;
					case 10:
						ucNumberPanel1_OnButtonClick("000000", null);
						goto end_IL_0025;
					case 14:
					case 15:
						goto IL_0225;
					case 12:
					case 16:
					case 17:
					case 18:
					case 19:
						goto end_IL_0025;
						IL_0225:
						ucNumberPanel1_OnButtonClick("/", null);
						goto end_IL_0025;
						IL_0134:
						ucNumberPanel1_OnButtonClick("000", null);
						goto end_IL_0025;
					}
					goto case 11;
				case 0:
					ucNumberPanel1_OnIncrease(5000.0, null);
					break;
				case 11:
					ucNumberPanel1_OnButtonClick("1", null);
					break;
				case 12:
					goto IL_0178;
				case 13:
					goto IL_018a;
				case 14:
					goto IL_019c;
				case 15:
					goto IL_01ae;
				case 16:
					goto IL_01c0;
				case 17:
					goto IL_01cf;
				case 18:
					goto IL_01de;
				case 19:
					goto IL_01ed;
				case 10:
					goto IL_01fc;
				case 2:
					ucNumberPanel1_OnDecrease(5000.0, null);
					break;
				case 1:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					break;
					IL_01fc:
					ucNumberPanel1_OnButtonClick("0", null);
					break;
					IL_01ed:
					ucNumberPanel1_OnButtonClick("9", null);
					break;
					IL_01de:
					ucNumberPanel1_OnButtonClick("8", null);
					break;
					IL_01cf:
					ucNumberPanel1_OnButtonClick("7", null);
					break;
					IL_01c0:
					ucNumberPanel1_OnButtonClick("6", null);
					break;
					IL_01ae:
					ucNumberPanel1_OnButtonClick("5", null);
					break;
					IL_019c:
					ucNumberPanel1_OnButtonClick("4", null);
					break;
					IL_018a:
					ucNumberPanel1_OnButtonClick("3", null);
					break;
					IL_0178:
					ucNumberPanel1_OnButtonClick("2", null);
					break;
					end_IL_0025:
					break;
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ProcessCmdKey", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 100);
		}
		return ((Form)this).ProcessCmdKey(ref msg, keyData);
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 112);
		}
	}

	public frmKeybord(string contextGuid, decimal inputPrice, EnWageCalculationMethod? type, string text)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			((Control)this).Text = text;
			Type = type;
			InputPrice = inputPrice;
			SetInput(InputPrice.ToString());
			if (!Type.HasValue)
			{
				rbAmount.Checked = true;
				RadioButton obj = rbAmount;
				bool enabled = (((Control)rbPercent).Enabled = true);
				((Control)obj).Enabled = enabled;
			}
			else if (Type == EnWageCalculationMethod.Price)
			{
				rbAmount.Checked = true;
				((Control)rbPercent).Enabled = false;
			}
			else
			{
				rbPercent.Checked = true;
				((Control)rbAmount).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 143);
		}
	}

	private Task ucNumberPanel1_OnButtonClick(string value, EventArgs e)
	{
		try
		{
			SetInput(value);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ucNumberPanel1_OnButtonClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 152);
		}
		return Task.CompletedTask;
	}

	private Task ucNumberPanel1_OnClear(object sender, EventArgs e)
	{
		try
		{
			((Control)txtPrice).Text = ((((Control)txtPrice).Text.Length > 0) ? ((Control)txtPrice).Text.Substring(0, ((Control)txtPrice).Text.Length - 1) : "");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ucNumberPanel1_OnClear", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 161);
		}
		return Task.CompletedTask;
	}

	private Task ucNumberPanel1_OnDecrease(double value, EventArgs e)
	{
		try
		{
			if (InputPrice - (decimal)value >= 0m)
			{
				((Control)txtPrice).Text = (InputPrice - (decimal)value).NpToString("N0", 0);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ucNumberPanel1_OnDecrease", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 171);
		}
		return Task.CompletedTask;
	}

	private Task ucNumberPanel1_OnIncrease(double value, EventArgs e)
	{
		try
		{
			((Control)txtPrice).Text = (InputPrice + (decimal)value).NpToString("N0", 0);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ucNumberPanel1_OnIncrease", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 180);
		}
		return Task.CompletedTask;
	}

	private void ucButtons1_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ucButtons1_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 190);
		}
	}

	private void frmKeybord_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void frmKeybord_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected I4, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected I4, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected I4, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 27)
			{
				if ((int)keyCode != 8)
				{
					if ((int)keyCode != 13)
					{
						if ((int)keyCode == 27)
						{
							btnCancel.PerformClick();
						}
					}
					else
					{
						btnOk.PerformClick();
					}
				}
				else
				{
					ucNumberPanel1_OnClear(null, null);
				}
				return;
			}
			switch (keyCode - 38)
			{
			default:
				switch (keyCode - 96)
				{
				default:
					switch (keyCode - 187)
					{
					default:
						return;
					case 0:
						break;
					case 2:
						goto IL_013b;
					case 3:
					case 4:
						goto IL_022c;
					case 1:
						return;
					}
					goto case 11;
				case 11:
					ucNumberPanel1_OnButtonClick("00", null);
					return;
				case 13:
					goto IL_013b;
				case 20:
					btnOk.PerformClick();
					return;
				case 1:
					break;
				case 2:
					goto IL_017f;
				case 3:
					goto IL_0191;
				case 4:
					goto IL_01a3;
				case 5:
					goto IL_01b5;
				case 6:
					goto IL_01c7;
				case 7:
					goto IL_01d6;
				case 8:
					goto IL_01e5;
				case 9:
					goto IL_01f4;
				case 0:
					goto IL_0203;
				case 10:
					ucNumberPanel1_OnButtonClick("000000", null);
					return;
				case 14:
				case 15:
					goto IL_022c;
				case 12:
				case 16:
				case 17:
				case 18:
				case 19:
					return;
					IL_022c:
					ucNumberPanel1_OnButtonClick("/", null);
					return;
					IL_013b:
					ucNumberPanel1_OnButtonClick("000", null);
					return;
				}
				goto case 11;
			case 0:
				ucNumberPanel1_OnIncrease(5000.0, null);
				break;
			case 11:
				ucNumberPanel1_OnButtonClick("1", null);
				break;
			case 12:
				goto IL_017f;
			case 13:
				goto IL_0191;
			case 14:
				goto IL_01a3;
			case 15:
				goto IL_01b5;
			case 16:
				goto IL_01c7;
			case 17:
				goto IL_01d6;
			case 18:
				goto IL_01e5;
			case 19:
				goto IL_01f4;
			case 10:
				goto IL_0203;
			case 2:
				ucNumberPanel1_OnDecrease(5000.0, null);
				break;
			case 1:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				break;
				IL_0203:
				ucNumberPanel1_OnButtonClick("0", null);
				break;
				IL_01f4:
				ucNumberPanel1_OnButtonClick("9", null);
				break;
				IL_01e5:
				ucNumberPanel1_OnButtonClick("8", null);
				break;
				IL_01d6:
				ucNumberPanel1_OnButtonClick("7", null);
				break;
				IL_01c7:
				ucNumberPanel1_OnButtonClick("6", null);
				break;
				IL_01b5:
				ucNumberPanel1_OnButtonClick("5", null);
				break;
				IL_01a3:
				ucNumberPanel1_OnButtonClick("4", null);
				break;
				IL_0191:
				ucNumberPanel1_OnButtonClick("3", null);
				break;
				IL_017f:
				ucNumberPanel1_OnButtonClick("2", null);
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmKeybord_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 246);
		}
	}

	private void btnOk_OnClick(object sender, EventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((Control)txtPrice).Text.ParseToDecimal() <= 0m)
			{
				((CacheBaseForm)this).ShowError("مبلغ را وارد کنید", "خطا");
				((Control)txtPrice).Focus();
				return;
			}
			if (rbAmount.Checked)
			{
				Type = EnWageCalculationMethod.Price;
			}
			if (rbPercent.Checked)
			{
				Type = EnWageCalculationMethod.Percent;
			}
			InputPrice = ((Control)txtPrice).Text.ParseToDecimal(0m, 3);
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 265);
		}
	}

	private void txtPrice_TextChanged(object sender, EventArgs e)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (rbPercent.Checked && Type.HasValue && !(((Control)txtPrice).Text.ParseToDecimal() <= 100m))
			{
				this.ShowMessage("درصد وارد شده نمی تواند از 100 درصد بیشتر باشد", "پیغام سیستم", "txtPrice_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs");
				((Control)txtPrice).Text = "100";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtPrice_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmKeybord.cs", 276);
		}
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Expected O, but got Unknown
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0788: Expected O, but got Unknown
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmKeybord));
		rbAmount = new RadioButton();
		rbPercent = new RadioButton();
		ucNumberPanel1 = new UcNumberPanel();
		ucGroupBox1 = new UcGroupBox();
		txtPrice = new TextBox();
		btnCancel = new UcButtons();
		btnOk = new UcButtons();
		((Control)this).SuspendLayout();
		((Control)rbAmount).AutoSize = true;
		((Control)rbAmount).Cursor = Cursors.Hand;
		((Control)rbAmount).Font = new Font("B Titr", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)rbAmount).ForeColor = Color.Black;
		((Control)rbAmount).Location = new Point(137, 4);
		((Control)rbAmount).Name = "rbAmount";
		((Control)rbAmount).Size = new Size(62, 37);
		((Control)rbAmount).TabIndex = 0;
		rbAmount.TabStop = true;
		((Control)rbAmount).Text = "[مبلغ]";
		((ButtonBase)rbAmount).UseVisualStyleBackColor = true;
		((Control)rbPercent).AutoSize = true;
		((Control)rbPercent).Cursor = Cursors.Hand;
		((Control)rbPercent).Font = new Font("B Titr", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)rbPercent).ForeColor = Color.Black;
		((Control)rbPercent).Location = new Point(269, 4);
		((Control)rbPercent).Name = "rbPercent";
		((Control)rbPercent).Size = new Size(73, 37);
		((Control)rbPercent).TabIndex = 1;
		rbPercent.TabStop = true;
		((Control)rbPercent).Text = "[درصد]";
		((ButtonBase)rbPercent).UseVisualStyleBackColor = true;
		((Control)ucNumberPanel1).BackColor = Color.FromArgb(252, 252, 255);
		((Control)ucNumberPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)ucNumberPanel1).Location = new Point(14, 100);
		((Control)ucNumberPanel1).Margin = new Padding(4, 3, 4, 3);
		((Control)ucNumberPanel1).Name = "ucNumberPanel1";
		ucNumberPanel1.PanelType = NumberPanelType.Price;
		((Control)ucNumberPanel1).RightToLeft = (RightToLeft)1;
		((Control)ucNumberPanel1).Size = new Size(434, 346);
		((Control)ucNumberPanel1).TabIndex = 2;
		ucNumberPanel1.OnButtonClick += ucNumberPanel1_OnButtonClick;
		ucNumberPanel1.OnClear += ucNumberPanel1_OnClear;
		ucNumberPanel1.OnIncrease += ucNumberPanel1_OnIncrease;
		ucNumberPanel1.OnDecrease += ucNumberPanel1_OnDecrease;
		((Control)ucGroupBox1).BackColor = Color.Transparent;
		ucGroupBox1.DisplayFont = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucGroupBox1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucGroupBox1.HeadHeight = 40;
		ucGroupBox1.IsShowHeader = false;
		((Control)ucGroupBox1).Location = new Point(3, 44);
		((Control)ucGroupBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucGroupBox1).Name = "ucGroupBox1";
		((Control)ucGroupBox1).RightToLeft = (RightToLeft)1;
		((Control)ucGroupBox1).Size = new Size(456, 446);
		((Control)ucGroupBox1).TabIndex = 3;
		ucGroupBox1.Title = "[عنوان]";
		((Control)txtPrice).Enabled = false;
		((Control)txtPrice).Font = new Font("B Titr", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtPrice).Location = new Point(63, 55);
		((Control)txtPrice).Name = "txtPrice";
		((Control)txtPrice).Size = new Size(341, 44);
		((Control)txtPrice).TabIndex = 5;
		txtPrice.TextAlign = (HorizontalAlignment)2;
		((Control)txtPrice).TextChanged += txtPrice_TextChanged;
		((Control)btnCancel).BackColor = Color.Transparent;
		btnCancel.CornerType = EnButtonCornerType.Round;
		btnCancel.DisplayText = "[انصراف](ESC)";
		((Control)btnCancel).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(27, 446);
		((Control)btnCancel).Margin = new Padding(4, 5, 4, 5);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(127, 38);
		((Control)btnCancel).TabIndex = 6;
		btnCancel.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Type = EnButtonType.Normal;
		btnCancel.OnClick += ucButtons1_OnClick;
		((Control)btnOk).BackColor = Color.Transparent;
		btnOk.CornerType = EnButtonCornerType.Round;
		btnOk.DisplayText = "[تایید](F5)";
		((Control)btnOk).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnOk.IsShowMoreDetails = false;
		((Control)btnOk).Location = new Point(162, 446);
		((Control)btnOk).Margin = new Padding(4, 5, 4, 5);
		((Control)btnOk).Name = "btnOk";
		((Control)btnOk).RightToLeft = (RightToLeft)1;
		((Control)btnOk).Size = new Size(271, 38);
		((Control)btnOk).TabIndex = 7;
		btnOk.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Type = EnButtonType.Bold;
		btnOk.OnClick += btnOk_OnClick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = SystemColors.Control;
		((Form)this).ClientSize = new Size(461, 490);
		((Control)this).Controls.Add((Control)(object)btnOk);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)txtPrice);
		((Control)this).Controls.Add((Control)(object)ucNumberPanel1);
		((Control)this).Controls.Add((Control)(object)rbPercent);
		((Control)this).Controls.Add((Control)(object)rbAmount);
		((Control)this).Controls.Add((Control)(object)ucGroupBox1);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "frmKeybord";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).FormClosed += new FormClosedEventHandler(frmKeybord_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmKeybord_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}

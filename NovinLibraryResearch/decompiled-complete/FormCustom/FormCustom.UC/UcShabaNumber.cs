using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.ExtendedControls.Notification;
using FormCustom.Properties;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcShabaNumber : UserControl
{
	private IContainer components;

	private PictureBox picCopyShaba;

	internal Label label2;

	internal Label Label46;

	internal TextBox txtShaba3;

	internal TextBox txtShaba2;

	internal TextBox txtShaba8;

	internal TextBox txtShaba1;

	internal TextBox txtShaba7;

	internal TextBox txtShaba6;

	internal TextBox txtShaba5;

	internal TextBox txtShaba4;

	public string ShabaNumber
	{
		get
		{
			string text = "IR" + ((Control)txtShaba1).Text + ((Control)txtShaba2).Text + ((Control)txtShaba3).Text + ((Control)txtShaba4).Text + ((Control)txtShaba5).Text + ((Control)txtShaba6).Text + ((Control)txtShaba7).Text + ((Control)txtShaba8).Text;
			if (!(text == "IR"))
			{
				return text;
			}
			return "";
		}
	}

	public event Action OnLeaveRequest;

	public event Func<Task> OnBankDetect;

	public void Init(ShabaNumberModel info)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)(object)this).Transelate();
			new ToolTip().SetToolTip((Control)(object)picCopyShaba, "کپی کردن شماره شبا".GetTranslate());
			((Control)txtShaba1).Text = info.Shaba2;
			((Control)txtShaba2).Text = info.Shaba3;
			((Control)txtShaba3).Text = info.Shaba4;
			((Control)txtShaba4).Text = info.Shaba5;
			((Control)txtShaba5).Text = info.Shaba6;
			((Control)txtShaba6).Text = info.Shaba7;
			((Control)txtShaba7).Text = info.Shaba8;
			((Control)txtShaba8).Text = info.Shaba9;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShabaNumber.cs", 41);
		}
	}

	public UcShabaNumber()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void picCopyShaba_Click(object sender, EventArgs e)
	{
		try
		{
			if (!string.IsNullOrEmpty(ShabaNumber))
			{
				Clipboard.SetText(ShabaNumber);
				NotificationManager.MessageBoxShowByNotif(showPopUp: true, EnNotification.Information, "شماره شبا در کلیپ بورد کپی شد".GetTranslate(), "", 1);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picCopyShaba_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShabaNumber.cs", 58);
		}
	}

	private void txtShaba1_Enter(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null)
			{
				((Control)val).BackColor = Color.FromArgb(210, 221, 251);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtShaba1_Enter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShabaNumber.cs", 67);
		}
	}

	private void txtShaba1_Leave(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null)
			{
				((Control)val).BackColor = Color.White;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtShaba1_Leave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShabaNumber.cs", 76);
		}
	}

	private void txtShaba1_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)txtShaba1).Text = ((Control)txtShaba1).Text.RemoveNoNumbers();
			if (((Control)txtShaba1).Text.Length >= 2)
			{
				((Control)txtShaba2).Focus();
				((TextBoxBase)txtShaba2).SelectAll();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtShaba1_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShabaNumber.cs", 89);
		}
	}

	private void txtShaba2_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)txtShaba2).Text = ((Control)txtShaba2).Text.RemoveNoNumbers();
			if (((Control)txtShaba2).Text.Length >= 3)
			{
				((Control)txtShaba3).Focus();
				((TextBoxBase)txtShaba3).SelectAll();
				this.OnBankDetect?.Invoke();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtShaba2_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShabaNumber.cs", 103);
		}
	}

	private void txtShaba3_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)txtShaba3).Text = ((Control)txtShaba3).Text.RemoveNoNumbers();
			if (((Control)txtShaba3).Text.Length >= 1)
			{
				((Control)txtShaba4).Focus();
				((TextBoxBase)txtShaba4).SelectAll();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtShaba3_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShabaNumber.cs", 116);
		}
	}

	private void txtShaba4_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val == null)
			{
				return;
			}
			((Control)val).Text = ((Control)val).Text.RemoveNoNumbers();
			if (((Control)val).Text.Length >= 4)
			{
				if (((Control)val).Name == ((Control)txtShaba4).Name)
				{
					((Control)txtShaba5).Focus();
					((TextBoxBase)txtShaba5).SelectAll();
				}
				if (((Control)val).Name == ((Control)txtShaba5).Name)
				{
					((Control)txtShaba6).Focus();
					((TextBoxBase)txtShaba6).SelectAll();
				}
				if (((Control)val).Name == ((Control)txtShaba6).Name)
				{
					((Control)txtShaba7).Focus();
					((TextBoxBase)txtShaba7).SelectAll();
				}
				if (((Control)val).Name == ((Control)txtShaba7).Name)
				{
					((Control)txtShaba8).Focus();
					((TextBoxBase)txtShaba8).SelectAll();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtShaba4_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShabaNumber.cs", 146);
		}
	}

	private void txtShaba8_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)txtShaba8).Text.Length >= 2 && this.OnLeaveRequest != null)
			{
				this.OnLeaveRequest?.Invoke();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtShaba8_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShabaNumber.cs", 155);
		}
	}

	private void txtShaba1_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 8)
			{
				if ((int)keyCode != 86 || !e.Control || !(((Control)(TextBox)sender).Name == ((Control)txtShaba1).Name))
				{
					return;
				}
				string text = Clipboard.GetText().RemoveNoNumbers();
				if (string.IsNullOrEmpty(text))
				{
					return;
				}
				if (text.Length <= 2)
				{
					((Control)txtShaba1).Text = text;
				}
				if (text.Length >= 24 && text.Length <= 26)
				{
					if (text.Length == 24 && !text.Contains("IR"))
					{
						text = "IR" + text;
					}
					((Control)txtShaba1).Text = ((text.Length < 4) ? "" : text.Substring(2, 2));
					((Control)txtShaba2).Text = ((text.Length < 7) ? "" : text.Substring(4, 3));
					((Control)txtShaba3).Text = ((text.Length < 8) ? "" : text.Substring(7, 1));
					((Control)txtShaba4).Text = ((text.Length < 12) ? "" : text.Substring(8, 4));
					((Control)txtShaba5).Text = ((text.Length < 16) ? "" : text.Substring(12, 4));
					((Control)txtShaba6).Text = ((text.Length < 20) ? "" : text.Substring(16, 4));
					((Control)txtShaba7).Text = ((text.Length < 24) ? "" : text.Substring(20, 4));
					((Control)txtShaba8).Text = ((text.Length < 26) ? "" : text.Substring(24, 2));
				}
				return;
			}
			TextBox val = (TextBox)sender;
			if (((Control)val).Name == ((Control)txtShaba8).Name && string.IsNullOrEmpty(((Control)val).Text))
			{
				((Control)txtShaba7).Focus();
				((TextBoxBase)txtShaba7).SelectionStart = ((Control)txtShaba7).Text.Length;
			}
			else if (((Control)val).Name == ((Control)txtShaba7).Name && string.IsNullOrEmpty(((Control)val).Text))
			{
				((Control)txtShaba6).Focus();
				((TextBoxBase)txtShaba6).SelectionStart = ((Control)txtShaba6).Text.Length;
			}
			else if (((Control)val).Name == ((Control)txtShaba6).Name && string.IsNullOrEmpty(((Control)val).Text))
			{
				((Control)txtShaba5).Focus();
				((TextBoxBase)txtShaba5).SelectionStart = ((Control)txtShaba5).Text.Length;
			}
			else if (((Control)val).Name == ((Control)txtShaba5).Name && string.IsNullOrEmpty(((Control)val).Text))
			{
				((Control)txtShaba4).Focus();
				((TextBoxBase)txtShaba4).SelectionStart = ((Control)txtShaba4).Text.Length;
			}
			else if (((Control)val).Name == ((Control)txtShaba4).Name && string.IsNullOrEmpty(((Control)val).Text))
			{
				((Control)txtShaba3).Focus();
				((TextBoxBase)txtShaba3).SelectionStart = ((Control)txtShaba3).Text.Length;
			}
			else if (((Control)val).Name == ((Control)txtShaba3).Name && string.IsNullOrEmpty(((Control)val).Text))
			{
				((Control)txtShaba2).Focus();
				((TextBoxBase)txtShaba2).SelectionStart = ((Control)txtShaba2).Text.Length;
			}
			else if (((Control)val).Name == ((Control)txtShaba2).Name && string.IsNullOrEmpty(((Control)val).Text))
			{
				((Control)txtShaba1).Focus();
				((TextBoxBase)txtShaba1).SelectionStart = ((Control)txtShaba1).Text.Length;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtShaba1_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShabaNumber.cs", 224);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Expected O, but got Unknown
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Expected O, but got Unknown
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Expected O, but got Unknown
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Expected O, but got Unknown
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Expected O, but got Unknown
		//IL_082b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Expected O, but got Unknown
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Expected O, but got Unknown
		//IL_092d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Expected O, but got Unknown
		//IL_096a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a05: Expected O, but got Unknown
		//IL_0bd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be1: Expected O, but got Unknown
		//IL_0be6: Unknown result type (might be due to invalid IL or missing references)
		label2 = new Label();
		Label46 = new Label();
		txtShaba3 = new TextBox();
		txtShaba2 = new TextBox();
		txtShaba8 = new TextBox();
		txtShaba1 = new TextBox();
		txtShaba7 = new TextBox();
		txtShaba6 = new TextBox();
		txtShaba5 = new TextBox();
		txtShaba4 = new TextBox();
		picCopyShaba = new PictureBox();
		((ISupportInitialize)picCopyShaba).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label2).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((Control)label2).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label2).Location = new Point(4, 5);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(19, 18);
		((Control)label2).TabIndex = 157;
		((Control)label2).Text = "IR";
		label2.TextAlign = (ContentAlignment)64;
		((Control)Label46).Anchor = (AnchorStyles)9;
		((Control)Label46).AutoSize = true;
		((Control)Label46).BackColor = Color.FromArgb(249, 250, 255);
		((Control)Label46).Font = new Font("B Yekan", 9.75f);
		((Control)Label46).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)Label46).Location = new Point(339, 5);
		((Control)Label46).Name = "Label46";
		((Control)Label46).Size = new Size(71, 20);
		((Control)Label46).TabIndex = 156;
		((Control)Label46).Text = "[شماره شبا]:";
		Label46.TextAlign = (ContentAlignment)64;
		((Control)txtShaba3).Font = new Font("B Yekan", 9.75f);
		((Control)txtShaba3).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtShaba3).Location = new Point(90, 2);
		((Control)txtShaba3).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtShaba3).MaxLength = 1;
		((Control)txtShaba3).Name = "txtShaba3";
		((Control)txtShaba3).Size = new Size(15, 27);
		((Control)txtShaba3).TabIndex = 150;
		txtShaba3.TextAlign = (HorizontalAlignment)2;
		((Control)txtShaba3).TextChanged += txtShaba3_TextChanged;
		((Control)txtShaba3).Enter += txtShaba1_Enter;
		((Control)txtShaba3).KeyDown += new KeyEventHandler(txtShaba1_KeyDown);
		((Control)txtShaba3).Leave += txtShaba1_Leave;
		((Control)txtShaba2).Font = new Font("B Yekan", 9.75f);
		((Control)txtShaba2).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtShaba2).Location = new Point(57, 2);
		((Control)txtShaba2).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtShaba2).MaxLength = 3;
		((Control)txtShaba2).Name = "txtShaba2";
		((Control)txtShaba2).Size = new Size(30, 27);
		((Control)txtShaba2).TabIndex = 149;
		txtShaba2.TextAlign = (HorizontalAlignment)2;
		((Control)txtShaba2).TextChanged += txtShaba2_TextChanged;
		((Control)txtShaba2).Enter += txtShaba1_Enter;
		((Control)txtShaba2).KeyDown += new KeyEventHandler(txtShaba1_KeyDown);
		((Control)txtShaba2).Leave += txtShaba1_Leave;
		((Control)txtShaba8).Font = new Font("B Yekan", 9.75f);
		((Control)txtShaba8).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtShaba8).Location = new Point(279, 2);
		((Control)txtShaba8).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtShaba8).MaxLength = 2;
		((Control)txtShaba8).Name = "txtShaba8";
		((Control)txtShaba8).Size = new Size(25, 27);
		((Control)txtShaba8).TabIndex = 155;
		txtShaba8.TextAlign = (HorizontalAlignment)2;
		((Control)txtShaba8).TextChanged += txtShaba8_TextChanged;
		((Control)txtShaba8).Enter += txtShaba1_Enter;
		((Control)txtShaba8).KeyDown += new KeyEventHandler(txtShaba1_KeyDown);
		((Control)txtShaba8).Leave += txtShaba1_Leave;
		((Control)txtShaba1).Font = new Font("B Yekan", 9.75f);
		((Control)txtShaba1).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtShaba1).Location = new Point(29, 2);
		((Control)txtShaba1).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtShaba1).MaxLength = 2;
		((Control)txtShaba1).Name = "txtShaba1";
		((Control)txtShaba1).Size = new Size(25, 27);
		((Control)txtShaba1).TabIndex = 148;
		txtShaba1.TextAlign = (HorizontalAlignment)2;
		((Control)txtShaba1).TextChanged += txtShaba1_TextChanged;
		((Control)txtShaba1).Enter += txtShaba1_Enter;
		((Control)txtShaba1).KeyDown += new KeyEventHandler(txtShaba1_KeyDown);
		((Control)txtShaba1).Leave += txtShaba1_Leave;
		((Control)txtShaba7).Font = new Font("B Yekan", 9.75f);
		((Control)txtShaba7).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtShaba7).Location = new Point(237, 2);
		((Control)txtShaba7).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtShaba7).MaxLength = 4;
		((Control)txtShaba7).Name = "txtShaba7";
		((Control)txtShaba7).Size = new Size(40, 27);
		((Control)txtShaba7).TabIndex = 154;
		txtShaba7.TextAlign = (HorizontalAlignment)2;
		((Control)txtShaba7).TextChanged += txtShaba4_TextChanged;
		((Control)txtShaba7).Enter += txtShaba1_Enter;
		((Control)txtShaba7).KeyDown += new KeyEventHandler(txtShaba1_KeyDown);
		((Control)txtShaba7).Leave += txtShaba1_Leave;
		((Control)txtShaba6).Font = new Font("B Yekan", 9.75f);
		((Control)txtShaba6).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtShaba6).Location = new Point(194, 2);
		((Control)txtShaba6).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtShaba6).MaxLength = 4;
		((Control)txtShaba6).Name = "txtShaba6";
		((Control)txtShaba6).Size = new Size(40, 27);
		((Control)txtShaba6).TabIndex = 153;
		txtShaba6.TextAlign = (HorizontalAlignment)2;
		((Control)txtShaba6).TextChanged += txtShaba4_TextChanged;
		((Control)txtShaba6).Enter += txtShaba1_Enter;
		((Control)txtShaba6).KeyDown += new KeyEventHandler(txtShaba1_KeyDown);
		((Control)txtShaba6).Leave += txtShaba1_Leave;
		((Control)txtShaba5).Font = new Font("B Yekan", 9.75f);
		((Control)txtShaba5).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtShaba5).Location = new Point(151, 2);
		((Control)txtShaba5).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtShaba5).MaxLength = 4;
		((Control)txtShaba5).Name = "txtShaba5";
		((Control)txtShaba5).Size = new Size(40, 27);
		((Control)txtShaba5).TabIndex = 152;
		txtShaba5.TextAlign = (HorizontalAlignment)2;
		((Control)txtShaba5).TextChanged += txtShaba4_TextChanged;
		((Control)txtShaba5).Enter += txtShaba1_Enter;
		((Control)txtShaba5).KeyDown += new KeyEventHandler(txtShaba1_KeyDown);
		((Control)txtShaba5).Leave += txtShaba1_Leave;
		((Control)txtShaba4).Font = new Font("B Yekan", 9.75f);
		((Control)txtShaba4).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtShaba4).Location = new Point(108, 2);
		((Control)txtShaba4).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtShaba4).MaxLength = 4;
		((Control)txtShaba4).Name = "txtShaba4";
		((Control)txtShaba4).Size = new Size(40, 27);
		((Control)txtShaba4).TabIndex = 151;
		txtShaba4.TextAlign = (HorizontalAlignment)2;
		((Control)txtShaba4).TextChanged += txtShaba4_TextChanged;
		((Control)txtShaba4).Enter += txtShaba1_Enter;
		((Control)txtShaba4).KeyDown += new KeyEventHandler(txtShaba1_KeyDown);
		((Control)txtShaba4).Leave += txtShaba1_Leave;
		((Control)picCopyShaba).BackColor = Color.FromArgb(249, 250, 255);
		((Control)picCopyShaba).Cursor = Cursors.Hand;
		picCopyShaba.Image = (Image)(object)FormCustom.Properties.Resources.copy_Blue;
		((Control)picCopyShaba).Location = new Point(311, 4);
		((Control)picCopyShaba).Name = "picCopyShaba";
		((Control)picCopyShaba).Size = new Size(23, 21);
		picCopyShaba.SizeMode = (PictureBoxSizeMode)4;
		picCopyShaba.TabIndex = 158;
		picCopyShaba.TabStop = false;
		((Control)picCopyShaba).Click += picCopyShaba_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(249, 250, 255);
		((Control)this).Controls.Add((Control)(object)picCopyShaba);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)Label46);
		((Control)this).Controls.Add((Control)(object)txtShaba3);
		((Control)this).Controls.Add((Control)(object)txtShaba2);
		((Control)this).Controls.Add((Control)(object)txtShaba8);
		((Control)this).Controls.Add((Control)(object)txtShaba1);
		((Control)this).Controls.Add((Control)(object)txtShaba7);
		((Control)this).Controls.Add((Control)(object)txtShaba6);
		((Control)this).Controls.Add((Control)(object)txtShaba5);
		((Control)this).Controls.Add((Control)(object)txtShaba4);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcShabaNumber";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(414, 31);
		((ISupportInitialize)picCopyShaba).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}

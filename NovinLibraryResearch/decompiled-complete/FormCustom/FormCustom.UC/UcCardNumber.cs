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

public class UcCardNumber : UserControl
{
	private IContainer components;

	private PictureBox picCopyCardNumber;

	internal Label label5;

	internal Label label4;

	internal Label label3;

	internal Label Label45;

	internal TextBox txtCard1;

	internal TextBox txtCard2;

	internal TextBox txtCard3;

	internal TextBox txtCard4;

	public string CardNumber => ((Control)txtCard1).Text + ((Control)txtCard2).Text + ((Control)txtCard3).Text + ((Control)txtCard4).Text;

	public event Action OnLeaveRequest;

	public event Func<Task> OnBankDetect;

	public void Init(CardNumberModel info)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)(object)this).Transelate();
			new ToolTip().SetToolTip((Control)(object)picCopyCardNumber, "کپی کردن شماره کارت".GetTranslate());
			((Control)txtCard1).Text = info.Card1;
			((Control)txtCard2).Text = info.Card2;
			((Control)txtCard3).Text = info.Card3;
			((Control)txtCard4).Text = info.Card4;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCardNumber.cs", 30);
		}
	}

	public void GetFocus()
	{
		try
		{
			((Control)txtCard1).Focus();
			((TextBoxBase)txtCard1).SelectAll();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCardNumber.cs", 39);
		}
	}

	public UcCardNumber()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void txtCard1_Enter(object sender, EventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "txtCard1_Enter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCardNumber.cs", 55);
		}
	}

	private void txtCard1_Leave(object sender, EventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "txtCard1_Leave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCardNumber.cs", 64);
		}
	}

	private void txtCard1_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val == null)
			{
				return;
			}
			((Control)val).Text = ((Control)val).Text.RemoveNoNumbers();
			if (((Control)val).Text.Length == 2 && ((Control)val).Name == ((Control)txtCard2).Name && this.OnBankDetect != null)
			{
				this.OnBankDetect();
			}
			if (((Control)val).Text.Length >= 4)
			{
				if (((Control)val).Name == ((Control)txtCard1).Name)
				{
					((Control)txtCard2).Focus();
					((TextBoxBase)txtCard2).SelectAll();
				}
				if (((Control)val).Name == ((Control)txtCard2).Name)
				{
					((Control)txtCard3).Focus();
					((TextBoxBase)txtCard3).SelectAll();
				}
				if (((Control)val).Name == ((Control)txtCard3).Name)
				{
					((Control)txtCard4).Focus();
					((TextBoxBase)txtCard4).SelectAll();
				}
				if (((Control)val).Name == ((Control)txtCard4).Name && this.OnLeaveRequest != null)
				{
					this.OnLeaveRequest();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtCard1_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCardNumber.cs", 92);
		}
	}

	private void picCopyCardNumber_Click(object sender, EventArgs e)
	{
		try
		{
			if (!string.IsNullOrEmpty(CardNumber))
			{
				Clipboard.SetText(CardNumber);
				NotificationManager.MessageBoxShowByNotif(showPopUp: true, EnNotification.Information, "شماره کارت در کلیپ بورد کپی شد".GetTranslate(), "", 1);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picCopyCardNumber_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCardNumber.cs", 102);
		}
	}

	private void txtCard1_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 8)
			{
				if ((int)keyCode != 86 || !e.Control)
				{
					return;
				}
				string text = Clipboard.GetText().RemoveNoNumbers();
				if (string.IsNullOrEmpty(text))
				{
					return;
				}
				if (text.Length <= 4)
				{
					((Control)(TextBox)sender).Text = text;
					return;
				}
				if (text.Length >= 4)
				{
					((Control)txtCard1).Text = ((text.Length < 4) ? "" : text.Substring(0, 4));
				}
				if (text.Length >= 8)
				{
					((Control)txtCard2).Text = ((text.Length < 8) ? "" : text.Substring(4, 4));
				}
				if (text.Length >= 12)
				{
					((Control)txtCard3).Text = ((text.Length < 12) ? "" : text.Substring(8, 4));
				}
				if (text.Length >= 16)
				{
					((Control)txtCard4).Text = ((text.Length < 16) ? "" : text.Substring(12, 4));
				}
				this.OnBankDetect?.Invoke();
			}
			else
			{
				TextBox val = (TextBox)sender;
				if (((Control)val).Name == ((Control)txtCard4).Name && string.IsNullOrEmpty(((Control)val).Text))
				{
					((Control)txtCard3).Focus();
					((TextBoxBase)txtCard3).SelectionStart = ((Control)txtCard3).Text.Length;
				}
				else if (((Control)val).Name == ((Control)txtCard3).Name && string.IsNullOrEmpty(((Control)val).Text))
				{
					((Control)txtCard2).Focus();
					((TextBoxBase)txtCard2).SelectionStart = ((Control)txtCard2).Text.Length;
				}
				else if (((Control)val).Name == ((Control)txtCard2).Name && string.IsNullOrEmpty(((Control)val).Text))
				{
					((Control)txtCard1).Focus();
					((TextBoxBase)txtCard1).SelectionStart = ((Control)txtCard1).Text.Length;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtCard1_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCardNumber.cs", 145);
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
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Expected O, but got Unknown
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Expected O, but got Unknown
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Expected O, but got Unknown
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Expected O, but got Unknown
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Expected O, but got Unknown
		//IL_0923: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Expected O, but got Unknown
		//IL_0932: Unknown result type (might be due to invalid IL or missing references)
		picCopyCardNumber = new PictureBox();
		label5 = new Label();
		label4 = new Label();
		label3 = new Label();
		Label45 = new Label();
		txtCard1 = new TextBox();
		txtCard2 = new TextBox();
		txtCard3 = new TextBox();
		txtCard4 = new TextBox();
		((ISupportInitialize)picCopyCardNumber).BeginInit();
		((Control)this).SuspendLayout();
		((Control)picCopyCardNumber).BackColor = Color.FromArgb(249, 250, 255);
		((Control)picCopyCardNumber).Cursor = Cursors.Hand;
		picCopyCardNumber.Image = (Image)(object)FormCustom.Properties.Resources.copy_Blue;
		((Control)picCopyCardNumber).Location = new Point(285, 4);
		((Control)picCopyCardNumber).Name = "picCopyCardNumber";
		((Control)picCopyCardNumber).Size = new Size(23, 21);
		picCopyCardNumber.SizeMode = (PictureBoxSizeMode)4;
		picCopyCardNumber.TabIndex = 156;
		picCopyCardNumber.TabStop = false;
		((Control)picCopyCardNumber).Click += picCopyCardNumber_Click;
		((Control)label5).AutoSize = true;
		((Control)label5).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label5).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((Control)label5).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label5).Location = new Point(209, 6);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(13, 18);
		((Control)label5).TabIndex = 153;
		((Control)label5).Text = "-";
		label5.TextAlign = (ContentAlignment)64;
		((Control)label4).AutoSize = true;
		((Control)label4).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label4).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((Control)label4).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label4).Location = new Point(135, 6);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(13, 18);
		((Control)label4).TabIndex = 154;
		((Control)label4).Text = "-";
		label4.TextAlign = (ContentAlignment)64;
		((Control)label3).AutoSize = true;
		((Control)label3).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label3).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1);
		((Control)label3).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label3).Location = new Point(60, 6);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(13, 18);
		((Control)label3).TabIndex = 155;
		((Control)label3).Text = "-";
		label3.TextAlign = (ContentAlignment)64;
		((Control)Label45).Anchor = (AnchorStyles)9;
		((Control)Label45).AutoSize = true;
		((Control)Label45).BackColor = Color.FromArgb(249, 250, 255);
		((Control)Label45).Font = new Font("B Yekan", 9.75f);
		((Control)Label45).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)Label45).Location = new Point(329, 5);
		((Control)Label45).Name = "Label45";
		((Control)Label45).Size = new Size(79, 20);
		((Control)Label45).TabIndex = 152;
		((Control)Label45).Text = "[شماره کارت]:";
		Label45.TextAlign = (ContentAlignment)64;
		((Control)txtCard1).Font = new Font("B Yekan", 9.75f);
		((Control)txtCard1).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtCard1).Location = new Point(2, 2);
		((Control)txtCard1).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtCard1).MaxLength = 4;
		((Control)txtCard1).Name = "txtCard1";
		((Control)txtCard1).Size = new Size(53, 27);
		((Control)txtCard1).TabIndex = 148;
		txtCard1.TextAlign = (HorizontalAlignment)2;
		((Control)txtCard1).TextChanged += txtCard1_TextChanged;
		((Control)txtCard1).Enter += txtCard1_Enter;
		((Control)txtCard1).KeyDown += new KeyEventHandler(txtCard1_KeyDown);
		((Control)txtCard1).Leave += txtCard1_Leave;
		((Control)txtCard2).Font = new Font("B Yekan", 9.75f);
		((Control)txtCard2).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtCard2).Location = new Point(77, 2);
		((Control)txtCard2).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtCard2).MaxLength = 4;
		((Control)txtCard2).Name = "txtCard2";
		((Control)txtCard2).Size = new Size(53, 27);
		((Control)txtCard2).TabIndex = 149;
		txtCard2.TextAlign = (HorizontalAlignment)2;
		((Control)txtCard2).TextChanged += txtCard1_TextChanged;
		((Control)txtCard2).Enter += txtCard1_Enter;
		((Control)txtCard2).KeyDown += new KeyEventHandler(txtCard1_KeyDown);
		((Control)txtCard2).Leave += txtCard1_Leave;
		((Control)txtCard3).Font = new Font("B Yekan", 9.75f);
		((Control)txtCard3).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtCard3).Location = new Point(152, 2);
		((Control)txtCard3).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtCard3).MaxLength = 4;
		((Control)txtCard3).Name = "txtCard3";
		((Control)txtCard3).Size = new Size(53, 27);
		((Control)txtCard3).TabIndex = 150;
		txtCard3.TextAlign = (HorizontalAlignment)2;
		((Control)txtCard3).TextChanged += txtCard1_TextChanged;
		((Control)txtCard3).Enter += txtCard1_Enter;
		((Control)txtCard3).KeyDown += new KeyEventHandler(txtCard1_KeyDown);
		((Control)txtCard3).Leave += txtCard1_Leave;
		((Control)txtCard4).Font = new Font("B Yekan", 9.75f);
		((Control)txtCard4).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtCard4).Location = new Point(225, 2);
		((Control)txtCard4).Margin = new Padding(3, 4, 3, 4);
		((TextBoxBase)txtCard4).MaxLength = 4;
		((Control)txtCard4).Name = "txtCard4";
		((Control)txtCard4).Size = new Size(53, 27);
		((Control)txtCard4).TabIndex = 151;
		txtCard4.TextAlign = (HorizontalAlignment)2;
		((Control)txtCard4).TextChanged += txtCard1_TextChanged;
		((Control)txtCard4).Enter += txtCard1_Enter;
		((Control)txtCard4).KeyDown += new KeyEventHandler(txtCard1_KeyDown);
		((Control)txtCard4).Leave += txtCard1_Leave;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(249, 250, 255);
		((Control)this).Controls.Add((Control)(object)picCopyCardNumber);
		((Control)this).Controls.Add((Control)(object)label5);
		((Control)this).Controls.Add((Control)(object)label4);
		((Control)this).Controls.Add((Control)(object)label3);
		((Control)this).Controls.Add((Control)(object)Label45);
		((Control)this).Controls.Add((Control)(object)txtCard1);
		((Control)this).Controls.Add((Control)(object)txtCard2);
		((Control)this).Controls.Add((Control)(object)txtCard3);
		((Control)this).Controls.Add((Control)(object)txtCard4);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcCardNumber";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(414, 31);
		((ISupportInitialize)picCopyCardNumber).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BehComponents.MessageBox_Items;

internal class frmYesNoCancel : Form
{
	private IContainer components;

	private PictureBox picMessage;

	private Label lblMessage;

	private ToolTip toolTip1;

	private Panel pnlButtons;

	private ButtonX btnYes;

	private ButtonX btnNo;

	private ButtonX btnCancel;

	private MessageBoxFarsiStyle _style;

	private DialogResult drMessageBox = (DialogResult)2;

	private string sFarsiYes = "  بله  ";

	private string sFarsiCancel = " انصراف ";

	private string sFarsiNo = "  خیر  ";

	private string sEnglishYes = "  Yes  ";

	private string sEnglishCancel = " Cancel ";

	private string sEnglishNo = "  No  ";

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Expected O, but got Unknown
		components = new Container();
		picMessage = new PictureBox();
		lblMessage = new Label();
		toolTip1 = new ToolTip(components);
		pnlButtons = new Panel();
		btnCancel = new ButtonX();
		btnYes = new ButtonX();
		btnNo = new ButtonX();
		((ISupportInitialize)picMessage).BeginInit();
		((Control)pnlButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)picMessage).Location = new Point(348, 28);
		((Control)picMessage).Name = "picMessage";
		((Control)picMessage).Size = new Size(44, 50);
		picMessage.SizeMode = (PictureBoxSizeMode)4;
		picMessage.TabIndex = 0;
		picMessage.TabStop = false;
		((Control)lblMessage).Location = new Point(18, 21);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).Size = new Size(324, 57);
		((Control)lblMessage).TabIndex = 0;
		((Control)lblMessage).Text = "sa";
		lblMessage.TextAlign = (ContentAlignment)16;
		((Control)pnlButtons).Controls.Add((Control)(object)btnCancel);
		((Control)pnlButtons).Controls.Add((Control)(object)btnYes);
		((Control)pnlButtons).Controls.Add((Control)(object)btnNo);
		((Control)pnlButtons).Location = new Point(0, 122);
		((Control)pnlButtons).Name = "pnlButtons";
		((Control)pnlButtons).Size = new Size(392, 58);
		((Control)pnlButtons).TabIndex = 10;
		btnCancel.BoldedForeColor = Color.Blue;
		btnCancel.HolidayForeColor = Color.Red;
		btnCancel.ImageFixedSize = new Size(0, 0);
		btnCancel.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnCancel.IsBolded = false;
		btnCancel.IsHoliday = false;
		btnCancel.IsTrailing = false;
		((Control)btnCancel).Location = new Point(194, 3);
		((Control)btnCancel).Name = "btnCancel";
		btnCancel.PushedAlways = false;
		btnCancel.SecondBorderColor = Color.Red;
		btnCancel.SecondBorderDistanceToEdge = 3f;
		btnCancel.SecondBorderWidth = 3f;
		btnCancel.ShowFirstBorder = true;
		btnCancel.ShowSecondBorder = false;
		((Control)btnCancel).Size = new Size(90, 23);
		btnCancel.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnCancel).TabIndex = 12;
		((Control)btnCancel).Text = "انصراف";
		btnCancel.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnCancel).UseVisualStyleBackColor = true;
		btnYes.BoldedForeColor = Color.Blue;
		btnYes.HolidayForeColor = Color.Red;
		btnYes.ImageFixedSize = new Size(0, 0);
		btnYes.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnYes.IsBolded = false;
		btnYes.IsHoliday = false;
		btnYes.IsTrailing = false;
		((Control)btnYes).Location = new Point(34, 3);
		((Control)btnYes).Name = "btnYes";
		btnYes.PushedAlways = false;
		btnYes.SecondBorderColor = Color.Red;
		btnYes.SecondBorderDistanceToEdge = 3f;
		btnYes.SecondBorderWidth = 3f;
		btnYes.ShowFirstBorder = true;
		btnYes.ShowSecondBorder = false;
		((Control)btnYes).Size = new Size(74, 23);
		btnYes.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnYes).TabIndex = 11;
		((Control)btnYes).Text = "بله";
		btnYes.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnYes).UseVisualStyleBackColor = true;
		btnNo.BoldedForeColor = Color.Blue;
		btnNo.HolidayForeColor = Color.Red;
		btnNo.ImageFixedSize = new Size(0, 0);
		btnNo.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnNo.IsBolded = false;
		btnNo.IsHoliday = false;
		btnNo.IsTrailing = false;
		((Control)btnNo).Location = new Point(114, 3);
		((Control)btnNo).Name = "btnNo";
		btnNo.PushedAlways = false;
		btnNo.SecondBorderColor = Color.Red;
		btnNo.SecondBorderDistanceToEdge = 3f;
		btnNo.SecondBorderWidth = 3f;
		btnNo.ShowFirstBorder = true;
		btnNo.ShowSecondBorder = false;
		((Control)btnNo).Size = new Size(74, 23);
		btnNo.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnNo).TabIndex = 10;
		((Control)btnNo).Text = "خیر";
		btnNo.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnNo).UseVisualStyleBackColor = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(404, 185);
		((Control)this).Controls.Add((Control)(object)pnlButtons);
		((Control)this).Controls.Add((Control)(object)picMessage);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmYesNoCancel";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((ISupportInitialize)picMessage).EndInit();
		((Control)pnlButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	internal frmYesNoCancel(MessageBoxFarsiStyle style)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_style = style;
		LoadStyle();
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		((Form)this).OnFormClosed(e);
		((Form)this).DialogResult = drMessageBox;
	}

	private void LoadStyle()
	{
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).RightToLeft = (RightToLeft)(_style.RightToLeftLayout ? 1 : 0);
		((Control)this).BackColor = _style.FormBackColor;
		((Control)pnlButtons).BackColor = _style.ButtonsBackColor;
		((Control)this).Text = _style.Caption;
		((Control)lblMessage).Text = _style.Message;
		((Control)lblMessage).Font = _style.Font;
		((Control)lblMessage).RightToLeft = (RightToLeft)(_style.RightToLeftMessage ? 1 : 0);
		toolTip1.Active = _style.ShowToolTip;
		picMessage.Image = MessageBoxFarsi.GetIcon(_style.Icon);
		setButtonsTextAndImage();
		setSizeAndLocations4Controls();
		setTabIndexes();
		setButtonStyle();
		((Form)this).CancelButton = (IButtonControl)(object)btnCancel;
		drMessageBox = (DialogResult)7;
		((Form)this).DialogResult = (DialogResult)0;
	}

	private void setButtonsTextAndImage()
	{
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		((ButtonBase)btnYes).Image = _style.YesButtonImage;
		((ButtonBase)btnCancel).Image = _style.CancelButtonImage;
		((ButtonBase)btnNo).Image = _style.NoButtonImage;
		ButtonX buttonX = btnYes;
		ButtonX buttonX2 = btnCancel;
		ButtonX.ImageSizeModes imageSizeModes2 = (btnNo.ImageSizeMode = ButtonX.ImageSizeModes.Zoom);
		ButtonX.ImageSizeModes imageSizeMode = (buttonX2.ImageSizeMode = imageSizeModes2);
		buttonX.ImageSizeMode = imageSizeMode;
		if (_style.EnglishButtons)
		{
			((Control)btnYes).Text = sEnglishYes;
			((Control)btnCancel).Text = sEnglishCancel;
			((Control)btnNo).Text = sEnglishNo;
			toolTip1.SetToolTip((Control)(object)btnYes, sFarsiYes);
			toolTip1.SetToolTip((Control)(object)btnCancel, sFarsiCancel);
			toolTip1.SetToolTip((Control)(object)btnNo, sFarsiNo);
			ContentAlignment val = (ContentAlignment)16;
			ButtonX buttonX3 = btnYes;
			ButtonX buttonX4 = btnCancel;
			ContentAlignment val3 = (((ButtonBase)btnNo).ImageAlign = val);
			ContentAlignment imageAlign = (((ButtonBase)buttonX4).ImageAlign = val3);
			((ButtonBase)buttonX3).ImageAlign = imageAlign;
			val = (ContentAlignment)64;
			if (((ButtonBase)btnYes).Image != null)
			{
				((ButtonBase)btnYes).TextAlign = val;
			}
			if (((ButtonBase)btnCancel).Image != null)
			{
				((ButtonBase)btnCancel).TextAlign = val;
			}
			if (((ButtonBase)btnNo).Image != null)
			{
				((ButtonBase)btnNo).TextAlign = val;
			}
			return;
		}
		((Control)btnYes).Text = sFarsiYes;
		((Control)btnCancel).Text = sFarsiCancel;
		((Control)btnNo).Text = sFarsiNo;
		toolTip1.SetToolTip((Control)(object)btnYes, sEnglishYes);
		toolTip1.SetToolTip((Control)(object)btnCancel, sEnglishCancel);
		toolTip1.SetToolTip((Control)(object)btnNo, sEnglishNo);
		ContentAlignment val5 = (ContentAlignment)64;
		if (_style.ButtonStyle == MessageBoxFarsiButtonStyles.System)
		{
			val5 = (ContentAlignment)16;
		}
		ButtonX buttonX5 = btnYes;
		ButtonX buttonX6 = btnCancel;
		ContentAlignment val7 = (((ButtonBase)btnNo).ImageAlign = val5);
		ContentAlignment imageAlign2 = (((ButtonBase)buttonX6).ImageAlign = val7);
		((ButtonBase)buttonX5).ImageAlign = imageAlign2;
		val5 = (ContentAlignment)64;
		if (((ButtonBase)btnYes).Image != null)
		{
			((ButtonBase)btnYes).TextAlign = val5;
		}
		if (((ButtonBase)btnCancel).Image != null)
		{
			((ButtonBase)btnCancel).TextAlign = val5;
		}
		if (((ButtonBase)btnNo).Image != null)
		{
			((ButtonBase)btnNo).TextAlign = val5;
		}
	}

	private void setSizeAndLocations4Controls()
	{
		int num = TextRenderer.MeasureText(((Control)btnYes).Text, ((Control)btnYes).Font).Width;
		if (((ButtonBase)btnYes).Image != null)
		{
			num += 3 + ((ButtonBase)btnYes).Image.Width;
		}
		num += 4;
		((Control)btnYes).Width = num;
		num = TextRenderer.MeasureText(((Control)btnCancel).Text, ((Control)btnCancel).Font).Width;
		if (((ButtonBase)btnCancel).Image != null)
		{
			num += 3 + ((ButtonBase)btnCancel).Image.Width;
		}
		num += 4;
		((Control)btnCancel).Width = num;
		num = TextRenderer.MeasureText(((Control)btnNo).Text, ((Control)btnNo).Font).Width;
		if (((ButtonBase)btnNo).Image != null)
		{
			num += 3 + ((ButtonBase)btnNo).Image.Width;
		}
		num += 4;
		((Control)btnNo).Width = num;
		Size size = new Size(MessageBoxFarsi.MaxWidth, MessageBoxFarsi.MaxHeight);
		Size size2 = default(Size);
		size2.Width = 12 + ((Control)btnYes).Width + 12 + ((Control)btnCancel).Width + 12 + ((Control)btnNo).Width + 12;
		size2.Height = 74;
		Size size3 = TextRenderer.MeasureText(_style.Message, ((Control)lblMessage).Font);
		((Control)lblMessage).Size = size3;
		if (_style.Icon == MessageBoxFarsiIcon.None)
		{
			((Control)picMessage).Visible = false;
			int num2 = 12 + ((Control)lblMessage).Width + 12;
			if (num2 > size.Width)
			{
				((Control)lblMessage).Width = ((Control)lblMessage).Width - (num2 - size.Width);
				int num3 = num2 / size.Width;
				num3 += 3;
				Label obj = lblMessage;
				((Control)obj).Height = ((Control)obj).Height + num3 * ((Control)lblMessage).Font.Height;
			}
			if (num2 < size2.Width)
			{
				((Control)lblMessage).Width = ((Control)lblMessage).Width + (size2.Width - num2);
			}
			int num4 = 12 + ((Control)lblMessage).Height + 12 + 50;
			if (num4 > size.Height)
			{
				((Control)lblMessage).Height = ((Control)lblMessage).Height - (num4 - size.Height);
			}
			if (num4 < size2.Height)
			{
				((Control)lblMessage).Height = ((Control)lblMessage).Height + (size2.Height - num4);
			}
			num2 = 12 + ((Control)lblMessage).Width + 12;
			num4 = 12 + ((Control)lblMessage).Height + 12 + 50;
			((Form)this).ClientSize = new Size(num2, num4);
		}
		else
		{
			((Control)picMessage).Visible = true;
			int num5 = 12 + ((Control)lblMessage).Width + 12 + ((Control)picMessage).Width + 12;
			if (num5 > size.Width)
			{
				((Control)lblMessage).Width = ((Control)lblMessage).Width - (num5 - size.Width);
				int num6 = num5 / size.Width;
				num6 += 3;
				Label obj2 = lblMessage;
				((Control)obj2).Height = ((Control)obj2).Height + num6 * ((Control)lblMessage).Font.Height;
			}
			if (num5 < size2.Width)
			{
				((Control)lblMessage).Width = ((Control)lblMessage).Width + (size2.Width - num5);
			}
			int num7 = Math.Max(((Control)lblMessage).Height, ((Control)picMessage).Height);
			int num8 = 12 + num7 + 12 + 50;
			if (num8 > size.Height)
			{
				if (((Control)lblMessage).Height >= ((Control)picMessage).Height)
				{
					((Control)lblMessage).Height = ((Control)lblMessage).Height - (num8 - size.Height);
				}
				else
				{
					((Control)picMessage).Height = ((Control)picMessage).Height - (num8 - size.Height);
				}
			}
			if (num8 < size2.Height)
			{
				((Control)lblMessage).Height = num7 + (size2.Height - num8);
			}
			num5 = 12 + ((Control)lblMessage).Width + 12 + ((Control)picMessage).Width + 12;
			num7 = Math.Max(((Control)lblMessage).Height, ((Control)picMessage).Height);
			num8 = 12 + num7 + 12 + 50;
			((Form)this).ClientSize = new Size(num5, num8);
		}
		if (_style.RightToLeftLayout)
		{
			if (_style.Icon != 0)
			{
				int num7 = Math.Max(((Control)lblMessage).Height, ((Control)picMessage).Height);
				int num9 = 12;
				int y = 12 + (num7 / 2 - ((Control)lblMessage).Height / 2);
				((Control)lblMessage).Location = new Point(num9, y);
				num9 += ((Control)lblMessage).Width + 12;
				y = 12 + (num7 / 2 - ((Control)picMessage).Height / 2);
				((Control)picMessage).Location = new Point(num9, y);
			}
			else
			{
				int num9 = 12;
				int y = 12;
				((Control)lblMessage).Location = new Point(num9, y);
			}
		}
		else if (_style.Icon != 0)
		{
			int num7 = Math.Max(((Control)lblMessage).Height, ((Control)picMessage).Height);
			int num9 = 12;
			int y = 12 + (num7 / 2 - ((Control)picMessage).Height / 2);
			((Control)picMessage).Location = new Point(num9, y);
			num9 = num9 + ((Control)picMessage).Width + 12;
			y = 12 + (num7 / 2 - ((Control)lblMessage).Height / 2);
			((Control)lblMessage).Location = new Point(num9, y);
		}
		else
		{
			int num9 = 12;
			int y = 12;
			((Control)lblMessage).Location = new Point(num9, y);
		}
		((Control)pnlButtons).Size = new Size(((Control)this).Width, 50);
		((Control)pnlButtons).Location = new Point(0, ((Form)this).ClientSize.Height - 50);
		if (_style.RightToLeftLayout)
		{
			int y = 25 - ((Control)btnYes).Height / 2;
			int num9 = 12;
			((Control)btnCancel).Location = new Point(num9, y);
			num9 = num9 + ((Control)btnCancel).Width + 12;
			((Control)btnNo).Location = new Point(num9, y);
			num9 = num9 + ((Control)btnNo).Width + 12;
			((Control)btnYes).Location = new Point(num9, y);
		}
		else
		{
			int y = 25 - ((Control)btnYes).Height / 2;
			int num9 = ((Control)pnlButtons).Width - 12 - ((Control)btnCancel).Width;
			((Control)btnCancel).Location = new Point(num9, y);
			num9 = num9 - 12 - ((Control)btnNo).Width;
			((Control)btnNo).Location = new Point(num9, y);
			num9 = num9 - 12 - ((Control)btnYes).Width;
			((Control)btnYes).Location = new Point(num9, y);
		}
		((Control)btnYes).Click += btnYes_Click;
		((Control)btnNo).Click += btnNo_Click;
		((Control)btnCancel).Click += btnCancel_Click;
	}

	private void setTabIndexes()
	{
		switch (_style.DefaultButton)
		{
		case MessageBoxFarsiDefaultButton.Button1:
			((Control)btnYes).TabIndex = 0;
			((Control)btnNo).TabIndex = 1;
			((Control)btnCancel).TabIndex = 2;
			break;
		case MessageBoxFarsiDefaultButton.Button2:
			((Control)btnYes).TabIndex = 2;
			((Control)btnNo).TabIndex = 0;
			((Control)btnCancel).TabIndex = 1;
			break;
		case MessageBoxFarsiDefaultButton.Button3:
			((Control)btnYes).TabIndex = 1;
			((Control)btnNo).TabIndex = 2;
			((Control)btnCancel).TabIndex = 0;
			break;
		}
	}

	private void setButtonStyle()
	{
		switch (_style.ButtonStyle)
		{
		case MessageBoxFarsiButtonStyles.Blue:
		{
			ButtonX buttonX5 = btnYes;
			ButtonX buttonX6 = btnCancel;
			ButtonX.ButtonStyles buttonStyles8 = (btnNo.Style = ButtonX.ButtonStyles.Blue);
			ButtonX.ButtonStyles style3 = (buttonX6.Style = buttonStyles8);
			buttonX5.Style = style3;
			break;
		}
		case MessageBoxFarsiButtonStyles.Green:
		{
			ButtonX buttonX3 = btnYes;
			ButtonX buttonX4 = btnCancel;
			ButtonX.ButtonStyles buttonStyles5 = (btnNo.Style = ButtonX.ButtonStyles.Green);
			ButtonX.ButtonStyles style2 = (buttonX4.Style = buttonStyles5);
			buttonX3.Style = style2;
			break;
		}
		case MessageBoxFarsiButtonStyles.System:
		{
			ButtonX buttonX = btnYes;
			ButtonX buttonX2 = btnCancel;
			ButtonX.ButtonStyles buttonStyles2 = (btnNo.Style = ButtonX.ButtonStyles.System);
			ButtonX.ButtonStyles style = (buttonX2.Style = buttonStyles2);
			buttonX.Style = style;
			break;
		}
		}
	}

	private void btnYes_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		drMessageBox = (DialogResult)6;
		((Form)this).Close();
	}

	private void btnNo_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		drMessageBox = (DialogResult)7;
		((Form)this).Close();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		drMessageBox = (DialogResult)2;
		((Form)this).Close();
	}
}

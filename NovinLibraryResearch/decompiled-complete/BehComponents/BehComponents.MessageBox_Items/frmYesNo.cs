using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BehComponents.MessageBox_Items;

internal class frmYesNo : Form
{
	private IContainer components;

	private PictureBox picMessage;

	private Label lblMessage;

	private ToolTip toolTip1;

	private Panel pnlButtons;

	private ButtonX btnYes;

	private ButtonX btnNo;

	private MessageBoxFarsiStyle _style;

	private DialogResult drMessageBox = (DialogResult)6;

	private string sFarsiYes = "  بله  ";

	private string sFarsiNo = "  خیر  ";

	private string sEnglishYes = "  Yes  ";

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
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Expected O, but got Unknown
		components = new Container();
		picMessage = new PictureBox();
		lblMessage = new Label();
		toolTip1 = new ToolTip(components);
		pnlButtons = new Panel();
		btnYes = new ButtonX();
		btnNo = new ButtonX();
		((ISupportInitialize)picMessage).BeginInit();
		((Control)pnlButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)picMessage).Location = new Point(342, 32);
		((Control)picMessage).Name = "picMessage";
		((Control)picMessage).Size = new Size(44, 50);
		picMessage.SizeMode = (PictureBoxSizeMode)4;
		picMessage.TabIndex = 0;
		picMessage.TabStop = false;
		((Control)lblMessage).Location = new Point(12, 9);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).Size = new Size(324, 73);
		((Control)lblMessage).TabIndex = 0;
		((Control)lblMessage).Text = "ds";
		lblMessage.TextAlign = (ContentAlignment)16;
		((Control)pnlButtons).Controls.Add((Control)(object)btnYes);
		((Control)pnlButtons).Controls.Add((Control)(object)btnNo);
		((Control)pnlButtons).Location = new Point(0, 138);
		((Control)pnlButtons).Name = "pnlButtons";
		((Control)pnlButtons).Size = new Size(392, 45);
		((Control)pnlButtons).TabIndex = 13;
		btnYes.BoldedForeColor = Color.Blue;
		btnYes.HolidayForeColor = Color.Red;
		btnYes.ImageFixedSize = new Size(0, 0);
		btnYes.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnYes.IsBolded = false;
		btnYes.IsHoliday = false;
		btnYes.IsTrailing = false;
		((Control)btnYes).Location = new Point(252, 12);
		((Control)btnYes).Name = "btnYes";
		btnYes.PushedAlways = false;
		btnYes.SecondBorderColor = Color.Red;
		btnYes.SecondBorderDistanceToEdge = 3f;
		btnYes.SecondBorderWidth = 3f;
		btnYes.ShowFirstBorder = true;
		btnYes.ShowSecondBorder = false;
		((Control)btnYes).Size = new Size(74, 23);
		btnYes.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnYes).TabIndex = 9;
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
		((Control)btnNo).Location = new Point(172, 12);
		((Control)btnNo).Name = "btnNo";
		btnNo.PushedAlways = false;
		btnNo.SecondBorderColor = Color.Red;
		btnNo.SecondBorderDistanceToEdge = 3f;
		btnNo.SecondBorderWidth = 3f;
		btnNo.ShowFirstBorder = true;
		btnNo.ShowSecondBorder = false;
		((Control)btnNo).Size = new Size(74, 23);
		btnNo.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnNo).TabIndex = 8;
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
		((Control)this).Name = "frmYesNo";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((ISupportInitialize)picMessage).EndInit();
		((Control)pnlButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	internal frmYesNo(MessageBoxFarsiStyle style)
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
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
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
		drMessageBox = (DialogResult)6;
		((Form)this).DialogResult = (DialogResult)0;
	}

	private void setButtonsTextAndImage()
	{
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		((ButtonBase)btnNo).Image = _style.NoButtonImage;
		((ButtonBase)btnYes).Image = _style.YesButtonImage;
		ButtonX buttonX = btnNo;
		ButtonX.ImageSizeModes imageSizeMode = (btnYes.ImageSizeMode = ButtonX.ImageSizeModes.Zoom);
		buttonX.ImageSizeMode = imageSizeMode;
		if (_style.EnglishButtons)
		{
			((Control)btnNo).Text = sEnglishNo;
			((Control)btnYes).Text = sEnglishYes;
			toolTip1.SetToolTip((Control)(object)btnNo, sFarsiNo);
			toolTip1.SetToolTip((Control)(object)btnYes, sFarsiYes);
			ContentAlignment val = (ContentAlignment)16;
			ButtonX buttonX2 = btnNo;
			ContentAlignment imageAlign = (((ButtonBase)btnYes).ImageAlign = val);
			((ButtonBase)buttonX2).ImageAlign = imageAlign;
			val = (ContentAlignment)64;
			if (((ButtonBase)btnNo).Image != null)
			{
				((ButtonBase)btnNo).TextAlign = val;
			}
			if (((ButtonBase)btnYes).Image != null)
			{
				((ButtonBase)btnYes).TextAlign = val;
			}
			return;
		}
		((Control)btnNo).Text = sFarsiNo;
		((Control)btnYes).Text = sFarsiYes;
		toolTip1.SetToolTip((Control)(object)btnNo, sEnglishNo);
		toolTip1.SetToolTip((Control)(object)btnYes, sEnglishYes);
		ContentAlignment val3 = (ContentAlignment)64;
		if (_style.ButtonStyle == MessageBoxFarsiButtonStyles.System)
		{
			val3 = (ContentAlignment)16;
		}
		ButtonX buttonX3 = btnNo;
		ContentAlignment imageAlign2 = (((ButtonBase)btnYes).ImageAlign = val3);
		((ButtonBase)buttonX3).ImageAlign = imageAlign2;
		val3 = (ContentAlignment)64;
		if (((ButtonBase)btnNo).Image != null)
		{
			((ButtonBase)btnNo).TextAlign = val3;
		}
		if (((ButtonBase)btnYes).Image != null)
		{
			((ButtonBase)btnYes).TextAlign = val3;
		}
	}

	private void setSizeAndLocations4Controls()
	{
		int num = TextRenderer.MeasureText(((Control)btnNo).Text, ((Control)btnNo).Font).Width;
		if (((ButtonBase)btnNo).Image != null)
		{
			num += 3 + ((ButtonBase)btnNo).Image.Width;
		}
		num += 4;
		((Control)btnNo).Width = num;
		num = TextRenderer.MeasureText(((Control)btnYes).Text, ((Control)btnYes).Font).Width;
		if (((ButtonBase)btnYes).Image != null)
		{
			num += 3 + ((ButtonBase)btnYes).Image.Width;
		}
		num += 4;
		((Control)btnYes).Width = num;
		Size size = new Size(MessageBoxFarsi.MaxWidth, MessageBoxFarsi.MaxHeight);
		Size size2 = default(Size);
		size2.Width = 12 + ((Control)btnNo).Width + 12 + ((Control)btnYes).Width + 12;
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
			((Control)btnNo).Location = new Point(num9, y);
			num9 = num9 + ((Control)btnNo).Width + 12;
			((Control)btnYes).Location = new Point(num9, y);
		}
		else
		{
			int y = 25 - ((Control)btnYes).Height / 2;
			int num9 = ((Control)pnlButtons).Width - 12 - ((Control)btnNo).Width;
			((Control)btnNo).Location = new Point(num9, y);
			num9 = num9 - 12 - ((Control)btnYes).Width;
			((Control)btnYes).Location = new Point(num9, y);
		}
		((Control)btnYes).Click += btnYes_Click;
		((Control)btnNo).Click += btnNo_Click;
	}

	private void setTabIndexes()
	{
		switch (_style.DefaultButton)
		{
		case MessageBoxFarsiDefaultButton.Button1:
			((Control)btnYes).TabIndex = 0;
			((Control)btnNo).TabIndex = 1;
			break;
		case MessageBoxFarsiDefaultButton.Button2:
			((Control)btnYes).TabIndex = 1;
			((Control)btnNo).TabIndex = 0;
			break;
		case MessageBoxFarsiDefaultButton.Button3:
			((Control)btnYes).TabIndex = 1;
			((Control)btnNo).TabIndex = 0;
			break;
		}
	}

	private void setButtonStyle()
	{
		switch (_style.ButtonStyle)
		{
		case MessageBoxFarsiButtonStyles.Blue:
		{
			ButtonX buttonX3 = btnNo;
			ButtonX.ButtonStyles style3 = (btnYes.Style = ButtonX.ButtonStyles.Blue);
			buttonX3.Style = style3;
			break;
		}
		case MessageBoxFarsiButtonStyles.Green:
		{
			ButtonX buttonX2 = btnNo;
			ButtonX.ButtonStyles style2 = (btnYes.Style = ButtonX.ButtonStyles.Green);
			buttonX2.Style = style2;
			break;
		}
		case MessageBoxFarsiButtonStyles.System:
		{
			ButtonX buttonX = btnNo;
			ButtonX.ButtonStyles style = (btnYes.Style = ButtonX.ButtonStyles.System);
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
}

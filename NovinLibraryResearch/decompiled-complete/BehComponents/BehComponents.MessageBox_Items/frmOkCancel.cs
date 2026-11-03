using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BehComponents.MessageBox_Items;

internal class frmOkCancel : Form
{
	private IContainer components;

	private PictureBox picMessage;

	private Label lblMessage;

	private ToolTip toolTip1;

	private Panel pnlButtons;

	private ButtonX btnOK;

	private ButtonX btnCancel;

	private MessageBoxFarsiStyle _style;

	private DialogResult drMessageBox = (DialogResult)2;

	private string sFarsiOK = " تایید ";

	private string sFarsiCancel = " انصراف ";

	private string sEnglishOK = "  OK  ";

	private string sEnglishCancel = "Cancel";

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
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Expected O, but got Unknown
		components = new Container();
		picMessage = new PictureBox();
		lblMessage = new Label();
		toolTip1 = new ToolTip(components);
		pnlButtons = new Panel();
		btnOK = new ButtonX();
		btnCancel = new ButtonX();
		((ISupportInitialize)picMessage).BeginInit();
		((Control)pnlButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)picMessage).Location = new Point(354, 12);
		((Control)picMessage).Name = "picMessage";
		((Control)picMessage).Size = new Size(44, 50);
		picMessage.SizeMode = (PictureBoxSizeMode)4;
		picMessage.TabIndex = 0;
		picMessage.TabStop = false;
		((Control)lblMessage).Location = new Point(24, 9);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).Size = new Size(324, 48);
		((Control)lblMessage).TabIndex = 0;
		((Control)lblMessage).Text = "sa";
		lblMessage.TextAlign = (ContentAlignment)16;
		((Control)pnlButtons).Controls.Add((Control)(object)btnOK);
		((Control)pnlButtons).Controls.Add((Control)(object)btnCancel);
		((Control)pnlButtons).Location = new Point(6, 70);
		((Control)pnlButtons).Name = "pnlButtons";
		((Control)pnlButtons).Size = new Size(392, 45);
		((Control)pnlButtons).TabIndex = 11;
		btnOK.BoldedForeColor = Color.Blue;
		btnOK.HolidayForeColor = Color.Red;
		btnOK.ImageFixedSize = new Size(0, 0);
		btnOK.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnOK.IsBolded = false;
		btnOK.IsHoliday = false;
		btnOK.IsTrailing = false;
		((Control)btnOK).Location = new Point(286, 3);
		((Control)btnOK).Name = "btnOK";
		btnOK.PushedAlways = false;
		btnOK.SecondBorderColor = Color.Red;
		btnOK.SecondBorderDistanceToEdge = 3f;
		btnOK.SecondBorderWidth = 3f;
		btnOK.ShowFirstBorder = true;
		btnOK.ShowSecondBorder = false;
		((Control)btnOK).Size = new Size(80, 23);
		btnOK.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnOK).TabIndex = 7;
		((Control)btnOK).Text = "تایید";
		btnOK.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnOK).UseVisualStyleBackColor = true;
		btnCancel.BoldedForeColor = Color.Blue;
		btnCancel.HolidayForeColor = Color.Red;
		btnCancel.ImageFixedSize = new Size(0, 0);
		btnCancel.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnCancel.IsBolded = false;
		btnCancel.IsHoliday = false;
		btnCancel.IsTrailing = false;
		((Control)btnCancel).Location = new Point(194, 19);
		((Control)btnCancel).Name = "btnCancel";
		btnCancel.PushedAlways = false;
		btnCancel.SecondBorderColor = Color.Red;
		btnCancel.SecondBorderDistanceToEdge = 3f;
		btnCancel.SecondBorderWidth = 3f;
		btnCancel.ShowFirstBorder = true;
		btnCancel.ShowSecondBorder = false;
		((Control)btnCancel).Size = new Size(80, 23);
		btnCancel.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnCancel).TabIndex = 6;
		((Control)btnCancel).Text = "انصراف";
		btnCancel.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnCancel).UseVisualStyleBackColor = true;
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
		((Control)this).Name = "frmOkCancel";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((ISupportInitialize)picMessage).EndInit();
		((Control)pnlButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	internal frmOkCancel(MessageBoxFarsiStyle style)
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
		drMessageBox = (DialogResult)1;
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
		((ButtonBase)btnCancel).Image = _style.CancelButtonImage;
		((ButtonBase)btnOK).Image = _style.OkButtonImage;
		ButtonX buttonX = btnCancel;
		ButtonX.ImageSizeModes imageSizeMode = (btnOK.ImageSizeMode = ButtonX.ImageSizeModes.Zoom);
		buttonX.ImageSizeMode = imageSizeMode;
		if (_style.EnglishButtons)
		{
			((Control)btnCancel).Text = sEnglishCancel;
			((Control)btnOK).Text = sEnglishOK;
			toolTip1.SetToolTip((Control)(object)btnCancel, sFarsiCancel);
			toolTip1.SetToolTip((Control)(object)btnOK, sFarsiOK);
			ContentAlignment val = (ContentAlignment)16;
			ButtonX buttonX2 = btnCancel;
			ContentAlignment imageAlign = (((ButtonBase)btnOK).ImageAlign = val);
			((ButtonBase)buttonX2).ImageAlign = imageAlign;
			val = (ContentAlignment)64;
			if (((ButtonBase)btnCancel).Image != null)
			{
				((ButtonBase)btnCancel).TextAlign = val;
			}
			if (((ButtonBase)btnOK).Image != null)
			{
				((ButtonBase)btnOK).TextAlign = val;
			}
			return;
		}
		((Control)btnCancel).Text = sFarsiCancel;
		((Control)btnOK).Text = sFarsiOK;
		toolTip1.SetToolTip((Control)(object)btnCancel, sEnglishCancel);
		toolTip1.SetToolTip((Control)(object)btnOK, sEnglishOK);
		ContentAlignment val3 = (ContentAlignment)64;
		if (_style.ButtonStyle == MessageBoxFarsiButtonStyles.System)
		{
			val3 = (ContentAlignment)16;
		}
		ButtonX buttonX3 = btnCancel;
		ContentAlignment imageAlign2 = (((ButtonBase)btnOK).ImageAlign = val3);
		((ButtonBase)buttonX3).ImageAlign = imageAlign2;
		val3 = (ContentAlignment)64;
		if (((ButtonBase)btnCancel).Image != null)
		{
			((ButtonBase)btnCancel).TextAlign = val3;
		}
		if (((ButtonBase)btnOK).Image != null)
		{
			((ButtonBase)btnOK).TextAlign = val3;
		}
	}

	private void setSizeAndLocations4Controls()
	{
		int num = TextRenderer.MeasureText(((Control)btnCancel).Text, ((Control)btnCancel).Font).Width;
		if (((ButtonBase)btnCancel).Image != null)
		{
			num += 3 + ((ButtonBase)btnCancel).Image.Width;
		}
		num += 4;
		((Control)btnCancel).Width = num;
		num = TextRenderer.MeasureText(((Control)btnOK).Text, ((Control)btnOK).Font).Width;
		if (((ButtonBase)btnOK).Image != null)
		{
			num += 3 + ((ButtonBase)btnOK).Image.Width;
		}
		num += 4;
		((Control)btnOK).Width = num;
		Size size = new Size(MessageBoxFarsi.MaxWidth, MessageBoxFarsi.MaxHeight);
		Size size2 = default(Size);
		size2.Width = 12 + ((Control)btnCancel).Width + 12 + ((Control)btnOK).Width + 12;
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
			int y = 25 - ((Control)btnOK).Height / 2;
			int num9 = 12;
			((Control)btnCancel).Location = new Point(num9, y);
			num9 = num9 + ((Control)btnCancel).Width + 12;
			((Control)btnOK).Location = new Point(num9, y);
		}
		else
		{
			int y = 25 - ((Control)btnOK).Height / 2;
			int num9 = ((Control)pnlButtons).Width - 12 - ((Control)btnCancel).Width;
			((Control)btnCancel).Location = new Point(num9, y);
			num9 = num9 - 12 - ((Control)btnOK).Width;
			((Control)btnOK).Location = new Point(num9, y);
		}
		((Control)btnOK).Click += btnOK_Click;
		((Control)btnCancel).Click += btnCancel_Click;
	}

	private void setTabIndexes()
	{
		switch (_style.DefaultButton)
		{
		case MessageBoxFarsiDefaultButton.Button1:
			((Control)btnOK).TabIndex = 0;
			((Control)btnCancel).TabIndex = 1;
			break;
		case MessageBoxFarsiDefaultButton.Button2:
			((Control)btnOK).TabIndex = 1;
			((Control)btnCancel).TabIndex = 0;
			break;
		case MessageBoxFarsiDefaultButton.Button3:
			((Control)btnOK).TabIndex = 1;
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
			ButtonX buttonX3 = btnCancel;
			ButtonX.ButtonStyles style3 = (btnOK.Style = ButtonX.ButtonStyles.Blue);
			buttonX3.Style = style3;
			break;
		}
		case MessageBoxFarsiButtonStyles.Green:
		{
			ButtonX buttonX2 = btnCancel;
			ButtonX.ButtonStyles style2 = (btnOK.Style = ButtonX.ButtonStyles.Green);
			buttonX2.Style = style2;
			break;
		}
		case MessageBoxFarsiButtonStyles.System:
		{
			ButtonX buttonX = btnCancel;
			ButtonX.ButtonStyles style = (btnOK.Style = ButtonX.ButtonStyles.System);
			buttonX.Style = style;
			break;
		}
		}
	}

	private void btnOK_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		drMessageBox = (DialogResult)1;
		((Form)this).Close();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		drMessageBox = (DialogResult)2;
		((Form)this).Close();
	}
}

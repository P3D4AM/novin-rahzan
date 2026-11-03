using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BehComponents.MessageBox_Items;

internal class frmAbortRetryIgnore : Form
{
	private IContainer components;

	private ButtonX btnIgnore;

	private Panel pnlButtons;

	private ButtonX btnRetry;

	private ButtonX btnAbort;

	private ToolTip toolTip1;

	private PictureBox picMessage;

	private Label lblMessage;

	private MessageBoxFarsiStyle _style;

	private DialogResult drMessageBox;

	private string sFarsiAbort = "لغو عملیات";

	private string sFarsiIgnore = "چشم پوشی کن";

	private string sFarsiRetry = "دوباره سعی کن";

	private string sEnglishAbort = " Abort ";

	private string sEnglishIgnore = " Ignore ";

	private string sEnglishRetry = " Retry ";

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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Expected O, but got Unknown
		components = new Container();
		btnIgnore = new ButtonX();
		pnlButtons = new Panel();
		btnRetry = new ButtonX();
		btnAbort = new ButtonX();
		lblMessage = new Label();
		toolTip1 = new ToolTip(components);
		picMessage = new PictureBox();
		((Control)pnlButtons).SuspendLayout();
		((ISupportInitialize)picMessage).BeginInit();
		((Control)this).SuspendLayout();
		btnIgnore.BoldedForeColor = Color.Blue;
		btnIgnore.HolidayForeColor = Color.Red;
		btnIgnore.ImageFixedSize = new Size(0, 0);
		btnIgnore.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnIgnore.IsBolded = false;
		btnIgnore.IsHoliday = false;
		btnIgnore.IsTrailing = false;
		((Control)btnIgnore).Location = new Point(12, 9);
		((Control)btnIgnore).Name = "btnIgnore";
		btnIgnore.PushedAlways = false;
		btnIgnore.SecondBorderColor = Color.Red;
		btnIgnore.SecondBorderDistanceToEdge = 3f;
		btnIgnore.SecondBorderWidth = 3f;
		btnIgnore.ShowFirstBorder = true;
		btnIgnore.ShowSecondBorder = false;
		((Control)btnIgnore).Size = new Size(106, 23);
		btnIgnore.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnIgnore).TabIndex = 3;
		((Control)btnIgnore).Text = "چشم پوشی کن";
		btnIgnore.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnIgnore).UseVisualStyleBackColor = true;
		((Control)pnlButtons).Controls.Add((Control)(object)btnIgnore);
		((Control)pnlButtons).Controls.Add((Control)(object)btnRetry);
		((Control)pnlButtons).Controls.Add((Control)(object)btnAbort);
		((Control)pnlButtons).Location = new Point(12, 66);
		((Control)pnlButtons).Name = "pnlButtons";
		((Control)pnlButtons).Size = new Size(392, 63);
		((Control)pnlButtons).TabIndex = 12;
		btnRetry.BoldedForeColor = Color.Blue;
		btnRetry.HolidayForeColor = Color.Red;
		btnRetry.ImageFixedSize = new Size(0, 0);
		btnRetry.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnRetry.IsBolded = false;
		btnRetry.IsHoliday = false;
		btnRetry.IsTrailing = false;
		((Control)btnRetry).Location = new Point(124, 9);
		((Control)btnRetry).Name = "btnRetry";
		btnRetry.PushedAlways = false;
		btnRetry.SecondBorderColor = Color.Red;
		btnRetry.SecondBorderDistanceToEdge = 3f;
		btnRetry.SecondBorderWidth = 3f;
		btnRetry.ShowFirstBorder = true;
		btnRetry.ShowSecondBorder = false;
		((Control)btnRetry).Size = new Size(108, 23);
		btnRetry.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnRetry).TabIndex = 3;
		((Control)btnRetry).Text = "دوباره سعی کن";
		btnRetry.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnRetry).UseVisualStyleBackColor = true;
		btnAbort.BoldedForeColor = Color.Blue;
		btnAbort.HolidayForeColor = Color.Red;
		btnAbort.ImageFixedSize = new Size(0, 0);
		btnAbort.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnAbort.IsBolded = false;
		btnAbort.IsHoliday = false;
		btnAbort.IsTrailing = false;
		((Control)btnAbort).Location = new Point(238, 9);
		((Control)btnAbort).Name = "btnAbort";
		btnAbort.PushedAlways = false;
		btnAbort.SecondBorderColor = Color.Red;
		btnAbort.SecondBorderDistanceToEdge = 3f;
		btnAbort.SecondBorderWidth = 3f;
		btnAbort.ShowFirstBorder = true;
		btnAbort.ShowSecondBorder = false;
		((Control)btnAbort).Size = new Size(80, 23);
		btnAbort.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnAbort).TabIndex = 3;
		((Control)btnAbort).Text = "انصراف";
		btnAbort.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnAbort).UseVisualStyleBackColor = true;
		((Control)lblMessage).Anchor = (AnchorStyles)1;
		((Control)lblMessage).Location = new Point(12, 10);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).Size = new Size(324, 53);
		((Control)lblMessage).TabIndex = 10;
		((Control)lblMessage).Text = "text\r\ndf\r\n";
		lblMessage.TextAlign = (ContentAlignment)16;
		((Control)picMessage).Anchor = (AnchorStyles)1;
		((Control)picMessage).Location = new Point(342, 10);
		((Control)picMessage).Name = "picMessage";
		((Control)picMessage).Size = new Size(50, 50);
		picMessage.SizeMode = (PictureBoxSizeMode)4;
		picMessage.TabIndex = 11;
		picMessage.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(404, 131);
		((Control)this).Controls.Add((Control)(object)pnlButtons);
		((Control)this).Controls.Add((Control)(object)picMessage);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Font = new Font("Tahoma", 8.25f);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmAbortRetryIgnore2";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)pnlButtons).ResumeLayout(false);
		((ISupportInitialize)picMessage).EndInit();
		((Control)this).ResumeLayout(false);
	}

	internal frmAbortRetryIgnore(MessageBoxFarsiStyle style)
	{
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
		drMessageBox = (DialogResult)4;
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
		((ButtonBase)btnAbort).Image = _style.AbortButtonImage;
		((ButtonBase)btnIgnore).Image = _style.IgnoreButtonImage;
		((ButtonBase)btnRetry).Image = _style.RetryButtonImage;
		ButtonX buttonX = btnAbort;
		ButtonX buttonX2 = btnIgnore;
		ButtonX.ImageSizeModes imageSizeModes2 = (btnRetry.ImageSizeMode = ButtonX.ImageSizeModes.Zoom);
		ButtonX.ImageSizeModes imageSizeMode = (buttonX2.ImageSizeMode = imageSizeModes2);
		buttonX.ImageSizeMode = imageSizeMode;
		if (_style.EnglishButtons)
		{
			((Control)btnAbort).Text = sEnglishAbort;
			((Control)btnIgnore).Text = sEnglishIgnore;
			((Control)btnRetry).Text = sEnglishRetry;
			toolTip1.SetToolTip((Control)(object)btnAbort, sFarsiAbort);
			toolTip1.SetToolTip((Control)(object)btnIgnore, sFarsiIgnore);
			toolTip1.SetToolTip((Control)(object)btnRetry, sFarsiRetry);
			ContentAlignment val = (ContentAlignment)16;
			ButtonX buttonX3 = btnAbort;
			ButtonX buttonX4 = btnIgnore;
			ContentAlignment val3 = (((ButtonBase)btnRetry).ImageAlign = val);
			ContentAlignment imageAlign = (((ButtonBase)buttonX4).ImageAlign = val3);
			((ButtonBase)buttonX3).ImageAlign = imageAlign;
			val = (ContentAlignment)64;
			if (((ButtonBase)btnAbort).Image != null)
			{
				((ButtonBase)btnAbort).TextAlign = val;
			}
			if (((ButtonBase)btnIgnore).Image != null)
			{
				((ButtonBase)btnIgnore).TextAlign = val;
			}
			if (((ButtonBase)btnRetry).Image != null)
			{
				((ButtonBase)btnRetry).TextAlign = val;
			}
			return;
		}
		((Control)btnAbort).Text = sFarsiAbort;
		((Control)btnIgnore).Text = sFarsiIgnore;
		((Control)btnRetry).Text = sFarsiRetry;
		toolTip1.SetToolTip((Control)(object)btnAbort, sEnglishAbort);
		toolTip1.SetToolTip((Control)(object)btnIgnore, sEnglishIgnore);
		toolTip1.SetToolTip((Control)(object)btnRetry, sEnglishRetry);
		ContentAlignment val5 = (ContentAlignment)64;
		if (_style.ButtonStyle == MessageBoxFarsiButtonStyles.System)
		{
			val5 = (ContentAlignment)16;
		}
		ButtonX buttonX5 = btnAbort;
		ButtonX buttonX6 = btnIgnore;
		ContentAlignment val7 = (((ButtonBase)btnRetry).ImageAlign = val5);
		ContentAlignment imageAlign2 = (((ButtonBase)buttonX6).ImageAlign = val7);
		((ButtonBase)buttonX5).ImageAlign = imageAlign2;
		val5 = (ContentAlignment)64;
		if (((ButtonBase)btnAbort).Image != null)
		{
			((ButtonBase)btnAbort).TextAlign = val5;
		}
		if (((ButtonBase)btnIgnore).Image != null)
		{
			((ButtonBase)btnIgnore).TextAlign = val5;
		}
		if (((ButtonBase)btnRetry).Image != null)
		{
			((ButtonBase)btnRetry).TextAlign = val5;
		}
	}

	private void setSizeAndLocations4Controls()
	{
		int num = TextRenderer.MeasureText(((Control)btnAbort).Text, ((Control)btnAbort).Font).Width;
		if (((ButtonBase)btnAbort).Image != null)
		{
			num += 3 + ((ButtonBase)btnAbort).Image.Width;
		}
		num += 4;
		((Control)btnAbort).Width = num;
		num = TextRenderer.MeasureText(((Control)btnIgnore).Text, ((Control)btnIgnore).Font).Width;
		if (((ButtonBase)btnIgnore).Image != null)
		{
			num += 3 + ((ButtonBase)btnIgnore).Image.Width;
		}
		num += 4;
		((Control)btnIgnore).Width = num;
		num = TextRenderer.MeasureText(((Control)btnRetry).Text, ((Control)btnRetry).Font).Width;
		if (((ButtonBase)btnRetry).Image != null)
		{
			num += 3 + ((ButtonBase)btnRetry).Image.Width;
		}
		num += 4;
		((Control)btnRetry).Width = num;
		Size size = new Size(MessageBoxFarsi.MaxWidth, MessageBoxFarsi.MaxHeight);
		Size size2 = default(Size);
		size2.Width = 12 + ((Control)btnAbort).Width + 12 + ((Control)btnIgnore).Width + 12 + ((Control)btnRetry).Width + 12;
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
			int y = 25 - ((Control)btnAbort).Height / 2;
			int num9 = 12;
			((Control)btnIgnore).Location = new Point(num9, y);
			num9 = num9 + ((Control)btnIgnore).Width + 12;
			((Control)btnRetry).Location = new Point(num9, y);
			num9 = num9 + ((Control)btnRetry).Width + 12;
			((Control)btnAbort).Location = new Point(num9, y);
		}
		else
		{
			int y = 25 - ((Control)btnAbort).Height / 2;
			int num9 = ((Control)pnlButtons).Width - 12 - ((Control)btnIgnore).Width;
			((Control)btnIgnore).Location = new Point(num9, y);
			num9 = num9 - 12 - ((Control)btnRetry).Width;
			((Control)btnRetry).Location = new Point(num9, y);
			num9 = num9 - 12 - ((Control)btnAbort).Width;
			((Control)btnAbort).Location = new Point(num9, y);
		}
		((Control)btnAbort).Click += btnAbort_Click;
		((Control)btnRetry).Click += btnRetry_Click;
		((Control)btnIgnore).Click += btnIgnore_Click;
	}

	private void setTabIndexes()
	{
		switch (_style.DefaultButton)
		{
		case MessageBoxFarsiDefaultButton.Button1:
			((Control)btnAbort).TabIndex = 0;
			((Control)btnRetry).TabIndex = 1;
			((Control)btnIgnore).TabIndex = 2;
			break;
		case MessageBoxFarsiDefaultButton.Button2:
			((Control)btnAbort).TabIndex = 2;
			((Control)btnRetry).TabIndex = 0;
			((Control)btnIgnore).TabIndex = 1;
			break;
		case MessageBoxFarsiDefaultButton.Button3:
			((Control)btnAbort).TabIndex = 1;
			((Control)btnRetry).TabIndex = 2;
			((Control)btnIgnore).TabIndex = 0;
			break;
		}
	}

	private void setButtonStyle()
	{
		switch (_style.ButtonStyle)
		{
		case MessageBoxFarsiButtonStyles.Blue:
		{
			ButtonX buttonX5 = btnAbort;
			ButtonX buttonX6 = btnIgnore;
			ButtonX.ButtonStyles buttonStyles8 = (btnRetry.Style = ButtonX.ButtonStyles.Blue);
			ButtonX.ButtonStyles style3 = (buttonX6.Style = buttonStyles8);
			buttonX5.Style = style3;
			break;
		}
		case MessageBoxFarsiButtonStyles.Green:
		{
			ButtonX buttonX3 = btnAbort;
			ButtonX buttonX4 = btnIgnore;
			ButtonX.ButtonStyles buttonStyles5 = (btnRetry.Style = ButtonX.ButtonStyles.Green);
			ButtonX.ButtonStyles style2 = (buttonX4.Style = buttonStyles5);
			buttonX3.Style = style2;
			break;
		}
		case MessageBoxFarsiButtonStyles.System:
		{
			ButtonX buttonX = btnAbort;
			ButtonX buttonX2 = btnIgnore;
			ButtonX.ButtonStyles buttonStyles2 = (btnRetry.Style = ButtonX.ButtonStyles.System);
			ButtonX.ButtonStyles style = (buttonX2.Style = buttonStyles2);
			buttonX.Style = style;
			break;
		}
		}
	}

	private void btnAbort_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		drMessageBox = (DialogResult)3;
		((Form)this).Close();
	}

	private void btnRetry_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		drMessageBox = (DialogResult)4;
		((Form)this).Close();
	}

	private void btnIgnore_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		drMessageBox = (DialogResult)5;
		((Form)this).Close();
	}
}

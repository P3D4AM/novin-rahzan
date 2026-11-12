using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class TaskDialogForm : OfficeForm
{
	private eTaskDialogBackgroundColor _TaskBackgroundColor;

	private eTaskDialogButton _DefaultButton = eTaskDialogButton.Yes;

	private static readonly int MinimumWidth = 420;

	private static readonly int MinimumHeight = 200;

	private eTaskDialogResult _Result;

	private bool _AntiAlias = true;

	private IContainer components;

	private PictureBox headerImage;

	private LabelX headerLabel;

	private LabelX contentLabel;

	private ItemPanel buttonsPanel;

	private PanelEx bottomPanel;

	private ButtonX buttonOk;

	private ButtonX buttonClose;

	private ButtonX buttonRetry;

	private ButtonX buttonCancel;

	private ButtonX buttonNo;

	private ButtonX buttonYes;

	private CheckBoxX taskCheckBox;

	private LabelX footerLabel;

	private PictureBox footerImage;

	private PanelEx footerPanel;

	private FlowLayoutPanel flowLayoutPanel1;

	[DefaultValue(eTaskDialogBackgroundColor.Default)]
	public eTaskDialogBackgroundColor TaskBackgroundColor
	{
		get
		{
			return _TaskBackgroundColor;
		}
		set
		{
			_TaskBackgroundColor = value;
		}
	}

	public eTaskDialogResult Result => _Result;

	public bool AntiAlias
	{
		get
		{
			return _AntiAlias;
		}
		set
		{
			_AntiAlias = value;
			headerLabel.AntiAlias = value;
			contentLabel.AntiAlias = value;
			buttonsPanel.AntiAlias = value;
			buttonOk.AntiAlias = value;
			buttonCancel.AntiAlias = value;
			buttonYes.AntiAlias = value;
			buttonNo.AntiAlias = value;
			buttonRetry.AntiAlias = value;
			footerLabel.AntiAlias = value;
			taskCheckBox.AntiAlias = value;
		}
	}

	public TaskDialogForm()
	{
		InitializeComponent();
		Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
		bottomPanel.Style.BackColor1.Color = colorTable.Form.BackColor;
		headerLabel.MarkupLinkClick += MarkupLinkClick;
		contentLabel.MarkupLinkClick += MarkupLinkClick;
		footerLabel.MarkupLinkClick += MarkupLinkClick;
		((Form)this).ShowInTaskbar = false;
	}

	private void MarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
	{
		TaskDialog.InvokeMarkupLinkClick(sender, e);
	}

	private void TaskDialogForm_Load(object sender, EventArgs e)
	{
	}

	public eTaskDialogResult ShowTaskDialog(IWin32Window owner, TaskDialogInfo dialogInfo)
	{
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		if (owner != null)
		{
			((Form)this).StartPosition = (FormStartPosition)4;
		}
		else
		{
			((Form)this).StartPosition = (FormStartPosition)1;
		}
		UpdateDialogColor(dialogInfo);
		int num = 20;
		int num2 = 0;
		int num3 = 32;
		((Control)this).Text = dialogInfo.Title;
		((Control)headerLabel).Text = dialogInfo.Header;
		((Control)contentLabel).Text = dialogInfo.Text;
		_DefaultButton = dialogInfo.DefaultButton;
		if (dialogInfo.TaskDialogIcon == eTaskDialogIcon.None)
		{
			int num4 = ((Control)this).Width - ((Control)headerLabel).Right;
			((Control)headerLabel).Left = ((Control)headerImage).Left;
			((Control)headerLabel).Width = ((Control)this).Width - num4 - ((Control)headerLabel).Left;
			num4 = ((Control)this).Width - ((Control)contentLabel).Right;
			((Control)contentLabel).Left = ((Control)headerImage).Left;
			((Control)contentLabel).Width = ((Control)this).Width - num4 - ((Control)contentLabel).Left;
			num4 = ((Control)this).Width - ((Control)buttonsPanel).Right;
			((Control)buttonsPanel).Left = ((Control)headerImage).Left;
			((Control)buttonsPanel).Width = ((Control)this).Width - num4 - ((Control)buttonsPanel).Left;
			((Control)headerImage).Visible = false;
		}
		else
		{
			headerImage.Image = TaskDialog.GetImage(dialogInfo.TaskDialogIcon);
		}
		if (dialogInfo.CheckBoxCommand != null)
		{
			taskCheckBox.Command = dialogInfo.CheckBoxCommand;
			num += ((Control)taskCheckBox).Width + 12;
		}
		else
		{
			((Control)taskCheckBox).Visible = false;
		}
		if (!string.IsNullOrEmpty(dialogInfo.FooterText))
		{
			((Control)footerLabel).Text = dialogInfo.FooterText;
			num2 += ((Control)footerLabel).Width + 12;
		}
		else
		{
			((Control)footerLabel).Visible = false;
		}
		if (dialogInfo.FooterImage != null)
		{
			footerImage.Image = dialogInfo.FooterImage;
			num2 += ((Control)footerImage).Width;
		}
		else
		{
			((Control)footerImage).Visible = false;
			((Control)footerLabel).Left = ((Control)footerImage).Left;
		}
		if (dialogInfo.RadioButtons != null && dialogInfo.RadioButtons.Length != 0)
		{
			Command[] radioButtons = dialogInfo.RadioButtons;
			foreach (Command command in radioButtons)
			{
				CheckBoxItem checkBoxItem = new CheckBoxItem();
				checkBoxItem.CheckBoxStyle = eCheckBoxStyle.RadioButton;
				checkBoxItem.Command = command;
				buttonsPanel.Items.Add(checkBoxItem);
			}
		}
		if (dialogInfo.Buttons != null && dialogInfo.Buttons.Length != 0)
		{
			Command[] buttons = dialogInfo.Buttons;
			foreach (Command command2 in buttons)
			{
				ButtonItem buttonItem = new ButtonItem();
				if (command2.Image != null)
				{
					buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
					buttonItem.ImagePosition = eImagePosition.Left;
				}
				buttonItem.Command = command2;
				buttonsPanel.Items.Add(buttonItem);
				ButtonItemLayout.LayoutButton(buttonItem);
				num2 = Math.Max(num2, buttonItem.WidthInternal + ((Control)buttonsPanel).Left * 2);
			}
		}
		if ((dialogInfo.DialogButtons & eTaskDialogButton.Ok) == 0)
		{
			((Control)buttonOk).Visible = false;
		}
		else
		{
			num += ((Control)buttonOk).Width + 3;
			if (dialogInfo.DefaultButton == eTaskDialogButton.Ok)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
			}
		}
		if ((dialogInfo.DialogButtons & eTaskDialogButton.Cancel) == 0)
		{
			((Control)buttonCancel).Visible = false;
		}
		else
		{
			num += ((Control)buttonCancel).Width + 3;
			((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
			if (dialogInfo.DefaultButton == eTaskDialogButton.Cancel)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)buttonCancel;
			}
		}
		if ((dialogInfo.DialogButtons & eTaskDialogButton.Yes) == 0)
		{
			((Control)buttonYes).Visible = false;
		}
		else
		{
			num += ((Control)buttonYes).Width + 3;
			if (dialogInfo.DefaultButton == eTaskDialogButton.Yes)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)buttonYes;
			}
		}
		if ((dialogInfo.DialogButtons & eTaskDialogButton.No) == 0)
		{
			((Control)buttonNo).Visible = false;
		}
		else
		{
			num += ((Control)buttonNo).Width + 3;
			if (((Form)this).CancelButton == null)
			{
				((Form)this).CancelButton = (IButtonControl)(object)buttonNo;
			}
			if (dialogInfo.DefaultButton == eTaskDialogButton.No)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)buttonNo;
			}
		}
		if ((dialogInfo.DialogButtons & eTaskDialogButton.Close) == 0)
		{
			((Control)buttonClose).Visible = false;
		}
		else
		{
			num += ((Control)buttonClose).Width + 3;
			if (((Form)this).CancelButton == null)
			{
				((Form)this).CancelButton = (IButtonControl)(object)buttonClose;
			}
			else if (dialogInfo.DefaultButton == eTaskDialogButton.Close)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)buttonClose;
			}
		}
		if ((dialogInfo.DialogButtons & eTaskDialogButton.Retry) == 0)
		{
			((Control)buttonRetry).Visible = false;
		}
		else
		{
			num += ((Control)buttonRetry).Width + 3;
			if (dialogInfo.DefaultButton == eTaskDialogButton.Retry)
			{
				((Form)this).AcceptButton = (IButtonControl)(object)buttonRetry;
			}
		}
		if (dialogInfo.DialogButtons == eTaskDialogButton.Ok)
		{
			((Form)this).CancelButton = (IButtonControl)(object)buttonOk;
		}
		if (string.IsNullOrEmpty(dialogInfo.FooterText) && dialogInfo.FooterImage == null)
		{
			((Control)footerPanel).Visible = false;
			((Control)bottomPanel).Height = ((Control)flowLayoutPanel1).Height + 4;
		}
		else
		{
			num3 += ((Control)footerImage).Height;
		}
		((Control)this).Width = Math.Max(MinimumWidth, Math.Max(num2, num));
		Graphics val = headerLabel.CreateGraphics();
		try
		{
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		((Control)headerLabel).MaximumSize = new Size(((Control)headerLabel).Width, 1000);
		((Control)headerLabel).Height = ((Control)headerLabel).GetPreferredSize(Size.Empty).Height;
		num3 += ((Control)headerLabel).Height + 3;
		Graphics val2 = contentLabel.CreateGraphics();
		try
		{
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		((Control)contentLabel).Top = ((Control)headerLabel).Bounds.Bottom + 3;
		((Control)contentLabel).MaximumSize = new Size(((Control)contentLabel).Width, 1000);
		((Control)contentLabel).Height = ((Control)contentLabel).GetPreferredSize(Size.Empty).Height;
		num3 += ((Control)contentLabel).Height + 3;
		if (contentLabel.IsUsingTextMarkup)
		{
			num3 += 8;
		}
		((Control)buttonsPanel).Top = ((Control)contentLabel).Bottom + 3;
		if (buttonsPanel.Items.Count == 0)
		{
			((Control)buttonsPanel).Visible = false;
		}
		else
		{
			Graphics val3 = buttonsPanel.CreateGraphics();
			try
			{
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			((Control)buttonsPanel).Height = buttonsPanel.GetAutoSizeHeight() + 2;
			num3 += ((Control)buttonsPanel).Height + 6;
			if (string.IsNullOrEmpty(dialogInfo.FooterText) && dialogInfo.FooterImage == null)
			{
				num3 += 8;
			}
		}
		num3 += ((Control)bottomPanel).Height;
		((Control)this).Height = Math.Max(MinimumHeight, num3);
		if (dialogInfo.TaskDialogIcon == eTaskDialogIcon.Help)
		{
			SystemSounds.Question.Play();
		}
		else if (dialogInfo.TaskDialogIcon == eTaskDialogIcon.Information)
		{
			SystemSounds.Asterisk.Play();
		}
		else
		{
			SystemSounds.Exclamation.Play();
		}
		LocalizeText();
		((Form)this).ShowDialog(owner);
		return _Result;
	}

	protected override void OnShown(EventArgs e)
	{
		if (_DefaultButton == eTaskDialogButton.Cancel && ((Control)buttonCancel).Visible)
		{
			((Control)buttonCancel).Focus();
		}
		else if (_DefaultButton == eTaskDialogButton.Close && ((Control)buttonClose).Visible)
		{
			((Control)buttonClose).Focus();
		}
		else if (_DefaultButton == eTaskDialogButton.No && ((Control)buttonNo).Visible)
		{
			((Control)buttonNo).Focus();
		}
		else if (_DefaultButton == eTaskDialogButton.Ok && ((Control)buttonOk).Visible)
		{
			((Control)buttonOk).Focus();
		}
		else if (_DefaultButton == eTaskDialogButton.Retry && ((Control)buttonRetry).Visible)
		{
			((Control)buttonRetry).Focus();
		}
		else if (_DefaultButton == eTaskDialogButton.Yes && ((Control)buttonYes).Visible)
		{
			((Control)buttonYes).Focus();
		}
		((Form)this).OnShown(e);
	}

	private void LocalizeText()
	{
		((Control)buttonCancel).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxCancelButton, ((Control)buttonCancel).Text);
		((Control)buttonClose).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxCloseButton, ((Control)buttonClose).Text);
		((Control)buttonNo).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxNoButton, ((Control)buttonNo).Text);
		((Control)buttonOk).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxOkButton, ((Control)buttonOk).Text);
		((Control)buttonRetry).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxRetryButton, ((Control)buttonRetry).Text);
		((Control)buttonYes).Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxYesButton, ((Control)buttonYes).Text);
	}

	private void UpdateDialogColor(TaskDialogInfo dialogInfo)
	{
		eTaskDialogBackgroundColor dialogColor = dialogInfo.DialogColor;
		switch (dialogColor)
		{
		case eTaskDialogBackgroundColor.Aqua:
			bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(14413555);
			((Control)headerLabel).ForeColor = ColorScheme.GetColor(2119783);
			((Control)contentLabel).ForeColor = ColorScheme.GetColor(2119783);
			break;
		case eTaskDialogBackgroundColor.Blue:
			bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(14411249);
			((Control)headerLabel).ForeColor = ColorScheme.GetColor(2375777);
			((Control)contentLabel).ForeColor = ColorScheme.GetColor(2375777);
			break;
		case eTaskDialogBackgroundColor.DarkBlue:
			bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(13031920);
			((Control)headerLabel).ForeColor = ColorScheme.GetColor(992318);
			((Control)contentLabel).ForeColor = ColorScheme.GetColor(992318);
			break;
		case eTaskDialogBackgroundColor.OliveGreen:
			bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(15462877);
			((Control)headerLabel).ForeColor = ColorScheme.GetColor(5202216);
			((Control)contentLabel).ForeColor = ColorScheme.GetColor(5202216);
			break;
		case eTaskDialogBackgroundColor.Orange:
			bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(16640730);
			((Control)headerLabel).ForeColor = ColorScheme.GetColor(9914374);
			((Control)contentLabel).ForeColor = ColorScheme.GetColor(9914374);
			break;
		case eTaskDialogBackgroundColor.Purple:
			bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(15065324);
			((Control)headerLabel).ForeColor = ColorScheme.GetColor(4141393);
			((Control)contentLabel).ForeColor = ColorScheme.GetColor(4141393);
			break;
		case eTaskDialogBackgroundColor.Red:
			bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(15916251);
			((Control)headerLabel).ForeColor = ColorScheme.GetColor(6497315);
			((Control)contentLabel).ForeColor = ColorScheme.GetColor(6497315);
			break;
		case eTaskDialogBackgroundColor.Silver:
			bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(15921906);
			((Control)headerLabel).ForeColor = ColorScheme.GetColor(789516);
			((Control)contentLabel).ForeColor = ColorScheme.GetColor(789516);
			break;
		case eTaskDialogBackgroundColor.Tan:
			bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(14539203);
			((Control)headerLabel).ForeColor = ColorScheme.GetColor(1907472);
			((Control)contentLabel).ForeColor = ColorScheme.GetColor(1907472);
			break;
		}
		if (dialogColor != 0)
		{
			bottomPanel.Style.BorderColor.Color = ((Control)headerLabel).ForeColor;
		}
	}

	private void buttonOk_Click(object sender, EventArgs e)
	{
		CloseDialog(eTaskDialogResult.Ok);
	}

	private void buttonYes_Click(object sender, EventArgs e)
	{
		CloseDialog(eTaskDialogResult.Yes);
	}

	private void buttonNo_Click(object sender, EventArgs e)
	{
		CloseDialog(eTaskDialogResult.No);
	}

	private void buttonCancel_Click(object sender, EventArgs e)
	{
		CloseDialog(eTaskDialogResult.Cancel);
	}

	private void buttonRetry_Click(object sender, EventArgs e)
	{
		CloseDialog(eTaskDialogResult.Retry);
	}

	private void buttonClose_Click(object sender, EventArgs e)
	{
		CloseDialog(eTaskDialogResult.Close);
	}

	internal void CloseDialog(eTaskDialogResult result)
	{
		_Result = result;
		((Form)this).Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c10: Unknown result type (might be due to invalid IL or missing references)
		headerImage = new PictureBox();
		headerLabel = new LabelX();
		contentLabel = new LabelX();
		buttonsPanel = new ItemPanel();
		bottomPanel = new PanelEx();
		footerLabel = new LabelX();
		footerImage = new PictureBox();
		taskCheckBox = new CheckBoxX();
		footerPanel = new PanelEx();
		flowLayoutPanel1 = new FlowLayoutPanel();
		buttonClose = new ButtonX();
		buttonRetry = new ButtonX();
		buttonCancel = new ButtonX();
		buttonOk = new ButtonX();
		buttonNo = new ButtonX();
		buttonYes = new ButtonX();
		((ISupportInitialize)headerImage).BeginInit();
		((Control)bottomPanel).SuspendLayout();
		((ISupportInitialize)footerImage).BeginInit();
		((Control)flowLayoutPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)headerImage).Location = new Point(12, 12);
		((Control)headerImage).Name = "headerImage";
		((Control)headerImage).Size = new Size(32, 32);
		headerImage.TabIndex = 0;
		headerImage.TabStop = false;
		((Control)headerLabel).Anchor = (AnchorStyles)13;
		headerLabel.BackgroundStyle.Class = "";
		headerLabel.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)headerLabel).Font = new Font("Microsoft Sans Serif", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)headerLabel).Location = new Point(50, 12);
		((Control)headerLabel).Name = "headerLabel";
		((Control)headerLabel).Size = new Size(444, 32);
		((Control)headerLabel).TabIndex = 0;
		((Control)headerLabel).Text = "Task Dialog Header Instructions";
		headerLabel.TextLineAlignment = (StringAlignment)0;
		headerLabel.WordWrap = true;
		((Control)contentLabel).Anchor = (AnchorStyles)13;
		contentLabel.BackgroundStyle.Class = "";
		contentLabel.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)contentLabel).Location = new Point(50, 60);
		((Control)contentLabel).Name = "contentLabel";
		((Control)contentLabel).Size = new Size(444, 35);
		((Control)contentLabel).TabIndex = 1;
		((Control)contentLabel).Text = "Task Dialog actual content of the dialog with some instructions and more text with text-markup support and word wrapping.";
		contentLabel.TextLineAlignment = (StringAlignment)0;
		contentLabel.WordWrap = true;
		((Control)buttonsPanel).Anchor = (AnchorStyles)13;
		buttonsPanel.BackgroundStyle.Class = "";
		buttonsPanel.BackgroundStyle.CornerType = eCornerType.Square;
		buttonsPanel.ContainerControlProcessDialogKey = true;
		buttonsPanel.LayoutOrientation = eOrientation.Vertical;
		((Control)buttonsPanel).Location = new Point(50, 101);
		((Control)buttonsPanel).Name = "buttonsPanel";
		((Control)buttonsPanel).Size = new Size(444, 120);
		((Control)buttonsPanel).TabIndex = 2;
		((Control)buttonsPanel).Text = "itemPanel1";
		bottomPanel.CanvasColor = SystemColors.Control;
		bottomPanel.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)bottomPanel).Controls.Add((Control)(object)footerLabel);
		((Control)bottomPanel).Controls.Add((Control)(object)footerImage);
		((Control)bottomPanel).Controls.Add((Control)(object)taskCheckBox);
		((Control)bottomPanel).Controls.Add((Control)(object)footerPanel);
		((Control)bottomPanel).Controls.Add((Control)(object)flowLayoutPanel1);
		((Control)bottomPanel).Dock = (DockStyle)2;
		((Control)bottomPanel).Location = new Point(0, 240);
		((Control)bottomPanel).Name = "bottomPanel";
		((Control)bottomPanel).Padding = new Padding(3, 3, 3, 0);
		((Control)bottomPanel).Size = new Size(506, 70);
		bottomPanel.Style.Alignment = (StringAlignment)1;
		bottomPanel.Style.BackColor1.Color = Color.FromArgb(227, 239, 255);
		bottomPanel.Style.Border = eBorderType.SingleLine;
		bottomPanel.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		bottomPanel.Style.BorderSide = eBorderSide.Top;
		bottomPanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		bottomPanel.Style.GradientAngle = 90;
		((Control)bottomPanel).TabIndex = 0;
		((Control)footerLabel).Anchor = (AnchorStyles)13;
		footerLabel.BackgroundStyle.Class = "";
		footerLabel.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)footerLabel).Location = new Point(33, 49);
		((Control)footerLabel).Name = "footerLabel";
		((Control)footerLabel).Size = new Size(467, 15);
		((Control)footerLabel).TabIndex = 2;
		((Control)footerLabel).Text = "Footer Label with text-markup support";
		((Control)footerImage).Location = new Point(8, 48);
		((Control)footerImage).Name = "footerImage";
		((Control)footerImage).Size = new Size(16, 16);
		footerImage.TabIndex = 8;
		footerImage.TabStop = false;
		((Control)taskCheckBox).AutoSize = true;
		taskCheckBox.BackgroundStyle.Class = "";
		taskCheckBox.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)taskCheckBox).Location = new Point(3, 10);
		((Control)taskCheckBox).Name = "taskCheckBox";
		((Control)taskCheckBox).Size = new Size(75, 15);
		taskCheckBox.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)taskCheckBox).TabIndex = 10;
		((Control)taskCheckBox).Text = "Check-box";
		footerPanel.CanvasColor = SystemColors.Control;
		footerPanel.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)footerPanel).Dock = (DockStyle)2;
		((Control)footerPanel).Location = new Point(3, 40);
		((Control)footerPanel).Name = "footerPanel";
		((Control)footerPanel).Size = new Size(500, 30);
		footerPanel.Style.Alignment = (StringAlignment)1;
		footerPanel.Style.Border = eBorderType.Raised;
		footerPanel.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		footerPanel.Style.BorderSide = eBorderSide.Top;
		footerPanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		footerPanel.Style.GradientAngle = 90;
		((Control)footerPanel).TabIndex = 10;
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)buttonClose);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)buttonCancel);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)buttonOk);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)buttonRetry);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)buttonNo);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)buttonYes);
		((Control)flowLayoutPanel1).Dock = (DockStyle)1;
		flowLayoutPanel1.FlowDirection = (FlowDirection)2;
		((Control)flowLayoutPanel1).Location = new Point(3, 3);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).Size = new Size(500, 35);
		((Control)flowLayoutPanel1).TabIndex = 0;
		flowLayoutPanel1.WrapContents = false;
		((Control)buttonClose).AccessibleRole = (AccessibleRole)43;
		buttonClose.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonClose).Location = new Point(436, 5);
		((Control)buttonClose).Margin = new Padding(3, 5, 3, 3);
		((Control)buttonClose).Name = "buttonClose";
		((Control)buttonClose).Size = new Size(61, 24);
		((Control)buttonClose).MinimumSize = new Size(61, 24);
		((Control)buttonClose).AutoSize = true;
		buttonClose.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonClose).TabIndex = 5;
		((Control)buttonClose).Text = "&Close";
		((Control)buttonClose).Click += buttonClose_Click;
		((Control)buttonRetry).AccessibleRole = (AccessibleRole)43;
		buttonRetry.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonRetry).Location = new Point(369, 5);
		((Control)buttonRetry).Margin = new Padding(3, 5, 3, 3);
		((Control)buttonRetry).Name = "buttonRetry";
		((Control)buttonRetry).Size = new Size(61, 24);
		((Control)buttonRetry).MinimumSize = new Size(61, 24);
		((Control)buttonRetry).AutoSize = true;
		buttonRetry.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonRetry).TabIndex = 4;
		((Control)buttonRetry).Text = "&Retry";
		((Control)buttonRetry).Click += buttonRetry_Click;
		((Control)buttonCancel).AccessibleRole = (AccessibleRole)43;
		buttonCancel.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonCancel).Location = new Point(302, 5);
		((Control)buttonCancel).Margin = new Padding(3, 5, 3, 3);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(61, 24);
		((Control)buttonCancel).MinimumSize = new Size(61, 24);
		((Control)buttonCancel).AutoSize = true;
		buttonCancel.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonCancel).TabIndex = 3;
		((Control)buttonCancel).Text = "&Cancel";
		((Control)buttonCancel).Click += buttonCancel_Click;
		((Control)buttonOk).AccessibleRole = (AccessibleRole)43;
		buttonOk.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonOk).Location = new Point(235, 5);
		((Control)buttonOk).Margin = new Padding(3, 5, 3, 3);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(61, 24);
		((Control)buttonOk).MinimumSize = new Size(61, 24);
		((Control)buttonOk).AutoSize = true;
		buttonOk.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonOk).TabIndex = 2;
		((Control)buttonOk).Text = "&OK";
		((Control)buttonOk).Click += buttonOk_Click;
		((Control)buttonNo).AccessibleRole = (AccessibleRole)43;
		buttonNo.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonNo).Location = new Point(168, 5);
		((Control)buttonNo).Margin = new Padding(3, 5, 3, 3);
		((Control)buttonNo).Name = "buttonNo";
		((Control)buttonNo).Size = new Size(61, 24);
		((Control)buttonNo).MinimumSize = new Size(61, 24);
		((Control)buttonNo).AutoSize = true;
		buttonNo.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonNo).TabIndex = 1;
		((Control)buttonNo).Text = "&No";
		((Control)buttonNo).Click += buttonNo_Click;
		((Control)buttonYes).AccessibleRole = (AccessibleRole)43;
		buttonYes.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonYes).Location = new Point(101, 5);
		((Control)buttonYes).Margin = new Padding(3, 5, 3, 3);
		((Control)buttonYes).Name = "buttonYes";
		((Control)buttonYes).Size = new Size(61, 24);
		((Control)buttonYes).MinimumSize = new Size(61, 24);
		((Control)buttonYes).AutoSize = true;
		buttonYes.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonYes).TabIndex = 0;
		((Control)buttonYes).Text = "&Yes";
		((Control)buttonYes).Click += buttonYes_Click;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(506, 310);
		((Control)this).Controls.Add((Control)(object)bottomPanel);
		((Control)this).Controls.Add((Control)(object)buttonsPanel);
		((Control)this).Controls.Add((Control)(object)contentLabel);
		((Control)this).Controls.Add((Control)(object)headerLabel);
		((Control)this).Controls.Add((Control)(object)headerImage);
		((Control)this).DoubleBuffered = true;
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "TaskDialogForm";
		((Control)this).Text = "DotNetBar Task Dialog";
		((Form)this).Load += TaskDialogForm_Load;
		((ISupportInitialize)headerImage).EndInit();
		((Control)bottomPanel).ResumeLayout(false);
		((Control)bottomPanel).PerformLayout();
		((ISupportInitialize)footerImage).EndInit();
		((Control)flowLayoutPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

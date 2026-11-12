using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

internal class MetroMessageBoxDialog : MetroForm
{
	private enum SystemStrings
	{
		OK,
		Cancel,
		Abort,
		Retry,
		Ignore,
		Yes,
		No,
		Close,
		Help,
		TryAgain,
		Continue
	}

	private ButtonX Button1;

	private ButtonX Button2;

	private ButtonX Button3;

	private PictureBox PictureBox1;

	private PanelEx TextPanel;

	private PanelEx ButtonBackgroundPanel;

	private eDotNetBarStyle m_Style = eDotNetBarStyle.Metro;

	private bool m_Button1Visible = true;

	private bool m_Button2Visible = true;

	private bool m_Button3Visible = true;

	private MessageBoxButtons m_Buttons;

	private IContainer components;

	private bool _ButtonsDividerVisible = true;

	private Color _MessageTextColor = Color.Empty;

	public eDotNetBarStyle Style
	{
		get
		{
			return m_Style;
		}
		set
		{
			m_Style = value;
		}
	}

	public bool ButtonsDividerVisible
	{
		get
		{
			return _ButtonsDividerVisible;
		}
		set
		{
			if (_ButtonsDividerVisible != value)
			{
				_ButtonsDividerVisible = value;
				((Control)ButtonBackgroundPanel).Visible = _ButtonsDividerVisible;
			}
		}
	}

	public Color MessageTextColor
	{
		get
		{
			return _MessageTextColor;
		}
		set
		{
			_MessageTextColor = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Gets or sets whether Text supports and renders text markup.")]
	public bool TextMarkupEnabled
	{
		get
		{
			return TextPanel.TextMarkupEnabled;
		}
		set
		{
			TextPanel.TextMarkupEnabled = value;
		}
	}

	public bool AntiAlias
	{
		get
		{
			return TextPanel.AntiAlias;
		}
		set
		{
			TextPanel.AntiAlias = value;
		}
	}

	public MetroMessageBoxDialog()
	{
		InitializeComponent();
		TextPanel.DisableSelection();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Control)Button1).Click -= Button1_Click;
			((Control)Button2).Click -= Button2_Click;
			((Control)Button3).Click -= Button3_Click;
			TextPanel.MarkupLinkClick -= TextPanelMarkupLinkClick;
		}
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		Button1 = new ButtonX();
		Button2 = new ButtonX();
		Button3 = new ButtonX();
		PictureBox1 = new PictureBox();
		TextPanel = new PanelEx();
		ButtonBackgroundPanel = new PanelEx();
		((Control)this).SuspendLayout();
		((Control)Button1).AccessibleRole = (AccessibleRole)43;
		Button1.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)Button1).Location = new Point(26, 85);
		((Control)Button1).Name = "Button1";
		((Control)Button1).Size = new Size(77, 24);
		((Control)Button1).TabIndex = 0;
		((Control)Button1).Text = "&OK";
		((Control)Button1).Click += Button1_Click;
		Button1.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Button2).AccessibleRole = (AccessibleRole)43;
		Button2.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)Button2).Location = new Point(109, 85);
		((Control)Button2).Name = "Button2";
		((Control)Button2).Size = new Size(77, 24);
		((Control)Button2).TabIndex = 1;
		((Control)Button2).Text = "&Cancel";
		((Control)Button2).Click += Button2_Click;
		Button2.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Button3).AccessibleRole = (AccessibleRole)43;
		Button3.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)Button3).Location = new Point(192, 85);
		((Control)Button3).Name = "Button3";
		((Control)Button3).Size = new Size(77, 24);
		((Control)Button3).TabIndex = 2;
		((Control)Button3).Text = "&Ignore";
		((Control)Button3).Click += Button3_Click;
		Button3.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)PictureBox1).BackColor = Color.Transparent;
		((Control)PictureBox1).Location = new Point(10, 10);
		((Control)PictureBox1).Name = "PictureBox1";
		((Control)PictureBox1).Size = new Size(34, 34);
		PictureBox1.TabIndex = 3;
		PictureBox1.TabStop = false;
		TextPanel.AntiAlias = false;
		TextPanel.CanvasColor = SystemColors.Control;
		((Control)TextPanel).Location = new Point(53, 10);
		((Control)TextPanel).Name = "TextPanel";
		((Control)TextPanel).Size = new Size(225, 53);
		TextPanel.Style.Border = eBorderType.SingleLine;
		TextPanel.Style.BorderWidth = 0;
		TextPanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		TextPanel.Style.GradientAngle = 90;
		TextPanel.Style.LineAlignment = (StringAlignment)0;
		((Control)TextPanel).TabIndex = 4;
		((Panel)TextPanel).TabStop = false;
		TextPanel.Style.WordWrap = true;
		TextPanel.MarkupLinkClick += TextPanelMarkupLinkClick;
		ButtonBackgroundPanel.AntiAlias = false;
		ButtonBackgroundPanel.CanvasColor = SystemColors.Control;
		((Control)ButtonBackgroundPanel).Location = new Point(53, 10);
		((Control)ButtonBackgroundPanel).Name = "ButtonBackgroundPanel";
		((Control)ButtonBackgroundPanel).Size = new Size(225, 42);
		((Control)ButtonBackgroundPanel).Dock = (DockStyle)2;
		ButtonBackgroundPanel.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		ButtonBackgroundPanel.Style.Border = eBorderType.SingleLine;
		ButtonBackgroundPanel.Style.BorderWidth = 1;
		ButtonBackgroundPanel.Style.BorderSide = eBorderSide.Top;
		ButtonBackgroundPanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		ButtonBackgroundPanel.Style.BackColor1.ColorSchemePart = eColorSchemePart.BarBackground;
		ButtonBackgroundPanel.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		ButtonBackgroundPanel.Style.GradientAngle = 90;
		ButtonBackgroundPanel.Style.LineAlignment = (StringAlignment)0;
		((Control)ButtonBackgroundPanel).TabIndex = 4;
		((Panel)ButtonBackgroundPanel).TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Form)this).ClientSize = new Size(290, 121);
		((Form)this).ShowInTaskbar = false;
		((Control)this).Controls.Add((Control)(object)TextPanel);
		((Control)this).Controls.Add((Control)(object)PictureBox1);
		((Control)this).Controls.Add((Control)(object)Button3);
		((Control)this).Controls.Add((Control)(object)Button2);
		((Control)this).Controls.Add((Control)(object)Button1);
		((Control)this).Controls.Add((Control)(object)ButtonBackgroundPanel);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "MessageBoxDialog";
		((Control)this).ResumeLayout(false);
	}

	private void TextPanelMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
	{
		MessageBoxEx.InvokeMarkupLinkClick(sender, e);
	}

	public DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, bool topMost)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Invalid comparison between Unknown and I4
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Invalid comparison between Unknown and I4
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Invalid comparison between Unknown and I4
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Invalid comparison between Unknown and I4
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Invalid comparison between Unknown and I4
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Invalid comparison between Unknown and I4
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Invalid comparison between Unknown and I4
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Invalid comparison between Unknown and I4
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Invalid comparison between Unknown and I4
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Invalid comparison between Unknown and I4
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		m_Buttons = buttons;
		((Control)this).Text = caption;
		TextPanel.Style.UseMnemonic = false;
		((Control)TextPanel).Text = text;
		if ((int)icon != 0)
		{
			Image systemImage = GetSystemImage(icon);
			if (systemImage != null && systemImage.Width > ((Control)PictureBox1).Width)
			{
				((Control)PictureBox1).Width = systemImage.Width;
			}
			if (systemImage != null && systemImage.Height > ((Control)PictureBox1).Height)
			{
				((Control)PictureBox1).Height = systemImage.Height;
			}
			PictureBox1.Image = systemImage;
		}
		else
		{
			PictureBox1.Image = null;
			((Control)PictureBox1).Visible = false;
		}
		if ((int)buttons == 1 || (int)buttons == 5 || (int)buttons == 4)
		{
			((Control)Button3).Visible = false;
			m_Button3Visible = false;
		}
		else if ((int)buttons == 0)
		{
			((Control)Button2).Visible = false;
			((Control)Button3).Visible = false;
			m_Button2Visible = false;
			m_Button3Visible = false;
		}
		if ((int)buttons == 0)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)Button1;
			((Form)this).CancelButton = (IButtonControl)(object)Button1;
		}
		else if ((int)buttons == 1 || (int)buttons == 5 || (int)buttons == 4)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)Button1;
			((Form)this).CancelButton = (IButtonControl)(object)Button2;
		}
		else if ((int)buttons == 3)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)Button1;
			((Form)this).CancelButton = (IButtonControl)(object)Button3;
		}
		SetButtonText(buttons);
		if ((int)defaultButton == 0 && m_Button1Visible)
		{
			((Control)Button1).Select();
			((Form)this).AcceptButton = (IButtonControl)(object)Button1;
		}
		else if ((int)defaultButton == 256 && m_Button2Visible)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)Button2;
			((Control)Button2).Select();
		}
		else if ((int)defaultButton == 512 && m_Button3Visible)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)Button3;
			((Control)Button3).Select();
		}
		ResizeDialog();
		SetupColors();
		if ((int)icon == 32)
		{
			SystemSounds.Question.Play();
		}
		else if ((int)icon == 64)
		{
			SystemSounds.Asterisk.Play();
		}
		else
		{
			SystemSounds.Exclamation.Play();
		}
		if ((int)buttons != 0)
		{
			base.CloseEnabled = false;
		}
		if (((Form)this).TopMost != topMost)
		{
			((Form)this).TopMost = topMost;
		}
		return ((Form)this).ShowDialog(owner);
	}

	private void SetupColors()
	{
		if (!_MessageTextColor.IsEmpty)
		{
			TextPanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.Custom;
			TextPanel.Style.ForeColor.Color = _MessageTextColor;
		}
		else if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			TextPanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.Custom;
			TextPanel.Style.ForeColor.Color = colorTable.Form.TextColor;
			TextPanel.Style.BackColor1.Color = colorTable.Form.BackColor;
		}
	}

	private void SetButtonText(MessageBoxButtons buttons)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Invalid comparison between Unknown and I4
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Invalid comparison between Unknown and I4
		if ((int)buttons == 2)
		{
			((Control)Button1).Text = GetString(SystemStrings.Abort);
			((Control)Button2).Text = GetString(SystemStrings.Retry);
			((Control)Button3).Text = GetString(SystemStrings.Ignore);
		}
		else if ((int)buttons == 0)
		{
			((Control)Button1).Text = GetString(SystemStrings.OK);
		}
		else if ((int)buttons == 1)
		{
			((Control)Button1).Text = GetString(SystemStrings.OK);
			((Control)Button2).Text = GetString(SystemStrings.Cancel);
		}
		else if ((int)buttons == 5)
		{
			((Control)Button1).Text = GetString(SystemStrings.Retry);
			((Control)Button2).Text = GetString(SystemStrings.Cancel);
		}
		else if ((int)buttons == 4)
		{
			((Control)Button1).Text = GetString(SystemStrings.Yes);
			((Control)Button2).Text = GetString(SystemStrings.No);
		}
		else if ((int)buttons == 3)
		{
			((Control)Button1).Text = GetString(SystemStrings.Yes);
			((Control)Button2).Text = GetString(SystemStrings.No);
			((Control)Button3).Text = GetString(SystemStrings.Cancel);
		}
	}

	internal static Image GetSystemImage(MessageBoxIcon icon)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Invalid comparison between Unknown and I4
		Icon val = null;
		if ((int)icon == 64)
		{
			val = SystemIcons.Asterisk;
		}
		else if ((int)icon == 16 || (int)icon == 16)
		{
			val = SystemIcons.Error;
		}
		else if ((int)icon == 48)
		{
			val = SystemIcons.Exclamation;
		}
		else if ((int)icon == 16)
		{
			val = SystemIcons.Hand;
		}
		else if ((int)icon == 64)
		{
			val = SystemIcons.Information;
		}
		else if ((int)icon == 32)
		{
			val = SystemIcons.Question;
		}
		else if ((int)icon == 48)
		{
			val = SystemIcons.Warning;
		}
		Bitmap val2 = new Bitmap(val.Width, val.Height);
		val2.MakeTransparent();
		Graphics val3 = Graphics.FromImage((Image)(object)val2);
		try
		{
			if (Environment.Version.Build <= 3705 && Environment.Version.Revision == 288 && Environment.Version.Major == 1 && Environment.Version.Minor == 0)
			{
				IntPtr hdc = val3.GetHdc();
				try
				{
					NativeFunctions.DrawIconEx(hdc, 0, 0, val.Handle, val.Width, val.Height, 0, IntPtr.Zero, 3);
				}
				finally
				{
					val3.ReleaseHdc(hdc);
				}
			}
			else if (val.Handle != IntPtr.Zero)
			{
				try
				{
					val3.DrawIcon(val, 0, 0);
				}
				catch
				{
				}
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		return (Image)(object)val2;
	}

	private void ResizeDialog()
	{
		Size size = Size.Empty;
		int num = 6;
		int num2 = 40;
		int num3 = 10;
		int num4 = 110;
		if (PictureBox1.Image != null)
		{
			((Control)TextPanel).Left = ((Control)PictureBox1).Bounds.Right + 16;
		}
		else
		{
			((Control)TextPanel).Left = ((Control)PictureBox1).Left;
		}
		Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
		((Control)TextPanel).Size = TextPanel.GetAutoSize(workingArea.Width);
		if ((double)((Control)TextPanel).Size.Width > (double)workingArea.Width * 0.75)
		{
			((Control)TextPanel).Size = TextPanel.GetAutoSize((int)((double)workingArea.Width * 0.75));
		}
		else if (((Control)TextPanel).Size.Width < num4)
		{
			((Control)TextPanel).Width = num4;
		}
		if (((Control)this).Text.Length > 0)
		{
			Size empty = Size.Empty;
			Font font = ((Control)this).Font;
			Graphics val = BarFunctions.CreateGraphics((Control)(object)this);
			try
			{
				size = TextDrawing.MeasureString(val, ((Control)this).Text, font);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			size.Width += 2;
			size.Height += 2;
			if (size.Width > ((Control)TextPanel).Width)
			{
				((Control)TextPanel).Width = size.Width;
			}
		}
		int num5 = Math.Max(((Control)TextPanel).Bounds.Bottom, ((Control)PictureBox1).Bounds.Bottom);
		num5 += 19;
		((Control)Button1).Top = num5;
		((Control)Button2).Top = num5;
		((Control)Button3).Top = num5;
		int num6 = ((Control)Button1).Width + (m_Button2Visible ? (((Control)Button2).Width + num) : 0) + (m_Button3Visible ? (((Control)Button3).Width + num) : 0);
		int num7 = num6 + num2 * 2;
		if (num6 < ((Control)TextPanel).Bounds.Right + num3)
		{
			num7 = ((Control)TextPanel).Bounds.Right + num3;
		}
		else
		{
			PanelEx textPanel = TextPanel;
			((Control)textPanel).Width = ((Control)textPanel).Width + (num7 - ((Control)TextPanel).Bounds.Right - num3);
		}
		int num8 = (num7 - num6) / 2;
		((Control)Button1).Left = num8;
		num8 += ((Control)Button1).Width + num;
		if (m_Button2Visible)
		{
			((Control)Button2).Left = num8;
			num8 += ((Control)Button2).Width + num;
		}
		if (m_Button3Visible)
		{
			((Control)Button3).Left = num8;
			num8 += ((Control)Button3).Width + num;
		}
		size = (((Form)this).Size = new Size(((Control)TextPanel).Bounds.Right + num3 + SystemInformation.FixedFrameBorderSize.Width * 2, ((Control)Button1).Bounds.Bottom + num3 + SystemInformation.FixedFrameBorderSize.Height * 2 + SystemInformation.CaptionHeight));
	}

	private void Button1_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		DialogResult dialogResult = (DialogResult)1;
		if ((int)m_Buttons == 0 || (int)m_Buttons == 1)
		{
			dialogResult = (DialogResult)1;
		}
		else if ((int)m_Buttons == 4 || (int)m_Buttons == 3)
		{
			dialogResult = (DialogResult)6;
		}
		else if ((int)m_Buttons == 2)
		{
			dialogResult = (DialogResult)3;
		}
		else if ((int)m_Buttons == 5)
		{
			dialogResult = (DialogResult)4;
		}
		((Form)this).DialogResult = dialogResult;
	}

	private void Button2_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		DialogResult dialogResult = (DialogResult)2;
		if ((int)m_Buttons == 1)
		{
			dialogResult = (DialogResult)2;
		}
		else if ((int)m_Buttons == 4 || (int)m_Buttons == 3)
		{
			dialogResult = (DialogResult)7;
		}
		else if ((int)m_Buttons == 2)
		{
			dialogResult = (DialogResult)4;
		}
		else if ((int)m_Buttons == 5)
		{
			dialogResult = (DialogResult)2;
		}
		((Form)this).DialogResult = dialogResult;
	}

	private void Button3_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		DialogResult dialogResult = (DialogResult)2;
		if ((int)m_Buttons == 2)
		{
			dialogResult = (DialogResult)5;
		}
		else if ((int)m_Buttons == 3)
		{
			dialogResult = (DialogResult)2;
		}
		((Form)this).DialogResult = dialogResult;
	}

	protected override void WndProc(ref Message m)
	{
		int num = WinApi.ToInt(((Message)(ref m)).WParam);
		if (((Message)(ref m)).Msg != 274 || (num != 61488 && num != 61472))
		{
			base.WndProc(ref m);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		if ((keyData & 0x43) == 67 && (keyData & 0x20000) == 131072)
		{
			string text = "------------------------------\r\n" + ((Control)this).Text + "\r\n------------------------------\r\n" + ((Control)TextPanel).Text + "\r\n------------------------------\r\n";
			if (((Control)Button1).Visible)
			{
				text = text + "[" + ((Control)Button1).Text.Replace("&", "") + "]  ";
			}
			if (((Control)Button2).Visible)
			{
				text = text + "[" + ((Control)Button2).Text.Replace("&", "") + "]  ";
			}
			if (((Control)Button3).Visible)
			{
				text = text + "[" + ((Control)Button3).Text.Replace("&", "") + "]";
			}
			text += "\r\n------------------------------";
			Clipboard.SetText(text);
		}
		return ((Form)this).ProcessCmdKey(ref msg, keyData);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern string MB_GetString(int i);

	private static string GetLocalizedText(SystemStrings sysString)
	{
		string text = "";
		string text2 = null;
		switch (sysString)
		{
		case SystemStrings.Abort:
			text2 = "&Abort";
			text = LocalizationKeys.MessageBoxAbortButton;
			break;
		case SystemStrings.Cancel:
			text2 = "&Cancel";
			text = LocalizationKeys.MessageBoxCancelButton;
			break;
		case SystemStrings.Close:
			text2 = "C&lose";
			text = LocalizationKeys.MessageBoxCloseButton;
			break;
		case SystemStrings.Continue:
			text2 = "Co&ntinue";
			text = LocalizationKeys.MessageBoxContinueButton;
			break;
		case SystemStrings.Help:
			text2 = "&Help";
			text = LocalizationKeys.MessageBoxHelpButton;
			break;
		case SystemStrings.Ignore:
			text2 = "&Ignore";
			text = LocalizationKeys.MessageBoxIgnoreButton;
			break;
		case SystemStrings.No:
			text2 = "&No";
			text = LocalizationKeys.MessageBoxNoButton;
			break;
		case SystemStrings.OK:
			text2 = "&OK";
			text = LocalizationKeys.MessageBoxOkButton;
			break;
		case SystemStrings.Retry:
			text2 = "&Retry";
			text = LocalizationKeys.MessageBoxRetryButton;
			break;
		case SystemStrings.TryAgain:
			text2 = "&Try Again";
			text = LocalizationKeys.MessageBoxTryAgainButton;
			break;
		case SystemStrings.Yes:
			text2 = "&Yes";
			text = LocalizationKeys.MessageBoxYesButton;
			break;
		}
		if (text != null)
		{
			text2 = LocalizationManager.GetLocalizedString(text, text2);
		}
		return text2;
	}

	private static string GetString(SystemStrings sysString)
	{
		string text = "";
		if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower() != "en" && MessageBoxEx.UseSystemLocalizedString)
		{
			try
			{
				text = MB_GetString((int)sysString);
			}
			catch
			{
				text = "";
			}
		}
		if (text == "")
		{
			text = GetLocalizedText(sysString);
		}
		return text;
	}
}

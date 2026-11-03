using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Metro;

internal class Ⲩ : MetroForm
{
	private enum ⳑ
	{
		Ⳓ,
		ⳓ,
		Ⳕ,
		ⳕ,
		Ⳗ,
		ⳗ,
		Ⳙ,
		ᆤ,
		_1C8E,
		ⳙ,
		Ⳛ
	}

	private ButtonX ⲭ;

	private ButtonX Ⲯ;

	private ButtonX ⲯ;

	private PictureBox Ⲱ;

	private PanelEx ⲱ;

	private PanelEx Ⲳ;

	private eDotNetBarStyle Ӷ = eDotNetBarStyle.Metro;

	private bool ⲳ = true;

	private bool Ⲵ = true;

	private bool ⲵ = true;

	private MessageBoxButtons _27C7;

	private IContainer Պ;

	private bool _Ⲷ = true;

	private Color _ⲷ = Color.Empty;

	public eDotNetBarStyle ߕ
	{
		get
		{
			return Ӷ;
		}
		set
		{
			Ӷ = value;
		}
	}

	public bool ⲩ
	{
		get
		{
			return _Ⲷ;
		}
		set
		{
			if (_Ⲷ != value)
			{
				_Ⲷ = value;
				((Control)Ⲳ).Visible = _Ⲷ;
			}
		}
	}

	public Color Ⲫ
	{
		get
		{
			return _ⲷ;
		}
		set
		{
			_ⲷ = value;
		}
	}

	[Description("Gets or sets whether Text supports and renders text markup.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool ⲫ
	{
		get
		{
			return ⲱ.TextMarkupEnabled;
		}
		set
		{
			ⲱ.TextMarkupEnabled = value;
		}
	}

	public bool Ⲭ
	{
		get
		{
			return ⲱ.AntiAlias;
		}
		set
		{
			ⲱ.AntiAlias = value;
		}
	}

	public Ⲩ()
	{
		ـ();
		ⲱ.ᖦ();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			((Control)ⲭ).Click -= Ⳁ;
			((Control)Ⲯ).Click -= ⳁ;
			((Control)ⲯ).Click -= Ⳃ;
			ⲱ.MarkupLinkClick -= Ⲹ;
		}
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		base.Dispose(disposing);
	}

	private void ـ()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		ⲭ = new ButtonX();
		Ⲯ = new ButtonX();
		ⲯ = new ButtonX();
		Ⲱ = new PictureBox();
		ⲱ = new PanelEx();
		Ⲳ = new PanelEx();
		((Control)this).SuspendLayout();
		((Control)ⲭ).AccessibleRole = (AccessibleRole)43;
		ⲭ.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)ⲭ).Location = new Point(26, 85);
		((Control)ⲭ).Name = "Button1";
		((Control)ⲭ).Size = new Size(77, 24);
		((Control)ⲭ).TabIndex = 0;
		((Control)ⲭ).Text = "&OK";
		((Control)ⲭ).Click += Ⳁ;
		ⲭ.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Ⲯ).AccessibleRole = (AccessibleRole)43;
		Ⲯ.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)Ⲯ).Location = new Point(109, 85);
		((Control)Ⲯ).Name = "Button2";
		((Control)Ⲯ).Size = new Size(77, 24);
		((Control)Ⲯ).TabIndex = 1;
		((Control)Ⲯ).Text = "&Cancel";
		((Control)Ⲯ).Click += ⳁ;
		Ⲯ.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ⲯ).AccessibleRole = (AccessibleRole)43;
		ⲯ.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)ⲯ).Location = new Point(192, 85);
		((Control)ⲯ).Name = "Button3";
		((Control)ⲯ).Size = new Size(77, 24);
		((Control)ⲯ).TabIndex = 2;
		((Control)ⲯ).Text = "&Ignore";
		((Control)ⲯ).Click += Ⳃ;
		ⲯ.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)Ⲱ).BackColor = Color.Transparent;
		((Control)Ⲱ).Location = new Point(10, 10);
		((Control)Ⲱ).Name = "PictureBox1";
		((Control)Ⲱ).Size = new Size(34, 34);
		Ⲱ.TabIndex = 3;
		Ⲱ.TabStop = false;
		ⲱ.AntiAlias = false;
		ⲱ.CanvasColor = SystemColors.Control;
		((Control)ⲱ).Location = new Point(53, 10);
		((Control)ⲱ).Name = "TextPanel";
		((Control)ⲱ).Size = new Size(225, 53);
		ⲱ.Style.Border = eBorderType.SingleLine;
		ⲱ.Style.BorderWidth = 0;
		ⲱ.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		ⲱ.Style.GradientAngle = 90;
		ⲱ.Style.LineAlignment = (StringAlignment)0;
		((Control)ⲱ).TabIndex = 4;
		((Panel)ⲱ).TabStop = false;
		ⲱ.Style.WordWrap = true;
		ⲱ.MarkupLinkClick += Ⲹ;
		Ⲳ.AntiAlias = false;
		Ⲳ.CanvasColor = SystemColors.Control;
		((Control)Ⲳ).Location = new Point(53, 10);
		((Control)Ⲳ).Name = "ButtonBackgroundPanel";
		((Control)Ⲳ).Size = new Size(225, 42);
		((Control)Ⲳ).Dock = (DockStyle)2;
		Ⲳ.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		Ⲳ.Style.Border = eBorderType.SingleLine;
		Ⲳ.Style.BorderWidth = 1;
		Ⲳ.Style.BorderSide = eBorderSide.Top;
		Ⲳ.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		Ⲳ.Style.BackColor1.ColorSchemePart = eColorSchemePart.BarBackground;
		Ⲳ.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		Ⲳ.Style.GradientAngle = 90;
		Ⲳ.Style.LineAlignment = (StringAlignment)0;
		((Control)Ⲳ).TabIndex = 4;
		((Panel)Ⲳ).TabStop = false;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(96f, 96f);
		((Form)this).ClientSize = new Size(290, 121);
		((Form)this).ShowInTaskbar = false;
		((Control)this).Controls.Add((Control)(object)ⲱ);
		((Control)this).Controls.Add((Control)(object)Ⲱ);
		((Control)this).Controls.Add((Control)(object)ⲯ);
		((Control)this).Controls.Add((Control)(object)Ⲯ);
		((Control)this).Controls.Add((Control)(object)ⲭ);
		((Control)this).Controls.Add((Control)(object)Ⲳ);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "MessageBoxDialog";
		((Control)this).ResumeLayout(false);
	}

	private void Ⲹ(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		MessageBoxEx.ۃ(_٠, ؿ);
	}

	public DialogResult _26BA(IWin32Window ॡ, string _06EA, string ⲹ, MessageBoxButtons _2827, MessageBoxIcon ᆠ, MessageBoxDefaultButton Ⲻ, bool ⲻ)
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
		//IL_021a: Invalid comparison between Unknown and I4
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Invalid comparison between Unknown and I4
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		_27C7 = _2827;
		((Control)this).Text = ⲹ;
		ⲱ.Style.UseMnemonic = false;
		((Control)ⲱ).Text = _06EA;
		if ((int)ᆠ != 0)
		{
			Image val = Ⲿ(ᆠ);
			if (val != null && val.Width > ((Control)Ⲱ).Width)
			{
				((Control)Ⲱ).Width = val.Width;
			}
			if (val != null && val.Height > ((Control)Ⲱ).Height)
			{
				((Control)Ⲱ).Height = val.Height;
			}
			Ⲱ.Image = val;
		}
		else
		{
			Ⲱ.Image = null;
			((Control)Ⲱ).Visible = false;
		}
		if ((int)_2827 == 1 || (int)_2827 == 5 || (int)_2827 == 4)
		{
			((Control)ⲯ).Visible = false;
			ⲵ = false;
		}
		else if ((int)_2827 == 0)
		{
			((Control)Ⲯ).Visible = false;
			((Control)ⲯ).Visible = false;
			Ⲵ = false;
			ⲵ = false;
		}
		if ((int)_2827 == 0)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)ⲭ;
			((Form)this).CancelButton = (IButtonControl)(object)ⲭ;
		}
		else if ((int)_2827 == 1 || (int)_2827 == 5 || (int)_2827 == 4)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)ⲭ;
			((Form)this).CancelButton = (IButtonControl)(object)Ⲯ;
		}
		else if ((int)_2827 == 3)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)ⲭ;
			((Form)this).CancelButton = (IButtonControl)(object)ⲯ;
		}
		ⲽ(_2827);
		if ((int)Ⲻ == 0 && ⲳ)
		{
			((Control)ⲭ).Select();
			((Form)this).AcceptButton = (IButtonControl)(object)ⲭ;
		}
		else if ((int)Ⲻ == 256 && Ⲵ)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)Ⲯ;
			((Control)Ⲯ).Select();
		}
		else if ((int)Ⲻ == 512 && ⲵ)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)ⲯ;
			((Control)ⲯ).Select();
		}
		ⲿ();
		Ⲽ();
		if ((int)ᆠ == 32)
		{
			SystemSounds.Question.Play();
		}
		else if ((int)ᆠ == 64)
		{
			SystemSounds.Asterisk.Play();
		}
		else
		{
			SystemSounds.Exclamation.Play();
		}
		if ((int)_2827 == 2 || (int)_2827 == 4)
		{
			base.CloseEnabled = false;
		}
		if (((Form)this).TopMost != ⲻ)
		{
			((Form)this).TopMost = ⲻ;
		}
		return ((Form)this).ShowDialog(ॡ);
	}

	private void Ⲽ()
	{
		if (!_ⲷ.IsEmpty)
		{
			ⲱ.Style.ForeColor.ColorSchemePart = eColorSchemePart.Custom;
			ⲱ.Style.ForeColor.Color = _ⲷ;
		}
		else if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			ⲱ.Style.ForeColor.ColorSchemePart = eColorSchemePart.Custom;
			ⲱ.Style.ForeColor.Color = colorTable.Form.TextColor;
			ⲱ.Style.BackColor1.Color = colorTable.Form.BackColor;
		}
	}

	private void ⲽ(MessageBoxButtons _2827)
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
		if ((int)_2827 == 2)
		{
			((Control)ⲭ).Text = ⳅ(ⳑ.Ⳕ);
			((Control)Ⲯ).Text = ⳅ(ⳑ.ⳕ);
			((Control)ⲯ).Text = ⳅ(ⳑ.Ⳗ);
		}
		else if ((int)_2827 == 0)
		{
			((Control)ⲭ).Text = ⳅ(ⳑ.Ⳓ);
		}
		else if ((int)_2827 == 1)
		{
			((Control)ⲭ).Text = ⳅ(ⳑ.Ⳓ);
			((Control)Ⲯ).Text = ⳅ(ⳑ.ⳓ);
		}
		else if ((int)_2827 == 5)
		{
			((Control)ⲭ).Text = ⳅ(ⳑ.ⳕ);
			((Control)Ⲯ).Text = ⳅ(ⳑ.ⳓ);
		}
		else if ((int)_2827 == 4)
		{
			((Control)ⲭ).Text = ⳅ(ⳑ.ⳗ);
			((Control)Ⲯ).Text = ⳅ(ⳑ.Ⳙ);
		}
		else if ((int)_2827 == 3)
		{
			((Control)ⲭ).Text = ⳅ(ⳑ.ⳗ);
			((Control)Ⲯ).Text = ⳅ(ⳑ.Ⳙ);
			((Control)ⲯ).Text = ⳅ(ⳑ.ⳓ);
		}
	}

	internal static Image Ⲿ(MessageBoxIcon ᆠ)
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
		if ((int)ᆠ == 64)
		{
			val = SystemIcons.Asterisk;
		}
		else if ((int)ᆠ == 16 || (int)ᆠ == 16)
		{
			val = SystemIcons.Error;
		}
		else if ((int)ᆠ == 48)
		{
			val = SystemIcons.Exclamation;
		}
		else if ((int)ᆠ == 16)
		{
			val = SystemIcons.Hand;
		}
		else if ((int)ᆠ == 64)
		{
			val = SystemIcons.Information;
		}
		else if ((int)ᆠ == 32)
		{
			val = SystemIcons.Question;
		}
		else if ((int)ᆠ == 48)
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
					_089F.DrawIconEx(hdc, 0, 0, val.Handle, val.Width, val.Height, 0, IntPtr.Zero, 3);
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

	private void ⲿ()
	{
		Size size = Size.Empty;
		int num = 6;
		int num2 = Dpi.Width(40);
		int width = Dpi.Width10;
		int num3 = Dpi.Width(110);
		if (Ⲱ.Image != null)
		{
			((Control)ⲱ).Left = ((Control)Ⲱ).Bounds.Right + Dpi.Width16;
		}
		else
		{
			((Control)ⲱ).Left = ((Control)Ⲱ).Left;
		}
		Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
		((Control)ⲱ).Size = ⲱ.GetAutoSize(workingArea.Width);
		if ((double)((Control)ⲱ).Size.Width > (double)workingArea.Width * 0.75)
		{
			((Control)ⲱ).Size = ⲱ.GetAutoSize((int)((double)workingArea.Width * 0.75));
		}
		else if (((Control)ⲱ).Size.Width < num3)
		{
			((Control)ⲱ).Width = num3;
		}
		if (((Control)this).Text.Length > 0)
		{
			_ = Size.Empty;
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
			if (size.Width > ((Control)ⲱ).Width)
			{
				((Control)ⲱ).Width = size.Width;
			}
		}
		int num4 = Math.Max(((Control)ⲱ).Bounds.Bottom, ((Control)Ⲱ).Bounds.Bottom);
		num4 += (int)(19f / Dpi.Factor.Height);
		((Control)ⲭ).Top = num4;
		((Control)Ⲯ).Top = num4;
		((Control)ⲯ).Top = num4;
		int num5 = ((Control)ⲭ).Width + (Ⲵ ? (((Control)Ⲯ).Width + num) : 0) + (ⲵ ? (((Control)ⲯ).Width + num) : 0);
		int num6 = num5 + num2 * 2;
		if (num5 < ((Control)ⲱ).Bounds.Right + width)
		{
			num6 = ((Control)ⲱ).Bounds.Right + width;
		}
		else
		{
			PanelEx panelEx = ⲱ;
			((Control)panelEx).Width = ((Control)panelEx).Width + (num6 - ((Control)ⲱ).Bounds.Right - width);
		}
		int num7 = (num6 - num5) / 2;
		((Control)ⲭ).Left = num7;
		num7 += ((Control)ⲭ).Width + num;
		if (Ⲵ)
		{
			((Control)Ⲯ).Left = num7;
			num7 += ((Control)Ⲯ).Width + num;
		}
		if (ⲵ)
		{
			((Control)ⲯ).Left = num7;
			num7 += ((Control)ⲯ).Width + num;
		}
		size = (((Form)this).ClientSize = new Size(((Control)ⲱ).Bounds.Right + width, ((Control)ⲭ).Bounds.Bottom + width));
	}

	private void Ⳁ(object _٠, EventArgs ؿ)
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
		if ((int)_27C7 == 0 || (int)_27C7 == 1)
		{
			dialogResult = (DialogResult)1;
		}
		else if ((int)_27C7 == 4 || (int)_27C7 == 3)
		{
			dialogResult = (DialogResult)6;
		}
		else if ((int)_27C7 == 2)
		{
			dialogResult = (DialogResult)3;
		}
		else if ((int)_27C7 == 5)
		{
			dialogResult = (DialogResult)4;
		}
		((Form)this).DialogResult = dialogResult;
	}

	private void ⳁ(object _٠, EventArgs ؿ)
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
		if ((int)_27C7 == 1)
		{
			dialogResult = (DialogResult)2;
		}
		else if ((int)_27C7 == 4 || (int)_27C7 == 3)
		{
			dialogResult = (DialogResult)7;
		}
		else if ((int)_27C7 == 2)
		{
			dialogResult = (DialogResult)4;
		}
		else if ((int)_27C7 == 5)
		{
			dialogResult = (DialogResult)2;
		}
		((Form)this).DialogResult = dialogResult;
	}

	private void Ⳃ(object _٠, EventArgs ؿ)
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
		if ((int)_27C7 == 2)
		{
			dialogResult = (DialogResult)5;
		}
		else if ((int)_27C7 == 3)
		{
			dialogResult = (DialogResult)2;
		}
		((Form)this).DialogResult = dialogResult;
	}

	protected override void WndProc(ref Message m)
	{
		int num = _0EFE._0F11(((Message)(ref m)).WParam);
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
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		if ((keyData & 0x43) == 67 && (keyData & 0x20000) == 131072)
		{
			string text = "------------------------------\r\n" + ((Control)this).Text + "\r\n------------------------------\r\n" + ((Control)ⲱ).Text + "\r\n------------------------------\r\n";
			if (((Control)ⲭ).Visible)
			{
				text = text + "[" + ((Control)ⲭ).Text.Replace("&", "") + "]  ";
			}
			if (((Control)Ⲯ).Visible)
			{
				text = text + "[" + ((Control)Ⲯ).Text.Replace("&", "") + "]  ";
			}
			if (((Control)ⲯ).Visible)
			{
				text = text + "[" + ((Control)ⲯ).Text.Replace("&", "") + "]";
			}
			text += "\r\n------------------------------";
			Clipboard.SetText(text);
		}
		return ((Form)this).ProcessCmdKey(ref msg, keyData);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern string MB_GetString(int i);

	private static string ⳃ(ⳑ Ⳅ)
	{
		string text = "";
		string text2 = null;
		switch (Ⳅ)
		{
		case ⳑ.Ⳕ:
			text2 = "&Abort";
			text = LocalizationKeys.MessageBoxAbortButton;
			break;
		case ⳑ.ⳓ:
			text2 = "&Cancel";
			text = LocalizationKeys.MessageBoxCancelButton;
			break;
		case ⳑ.ᆤ:
			text2 = "C&lose";
			text = LocalizationKeys.MessageBoxCloseButton;
			break;
		case ⳑ.Ⳛ:
			text2 = "Co&ntinue";
			text = LocalizationKeys.MessageBoxContinueButton;
			break;
		case ⳑ._1C8E:
			text2 = "&Help";
			text = LocalizationKeys.MessageBoxHelpButton;
			break;
		case ⳑ.Ⳗ:
			text2 = "&Ignore";
			text = LocalizationKeys.MessageBoxIgnoreButton;
			break;
		case ⳑ.Ⳙ:
			text2 = "&No";
			text = LocalizationKeys.MessageBoxNoButton;
			break;
		case ⳑ.Ⳓ:
			text2 = "&OK";
			text = LocalizationKeys.MessageBoxOkButton;
			break;
		case ⳑ.ⳕ:
			text2 = "&Retry";
			text = LocalizationKeys.MessageBoxRetryButton;
			break;
		case ⳑ.ⳙ:
			text2 = "&Try Again";
			text = LocalizationKeys.MessageBoxTryAgainButton;
			break;
		case ⳑ.ⳗ:
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

	private static string ⳅ(ⳑ Ⳅ)
	{
		string text = "";
		if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower() != "en" && MessageBoxEx.UseSystemLocalizedString)
		{
			try
			{
				text = MB_GetString((int)Ⳅ);
			}
			catch
			{
				text = "";
			}
		}
		if (text == "")
		{
			text = ⳃ(Ⳅ);
		}
		return text;
	}
}

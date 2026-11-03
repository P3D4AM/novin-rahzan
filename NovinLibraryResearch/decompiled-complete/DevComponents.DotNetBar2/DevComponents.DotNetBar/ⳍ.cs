using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

internal class ⳍ : OfficeForm
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

	private eDotNetBarStyle Ӷ = eDotNetBarStyle.Office2007;

	private bool ⲳ = true;

	private bool Ⲵ = true;

	private bool ⲵ = true;

	private MessageBoxButtons _27C7;

	private IContainer Պ;

	private bool _Ⲷ = true;

	private Color _ⲷ = Color.Empty;

	[DefaultValue(true)]
	[Description("Gets or sets whether Text supports and renders text markup.")]
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

	public ⳍ()
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
		((Control)Ⲯ).AccessibleRole = (AccessibleRole)43;
		Ⲯ.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)Ⲯ).Location = new Point(109, 85);
		((Control)Ⲯ).Name = "Button2";
		((Control)Ⲯ).Size = new Size(77, 24);
		((Control)Ⲯ).TabIndex = 1;
		((Control)Ⲯ).Text = "&Cancel";
		((Control)Ⲯ).Click += ⳁ;
		((Control)ⲯ).AccessibleRole = (AccessibleRole)43;
		ⲯ.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)ⲯ).Location = new Point(192, 85);
		((Control)ⲯ).Name = "Button3";
		((Control)ⲯ).Size = new Size(77, 24);
		((Control)ⲯ).TabIndex = 2;
		((Control)ⲯ).Text = "&Ignore";
		((Control)ⲯ).Click += Ⳃ;
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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Invalid comparison between Unknown and I4
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Invalid comparison between Unknown and I4
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Invalid comparison between Unknown and I4
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Invalid comparison between Unknown and I4
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Invalid comparison between Unknown and I4
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Invalid comparison between Unknown and I4
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Invalid comparison between Unknown and I4
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Invalid comparison between Unknown and I4
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Invalid comparison between Unknown and I4
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Invalid comparison between Unknown and I4
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Invalid comparison between Unknown and I4
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Invalid comparison between Unknown and I4
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Invalid comparison between Unknown and I4
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		MethodInvoker val = null;
		MethodInvoker val2 = null;
		MethodInvoker val3 = null;
		_27C7 = _2827;
		((Control)this).Text = ⲹ;
		ⲱ.Style.UseMnemonic = false;
		((Control)ⲱ).Text = _06EA;
		if ((int)ᆠ != 0)
		{
			Image val4 = Ⲿ(ᆠ);
			if (val4 != null && val4.Width > ((Control)Ⲱ).Width)
			{
				((Control)Ⲱ).Width = val4.Width;
			}
			if (val4 != null && val4.Height > ((Control)Ⲱ).Height)
			{
				((Control)Ⲱ).Height = val4.Height;
			}
			Ⲱ.Image = val4;
		}
		else
		{
			Ⲱ.Image = null;
			((Control)Ⲱ).Visible = false;
		}
		if (!BarFunctions.IsOffice2007Style(Ӷ))
		{
			base.EnableCustomStyle = false;
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
			if (val == null)
			{
				val = (MethodInvoker)delegate
				{
					((Control)ⲭ).Select();
				};
			}
			BarUtilities.InvokeDelayed(val);
			((Form)this).AcceptButton = (IButtonControl)(object)ⲭ;
		}
		else if ((int)Ⲻ == 256 && Ⲵ)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)Ⲯ;
			if (val2 == null)
			{
				val2 = (MethodInvoker)delegate
				{
					((Control)Ⲯ).Select();
				};
			}
			BarUtilities.InvokeDelayed(val2);
		}
		else if ((int)Ⲻ == 512 && ⲵ)
		{
			((Form)this).AcceptButton = (IButtonControl)(object)ⲯ;
			if (val3 == null)
			{
				val3 = (MethodInvoker)delegate
				{
					((Control)ⲯ).Select();
				};
			}
			BarUtilities.InvokeDelayed(val3);
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
		else if (BarFunctions.IsOffice2007Style(Ӷ) && GlobalManager.Renderer is Office2007Renderer)
		{
			if (_0EFE.ষ)
			{
				ⲱ.Style.ForeColor.ColorSchemePart = eColorSchemePart.Custom;
				ⲱ.Style.ForeColor.Color = SystemColors.ControlText;
			}
			else
			{
				Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
				ⲱ.Style.ForeColor.ColorSchemePart = eColorSchemePart.Custom;
				ⲱ.Style.ForeColor.Color = colorTable.Form.TextColor;
			}
		}
		if (EnableGlass && _0EFE.ষ)
		{
			ⲭ.ThemeAware = true;
			Ⲯ.ThemeAware = true;
			ⲯ.ThemeAware = true;
			Ⲳ.Style.BackColor1.Color = SystemColors.ControlLight;
			Ⲳ.Style.BorderColor.Color = SystemColors.ControlDark;
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
		int width = Dpi.Width6;
		int num = Dpi.Width(40);
		int width2 = Dpi.Width10;
		int num2 = Dpi.Width(110);
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
		else if (((Control)ⲱ).Size.Width < num2)
		{
			((Control)ⲱ).Width = num2;
		}
		if (((Control)this).Text.Length > 0)
		{
			_ = Size.Empty;
			Font font = ((Control)this).Font;
			Graphics val = BarFunctions.CreateGraphics((Control)(object)this);
			try
			{
				size = TextDrawing.MeasureString(val, ((Control)this).Text, font, 0, eTextFormat.NoPrefix);
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
		int num3 = Math.Max(((Control)ⲱ).Bounds.Bottom, ((Control)Ⲱ).Bounds.Bottom);
		num3 += Dpi.Height(19);
		((Control)ⲭ).Top = num3;
		((Control)Ⲯ).Top = num3;
		((Control)ⲯ).Top = num3;
		int num4 = ((Control)ⲭ).Width + (Ⲵ ? (((Control)Ⲯ).Width + width) : 0) + (ⲵ ? (((Control)ⲯ).Width + width) : 0);
		int num5 = num4 + num * 2;
		if (num4 < ((Control)ⲱ).Bounds.Right + width2)
		{
			num5 = ((Control)ⲱ).Bounds.Right + width2;
		}
		else
		{
			PanelEx panelEx = ⲱ;
			((Control)panelEx).Width = ((Control)panelEx).Width + (num5 - ((Control)ⲱ).Bounds.Right - width2);
		}
		int num6 = (num5 - num4) / 2;
		((Control)ⲭ).Left = num6;
		num6 += ((Control)ⲭ).Width + width;
		if (Ⲵ)
		{
			((Control)Ⲯ).Left = num6;
			num6 += ((Control)Ⲯ).Width + width;
		}
		if (ⲵ)
		{
			((Control)ⲯ).Left = num6;
			num6 += ((Control)ⲯ).Width + width;
		}
		Ἇ ἇ = Ꮛ();
		size = (((Form)this).Size = new Size(((Control)ⲱ).Bounds.Right + width2 + ἇ.ἒ + ἇ.ἓ, ((Control)ⲭ).Bounds.Bottom + width2 + ἇ.ἐ + ἇ.ἑ));
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

	[CompilerGenerated]
	private void Ⳏ()
	{
		((Control)ⲭ).Select();
	}

	[CompilerGenerated]
	private void ⳏ()
	{
		((Control)Ⲯ).Select();
	}

	[CompilerGenerated]
	private void Ⳑ()
	{
		((Control)ⲯ).Select();
	}
}

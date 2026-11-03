using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.ColorPickers;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.ColorPickerItem;

[ToolboxItem(false)]
internal class Ẕ : MetroForm
{
	internal ButtonX ẗ;

	internal ButtonX ẘ;

	internal Label ẙ;

	internal Label ẚ;

	private Color ẛ = Color.Black;

	private Color ẜ = Color.Empty;

	private ColorCombControl ẝ;

	internal Label ẞ;

	internal Label ẟ;

	internal Label Ạ;

	internal Label ạ;

	private TabControl Ả;

	internal TabItem ả;

	private TabControlPanel Ấ;

	internal TabItem ấ;

	private TabControlPanel Ầ;

	private CustomColorBlender ầ;

	internal Label Ẩ;

	internal Label ẩ;

	internal ComboBoxEx Ẫ;

	private IntegerInput ẫ;

	private IntegerInput Ậ;

	private IntegerInput ậ;

	private _2357 Ắ;

	private _235E ắ;

	private IContainer Պ;

	private bool Ằ;

	public Color ẕ
	{
		get
		{
			return ẛ;
		}
		set
		{
			ẛ = value;
			ắ.ẕ = value;
			ầ.SelectedColor = value;
			Ắ._2358 = value;
			if (!value.IsEmpty)
			{
				ậ.Value = value.R;
				ẫ.Value = value.B;
				Ậ.Value = value.G;
			}
		}
	}

	public Color ẖ => ẜ;

	public Ẕ()
	{
		ـ();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		base.Dispose(disposing);
	}

	private void ـ()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Expected O, but got Unknown
		Պ = new Container();
		ẞ = new Label();
		ẝ = new ColorCombControl();
		ạ = new Label();
		Ạ = new Label();
		ẫ = new IntegerInput();
		Ậ = new IntegerInput();
		ậ = new IntegerInput();
		ẟ = new Label();
		ẗ = new ButtonX();
		ẘ = new ButtonX();
		ẙ = new Label();
		ẚ = new Label();
		Ả = new TabControl();
		Ấ = new TabControlPanel();
		ả = new TabItem(Պ);
		Ầ = new TabControlPanel();
		Ắ = new _2357();
		ầ = new CustomColorBlender();
		Ẫ = new ComboBoxEx();
		Ẩ = new Label();
		ẩ = new Label();
		ấ = new TabItem(Պ);
		ắ = new _235E();
		((ISupportInitialize)ẫ).BeginInit();
		((ISupportInitialize)Ậ).BeginInit();
		((ISupportInitialize)ậ).BeginInit();
		((ISupportInitialize)Ả).BeginInit();
		((Control)Ả).SuspendLayout();
		((Control)Ấ).SuspendLayout();
		((Control)Ầ).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)ẞ).BackColor = Color.Transparent;
		((Control)ẞ).Dock = (DockStyle)1;
		((Control)ẞ).Location = new Point(1, 1);
		((Control)ẞ).Name = "labelStandardColors";
		((Control)ẞ).Size = new Size(224, 19);
		((Control)ẞ).TabIndex = 1;
		((Control)ẞ).Text = " Colors:";
		ẞ.TextAlign = (ContentAlignment)256;
		((Control)ẝ).BackColor = Color.Transparent;
		((Control)ẝ).Dock = (DockStyle)5;
		((Control)ẝ).Location = new Point(1, 20);
		((Control)ẝ).Name = "colorCombControl1";
		((Control)ẝ).Size = new Size(224, 267);
		((Control)ẝ).TabIndex = 0;
		ẝ.SelectedColorChanged += ằ;
		((Control)ạ).BackColor = Color.Transparent;
		((Control)ạ).Location = new Point(4, 256);
		((Control)ạ).Name = "labelBlue";
		((Control)ạ).Size = new Size(90, 16);
		((Control)ạ).TabIndex = 10;
		((Control)ạ).Text = "&Blue:";
		((Control)Ạ).BackColor = Color.Transparent;
		((Control)Ạ).Location = new Point(4, 232);
		((Control)Ạ).Name = "labelGreen";
		((Control)Ạ).Size = new Size(90, 16);
		((Control)Ạ).TabIndex = 8;
		((Control)Ạ).Text = "&Green:";
		((Control)ẫ).Location = new Point(98, 260);
		ẫ.MaxValue = 255;
		ẫ.MinValue = 0;
		ẫ.ShowUpDown = true;
		ẫ.AllowEmptyState = false;
		((Control)ẫ).Name = "numericBlue";
		((Control)ẫ).Size = new Size(47, 20);
		((Control)ẫ).TabIndex = 11;
		ẫ.ValueChanged += ẵ;
		((Control)Ậ).Location = new Point(98, 236);
		Ậ.MaxValue = 255;
		Ậ.MinValue = 0;
		Ậ.ShowUpDown = true;
		Ậ.AllowEmptyState = false;
		((Control)Ậ).Name = "numericGreen";
		((Control)Ậ).Size = new Size(47, 20);
		((Control)Ậ).TabIndex = 9;
		Ậ.ValueChanged += ẵ;
		((Control)ậ).Location = new Point(98, 212);
		ậ.MaxValue = 255;
		ậ.MinValue = 0;
		ậ.ShowUpDown = true;
		ậ.AllowEmptyState = false;
		((Control)ậ).Name = "numericRed";
		((Control)ậ).Size = new Size(47, 20);
		((Control)ậ).TabIndex = 7;
		ậ.ValueChanged += ẵ;
		((Control)ẟ).BackColor = Color.Transparent;
		((Control)ẟ).Dock = (DockStyle)1;
		((Control)ẟ).Location = new Point(1, 1);
		((Control)ẟ).Name = "labelCustomColors";
		((Control)ẟ).Size = new Size(224, 20);
		((Control)ẟ).TabIndex = 2;
		((Control)ẟ).Text = " Colors:";
		ẟ.TextAlign = (ContentAlignment)256;
		ẗ.DialogResult = (DialogResult)1;
		((Control)ẗ).Location = new Point(242, 6);
		ẗ.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)ẗ).Name = "buttonOK";
		((Control)ẗ).Size = new Size(74, 24);
		((Control)ẗ).TabIndex = 1;
		((Control)ẗ).Text = "OK";
		ẘ.DialogResult = (DialogResult)2;
		((Control)ẘ).Location = new Point(242, 36);
		ẘ.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)ẘ).Name = "buttonCancel";
		((Control)ẘ).Size = new Size(74, 24);
		((Control)ẘ).TabIndex = 2;
		((Control)ẘ).Text = "Cancel";
		((Control)ẙ).Location = new Point(256, 232);
		((Control)ẙ).Name = "labelNewColor";
		((Control)ẙ).Size = new Size(44, 12);
		((Control)ẙ).TabIndex = 4;
		((Control)ẙ).Text = "New";
		ẙ.TextAlign = (ContentAlignment)32;
		((Control)ẚ).Location = new Point(256, 306);
		((Control)ẚ).Name = "labelCurrentColor";
		((Control)ẚ).Size = new Size(44, 16);
		((Control)ẚ).TabIndex = 5;
		((Control)ẚ).Text = "Current";
		ẚ.TextAlign = (ContentAlignment)32;
		((Control)Ả).BackColor = Color.White;
		((Control)Ả).ForeColor = Color.Black;
		Ả.CanReorderTabs = false;
		((Control)Ả).Controls.Add((Control)(object)Ầ);
		((Control)Ả).Controls.Add((Control)(object)Ấ);
		Ả.FixedTabSize = new Size(60, 0);
		((Control)Ả).Location = new Point(8, 8);
		((Control)Ả).Name = "tabControl2";
		Ả.SelectedTabFont = new Font("Segoe UI", 9f, (FontStyle)1);
		Ả.SelectedTabIndex = 0;
		((Control)Ả).Size = new Size(226, 314);
		Ả.Style = eTabStripStyle.Metro;
		((Control)Ả).TabIndex = 6;
		Ả.TabLayoutType = eTabLayoutType.FixedWithNavigationBox;
		Ả.Tabs.Add(ả);
		Ả.Tabs.Add(ấ);
		Ả.ThemeAware = true;
		((Control)Ấ).Controls.Add((Control)(object)ẝ);
		((Control)Ấ).Controls.Add((Control)(object)ẞ);
		((Control)Ấ).Dock = (DockStyle)5;
		((ScrollableControl)Ấ).DockPadding.All = 1;
		((Control)Ấ).Location = new Point(0, 26);
		((Control)Ấ).Name = "tabControlPanel1";
		((Control)Ấ).Size = new Size(226, 288);
		Ấ.Style.BackColor1.Color = Color.White;
		Ấ.Style.Border = eBorderType.SingleLine;
		Ấ.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		Ấ.Style.GradientAngle = 90;
		((Control)Ấ).TabIndex = 1;
		Ấ.TabItem = ả;
		Ấ.ThemeAware = false;
		ả.AttachedControl = (Control)(object)Ấ;
		ả.CloseButtonBounds = new Rectangle(0, 0, 0, 0);
		ả.Name = "tabItemStandard";
		ả.Text = "Standard";
		((Control)Ầ).Controls.Add((Control)(object)Ắ);
		((Control)Ầ).Controls.Add((Control)(object)Ậ);
		((Control)Ầ).Controls.Add((Control)(object)Ạ);
		((Control)Ầ).Controls.Add((Control)(object)ậ);
		((Control)Ầ).Controls.Add((Control)(object)ầ);
		((Control)Ầ).Controls.Add((Control)(object)Ẫ);
		((Control)Ầ).Controls.Add((Control)(object)Ẩ);
		((Control)Ầ).Controls.Add((Control)(object)ẩ);
		((Control)Ầ).Controls.Add((Control)(object)ẫ);
		((Control)Ầ).Controls.Add((Control)(object)ạ);
		((Control)Ầ).Controls.Add((Control)(object)ẟ);
		((Control)Ầ).Dock = (DockStyle)5;
		((ScrollableControl)Ầ).DockPadding.All = 1;
		((Control)Ầ).Location = new Point(0, 26);
		((Control)Ầ).Name = "tabControlPanel2";
		((Control)Ầ).Size = new Size(226, 288);
		Ầ.Style.BackColor1.Color = Color.White;
		Ầ.Style.Border = eBorderType.SingleLine;
		Ầ.Style.BorderSide = eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom;
		Ầ.Style.GradientAngle = 90;
		((Control)Ầ).TabIndex = 2;
		Ầ.TabItem = ấ;
		Ầ.ThemeAware = true;
		((Control)Ắ).BackColor = Color.Transparent;
		((Control)Ắ).Location = new Point(188, 28);
		((Control)Ắ).Name = "colorContrastControl1";
		Ắ._2358 = Color.FromArgb(255, 255, 255);
		((Control)Ắ).Size = new Size(32, 152);
		((Control)Ắ).TabIndex = 12;
		Ắ.Ṳ += Ặ;
		((Control)ầ).Location = new Point(8, 28);
		((Control)ầ).Name = "customColorBlender3";
		((Control)ầ).Size = new Size(174, 152);
		((Control)ầ).TabIndex = 3;
		ầ.SelectedColorChanged += Ẳ;
		((ComboBox)Ẫ).DrawMode = (DrawMode)1;
		((ComboBox)Ẫ).DropDownStyle = (ComboBoxStyle)2;
		Ẫ.Items.AddRange(new object[1] { "RGB" });
		((Control)Ẫ).Location = new Point(98, 188);
		((Control)Ẫ).Name = "comboColorModel";
		((Control)Ẫ).Size = new Size(89, 21);
		((Control)Ẫ).TabIndex = 5;
		((Control)Ẩ).BackColor = Color.Transparent;
		((Control)Ẩ).Location = new Point(4, 212);
		((Control)Ẩ).Name = "labelRed";
		((Control)Ẩ).Size = new Size(90, 16);
		((Control)Ẩ).TabIndex = 6;
		((Control)Ẩ).Text = "&Red:";
		((Control)ẩ).BackColor = Color.Transparent;
		((Control)ẩ).Location = new Point(4, 192);
		((Control)ẩ).Name = "labelColorModel";
		((Control)ẩ).Size = new Size(90, 16);
		((Control)ẩ).TabIndex = 4;
		((Control)ẩ).Text = "Color Mo&del:";
		ấ.AttachedControl = (Control)(object)Ầ;
		ấ.CloseButtonBounds = new Rectangle(0, 0, 0, 0);
		ấ.Name = "tabItemCustom";
		ấ.Text = "Custom";
		ắ.ẕ = Color.Black;
		((Control)ắ).Location = new Point(252, 252);
		((Control)ắ).Name = "colorSelectionPreview";
		ắ.ẖ = Color.Empty;
		((Control)ắ).Size = new Size(56, 48);
		((Control)ắ).TabIndex = 7;
		((Form)this).AcceptButton = (IButtonControl)(object)ẗ;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).CancelButton = (IButtonControl)(object)ẘ;
		((Form)this).ClientSize = new Size(324, 326);
		((Control)this).Controls.Add((Control)(object)ắ);
		((Control)this).Controls.Add((Control)(object)Ả);
		((Control)this).Controls.Add((Control)(object)ẚ);
		((Control)this).Controls.Add((Control)(object)ẙ);
		((Control)this).Controls.Add((Control)(object)ẘ);
		((Control)this).Controls.Add((Control)(object)ẗ);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "CustomColorDialog";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "Colors";
		((Form)this).Load += Ẵ;
		((ISupportInitialize)ẫ).EndInit();
		((ISupportInitialize)Ậ).EndInit();
		((ISupportInitialize)ậ).EndInit();
		((ISupportInitialize)Ả).EndInit();
		Ả.ResumeLayout(performLayout: false);
		((Control)Ấ).ResumeLayout(false);
		((Control)Ầ).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		Size fixedTabSize = Ả.FixedTabSize;
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			Size size = TextDrawing.MeasureString(val, ấ.Text, ((Control)this).Font);
			Size size2 = TextDrawing.MeasureString(val, ả.Text, ((Control)this).Font);
			fixedTabSize.Width = Math.Max(fixedTabSize.Width, size.Width + 5);
			fixedTabSize.Width = Math.Max(fixedTabSize.Width, size2.Width + 5);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Ả.FixedTabSize = fixedTabSize;
		if (fixedTabSize.Width > ((Control)Ả).Width / 2)
		{
			Ả.FixedTabSize = Size.Empty;
		}
	}

	internal void _᱐(eDotNetBarStyle _0657)
	{
		ẘ.Style = _0657;
		ẗ.Style = _0657;
	}

	private void ằ(object _٠, EventArgs ؿ)
	{
		ẳ(ẝ.SelectedColor);
		Ắ._2358 = ẝ.SelectedColor;
	}

	private void Ẳ(object _٠, EventArgs ؿ)
	{
		Ắ._2358 = ầ.SelectedColor;
		ẳ(Ắ._2358);
	}

	private void ẳ(Color _065B)
	{
		if (Ằ)
		{
			return;
		}
		Ằ = true;
		try
		{
			ẜ = _065B;
			ắ.ẖ = _065B;
			ậ.Value = ẜ.R;
			Ậ.Value = ẜ.G;
			ẫ.Value = ẜ.B;
		}
		finally
		{
			Ằ = false;
		}
	}

	private void Ẵ(object _٠, EventArgs ؿ)
	{
		((ListControl)Ẫ).SelectedIndex = 0;
	}

	private void ẵ(object _٠, EventArgs ؿ)
	{
		ẳ(Color.FromArgb(ậ.Value, Ậ.Value, ẫ.Value));
	}

	private void Ặ(object _٠, EventArgs ؿ)
	{
		ẳ(Ắ._2358);
	}
}

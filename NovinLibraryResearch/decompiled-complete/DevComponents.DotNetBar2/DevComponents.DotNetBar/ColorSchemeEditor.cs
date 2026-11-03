using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace DevComponents.DotNetBar;

public class ColorSchemeEditor : Form
{
	private PropertyGrid _28E5;

	private GroupBox Ḍ;

	private Container Պ;

	private ColorScheme լ;

	private Button ḇ;

	private Button Ḉ;

	private MenuPanel _28E6;

	private Bar _28E7;

	private Bar _28E8;

	private Button _28E9;

	private Bar ಕ;

	private Button _28EA;

	private Button _28EB;

	private OpenFileDialog _28EC;

	private SaveFileDialog _28ED;

	public bool ColorSchemeChanged;

	public ColorScheme ColorScheme
	{
		get
		{
			return լ;
		}
		set
		{
			if (_28E6 == null)
			{
				_28EE();
			}
			լ = value;
			_28E5.SelectedObject = լ;
			_28E6.ColorScheme = լ;
			_28E7.ColorScheme = լ;
			_28E8.ColorScheme = լ;
			ಕ.ColorScheme = լ;
			ColorSchemeChanged = false;
		}
	}

	public ColorSchemeEditor()
	{
		ـ();
		((Form)this).StartPosition = (FormStartPosition)1;
	}

	private void _28EE()
	{
		if (_28E6 != null)
		{
			_28E6.RecalcSize();
			_28E7.Size = new Size(((Control)Ḍ).ClientRectangle.Width - 12, 16);
			_28E7.RecalcSize();
			_28E8.Size = new Size(((Control)Ḍ).ClientRectangle.Width, 16);
			_28E8.RecalcSize();
			ಕ.Size = new Size(((Control)Ḍ).ClientRectangle.Width, 24);
			ಕ.RecalcSize();
			return;
		}
		_28E6 = new MenuPanel();
		_28E6.PopupMenu = false;
		((Control)_28E6).Location = new Point(8, 135);
		_28E7 = new Bar();
		_28E7.ஏ = true;
		_28E7.Location = new Point(4, 16);
		_28E7.ThemeAware = false;
		_28E8 = new Bar();
		_28E8.ஏ = true;
		_28E8.Location = new Point(4, 46);
		_28E8.ThemeAware = false;
		ಕ = new Bar();
		ಕ.ஏ = true;
		ಕ.Location = new Point(4, 96);
		ಕ.ThemeAware = false;
		ButtonItem buttonItem = new ButtonItem();
		Bitmap val = null;
		ButtonItem buttonItem2 = new ButtonItem("new", "&New...");
		val = BarFunctions.ই("BarEditorImages.FileNew.bmp");
		val.MakeTransparent(Color.Magenta);
		buttonItem2.Image = (Image)(object)val;
		_28E7.Items.Add(buttonItem2.Copy());
		_28E8.Items.Add(buttonItem2.Copy());
		ಕ.Items.Add(buttonItem2.Copy());
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("open", "&Open");
		val = BarFunctions.ই("BarEditorImages.FileOpen.bmp");
		val.MakeTransparent(Color.Magenta);
		buttonItem2.Image = (Image)(object)val;
		ButtonItem buttonItem3 = (ButtonItem)buttonItem2.Copy();
		buttonItem3.ButtonStyle = eButtonStyle.ImageAndText;
		_28E7.Items.Add(buttonItem3);
		_28E8.Items.Add(buttonItem3.Copy());
		ಕ.Items.Add(buttonItem3.Copy());
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("close", "&Close");
		val = BarFunctions.ই("BarEditorImages.FileClose.bmp");
		val.MakeTransparent(Color.Magenta);
		buttonItem2.Image = (Image)(object)val;
		buttonItem3 = (ButtonItem)buttonItem2.Copy();
		buttonItem3.Checked = true;
		buttonItem3.ButtonStyle = eButtonStyle.ImageAndText;
		_28E7.Items.Add(buttonItem3);
		ಕ.Items.Add(buttonItem3.Copy());
		buttonItem3 = (ButtonItem)buttonItem3.Copy();
		buttonItem3.Enabled = false;
		_28E8.Items.Add(buttonItem3.Copy());
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("open", "Add Ne&w Item...");
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("open", "Add Existin&g Item...");
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("opensol", "Open Solution...");
		buttonItem2.BeginGroup = true;
		val = BarFunctions.ই("BarEditorImages.FileOpenSol.bmp");
		val.MakeTransparent(Color.Magenta);
		buttonItem2.Image = (Image)(object)val;
		buttonItem3 = (ButtonItem)buttonItem2.Copy();
		buttonItem3.Enabled = false;
		_28E7.Items.Add(buttonItem3);
		_28E8.Items.Add(buttonItem3.Copy());
		ಕ.Items.Add(buttonItem3.Copy());
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("open", "Close Solution");
		val = BarFunctions.ই("BarEditorImages.FileCloseSol.bmp");
		val.MakeTransparent(Color.Magenta);
		buttonItem2.Image = (Image)(object)val;
		buttonItem2.Enabled = false;
		buttonItem.SubItems.Add(buttonItem2);
		_28E6.ParentItem = buttonItem;
		((Control)Ḍ).Controls.Add((Control)(object)_28E6);
		_28E6.RecalcSize();
		_28E6.Show();
		_28E7.Size = new Size(((Control)Ḍ).ClientRectangle.Width, 16);
		_28E7.GrabHandleStyle = eGrabHandleStyle.StripeFlat;
		((Control)Ḍ).Controls.Add((Control)(object)_28E7);
		_28E7.RecalcSize();
		_28E7.Show();
		_28E8.మ(eBarState.Floating);
		_28E8.Size = new Size(((Control)Ḍ).ClientRectangle.Width, 16);
		((Control)Ḍ).Controls.Add((Control)(object)_28E8);
		((Control)_28E8).Text = "Bar Caption";
		_28E8.RecalcSize();
		_28E8.Show();
		ಕ.మ(eBarState.Popup);
		ಕ.Size = new Size(((Control)Ḍ).ClientRectangle.Width, 24);
		ಕ._0B0E = ((Control)Ḍ).ClientRectangle.Width;
		((Control)Ḍ).Controls.Add((Control)(object)ಕ);
		ಕ.RecalcSize();
		ಕ.Show();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void ـ()
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
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		_28E5 = new PropertyGrid();
		Ḍ = new GroupBox();
		ḇ = new Button();
		Ḉ = new Button();
		_28E9 = new Button();
		_28EA = new Button();
		_28EB = new Button();
		_28EC = new OpenFileDialog();
		_28ED = new SaveFileDialog();
		((Control)this).SuspendLayout();
		_28E5.CommandsVisibleIfAvailable = true;
		_28E5.LargeButtons = false;
		_28E5.LineColor = SystemColors.ScrollBar;
		((Control)_28E5).Location = new Point(208, 4);
		((Control)_28E5).Name = "propertyGrid1";
		((Control)_28E5).Size = new Size(352, 299);
		((Control)_28E5).TabIndex = 0;
		((Control)_28E5).Text = "propertyGrid1";
		_28E5.ViewBackColor = SystemColors.Window;
		_28E5.ViewForeColor = SystemColors.WindowText;
		_28E5.PropertyValueChanged += new PropertyValueChangedEventHandler(_28F0);
		((Control)Ḍ).Location = new Point(10, 0);
		((Control)Ḍ).Name = "groupBox1";
		((Control)Ḍ).Size = new Size(190, 303);
		((Control)Ḍ).TabIndex = 1;
		Ḍ.TabStop = false;
		((Control)Ḍ).Text = "Color Scheme Preview:";
		((ButtonBase)ḇ).FlatStyle = (FlatStyle)3;
		((Control)ḇ).Location = new Point(404, 307);
		((Control)ḇ).Name = "button1";
		((Control)ḇ).Size = new Size(76, 24);
		((Control)ḇ).TabIndex = 2;
		((Control)ḇ).Text = "OK";
		((Control)ḇ).Click += ḗ;
		((ButtonBase)Ḉ).FlatStyle = (FlatStyle)3;
		((Control)Ḉ).Location = new Point(316, 307);
		((Control)Ḉ).Name = "button2";
		((Control)Ḉ).Size = new Size(76, 24);
		((Control)Ḉ).TabIndex = 3;
		((Control)Ḉ).Text = "&Reset";
		((Control)Ḉ).Click += Ḗ;
		((ButtonBase)_28E9).FlatStyle = (FlatStyle)3;
		((Control)_28E9).Location = new Point(484, 307);
		((Control)_28E9).Name = "button3";
		((Control)_28E9).Size = new Size(76, 24);
		((Control)_28E9).TabIndex = 4;
		((Control)_28E9).Text = "Cancel";
		((Control)_28E9).Click += _28F1;
		((ButtonBase)_28EA).FlatStyle = (FlatStyle)3;
		((Control)_28EA).Location = new Point(10, 308);
		((Control)_28EA).Name = "btnSaveScheme";
		((Control)_28EA).Size = new Size(92, 24);
		((Control)_28EA).TabIndex = 5;
		((Control)_28EA).Text = "Save Scheme...";
		((Control)_28EA).Click += _28F2;
		((ButtonBase)_28EB).FlatStyle = (FlatStyle)3;
		((Control)_28EB).Location = new Point(107, 308);
		((Control)_28EB).Name = "btnLoadScheme";
		((Control)_28EB).Size = new Size(92, 24);
		((Control)_28EB).TabIndex = 6;
		((Control)_28EB).Text = "Load Scheme...";
		((Control)_28EB).Click += _28F3;
		((FileDialog)_28EC).DefaultExt = "*.dcs";
		((FileDialog)_28EC).Filter = "DotNetBar Color Scheme files|*.dcs|All files|*.*";
		((FileDialog)_28EC).Title = "Open DotNetBar Color Scheme File";
		((FileDialog)_28ED).DefaultExt = "dcs";
		((FileDialog)_28ED).FileName = "colorscheme1";
		((FileDialog)_28ED).Filter = "DotNetBar Color Scheme files|*.dcs|All files|*.*";
		((FileDialog)_28ED).Title = "Save DotNetBar Color Scheme File";
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(567, 338);
		((Control)this).Controls.AddRange((Control[])(object)new Control[7]
		{
			(Control)_28EB,
			(Control)_28EA,
			(Control)_28E9,
			(Control)Ḉ,
			(Control)ḇ,
			(Control)Ḍ,
			(Control)_28E5
		});
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "ColorSchemeEditor";
		((Control)this).Text = "ColorScheme Editor";
		((Form)this).Load += _28EF;
		((Control)this).ResumeLayout(false);
	}

	private void _28EF(object _٠, EventArgs ؿ)
	{
		_28EE();
	}

	private void _28F0(object ٮ, PropertyValueChangedEventArgs ؿ)
	{
		((Control)_28E6).Refresh();
		((Control)_28E7).Refresh();
		((Control)_28E8).Refresh();
		((Control)ಕ).Refresh();
		ColorSchemeChanged = true;
	}

	private void ḗ(object _٠, EventArgs ؿ)
	{
		((Control)this).Hide();
	}

	private void Ḗ(object _٠, EventArgs ؿ)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		if ((int)MessageBox.Show("Reseting the Color Scheme will destroy any changes you made. Are you sure you want to reset the Color Scheme?", "Color Scheme Editor", (MessageBoxButtons)4) == 6)
		{
			ColorScheme = new ColorScheme(ColorScheme.ߕ);
			ColorSchemeChanged = true;
		}
	}

	private void _28F1(object _٠, EventArgs ؿ)
	{
		ColorSchemeChanged = false;
		((Control)this).Hide();
	}

	private void _28F2(object _٠, EventArgs ؿ)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if ((int)((CommonDialog)_28ED).ShowDialog() == 1)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = xmlDocument.CreateElement("colorschemes");
			xmlDocument.AppendChild(xmlElement);
			XmlElement xmlElement2 = xmlDocument.CreateElement("colorscheme");
			xmlElement.AppendChild(xmlElement2);
			լ.Serialize(xmlElement2);
			xmlDocument.Save(((FileDialog)_28ED).FileName);
			MessageBox.Show("Color Scheme has been saved.", "DotNetBar Color Scheme Editor", (MessageBoxButtons)0, (MessageBoxIcon)64);
		}
	}

	private void _28F3(object _٠, EventArgs ؿ)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)_28EC).ShowDialog() == 1 && File.Exists(((FileDialog)_28EC).FileName))
		{
			ColorScheme colorScheme = new ColorScheme();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(((FileDialog)_28EC).FileName);
			colorScheme.Deserialize(xmlDocument.FirstChild.FirstChild as XmlElement);
			ColorScheme = colorScheme;
			ColorSchemeChanged = true;
		}
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar;

namespace DevComponents.UI;

[ToolboxItem(false)]
internal class ӓ : UserControl
{
	private Color[] _2993 = new Color[48];

	private Rectangle[] _2994 = new Rectangle[48];

	private object լ;

	private string _2995 = "";

	private TabControl _2996;

	private TabPage _2963;

	private TabPage _2961;

	private TabPage _2962;

	private TabPage _2997;

	private Label ḅ;

	private ListBox _2998;

	private Button _2999;

	private Button _299A;

	private Panel _299B;

	private ListBox _299C;

	private ListBox _299D;

	private Color Ṽ = Color.Empty;

	private string _299E = "";

	private Panel _299F;

	private TrackBar _29A0;

	private bool _29A1;

	private IWindowsFormsEditorService _28E4;

	private Container Պ;

	internal IWindowsFormsEditorService _2990
	{
		get
		{
			return _28E4;
		}
		set
		{
			_28E4 = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public object ԫ
	{
		get
		{
			return լ;
		}
		set
		{
			լ = value;
			_29A4();
		}
	}

	public Color _2358
	{
		get
		{
			return Ṽ;
		}
		set
		{
			Ṽ = value;
			_29A9();
		}
	}

	public string _2991
	{
		get
		{
			return _299E;
		}
		set
		{
			_299E = value;
		}
	}

	public bool _2992 => _29A1;

	public ӓ()
	{
		ـ();
		_29A2();
		_29A3();
		_2995 = ((Control)ḅ).Text;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Expected O, but got Unknown
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Expected O, but got Unknown
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Expected O, but got Unknown
		_2996 = new TabControl();
		_2963 = new TabPage();
		_2998 = new ListBox();
		_2961 = new TabPage();
		_299C = new ListBox();
		_2962 = new TabPage();
		_299D = new ListBox();
		_2997 = new TabPage();
		_299B = new Panel();
		_29A0 = new TrackBar();
		ḅ = new Label();
		_299F = new Panel();
		_2999 = new Button();
		_299A = new Button();
		((Control)_2996).SuspendLayout();
		((Control)_2963).SuspendLayout();
		((Control)_2961).SuspendLayout();
		((Control)_2962).SuspendLayout();
		((Control)_2997).SuspendLayout();
		((ISupportInitialize)_29A0).BeginInit();
		((Control)this).SuspendLayout();
		((Control)_2996).Controls.Add((Control)(object)_2963);
		((Control)_2996).Controls.Add((Control)(object)_2961);
		((Control)_2996).Controls.Add((Control)(object)_2962);
		((Control)_2996).Controls.Add((Control)(object)_2997);
		((Control)_2996).Location = new Point(1, 1);
		((Control)_2996).Name = "tabControl1";
		_2996.SelectedIndex = 0;
		((Control)_2996).Size = new Size(208, 192);
		((Control)_2996).TabIndex = 0;
		_2996.SelectedIndexChanged += _29B4;
		((Control)_2963).Controls.Add((Control)(object)_2998);
		_2963.Location = new Point(4, 22);
		((Control)_2963).Name = "tabPage1";
		((Control)_2963).Size = new Size(200, 166);
		_2963.TabIndex = 0;
		((Control)_2963).Text = "Scheme";
		((Control)_2998).Dock = (DockStyle)5;
		_2998.DrawMode = (DrawMode)1;
		_2998.IntegralHeight = false;
		((Control)_2998).Location = new Point(0, 0);
		((Control)_2998).Name = "listScheme";
		((Control)_2998).Size = new Size(200, 166);
		((Control)_2998).TabIndex = 0;
		_2998.DrawItem += new DrawItemEventHandler(_29A6);
		_2998.SelectedIndexChanged += _29A8;
		((Control)_2961).Controls.Add((Control)(object)_299C);
		_2961.Location = new Point(4, 22);
		((Control)_2961).Name = "tabPage2";
		((Control)_2961).Size = new Size(200, 166);
		_2961.TabIndex = 1;
		((Control)_2961).Text = "System";
		((Control)_299C).Dock = (DockStyle)5;
		_299C.DrawMode = (DrawMode)1;
		_299C.IntegralHeight = false;
		((Control)_299C).Location = new Point(0, 0);
		((Control)_299C).Name = "listSystem";
		((Control)_299C).Size = new Size(200, 166);
		((Control)_299C).TabIndex = 1;
		_299C.DrawItem += new DrawItemEventHandler(_29A6);
		_299C.SelectedIndexChanged += _29A8;
		((Control)_2962).Controls.Add((Control)(object)_299D);
		_2962.Location = new Point(4, 22);
		((Control)_2962).Name = "tabPage3";
		((Control)_2962).Size = new Size(200, 166);
		_2962.TabIndex = 2;
		((Control)_2962).Text = "Web";
		((Control)_299D).Dock = (DockStyle)5;
		_299D.DrawMode = (DrawMode)1;
		_299D.IntegralHeight = false;
		((Control)_299D).Location = new Point(0, 0);
		((Control)_299D).Name = "listWeb";
		((Control)_299D).Size = new Size(200, 166);
		((Control)_299D).TabIndex = 1;
		_299D.DrawItem += new DrawItemEventHandler(_29A6);
		_299D.SelectedIndexChanged += _29A8;
		((Control)_2997).Controls.Add((Control)(object)_299B);
		_2997.Location = new Point(4, 22);
		((Control)_2997).Name = "tabPage4";
		((Control)_2997).Size = new Size(200, 166);
		_2997.TabIndex = 3;
		((Control)_2997).Text = "Custom";
		((Control)_299B).Dock = (DockStyle)5;
		((Control)_299B).Location = new Point(0, 0);
		((Control)_299B).Name = "colorPanel";
		((Control)_299B).Size = new Size(200, 166);
		((Control)_299B).TabIndex = 0;
		((Control)_299B).MouseUp += new MouseEventHandler(_29A7);
		((Control)_299B).Paint += new PaintEventHandler(_29A5);
		((Control)_29A0).Enabled = false;
		((Control)_29A0).Location = new Point(1, 204);
		_29A0.Maximum = 255;
		((Control)_29A0).Name = "transparencyTrack";
		((Control)_29A0).Size = new Size(200, 45);
		((Control)_29A0).TabIndex = 1;
		_29A0.TickFrequency = 16;
		_29A0.Value = 255;
		_29A0.ValueChanged += _29AE;
		((Control)ḅ).Location = new Point(1, 194);
		((Control)ḅ).Name = "label1";
		((Control)ḅ).Size = new Size(136, 16);
		((Control)ḅ).TabIndex = 2;
		((Control)ḅ).Text = "Transparency";
		((Control)_299F).BackColor = Color.White;
		_299F.BorderStyle = (BorderStyle)1;
		((Control)_299F).Location = new Point(8, 240);
		((Control)_299F).Name = "colorPreview";
		((Control)_299F).Size = new Size(40, 32);
		((Control)_299F).TabIndex = 3;
		((Control)_299F).Paint += new PaintEventHandler(_29AF);
		((ButtonBase)_2999).FlatStyle = (FlatStyle)3;
		((Control)_2999).Location = new Point(72, 248);
		((Control)_2999).Name = "btnOK";
		((Control)_2999).Size = new Size(64, 24);
		((Control)_2999).TabIndex = 4;
		((Control)_2999).Text = "OK";
		((Control)_2999).Click += _29B0;
		((ButtonBase)_299A).FlatStyle = (FlatStyle)3;
		((Control)_299A).Location = new Point(142, 248);
		((Control)_299A).Name = "btnCancel";
		((Control)_299A).Size = new Size(64, 24);
		((Control)_299A).TabIndex = 5;
		((Control)_299A).Text = "Cancel";
		((Control)_299A).Click += _29B1;
		((Control)this).Controls.Add((Control)(object)_299A);
		((Control)this).Controls.Add((Control)(object)_2999);
		((Control)this).Controls.Add((Control)(object)_299F);
		((Control)this).Controls.Add((Control)(object)ḅ);
		((Control)this).Controls.Add((Control)(object)_2996);
		((Control)this).Controls.Add((Control)(object)_29A0);
		((ScrollableControl)this).DockPadding.All = 1;
		((Control)this).Name = "ColorPicker";
		((Control)this).Size = new Size(211, 280);
		((Control)this).Paint += new PaintEventHandler(_29B3);
		((Control)_2996).ResumeLayout(false);
		((Control)_2963).ResumeLayout(false);
		((Control)_2961).ResumeLayout(false);
		((Control)_2962).ResumeLayout(false);
		((Control)_2997).ResumeLayout(false);
		((ISupportInitialize)_29A0).EndInit();
		((Control)this).ResumeLayout(false);
	}

	private void _29A2()
	{
		ref Color reference = ref _2993[0];
		reference = Color.FromArgb(255, 255, 255);
		ref Color reference2 = ref _2993[1];
		reference2 = Color.FromArgb(255, 195, 198);
		ref Color reference3 = ref _2993[2];
		reference3 = Color.FromArgb(255, 227, 198);
		ref Color reference4 = ref _2993[3];
		reference4 = Color.FromArgb(255, 255, 198);
		ref Color reference5 = ref _2993[4];
		reference5 = Color.FromArgb(198, 255, 198);
		ref Color reference6 = ref _2993[5];
		reference6 = Color.FromArgb(198, 255, 255);
		ref Color reference7 = ref _2993[6];
		reference7 = Color.FromArgb(198, 195, 255);
		ref Color reference8 = ref _2993[7];
		reference8 = Color.FromArgb(255, 195, 255);
		ref Color reference9 = ref _2993[8];
		reference9 = Color.FromArgb(231, 227, 231);
		ref Color reference10 = ref _2993[9];
		reference10 = Color.FromArgb(255, 130, 132);
		ref Color reference11 = ref _2993[10];
		reference11 = Color.FromArgb(255, 195, 132);
		ref Color reference12 = ref _2993[11];
		reference12 = Color.FromArgb(255, 255, 132);
		ref Color reference13 = ref _2993[12];
		reference13 = Color.FromArgb(132, 255, 132);
		ref Color reference14 = ref _2993[13];
		reference14 = Color.FromArgb(132, 255, 255);
		ref Color reference15 = ref _2993[14];
		reference15 = Color.FromArgb(132, 130, 255);
		ref Color reference16 = ref _2993[15];
		reference16 = Color.FromArgb(255, 130, 255);
		ref Color reference17 = ref _2993[16];
		reference17 = Color.FromArgb(198, 195, 198);
		ref Color reference18 = ref _2993[17];
		reference18 = Color.FromArgb(255, 0, 0);
		ref Color reference19 = ref _2993[18];
		reference19 = Color.FromArgb(255, 130, 0);
		ref Color reference20 = ref _2993[19];
		reference20 = Color.FromArgb(255, 255, 0);
		ref Color reference21 = ref _2993[20];
		reference21 = Color.FromArgb(0, 255, 0);
		ref Color reference22 = ref _2993[21];
		reference22 = Color.FromArgb(0, 255, 255);
		ref Color reference23 = ref _2993[22];
		reference23 = Color.FromArgb(0, 0, 255);
		ref Color reference24 = ref _2993[23];
		reference24 = Color.FromArgb(255, 0, 255);
		ref Color reference25 = ref _2993[24];
		reference25 = Color.FromArgb(132, 130, 132);
		ref Color reference26 = ref _2993[25];
		reference26 = Color.FromArgb(198, 0, 0);
		ref Color reference27 = ref _2993[26];
		reference27 = Color.FromArgb(198, 65, 0);
		ref Color reference28 = ref _2993[27];
		reference28 = Color.FromArgb(198, 195, 0);
		ref Color reference29 = ref _2993[28];
		reference29 = Color.FromArgb(0, 195, 0);
		ref Color reference30 = ref _2993[29];
		reference30 = Color.FromArgb(0, 195, 198);
		ref Color reference31 = ref _2993[30];
		reference31 = Color.FromArgb(0, 0, 198);
		ref Color reference32 = ref _2993[31];
		reference32 = Color.FromArgb(198, 0, 198);
		ref Color reference33 = ref _2993[32];
		reference33 = Color.FromArgb(66, 65, 66);
		ref Color reference34 = ref _2993[33];
		reference34 = Color.FromArgb(132, 0, 0);
		ref Color reference35 = ref _2993[34];
		reference35 = Color.FromArgb(132, 65, 0);
		ref Color reference36 = ref _2993[35];
		reference36 = Color.FromArgb(132, 130, 0);
		ref Color reference37 = ref _2993[36];
		reference37 = Color.FromArgb(0, 130, 0);
		ref Color reference38 = ref _2993[37];
		reference38 = Color.FromArgb(0, 130, 132);
		ref Color reference39 = ref _2993[38];
		reference39 = Color.FromArgb(0, 0, 132);
		ref Color reference40 = ref _2993[39];
		reference40 = Color.FromArgb(132, 0, 132);
		ref Color reference41 = ref _2993[40];
		reference41 = Color.FromArgb(0, 0, 0);
		ref Color reference42 = ref _2993[41];
		reference42 = Color.FromArgb(66, 0, 0);
		ref Color reference43 = ref _2993[42];
		reference43 = Color.FromArgb(132, 65, 66);
		ref Color reference44 = ref _2993[43];
		reference44 = Color.FromArgb(66, 65, 0);
		ref Color reference45 = ref _2993[44];
		reference45 = Color.FromArgb(0, 65, 0);
		ref Color reference46 = ref _2993[45];
		reference46 = Color.FromArgb(0, 65, 66);
		ref Color reference47 = ref _2993[46];
		reference47 = Color.FromArgb(0, 0, 66);
		ref Color reference48 = ref _2993[47];
		reference48 = Color.FromArgb(66, 0, 66);
	}

	private void _29A3()
	{
		_299D.BeginUpdate();
		_299D.Items.Clear();
		Type typeFromHandle = typeof(Color);
		PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Static | BindingFlags.Public);
		Color color = default(Color);
		PropertyInfo[] array = properties;
		foreach (PropertyInfo propertyInfo in array)
		{
			_299D.Items.Add(propertyInfo.GetValue(color, null));
		}
		_299D.EndUpdate();
		_299C.BeginUpdate();
		_299C.Items.Clear();
		typeFromHandle = typeof(SystemColors);
		properties = typeFromHandle.GetProperties(BindingFlags.Static | BindingFlags.Public);
		PropertyInfo[] array2 = properties;
		foreach (PropertyInfo propertyInfo2 in array2)
		{
			_299C.Items.Add(propertyInfo2.GetValue(color, null));
		}
		_299C.EndUpdate();
	}

	private void _29A4()
	{
		if (լ != null)
		{
			if (!_2996.TabPages.Contains(_2963))
			{
				_2996.TabPages.Add(_2963);
			}
			_2998.BeginUpdate();
			_2998.Items.Clear();
			Type type = լ.GetType();
			PropertyInfo[] properties = type.GetProperties();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				if ((object)propertyInfo.PropertyType == typeof(Color))
				{
					_2998.Items.Add((object)propertyInfo.Name);
				}
			}
			_2998.EndUpdate();
		}
		else if (_2996.TabPages.Contains(_2963))
		{
			_2996.TabPages.Remove(_2963);
		}
	}

	private void _29A5(object _٠, PaintEventArgs ؿ)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		Rectangle empty = Rectangle.Empty;
		int num = 6;
		int num2 = 12;
		Graphics graphics = ؿ.Graphics;
		Border3DSide val = (Border3DSide)15;
		int width = ((Control)_299B).ClientRectangle.Width;
		int num3 = 0;
		Color[] array = _2993;
		foreach (Color color in array)
		{
			empty = new Rectangle(num, num2, 21, 21);
			if (empty.Right > width)
			{
				num2 += 25;
				num = (empty.X = 6);
				empty.Y = num2;
			}
			ControlPaint.DrawBorder3D(graphics, num, num2, 21, 21, (Border3DStyle)10, val);
			empty.Inflate(-2, -2);
			graphics.FillRectangle((Brush)new SolidBrush(color), empty);
			_2994[num3] = empty;
			num3++;
			num += 24;
		}
	}

	private void _29A6(object _٠, DrawItemEventArgs ؿ)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		Rectangle bounds = ؿ.Bounds;
		Rectangle rectangle = new Rectangle(bounds.X + 1, bounds.Y + 2, 24, bounds.Height - 4);
		ListBox val = (ListBox)((_٠ is ListBox) ? _٠ : null);
		Color color = SystemColors.ControlText;
		if ((ؿ.State & 1) != 0)
		{
			color = SystemColors.HighlightText;
			ؿ.Graphics.FillRectangle(SystemBrushes.Highlight, ؿ.Bounds);
		}
		else
		{
			ؿ.Graphics.FillRectangle(SystemBrushes.Window, ؿ.Bounds);
		}
		Color empty = Color.Empty;
		string text = "";
		if ((object)val.Items[ؿ.Index].GetType() == typeof(Color))
		{
			empty = (Color)val.Items[ؿ.Index];
			text = empty.Name;
		}
		else
		{
			text = val.Items[ؿ.Index].ToString();
			empty = (Color)լ.GetType().GetProperty(text).GetValue(լ, null);
		}
		ؿ.Graphics.FillRectangle((Brush)new SolidBrush(empty), rectangle);
		ؿ.Graphics.DrawRectangle(SystemPens.ControlText, rectangle);
		bounds.Offset(30, 0);
		bounds.Width -= 30;
		TextDrawing.DrawString(ؿ.Graphics, text, ((Control)val).Font, color, bounds, eTextFormat.Default);
	}

	private void _29A7(object _٠, MouseEventArgs ؿ)
	{
		for (int i = 0; i < 48; i++)
		{
			if (_2994[i].Contains(ؿ.X, ؿ.Y))
			{
				_2358 = _2993[i];
				_299E = "";
				break;
			}
		}
	}

	private void _29A8(object _٠, EventArgs ؿ)
	{
		ListBox val = (ListBox)((_٠ is ListBox) ? _٠ : null);
		if (val.SelectedItem != null)
		{
			if (val.SelectedItem is Color)
			{
				_2358 = (Color)val.SelectedItem;
				_299E = "";
			}
			else
			{
				_299E = val.SelectedItem.ToString();
				_2358 = (Color)լ.GetType().GetProperty(_2991).GetValue(լ, null);
			}
		}
		else
		{
			_2358 = Color.Empty;
			_299E = "";
		}
	}

	private void _29A9()
	{
		((Control)_299F).BackColor = Ṽ;
		_29A0.Value = Ṽ.A;
		_29AD();
	}

	public void _29AA()
	{
		((ListControl)_299C).SelectedIndex = -1;
		((ListControl)_299D).SelectedIndex = -1;
		((ListControl)_2998).SelectedIndex = -1;
		if (Ṽ.IsSystemColor)
		{
			_2996.SelectedTab = _2961;
			_29AB(_299C, Ṽ.Name);
		}
		else if (Ṽ.IsNamedColor)
		{
			_2996.SelectedTab = _2962;
			_29AB(_299D, Ṽ.Name);
		}
		else if (_299E != "")
		{
			_2996.SelectedTab = _2963;
			_29AB(_2998, _299E);
		}
		else
		{
			_2996.SelectedTab = _2997;
		}
	}

	private void _29AB(ListBox _29AC, string _06EC)
	{
		foreach (object item in _29AC.Items)
		{
			if (item.ToString() == _06EC)
			{
				_29AC.SelectedItem = item;
				break;
			}
		}
	}

	private void _29AD()
	{
		if (!_2358.IsEmpty)
		{
			((Control)ḅ).Text = _2995 + " (" + _2358.A + ")";
		}
		else
		{
			((Control)ḅ).Text = _2995;
		}
	}

	private void _29AE(object _٠, EventArgs ؿ)
	{
		if (!_2358.IsEmpty && _2358.A != _29A0.Value)
		{
			_2358 = Color.FromArgb(_29A0.Value, _2358);
			_299E = "";
		}
		_29AD();
	}

	private void _29AF(object _٠, PaintEventArgs ؿ)
	{
		if (_2358.IsEmpty)
		{
			Rectangle clientRectangle = ((Control)_299F).ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			ؿ.Graphics.DrawLine(SystemPens.ControlText, clientRectangle.X, clientRectangle.Y, clientRectangle.Right, clientRectangle.Bottom);
			ؿ.Graphics.DrawLine(SystemPens.ControlText, clientRectangle.Right, clientRectangle.Y, clientRectangle.X, clientRectangle.Bottom);
		}
	}

	private void _29B0(object _٠, EventArgs ؿ)
	{
		_29B2();
	}

	private void _29B1(object _٠, EventArgs ؿ)
	{
		_29A1 = true;
		_29B2();
	}

	private void _29B2()
	{
		if (_28E4 != null)
		{
			_28E4.CloseDropDown();
		}
		else if (((Control)this).Parent != null)
		{
			((Control)this).Parent.Hide();
		}
		else
		{
			((Control)this).Hide();
		}
	}

	private void _29B3(object _٠, PaintEventArgs ؿ)
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Width--;
		clientRectangle.Height--;
		ؿ.Graphics.DrawRectangle(SystemPens.ControlDarkDark, clientRectangle);
	}

	private void _29B4(object _٠, EventArgs ؿ)
	{
		if (_2996.SelectedIndex == 0 && լ != null && _299E != "")
		{
			((Control)_29A0).Enabled = false;
		}
		else
		{
			((Control)_29A0).Enabled = true;
		}
	}
}

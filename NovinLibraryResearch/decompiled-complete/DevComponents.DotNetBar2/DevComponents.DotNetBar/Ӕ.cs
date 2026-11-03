using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace DevComponents.DotNetBar;

internal class Ӕ : Form
{
	private Label ḅ;

	private Label Ḇ;

	private Button ḇ;

	private Button Ḉ;

	private Timer ḉ;

	private Label Ḋ;

	private Label ḋ;

	private GroupBox Ḍ;

	private Label ḍ;

	private Label Ḏ;

	private LinkLabel ḏ;

	private Label Ḑ;

	private LinkLabel ḑ;

	private LinkLabel Ḓ;

	private IContainer Պ;

	private static DateTime _ḓ = DateTime.MinValue;

	public Ӕ()
	{
		ـ();
		((Control)Ḇ).Text = "(c) " + DateTime.Now.Year + " by DevComponents, All Rights Reserved.";
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	public void Ḕ()
	{
	}

	private void ـ()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Expected O, but got Unknown
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Expected O, but got Unknown
		Պ = new Container();
		ḅ = new Label();
		Ḇ = new Label();
		ḇ = new Button();
		Ḉ = new Button();
		ḉ = new Timer(Պ);
		ḑ = new LinkLabel();
		Ḋ = new Label();
		ḋ = new Label();
		Ḓ = new LinkLabel();
		Ḍ = new GroupBox();
		Ḑ = new Label();
		ḏ = new LinkLabel();
		Ḏ = new Label();
		ḍ = new Label();
		((Control)Ḍ).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)ḅ).AutoSize = true;
		((Control)ḅ).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ḅ).Location = new Point(12, 8);
		((Control)ḅ).Name = "label1";
		((Control)ḅ).Size = new Size(252, 15);
		((Control)ḅ).TabIndex = 0;
		((Control)ḅ).Text = "DevComponents DotNetBar Component";
		((Control)Ḇ).AutoSize = true;
		((Control)Ḇ).Location = new Point(10, 32);
		((Control)Ḇ).Name = "label2";
		((Control)Ḇ).Size = new Size(307, 13);
		((Control)Ḇ).TabIndex = 1;
		((Control)Ḇ).Text = "(c) 2001-2004 by DevComponents.com, All Rights Reserved";
		((Control)ḇ).Enabled = false;
		((Control)ḇ).Location = new Point(10, 242);
		((Control)ḇ).Name = "button1";
		((Control)ḇ).Size = new Size(72, 24);
		((Control)ḇ).TabIndex = 0;
		((Control)ḇ).Text = "OK";
		((Control)ḇ).Click += ḗ;
		((Control)Ḉ).Location = new Point(89, 242);
		((Control)Ḉ).Name = "button2";
		((Control)Ḉ).Size = new Size(72, 24);
		((Control)Ḉ).TabIndex = 3;
		((Control)Ḉ).Text = "Buy Now";
		((Control)Ḉ).Click += Ḗ;
		ḉ.Enabled = true;
		ḉ.Interval = 800;
		ḉ.Tick += ḕ;
		((Control)ḑ).Location = new Point(52, 200);
		((Control)ḑ).Name = "linkEmail";
		((Control)ḑ).Size = new Size(145, 16);
		((Control)ḑ).TabIndex = 4;
		((Label)ḑ).TabStop = true;
		((Control)ḑ).Text = "info@devcomponents.com";
		ḑ.LinkClicked += new LinkLabelLinkClickedEventHandler(ḙ);
		((Control)Ḋ).AutoSize = true;
		((Control)Ḋ).Location = new Point(12, 201);
		((Control)Ḋ).Name = "label3";
		((Control)Ḋ).Size = new Size(40, 13);
		((Control)Ḋ).TabIndex = 5;
		((Control)Ḋ).Text = "E-Mail:";
		((Control)ḋ).AutoSize = true;
		((Control)ḋ).Location = new Point(13, 217);
		((Control)ḋ).Name = "label4";
		((Control)ḋ).Size = new Size(31, 13);
		((Control)ḋ).TabIndex = 7;
		((Control)ḋ).Text = "Web:";
		((Control)Ḓ).Location = new Point(53, 216);
		((Control)Ḓ).Name = "linkHome";
		((Control)Ḓ).Size = new Size(145, 16);
		((Control)Ḓ).TabIndex = 6;
		((Label)Ḓ).TabStop = true;
		((Control)Ḓ).Text = "www.devcomponents.com";
		Ḓ.LinkClicked += new LinkLabelLinkClickedEventHandler(Ḙ);
		((Control)Ḍ).Controls.AddRange((Control[])(object)new Control[4]
		{
			(Control)Ḑ,
			(Control)ḏ,
			(Control)Ḏ,
			(Control)ḍ
		});
		((Control)Ḍ).Location = new Point(14, 55);
		((Control)Ḍ).Name = "groupBox1";
		((Control)Ḍ).Size = new Size(294, 135);
		((Control)Ḍ).TabIndex = 8;
		Ḍ.TabStop = false;
		((Control)Ḑ).Location = new Point(3, 104);
		((Control)Ḑ).Name = "label7";
		((Control)Ḑ).Size = new Size(285, 12);
		((Control)Ḑ).TabIndex = 3;
		((Control)Ḑ).Text = "Order at:";
		Ḑ.TextAlign = (ContentAlignment)32;
		((Control)ḏ).Location = new Point(6, 116);
		((Control)ḏ).Name = "linkOrder";
		((Control)ḏ).Size = new Size(280, 15);
		((Control)ḏ).TabIndex = 2;
		((Label)ḏ).TabStop = true;
		((Control)ḏ).Text = "http://www.devcomponents.com/dotnetbar/order.html";
		((Label)ḏ).TextAlign = (ContentAlignment)32;
		ḏ.LinkClicked += new LinkLabelLinkClickedEventHandler(Ḛ);
		((Control)Ḏ).Location = new Point(9, 72);
		((Control)Ḏ).Name = "label6";
		((Control)Ḏ).Size = new Size(276, 31);
		((Control)Ḏ).TabIndex = 1;
		((Control)Ḏ).Text = "For pricing and licensing information please visit our web site. ";
		((Control)ḍ).ForeColor = Color.Maroon;
		((Control)ḍ).Location = new Point(8, 20);
		((Control)ḍ).Name = "label5";
		((Control)ḍ).Size = new Size(276, 41);
		((Control)ḍ).TabIndex = 0;
		((Control)ḍ).Text = "This component is not registered and it is provided for evaluation purposes only. This message will not appear after you register the component.";
		((Control)this).BackColor = Color.WhiteSmoke;
		((Form)this).AcceptButton = (IButtonControl)(object)ḇ;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(321, 273);
		((Form)this).ControlBox = false;
		((Control)this).Controls.AddRange((Control[])(object)new Control[9]
		{
			(Control)Ḍ,
			(Control)ḋ,
			(Control)Ḋ,
			(Control)Ḇ,
			(Control)ḅ,
			(Control)Ḓ,
			(Control)ḑ,
			(Control)Ḉ,
			(Control)ḇ
		});
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "RemindForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Component not registered";
		((Control)Ḍ).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private void ḕ(object _٠, EventArgs ؿ)
	{
		ḉ.Enabled = false;
		((Control)ḇ).Enabled = true;
		((Control)ḇ).Focus();
	}

	private void Ḗ(object _٠, EventArgs ؿ)
	{
		Process.Start(((Control)ḏ).Text);
	}

	private void ḗ(object _٠, EventArgs ؿ)
	{
		((Form)this).Close();
	}

	private void Ḙ(object _٠, LinkLabelLinkClickedEventArgs ؿ)
	{
		Process.Start("http://www.devcomponents.com");
	}

	private void ḙ(object _٠, LinkLabelLinkClickedEventArgs ؿ)
	{
		Process.Start("mailto:support@devcomponents.com");
	}

	private void Ḛ(object _٠, LinkLabelLinkClickedEventArgs ؿ)
	{
		Process.Start(((Control)ḏ).Text);
	}
}
[ToolboxItem(false)]
internal class ӕ : Form
{
	public bool _298A;

	public TextBox _298B;

	private Label ḅ;

	private Button _2181;

	private Button _2182;

	private Container Պ;

	public ӕ()
	{
		ـ();
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
		_2181 = new Button();
		_2182 = new Button();
		ḅ = new Label();
		_298B = new TextBox();
		((Control)this).SuspendLayout();
		_2181.DialogResult = (DialogResult)1;
		((ButtonBase)_2181).FlatStyle = (FlatStyle)3;
		((Control)_2181).Location = new Point(113, 56);
		((Control)_2181).Name = "cmdOK";
		((Control)_2181).Size = new Size(80, 24);
		((Control)_2181).TabIndex = 2;
		((Control)_2181).Text = "barname_ok";
		((Control)_2182).CausesValidation = false;
		_2182.DialogResult = (DialogResult)2;
		((ButtonBase)_2182).FlatStyle = (FlatStyle)3;
		((Control)_2182).Location = new Point(197, 56);
		((Control)_2182).Name = "cmdCancel";
		((Control)_2182).Size = new Size(80, 24);
		((Control)_2182).TabIndex = 3;
		((Control)_2182).Text = "barname_cancel";
		((Control)ḅ).Location = new Point(8, 8);
		((Control)ḅ).Name = "label1";
		((Control)ḅ).Size = new Size(272, 16);
		((Control)ḅ).TabIndex = 0;
		((Control)ḅ).Text = "barname_name";
		((Control)_298B).Location = new Point(8, 28);
		((Control)_298B).Name = "txtName";
		((Control)_298B).Size = new Size(268, 20);
		((Control)_298B).TabIndex = 1;
		((Control)_298B).Text = "";
		((Control)_298B).Validating += _298D;
		((Form)this).AcceptButton = (IButtonControl)(object)_2181;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).CancelButton = (IButtonControl)(object)_2182;
		((Form)this).ClientSize = new Size(286, 87);
		((Control)this).Controls.AddRange((Control[])(object)new Control[4]
		{
			(Control)_2182,
			(Control)_2181,
			(Control)ḅ,
			(Control)_298B
		});
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "ToolbarName";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "barname_caption";
		((Form)this).Load += _298C;
		((Control)this).ResumeLayout(false);
	}

	private void _298C(object _٠, EventArgs ؿ)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		DotNetBarManager manager = null;
		if (((Form)this).Owner is frmCustomize)
		{
			manager = ((frmCustomize)(object)((Form)this).Owner)._290C();
		}
		using (LocalizationManager localizationManager = new LocalizationManager(manager))
		{
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (!(val is TextBox))
				{
					val.Text = localizationManager.GetLocalizedString(val.Text);
				}
			}
			if (_298A)
			{
				((Control)this).Text = localizationManager.GetLocalizedString(LocalizationKeys.RenameBarDialogCaption);
			}
			else
			{
				((Control)this).Text = localizationManager.GetLocalizedString(((Control)this).Text);
			}
		}
		manager = null;
	}

	private void _298D(object _٠, CancelEventArgs ؿ)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)_298B).Text.Trim() == "")
		{
			DotNetBarManager manager = null;
			if (((Form)this).Owner is frmCustomize)
			{
				manager = ((frmCustomize)(object)((Form)this).Owner)._290C();
			}
			using (LocalizationManager localizationManager = new LocalizationManager(manager))
			{
				MessageBox.Show(localizationManager.GetLocalizedString(LocalizationKeys.BarEditDialogInvalidNameMessage));
			}
			manager = null;
			ؿ.Cancel = true;
		}
	}
}

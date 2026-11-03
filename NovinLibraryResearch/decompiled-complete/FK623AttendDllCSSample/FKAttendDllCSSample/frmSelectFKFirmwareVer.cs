using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace FKAttendDllCSSample;

public class frmSelectFKFirmwareVer : Form
{
	private IContainer components = null;

	internal Button btnSelect;

	internal Button btnCancel;

	internal ComboBox cboFKFirmware;

	internal Label Label1;

	public string FKFirmware;

	public string AssemblyTitle
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), inherit: false);
			if (customAttributes.Length != 0)
			{
				AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)customAttributes[0];
				if (assemblyTitleAttribute.Title != "")
				{
					return assemblyTitleAttribute.Title;
				}
			}
			return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
		}
	}

	public string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

	public string AssemblyDescription
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return "";
			}
			return ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
		}
	}

	public string AssemblyProduct
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return "";
			}
			return ((AssemblyProductAttribute)customAttributes[0]).Product;
		}
	}

	public string AssemblyCopyright
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return "";
			}
			return ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
		}
	}

	public string AssemblyCompany
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return "";
			}
			return ((AssemblyCompanyAttribute)customAttributes[0]).Company;
		}
	}

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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		btnSelect = new Button();
		btnCancel = new Button();
		cboFKFirmware = new ComboBox();
		Label1 = new Label();
		((Control)this).SuspendLayout();
		((Control)btnSelect).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)btnSelect).Location = new Point(212, 64);
		((Control)btnSelect).Name = "btnSelect";
		((Control)btnSelect).Size = new Size(97, 30);
		((Control)btnSelect).TabIndex = 5;
		((Control)btnSelect).Text = "Select";
		((ButtonBase)btnSelect).UseVisualStyleBackColor = true;
		((Control)btnSelect).Click += btnSelect_Click;
		((Control)btnCancel).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)btnCancel).Location = new Point(83, 64);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).Size = new Size(97, 30);
		((Control)btnCancel).TabIndex = 6;
		((Control)btnCancel).Text = "Cancel";
		((ButtonBase)btnCancel).UseVisualStyleBackColor = true;
		((Control)btnCancel).Click += btnCancel_Click;
		((Control)cboFKFirmware).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((ListControl)cboFKFirmware).FormattingEnabled = true;
		((Control)cboFKFirmware).Location = new Point(122, 17);
		((Control)cboFKFirmware).Name = "cboFKFirmware";
		((Control)cboFKFirmware).Size = new Size(187, 27);
		((Control)cboFKFirmware).TabIndex = 4;
		((Control)Label1).AutoSize = true;
		((Control)Label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label1).Location = new Point(12, 20);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(94, 19);
		((Control)Label1).TabIndex = 3;
		((Control)Label1).Text = "FK firmware :";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(324, 110);
		((Control)this).Controls.Add((Control)(object)btnSelect);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)cboFKFirmware);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmSelectFKFirmwareVer";
		((Control)this).Padding = new Padding(9);
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Select FK firmware version";
		((Form)this).Load += frmSelectFKFirmwareVer_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	public frmSelectFKFirmwareVer()
	{
		InitializeComponent();
	}

	private void frmSelectFKFirmwareVer_Load(object sender, EventArgs e)
	{
		FKFirmware = "";
		cboFKFirmware.Items.Add((object)"HS001");
		cboFKFirmware.Items.Add((object)"HS101");
		cboFKFirmware.Items.Add((object)"HS102");
		cboFKFirmware.Items.Add((object)"FK754HS2");
		cboFKFirmware.Items.Add((object)"FK754HS2");
		cboFKFirmware.Items.Add((object)"FK735HL2");
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		FKFirmware = "";
		((Component)this).Dispose();
	}

	private void btnSelect_Click(object sender, EventArgs e)
	{
		FKFirmware = ((Control)cboFKFirmware).Text;
		((Component)this).Dispose();
	}
}

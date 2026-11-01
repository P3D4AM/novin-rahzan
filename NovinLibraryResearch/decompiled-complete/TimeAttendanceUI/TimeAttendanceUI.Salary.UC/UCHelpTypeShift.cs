using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PacketParser;
using PacketParser.Entities.Attendance;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.UC;

public class UCHelpTypeShift : UserControl
{
	public delegate void HozorEventHandler(object sender, EventArgs e);

	private TypeShiftEnumType _Model = (TypeShiftEnumType)100;

	private IContainer components;

	private PictureBox pictureBox1;

	private PanelEx panelEx1;

	private ButtonX btnGetShiftDesc;

	public TypeShiftEnumType Model
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _Model;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			_Model = value;
			((Control)btnGetShiftDesc).Text = Utilities.GetDisplay<TypeShiftEnumType>(_Model);
		}
	}

	public event HozorEventHandler ChangeValue;

	public UCHelpTypeShift()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
	}

	public UCHelpTypeShift(TypeShiftEnumType pModel)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_Model = pModel;
	}

	private void BtnClick(object sender, EventArgs e)
	{
		if (this.ChangeValue != null)
		{
			this.ChangeValue(sender, e);
		}
	}

	private void btnGetShiftDesc_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected I4, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		string text = "";
		TypeShiftEnumType model = Model;
		switch ((int)model)
		{
		default:
			if ((int)model != 100)
			{
				break;
			}
			goto case 3;
		case 3:
		case 4:
		case 6:
			text = "HelpHozorShifts.chm";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			break;
		case 0:
		case 1:
		case 2:
		case 5:
			break;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		panelEx1 = new PanelEx();
		btnGetShiftDesc = new ButtonX();
		pictureBox1 = new PictureBox();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)btnGetShiftDesc);
		((Control)panelEx1).Controls.Add((Control)(object)pictureBox1);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(149, 177);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 1;
		((Control)btnGetShiftDesc).AccessibleRole = (AccessibleRole)43;
		((Control)btnGetShiftDesc).Anchor = (AnchorStyles)14;
		btnGetShiftDesc.ColorTable = (eButtonColor)3;
		((Control)btnGetShiftDesc).Location = new Point(0, 138);
		((Control)btnGetShiftDesc).Margin = new Padding(4, 5, 4, 5);
		((Control)btnGetShiftDesc).Name = "btnGetShiftDesc";
		((Control)btnGetShiftDesc).Size = new Size(149, 39);
		((PopupItemControl)btnGetShiftDesc).Style = (eDotNetBarStyle)9;
		((Control)btnGetShiftDesc).TabIndex = 2;
		((Control)btnGetShiftDesc).Text = "کمک شیفت 24-48";
		((Control)btnGetShiftDesc).Click += btnGetShiftDesc_Click;
		((Control)pictureBox1).Anchor = (AnchorStyles)15;
		pictureBox1.Image = (Image)(object)Resources.typeshift2;
		((Control)pictureBox1).Location = new Point(0, 0);
		((Control)pictureBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(146, 128);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(9f, 23f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("B Homa", 9.75f);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UCHelpTypeShift";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(149, 177);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

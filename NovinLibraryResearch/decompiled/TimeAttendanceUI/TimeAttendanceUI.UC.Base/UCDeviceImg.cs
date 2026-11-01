using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using NPResourceManagement;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.UC.Base;

public class UCDeviceImg : UserControl
{
	public delegate void HozorEventHandler(object sender, EventArgs e);

	private DeviceModelEnumType _Model = (DeviceModelEnumType)9201;

	private IContainer components;

	private PanelEx panelEx9;

	private PictureBox PicBxHozor;

	public string DeviceName { get; set; }

	public DeviceModelEnumType Model
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
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Invalid comparison between Unknown and I4
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected I4, but got Unknown
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Invalid comparison between Unknown and I4
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected I4, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Invalid comparison between Unknown and I4
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Invalid comparison between Unknown and I4
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected I4, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Invalid comparison between Unknown and I4
			_Model = value;
			DeviceModelEnumType model = _Model;
			if ((int)model <= 9102)
			{
				switch (model - 9001)
				{
				default:
					if ((int)model != 9101)
					{
						if ((int)model == 9102)
						{
							PicBxHozor.Image = (Image)(object)Resources.N1;
						}
					}
					else
					{
						PicBxHozor.Image = (Image)(object)Resources.ETC132;
					}
					break;
				case 0:
					PicBxHozor.Image = (Image)(object)Resources.EC160;
					break;
				case 1:
					PicBxHozor.Image = (Image)(object)Resources.ETC132;
					break;
				case 2:
					PicBxHozor.Image = (Image)(object)Resources.N1;
					break;
				}
			}
			else if ((int)model <= 9310)
			{
				switch (model - 9201)
				{
				default:
					switch (model - 9301)
					{
					case 0:
						PicBxHozor.Image = (Image)(object)Resources.NP395;
						break;
					case 1:
					case 3:
						PicBxHozor.Image = (Image)(object)Resources.NP587;
						break;
					case 4:
						PicBxHozor.Image = (Image)(object)Resources.NP255;
						break;
					case 5:
						PicBxHozor.Image = (Image)(object)ImageResourceManagement.NP290;
						break;
					case 6:
						PicBxHozor.Image = (Image)(object)ImageResourceManagement.NP770;
						break;
					case 7:
						PicBxHozor.Image = (Image)(object)Resources.NP756_;
						break;
					case 9:
						PicBxHozor.Image = (Image)(object)Resources.NP245;
						break;
					case 8:
						PicBxHozor.Image = (Image)(object)Resources.NP761p;
						break;
					}
					break;
				case 0:
					PicBxHozor.Image = (Image)(object)Resources.NP2815;
					break;
				case 1:
					PicBxHozor.Image = (Image)(object)ImageResourceManagement.Device302;
					break;
				case 2:
					PicBxHozor.Image = (Image)(object)Resources.NP261;
					break;
				case 3:
					PicBxHozor.Image = (Image)(object)Resources.NP550;
					break;
				}
			}
			else if ((int)model != 9400)
			{
				if ((int)model == 9500)
				{
					PicBxHozor.Image = (Image)(object)Resources.NP620;
				}
			}
			else
			{
				PicBxHozor.Image = (Image)(object)Resources.ZKTeco;
			}
			((Control)GetRDBTN).Text = DeviceName;
		}
	}

	public RadioButton GetRDBTN { get; private set; }

	public event HozorEventHandler ChangeValue;

	public UCDeviceImg()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
	}

	private void BtnClick(object sender, EventArgs e)
	{
		if (this.ChangeValue != null)
		{
			this.ChangeValue(sender, e);
		}
	}

	private void RdBtnHozor_CheckedChanged(object sender, EventArgs e)
	{
		if (this.ChangeValue != null)
		{
			this.ChangeValue(sender, e);
		}
	}

	public void SetCheckRDBTN(bool sw)
	{
		try
		{
			GetRDBTN.CheckedChanged -= RdBtnHozor_CheckedChanged;
			GetRDBTN.Checked = sw;
			GetRDBTN.CheckedChanged += RdBtnHozor_CheckedChanged;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetCheckRDBTN", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDeviceImg.cs", 123);
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
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		panelEx9 = new PanelEx();
		PicBxHozor = new PictureBox();
		GetRDBTN = new RadioButton();
		((Control)panelEx9).SuspendLayout();
		((ISupportInitialize)PicBxHozor).BeginInit();
		((Control)this).SuspendLayout();
		panelEx9.CanvasColor = SystemColors.Control;
		panelEx9.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx9).Controls.Add((Control)(object)PicBxHozor);
		((Control)panelEx9).Controls.Add((Control)(object)GetRDBTN);
		panelEx9.DisabledBackColor = Color.Empty;
		((Control)panelEx9).Location = new Point(3, 3);
		((Control)panelEx9).Name = "panelEx9";
		((Control)panelEx9).Size = new Size(83, 75);
		panelEx9.Style.Alignment = (StringAlignment)1;
		panelEx9.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx9.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx9.Style.Border = (eBorderType)1;
		panelEx9.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx9.Style.CornerType = (eCornerType)2;
		panelEx9.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx9.Style.GradientAngle = 90;
		((Control)panelEx9).TabIndex = 4;
		PicBxHozor.Image = (Image)(object)Resources.EN261;
		((Control)PicBxHozor).Location = new Point(15, 5);
		((Control)PicBxHozor).Name = "PicBxHozor";
		((Control)PicBxHozor).Size = new Size(49, 46);
		PicBxHozor.SizeMode = (PictureBoxSizeMode)4;
		PicBxHozor.TabIndex = 1;
		PicBxHozor.TabStop = false;
		((Control)GetRDBTN).Font = new Font("Tahoma", 8f);
		((Control)GetRDBTN).Location = new Point(5, 55);
		((Control)GetRDBTN).Margin = new Padding(0);
		((Control)GetRDBTN).Name = "RdBtnHozor";
		((Control)GetRDBTN).Size = new Size(62, 17);
		((Control)GetRDBTN).TabIndex = 0;
		GetRDBTN.TabStop = true;
		((Control)GetRDBTN).Text = "NP2815";
		((ButtonBase)GetRDBTN).TextAlign = (ContentAlignment)32;
		((ButtonBase)GetRDBTN).UseVisualStyleBackColor = true;
		GetRDBTN.CheckedChanged += RdBtnHozor_CheckedChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelEx9);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this).Name = "UCDeviceImg";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(89, 82);
		((Control)panelEx9).ResumeLayout(false);
		((ISupportInitialize)PicBxHozor).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

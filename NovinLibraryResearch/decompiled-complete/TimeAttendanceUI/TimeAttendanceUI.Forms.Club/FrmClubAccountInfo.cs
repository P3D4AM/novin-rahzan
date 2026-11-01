using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using TimeAttendanceDAL.Dal.FLRClub;

namespace TimeAttendanceUI.Forms.Club;

public class FrmClubAccountInfo : FormExtention, IClubForms
{
	private readonly TimeAttendanceDAL.Dal.FLRClub.Club _club;

	private IClubForms _nextPage;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	private Label label1;

	public FrmClubAccountInfo(string contextGuid, TimeAttendanceDAL.Dal.FLRClub.Club club)
		: base(contextGuid, true, true)
	{
		InitializeComponent();
		_club = club;
	}

	protected override void OnLoad(EventArgs e)
	{
		((FormExtention)this).OnLoad(e);
		((Control)base.statusStrip1).Visible = false;
	}

	public void FillFrmFields()
	{
		throw new NotImplementedException();
	}

	public IClubForms NextPage()
	{
		return null;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FormExtention)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		label1 = new Label();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel4);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(582, 500);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)label1);
		groupPanel4.DisabledBackColor = Color.Empty;
		((Control)groupPanel4).Dock = (DockStyle)5;
		((Control)groupPanel4).Location = new Point(0, 0);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(582, 500);
		((PanelControl)groupPanel4).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel4).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel4).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel4).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel4).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel4).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel4).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel4).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel4).Style.CornerDiameter = 4;
		((PanelControl)groupPanel4).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel4).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel4).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel4).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel4).TabIndex = 5;
		((Control)groupPanel4).Text = "فرم مالی";
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(401, 60);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(160, 23);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "ثبت اطلاعات مالی در این قسمت";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmClubAccountInfo";
		((Control)this).Text = "FrmCheckoutSanavat";
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((Control)groupPanel4).PerformLayout();
		((Control)this).ResumeLayout(false);
	}

	bool IClubForms.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void IClubForms.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}

	void IClubForms.Show()
	{
		((Control)this).Show();
	}
}

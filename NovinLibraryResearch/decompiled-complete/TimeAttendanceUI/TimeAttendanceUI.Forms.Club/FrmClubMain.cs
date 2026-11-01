using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using BehComponents;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRClub;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Club;

public class FrmClubMain : BaseForm, IClubForms
{
	private Font _font;

	private readonly Stack<IClubForms> _pages = new Stack<IClubForms>();

	private TimeAttendanceDAL.Dal.FLRClub.Club _club;

	private IContainer components;

	public PanelEx PanelEx1;

	private Panel pnlLeft;

	private Panel pnlBottom;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel pnlContent;

	private ButtonX btnBack;

	private ButtonX btnCancel;

	private ButtonX btnNext;

	private ButtonX BtnAccept;

	private PictureBox pictureBox1;

	public FrmClubMain(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			_font = font;
			InitializeComponent();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubMain.cs", 29);
		}
	}

	private async Task LoadTopFormAsync(IClubForms frm)
	{
		try
		{
			frm.TopLevel = false;
			frm.AutoScroll = true;
			((Control)pnlContent).Controls.Clear();
			CacheBaseForm val = (CacheBaseForm)frm;
			((Control)val).Dock = (DockStyle)5;
			((Control)pnlContent).Controls.Add((Control)(object)val);
			await val.ShowAsync((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadTopFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubMain.cs", 49);
		}
	}

	private async Task BackAsync()
	{
		try
		{
			((Control)btnNext).Enabled = true;
			((Control)BtnAccept).Enabled = false;
			_pages.Pop();
			await LoadTopFormAsync(_pages.Peek());
			if (_pages.Count <= 1)
			{
				((Control)btnBack).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BackAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubMain.cs", 66);
		}
	}

	private async void Next()
	{
		try
		{
			IClubForms nextPage = _pages.Peek().NextPage();
			if (nextPage != null)
			{
				((Control)btnBack).Enabled = true;
				_pages.Push(nextPage);
				await LoadTopFormAsync(nextPage);
				if (nextPage.NextPage() == null)
				{
					((Control)btnNext).Enabled = false;
					((Control)BtnAccept).Enabled = true;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Next", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubMain.cs", 86);
		}
	}

	public void FillFrmFields()
	{
		throw new NotImplementedException();
	}

	public IClubForms NextPage()
	{
		return null;
	}

	private void BtnNext_Click(object sender, EventArgs e)
	{
		Next();
	}

	private async void BtnBack_Click(object sender, EventArgs e)
	{
		await BackAsync();
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			MessageBoxFarsi.Show("مثلا اطلاعات ثبت شد");
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubMain.cs", 122);
		}
	}

	private async void FrmClubMain_Load(object sender, EventArgs e)
	{
		try
		{
			_club = new TimeAttendanceDAL.Dal.FLRClub.Club(((CacheBaseForm)this).ContextGuid);
			IClubForms clubForms = new FrmClubPersonInfo(((CacheBaseForm)this).ContextGuid, _club, _font);
			_pages.Push(clubForms);
			((Control)btnBack).Enabled = false;
			await LoadTopFormAsync(clubForms);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmClubMain_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubMain.cs", 137);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((BaseForm)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Expected O, but got Unknown
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Expected O, but got Unknown
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Expected O, but got Unknown
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Expected O, but got Unknown
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmClubMain));
		PanelEx1 = new PanelEx();
		pnlContent = new Panel();
		pnlLeft = new Panel();
		pictureBox1 = new PictureBox();
		pnlBottom = new Panel();
		tableLayoutPanel1 = new TableLayoutPanel();
		btnCancel = new ButtonX();
		btnBack = new ButtonX();
		btnNext = new ButtonX();
		BtnAccept = new ButtonX();
		((Control)PanelEx1).SuspendLayout();
		((Control)pnlLeft).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)pnlBottom).SuspendLayout();
		((Control)tableLayoutPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)pnlContent);
		((Control)PanelEx1).Controls.Add((Control)(object)pnlLeft);
		((Control)PanelEx1).Controls.Add((Control)(object)pnlBottom);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(2);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(667, 562);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 1;
		((Control)pnlContent).BackColor = SystemColors.GradientInactiveCaption;
		((Control)pnlContent).Dock = (DockStyle)5;
		((Control)pnlContent).Location = new Point(0, 0);
		((Control)pnlContent).Margin = new Padding(0);
		((Control)pnlContent).Name = "pnlContent";
		((Control)pnlContent).Size = new Size(500, 504);
		((Control)pnlContent).TabIndex = 10;
		((Control)pnlLeft).BackColor = Color.FromArgb(255, 153, 0);
		((Control)pnlLeft).Controls.Add((Control)(object)pictureBox1);
		((Control)pnlLeft).Dock = (DockStyle)4;
		((Control)pnlLeft).Location = new Point(500, 0);
		((Control)pnlLeft).Margin = new Padding(2);
		((Control)pnlLeft).Name = "pnlLeft";
		((Control)pnlLeft).Size = new Size(167, 504);
		((Control)pnlLeft).TabIndex = 9;
		((Control)pictureBox1).Dock = (DockStyle)5;
		pictureBox1.Image = (Image)componentResourceManager.GetObject("pictureBox1.Image");
		((Control)pictureBox1).Location = new Point(0, 0);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(167, 504);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		((Control)pnlBottom).BackColor = SystemColors.Control;
		((Control)pnlBottom).Controls.Add((Control)(object)tableLayoutPanel1);
		((Control)pnlBottom).Dock = (DockStyle)2;
		((Control)pnlBottom).Location = new Point(0, 504);
		((Control)pnlBottom).Margin = new Padding(2);
		((Control)pnlBottom).Name = "pnlBottom";
		((Control)pnlBottom).Padding = new Padding(7, 6, 7, 6);
		((Control)pnlBottom).Size = new Size(667, 58);
		((Control)pnlBottom).TabIndex = 8;
		tableLayoutPanel1.ColumnCount = 4;
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
		tableLayoutPanel1.Controls.Add((Control)(object)btnCancel, 3, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)btnBack, 2, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)btnNext, 1, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)BtnAccept, 0, 0);
		((Control)tableLayoutPanel1).Dock = (DockStyle)3;
		((Control)tableLayoutPanel1).Location = new Point(7, 6);
		((Control)tableLayoutPanel1).Margin = new Padding(2);
		((Control)tableLayoutPanel1).Name = "tableLayoutPanel1";
		((Control)tableLayoutPanel1).Padding = new Padding(2);
		tableLayoutPanel1.RowCount = 1;
		tableLayoutPanel1.RowStyles.Add(new RowStyle((SizeType)2, 100f));
		((Control)tableLayoutPanel1).Size = new Size(440, 46);
		((Control)tableLayoutPanel1).TabIndex = 0;
		((Control)btnCancel).AccessibleRole = (AccessibleRole)43;
		btnCancel.ColorTable = (eButtonColor)6;
		((Control)btnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnCancel.Image = (Image)(object)Resources.stop;
		btnCancel.ImagePosition = (eImagePosition)1;
		((Control)btnCancel).Location = new Point(329, 4);
		((Control)btnCancel).Margin = new Padding(2);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(105, 38);
		((PopupItemControl)btnCancel).Style = (eDotNetBarStyle)9;
		((Control)btnCancel).TabIndex = 4;
		((Control)btnCancel).TabStop = false;
		((Control)btnCancel).Text = "[لغو] (Esc)";
		((Control)btnCancel).Click += BtnCancel_Click;
		((Control)btnBack).AccessibleRole = (AccessibleRole)43;
		btnBack.ColorTable = (eButtonColor)6;
		((Control)btnBack).Enabled = false;
		((Control)btnBack).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnBack.Image = (Image)(object)Resources.next2;
		btnBack.ImagePosition = (eImagePosition)1;
		((Control)btnBack).Location = new Point(221, 4);
		((Control)btnBack).Margin = new Padding(2);
		((Control)btnBack).Name = "btnBack";
		((Control)btnBack).RightToLeft = (RightToLeft)1;
		((Control)btnBack).Size = new Size(104, 38);
		((PopupItemControl)btnBack).Style = (eDotNetBarStyle)9;
		((Control)btnBack).TabIndex = 4;
		((Control)btnBack).TabStop = false;
		((Control)btnBack).Text = "قبلی";
		((Control)btnBack).Click += BtnBack_Click;
		((Control)btnNext).AccessibleRole = (AccessibleRole)43;
		btnNext.ColorTable = (eButtonColor)6;
		((Control)btnNext).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnNext.Image = (Image)(object)Resources.back2;
		btnNext.ImagePosition = (eImagePosition)1;
		((Control)btnNext).Location = new Point(112, 4);
		((Control)btnNext).Margin = new Padding(2);
		((Control)btnNext).Name = "btnNext";
		((Control)btnNext).RightToLeft = (RightToLeft)1;
		((Control)btnNext).Size = new Size(105, 38);
		((PopupItemControl)btnNext).Style = (eDotNetBarStyle)9;
		((Control)btnNext).TabIndex = 4;
		((Control)btnNext).TabStop = false;
		((Control)btnNext).Text = "بعدی";
		((Control)btnNext).Click += BtnNext_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Enabled = false;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(4, 4);
		((Control)BtnAccept).Margin = new Padding(2);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(104, 38);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 3;
		((Control)BtnAccept).Text = "[ذخیره] (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(667, 562);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmClubMain";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "فرم ثبت نام اعضاء";
		((Form)this).Load += FrmClubMain_Load;
		((Control)PanelEx1).ResumeLayout(false);
		((Control)pnlLeft).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)pnlBottom).ResumeLayout(false);
		((Control)tableLayoutPanel1).ResumeLayout(false);
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

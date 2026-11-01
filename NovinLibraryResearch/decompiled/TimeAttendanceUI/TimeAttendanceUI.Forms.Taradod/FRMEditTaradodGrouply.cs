using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Attendance.Access;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Taradod;

public class FRMEditTaradodGrouply : CacheBaseForm, IHasContext
{
	public List<long> SelectedCodeTafsil = new List<long>();

	private IContainer components;

	private PanelEx panelEx1;

	private DataGridViewX DGrid;

	private BindingSource editGrouplyBindingSource;

	private LabelX labelX5;

	private MaskedTextBox TbxNewTime;

	private LabelX labelX1;

	private DataGridViewCheckBoxColumn DgChecked;

	private DataGridViewTextBoxColumn DgCodeTafsil;

	private DataGridViewTextBoxColumn DgDateSh;

	private DataGridViewTextBoxColumn DgFullName;

	private DataGridViewTextBoxColumn DgTime;

	private ButtonX BtnCancel;

	internal ButtonX BtnApply;

	private LabelX LblDate;

	public string NewTime { get; private set; }

	public FRMEditTaradodGrouply(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
	}

	public FRMEditTaradodGrouply(string contextGuid, List<EditGrouply> data, string dateSh)
		: base(contextGuid)
	{
		InitializeComponent();
		try
		{
			editGrouplyBindingSource.DataSource = data;
			((Control)LblDate).Text = dateSh;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMEditTaradodGrouply.cs", 31);
		}
	}

	private void BtnApply_Click(object sender, EventArgs e)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		try
		{
			if ((int)AccessLevelClass.Instance.LevelHozor != 5)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, "جهت استفاده از این قابلیت، بایستی نرم افزار خود را به سطح 5 ارتقاء دهید", "", 3);
				return;
			}
			SelectedCodeTafsil = new List<long>();
			NewTime = ((Control)TbxNewTime).Text;
			if (!TbxNewTime.MaskCompleted)
			{
				Utility.MessageBoxShowError("ساعت تردد را کامل وارد نمائید", ((CacheBaseForm)this).ContextGuid);
				return;
			}
			short.TryParse(((Control)TbxNewTime).Text.Substring(0, 2), out var result);
			short.TryParse(((Control)TbxNewTime).Text.Substring(3, 2), out var result2);
			if (result > 24 || result2 > 59 || (result == 24 && result2 > 0))
			{
				Utility.MessageBoxShowError("ساعت تردد داده شده نامعتبر است", ((CacheBaseForm)this).ContextGuid);
			}
			else
			{
				if (((DataGridView)DGrid).Rows.Count <= 0)
				{
					return;
				}
				foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGrid).Rows)
				{
					DataGridViewRow val = item;
					DataGridViewCell obj = val.Cells[((DataGridViewBand)DgChecked).Index];
					DataGridViewCheckBoxCell val2 = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
					if (val2 != null && ((DataGridViewCell)val2).Value != null && (bool)((DataGridViewCell)val2).Value)
					{
						SelectedCodeTafsil.Add(Convert.ToInt64(val.Cells[((DataGridViewBand)DgCodeTafsil).Index].Value.ToString()));
					}
				}
				if (SelectedCodeTafsil.Count <= 0)
				{
					Utility.MessageBoxShowError("اشخاص مورد نظر را انتخاب نمائید", ((CacheBaseForm)this).ContextGuid);
					return;
				}
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnApply_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMEditTaradodGrouply.cs", 76);
		}
	}

	private async void FRMEditTaradodGrouply_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode != 32)
				{
					if ((int)keyCode == 123)
					{
						e.Handled = true;
						FrmCalc frm = new FrmCalc(((CacheBaseForm)this).ContextGuid);
						await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
						((Component)(object)frm).Dispose();
					}
					return;
				}
				e.Handled = true;
				if (((Control)DGrid).Focused && ((DataGridView)DGrid).CurrentRow != null && ((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)DgChecked).Index)
				{
					DataGridViewCell obj = ((DataGridView)DGrid).CurrentRow.Cells[((DataGridViewBand)DgChecked).Index];
					DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
					if (val != null)
					{
						((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
					}
				}
			}
			else
			{
				e.Handled = true;
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMEditTaradodGrouply_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMEditTaradodGrouply.cs", 107);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)DgChecked).Index)
			{
				DataGridViewCell currentCell = ((DataGridView)DGrid).CurrentCell;
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((currentCell is DataGridViewCheckBoxCell) ? currentCell : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMEditTaradodGrouply.cs", 121);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMEditTaradodGrouply.cs", 134);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((CacheBaseForm)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
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
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Expected O, but got Unknown
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Expected O, but got Unknown
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Expected O, but got Unknown
		//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a11: Expected O, but got Unknown
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af6: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		BtnApply = new ButtonX();
		BtnCancel = new ButtonX();
		labelX1 = new LabelX();
		TbxNewTime = new MaskedTextBox();
		labelX5 = new LabelX();
		DGrid = new DataGridViewX();
		DgChecked = new DataGridViewCheckBoxColumn();
		DgCodeTafsil = new DataGridViewTextBoxColumn();
		DgDateSh = new DataGridViewTextBoxColumn();
		DgFullName = new DataGridViewTextBoxColumn();
		DgTime = new DataGridViewTextBoxColumn();
		editGrouplyBindingSource = new BindingSource(components);
		LblDate = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)editGrouplyBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)LblDate);
		((Control)panelEx1).Controls.Add((Control)(object)BtnApply);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)TbxNewTime);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(634, 562);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.LightBlue;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnApply).AccessibleRole = (AccessibleRole)43;
		((Control)BtnApply).Anchor = (AnchorStyles)13;
		BtnApply.ColorTable = (eButtonColor)6;
		((Control)BtnApply).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnApply).ForeColor = Color.Black;
		BtnApply.Image = (Image)(object)Resources.accept1;
		BtnApply.ImagePosition = (eImagePosition)1;
		BtnApply.ImageTextSpacing = 5;
		((Control)BtnApply).Location = new Point(144, 522);
		((Control)BtnApply).Name = "BtnApply";
		((Control)BtnApply).RightToLeft = (RightToLeft)1;
		((Control)BtnApply).Size = new Size(173, 34);
		((PopupItemControl)BtnApply).Style = (eDotNetBarStyle)9;
		((Control)BtnApply).TabIndex = 2;
		((Control)BtnApply).Text = "اعمال";
		((Control)BtnApply).Click += BtnApply_Click;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 522);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 3;
		((Control)BtnCancel).Text = "لغو   (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)labelX1).Anchor = (AnchorStyles)10;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(559, 504);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(58, 23);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "تاریخ روز:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)TbxNewTime).Anchor = (AnchorStyles)10;
		((Control)TbxNewTime).Font = new Font("Tahoma", 10f, (FontStyle)1);
		((Control)TbxNewTime).Location = new Point(323, 527);
		TbxNewTime.Mask = "00:00";
		((Control)TbxNewTime).Name = "TbxNewTime";
		((Control)TbxNewTime).Size = new Size(62, 24);
		((Control)TbxNewTime).TabIndex = 1;
		((Control)TbxNewTime).Text = "0000";
		TbxNewTime.TextAlign = (HorizontalAlignment)2;
		TbxNewTime.ValidatingType = typeof(DateTime);
		((Control)labelX5).Anchor = (AnchorStyles)10;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(395, 528);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(227, 23);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "اصلاح ساعت تردد پرسنل انتخاب شده به:";
		labelX5.TextAlignment = (StringAlignment)2;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[5]
		{
			(DataGridViewColumn)DgChecked,
			(DataGridViewColumn)DgCodeTafsil,
			(DataGridViewColumn)DgDateSh,
			(DataGridViewColumn)DgFullName,
			(DataGridViewColumn)DgTime
		});
		((DataGridView)DGrid).DataSource = editGrouplyBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(12, 12);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(610, 484);
		((Control)DGrid).TabIndex = 0;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridViewColumn)DgChecked).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgChecked).DataPropertyName = "Checked";
		((DataGridViewColumn)DgChecked).HeaderText = "";
		((DataGridViewColumn)DgChecked).Name = "DgChecked";
		((DataGridViewBand)DgChecked).ReadOnly = true;
		((DataGridViewColumn)DgChecked).Width = 5;
		((DataGridViewColumn)DgCodeTafsil).DataPropertyName = "CodeTafsil";
		((DataGridViewColumn)DgCodeTafsil).HeaderText = "کد تفصیل شخص";
		((DataGridViewColumn)DgCodeTafsil).Name = "DgCodeTafsil";
		((DataGridViewBand)DgCodeTafsil).ReadOnly = true;
		((DataGridViewBand)DgCodeTafsil).Visible = false;
		((DataGridViewColumn)DgDateSh).DataPropertyName = "DateSh";
		((DataGridViewColumn)DgDateSh).HeaderText = "تاریخ ";
		((DataGridViewColumn)DgDateSh).Name = "DgDateSh";
		((DataGridViewBand)DgDateSh).ReadOnly = true;
		((DataGridViewBand)DgDateSh).Visible = false;
		((DataGridViewColumn)DgFullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DgFullName).DataPropertyName = "FullName";
		((DataGridViewColumn)DgFullName).HeaderText = "نام و نام خانوادگی";
		((DataGridViewColumn)DgFullName).Name = "DgFullName";
		((DataGridViewBand)DgFullName).ReadOnly = true;
		((DataGridViewColumn)DgTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgTime).DataPropertyName = "Time";
		((DataGridViewColumn)DgTime).HeaderText = "ساعت تردد";
		((DataGridViewColumn)DgTime).Name = "DgTime";
		((DataGridViewBand)DgTime).ReadOnly = true;
		((DataGridViewColumn)DgTime).Width = 84;
		editGrouplyBindingSource.DataSource = typeof(EditGrouply);
		((Control)LblDate).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblDate).BackgroundStyle.Class = "";
		((BaseItemControl)LblDate).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblDate).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblDate).ForeColor = Color.DarkRed;
		((Control)LblDate).Location = new Point(435, 504);
		((Control)LblDate).Name = "LblDate";
		((Control)LblDate).Size = new Size(118, 23);
		((Control)LblDate).TabIndex = 0;
		((Control)LblDate).Text = Calendar.MiladiToCalture(ConstantValues.MinDateM);
		LblDate.TextAlignment = (StringAlignment)1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(634, 562);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(650, 453);
		((Control)this).Name = "FRMEditTaradodGrouply";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "اصلاح ترددها بصورت گروهی";
		((Control)this).KeyDown += new KeyEventHandler(FRMEditTaradodGrouply_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)editGrouplyBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

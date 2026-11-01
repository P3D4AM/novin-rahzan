using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Forms;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Taradod;

public class FrmMor_SaatiList : BaseForm
{
	private List<MorakhasiSaati> ListData = new List<MorakhasiSaati>();

	public CancellationTokenSource TokenFillData = new CancellationTokenSource();

	private IContainer components;

	private PanelEx panelEx1;

	private DataGridViewX dataGridViewX1;

	private BindingSource morakhasiSaatiBindingSource;

	private ButtonX BtnAccept;

	private DataGridViewTextBoxColumn fullNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn personIDDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn PersonFullName;

	private DataGridViewTextBoxColumn dateSHDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dOWDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateMDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn durationDataGridViewTextBoxColumn;

	public FrmMor_SaatiList(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
	}

	public FrmMor_SaatiList(string contextGuid, List<MorakhasiSaati> plst)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		ListData = plst;
		TokenFillData?.Cancel();
		TokenFillData = new CancellationTokenSource();
		_ = TokenFillData.Token;
		FillDatasource();
	}

	private void FillDatasource()
	{
		try
		{
			morakhasiSaatiBindingSource.DataSource = ListData;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		BtnAccept = new ButtonX();
		dataGridViewX1 = new DataGridViewX();
		morakhasiSaatiBindingSource = new BindingSource(components);
		personIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		PersonFullName = new DataGridViewTextBoxColumn();
		dateSHDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dOWDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dateMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		durationDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)dataGridViewX1).BeginInit();
		((ISupportInitialize)morakhasiSaatiBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)dataGridViewX1);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(634, 415);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.LightBlue;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		BtnAccept.DialogResult = (DialogResult)2;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(12, 369);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 16;
		((Control)BtnAccept).Text = "[تائید]";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((DataGridView)dataGridViewX1).AllowUserToAddRows = false;
		((DataGridView)dataGridViewX1).AllowUserToDeleteRows = false;
		((Control)dataGridViewX1).Anchor = (AnchorStyles)15;
		((DataGridView)dataGridViewX1).AutoGenerateColumns = false;
		((DataGridView)dataGridViewX1).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)dataGridViewX1).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[6]
		{
			(DataGridViewColumn)personIDDataGridViewTextBoxColumn,
			(DataGridViewColumn)PersonFullName,
			(DataGridViewColumn)dateSHDataGridViewTextBoxColumn,
			(DataGridViewColumn)dOWDataGridViewTextBoxColumn,
			(DataGridViewColumn)dateMDataGridViewTextBoxColumn,
			(DataGridViewColumn)durationDataGridViewTextBoxColumn
		});
		((DataGridView)dataGridViewX1).DataSource = morakhasiSaatiBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)dataGridViewX1).DefaultCellStyle = val;
		((DataGridView)dataGridViewX1).GridColor = Color.FromArgb(208, 215, 229);
		((Control)dataGridViewX1).Location = new Point(12, 12);
		((DataGridView)dataGridViewX1).MultiSelect = false;
		((Control)dataGridViewX1).Name = "dataGridViewX1";
		((DataGridView)dataGridViewX1).ReadOnly = true;
		((DataGridView)dataGridViewX1).RowHeadersVisible = false;
		((DataGridView)dataGridViewX1).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)dataGridViewX1).Size = new Size(610, 351);
		((Control)dataGridViewX1).TabIndex = 0;
		morakhasiSaatiBindingSource.DataSource = typeof(MorakhasiSaati);
		((DataGridViewColumn)personIDDataGridViewTextBoxColumn).DataPropertyName = "PersonID";
		((DataGridViewColumn)personIDDataGridViewTextBoxColumn).HeaderText = "PersonID";
		((DataGridViewColumn)personIDDataGridViewTextBoxColumn).Name = "personIDDataGridViewTextBoxColumn";
		((DataGridViewBand)personIDDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)personIDDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)PersonFullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)PersonFullName).DataPropertyName = "PersonFullName";
		((DataGridViewColumn)PersonFullName).HeaderText = "نام شخص";
		((DataGridViewColumn)PersonFullName).Name = "PersonFullName";
		((DataGridViewBand)PersonFullName).ReadOnly = true;
		((DataGridViewColumn)dateSHDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dateSHDataGridViewTextBoxColumn).DataPropertyName = "DateCalutre";
		((DataGridViewColumn)dateSHDataGridViewTextBoxColumn).HeaderText = "[تاریخ]";
		((DataGridViewColumn)dateSHDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)dateSHDataGridViewTextBoxColumn).Name = "dateSHDataGridViewTextBoxColumn";
		((DataGridViewBand)dateSHDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dOWDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dOWDataGridViewTextBoxColumn).DataPropertyName = "DayOfWeek";
		((DataGridViewColumn)dOWDataGridViewTextBoxColumn).HeaderText = "[روز هفته]";
		((DataGridViewColumn)dOWDataGridViewTextBoxColumn).MinimumWidth = 70;
		((DataGridViewColumn)dOWDataGridViewTextBoxColumn).Name = "dOWDataGridViewTextBoxColumn";
		((DataGridViewBand)dOWDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dOWDataGridViewTextBoxColumn).Width = 87;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).DataPropertyName = "DateM";
		val2.Format = "t";
		val2.NullValue = null;
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).DefaultCellStyle = val2;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).HeaderText = "[از ساعت]";
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).MinimumWidth = 70;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).Name = "dateMDataGridViewTextBoxColumn";
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).Width = 88;
		((DataGridViewColumn)durationDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)durationDataGridViewTextBoxColumn).DataPropertyName = "DurationTime";
		((DataGridViewColumn)durationDataGridViewTextBoxColumn).HeaderText = "[مدت] ";
		((DataGridViewColumn)durationDataGridViewTextBoxColumn).MinimumWidth = 50;
		((DataGridViewColumn)durationDataGridViewTextBoxColumn).Name = "durationDataGridViewTextBoxColumn";
		((DataGridViewBand)durationDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)durationDataGridViewTextBoxColumn).Width = 68;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnAccept;
		((Form)this).ClientSize = new Size(634, 415);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(650, 453);
		((Control)this).Name = "FrmMor_SaatiList";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[لیست مرخص ساعتی های استفاده شده]";
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)dataGridViewX1).EndInit();
		((ISupportInitialize)morakhasiSaatiBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

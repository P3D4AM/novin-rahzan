using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceUI.Forms.User.Buisness;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.User;

public class FrmUseAccLevelNew : FormExtention
{
	private List<AccessLevelEnumItems> _lstAccLevel;

	private readonly UserAttendance _userAttendance;

	private int _lastrow = -1;

	private IContainer components;

	private PanelEx panelEx1;

	private LabelX LblUserName;

	private LabelX LblUserCode;

	private LabelX labelX3;

	private LabelX labelX1;

	private ButtonX BtnAccept;

	private ButtonX BtnCancel;

	private DataGridViewX DGrid;

	private DataGridViewX DGridDet;

	private BindingSource propertyInfoClassBindingSource;

	private BindingSource accessLevelClassBindingSource;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn checkedMasterDG;

	private DataGridViewTextBoxColumn persianNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;

	private DataGridViewCheckBoxColumn checkedSlaveDG;

	private DataGridViewTextBoxColumn persianNameDataGridViewTextBoxColumn1;

	public FrmUseAccLevelNew(string contextGuid, UserAttendance userAttendance)
		: base(contextGuid, true, true)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		_userAttendance = userAttendance;
		InitializeComponent();
		FillObjects();
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
		((Control)labelX1).Text = "کد کاربر";
	}

	private void FillObjects()
	{
		try
		{
			_lstAccLevel = AccessLevelEnumItems.ListInfo(_userAttendance.AccessLevelSelf);
			accessLevelClassBindingSource.DataSource = _lstAccLevel;
			((Control)LblUserCode).Text = _userAttendance.Code.ToString();
			((Control)LblUserName).Text = _userAttendance.Name;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillObjects", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUseAccLevelNew.cs", 39);
		}
	}

	private void FrmUseAccLevelNew_Load(object sender, EventArgs e)
	{
		for (int i = 0; i < ((DataGridView)DGrid).Rows.Count; i++)
		{
			SetGridMaster(i, swSetGridDet: true);
		}
		SetGridDet();
	}

	private void SetGridDet(int row = -1)
	{
		try
		{
			if (accessLevelClassBindingSource.Current != null)
			{
				PropertyInfoClass propertyInfoClass = ((row == -1) ? ((AccessLevelEnumItems)accessLevelClassBindingSource.Current).Info : ((AccessLevelEnumItems)accessLevelClassBindingSource[row]).Info);
				propertyInfoClassBindingSource.DataSource = propertyInfoClass.ListInfo;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetGridDet", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUseAccLevelNew.cs", 69);
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			((FormExtention)this).OnKeyDown(e);
			if ((int)e.KeyCode != 32)
			{
				return;
			}
			e.Handled = true;
			if (((Control)DGrid).Focused)
			{
				if (((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)checkedMasterDG).Index)
				{
					ChangeValOfDGrid();
				}
			}
			else if (((Control)DGridDet).Focused && ((DataGridView)DGridDet).CurrentCell.ColumnIndex != ((DataGridViewBand)checkedSlaveDG).Index)
			{
				ChangeValOfDGridDet();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUseAccLevelNew.cs", 102);
		}
	}

	private void DGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
	}

	private void DGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex != _lastrow)
		{
			_lastrow = e.RowIndex;
			SetGridDet();
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)checkedMasterDG).Index)
			{
				ChangeValOfDGrid();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUseAccLevelNew.cs", 139);
		}
	}

	private void ChangeValOfDGrid()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Invalid comparison between Unknown and I4
		if (((BaseCollection)((DataGridView)DGrid).SelectedRows).Count <= 0)
		{
			return;
		}
		DataGridViewCell obj = ((DataGridView)DGrid).SelectedRows[0].Cells[((DataGridViewBand)checkedMasterDG).Index];
		DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
		if (val != null && ((DataGridViewCell)val).Value != null)
		{
			((DataGridViewCell)val).Value = (((int)(CheckBoxGridviewEnumType)((DataGridViewCell)val).Value == 1) ? val.FalseValue : val.TrueValue);
			for (int i = 0; i < ((DataGridView)DGridDet).RowCount; i++)
			{
				DataGridViewCell obj2 = ((DataGridView)DGridDet).Rows[i].Cells[((DataGridViewBand)checkedSlaveDG).Index];
				DataGridViewCheckBoxCell val2 = (DataGridViewCheckBoxCell)(object)((obj2 is DataGridViewCheckBoxCell) ? obj2 : null);
				if (val2 != null)
				{
					if ((int)(CheckBoxGridviewEnumType)((DataGridViewCell)val).Value == 1)
					{
						((DataGridViewCell)val2).Value = val2.TrueValue;
					}
					else
					{
						((DataGridViewCell)val2).Value = val2.FalseValue;
					}
				}
			}
		}
		((Control)DGridDet).Refresh();
		((BaseForm)this).IsFormContentsChanged = true;
	}

	private void ChangeValOfDGridDet()
	{
		DataGridViewCell obj = ((DataGridView)DGridDet).SelectedRows[0].Cells[((DataGridViewBand)checkedSlaveDG).Index];
		DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
		if (val != null)
		{
			((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
		}
		((BaseForm)this).IsFormContentsChanged = true;
		SetGridMaster(((DataGridView)DGrid).SelectedCells[0].RowIndex, swSetGridDet: false);
	}

	private void SetGridMaster(int row, bool swSetGridDet)
	{
		bool flag = true;
		bool flag2 = true;
		if (swSetGridDet)
		{
			SetGridDet(row);
		}
		for (int i = 0; i < ((DataGridView)DGridDet).RowCount; i++)
		{
			if (!(bool)((DataGridView)DGridDet).Rows[i].Cells[((DataGridViewBand)checkedSlaveDG).Index].Value)
			{
				flag = false;
			}
			else
			{
				flag2 = false;
			}
		}
		if (row >= ((DataGridView)DGrid).Rows.Count)
		{
			return;
		}
		DataGridViewCell obj = ((DataGridView)DGrid).Rows[row].Cells[((DataGridViewBand)checkedMasterDG).Index];
		DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
		if (val != null && ((DataGridViewCell)val).Value != null)
		{
			if (flag2)
			{
				((DataGridViewCell)val).Value = val.FalseValue;
			}
			else if (flag)
			{
				((DataGridViewCell)val).Value = val.TrueValue;
			}
			else
			{
				((DataGridViewCell)val).Value = val.IndeterminateValue;
			}
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		await _userAttendance.SaveAsync();
		((BaseForm)this).IsFormContentsChanged = false;
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	private void DGridDet_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)checkedSlaveDG).Index)
			{
				ChangeValOfDGridDet();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGridDet_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FrmUseAccLevelNew.cs", 226);
		}
	}

	private void DGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		((DataGridView)DGridDet).CommitEdit((DataGridViewDataErrorContexts)512);
	}

	private void DGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
	{
		SetGridDet();
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
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
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
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Expected O, but got Unknown
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Expected O, but got Unknown
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Expected O, but got Unknown
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Expected O, but got Unknown
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Expected O, but got Unknown
		//IL_0932: Unknown result type (might be due to invalid IL or missing references)
		//IL_093c: Expected O, but got Unknown
		//IL_0a1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a26: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		DGridDet = new DataGridViewX();
		nameDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		checkedSlaveDG = new DataGridViewCheckBoxColumn();
		persianNameDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		propertyInfoClassBindingSource = new BindingSource(components);
		DGrid = new DataGridViewX();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		checkedMasterDG = new DataGridViewCheckBoxColumn();
		persianNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		accessLevelClassBindingSource = new BindingSource(components);
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		LblUserName = new LabelX();
		LblUserCode = new LabelX();
		labelX3 = new LabelX();
		labelX1 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGridDet).BeginInit();
		((ISupportInitialize)propertyInfoClassBindingSource).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)accessLevelClassBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)DGridDet);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)LblUserName);
		((Control)panelEx1).Controls.Add((Control)(object)LblUserCode);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(800, 428);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((DataGridView)DGridDet).AllowUserToAddRows = false;
		((DataGridView)DGridDet).AllowUserToDeleteRows = false;
		((DataGridView)DGridDet).AllowUserToResizeRows = false;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGridDet).AlternatingRowsDefaultCellStyle = val;
		((DataGridView)DGridDet).AutoGenerateColumns = false;
		((DataGridView)DGridDet).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGridDet).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)nameDataGridViewTextBoxColumn1,
			(DataGridViewColumn)checkedSlaveDG,
			(DataGridViewColumn)persianNameDataGridViewTextBoxColumn1
		});
		((DataGridView)DGridDet).DataSource = propertyInfoClassBindingSource;
		((DataGridView)DGridDet).DefaultCellStyle = val;
		((DataGridView)DGridDet).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGridDet).Location = new Point(12, 70);
		((DataGridView)DGridDet).MultiSelect = false;
		((Control)DGridDet).Name = "DGridDet";
		((DataGridView)DGridDet).ReadOnly = true;
		((DataGridView)DGridDet).RowHeadersVisible = false;
		((DataGridView)DGridDet).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGridDet).Size = new Size(397, 299);
		((Control)DGridDet).TabIndex = 31;
		((DataGridView)DGridDet).CellClick += new DataGridViewCellEventHandler(DGridDet_CellClick);
		((DataGridViewColumn)nameDataGridViewTextBoxColumn1).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn1).HeaderText = "[نام]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn1).Name = "nameDataGridViewTextBoxColumn1";
		((DataGridViewBand)nameDataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewBand)nameDataGridViewTextBoxColumn1).Visible = false;
		((DataGridViewColumn)checkedSlaveDG).DataPropertyName = "Checked";
		checkedSlaveDG.FalseValue = "False";
		((DataGridViewColumn)checkedSlaveDG).HeaderText = "  ";
		((DataGridViewColumn)checkedSlaveDG).MinimumWidth = 20;
		((DataGridViewColumn)checkedSlaveDG).Name = "checkedSlaveDG";
		((DataGridViewBand)checkedSlaveDG).ReadOnly = true;
		((DataGridViewBand)checkedSlaveDG).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)checkedSlaveDG).SortMode = (DataGridViewColumnSortMode)1;
		checkedSlaveDG.TrueValue = "True";
		((DataGridViewColumn)checkedSlaveDG).Width = 20;
		((DataGridViewColumn)persianNameDataGridViewTextBoxColumn1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)persianNameDataGridViewTextBoxColumn1).DataPropertyName = "PersianName";
		((DataGridViewColumn)persianNameDataGridViewTextBoxColumn1).HeaderText = "[نام]";
		((DataGridViewColumn)persianNameDataGridViewTextBoxColumn1).Name = "persianNameDataGridViewTextBoxColumn1";
		((DataGridViewBand)persianNameDataGridViewTextBoxColumn1).ReadOnly = true;
		propertyInfoClassBindingSource.DataSource = typeof(PropertyInfoClass);
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val2;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)nameDataGridViewTextBoxColumn,
			(DataGridViewColumn)checkedMasterDG,
			(DataGridViewColumn)persianNameDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = accessLevelClassBindingSource;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(415, 12);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(373, 357);
		((Control)DGrid).TabIndex = 30;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridView)DGrid).CellContentClick += new DataGridViewCellEventHandler(DGrid_CellContentClick);
		((DataGridView)DGrid).CellEndEdit += new DataGridViewCellEventHandler(DGrid_CellEndEdit);
		((DataGridView)DGrid).CellEnter += new DataGridViewCellEventHandler(DGrid_CellEnter);
		((DataGridView)DGrid).CellValueChanged += new DataGridViewCellEventHandler(DGrid_CellValueChanged);
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "[نام]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)nameDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)checkedMasterDG).DataPropertyName = "Checked";
		checkedMasterDG.FalseValue = (object)(CheckBoxGridviewEnumType)2;
		((DataGridViewColumn)checkedMasterDG).HeaderText = "  ";
		checkedMasterDG.IndeterminateValue = (object)(CheckBoxGridviewEnumType)0;
		((DataGridViewColumn)checkedMasterDG).MinimumWidth = 20;
		((DataGridViewColumn)checkedMasterDG).Name = "checkedMasterDG";
		((DataGridViewBand)checkedMasterDG).ReadOnly = true;
		((DataGridViewBand)checkedMasterDG).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)checkedMasterDG).SortMode = (DataGridViewColumnSortMode)1;
		checkedMasterDG.ThreeState = true;
		checkedMasterDG.TrueValue = (object)(CheckBoxGridviewEnumType)1;
		((DataGridViewColumn)checkedMasterDG).Width = 20;
		((DataGridViewColumn)persianNameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)persianNameDataGridViewTextBoxColumn).DataPropertyName = "PersianName";
		((DataGridViewColumn)persianNameDataGridViewTextBoxColumn).HeaderText = "[نام]";
		((DataGridViewColumn)persianNameDataGridViewTextBoxColumn).Name = "persianNameDataGridViewTextBoxColumn";
		((DataGridViewBand)persianNameDataGridViewTextBoxColumn).ReadOnly = true;
		accessLevelClassBindingSource.DataSource = typeof(AccessLevelEnumItems);
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 375);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(107, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 7;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(125, 375);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(107, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 6;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((BaseItemControl)LblUserName).BackgroundStyle.Class = "";
		((BaseItemControl)LblUserName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblUserName).ForeColor = Color.DarkRed;
		((Control)LblUserName).Location = new Point(12, 41);
		((Control)LblUserName).Name = "LblUserName";
		((Control)LblUserName).Size = new Size(332, 23);
		((Control)LblUserName).TabIndex = 3;
		((Control)LblUserName).Text = "---------------";
		((BaseItemControl)LblUserCode).BackgroundStyle.Class = "";
		((BaseItemControl)LblUserCode).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblUserCode).ForeColor = Color.DarkRed;
		((Control)LblUserCode).Location = new Point(12, 12);
		((Control)LblUserCode).Name = "LblUserCode";
		((Control)LblUserCode).Size = new Size(332, 23);
		((Control)LblUserCode).TabIndex = 1;
		((Control)LblUserCode).Text = "---------------";
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(350, 41);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(59, 23);
		((Control)labelX3).TabIndex = 2;
		((Control)labelX3).Text = "[نام کاربری] :";
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(350, 12);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(59, 23);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "[UserCode] :";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(800, 450);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Name = "FrmUseAccLevelNew";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "[سطح دسترسی کاربر]";
		((Form)this).Load += FrmUseAccLevelNew_Load;
		((Control)this).Controls.SetChildIndex((Control)(object)panelEx1, 0);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)DGridDet).EndInit();
		((ISupportInitialize)propertyInfoClassBindingSource).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)accessLevelClassBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}

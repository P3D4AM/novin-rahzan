using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Forms.Person.Group;
using TimeAttendanceUI.Forms.Taradod;

namespace TimeAttendanceUI.Forms.Person.Import;

public class FrmAddExcelToPerson : Form, IHasContext
{
	private Font _font;

	public List<string> ListNames = new List<string>();

	public List<PersonGroup> LstPrsnGroup = new List<PersonGroup>();

	public List<PersonTitle> LstPersonTitles;

	internal PanelEx PanelEx1_NOTSend;

	internal LabelX Lbl_Won;

	internal GroupPanel GpStatuse;

	internal ButtonX BTNAdd_List;

	internal ButtonX BtnMoreGroup;

	internal TextBoxX TXTFile_Address;

	internal ButtonX BTNSelectFile;

	internal DataGridViewX DGrid;

	internal ComboBoxEx CMBSheet;

	internal Panel Panel1;

	internal TextBoxX txtMadrakTahsili;

	internal TextBoxX txtAddress;

	internal LabelX LabelX44;

	internal LabelX LabelX45;

	internal TextBoxX txtFatherName;

	internal LabelX LabelX47;

	internal TextBoxX txtMSodor;

	internal LabelX LabelX48;

	internal LabelX LabelX14;

	internal TextBoxX txtZipCode;

	internal TextBoxX txtName;

	internal TextBoxX txtMobile;

	internal LabelX LabelX31;

	internal TextBoxX txtGrpPerson;

	internal LabelX LabelX10;

	internal TextBoxX txtStartWork;

	internal TextBoxX txtShsh;

	internal LabelX LabelX6;

	internal TextBoxX txtMeliCode;

	internal TextBoxX txtJensiat;

	internal LabelX LabelX32;

	internal LabelX LabelX7;

	internal LabelX LabelX8;

	internal TextBoxX txtMTavalod;

	internal LabelX LabelX30;

	internal TextBoxX txtTel;

	internal LabelX LabelX28;

	internal LabelX LabelX13;

	internal TextBoxX txtBimehNo;

	internal TextBoxX txtMoein;

	internal LabelX LabelX12;

	internal LabelX LabelX9;

	internal TextBoxX txtFamily;

	internal LabelX LabelX33;

	internal LabelX LabelX26;

	internal TextBoxX txtSerialSh;

	internal LabelX LabelX5;

	internal TextBoxX txtTitle;

	internal LabelX LabelX1;

	internal TextBoxX txtBirthDate;

	internal LabelX LabelX4;

	internal TextBoxX txtTafsil;

	private DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn groupIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn titleIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn fullCodeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn familyDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn fullNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn fatherNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateMStartWorkDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateShStartWorkDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateMBirthdayDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateShBirthdayDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn codeMeliDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn codeTafsilDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn zipCodeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn mob1DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn mob2DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn tel1DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn tel2DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn adressDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn sTDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn gUIDDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn hIDDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn hTypeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn picPathDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn sodorShDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn noBimehDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn codeMoeinDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn mTavalodDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateMFinishWDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateShFinishWDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn eisarIdDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn shshDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn serialShDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn jensiatDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn madrakTahsiliDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn groupNameDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn checkDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn strEnrollDatasDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn defPersonNoDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn defDeviceNoDataGridViewTextBoxColumn;

	private BindingSource personBindingSource;

	private IContainer components;

	internal CheckBox chbxTitle;

	internal CheckBox chbxGroupPerson;

	internal ButtonX btnMoreTitlePerson;

	internal ComboBoxEx cbTitlePerson;

	internal ComboBoxEx cbGroupPerson;

	public string ContextGuid { get; set; }

	private short SelectedGroupId
	{
		get
		{
			object selectedValue = ((ListControl)cbGroupPerson).SelectedValue;
			if (selectedValue == null)
			{
				return 0;
			}
			return Utilities.ParseToShort(selectedValue.ToString(), (short)0);
		}
	}

	public FrmAddExcelToPerson(string contextGuid, Font font)
	{
		ContextGuid = contextGuid;
		try
		{
			InitializeComponent();
			LoadDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 34);
		}
		_font = font;
	}

	private async Task LoadDataAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			LstPersonTitles = await PersonTitle.SelectAllAsync(ContextGuid, "", token);
			((Control)cbGroupPerson).Focus();
			LstPrsnGroup = await PersonGroup.SelectAllAsync(ContextGuid, containsAllGroups: true, token);
			((ComboBox)cbGroupPerson).DataSource = LstPrsnGroup;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 49);
		}
	}

	private async void FrmAddExcelToPerson_Load(object sender, EventArgs e)
	{
		try
		{
			await LoadDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmAddExcelToPerson_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 58);
		}
	}

	private async void BtnMoreGroupPerson_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)(await ((CacheBaseForm)new FRMPersonGroupMain(ContextGuid, _font)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				await LoadDataAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMoreGroupPerson_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 69);
		}
	}

	private async void CmbGroup_KeyDown(object sender, KeyEventArgs e)
	{
		_ = 3;
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 45)
			{
				if ((int)keyCode == 118 && (int)(await ((CacheBaseForm)new FRMPersonGroup(ContextGuid, SelectedGroupId)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					await LoadDataAsync();
				}
			}
			else if ((int)(await ((CacheBaseForm)new FRMPersonGroup(ContextGuid)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				await LoadDataAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CmbGroup_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 90);
		}
	}

	private async void BTNAdd_List_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			ListNames.Clear();
			if (!((Control)BTNAdd_List).Enabled)
			{
				return;
			}
			((Control)BTNAdd_List).Enabled = false;
			if (((DataGridView)DGrid).RowCount == 0)
			{
				Utility.MessageBoxShowError("هیچ داده ای جهت ثبت وجود ندارد", ContextGuid);
				((Control)BTNAdd_List).Enabled = true;
				return;
			}
			FRMProgress frm = new FRMProgress(ContextGuid, ((DataGridView)DGrid).RowCount - 1);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			for (int i = 0; i <= ((DataGridView)DGrid).RowCount - 1; i++)
			{
				await SaveItemAsync(i);
			}
			((Form)frm).Close();
			((Component)(object)frm).Dispose();
			((Control)BTNAdd_List).Enabled = true;
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			((Control)BTNAdd_List).Enabled = true;
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNAdd_List_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 185);
		}
	}

	private async Task SaveItemAsync(int i)
	{
		try
		{
			await (GetItem(i) ?? throw new NullReferenceException("getItem result is null!!")).SaveAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 197);
		}
	}

	private TimeAttendanceDAL.Dal.FLRPerson.Person GetItem(int index)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetItem", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 233);
		}
		return null;
	}

	private string GetValue(int col, int row)
	{
		try
		{
			if (row < 0 || col <= 0)
			{
				return "";
			}
			col--;
			if (((DataGridView)DGrid)[col, row].Value != null)
			{
				return Text.FixString(((DataGridView)DGrid)[col, row].Value.ToString()).Trim();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetValue", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 266);
		}
		return "";
	}

	private async void BTNSelectFile_Click(object sender, EventArgs e)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNSelectFile_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 285);
		}
	}

	public async Task<short> GetGroupIDAsync(string input, CancellationToken token = default(CancellationToken))
	{
		short gid = 1;
		try
		{
			if (!chbxGroupPerson.Checked && input != "")
			{
				if (string.IsNullOrEmpty(input))
				{
					gid = 1;
				}
				else
				{
					PersonGroup personGroup = LstPrsnGroup.FirstOrDefault((PersonGroup q) => q.Name.Contains(input));
					if (personGroup != null)
					{
						gid = personGroup.Id;
					}
					else
					{
						PersonGroup vStr1 = new PersonGroup(ContextGuid)
						{
							Name = input
						};
						await vStr1.SaveAsync();
						gid = vStr1.Id;
					}
				}
			}
			else if (chbxGroupPerson.Checked && SelectedGroupId > 0)
			{
				gid = SelectedGroupId;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetGroupIDAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 345);
		}
		return gid;
	}

	private async Task<short> GetTitleIdAsync(string input, CancellationToken token)
	{
		short tid = 0;
		try
		{
			PersonTitle personTitle = LstPersonTitles.FirstOrDefault((PersonTitle q) => q.Name.Contains(input));
			if (personTitle != null)
			{
				tid = personTitle.Id;
			}
			else
			{
				PersonTitle vStr1 = new PersonTitle(ContextGuid)
				{
					Name = input
				};
				await vStr1.SaveAsync(token);
				tid = vStr1.Id;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetTitleIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\FrmAddExcelToPerson.cs", 365);
		}
		return tid;
	}

	[DebuggerNonUserCode]
	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		finally
		{
			((Form)this).Dispose(disposing);
		}
	}

	[DebuggerStepThrough]
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
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Expected O, but got Unknown
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Expected O, but got Unknown
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Expected O, but got Unknown
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Expected O, but got Unknown
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Expected O, but got Unknown
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Expected O, but got Unknown
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Expected O, but got Unknown
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Expected O, but got Unknown
		//IL_0d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3f: Expected O, but got Unknown
		//IL_0dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc9: Expected O, but got Unknown
		//IL_0e49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e53: Expected O, but got Unknown
		//IL_0ed9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee3: Expected O, but got Unknown
		//IL_0f6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f77: Expected O, but got Unknown
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Expected O, but got Unknown
		//IL_10dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e7: Expected O, but got Unknown
		//IL_1291: Unknown result type (might be due to invalid IL or missing references)
		//IL_129b: Expected O, but got Unknown
		//IL_1347: Unknown result type (might be due to invalid IL or missing references)
		//IL_1351: Expected O, but got Unknown
		//IL_1459: Unknown result type (might be due to invalid IL or missing references)
		//IL_1463: Expected O, but got Unknown
		//IL_149c: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a6: Expected O, but got Unknown
		//IL_15da: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e4: Expected O, but got Unknown
		//IL_161d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1627: Expected O, but got Unknown
		//IL_16d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e1: Expected O, but got Unknown
		//IL_1765: Unknown result type (might be due to invalid IL or missing references)
		//IL_176f: Expected O, but got Unknown
		//IL_17f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fd: Expected O, but got Unknown
		//IL_1881: Unknown result type (might be due to invalid IL or missing references)
		//IL_188b: Expected O, but got Unknown
		//IL_193b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1945: Expected O, but got Unknown
		//IL_19c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d3: Expected O, but got Unknown
		//IL_1a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a89: Expected O, but got Unknown
		//IL_1b0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b17: Expected O, but got Unknown
		//IL_1b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba5: Expected O, but got Unknown
		//IL_1c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5c: Expected O, but got Unknown
		//IL_1ce0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cea: Expected O, but got Unknown
		//IL_1d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d78: Expected O, but got Unknown
		//IL_1e28: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e32: Expected O, but got Unknown
		//IL_1ee2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eec: Expected O, but got Unknown
		//IL_1f9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa6: Expected O, but got Unknown
		//IL_202a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2034: Expected O, but got Unknown
		//IL_20e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ee: Expected O, but got Unknown
		//IL_2172: Unknown result type (might be due to invalid IL or missing references)
		//IL_217c: Expected O, but got Unknown
		//IL_2229: Unknown result type (might be due to invalid IL or missing references)
		//IL_2233: Expected O, but got Unknown
		//IL_22e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ed: Expected O, but got Unknown
		//IL_2371: Unknown result type (might be due to invalid IL or missing references)
		//IL_237b: Expected O, but got Unknown
		//IL_23ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2409: Expected O, but got Unknown
		//IL_24b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c3: Expected O, but got Unknown
		//IL_2573: Unknown result type (might be due to invalid IL or missing references)
		//IL_257d: Expected O, but got Unknown
		//IL_2601: Unknown result type (might be due to invalid IL or missing references)
		//IL_260b: Expected O, but got Unknown
		//IL_26bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c5: Expected O, but got Unknown
		//IL_2775: Unknown result type (might be due to invalid IL or missing references)
		//IL_277f: Expected O, but got Unknown
		//IL_2803: Unknown result type (might be due to invalid IL or missing references)
		//IL_280d: Expected O, but got Unknown
		//IL_28bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c7: Expected O, but got Unknown
		//IL_294b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2955: Expected O, but got Unknown
		//IL_2a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a0f: Expected O, but got Unknown
		//IL_2a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a9d: Expected O, but got Unknown
		//IL_2b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b57: Expected O, but got Unknown
		//IL_2d98: Unknown result type (might be due to invalid IL or missing references)
		//IL_2da2: Expected O, but got Unknown
		//IL_303f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3049: Expected O, but got Unknown
		//IL_3c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c3e: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		PanelEx1_NOTSend = new PanelEx();
		GpStatuse = new GroupPanel();
		Panel1 = new Panel();
		txtMadrakTahsili = new TextBoxX();
		txtAddress = new TextBoxX();
		LabelX44 = new LabelX();
		chbxTitle = new CheckBox();
		chbxGroupPerson = new CheckBox();
		LabelX45 = new LabelX();
		txtFatherName = new TextBoxX();
		btnMoreTitlePerson = new ButtonX();
		BtnMoreGroup = new ButtonX();
		LabelX47 = new LabelX();
		txtMSodor = new TextBoxX();
		cbTitlePerson = new ComboBoxEx();
		LabelX48 = new LabelX();
		cbGroupPerson = new ComboBoxEx();
		LabelX14 = new LabelX();
		txtZipCode = new TextBoxX();
		txtName = new TextBoxX();
		txtMobile = new TextBoxX();
		LabelX31 = new LabelX();
		txtGrpPerson = new TextBoxX();
		LabelX10 = new LabelX();
		txtStartWork = new TextBoxX();
		txtShsh = new TextBoxX();
		LabelX6 = new LabelX();
		txtMeliCode = new TextBoxX();
		txtJensiat = new TextBoxX();
		LabelX32 = new LabelX();
		LabelX7 = new LabelX();
		LabelX8 = new LabelX();
		txtMTavalod = new TextBoxX();
		LabelX30 = new LabelX();
		txtTel = new TextBoxX();
		LabelX28 = new LabelX();
		LabelX13 = new LabelX();
		txtBimehNo = new TextBoxX();
		txtMoein = new TextBoxX();
		LabelX12 = new LabelX();
		LabelX9 = new LabelX();
		txtFamily = new TextBoxX();
		LabelX33 = new LabelX();
		LabelX26 = new LabelX();
		txtSerialSh = new TextBoxX();
		LabelX5 = new LabelX();
		txtTitle = new TextBoxX();
		LabelX1 = new LabelX();
		txtBirthDate = new TextBoxX();
		LabelX4 = new LabelX();
		txtTafsil = new TextBoxX();
		CMBSheet = new ComboBoxEx();
		TXTFile_Address = new TextBoxX();
		BTNSelectFile = new ButtonX();
		Lbl_Won = new LabelX();
		DGrid = new DataGridViewX();
		iDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		groupIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		titleIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		fullCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		typeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		familyDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		fullNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		fatherNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dateMStartWorkDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dateShStartWorkDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dateMBirthdayDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dateShBirthdayDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		codeMeliDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		codeTafsilDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		zipCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		mob1DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		mob2DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		tel1DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		tel2DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		adressDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		sTDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		gUIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		hIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		hTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		picPathDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		descriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		sodorShDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		noBimehDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		codeMoeinDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		mTavalodDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dateMFinishWDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dateShFinishWDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		eisarIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		shshDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		serialShDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		jensiatDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		madrakTahsiliDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		groupNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		checkDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		strEnrollDatasDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		defPersonNoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		defDeviceNoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		personBindingSource = new BindingSource(components);
		BTNAdd_List = new ButtonX();
		((Control)PanelEx1_NOTSend).SuspendLayout();
		((Control)GpStatuse).SuspendLayout();
		((Control)Panel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)personBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1_NOTSend.CanvasColor = SystemColors.Control;
		PanelEx1_NOTSend.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1_NOTSend).Controls.Add((Control)(object)GpStatuse);
		((Control)PanelEx1_NOTSend).Dock = (DockStyle)5;
		((Control)PanelEx1_NOTSend).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)PanelEx1_NOTSend).Location = new Point(0, 0);
		((Control)PanelEx1_NOTSend).Name = "PanelEx1_NOTSend";
		((Control)PanelEx1_NOTSend).Size = new Size(794, 571);
		PanelEx1_NOTSend.Style.Alignment = (StringAlignment)1;
		PanelEx1_NOTSend.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx1_NOTSend.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx1_NOTSend.Style.Border = (eBorderType)1;
		PanelEx1_NOTSend.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1_NOTSend.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1_NOTSend.Style.GradientAngle = 90;
		((Control)PanelEx1_NOTSend).TabIndex = 0;
		((Control)GpStatuse).Anchor = (AnchorStyles)15;
		((PanelControl)GpStatuse).CanvasColor = SystemColors.Control;
		((PanelControl)GpStatuse).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GpStatuse).Controls.Add((Control)(object)Panel1);
		((Control)GpStatuse).Controls.Add((Control)(object)CMBSheet);
		((Control)GpStatuse).Controls.Add((Control)(object)TXTFile_Address);
		((Control)GpStatuse).Controls.Add((Control)(object)BTNSelectFile);
		((Control)GpStatuse).Controls.Add((Control)(object)Lbl_Won);
		((Control)GpStatuse).Controls.Add((Control)(object)DGrid);
		((Control)GpStatuse).Controls.Add((Control)(object)BTNAdd_List);
		((Control)GpStatuse).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)GpStatuse).Location = new Point(12, 3);
		((Control)GpStatuse).Name = "GpStatuse";
		((Control)GpStatuse).Size = new Size(770, 565);
		((PanelControl)GpStatuse).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)GpStatuse).Style.BackColorGradientAngle = 90;
		((PanelControl)GpStatuse).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)GpStatuse).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)GpStatuse).Style.BorderBottomWidth = 2;
		((PanelControl)GpStatuse).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)GpStatuse).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)GpStatuse).Style.BorderLeftWidth = 2;
		((PanelControl)GpStatuse).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)GpStatuse).Style.BorderRightWidth = 2;
		((PanelControl)GpStatuse).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)GpStatuse).Style.BorderTopWidth = 2;
		((PanelControl)GpStatuse).Style.Class = "";
		((PanelControl)GpStatuse).Style.CornerDiameter = 4;
		((PanelControl)GpStatuse).Style.CornerType = (eCornerType)2;
		((PanelControl)GpStatuse).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)GpStatuse).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GpStatuse).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GpStatuse).StyleMouseDown.Class = "";
		((PanelControl)GpStatuse).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GpStatuse).StyleMouseOver.Class = "";
		((PanelControl)GpStatuse).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GpStatuse).TabIndex = 0;
		((Control)Panel1).Controls.Add((Control)(object)txtMadrakTahsili);
		((Control)Panel1).Controls.Add((Control)(object)txtAddress);
		((Control)Panel1).Controls.Add((Control)(object)LabelX44);
		((Control)Panel1).Controls.Add((Control)(object)chbxTitle);
		((Control)Panel1).Controls.Add((Control)(object)chbxGroupPerson);
		((Control)Panel1).Controls.Add((Control)(object)LabelX45);
		((Control)Panel1).Controls.Add((Control)(object)txtFatherName);
		((Control)Panel1).Controls.Add((Control)(object)btnMoreTitlePerson);
		((Control)Panel1).Controls.Add((Control)(object)BtnMoreGroup);
		((Control)Panel1).Controls.Add((Control)(object)LabelX47);
		((Control)Panel1).Controls.Add((Control)(object)txtMSodor);
		((Control)Panel1).Controls.Add((Control)(object)cbTitlePerson);
		((Control)Panel1).Controls.Add((Control)(object)LabelX48);
		((Control)Panel1).Controls.Add((Control)(object)cbGroupPerson);
		((Control)Panel1).Controls.Add((Control)(object)LabelX14);
		((Control)Panel1).Controls.Add((Control)(object)txtZipCode);
		((Control)Panel1).Controls.Add((Control)(object)txtName);
		((Control)Panel1).Controls.Add((Control)(object)txtMobile);
		((Control)Panel1).Controls.Add((Control)(object)LabelX31);
		((Control)Panel1).Controls.Add((Control)(object)txtGrpPerson);
		((Control)Panel1).Controls.Add((Control)(object)LabelX10);
		((Control)Panel1).Controls.Add((Control)(object)txtStartWork);
		((Control)Panel1).Controls.Add((Control)(object)txtShsh);
		((Control)Panel1).Controls.Add((Control)(object)LabelX6);
		((Control)Panel1).Controls.Add((Control)(object)txtMeliCode);
		((Control)Panel1).Controls.Add((Control)(object)txtJensiat);
		((Control)Panel1).Controls.Add((Control)(object)LabelX32);
		((Control)Panel1).Controls.Add((Control)(object)LabelX7);
		((Control)Panel1).Controls.Add((Control)(object)LabelX8);
		((Control)Panel1).Controls.Add((Control)(object)txtMTavalod);
		((Control)Panel1).Controls.Add((Control)(object)LabelX30);
		((Control)Panel1).Controls.Add((Control)(object)txtTel);
		((Control)Panel1).Controls.Add((Control)(object)LabelX28);
		((Control)Panel1).Controls.Add((Control)(object)LabelX13);
		((Control)Panel1).Controls.Add((Control)(object)txtBimehNo);
		((Control)Panel1).Controls.Add((Control)(object)txtMoein);
		((Control)Panel1).Controls.Add((Control)(object)LabelX12);
		((Control)Panel1).Controls.Add((Control)(object)LabelX9);
		((Control)Panel1).Controls.Add((Control)(object)txtFamily);
		((Control)Panel1).Controls.Add((Control)(object)LabelX33);
		((Control)Panel1).Controls.Add((Control)(object)LabelX26);
		((Control)Panel1).Controls.Add((Control)(object)txtSerialSh);
		((Control)Panel1).Controls.Add((Control)(object)LabelX5);
		((Control)Panel1).Controls.Add((Control)(object)txtTitle);
		((Control)Panel1).Controls.Add((Control)(object)LabelX1);
		((Control)Panel1).Controls.Add((Control)(object)txtBirthDate);
		((Control)Panel1).Controls.Add((Control)(object)LabelX4);
		((Control)Panel1).Controls.Add((Control)(object)txtTafsil);
		((Control)Panel1).Location = new Point(6, 323);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).Size = new Size(754, 182);
		((Control)Panel1).TabIndex = 31;
		txtMadrakTahsili.Border.Class = "TextBoxBorder";
		txtMadrakTahsili.Border.CornerType = (eCornerType)1;
		((Control)txtMadrakTahsili).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtMadrakTahsili).Location = new Point(2, 91);
		((Control)txtMadrakTahsili).Name = "txtMadrakTahsili";
		((Control)txtMadrakTahsili).Size = new Size(36, 21);
		((Control)txtMadrakTahsili).TabIndex = 225;
		txtAddress.Border.Class = "TextBoxBorder";
		txtAddress.Border.CornerType = (eCornerType)1;
		((Control)txtAddress).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtAddress).Location = new Point(2, 37);
		((Control)txtAddress).Name = "txtAddress";
		((Control)txtAddress).Size = new Size(36, 21);
		((Control)txtAddress).TabIndex = 223;
		((BaseItemControl)LabelX44).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX44).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX44).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX44).ForeColor = Color.Blue;
		((Control)LabelX44).Location = new Point(639, 64);
		((Control)LabelX44).Name = "LabelX44";
		((Control)LabelX44).Size = new Size(101, 21);
		((Control)LabelX44).TabIndex = 0;
		((Control)LabelX44).Text = "کد پستی :";
		LabelX44.TextAlignment = (StringAlignment)2;
		((Control)chbxTitle).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)chbxTitle).ForeColor = Color.Blue;
		((Control)chbxTitle).Location = new Point(206, 145);
		((Control)chbxTitle).Name = "chbxTitle";
		((Control)chbxTitle).Size = new Size(115, 21);
		((Control)chbxTitle).TabIndex = 6;
		((Control)chbxTitle).Text = "عـنـوان شـخـص :";
		((ButtonBase)chbxTitle).TextAlign = (ContentAlignment)64;
		((Control)chbxGroupPerson).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)chbxGroupPerson).ForeColor = Color.Blue;
		((Control)chbxGroupPerson).Location = new Point(597, 145);
		((Control)chbxGroupPerson).Name = "chbxGroupPerson";
		((Control)chbxGroupPerson).Size = new Size(127, 21);
		((Control)chbxGroupPerson).TabIndex = 6;
		((Control)chbxGroupPerson).Text = "گـــــروه شــــخـــص :";
		((ButtonBase)chbxGroupPerson).TextAlign = (ContentAlignment)64;
		((BaseItemControl)LabelX45).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX45).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX45).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX45).ForeColor = Color.Blue;
		((Control)LabelX45).Location = new Point(639, 91);
		((Control)LabelX45).Name = "LabelX45";
		((Control)LabelX45).Size = new Size(68, 21);
		((Control)LabelX45).TabIndex = 0;
		((Control)LabelX45).Text = "جنسیت :";
		LabelX45.TextAlignment = (StringAlignment)2;
		txtFatherName.Border.Class = "TextBoxBorder";
		txtFatherName.Border.CornerType = (eCornerType)1;
		((Control)txtFatherName).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtFatherName).Location = new Point(2, 10);
		((Control)txtFatherName).Name = "txtFatherName";
		((Control)txtFatherName).Size = new Size(36, 21);
		((Control)txtFatherName).TabIndex = 222;
		((Control)btnMoreTitlePerson).AccessibleRole = (AccessibleRole)43;
		btnMoreTitlePerson.ColorTable = (eButtonColor)6;
		((Control)btnMoreTitlePerson).Location = new Point(8, 145);
		((Control)btnMoreTitlePerson).Name = "btnMoreTitlePerson";
		((Control)btnMoreTitlePerson).Size = new Size(30, 21);
		((PopupItemControl)btnMoreTitlePerson).Style = (eDotNetBarStyle)9;
		((Control)btnMoreTitlePerson).TabIndex = 8;
		((Control)btnMoreTitlePerson).Text = "...";
		((Control)btnMoreTitlePerson).Click += BtnMoreGroupPerson_Click;
		((Control)BtnMoreGroup).AccessibleRole = (AccessibleRole)43;
		BtnMoreGroup.ColorTable = (eButtonColor)6;
		((Control)BtnMoreGroup).Location = new Point(399, 145);
		((Control)BtnMoreGroup).Name = "BtnMoreGroup";
		((Control)BtnMoreGroup).Size = new Size(30, 21);
		((PopupItemControl)BtnMoreGroup).Style = (eDotNetBarStyle)9;
		((Control)BtnMoreGroup).TabIndex = 8;
		((Control)BtnMoreGroup).Text = "...";
		((Control)BtnMoreGroup).Click += BtnMoreGroupPerson_Click;
		((BaseItemControl)LabelX47).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX47).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX47).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX47).ForeColor = Color.Blue;
		((Control)LabelX47).Location = new Point(347, 91);
		((Control)LabelX47).Name = "LabelX47";
		((Control)LabelX47).Size = new Size(82, 21);
		((Control)LabelX47).TabIndex = 0;
		((Control)LabelX47).Text = "محل تولد :";
		LabelX47.TextAlignment = (StringAlignment)2;
		txtMSodor.Border.Class = "TextBoxBorder";
		txtMSodor.Border.CornerType = (eCornerType)1;
		((Control)txtMSodor).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtMSodor).Location = new Point(2, 64);
		((Control)txtMSodor).Name = "txtMSodor";
		((Control)txtMSodor).Size = new Size(36, 21);
		((Control)txtMSodor).TabIndex = 224;
		((ListControl)cbTitlePerson).DisplayMember = "Id";
		((ComboBox)cbTitlePerson).DrawMode = (DrawMode)1;
		((ComboBox)cbTitlePerson).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbTitlePerson).FormattingEnabled = true;
		((ComboBox)cbTitlePerson).ItemHeight = 15;
		((Control)cbTitlePerson).Location = new Point(44, 145);
		((Control)cbTitlePerson).Name = "cbTitlePerson";
		((Control)cbTitlePerson).RightToLeft = (RightToLeft)1;
		((Control)cbTitlePerson).Size = new Size(156, 21);
		cbTitlePerson.Style = (eDotNetBarStyle)9;
		((Control)cbTitlePerson).TabIndex = 7;
		((ListControl)cbTitlePerson).ValueMember = "Id";
		((Control)cbTitlePerson).KeyDown += new KeyEventHandler(CmbGroup_KeyDown);
		((BaseItemControl)LabelX48).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX48).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX48).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX48).ForeColor = Color.Blue;
		((Control)LabelX48).Location = new Point(44, 89);
		((Control)LabelX48).Name = "LabelX48";
		((Control)LabelX48).Size = new Size(101, 21);
		((Control)LabelX48).TabIndex = 0;
		((Control)LabelX48).Text = "مدرک تحصیلی :";
		LabelX48.TextAlignment = (StringAlignment)2;
		((ListControl)cbGroupPerson).DisplayMember = "Id";
		((ComboBox)cbGroupPerson).DrawMode = (DrawMode)1;
		((ComboBox)cbGroupPerson).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbGroupPerson).FormattingEnabled = true;
		((ComboBox)cbGroupPerson).ItemHeight = 15;
		((Control)cbGroupPerson).Location = new Point(435, 145);
		((Control)cbGroupPerson).Name = "cbGroupPerson";
		((Control)cbGroupPerson).RightToLeft = (RightToLeft)1;
		((Control)cbGroupPerson).Size = new Size(156, 21);
		cbGroupPerson.Style = (eDotNetBarStyle)9;
		((Control)cbGroupPerson).TabIndex = 7;
		((ListControl)cbGroupPerson).ValueMember = "Id";
		((Control)cbGroupPerson).KeyDown += new KeyEventHandler(CmbGroup_KeyDown);
		((BaseItemControl)LabelX14).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX14).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX14).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX14).ForeColor = Color.Blue;
		((Control)LabelX14).Location = new Point(639, 37);
		((Control)LabelX14).Name = "LabelX14";
		((Control)LabelX14).Size = new Size(89, 21);
		((Control)LabelX14).TabIndex = 153;
		((Control)LabelX14).Text = "عنوان :";
		LabelX14.TextAlignment = (StringAlignment)2;
		txtZipCode.Border.Class = "TextBoxBorder";
		txtZipCode.Border.CornerType = (eCornerType)1;
		((Control)txtZipCode).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtZipCode).Location = new Point(597, 66);
		((Control)txtZipCode).Name = "txtZipCode";
		((Control)txtZipCode).Size = new Size(36, 21);
		((Control)txtZipCode).TabIndex = 166;
		txtName.Border.Class = "TextBoxBorder";
		txtName.Border.CornerType = (eCornerType)1;
		((Control)txtName).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtName).Location = new Point(305, 10);
		((Control)txtName).Name = "txtName";
		((Control)txtName).Size = new Size(36, 21);
		((Control)txtName).TabIndex = 154;
		txtMobile.Border.Class = "TextBoxBorder";
		txtMobile.Border.CornerType = (eCornerType)1;
		((Control)txtMobile).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtMobile).Location = new Point(305, 37);
		((Control)txtMobile).Name = "txtMobile";
		((Control)txtMobile).Size = new Size(36, 21);
		((Control)txtMobile).TabIndex = 162;
		((BaseItemControl)LabelX31).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX31).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX31).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX31).ForeColor = Color.Blue;
		((Control)LabelX31).Location = new Point(491, 91);
		((Control)LabelX31).Name = "LabelX31";
		((Control)LabelX31).Size = new Size(90, 21);
		((Control)LabelX31).TabIndex = 165;
		((Control)LabelX31).Text = "تاریخ تولد :";
		LabelX31.TextAlignment = (StringAlignment)2;
		txtGrpPerson.Border.Class = "TextBoxBorder";
		txtGrpPerson.Border.CornerType = (eCornerType)1;
		((Control)txtGrpPerson).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtGrpPerson).Location = new Point(449, 37);
		((Control)txtGrpPerson).Name = "txtGrpPerson";
		((Control)txtGrpPerson).Size = new Size(36, 21);
		((Control)txtGrpPerson).TabIndex = 160;
		((BaseItemControl)LabelX10).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX10).ForeColor = Color.Blue;
		((Control)LabelX10).Location = new Point(44, 8);
		((Control)LabelX10).Name = "LabelX10";
		((Control)LabelX10).Size = new Size(89, 21);
		((Control)LabelX10).TabIndex = 161;
		((Control)LabelX10).Text = "نام پدر :";
		LabelX10.TextAlignment = (StringAlignment)2;
		txtStartWork.Border.Class = "TextBoxBorder";
		txtStartWork.Border.CornerType = (eCornerType)1;
		((Control)txtStartWork).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtStartWork).Location = new Point(156, 91);
		((Control)txtStartWork).Name = "txtStartWork";
		((Control)txtStartWork).Size = new Size(36, 21);
		((Control)txtStartWork).TabIndex = 180;
		txtShsh.Border.Class = "TextBoxBorder";
		txtShsh.Border.CornerType = (eCornerType)1;
		((Control)txtShsh).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtShsh).Location = new Point(305, 64);
		((Control)txtShsh).Name = "txtShsh";
		((Control)txtShsh).Size = new Size(36, 21);
		((Control)txtShsh).TabIndex = 170;
		((BaseItemControl)LabelX6).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX6).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX6).ForeColor = Color.Blue;
		((Control)LabelX6).Location = new Point(44, 35);
		((Control)LabelX6).Name = "LabelX6";
		((Control)LabelX6).Size = new Size(101, 21);
		((Control)LabelX6).TabIndex = 173;
		((Control)LabelX6).Text = "آدرس :";
		LabelX6.TextAlignment = (StringAlignment)2;
		txtMeliCode.Border.Class = "TextBoxBorder";
		txtMeliCode.Border.CornerType = (eCornerType)1;
		((Control)txtMeliCode).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtMeliCode).Location = new Point(449, 64);
		((Control)txtMeliCode).Name = "txtMeliCode";
		((Control)txtMeliCode).Size = new Size(36, 21);
		((Control)txtMeliCode).TabIndex = 168;
		txtJensiat.Border.Class = "TextBoxBorder";
		txtJensiat.Border.CornerType = (eCornerType)1;
		((Control)txtJensiat).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtJensiat).Location = new Point(597, 91);
		((Control)txtJensiat).Name = "txtJensiat";
		((Control)txtJensiat).Size = new Size(36, 21);
		((Control)txtJensiat).TabIndex = 174;
		((BaseItemControl)LabelX32).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX32).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX32).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX32).ForeColor = Color.Blue;
		((Control)LabelX32).Location = new Point(347, 37);
		((Control)LabelX32).Name = "LabelX32";
		((Control)LabelX32).Size = new Size(48, 21);
		((Control)LabelX32).TabIndex = 169;
		((Control)LabelX32).Text = "همراه :";
		LabelX32.TextAlignment = (StringAlignment)2;
		((BaseItemControl)LabelX7).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX7).ForeColor = Color.Blue;
		((Control)LabelX7).Location = new Point(198, 10);
		((Control)LabelX7).Name = "LabelX7";
		((Control)LabelX7).Size = new Size(77, 21);
		((Control)LabelX7).TabIndex = 159;
		((Control)LabelX7).Text = "نام خانوادگی :";
		LabelX7.TextAlignment = (StringAlignment)2;
		((BaseItemControl)LabelX8).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX8).ForeColor = Color.Blue;
		((Control)LabelX8).Location = new Point(639, 118);
		((Control)LabelX8).Name = "LabelX8";
		((Control)LabelX8).Size = new Size(101, 21);
		((Control)LabelX8).TabIndex = 181;
		((Control)LabelX8).Text = "شماره بیمه :";
		LabelX8.TextAlignment = (StringAlignment)2;
		txtMTavalod.Border.Class = "TextBoxBorder";
		txtMTavalod.Border.CornerType = (eCornerType)1;
		((Control)txtMTavalod).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtMTavalod).Location = new Point(305, 91);
		((Control)txtMTavalod).Name = "txtMTavalod";
		((Control)txtMTavalod).Size = new Size(36, 21);
		((Control)txtMTavalod).TabIndex = 178;
		((BaseItemControl)LabelX30).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX30).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX30).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX30).ForeColor = Color.Blue;
		((Control)LabelX30).Location = new Point(198, 91);
		((Control)LabelX30).Name = "LabelX30";
		((Control)LabelX30).Size = new Size(96, 21);
		((Control)LabelX30).TabIndex = 167;
		((Control)LabelX30).Text = "تاریخ شروع کار :";
		LabelX30.TextAlignment = (StringAlignment)2;
		txtTel.Border.Class = "TextBoxBorder";
		txtTel.Border.CornerType = (eCornerType)1;
		((Control)txtTel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtTel).Location = new Point(156, 37);
		((Control)txtTel).Name = "txtTel";
		((Control)txtTel).Size = new Size(36, 21);
		((Control)txtTel).TabIndex = 164;
		((BaseItemControl)LabelX28).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX28).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX28).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX28).ForeColor = Color.Blue;
		((Control)LabelX28).Location = new Point(44, 62);
		((Control)LabelX28).Name = "LabelX28";
		((Control)LabelX28).Size = new Size(91, 21);
		((Control)LabelX28).TabIndex = 179;
		((Control)LabelX28).Text = "محل صدور :";
		LabelX28.TextAlignment = (StringAlignment)2;
		((BaseItemControl)LabelX13).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX13).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX13).ForeColor = Color.Blue;
		((Control)LabelX13).Location = new Point(491, 64);
		((Control)LabelX13).Name = "LabelX13";
		((Control)LabelX13).Size = new Size(70, 21);
		((Control)LabelX13).TabIndex = 163;
		((Control)LabelX13).Text = "کد ملی :";
		LabelX13.TextAlignment = (StringAlignment)2;
		txtBimehNo.Border.Class = "TextBoxBorder";
		txtBimehNo.Border.CornerType = (eCornerType)1;
		((Control)txtBimehNo).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtBimehNo).Location = new Point(597, 118);
		((Control)txtBimehNo).Name = "txtBimehNo";
		((Control)txtBimehNo).Size = new Size(36, 21);
		((Control)txtBimehNo).TabIndex = 182;
		txtMoein.Border.Class = "TextBoxBorder";
		txtMoein.Border.CornerType = (eCornerType)1;
		((Control)txtMoein).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtMoein).Location = new Point(597, 10);
		((Control)txtMoein).Name = "txtMoein";
		((Control)txtMoein).Size = new Size(36, 21);
		((Control)txtMoein).TabIndex = 150;
		((BaseItemControl)LabelX12).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX12).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX12).ForeColor = Color.Blue;
		((Control)LabelX12).Location = new Point(639, 10);
		((Control)LabelX12).Name = "LabelX12";
		((Control)LabelX12).Size = new Size(101, 21);
		((Control)LabelX12).TabIndex = 149;
		((Control)LabelX12).Text = "کد میعن :";
		LabelX12.TextAlignment = (StringAlignment)2;
		((BaseItemControl)LabelX9).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX9).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX9).ForeColor = Color.Blue;
		((Control)LabelX9).Location = new Point(347, 64);
		((Control)LabelX9).Name = "LabelX9";
		((Control)LabelX9).Size = new Size(82, 21);
		((Control)LabelX9).TabIndex = 175;
		((Control)LabelX9).Text = "ش شناسنامه :";
		LabelX9.TextAlignment = (StringAlignment)2;
		txtFamily.Border.Class = "TextBoxBorder";
		txtFamily.Border.CornerType = (eCornerType)1;
		((Control)txtFamily).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtFamily).Location = new Point(156, 10);
		((Control)txtFamily).Name = "txtFamily";
		((Control)txtFamily).Size = new Size(36, 21);
		((Control)txtFamily).TabIndex = 156;
		((BaseItemControl)LabelX33).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX33).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX33).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX33).ForeColor = Color.Blue;
		((Control)LabelX33).Location = new Point(491, 37);
		((Control)LabelX33).Name = "LabelX33";
		((Control)LabelX33).Size = new Size(70, 21);
		((Control)LabelX33).TabIndex = 155;
		((Control)LabelX33).Text = "گروه پرسنل :";
		LabelX33.TextAlignment = (StringAlignment)2;
		((BaseItemControl)LabelX26).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX26).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX26).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX26).ForeColor = Color.Blue;
		((Control)LabelX26).Location = new Point(198, 64);
		((Control)LabelX26).Name = "LabelX26";
		((Control)LabelX26).Size = new Size(88, 21);
		((Control)LabelX26).TabIndex = 177;
		((Control)LabelX26).Text = "س شناسنامه :";
		LabelX26.TextAlignment = (StringAlignment)2;
		txtSerialSh.Border.Class = "TextBoxBorder";
		txtSerialSh.Border.CornerType = (eCornerType)1;
		((Control)txtSerialSh).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtSerialSh).Location = new Point(156, 64);
		((Control)txtSerialSh).Name = "txtSerialSh";
		((Control)txtSerialSh).Size = new Size(36, 21);
		((Control)txtSerialSh).TabIndex = 172;
		((BaseItemControl)LabelX5).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX5).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX5).ForeColor = Color.Blue;
		((Control)LabelX5).Location = new Point(198, 37);
		((Control)LabelX5).Name = "LabelX5";
		((Control)LabelX5).Size = new Size(77, 21);
		((Control)LabelX5).TabIndex = 171;
		((Control)LabelX5).Text = "تلفن :";
		LabelX5.TextAlignment = (StringAlignment)2;
		txtTitle.Border.Class = "TextBoxBorder";
		txtTitle.Border.CornerType = (eCornerType)1;
		((Control)txtTitle).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtTitle).Location = new Point(597, 37);
		((Control)txtTitle).Name = "txtTitle";
		((Control)txtTitle).Size = new Size(36, 21);
		((Control)txtTitle).TabIndex = 158;
		((BaseItemControl)LabelX1).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX1).ForeColor = Color.Blue;
		((Control)LabelX1).Location = new Point(491, 10);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(70, 21);
		((Control)LabelX1).TabIndex = 151;
		((Control)LabelX1).Text = "کد تفضیل :";
		LabelX1.TextAlignment = (StringAlignment)2;
		txtBirthDate.Border.Class = "TextBoxBorder";
		txtBirthDate.Border.CornerType = (eCornerType)1;
		((Control)txtBirthDate).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtBirthDate).Location = new Point(449, 91);
		((Control)txtBirthDate).Name = "txtBirthDate";
		((Control)txtBirthDate).Size = new Size(36, 21);
		((Control)txtBirthDate).TabIndex = 176;
		((BaseItemControl)LabelX4).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LabelX4).ForeColor = Color.Blue;
		((Control)LabelX4).Location = new Point(347, 10);
		((Control)LabelX4).Name = "LabelX4";
		((Control)LabelX4).Size = new Size(48, 21);
		((Control)LabelX4).TabIndex = 157;
		((Control)LabelX4).Text = "نام :";
		LabelX4.TextAlignment = (StringAlignment)2;
		txtTafsil.Border.Class = "";
		txtTafsil.Border.CornerType = (eCornerType)1;
		((Control)txtTafsil).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtTafsil).Location = new Point(449, 10);
		((Control)txtTafsil).Name = "txtTafsil";
		((Control)txtTafsil).Size = new Size(36, 21);
		((Control)txtTafsil).TabIndex = 152;
		((ListControl)CMBSheet).DisplayMember = "Id";
		((ComboBox)CMBSheet).DrawMode = (DrawMode)1;
		((ComboBox)CMBSheet).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CMBSheet).FormattingEnabled = true;
		((ComboBox)CMBSheet).ItemHeight = 15;
		((Control)CMBSheet).Location = new Point(3, 5);
		((Control)CMBSheet).Name = "CMBSheet";
		((Control)CMBSheet).Size = new Size(199, 21);
		CMBSheet.Style = (eDotNetBarStyle)9;
		((Control)CMBSheet).TabIndex = 4;
		((ListControl)CMBSheet).ValueMember = "Id";
		((Control)TXTFile_Address).Anchor = (AnchorStyles)13;
		TXTFile_Address.Border.Class = "TextBoxBorder";
		TXTFile_Address.Border.CornerType = (eCornerType)1;
		((Control)TXTFile_Address).Location = new Point(233, 3);
		((Control)TXTFile_Address).Name = "TXTFile_Address";
		((Control)TXTFile_Address).Size = new Size(402, 21);
		((Control)TXTFile_Address).TabIndex = 2;
		((Control)BTNSelectFile).AccessibleRole = (AccessibleRole)43;
		BTNSelectFile.ColorTable = (eButtonColor)6;
		((Control)BTNSelectFile).Location = new Point(208, 1);
		((Control)BTNSelectFile).Name = "BTNSelectFile";
		((Control)BTNSelectFile).Size = new Size(25, 25);
		((PopupItemControl)BTNSelectFile).Style = (eDotNetBarStyle)9;
		((Control)BTNSelectFile).TabIndex = 3;
		((Control)BTNSelectFile).Text = "...";
		((Control)BTNSelectFile).Click += BTNSelectFile_Click;
		((Control)Lbl_Won).Anchor = (AnchorStyles)13;
		((BaseItemControl)Lbl_Won).BackgroundStyle.Class = "";
		((BaseItemControl)Lbl_Won).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lbl_Won).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)Lbl_Won).ForeColor = Color.Blue;
		((Control)Lbl_Won).Location = new Point(649, 5);
		((Control)Lbl_Won).Name = "Lbl_Won";
		((Control)Lbl_Won).Size = new Size(112, 18);
		((Control)Lbl_Won).TabIndex = 0;
		((Control)Lbl_Won).Text = "آدرس فایل اکسل :";
		Lbl_Won.TextAlignment = (StringAlignment)2;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)13;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[43]
		{
			(DataGridViewColumn)iDDataGridViewTextBoxColumn,
			(DataGridViewColumn)groupIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)titleIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)fullCodeDataGridViewTextBoxColumn,
			(DataGridViewColumn)typeDataGridViewTextBoxColumn,
			(DataGridViewColumn)nameDataGridViewTextBoxColumn,
			(DataGridViewColumn)familyDataGridViewTextBoxColumn,
			(DataGridViewColumn)fullNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)fatherNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)dateMStartWorkDataGridViewTextBoxColumn,
			(DataGridViewColumn)dateShStartWorkDataGridViewTextBoxColumn,
			(DataGridViewColumn)dateMBirthdayDataGridViewTextBoxColumn,
			(DataGridViewColumn)dateShBirthdayDataGridViewTextBoxColumn,
			(DataGridViewColumn)codeMeliDataGridViewTextBoxColumn,
			(DataGridViewColumn)codeTafsilDataGridViewTextBoxColumn,
			(DataGridViewColumn)zipCodeDataGridViewTextBoxColumn,
			(DataGridViewColumn)mob1DataGridViewTextBoxColumn,
			(DataGridViewColumn)mob2DataGridViewTextBoxColumn,
			(DataGridViewColumn)tel1DataGridViewTextBoxColumn,
			(DataGridViewColumn)tel2DataGridViewTextBoxColumn,
			(DataGridViewColumn)adressDataGridViewTextBoxColumn,
			(DataGridViewColumn)sTDataGridViewCheckBoxColumn,
			(DataGridViewColumn)gUIDDataGridViewTextBoxColumn,
			(DataGridViewColumn)hIDDataGridViewTextBoxColumn,
			(DataGridViewColumn)hTypeDataGridViewTextBoxColumn,
			(DataGridViewColumn)picPathDataGridViewTextBoxColumn,
			(DataGridViewColumn)descriptionDataGridViewTextBoxColumn,
			(DataGridViewColumn)sodorShDataGridViewTextBoxColumn,
			(DataGridViewColumn)noBimehDataGridViewTextBoxColumn,
			(DataGridViewColumn)codeMoeinDataGridViewTextBoxColumn,
			(DataGridViewColumn)mTavalodDataGridViewTextBoxColumn,
			(DataGridViewColumn)dateMFinishWDataGridViewTextBoxColumn,
			(DataGridViewColumn)dateShFinishWDataGridViewTextBoxColumn,
			(DataGridViewColumn)eisarIdDataGridViewTextBoxColumn,
			(DataGridViewColumn)shshDataGridViewTextBoxColumn,
			(DataGridViewColumn)serialShDataGridViewTextBoxColumn,
			(DataGridViewColumn)jensiatDataGridViewTextBoxColumn,
			(DataGridViewColumn)madrakTahsiliDataGridViewTextBoxColumn,
			(DataGridViewColumn)groupNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)checkDataGridViewCheckBoxColumn,
			(DataGridViewColumn)strEnrollDatasDataGridViewTextBoxColumn,
			(DataGridViewColumn)defPersonNoDataGridViewTextBoxColumn,
			(DataGridViewColumn)defDeviceNoDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = personBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(2, 27);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((DataGridView)DGrid).ShowCellErrors = false;
		((DataGridView)DGrid).ShowRowErrors = false;
		((Control)DGrid).Size = new Size(740, 290);
		((Control)DGrid).TabIndex = 0;
		((DataGridViewColumn)iDDataGridViewTextBoxColumn).DataPropertyName = "Id";
		((DataGridViewColumn)iDDataGridViewTextBoxColumn).HeaderText = "Id";
		((DataGridViewColumn)iDDataGridViewTextBoxColumn).Name = "iDDataGridViewTextBoxColumn";
		((DataGridViewBand)iDDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)groupIdDataGridViewTextBoxColumn).DataPropertyName = "GroupId";
		((DataGridViewColumn)groupIdDataGridViewTextBoxColumn).HeaderText = "GroupId";
		((DataGridViewColumn)groupIdDataGridViewTextBoxColumn).Name = "groupIdDataGridViewTextBoxColumn";
		((DataGridViewBand)groupIdDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)titleIdDataGridViewTextBoxColumn).DataPropertyName = "TitleId";
		((DataGridViewColumn)titleIdDataGridViewTextBoxColumn).HeaderText = "TitleId";
		((DataGridViewColumn)titleIdDataGridViewTextBoxColumn).Name = "titleIdDataGridViewTextBoxColumn";
		((DataGridViewBand)titleIdDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)fullCodeDataGridViewTextBoxColumn).DataPropertyName = "FullCode";
		((DataGridViewColumn)fullCodeDataGridViewTextBoxColumn).HeaderText = "FullCode";
		((DataGridViewColumn)fullCodeDataGridViewTextBoxColumn).Name = "fullCodeDataGridViewTextBoxColumn";
		((DataGridViewBand)fullCodeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)typeDataGridViewTextBoxColumn).DataPropertyName = "Type";
		((DataGridViewColumn)typeDataGridViewTextBoxColumn).HeaderText = "Type";
		((DataGridViewColumn)typeDataGridViewTextBoxColumn).Name = "typeDataGridViewTextBoxColumn";
		((DataGridViewBand)typeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)familyDataGridViewTextBoxColumn).DataPropertyName = "Family";
		((DataGridViewColumn)familyDataGridViewTextBoxColumn).HeaderText = "Family";
		((DataGridViewColumn)familyDataGridViewTextBoxColumn).Name = "familyDataGridViewTextBoxColumn";
		((DataGridViewBand)familyDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)fullNameDataGridViewTextBoxColumn).DataPropertyName = "FullName";
		((DataGridViewColumn)fullNameDataGridViewTextBoxColumn).HeaderText = "FullName";
		((DataGridViewColumn)fullNameDataGridViewTextBoxColumn).Name = "fullNameDataGridViewTextBoxColumn";
		((DataGridViewBand)fullNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)fatherNameDataGridViewTextBoxColumn).DataPropertyName = "FatherName";
		((DataGridViewColumn)fatherNameDataGridViewTextBoxColumn).HeaderText = "FatherName";
		((DataGridViewColumn)fatherNameDataGridViewTextBoxColumn).Name = "fatherNameDataGridViewTextBoxColumn";
		((DataGridViewBand)fatherNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateMStartWorkDataGridViewTextBoxColumn).DataPropertyName = "DateMStartWork";
		((DataGridViewColumn)dateMStartWorkDataGridViewTextBoxColumn).HeaderText = "DateMStartWork";
		((DataGridViewColumn)dateMStartWorkDataGridViewTextBoxColumn).Name = "dateMStartWorkDataGridViewTextBoxColumn";
		((DataGridViewBand)dateMStartWorkDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateShStartWorkDataGridViewTextBoxColumn).DataPropertyName = "DateShStartWork";
		((DataGridViewColumn)dateShStartWorkDataGridViewTextBoxColumn).HeaderText = "DateShStartWork";
		((DataGridViewColumn)dateShStartWorkDataGridViewTextBoxColumn).Name = "dateShStartWorkDataGridViewTextBoxColumn";
		((DataGridViewBand)dateShStartWorkDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateMBirthdayDataGridViewTextBoxColumn).DataPropertyName = "DateMBirthday";
		((DataGridViewColumn)dateMBirthdayDataGridViewTextBoxColumn).HeaderText = "DateMBirthday";
		((DataGridViewColumn)dateMBirthdayDataGridViewTextBoxColumn).Name = "dateMBirthdayDataGridViewTextBoxColumn";
		((DataGridViewBand)dateMBirthdayDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateShBirthdayDataGridViewTextBoxColumn).DataPropertyName = "DateShBirthday";
		((DataGridViewColumn)dateShBirthdayDataGridViewTextBoxColumn).HeaderText = "DateShBirthday";
		((DataGridViewColumn)dateShBirthdayDataGridViewTextBoxColumn).Name = "dateShBirthdayDataGridViewTextBoxColumn";
		((DataGridViewBand)dateShBirthdayDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)codeMeliDataGridViewTextBoxColumn).DataPropertyName = "CodeMeli";
		((DataGridViewColumn)codeMeliDataGridViewTextBoxColumn).HeaderText = "CodeMeli";
		((DataGridViewColumn)codeMeliDataGridViewTextBoxColumn).Name = "codeMeliDataGridViewTextBoxColumn";
		((DataGridViewBand)codeMeliDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)codeTafsilDataGridViewTextBoxColumn).DataPropertyName = "CodeTafsil";
		((DataGridViewColumn)codeTafsilDataGridViewTextBoxColumn).HeaderText = "CodeTafsil";
		((DataGridViewColumn)codeTafsilDataGridViewTextBoxColumn).Name = "codeTafsilDataGridViewTextBoxColumn";
		((DataGridViewBand)codeTafsilDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)zipCodeDataGridViewTextBoxColumn).DataPropertyName = "ZipCode";
		((DataGridViewColumn)zipCodeDataGridViewTextBoxColumn).HeaderText = "ZipCode";
		((DataGridViewColumn)zipCodeDataGridViewTextBoxColumn).Name = "zipCodeDataGridViewTextBoxColumn";
		((DataGridViewBand)zipCodeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)mob1DataGridViewTextBoxColumn).DataPropertyName = "Mob1";
		((DataGridViewColumn)mob1DataGridViewTextBoxColumn).HeaderText = "Mob1";
		((DataGridViewColumn)mob1DataGridViewTextBoxColumn).Name = "mob1DataGridViewTextBoxColumn";
		((DataGridViewBand)mob1DataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)mob2DataGridViewTextBoxColumn).DataPropertyName = "Mob2";
		((DataGridViewColumn)mob2DataGridViewTextBoxColumn).HeaderText = "Mob2";
		((DataGridViewColumn)mob2DataGridViewTextBoxColumn).Name = "mob2DataGridViewTextBoxColumn";
		((DataGridViewBand)mob2DataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)tel1DataGridViewTextBoxColumn).DataPropertyName = "Tel1";
		((DataGridViewColumn)tel1DataGridViewTextBoxColumn).HeaderText = "Tel1";
		((DataGridViewColumn)tel1DataGridViewTextBoxColumn).Name = "tel1DataGridViewTextBoxColumn";
		((DataGridViewBand)tel1DataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)tel2DataGridViewTextBoxColumn).DataPropertyName = "Tel2";
		((DataGridViewColumn)tel2DataGridViewTextBoxColumn).HeaderText = "Tel2";
		((DataGridViewColumn)tel2DataGridViewTextBoxColumn).Name = "tel2DataGridViewTextBoxColumn";
		((DataGridViewBand)tel2DataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)adressDataGridViewTextBoxColumn).DataPropertyName = "Address";
		((DataGridViewColumn)adressDataGridViewTextBoxColumn).HeaderText = "Address";
		((DataGridViewColumn)adressDataGridViewTextBoxColumn).Name = "adressDataGridViewTextBoxColumn";
		((DataGridViewBand)adressDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).DataPropertyName = "St";
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).HeaderText = "St";
		((DataGridViewColumn)sTDataGridViewCheckBoxColumn).Name = "sTDataGridViewCheckBoxColumn";
		((DataGridViewBand)sTDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewColumn)gUIDDataGridViewTextBoxColumn).DataPropertyName = "Guid";
		((DataGridViewColumn)gUIDDataGridViewTextBoxColumn).HeaderText = "Guid";
		((DataGridViewColumn)gUIDDataGridViewTextBoxColumn).Name = "gUIDDataGridViewTextBoxColumn";
		((DataGridViewBand)gUIDDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)hIDDataGridViewTextBoxColumn).DataPropertyName = "Hid";
		((DataGridViewColumn)hIDDataGridViewTextBoxColumn).HeaderText = "Hid";
		((DataGridViewColumn)hIDDataGridViewTextBoxColumn).Name = "hIDDataGridViewTextBoxColumn";
		((DataGridViewBand)hIDDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)hTypeDataGridViewTextBoxColumn).DataPropertyName = "HType";
		((DataGridViewColumn)hTypeDataGridViewTextBoxColumn).HeaderText = "HType";
		((DataGridViewColumn)hTypeDataGridViewTextBoxColumn).Name = "hTypeDataGridViewTextBoxColumn";
		((DataGridViewBand)hTypeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)picPathDataGridViewTextBoxColumn).DataPropertyName = "PicPath";
		((DataGridViewColumn)picPathDataGridViewTextBoxColumn).HeaderText = "PicPath";
		((DataGridViewColumn)picPathDataGridViewTextBoxColumn).Name = "picPathDataGridViewTextBoxColumn";
		((DataGridViewBand)picPathDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).DataPropertyName = "Description";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).HeaderText = "Description";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).Name = "descriptionDataGridViewTextBoxColumn";
		((DataGridViewBand)descriptionDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)sodorShDataGridViewTextBoxColumn).DataPropertyName = "SodorSh";
		((DataGridViewColumn)sodorShDataGridViewTextBoxColumn).HeaderText = "SodorSh";
		((DataGridViewColumn)sodorShDataGridViewTextBoxColumn).Name = "sodorShDataGridViewTextBoxColumn";
		((DataGridViewBand)sodorShDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)noBimehDataGridViewTextBoxColumn).DataPropertyName = "NoBimeh";
		((DataGridViewColumn)noBimehDataGridViewTextBoxColumn).HeaderText = "NoBimeh";
		((DataGridViewColumn)noBimehDataGridViewTextBoxColumn).Name = "noBimehDataGridViewTextBoxColumn";
		((DataGridViewBand)noBimehDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)codeMoeinDataGridViewTextBoxColumn).DataPropertyName = "CodeMoein";
		((DataGridViewColumn)codeMoeinDataGridViewTextBoxColumn).HeaderText = "CodeMoein";
		((DataGridViewColumn)codeMoeinDataGridViewTextBoxColumn).Name = "codeMoeinDataGridViewTextBoxColumn";
		((DataGridViewBand)codeMoeinDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)mTavalodDataGridViewTextBoxColumn).DataPropertyName = "MTavalod";
		((DataGridViewColumn)mTavalodDataGridViewTextBoxColumn).HeaderText = "MTavalod";
		((DataGridViewColumn)mTavalodDataGridViewTextBoxColumn).Name = "mTavalodDataGridViewTextBoxColumn";
		((DataGridViewBand)mTavalodDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateMFinishWDataGridViewTextBoxColumn).DataPropertyName = "DateMFinishW";
		((DataGridViewColumn)dateMFinishWDataGridViewTextBoxColumn).HeaderText = "DateMFinishW";
		((DataGridViewColumn)dateMFinishWDataGridViewTextBoxColumn).Name = "dateMFinishWDataGridViewTextBoxColumn";
		((DataGridViewBand)dateMFinishWDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateShFinishWDataGridViewTextBoxColumn).DataPropertyName = "DateShFinishW";
		((DataGridViewColumn)dateShFinishWDataGridViewTextBoxColumn).HeaderText = "DateShFinishW";
		((DataGridViewColumn)dateShFinishWDataGridViewTextBoxColumn).Name = "dateShFinishWDataGridViewTextBoxColumn";
		((DataGridViewBand)dateShFinishWDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)eisarIdDataGridViewTextBoxColumn).DataPropertyName = "EisarId";
		((DataGridViewColumn)eisarIdDataGridViewTextBoxColumn).HeaderText = "EisarId";
		((DataGridViewColumn)eisarIdDataGridViewTextBoxColumn).Name = "eisarIdDataGridViewTextBoxColumn";
		((DataGridViewBand)eisarIdDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)shshDataGridViewTextBoxColumn).DataPropertyName = "Shsh";
		((DataGridViewColumn)shshDataGridViewTextBoxColumn).HeaderText = "Shsh";
		((DataGridViewColumn)shshDataGridViewTextBoxColumn).Name = "shshDataGridViewTextBoxColumn";
		((DataGridViewBand)shshDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)serialShDataGridViewTextBoxColumn).DataPropertyName = "SerialSh";
		((DataGridViewColumn)serialShDataGridViewTextBoxColumn).HeaderText = "SerialSh";
		((DataGridViewColumn)serialShDataGridViewTextBoxColumn).Name = "serialShDataGridViewTextBoxColumn";
		((DataGridViewBand)serialShDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)jensiatDataGridViewTextBoxColumn).DataPropertyName = "Jensiat";
		((DataGridViewColumn)jensiatDataGridViewTextBoxColumn).HeaderText = "Jensiat";
		((DataGridViewColumn)jensiatDataGridViewTextBoxColumn).Name = "jensiatDataGridViewTextBoxColumn";
		((DataGridViewBand)jensiatDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)madrakTahsiliDataGridViewTextBoxColumn).DataPropertyName = "MadrakTahsili";
		((DataGridViewColumn)madrakTahsiliDataGridViewTextBoxColumn).HeaderText = "MadrakTahsili";
		((DataGridViewColumn)madrakTahsiliDataGridViewTextBoxColumn).Name = "madrakTahsiliDataGridViewTextBoxColumn";
		((DataGridViewBand)madrakTahsiliDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)groupNameDataGridViewTextBoxColumn).DataPropertyName = "GroupName";
		((DataGridViewColumn)groupNameDataGridViewTextBoxColumn).HeaderText = "GroupName";
		((DataGridViewColumn)groupNameDataGridViewTextBoxColumn).Name = "groupNameDataGridViewTextBoxColumn";
		((DataGridViewBand)groupNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)checkDataGridViewCheckBoxColumn).DataPropertyName = "Check";
		((DataGridViewColumn)checkDataGridViewCheckBoxColumn).HeaderText = "Check";
		((DataGridViewColumn)checkDataGridViewCheckBoxColumn).Name = "checkDataGridViewCheckBoxColumn";
		((DataGridViewBand)checkDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewColumn)strEnrollDatasDataGridViewTextBoxColumn).DataPropertyName = "StrEnrollDatas";
		((DataGridViewColumn)strEnrollDatasDataGridViewTextBoxColumn).HeaderText = "StrEnrollDatas";
		((DataGridViewColumn)strEnrollDatasDataGridViewTextBoxColumn).Name = "strEnrollDatasDataGridViewTextBoxColumn";
		((DataGridViewBand)strEnrollDatasDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)defPersonNoDataGridViewTextBoxColumn).DataPropertyName = "DefPersonNo";
		((DataGridViewColumn)defPersonNoDataGridViewTextBoxColumn).HeaderText = "DefPersonNo";
		((DataGridViewColumn)defPersonNoDataGridViewTextBoxColumn).Name = "defPersonNoDataGridViewTextBoxColumn";
		((DataGridViewBand)defPersonNoDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)defDeviceNoDataGridViewTextBoxColumn).DataPropertyName = "DefDeviceNo";
		((DataGridViewColumn)defDeviceNoDataGridViewTextBoxColumn).HeaderText = "DefDeviceNo";
		((DataGridViewColumn)defDeviceNoDataGridViewTextBoxColumn).Name = "defDeviceNoDataGridViewTextBoxColumn";
		((DataGridViewBand)defDeviceNoDataGridViewTextBoxColumn).ReadOnly = true;
		personBindingSource.DataSource = typeof(TimeAttendanceDAL.Dal.FLRPerson.Person);
		((Control)BTNAdd_List).AccessibleRole = (AccessibleRole)43;
		((Control)BTNAdd_List).Anchor = (AnchorStyles)14;
		BTNAdd_List.ColorTable = (eButtonColor)6;
		((Control)BTNAdd_List).Location = new Point(3, 505);
		((Control)BTNAdd_List).Name = "BTNAdd_List";
		((Control)BTNAdd_List).Size = new Size(740, 33);
		((Control)BTNAdd_List).TabIndex = 9;
		((Control)BTNAdd_List).TabStop = false;
		((Control)BTNAdd_List).Text = "افزودن به لیست اشــخــاص";
		((Control)BTNAdd_List).Click += BTNAdd_List_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Form)this).ClientSize = new Size(794, 571);
		((Control)this).Controls.Add((Control)(object)PanelEx1_NOTSend);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmAddExcelToPerson";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "افزودن گروهی کالاها از فایل Excel";
		((Form)this).Load += FrmAddExcelToPerson_Load;
		((Control)PanelEx1_NOTSend).ResumeLayout(false);
		((Control)GpStatuse).ResumeLayout(false);
		((Control)Panel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)personBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

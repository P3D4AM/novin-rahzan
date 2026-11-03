using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FKAttendDllCSSample;

public class frmPostShiftInfo : Form
{
	private byte[] mbytShiftInfo = new byte[2476];

	private byte[] mbytUserInfo = new byte[184];

	private POST_SHIFT_INFO mPostShiftInfo;

	private USER_INFO mUserInfo;

	private const string mcstrAdoConn = "Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=false;Data Source=";

	private OleDbConnection mConn = new OleDbConnection();

	private IContainer components = null;

	public ToolTip ToolTip1;

	public Button cmdGetShiftInfo;

	public Label Label3;

	public Label Label1;

	public Button cmdSetShiftInfo;

	internal DataGridView dgvUserShiftOfMonth;

	public TextBox txtEnrollNumber;

	public Label Label102;

	public TextBox txtPostID;

	public OpenFileDialog dlgOpen;

	public TextBox txtUserName;

	public Button cmdGetUserInfo;

	public TextBox txtCompanyName;

	public Button cmdSetUserInfo;

	internal DataGridView dgvPost;

	internal DataGridView dgvShift;

	public GroupBox Frame1;

	public Label Label99;

	public Label Label2;

	public GroupBox frmPostGroup;

	public Label lblMessage;

	public Label label4;

	private ComboBox cmbUserShiftYear;

	private ComboBox cmbUserShiftMonth;

	public Label label5;

	public frmPostShiftInfo()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		InitializeComponent();
	}

	private async Task GetPostShiftInfoAsync(CancellationToken token = default(CancellationToken))
	{
		FKAttendDLL.StringToByteArrayUtf16(((Control)txtCompanyName).Text, mPostShiftInfo.CompanyName);
		for (int j = 1; j <= 24; j++)
		{
			mPostShiftInfo.ShiftTime[j - 1].AMStartH = Convert.ToByte(dgvShift.Rows[j - 1].Cells[1].Value);
			mPostShiftInfo.ShiftTime[j - 1].AMStartM = Convert.ToByte(dgvShift.Rows[j - 1].Cells[2].Value);
			mPostShiftInfo.ShiftTime[j - 1].AMEndH = Convert.ToByte(dgvShift.Rows[j - 1].Cells[3].Value);
			mPostShiftInfo.ShiftTime[j - 1].AMEndM = Convert.ToByte(dgvShift.Rows[j - 1].Cells[4].Value);
			mPostShiftInfo.ShiftTime[j - 1].PMStartH = Convert.ToByte(dgvShift.Rows[j - 1].Cells[5].Value);
			mPostShiftInfo.ShiftTime[j - 1].PMStartM = Convert.ToByte(dgvShift.Rows[j - 1].Cells[6].Value);
			mPostShiftInfo.ShiftTime[j - 1].PMEndH = Convert.ToByte(dgvShift.Rows[j - 1].Cells[7].Value);
			mPostShiftInfo.ShiftTime[j - 1].PMEndM = Convert.ToByte(dgvShift.Rows[j - 1].Cells[8].Value);
			mPostShiftInfo.ShiftTime[j - 1].OVStartH = Convert.ToByte(dgvShift.Rows[j - 1].Cells[9].Value);
			mPostShiftInfo.ShiftTime[j - 1].OVStartM = Convert.ToByte(dgvShift.Rows[j - 1].Cells[10].Value);
			mPostShiftInfo.ShiftTime[j - 1].OVEndH = Convert.ToByte(dgvShift.Rows[j - 1].Cells[11].Value);
			mPostShiftInfo.ShiftTime[j - 1].OVEndM = Convert.ToByte(dgvShift.Rows[j - 1].Cells[12].Value);
		}
		for (int j = 1; j <= 16; j++)
		{
			string strPost;
			try
			{
				strPost = Convert.ToString(dgvPost.Rows[j - 1].Cells[1].Value);
			}
			catch (Exception)
			{
				strPost = "";
			}
			FKAttendDLL.StringToByteArrayUtf16(strPost, mPostShiftInfo.PostInfo[j - 1].PostName);
		}
		await SavePostInfoToDbAsync(token);
		await SaveShiftInfoToDbAsync(token);
	}

	private void RefreshPostShiftInfoFromDb()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		DataSet dataSet = new DataSet();
		DataSet dataSet2 = new DataSet();
		OleDbDataAdapter val = null;
		if (((DbConnection)(object)mConn).State == ConnectionState.Open)
		{
			val = new OleDbDataAdapter("select * from tblPostName", mConn);
			((DataAdapter)(object)val).Fill(dataSet);
			dgvPost.DataSource = dataSet.Tables[0];
			val = new OleDbDataAdapter("select * from tblShiftTime", mConn);
			((DataAdapter)(object)val).Fill(dataSet2);
			dgvShift.DataSource = dataSet2.Tables[0];
		}
	}

	private async Task SavePostInfoToDbAsync(CancellationToken token = default(CancellationToken))
	{
		OleDbCommand cmd = new OleDbCommand();
		if (((DbConnection)(object)mConn).State != ConnectionState.Open)
		{
			return;
		}
		try
		{
			cmd.Connection = mConn;
			((DbCommand)(object)cmd).CommandText = "DELETE FROM tblPostName";
			await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			for (int i = 1; i <= 16; i++)
			{
				string strPost = Convert.ToString(FKAttendDLL.ByteArrayUtf16ToString(mPostShiftInfo.PostInfo[i - 1].PostName));
				if (strPost.Length > 0)
				{
					((DbCommand)(object)cmd).CommandText = "INSERT INTO tblPostName([No], PostName)  VALUES(" + Convert.ToString(i) + ", '" + strPost + "')";
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
				}
				else
				{
					((DbCommand)(object)cmd).CommandText = "INSERT INTO tblPostName([No], PostName)  VALUES(" + Convert.ToString(i) + ", '')";
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private async Task SaveShiftInfoToDbAsync(CancellationToken token = default(CancellationToken))
	{
		OleDbCommand cmd = new OleDbCommand();
		if (((DbConnection)(object)mConn).State != ConnectionState.Open)
		{
			return;
		}
		try
		{
			cmd.Connection = mConn;
			((DbCommand)(object)cmd).CommandText = "DELETE FROM tblShiftTime";
			await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			for (int i = 1; i <= 24; i++)
			{
				((DbCommand)(object)cmd).CommandText = "INSERT INTO tblShiftTime(ShiftNo, TimeSH1, TimeSM1, TimeEH1, TimeEM1, TimeSH2, TimeSM2, TimeEH2, TimeEM2, TimeSH3, TimeSM3, TimeEH3, TimeEM3)  VALUES(" + Convert.ToString(i) + ", " + mPostShiftInfo.ShiftTime[i - 1].AMStartH + ", " + mPostShiftInfo.ShiftTime[i - 1].AMStartM + ", " + mPostShiftInfo.ShiftTime[i - 1].AMEndH + ", " + mPostShiftInfo.ShiftTime[i - 1].AMEndM + ", " + mPostShiftInfo.ShiftTime[i - 1].PMStartH + ", " + mPostShiftInfo.ShiftTime[i - 1].PMStartM + ", " + mPostShiftInfo.ShiftTime[i - 1].PMEndH + ", " + mPostShiftInfo.ShiftTime[i - 1].PMEndM + ", " + mPostShiftInfo.ShiftTime[i - 1].OVStartH + ", " + mPostShiftInfo.ShiftTime[i - 1].OVStartM + ", " + mPostShiftInfo.ShiftTime[i - 1].OVEndH + ", " + mPostShiftInfo.ShiftTime[i - 1].OVEndM + ")";
				await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			}
		}
		catch (Exception)
		{
		}
	}

	private async Task ShowPostShiftInfoAsync(CancellationToken token = default(CancellationToken))
	{
		await SavePostInfoToDbAsync(token);
		await SaveShiftInfoToDbAsync(token);
		((Control)txtCompanyName).Text = FKAttendDLL.ByteArrayUtf16ToString(mPostShiftInfo.CompanyName);
		RefreshPostShiftInfoFromDb();
	}

	private void SetupGridUserShiftOfMonth()
	{
		int num = 0;
		string[] array = new string[31];
		dgvUserShiftOfMonth.ColumnCount = 31;
		for (num = 1; num <= 31; num++)
		{
			dgvUserShiftOfMonth.Columns[num - 1].Name = Convert.ToString(num) ?? "";
			dgvUserShiftOfMonth.Columns[num - 1].Width = 24;
			array[num - 1] = Convert.ToString(0) ?? "";
		}
		DataGridViewRowCollection rows = dgvUserShiftOfMonth.Rows;
		object[] array2 = array;
		rows.Add(array2);
	}

	private int GetShiftNumOfDayOfUser(int aDay)
	{
		if (aDay < 1 || aDay > 31)
		{
			return 0;
		}
		return Convert.ToInt32(dgvUserShiftOfMonth.Rows[0].Cells[aDay - 1].Value);
	}

	private void SetShiftNumOfDayOfUser(int aDay, int aShiftNumber)
	{
		if (aDay >= 1 && aDay <= 31)
		{
			dgvUserShiftOfMonth.Rows[0].Cells[aDay - 1].Value = Convert.ToString(aShiftNumber) ?? "";
		}
	}

	private void cmdGetUserInfo_Click(object sender, EventArgs e)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		((Control)cmdGetUserInfo).Enabled = false;
		((Control)lblMessage).Text = "Getting UserInfo ...";
		Application.DoEvents();
		num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetUserInfo).Enabled = true;
			return;
		}
		num2 = (int)Convert.ToUInt32(((Control)txtEnrollNumber).Text);
		mUserInfo.YearAssigned = Convert.ToInt16(((Control)cmbUserShiftYear).Text);
		mUserInfo.MonthAssigned = Convert.ToByte(((Control)cmbUserShiftMonth).Text);
		FKAttendDLL.ConvertStructureToByteArray(mUserInfo, mbytUserInfo);
		num3 = 184;
		num = FKAttendDLL.FK_GetUserInfoEx(FKAttendDLL.nCommHandleIndex, num2, mbytUserInfo, ref num3);
		if (num == 1)
		{
			mUserInfo = (USER_INFO)FKAttendDLL.ConvertByteArrayToStructure(mbytUserInfo, typeof(USER_INFO));
			((Control)txtUserName).Text = FKAttendDLL.ByteArrayUtf16ToString(mUserInfo.UserName);
			((Control)txtPostID).Text = mUserInfo.PostId.ToString();
			for (int i = 1; i <= 31; i++)
			{
				SetShiftNumOfDayOfUser(i, mUserInfo.ShiftId[i - 1]);
			}
			((Control)lblMessage).Text = "EnrollNumber=" + num2 + "  Message=OK";
		}
		else
		{
			((Control)lblMessage).Text = "EnrollNumber=" + num2 + "  Message=" + FKAttendDLL.ReturnResultPrint(num);
		}
		Application.DoEvents();
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetUserInfo).Enabled = true;
	}

	private void cmdSetUserInfo_Click(object sender, EventArgs e)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		((Control)cmdSetUserInfo).Enabled = false;
		((Control)lblMessage).Text = "Setting UserInfo ...";
		Application.DoEvents();
		num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetUserInfo).Enabled = true;
			return;
		}
		mUserInfo.Size = 184;
		mUserInfo.Ver = 2;
		num2 = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		mUserInfo.UserId = num2;
		FKAttendDLL.StringToByteArrayUtf16(Convert.ToString(((Control)txtUserName).Text), mUserInfo.UserName);
		mUserInfo.PostId = Convert.ToByte(((Control)txtPostID).Text);
		mUserInfo.YearAssigned = Convert.ToInt16(((Control)cmbUserShiftYear).Text);
		mUserInfo.MonthAssigned = Convert.ToByte(((Control)cmbUserShiftMonth).Text);
		for (int i = 1; i <= 31; i++)
		{
			mUserInfo.ShiftId[i - 1] = Convert.ToByte(GetShiftNumOfDayOfUser(i));
		}
		num3 = 184;
		FKAttendDLL.ConvertStructureToByteArray(mUserInfo, mbytUserInfo);
		num = FKAttendDLL.FK_SetUserInfoEx(FKAttendDLL.nCommHandleIndex, num2, mbytUserInfo, num3);
		if (num == 1)
		{
			((Control)lblMessage).Text = "EnrollNumber=" + num2 + "  Message=OK";
		}
		else
		{
			((Control)lblMessage).Text = "EnrollNumber=" + num2 + "  Message=" + FKAttendDLL.ReturnResultPrint(num);
		}
		Application.DoEvents();
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetUserInfo).Enabled = true;
	}

	private async void cmdGetShiftInfo_Click(object sender, EventArgs e)
	{
		((Control)cmdGetShiftInfo).Enabled = false;
		((Control)lblMessage).Text = "Getting ShiftInfo ...";
		int vnResultCode2 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (vnResultCode2 != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetShiftInfo).Enabled = true;
			return;
		}
		int vShiftLen = 2476;
		vnResultCode2 = FKAttendDLL.FK_GetPostShiftInfo(FKAttendDLL.nCommHandleIndex, mbytShiftInfo, ref vShiftLen);
		if (vnResultCode2 == 1)
		{
			mPostShiftInfo = (POST_SHIFT_INFO)FKAttendDLL.ConvertByteArrayToStructure(mbytShiftInfo, typeof(POST_SHIFT_INFO));
			await ShowPostShiftInfoAsync();
			((Control)lblMessage).Text = "Message=OK";
		}
		else
		{
			((Control)lblMessage).Text = "Message=" + FKAttendDLL.ReturnResultPrint(vnResultCode2);
		}
		Application.DoEvents();
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetShiftInfo).Enabled = true;
	}

	private async void cmdSetShiftInfo_Click(object sender, EventArgs e)
	{
		((Control)cmdSetShiftInfo).Enabled = false;
		((Control)lblMessage).Text = "Setting ShiftInfo ...";
		Application.DoEvents();
		int vnResultCode2 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (vnResultCode2 != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetShiftInfo).Enabled = true;
			return;
		}
		await GetPostShiftInfoAsync();
		int vnShiftLen = 2476;
		mPostShiftInfo.Size = 2476;
		mPostShiftInfo.Ver = 2;
		if (vnResultCode2 == 1)
		{
			FKAttendDLL.ConvertStructureToByteArray(mPostShiftInfo, mbytShiftInfo);
		}
		vnResultCode2 = FKAttendDLL.FK_SetPostShiftInfo(FKAttendDLL.nCommHandleIndex, mbytShiftInfo, vnShiftLen);
		if (vnResultCode2 == 1)
		{
			((Control)lblMessage).Text = "Message=OK";
		}
		else
		{
			((Control)lblMessage).Text = "Message=" + FKAttendDLL.ReturnResultPrint(vnResultCode2);
		}
		Application.DoEvents();
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdSetShiftInfo).Enabled = true;
	}

	private void frmPostShiftInfo_Load(object sender, EventArgs e)
	{
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			mPostShiftInfo.Init();
			mUserInfo.Init();
			string text = "";
			SetupGridUserShiftOfMonth();
			for (int i = 0; i <= 20; i++)
			{
				cmbUserShiftYear.Items.Add((object)Convert.ToString(2005 + i));
			}
			cmbUserShiftYear.SelectedText = Convert.ToString(DateTime.Now.Year);
			for (int i = 1; i <= 12; i++)
			{
				cmbUserShiftMonth.Items.Add((object)Convert.ToString(i));
			}
			cmbUserShiftMonth.SelectedText = Convert.ToString(DateTime.Now.Month);
			text = Directory.GetCurrentDirectory() + "\\datEnrollDat.mdb";
			if (!File.Exists(text))
			{
				((FileDialog)dlgOpen).InitialDirectory = Directory.GetCurrentDirectory();
				dlgOpen.ShowReadOnly = false;
				((FileDialog)dlgOpen).Filter = "DB Files (*.mdb)|*.mdb|All Files (*.*)|*.*";
				((FileDialog)dlgOpen).FilterIndex = 1;
				((CommonDialog)dlgOpen).ShowDialog();
				text = Convert.ToString(((FileDialog)dlgOpen).FileName);
				((FileDialog)dlgOpen).FileName = null;
				if (text.Length <= 0 || !File.Exists(text))
				{
					return;
				}
			}
			((DbConnection)(object)mConn).ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=false;Data Source=" + text;
			((DbConnection)(object)mConn).Open();
			if (((DbConnection)(object)mConn).State == ConnectionState.Open)
			{
				RefreshPostShiftInfoFromDb();
			}
		}
		catch (Exception)
		{
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
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
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Expected O, but got Unknown
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Expected O, but got Unknown
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Expected O, but got Unknown
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ab: Expected O, but got Unknown
		//IL_0a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a67: Expected O, but got Unknown
		//IL_0d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d76: Expected O, but got Unknown
		//IL_0db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f08: Expected O, but got Unknown
		//IL_0fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd0: Expected O, but got Unknown
		//IL_109b: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a5: Expected O, but got Unknown
		//IL_1170: Unknown result type (might be due to invalid IL or missing references)
		//IL_117a: Expected O, but got Unknown
		//IL_12a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ae: Expected O, but got Unknown
		//IL_12ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_138e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1398: Expected O, but got Unknown
		components = new Container();
		ToolTip1 = new ToolTip(components);
		cmdGetShiftInfo = new Button();
		Label3 = new Label();
		Label1 = new Label();
		cmdSetShiftInfo = new Button();
		dgvUserShiftOfMonth = new DataGridView();
		txtEnrollNumber = new TextBox();
		Label102 = new Label();
		txtPostID = new TextBox();
		dlgOpen = new OpenFileDialog();
		txtUserName = new TextBox();
		cmdGetUserInfo = new Button();
		txtCompanyName = new TextBox();
		cmdSetUserInfo = new Button();
		dgvPost = new DataGridView();
		dgvShift = new DataGridView();
		Frame1 = new GroupBox();
		cmbUserShiftMonth = new ComboBox();
		cmbUserShiftYear = new ComboBox();
		label5 = new Label();
		label4 = new Label();
		Label99 = new Label();
		Label2 = new Label();
		frmPostGroup = new GroupBox();
		lblMessage = new Label();
		((ISupportInitialize)dgvUserShiftOfMonth).BeginInit();
		((ISupportInitialize)dgvPost).BeginInit();
		((ISupportInitialize)dgvShift).BeginInit();
		((Control)Frame1).SuspendLayout();
		((Control)frmPostGroup).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)cmdGetShiftInfo).BackColor = SystemColors.Control;
		((Control)cmdGetShiftInfo).Cursor = Cursors.Default;
		((Control)cmdGetShiftInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetShiftInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdGetShiftInfo).Location = new Point(435, 380);
		((Control)cmdGetShiftInfo).Name = "cmdGetShiftInfo";
		((Control)cmdGetShiftInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdGetShiftInfo).Size = new Size(154, 30);
		((Control)cmdGetShiftInfo).TabIndex = 1;
		((Control)cmdGetShiftInfo).Text = "Get Shift Info";
		((ButtonBase)cmdGetShiftInfo).UseVisualStyleBackColor = false;
		((Control)cmdGetShiftInfo).Click += cmdGetShiftInfo_Click;
		((Control)Label3).BackColor = SystemColors.Control;
		((Control)Label3).Cursor = Cursors.Default;
		((Control)Label3).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label3).ForeColor = SystemColors.ControlText;
		((Control)Label3).Location = new Point(25, 106);
		((Control)Label3).Name = "Label3";
		((Control)Label3).RightToLeft = (RightToLeft)0;
		((Control)Label3).Size = new Size(186, 19);
		((Control)Label3).TabIndex = 47;
		((Control)Label3).Text = "Shift setting in a month :";
		Label3.TextAlign = (ContentAlignment)16;
		((Control)Label1).BackColor = SystemColors.Control;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = SystemColors.ControlText;
		((Control)Label1).Location = new Point(148, 83);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		((Control)Label1).Size = new Size(90, 19);
		((Control)Label1).TabIndex = 47;
		((Control)Label1).Text = "PostID :";
		Label1.TextAlign = (ContentAlignment)4;
		((Control)cmdSetShiftInfo).BackColor = SystemColors.Control;
		((Control)cmdSetShiftInfo).Cursor = Cursors.Default;
		((Control)cmdSetShiftInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetShiftInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdSetShiftInfo).Location = new Point(597, 380);
		((Control)cmdSetShiftInfo).Name = "cmdSetShiftInfo";
		((Control)cmdSetShiftInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdSetShiftInfo).Size = new Size(154, 30);
		((Control)cmdSetShiftInfo).TabIndex = 2;
		((Control)cmdSetShiftInfo).Text = "Set Shift Info";
		((ButtonBase)cmdSetShiftInfo).UseVisualStyleBackColor = false;
		((Control)cmdSetShiftInfo).Click += cmdSetShiftInfo_Click;
		dgvUserShiftOfMonth.AllowUserToAddRows = false;
		dgvUserShiftOfMonth.AllowUserToDeleteRows = false;
		dgvUserShiftOfMonth.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((Control)dgvUserShiftOfMonth).Location = new Point(25, 128);
		((Control)dgvUserShiftOfMonth).Name = "dgvUserShiftOfMonth";
		dgvUserShiftOfMonth.RowHeadersVisible = false;
		((Control)dgvUserShiftOfMonth).Size = new Size(785, 47);
		((Control)dgvUserShiftOfMonth).TabIndex = 46;
		txtEnrollNumber.AcceptsReturn = true;
		((Control)txtEnrollNumber).BackColor = SystemColors.Window;
		((Control)txtEnrollNumber).Cursor = Cursors.IBeam;
		((Control)txtEnrollNumber).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtEnrollNumber).ForeColor = SystemColors.WindowText;
		((Control)txtEnrollNumber).Location = new Point(246, 22);
		((TextBoxBase)txtEnrollNumber).MaxLength = 0;
		((Control)txtEnrollNumber).Name = "txtEnrollNumber";
		((Control)txtEnrollNumber).RightToLeft = (RightToLeft)0;
		((Control)txtEnrollNumber).Size = new Size(142, 26);
		((Control)txtEnrollNumber).TabIndex = 16;
		((Control)txtEnrollNumber).Text = "1";
		txtEnrollNumber.TextAlign = (HorizontalAlignment)2;
		((Control)Label102).BackColor = SystemColors.Control;
		((Control)Label102).Cursor = Cursors.Default;
		((Control)Label102).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label102).ForeColor = SystemColors.ControlText;
		((Control)Label102).Location = new Point(138, 384);
		((Control)Label102).Name = "Label102";
		((Control)Label102).RightToLeft = (RightToLeft)0;
		((Control)Label102).Size = new Size(107, 19);
		((Control)Label102).TabIndex = 6;
		((Control)Label102).Text = "Company Name";
		txtPostID.AcceptsReturn = true;
		((Control)txtPostID).BackColor = SystemColors.Window;
		((Control)txtPostID).Cursor = Cursors.IBeam;
		((Control)txtPostID).Font = new Font("Times New Roman", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtPostID).ForeColor = SystemColors.WindowText;
		((Control)txtPostID).Location = new Point(246, 82);
		((TextBoxBase)txtPostID).MaxLength = 0;
		((Control)txtPostID).Name = "txtPostID";
		((Control)txtPostID).RightToLeft = (RightToLeft)0;
		((Control)txtPostID).Size = new Size(79, 20);
		((Control)txtPostID).TabIndex = 15;
		txtPostID.TextAlign = (HorizontalAlignment)2;
		txtUserName.AcceptsReturn = true;
		((Control)txtUserName).BackColor = SystemColors.Window;
		((Control)txtUserName).Cursor = Cursors.IBeam;
		((Control)txtUserName).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtUserName).ForeColor = SystemColors.WindowText;
		((Control)txtUserName).Location = new Point(246, 52);
		((TextBoxBase)txtUserName).MaxLength = 0;
		((Control)txtUserName).Name = "txtUserName";
		((Control)txtUserName).RightToLeft = (RightToLeft)0;
		((Control)txtUserName).Size = new Size(124, 26);
		((Control)txtUserName).TabIndex = 14;
		((Control)cmdGetUserInfo).BackColor = SystemColors.Control;
		((Control)cmdGetUserInfo).Cursor = Cursors.Default;
		((Control)cmdGetUserInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetUserInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdGetUserInfo).Location = new Point(436, 22);
		((Control)cmdGetUserInfo).Name = "cmdGetUserInfo";
		((Control)cmdGetUserInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdGetUserInfo).Size = new Size(154, 35);
		((Control)cmdGetUserInfo).TabIndex = 8;
		((Control)cmdGetUserInfo).Text = "Get UserInfo";
		((ButtonBase)cmdGetUserInfo).UseVisualStyleBackColor = false;
		((Control)cmdGetUserInfo).Click += cmdGetUserInfo_Click;
		txtCompanyName.AcceptsReturn = true;
		((Control)txtCompanyName).BackColor = SystemColors.Window;
		((Control)txtCompanyName).Cursor = Cursors.IBeam;
		((Control)txtCompanyName).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)txtCompanyName).ForeColor = SystemColors.WindowText;
		((Control)txtCompanyName).Location = new Point(257, 383);
		((TextBoxBase)txtCompanyName).MaxLength = 0;
		((Control)txtCompanyName).Name = "txtCompanyName";
		((Control)txtCompanyName).RightToLeft = (RightToLeft)0;
		((Control)txtCompanyName).Size = new Size(170, 26);
		((Control)txtCompanyName).TabIndex = 3;
		((Control)cmdSetUserInfo).BackColor = SystemColors.Control;
		((Control)cmdSetUserInfo).Cursor = Cursors.Default;
		((Control)cmdSetUserInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetUserInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdSetUserInfo).Location = new Point(596, 22);
		((Control)cmdSetUserInfo).Name = "cmdSetUserInfo";
		((Control)cmdSetUserInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdSetUserInfo).Size = new Size(154, 35);
		((Control)cmdSetUserInfo).TabIndex = 9;
		((Control)cmdSetUserInfo).Text = "Set UserInfo";
		((ButtonBase)cmdSetUserInfo).UseVisualStyleBackColor = false;
		((Control)cmdSetUserInfo).Click += cmdSetUserInfo_Click;
		dgvPost.AllowUserToAddRows = false;
		dgvPost.AllowUserToDeleteRows = false;
		dgvPost.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((Control)dgvPost).Location = new Point(24, 32);
		((Control)dgvPost).Name = "dgvPost";
		dgvPost.RowHeadersWidth = 20;
		((Control)dgvPost).Size = new Size(258, 331);
		((Control)dgvPost).TabIndex = 7;
		dgvShift.AllowUserToAddRows = false;
		dgvShift.AllowUserToDeleteRows = false;
		dgvShift.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((Control)dgvShift).Location = new Point(291, 32);
		((Control)dgvShift).Name = "dgvShift";
		((Control)dgvShift).Size = new Size(518, 331);
		((Control)dgvShift).TabIndex = 8;
		((Control)Frame1).BackColor = SystemColors.Control;
		((Control)Frame1).Controls.Add((Control)(object)cmbUserShiftMonth);
		((Control)Frame1).Controls.Add((Control)(object)cmbUserShiftYear);
		((Control)Frame1).Controls.Add((Control)(object)label5);
		((Control)Frame1).Controls.Add((Control)(object)Label3);
		((Control)Frame1).Controls.Add((Control)(object)label4);
		((Control)Frame1).Controls.Add((Control)(object)Label1);
		((Control)Frame1).Controls.Add((Control)(object)dgvUserShiftOfMonth);
		((Control)Frame1).Controls.Add((Control)(object)txtEnrollNumber);
		((Control)Frame1).Controls.Add((Control)(object)txtPostID);
		((Control)Frame1).Controls.Add((Control)(object)txtUserName);
		((Control)Frame1).Controls.Add((Control)(object)cmdSetUserInfo);
		((Control)Frame1).Controls.Add((Control)(object)cmdGetUserInfo);
		((Control)Frame1).Controls.Add((Control)(object)Label99);
		((Control)Frame1).Controls.Add((Control)(object)Label2);
		((Control)Frame1).Font = new Font("Times New Roman", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Frame1).ForeColor = SystemColors.ControlText;
		((Control)Frame1).Location = new Point(20, 465);
		((Control)Frame1).Name = "Frame1";
		((Control)Frame1).Padding = new Padding(0);
		((Control)Frame1).RightToLeft = (RightToLeft)0;
		((Control)Frame1).Size = new Size(831, 190);
		((Control)Frame1).TabIndex = 82;
		Frame1.TabStop = false;
		((Control)Frame1).Text = "User Information";
		((ListControl)cmbUserShiftMonth).FormattingEnabled = true;
		((Control)cmbUserShiftMonth).Location = new Point(673, 80);
		((Control)cmbUserShiftMonth).Name = "cmbUserShiftMonth";
		((Control)cmbUserShiftMonth).Size = new Size(75, 25);
		((Control)cmbUserShiftMonth).TabIndex = 48;
		((ListControl)cmbUserShiftYear).FormattingEnabled = true;
		((Control)cmbUserShiftYear).Location = new Point(495, 80);
		((Control)cmbUserShiftYear).Name = "cmbUserShiftYear";
		((Control)cmbUserShiftYear).Size = new Size(75, 25);
		((Control)cmbUserShiftYear).TabIndex = 48;
		((Control)label5).BackColor = SystemColors.Control;
		((Control)label5).Cursor = Cursors.Default;
		((Control)label5).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label5).ForeColor = SystemColors.ControlText;
		((Control)label5).Location = new Point(594, 83);
		((Control)label5).Name = "label5";
		((Control)label5).RightToLeft = (RightToLeft)0;
		((Control)label5).Size = new Size(73, 19);
		((Control)label5).TabIndex = 47;
		((Control)label5).Text = "Month :";
		label5.TextAlign = (ContentAlignment)4;
		((Control)label4).BackColor = SystemColors.Control;
		((Control)label4).Cursor = Cursors.Default;
		((Control)label4).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label4).ForeColor = SystemColors.ControlText;
		((Control)label4).Location = new Point(432, 83);
		((Control)label4).Name = "label4";
		((Control)label4).RightToLeft = (RightToLeft)0;
		((Control)label4).Size = new Size(57, 19);
		((Control)label4).TabIndex = 47;
		((Control)label4).Text = "Year :";
		label4.TextAlign = (ContentAlignment)4;
		((Control)Label99).AutoSize = true;
		((Control)Label99).BackColor = SystemColors.Control;
		((Control)Label99).Cursor = Cursors.Default;
		((Control)Label99).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label99).ForeColor = SystemColors.ControlText;
		((Control)Label99).Location = new Point(150, 54);
		((Control)Label99).Name = "Label99";
		((Control)Label99).RightToLeft = (RightToLeft)0;
		((Control)Label99).Size = new Size(90, 19);
		((Control)Label99).TabIndex = 45;
		((Control)Label99).Text = "UserName :";
		Label99.TextAlign = (ContentAlignment)4;
		((Control)Label2).AutoSize = true;
		((Control)Label2).BackColor = SystemColors.Control;
		((Control)Label2).Cursor = Cursors.Default;
		((Control)Label2).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label2).ForeColor = SystemColors.ControlText;
		((Control)Label2).Location = new Point(129, 27);
		((Control)Label2).Name = "Label2";
		((Control)Label2).RightToLeft = (RightToLeft)0;
		((Control)Label2).Size = new Size(112, 19);
		((Control)Label2).TabIndex = 44;
		((Control)Label2).Text = "EnrollNumber :";
		((Control)frmPostGroup).BackColor = SystemColors.Control;
		((Control)frmPostGroup).Controls.Add((Control)(object)dgvShift);
		((Control)frmPostGroup).Controls.Add((Control)(object)dgvPost);
		((Control)frmPostGroup).Controls.Add((Control)(object)txtCompanyName);
		((Control)frmPostGroup).Controls.Add((Control)(object)cmdSetShiftInfo);
		((Control)frmPostGroup).Controls.Add((Control)(object)cmdGetShiftInfo);
		((Control)frmPostGroup).Controls.Add((Control)(object)Label102);
		((Control)frmPostGroup).Font = new Font("Times New Roman", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)frmPostGroup).ForeColor = SystemColors.ControlText;
		((Control)frmPostGroup).Location = new Point(21, 36);
		((Control)frmPostGroup).Name = "frmPostGroup";
		((Control)frmPostGroup).Padding = new Padding(0);
		((Control)frmPostGroup).RightToLeft = (RightToLeft)0;
		((Control)frmPostGroup).Size = new Size(829, 423);
		((Control)frmPostGroup).TabIndex = 81;
		frmPostGroup.TabStop = false;
		((Control)frmPostGroup).Text = "Shift Information";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(23, 4);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(828, 30);
		((Control)lblMessage).TabIndex = 83;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).AutoSizeMode = (AutoSizeMode)0;
		((Form)this).ClientSize = new Size(870, 645);
		((Control)this).Controls.Add((Control)(object)Frame1);
		((Control)this).Controls.Add((Control)(object)frmPostGroup);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmPostShiftInfo";
		((Form)this).ShowIcon = false;
		((Control)this).Text = "Setting Post & Shift information";
		((Form)this).Load += frmPostShiftInfo_Load;
		((ISupportInitialize)dgvUserShiftOfMonth).EndInit();
		((ISupportInitialize)dgvPost).EndInit();
		((ISupportInitialize)dgvShift).EndInit();
		((Control)Frame1).ResumeLayout(false);
		((Control)Frame1).PerformLayout();
		((Control)frmPostGroup).ResumeLayout(false);
		((Control)frmPostGroup).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}

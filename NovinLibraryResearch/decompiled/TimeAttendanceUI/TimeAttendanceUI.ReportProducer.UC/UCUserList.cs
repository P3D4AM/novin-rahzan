using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Waiting;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceUI.ReportProducer.UC;

public class UCUserList : UserControl, IHasContext
{
	private bool _isDisposed;

	private Font _font;

	private bool _Enable;

	private CancellationTokenSource _loadDataToken;

	private List<UserAttendance> _lstUsers;

	private IContainer components;

	private UCPanel ucPanel1;

	private PanelEx panelEx1;

	internal DataGridViewX DGrid;

	private TextBoxX TxtUser;

	private LabelX labelX2;

	private BindingSource userAttendanceBindingSource;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	[JsonIgnore]
	public string ContextGuid { get; private set; }

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
	}

	public int ID
	{
		get
		{
			DataGridViewX dGrid = DGrid;
			if (dGrid == null || ((DataGridView)dGrid).RowCount <= 0 || ((DataGridView)DGrid).CurrentRow == null)
			{
				return 0;
			}
			if (DGID != null)
			{
				return int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
			}
			return 0;
		}
	}

	public List<int> ListId
	{
		get
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			List<int> list = new List<int>();
			try
			{
				if (((DataGridView)DGrid).RowCount > 0)
				{
					for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
					{
						if (Convert.ToBoolean(((DataGridViewCell)(DataGridViewCheckBoxCell)((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i]).Value))
						{
							list.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value.ToString()));
						}
					}
				}
				if (list.Count <= 0 && ID > 0)
				{
					list.Add(ID);
				}
			}
			catch (TaskCanceledException)
			{
			}
			catch (Exception ex2)
			{
				Log.Error(ex2, "");
			}
			return list;
		}
	}

	public bool EnalbleControl
	{
		get
		{
			_Enable = ucPanel1.Expand;
			return _Enable;
		}
		set
		{
			_Enable = value;
			try
			{
				_Enable = ucPanel1.Expand;
				RefreshForm();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnalbleControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCUserList.cs", 148);
			}
		}
	}

	public void SetContextGuid(string contextGuid)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (string.IsNullOrEmpty(contextGuid))
			{
				throw new InvalidOperationException("contextGuid is null or rmpty!!");
			}
			ContextGuid = contextGuid;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Control)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "SetContextGuid", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCUserList.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public UCUserList()
	{
		try
		{
			InitializeComponent();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCUserList.cs", 51);
		}
	}

	public async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			new UserAttendance(ContextGuid);
			_lstUsers = await UserAttendance.GetAllAsync(ContextGuid, token);
			_font = TimeAttendanceSettings.appFont;
			do
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						userAttendanceBindingSource.DataSource = _lstUsers;
						((Control)ucPanel1.panelMain).Controls.Add((Control)(object)panelEx1);
						((Control)panelEx1).Top = 0;
						((Control)this).Size = ((Control)ucPanel1).Size;
						((Control)TxtUser).Focus();
						((Control)DGrid).Font = _font;
					}
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
			RefreshForm();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCUserList.cs", 85);
		}
	}

	private void RefreshForm()
	{
		try
		{
			ucPanel1.HeaderText2 = (EnalbleControl ? "کلیه کاربران" : "کلیه کاربران - غیر فعال");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCUserList.cs", 128);
		}
	}

	private void ucPanel1_Resize(object sender, EventArgs e)
	{
		try
		{
			((Control)this).Size = ((Control)ucPanel1).Size;
			EnalbleControl = EnalbleControl;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ucPanel1_Resize", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCUserList.cs", 161);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)DGCheck).Index && ID >= 0)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCUserList.cs", 175);
		}
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode != 32)
			{
				return;
			}
			e.Handled = true;
			if (((Control)DGrid).Focused && ID >= 0 && ((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)DGCheck).Index)
			{
				DataGridViewCell obj = ((DataGridView)DGrid).SelectedRows[0].Cells[((DataGridViewBand)DGCheck).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCUserList.cs", 197);
		}
	}

	private void TxtUser_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string text = Text.FixString(((Control)TxtUser).Text);
			if (string.IsNullOrEmpty(text))
			{
				userAttendanceBindingSource.Filter = "";
			}
			else
			{
				userAttendanceBindingSource.Filter = " Name like '%" + text + "%' ";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtUser_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCUserList.cs", 211);
		}
	}

	private void TxtPerson_Enter(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtUser).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtPerson_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCUserList.cs", 223);
		}
	}

	private void TxtPerson_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtUser).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtPerson_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCUserList.cs", 232);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
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
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Expected O, but got Unknown
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		labelX2 = new LabelX();
		TxtUser = new TextBoxX();
		DGrid = new DataGridViewX();
		DGCheck = new DataGridViewCheckBoxColumn();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		userAttendanceBindingSource = new BindingSource(components);
		ucPanel1 = new UCPanel();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)userAttendanceBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)15;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)TxtUser);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(2, 30);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).RightToLeft = (RightToLeft)1;
		((Control)panelEx1).Size = new Size(595, 267);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(422, 8);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(170, 21);
		((Control)labelX2).TabIndex = 9;
		((Control)labelX2).Text = "[نام یا کد کاربر] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)TxtUser).Anchor = (AnchorStyles)13;
		TxtUser.Border.Class = "TextBoxBorder";
		TxtUser.Border.CornerType = (eCornerType)1;
		((Control)TxtUser).Location = new Point(6, 8);
		((Control)TxtUser).Name = "TxtUser";
		((Control)TxtUser).Size = new Size(410, 21);
		((Control)TxtUser).TabIndex = 8;
		((Control)TxtUser).TextChanged += TxtUser_TextChanged;
		((Control)TxtUser).Enter += TxtPerson_Enter;
		((Control)TxtUser).Leave += TxtPerson_Leave;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[2]
		{
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)nameDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = userAttendanceBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(6, 37);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(586, 227);
		((Control)DGrid).TabIndex = 6;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((DataGridViewColumn)DGCheck).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCheck).HeaderText = "";
		((DataGridViewColumn)DGCheck).MinimumWidth = 30;
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewBand)DGCheck).ReadOnly = true;
		((DataGridViewColumn)DGCheck).Width = 30;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "[نام شخص]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		userAttendanceBindingSource.DataSource = typeof(UserAttendance);
		((Control)ucPanel1).Anchor = (AnchorStyles)15;
		ucPanel1.Expand = true;
		((Control)ucPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucPanel1.HeaderText = "";
		ucPanel1.HeaderText2 = "";
		((Control)ucPanel1).Location = new Point(0, 0);
		((Control)ucPanel1).Name = "ucPanel1";
		((Control)ucPanel1).Size = new Size(600, 300);
		((Control)ucPanel1).TabIndex = 1;
		((Control)ucPanel1).Resize += ucPanel1_Resize;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Controls.Add((Control)(object)ucPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).MaximumSize = new Size(600, 300);
		((Control)this).MinimumSize = new Size(600, 10);
		((Control)this).Name = "UCUserList";
		((Control)this).Size = new Size(600, 300);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)userAttendanceBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

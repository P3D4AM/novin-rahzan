using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using BackUpDLL;
using BackUpDLL.Assistance;
using DevComponents.DotNetBar;
using FormCustom.Properties;
using NPResourceManagement;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Attendance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;

namespace FormCustom.BackUp.Forms;

public class FRMExitAutoBackup : AccountingBaseForm
{
	private short _count = 21;

	private bool _loaded;

	private bool _isDisposed;

	private string _path;

	private string Pathspecific = "";

	private string _connectionString = "";

	private string _autoBkPath = TextResourceManagement.TextResource.DefaultBackUPFolderName;

	private ENSource _source;

	private Guid _guid = Guid.NewGuid();

	private BackUpMethod _method;

	private IContainer components;

	internal Label Label1;

	internal Label LBLAddress;

	internal Label LBLBackupST;

	internal DataGridView DGrid;

	internal DataGridViewImageColumn DGPic;

	internal DataGridViewTextBoxColumn DGDrive;

	internal DataGridViewTextBoxColumn DGType;

	internal DataGridViewTextBoxColumn DGMessage;

	internal Button BTNExit;

	internal Timer Exit_Timer;

	internal PanelEx PanelEx1;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	private List<PathInformation> CreateDirectories()
	{
		List<PathInformation> list = new List<PathInformation>();
		try
		{
			PathInformation pathInformation = new PathInformation();
			foreach (DriveInfo item in DriveInfo.GetDrives().ToList())
			{
				pathInformation = new PathInformation
				{
					Path = item.Name + _autoBkPath,
					DriveInfo = item
				};
				if (item.DriveType != DriveType.CDRom && item.IsReady && !(pathInformation.Path == ""))
				{
					if (pathInformation.DriveInfo.DriveFormat == "NTFS")
					{
						Directory.CreateDirectory(pathInformation.Path).Attributes = FileAttributes.Compressed;
					}
					else
					{
						Directory.CreateDirectory(pathInformation.Path);
					}
					list.Add(pathInformation);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateDirectories", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 52);
		}
		return list;
	}

	private ReturnedSaveFuncInfo _Backup(string path)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (!string.IsNullOrEmpty(_path))
			{
				path = path + "\\" + new FileInfo(_path).Name;
			}
			if (File.Exists(path) || string.IsNullOrEmpty(_path))
			{
				return returnedSaveFuncInfo;
			}
			File.Copy(_path, path);
			((Control)LBLAddress).Text = path;
		}
		catch (IOException ex)
		{
			if (ex.Message.Contains("There is not enough space on the disk"))
			{
				returnedSaveFuncInfo.AddError(EnSystem.None, "فضای دیسک پر شده است.".GetTranslate() + "\r\n" + path, "", "_Backup", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 69);
				Log.Warning<string>((Exception)ex, "path:{@Path}", path);
			}
			else
			{
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "_Backup", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 75);
				WebErrorLog.Instance.StartLog(ex, "path:{@Path}", path, "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 76);
			}
		}
		catch (Exception ex2)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex2, "", "_Backup", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 81);
			Log.Warning<string>(ex2, "path:{@Path}", path);
		}
		return returnedSaveFuncInfo;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 95);
		}
	}

	private void SetGrid(string path, string message, short messagst)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (string.IsNullOrEmpty(path))
			{
				MessageBox.Show("آدرس مسیر پشتیبان گیری خالی است".GetTranslate());
				return;
			}
			for (int i = 0; i < DGrid.RowCount; i++)
			{
				if (DGrid[((DataGridViewBand)DGDrive).Index, i].Value.ToString() == path.Substring(0, 1))
				{
					DGrid[((DataGridViewBand)DGPic).Index, i].Value = ((messagst == 0) ? "" : "");
					DGrid[((DataGridViewBand)DGPic).Index, i].Value = ((messagst == 0) ? FormCustom.Properties.Resources.accept : FormCustom.Properties.Resources.stop);
					DGrid[((DataGridViewBand)DGMessage).Index, i].Value = message;
					((DataGridViewBand)DGrid.Rows[i]).DefaultCellStyle.BackColor = ((messagst == 0) ? Color.Green : Color.Red);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetGrid", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 118);
		}
	}

	private async Task<ReturnedSaveFuncInfoWithValue<string>> FirstBackupAsync(string contextGuid, string version, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			BackUpConfig config = new BackUpConfig(version)
			{
				StartUpPath = Application.StartupPath,
				ConnectionString = _connectionString,
				Source = _source,
				Path = Pathspecific,
				Guid = _guid,
				CompressMethod = _method
			};
			res = await DataBaseAction.Instance.BackupDbAsync(config, BackupSettingsRepo.GetInstance(contextGuid), token);
			if (!res.HasError)
			{
				_path = res.value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FirstBackupAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 137);
		}
		return res;
	}

	public FRMExitAutoBackup(string contextGuid, string connectionString, ENSource source, string path, BackUpMethod zipMethod = BackUpMethod.GZip, Guid? userAccGuid = null, IBackUpLog_GetUserName _interface = null, CancellationToken token = default(CancellationToken))
		: base(contextGuid)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs"))
		{
			if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(path))
			{
				throw new NullReferenceException("connectionString");
			}
			try
			{
				SqlBackUpLogMaster.InitAsync(BackupSettingsRepo.GetInstance(contextGuid), connectionString, Guid.NewGuid(), source, EnBackUpType.BackUp, userAccGuid, _interface);
				_source = source;
				_method = zipMethod;
				InitializeComponent();
				_connectionString = connectionString;
				Pathspecific = path;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 155);
			}
		}
	}

	private async void FRMExitAutoBackup_Load(object sender, EventArgs e)
	{
		using (new JaegerMonitor("FRMExitAutoBackup_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				try
				{
					line = 1;
					if (_loaded)
					{
						line = 2;
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
						return;
					}
					_loaded = true;
					line = 10;
					if (!string.IsNullOrEmpty(_connectionString))
					{
						goto IL_0109;
					}
					line = 12;
					FRMInitialSettings fRMInitialSettings = new FRMInitialSettings(base.ContextGuid, "");
					((Form)(object)this).CheckVisibility("FRMExitAutoBackup_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs");
					((Form)fRMInitialSettings).ShowDialog((IWin32Window)(object)this);
					line = 15;
					if ((int)((Form)fRMInitialSettings).DialogResult == 1)
					{
						_connectionString = fRMInitialSettings.ConnectionString;
					}
					line = 20;
					if ((int)((Form)fRMInitialSettings).DialogResult != 2)
					{
						goto IL_0109;
					}
					goto end_IL_0061;
					IL_0109:
					line = 25;
					List<PathInformation> lsPathInformation = CreateDirectories();
					if (lsPathInformation == null)
					{
						line = 40;
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
						goto end_IL_0061;
					}
					line = 45;
					foreach (PathInformation item in lsPathInformation)
					{
						line = 50;
						string value = item.DriveInfo.DriveType switch
						{
							DriveType.CDRom => "سی دی درایو".GetTranslate(), 
							DriveType.Fixed => "هارد".GetTranslate(), 
							DriveType.Removable => "حافظه جانبی".GetTranslate(), 
							DriveType.Network => "شبکه".GetTranslate(), 
							DriveType.NoRootDirectory => "غیر ریشه".GetTranslate(), 
							DriveType.Ram => "رم".GetTranslate(), 
							DriveType.Unknown => "نامشخص".GetTranslate(), 
							_ => "نامعلوم".GetTranslate(), 
						};
						line = 70;
						if (item.DriveInfo.DriveType != DriveType.CDRom)
						{
							line = 71;
							DGrid.Rows.Add();
							DGrid[((DataGridViewBand)DGDrive).Index, DGrid.RowCount - 1].Value = item.Path.Substring(0, 1);
							DGrid[((DataGridViewBand)DGMessage).Index, DGrid.RowCount - 1].Value = "کمی صبر کنید ...".GetTranslate();
							DGrid[((DataGridViewBand)DGType).Index, DGrid.RowCount - 1].Value = value;
							line = 75;
							DGrid[((DataGridViewBand)DGPic).Index, DGrid.RowCount - 1].Value = FormCustom.Properties.Resources.wait_Small_JPG;
						}
					}
					line = 80;
					ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await FirstBackupAsync(base.ContextGuid, Application.ProductVersion, default(CancellationToken));
					if (returnedSaveFuncInfoWithValue.HasError)
					{
						((Form)new FrmShowErrorMessage(base.ContextGuid, returnedSaveFuncInfoWithValue, "خطای پشتیبان گیری در محل اجرای برنامه".GetTranslate(), (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "FRMExitAutoBackup_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs")).ShowDialog();
					}
					foreach (PathInformation item2 in lsPathInformation)
					{
						line = 101;
						if (item2.DriveInfo.DriveType != DriveType.Removable && item2.DriveInfo.DriveType != DriveType.Fixed)
						{
							continue;
						}
						try
						{
							line = 103;
							ret.AddReturnedValue(_Backup(item2.Path), ReturnedState.None, "FRMExitAutoBackup_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 236);
							if (!ret.HasError)
							{
								((Control)LBLBackupST).Text = "با موفقیت انجام شد".GetTranslate();
								((Control)LBLBackupST).ForeColor = Color.LimeGreen;
								line = 110;
								SetGrid(item2.Path, "پشتیبان با موفقیت انجام شد".GetTranslate(), 0);
							}
						}
						catch (Exception ex)
						{
							SetGrid(item2.Path, "پشتیبان با شکست مواجه شد".GetTranslate() + ex.Message, 1);
							ret.AddReturnedValue(ex, "", "FRMExitAutoBackup_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 246);
							WebErrorLog.Instance.StartLog(ex, $"line:{line}", "FRMExitAutoBackup_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 247);
						}
					}
					Exit_Timer.Enabled = true;
					goto end_IL_0058;
					end_IL_0061:;
				}
				catch (Exception ex2)
				{
					ret.AddReturnedValue(ex2, "", "FRMExitAutoBackup_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 255);
					WebErrorLog.Instance.StartLog(ex2, $"line:{line}", "FRMExitAutoBackup_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 256);
					goto end_IL_0058;
				}
				return;
				end_IL_0058:;
			}
			finally
			{
				if (!IsDisposed && ret.HasError)
				{
					((Control)LBLBackupST).Text = "با شکست مواجه شد".GetTranslate();
					((Control)LBLBackupST).ForeColor = Color.Red;
					await this.ShowReturnedSavedFuncInfoAsync(ret, "خطا در تهیه نسخه پشتیبان".GetTranslate(), "FRMExitAutoBackup_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs");
				}
			}
		}
	}

	private void Exit_Timer_Tick(object sender, EventArgs e)
	{
		try
		{
			((Control)BTNExit).Enabled = true;
			_count--;
			((Control)BTNExit).Text = "خروج".GetTranslate() + "(" + _count + ")";
			if (_count == 0)
			{
				BTNExit.PerformClick();
				Exit_Timer.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Exit_Timer_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMExitAutoBackup.cs", 279);
		}
	}

	private void BTNExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		components = new Container();
		Label1 = new Label();
		LBLAddress = new Label();
		LBLBackupST = new Label();
		DGrid = new DataGridView();
		DGPic = new DataGridViewImageColumn();
		DGDrive = new DataGridViewTextBoxColumn();
		DGType = new DataGridViewTextBoxColumn();
		DGMessage = new DataGridViewTextBoxColumn();
		BTNExit = new Button();
		Exit_Timer = new Timer(components);
		PanelEx1 = new PanelEx();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)PanelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)Label1).Anchor = (AnchorStyles)13;
		((Control)Label1).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label1).ForeColor = SystemColors.HotTrack;
		((Control)Label1).Location = new Point(17, 5);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(718, 58);
		((Control)Label1).TabIndex = 0;
		((Control)Label1).Text = "[برنامه در حال تهیه نسخه پشتیبان است]\r\n[لطفا کمی صبر کنید]\r\n";
		((Control)LBLAddress).Anchor = (AnchorStyles)13;
		((Control)LBLAddress).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LBLAddress).ForeColor = Color.Crimson;
		((Control)LBLAddress).Location = new Point(12, 72);
		((Control)LBLAddress).Name = "LBLAddress";
		((Control)LBLAddress).RightToLeft = (RightToLeft)0;
		((Control)LBLAddress).Size = new Size(723, 27);
		((Control)LBLAddress).TabIndex = 1;
		((Control)LBLAddress).Text = "Wait .....";
		LBLAddress.TextAlign = (ContentAlignment)16;
		((Control)LBLBackupST).Anchor = (AnchorStyles)13;
		((Control)LBLBackupST).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LBLBackupST).ForeColor = Color.Blue;
		((Control)LBLBackupST).Location = new Point(17, 99);
		((Control)LBLBackupST).Name = "LBLBackupST";
		((Control)LBLBackupST).RightToLeft = (RightToLeft)0;
		((Control)LBLBackupST).Size = new Size(713, 33);
		((Control)LBLBackupST).TabIndex = 2;
		LBLBackupST.TextAlign = (ContentAlignment)32;
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		DGrid.AllowUserToResizeRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[4]
		{
			(DataGridViewColumn)DGPic,
			(DataGridViewColumn)DGDrive,
			(DataGridViewColumn)DGType,
			(DataGridViewColumn)DGMessage
		});
		((Control)DGrid).Location = new Point(17, 135);
		DGrid.MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		DGrid.ReadOnly = true;
		DGrid.RowHeadersVisible = false;
		DGrid.RowTemplate.Height = 30;
		DGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(718, 207);
		((Control)DGrid).TabIndex = 6;
		((DataGridViewColumn)DGPic).HeaderText = "";
		DGPic.ImageLayout = (DataGridViewImageCellLayout)2;
		((DataGridViewColumn)DGPic).Name = "DGPic";
		((DataGridViewBand)DGPic).ReadOnly = true;
		((DataGridViewColumn)DGPic).Width = 30;
		((DataGridViewColumn)DGDrive).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDrive).HeaderText = "[درایو]";
		((DataGridViewColumn)DGDrive).Name = "DGDrive";
		((DataGridViewBand)DGDrive).ReadOnly = true;
		((DataGridViewBand)DGDrive).Resizable = (DataGridViewTriState)1;
		DGDrive.SortMode = (DataGridViewColumnSortMode)0;
		((DataGridViewColumn)DGDrive).Width = 36;
		((DataGridViewColumn)DGType).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGType).HeaderText = "[نوع]";
		((DataGridViewColumn)DGType).Name = "DGType";
		((DataGridViewBand)DGType).ReadOnly = true;
		((DataGridViewColumn)DGType).Width = 49;
		((DataGridViewColumn)DGMessage).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGMessage).HeaderText = "[پیغام]";
		((DataGridViewColumn)DGMessage).Name = "DGMessage";
		((DataGridViewBand)DGMessage).ReadOnly = true;
		((Control)BTNExit).Anchor = (AnchorStyles)14;
		BTNExit.DialogResult = (DialogResult)2;
		((Control)BTNExit).Enabled = false;
		((Control)BTNExit).Location = new Point(22, 348);
		((Control)BTNExit).Name = "BTNExit";
		((Control)BTNExit).Size = new Size(713, 30);
		((Control)BTNExit).TabIndex = 7;
		((Control)BTNExit).Text = "[خروج]";
		((ButtonBase)BTNExit).UseVisualStyleBackColor = true;
		((Control)BTNExit).Click += BTNExit_Click;
		Exit_Timer.Interval = 1000;
		Exit_Timer.Tick += Exit_Timer_Tick;
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx1).Controls.Add((Control)(object)BTNExit);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)Label1);
		((Control)PanelEx1).Controls.Add((Control)(object)LBLAddress);
		((Control)PanelEx1).Controls.Add((Control)(object)LBLBackupST);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(747, 390);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		PanelEx1.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		PanelEx1.Style.Border = eBorderType.SingleLine;
		PanelEx1.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 8;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(747, 390);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(747, 390);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(747, 390);
		((Control)this).Name = "FRMExitAutoBackup";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FRMExitAutoBackup";
		((Form)this).Load += FRMExitAutoBackup_Load;
		((ISupportInitialize)DGrid).EndInit();
		((Control)PanelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using BackUpDLL;
using BackUpDLL.Assistance;
using EntityCache;
using EntityCache.Business.Accounting;
using FormCustom.Cache;
using FormCustom.ManagementPassword;
using FormCustom.Properties;
using FormCustom.UC;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.BackUp.Forms;

public class frmBackUp : AccountingBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private Timer _timer = new Timer
	{
		Enabled = true,
		Interval = 750
	};

	private short index;

	private IContainer components;

	private UcGroupBox ucGroupBox1;

	private PictureBox pictureBox1;

	private Label label1;

	private Label label2;

	internal TextBox txtPassword;

	private PictureBox picRemovePass;

	private Label label3;

	internal TextBox txtPath;

	private UcButtons btnSelectPath;

	private Label label4;

	private UcGroupBox ucGroupBox2;

	private Label label5;

	private Label label6;

	private Label label7;

	private Label label8;

	private Label label9;

	private Label label10;

	private UcButtons btnCancel;

	private UcButtons btnOk;

	private Label lblLastBackUpName;

	private Label lblLastBackUpPath;

	private Label lblLastBackUpDate;

	private Label lblLink;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			SqlBackUpLogMaster lastBackUp = await SqlBackUpLogMaster.GetLastAsync(base.ContextGuid, base.ConnectionString, token);
			if (string.IsNullOrEmpty(lastBackUp?.Path) || IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			FileInfo inf = new FileInfo(lastBackUp.Path);
			string text = Path.GetFileName(Assembly.GetEntryAssembly().Location).Replace(".exe", "__");
			string text2 = DateTime.Now.MiladiToCalture().Replace("/", "_");
			text2 = text2 + "__" + DateTime.Now.Hour + " - " + DateTime.Now.Minute;
			text += text2;
			string newBackUpFileName = inf.DirectoryName + "\\" + text;
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					((Control)lblLastBackUpName).Text = inf.Name;
					((Control)lblLastBackUpPath).Text = lastBackUp.Path;
					((Control)lblLastBackUpDate).Text = lastBackUp.Time + "     " + lastBackUp.DateSh;
					((Control)txtPath).Text = newBackUpFileName + "-version_" + Application.ProductVersion.Replace(".", "_") + ".NP";
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs", 64);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs", 67);
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
				Timer timer = _timer;
				if (timer != null)
				{
					timer.Stop();
				}
				if (_timer != null)
				{
					_timer.Tick -= Timer_Tick;
				}
				_timer = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs", 87);
		}
	}

	public frmBackUp(string contextGuid)
		: base(contextGuid)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		frmBackUp frmBackUp2 = this;
		try
		{
			InitializeComponent();
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			Task.Run(() => frmBackUp2.LoadDataAsync(token));
			new ToolTip().SetToolTip((Control)(object)picRemovePass, "حذف رمز مدیریتی");
			if (_timer != null)
			{
				_timer.Tick -= Timer_Tick;
				_timer.Tick += Timer_Tick;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs", 108);
		}
	}

	private void frmBackUp_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose();
	}

	private void btnCancel_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnCancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs", 119);
		}
	}

	private void frmBackUp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					btnOk.PerformClick();
				}
			}
			else
			{
				btnCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmBackUp_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs", 131);
		}
	}

	private void btnSelectPath_OnClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Invalid comparison between Unknown and I4
		try
		{
			SaveFileDialog val = new SaveFileDialog
			{
				Filter = "NovinPardaz Backup File|*.BAK|NovinPardaz Backup File|*.NP|NovinPardaz Ziped Backup File|*.NPZ"
			};
			string text = Path.GetFileName(Assembly.GetEntryAssembly().Location).Replace(".exe", "__");
			string text2 = DateTime.Now.MiladiToCalture().Replace("/", "_");
			text2 = text2 + "__" + DateTime.Now.Hour + " - " + DateTime.Now.Minute;
			text += text2;
			((FileDialog)val).FileName = text;
			if ((int)((CommonDialog)val).ShowDialog((IWin32Window)(object)this) == 1)
			{
				FileInfo fileInfo = new FileInfo(((FileDialog)val).FileName);
				((Control)txtPath).Text = fileInfo.FullName.Replace(fileInfo.Extension, "") + "-version_" + Application.ProductVersion.Replace(".", "_") + ".NP";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnSelectPath_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs", 148);
		}
	}

	private void picRemovePass_Click(object sender, EventArgs e)
	{
		((Control)txtPassword).Text = "";
	}

	private async void btnOk_OnClick(object sender, EventArgs e)
	{
		Waiter waiter = new Waiter("در حال پشتیبان گیری ...", (Form)(object)this, null, freezeForm: true, EnWaiterType.Circle, null, "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs");
		int line = 0;
		try
		{
			line = 1;
			if (((Control)txtPassword).Text != (string)CLSSystem.GlobalPass)
			{
				line = 2;
				waiter?.Dispose();
				line = 3;
				await this.ShowErrorAsync("رمز وارد شده صحیح نیست");
				return;
			}
			line = 5;
			if (string.IsNullOrEmpty(((Control)txtPath).Text))
			{
				line = 6;
				waiter?.Dispose();
				line = 7;
				await this.ShowErrorAsync("آدرس وارد شده جهت ذخیره کردن پشتیبان معتبر نمی باشد");
				return;
			}
			line = 8;
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(base.ContextGuid);
			line = 9;
			BackUpConfig config = new BackUpConfig(Application.ProductVersion)
			{
				StartUpPath = Application.StartupPath,
				ConnectionString = base.ContextGuid.GetSqlConnectionString(),
				Source = ClsCache.GetCache(base.ContextGuid).Source,
				Path = ((Control)txtPath).Text,
				UserAccGuid = cashierBusiness.Guid
			};
			line = 10;
			Task.Run(async delegate
			{
				try
				{
					line = 11;
					ReturnedSaveFuncInfoWithValue<string> obj = await DataBaseAction.Instance.BackupDbAsync(config, BackupSettingsRepo.GetInstance(base.ContextGuid));
					line = 12;
					if (obj.HasError)
					{
						((Control)this).Invoke((Delegate)(Action)delegate
						{
							//IL_0036: Unknown result type (might be due to invalid IL or missing references)
							line = 13;
							waiter?.Dispose();
							line = 14;
							((CacheBaseForm)this).ShowError("خطا در پشتیبان گیری از اطلاعات".GetTranslate(), "خطا");
						});
					}
					else
					{
						((Control)this).Invoke((Delegate)(Action)delegate
						{
							//IL_0040: Unknown result type (might be due to invalid IL or missing references)
							line = 15;
							waiter?.Dispose();
							line = 16;
							this.ShowMessage("پشتیبان گیری اطلاعات با موفقیت انجام شد.".GetTranslate(), "پیغام سیستم", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs");
							line = 17;
							Process.Start(new FileInfo(((Control)txtPath).Text).DirectoryName);
							line = 18;
							((Form)this).DialogResult = (DialogResult)1;
							line = 19;
							((Form)this).Close();
						});
						line = 20;
					}
					line = 21;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, $"logger1 #Line_{line}", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs", 221);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"logger2 #Line_{line}", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs", 224);
		}
	}

	private void txtPassword_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				btnOk.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtPassword_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs", 234);
		}
	}

	private void label11_Click(object sender, EventArgs e)
	{
		Process.Start("https://novinacc.ir/backup-service");
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		try
		{
			if (!IsDisposed)
			{
				if (index == 0)
				{
					((Control)lblLink).ForeColor = Color.FromArgb(65, 81, 220);
					index = 1;
				}
				else
				{
					((Control)lblLink).ForeColor = Color.FromArgb(249, 250, 255);
					index = 0;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Timer_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUp.cs", 253);
		}
	}

	private void InitializeComponent()
	{
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
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Expected O, but got Unknown
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Expected O, but got Unknown
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Expected O, but got Unknown
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Expected O, but got Unknown
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Expected O, but got Unknown
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Expected O, but got Unknown
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Expected O, but got Unknown
		//IL_095b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Expected O, but got Unknown
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Expected O, but got Unknown
		//IL_0b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1c: Expected O, but got Unknown
		//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be6: Expected O, but got Unknown
		//IL_0c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca4: Expected O, but got Unknown
		//IL_0d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d79: Expected O, but got Unknown
		//IL_0e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e34: Expected O, but got Unknown
		//IL_0f11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1b: Expected O, but got Unknown
		//IL_0f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc5: Expected O, but got Unknown
		//IL_104f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1059: Expected O, but got Unknown
		//IL_109b: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1105: Expected O, but got Unknown
		//IL_116b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1175: Expected O, but got Unknown
		//IL_1236: Unknown result type (might be due to invalid IL or missing references)
		//IL_1240: Expected O, but got Unknown
		//IL_1301: Unknown result type (might be due to invalid IL or missing references)
		//IL_130b: Expected O, but got Unknown
		//IL_13e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13eb: Expected O, but got Unknown
		//IL_163f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1649: Expected O, but got Unknown
		//IL_165c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1666: Expected O, but got Unknown
		//IL_166b: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_16dc: Expected O, but got Unknown
		//IL_16e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ee: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmBackUp));
		ucGroupBox1 = new UcGroupBox();
		pictureBox1 = new PictureBox();
		label1 = new Label();
		label2 = new Label();
		txtPassword = new TextBox();
		picRemovePass = new PictureBox();
		label3 = new Label();
		txtPath = new TextBox();
		btnSelectPath = new UcButtons();
		label4 = new Label();
		ucGroupBox2 = new UcGroupBox();
		label5 = new Label();
		label6 = new Label();
		label7 = new Label();
		label8 = new Label();
		label9 = new Label();
		label10 = new Label();
		btnCancel = new UcButtons();
		btnOk = new UcButtons();
		lblLastBackUpName = new Label();
		lblLastBackUpPath = new Label();
		lblLastBackUpDate = new Label();
		lblLink = new Label();
		((ISupportInitialize)pictureBox1).BeginInit();
		((ISupportInitialize)picRemovePass).BeginInit();
		((Control)this).SuspendLayout();
		((Control)ucGroupBox1).BackColor = Color.Transparent;
		ucGroupBox1.DisplayFont = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucGroupBox1).Dock = (DockStyle)5;
		((Control)ucGroupBox1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucGroupBox1.HeadHeight = 40;
		ucGroupBox1.IsShowHeader = false;
		((Control)ucGroupBox1).Location = new Point(0, 0);
		((Control)ucGroupBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucGroupBox1).Name = "ucGroupBox1";
		((Control)ucGroupBox1).RightToLeft = (RightToLeft)1;
		((Control)ucGroupBox1).Size = new Size(716, 617);
		((Control)ucGroupBox1).TabIndex = 4;
		ucGroupBox1.Title = "        [عنوان]";
		((Control)pictureBox1).Anchor = (AnchorStyles)9;
		((Control)pictureBox1).BackColor = Color.FromArgb(249, 250, 255);
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.NP_SQL;
		((Control)pictureBox1).Location = new Point(646, 12);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(58, 41);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 1;
		pictureBox1.TabStop = false;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label1).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)label1).Location = new Point(456, 18);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(196, 29);
		((Control)label1).TabIndex = 2;
		((Control)label1).Text = "[تهیه پشتیبان از پایگاه اطلاعاتی]";
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label2).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label2).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)label2).Location = new Point(241, 18);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(101, 29);
		((Control)label2).TabIndex = 3;
		((Control)label2).Text = "[رمز مدیریتی]:";
		((Control)txtPassword).BackColor = Color.White;
		((Control)txtPassword).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)txtPassword).ForeColor = Color.FromArgb(64, 64, 64);
		((Control)txtPassword).Location = new Point(50, 22);
		((Control)txtPassword).Name = "txtPassword";
		((Control)txtPassword).Size = new Size(185, 27);
		((Control)txtPassword).TabIndex = 0;
		txtPassword.TextAlign = (HorizontalAlignment)2;
		txtPassword.UseSystemPasswordChar = true;
		((Control)txtPassword).KeyDown += new KeyEventHandler(txtPassword_KeyDown);
		((Control)picRemovePass).BackColor = Color.FromArgb(249, 250, 255);
		((Control)picRemovePass).Cursor = Cursors.Hand;
		picRemovePass.Image = (Image)(object)FormCustom.Properties.Resources.NP_close;
		((Control)picRemovePass).Location = new Point(18, 22);
		((Control)picRemovePass).Name = "picRemovePass";
		((Control)picRemovePass).Size = new Size(29, 27);
		picRemovePass.SizeMode = (PictureBoxSizeMode)4;
		picRemovePass.TabIndex = 1;
		picRemovePass.TabStop = false;
		((Control)picRemovePass).Click += picRemovePass_Click;
		((Control)label3).Anchor = (AnchorStyles)9;
		((Control)label3).AutoSize = true;
		((Control)label3).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label3).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label3).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label3).Location = new Point(598, 67);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(110, 24);
		((Control)label3).TabIndex = 5;
		((Control)label3).Text = "[مسیر پشتیبان]:";
		((Control)txtPath).Anchor = (AnchorStyles)13;
		((Control)txtPath).BackColor = Color.White;
		((Control)txtPath).Enabled = false;
		((Control)txtPath).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)txtPath).ForeColor = Color.FromArgb(64, 64, 64);
		((Control)txtPath).Location = new Point(51, 67);
		((Control)txtPath).Name = "txtPath";
		((TextBoxBase)txtPath).ReadOnly = true;
		((Control)txtPath).Size = new Size(533, 27);
		((Control)txtPath).TabIndex = 6;
		txtPath.TextAlign = (HorizontalAlignment)1;
		((Control)btnSelectPath).BackColor = Color.Transparent;
		btnSelectPath.CornerType = EnButtonCornerType.Square;
		btnSelectPath.DisplayText = "...";
		((Control)btnSelectPath).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnSelectPath.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnSelectPath.IsShowMoreDetails = true;
		((Control)btnSelectPath).Location = new Point(18, 67);
		((Control)btnSelectPath).Margin = new Padding(4, 5, 4, 5);
		((Control)btnSelectPath).Name = "btnSelectPath";
		((Control)btnSelectPath).RightToLeft = (RightToLeft)1;
		((Control)btnSelectPath).Size = new Size(29, 27);
		((Control)btnSelectPath).TabIndex = 7;
		btnSelectPath.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnSelectPath.Type = EnButtonType.Bold;
		btnSelectPath.OnClick += btnSelectPath_OnClick;
		((Control)label4).Anchor = (AnchorStyles)9;
		((Control)label4).AutoSize = true;
		((Control)label4).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label4).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label4).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label4).Location = new Point(497, 105);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(211, 24);
		((Control)label4).TabIndex = 8;
		((Control)label4).Text = "[مشخصات آخرین پشتیبان موجود]";
		((Control)ucGroupBox2).Anchor = (AnchorStyles)13;
		((Control)ucGroupBox2).BackColor = Color.Transparent;
		ucGroupBox2.DisplayFont = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucGroupBox2).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucGroupBox2.HeadHeight = 40;
		ucGroupBox2.IsShowHeader = false;
		((Control)ucGroupBox2).Location = new Point(15, 134);
		((Control)ucGroupBox2).Margin = new Padding(4, 5, 4, 5);
		((Control)ucGroupBox2).Name = "ucGroupBox2";
		((Control)ucGroupBox2).RightToLeft = (RightToLeft)1;
		((Control)ucGroupBox2).Size = new Size(689, 120);
		((Control)ucGroupBox2).TabIndex = 9;
		ucGroupBox2.Title = "[عنوان]";
		((Control)label5).Anchor = (AnchorStyles)9;
		((Control)label5).AutoSize = true;
		((Control)label5).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label5).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label5).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label5).Location = new Point(628, 146);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(62, 20);
		((Control)label5).TabIndex = 10;
		((Control)label5).Text = "[نام فایل]:";
		((Control)label6).Anchor = (AnchorStyles)9;
		((Control)label6).AutoSize = true;
		((Control)label6).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label6).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label6).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label6).Location = new Point(641, 180);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(49, 20);
		((Control)label6).TabIndex = 11;
		((Control)label6).Text = "[مسیر]:";
		((Control)label7).Anchor = (AnchorStyles)9;
		((Control)label7).AutoSize = true;
		((Control)label7).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label7).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label7).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label7).Location = new Point(616, 217);
		((Control)label7).Name = "label7";
		((Control)label7).Size = new Size(74, 20);
		((Control)label7).TabIndex = 12;
		((Control)label7).Text = "[تاریخ ایجاد]:";
		((Control)label8).Anchor = (AnchorStyles)13;
		((Control)label8).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label8).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label8).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label8).Location = new Point(15, 277);
		((Control)label8).Name = "label8";
		((Control)label8).Size = new Size(671, 112);
		((Control)label8).TabIndex = 13;
		((Control)label8).Text = componentResourceManager.GetString("label8.Text").GetTranslate();
		((Control)label9).Anchor = (AnchorStyles)9;
		((Control)label9).AutoSize = true;
		((Control)label9).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label9).Font = new Font("B Yekan", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label9).ForeColor = Color.Red;
		((Control)label9).Location = new Point(321, 403);
		((Control)label9).Name = "label9";
		((Control)label9).Size = new Size(381, 29);
		((Control)label9).TabIndex = 14;
		((Control)label9).Text = "[سرویس پشتیبان گیری اینترنتی نوین پرداز چیست؟]";
		((Control)label10).Anchor = (AnchorStyles)13;
		((Control)label10).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label10).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label10).ForeColor = Color.Red;
		((Control)label10).Location = new Point(15, 436);
		((Control)label10).Name = "label10";
		((Control)label10).Size = new Size(671, 70);
		((Control)label10).TabIndex = 15;
		((Control)label10).Text = componentResourceManager.GetString("label10.Text").GetTranslate();
		((Control)btnCancel).Anchor = (AnchorStyles)6;
		((Control)btnCancel).BackColor = Color.FromArgb(249, 250, 255);
		btnCancel.CornerType = EnButtonCornerType.Round;
		((Control)btnCancel).Cursor = Cursors.Hand;
		btnCancel.DisplayText = "[انصراف](Esc)";
		((Control)btnCancel).Font = new Font("B Yekan", 9.75f);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(9, 569);
		((Control)btnCancel).Margin = new Padding(0);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(135, 39);
		((Control)btnCancel).TabIndex = 16;
		btnCancel.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Type = EnButtonType.Normal;
		btnCancel.OnClick += btnCancel_OnClick;
		((Control)btnOk).Anchor = (AnchorStyles)6;
		((Control)btnOk).BackColor = Color.FromArgb(249, 250, 255);
		btnOk.CornerType = EnButtonCornerType.Round;
		((Control)btnOk).Cursor = Cursors.Hand;
		btnOk.DisplayText = "[تایید](F5)";
		((Control)btnOk).Font = new Font("B Yekan", 9.75f);
		btnOk.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnOk.IsShowMoreDetails = false;
		((Control)btnOk).Location = new Point(153, 569);
		((Control)btnOk).Margin = new Padding(0);
		((Control)btnOk).Name = "btnOk";
		((Control)btnOk).RightToLeft = (RightToLeft)1;
		((Control)btnOk).Size = new Size(273, 39);
		((Control)btnOk).TabIndex = 1;
		btnOk.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Type = EnButtonType.Bold;
		btnOk.OnClick += btnOk_OnClick;
		((Control)lblLastBackUpName).Anchor = (AnchorStyles)13;
		((Control)lblLastBackUpName).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblLastBackUpName).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblLastBackUpName).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)lblLastBackUpName).Location = new Point(27, 146);
		((Control)lblLastBackUpName).Name = "lblLastBackUpName";
		((Control)lblLastBackUpName).Size = new Size(574, 20);
		((Control)lblLastBackUpName).TabIndex = 10;
		((Control)lblLastBackUpName).Text = "---";
		lblLastBackUpName.TextAlign = (ContentAlignment)64;
		((Control)lblLastBackUpPath).Anchor = (AnchorStyles)13;
		((Control)lblLastBackUpPath).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblLastBackUpPath).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblLastBackUpPath).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)lblLastBackUpPath).Location = new Point(27, 180);
		((Control)lblLastBackUpPath).Name = "lblLastBackUpPath";
		((Control)lblLastBackUpPath).Size = new Size(574, 20);
		((Control)lblLastBackUpPath).TabIndex = 10;
		((Control)lblLastBackUpPath).Text = "---";
		lblLastBackUpPath.TextAlign = (ContentAlignment)64;
		((Control)lblLastBackUpDate).Anchor = (AnchorStyles)13;
		((Control)lblLastBackUpDate).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblLastBackUpDate).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblLastBackUpDate).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)lblLastBackUpDate).Location = new Point(27, 217);
		((Control)lblLastBackUpDate).Name = "lblLastBackUpDate";
		((Control)lblLastBackUpDate).Size = new Size(574, 20);
		((Control)lblLastBackUpDate).TabIndex = 10;
		((Control)lblLastBackUpDate).Text = "---";
		lblLastBackUpDate.TextAlign = (ContentAlignment)64;
		((Control)lblLink).Anchor = (AnchorStyles)9;
		((Control)lblLink).AutoSize = true;
		((Control)lblLink).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblLink).Cursor = Cursors.Hand;
		((Control)lblLink).Font = new Font("B Yekan", 11.5f);
		((Control)lblLink).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblLink).Location = new Point(27, 503);
		((Control)lblLink).Name = "lblLink";
		((Control)lblLink).Size = new Size(217, 24);
		((Control)lblLink).TabIndex = 8;
		((Control)lblLink).Text = "[جهت اطلاعات بیشتر، کلیک نمایید]";
		((Control)lblLink).Click += label11_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(716, 617);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)btnOk);
		((Control)this).Controls.Add((Control)(object)label7);
		((Control)this).Controls.Add((Control)(object)label6);
		((Control)this).Controls.Add((Control)(object)label10);
		((Control)this).Controls.Add((Control)(object)label8);
		((Control)this).Controls.Add((Control)(object)lblLastBackUpDate);
		((Control)this).Controls.Add((Control)(object)lblLastBackUpPath);
		((Control)this).Controls.Add((Control)(object)lblLastBackUpName);
		((Control)this).Controls.Add((Control)(object)label5);
		((Control)this).Controls.Add((Control)(object)ucGroupBox2);
		((Control)this).Controls.Add((Control)(object)btnSelectPath);
		((Control)this).Controls.Add((Control)(object)txtPath);
		((Control)this).Controls.Add((Control)(object)txtPassword);
		((Control)this).Controls.Add((Control)(object)lblLink);
		((Control)this).Controls.Add((Control)(object)label4);
		((Control)this).Controls.Add((Control)(object)label3);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)label9);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)picRemovePass);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Controls.Add((Control)(object)ucGroupBox1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(732, 656);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(732, 656);
		((Control)this).Name = "frmBackUp";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[ایجاد پشتیبان]";
		((Form)this).FormClosed += new FormClosedEventHandler(frmBackUp_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmBackUp_KeyDown);
		((ISupportInitialize)pictureBox1).EndInit();
		((ISupportInitialize)picRemovePass).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}

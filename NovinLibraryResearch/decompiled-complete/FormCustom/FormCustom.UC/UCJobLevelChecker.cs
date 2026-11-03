using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UCJobLevelChecker : UserControl
{
	private bool _isDisposed;

	private CancellationTokenSource _statusToken = new CancellationTokenSource();

	private CancellationTokenSource _totalCountToken = new CancellationTokenSource();

	private CancellationTokenSource _shortMessageToken = new CancellationTokenSource();

	private CancellationTokenSource _taskToken = new CancellationTokenSource();

	private string _shortMessage = "";

	private decimal _percent;

	private int _totalCount;

	private string _title = "";

	private bool? _status;

	private Task<ReturnedSaveFuncInfo> _t;

	private IContainer components;

	private Label lblTitle;

	private PictureBox pictureBox1;

	private Label lblShortMessage;

	private ToolTip toolTip1;

	private Button BtnAction;

	public decimal Persent
	{
		get
		{
			return _percent;
		}
		set
		{
			try
			{
				_percent = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Persent", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 43);
			}
		}
	}

	public string Title
	{
		get
		{
			return _title;
		}
		set
		{
			try
			{
				_title = value;
				((Control)lblTitle).Text = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Title", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 57);
			}
		}
	}

	public bool? Status
	{
		get
		{
			return _status;
		}
		set
		{
			try
			{
				_statusToken?.Cancel();
				_statusToken = new CancellationTokenSource(60000);
				CancellationToken token = _statusToken.Token;
				_status = value;
				ChangeStatusAsync(token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Status", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 74);
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> _Task
	{
		get
		{
			return _t;
		}
		set
		{
			try
			{
				Status = null;
				_taskToken?.Cancel();
				_taskToken = new CancellationTokenSource(60000);
				CancellationToken token = _taskToken.Token;
				_t = value;
				RunTaskAsync(token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "_Task", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 93);
			}
		}
	}

	public bool IsDisposed
	{
		get
		{
			if (!((Control)this).IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public ReturnedSaveFuncInfo FinalStatus { get; set; }

	public string ShortMessage
	{
		get
		{
			return _shortMessage;
		}
		set
		{
			try
			{
				if (!IsDisposed)
				{
					_shortMessage = value;
					_shortMessageToken?.Cancel();
					_shortMessageToken = new CancellationTokenSource(60000);
					CancellationToken token = _shortMessageToken.Token;
					ChangeShortMessageAsync(token);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ShortMessage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 182);
			}
		}
	}

	[Browsable(true)]
	[Category("Action")]
	[Description("Invoked when user clicks button")]
	public event EventHandler ButtonClick;

	public UCJobLevelChecker()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 106);
		}
	}

	public async Task<ReturnedSaveFuncInfo> RunTaskAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (_t != null)
			{
				new Random();
				res = await _t;
				await Task.Delay(500);
				if (res.HasError)
				{
					ShortMessage = res.GetErrorMessage();
				}
				if (res.HasInformation)
				{
					ShortMessage = res.GetInformationMessage();
				}
				Status = !res.HasError;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RunTaskAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 126);
			res.AddReturnedValue(ex, "", "RunTaskAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 127);
		}
		return res;
	}

	private async Task ChangeStatusAsync(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(500);
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						if (!Status.HasValue)
						{
							pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.wait_Small_JPG;
						}
						else if (_status.GetValueOrDefault())
						{
							pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.accept;
						}
						else
						{
							pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.Error;
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 160);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 163);
		}
	}

	private async Task ChangeShortMessageAsync(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(500);
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
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)lblShortMessage).Text = _shortMessage;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ChangeShortMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 204);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeShortMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 207);
		}
	}

	private void controls_Click(object sender, EventArgs e)
	{
		try
		{
			if (this.ButtonClick != null)
			{
				this.ButtonClick(this, e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "controls_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 217);
		}
	}

	private void UCJobLevelChecker_Load(object sender, EventArgs e)
	{
	}

	private void BtnAction_Click(object sender, EventArgs e)
	{
		try
		{
			if (this.ButtonClick != null)
			{
				this.ButtonClick(sender, e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnAction_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UCJobLevelChecker.cs", 233);
		}
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		components = new Container();
		lblTitle = new Label();
		lblShortMessage = new Label();
		toolTip1 = new ToolTip(components);
		pictureBox1 = new PictureBox();
		BtnAction = new Button();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)lblTitle).Anchor = (AnchorStyles)13;
		((Control)lblTitle).BackColor = Color.Transparent;
		((Control)lblTitle).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.DarkBlue;
		((Control)lblTitle).Location = new Point(36, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(325, 20);
		((Control)lblTitle).TabIndex = 0;
		((Control)lblTitle).Text = "[عنواان عملیات]";
		lblTitle.TextAlign = (ContentAlignment)32;
		((Control)lblTitle).Click += controls_Click;
		((Control)lblShortMessage).Anchor = (AnchorStyles)15;
		((Control)lblShortMessage).BackColor = Color.Transparent;
		((Control)lblShortMessage).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblShortMessage).Location = new Point(36, 16);
		((Control)lblShortMessage).Name = "lblShortMessage";
		((Control)lblShortMessage).Size = new Size(325, 24);
		((Control)lblShortMessage).TabIndex = 2;
		((Control)lblShortMessage).Text = "[نتیجه اجرای عملیات ...]";
		lblShortMessage.TextAlign = (ContentAlignment)16;
		((Control)lblShortMessage).Click += controls_Click;
		((Control)pictureBox1).Anchor = (AnchorStyles)11;
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.timg11;
		((Control)pictureBox1).Location = new Point(364, 1);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(34, 38);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 1;
		pictureBox1.TabStop = false;
		((Control)pictureBox1).Click += controls_Click;
		((Control)BtnAction).Dock = (DockStyle)3;
		((ButtonBase)BtnAction).Image = (Image)(object)FormCustom.Properties.Resources.Setting;
		((Control)BtnAction).Location = new Point(0, 0);
		((Control)BtnAction).Name = "BtnAction";
		((Control)BtnAction).Size = new Size(41, 40);
		((Control)BtnAction).TabIndex = 4;
		((ButtonBase)BtnAction).UseVisualStyleBackColor = true;
		((Control)BtnAction).Click += BtnAction_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((UserControl)this).BorderStyle = (BorderStyle)1;
		((Control)this).Controls.Add((Control)(object)BtnAction);
		((Control)this).Controls.Add((Control)(object)lblShortMessage);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Controls.Add((Control)(object)lblTitle);
		((Control)this).Name = "UCJobLevelChecker";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(400, 40);
		((UserControl)this).Load += UCJobLevelChecker_Load;
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

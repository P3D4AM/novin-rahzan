using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.UC;

public class UcReturnedSavedFuncInfo : UserControl, IReturnedSaveFuncInfo, INpDisposable, IDisposable
{
	private CancellationTokenSource _retToken = new CancellationTokenSource();

	private ReturnedSaveFuncInfo returnedFuncInfo;

	private bool _isDisposed;

	private IContainer components;

	private PictureBox pictureBox1;

	private Label label1;

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

	public ReturnedSaveFuncInfo ReturnedFuncInfo
	{
		get
		{
			return returnedFuncInfo;
		}
		set
		{
			if (value != null)
			{
				returnedFuncInfo = value;
				if (value.HasError)
				{
					AddErrorAsync(value.ErrorMessage);
				}
				else if (value.HasWarning)
				{
					AddWarningAsync(value.WarningMessage);
				}
				else if (value.HasInformation)
				{
					AddInformationAsync(value.InformationMessage);
				}
				else
				{
					ResetResults();
				}
			}
		}
	}

	public UcReturnedSavedFuncInfo()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private async Task ResetResults()
	{
		_retToken?.Cancel();
		_retToken = new CancellationTokenSource(60000);
		CancellationToken token = _retToken.Token;
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
		((Control)this).Invoke((Delegate)(Action)delegate
		{
			try
			{
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					((Control)label1).Text = "";
					pictureBox1.Image = null;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResetResults", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcReturnedSavedFuncInfo.cs", 72);
			}
		});
	}

	public async Task AddErrorAsync(string errorMessage, string key = "")
	{
		try
		{
			if (!string.IsNullOrEmpty(errorMessage))
			{
				_retToken?.Cancel();
				_retToken = new CancellationTokenSource(60000);
				CancellationToken tkn = _retToken.Token;
				Task.Run(() => ShowMessageAsync(errorMessage, tkn));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcReturnedSavedFuncInfo.cs", 86);
		}
	}

	public async Task AddWarningAsync(string warningMessage, string key = "")
	{
		try
		{
			if (!string.IsNullOrEmpty(warningMessage))
			{
				_retToken?.Cancel();
				_retToken = new CancellationTokenSource(60000);
				CancellationToken tkn = _retToken.Token;
				Task.Run(() => ShowMessageAsync(warningMessage, tkn));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcReturnedSavedFuncInfo.cs", 99);
		}
	}

	public async Task AddInformationAsync(string message, string key = "")
	{
		try
		{
			if (!string.IsNullOrEmpty(message))
			{
				_retToken?.Cancel();
				_retToken = new CancellationTokenSource(60000);
				CancellationToken tkn = _retToken.Token;
				Task.Run(() => ShowMessageAsync(message, tkn));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcReturnedSavedFuncInfo.cs", 112);
		}
	}

	private async Task ShowMessageAsync(string message, CancellationToken token)
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
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)label1).Text = message;
						pictureBox1.Image = null;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ShowMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcReturnedSavedFuncInfo.cs", 135);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcReturnedSavedFuncInfo.cs", 138);
		}
	}

	public void Dispose()
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			((Component)this).Dispose();
		}
	}

	private void label1_Click(object sender, EventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		this.ShowReturnedSavedFuncInfoForUserControl(ReturnedFuncInfo, "پیغام سیستم", "label1_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcReturnedSavedFuncInfo.cs");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		pictureBox1 = new PictureBox();
		label1 = new Label();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)pictureBox1).Anchor = (AnchorStyles)7;
		((Control)pictureBox1).Location = new Point(0, 0);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(55, 29);
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		((Control)label1).Anchor = (AnchorStyles)15;
		((Control)label1).Location = new Point(61, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(478, 28);
		((Control)label1).TabIndex = 1;
		((Control)label1).Text = "label1";
		label1.TextAlign = (ContentAlignment)64;
		((Control)label1).Click += label1_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Name = "UcReturnedSavedFuncInfo";
		((Control)this).Size = new Size(539, 28);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

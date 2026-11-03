using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.FormControls;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.UC;

public class UcGsmStatus : UserControl, INpDisposable, IDisposable
{
	private bool _isDisposed;

	private IContainer components;

	private SplitContainer splitContainer1;

	private BlinkingLabel blinkingLabel1;

	private RichTextBox richTextBox1;

	private Label label2;

	private Label label1;

	private Label label3;

	public int MaxLineRichText { get; set; } = 20;


	public string SoundFileDirectory { get; set; }

	public ReturnedSaveFuncInfo ReturnedSaveFunc
	{
		set
		{
			try
			{
				foreach (string information in value.InformationList)
				{
					WriteMessageAsync(information);
				}
				foreach (string warning in value.WarningList)
				{
					WriteMessageAsync(warning);
				}
				foreach (string error in value.ErrorList)
				{
					WriteMessageAsync(error, showInLable: true);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReturnedSaveFunc", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcGsmStatus.cs", 87);
			}
		}
	}

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

	public async Task SetLableSimNumberAsync(string value)
	{
		try
		{
			CancellationToken token = new CancellationTokenSource(10000).Token;
			while (!IsDisposed && !((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
			}
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				((Control)this).Invoke((Delegate)(Action)delegate
				{
					((Control)label1).Text = value;
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetLableSimNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcGsmStatus.cs", 36);
		}
	}

	public async Task SetLableCreaditAsync(string value)
	{
		try
		{
			CancellationToken token = new CancellationTokenSource(10000).Token;
			while (!IsDisposed && !((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
			}
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				((Control)this).Invoke((Delegate)(Action)delegate
				{
					((Control)label2).Text = value;
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetLableCreaditAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcGsmStatus.cs", 54);
		}
	}

	public async Task SetLableOperatorNameAsync(string value)
	{
		try
		{
			CancellationToken token = new CancellationTokenSource(10000).Token;
			while (!IsDisposed && !((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
			}
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				((Control)this).Invoke((Delegate)(Action)delegate
				{
					((Control)label3).Text = value;
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetLableOperatorNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcGsmStatus.cs", 69);
		}
	}

	public UcGsmStatus()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	public async Task ResetAsync()
	{
		CancellationToken token = new CancellationTokenSource(60000).Token;
		while (!IsDisposed && !((Control)this).IsHandleCreated)
		{
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
					((Control)richTextBox1).ResetText();
					((Control)blinkingLabel1).ResetText();
					((Control)label1).ResetText();
					((Control)label2).ResetText();
					((Control)label3).ResetText();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResetAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcGsmStatus.cs", 114);
			}
		});
	}

	public async Task WriteMessageAsync(string msg, bool showInLable = false, string soundFileName = "", int repeatPlaySound = 1)
	{
		try
		{
			if (msg.Trim().Length <= 0)
			{
				return;
			}
			CancellationToken token = new CancellationTokenSource(60000).Token;
			while (!((Control)this).IsHandleCreated)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(500);
			}
			string finalMsg = "";
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					finalMsg = ((Control)richTextBox1).Text;
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "WriteMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcGsmStatus.cs", 136);
				}
			});
			string[] array = finalMsg.Split(new char[1] { '\n' });
			if (array.Length != 0 && array[0].Contains(msg))
			{
				return;
			}
			if (array.Length < MaxLineRichText)
			{
				finalMsg = msg + " \n " + finalMsg;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(msg);
				for (int i = 0; i < 20; i++)
				{
					stringBuilder.Append(array[i]);
				}
				finalMsg = stringBuilder.ToString();
			}
			if (IsDisposed)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					((Control)richTextBox1).Text = finalMsg;
					if (showInLable)
					{
						((Control)blinkingLabel1).Text = msg;
						((Control)blinkingLabel1).Enabled = true;
					}
					if (!string.IsNullOrEmpty(soundFileName))
					{
						PlaySoundAsync(soundFileName, repeatPlaySound);
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "WriteMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcGsmStatus.cs", 175);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WriteMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcGsmStatus.cs", 180);
		}
	}

	private async Task PlaySoundAsync(string soundFileName, int repeatCount)
	{
		try
		{
			SoundPlayer player = new SoundPlayer();
			if (string.IsNullOrEmpty(SoundFileDirectory) || soundFileName.IndexOf("\\") > 0)
			{
				player.SoundLocation = soundFileName;
			}
			else
			{
				player.SoundLocation = Path.Combine(SoundFileDirectory, soundFileName);
			}
			if (!File.Exists(player.SoundLocation))
			{
				MessageBox.Show("Mp3 file does not exist in " + player.SoundLocation);
				return;
			}
			for (int i = 0; i < repeatCount; i++)
			{
				player.Play();
				await Task.Delay(3000);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PlaySoundAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcGsmStatus.cs", 204);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			components?.Dispose();
			((Component)this).Dispose();
		}
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		splitContainer1 = new SplitContainer();
		label2 = new Label();
		label1 = new Label();
		richTextBox1 = new RichTextBox();
		blinkingLabel1 = new BlinkingLabel();
		label3 = new Label();
		((ISupportInitialize)splitContainer1).BeginInit();
		((Control)splitContainer1.Panel1).SuspendLayout();
		((Control)splitContainer1.Panel2).SuspendLayout();
		((Control)splitContainer1).SuspendLayout();
		((Control)this).SuspendLayout();
		splitContainer1.Dock = (DockStyle)5;
		((Control)splitContainer1).Location = new Point(0, 0);
		((Control)splitContainer1).Name = "splitContainer1";
		splitContainer1.Orientation = (Orientation)0;
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)label3);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)label2);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)label1);
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)blinkingLabel1);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)richTextBox1);
		((Control)splitContainer1).Size = new Size(467, 350);
		splitContainer1.SplitterDistance = 63;
		((Control)splitContainer1).TabIndex = 1;
		((Control)label2).Anchor = (AnchorStyles)7;
		((Control)label2).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label2).ForeColor = Color.FromArgb(0, 64, 0);
		((Control)label2).Location = new Point(3, 22);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(194, 16);
		((Control)label2).TabIndex = 2;
		((Control)label1).Anchor = (AnchorStyles)7;
		((Control)label1).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.Blue;
		((Control)label1).Location = new Point(3, 6);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(194, 16);
		((Control)label1).TabIndex = 1;
		((Control)richTextBox1).Dock = (DockStyle)5;
		((Control)richTextBox1).Location = new Point(0, 0);
		((Control)richTextBox1).Name = "richTextBox1";
		((Control)richTextBox1).RightToLeft = (RightToLeft)1;
		((Control)richTextBox1).Size = new Size(467, 283);
		((Control)richTextBox1).TabIndex = 1;
		((Control)richTextBox1).Text = "";
		((Control)blinkingLabel1).Anchor = (AnchorStyles)15;
		blinkingLabel1.BackColor2 = Color.Red;
		((Control)blinkingLabel1).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)blinkingLabel1).Location = new Point(203, 0);
		((Control)blinkingLabel1).Name = "blinkingLabel1";
		((Control)blinkingLabel1).RightToLeft = (RightToLeft)1;
		((Control)blinkingLabel1).Size = new Size(261, 63);
		((Control)blinkingLabel1).TabIndex = 0;
		((Label)blinkingLabel1).TextAlign = (ContentAlignment)32;
		((Control)label3).Anchor = (AnchorStyles)7;
		((Control)label3).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label3).ForeColor = Color.Maroon;
		((Control)label3).Location = new Point(3, 38);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(194, 16);
		((Control)label3).TabIndex = 3;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)splitContainer1);
		((Control)this).Name = "UcGsmStatus";
		((Control)this).Size = new Size(467, 350);
		((Control)splitContainer1.Panel1).ResumeLayout(false);
		((Control)splitContainer1.Panel2).ResumeLayout(false);
		((ISupportInitialize)splitContainer1).EndInit();
		((Control)splitContainer1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.FormControls;

public sealed class BlinkingLabel : Label
{
	private bool _isFirstColor;

	private CancellationTokenSource _tokenSource = new CancellationTokenSource();

	public Color BackColor2 { get; set; } = Color.Red;


	private Color BackColor1 { get; set; }

	public bool Blinking
	{
		set
		{
			_tokenSource?.Cancel();
			_tokenSource = new CancellationTokenSource(60000);
			CancellationToken token = _tokenSource.Token;
			if (value)
			{
				Task.Run(async delegate
				{
					while (!((Control)this).IsHandleCreated && !IsDisposed && !token.IsCancellationRequested)
					{
						await Task.Delay(100);
					}
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)this).Invoke((Delegate)(Action)async delegate
						{
							try
							{
								while (!token.IsCancellationRequested)
								{
									await Task.Delay(500);
									((Control)this).BackColor = (_isFirstColor ? BackColor1 : BackColor2);
									_isFirstColor = !_isFirstColor;
								}
							}
							catch (Exception ex2)
							{
								WebErrorLog.Instance.StartLog(ex2, "", "Blinking", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\BlinkingLabel.cs", 44);
							}
						});
					}
				});
				return;
			}
			Task.Run(async delegate
			{
				while (!((Control)this).IsHandleCreated && !IsDisposed && !token.IsCancellationRequested)
				{
					await Task.Delay(100);
				}
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					((Control)this).Invoke((Delegate)(Action)async delegate
					{
						try
						{
							((Control)this).BackColor = BackColor2;
						}
						catch (Exception ex)
						{
							WebErrorLog.Instance.StartLog(ex, "", "Blinking", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\BlinkingLabel.cs", 63);
						}
					});
				}
			});
		}
	}

	private bool IsDisposed { get; set; }

	public BlinkingLabel()
	{
		BackColor1 = ((Control)this).BackColor;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_tokenSource?.Cancel();
				((Label)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\BlinkingLabel.cs", 84);
		}
	}
}

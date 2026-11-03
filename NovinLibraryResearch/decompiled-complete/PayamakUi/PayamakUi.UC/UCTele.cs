using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PayamakSettings;
using Tele;

namespace PayamakUi.UC;

public class UCTele : UserControl, IHasContext
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static EndCallEventHandler _003C0_003E___Phone_EndCall;

		public static PlayingFinishedEventHandler _003C1_003E___Phone_PlayingFinished;
	}

	private static bool _PlayingVoiceFinished;

	private static NovinPhone __Phone;

	private static DateTime LastRingTime;

	private IContainer components;

	internal Button Button1;

	internal TextBox TextBox1;

	internal TextBox TextBox2;

	internal Timer Timer1;

	private static NovinPhone _Phone
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		get
		{
			return __Phone;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			if (__Phone != null)
			{
				NovinPhone obj = __Phone;
				object obj2 = _003C_003EO._003C0_003E___Phone_EndCall;
				if (obj2 == null)
				{
					EndCallEventHandler val = _Phone_EndCall;
					_003C_003EO._003C0_003E___Phone_EndCall = val;
					obj2 = (object)val;
				}
				obj.EndCall -= (EndCallEventHandler)obj2;
				NovinPhone obj3 = __Phone;
				object obj4 = _003C_003EO._003C1_003E___Phone_PlayingFinished;
				if (obj4 == null)
				{
					PlayingFinishedEventHandler val2 = _Phone_PlayingFinished;
					_003C_003EO._003C1_003E___Phone_PlayingFinished = val2;
					obj4 = (object)val2;
				}
				obj3.PlayingFinished -= (PlayingFinishedEventHandler)obj4;
			}
			__Phone = value;
			if (__Phone != null)
			{
				NovinPhone obj5 = __Phone;
				object obj6 = _003C_003EO._003C0_003E___Phone_EndCall;
				if (obj6 == null)
				{
					EndCallEventHandler val3 = _Phone_EndCall;
					_003C_003EO._003C0_003E___Phone_EndCall = val3;
					obj6 = (object)val3;
				}
				obj5.EndCall += (EndCallEventHandler)obj6;
				NovinPhone obj7 = __Phone;
				object obj8 = _003C_003EO._003C1_003E___Phone_PlayingFinished;
				if (obj8 == null)
				{
					PlayingFinishedEventHandler val4 = _Phone_PlayingFinished;
					_003C_003EO._003C1_003E___Phone_PlayingFinished = val4;
					obj8 = (object)val4;
				}
				obj7.PlayingFinished += (PlayingFinishedEventHandler)obj8;
			}
		}
	}

	public string ContextGuid { get; set; }

	static UCTele()
	{
		_PlayingVoiceFinished = true;
		LastRingTime = DateTime.Now.AddDays(-5.0);
		_Phone = null;
	}

	public UCTele()
	{
		try
		{
			InitializeComponent();
			LoadDataAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCTele.cs", 65);
		}
	}

	private async Task LoadDataAsync()
	{
		_ = 2;
		try
		{
			bool autoConnect = await CLSDialUp.GetAutoConnectAsync();
			string numberPort2 = $"{await CLSDialUp.GetNumberPortAsync()}";
			if (autoConnect && (numberPort2?.ParseToLong(0L) ?? 0) > 0)
			{
				numberPort2 = "COM" + numberPort2.ToString();
				int num = int.Parse((await CLSDialUp.GetSpeedPortAsync()).ToString());
				_Phone = new NovinPhone(numberPort2, (long)num, (short)8, (StopBits)1, (Parity)0, (Handshake)0, (short)3);
			}
			else
			{
				((Control)this).Hide();
				((Control)this).Visible = false;
				_Phone = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCTele.cs", 90);
		}
	}

	public static async Task<bool> ConnectAync()
	{
		_ = 1;
		try
		{
			string NumberPort = $"COM{await CLSDialUp.GetNumberPortAsync()}";
			long num = await CLSDialUp.GetSpeedPortAsync();
			if (_Phone == null)
			{
				_Phone = new NovinPhone(NumberPort, (long)(int)num, (short)8, (StopBits)1, (Parity)0, (Handshake)0, (short)3);
			}
			else
			{
				_Phone = new NovinPhone(NumberPort, (long)(int)num, (short)8, (StopBits)1, (Parity)0, (Handshake)0, (short)3);
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ConnectAync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCTele.cs", 108);
			return false;
		}
	}

	private static void _Phone_EndCall(object sender, string Number)
	{
		try
		{
			_PlayingVoiceFinished = true;
			LastRingTime = DateTime.Now.AddDays(-5.0);
			_Phone.StopPlaying = true;
			_Phone.ActivateCallerID();
			_Phone.AutoAnswerConfiguration();
			_PlayingVoiceFinished = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_Phone_EndCall", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCTele.cs", 126);
		}
	}

	private static void _Phone_PlayingFinished(object sender)
	{
		try
		{
			_PlayingVoiceFinished = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_Phone_PlayingFinished", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCTele.cs", 138);
		}
	}

	private static bool SabteSefaresh()
	{
		return false;
	}

	public static bool UserNameMenu(int err_Count = 0)
	{
		try
		{
			if (err_Count > 1)
			{
				_Phone.HangUp();
				return false;
			}
			_PlayingVoiceFinished = false;
			_Phone.Play_Wave("D:\\PhoneVoice\\MenuVoices\\LoginMenu\\Welcome.wav");
			for (int i = 1; i <= 5000; i++)
			{
				Application.DoEvents();
				if (_PlayingVoiceFinished)
				{
					break;
				}
			}
			if (CLSNumberDetector.GetInput(_Phone, (short)5) == "100")
			{
				return true;
			}
			_Phone.Play_Wave("D:\\PhoneVoice\\MenuVoices\\LoginMenu\\Err_UserCode.wav");
			return UserNameMenu(err_Count + 1);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UserNameMenu", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCTele.cs", 216);
			return false;
		}
	}

	public static bool PasswordMenu(int err_Count = 0)
	{
		try
		{
			if (err_Count > 2)
			{
				_Phone.HangUp();
				return false;
			}
			_PlayingVoiceFinished = false;
			_Phone.Play_Wave("D:\\PhoneVoice\\MenuVoices\\LoginMenu\\EnterPass.wav");
			for (int i = 1; i <= 5000; i++)
			{
				if (_PlayingVoiceFinished)
				{
					break;
				}
			}
			if (CLSNumberDetector.GetInput(_Phone, (short)5) == "100")
			{
				return true;
			}
			_Phone.Play_Wave("D:\\PhoneVoice\\MenuVoices\\LoginMenu\\Err_UserPass.wav");
			return PasswordMenu(err_Count + 1);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PasswordMenu", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCTele.cs", 252);
			return false;
		}
	}

	private void Button1_Click(object sender, EventArgs e)
	{
		try
		{
			_Phone.ExecCommnad(((Control)TextBox1).Text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Button1_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCTele.cs", 265);
		}
	}

	private void Timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			if (_Phone == null)
			{
				Timer1.Enabled = false;
				return;
			}
			Timer1.Interval = 3000;
			if (_Phone.LastTerminalMessage != "")
			{
				TextBox textBox = TextBox2;
				((Control)textBox).Text = ((Control)textBox).Text + Environment.NewLine + _Phone.LastTerminalMessage;
				_Phone.LastTerminalMessage = "";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Timer1_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCTele.cs", 286);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		components = new Container();
		Button1 = new Button();
		TextBox1 = new TextBox();
		TextBox2 = new TextBox();
		Timer1 = new Timer(components);
		((Control)this).SuspendLayout();
		((Control)Button1).Anchor = (AnchorStyles)14;
		((Control)Button1).Location = new Point(0, 326);
		((Control)Button1).Name = "Button1";
		((Control)Button1).Size = new Size(445, 42);
		((Control)Button1).TabIndex = 0;
		((Control)Button1).Text = "Call";
		((ButtonBase)Button1).UseVisualStyleBackColor = true;
		((Control)Button1).Click += Button1_Click;
		((Control)TextBox1).Anchor = (AnchorStyles)13;
		((Control)TextBox1).Location = new Point(3, 3);
		((Control)TextBox1).Name = "TextBox1";
		((Control)TextBox1).Size = new Size(439, 20);
		((Control)TextBox1).TabIndex = 1;
		((Control)TextBox2).Anchor = (AnchorStyles)15;
		((Control)TextBox2).Location = new Point(3, 29);
		((TextBoxBase)TextBox2).Multiline = true;
		((Control)TextBox2).Name = "TextBox2";
		TextBox2.ScrollBars = (ScrollBars)3;
		((Control)TextBox2).Size = new Size(439, 291);
		((Control)TextBox2).TabIndex = 1;
		Timer1.Enabled = true;
		Timer1.Interval = 1000;
		Timer1.Tick += Timer1_Tick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(224, 224, 224);
		((Control)this).Controls.Add((Control)(object)TextBox2);
		((Control)this).Controls.Add((Control)(object)TextBox1);
		((Control)this).Controls.Add((Control)(object)Button1);
		((Control)this).Name = "UCTele";
		((Control)this).Size = new Size(445, 371);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}

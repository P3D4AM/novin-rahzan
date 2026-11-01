using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PacketParser.Entities;

namespace TimeAttendanceUI.Components;

public sealed class MarqueeLabel : Label
{
	private int CurrentPosition { get; set; }

	public Timer Timer { get; set; }

	public List<string> MessageList { get; set; }

	public List<Color> ColorList { get; set; }

	private int IndexMessage { get; set; }

	public int Speed { get; set; } = 15;


	public MarqueeLabel()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			((Label)this).UseCompatibleTextRendering = true;
			Timer = new Timer
			{
				Interval = Speed
			};
			Timer.Tick += Timer_Tick;
			Timer.Start();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Components\\MarqueeLabel.cs", 32);
		}
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		try
		{
			Timer.Interval = Speed;
			if (CurrentPosition >= 0)
			{
				if (MessageList == null || MessageList.Count == 0)
				{
					return;
				}
				IndexMessage %= MessageList.Count;
				((Control)this).Text = MessageList[IndexMessage];
				((Control)this).ForeColor = ((ColorList?.Count > IndexMessage) ? ColorList[IndexMessage] : Color.Black);
				IndexMessage++;
				if (((Control)this).Text.Trim() == "")
				{
					CurrentPosition = 0;
					return;
				}
				CurrentPosition = -((Control)this).Width;
			}
			else
			{
				CurrentPosition++;
			}
			((Control)this).Invalidate();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Timer_Tick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Components\\MarqueeLabel.cs", 64);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		try
		{
			e.Graphics.TranslateTransform((float)CurrentPosition, 0f);
			((Label)this).OnPaint(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnPaint", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Components\\MarqueeLabel.cs", 77);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing)
			{
				((Component)(object)Timer)?.Dispose();
			}
			Timer = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Components\\MarqueeLabel.cs", 93);
		}
	}
}

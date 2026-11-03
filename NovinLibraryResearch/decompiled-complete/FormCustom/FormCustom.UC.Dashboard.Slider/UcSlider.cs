using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using EntityCache.Business.Global;
using EntityCache.Mapping.Global;
using FormCustom.Properties;
using PacketParser.Entities;

namespace FormCustom.UC.Dashboard.Slider;

public class UcSlider : UserControl
{
	private IEnumerable<PostBusiness> _cls;

	private PostBusiness _current;

	private Timer _timer;

	private IContainer components;

	private PictureBox picSlider;

	private SplitContainer splitContainer1;

	private FlowLayoutPanel flowLayoutPanel;

	public string ContextGuid { get; private set; }

	public event Action<bool> OnShow;

	public async Task InitAsync(string contextGuid, ENSource source, CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested || ((Control)this).IsDisposed)
			{
				return;
			}
			ContextGuid = contextGuid;
			List<WebPost> list = await WebPost.GetAllBySourceAsync(source, token);
			if (token.IsCancellationRequested || ((Control)this).IsDisposed || list == null || !list.Any())
			{
				return;
			}
			_cls = (await PostMapper.Instance.MapListAsync(ContextGuid, list, null, "", token)).OrderBy((PostBusiness q) => q.Ordinal).Take(10)?.Where((PostBusiness q) => q.Status && q.Startdate <= DateTime.Now && q.EndDate >= DateTime.Now);
			if (token.IsCancellationRequested || ((Control)this).IsDisposed)
			{
				return;
			}
			while (!((Control)((Control)this).FindForm()).IsHandleCreated)
			{
				await Task.Delay(1000);
				if (((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (token.IsCancellationRequested || ((Control)this).IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Expected O, but got Unknown
				//IL_0170: Unknown result type (might be due to invalid IL or missing references)
				//IL_0175: Unknown result type (might be due to invalid IL or missing references)
				//IL_0185: Expected O, but got Unknown
				try
				{
					if (!token.IsCancellationRequested && !((Control)this).IsDisposed)
					{
						foreach (PostBusiness cl in _cls)
						{
							PictureBox val = new PictureBox
							{
								Tag = cl,
								SizeMode = (PictureBoxSizeMode)4,
								Size = new Size(30, 30),
								Image = ((cl.Image != null) ? Image.FromStream((Stream)new MemoryStream(cl.Image)) : null),
								Cursor = Cursors.Hand
							};
							((Control)flowLayoutPanel).Controls.Add((Control)(object)val);
							((Control)val).Click += BtnSlider_Click;
							this.OnShow?.Invoke(obj: true);
						}
						_current = _cls?.FirstOrDefault();
						if (_current != null && _current.Image != null)
						{
							using MemoryStream memoryStream = new MemoryStream(_current.Image);
							picSlider.Image = Image.FromStream((Stream)memoryStream);
						}
						SetSizeWhenSelect();
						_timer = new Timer
						{
							Interval = 5000
						};
						_timer.Tick += Timer_Tick;
						_timer.Start();
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\UcSlider.cs", 78);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\UcSlider.cs", 81);
		}
	}

	private void SetSizeWhenSelect()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		try
		{
			foreach (PictureBox item in (ArrangedElementCollection)((Control)flowLayoutPanel).Controls)
			{
				PictureBox val = item;
				if (((Control)val).Tag == _current)
				{
					((Control)val).Size = new Size(40, 40);
					val.BorderStyle = (BorderStyle)1;
				}
				else
				{
					((Control)val).Size = new Size(30, 30);
					val.BorderStyle = (BorderStyle)0;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSizeWhenSelect", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\UcSlider.cs", 101);
		}
	}

	public UcSlider()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void BtnSlider_Click(object sender, EventArgs e)
	{
		try
		{
			PictureBox val = (PictureBox)((sender is PictureBox) ? sender : null);
			if (val == null || !(((Control)val).Tag is PostBusiness postBusiness))
			{
				return;
			}
			_current = postBusiness;
			if (postBusiness.Image == null)
			{
				return;
			}
			using MemoryStream memoryStream = new MemoryStream(postBusiness.Image);
			picSlider.Image = Image.FromStream((Stream)memoryStream);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnSlider_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\UcSlider.cs", 128);
		}
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		try
		{
			IEnumerable<PostBusiness> cls = _cls;
			if (cls == null || !cls.Any() || ((Control)this).IsDisposed || ((Control)this).FindForm() == null || ((Control)((Control)this).FindForm()).IsDisposed)
			{
				_timer.Tick -= Timer_Tick;
				((Component)(object)_timer)?.Dispose();
				_timer = null;
				return;
			}
			int num = _cls.ToList().IndexOf(_current);
			num = (num + 1) % _cls.Count();
			_current = _cls.ElementAt(num);
			SetSizeWhenSelect();
			if (_current == null || _current.Image == null)
			{
				return;
			}
			using MemoryStream memoryStream = new MemoryStream(_current.Image);
			picSlider.Image = Image.FromStream((Stream)memoryStream);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Timer_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\UcSlider.cs", 152);
		}
	}

	private void picSlider_Click(object sender, EventArgs e)
	{
		try
		{
			if (!string.IsNullOrEmpty(_current.Link) && _current != null)
			{
				IEnumerable<PostBusiness> cls = _cls;
				if (cls != null && cls.Any())
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = _current.Link,
						UseShellExecute = true
					});
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picSlider_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\UcSlider.cs", 165);
		}
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		splitContainer1 = new SplitContainer();
		picSlider = new PictureBox();
		flowLayoutPanel = new FlowLayoutPanel();
		((ISupportInitialize)splitContainer1).BeginInit();
		((Control)splitContainer1.Panel1).SuspendLayout();
		((Control)splitContainer1.Panel2).SuspendLayout();
		((Control)splitContainer1).SuspendLayout();
		((ISupportInitialize)picSlider).BeginInit();
		((Control)this).SuspendLayout();
		((Control)splitContainer1).BackgroundImage = (Image)(object)FormCustom.Properties.Resources.RightSide;
		splitContainer1.Dock = (DockStyle)5;
		((Control)splitContainer1).Location = new Point(0, 0);
		((Control)splitContainer1).Name = "splitContainer1";
		splitContainer1.Orientation = (Orientation)0;
		((Control)splitContainer1.Panel1).BackgroundImage = (Image)(object)FormCustom.Properties.Resources.RightSide;
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)picSlider);
		((Control)splitContainer1.Panel1).RightToLeft = (RightToLeft)1;
		splitContainer1.Panel1MinSize = 3;
		((Control)splitContainer1.Panel2).BackgroundImage = (Image)(object)FormCustom.Properties.Resources.RightSide;
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)flowLayoutPanel);
		((Control)splitContainer1.Panel2).Padding = new Padding(0, 0, 0, 7);
		((Control)splitContainer1.Panel2).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1).Size = new Size(609, 520);
		splitContainer1.SplitterDistance = 491;
		((Control)splitContainer1).TabIndex = 2;
		((Control)picSlider).BackgroundImage = (Image)(object)FormCustom.Properties.Resources.RightSide;
		((Control)picSlider).Cursor = Cursors.Hand;
		((Control)picSlider).Dock = (DockStyle)5;
		((Control)picSlider).Location = new Point(0, 0);
		((Control)picSlider).Margin = new Padding(0);
		((Control)picSlider).Name = "picSlider";
		((Control)picSlider).Size = new Size(609, 491);
		picSlider.SizeMode = (PictureBoxSizeMode)1;
		picSlider.TabIndex = 0;
		picSlider.TabStop = false;
		((Control)picSlider).Click += picSlider_Click;
		((Control)flowLayoutPanel).Anchor = (AnchorStyles)12;
		((Control)flowLayoutPanel).AutoSize = true;
		((Panel)flowLayoutPanel).AutoSizeMode = (AutoSizeMode)0;
		((Control)flowLayoutPanel).Location = new Point(490, -2);
		((Control)flowLayoutPanel).Margin = new Padding(0, 0, 0, 3);
		((Control)flowLayoutPanel).Name = "flowLayoutPanel";
		((Control)flowLayoutPanel).Padding = new Padding(0, 0, 0, 3);
		((Control)flowLayoutPanel).RightToLeft = (RightToLeft)0;
		((Control)flowLayoutPanel).Size = new Size(0, 3);
		((Control)flowLayoutPanel).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)FormCustom.Properties.Resources.RightSide;
		((Control)this).Controls.Add((Control)(object)splitContainer1);
		((Control)this).Name = "UcSlider";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(609, 520);
		((Control)splitContainer1.Panel1).ResumeLayout(false);
		((Control)splitContainer1.Panel2).ResumeLayout(false);
		((Control)splitContainer1.Panel2).PerformLayout();
		((ISupportInitialize)splitContainer1).EndInit();
		((Control)splitContainer1).ResumeLayout(false);
		((ISupportInitialize)picSlider).EndInit();
		((Control)this).ResumeLayout(false);
	}
}

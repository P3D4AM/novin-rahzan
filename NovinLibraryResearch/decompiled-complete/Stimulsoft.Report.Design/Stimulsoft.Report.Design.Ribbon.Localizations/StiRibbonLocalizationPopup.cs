using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;

namespace Stimulsoft.Report.Design.Ribbon.Localizations;

public sealed class StiRibbonLocalizationPopup
{
	[ToolboxItem(false)]
	private class StiLocPanel : Panel
	{
		public StiLocPanel()
		{
			((Control)this).DoubleBuffered = true;
		}
	}

	private class StiLocItem
	{
		public string Text;

		public string Tag;

		public string Language;

		public StiLocItem(string text, string tag, string language)
		{
			Text = text;
			Tag = tag;
			Language = language;
		}

		public override string ToString()
		{
			return Text + ", " + Tag;
		}
	}

	public const int ItemWidth = 180;

	public const int ItemHeight = 38;

	private StiRibbonDesignerUserControl designer;

	private ButtonItem button;

	private StiPopup popup;

	private StiLocPanel panel;

	private int columns = 1;

	private int rows = 1;

	private List<StiLocItem> items = new List<StiLocItem>();

	private CircularProgress progressOperation;

	private bool isDisposed;

	private List<Control> allControls = new List<Control>();

	public StiRibbonLocalizationPopup(StiRibbonDesignerUserControl designer, ButtonItem button)
	{
		this.designer = designer;
		this.button = button;
	}

	private void Build()
	{
		string directory = StiOptions.Localization.GetDirectory();
		DirectoryInfo directoryInfo = new DirectoryInfo(directory);
		if (!directoryInfo.Exists)
		{
			return;
		}
		FileInfo[] files = directoryInfo.GetFiles("*.xml");
		FileInfo[] array = files;
		foreach (FileInfo fileInfo in array)
		{
			if (!fileInfo.Name.Contains(".ext") && !fileInfo.Name.Contains("server."))
			{
				StiLocalization.GetParam(fileInfo.FullName, out var language, out var description, out var _, directory);
				if (!string.IsNullOrEmpty(language))
				{
					items.Add(new StiLocItem(description, fileInfo.Name, language));
				}
			}
		}
		int count = items.Count;
		if (count < 3)
		{
			columns = 1;
		}
		else if (count < 7)
		{
			columns = 2;
		}
		else
		{
			columns = 3;
		}
		rows = (int)Math.Ceiling((decimal)count / (decimal)columns);
		items = items.OrderBy((StiLocItem x) => x.Text).ToList();
	}

	private void View()
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		try
		{
			int num = StiScale.XXI(2.0);
			int num2 = StiScale.YYI(2.0);
			int num3 = 0;
			StiLocButton stiLocButton = new StiLocButton(StiLocalization.Get("FormOptions", "Default"), null);
			((Control)stiLocButton).Width = StiScale.XXI(180.0);
			((Control)stiLocButton).Height = StiScale.YYI(38.0);
			stiLocButton.IsChecked = StiLocalization.Localization == string.Empty;
			StiLocButton stiLocButton2 = stiLocButton;
			stiLocButton2.ButtonClick += LocItem_Click;
			((Control)stiLocButton2).Location = new Point(num, num2);
			allControls.Add((Control)(object)stiLocButton2);
			num3++;
			num += StiScale.XXI(182.0);
			num2 += StiScale.YYI(40.0);
			if (items.Count == 0)
			{
				return;
			}
			Panel val = new Panel
			{
				Width = ((Control)panel).Width - StiScale.XXI(2.0),
				Height = StiScale.YYI(5.0),
				Location = new Point(StiScale.XXI(1.0), num2)
			};
			((Control)val).Paint += new PaintEventHandler(PanelSeparator_Paint);
			allControls.Add((Control)(object)val);
			if (isDisposed)
			{
				return;
			}
			num3 = 0;
			num = StiScale.XXI(2.0);
			num2 += StiScale.XXI(5.0);
			int num4 = num2;
			foreach (StiLocItem item in items)
			{
				if (item.Tag == "en.xml")
				{
					StiLocButton stiLocButton3 = new StiLocButton(item.Text, item.Tag);
					((Control)stiLocButton3).Width = StiScale.XXI(180.0);
					((Control)stiLocButton3).Height = StiScale.YYI(38.0);
					((Control)stiLocButton3).Location = new Point(StiScale.XXI(184.0), StiScale.YYI(2.0));
					stiLocButton3.IsChecked = item.Tag == StiLocalization.Localization;
					StiLocButton stiLocButton4 = stiLocButton3;
					stiLocButton4.ButtonClick += LocItem_Click;
					allControls.Add((Control)(object)stiLocButton4);
				}
				if (num3 == rows)
				{
					num3 = 0;
					num += StiScale.XXI(180.0);
					num2 = num4;
				}
				StiLocButton obj = new StiLocButton(item.Text, item.Tag)
				{
					IsChecked = (item.Tag == StiLocalization.Localization)
				};
				((Control)obj).Width = StiScale.XXI(180.0);
				((Control)obj).Height = StiScale.YYI(38.0);
				((Control)obj).Location = new Point(num, num2);
				StiLocButton stiLocButton5 = obj;
				stiLocButton5.ButtonClick += LocItem_Click;
				if (isDisposed)
				{
					break;
				}
				allControls.Add((Control)(object)stiLocButton5);
				num3++;
				num2 += StiScale.XXI(38.0);
			}
		}
		catch
		{
		}
	}

	public void Show()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		Rectangle screenRectangle = button.ScreenRectangle;
		Build();
		StiLocPanel stiLocPanel = new StiLocPanel();
		((Control)stiLocPanel).BackColor = Color.White;
		panel = stiLocPanel;
		((ScrollableControl)panel).Scroll += new ScrollEventHandler(Panel_Scroll);
		int num = screenRectangle.Bottom + StiScale.I2;
		int num2 = StiScale.YYI(50.0) + items.Count / columns * StiScale.YYI(38.0);
		if (items.Count % columns > 0)
		{
			num2 += StiScale.YYI(38.0);
		}
		if (items.Count == 0)
		{
			num2 = StiScale.YYI(42.0);
		}
		int num3 = Screen.PrimaryScreen.Bounds.Height - num - 30;
		int num4 = 0;
		if (num2 > num3)
		{
			((ScrollableControl)panel).AutoScroll = true;
			num2 = num3;
			num4 += 24;
		}
		((Control)panel).Width = StiScale.XXI(180 * columns + num4 + 4 + (columns - 1) * 2);
		((Control)panel).Height = num2;
		((Control)panel).Paint += new PaintEventHandler(Panel_Paint);
		popup = new StiPopup((Control)(object)panel);
		((ToolStripDropDown)popup).Closed += (ToolStripDropDownClosedEventHandler)delegate
		{
			isDisposed = true;
		};
		if (items.Count > 10)
		{
			CircularProgress circularProgress = new CircularProgress();
			((Control)circularProgress).BackColor = Color.White;
			((Control)circularProgress).Margin = new Padding(6);
			circularProgress.ProgressBarType = eCircularProgressType.Donut;
			circularProgress.ProgressColor = Color.SteelBlue;
			((Control)circularProgress).Size = new Size(StiScale.XXI(52.0), StiScale.YYI(52.0));
			circularProgress.Style = eDotNetBarStyle.OfficeXP;
			progressOperation = circularProgress;
			progressOperation.BackgroundStyle.CornerType = eCornerType.Square;
			progressOperation.Value = 0;
			progressOperation.IsRunning = true;
			((Control)progressOperation).Location = new Point((((Control)panel).Width - ((Control)progressOperation).Width) / 2, (((Control)panel).Height - ((Control)progressOperation).Height) / 2);
			((Control)panel).Controls.Add((Control)(object)progressOperation);
		}
		using (BackgroundWorker backgroundWorker = new BackgroundWorker())
		{
			backgroundWorker.DoWork += delegate
			{
				View();
			};
			backgroundWorker.RunWorkerCompleted += delegate
			{
				try
				{
					if (progressOperation != null)
					{
						progressOperation.IsRunning = false;
						((Control)panel).Controls.Remove((Control)(object)progressOperation);
					}
					((Control)panel).Controls.AddRange(allControls.ToArray());
				}
				catch
				{
				}
			};
			backgroundWorker.RunWorkerAsync();
		}
		((ToolStripDropDown)popup).Show(new Point(screenRectangle.Left + screenRectangle.Width - ((Control)popup).Width, num));
	}

	private void LocItem_Click(object sender, EventArgs e)
	{
		StiLocButton stiLocButton = (StiLocButton)sender;
		designer.SelectLanguage(((Control)stiLocButton).Tag as string);
		((Control)popup).Hide();
	}

	private void Panel_Scroll(object sender, ScrollEventArgs e)
	{
		((Control)panel).Invalidate();
	}

	private void Panel_Paint(object sender, PaintEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		Panel val = (Panel)sender;
		Rectangle clientRectangle = ((Control)val).ClientRectangle;
		e.Graphics.FillRectangle(Brushes.White, clientRectangle);
		Pen val2 = new Pen(Color.FromArgb(255, 198, 198, 198));
		try
		{
			e.Graphics.DrawRectangle(val2, 0, 0, ((Control)val).Width - 1, ((Control)val).Height - 1);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void PanelSeparator_Paint(object sender, PaintEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		Panel val = (Panel)sender;
		Pen val2 = new Pen(Color.FromArgb(255, 198, 198, 198));
		try
		{
			val2.DashStyle = (DashStyle)1;
			e.Graphics.DrawLine(val2, 0, 3, ((Control)val).Width, 3);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}

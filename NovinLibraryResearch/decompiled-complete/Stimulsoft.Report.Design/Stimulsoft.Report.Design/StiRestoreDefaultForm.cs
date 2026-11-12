using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.TaskScheduler;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design;

public class StiRestoreDefaultForm : Form
{
	internal abstract class StiCacheItem
	{
		protected StiRestoreDefaultForm form;

		protected long fullSize;

		protected Panel panel;

		private Panel separatorPanel;

		protected StiLabelUX labelName;

		protected Button buttonClear;

		public StiCacheItem(StiRestoreDefaultForm form)
		{
			this.form = form;
			Do();
		}

		protected abstract string GetPath();

		protected abstract void Do();

		public abstract int Init(Control parent);

		protected abstract void ProcessClear();

		protected abstract Image GetImage();

		protected void CreateDefaultPanel(Control parent, string text, bool addSeparator = true)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Expected O, but got Unknown
			if (addSeparator)
			{
				CreateSeparator(parent);
			}
			panel = new Panel
			{
				BackColor = Color.White,
				Dock = (DockStyle)1,
				Location = new Point(0, 0),
				Size = new Size(StiScale.XXI(200.0), StiScale.YYI(46.0))
			};
			parent.Controls.Add((Control)(object)panel);
			PictureBox val = new PictureBox
			{
				Location = new Point(StiScale.XXI(10.0), StiScale.YYI(10.0)),
				Size = new Size(StiScale.XXI(32.0), StiScale.YYI(32.0)),
				Image = GetImage()
			};
			((Control)panel).Controls.Add((Control)(object)val);
			StiLabelUX stiLabelUX = new StiLabelUX();
			((Label)stiLabelUX).AutoEllipsis = true;
			((Control)stiLabelUX).Location = new Point(StiScale.XXI(46.0), StiScale.YYI(12.0));
			((Control)stiLabelUX).Size = new Size(StiScale.XXI(200.0), StiScale.YYI(22.0));
			((Control)stiLabelUX).Text = text;
			((Label)stiLabelUX).TextAlign = (ContentAlignment)16;
			((Label)stiLabelUX).UseCompatibleTextRendering = true;
			labelName = stiLabelUX;
			((Control)panel).Controls.Add((Control)(object)labelName);
			buttonClear = new Button
			{
				Enabled = (fullSize > 0),
				Location = new Point(((Control)panel).Parent.Width - StiScale.XXI(95.0), StiScale.YYI(12.0)),
				Size = new Size(StiScale.XXI(75.0), StiScale.YYI(24.0)),
				TabIndex = 1,
				Text = StiLocalization.Get("Gui", "monthcalendar_clearbutton"),
				UseVisualStyleBackColor = true
			};
			((Control)buttonClear).Click += ButtonClear_Click;
			((Control)panel).Controls.Add((Control)(object)buttonClear);
		}

		protected void CreateSeparator(Control parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			separatorPanel = new Panel
			{
				BackColor = Color.White,
				Dock = (DockStyle)1,
				Location = new Point(0, 0),
				Size = new Size(StiScale.XXI(200.0), StiScale.YYI(1.0))
			};
			((Control)separatorPanel).Paint += new PaintEventHandler(Separator_Paint);
			parent.Controls.Add((Control)(object)separatorPanel);
		}

		protected string GetSize(long size)
		{
			if (size < 1024)
			{
				return $"{size} byte";
			}
			if (size < 1048576)
			{
				return $"{Math.Round((double)size / 1024.0, 2)} KB";
			}
			if (size < 1073741824)
			{
				return $"{Math.Round((double)size / 1048576.0, 2)} MB";
			}
			return $"{Math.Round((double)size / 1073741824.0, 2)} GB";
		}

		private void Separator_Paint(object sender, PaintEventArgs e)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			e.Graphics.FillRectangle(Brushes.White, ((Control)separatorPanel).ClientRectangle);
			Pen val = new Pen(Color.FromArgb(170, 170, 170));
			try
			{
				val.DashStyle = (DashStyle)1;
				e.Graphics.DrawLine(val, new Point(0, 0), new Point(((Control)separatorPanel).Width, 0));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		private void ButtonClear_Click(object sender, EventArgs e)
		{
			ProcessClear();
		}
	}

	internal sealed class StiDesignerSettingsCacheItem : StiCacheItem
	{
		public StiDesignerSettingsCacheItem(StiRestoreDefaultForm form)
			: base(form)
		{
		}

		protected override Image GetImage()
		{
			return StiReportDesignImages.RestoreDefault.DesignerSettings(StiImageSize.OneHalf);
		}

		protected override string GetPath()
		{
			return null;
		}

		protected override void Do()
		{
			fullSize = 1L;
		}

		public override int Init(Control parent)
		{
			CreateDefaultPanel(parent, "Designer Settings");
			return ((Control)panel).Bottom;
		}

		protected override void ProcessClear()
		{
			((Control)buttonClear).Enabled = false;
			form.IsDesignerSettingsChanged = true;
			try
			{
				string path = StiPath.GetPath(StiDesigner.GetDefaultRibbonDockingPanelsConfigPath());
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch
			{
			}
			try
			{
				string path = StiPath.GetPath(StiDesigner.GetDefaultDesignerConfigPath());
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch
			{
			}
			try
			{
				string path = StiPath.GetPath(StiDesigner.GetDefaultDockingPanelsConfigPath());
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch
			{
			}
			StiDesigner.DontSaveDockingPanelsConfig = true;
			StiDesigner.DontSaveDesignerConfig = true;
			StiSettings.Load();
			StiSettings.ClearKey("CloudDesigner", "ShowStandardToolbox");
			StiSettings.ClearKey("CloudDesigner", "ShowInsertTab");
			StiSettings.ClearKey("Cloud", "RequestChangesWhenSavingToCloud");
			StiSettings.ClearKey("Designer", "ShowDialogFormsV1");
			StiSettings.ClearKey("Viewer", "AnimationPlaybackType");
			StiSettings.ClearKey("Viewer", "AllowAnimation");
			StiSettings.ClearKey("Designer", "SimpleChartEditorByDefault");
			StiSettings.ClearKey("Designer", "ShowOldGaugeEditorV1");
			StiSettings.ClearKey("Designer", "BlocksScriptEditorByDefault");
			StiSettings.ClearKey("Designer", "StartScreen");
			StiSettings.ClearKey("StiDesigner", "NewReportDictionaryBehavior");
			StiSettings.Save();
		}
	}

	internal sealed class StiWindowsSettingsCacheItem : StiCacheItem
	{
		public StiWindowsSettingsCacheItem(StiRestoreDefaultForm form)
			: base(form)
		{
		}

		protected override Image GetImage()
		{
			return StiReportDesignImages.RestoreDefault.WindowsSettings(StiImageSize.OneHalf);
		}

		protected override string GetPath()
		{
			return StiWindowsSettings.GetSettingsPath();
		}

		protected override void Do()
		{
			string path = GetPath();
			if (File.Exists(path))
			{
				FileInfo fileInfo = new FileInfo(path);
				fullSize = fileInfo.Length;
			}
		}

		public override int Init(Control parent)
		{
			CreateDefaultPanel(parent, "Windows Settings: " + GetSize(fullSize));
			return ((Control)panel).Bottom;
		}

		protected override void ProcessClear()
		{
			((Control)buttonClear).Enabled = false;
			try
			{
				string path = GetPath();
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				fullSize = 0L;
				((Control)labelName).Text = "Windows Settings: " + GetSize(fullSize);
			}
			catch
			{
			}
		}
	}

	internal sealed class StiGisMapCacheItem : StiCacheItem
	{
		public StiGisMapCacheItem(StiRestoreDefaultForm form)
			: base(form)
		{
		}

		protected override Image GetImage()
		{
			return StiReportDesignImages.RestoreDefault.GisMap(StiImageSize.OneHalf);
		}

		protected override string GetPath()
		{
			try
			{
				string text = $"Stimulsoft{Path.DirectorySeparatorChar}GisMap";
				if (StiBaseOptions.FullTrust)
				{
					string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
					if (!string.IsNullOrEmpty(folderPath))
					{
						text = Path.Combine(folderPath, text);
					}
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
			catch
			{
				return null;
			}
		}

		protected override void Do()
		{
			string path = GetPath();
			if (path != null)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
				FileInfo[] array = files;
				foreach (FileInfo fileInfo in array)
				{
					fullSize += fileInfo.Length;
				}
			}
		}

		public override int Init(Control parent)
		{
			CreateDefaultPanel(parent, "Gis Map: " + GetSize(fullSize));
			return ((Control)panel).Bottom;
		}

		protected override void ProcessClear()
		{
			((Control)buttonClear).Enabled = false;
			try
			{
				string path = GetPath();
				if (Directory.Exists(path))
				{
					Directory.Delete(path, recursive: true);
				}
				Do();
				fullSize = 0L;
				((Control)labelName).Text = "Gis Map: " + GetSize(fullSize);
			}
			catch
			{
			}
		}
	}

	internal sealed class StiSchedulerCacheItem : StiCacheItem
	{
		private long logSize;

		private long smtpSettingsSize;

		private StiCheckBoxUX checkBoxLog;

		private StiCheckBoxUX checkBoxSmtpSettings;

		public StiSchedulerCacheItem(StiRestoreDefaultForm form)
			: base(form)
		{
		}

		protected override string GetPath()
		{
			try
			{
				string text = $"Stimulsoft{Path.DirectorySeparatorChar}Scheduler{Path.DirectorySeparatorChar}Logs";
				if (StiBaseOptions.FullTrust)
				{
					string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
					if (!string.IsNullOrEmpty(folderPath))
					{
						text = Path.Combine(folderPath, text);
					}
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
			catch
			{
				return null;
			}
		}

		protected override void Do()
		{
			fullSize = 0L;
			logSize = 0L;
			smtpSettingsSize = 0L;
			string path = GetPath();
			if (path != null)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
				FileInfo[] array = files;
				foreach (FileInfo fileInfo in array)
				{
					logSize += fileInfo.Length;
				}
			}
			fullSize = logSize;
			path = StiSmtpConfig.GetSettingsPath();
			if (path != null && File.Exists(path))
			{
				FileInfo fileInfo2 = new FileInfo(path);
				smtpSettingsSize = fileInfo2.Length;
			}
			fullSize += smtpSettingsSize;
		}

		public override int Init(Control parent)
		{
			CreateSeparator(parent);
			CreateDefaultPanel(parent, "Scheduler: " + GetSize(fullSize), addSeparator: false);
			int num = 40;
			StiCheckBoxUX stiCheckBoxUX = new StiCheckBoxUX();
			((Control)stiCheckBoxUX).Enabled = logSize > 0;
			((CheckBox)stiCheckBoxUX).Checked = true;
			((Control)stiCheckBoxUX).Location = new Point(StiScale.XXI(46.0), StiScale.YYI(num));
			((Control)stiCheckBoxUX).Size = new Size(StiScale.XXI(300.0), StiScale.YYI(20.0));
			((Control)stiCheckBoxUX).Text = "Logs: " + GetSize(logSize);
			checkBoxLog = stiCheckBoxUX;
			((Control)panel).Controls.Add((Control)(object)checkBoxLog);
			num += 24;
			StiCheckBoxUX stiCheckBoxUX2 = new StiCheckBoxUX();
			((Control)stiCheckBoxUX2).Enabled = smtpSettingsSize > 0;
			((CheckBox)stiCheckBoxUX2).Checked = true;
			((Control)stiCheckBoxUX2).Location = new Point(StiScale.XXI(46.0), StiScale.YYI(num));
			((Control)stiCheckBoxUX2).Size = new Size(StiScale.XXI(300.0), StiScale.YYI(20.0));
			((Control)stiCheckBoxUX2).Text = "Smtp Settings: " + GetSize(smtpSettingsSize);
			checkBoxSmtpSettings = stiCheckBoxUX2;
			((Control)panel).Controls.Add((Control)(object)checkBoxSmtpSettings);
			num += 24;
			((Control)panel).Height = StiScale.YYI(num + 8);
			((Control)panel).Controls.Add((Control)(object)buttonClear);
			return ((Control)panel).Bottom;
		}

		protected override Image GetImage()
		{
			return StiReportDesignImages.RestoreDefault.SchedulerLogs(StiImageSize.OneHalf);
		}

		protected override void ProcessClear()
		{
			((Control)buttonClear).Enabled = false;
			if (((Control)checkBoxLog).Enabled && ((CheckBox)checkBoxLog).Checked)
			{
				((Control)checkBoxLog).Enabled = false;
				((CheckBox)checkBoxLog).Checked = false;
				try
				{
					string path = GetPath();
					if (Directory.Exists(path))
					{
						Directory.Delete(path, recursive: true);
					}
				}
				catch
				{
				}
			}
			if (((Control)checkBoxSmtpSettings).Enabled && ((CheckBox)checkBoxSmtpSettings).Checked)
			{
				((Control)checkBoxSmtpSettings).Enabled = false;
				((CheckBox)checkBoxSmtpSettings).Checked = false;
				try
				{
					string settingsPath = StiSmtpConfig.GetSettingsPath();
					if (File.Exists(settingsPath))
					{
						File.Delete(settingsPath);
					}
					StiSmtpConfig.Clear();
					form.IsSmtpSettingsChanged = true;
				}
				catch
				{
				}
			}
			Do();
			((Control)labelName).Text = "Scheduler: " + GetSize(fullSize);
			((Control)checkBoxLog).Text = "Logs: " + GetSize(logSize);
			((Control)checkBoxSmtpSettings).Text = "Smtp Settings: " + GetSize(smtpSettingsSize);
			((Control)buttonClear).Enabled = fullSize > 0;
		}
	}

	internal sealed class StiThumbnailsLogsCacheItem : StiCacheItem
	{
		public StiThumbnailsLogsCacheItem(StiRestoreDefaultForm form)
			: base(form)
		{
		}

		protected override Image GetImage()
		{
			return StiReportDesignImages.RestoreDefault.Thumbnails(StiImageSize.OneHalf);
		}

		protected override string GetPath()
		{
			try
			{
				string text = $"Stimulsoft{Path.DirectorySeparatorChar}Cache";
				if (StiBaseOptions.FullTrust)
				{
					string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
					if (!string.IsNullOrEmpty(folderPath))
					{
						text = Path.Combine(folderPath, text);
					}
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
			catch
			{
				return null;
			}
		}

		protected override void Do()
		{
			string path = GetPath();
			if (path != null)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
				FileInfo[] array = files;
				foreach (FileInfo fileInfo in array)
				{
					fullSize += fileInfo.Length;
				}
			}
		}

		public override int Init(Control parent)
		{
			CreateDefaultPanel(parent, "Thumbnails: " + GetSize(fullSize));
			return ((Control)panel).Bottom;
		}

		protected override void ProcessClear()
		{
			((Control)buttonClear).Enabled = false;
			try
			{
				string path = GetPath();
				if (Directory.Exists(path))
				{
					Directory.Delete(path, recursive: true);
				}
				Do();
				fullSize = 0L;
				((Control)labelName).Text = "Thumbnails: " + GetSize(fullSize);
			}
			catch
			{
			}
		}
	}

	internal sealed class StiBlocklyLogsCacheItem : StiCacheItem
	{
		public StiBlocklyLogsCacheItem(StiRestoreDefaultForm form)
			: base(form)
		{
		}

		protected override Image GetImage()
		{
			return StiReportDesignImages.RestoreDefault.Blockly(StiImageSize.OneHalf);
		}

		protected override string GetPath()
		{
			try
			{
				string text = $"Stimulsoft{Path.DirectorySeparatorChar}WebView2";
				if (StiBaseOptions.FullTrust)
				{
					string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
					if (!string.IsNullOrEmpty(folderPath))
					{
						text = Path.Combine(folderPath, text);
					}
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
			catch
			{
				return null;
			}
		}

		protected override void Do()
		{
			string path = GetPath();
			if (path != null)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
				FileInfo[] array = files;
				foreach (FileInfo fileInfo in array)
				{
					fullSize += fileInfo.Length;
				}
			}
		}

		public override int Init(Control parent)
		{
			CreateDefaultPanel(parent, "Blockly: " + GetSize(fullSize));
			return ((Control)panel).Bottom;
		}

		protected override void ProcessClear()
		{
			((Control)buttonClear).Enabled = false;
			try
			{
				string path = GetPath();
				if (Directory.Exists(path))
				{
					Directory.Delete(path, recursive: true);
				}
				Do();
				fullSize = 0L;
				((Control)labelName).Text = "Blockly: " + GetSize(fullSize);
			}
			catch
			{
			}
		}
	}

	internal sealed class StiDataAdaptersLogsCacheItem : StiCacheItem
	{
		private Dictionary<string, long> datas = new Dictionary<string, long>();

		private List<CheckBox> checkBoxes = new List<CheckBox>();

		public StiDataAdaptersLogsCacheItem(StiRestoreDefaultForm form)
			: base(form)
		{
		}

		protected override Image GetImage()
		{
			return StiReportDesignImages.RestoreDefault.DataAdapters(StiImageSize.OneHalf);
		}

		protected override string GetPath()
		{
			try
			{
				string text = $"Stimulsoft{Path.DirectorySeparatorChar}DataAdapters";
				if (StiBaseOptions.FullTrust)
				{
					string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
					if (!string.IsNullOrEmpty(folderPath))
					{
						text = Path.Combine(folderPath, text);
					}
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
			catch
			{
				return null;
			}
		}

		protected override void Do()
		{
			string path = GetPath();
			if (path == null)
			{
				return;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			DirectoryInfo[] array = directories;
			foreach (DirectoryInfo directoryInfo2 in array)
			{
				long num = 0L;
				FileInfo[] files = directoryInfo2.GetFiles("*.*", SearchOption.AllDirectories);
				FileInfo[] array2 = files;
				foreach (FileInfo fileInfo in array2)
				{
					num += fileInfo.Length;
				}
				datas.Add(directoryInfo2.Name, num);
				fullSize += num;
			}
		}

		public override int Init(Control parent)
		{
			CreateDefaultPanel(parent, "Data Adapters: " + GetSize(fullSize), addSeparator: false);
			int num = 40;
			List<string> list = (from x in datas.Keys.ToList()
				orderby x
				select x).ToList();
			foreach (string item in list)
			{
				StiCheckBoxUX stiCheckBoxUX = new StiCheckBoxUX();
				((CheckBox)stiCheckBoxUX).Checked = true;
				((Control)stiCheckBoxUX).Location = new Point(StiScale.XXI(46.0), StiScale.YYI(num));
				((Control)stiCheckBoxUX).Size = new Size(StiScale.XXI(300.0), StiScale.YYI(20.0));
				((Control)stiCheckBoxUX).Text = item + ": " + GetSize(datas[item]);
				((Control)stiCheckBoxUX).Tag = item;
				StiCheckBoxUX stiCheckBoxUX2 = stiCheckBoxUX;
				checkBoxes.Add((CheckBox)(object)stiCheckBoxUX2);
				((Control)panel).Controls.Add((Control)(object)stiCheckBoxUX2);
				num += 24;
			}
			((Control)panel).Height = StiScale.YYI(num + 8);
			((Control)panel).Controls.Add((Control)(object)buttonClear);
			return ((Control)panel).Bottom;
		}

		protected override void ProcessClear()
		{
			((Control)buttonClear).Enabled = false;
			string path = GetPath();
			if (path == null)
			{
				return;
			}
			foreach (CheckBox checkBox in checkBoxes)
			{
				if (!checkBox.Checked)
				{
					continue;
				}
				string text = (string)((Control)checkBox).Tag;
				string path2 = Path.Combine(path, text);
				try
				{
					if (Directory.Exists(path2))
					{
						Directory.Delete(path2, recursive: true);
					}
				}
				catch
				{
				}
				long num = 0L;
				try
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(path2);
					if (directoryInfo.Exists)
					{
						FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
						FileInfo[] array = files;
						foreach (FileInfo fileInfo in array)
						{
							num += fileInfo.Length;
						}
					}
				}
				catch
				{
				}
				datas[text] = num;
				((Control)checkBox).Text = text + ": " + GetSize(num);
				if (num == 0L)
				{
					((Control)checkBox).Enabled = false;
					checkBox.Checked = false;
				}
			}
			bool enabled = false;
			foreach (KeyValuePair<string, long> data in datas)
			{
				if (data.Value > 0)
				{
					enabled = true;
					break;
				}
			}
			((Control)buttonClear).Enabled = enabled;
		}
	}

	private IContainer components;

	private Button btClose;

	private StiGroupLine footerPanel;

	private Panel rootPanel;

	public bool IsDesignerSettingsChanged { get; private set; }

	public bool IsSmtpSettingsChanged { get; private set; }

	public StiRestoreDefaultForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			Localize();
			DoScaling();
			Build();
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Buttons", "RestoreDefaults");
		((Control)btClose).Text = StiLocalization.Get("Buttons", "Close");
	}

	private void DoScaling()
	{
	}

	private void Build()
	{
		List<StiCacheItem> list = new List<StiCacheItem>
		{
			new StiDataAdaptersLogsCacheItem(this),
			new StiSchedulerCacheItem(this),
			new StiGisMapCacheItem(this),
			new StiThumbnailsLogsCacheItem(this),
			new StiBlocklyLogsCacheItem(this),
			new StiWindowsSettingsCacheItem(this),
			new StiDesignerSettingsCacheItem(this)
		};
		int num = 0;
		foreach (StiCacheItem item in list)
		{
			int val = item.Init((Control)(object)rootPanel);
			num = Math.Max(num, val);
		}
		if (num > ((Control)rootPanel).Height)
		{
			((ScrollableControl)rootPanel).AutoScroll = true;
			((ScrollableControl)rootPanel).AutoScrollMargin = new Size(StiScale.XXI(10.0), StiScale.YYI(10.0));
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRestoreDefaultForm));
		btClose = new Button();
		footerPanel = new StiGroupLine();
		rootPanel = new Panel();
		((Control)footerPanel).SuspendLayout();
		((Control)rootPanel).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btClose).Anchor = (AnchorStyles)10;
		btClose.DialogResult = (DialogResult)2;
		((Control)btClose).Location = new Point(429, 12);
		((Control)btClose).Name = "btClose";
		((Control)btClose).Size = new Size(75, 24);
		((Control)btClose).TabIndex = 100;
		((Control)btClose).Text = "Close";
		((ButtonBase)btClose).UseVisualStyleBackColor = true;
		((Control)btClose).Click += This_CancelClick;
		((Control)footerPanel).BackColor = Color.White;
		((Control)footerPanel).Controls.Add((Control)(object)btClose);
		((Control)footerPanel).Dock = (DockStyle)2;
		footerPanel.LinePosition = StiLinePosition.Top;
		footerPanel.LineStyle = StiLineStyle.Dot;
		((Control)footerPanel).Location = new Point(0, 537);
		((Control)footerPanel).Name = "footerPanel";
		((Control)footerPanel).Size = new Size(516, 46);
		((Control)footerPanel).TabIndex = 1;
		((Control)rootPanel).BackColor = Color.White;
		((Control)rootPanel).TabIndex = 2;
		((Control)rootPanel).Name = "rootPanel";
		((Control)rootPanel).Size = new Size(516, 300);
		((Control)rootPanel).Dock = (DockStyle)5;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btClose;
		((Form)this).ClientSize = new Size(516, 610);
		((Control)this).Controls.Add((Control)(object)footerPanel);
		((Control)this).Controls.Add((Control)(object)rootPanel);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiRestoreDefaultForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Restore Default";
		((Control)rootPanel).ResumeLayout();
		((Control)footerPanel).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

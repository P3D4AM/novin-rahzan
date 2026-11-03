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
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using EntityCache.Business.Global;
using EntityCache.Business.LkService;
using EntityCache.Mapping.Global;
using FormCustom.Properties;
using NPResourceManagement;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Department;

namespace FormCustom.UC;

public class Slider : UserControl
{
	private CancellationTokenSource _savePost = new CancellationTokenSource(60000);

	private CancellationTokenSource _PourDataToLocalToken = new CancellationTokenSource(60000);

	private CancellationTokenSource _PourLockKeysToken = new CancellationTokenSource(60000);

	private CancellationTokenSource _pourVersionsToken = new CancellationTokenSource(60000);

	private CancellationTokenSource _pourSliderToken = new CancellationTokenSource(60000);

	private CancellationTokenSource _pourNotificationToken = new CancellationTokenSource(60000);

	private bool _isDisposed;

	private LKInfo _lkObjects;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private PanelEx btnTamdid;

	private Label label6;

	private Label lblBoughtDate;

	private Label lblSofwareLevel;

	private Label label1;

	private LabelX lblTamdidPoshtibany;

	private PictureBox pictureBox2;

	private PictureBox pictureBox3;

	private FlowLayoutPanel flowLayoutSideBar;

	private FlowLayoutPanel flpNotifications;

	private Label lblSuportDays;

	private PhotoOrWeb ucPhotoOrWeb;

	private PanelEx panelEx1;

	private LabelX lblContactUs;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private FlowLayoutPanel flpOptionsNow;

	private TabPage tabPage5;

	private FlowLayoutPanel flpOptionsLater;

	private PanelEx panelEx3;

	private LabelX lblUpdateVersion;

	private Line button1;

	private CircularProgress progressBar;

	private Label lblRooz;

	private Label lblDaysInProgress;

	private Label label2;

	private Label lblBarCodeStatus;

	private Label label4;

	private Label lblTolidStatus;

	public long CurrentLongVersion { get; private set; }

	public string ContextGuid { get; private set; }

	public ENSource CurrentSource { get; private set; }

	public List<PhotoOrWeb> ListPhotoOrWeb { get; private set; } = new List<PhotoOrWeb>();


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

	public event EventHandler ShowAutoUpdaterForm;

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				_isDisposed = true;
				_savePost?.Cancel();
				_savePost?.Dispose();
				_savePost = null;
				_pourNotificationToken?.Cancel();
				_pourNotificationToken?.Dispose();
				_pourNotificationToken = null;
				_pourSliderToken?.Cancel();
				_pourSliderToken?.Dispose();
				_pourSliderToken = null;
				_pourVersionsToken?.Cancel();
				_pourVersionsToken?.Dispose();
				_pourVersionsToken = null;
				_PourDataToLocalToken?.Cancel();
				_PourDataToLocalToken?.Dispose();
				_PourDataToLocalToken = null;
				_PourLockKeysToken?.Cancel();
				_PourLockKeysToken?.Dispose();
				_PourLockKeysToken = null;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 71);
		}
	}

	private async Task PourDataToLocalAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			while (!IsDisposed && !token.IsCancellationRequested)
			{
				List<WebPost> source = await WebPost.GetAllBySourceAsync(CurrentSource);
				List<PostBusiness> lst = await PostMapper.Instance.MapListAsync(ContextGuid, source);
				_savePost?.Cancel();
				_savePost = new CancellationTokenSource(60000);
				if ((await PostBusiness.SaveRangeAsync(lst, _savePost.Token)).HasError)
				{
					((UserControl)(object)this).ShowError("داده های واکشی شده از سرور در دیتابیس لوکال ذخیره نشد".GetTranslate());
				}
				await Task.Delay(10000);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PourDataToLocalAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 92);
			((UserControl)(object)this).ShowError(ex.Message);
		}
	}

	private async Task PourLockKeysAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			int dayLeft = 0;
			_ = CurrentSource;
			_ = 1;
			_lkObjects = (await LkActivatorBaseBusiness.GetLkInfoAsync(ContextGuid, token))?.value;
			dayLeft = (int)Math.Floor((_lkObjects.ExpireDate - DateTime.Now).TotalDays);
			int valueShownInProgressBar = (int)((double)dayLeft * 100.0 / (_lkObjects.ExpireDate - _lkObjects.StartDate).TotalDays) + 1;
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
					if (_lkObjects.BarCode)
					{
						((Control)lblBarCodeStatus).Text = "فعال";
					}
					else
					{
						((Control)lblBarCodeStatus).Text = "غیر فعال";
					}
					if (_lkObjects.Tolid)
					{
						((Control)lblTolidStatus).Text = "فعال";
					}
					else
					{
						((Control)lblTolidStatus).Text = "غیر فعال";
					}
					((Control)lblSofwareLevel).Text = _lkObjects.StrTypeAccounting;
					((Control)lblBoughtDate).Text = _lkObjects.StartDateSh;
					((Control)lblSuportDays).Text = $"از اعتبار پشتیبانی شما {dayLeft}  روز مانده است ";
					((Control)lblSuportDays).ForeColor = Color.FromArgb(93, 185, 80);
					progressBar.ProgressColor = Color.FromArgb(93, 185, 80);
					((Control)lblRooz).ForeColor = Color.FromArgb(93, 185, 80);
					((Control)lblDaysInProgress).ForeColor = Color.FromArgb(93, 185, 80);
					((Control)lblDaysInProgress).Text = dayLeft.ToString();
					progressBar.Value = valueShownInProgressBar;
					if (dayLeft < 30)
					{
						if (dayLeft >= 1)
						{
							if (dayLeft < 15)
							{
								progressBar.Value = 100;
								progressBar.ProgressColor = Color.FromArgb(247, 40, 40);
								((Control)lblSuportDays).ForeColor = Color.FromArgb(247, 40, 40);
								((Control)lblDaysInProgress).Text = dayLeft.ToString();
								((Control)lblDaysInProgress).ForeColor = Color.FromArgb(247, 40, 40);
								((Control)lblRooz).ForeColor = Color.FromArgb(247, 40, 40);
							}
							else
							{
								progressBar.Value = valueShownInProgressBar;
								progressBar.ProgressColor = Color.FromArgb(236, 180, 20);
								((Control)lblSuportDays).ForeColor = Color.FromArgb(236, 180, 20);
								((Control)lblDaysInProgress).Text = dayLeft.ToString();
								((Control)lblDaysInProgress).ForeColor = Color.FromArgb(236, 180, 20);
								((Control)lblRooz).ForeColor = Color.FromArgb(236, 180, 20);
							}
						}
						else
						{
							progressBar.Value = 100;
							progressBar.ProgressColor = Color.FromArgb(247, 40, 40);
							((Control)lblSuportDays).Text = $"از اعتبار پشتیبانی شما {-1 * dayLeft}  روز گذشته است ";
							((Control)lblSuportDays).ForeColor = Color.FromArgb(247, 40, 40);
							((Control)lblDaysInProgress).Text = (-1 * dayLeft).ToString();
							((Control)lblDaysInProgress).ForeColor = Color.FromArgb(247, 40, 40);
							((Control)lblRooz).ForeColor = Color.FromArgb(247, 40, 40);
						}
					}
					else if (dayLeft < 60)
					{
						progressBar.Value = valueShownInProgressBar;
						progressBar.ProgressColor = Color.FromArgb(247, 112, 40);
						((Control)lblSuportDays).ForeColor = Color.FromArgb(247, 112, 40);
						((Control)lblDaysInProgress).Text = dayLeft.ToString();
						((Control)lblDaysInProgress).ForeColor = Color.FromArgb(247, 112, 40);
						((Control)lblRooz).ForeColor = Color.FromArgb(247, 112, 40);
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "PourLockKeysAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 172);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PourLockKeysAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 175);
		}
	}

	private async Task PourVersionsAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			IEnumerable<SoftwarePackage> enumerable = (await SoftwarePackage.GetListFromApiAsync(token))?.Where((SoftwarePackage x) => x.Source == CurrentSource);
			List<SoftwarePackage> listLaterVersion = enumerable?.Where((SoftwarePackage x) => x.VersionLong > CurrentLongVersion)?.OrderByDescending((SoftwarePackage x) => x.Version)?.ToList();
			SoftwarePackage itemNowVersion = enumerable?.Single((SoftwarePackage x) => x.VersionLong == CurrentLongVersion);
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
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Expected O, but got Unknown
				//IL_008b: Expected O, but got Unknown
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Expected O, but got Unknown
				//IL_00bf: Expected O, but got Unknown
				//IL_0137: Unknown result type (might be due to invalid IL or missing references)
				//IL_013c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0158: Unknown result type (might be due to invalid IL or missing references)
				//IL_015f: Unknown result type (might be due to invalid IL or missing references)
				//IL_016b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0175: Expected O, but got Unknown
				//IL_0177: Expected O, but got Unknown
				//IL_0177: Unknown result type (might be due to invalid IL or missing references)
				//IL_017c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0189: Unknown result type (might be due to invalid IL or missing references)
				//IL_0190: Unknown result type (might be due to invalid IL or missing references)
				//IL_019c: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a6: Expected O, but got Unknown
				//IL_01a8: Expected O, but got Unknown
				//IL_0283: Unknown result type (might be due to invalid IL or missing references)
				//IL_0288: Unknown result type (might be due to invalid IL or missing references)
				//IL_0298: Expected O, but got Unknown
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					int num = 0;
					if (itemNowVersion != null && !string.IsNullOrEmpty(itemNowVersion.DescriptionUrl))
					{
						Label val = new Label
						{
							Text = "\r\nنسخه " + itemNowVersion.VersionForDisplay + "\r\n",
							AutoSize = true,
							Font = new Font("PeydaWeb", 15f, (FontStyle)1)
						};
						Label val2 = new Label
						{
							Text = itemNowVersion.DescriptionUrl,
							AutoSize = true,
							Font = new Font("PeydaWeb", 11f, (FontStyle)1)
						};
						((Control)flpOptionsNow).Controls.Add((Control)(object)val);
						((Control)flpOptionsNow).Controls.Add((Control)(object)val2);
					}
					List<SoftwarePackage> list = listLaterVersion;
					if (list != null && list.Any())
					{
						foreach (SoftwarePackage item in listLaterVersion)
						{
							if (!string.IsNullOrEmpty(item.DescriptionUrl))
							{
								Label val3 = new Label
								{
									Text = "\r\nنسخه " + item.VersionForDisplay + "\r\n",
									AutoSize = true,
									Font = new Font("PeydaWeb", 15f, (FontStyle)1)
								};
								Label val4 = new Label
								{
									Text = item.DescriptionUrl,
									AutoSize = true,
									Font = new Font("PeydaWeb", 11f, (FontStyle)1)
								};
								Line line = new Line();
								((Control)line).ForeColor = Color.FromArgb(6, 14, 146);
								((Control)line).Width = ((Control)flpOptionsLater).Width - 60;
								((Control)line).AutoSize = true;
								Line line2 = line;
								if (num % 2 == 0)
								{
									((Control)val4).ForeColor = Color.FromArgb(46, 57, 239);
									((Control)val3).ForeColor = Color.FromArgb(46, 57, 239);
								}
								else
								{
									((Control)val4).ForeColor = Color.FromArgb(0, 0, 0);
									((Control)val3).ForeColor = Color.FromArgb(0, 0, 0);
								}
								((Control)flpOptionsLater).Controls.Add((Control)(object)val3);
								((Control)flpOptionsLater).Controls.Add((Control)(object)val4);
								((Control)flpOptionsLater).Controls.Add((Control)new Label
								{
									Text = "\r\n"
								});
								if (item != listLaterVersion[listLaterVersion.Count() - 1])
								{
									((Control)flpOptionsLater).Controls.Add((Control)(object)line2);
								}
								num++;
							}
						}
						return;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "PourVersionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 260);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PourVersionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 263);
		}
	}

	private async Task PourNotificationAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			IEnumerable<PushNotificationBusiness> notificationList = await PushNotificationBusiness.GetActiveNotificationAsync(ContextGuid, CurrentSource);
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
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Expected O, but got Unknown
				//IL_009b: Expected O, but got Unknown
				//IL_009b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Expected O, but got Unknown
				//IL_00cb: Expected O, but got Unknown
				//IL_018f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0194: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a4: Expected O, but got Unknown
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					int num = 0;
					IEnumerable<PushNotificationBusiness> enumerable = notificationList;
					if (enumerable != null && enumerable.Any())
					{
						foreach (PushNotificationBusiness item in notificationList)
						{
							if (!string.IsNullOrEmpty(item.Message))
							{
								Label val = new Label
								{
									Text = "\r\n⚡\ufe0f " + item.Title + "\r\n",
									AutoSize = true,
									Font = new Font("PeydaWeb", 15f, (FontStyle)1)
								};
								Label val2 = new Label
								{
									Text = item.Message,
									AutoSize = true,
									Font = new Font("PeydaWeb", 11f, (FontStyle)1)
								};
								Line line = new Line();
								((Control)line).ForeColor = Color.FromArgb(6, 14, 146);
								((Control)line).Width = ((Control)flpOptionsLater).Width - 60;
								((Control)line).AutoSize = true;
								Line line2 = line;
								if (num % 2 == 0)
								{
									((Control)val2).ForeColor = Color.FromArgb(46, 57, 239);
									((Control)val).ForeColor = Color.FromArgb(46, 57, 239);
								}
								else
								{
									((Control)val2).ForeColor = Color.FromArgb(0, 0, 0);
									((Control)val).ForeColor = Color.FromArgb(0, 0, 0);
								}
								((Control)flpNotifications).Controls.Add((Control)(object)val);
								((Control)flpNotifications).Controls.Add((Control)(object)val2);
								((Control)flpNotifications).Controls.Add((Control)new Label
								{
									Text = "\r\n"
								});
								if (item != notificationList.ToList()[notificationList.Count() - 1])
								{
									((Control)flpNotifications).Controls.Add((Control)(object)line2);
								}
								num++;
							}
						}
						return;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "PourNotificationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 324);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PourNotificationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 327);
		}
	}

	private async Task PourSliderAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			while (true)
			{
				IEnumerable<PostBusiness> postLocalList = await PostBusiness.GetAllAsync(ContextGuid, token);
				if (IsDisposed || token.IsCancellationRequested)
				{
					break;
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
					break;
				}
				((Control)this).BeginInvoke((Delegate)(Action)delegate
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
					try
					{
						((Control)flowLayoutSideBar).Controls.Clear();
						IEnumerable<PostBusiness> enumerable = postLocalList;
						if (enumerable != null && enumerable.Any())
						{
							ucPhotoOrWeb.Init(postLocalList.First(), IsMain: true);
							{
								foreach (PostBusiness item in postLocalList)
								{
									PhotoOrWeb photoOrWeb = new PhotoOrWeb();
									((Control)photoOrWeb).Cursor = Cursors.Hand;
									((Control)photoOrWeb).Width = 15;
									((Control)photoOrWeb).Height = 15;
									PhotoOrWeb photoOrWeb2 = photoOrWeb;
									if (item == postLocalList.First())
									{
										photoOrWeb2.Init(item, IsMain: false, IsClicked: true);
									}
									else
									{
										photoOrWeb2.Init(item, IsMain: false);
									}
									photoOrWeb2.OnClick += SideBarIcons_OnClick;
									((Control)flowLayoutSideBar).Controls.Add((Control)(object)photoOrWeb2);
									ListPhotoOrWeb.Add(photoOrWeb2);
								}
								return;
							}
						}
						PostBusiness post = new PostBusiness(ContextGuid)
						{
							Image = ImageToByte((Image)(object)FormCustom.Properties.Resources.Novin)
						};
						ucPhotoOrWeb.Init(post, IsMain: true);
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "PourSliderAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 374);
					}
				});
				await Task.Delay(10000);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PourSliderAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 379);
		}
	}

	private void SideBarIcons_OnClick(object sender, EventArgs e)
	{
		try
		{
			if (!(sender is PhotoOrWeb))
			{
				return;
			}
			PhotoOrWeb photoOrWeb = (PhotoOrWeb)sender;
			foreach (PhotoOrWeb item in ListPhotoOrWeb)
			{
				item.ChangeBackColor();
			}
			ucPhotoOrWeb.Init(photoOrWeb.Post, IsMain: true);
			photoOrWeb.Init(photoOrWeb.Post, IsMain: false, IsClicked: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SideBarIcons_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 392);
		}
	}

	private byte[] ImageToByte(Image img)
	{
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			img.Save((Stream)memoryStream, img.RawFormat);
			return memoryStream.ToArray();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ImageToByte", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 402);
		}
		return null;
	}

	public async Task InitializeAsync(long longVersion, string contextGuid, ENSource source)
	{
		try
		{
			((Control)this).AutoSize = false;
			CurrentLongVersion = longVersion;
			ContextGuid = contextGuid;
			CurrentSource = source;
			Task.Run(() => PourDataToLocalAsync(_PourDataToLocalToken.Token));
			Task.Run(() => PourLockKeysAsync(_PourLockKeysToken.Token));
			Task.Run(() => PourVersionsAsync(_pourVersionsToken.Token));
			Task.Run(() => PourSliderAsync(_pourSliderToken.Token));
			Task.Run(() => PourNotificationAsync(_pourNotificationToken.Token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 420);
		}
	}

	public Slider()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void lblTamdidPoshtibany_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start($"{TextResourceManagement.TextResource.CRMBaseURL}/extend/lk/{_lkObjects?.LKSerial}");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblTamdidPoshtibany_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 432);
		}
	}

	private void lblContactUs_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start("https://novinhozor.com/contact-us/");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblContactUs_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 437);
		}
	}

	private void lblUpdateVersion_Click(object sender, EventArgs e)
	{
		try
		{
			this.ShowAutoUpdaterForm?.Invoke(this, new EventArgs());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblUpdateVersion_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\Slider.cs", 442);
		}
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
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
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Expected O, but got Unknown
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Expected O, but got Unknown
		//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Expected O, but got Unknown
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_0990: Expected O, but got Unknown
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2b: Expected O, but got Unknown
		//IL_0bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be7: Expected O, but got Unknown
		//IL_0c77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c81: Expected O, but got Unknown
		//IL_0d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d18: Expected O, but got Unknown
		//IL_0da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0daf: Expected O, but got Unknown
		//IL_0e81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1000: Expected O, but got Unknown
		//IL_10cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_138c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1396: Expected O, but got Unknown
		//IL_1427: Unknown result type (might be due to invalid IL or missing references)
		//IL_1431: Expected O, but got Unknown
		//IL_14bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c9: Expected O, but got Unknown
		//IL_155a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1564: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Slider));
		tabControl1 = new TabControl();
		tabPage1 = new TabPage();
		panelEx3 = new PanelEx();
		lblUpdateVersion = new LabelX();
		lblRooz = new Label();
		lblDaysInProgress = new Label();
		progressBar = new CircularProgress();
		panelEx1 = new PanelEx();
		lblContactUs = new LabelX();
		lblSuportDays = new Label();
		btnTamdid = new PanelEx();
		lblTamdidPoshtibany = new LabelX();
		label6 = new Label();
		lblBoughtDate = new Label();
		lblSofwareLevel = new Label();
		label1 = new Label();
		tabPage2 = new TabPage();
		flpNotifications = new FlowLayoutPanel();
		flowLayoutSideBar = new FlowLayoutPanel();
		tabControl2 = new TabControl();
		tabPage3 = new TabPage();
		flpOptionsNow = new FlowLayoutPanel();
		tabPage5 = new TabPage();
		flpOptionsLater = new FlowLayoutPanel();
		button1 = new Line();
		ucPhotoOrWeb = new PhotoOrWeb();
		label2 = new Label();
		lblBarCodeStatus = new Label();
		label4 = new Label();
		lblTolidStatus = new Label();
		((Control)tabControl1).SuspendLayout();
		((Control)tabPage1).SuspendLayout();
		((Control)panelEx3).SuspendLayout();
		((Control)panelEx1).SuspendLayout();
		((Control)btnTamdid).SuspendLayout();
		((Control)tabPage2).SuspendLayout();
		((Control)tabControl2).SuspendLayout();
		((Control)tabPage3).SuspendLayout();
		((Control)tabPage5).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)tabControl1).Anchor = (AnchorStyles)7;
		((Control)tabControl1).Controls.Add((Control)(object)tabPage1);
		((Control)tabControl1).Controls.Add((Control)(object)tabPage2);
		((Control)tabControl1).Font = new Font("PeydaWeb", 9.749999f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)tabControl1).Location = new Point(8, 3);
		((Control)tabControl1).Name = "tabControl1";
		((Control)tabControl1).RightToLeft = (RightToLeft)1;
		tabControl1.RightToLeftLayout = true;
		tabControl1.SelectedIndex = 0;
		((Control)tabControl1).Size = new Size(296, 678);
		((Control)tabControl1).TabIndex = 0;
		((Control)tabPage1).BackColor = Color.FromArgb(227, 239, 255);
		((Control)tabPage1).Controls.Add((Control)(object)label4);
		((Control)tabPage1).Controls.Add((Control)(object)lblTolidStatus);
		((Control)tabPage1).Controls.Add((Control)(object)label2);
		((Control)tabPage1).Controls.Add((Control)(object)lblBarCodeStatus);
		((Control)tabPage1).Controls.Add((Control)(object)panelEx3);
		((Control)tabPage1).Controls.Add((Control)(object)lblRooz);
		((Control)tabPage1).Controls.Add((Control)(object)lblDaysInProgress);
		((Control)tabPage1).Controls.Add((Control)(object)progressBar);
		((Control)tabPage1).Controls.Add((Control)(object)panelEx1);
		((Control)tabPage1).Controls.Add((Control)(object)lblSuportDays);
		((Control)tabPage1).Controls.Add((Control)(object)btnTamdid);
		((Control)tabPage1).Controls.Add((Control)(object)label6);
		((Control)tabPage1).Controls.Add((Control)(object)lblBoughtDate);
		((Control)tabPage1).Controls.Add((Control)(object)lblSofwareLevel);
		((Control)tabPage1).Controls.Add((Control)(object)label1);
		tabPage1.Location = new Point(4, 27);
		((Control)tabPage1).Name = "tabPage1";
		((Control)tabPage1).Padding = new Padding(3);
		((Control)tabPage1).Size = new Size(288, 647);
		tabPage1.TabIndex = 6;
		((Control)tabPage1).Text = "[اعتبار سنجی]";
		panelEx3.CanvasColor = Color.SkyBlue;
		((Control)panelEx3).Controls.Add((Control)(object)lblUpdateVersion);
		((Control)panelEx3).Cursor = Cursors.Hand;
		panelEx3.DisabledBackColor = Color.Empty;
		((Control)panelEx3).Location = new Point(23, 506);
		((Control)panelEx3).Name = "panelEx3";
		((Control)panelEx3).Size = new Size(232, 45);
		panelEx3.Style.BackColor1.Color = Color.SkyBlue;
		panelEx3.Style.BorderWidth = 10;
		panelEx3.Style.CornerDiameter = 35;
		panelEx3.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx3).TabIndex = 25;
		((Control)panelEx3).Text = "button1";
		lblUpdateVersion.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)lblUpdateVersion).Font = new Font("PeydaWeb", 11.25f, (FontStyle)1);
		lblUpdateVersion.Image = (Image)(object)FormCustom.Properties.Resources.iyjygmg_icons8;
		lblUpdateVersion.ImagePosition = eImagePosition.Right;
		((Control)lblUpdateVersion).Location = new Point(64, 0);
		((Control)lblUpdateVersion).Name = "lblUpdateVersion";
		((Control)lblUpdateVersion).Size = new Size(128, 43);
		((Control)lblUpdateVersion).TabIndex = 0;
		((Control)lblUpdateVersion).Text = "[بروزرسانی]";
		lblUpdateVersion.TextAlignment = (StringAlignment)1;
		((Control)lblUpdateVersion).Click += lblUpdateVersion_Click;
		((Control)lblRooz).AutoSize = true;
		((Control)lblRooz).Location = new Point(132, 125);
		((Control)lblRooz).Name = "lblRooz";
		((Control)lblRooz).Size = new Size(25, 18);
		((Control)lblRooz).TabIndex = 39;
		((Control)lblRooz).Text = "[روز]";
		((Control)lblDaysInProgress).Font = new Font("PeydaWeb", 11.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblDaysInProgress).Location = new Point(114, 103);
		((Control)lblDaysInProgress).Name = "lblDaysInProgress";
		((Control)lblDaysInProgress).Size = new Size(60, 18);
		((Control)lblDaysInProgress).TabIndex = 38;
		((Control)lblDaysInProgress).Text = "61";
		lblDaysInProgress.TextAlign = (ContentAlignment)32;
		progressBar.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)progressBar).Location = new Point(21, 10);
		((Control)progressBar).Name = "progressBar";
		progressBar.PieBorderLight = Color.Yellow;
		progressBar.ProgressBarType = eCircularProgressType.Spoke;
		((Control)progressBar).Size = new Size(246, 211);
		progressBar.SpokeBorderDark = Color.FromArgb(227, 239, 255);
		progressBar.SpokeBorderLight = Color.FromArgb(227, 239, 255);
		progressBar.Style = eDotNetBarStyle.OfficeXP;
		((Control)progressBar).TabIndex = 31;
		progressBar.Value = 100;
		panelEx1.CanvasColor = Color.SkyBlue;
		((Control)panelEx1).Controls.Add((Control)(object)lblContactUs);
		((Control)panelEx1).Cursor = Cursors.Hand;
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(23, 575);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(232, 45);
		panelEx1.Style.BackColor1.Color = Color.SkyBlue;
		panelEx1.Style.BorderWidth = 10;
		panelEx1.Style.CornerDiameter = 35;
		panelEx1.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx1).TabIndex = 17;
		((Control)panelEx1).Text = "button1";
		lblContactUs.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)lblContactUs).Font = new Font("PeydaWeb", 11.25f, (FontStyle)1);
		lblContactUs.Image = (Image)componentResourceManager.GetObject("lblContactUs.Image");
		lblContactUs.ImagePosition = eImagePosition.Right;
		((Control)lblContactUs).Location = new Point(72, 0);
		((Control)lblContactUs).Name = "lblContactUs";
		((Control)lblContactUs).Size = new Size(120, 45);
		((Control)lblContactUs).TabIndex = 0;
		((Control)lblContactUs).Text = "[ارتباط با ما]";
		lblContactUs.TextAlignment = (StringAlignment)1;
		((Control)lblContactUs).Click += lblContactUs_Click;
		((Control)lblSuportDays).Font = new Font("PeydaWeb", 11.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblSuportDays).ForeColor = Color.FromArgb(51, 51, 255);
		((Control)lblSuportDays).Location = new Point(3, 228);
		((Control)lblSuportDays).Name = "lblSuportDays";
		((Control)lblSuportDays).Size = new Size(276, 53);
		((Control)lblSuportDays).TabIndex = 19;
		((Control)lblSuportDays).Text = "[مدت اعتبار پشتیبانی]";
		lblSuportDays.TextAlign = (ContentAlignment)2;
		btnTamdid.CanvasColor = Color.SkyBlue;
		((Control)btnTamdid).Controls.Add((Control)(object)lblTamdidPoshtibany);
		((Control)btnTamdid).Cursor = Cursors.Hand;
		btnTamdid.DisabledBackColor = Color.Empty;
		((Control)btnTamdid).Location = new Point(23, 439);
		((Control)btnTamdid).Name = "btnTamdid";
		((Control)btnTamdid).Size = new Size(232, 45);
		btnTamdid.Style.BackColor1.Color = Color.SkyBlue;
		btnTamdid.Style.BorderWidth = 10;
		btnTamdid.Style.CornerDiameter = 35;
		btnTamdid.Style.CornerType = eCornerType.Rounded;
		((Control)btnTamdid).TabIndex = 13;
		((Control)btnTamdid).Text = "button1";
		lblTamdidPoshtibany.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)lblTamdidPoshtibany).Font = new Font("PeydaWeb", 11.25f, (FontStyle)1);
		lblTamdidPoshtibany.Image = (Image)(object)FormCustom.Properties.Resources.techSupport;
		lblTamdidPoshtibany.ImagePosition = eImagePosition.Right;
		((Control)lblTamdidPoshtibany).Location = new Point(16, 0);
		((Control)lblTamdidPoshtibany).Name = "lblTamdidPoshtibany";
		((Control)lblTamdidPoshtibany).Size = new Size(189, 45);
		((Control)lblTamdidPoshtibany).TabIndex = 0;
		((Control)lblTamdidPoshtibany).Text = "[تمدید اعتبار پشتیبانی]";
		lblTamdidPoshtibany.TextAlignment = (StringAlignment)1;
		((Control)lblTamdidPoshtibany).Click += lblTamdidPoshtibany_Click;
		((Control)label6).AutoSize = true;
		((Control)label6).Font = new Font("PeydaWeb", 12f, (FontStyle)1);
		((Control)label6).ForeColor = Color.FromArgb(51, 51, 255);
		((Control)label6).Location = new Point(200, 329);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(82, 22);
		((Control)label6).TabIndex = 7;
		((Control)label6).Text = "[تاریخ خرید]:";
		((Control)lblBoughtDate).AutoSize = true;
		((Control)lblBoughtDate).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1);
		((Control)lblBoughtDate).ForeColor = Color.FromArgb(51, 51, 255);
		((Control)lblBoughtDate).Location = new Point(16, 329);
		((Control)lblBoughtDate).Name = "lblBoughtDate";
		((Control)lblBoughtDate).Size = new Size(88, 22);
		((Control)lblBoughtDate).TabIndex = 6;
		((Control)lblBoughtDate).Text = "1401/01/01";
		((Control)lblSofwareLevel).AutoSize = true;
		((Control)lblSofwareLevel).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1);
		((Control)lblSofwareLevel).ForeColor = Color.FromArgb(51, 51, 255);
		((Control)lblSofwareLevel).Location = new Point(17, 297);
		((Control)lblSofwareLevel).Name = "lblSofwareLevel";
		((Control)lblSofwareLevel).Size = new Size(62, 22);
		((Control)lblSofwareLevel).TabIndex = 4;
		((Control)lblSofwareLevel).Text = "[پیشرفته]";
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("PeydaWeb", 12f, (FontStyle)1);
		((Control)label1).ForeColor = Color.FromArgb(51, 51, 255);
		((Control)label1).Location = new Point(181, 297);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(102, 22);
		((Control)label1).TabIndex = 3;
		((Control)label1).Text = "[سطح نرم افزار]:";
		((Control)tabPage2).BackColor = Color.FromArgb(227, 239, 255);
		((Control)tabPage2).Controls.Add((Control)(object)flpNotifications);
		tabPage2.Location = new Point(4, 27);
		((Control)tabPage2).Name = "tabPage2";
		((Control)tabPage2).Padding = new Padding(3);
		((Control)tabPage2).Size = new Size(288, 647);
		tabPage2.TabIndex = 7;
		((Control)tabPage2).Text = "[اعلانات]";
		((Control)flpNotifications).Anchor = (AnchorStyles)15;
		((ScrollableControl)flpNotifications).AutoScroll = true;
		flpNotifications.FlowDirection = (FlowDirection)1;
		((Control)flpNotifications).Location = new Point(3, 3);
		((Control)flpNotifications).Name = "flpNotifications";
		((Control)flpNotifications).Size = new Size(285, 731);
		((Control)flpNotifications).TabIndex = 1;
		flpNotifications.WrapContents = false;
		((Control)flowLayoutSideBar).Anchor = (AnchorStyles)2;
		((ScrollableControl)flowLayoutSideBar).AutoScroll = true;
		((Control)flowLayoutSideBar).Location = new Point(729, 622);
		((Control)flowLayoutSideBar).Name = "flowLayoutSideBar";
		((Control)flowLayoutSideBar).Size = new Size(211, 49);
		((Control)flowLayoutSideBar).TabIndex = 4;
		flowLayoutSideBar.WrapContents = false;
		((Control)tabControl2).Anchor = (AnchorStyles)7;
		((Control)tabControl2).Controls.Add((Control)(object)tabPage3);
		((Control)tabControl2).Controls.Add((Control)(object)tabPage5);
		((Control)tabControl2).Font = new Font("PeydaWeb", 9.749999f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)tabControl2).Location = new Point(306, 3);
		((Control)tabControl2).Name = "tabControl2";
		((Control)tabControl2).RightToLeft = (RightToLeft)1;
		tabControl2.RightToLeftLayout = true;
		tabControl2.SelectedIndex = 0;
		((Control)tabControl2).Size = new Size(296, 678);
		((Control)tabControl2).TabIndex = 7;
		((Control)tabPage3).BackColor = Color.FromArgb(227, 239, 255);
		((Control)tabPage3).Controls.Add((Control)(object)flpOptionsNow);
		tabPage3.Location = new Point(4, 27);
		((Control)tabPage3).Name = "tabPage3";
		((Control)tabPage3).Padding = new Padding(3);
		((Control)tabPage3).Size = new Size(288, 647);
		tabPage3.TabIndex = 0;
		((Control)tabPage3).Text = "[امکانات نسخه جاری]";
		((Control)flpOptionsNow).Anchor = (AnchorStyles)7;
		((ScrollableControl)flpOptionsNow).AutoScroll = true;
		flpOptionsNow.FlowDirection = (FlowDirection)1;
		((Control)flpOptionsNow).Location = new Point(3, 3);
		((Control)flpOptionsNow).Name = "flpOptionsNow";
		((Control)flpOptionsNow).Size = new Size(282, 638);
		((Control)flpOptionsNow).TabIndex = 0;
		flpOptionsNow.WrapContents = false;
		((Control)tabPage5).BackColor = Color.FromArgb(227, 239, 255);
		((Control)tabPage5).Controls.Add((Control)(object)flpOptionsLater);
		tabPage5.Location = new Point(4, 27);
		((Control)tabPage5).Name = "tabPage5";
		((Control)tabPage5).Padding = new Padding(3);
		((Control)tabPage5).Size = new Size(288, 647);
		tabPage5.TabIndex = 1;
		((Control)tabPage5).Text = "[امکانات نسخه جدید]";
		((Control)flpOptionsLater).Anchor = (AnchorStyles)7;
		((ScrollableControl)flpOptionsLater).AutoScroll = true;
		flpOptionsLater.FlowDirection = (FlowDirection)1;
		((Control)flpOptionsLater).Location = new Point(3, 3);
		((Control)flpOptionsLater).Name = "flpOptionsLater";
		((Control)flpOptionsLater).Size = new Size(282, 641);
		((Control)flpOptionsLater).TabIndex = 0;
		flpOptionsLater.WrapContents = false;
		((Control)button1).Anchor = (AnchorStyles)14;
		((Control)button1).Location = new Point(608, 666);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(365, 23);
		((Control)button1).TabIndex = 0;
		((Control)button1).Text = "button1";
		((Control)ucPhotoOrWeb).Anchor = (AnchorStyles)15;
		((Control)ucPhotoOrWeb).Location = new Point(604, 30);
		((Control)ucPhotoOrWeb).Name = "ucPhotoOrWeb";
		ucPhotoOrWeb.Radius = 30;
		((Control)ucPhotoOrWeb).Size = new Size(369, 586);
		((Control)ucPhotoOrWeb).TabIndex = 5;
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("PeydaWeb", 12f, (FontStyle)1);
		((Control)label2).ForeColor = Color.FromArgb(51, 51, 255);
		((Control)label2).Location = new Point(198, 360);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(84, 22);
		((Control)label2).TabIndex = 47;
		((Control)label2).Text = "[بسته بارکد]:";
		((Control)lblBarCodeStatus).AutoSize = true;
		((Control)lblBarCodeStatus).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1);
		((Control)lblBarCodeStatus).ForeColor = Color.FromArgb(51, 51, 255);
		((Control)lblBarCodeStatus).Location = new Point(16, 360);
		((Control)lblBarCodeStatus).Name = "lblBarCodeStatus";
		((Control)lblBarCodeStatus).Size = new Size(66, 22);
		((Control)lblBarCodeStatus).TabIndex = 46;
		((Control)lblBarCodeStatus).Text = "[غیر فعال]";
		((Control)label4).AutoSize = true;
		((Control)label4).Font = new Font("PeydaWeb", 12f, (FontStyle)1);
		((Control)label4).ForeColor = Color.FromArgb(51, 51, 255);
		((Control)label4).Location = new Point(200, 391);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(81, 22);
		((Control)label4).TabIndex = 49;
		((Control)label4).Text = "[بسته تولید]:";
		((Control)lblTolidStatus).AutoSize = true;
		((Control)lblTolidStatus).Font = new Font("PeydaWeb SemBd", 12f, (FontStyle)1);
		((Control)lblTolidStatus).ForeColor = Color.FromArgb(51, 51, 255);
		((Control)lblTolidStatus).Location = new Point(16, 391);
		((Control)lblTolidStatus).Name = "lblTolidStatus";
		((Control)lblTolidStatus).Size = new Size(66, 22);
		((Control)lblTolidStatus).TabIndex = 48;
		((Control)lblTolidStatus).Text = "[غیر فعال]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)button1);
		((Control)this).Controls.Add((Control)(object)flowLayoutSideBar);
		((Control)this).Controls.Add((Control)(object)tabControl2);
		((Control)this).Controls.Add((Control)(object)ucPhotoOrWeb);
		((Control)this).Controls.Add((Control)(object)tabControl1);
		((Control)this).MinimumSize = new Size(989, 664);
		((Control)this).Name = "Slider";
		((Control)this).Size = new Size(989, 692);
		((Control)tabControl1).ResumeLayout(false);
		((Control)tabPage1).ResumeLayout(false);
		((Control)tabPage1).PerformLayout();
		((Control)panelEx3).ResumeLayout(false);
		((Control)panelEx1).ResumeLayout(false);
		((Control)btnTamdid).ResumeLayout(false);
		((Control)tabPage2).ResumeLayout(false);
		((Control)tabControl2).ResumeLayout(false);
		((Control)tabPage3).ResumeLayout(false);
		((Control)tabPage5).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}

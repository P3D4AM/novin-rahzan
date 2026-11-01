using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using EntityCache.Business.Attendance;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Forms;
using TimeAttendanceUI.Forms.Taradod;

namespace TimeAttendanceUI.AttPersonLeave;

public class FrmRequestedVacation : BaseForm
{
	private bool _isDisposed;

	private IContainer components;

	private UcAttPersonLeave ucAttPersonLeave1;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
		}
	}

	private async void UcAttPersonLeave1_OnReject(AttPersonLeaveBusiness obj)
	{
		_ = 1;
		try
		{
			if (obj == null)
			{
				return;
			}
			string text = await FrmPrompt.ShowDialogAsync((IWin32Window)(object)this, ((CacheBaseForm)this).ContextGuid, "علت رد درخواست:", "فرم رد درخواست پرسنل");
			if (!string.IsNullOrEmpty(text))
			{
				obj.ManagerMessage = text;
				obj.ManagerDate = DateTime.Now;
				obj.LeaveStatus = (EnPersonLeaveStatus)1;
				ReturnedSaveFuncInfo val = await obj.SaveAsync(((CacheBaseForm)this).system, true, (Priority)0, default(CancellationToken), true, false, "");
				if (val.HasError)
				{
					val.Show(((CacheBaseForm)this).ContextGuid);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void UcAttPersonLeave1_OnConfirm(List<AttPersonLeaveBusiness> obj)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			_ = 6;
			try
			{
				if (!(obj?.Any() ?? false) || (obj.Count == 1 && (int)obj.Single().Quiddity == 0 && (int)(await ((CacheBaseForm)new FrmMorakhasiDaily(((CacheBaseForm)this).ContextGuid, obj.Single())).ShowAsync((IWin32Window)(object)this)) != 1))
				{
					goto end_IL_0047;
				}
				foreach (AttPersonLeaveBusiness item in obj)
				{
					Person person = await Person.GetBypersonGuidAsync(((CacheBaseForm)this).ContextGuid, item.PersonGuid, default(CancellationToken));
					string desc = item.Reason;
					TimeSpan temp = default(TimeSpan);
					if ((int)item.Quiddity != 0)
					{
						desc = $"از ساعت {item.DateFrom.Hour}:{item.DateFrom.Minute} تا ساعت {item.DateTo.Hour}:{item.DateTo.Minute} \r\n {item.Reason}";
						temp = item.DateTo.Subtract(item.DateFrom);
					}
					EnPersonLeaveQuiddity quiddity = item.Quiddity;
					if ((int)quiddity != 1)
					{
						if ((int)quiddity == 2)
						{
							ATT_TaradodDet obj2 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)person).Id, item.DateFrom);
							obj2.MissionDesc = desc;
							obj2.EKDesc = desc;
							obj2.MissionStart = item.DateFrom;
							obj2.MissionEnd = item.DateTo;
							obj2.MissionTime = (int)Math.Round(temp.TotalMinutes);
							await obj2.SaveAsync();
						}
					}
					else
					{
						ATT_TaradodDet obj3 = await ATT_TaradodDet.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)person).Id, item.DateFrom);
						obj3.EKDesc = desc;
						obj3.EKStart = item.DateFrom;
						obj3.EKEnd = item.DateTo;
						obj3.EKTime = (int)Math.Round(temp.TotalMinutes);
						await obj3.SaveAsync();
					}
					item.LeaveStatus = (EnPersonLeaveStatus)2;
					item.ManagerDate = DateTime.Now;
					item.ManagerMessage = "مورد تائید است";
					ReturnedSaveFuncInfo val = res;
					val.AddReturnedValue(await item.SaveAsync(((CacheBaseForm)this).system, true, (Priority)0, default(CancellationToken), true, false, ""), (ReturnedState)0, "UcAttPersonLeave1_OnConfirm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\AttPersonLeave\\FrmRequestedVacation.cs", 86);
				}
				goto end_IL_0033;
				end_IL_0047:;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				res.AddReturnedValue(ex, "", "UcAttPersonLeave1_OnConfirm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\AttPersonLeave\\FrmRequestedVacation.cs", 92);
				goto end_IL_0033;
			}
			end_IL_0033:;
		}
		finally
		{
			if (res.HasError)
			{
				await AggregatorExtention.ShowReturnedSavedFuncInfoAsync<FrmRequestedVacation>(this, res, "پیغام سیستم", "UcAttPersonLeave1_OnConfirm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\AttPersonLeave\\FrmRequestedVacation.cs");
			}
			else
			{
				ucAttPersonLeave1.Initialize(((CacheBaseForm)this).ContextGuid, default(CancellationToken));
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (ucAttPersonLeave1 != null)
				{
					ucAttPersonLeave1.OnConfirm -= UcAttPersonLeave1_OnConfirm;
					ucAttPersonLeave1.OnReject -= UcAttPersonLeave1_OnReject;
				}
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmRequestedVacation(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			InitializeComponent();
			ucAttPersonLeave1.Initialize(contextGuid, default(CancellationToken));
			ucAttPersonLeave1.OnConfirm += UcAttPersonLeave1_OnConfirm;
			ucAttPersonLeave1.OnReject += UcAttPersonLeave1_OnReject;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void FrmRequestedVacation_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void FrmRequestedVacation_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void InitializeComponent()
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		ucAttPersonLeave1 = new UcAttPersonLeave();
		((Control)this).SuspendLayout();
		((Control)ucAttPersonLeave1).BackColor = Color.FromArgb(175, 210, 255);
		((Control)ucAttPersonLeave1).Dock = (DockStyle)5;
		((Control)ucAttPersonLeave1).Font = new Font("Microsoft Sans Serif", 8.25f);
		((Control)ucAttPersonLeave1).Location = new Point(0, 0);
		((Control)ucAttPersonLeave1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucAttPersonLeave1).Name = "ucAttPersonLeave1";
		((Control)ucAttPersonLeave1).RightToLeft = (RightToLeft)1;
		((Control)ucAttPersonLeave1).Size = new Size(1169, 516);
		((Control)ucAttPersonLeave1).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 17f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1169, 516);
		((Control)this).Controls.Add((Control)(object)ucAttPersonLeave1);
		((Control)this).Font = new Font("B Titr", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(3, 4, 3, 4);
		((Control)this).Name = "FrmRequestedVacation";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "فرم درخواست مرخصی ها";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosed += new FormClosedEventHandler(FrmRequestedVacation_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(FrmRequestedVacation_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}

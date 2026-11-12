using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;

namespace Stimulsoft.Base.Helpers;

public static class StiComponentProgressHelper
{
	public const float ProgressDelta = 10f;

	public const int TimerInterval = 50;

	private static Timer timer = new Timer();

	private static bool lockCompletedProgressHandler;

	private static object lockObject = new object();

	private static ObservableCollection<IStiAppComponent> hash = new ObservableCollection<IStiAppComponent>();

	private static Dictionary<IStiAppComponent, long> dictTick = new Dictionary<IStiAppComponent, long>();

	public static float CurrentValue { get; set; }

	public static event EventHandler Tick;

	public static event EventHandler CompletedProgress;

	private static void InvokeTick()
	{
		StiComponentProgressHelper.Tick?.Invoke(null, EventArgs.Empty);
	}

	private static void InvokeCompletedProgress(object comp)
	{
		if (!lockCompletedProgressHandler)
		{
			StiComponentProgressHelper.CompletedProgress?.Invoke(comp, EventArgs.Empty);
		}
	}

	private static void ActiveProgress_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems.Count > 0)
		{
			InvokeCompletedProgress(e.OldItems[0]);
		}
		if (hash.Count > 0)
		{
			timer.Start();
		}
		else
		{
			timer.Stop();
		}
	}

	private static void Timer_Tick(object sender, EventArgs e)
	{
		CurrentValue += 10f;
		if (CurrentValue >= 360f)
		{
			CurrentValue = 0f;
		}
		InvokeTick();
	}

	public static void Init()
	{
		timer.Interval = 50;
		timer.Tick += Timer_Tick;
		hash.CollectionChanged += ActiveProgress_CollectionChanged;
	}

	public static void Add(IStiAppComponent comp)
	{
		lock (lockObject)
		{
			if (!hash.Contains(comp))
			{
				hash.Add(comp);
			}
			if (!dictTick.ContainsKey(comp))
			{
				dictTick.Add(comp, Environment.TickCount);
			}
		}
	}

	public static StiProgressStatus Contains(IStiAppComponent comp)
	{
		lock (lockObject)
		{
			if (!hash.Contains(comp))
			{
				return StiProgressStatus.None;
			}
			if (!dictTick.ContainsKey(comp))
			{
				return StiProgressStatus.Long;
			}
			if (IsHidenProgress(dictTick[comp]))
			{
				return StiProgressStatus.Hiden;
			}
			if (IsLongProgress(dictTick[comp]))
			{
				return StiProgressStatus.Long;
			}
			return StiProgressStatus.Short;
		}
	}

	public static bool IsActiveProgress(StiProgressStatus status)
	{
		if (status != StiProgressStatus.Short)
		{
			return status == StiProgressStatus.Long;
		}
		return true;
	}

	public static bool IsLongProgress(long tick)
	{
		return Environment.TickCount - tick > 2000;
	}

	public static bool IsHidenProgress(long tick)
	{
		return Environment.TickCount - tick < 500;
	}

	public static void Remove(IStiAppComponent comp, bool lockCompledProgressr = false)
	{
		lock (lockObject)
		{
			lockCompletedProgressHandler = lockCompledProgressr;
			if (hash.Contains(comp))
			{
				hash.Remove(comp);
			}
			if (dictTick.ContainsKey(comp))
			{
				dictTick.Remove(comp);
			}
			lockCompletedProgressHandler = false;
		}
	}

	public static List<IStiAppComponent> FetchAllComponents()
	{
		lock (lockObject)
		{
			return hash.Where((IStiAppComponent c) => c != null).ToList();
		}
	}

	public static void Dispose()
	{
		timer.Tick -= Timer_Tick;
		hash.CollectionChanged -= ActiveProgress_CollectionChanged;
	}
}

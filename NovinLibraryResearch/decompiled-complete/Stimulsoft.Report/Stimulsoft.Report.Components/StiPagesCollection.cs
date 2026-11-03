using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.SaveLoad;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[Serializable]
public class StiPagesCollection : CollectionBase, IStiStateSaveRestore, IStiJsonReportObject
{
	public class PageMetric
	{
		public int BlockIndex;

		public int PageIndex;

		public int OffsetInBlock;

		public int SizeInBlock;
	}

	public class CacheL2
	{
		public class Block
		{
			public delegate void SavePageBlockToStorageDelegate(byte[] bytes, string blockGuid, List<PageMetric> pageMetric);

			public delegate byte[] LoadPageBlockFromStorageDelegate(string blockGuid);

			public CacheL2 ParentCache;

			public List<int> StoredPageNumbers;

			public string Guid;

			public bool IsSaved;

			public bool IsChanged;

			public List<PageMetric> Metrics;

			public bool HasFreeSpace
			{
				get
				{
					if (ParentCache.blocks.IndexOf(this) == 0 && StoredPageNumbers.Count > 4)
					{
						return false;
					}
					int num = 0;
					foreach (int storedPageNumber in StoredPageNumbers)
					{
						if (ParentCache.pagesData[storedPageNumber] != null)
						{
							num += ((byte[])ParentCache.pagesData[storedPageNumber]).Length;
						}
					}
					return num < 5000000;
				}
			}

			public bool NeedLoad
			{
				get
				{
					if (!IsSaved)
					{
						return false;
					}
					if (StoredPageNumbers.Count == 0)
					{
						return false;
					}
					if (ParentCache.pagesData[StoredPageNumbers[0]] == null)
					{
						return true;
					}
					return false;
				}
			}

			public void Save()
			{
				if (ParentCache.pages.SavePageBlockToStorageIncoming != null)
				{
					using MemoryStream memoryStream = new MemoryStream();
					SaveToStream(memoryStream);
					ParentCache.pages.SavePageBlockToStorageIncoming(memoryStream.ToArray(), Guid, Metrics);
				}
				else
				{
					string pageCacheName = StiReportCache.GetPageCacheName(ParentCache.ReportCachePath, Guid);
					StiFileUtils.ProcessReadOnly(pageCacheName);
					using FileStream stream = new FileStream(pageCacheName, FileMode.Create, FileAccess.Write);
					SaveToStream(stream);
				}
				IsSaved = true;
				IsChanged = false;
			}

			private void SaveToStream(Stream stream)
			{
				for (int i = 0; i < StoredPageNumbers.Count; i++)
				{
					int num = StoredPageNumbers[i];
					byte[] array = (byte[])ParentCache.pagesData[num];
					Metrics[i].OffsetInBlock = (int)stream.Position;
					Metrics[i].SizeInBlock = array.Length;
					stream.Write(array, 0, array.Length);
				}
			}

			public void Load()
			{
				if (ParentCache.pages.LoadPageBlockFromStorageIncoming != null)
				{
					byte[] buffer = ParentCache.pages.LoadPageBlockFromStorageIncoming(Guid);
					using MemoryStream stream = new MemoryStream(buffer);
					LoadFromStream(stream);
				}
				else
				{
					string pageCacheName = StiReportCache.GetPageCacheName(ParentCache.ReportCachePath, Guid);
					StiFileUtils.ProcessReadOnly(pageCacheName);
					using FileStream stream2 = new FileStream(pageCacheName, FileMode.Open, FileAccess.Read);
					LoadFromStream(stream2);
				}
				IsChanged = false;
			}

			private void LoadFromStream(Stream stream)
			{
				for (int i = 0; i < StoredPageNumbers.Count; i++)
				{
					int num = StoredPageNumbers[i];
					byte[] array = new byte[Metrics[i].SizeInBlock];
					stream.Read(array, 0, array.Length);
					ParentCache.pagesData[num] = array;
				}
			}

			public override string ToString()
			{
				return $"CacheBlock, pos={ParentCache.blocks.IndexOf(this)}";
			}

			public Block(CacheL2 parent)
			{
				ParentCache = parent;
				StoredPageNumbers = new List<int>();
				Guid = System.Guid.NewGuid().ToString().Replace("-", "");
				Metrics = new List<PageMetric>();
			}
		}

		internal class PageToSave
		{
			public StiPage Page;

			public int PageNumber;

			public byte[] ResultData;

			public BackgroundWorker Worker;

			public bool ClearContent;
		}

		private const int MaxBlockSize = 5000000;

		private const int MaxQuickCacheSize = 5;

		private Hashtable pageToBlock;

		private Hashtable pagesData;

		private List<Block> blocks;

		private List<Block> quickCache;

		internal List<PageToSave> pagesToSave;

		private BackgroundWorker mainWorker;

		private object lockPagesToSave = new object();

		private object lockBlocks = new object();

		internal string ReportCachePath;

		internal StiPagesCollection pages;

		public void AddPageToProcess(StiPage page, int pageNumber, bool clearContent)
		{
			if (page.Report != null && page.Report.ReportPass == StiReportPass.First)
			{
				page.DisposeImagesAndClearComponents();
				return;
			}
			PageToSave pageToSave = new PageToSave();
			pageToSave.Page = page;
			pageToSave.PageNumber = pageNumber;
			pageToSave.ClearContent = clearContent;
			pageToSave.Worker = new BackgroundWorker();
			pageToSave.Worker.DoWork += AddPageToProcess_Worker_DoWork;
			lock (lockPagesToSave)
			{
				pagesToSave.Add(pageToSave);
			}
			pageToSave.Worker.RunWorkerAsync(pageToSave);
			if (mainWorker == null)
			{
				mainWorker = new BackgroundWorker();
				mainWorker.DoWork += mainWorker_DoWork;
				mainWorker.RunWorkerAsync();
			}
		}

		private void AddPageToProcess_Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Invalid comparison between Unknown and I4
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			PageToSave pageToSave = e.Argument as PageToSave;
			try
			{
				StiPage page = pageToSave.Page;
				StiReport report = pages.Report;
				if (page.Report != null && (int)page.Report.EngineVersion == 2)
				{
					StiRenderProviderV2.ProcessPageToCache(page.Report, page, final: true);
				}
				if (page != null && report.RenderedPages.NotCachedPages != null)
				{
					lock (report.RenderedPages.NotCachedPages)
					{
						int num = report.RenderedPages.NotCachedPages.IndexOf(page);
						if (num != -1)
						{
							report.RenderedPages.NotCachedPages.RemoveAt(num);
						}
					}
				}
				page.Report = null;
				if (string.IsNullOrEmpty(report.ReportCachePath))
				{
					report.ReportCachePath = StiReportCache.CreateNewCache();
					StiOptions.Engine.GlobalEvents.InvokeReportCacheCreated(report, EventArgs.Empty);
					ReportCachePath = report.ReportCachePath;
				}
				StiSerializing sr = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
				using (MemoryStream memoryStream = new MemoryStream())
				{
					pages.SerializePage(memoryStream, sr, pageToSave.Page, pages.SavePageBlockToStorageIncoming != null && !pagesData.ContainsKey(pageToSave.PageNumber));
					pageToSave.ResultData = memoryStream.ToArray();
				}
				page.Report = report;
			}
			catch
			{
				pageToSave.ResultData = new byte[0];
			}
			pageToSave.Worker.DoWork -= AddPageToProcess_Worker_DoWork;
		}

		private void mainWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			int num = 0;
			int num2 = 0;
			while (true)
			{
				lock (lockPagesToSave)
				{
					bool flag = true;
					while (flag)
					{
						flag = false;
						if (pagesToSave.Count <= 0)
						{
							continue;
						}
						foreach (PageToSave item in pagesToSave)
						{
							if (item.ResultData == null)
							{
								continue;
							}
							pagesToSave.Remove(item);
							Save(item.PageNumber, item.ResultData);
							item.Worker.Dispose();
							if (item.ClearContent)
							{
								item.Page.DisposeImagesAndClearComponents();
							}
							if (StiOptions.Engine.ReportCache.AllowGCCollect)
							{
								num += item.ResultData.Length;
								if (num > 25000000)
								{
									num = 0;
									num2++;
									GC.Collect();
									if (num2 >= 4)
									{
										num2 = 0;
										GC.WaitForPendingFinalizers();
									}
								}
							}
							flag = true;
							break;
						}
					}
				}
				Thread.Sleep(20);
			}
		}

		public byte[] GetPage(int pageNumber)
		{
			while (!pagesData.ContainsKey(pageNumber))
			{
				bool flag = false;
				lock (lockPagesToSave)
				{
					foreach (PageToSave item in pagesToSave)
					{
						if (item.PageNumber == pageNumber)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return null;
					}
				}
				Thread.Sleep(20);
			}
			return Load(pageNumber);
		}

		public void CheckForPageInPagesToSave(int pageNumber)
		{
			while (true)
			{
				bool flag = false;
				lock (lockPagesToSave)
				{
					foreach (PageToSave item in pagesToSave)
					{
						if (item.PageNumber == pageNumber)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
				Thread.Sleep(20);
			}
		}

		private void Save(int pageNumber, byte[] pageData)
		{
			lock (lockBlocks)
			{
				Block block = GetBlock(pageNumber);
				if (!pagesData.ContainsKey(pageNumber))
				{
					int num = blocks.IndexOf(block);
					block.StoredPageNumbers.Add(pageNumber);
					block.Metrics.Add(new PageMetric
					{
						BlockIndex = num,
						PageIndex = pageNumber
					});
					pageToBlock[pageNumber] = num;
				}
				byte[] oldData = (byte[])pagesData[pageNumber];
				if (isDataChanged(pageData, oldData))
				{
					pagesData[pageNumber] = pageData;
					block.IsChanged = true;
				}
			}
		}

		private bool isDataChanged(byte[] newData, byte[] oldData)
		{
			if (oldData == null && newData == null)
			{
				return false;
			}
			if (oldData == null && newData != null)
			{
				return true;
			}
			if (oldData != null && newData == null)
			{
				return true;
			}
			if (oldData.Length != newData.Length)
			{
				return true;
			}
			for (int i = 0; i < newData.Length; i++)
			{
				if (oldData[i] != newData[i])
				{
					return true;
				}
			}
			return false;
		}

		private byte[] Load(int pageNumber)
		{
			lock (lockBlocks)
			{
				Block block = GetBlock(pageNumber);
				if (pagesData.ContainsKey(pageNumber))
				{
					return (byte[])pagesData[pageNumber];
				}
				return null;
			}
		}

		private Block GetBlock(int pageNumber)
		{
			int blockIndex = GetBlockIndex(pageNumber);
			Block block = blocks[blockIndex];
			if (block.NeedLoad)
			{
				block.Load();
			}
			if (quickCache.Contains(block))
			{
				return block;
			}
			quickCache.Add(block);
			if (quickCache.Count > 5)
			{
				Block block2 = quickCache[0];
				quickCache.RemoveAt(0);
				if (!block2.IsSaved || block2.IsChanged)
				{
					block2.Save();
				}
				foreach (int storedPageNumber in block2.StoredPageNumbers)
				{
					pagesData[storedPageNumber] = null;
				}
				block2.IsSaved = true;
			}
			return block;
		}

		private int GetBlockIndex(int pageNumber)
		{
			if (pageToBlock.ContainsKey(pageNumber))
			{
				return (int)pageToBlock[pageNumber];
			}
			if (blocks.Count == 0)
			{
				blocks.Add(new Block(this));
			}
			int num = blocks.Count - 1;
			Block block = blocks[num];
			if (block.HasFreeSpace)
			{
				return num;
			}
			if (num == 0)
			{
				block.Save();
			}
			blocks.Add(new Block(this));
			return blocks.Count - 1;
		}

		public void Clear()
		{
			StopMainWorker(final: true);
			pageToBlock.Clear();
			pagesData.Clear();
			blocks.Clear();
			quickCache.Clear();
		}

		private void StopMainWorker(bool final)
		{
			if (mainWorker != null)
			{
				while (pagesToSave.Count != 0)
				{
					Thread.Sleep(20);
				}
				if (final)
				{
					mainWorker.Dispose();
					mainWorker = null;
				}
			}
		}

		public void Flush(bool final)
		{
			StopMainWorker(final);
			lock (lockBlocks)
			{
				foreach (Block item in quickCache)
				{
					if (!item.IsSaved || item.IsChanged)
					{
						item.Save();
					}
				}
			}
		}

		public CacheL2(StiPagesCollection pages)
		{
			this.pages = pages;
			pageToBlock = new Hashtable();
			pagesData = new Hashtable();
			blocks = new List<Block>();
			quickCache = new List<Block>();
			pagesToSave = new List<PageToSave>();
		}
	}

	private class PageStore
	{
		public bool SavePage { get; set; }

		public StiPage Page { get; set; }

		public bool IsNotSaved => SavePage;

		public PageStore(StiPage page, bool savePage)
		{
			Page = page;
			SavePage = savePage;
		}
	}

	private int amountOfProcessedPagesForStartGCCollect;

	private CacheL2 cacheL2;

	private Hashtable pageHashToCacheIndex;

	public CacheL2.Block.SavePageBlockToStorageDelegate SavePageBlockToStorageIncoming;

	public CacheL2.Block.LoadPageBlockFromStorageDelegate LoadPageBlockFromStorageIncoming;

	public StiPage this[int index]
	{
		get
		{
			StiPage stiPage = base.List[index] as StiPage;
			GetPage(stiPage);
			return stiPage;
		}
		set
		{
			GetPage(value);
			base.List[index] = value;
		}
	}

	public StiPage this[string name]
	{
		get
		{
			name = name.ToLowerInvariant();
			lock (base.List.SyncRoot)
			{
				foreach (StiPage item in base.List)
				{
					if (item.Name.ToLowerInvariant() == name)
					{
						GetPage(item);
						return item;
					}
				}
			}
			return null;
		}
		set
		{
			name = name.ToLowerInvariant();
			for (int i = 0; i < base.List.Count; i++)
			{
				StiComponent stiComponent = base.List[i] as StiComponent;
				if (stiComponent.Name.ToLowerInvariant() == name)
				{
					GetPage(value);
					base.List[i] = value;
					return;
				}
			}
			Add(value);
		}
	}

	public StiPage[] Items => (StiPage[])base.InnerList.ToArray(typeof(StiPage));

	public bool CanUseCacheMode { get; set; }

	public StiReport Report { get; set; }

	public bool CacheMode { get; set; }

	public bool ContainsDashboards
	{
		get
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (current is IStiDashboard)
					{
						return true;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return false;
		}
	}

	public bool ContainsScreens
	{
		get
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (current is IStiScreen)
					{
						return true;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return false;
		}
	}

	public bool ContainsOnlyDashboards
	{
		get
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (!(current is IStiDashboard))
					{
						return false;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return true;
		}
	}

	public bool ContainsOnlyScreens
	{
		get
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (!(current is IStiScreen))
					{
						return false;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return true;
		}
	}

	internal StiSerializing Serialization { get; set; }

	private List<PageStore> QuickCachedPages { get; set; }

	internal List<StiPage> NotCachedPages { get; set; }

	public event StiSaveLoadPageEventHandler SavePageToCache;

	public event StiSaveLoadPageEventHandler LoadPageFromCache;

	public event StiLoadPageFromServerEventHandler LoadPageFromServer;

	public event EventHandler PageAdded;

	public event EventHandler PageRemoved;

	public event EventHandler PageCleared;

	public void CheckCacheL2()
	{
		if (this.SavePageToCache != null || (StiOptions.Engine.ReportCache.ThreadMode != StiReportCacheThreadMode.On && (StiOptions.Engine.ReportCache.ThreadMode != StiReportCacheThreadMode.Auto || Environment.ProcessorCount <= 1 || StiOptions.Configuration.IsWeb)))
		{
			return;
		}
		bool flag = Report != null && Report.Collate > 1;
		if (cacheL2 == null && !flag)
		{
			cacheL2 = new CacheL2(this);
			if (Report != null)
			{
				cacheL2.ReportCachePath = Report.ReportCachePath;
			}
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (base.List.Count == 0)
		{
			return null;
		}
		JObject val = new JObject();
		int num = 0;
		foreach (StiPage item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			StiPage stiPage = null;
			JObject val = (JObject)item.Value;
			switch (val.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>())
			{
			case "StiDashboard":
				stiPage = StiDashboardCreator.CreateDashboard(Report) as StiPage;
				if (stiPage == null)
				{
					throw new StiDashboardNotSupportedException();
				}
				break;
			case "StiScreen":
				stiPage = StiAppCreator.CreateScreen(Report) as StiPage;
				if (stiPage == null)
				{
					throw new StiAppNotSupportedException();
				}
				break;
			case "StiPage":
				stiPage = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage") as StiPage;
				break;
			case "StiForm":
				stiPage = new StiForm();
				break;
			}
			Add(stiPage);
			stiPage.LoadFromJsonObject(val);
		}
	}

	public List<StiPage> ToList()
	{
		return base.InnerList.Cast<StiPage>().ToList();
	}

	public void Add(StiPage page)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (Report != null)
		{
			page.DisplayRectangle = page.Unit.ConvertFromHInches(page.DisplayRectangle);
		}
		if (Report != null && Report.IsDesigning && string.IsNullOrEmpty(page.Name))
		{
			page.Name = StiNameCreation.CreateName(Report, StiNameCreation.GenerateName(page));
		}
		if (string.IsNullOrEmpty(page.Name))
		{
			if (Report != null && Report.IsDesigning)
			{
				page.Name = StiNameCreation.CreateName(Report, StiNameCreation.GenerateName(page));
			}
			else
			{
				page.Name = StiNameCreation.CreateSimpleName(Report, StiNameCreation.GenerateName(page));
			}
		}
		AddV2Internal(page);
	}

	internal void AddV2Internal(StiPage page)
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Invalid comparison between Unknown and I4
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Invalid comparison between Unknown and I4
		if (page.Report == null)
		{
			page.Report = Report;
		}
		base.List.Add(page);
		if (Report == null || !CanUseCacheMode)
		{
			return;
		}
		if (CacheMode)
		{
			AddPageToQuickCache(page, savePage: true);
		}
		else
		{
			if (Report.ReportCacheMode != StiReportCacheMode.Auto || base.Count <= StiOptions.Engine.ReportCache.LimitForStartUsingCache)
			{
				return;
			}
			CacheMode = true;
			CheckCacheL2();
			int num = base.Count;
			lock (((ICollection)this).SyncRoot)
			{
				IEnumerator enumerator = GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						StiPage stiPage = (StiPage)enumerator.Current;
						if (Report != null && (int)Report.EngineVersion == 1)
						{
							StiPostProcessProviderV1.PostProcessPage(stiPage, page.PageInfoV1.PageNumber == 1, isLastPage: false);
							StiPostProcessProviderV1.PostProcessPrimitives(stiPage);
						}
						AddPageToQuickCache(stiPage, savePage: true);
						if (stiPage.Report != null && (int)stiPage.Report.EngineVersion == 2)
						{
							StiRenderProviderV2.ProcessPageToCache(stiPage.Report, stiPage, final: false);
						}
						num--;
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
	}

	public void AddRange(StiPage[] pages)
	{
		lock (pages.SyncRoot)
		{
			foreach (StiPage page in pages)
			{
				Add(page);
			}
		}
	}

	public void AddRange(StiPagesCollection pages)
	{
		lock (((ICollection)pages).SyncRoot)
		{
			foreach (StiPage page in pages)
			{
				Add(page);
			}
		}
	}

	public bool Contains(StiPage page)
	{
		return base.List.Contains(page);
	}

	public int IndexOf(StiPage page)
	{
		return base.List.IndexOf(page);
	}

	public void Insert(int index, StiPage page)
	{
		base.List.Insert(index, page);
	}

	public void Remove(StiPage page)
	{
		base.List.Remove(page);
	}

	public void Remove(int startIndex, int endCount)
	{
		while (endCount > 0)
		{
			RemoveAt(base.Count - 1);
			endCount--;
		}
		while (startIndex > 0)
		{
			RemoveAt(0);
			startIndex--;
		}
	}

	internal StiPage GetPageWithoutCache(int pageIndex)
	{
		return base.List[pageIndex] as StiPage;
	}

	internal void SetPageWithoutCache(int pageIndex, StiPage page)
	{
		base.List[pageIndex] = page;
	}

	public void SortByPriority()
	{
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiPage stiPage = (StiPage)enumerator.Current;
					stiPage.Components.SortByPriority();
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		InvokePageAdded(value, EventArgs.Empty);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		InvokePageRemoved(value, EventArgs.Empty);
	}

	protected override void OnClearComplete()
	{
		base.OnClearComplete();
		InvokePageCleared(this, EventArgs.Empty);
	}

	public StiComponent GetComponentByName(string componentName)
	{
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiPage stiPage = (StiPage)enumerator.Current;
					StiComponent componentByName = stiPage.Components.GetComponentByName(componentName, stiPage);
					if (componentByName != null)
					{
						return componentByName;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
		return null;
	}

	public StiComponent GetComponentByGuid(string guid)
	{
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiPage stiPage = (StiPage)enumerator.Current;
					StiComponent componentByGuid = stiPage.Components.GetComponentByGuid(guid, stiPage);
					if (componentByGuid != null)
					{
						return componentByGuid;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
		return null;
	}

	private static void SetParent(StiContainer parent)
	{
		lock (((ICollection)parent.Components).SyncRoot)
		{
			foreach (StiComponent component in parent.Components)
			{
				component.Parent = parent;
				if (component is StiContainer parent2)
				{
					SetParent(parent2);
				}
			}
		}
	}

	public virtual void SaveState(string stateName)
	{
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiPage stiPage = (StiPage)enumerator.Current;
					stiPage.SaveState(stateName);
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	public virtual void RestoreState(string stateName)
	{
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiPage stiPage = (StiPage)enumerator.Current;
					stiPage.RestoreState(stateName);
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	public virtual void ClearAllStates()
	{
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiPage stiPage = (StiPage)enumerator.Current;
					stiPage.ClearAllStates();
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	internal void AddPageToQuickCache(StiPage page, bool savePage)
	{
		if (QuickCachedPages == null)
		{
			QuickCachedPages = new List<PageStore>();
		}
		PageStore pageStore = null;
		foreach (PageStore quickCachedPage in QuickCachedPages)
		{
			if (quickCachedPage.Page == page)
			{
				pageStore = quickCachedPage;
				break;
			}
		}
		if (pageStore == null)
		{
			QuickCachedPages.Add(new PageStore(page, savePage));
		}
		else
		{
			QuickCachedPages.Remove(pageStore);
			QuickCachedPages.Add(new PageStore(page, savePage || pageStore.SavePage));
		}
		if (QuickCachedPages.Count <= StiOptions.Engine.ReportCache.AmountOfQuickAccessPages)
		{
			return;
		}
		PageStore pageStore2 = QuickCachedPages[0];
		bool flag = true;
		if (cacheL2 == null)
		{
			if (pageStore2.SavePage)
			{
				SavePage(pageStore2.Page);
			}
		}
		else if (pageStore2.SavePage)
		{
			cacheL2.AddPageToProcess(pageStore2.Page, GetCacheIndexFromPageHash(pageStore2.Page), clearContent: true);
			flag = false;
		}
		if (flag)
		{
			pageStore2.Page.DisposeImagesAndClearComponents();
		}
		QuickCachedPages.RemoveAt(0);
		if (cacheL2 == null && StiOptions.Engine.ReportCache.AllowGCCollect)
		{
			int num = StiOptions.Engine.ReportCache.AmountOfProcessedPagesForStartGCCollect;
			amountOfProcessedPagesForStartGCCollect++;
			if (amountOfProcessedPagesForStartGCCollect >= num)
			{
				amountOfProcessedPagesForStartGCCollect = 0;
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}
	}

	public bool IsNotSavedPage(StiPage page)
	{
		lock (((ICollection)QuickCachedPages).SyncRoot)
		{
			foreach (PageStore quickCachedPage in QuickCachedPages)
			{
				if (quickCachedPage.Page == page)
				{
					return quickCachedPage.IsNotSaved;
				}
			}
		}
		return false;
	}

	public void MarkPageAsNotSaved(StiPage page)
	{
		lock (((ICollection)QuickCachedPages).SyncRoot)
		{
			foreach (PageStore quickCachedPage in QuickCachedPages)
			{
				if (quickCachedPage.Page == page)
				{
					quickCachedPage.SavePage = true;
				}
			}
		}
	}

	public void GetPage(StiPage page)
	{
		if (this.LoadPageFromServer != null)
		{
			if (page.Components.Count == 0)
			{
				this.LoadPageFromServer(page, EventArgs.Empty);
			}
		}
		else
		{
			if (Report != null && (!CacheMode || Report.ReportPass == StiReportPass.First))
			{
				return;
			}
			if (cacheL2 != null)
			{
				cacheL2.CheckForPageInPagesToSave(IndexOf(page));
			}
			if (page.Components.Count == 0)
			{
				if (Report?.EngineV1?.ProgressHelper != null && Report.EngineV1.ProgressHelper.AllowCachedPagesCache)
				{
					Report.InvokeRendering();
					Report.EngineV1.ProgressHelper.ProcessInCache(page);
				}
				if (Report?.Engine?.ProgressHelper != null && Report.Engine.ProgressHelper.AllowCachedPagesCache)
				{
					Report.InvokeRendering();
					Report.Engine.ProgressHelper.ProcessInCache(page);
				}
				LoadPage(page);
				AddPageToQuickCache(page, savePage: false);
			}
		}
	}

	public void SavePage(StiPage page, bool clearContent = true)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		if (page.Report != null && (int)page.Report.EngineVersion == 2)
		{
			StiRenderProviderV2.ProcessPageToCache(page.Report, page, final: true);
		}
		if (page != null && Report.RenderedPages.NotCachedPages != null)
		{
			int num = Report.RenderedPages.NotCachedPages.IndexOf(page);
			if (num != -1)
			{
				Report.RenderedPages.NotCachedPages.RemoveAt(num);
			}
		}
		if (page.Report != null && page.Report.ReportPass == StiReportPass.First)
		{
			return;
		}
		if (Serialization == null)
		{
			Serialization = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		}
		string pageCacheName = StiReportCache.GetPageCacheName(Report.ReportCachePath, page.CacheGuid);
		page.Report = null;
		if (this.SavePageToCache != null)
		{
			this.SavePageToCache(page, new StiSaveLoadPageEventArgs(page, IndexOf(page), pageCacheName));
		}
		else
		{
			if (string.IsNullOrEmpty(Report.ReportCachePath))
			{
				Report.ReportCachePath = StiReportCache.CreateNewCache();
				StiOptions.Engine.GlobalEvents.InvokeReportCacheCreated(Report, EventArgs.Empty);
				if (cacheL2 != null)
				{
					cacheL2.ReportCachePath = Report.ReportCachePath;
				}
				pageCacheName = StiReportCache.GetPageCacheName(Report.ReportCachePath, page.CacheGuid);
			}
			if (cacheL2 != null)
			{
				cacheL2.AddPageToProcess(page, GetCacheIndexFromPageHash(page), clearContent);
			}
			else
			{
				StiFileUtils.ProcessReadOnly(pageCacheName);
				using Stream stream = new FileStream(pageCacheName, FileMode.Create, FileAccess.Write);
				SerializePage(stream, Serialization, page);
				stream.Close();
			}
		}
		page.Report = Report;
	}

	internal void SerializePage(Stream stream, StiSerializing sr, StiPage page, bool convertToHInches = false)
	{
		StiReport report = page.Report;
		page.Report = null;
		if (convertToHInches && Report.ReportUnit != StiReportUnitType.HundredthsOfInch)
		{
			page.Convert(Report.Unit, StiUnit.HundredthsOfInch);
		}
		StiXmlDocumentSLService.RegPropertyNames(sr);
		sr.SortProperties = false;
		sr.CheckSerializable = true;
		sr.IgnoreSerializableForContainer = true;
		sr.Serialize((object)page, stream, "StiCache", (StiSerializeTypes)8);
		stream.Flush();
		page.Report = report;
	}

	public void LoadPage(StiPage page)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (Serialization == null)
		{
			Serialization = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter(allowLoadOptimization: true));
		}
		string pageCacheName = StiReportCache.GetPageCacheName(Report.ReportCachePath, page.CacheGuid);
		if (this.LoadPageFromCache != null)
		{
			this.LoadPageFromCache(page, new StiSaveLoadPageEventArgs(page, IndexOf(page), pageCacheName));
		}
		else if (cacheL2 != null)
		{
			byte[] page2 = cacheL2.GetPage(GetCacheIndexFromPageHash(page));
			if (page2 != null && page2.Length != 0)
			{
				using MemoryStream stream = new MemoryStream(page2);
				DeserializePage(stream, Serialization, page);
			}
		}
		else if (File.Exists(pageCacheName))
		{
			using Stream stream2 = new FileStream(pageCacheName, FileMode.Open, FileAccess.Read);
			DeserializePage(stream2, Serialization, page);
		}
		page.Report = Report;
		SetParent(page);
		StiComponentsCollection components = page.GetComponents();
		lock (((ICollection)components).SyncRoot)
		{
			foreach (StiComponent item in components)
			{
				item.Page = page;
			}
		}
		if (cacheL2 != null && StiOptions.Engine.ReportCache.AllowGCCollect)
		{
			int num = StiOptions.Engine.ReportCache.AmountOfProcessedPagesForStartGCCollect;
			amountOfProcessedPagesForStartGCCollect++;
			if (amountOfProcessedPagesForStartGCCollect == num)
			{
				GC.Collect();
			}
			if (amountOfProcessedPagesForStartGCCollect >= num * 2)
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
				amountOfProcessedPagesForStartGCCollect = 0;
			}
		}
	}

	private int GetCacheIndexFromPageHash(StiPage page)
	{
		if (pageHashToCacheIndex == null)
		{
			pageHashToCacheIndex = new Hashtable();
		}
		object obj = pageHashToCacheIndex[page.CacheGuid];
		if (obj != null)
		{
			return (int)obj;
		}
		int count = pageHashToCacheIndex.Count;
		pageHashToCacheIndex[page.CacheGuid] = count;
		return count;
	}

	private void DeserializePage(Stream stream, StiSerializing sr, StiPage page)
	{
		StiXmlDocumentSLService.RegPropertyNames(sr);
		sr.IgnoreSerializableForContainer = true;
		sr.Deserialize((object)page, stream, "StiCache");
		stream.Flush();
		stream.Close();
	}

	public void SaveQuickPagesToCache()
	{
		if (QuickCachedPages == null)
		{
			return;
		}
		lock (((ICollection)QuickCachedPages).SyncRoot)
		{
			foreach (PageStore quickCachedPage in QuickCachedPages)
			{
				if (quickCachedPage.IsNotSaved)
				{
					if (cacheL2 != null)
					{
						cacheL2.CheckForPageInPagesToSave(base.List.IndexOf(quickCachedPage.Page));
					}
					SavePage(quickCachedPage.Page, clearContent: false);
					quickCachedPage.SavePage = false;
				}
			}
		}
	}

	public void Flush(bool final = false)
	{
		if (cacheL2 != null)
		{
			if (final)
			{
				SaveQuickPagesToCache();
			}
			cacheL2.Flush(final);
		}
	}

	protected virtual void OnPageAdded(EventArgs e)
	{
	}

	public virtual void InvokePageAdded(object sender, EventArgs e)
	{
		OnPageAdded(e);
		this.PageAdded?.Invoke(sender, e);
	}

	protected virtual void OnPageRemoved(EventArgs e)
	{
	}

	public virtual void InvokePageRemoved(object sender, EventArgs e)
	{
		OnPageRemoved(e);
		this.PageRemoved?.Invoke(sender, e);
	}

	protected virtual void OnPageCleared(EventArgs e)
	{
	}

	public virtual void InvokePageCleared(object sender, EventArgs e)
	{
		OnPageCleared(e);
		this.PageCleared?.Invoke(sender, e);
		QuickCachedPages?.Clear();
		cacheL2?.Clear();
		pageHashToCacheIndex?.Clear();
	}

	internal static void CopyEventsOfPagesCollection(StiPagesCollection sourcePages, StiPagesCollection destinationPages)
	{
		destinationPages.LoadPageFromCache = sourcePages.LoadPageFromCache;
		destinationPages.SavePageToCache = sourcePages.SavePageToCache;
	}

	public StiPagesCollection(StiReport report, StiPagesCollection originalPages)
	{
		Report = report;
		cacheL2 = originalPages.cacheL2;
		this.LoadPageFromServer = originalPages.LoadPageFromServer;
		CopyEventsOfPagesCollection(originalPages, this);
		QuickCachedPages = originalPages.QuickCachedPages;
		pageHashToCacheIndex = originalPages.pageHashToCacheIndex;
	}

	public StiPagesCollection(StiReport report)
	{
		Report = report;
		if (report != null && report.ReportCacheMode != 0)
		{
			CheckCacheL2();
		}
	}
}

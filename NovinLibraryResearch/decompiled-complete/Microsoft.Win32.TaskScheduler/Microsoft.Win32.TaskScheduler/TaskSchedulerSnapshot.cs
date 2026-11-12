using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Microsoft.Win32.TaskScheduler;

public sealed class TaskSchedulerSnapshot : IXmlSerializable
{
	private const string hdrfile = ".metadata.xml";

	private List<SnapshotItem> items;

	public List<SnapshotItem> Items
	{
		get
		{
			return items ?? (items = ((Path == null) ? new List<SnapshotItem>() : GetArchiveItems(Path)));
		}
		internal set
		{
			items = value;
		}
	}

	public string Path { get; private set; }

	public string TargetServer { get; private set; }

	public DateTime TimeStamp { get; private set; } = DateTime.UtcNow;


	public TaskSchedulerSnapshot(string path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (!File.Exists(path))
		{
			throw new FileNotFoundException("Invalid file location.", "path");
		}
		try
		{
			using ZipArchive zipArchive = ZipFile.OpenRead(path);
			zipArchive.GetEntry(".metadata.xml");
		}
		catch
		{
			throw new InvalidOperationException("Invalid file format.");
		}
		Path = path;
	}

	internal TaskSchedulerSnapshot()
	{
	}

	public static TaskSchedulerSnapshot Create(TaskService ts, string path)
	{
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		return InternalCreate(ts.Token, path, cancellationTokenSource.Token, null);
	}

	public static async Task<TaskSchedulerSnapshot> Create(TaskService.ConnectionToken tsToken, string path, CancellationToken cancelToken, IProgress<Tuple<int, string>> progress)
	{
		return await System.Threading.Tasks.Task.Run(() => InternalCreate(tsToken, path, cancelToken, progress), cancelToken);
	}

	public static TaskSchedulerSnapshot Open(string path)
	{
		return new TaskSchedulerSnapshot(path);
	}

	public async System.Threading.Tasks.Task Restore(TaskService.ConnectionToken tsToken, IEnumerable<string> itemPaths, bool applyAccessRights, bool overwriteExisting, IDictionary<string, string> passwords, CancellationToken cancelToken, IProgress<Tuple<int, string>> progress)
	{
		if (itemPaths == null)
		{
			throw new ArgumentNullException("itemPaths");
		}
		List<SnapshotItem> items = (from i in Items
			where i is TaskSnapshot
			select i into a
			join b in itemPaths on a.Path equals b
			select a).ToList();
		if (items.Count != itemPaths.Count())
		{
			throw new ArgumentException("Unable to locate matching tasks to all values of itemPaths.", "itemPaths");
		}
		await System.Threading.Tasks.Task.Run(delegate
		{
			InternalRestore(tsToken, items, applyAccessRights, overwriteExisting, passwords, cancelToken, progress);
		});
	}

	public async System.Threading.Tasks.Task Restore(TaskService.ConnectionToken tsToken, ICollection<SnapshotItem> items, bool applyAccessRights, bool overwriteExisting, IDictionary<string, string> passwords, CancellationToken cancelToken, IProgress<Tuple<int, string>> progress)
	{
		if (items == null)
		{
			throw new ArgumentNullException("items");
		}
		await System.Threading.Tasks.Task.Run(delegate
		{
			InternalRestore(tsToken, items, applyAccessRights, overwriteExisting, passwords, cancelToken, progress);
		});
	}

	public void Restore(TaskService ts, ICollection<SnapshotItem> items, bool applyAccessRights, bool overwriteExisting, IDictionary<string, string> passwords)
	{
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		InternalRestore(ts.Token, items, applyAccessRights, overwriteExisting, passwords, cancellationTokenSource.Token, null);
	}

	XmlSchema IXmlSerializable.GetSchema()
	{
		return null;
	}

	void IXmlSerializable.ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		if (DateTime.TryParseExact(reader.GetAttribute("timestamp"), "o", CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal, out var result))
		{
			TimeStamp = result;
		}
		TargetServer = reader.GetAttribute("machine");
		reader.GetAttribute("version");
		while (reader.Read())
		{
			if (reader.Name == "Task")
			{
				TaskSnapshot taskSnapshot = new TaskSnapshot(reader.GetAttribute("path"), reader.GetAttribute("sddl"), enabled: true);
				string attribute = reader.GetAttribute("enabled");
				if (attribute != null && attribute == "false")
				{
					taskSnapshot.Enabled = false;
				}
				Items.Add(taskSnapshot);
			}
			else if (reader.Name == "TaskFolder")
			{
				Items.Add(new TaskFolderSnapshot(reader.GetAttribute("path"), reader.GetAttribute("sddl")));
			}
		}
	}

	void IXmlSerializable.WriteXml(XmlWriter writer)
	{
		writer.WriteAttributeString("timestamp", TimeStamp.ToString("o"));
		if (TargetServer != null)
		{
			writer.WriteAttributeString("machine", TargetServer);
		}
		writer.WriteAttributeString("version", "1.0");
		foreach (SnapshotItem item in items)
		{
			if (item is TaskSnapshot taskSnapshot)
			{
				writer.WriteStartElement("Task");
				writer.WriteAttributeString("path", taskSnapshot.Path);
				writer.WriteAttributeString("sddl", taskSnapshot.Sddl);
				if (!taskSnapshot.Enabled)
				{
					writer.WriteAttributeString("enabled", "false");
				}
				writer.WriteEndElement();
			}
			else if (item is TaskFolderSnapshot taskFolderSnapshot)
			{
				writer.WriteStartElement("TaskFolder");
				writer.WriteAttributeString("path", taskFolderSnapshot.Path);
				writer.WriteAttributeString("sddl", taskFolderSnapshot.Sddl);
				writer.WriteEndElement();
			}
		}
	}

	private static List<SnapshotItem> GetArchiveItems(string archiveFile)
	{
		TaskSchedulerSnapshot taskSchedulerSnapshot = null;
		using (ZipArchive zipArchive = ZipFile.OpenRead(archiveFile))
		{
			using (Stream stream = zipArchive.GetEntry(".metadata.xml").Open())
			{
				taskSchedulerSnapshot = new XmlSerializer(typeof(TaskSchedulerSnapshot)).Deserialize(stream) as TaskSchedulerSnapshot;
			}
			if (taskSchedulerSnapshot == null)
			{
				return null;
			}
			for (int i = 0; i < taskSchedulerSnapshot.Items.Count; i++)
			{
				if (taskSchedulerSnapshot.Items[i] is TaskSnapshot taskSnapshot)
				{
					using StreamReader streamReader = new StreamReader(zipArchive.GetEntry(taskSnapshot.Path.TrimStart(new char[1] { '\\' }) + ".xml").Open(), Encoding.UTF8);
					taskSnapshot.TaskDefinitionXml = streamReader.ReadToEnd();
				}
			}
		}
		return taskSchedulerSnapshot?.Items;
	}

	private static TaskSchedulerSnapshot InternalCreate(TaskService.ConnectionToken token, string path, CancellationToken cancelToken, IProgress<Tuple<int, string>> progress)
	{
		TaskService taskService = TaskService.CreateFromToken(token);
		if (File.Exists(path))
		{
			throw new ArgumentException("Output file already exists.", "path");
		}
		int i = 0;
		int count = 0;
		using (FileStream stream = new FileStream(path, FileMode.CreateNew))
		{
			using ZipArchive zipArchive = new ZipArchive(stream, ZipArchiveMode.Create);
			GetCount(taskService.RootFolder);
			TaskSchedulerSnapshot taskSchedulerSnapshot = new TaskSchedulerSnapshot
			{
				TargetServer = (taskService.TargetServer ?? Environment.MachineName)
			};
			GetContents(taskService.RootFolder, taskSchedulerSnapshot.Items, zipArchive);
			taskSchedulerSnapshot.Items.Sort((SnapshotItem t1, SnapshotItem t2) => string.Compare(t1.Path, t2.Path, StringComparison.InvariantCultureIgnoreCase));
			using (StreamWriter textWriter = new StreamWriter(zipArchive.CreateEntry(".metadata.xml").Open(), Encoding.UTF8))
			{
				new XmlSerializer(taskSchedulerSnapshot.GetType()).Serialize(textWriter, taskSchedulerSnapshot);
			}
			taskSchedulerSnapshot.Path = path;
			return taskSchedulerSnapshot;
		}
		void GetContents(TaskFolder f, List<SnapshotItem> list, ZipArchive zip)
		{
			cancelToken.ThrowIfCancellationRequested();
			foreach (Task task in f.Tasks)
			{
				list.Add(new TaskSnapshot(task.Path, task.GetSecurityDescriptorSddlForm((SecurityInfos)15), task.Enabled, task.Xml));
				using (StreamWriter streamWriter = new StreamWriter(zip.CreateEntry(task.Path.TrimStart(new char[1] { '\\' }) + ".xml").Open(), Encoding.Unicode))
				{
					streamWriter.Write(task.Xml);
				}
				cancelToken.ThrowIfCancellationRequested();
				progress?.Report(new Tuple<int, string>(++i * 100 / count, task.Path));
			}
			foreach (TaskFolder subFolder in f.SubFolders)
			{
				list.Add(new TaskFolderSnapshot(subFolder.Path, subFolder.GetSecurityDescriptorSddlForm((SecurityInfos)15)));
				zip.CreateEntry(subFolder.Path.TrimStart(new char[1] { '\\' }) + "\\");
				GetContents(subFolder, list, zip);
				cancelToken.ThrowIfCancellationRequested();
				progress?.Report(new Tuple<int, string>(++i * 100 / count, subFolder.Path));
			}
		}
		void GetCount(TaskFolder f)
		{
			count += f.Tasks.Count;
			foreach (TaskFolder subFolder2 in f.SubFolders)
			{
				count++;
				GetCount(subFolder2);
			}
		}
	}

	private void InternalRestore(TaskService.ConnectionToken token, ICollection<SnapshotItem> items, bool applyAccessRights, bool overwriteExisting, IDictionary<string, string> passwords, CancellationToken cancelToken, IProgress<Tuple<int, string>> progress)
	{
		TaskService ts = TaskService.CreateFromToken(token);
		int num = 0;
		progress?.Report(new Tuple<int, string>(0, ""));
		foreach (SnapshotItem item in items)
		{
			cancelToken.ThrowIfCancellationRequested();
			if (item is TaskSnapshot taskSnapshot)
			{
				TaskDefinition taskDefinition = ts.NewTask();
				taskDefinition.XmlText = taskSnapshot.TaskDefinitionXml;
				string password2 = ((!taskDefinition.Principal.RequiresPassword()) ? null : passwords?[taskDefinition.Principal.ToString()]);
				Task task2 = ts.GetTask(taskSnapshot.Path);
				if (task2 == null)
				{
					CreateTask(taskSnapshot, taskDefinition, password2);
				}
				else if (overwriteExisting)
				{
					task2 = task2.Folder.RegisterTaskDefinition(System.IO.Path.GetFileName(taskSnapshot.Path), taskDefinition, TaskCreation.CreateOrUpdate, taskDefinition.Principal.ToString(), password2, taskDefinition.Principal.LogonType, applyAccessRights ? taskSnapshot.Sddl : null);
					if (!taskSnapshot.Enabled)
					{
						task2.Enabled = false;
					}
				}
			}
			else if (item is TaskFolderSnapshot taskFolderSnapshot)
			{
				TaskFolder folder = ts.GetFolder(taskFolderSnapshot.Path);
				if (folder == null)
				{
					folder = EnsureFolder(taskFolderSnapshot.Path);
				}
				else if (overwriteExisting && applyAccessRights)
				{
					folder.SetSecurityDescriptorSddlForm(taskFolderSnapshot.Sddl);
				}
			}
			progress?.Report(new Tuple<int, string>(++num * 100 / items.Count, item.Path));
		}
		progress?.Report(new Tuple<int, string>(100, ""));
		void CreateTask(TaskSnapshot task, TaskDefinition td, string password)
		{
			string directoryName = System.IO.Path.GetDirectoryName(task.Path);
			Task task3 = EnsureFolder(directoryName).RegisterTaskDefinition(System.IO.Path.GetFileName(task.Path), td, TaskCreation.CreateOrUpdate, td.Principal.ToString(), password, td.Principal.LogonType, applyAccessRights ? task.Sddl : null);
			if (!task.Enabled)
			{
				task3.Enabled = false;
			}
		}
		TaskFolder EnsureFolder(string fpath)
		{
			if (!overwriteExisting)
			{
				TaskFolder folder2 = ts.GetFolder(fpath);
				if (folder2 != null)
				{
					return folder2;
				}
			}
			string[] array = fpath.Split(new char[1] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
			TaskFolder taskFolder = ts.RootFolder;
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				text = text + "\\" + array[i];
				TaskFolder taskFolder2 = (taskFolder.SubFolders.Exists(array[i]) ? taskFolder.SubFolders[array[i]] : null);
				if (taskFolder2 == null)
				{
					taskFolder2 = taskFolder.CreateFolder(array[i], GetFolderSddl(text), exceptionOnExists: false);
				}
				else if (overwriteExisting)
				{
					taskFolder2.SetSecurityDescriptorSddlForm(GetFolderSddl(text));
				}
				taskFolder = taskFolder2;
			}
			return taskFolder;
		}
		string GetFolderSddl(string path)
		{
			return Items?.Find((SnapshotItem tci) => string.Equals(tci.Path, path, StringComparison.OrdinalIgnoreCase)).Sddl;
		}
	}
}

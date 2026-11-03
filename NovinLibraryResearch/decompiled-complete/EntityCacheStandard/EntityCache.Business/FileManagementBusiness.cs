using System;
using System.Threading;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class FileManagementBusiness : Serializable<FileManagementBusiness>, IFileManagement, IHasIntId, IHasContext
{
	private ClsCache cache;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private string _description;

	private string _fileName;

	private string _fullUrl;

	private string _pageUrl;

	private string _title;

	private string _versionTxt;

	public int Id { get; }

	public long CRC { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToShamsi();

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value?.FixString() ?? "";
		}
	}

	public string FileName
	{
		get
		{
			return _fileName;
		}
		set
		{
			_fileName = value?.FixString() ?? "";
		}
	}

	public string FullURL
	{
		get
		{
			return _fullUrl;
		}
		set
		{
			_fullUrl = value?.FixString() ?? "";
		}
	}

	public string PageURL
	{
		get
		{
			return _pageUrl;
		}
		set
		{
			_pageUrl = value?.FixString() ?? "";
		}
	}

	public int PartID { get; set; }

	public string Title
	{
		get
		{
			return _title;
		}
		set
		{
			_title = value?.FixString() ?? "";
		}
	}

	public int Version { get; set; }

	public string VersionTXT
	{
		get
		{
			return _versionTxt;
		}
		set
		{
			_versionTxt = value?.FixString() ?? "";
		}
	}

	public string ContextGuid { get; set; }

	public FileManagementBusiness(ClsCache cache, string contextGuid)
	{
		this.cache = cache;
		ContextGuid = contextGuid;
	}

	public FileManagementBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}

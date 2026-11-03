using System;

namespace PacketParser.Settings.Accounting;

public class MaliYearSettings
{
	private DateTime? _minDate;

	private DateTime? _maxDate;

	private Guid _sanadEKhtetamieGuid = Guid.Empty;

	private Guid _sanadMovaghatGuid = Guid.Empty;

	private Guid _maliYearGuid = Guid.Empty;

	private Guid _anbarGardaniEkhtetamiyeGuid = Guid.Empty;

	private int _year;

	public DateTime MaliyearStart
	{
		get
		{
			if (!_minDate.HasValue)
			{
				_minDate = DateTime.Parse("2011/03/21");
			}
			return new DateTime(_minDate.Value.Year, _minDate.Value.Month, _minDate.Value.Day, 0, 0, 0);
		}
		set
		{
			_minDate = value;
		}
	}

	public DateTime MaliyearEnd
	{
		get
		{
			if (!_maxDate.HasValue)
			{
				_maxDate = DateTime.Parse("2022/03/20");
			}
			return new DateTime(_maxDate.Value.Year, _maxDate.Value.Month, _maxDate.Value.Day, 23, 59, 59);
		}
		set
		{
			_maxDate = value;
		}
	}

	public Guid MaliyearSanadEKhtetamieGuid
	{
		get
		{
			_ = _sanadEKhtetamieGuid != Guid.Empty;
			return _sanadEKhtetamieGuid;
		}
		set
		{
			_sanadEKhtetamieGuid = value;
		}
	}

	public Guid MaliyearSanadMovaghatGuid
	{
		get
		{
			_ = _sanadMovaghatGuid != Guid.Empty;
			return _sanadMovaghatGuid;
		}
		set
		{
			_sanadMovaghatGuid = value;
		}
	}

	public Guid MaliYearSanadBardashtGuid { get; set; }

	public int MaliyearYear
	{
		get
		{
			if (_year <= 0)
			{
				_year = Calendar.GetYearOfDateSh(DateTime.Now.MiladiToCalture());
			}
			return _year;
		}
		set
		{
			_year = ((value <= 0) ? Calendar.GetYearOfDateSh(DateTime.Now.MiladiToCalture()) : value);
		}
	}

	public string MaliyearSqlcnOld { get; set; }

	public Guid MaliyearGuidOld { get; set; }

	public Guid MaliyearGuid
	{
		get
		{
			if (_maliYearGuid == Guid.Empty)
			{
				_maliYearGuid = Guid.NewGuid();
			}
			return _maliYearGuid;
		}
		set
		{
			_maliYearGuid = value;
		}
	}

	public Guid AnbarGardaniEkhtetamiyeGuid
	{
		get
		{
			if (_anbarGardaniEkhtetamiyeGuid == Guid.Empty)
			{
				_anbarGardaniEkhtetamiyeGuid = Guid.NewGuid();
			}
			return _anbarGardaniEkhtetamiyeGuid;
		}
		set
		{
			_anbarGardaniEkhtetamiyeGuid = value;
		}
	}
}

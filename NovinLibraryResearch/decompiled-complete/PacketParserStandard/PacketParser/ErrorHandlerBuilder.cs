using Microsoft.Extensions.Logging;
using PacketParser.Entities;

namespace PacketParser;

public class ErrorHandlerBuilder
{
	private int _seconds;

	private ILogger _logger;

	private string _version;

	private ENSource _enSource;

	private string _path;

	private long _lKSerial;

	private LogLevel _minLogLevel = LogLevel.Error;

	private int _minutes;

	private bool _sendlogToConsole;

	private bool _sendLogToWebService;

	private bool _sendLogToSignalR;

	private LogLevel _consoleMinLogLevel = LogLevel.Debug;

	private LogLevel _telegramMinLogLevel = LogLevel.Error;

	private LogLevel _webServiceMinLogLevel = LogLevel.Error;

	private LogLevel _signalRMinLogLevel = LogLevel.Error;

	private string _consoleLogFormat = "{{short_time}} - {{log_level}} - {{{message}}}";

	private string _departmentUsername;

	public ErrorHandlerBuilder SetLogger(ILogger logger)
	{
		_logger = logger;
		return this;
	}

	public ErrorHandlerBuilder SetVersion(string version)
	{
		_version = version;
		return this;
	}

	public ErrorHandlerBuilder SetEnSource(ENSource eNSource)
	{
		_enSource = eNSource;
		return this;
	}

	public ErrorHandlerBuilder SetPath(string path)
	{
		_path = path;
		return this;
	}

	public ErrorHandlerBuilder SetLKSerial(long serial)
	{
		_lKSerial = serial;
		return this;
	}

	public ErrorHandlerBuilder SetIme(long ime)
	{
		_lKSerial = ime;
		return this;
	}

	public ErrorHandlerBuilder SetMinLogLevel(LogLevel minLogLevel = LogLevel.Error)
	{
		_minLogLevel = minLogLevel;
		return this;
	}

	public ErrorHandlerBuilder SetMinutes(int minutes = 10)
	{
		_minutes = minutes;
		return this;
	}

	public ErrorHandlerBuilder SetDepartmentUsername(string username)
	{
		_departmentUsername = username;
		return this;
	}

	public ErrorHandlerBuilder SetMinSecendsBetweenExeptions(int seconds = 30)
	{
		_seconds = seconds;
		return this;
	}

	public ErrorHandlerBuilder SetLogToConsole(bool logToConsole, LogLevel minLogLevel = LogLevel.Information, string format = "{{short_time}} - {{log_level}} - {{{message}}}")
	{
		_sendlogToConsole = logToConsole;
		_consoleMinLogLevel = minLogLevel;
		_consoleLogFormat = format;
		return this;
	}

	public ErrorHandlerBuilder SetLogToSignalR(LogLevel minLogLevel = LogLevel.Information)
	{
		_sendLogToSignalR = true;
		_signalRMinLogLevel = minLogLevel;
		return this;
	}

	public ErrorHandlerBuilder SetLogToTelegram(LogLevel minLogLevel = LogLevel.Information)
	{
		_telegramMinLogLevel = minLogLevel;
		return this;
	}

	public ErrorHandlerBuilder SetLogToWebService(bool logToWebservice = true, LogLevel minLogLevel = LogLevel.Error)
	{
		_sendLogToWebService = logToWebservice;
		_webServiceMinLogLevel = minLogLevel;
		ErrorHandler.WebServiceLog = _sendLogToWebService;
		return this;
	}

	public void Build()
	{
		ErrorHandler.AddHandler(_logger, screenshot: false, showUser: false, kill: false, _version, _enSource, _path, _lKSerial, "accountingerrorlog", "accountingerrorlog", _minutes, _seconds, _minLogLevel, _sendlogToConsole, _consoleMinLogLevel, _consoleLogFormat, _sendLogToSignalR, _signalRMinLogLevel, _sendLogToWebService, _webServiceMinLogLevel, _departmentUsername);
	}
}

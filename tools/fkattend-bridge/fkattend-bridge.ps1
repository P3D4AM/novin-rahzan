<#
.SYNOPSIS
  HozorPlus FK623 Bridge — READ-ONLY device access for NP756+ (port 4238).

.DESCRIPTION
  Runs a tiny JSON-over-HTTP server ON THIS MACHINE ONLY (127.0.0.1) that loads
  the 32-bit FK623Attend.dll with the EXACT signatures recorded as successful
  against the real NP756+ (32-bit PowerShell, SetDllDirectory, port 4238):

      FK_ConnectNet(machineNo, ip, port, timeout, protocolType, networkPassword, license)
      FK_EnableDevice(handle, 0)                     -- keypad lock; result MUST be exactly 1
      FK_LoadGeneralLogData(handle, 0)               -- readMark 0; result MUST be exactly 1
      FK_GetGeneralLogData(handle, refs...)          -- loop; ret 1 = record, -7 = normal end
      FK_EnableDevice(handle, 1)                     -- ALWAYS restored in finally
      FK_DisConnect(handle)

  NOTE: the confirmed FK623Attend.dll signature of FK_GetGeneralLogData returns
  enrollNumber / verifyMode / inOutMode / DateTime ONLY — NO workCode. The
  bridge therefore emits records WITHOUT a workCode field (it is NEVER
  fabricated) and HozorPlus treats it as optional/empty.

  v2.0.0 — supervised mode (doc: docs/BRIDGE-SUPERVISION.md):
    * The token comes from environment variable FKBRIDGE_TOKEN (preferred) —
      never from the command line, so it never shows in any process list.
      -Token remains available for manual/debug runs only.
    * GET/POST /ping    — unauthenticated liveness probe (returns pid only,
      no sensitive data) used by the supervisor readiness check.
    * POST /health     — NOW TOKEN-AUTHENTICATED (no unauthenticated
      endpoint reveals information beyond "alive").
    * POST /shutdown   — token-authenticated graceful stop (supervisor).
    * Exit codes: 0 = clean stop, 1 = wrong architecture, 2 = script error,
      3 = port already in use, 4 = FK623Attend.dll not found.

  Guarantees (unchanged from v1.4.0):
    * READ-ONLY: no destructive SDK call is EVER made; there is no endpoint
      that could empty/delete logs or change the device clock.
    * Keypad re-enable + disconnect happen in `finally` — a crash mid-read
      cannot leave the device locked. Restore/disconnect errors are logged
      distinctly and surfaced in the response.
    * Requests are serialized (single HTTP loop) → one device operation at a
      time; the device handle is never shared between requests.
    * The network password arrives per request, is used for the single SDK
      connect call and is NEVER echoed, logged or written to disk.
    * Binds to 127.0.0.1 only.

.PREREQUISITES
  * 32-bit PowerShell: %WINDIR%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe
    (START-BRIDGE.bat launches exactly that one; the supervisor does too)
  * FK623Attend.dll (and sibling DLLs) in -DllDir / $env:FKBRIDGE_DLLDIR

.PARAMETER Port
  Local HTTP port (default 8765). Localhost only.

.PARAMETER Token
  Shared secret for manual/debug runs. In supervised mode the token comes
  from the FKBRIDGE_TOKEN environment variable (env wins over this param).

.PARAMETER DllDir
  Directory containing FK623Attend.dll. Env override: FKBRIDGE_DLLDIR.
  Standard production location: <install>\tools\fkattend-bridge\bin

.PARAMETER ProbeExtras
  $false (DEFAULT in v2) → serial/device-time probes stay OFF the critical
  path. Set $true to enable the isolated child probe (debug only).

.EXAMPLE
  # supervised (HozorPlus does this itself — token via env, hidden window):
  powershell -NoProfile -ExecutionPolicy Bypass -File fkattend-bridge.ps1 -Port 8765

  # manual/debug:
  powershell ... -File fkattend-bridge.ps1 -Port 8765 -Token abc -DllDir "C:\path\to\SDK"
#>
param(
  [int]$Port = 8765,
  [string]$Token = '',
  [string]$DllDir = '',
  [bool]$ProbeExtras = $false
)

$ErrorActionPreference = 'Stop'
$script:BridgeVersion = '2.0.0'
$script:Root = $PSScriptRoot

# ---------------------------------------------------------------- environment
if ([Environment]::Is64BitProcess) {
  Write-Host '[fk-bridge] FATAL: this bridge is 32-bit only (x86 FK623Attend.dll).'
  Write-Host '[fk-bridge] Run it via START-BRIDGE.bat or:'
  Write-Host '[fk-bridge]   %WINDIR%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe -NoProfile -ExecutionPolicy Bypass -File fkattend-bridge.ps1'
  exit 1
}

if ($env:FKBRIDGE_DLLDIR) { $DllDir = $env:FKBRIDGE_DLLDIR }
if (-not $DllDir) { $DllDir = $script:Root }
$DllDir = [System.IO.Path]::GetFullPath($DllDir)
if (-not (Test-Path (Join-Path $DllDir 'FK623Attend.dll'))) {
  Write-Host "[fk-bridge] FATAL: FK623Attend.dll not found in: $DllDir"
  Write-Host '[fk-bridge] Production: copy the vendor SDK DLLs into tools\fkattend-bridge\bin next to the app.'
  exit 4
}

# token: env wins (supervised mode — never on the command line); param for debug;
# without either, a random token is generated. It is NEVER printed by this script.
if ($env:FKBRIDGE_TOKEN) { $Token = $env:FKBRIDGE_TOKEN }
if (-not $Token) { $Token = [guid]::NewGuid().ToString('N') }

if ($env:FKBRIDGE_PROBE_EXTRAS) {
  if ($env:FKBRIDGE_PROBE_EXTRAS -eq '1') { $ProbeExtras = $true }
  elseif ($env:FKBRIDGE_PROBE_EXTRAS -eq '0') { $ProbeExtras = $false }
}

# The DLL loader searches the process working directory → run from the SDK dir.
# SetDllDirectory additionally pins the search path (recorded successful method).
Push-Location $DllDir
$env:PATH = "$DllDir;$env:PATH"

Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;
// P/Invoke signatures EXACTLY as recorded successful against the real NP756+
// through the 32-bit FK623Attend.dll. Deliberately minimal and read-only.
public static class FKApi {
  [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  public static extern bool SetDllDirectory(string path);

  [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
  public static extern int FK_ConnectNet(int machine, string ip, int port, int timeoutMs, int protocolType, int password, int license);

  [DllImport("FK623Attend.dll")]
  public static extern int FK_EnableDevice(int handle, byte enable);

  [DllImport("FK623Attend.dll")]
  public static extern int FK_LoadGeneralLogData(int handle, int readMark);

  // confirmed FK623 signature: enroll/verify/inout + DateTime — NO workCode
  [DllImport("FK623Attend.dll")]
  public static extern int FK_GetGeneralLogData(int handle, ref int enroll, ref int verify, ref int inout, ref DateTime dateTime);

  [DllImport("FK623Attend.dll")]
  public static extern int FK_DisConnect(int handle);
}
"@ -ErrorAction Stop
[void][FKApi]::SetDllDirectory($DllDir)

$script:StartedAt = Get-Date

function Write-BridgeLog([string]$line) {
  # console-only logging — the token, request bodies (with the password) and
  # secrets are NEVER logged
  Write-Host "[fk-bridge] $line"
}

function ConvertTo-JValue([object]$v) {
  if ($null -eq $v) { return 'null' }
  return ($v | ConvertTo-Json -Compress)
}

# ------------------------------------------------------------- child probes
# FK_GetSerialNumber / FK_GetDeviceTime are NOT part of the recorded successful
# call set → OFF the critical path by default (v2). When explicitly enabled they
# run in an isolated child process so a signature mismatch can only kill the
# child. The probe reads its JSON config from STDIN (never the command line →
# the password never appears in any process list).
function Invoke-ExtrasProbe([object]$cfg) {
  if (-not $ProbeExtras) { return $null }
  $probeScript = Join-Path $script:Root 'fkattend-probe-extras.ps1'
  if (-not (Test-Path $probeScript)) { return $null }
  try {
    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName = 'powershell.exe'            # inherits the WOW64 (32-bit) context
    $psi.Arguments = "-NoProfile -ExecutionPolicy Bypass -File `"$probeScript`""
    $psi.WorkingDirectory = $DllDir
    $psi.UseShellExecute = $false
    $psi.RedirectStandardInput = $true
    $psi.RedirectStandardOutput = $true
    $psi.RedirectStandardError = $true
    $p = [System.Diagnostics.Process]::Start($psi)
    $p.StandardInput.WriteLine(($cfg | ConvertTo-Json -Compress))
    $p.StandardInput.Close()
    if (-not $p.WaitForExit(20000)) {
      try { $p.Kill() } catch {}
      Write-BridgeLog 'extras probe timed out (serial/time unavailable)'
      return $null
    }
    $out = $p.StandardOutput.ReadToEnd().Trim()
    if ($p.ExitCode -ne 0 -or -not $out) {
      Write-BridgeLog "extras probe failed (exit $($p.ExitCode)) — serial/time unavailable; keep -ProbeExtras 0 (default)"
      return $null
    }
    return ($out | ConvertFrom-Json)
  } catch {
    Write-BridgeLog "extras probe error: $($_.Exception.Message)"
    return $null
  }
}

# ------------------------------------------------------------------- helpers
function Send-Json([System.Net.HttpListenerContext]$c, [int]$status, [string]$body) {
  $bytes = [System.Text.Encoding]::UTF8.GetBytes($body)
  $c.Response.StatusCode = $status
  $c.Response.ContentType = 'application/json; charset=utf-8'
  $c.Response.ContentLength64 = $bytes.Length
  $c.Response.OutputStream.Write($bytes, 0, $bytes.Length)
  try { $c.Response.Close() } catch {}
}

function Test-Token([System.Net.HttpListenerContext]$c) {
  $hdr = $c.Request.Headers['x-bridge-token']
  if (-not $hdr -or ($hdr -ne $Token)) {
    Send-Json $c 401 '{"ok":false,"error":"احراز هویت Bridge نامعتبر است","code":"AUTH"}'
    return $false
  }
  return $true
}

function Read-RequestBody([System.Net.HttpListenerContext]$c) {
  $reader = New-Object System.IO.StreamReader($c.Request.InputStream, [System.Text.Encoding]::UTF8)
  $raw = $reader.ReadToEnd()
  if (-not $raw) { return $null }
  return ($raw | ConvertFrom-Json)
}

function Get-Num([object]$o, [string]$prop, [int]$def) {
  if ($null -eq $o) { return $def }
  $v = $o.$prop
  if ($null -eq $v) { return $def }
  try { return [int]$v } catch { return $def }
}

function Get-Str([object]$o, [string]$prop, [string]$def) {
  if ($null -eq $o) { return $def }
  $v = $o.$prop
  if ($null -eq $v) { return $def }
  return [string]$v
}

# ------------------------------------------------------- device operations
function Invoke-DeviceTest([object]$b) {
  # READ-ONLY: connect → (optional serial/time probe in a CHILD process) →
  # disconnect. No EnableDevice(0) → the keypad is never locked by a test.
  $sw = [System.Diagnostics.Stopwatch]::StartNew()
  $machine = Get-Num $b 'machine' 1
  $ip = Get-Str $b 'ip' ''
  $port = Get-Num $b 'port' 4238
  $timeoutMs = Get-Num $b 'timeoutMs' 5000
  $password = Get-Num $b 'password' 0
  $license = Get-Num $b 'license' 1261

  $handle = [FKApi]::FK_ConnectNet($machine, $ip, $port, $timeoutMs, 0, $password, $license)
  if ($handle -le 0) {
    return @{ ok = $false; error = "اتصال به دستگاه برقرار نشد (کد $handle از FK_ConnectNet) — IP، پورت و رمز شبکهٔ عددی را بررسی کنید"; code = 'CONNECT_FAILED' }
  }
  try {
    $extras = Invoke-ExtrasProbe $b
    $serial = $null
    $deviceTime = $null
    if ($extras) {
      $serial = $extras.serialNumber
      $deviceTime = $extras.deviceTime
    }
    return @{
      ok = $true
      serialNumber = $serial
      deviceTime = $deviceTime
      latencyMs = [int]$sw.ElapsedMilliseconds
      meta = @{ machine = $machine; ip = $ip; port = $port; timeoutMs = $timeoutMs; license = $license }
    }
  } finally {
    [void][FKApi]::FK_DisConnect($handle)
  }
}

function Invoke-DeviceReadLogs([object]$b) {
  # Full read with the recorded successful sequence:
  #   FK_ConnectNet → FK_EnableDevice(0) [must be exactly 1] →
  #   FK_LoadGeneralLogData(handle, 0) [must be exactly 1] →
  #   FK_GetGeneralLogData loop (1 = record, -7 = normal end) →
  #   FK_EnableDevice(1) + FK_DisConnect in finally.
  $swTotal = [System.Diagnostics.Stopwatch]::StartNew()
  $machine = Get-Num $b 'machine' 1
  $ip = Get-Str $b 'ip' ''
  $port = Get-Num $b 'port' 4238
  $timeoutMs = Get-Num $b 'timeoutMs' 5000
  $password = Get-Num $b 'password' 0
  $license = Get-Num $b 'license' 1261

  $handle = [FKApi]::FK_ConnectNet($machine, $ip, $port, $timeoutMs, 0, $password, $license)
  if ($handle -le 0) {
    return @{ ok = $false; error = "اتصال به دستگاه برقرار نشد (کد $handle از FK_ConnectNet) — IP، پورت و رمز شبکهٔ عددی را بررسی کنید"; code = 'CONNECT_FAILED' }
  }
  $connectMs = [int]$swTotal.ElapsedMilliseconds
  $restoreOk = $true
  $disconnectOk = $true
  try {
    # vendor pattern: lock the keypad while reading so data cannot change mid-read.
    # If locking FAILS, reading must NOT start (read-only safety rule §10).
    $enable = [FKApi]::FK_EnableDevice($handle, 0)
    if ($enable -ne 1) {
      return @{ ok = $false; error = "قفل‌کردن صفحه‌کلید دستگاه هنگام خواندن ناموفق بود (کد $enable از FK_EnableDevice) — خواندن انجام نشد"; code = 'ENABLE_FAILED' }
    }

    $load = [FKApi]::FK_LoadGeneralLogData($handle, 0)
    if ($load -ne 1) {
      return @{ ok = $false; error = "بارگذاری حافظهٔ ترددها ناموفق بود (کد $load از FK_LoadGeneralLogData)"; code = 'LOAD_FAILED' }
    }

    # records are hand-serialized for speed on large histories (2947+ rows);
    # the password is never included anywhere in the output.
    # NOTE: no workCode — the confirmed FK623Attend.dll signature does not
    # return one; a fake value must NEVER be produced.
    $sb = New-Object System.Text.StringBuilder
    [void]$sb.Append('[')
    $count = 0
    $enroll = 0; $verify = 0; $inout = 0; $dt = [DateTime]::MinValue
    $ret = [FKApi]::FK_GetGeneralLogData($handle, [ref]$enroll, [ref]$verify, [ref]$inout, [ref]$dt)
    while ($ret -eq 1) {
      if ($count -gt 0) { [void]$sb.Append(',') }
      [void]$sb.Append(('{{"enrollNumber":{0},"verifyMode":{1},"inOutMode":{2},"timestamp":"{3:yyyy-MM-dd HH:mm:ss}"}}' -f `
        $enroll, $verify, $inout, $dt))
      $count++
      if ($count -ge 200000) { break }   # safety valve
      $ret = [FKApi]::FK_GetGeneralLogData($handle, [ref]$enroll, [ref]$verify, [ref]$inout, [ref]$dt)
    }
    [void]$sb.Append(']')
    # -7 = normal end of reading (as observed on the real device);
    # 0 = empty. Anything else is an ABNORMAL end — reported as such, never
    # silently presented as a complete read.
    $endCode = $ret
    $endCodeAbnormal = ($endCode -ne -7 -and $endCode -ne 0)

    $extras = Invoke-ExtrasProbe $b
    $serial = $null
    $deviceTime = $null
    if ($extras) {
      $serial = $extras.serialNumber
      $deviceTime = $extras.deviceTime
    }

    $metaJson = '{{"machine":{0},"ip":{1},"port":{2},"timeoutMs":{3},"license":{4}}}' -f `
      $machine, ($ip | ConvertTo-Json -Compress), $port, $timeoutMs, $license

    $head = '{{"ok":true,"serialNumber":{0},"deviceTime":{1},"latencyMs":{2},"connectMs":{3},"endCode":{4},"endCodeAbnormal":{5},"records":' -f `
      (ConvertTo-JValue $serial), (ConvertTo-JValue $deviceTime), [int]$swTotal.ElapsedMilliseconds, $connectMs, $endCode, ($(if ($endCodeAbnormal) {'true'} else {'false'}))

    return @{
      ok = $true
      json = $head + $sb.ToString() + ',"meta":' + $metaJson + '}'
      recordCount = $count
      endCode = $endCode
    }
  } finally {
    # ALWAYS restore the keypad and disconnect — even on errors above.
    # Restore/disconnect failures are logged distinctly (read-only safety §11).
    try {
      $restore = [FKApi]::FK_EnableDevice($handle, 1)
      if ($restore -ne 1) {
        $restoreOk = $false
        Write-BridgeLog "WARNING: keypad re-enable after read returned $restore (expected 1)"
      }
    } catch {
      $restoreOk = $false
      Write-BridgeLog "WARNING: keypad re-enable after read threw: $($_.Exception.Message)"
    }
    try {
      $disc = [FKApi]::FK_DisConnect($handle)
      if ($disc -ne 1) {
        $disconnectOk = $false
        Write-BridgeLog "WARNING: FK_DisConnect returned $disc (expected 1)"
      }
    } catch {
      $disconnectOk = $false
      Write-BridgeLog "WARNING: FK_DisConnect threw: $($_.Exception.Message)"
    }
    if (-not ($restoreOk -and $disconnectOk)) {
      Write-BridgeLog 'device restore/degradation warning — see WARNING lines above'
    }
  }
}

# ------------------------------------------------------------- HTTP server
$listener = New-Object System.Net.HttpListener
$listener.Prefixes.Add("http://127.0.0.1:$Port/")
try {
  $listener.Start()
} catch {
  Write-BridgeLog "FATAL: cannot listen on 127.0.0.1:$Port — the port is already in use?"
  Write-BridgeLog "detail: $($_.Exception.Message)"
  exit 3
}
$script:StopRequested = $false
Write-BridgeLog "HozorPlus FK623 bridge v$BridgeVersion (READ-ONLY, 32-bit)"
Write-BridgeLog "listening : http://127.0.0.1:$Port  (localhost only)"
Write-BridgeLog "DLL dir   : $DllDir (FK623Attend.dll)"
Write-BridgeLog "pid       : $PID"
Write-BridgeLog "probe extras (serial/time): $(if ($ProbeExtras) {'on (isolated child — failure tolerated)'} else {'off (default — off the critical path)'})"

try {
  while ($listener.IsListening -and -not $script:StopRequested) {
    $ctx = $listener.GetContext()
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    $req = $ctx.Request
    $path = $req.Url.AbsolutePath.TrimEnd('/')
    try {
      # /ping is the ONLY unauthenticated endpoint — a minimal liveness signal
      # for the supervisor's readiness probe (no paths, no dllDir, no version).
      if ($path -eq '/ping') {
        if ($req.HttpMethod -eq 'GET' -or $req.HttpMethod -eq 'POST') {
          Send-Json $ctx 200 ('{{"ok":true,"pid":{0}}}' -f $PID)
        } else {
          Send-Json $ctx 405 '{"ok":false,"error":"method not allowed","code":"METHOD"}'
        }
        continue
      }

      if (-not (Test-Token $ctx)) { continue }

      switch ($path) {
        '/health' {
          if ($req.HttpMethod -eq 'POST' -or $req.HttpMethod -eq 'GET') {
            $uptime = [int]((Get-Date) - $script:StartedAt).TotalSeconds
            $body = '{{"ok":true,"version":"{0}","dllDir":{1},"uptimeSec":{2},"pid":{3},"probeExtras":{4}}}' -f `
              $BridgeVersion, ($DllDir | ConvertTo-Json -Compress), $uptime, $PID, ($(if ($ProbeExtras) {'true'} else {'false'}))
            Send-Json $ctx 200 $body
          } else {
            Send-Json $ctx 405 '{"ok":false,"error":"method not allowed","code":"METHOD"}'
          }
        }
        '/shutdown' {
          if ($req.HttpMethod -ne 'POST') { Send-Json $ctx 405 '{"ok":false,"error":"method not allowed","code":"METHOD"}'; continue }
          Send-Json $ctx 200 '{"ok":true}'
          Write-BridgeLog 'shutdown requested — stopping listener'
          $script:StopRequested = $true
        }
        '/device/test' {
          if ($req.HttpMethod -ne 'POST') { Send-Json $ctx 405 '{"ok":false,"error":"method not allowed","code":"METHOD"}'; continue }
          $b = Read-RequestBody $ctx
          $r = Invoke-DeviceTest $b
          if ($r.ok) {
            $out = '{{"ok":true,"serialNumber":{0},"deviceTime":{1},"latencyMs":{2},"meta":{3}}}' -f `
              (ConvertTo-JValue $r.serialNumber), (ConvertTo-JValue $r.deviceTime), $r.latencyMs, ($r.meta | ConvertTo-Json -Compress)
            Send-Json $ctx 200 $out
          } else {
            Send-Json $ctx 200 ('{{"ok":false,"error":{0},"code":"{1}"}' -f ($r.error | ConvertTo-Json -Compress), $r.code)
          }
          Write-BridgeLog ('test ip={0} port={1} ok={2} {3}ms' -f (Get-Str $b 'ip' '?'), (Get-Num $b 'port' 0), $r.ok, [int]$sw.ElapsedMilliseconds)
        }
        '/device/read-logs' {
          if ($req.HttpMethod -ne 'POST') { Send-Json $ctx 405 '{"ok":false,"error":"method not allowed","code":"METHOD"}'; continue }
          $b = Read-RequestBody $ctx
          $r = Invoke-DeviceReadLogs $b
          if (-not $r.ok) {
            Send-Json $ctx 200 ('{{"ok":false,"error":{0},"code":"{1}"}' -f ($r.error | ConvertTo-Json -Compress), $r.code)
            Write-BridgeLog ('read-logs FAILED ip={0} port={1}: {2}' -f (Get-Str $b 'ip' '?'), (Get-Num $b 'port' 0), $r.code)
            continue
          }
          Send-Json $ctx 200 $r.json
          Write-BridgeLog ('read-logs ip={0} port={1} records={2} endCode={3} {4}ms' -f (Get-Str $b 'ip' '?'), (Get-Num $b 'port' 0), $r.recordCount, $r.endCode, [int]$sw.ElapsedMilliseconds)
        }
        default {
          Send-Json $ctx 404 '{"ok":false,"error":"مسیر یافت نشد","code":"NOT_FOUND"}'
        }
      }
    } catch {
      try {
        Send-Json $ctx 200 ('{{"ok":false,"error":{0},"code":"BRIDGE_ERROR"}' -f ($_.Exception.Message | ConvertTo-Json -Compress))
      } catch {}
      Write-BridgeLog ('request error: ' + $_.Exception.Message)
    }
  }
} finally {
  try { $listener.Stop() } catch {}
  Pop-Location
  Write-BridgeLog 'stopped'
}
exit 0

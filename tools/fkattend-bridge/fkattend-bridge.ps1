<#
.SYNOPSIS
  HozorPlus FKAttend x86 Bridge — READ-ONLY device access for NP756+ (port 4238).

.DESCRIPTION
  Runs a tiny JSON-over-HTTP server ON THIS MACHINE ONLY (127.0.0.1) that loads
  the 32-bit FKAttend.dll with the EXACT method already recorded as successful
  against the real NP756+ (32-bit PowerShell + FK_ConnectNet on port 4238):

      FK_ConnectNet(machine, ip, port, timeoutMs, protocolType=0, password, license)
      FK_EnableDevice(handle, 0)            -- keypad lock while reading
      FK_LoadGeneralLogData(handle)         -- load log buffer
      FK_GetGeneralLogData(handle, refs...) -- loop; ret 1 = record, -7 = end
      FK_EnableDevice(handle, 1)            -- ALWAYS restored in finally
      FK_Disconnect(handle)

  Guarantees (HozorPlus addendum + architecture decision D2):
    * READ-ONLY: no destructive SDK call is EVER made; there is no endpoint
      that could empty/delete logs or change the device clock.
    * Keypad re-enable + disconnect happen in `finally` — a crash mid-read
      cannot leave the device locked.
    * Serial/time probes run in a CHILD PowerShell process (isolation):
      their P/Invoke signatures are NOT part of the recorded successful test,
      so a mismatch can only crash the child, never the bridge.
    * Requests are serialized (single HTTP loop) → one device operation at a
      time; the device handle is never shared between requests.
    * The network password arrives per request, is used for the single SDK
      connect call and is NEVER echoed, logged or written to disk.
    * Binds to 127.0.0.1 only. Token required on every request.

.PREREQUISITES
  * 32-bit PowerShell: %WINDIR%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe
    (START-BRIDGE.bat launches exactly that one)
  * FKAttend.dll (and its sibling DLLs) in -DllDir (default: folder containing
    this script; on the Windows machine usually the NP756-SDK-Analysis folder)

.PARAMETER Port
  Local HTTP port (default 8765). Localhost only.

.PARAMETER Token
  Shared secret. When omitted a GUID is generated and printed ONCE — paste it
  into the device settings ("توکن Bridge") in HozorPlus.

.PARAMETER DllDir
  Directory containing FKAttend.dll. Also prepended to PATH for dependencies.

.PARAMETER ProbeExtras
  $true (default) → /device/test also reads serial + device time through the
  isolated child probe. Set 0 to skip if the child probe crashes on your DLLs.

.EXAMPLE
  powershell -NoProfile -ExecutionPolicy Bypass -File fkattend-bridge.ps1
  powershell ... -File fkattend-bridge.ps1 -Port 8765 -Token abc123 -DllDir "C:\Users\Administrator\Desktop\NP756-SDK-Analysis"
#>
param(
  [int]$Port = 8765,
  [string]$Token = '',
  [string]$DllDir = '',
  [bool]$ProbeExtras = $true
)

$ErrorActionPreference = 'Stop'
$script:BridgeVersion = '1.4.0'
$script:Root = $PSScriptRoot

# ---------------------------------------------------------------- environment
if ([Environment]::Is64BitProcess) {
  Write-Host '[fk-bridge] FATAL: this bridge is 32-bit only (x86 FKAttend.dll).'
  Write-Host '[fk-bridge] Run it via START-BRIDGE.bat or:'
  Write-Host '[fk-bridge]   %WINDIR%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe -NoProfile -ExecutionPolicy Bypass -File fkattend-bridge.ps1'
  exit 1
}
if (-not $DllDir) { $DllDir = $script:Root }
$DllDir = [System.IO.Path]::GetFullPath($DllDir)
if (-not (Test-Path (Join-Path $DllDir 'FKAttend.dll'))) {
  Write-Host "[fk-bridge] FATAL: FKAttend.dll not found in: $DllDir"
  Write-Host '[fk-bridge] Pass -DllDir <folder containing FKAttend.dll> (NP756-SDK-Analysis folder).'
  exit 1
}
if (-not $Token) {
  $Token = [guid]::NewGuid().ToString('N')
  Write-Host ''
  Write-Host '==============================================================='
  Write-Host '  BRIDGE TOKEN (paste into HozorPlus device settings):'
  Write-Host "  $Token"
  Write-Host '==============================================================='
  Write-Host ''
}

# The DLL loader searches the process working directory → run from the SDK dir
Push-Location $DllDir
$env:PATH = "$DllDir;$env:PATH"

Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;
// P/Invoke signatures for the calls RECORDED as successful against the real
// NP756+ (32-bit FKAttend.dll). Deliberately minimal and read-only.
public static class FKApi {
  [DllImport("FKAttend.dll", CharSet = CharSet.Ansi)]
  public static extern int FK_ConnectNet(int machine, string ip, int port, int timeoutMs, int protocolType, int password, int license);

  [DllImport("FKAttend.dll", CharSet = CharSet.Ansi)]
  public static extern int FK_Disconnect(int handle);

  [DllImport("FKAttend.dll", CharSet = CharSet.Ansi)]
  public static extern int FK_EnableDevice(int handle, int enable);

  [DllImport("FKAttend.dll", CharSet = CharSet.Ansi)]
  public static extern int FK_LoadGeneralLogData(int handle);

  // the 10 out-params exactly as in the vendor sample (frmLog.cs):
  // enroll, verify, inout, Y, M, D, H, Mi, S, workcode → 1 = record, -7 = end
  [DllImport("FKAttend.dll", CharSet = CharSet.Ansi)]
  public static extern int FK_GetGeneralLogData(int handle, ref int enroll, ref int verify, ref int inout,
                                                ref int y, ref int mo, ref int d, ref int h, ref int mi, ref int s, ref int wc);
}
"@ -ErrorAction Stop

$script:StartedAt = Get-Date

function Write-BridgeLog([string]$line) {
  # console-only logging — request bodies (with the password) are NEVER logged
  Write-Host "[fk-bridge] $line"
}

function ConvertTo-JValue([object]$v) {
  if ($null -eq $v) { return 'null' }
  return ($v | ConvertTo-Json -Compress)
}

# ------------------------------------------------------------- child probes
# FK_GetSerialNumber / FK_GetDeviceTime are NOT part of the recorded successful
# call set → run them in an isolated child process so a signature mismatch can
# only kill the child. The probe reads its JSON config from STDIN (never the
# command line → the password never appears in any process list).
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
      Write-BridgeLog "extras probe failed (exit $($p.ExitCode)) — serial/time unavailable; set -ProbeExtras 0 to silence"
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
    Send-Json $c 401 '{"ok":false,"error":"توکن Bridge نامعتبر است","code":"AUTH"}'
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
  # READ-ONLY: connect → (probe serial/time in a CHILD process) → disconnect.
  # No EnableDevice(0) → the keypad is never locked by a connection test.
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
      # echo of the non-secret request params — used by config-propagation tests
      meta = @{ machine = $machine; ip = $ip; port = $port; timeoutMs = $timeoutMs; license = $license }
    }
  } finally {
    [void][FKApi]::FK_Disconnect($handle)
  }
}

function Invoke-DeviceReadLogs([object]$b) {
  # Full read with the recorded successful sequence. Keypad re-enable and
  # disconnect are in `finally` — no path leaves the device locked.
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
  try {
    # vendor pattern: lock the keypad while reading so data cannot change mid-read
    [void][FKApi]::FK_EnableDevice($handle, 0)

    $load = [FKApi]::FK_LoadGeneralLogData($handle)
    if ($load -ne 1 -and $load -ne 0) {
      return @{ ok = $false; error = "بارگذاری حافظهٔ ترددها ناموفق بود (کد $load از FK_LoadGeneralLogData)"; code = 'LOAD_FAILED' }
    }

    # records are hand-serialized for speed on large histories (2944+ rows);
    # the password is never included anywhere in the output
    $sb = New-Object System.Text.StringBuilder
    [void]$sb.Append('[')
    $count = 0
    $enroll = 0; $verify = 0; $inout = 0; $y = 0; $mo = 0; $d = 0; $h = 0; $mi = 0; $sec = 0; $wc = 0
    $ret = [FKApi]::FK_GetGeneralLogData($handle, [ref]$enroll, [ref]$verify, [ref]$inout, [ref]$y, [ref]$mo, [ref]$d, [ref]$h, [ref]$mi, [ref]$sec, [ref]$wc)
    while ($ret -eq 1) {
      if ($count -gt 0) { [void]$sb.Append(',') }
      [void]$sb.Append(('{{"enrollNumber":{0},"verifyMode":{1},"inOutMode":{2},"workCode":{3},"timestamp":"{4:d4}-{5:d2}-{6:d2} {7:d2}:{8:d2}:{9:d2}"}}' -f `
        $enroll, $verify, $inout, $wc, $y, $mo, $d, $h, $mi, $sec))
      $count++
      if ($count -ge 200000) { break }   # safety valve
      $ret = [FKApi]::FK_GetGeneralLogData($handle, [ref]$enroll, [ref]$verify, [ref]$inout, [ref]$y, [ref]$mo, [ref]$d, [ref]$h, [ref]$mi, [ref]$sec, [ref]$wc)
    }
    [void]$sb.Append(']')
    # -7 = normal end of reading (as observed on the real device); 0 = empty/end
    $endCode = $ret

    $extras = Invoke-ExtrasProbe $b
    $serial = $null
    $deviceTime = $null
    if ($extras) {
      $serial = $extras.serialNumber
      $deviceTime = $extras.deviceTime
    }

    $metaJson = '{{"machine":{0},"ip":{1},"port":{2},"timeoutMs":{3},"license":{4}}}' -f `
      $machine, ($ip | ConvertTo-Json -Compress), $port, $timeoutMs, $license

    $head = '{{"ok":true,"serialNumber":{0},"deviceTime":{1},"latencyMs":{2},"connectMs":{3},"endCode":{4},"records":' -f `
      (ConvertTo-JValue $serial), (ConvertTo-JValue $deviceTime), [int]$swTotal.ElapsedMilliseconds, $connectMs, $endCode

    return @{
      ok = $true
      json = $head + $sb.ToString() + ',"meta":' + $metaJson + '}'
      recordCount = $count
      endCode = $endCode
    }
  } finally {
    # ALWAYS restore the keypad and disconnect — even on errors above
    [void][FKApi]::FK_EnableDevice($handle, 1)
    [void][FKApi]::FK_Disconnect($handle)
  }
}

# ------------------------------------------------------------- HTTP server
$listener = New-Object System.Net.HttpListener
$listener.Prefixes.Add("http://127.0.0.1:$Port/")
$listener.Start()
Write-Host ''
Write-BridgeLog "HozorPlus FKAttend bridge v$BridgeVersion (READ-ONLY, 32-bit)"
Write-BridgeLog "listening : http://127.0.0.1:$Port  (localhost only)"
Write-BridgeLog "DLL dir   : $DllDir"
Write-BridgeLog "probe extras (serial/time): $(if ($ProbeExtras) {'on (isolated child — failure tolerated')} else {'off'})"
Write-Host ''
Write-BridgeLog 'ready — keep this window OPEN; Ctrl+C stops the bridge.'
Write-Host ''

try {
  while ($listener.IsListening) {
    $ctx = $listener.GetContext()
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    $req = $ctx.Request
    $path = $req.Url.AbsolutePath.TrimEnd('/')
    try {
      if ($path -ne '/health' -and -not (Test-Token $ctx)) { continue }

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

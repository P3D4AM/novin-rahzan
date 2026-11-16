<#
.SYNOPSIS
  Isolated FKAttend probe — reads serial number + device time (optional extras).

.DESCRIPTION
  Runs as a CHILD of fkattend-bridge.ps1 (always 32-bit). FK_GetSerialNumber
  and FK_GetDeviceTime are NOT part of the call set recorded as successful on
  the real NP756+, so this probe is crash-isolated: if a signature does not
  match the actual DLL, ONLY this child dies and the bridge keeps serving.

  Reads its JSON config from STDIN (never the command line → the network
  password is never visible in any process list):
    {"machine":1,"ip":"192.168.1.75","port":4238,"timeoutMs":5000,"password":123,"license":1261}

  Prints one JSON line to stdout:
    {"serialNumber":"7561202505176","deviceTime":"2026-11-16 10:12:30"}

  Exit code non-zero (or empty stdout) → extras unavailable; caller treats it
  as "no serial" and continues.
#>
param()
$ErrorActionPreference = 'Stop'
$DllDir = (Get-Location).Path

Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;
// Unverified-signature extras — isolated on purpose (see .DESCRIPTION).
public static class FKProbe {
  [DllImport("FKAttend.dll", CharSet = CharSet.Ansi)]
  public static extern int FK_ConnectNet(int machine, string ip, int port, int timeoutMs, int protocolType, int password, int license);

  [DllImport("FKAttend.dll", CharSet = CharSet.Ansi)]
  public static extern int FK_Disconnect(int handle);

  [DllImport("FKAttend.dll", CharSet = CharSet.Ansi)]
  public static extern int FK_EnableDevice(int handle, int enable);

  [DllImport("FKAttend.dll", CharSet = CharSet.Ansi)]
  public static extern int FK_GetSerialNumber(int handle, ref long serial);

  [DllImport("FKAttend.dll", CharSet = CharSet.Ansi)]
  public static extern int FK_GetDeviceTime(int handle, ref int y, ref int mo, ref int d, ref int h, ref int mi, ref int s);
}
"@

$raw = [Console]::In.ReadToEnd()
$cfg = $raw | ConvertFrom-Json

$machine = [int]$cfg.machine
$ip = [string]$cfg.ip
$port = [int]$cfg.port
$timeoutMs = [int]$cfg.timeoutMs
$password = [int]$cfg.password
$license = [int]$cfg.license

$handle = [FKProbe]::FK_ConnectNet($machine, $ip, $port, $timeoutMs, 0, $password, $license)
if ($handle -le 0) { exit 2 }
try {
  [void][FKProbe]::FK_EnableDevice($handle, 0)

  $serial = 0L
  $ret = [FKProbe]::FK_GetSerialNumber($handle, [ref]$serial)
  if ($ret -ne 1 -or $serial -le 0) { $serial = 0L }

  $y = 0; $mo = 0; $d = 0; $h = 0; $mi = 0; $s = 0
  $ret2 = [FKProbe]::FK_GetDeviceTime($handle, [ref]$y, [ref]$mo, [ref]$d, [ref]$h, [ref]$mi, [ref]$s)

  $serialJson = 'null'
  if ($serial -gt 0) { $serialJson = '"{0}"' -f $serial }
  $timeJson = 'null'
  if ($ret2 -eq 1 -and $y -ge 2000) {
    $timeJson = '"{0:d4}-{1:d2}-{2:d2} {3:d2}:{4:d2}:{5:d2}"' -f $y, $mo, $d, $h, $mi, $s
  }
  Write-Output ('{{"serialNumber":{0},"deviceTime":{1}}}' -f $serialJson, $timeJson)
} finally {
  [void][FKProbe]::FK_EnableDevice($handle, 1)
  [void][FKProbe]::FK_Disconnect($handle)
}

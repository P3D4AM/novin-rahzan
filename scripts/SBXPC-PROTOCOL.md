# SBXPC Binary Wire Protocol — Novin Pardaz NP-series (NP756+, NP746, NP770, …)

Reverse-engineered from the vendor's `SBXPCDLL64.dll` + `SBPCCOMM64.dll` (NovinHozor software).
The NP devices LISTEN on TCP port 5005 (default) and speak this binary protocol.
The same devices ALSO push real-time XML ("Ebio" push) to the server — both channels coexist.

## Frame formats

### PC → Device COMMAND (16 bytes)
```
55 AA | machineId(2 LE) | 79 19 | cmd(2 LE) | arg2(4 LE) | arg1(2 LE) | cksum(2 LE)
cksum = (Σ bytes[0..13]) & 0xFFFF
```
(0x1979 = company mark, default)

### Device → PC ACK (8 bytes) — sent after each command
```
5A A5 | machineId(2) | ?(1) | result(2)==1 | cksum(2)
cksum = (buf[2]+buf[3]+buf[4]+buf[5]+0xFF) & 0xFFFF     ; result==1 → ACK-OK
```

### Device → PC RESULT (14 bytes) — RecExeResultX
```
AA 55 | machineId(2) | ?(2)@4 | status(2)@6 | retCode(4)@8 | cksum(2)@12
cksum = (Σ bytes[0..11]) & 0xFFFF     ; status!=0 required; retCode carries count/etc.
```

### PC → Device DATA — SendDataX (max payload 0x3FC=1020)
```
A5 5A | machineId(2) | data(len) | cksum(2)
cksum = (Σ bytes[0..len+3]) & 0xFFFF
```

### Device → PC DATA — RecDataX
```
A5 5A | machineId(2) | data(len) | cksum(2)      (frame = len+6 bytes)
```

### Device → PC BIG DATA — RecBigDataX (chunked, ≤1020 payload per chunk)
```
5A A5 | machineId(2) | data(chunk) | cksum(2)
cksum = (Σ bytes[0..chunk+3]) & 0xFFFF
```

## Commands (classic/default machine-type path — used by NP devices)

| cmd  | meaning | args |
|------|---------|------|
| 0x101 | GetEnrollData | arg1: enroll# + backup encoding |
| 0x102 | SetEnrollData | |
| 0x103 | SetEnrollData1 / DeleteEnrollData variant | |
| 0x104 | DeleteEnrollData | |
| 0x105 | ReadAllSLogData (admin logs) | |
| 0x106 | ReadGeneralLogData (new logs) | |
| **0x107** | **ReadAllGLogData** | arg1=0 → count; arg1=1 → data |
| 0x108 | GetDeviceStatus | arg1 = status index (1..13) |
| 0x109 | GetDeviceInfo | arg1 = info index (1..24) |
| 0x10a | SetDeviceInfo | |
| 0x10b | EnableDevice(FALSE) | |
| 0x10c | EnableDevice(TRUE) | |
| 0x10e | GetDeviceTime | arg2=4 |
| 0x10f | SetDeviceTime | arg2=4, arg2 payload=packed time |
| 0x110/0x111 | (unknown — device options) | |
| 0x113 | GetSerialNumber | → RecDataX(32) ASCII serial |
| 0x118 | EmptyGeneralLogData | |
| 0x119 | EmptySuperLogData | |
| 0x127 | GetUserName | |
| 0x128 | SetUserName | |
| 0x200 | GeneralOperationXML (XML-capable firmwares) | SendBigDataX |

Timeout used by vendor: 5000 ms (0x1388).

## ReadAllGLogData flow (exactly as the vendor does it)

```
SetMachineIDX(machineId)
SendCommandX(0x107, arg1=0, arg2=0)          → ACK
RecExeResultX() → status@6, count@8           ; count = number of log records
if count == 0 → done
SendCommandX(0x107, arg1=1, arg2=0)          → ACK
SendDataX(uint32 LE = 1)                      ; 4-byte value 1
RecExeResultX() → count (again)
RecBigDataX(count * 8 bytes)                  ; chunked 5A A5 frames
SendDataX(uint32 LE = count)                  ; echo count back
RecExeResultX() → retCode == 0x3E8 (1000) = end-of-data OK
```

## Log record (8 bytes, little-endian)

```
[0..3] uint32  seconds since 2000-01-01 00:00:00 (device local time)
[4..5] uint16  enroll number (user id on device)
[6]    uint8   verify-mode byte (low nibble = mode index → table)
[7]    uint8   extra (new-format: seconds nibbles)
```

Verify-mode tables (16 entries, indexed by byte[6] — old fmt — or byte[6]&0xF — new fmt):
- old: [4,1,2,3,10,11,12,13,14,20,21,15,23,24,25,26]
- new: [23,1,2,3,10,11,12,13,14,20,21,4,5,6,7,22]

New-format second-level precision: seconds = (byte[7] << 12) | ((byte[6] & 0xF0) << 4) | NEW[byte[6] & 0xF]

## GetSerialNumber flow

```
SendCommandX(0x113, arg1=0, arg2=0) → ACK
RecExeResultX() → status != 0
RecDataX(32 bytes) → ASCII serial number string
```

## GetDeviceTime flow
```
SendCommandX(0x10e, arg1=0, arg2=4) → ACK
RecDataX(4 bytes) → packed datetime (vendor decodes via its date decoder)
```

## GetDeviceStatus / GetDeviceInfo flow
```
SendCommandX(0x108/0x109, arg1=index, arg2=0) → ACK
RecExeResultX() → status, value
```

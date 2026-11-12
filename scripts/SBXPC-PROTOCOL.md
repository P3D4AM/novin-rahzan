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

---

## Dialects discovered in the wild (v1.2.1)

Real NP756+ units do not all speak the classic dialect above. The SBPCCOMM64.dll
actually contains a SECOND frame family — the `_EXT` exports
(SendCommandX_EXT / RecvAckX_EXT / SendAckX_EXT / RecvDataX_EXT / SendDataX_EXT) —
and some devices answer classic 16-byte commands with a 12-byte EXT-style error ack.

### EXT command (PC → DEV, 12 bytes)
```
55 AA | machineId(2) | cmd(4 LE) | arg(2)=0 | cksum(2)
cksum = (machineId_lo + machineId_hi + cmd[4 bytes] + arg[2 bytes] + 0xFF) & 0xFFFF
      = (Σ bytes[2..9] + 0xFF) & 0xFFFF
```

### EXT ack (DEV → PC, 12 bytes)
```
AA 55 | machineId(2) | dword(4) | cksum(2)
cksum = (Σ bytes[0..9]) & 0xFFFF
dword = result code — 1 = OK; 0x10 (16) = "command not accepted/garbled"
```

### EXT ack (PC → DEV — acknowledge device data, 12 bytes)
```
AA 55 | machineId(2) | cmd(4) | arg(2)=0 | cksum(2)
cksum = (Σ bytes[2..9] + 0xFF) & 0xFFFF
```

### Observed on a real NP756+ (port 4238) — the EXT_SECURE dialect (v1.2.2)
Sending the classic 16-byte GetSerialNumber command made the device answer:

```
aa 55 01 00 10 00 00 00 a6 01     (10 bytes!)
```

The last two bytes are the CHECKSUM OF OUR OWN (correctly received) frame —
the device received the classic frame intact and deliberately REFUSED it.
A plaintext 12-byte EXT command got NO answer at all. This is the fingerprint
of the password-encrypted EXT dialect ("رمز شبکه") implemented by
SBPCCOMM64.dll v3.0.6 (2015), functions SendCommandX_EXT / RecvAckX_EXT /
SendDataX_EXT / RecvDataX_EXT:

* every 12-byte EXT frame (command, ack) is XOR-encrypted with the repeating
  4-byte key  **key = LE32(password + salt)**  — i.e. frame[j] ^= key[j % 4],
  with the key phase restarting at byte 0 of EVERY frame;
* **the salt is 0 by default** (v1.2.3 correction): the DLL's salt slot
  (`ChangeCompanyMark_EXT` write target — 0x2BF10 in SBPCCOMM64.dll /
  0x100252C0 in SBPCCOMM.dll) is zero-initialized BSS, and nothing in the
  vendor stack ever calls `ChangeCompanyMark_EXT`. The v1.2.2 constant
  0x25B84 was a misread of file padding beyond .data's raw size; it is kept
  as a fallback key variant because some firmware generations may set a
  custom company mark. The client tries salt ∈ {0, 0x25B84};
* the "network password" is exactly the «رمز شبکه» in the device menu
  (ConnectTcpip's 4th argument; the vendor sample's UI default is 123456);
* data is exchanged as **[payload][trailer 12B]** where the trailer is
  `5A A5 | machineId(2) | Σpayload(4) | ?(2) | cksum(2)`, cksum = Σ trailer[2..9] + 0xFF,
  and payload + trailer are each encrypted separately (key phase restarts per part);
* an encrypted ACK carries dword=1 for OK.

Reverse-engineering evidence (SBPCCOMM64.dll):
* `StartTcpX(ip, port, password, …)` stores password at devObj+0x14;
* `0x4630` (encrypted send/recv) computes `key = [dev+0x14] + dword[0x2BF10]`
  (the BSS salt slot — 0 unless `ChangeCompanyMark_EXT` was called), then XORs
  the whole frame with key[j%4] before the raw socket send (and XORs it back
  afterwards);
* the vendor's TimeAttendance.exe is 32-bit and its OCX (SBXPCDLL.dll) imports
  ONLY the classic functions from SBPCCOMM.dll — the encrypted `_EXT` family is
  never used by the vendor's own app;
* the newest NP face devices ("VisibleLight" — NP756P=9308, NP761P=9309 per the
  vendor UI model codes) speak an **HTTP JSON API on port 14460**
  (`/getDeviceKey`, `/getLogCount`, `/getLogInfo`, `/getDeviceTime`,
  `/setPassword`, … with `{pass: N}` bodies) — see scripts/np-probe.js.
* COM/USB links use key = 0x25B84 alone (no password); TCP/UDP/P2P add the password;
* `0x1d00` builds the EXT command `55 AA | mid | cmd(4) | arg(2)=0 | cksum`
  (cksum = Σ[2..9] + 0xFF — note 0x55+0xAA = 0xFF, so this equals Σ[0..9]);
* `0x1db0` builds the PC→DEV EXT ack the same way (header AA 55, also sums to 0xFF);
* `0x21f0/0x2320` implement the [payload][trailer] data exchange.

### Client strategy (src/lib/sbxpc.ts, v1.2.2)
The client PROBES on connect:
1. send classic 0x113 → drain → `5A A5 …` ⇒ CLASSIC dialect
2. `AA 55 …` + valid 14-byte result + data ⇒ RESULT_DIRECT dialect (no ACK)
3. `AA 55 …` + **10-byte refusal ack** (dword 0x10 + checksum echo) ⇒ EXT_SECURE:
   reconnect per candidate password («رمز شبکه» from the device profile, else
   common defaults 0, 123456, 1, 1234, 9999, 65535) and send the encrypted
   EXT 0x113 until a valid encrypted ack (dword=1) comes back
4. valid 12-byte plaintext EXT ack ⇒ reconnect and send EXT 0x113 ⇒ EXT dialect
5. fallback: encrypted EXT probing, then classic 0x10e, then report alive + raw hex

Discovered passwords are persisted on the device record (Device.password) and
reused by later syncs.

---

## v1.2.4 — the "VisibleLight" HTTP JSON API + final SBPCCOMM verification

### SBPCCOMM64.dll crypto dispatch — FINAL (disassembled 0x4630, per link type)

`0x4630(devObj, frame, len)` is the encrypt-send wrapper used ONLY by the `_EXT`
family (callers 0x1d80/0x1e33 = EXT command/ack, 0x22d4/0x22f1 = data blocks).
It switches on `[devObj+8]` (link type set by StartTcpX):

| link type | key | sender | meaning |
|-----------|-----|--------|---------|
| 1 | salt only | 0x2fb0 | COM (no password — company-mark XOR) |
| 2 | salt only | —      | USB |
| 3 | **password + salt** | — | **TCP** (StartTcpX writes 3) |
| 4 | password + salt | — | P2P/UDP-family |
| 5 | password + salt | 0x4370 | other |

* The salt slot (RVA 0x2BF10) is beyond `.data` raw size (0x1c00) → **BSS zero**;
  nothing but `ChangeCompanyMark_EXT` ever writes it → TCP key = **LE32(password)**.
* The frame is XORed **fully** (frame[j] ^= key[j%4], phase 0 at frame start),
  sent encrypted, then XORed back in memory.
* **The CLASSIC path (0x10f0 → 0x4370 etc.) NEVER calls 0x4630 — classic frames
  are never encrypted.** The vendor's own app (32-bit TimeAttendance.exe →
  SBXPCDLL.dll OCX → SBPCCOMM.dll) imports ONLY the classic ordinals
  (1..13,15,28 — none of 21..27) → the vendor software CANNOT talk to a
  binary-password device at all. Its answer for password-protected NP face
  devices is the HTTP API below.

### The "VisibleLight" HTTP JSON API (PacketParserStandard.dll → VisibleLightLanSDK)

The newest NP face devices (NP756P=9308, NP761P=9309 in the vendor UI; the
family the vendor calls **"VisibleLight"**) run an HTTP JSON service on
**TCP port 14460**:

```
POST http://<ip>:14460/<endpoint>   Content-Type: application/json
body: {"pass": <رمز شبکه>}          → reply: {"result": bool, "data": "<string>"
```

endpoints (strings extracted from PacketParserStandard.dll):
/getDeviceKey  /getLogCount  /getLogInfo  /getDeviceTime  /setTime
/getDeviceInfo /getConfig    /setConfig   /getOEM          /getDepartment
/setDepartment /getTimeGroup /setTimeGroup /getTimePart    /setTimePart
/getUserPower  /openDoorControl /resetDevice /restartDevice /setPassword
/deleteAllLog  /deleteNewLog  /getLogCallBack /setLogCallBack  ...
request shapes: {"pass":N}, {"pass":N,"serverTime":..}, {"pass":N,"index":..},
{"pass":N,"flag":..,"length":..,"index":..,"isphoto":..},
{"pass":N,"logCallBackURL":..,"readCard":..}, {"oldPass":..,"newPass":..}

`/getLogCount` reply → data = JSON string like {"count":N}.
`IsConnectAsync` simply calls `/getDeviceKey` — the vendor's connectivity test.
The log record the vendor parses (`DeviceLogOnPerson`) carries: PersonNo,
DateTimeLogM, VerifyMode, InOut, IoMode, DeviceNo, DeviceSerial, DeviceIp,
DevicePassword, IdTraffic, LogPhoto, TerminalType — the /getLogInfo payload
schema (field casing to confirm against a live device).

The binary port of these devices is a **legacy stub**: it answers a correctly
checksummed classic 16B frame with the 10-byte refusal ack (dword 0x10 + cksum
echo) and silently drops everything else — EXACTLY the fingerprint observed on
the user's real NP756+ (classic → refusal ack; plaintext EXT → silence;
encrypted EXT with LE32(12345) and LE32(12345+0x25B84) → silence).

### Client strategy (src/lib/sbxpc.ts, v1.2.4)

after the classic refusal ack:
1. try EXT_SECURE (salts {0, 0x25B84} × password/candidates) — as before;
2. **HTTP_JSON probe: POST /getDeviceKey {"pass":pw} on :14460** (number and
   string pass variants), then /getDeviceTime + /getLogCount → dialect HTTP_JSON
   with serial/time/logCount (npTest reports them; sync = v1.2.5 once the
   /getLogInfo record schema is confirmed on a live device);
3. **CLASSIC_SECURE hypothesis probe**: whole classic exchange XOR-encrypted
   (ACK 8B/RESULT 14B/DATA frames decrypted per frame — key phase restarts on
   every frame);
4. if the binary TCP connect itself fails, npTest falls back to the HTTP probe
   before reporting failure.

### scripts/np-probe.js — standalone diagnostic (v1.2.4)

`node np-probe.js <ip> <port> <password>` — tries the HTTP API (:14460),
~16 binary variants (classic/EXT × plaintext/encrypted × key formulas
LE32(pw), LE32(pw+0x25B84), ASCII, salt-only × full/partial coverage) and UDP,
prints every raw exchange plus a verdict table. This is the tool to run against
an unrecognized device; the hex output pins down the exact dialect.

---

## v1.2.5 — THE LOGIN HANDSHAKE (cmd 0x52): the real "how the vendor talks to NP756+"

The piece that was missing since v1.2.2: **StartTcpX performs a LOGIN right after
the TCP connect.** Disassembly chain (SBPCCOMM64.dll):

* StartTcpX (0x9740) homed args: [rbp+0x5f]=ip, [rbp+0x67]=port,
  [rbp+0x6f]=password(r8d→devObj+0x14), **[rbp+0x77]=arg4 (r9d) = login flag**,
  [rbp+0x7f]=arg5. The SBXPCDLL64 OCX calls it with **r9d=1** (flag on).
* After the socket connects it calls **devObj->vtable[2]** with the password
  (vtable at RVA 0x248D0 → entry 2 = RVA 0x23f0). `test flag: 0 → EXT login
  branch (encrypted EXT 0x52 + password data block); ≠0 → CLASSIC login branch`.
  The vendor passes flag=1 → **classic branch**.
* Classic login (branch A):
  `SendCommandX(cmd=0x52, arg1=0, arg2=PASSWORD)` — the normal classic 16-byte
  builder (0x10f0 → TCP sender 0x3c40 — **PLAINTEXT, no crypto**):
  ```
  55 AA | mid | 79 19 | 52 00 | PASSWORD(4B LE) | 00 00 | cksum(Σ[0..13])
  ```
* The expected reply is read by 0x1380 = the classic RESULT receiver
  (14B `AA 55|mid|?|status|ret|cksum`, cksum=Σ[0..11]): **status (word @+6) ≠ 0
  = login OK; status=0 = wrong password** (→ the OCX's ConnectTcpip returns
  3 "Password Mismatched!").
* Branch B (flag=0, unused by the vendor): `SendCommandX_EXT(0x52)` → ack(1) →
  `SendDataX_EXT(password 4B)` → ack(1) — encrypted on TCP via 0x4630.

**THIS is the semantics of the observed 10-byte refusal ack** (dword 0x10):
the device received a correctly checksummed frame but **nobody logged in yet**.
Every NP756+ behaviour seen in the wild is now explained:

| what we sent | device's answer | why |
|---|---|---|
| classic 0x113 (no login) | `AA 55|mid|0x10|cksum-echo` | login first! |
| plaintext/encrypted EXT | silence | not the expected protocol |
| encrypted classic | silence | garbage |
| **classic 0x52 + password, then classic commands** | RESULT status=1, then normal CLASSIC replies | **the vendor path** |

Client strategy (src/lib/sbxpc.ts v1.2.5): after the 10-byte refusal the client
runs `tryClassicLogin()` (configured «رمز شبکه» or common defaults, fresh
connection per candidate), and on success retries the classic 0x113 on the SAME
logged-in connection → CLASSIC/RESULT_DIRECT dialect → serial + clock + full
ReadAllGLogData sync. The login session lives on the socket — resetConnection
re-logins automatically (ensureLogin). np-probe.js runs the same battery
(L1…L5) against the real device.

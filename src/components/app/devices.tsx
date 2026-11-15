'use client'

import { useEffect, useState } from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from '@/components/ui/dialog'
import { Label } from '@/components/ui/label'
import { Switch } from '@/components/ui/switch'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { SessionUser, Bootstrap, api, DeviceT } from './types'
import { faDigits, faTime } from './jalali-ui'
import { useToast } from '@/hooks/use-toast'
import { Plus, Trash2, Pencil, Loader2, Plug, RefreshCw, Users, Upload, Fingerprint, Wifi, WifiOff, RadioTower, Activity, Download } from 'lucide-react'

interface Props { user: SessionUser; boot: Bootstrap; reloadBoot: () => void }

interface GatewayStatus {
  running: boolean
  port: number
  connections: number
  startedAt: string | null
  lastEventAt: string | null
  lastError: string | null
  events: number
  punches: number
  lastUnknownSerial: string | null
}

export function DevicesView({ user, boot, reloadBoot }: Props) {
  const [open, setOpen] = useState(false)
  const [editing, setEditing] = useState<DeviceT | null>(null)
  const [busy, setBusy] = useState<string | null>(null)
  const [logs, setLogs] = useState<Array<{ id: string; level: string; message: string; createdAt: string }>>([])
  const [testResult, setTestResult] = useState<Record<string, unknown> | null>(null)
  const [gateway, setGateway] = useState<GatewayStatus | null>(null)
  const [importOpen, setImportOpen] = useState(false)
  const [importMode, setImportMode] = useState('punches')
  const [importing, setImporting] = useState(false)
  const [syncDays, setSyncDays] = useState<Record<string, string>>({})
  const { toast } = useToast()

  const blank = { name: '', type: 'NOVIN_NP', protocolFamily: 'SBXPC', ipAddress: '', port: 5005, protocol: 'TCP', location: '', enabled: true, autoSync: false, syncIntervalMin: 5, serialNumber: '', password: '', connTimeoutMs: 5000, machineNumber: 1, sdkLicense: 1261, bridgeUrl: '', bridgeToken: '', rtEnabled: false, rtPollSec: 30 }
  const [form, setForm] = useState({ ...blank })

  const loadLogs = async () => {
    const res = await api<{ logs: typeof logs }>('/api/devices?logs=1')
    setLogs(res.logs)
  }
  const loadGateway = async () => {
    try {
      const res = await api<{ result: GatewayStatus }>('/api/devices', { method: 'PATCH', body: JSON.stringify({ action: 'gateway' }) })
      setGateway(res.result)
    } catch { /* ignore */ }
  }
  useEffect(() => { loadLogs(); loadGateway() }, [])

  const openNew = () => { setEditing(null); setForm({ ...blank }); setOpen(true) }
  const openEdit = (d: DeviceT) => {
    setEditing(d)
    // v1.3.0: the API never returns the password anymore — the edit form starts
    // empty and an empty field means "keep the stored secret" (PUT: no password key)
    setForm({
      name: d.name, type: d.type, protocolFamily: d.protocolFamily ?? 'SBXPC',
      ipAddress: d.ipAddress ?? '', port: d.port, protocol: d.protocol, location: d.location ?? '',
      enabled: d.enabled, autoSync: (d.syncIntervalMin ?? 0) > 0, syncIntervalMin: d.syncIntervalMin ?? 5,
      serialNumber: d.serialNumber ?? '', password: '',
      connTimeoutMs: d.connTimeoutMs ?? 5000, machineNumber: d.machineNumber ?? 1, sdkLicense: d.sdkLicense ?? 1261,
      bridgeUrl: d.bridgeUrl ?? '', bridgeToken: '',
      rtEnabled: d.rtEnabled ?? false, rtPollSec: d.rtPollSec ?? 30,
    })
    setOpen(true)
  }

  const save = async () => {
    if (!form.name.trim()) { toast({ title: 'نام دستگاه الزامی است', variant: 'destructive' }); return }
    if (form.type === 'NOVIN_NP' && !form.serialNumber.trim()) {
      toast({ title: 'شماره سریال الزامی است', description: 'پیام‌های دستگاه نوین پرداز با شماره سریال مطابقت داده می‌شوند (مثل نرم‌افزار اصلی).', variant: 'destructive' })
      return
    }
    try {
      // v1.3.0: send a password only when the admin typed one — empty field keeps
      // the stored secret on edit (PUT: undefined) and means "none" on create.
      // v1.4.0: same rule for the bridge token; autoSync checkbox maps to the interval.
      const payload: Record<string, unknown> = { ...form }
      delete payload.autoSync
      payload.syncIntervalMin = form.autoSync ? form.syncIntervalMin : 0
      if (String(form.password).trim() === '') delete payload.password
      if (String(form.bridgeToken ?? '').trim() === '') delete payload.bridgeToken
      if (editing) await api('/api/devices', { method: 'PUT', body: JSON.stringify({ id: editing.id, ...payload }) })
      else await api('/api/devices', { method: 'POST', body: JSON.stringify(payload) })
      setOpen(false)
      reloadBoot()
      loadLogs()
      toast({ title: 'ذخیره شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    }
  }

  const action = async (id: string, act: 'test' | 'sync' | 'users', daysBack?: number | null) => {
    setBusy(`${id}:${act}`)
    setTestResult(null)
    try {
      const res = await api<{ result: Record<string, unknown> }>('/api/devices', {
        method: 'PATCH',
        body: JSON.stringify(daysBack !== undefined ? { id, action: act, daysBack } : { id, action: act }),
      })
      setTestResult(res.result)
      reloadBoot()
      loadLogs()
      if (act === 'sync') {
        const r = res.result as { created?: number; skipped?: number; unmatched?: number; total?: number }
        toast({ title: 'دریافت از دستگاه انجام شد', description: `${faDigits(r.created ?? 0)} تردد جدید از ${faDigits(r.total ?? 0)} تردد حافظه دستگاه · ${faDigits(r.skipped ?? 0)} تکراری` })
      }
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setBusy(null) }
  }

  const doImport = async (file: File) => {
    setImporting(true)
    try {
      const fd = new FormData()
      fd.append('file', file)
      fd.append('mode', importMode)
      const res = await fetch('/api/import', { method: 'POST', body: fd })
      const data = await res.json()
      if (!res.ok) throw new Error(data.error || 'خطا')
      toast({
        title: 'ورود داده انجام شد',
        description: importMode === 'punches'
          ? `${faDigits(data.created)} تردد ثبت، ${faDigits(data.skipped)} تکراری، ${faDigits(data.unmatched)} بدون تطبیق`
          : `${faDigits(data.created)} پرسنل اضافه شد`,
      })
      setImportOpen(false)
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setImporting(false) }
  }

  const LEVEL_COLOR: Record<string, string> = { SUCCESS: 'text-emerald-600', ERROR: 'text-red-600', WARN: 'text-amber-600', INFO: 'text-muted-foreground' }

  return (
    <Tabs defaultValue="devices" dir="rtl">
      <TabsList className="mb-4">
        <TabsTrigger value="devices">دستگاه‌ها</TabsTrigger>
        <TabsTrigger value="import">ورود داده از فایل</TabsTrigger>
        <TabsTrigger value="logs">تاریخچه عملیات</TabsTrigger>
      </TabsList>

      <TabsContent value="devices" className="space-y-4">
        <div className="flex flex-wrap gap-2">
          <Button onClick={openNew} className="gap-1.5"><Plus className="h-4 w-4" /> دستگاه جدید</Button>
          <Button variant="outline" onClick={() => setImportOpen(true)} className="gap-1.5"><Upload className="h-4 w-4" /> ورود سریع از فایل</Button>
          <Button variant="outline" onClick={loadGateway} className="gap-1.5"><Activity className="h-4 w-4" /> وضعیت گیت‌وی</Button>
        </div>

        {/* Novin Pardaz push gateway status */}
        <Card className={gateway?.running ? 'border-emerald-400' : 'border-amber-400'}>
          <CardHeader className="pb-2">
            <CardTitle className="text-base flex items-center gap-2">
              <RadioTower className="h-5 w-5 text-primary" />
              گیت‌وی اتصال دستگاه‌های نوین پرداز (NP)
              {gateway && (
                <Badge variant={gateway.running ? 'default' : 'destructive'} className={gateway.running ? 'bg-emerald-600' : ''}>
                  {gateway.running ? 'فعال' : 'غیرفعال'}
                </Badge>
              )}
            </CardTitle>
          </CardHeader>
          <CardContent className="text-sm text-muted-foreground space-y-2">
            {gateway ? (
              <>
                <div className="grid grid-cols-2 md:grid-cols-4 gap-3">
                  <div className="flex justify-between md:block"><span className="md:text-xs">پورت شنود</span><span dir="ltr" className="font-mono">{faDigits(gateway.port)}</span></div>
                  <div className="flex justify-between md:block"><span className="md:text-xs">اتصال‌های فعال</span><span className="font-semibold text-foreground">{faDigits(gateway.connections)}</span></div>
                  <div className="flex justify-between md:block"><span className="md:text-xs">پیام‌های دریافتی</span><span className="font-semibold text-foreground">{faDigits(gateway.events)}</span></div>
                  <div className="flex justify-between md:block"><span className="md:text-xs">ترددهای ثبت‌شده</span><span className="font-semibold text-emerald-600">{faDigits(gateway.punches)}</span></div>
                </div>
                {gateway.lastEventAt && <div>آخرین پیام دستگاه: {faTime(gateway.lastEventAt)}</div>}
                {gateway.lastUnknownSerial && (
                  <div className="text-amber-600">آخرین سریال ناشناس: <span dir="ltr" className="font-mono">{gateway.lastUnknownSerial}</span> — این دستگاه ثبت نشده است؛ با همین سریال اضافه‌اش کنید.</div>
                )}
                {gateway.lastError && <div className="text-red-600">آخرین خطا: {gateway.lastError}</div>}
              </>
            ) : (
              <div className="flex items-center gap-2"><Loader2 className="h-4 w-4 animate-spin" /> در حال دریافت وضعیت…</div>
            )}
            <div className="text-xs leading-6 border-t pt-2">
              دستگاه‌های سری NP (مثل NP756+) خودشان به این سرور وصل می‌شوند: در منوی دستگاه ← تنظیمات ← ارتباط، «آدرس سرور» را IP همین سیستم و پورت را {faDigits(gateway?.port ?? 5005)} قرار دهید.
              در ویندوز فایروال اگر پرسید، <b>Allow access</b> را بزنید.
            </div>
          </CardContent>
        </Card>

        <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
          {boot.devices.map((d) => (
            <Card key={d.id} className={d.status === 'ONLINE' ? 'border-emerald-400' : ''}>
              <CardHeader className="pb-2">
                <CardTitle className="text-base flex items-center gap-2">
                  <Fingerprint className="h-5 w-5 text-primary" />
                  {d.name}
                  {d.status === 'ONLINE' ? <Wifi className="h-4 w-4 text-emerald-500" /> : d.status === 'OFFLINE' ? <WifiOff className="h-4 w-4 text-red-500" /> : null}
                  {d.type === 'NOVIN_NP' && d.protocolFamily === 'FKATTEND' && <Badge variant="secondary" className="text-[10px]">FKAttend · Bridge</Badge>}
                  {d.type === 'NOVIN_NP' && d.protocolFamily !== 'FKATTEND' && <Badge variant="secondary" className="text-[10px]">NP پوش</Badge>}
                </CardTitle>
              </CardHeader>
              <CardContent className="space-y-3 text-sm">
                <div className="space-y-1 text-muted-foreground">
                  <div className="flex justify-between">
                    <span>نوع</span>
                    <span>{d.type === 'NOVIN_NP' ? 'نوین پرداز NP (پوش + دانلود)' : 'ZKTeco و سازگارها'}</span>
                  </div>
                  {d.type === 'NOVIN_NP' && <div className="flex justify-between"><span>سریال دستگاه</span><span dir="ltr" className="font-mono text-xs">{d.serialNumber ?? '—'}</span></div>}
                  {d.type === 'NOVIN_NP' && <div className="flex justify-between"><span>پروتکل SDK</span><span className="text-xs">{d.protocolFamily === 'FKATTEND' ? 'FKAttend (SDK رسمی از طریق Bridge)' : 'SBXPC (باینری مستقیم)'}</span></div>}
                  {d.type === 'NOVIN_NP' && <div className="flex justify-between"><span>آدرس دستگاه</span><span dir="ltr" className="font-mono text-xs">{d.ipAddress ? `${d.ipAddress}:${d.port}` : '— (برای تست/دانلود وارد کنید)'}</span></div>}
                  {d.type === 'NOVIN_NP' && d.protocolFamily === 'FKATTEND' && <div className="flex justify-between"><span>Bridge</span><span dir="ltr" className="font-mono text-xs">{d.bridgeUrl || '— (تنظیم کنید)'}{d.hasBridgeToken ? ' · توکن: ●●●●' : ' · توکن: —'}</span></div>}
                  {d.type === 'NOVIN_NP' && d.rtEnabled && <div className="flex justify-between"><span>شبه‌بی‌درنگ (Polling)</span><span className="text-xs">فعال — هر {faDigits(d.rtPollSec ?? 30)} ثانیه</span></div>}
                  {d.type === 'NOVIN_NP' && <div className="flex justify-between"><span>رمز شبکه</span><span dir="ltr" className="font-mono text-xs">{d.hasPassword ? '●●●● (تنظیم‌شده — نمایش داده نمی‌شود)' : '— (خالی)'}</span></div>}
                  {d.type !== 'NOVIN_NP' && <div className="flex justify-between"><span>پروتکل</span><span dir="ltr">ZKTeco · {d.protocol} · {faDigits(d.port)}</span></div>}
                  {d.type !== 'NOVIN_NP' && <div className="flex justify-between"><span>آدرس</span><span dir="ltr" className="font-mono text-xs">{d.ipAddress ?? '—'}</span></div>}
                  {d.serialNumber && d.type !== 'NOVIN_NP' && <div className="flex justify-between"><span>سریال</span><span dir="ltr" className="font-mono text-xs">{d.serialNumber}</span></div>}
                  {d.location && <div className="flex justify-between"><span>موقعیت</span><span>{d.location}</span></div>}
                  <div className="flex justify-between"><span>آخرین دریافت</span><span>{d.lastSyncAt ? faTime(d.lastSyncAt) : '—'}</span></div>
                  {d.type === 'NOVIN_NP' && d.lastLogAt && <div className="flex justify-between"><span>تا تاریخِ دریافت‌شده</span><span>{faTime(d.lastLogAt)}</span></div>}
                  {d.lastSyncCount ? <div className="flex justify-between"><span>آخرین تعداد دریافت</span><span>{faDigits(d.lastSyncCount)} تردد</span></div> : null}
                  {d.syncIntervalMin ? <div className="flex justify-between"><span>دریافت خودکار</span><span>هر {faDigits(d.syncIntervalMin)} دقیقه</span></div> : null}
                  {d.syncIntervalMin ? <div className="flex justify-between"><span>دریافت بعدی (طبق برنامه)</span><span>{d.lastSyncAt ? faTime(new Date(new Date(d.lastSyncAt).getTime() + d.syncIntervalMin * 60000).toISOString()) : 'بلافاصله پس از راه‌اندازی سرور'}</span></div> : null}
                  {d.lastSyncError ? <div className="text-xs text-red-600 leading-5 rounded-lg bg-red-50 dark:bg-red-950/30 p-2">آخرین خطا: {d.lastSyncError}</div> : null}
                </div>
                {d.type === 'NOVIN_NP' ? (
                  <div className="space-y-2 pt-1">
                    <div className="flex flex-wrap gap-1.5">
                      <Button size="sm" variant="outline" onClick={() => action(d.id, 'test')} disabled={busy !== null} className="gap-1">
                        {busy === `${d.id}:test` ? <Loader2 className="h-3.5 w-3.5 animate-spin" /> : <Plug className="h-3.5 w-3.5" />} تست اتصال
                      </Button>
                    </div>
                    <div className="flex flex-wrap items-center gap-1.5">
                      <Select value={syncDays[d.id] ?? 'auto'} onValueChange={(v) => setSyncDays({ ...syncDays, [d.id]: v })}>
                        <SelectTrigger className="h-8 w-[170px] text-xs"><SelectValue /></SelectTrigger>
                        <SelectContent>
                          <SelectItem value="auto" className="text-xs">ادامه از آخرین دریافت</SelectItem>
                          <SelectItem value="1" className="text-xs">۱ روز اخیر</SelectItem>
                          <SelectItem value="3" className="text-xs">۳ روز اخیر</SelectItem>
                          <SelectItem value="7" className="text-xs">۷ روز اخیر</SelectItem>
                          <SelectItem value="14" className="text-xs">۱۴ روز اخیر</SelectItem>
                          <SelectItem value="30" className="text-xs">۳۰ روز اخیر</SelectItem>
                          <SelectItem value="90" className="text-xs">۹۰ روز اخیر</SelectItem>
                          <SelectItem value="365" className="text-xs">یک سال اخیر</SelectItem>
                          <SelectItem value="3650" className="text-xs">همه ترددهای حافظه</SelectItem>
                        </SelectContent>
                      </Select>
                      <Button size="sm" onClick={() => action(d.id, 'sync', syncDays[d.id] === 'auto' ? null : Number(syncDays[d.id] ?? 'auto'))} disabled={busy !== null} className="gap-1">
                        {busy === `${d.id}:sync` ? <Loader2 className="h-3.5 w-3.5 animate-spin" /> : <Download className="h-3.5 w-3.5" />} دانلود ترددها
                      </Button>
                    </div>
                    <div className="text-xs rounded-lg bg-muted/60 p-2.5 leading-6 text-muted-foreground">
                      {d.protocolFamily === 'FKATTEND'
                        ? 'دکمه «دانلود ترددها» کل حافظهٔ دستگاه را از طریق Bridge می‌خواند و فقط رکوردهای جدید را ذخیره می‌کند (تکراری‌ها idempotent نادیده گرفته می‌شوند). «تست اتصال» فقط وضعیت/زمان پاسخ را می‌خواند و چیزی روی دستگاه تغییر نمی‌دهد.'
                        : 'ترددهای جدید به‌صورت لحظه‌ای (پوش) هم دریافت می‌شوند؛ دکمه «دانلود ترددها» اطلاعات ذخیره‌شده در حافظه دستگاه را می‌گیرد و دفعه بعد به‌صورت خودکار از همان‌جا ادامه می‌دهد.'}
                    </div>
                  </div>
                ) : (
                  <div className="flex flex-wrap gap-1.5 pt-1">
                    <Button size="sm" variant="outline" onClick={() => action(d.id, 'test')} disabled={busy !== null} className="gap-1">
                      {busy === `${d.id}:test` ? <Loader2 className="h-3.5 w-3.5 animate-spin" /> : <Plug className="h-3.5 w-3.5" />} تست اتصال
                    </Button>
                    <Button size="sm" variant="outline" onClick={() => action(d.id, 'users')} disabled={busy !== null} className="gap-1">
                      {busy === `${d.id}:users` ? <Loader2 className="h-3.5 w-3.5 animate-spin" /> : <Users className="h-3.5 w-3.5" />} کاربران
                    </Button>
                    <Button size="sm" onClick={() => action(d.id, 'sync')} disabled={busy !== null} className="gap-1">
                      {busy === `${d.id}:sync` ? <Loader2 className="h-3.5 w-3.5 animate-spin" /> : <RefreshCw className="h-3.5 w-3.5" />} همگام‌سازی ترددها
                    </Button>
                  </div>
                )}
                <div className="flex gap-1.5">
                  <Button size="sm" variant="ghost" onClick={() => openEdit(d)}><Pencil className="h-3.5 w-3.5" /></Button>
                  <Button size="sm" variant="ghost" className="text-destructive" onClick={async () => { if (confirm('حذف دستگاه؟')) { await api(`/api/devices?id=${d.id}`, { method: 'DELETE' }); reloadBoot() } }}><Trash2 className="h-3.5 w-3.5" /></Button>
                </div>
              </CardContent>
            </Card>
          ))}
          {boot.devices.length === 0 && <Card><CardContent className="text-center text-muted-foreground py-10">دستگاهی ثبت نشده</CardContent></Card>}
        </div>

        {testResult && (
          <Card>
            <CardHeader className="pb-2"><CardTitle className="text-base">نتیجه عملیات</CardTitle></CardHeader>
            <CardContent>
              {typeof testResult.ok === 'boolean' && 'serialNumber' in testResult ? (
                <div className="space-y-2 text-sm">
                  {testResult.ok ? (
                    <>
                      <div className="flex items-center gap-2 font-semibold text-emerald-600"><Wifi className="h-4 w-4" /> ارتباط با دستگاه برقرار شد</div>
                      <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
                        <div className="flex justify-between md:block rounded-lg bg-muted/60 p-2"><span className="text-xs text-muted-foreground">شماره سریال دستگاه</span><span dir="ltr" className="font-mono">{String(testResult.serialNumber ?? '—')}</span></div>
                        {testResult.deviceTime ? (
                          <div className="flex justify-between md:block rounded-lg bg-muted/60 p-2"><span className="text-xs text-muted-foreground">ساعت دستگاه</span><span>{faTime(String(testResult.deviceTime))}</span></div>
                        ) : null}
                        {testResult.dialect ? (
                          <div className="flex justify-between md:block rounded-lg bg-muted/60 p-2"><span className="text-xs text-muted-foreground">گویش پروتکل</span><span>{({ CLASSIC: 'کلاسیک (۱۶بایتی)', RESULT_DIRECT: 'کلاسیک بدون ACK', EXT: 'EXT (۱۲بایتی)', EXT_SECURE: 'رمزشده (رمز شبکه)', CLASSIC_SECURE: 'کلاسیک رمزشده', HTTP_JSON: 'API HTTP (نسل جدید)' } as Record<string, string>)[String(testResult.dialect)] ?? String(testResult.dialect)}</span></div>
                        ) : null}
                        {testResult.logCount != null ? (
                          <div className="flex justify-between md:block rounded-lg bg-muted/60 p-2"><span className="text-xs text-muted-foreground">ترددهای ذخیره‌شده روی دستگاه</span><span>{String(testResult.logCount)}</span></div>
                        ) : null}
                        {testResult.passwordFound ? (
                          <div className="flex justify-between md:block rounded-lg bg-muted/60 p-2"><span className="text-xs text-muted-foreground">رمز شبکه</span><span>تنظیم/یافت شد — مقدار نمایش داده نمی‌شود</span></div>
                        ) : null}
                        {testResult.serialMatch === false ? (
                          <div className="md:col-span-2 text-amber-600 text-xs leading-5 rounded-lg bg-amber-50 dark:bg-amber-950/30 p-2">این سریال با سریال ثبت‌شده در پرونده دستگاه یکی نیست — پیام‌های لحظه‌ای (پوش) با سریال پرونده مطابقت داده می‌شوند. پیشنهاد: سریال پرونده را اصلاح کنید.</div>
                        ) : null}
                      </div>
                      {testResult.note ? <div className="text-xs text-muted-foreground leading-5 rounded-lg bg-muted/40 p-2">{String(testResult.note)}</div> : null}
                    </>
                  ) : (
                    <div className="space-y-1">
                      <div className="flex items-center gap-2 font-semibold text-red-600"><WifiOff className="h-4 w-4" /> ارتباط برقرار نشد</div>
                      <div className="text-sm">{String(testResult.error ?? '')}</div>
                      {testResult.detail ? <div dir="ltr" className="text-xs font-mono text-muted-foreground">{String(testResult.detail)}</div> : null}
                      {testResult.raw ? <pre dir="ltr" className="text-[10px] font-mono text-muted-foreground bg-muted rounded-lg p-2 overflow-auto max-h-40 leading-4">{String(testResult.raw)}</pre> : null}
                      <div className="text-xs text-muted-foreground leading-5 pt-1">راهنمایی: IP دستگاه را از منوی تنظیمات شبکه دستگاه بررسی کنید، مطمئن شوید سیستم و دستگاه در یک شبکه‌اند و پورتِ «ارتباط» دستگاه (مثلاً ۴۲۳۸ یا ۵۰۰۵) درست وارد شده است. اگر دستگاه رمز شبکه دارد، آن را در پرونده دستگاه (فیلد «رمز شبکه») وارد کنید — مدل‌های جدید NP فقط فرمان‌های رمزشده می‌پذیرند.</div>
                    </div>
                  )}
                </div>
              ) : (
                <pre className="text-xs bg-muted rounded-lg p-3 overflow-auto max-h-60" dir="ltr">{JSON.stringify(testResult, null, 2)}</pre>
              )}
            </CardContent>
          </Card>
        )}

        <Card>
          <CardContent className="text-sm text-muted-foreground leading-7 p-4">
            <div className="font-semibold text-foreground mb-1">راهنمای اتصال دستگاه‌ها:</div>
            <div>• <b>دستگاه‌های نوین پرداز (NP756+، NP756، NP746، NP770 و…):</b> روی «دستگاه جدید» نوع را «نوین پرداز NP» بگذارید و <b>شماره سریال دستگاه</b> را (مثل 7561202505176 — پشت دستگاه/باگ گزارش اطلاعات) وارد کنید و در قسمت IP هم آدرس دستگاه را بنویسید. سپس: <b>تست اتصال</b> را بزنید تا مطمئن شوید ارتباط برقرار است؛ با <b>دانلود ترددها</b> می‌توانید اطلاعات قبلیِ حافظه دستگاه را (تا چند روز قبل، به انتخاب خودتان) بگیرید؛ دفعات بعدی خودکار از همان‌جا ادامه می‌دهد. علاوه بر آن، ترددهای جدید به‌صورت لحظه‌ای هم دریافت می‌شوند: در منوی دستگاه ← تنظیمات ← ارتباط: آدرس سرور = IP سیستمِ نرم‌افزار و پورت 5005.</div>
            <div>• <b>ZKTeco و سازگارها:</b> IP و پورت ۴۳۷۰ دستگاه را وارد کنید و «تست اتصال» بزنید؛ ترددها با «همگام‌سازی» دریافت می‌شوند.</div>
            <div>• <b>سایر برندها (FK، SBX، Hysoon و…):</b> از خروجی Excel/CSV دستگاه استفاده کنید و از تب «ورود داده از فایل» بارگذاری کنید.</div>
            <div>• برای مطابقت خودکار، «شناسه روی دستگاه» هر پرسنلی را در پرونده‌اش ثبت کنید یا کد پرسنلی را با شماره کارت/شناسه دستگاه یکی نگه دارید.</div>
          </CardContent>
        </Card>
      </TabsContent>

      <TabsContent value="import" className="space-y-4">
        <Card>
          <CardHeader className="pb-3"><CardTitle className="text-base flex items-center gap-2"><Upload className="h-4 w-4 text-primary" /> ورود داده از فایل Excel / CSV</CardTitle></CardHeader>
          <CardContent className="space-y-4">
            <div className="flex gap-2">
              <Button variant={importMode === 'punches' ? 'default' : 'outline'} onClick={() => setImportMode('punches')}>ترددها (خروجی دستگاه)</Button>
              <Button variant={importMode === 'personnel' ? 'default' : 'outline'} onClick={() => setImportMode('personnel')}>پرسنل</Button>
            </div>
            <div className="text-sm text-muted-foreground leading-7">
              {importMode === 'punches' ? (
                <>ستون‌های قابل تشخیص: <b>کد / uid</b> (کد پرسنلی یا شناسه دستگاه)، <b>تاریخ</b> + <b>ساعت</b> یا <b>datetime</b>، و اختیاری <b>ورود/خروج</b>.<br />مثال ردیف: <span dir="ltr" className="font-mono text-xs">1001, 2025-11-01, 08:03</span></>
              ) : (
                <>ستون‌های قابل تشخیص: <b>کد پرسنلی</b>، <b>نام و نام خانوادگی</b>، <b>کد ملی</b>، <b>موبایل</b></>
              )}
            </div>
            <input type="file" accept=".xlsx,.xls,.csv" className="w-full text-sm border rounded-lg p-4 cursor-pointer"
              onChange={(e) => e.target.files?.[0] && doImport(e.target.files[0])} disabled={importing} />
            {importing && <div className="flex items-center gap-2 text-sm"><Loader2 className="h-4 w-4 animate-spin" /> در حال پردازش…</div>}
          </CardContent>
        </Card>
      </TabsContent>

      <TabsContent value="logs" className="space-y-4">
        <Card>
          <CardContent className="p-0">
            <div className="max-h-[70vh] overflow-auto">
              <Table>
                <TableHeader className="sticky top-0 bg-background z-10">
                  <TableRow><TableHead>زمان</TableHead><TableHead>سطح</TableHead><TableHead>پیام</TableHead></TableRow>
                </TableHeader>
                <TableBody>
                  {logs.map((l) => (
                    <TableRow key={l.id}>
                      <TableCell className="text-xs whitespace-nowrap" dir="ltr">{faDigits(l.createdAt.slice(0, 10))} · {faTime(l.createdAt)}</TableCell>
                      <TableCell className={`text-xs font-semibold ${LEVEL_COLOR[l.level] ?? ''}`}>{l.level}</TableCell>
                      <TableCell className="text-sm">{l.message}</TableCell>
                    </TableRow>
                  ))}
                  {logs.length === 0 && <TableRow><TableCell colSpan={3} className="text-center text-muted-foreground py-10">رکوردی نیست</TableCell></TableRow>}
                </TableBody>
              </Table>
            </div>
          </CardContent>
        </Card>
      </TabsContent>

      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent dir="rtl" className="max-w-md">
          <DialogHeader><DialogTitle>{editing ? 'ویرایش دستگاه' : 'دستگاه جدید'}</DialogTitle></DialogHeader>
          <div className="space-y-3">
            <div className="space-y-1.5">
              <Label>نوع دستگاه *</Label>
              <Select value={form.type} onValueChange={(v) => setForm({ ...form, type: v, port: v === 'NOVIN_NP' ? 5005 : 4370 })}>
                <SelectTrigger><SelectValue /></SelectTrigger>
                <SelectContent>
                  <SelectItem value="NOVIN_NP">نوین پرداز NP (NP756+، NP746 و…) — اتصال دستگاه به نرم‌افزار</SelectItem>
                  <SelectItem value="ZKTECO">ZKTeco و سازگارها — اتصال نرم‌افزار به دستگاه</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-1.5"><Label>نام *</Label><Input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} placeholder="مثلاً دستگاه ورودی اصلی" /></div>
            {form.type === 'NOVIN_NP' && (
              <div className="space-y-1.5">
                <Label>پروتکل ارتباطی (SDK) *</Label>
                <Select
                  value={form.protocolFamily}
                  onValueChange={(v) => setForm({ ...form, protocolFamily: v, port: v === 'FKATTEND' ? 4238 : 5005 })}
                >
                  <SelectTrigger><SelectValue /></SelectTrigger>
                  <SelectContent>
                    <SelectItem value="FKATTEND">FKAttend — SDK رسمی FKAttend از طریق Bridge (۳۲ بیتی) · پورت پیش‌فرض ۴۲۳۸ · مسیر تأییدشدهٔ NP756+</SelectItem>
                    <SelectItem value="SBXPC">SBXPC — پروتکل باینری مستقیم · پورت پیش‌فرض ۵۰۰۵ (مسیر قدیمی، حفظ شده)</SelectItem>
                  </SelectContent>
                </Select>
                <div className="text-xs text-muted-foreground leading-5">
                  پروتکل را <b>صریح</b> انتخاب می‌کنید و از روی شمارهٔ پورت حدس زده نمی‌شود؛ پورت پیش‌فرض هر مسیر فقط پیشنهاد اولیه است و بعداً آزادانه قابل تغییر است. تغییر تنظیمات اینجا <b>هیچ تنظیمی را روی خود دستگاه عوض نمی‌کند</b>.
                </div>
              </div>
            )}
            <div className="space-y-1.5">
              <Label>شماره سریال دستگاه {form.type === 'NOVIN_NP' ? '*' : '(اختیاری)'}</Label>
              <Input dir="ltr" className="font-mono" value={form.serialNumber} onChange={(e) => setForm({ ...form, serialNumber: e.target.value })} placeholder="7561202505176" />
              {form.type === 'NOVIN_NP' && <div className="text-xs text-muted-foreground leading-5">پیام‌های دستگاه با این سریال مطابقت داده می‌شوند — بدون سریال درست، دستگاه متصل نمی‌شود (مثل نرم‌افزار اصلی). سریال روی برچسب دستگاه یا در منوی «گزارش اطلاعات» دستگاه موجود است. با «تست اتصال» سریال واقعی از خود دستگاه خوانده می‌شود.</div>}
            </div>
            {form.type === 'NOVIN_NP' ? (
              <div className="space-y-2">
                <div className="grid grid-cols-2 gap-2">
                  <div className="space-y-1.5">
                    <Label>IP دستگاه (برای تست/دانلود)</Label>
                    <Input dir="ltr" value={form.ipAddress} onChange={(e) => setForm({ ...form, ipAddress: e.target.value })} placeholder="192.168.1.201" />
                  </div>
                  <div className="space-y-1.5">
                    <Label>پورت</Label>
                    <Input type="number" dir="ltr" value={form.port} onChange={(e) => setForm({ ...form, port: Number(e.target.value) })} placeholder="4238" />
                  </div>
                </div>
                <div className="space-y-1.5">
                  <Label>رمز شبکه (اختیاری)</Label>
                  <Input type="number" dir="ltr" value={form.password} onChange={(e) => setForm({ ...form, password: e.target.value })} placeholder={editing ? 'خالی = بدون تغییر (رمز ذخیره‌شده حفظ می‌شود)' : 'خالی = رمزهای رایج خودکار امتحان می‌شود'} />
                  <div className="text-xs text-muted-foreground leading-5">مدل‌های جدید NP (مثل NP756+) فرمان‌ها را فقط <b>رمزشده</b> می‌پذیرند؛ کلید رمز از همین «رمز شبکه» ساخته می‌شود (منوی دستگاه ← تنظیمات ← ارتباط ← رمز شبکه). اگر خالی بگذارید، نرم‌افزار هنگام «تست اتصال» رمزهای رایج (۰، ۱۲۳۴۵۶ و…) را خودکار امتحان می‌کند و رمز درست را به یاد می‌سپارد. <b>مقدار رمز هرگز نمایش داده یا در لاگ‌ها ثبت نمی‌شود.</b></div>
                </div>
                {form.protocolFamily === 'FKATTEND' && (
                  <div className="space-y-2 rounded-lg border p-3 bg-muted/30">
                    <div className="text-xs font-semibold">تنظیمات اتصال FKAttend (از طریق Bridge)</div>
                    <div className="grid grid-cols-2 gap-2">
                      <div className="space-y-1.5">
                        <Label>آدرس Bridge</Label>
                        <Input dir="ltr" value={form.bridgeUrl} onChange={(e) => setForm({ ...form, bridgeUrl: e.target.value })} placeholder="http://127.0.0.1:8765" />
                        <div className="text-[10px] text-muted-foreground leading-4">فقط روی همین سیستم (localhost) مجاز است. Bridge با اجرای START-BRIDGE.bat کنار نرم‌افزار بالا می‌آید.</div>
                      </div>
                      <div className="space-y-1.5">
                        <Label>توکن Bridge</Label>
                        <Input type="password" dir="ltr" value={form.bridgeToken} onChange={(e) => setForm({ ...form, bridgeToken: e.target.value })} placeholder={editing ? 'خالی = بدون تغییر' : 'از پنجرهٔ Bridge کپی کنید'} />
                      </div>
                    </div>
                    <div className="grid grid-cols-3 gap-2">
                      <div className="space-y-1.5"><Label>مهلت اتصال (ms)</Label><Input type="number" dir="ltr" value={form.connTimeoutMs} onChange={(e) => setForm({ ...form, connTimeoutMs: Number(e.target.value) })} /></div>
                      <div className="space-y-1.5"><Label>شمارهٔ دستگاه SDK</Label><Input type="number" dir="ltr" value={form.machineNumber} onChange={(e) => setForm({ ...form, machineNumber: Number(e.target.value) })} /></div>
                      <div className="space-y-1.5"><Label>License SDK</Label><Input type="number" dir="ltr" value={form.sdkLicense} onChange={(e) => setForm({ ...form, sdkLicense: Number(e.target.value) })} /></div>
                    </div>
                    <div className="text-[10px] text-muted-foreground leading-4">توکن و رمز شبکه هرگز نمایش داده یا در لاگ‌ها ثبت نمی‌شوند. Bridge فقط خواندن انجام می‌دهد و هیچ فرمان تغییر/حذف روی دستگاه ارسال نمی‌کند.</div>
                  </div>
                )}
                <div className="text-xs rounded-lg bg-muted/60 p-3 leading-6 text-muted-foreground">
                  {form.protocolFamily === 'FKATTEND'
                    ? <>دریافت ترددها از طریق Bridge مستقیم از حافظهٔ دستگاه دانلود می‌شود (همان روش موفق آزمایش‌شده روی پورت ۴۲۳۸)؛ دفعات بعدی از همان‌جا ادامه می‌دهد. ترددهای لحظه‌ای با «دریافت شبه‌بی‌درنگ» در تنظیمات خودکار فعال می‌شوند (Polling — نه پوش واقعی).</>
                    : <>دریافت ترددها به دو روش کار می‌کند: <b>لحظه‌ای (پوش)</b> — دستگاه خودش به سرور وصل می‌شود؛ کافی است در منوی دستگاه ← تنظیمات ← ارتباط، آدرس سرور را IP همین سیستم و پورت را 5005 بگذارید. و <b>دانلود مستقیم</b> — با وارد کردن IP دستگاه بالا، ترددهای ذخیره‌شده در حافظه دستگاه دریافت می‌شود (همان روش نرم‌افزار اصلی).</>}
                </div>
              </div>
            ) : (
              <div className="grid grid-cols-3 gap-2">
                <div className="space-y-1.5"><Label>IP</Label><Input dir="ltr" value={form.ipAddress} onChange={(e) => setForm({ ...form, ipAddress: e.target.value })} placeholder="192.168.1.201" /></div>
                <div className="space-y-1.5"><Label>پورت</Label><Input type="number" dir="ltr" value={form.port} onChange={(e) => setForm({ ...form, port: Number(e.target.value) })} /></div>
                <div className="space-y-1.5">
                  <Label>پروتکل</Label>
                  <Select value={form.protocol} onValueChange={(v) => setForm({ ...form, protocol: v })}>
                    <SelectTrigger><SelectValue /></SelectTrigger>
                    <SelectContent><SelectItem value="TCP">TCP</SelectItem><SelectItem value="UDP">UDP</SelectItem></SelectContent>
                  </Select>
                </div>
              </div>
            )}
            <div className="space-y-1.5"><Label>موقعیت نصب</Label><Input value={form.location} onChange={(e) => setForm({ ...form, location: e.target.value })} placeholder="مثلاً ورودی ساختمان" /></div>
            <div className="space-y-2 rounded-lg border p-3">
              <div className="text-xs font-semibold">دریافت خودکار (Scheduler سرور)</div>
              <div className="flex items-center gap-2"><Switch id="dev-auto" checked={form.autoSync} onCheckedChange={(v) => setForm({ ...form, autoSync: v })} /><Label htmlFor="dev-auto">فعال — دریافت خودکار ترددها</Label></div>
              {form.autoSync && (
                <div className="space-y-1.5">
                  <Label>فاصلهٔ دریافت (دقیقه)</Label>
                  <Input type="number" dir="ltr" value={form.syncIntervalMin} onChange={(e) => setForm({ ...form, syncIntervalMin: Number(e.target.value) })} placeholder="۵ (پیشنهادی)" />
                  <div className="text-xs text-muted-foreground leading-5">زمان‌بندی در سرور انجام می‌شود (نه مرورگر) — با بستن مرورگر یا ری‌استارت سرور ادامه می‌یابد و از تنظیمات همین دستگاه خوانده می‌شود. بازهٔ مجاز: ۱ تا ۱۴۴۰ دقیقه.</div>
                </div>
              )}
              {form.type === 'NOVIN_NP' && (
                <div className="space-y-1.5 pt-1 border-t">
                  <div className="flex items-center gap-2"><Switch id="dev-rt" checked={form.rtEnabled} onCheckedChange={(v) => setForm({ ...form, rtEnabled: v })} /><Label htmlFor="dev-rt">دریافت شبه‌بی‌درنگ (Polling)</Label></div>
                  {form.rtEnabled && (
                    <div className="space-y-1.5">
                      <Label>فاصلهٔ Polling (ثانیه)</Label>
                      <Input type="number" dir="ltr" value={form.rtPollSec} onChange={(e) => setForm({ ...form, rtPollSec: Number(e.target.value) })} placeholder="۳۰ (پیشنهادی)" />
                      <div className="text-xs text-muted-foreground leading-5">هر بارِ Poll کل حافظهٔ دستگاه خوانده و فقط رکوردهای جدید (idempotent) ذخیره می‌شود — با تکرار دریافت، هیچ رکوردی دوباره ثبت نمی‌شود. این حالت <b>شبه‌بی‌درنگ</b> است (تأخیر تا حد فاصلهٔ Polling) نه Real-Time واقعی؛ بازهٔ مجاز: ۱۵ تا ۶۰۰ ثانیه. برای دستگاه هم‌زمان با Scheduler معمولی تداخل ایجاد نمی‌کند (قفل هر دستگاه).</div>
                    </div>
                  )}
                </div>
              )}
              <div className="text-[10px] text-muted-foreground leading-4">خاموش کردن دریافت خودکار هیچ داده‌ای حذف نمی‌کند و «دانلود ترددها» دستی همیشه کار می‌کند.</div>
            </div>
            <div className="flex items-center gap-2"><Switch id="dev-en" checked={form.enabled} onCheckedChange={(v) => setForm({ ...form, enabled: v })} /><Label htmlFor="dev-en">فعال</Label></div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setOpen(false)}>انصراف</Button>
            <Button onClick={save}>ذخیره</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      <Dialog open={importOpen} onOpenChange={setImportOpen}>
        <DialogContent dir="rtl" className="max-w-sm">
          <DialogHeader><DialogTitle>ورود سریع از فایل</DialogTitle></DialogHeader>
          <Select value={importMode} onValueChange={setImportMode}>
            <SelectTrigger><SelectValue /></SelectTrigger>
            <SelectContent><SelectItem value="punches">ترددها</SelectItem><SelectItem value="personnel">پرسنل</SelectItem></SelectContent>
          </Select>
          <input type="file" accept=".xlsx,.xls,.csv" className="w-full text-sm border rounded-lg p-3"
            onChange={(e) => e.target.files?.[0] && doImport(e.target.files[0])} disabled={importing} />
          {importing && <div className="flex items-center gap-2 text-sm"><Loader2 className="h-4 w-4 animate-spin" /> در حال پردازش…</div>}
        </DialogContent>
      </Dialog>
    </Tabs>
  )
}

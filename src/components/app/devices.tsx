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
import { Plus, Trash2, Pencil, Loader2, Plug, RefreshCw, Users, Upload, Fingerprint, Wifi, WifiOff, RadioTower, Activity } from 'lucide-react'

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
  const { toast } = useToast()

  const blank = { name: '', type: 'NOVIN_NP', ipAddress: '', port: 5005, protocol: 'TCP', location: '', enabled: true, syncIntervalMin: 0, serialNumber: '' }
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
    setForm({ name: d.name, type: d.type, ipAddress: d.ipAddress ?? '', port: d.port, protocol: d.protocol, location: d.location ?? '', enabled: d.enabled, syncIntervalMin: d.syncIntervalMin ?? 0, serialNumber: d.serialNumber ?? '' })
    setOpen(true)
  }

  const save = async () => {
    if (!form.name.trim()) { toast({ title: 'نام دستگاه الزامی است', variant: 'destructive' }); return }
    if (form.type === 'NOVIN_NP' && !form.serialNumber.trim()) {
      toast({ title: 'شماره سریال الزامی است', description: 'پیام‌های دستگاه نوین پرداز با شماره سریال مطابقت داده می‌شوند (مثل نرم‌افزار اصلی).', variant: 'destructive' })
      return
    }
    try {
      if (editing) await api('/api/devices', { method: 'PUT', body: JSON.stringify({ id: editing.id, ...form }) })
      else await api('/api/devices', { method: 'POST', body: JSON.stringify(form) })
      setOpen(false)
      reloadBoot()
      loadLogs()
      toast({ title: 'ذخیره شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    }
  }

  const action = async (id: string, act: 'test' | 'sync' | 'users') => {
    setBusy(`${id}:${act}`)
    setTestResult(null)
    try {
      const res = await api<{ result: Record<string, unknown> }>('/api/devices', { method: 'PATCH', body: JSON.stringify({ id, action: act }) })
      setTestResult(res.result)
      reloadBoot()
      loadLogs()
      if (act === 'sync') {
        const r = res.result as { created?: number; skipped?: number; unmatched?: number }
        toast({ title: 'همگام‌سازی انجام شد', description: `${faDigits(r.created ?? 0)} تردد جدید، ${faDigits(r.skipped ?? 0)} تکراری` })
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
                  {d.type === 'NOVIN_NP' && <Badge variant="secondary" className="text-[10px]">NP پوش</Badge>}
                </CardTitle>
              </CardHeader>
              <CardContent className="space-y-3 text-sm">
                <div className="space-y-1 text-muted-foreground">
                  <div className="flex justify-between">
                    <span>نوع</span>
                    <span>{d.type === 'NOVIN_NP' ? 'نوین پرداز NP (پوش لحظه‌ای)' : 'ZKTeco و سازگارها'}</span>
                  </div>
                  {d.type === 'NOVIN_NP' ? (
                    <div className="flex justify-between"><span>سریال دستگاه</span><span dir="ltr" className="font-mono text-xs">{d.serialNumber ?? '—'}</span></div>
                  ) : (
                    <div className="flex justify-between"><span>پروتکل</span><span dir="ltr">ZKTeco · {d.protocol} · {faDigits(d.port)}</span></div>
                  )}
                  {d.type !== 'NOVIN_NP' && <div className="flex justify-between"><span>آدرس</span><span dir="ltr" className="font-mono text-xs">{d.ipAddress ?? '—'}</span></div>}
                  {d.serialNumber && d.type !== 'NOVIN_NP' && <div className="flex justify-between"><span>سریال</span><span dir="ltr" className="font-mono text-xs">{d.serialNumber}</span></div>}
                  {d.location && <div className="flex justify-between"><span>موقعیت</span><span>{d.location}</span></div>}
                  <div className="flex justify-between"><span>آخرین همگام‌سازی</span><span>{d.lastSyncAt ? faTime(d.lastSyncAt) : '—'}</span></div>
                  <div className="flex justify-between"><span>آخرین دریافت</span><span>{d.lastSyncCount ? `${faDigits(d.lastSyncCount)} تردد` : '—'}</span></div>
                </div>
                {d.type === 'NOVIN_NP' ? (
                  <div className="text-xs rounded-lg bg-muted/60 p-2.5 leading-6 text-muted-foreground">
                    دریافت ترددها <b>خودکار و لحظه‌ای</b> است؛ به‌محض ثبت در دستگاه اینجا ظاهر می‌شود. اگر ترددی نمی‌رسد: سریال، آدرس سرور دستگاه و «شناسه روی دستگاه» پرسنل را بررسی کنید (جزئیات در تاریخچه عملیات).
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
              <pre className="text-xs bg-muted rounded-lg p-3 overflow-auto max-h-60" dir="ltr">{JSON.stringify(testResult, null, 2)}</pre>
            </CardContent>
          </Card>
        )}

        <Card>
          <CardContent className="text-sm text-muted-foreground leading-7 p-4">
            <div className="font-semibold text-foreground mb-1">راهنمای اتصال دستگاه‌ها:</div>
            <div>• <b>دستگاه‌های نوین پرداز (NP756+، NP756، NP746، NP770 و…):</b> روی «دستگاه جدید» نوع را «نوین پرداز NP» بگذارید و <b>شماره سریال دستگاه</b> را (مثل 7561202505176 — پشت دستگاه/باگ گزارش اطلاعات) وارد کنید. سپس در منوی دستگاه ← تنظیمات ← ارتباط: نوع را «داخلی» یا «اینترنت»، <b>آدرس سرور = IP سیستمِ نرم‌افزار</b> و <b>پورت 5005</b> قرار دهید. ترددها از همان لحظه به‌صورت خودکار دریافت می‌شوند.</div>
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
            <div className="space-y-1.5">
              <Label>شماره سریال دستگاه {form.type === 'NOVIN_NP' ? '*' : '(اختیاری)'}</Label>
              <Input dir="ltr" className="font-mono" value={form.serialNumber} onChange={(e) => setForm({ ...form, serialNumber: e.target.value })} placeholder="7561202505176" />
              {form.type === 'NOVIN_NP' && <div className="text-xs text-muted-foreground leading-5">پیام‌های دستگاه با این سریال مطابقت داده می‌شوند — بدون سریال درست، دستگاه متصل نمی‌شود (مثل نرم‌افزار اصلی). سریال روی برچسب دستگاه یا در منوی «گزارش اطلاعات» دستگاه موجود است.</div>}
            </div>
            {form.type === 'NOVIN_NP' ? (
              <div className="text-xs rounded-lg bg-muted/60 p-3 leading-6 text-muted-foreground">
                این دستگاه‌ها خودشان به سرور وصل می‌شوند — IP لازم نیست.<br />
                تنظیمات دستگاه: منو ← تنظیمات ← ارتباط ← نوع: <b>داخلی</b> · آدرس سرور: <b>IP همین سیستم</b> · پورت: <b>5005</b>
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
            {form.type !== 'NOVIN_NP' && (
              <div className="space-y-1.5"><Label>همگام‌سازی خودکار (دقیقه، ۰=غیرفعال)</Label><Input type="number" dir="ltr" value={form.syncIntervalMin} onChange={(e) => setForm({ ...form, syncIntervalMin: Number(e.target.value) })} /></div>
            )}
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

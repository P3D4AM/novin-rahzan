'use client'

import { useEffect, useState } from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Progress } from '@/components/ui/progress'
import { QRCodeSVG } from 'qrcode.react'
import { SessionUser, api, STATUS_FA, STATUS_COLOR } from './types'
import { faDigits, durationFa, minutesToHM, jFormatFa, faTime } from './jalali-ui'
import { useToast } from '@/hooks/use-toast'
import { LogIn, LogOut, Loader2, Fingerprint, CalendarDays, Clock4, Palmtree, CreditCard } from 'lucide-react'

interface MeData {
  person: {
    id: string; personnelCode: string; firstName: string; lastName: string
    annualLeaveDays?: number
    department?: { name: string } | null
    jobTitle?: { name: string } | null
    employmentType?: { name: string } | null
    defaultShift?: { name: string; color: string; segments: Array<{ startMin: number; endMin: number }> } | null
    qrToken: string
  }
  todayPunches: Array<{ id: string; timestamp: string; direction: string; source: string }>
  dailyToday: { status: string; workedMinutes: number; lateMinutes: number; overtimeMinutes: number } | null
  monthRecords: Array<{ date: string; status: string; workedMinutes: number; lateMinutes: number; overtimeMinutes: number }>
  myLeaves: Array<{ id: string; status: string; kind: string; startDateTime: string; durationMinutes: number; leaveType?: { name: string; color: string } }>
  myOvertime: Array<{ id: string; date: string; minutes: number; status: string; reason?: string | null }>
  leaveBalance: { annual: number; usedThisMonth: number }
}

export function MyCardView({ user, onPunched }: { user: SessionUser; onPunched: () => void }) {
  const [data, setData] = useState<MeData | null>(null)
  const [punching, setPunching] = useState(false)
  const { toast } = useToast()

  const load = async () => {
    try {
      const res = await api<MeData>('/api/me')
      setData(res)
    } catch { /* ignore */ }
  }
  useEffect(() => { load()   }, [])

  const punch = async () => {
    setPunching(true)
    try {
      const geo = await new Promise<GeolocationPosition | null>((resolve) => {
        if (!navigator.geolocation) return resolve(null)
        navigator.geolocation.getCurrentPosition(resolve, () => resolve(null), { timeout: 3000 })
      })
      const res = await api<{ punch: { timestamp: string }; direction: string }>('/api/me', {
        method: 'POST',
        body: JSON.stringify({ latitude: geo?.coords.latitude?.toString(), longitude: geo?.coords.longitude?.toString() }),
      })
      toast({ title: res.direction === 'IN' ? 'ورود شما ثبت شد ✅' : 'خروج شما ثبت شد 👋' })
      load()
      onPunched()
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setPunching(false) }
  }

  if (!data) return <div className="flex items-center justify-center h-64 text-muted-foreground">در حال بارگذاری کارت…</div>

  const p = data.person
  const shiftLabel = p.defaultShift
    ? `${p.defaultShift.name} (${minutesToHM(p.defaultShift.segments[0]?.startMin ?? 0)} تا ${minutesToHM(p.defaultShift.segments[0]?.endMin ?? 0)})`
    : 'بدون شیفت ثابت'

  return (
    <div className="max-w-5xl mx-auto grid grid-cols-1 lg:grid-cols-3 gap-4">
      {/* punch card */}
      <Card className="lg:col-span-2">
        <CardHeader className="pb-3">
          <CardTitle className="text-base flex items-center gap-2"><CreditCard className="h-4 w-4 text-primary" /> کارت پرسنلی من</CardTitle>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="flex items-center gap-4">
            <div className="h-16 w-16 rounded-2xl bg-primary/10 text-primary flex items-center justify-center text-xl font-bold shrink-0">
              {p.firstName[0]}{p.lastName[0]}
            </div>
            <div className="flex-1 min-w-0">
              <div className="font-bold text-lg">{p.firstName} {p.lastName}</div>
              <div className="text-sm text-muted-foreground">کد {faDigits(p.personnelCode)} · {p.jobTitle?.name ?? '—'} · {p.department?.name ?? '—'}</div>
              <div className="text-xs text-muted-foreground mt-0.5 flex items-center gap-1">
                <CalendarDays className="h-3 w-3" /> {shiftLabel}
              </div>
            </div>
          </div>

          <Button size="lg" className="w-full h-14 text-base gap-2" onClick={punch} disabled={punching}>
            {punching ? <Loader2 className="h-5 w-5 animate-spin" /> : <Fingerprint className="h-5 w-5" />}
            {data.todayPunches.length === 0 ? 'ثبت ورود الان' : data.todayPunches[data.todayPunches.length - 1].direction === 'IN' ? 'ثبت خروج الان' : 'ثبت ورود الان'}
          </Button>

          <div className="space-y-1.5">
            <div className="text-sm font-semibold">ترددهای امروز</div>
            {data.todayPunches.length === 0 && <p className="text-sm text-muted-foreground">هنوز ترددی ثبت نشده</p>}
            {data.todayPunches.map((t) => (
              <div key={t.id} className="flex items-center gap-2 text-sm p-2 rounded-lg bg-muted/40">
                {t.direction === 'IN' ? <LogIn className="h-4 w-4 text-emerald-600" /> : <LogOut className="h-4 w-4 text-red-600" />}
                <span>{t.direction === 'IN' ? 'ورود' : 'خروج'} — {faTime(t.timestamp)}</span>
                <Badge variant="secondary" className="text-[10px] mr-auto">{t.source === 'WEB' ? 'وب' : t.source === 'KIOSK' ? 'کیشی' : t.source === 'QR' ? 'QR' : 'دستگاه'}</Badge>
              </div>
            ))}
          </div>

          {data.dailyToday && (
            <div className="grid grid-cols-3 gap-2 text-center">
              <div className="p-3 rounded-xl bg-muted/50">
                <div className="text-xs text-muted-foreground">وضعیت</div>
                <div className="font-bold text-sm mt-1">{STATUS_FA[data.dailyToday.status] ?? data.dailyToday.status}</div>
              </div>
              <div className="p-3 rounded-xl bg-muted/50">
                <div className="text-xs text-muted-foreground">کارکرد</div>
                <div className="font-bold text-sm mt-1">{durationFa(data.dailyToday.workedMinutes)}</div>
              </div>
              <div className="p-3 rounded-xl bg-muted/50">
                <div className="text-xs text-muted-foreground">تأخیر / اضافه</div>
                <div className="font-bold text-sm mt-1">{data.dailyToday.lateMinutes ? durationFa(data.dailyToday.lateMinutes) : '—'} / {data.dailyToday.overtimeMinutes ? durationFa(data.dailyToday.overtimeMinutes) : '—'}</div>
              </div>
            </div>
          )}
        </CardContent>
      </Card>

      {/* QR + leave balance */}
      <div className="space-y-4">
        <Card>
          <CardHeader className="pb-3"><CardTitle className="text-base">QR اختصاصی من</CardTitle></CardHeader>
          <CardContent className="flex flex-col items-center gap-3">
            <div className="p-3 bg-white rounded-xl border">
              <QRCodeSVG value={`hozorplus:${p.qrToken}`} size={150} />
            </div>
            <p className="text-xs text-muted-foreground text-center leading-5">
              این کد را در دستگاه کیشی (صفحه ثبت حضور) اسکن کنید یا به اپراتور نشان دهید.
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="pb-3"><CardTitle className="text-base flex items-center gap-2"><Palmtree className="h-4 w-4 text-primary" /> مانده مرخصی سالانه</CardTitle></CardHeader>
          <CardContent className="space-y-3">
            <div className="flex items-end gap-2">
              <span className="text-3xl font-bold text-primary">{faDigits(Math.max(0, (p.annualLeaveDays ?? 0) - data.leaveBalance.usedThisMonth))}</span>
              <span className="text-sm text-muted-foreground mb-1">روز از {faDigits(p.annualLeaveDays ?? 0)}</span>
            </div>
            <Progress value={Math.min(100, (data.leaveBalance.usedThisMonth / Math.max(1, p.annualLeaveDays ?? 1)) * 100)} className="h-2.5" />
            <div className="text-xs text-muted-foreground">مصرف ماه جاری: {faDigits(data.leaveBalance.usedThisMonth)} روز</div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="pb-3"><CardTitle className="text-base flex items-center gap-2"><Clock4 className="h-4 w-4 text-primary" /> خلاصه ماه جاری</CardTitle></CardHeader>
          <CardContent className="space-y-2 text-sm">
            {(() => {
              const worked = data.monthRecords.reduce((a, r) => a + r.workedMinutes, 0)
              const ot = data.monthRecords.reduce((a, r) => a + r.overtimeMinutes, 0)
              const late = data.monthRecords.reduce((a, r) => a + r.lateMinutes, 0)
              const present = data.monthRecords.filter((r) => r.status === 'PRESENT' || r.status === 'INCOMPLETE').length
              const absent = data.monthRecords.filter((r) => r.status === 'ABSENT').length
              return (
                <>
                  <div className="flex justify-between"><span className="text-muted-foreground">روزهای حضور</span><span className="font-semibold">{faDigits(present)}</span></div>
                  <div className="flex justify-between"><span className="text-muted-foreground">روزهای غیبت</span><span className={`font-semibold ${absent ? 'text-red-600' : ''}`}>{faDigits(absent)}</span></div>
                  <div className="flex justify-between"><span className="text-muted-foreground">جمع کارکرد</span><span className="font-semibold">{minutesToHM(worked)} ساعت</span></div>
                  <div className="flex justify-between"><span className="text-muted-foreground">جمع اضافه‌کاری</span><span className="font-semibold text-teal-600">{durationFa(ot)}</span></div>
                  <div className="flex justify-between"><span className="text-muted-foreground">جمع تأخیر</span><span className={`font-semibold ${late ? 'text-orange-600' : ''}`}>{durationFa(late)}</span></div>
                </>
              )
            })()}
          </CardContent>
        </Card>
      </div>

      {/* month history */}
      <Card className="lg:col-span-3">
        <CardHeader className="pb-3"><CardTitle className="text-base">کارکرد روزهای ماه جاری</CardTitle></CardHeader>
        <CardContent>
          <div className="max-h-72 overflow-y-auto">
            <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-2">
              {data.monthRecords.slice().reverse().map((r) => (
                <div key={r.date} className="flex items-center justify-between p-2.5 rounded-lg bg-muted/40 text-sm">
                  <span className="text-xs text-muted-foreground" dir="ltr">{jFormatFa(r.date)}</span>
                  <span className={`px-2 py-0.5 rounded-full text-[11px] ${STATUS_COLOR[r.status] ?? ''}`}>{STATUS_FA[r.status] ?? r.status}</span>
                  <span className="text-xs">{minutesToHM(r.workedMinutes)}</span>
                </div>
              ))}
              {data.monthRecords.length === 0 && <p className="text-sm text-muted-foreground col-span-4 text-center py-6">رکوردی برای این ماه نیست</p>}
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  )
}

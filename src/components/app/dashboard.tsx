'use client'

import { useEffect, useState } from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Progress } from '@/components/ui/progress'
import { SessionUser, Bootstrap, api, DailyRecordT } from './types'
import { minutesToHM, durationFa, jFormatFa, faDigits } from './jalali-ui'
import { Users, UserCheck, UserX, Clock4, TrendingUp, Palmtree, LogIn, LogOut, RefreshCw } from 'lucide-react'
import { ResponsiveContainer, AreaChart, Area, XAxis, YAxis, Tooltip, CartesianGrid, BarChart, Bar } from 'recharts'

interface Props {
  user: SessionUser
  boot: Bootstrap
  reloadBoot: () => void
  liveFeed: Array<{ personName: string; direction: string; time: string; source: string }>
}

interface Stats {
  totalPersons: number
  present: number
  absent: number
  late: number
  leave: number
  overtimeMinutes: number
  workedMinutes: number
  lateList: Array<{ name: string; minutes: number }>
  otList: Array<{ name: string; minutes: number }>
  trend: Array<{ day: string; present: number; absent: number }>
  deptDist: Array<{ name: string; value: number }>
}

export function DashboardView({ user, boot, liveFeed }: Props) {
  const [stats, setStats] = useState<Stats | null>(null)
  const [loading, setLoading] = useState(false)

  const load = async () => {
    setLoading(true)
    try {
      const today = new Date().toISOString().slice(0, 10)
      const res = await api<{ records: DailyRecordT[] }>(`/api/attendance?from=${today}&to=${today}`)
      const daily = res.records
      const persons = await api<{ personnel: Array<{ department?: { name: string } | null }> }>('/api/personnel')
      const total = persons.personnel.length

      const present = daily.filter((r) => r.status === 'PRESENT' || r.status === 'INCOMPLETE').length
      const absent = daily.filter((r) => r.status === 'ABSENT').length
      const late = daily.filter((r) => r.lateMinutes > 0).length
      const leave = daily.filter((r) => r.status === 'LEAVE').length
      const overtimeMinutes = daily.reduce((a, r) => a + r.overtimeMinutes, 0)
      const workedMinutes = daily.reduce((a, r) => a + r.workedMinutes, 0)

      const lateList = daily
        .filter((r) => r.lateMinutes > 0)
        .sort((a, b) => b.lateMinutes - a.lateMinutes)
        .slice(0, 5)
        .map((r) => ({ name: `${r.person?.firstName} ${r.person?.lastName}`, minutes: r.lateMinutes }))
      const otList = daily
        .filter((r) => r.overtimeMinutes > 0)
        .sort((a, b) => b.overtimeMinutes - a.overtimeMinutes)
        .slice(0, 5)
        .map((r) => ({ name: `${r.person?.firstName} ${r.person?.lastName}`, minutes: r.overtimeMinutes }))

      // 14-day trend
      const from = new Date(Date.now() - 13 * 86400000).toISOString().slice(0, 10)
      const hist = await api<{ records: DailyRecordT[] }>(`/api/attendance?from=${from}&to=${today}`)
      const byDay = new Map<string, { present: number; absent: number }>()
      for (const r of hist.records) {
        const key = r.date.slice(5, 10)
        const cur = byDay.get(key) ?? { present: 0, absent: 0 }
        if (r.status === 'PRESENT' || r.status === 'INCOMPLETE') cur.present++
        if (r.status === 'ABSENT') cur.absent++
        byDay.set(key, cur)
      }
      const trend = Array.from(byDay.entries()).map(([day, v]) => ({ day, present: v.present, absent: v.absent }))

      // department distribution
      const deptMap = new Map<string, number>()
      for (const r of daily) {
        if (r.status !== 'PRESENT' && r.status !== 'INCOMPLETE') continue
        const d = r.person?.department?.name ?? 'بدون دپارتمان'
        deptMap.set(d, (deptMap.get(d) ?? 0) + 1)
      }
      const deptDist = Array.from(deptMap.entries()).map(([name, value]) => ({ name, value }))

      setStats({ totalPersons: total, present, absent, late, leave, overtimeMinutes, workedMinutes, lateList, otList, trend, deptDist })
    } catch { /* ignore */ } finally {
      setLoading(false)
    }
  }

  useEffect(() => { load();   }, [])

  if (!stats) {
    return <div className="flex items-center justify-center h-64 text-muted-foreground">در حال بارگذاری داشبورد…</div>
  }

  const presenceRate = stats.totalPersons ? Math.round((stats.present / stats.totalPersons) * 100) : 0

  const cards = [
    { label: 'کل پرسنل فعال', value: faDigits(stats.totalPersons), icon: Users, color: 'text-primary', bg: 'bg-primary/10' },
    { label: 'حاضر الان', value: faDigits(stats.present), icon: UserCheck, color: 'text-emerald-600', bg: 'bg-emerald-500/10' },
    { label: 'غایب امروز', value: faDigits(stats.absent), icon: UserX, color: 'text-red-600', bg: 'bg-red-500/10' },
    { label: 'مرخصی امروز', value: faDigits(stats.leave), icon: Palmtree, color: 'text-amber-600', bg: 'bg-amber-500/10' },
    { label: 'دارای تأخیر', value: faDigits(stats.late), icon: Clock4, color: 'text-orange-600', bg: 'bg-orange-500/10' },
    { label: 'اضافه‌کاری امروز', value: durationFa(stats.overtimeMinutes), icon: TrendingUp, color: 'text-teal-600', bg: 'bg-teal-500/10' },
  ]

  return (
    <div className="space-y-6">
      {/* stat cards */}
      <div className="grid grid-cols-2 md:grid-cols-3 xl:grid-cols-6 gap-3">
        {cards.map((c) => {
          const Icon = c.icon
          return (
            <Card key={c.label} className="hover:shadow-md transition-shadow">
              <CardContent className="p-4 flex items-center gap-3">
                <div className={`h-11 w-11 rounded-xl ${c.bg} ${c.color} flex items-center justify-center shrink-0`}>
                  <Icon className="h-5 w-5" />
                </div>
                <div className="min-w-0">
                  <div className="text-xl font-bold truncate">{c.value}</div>
                  <div className="text-xs text-muted-foreground truncate">{c.label}</div>
                </div>
              </CardContent>
            </Card>
          )
        })}
      </div>

      <div className="grid grid-cols-1 xl:grid-cols-3 gap-4">
        {/* trend chart */}
        <Card className="xl:col-span-2">
          <CardHeader className="pb-2 flex flex-row items-center justify-between">
            <CardTitle className="text-base">روند حضور ۱۴ روز اخیر</CardTitle>
            <Button variant="ghost" size="sm" onClick={load} disabled={loading} className="gap-1">
              <RefreshCw className={`h-4 w-4 ${loading ? 'animate-spin' : ''}`} /> بروزرسانی
            </Button>
          </CardHeader>
          <CardContent>
            <div className="h-64" dir="ltr">
              <ResponsiveContainer width="100%" height="100%">
                <AreaChart data={stats.trend}>
                  <defs>
                    <linearGradient id="gPresent" x1="0" y1="0" x2="0" y2="1">
                      <stop offset="5%" stopColor="#10b981" stopOpacity={0.35} />
                      <stop offset="95%" stopColor="#10b981" stopOpacity={0} />
                    </linearGradient>
                    <linearGradient id="gAbsent" x1="0" y1="0" x2="0" y2="1">
                      <stop offset="5%" stopColor="#ef4444" stopOpacity={0.3} />
                      <stop offset="95%" stopColor="#ef4444" stopOpacity={0} />
                    </linearGradient>
                  </defs>
                  <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
                  <XAxis dataKey="day" tick={{ fontSize: 11 }} reversed />
                  <YAxis tick={{ fontSize: 11 }} orientation="right" allowDecimals={false} />
                  <Tooltip contentStyle={{ direction: 'rtl', borderRadius: 8, fontSize: 12 }} formatter={(v: number, name: string) => [faDigits(v), name === 'present' ? 'حاضر' : 'غایب']} />
                  <Area type="monotone" dataKey="present" stroke="#10b981" strokeWidth={2} fill="url(#gPresent)" name="present" />
                  <Area type="monotone" dataKey="absent" stroke="#ef4444" strokeWidth={2} fill="url(#gAbsent)" name="absent" />
                </AreaChart>
              </ResponsiveContainer>
            </div>
          </CardContent>
        </Card>

        {/* live feed */}
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-base flex items-center gap-2">
              ترددهای زنده
              <span className="h-2 w-2 rounded-full bg-emerald-500 animate-pulse" />
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="max-h-72 overflow-y-auto space-y-2">
              {liveFeed.length === 0 && (
                <p className="text-sm text-muted-foreground text-center py-8">
                  در انتظار ثبت تردد…<br />
                  <span className="text-xs">(با ثبت حضور از کیشی یا کارت من، اینجا زنده نمایش داده می‌شود)</span>
                </p>
              )}
              {liveFeed.map((f, i) => (
                <div key={i} className="flex items-center gap-2 p-2 rounded-lg bg-muted/60 text-sm animate-in fade-in slide-in-from-bottom-1">
                  <span className={`h-7 w-7 rounded-full flex items-center justify-center ${f.direction === 'IN' ? 'bg-emerald-500/15 text-emerald-600' : 'bg-red-500/15 text-red-600'}`}>
                    {f.direction === 'IN' ? <LogIn className="h-3.5 w-3.5" /> : <LogOut className="h-3.5 w-3.5" />}
                  </span>
                  <span className="flex-1 truncate font-medium">{f.personName}</span>
                  <span className="text-xs text-muted-foreground">{f.time}</span>
                  <Badge variant="secondary" className="text-[10px] px-1.5">
                    {f.source === 'KIOSK' ? 'کیشی' : f.source === 'QR' ? 'QR' : f.source === 'DEVICE' ? 'دستگاه' : 'وب'}
                  </Badge>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        {/* presence rate */}
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-base">نرخ حضور امروز</CardTitle>
          </CardHeader>
          <CardContent className="space-y-3">
            <div className="flex items-end gap-2">
              <span className="text-4xl font-bold text-primary">{faDigits(presenceRate)}٪</span>
              <span className="text-sm text-muted-foreground mb-1">از {faDigits(stats.totalPersons)} نفر</span>
            </div>
            <Progress value={presenceRate} className="h-3" />
            <div className="flex justify-between text-xs text-muted-foreground pt-1">
              <span>کارکرد جمع امروز: {minutesToHM(stats.workedMinutes)} ساعت</span>
            </div>
            <div className="pt-2 border-t space-y-2">
              <div className="text-xs font-semibold text-muted-foreground">بیشترین تأخیرها</div>
              {stats.lateList.length === 0 && <p className="text-xs text-muted-foreground">تأخیری ثبت نشده ✨</p>}
              {stats.lateList.map((l) => (
                <div key={l.name} className="flex justify-between text-sm">
                  <span className="truncate">{l.name}</span>
                  <span className="text-orange-600 font-medium shrink-0">{durationFa(l.minutes)}</span>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* overtime leaders */}
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-base">پیشتازان اضافه‌کاری امروز</CardTitle>
          </CardHeader>
          <CardContent className="space-y-2">
            {stats.otList.length === 0 && <p className="text-sm text-muted-foreground">اضافه‌کاری‌ای ثبت نشده</p>}
            {stats.otList.map((o, i) => (
              <div key={o.name} className="flex justify-between items-center text-sm p-2 rounded-lg bg-muted/40">
                <span className="flex items-center gap-2">
                  <span className={`h-6 w-6 rounded-full flex items-center justify-center text-xs font-bold ${i === 0 ? 'bg-amber-400/20 text-amber-600' : 'bg-muted'}`}>{faDigits(i + 1)}</span>
                  <span className="truncate">{o.name}</span>
                </span>
                <span className="text-teal-600 font-medium shrink-0">{durationFa(o.minutes)}</span>
              </div>
            ))}
          </CardContent>
        </Card>

        {/* dept distribution */}
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-base">حاضرین به تفکیک دپارتمان</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="h-52" dir="ltr">
              <ResponsiveContainer width="100%" height="100%">
                <BarChart data={stats.deptDist} layout="vertical">
                  <XAxis type="number" hide />
                  <YAxis type="category" dataKey="name" tick={{ fontSize: 10 }} width={100} orientation="right" />
                  <Tooltip contentStyle={{ direction: 'rtl', borderRadius: 8, fontSize: 12 }} formatter={(v: number) => [faDigits(v) + ' نفر', 'حاضر']} />
                  <Bar dataKey="value" fill="#10b981" radius={[0, 6, 6, 0]} barSize={16} />
                </BarChart>
              </ResponsiveContainer>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}

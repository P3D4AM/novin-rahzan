import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { createSession, destroySession, getSessionUser, verifyPassword, SESSION_COOKIE } from '@/lib/auth'
import { fail } from '@/lib/api'

export async function POST(req: NextRequest) {
  try {
    const { username, password } = await req.json()
    if (!username || !password) {
      return NextResponse.json({ error: 'نام کاربری و رمز عبور الزامی است' }, { status: 400 })
    }
    const user = await db.user.findUnique({ where: { username } })
    if (!user || !user.active || !verifyPassword(password, user.passwordHash)) {
      return NextResponse.json({ error: 'نام کاربری یا رمز عبور اشتباه است' }, { status: 401 })
    }
    const token = await createSession(user.id)
    const res = NextResponse.json({
      user: { id: user.id, username: user.username, role: user.role, fullName: user.fullName, personId: user.personnelId },
    })
    res.cookies.set(SESSION_COOKIE, token, { httpOnly: true, sameSite: 'lax', maxAge: 7 * 24 * 3600, path: '/' })
    return res
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    const token = req.cookies.get(SESSION_COOKIE)?.value
    if (token) await destroySession(token)
    const res = NextResponse.json({ ok: true })
    res.cookies.set(SESSION_COOKIE, '', { maxAge: 0, path: '/' })
    return res
  } catch (err) {
    return fail(err)
  }
}

export async function GET() {
  try {
    const user = await getSessionUser()
    if (!user) return NextResponse.json({ user: null }, { status: 200 })
    const person = user.personId
      ? await db.personnel.findUnique({ where: { id: user.personId }, select: { id: true, firstName: true, lastName: true, personnelCode: true, qrToken: true } })
      : null
    return NextResponse.json({ user, person })
  } catch (err) {
    return fail(err)
  }
}

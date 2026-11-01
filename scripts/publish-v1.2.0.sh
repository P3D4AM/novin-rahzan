#!/usr/bin/env bash
# Publish HozorPlus v1.2.0 — push develop + create GitHub release with the setup zip.
#
# Usage:   GH_TOKEN=github_pat_XXXX ./scripts/publish-v1.2.0.sh
# (run from the repo worktree that has the develop branch, e.g. /tmp/wt)
set -euo pipefail

REPO="P3D4AM/novin-rahzan"
BRANCH="develop"
TAG="v1.2.0"
ZIP="HozorPlus-v1.2.0-Windows-Setup.zip"
API="https://api.github.com/repos/$REPO"

if [ -z "${GH_TOKEN:-}" ]; then
  echo "✗ GH_TOKEN is not set — pass it as: GH_TOKEN=github_pat_... $0"
  exit 1
fi
AUTH="Authorization: Bearer $GH_TOKEN"

# 1) push develop
echo "▸ pushing $BRANCH …"
git push "https://x-access-token:${GH_TOKEN}@github.com/${REPO}.git" "$BRANCH"

# 2) create the release (body from release-body-v1.2.0.md)
echo "▸ creating release $TAG …"
python3 - "$API" "$TAG" "$BRANCH" << 'PYEOF'
import json, os, sys, urllib.request
api, tag, branch = sys.argv[1], sys.argv[2], sys.argv[3]
with open('scripts/release-body-v1.2.0.md', encoding='utf-8') as f:
    body = f.read()
payload = {
    "tag_name": tag,
    "target_commitish": branch,
    "name": "v1.2.0 — تست اتصال واقعی + دانلود ترددهای قبلی از دستگاه NP756+ (پروتکل SBXPC)",
    "body": body,
    "draft": False,
    "prerelease": False,
}
req = urllib.request.Request(
    api + "/releases",
    data=json.dumps(payload).encode(),
    headers={
        "Authorization": "Bearer " + os.environ["GH_TOKEN"],
        "Accept": "application/vnd.github+json",
        "Content-Type": "application/json",
    },
    method="POST",
)
try:
    with urllib.request.urlopen(req) as r:
        rel = json.load(r)
        print("release id:", rel["id"], "| url:", rel["html_url"])
        with open("scripts/release-id.txt", "w") as f:
            f.write(f"release_id: {rel['id']}\nhtml_url: {rel['html_url']}\nupload_url: {rel['upload_url']}\n")
except urllib.error.HTTPError as e:
    if e.code == 422:  # already exists — fetch it
        req2 = urllib.request.Request(api + "/releases/tags/" + tag,
                                      headers={"Authorization": "Bearer " + os.environ["GH_TOKEN"]})
        with urllib.request.urlopen(req2) as r:
            rel = json.load(r)
        print("release exists:", rel["id"])
        with open("scripts/release-id.txt", "w") as f:
            f.write(f"release_id: {rel['id']}\nhtml_url: {rel['html_url']}\nupload_url: {rel['upload_url']}\n")
    else:
        raise
PYEOF

# 3) upload the asset
REL_ID=$(sed -n 's/release_id: //p' scripts/release-id.txt | tr -d ' \n')
echo "▸ uploading $ZIP to release $REL_ID …"
curl -sS -X POST \
  -H "$AUTH" -H "Content-Type: application/zip" \
  --data-binary "@../$ZIP" \
  "$API/releases/$REL_ID/assets?name=$ZIP" | head -c 400
echo
echo "✓ done — https://github.com/$REPO/releases/tag/$TAG"

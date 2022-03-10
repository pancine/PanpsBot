#!/usr/bin/env bash
set -Eeuo pipefail;

CONTENT="{\"username\":\"EC2\", \"content\":\""$1"\"}"

curl --request POST \
    --header "Content-Type: application/json" \
    --progress-bar \
    --data "$CONTENT" \
    https://discord.com/api/webhooks/$WEBHOOK_ID/$WEBHOOK_TOKEN
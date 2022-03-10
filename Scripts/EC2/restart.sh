#!/usr/bin/env bash
set -Eeuo pipefail;

if [ "$(sudo docker container list --quiet --filter name="panps-bot")" ];
then
    discord_webhook "[BEGIN] Stopping container"
    sudo docker container stop panps-bot --time 0
    discord_webhook "[END]"

    discord_webhook "[BEGIN] Started cleaning docker containers, images, cache, etc"
    sudo docker container rm $(sudo docker container list --quiet --all)
    sudo docker image rm pancine/panps-bot
    sudo docker system prune --force --all
    discord_webhook "[END]"
fi

discord_webhook "[BEGIN] Pulling new image"
sudo docker pull pancine/panps-bot
discord_webhook "[END]"

discord_webhook "[BEGIN] Started container"
sudo docker container run -d --name panps-bot pancine/panps-bot
discord_webhook "[END]"
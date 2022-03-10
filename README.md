# PanpsBot 🤖
## Description
PanpsBot is a project that I started to work on at my beginning .NET days and I used to learn new technologies and libraries.
It's a Discord Bot client running on an Amazon EC2 machine under a Docker container.
It has a few functionalities that use some libraries that I had to work on in real job projects.

---

## Technologies used
### .NET 6.0
The project was made using .NET Core 3.1, but I updated it to use .NET 6.0

### Amazon EC2
Discord Bot was running inside an Amazon EC2 instance using Docker.
The deployment is done in **[push-to-ec2 GitHub Action](./.github/workflows/push-to-ec2.yml)** which builds the docker image and then run the **[restart script](./Scripts/EC2/restart.sh)** on the EC2 instance

### Azure Functions
- SpotifyAccessToken: Used to give PanpsBot API access to a specific user.
- ClearCacheDaily: Clear Redis databases daily

### Docker
Used to create the PanpsBot image used in Amazon EC2 **[(Dockerfile)](./Dockerfile)**

### PostgreSql
Used to store Spotify access tokens related to a discord user

Tables:
- **[spotify_user](./Scripts/SQL/CREATE_spotify_user_TABLE.sql)**

### Redis
Used to store the state parameter used on the spotify authentication process

---

## Packages used
- Dapper
- Discord.Net
- Npgsql
- Serilog
- StackExchange.Redis
- SpotifyAPI.Web

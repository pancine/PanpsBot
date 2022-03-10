using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PanpsBot.Models.Entities;

namespace PanpsBot.Infrastructure;

public class SpotifyUserRepository : ISpotifyUserRepository
{
    private readonly string _connectionString;

    public SpotifyUserRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Npgsql");
    }

    public async Task AddSpotifyUserAsync(SpotifyUser spotifyUser)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query =
            "INSERT INTO spotify_user (discord_id, code, access_token, refresh_token, date_added, expires_in) " +
            "VALUES (@discordId, @code, @accessToken, @refreshToken, @dateAdded, @expiresIn)";

        await connection.ExecuteAsync(query, spotifyUser);
    }

    public async Task<SpotifyUser> GetSpotifyUserAsync(string discordId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        const string query = "SELECT " +
            "   discord_id as DiscordId, " +
            "   code as Code," +
            "   access_token as AccessToken, " +
            "   refresh_token as RefreshToken, " +
            "   date_added as DateAdded, " +
            "   expires_in as ExpiresIn " +
            "FROM spotify_user " +
            "WHERE discord_id = @discordId";

        return await connection.QuerySingleOrDefaultAsync<SpotifyUser>(query, new { DiscordId = discordId });
    }

    public async Task UpdateSpotifyUserAsync(SpotifyUser spotifyUser)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        
        const string query =
                "UPDATE spotify_user " +
                "SET access_token = @accessToken, " +
                "    expires_in = @expiresIn " +
                "WHERE discord_id = @discordId";

        await connection.QueryAsync(query, spotifyUser, commandType: CommandType.Text);
    }
}

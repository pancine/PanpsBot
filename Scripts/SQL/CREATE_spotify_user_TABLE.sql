CREATE TABLE spotify_user (
  discord_id CHAR(18) NOT NULL CONSTRAINT table_name_pk PRIMARY KEY,
  code VARCHAR(255) NOT NULL,
  access_token VARCHAR(255),
  refresh_token VARCHAR(255),
  date_added CHAR(19) NOT NULL,
  expires_in CHAR(19)
);
CREATE UNIQUE INDEX table_name_discord_id_uindex ON spotify_user (discord_id);
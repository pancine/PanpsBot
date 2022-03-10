namespace PanpsBot.Models.Exceptions;

[Serializable]
public class SpotifyUserNotFoundException : Exception
{
    public SpotifyUserNotFoundException() { }
    public SpotifyUserNotFoundException(string message) : base(message) { }
    public SpotifyUserNotFoundException(string message, Exception inner) : base(message, inner) { }
    protected SpotifyUserNotFoundException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

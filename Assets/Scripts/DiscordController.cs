using UnityEngine;

[System.Serializable]
public class DiscordJoinEvent : UnityEngine.Events.UnityEvent<string>
{ }

[System.Serializable]
public class DiscordSpectateEvent : UnityEngine.Events.UnityEvent<string>
{ }

[System.Serializable]
public class DiscordJoinRequestEvent : UnityEngine.Events.UnityEvent<DiscordRpc.DiscordUser>
{ }

public class DiscordController : MonoBehaviour
{
    public DiscordRpc.RichPresence presence = new();
    public string applicationId;
    public string optionalSteamId;
    public UnityEngine.Events.UnityEvent onConnect;
    private DiscordRpc.EventHandlers handlers;
    public string details; /* max 128 bytes */
    public string state; /* max 128 bytes */
    public long startTimestamp;
    public string largeImageKey; /* max 32 bytes */
    public string largeImageText; /* max 128 bytes */
    public string smallImageKey; /* max 32 bytes */
    public string smallImageText; /* max 128 bytes */
    public int partySize;
    public int partyMax;

    public void PresenceData()
    {
        presence.state = state;
        presence.details = details;
        presence.startTimestamp = System.DateTimeOffset.Now.ToUnixTimeSeconds();
        presence.largeImageKey = largeImageKey;
        presence.largeImageText = largeImageText;
        presence.smallImageKey = smallImageKey;
        presence.smallImageText = smallImageText;
        presence.partyId = "01";
        presence.partySize = partySize;
        presence.partyMax = partyMax;

        DiscordRpc.UpdatePresence(presence);
    }

    public void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser)
    {
        Debug.Log(string.Format("Discord: connected to {0}#{1}: {2}", connectedUser.username, connectedUser.discriminator, connectedUser.userId));
        onConnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
    }

    private void Update()
    {
        DiscordRpc.RunCallbacks();
    }

    private void OnEnable()
    {
        Debug.Log("Discord: init");
        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback += ReadyCallback;
        handlers.errorCallback += ErrorCallback;
        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
    }

    private void OnDisable()
    {
        Debug.Log("Discord: shutdown");
        DiscordRpc.Shutdown();
    }
}
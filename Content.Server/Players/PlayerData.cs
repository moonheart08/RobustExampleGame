using Robust.Server.Interfaces.Player;
using Robust.Shared.Network;

namespace Content.Server.Players
{
    public sealed class PlayerData
    {

        public NetUserId UserId { get; }

        public PlayerData(NetUserId userId)
        {
            UserId = userId;
        }
    }

    public static class PlayerDataExt
    {

        public static PlayerData? ContentData(this IPlayerData data)
        {
            return (PlayerData?) data.ContentDataUncast;
        }

        public static PlayerData? ContentData(this IPlayerSession session)
        {
            return session.Data.ContentData();
        }
    }
}

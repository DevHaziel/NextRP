using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;

namespace NextRP.Commands.Player
{
    public class Pay : IScript
    {
        public Pay()
        {
            CommandHandler.RegisterCommand("pay", new Action<ShPlayer, ShPlayer, int>(Command), null, "pay");
        }

        public void Command(ShPlayer player, ShPlayer target, int count)
        {
            if (player.svPlayer.bankBalance < count)
            {
                player.svPlayer.SendGameMessage("&4[Err] &fYou don't have enoght money in your bank.");
                return;
            }

            player.svPlayer.bankBalance -= count;
            target.svPlayer.bankBalance += count;

            player.svPlayer.SendGameMessage(string.Format("&2[Bank] &fMoney transfered correctly to {0}", target.username));
            target.svPlayer.SendGameMessage(string.Format("&2[Bank] &fYou reseved money from {0}", player.username));
        }
    }
}

using BrokeProtocol.API;
using BrokeProtocol.Entities;
using Newtonsoft.Json;
using NextRP.Models.Player.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextRP.Commands.Player.Identity
{
    public class IdentityFunctions : IScript
    {
        public IdentityFunctions() 
        {
            CommandHandler.RegisterCommand("dni", new Action<ShPlayer, ShPlayer>(ShowIdentity), null, "ShowIdentity");
        }

        public void ShowIdentity(ShPlayer player, ShPlayer target = null)
        {
            target = target ?? player;

            bool hasIdentity = target.svPlayer.CustomData.TryFetchCustomData("IdentityDocument", out string IdentityJson);

            if(!hasIdentity)
            {
                player.svPlayer.SendGameMessage("&4[Err] &fYou aren't registered as NextRP citizen.");
                return;
            }

            IdentityDocument document = JsonConvert.DeserializeObject<IdentityDocument>(IdentityJson);

            target.svPlayer.VisualElementDisplay("ContainerCard", true);

            target.svPlayer.SetTextElementText("id_name", document.Name);
            target.svPlayer.SetTextElementText("id_lastname", document.Lastname);
            target.svPlayer.SetTextElementText("id_document", document.Id.ToString());
            target.svPlayer.SetTextElementText("id_age", document.Age);
            target.svPlayer.SetTextElementText("id_sex", document.Sex);

            target.StartCoroutine(closeIdentity(target));
        }

        public IEnumerator closeIdentity(ShPlayer player)
        {
            yield return new WaitForSeconds(10);

            player.svPlayer.VisualElementDisplay("ContainerCard", false);
        }
    }
}

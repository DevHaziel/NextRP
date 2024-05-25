using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using Newtonsoft.Json;
using NextRP.Models.Player.Identity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextRP
{
    public class CPlayerEvents : PlayerEvents
    {
        private Dictionary<string, IdentityDocument> temporaryData = new Dictionary<string, IdentityDocument>();
        private readonly List<string> sexDropdown = new List<string>() { "Masculino", "Femenino" };

        [CustomTarget]
        public void CloseIdentityForm(ShPlayer player, string element)
        {
            player.svPlayer.VisualElementDisplay("ContainerIdentityForm", false);
        }

        [CustomTarget]
        public void OnStartCreatingIdentityDocument(ShEntity entity, ShPlayer player)
        {
            if(player.svPlayer.CustomData.FetchCustomData<string>("IdentityDocument") != null)
            {
                player.svPlayer.SendGameMessage("&4[Err] &fYou already have an identity document.");
                return;
            }
            player.svPlayer.VisualElementDisplay("ContainerIdentityForm", true);
            player.svPlayer.AddButtonClickedEvent("sendIdentity", "OnSendIdentityForm");
            player.svPlayer.AddButtonClickedEvent("closeIdentity", "CloseIdentityForm");
            player.svPlayer.SetDropdownFieldChoices("gender", sexDropdown);
            player.svPlayer.CursorVisibility(true);
        }

        [CustomTarget]
        public void OnSendIdentityForm(ShPlayer player, string element)
        {
            temporaryData[player.username] = new IdentityDocument();

            foreach (var field in new string[] { "name", "lastname", "age" }) player.svPlayer.GetTextFieldText(field, "OnIdentityRegisterGetField");
           
            player.svPlayer.GetDropdownFieldValue("gender", "OnIdentityRegisterGetDropdown");
            player.svPlayer.VisualElementDisplay("ContainerIdentityForm", false);
            player.svPlayer.CursorVisibility(false);
            player.svPlayer.StartCoroutine(CheckSubmit(player));
        }

        private IEnumerator CheckSubmit(ShPlayer player)
        {
            yield return new WaitForSeconds(1f);
            temporaryData[player.username].Id = Core.rnd.Next(10000, 99999);
            if (temporaryData.TryGetValue(player.username, out IdentityDocument dniData) && dniData.VerifyData())
            {
                player.svPlayer.CustomData.AddOrUpdate<string>("IdentityDocument", JsonConvert.SerializeObject(dniData));
                player.svPlayer.SendGameMessage("&2[CITY] &fIdentity register success.");
            }
            else if (temporaryData.ContainsKey(player.username))
            {
                player.svPlayer.SendGameMessage("&4[Err] &fError on identity register, try again.");
            }

            temporaryData.Remove(player.username);
        }

        [CustomTarget]
        public void OnIdentityRegisterGetField(ShPlayer player, string element, string text)
        {
            switch (element)
            {
                case "name":
                    temporaryData[player.username].Name = text;
                    break;
                case "lastname":
                    temporaryData[player.username].Lastname = text;
                    break;
                case "age":
                    temporaryData[player.username].Age = text;
                    break;
            }
        }

        [CustomTarget]
        public void OnIdentityRegisterGetDropdown(ShPlayer player, string element, int index)
        {
            switch (element)
            {
                case "gender":
                    temporaryData[player.username].Sex = sexDropdown[index];
                    break;
            }

        }

        [Execution(ExecutionMode.Additive)]
        public override bool Spawn(ShEntity entity)
        {
            if (!string.IsNullOrEmpty(entity.data) && entity.Player)
            {
                if (entity.data == "IdentityNPC") entity.Player.svPlayer.SvAddDynamicAction("OnStartCreatingIdentityDocument", "Registrarse");
            }

            if (entity.isHuman)
            {
                ShPlayer player = (ShPlayer)entity;
                player.svPlayer.VisualTreeAssetClone("DNI");
            }

            return true;

            
        }
    }
}

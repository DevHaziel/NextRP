using BrokeProtocol.API;
using BrokeProtocol.Entities;
using Newtonsoft.Json;
using NextRP.Models.Player.Identity;
using System;

namespace NextRP.Commands.Player.Identity.Admin
{
    public class DocumentManager : IScript
    {
        public DocumentManager()
        {
            //CommandHandler.RegisterCommand("createdocument", new Action<ShPlayer, ShPlayer, string, string, int>(CreateDocument), null, "IdentityManager");
            CommandHandler.RegisterCommand("deletedocument", new Action<ShPlayer, ShPlayer>(DeleteDocument), null, "IdentityManager");
        }

        /*
        public void CreateDocument(ShPlayer player, ShPlayer target, string name, string lastname, int age)
        {
            bool documentExists = target.svPlayer.CustomData.TryFetchCustomData("IdentityDocument", out string IdentityJson);

            if(documentExists)
            {
                player.svPlayer.SendGameMessage(string.Format("&4[Err] &fThe player {0} already have an identity document.", target.username));
                return;
            }

            IdentityDocument doc = new IdentityDocument()
            {
                Id = Core.rnd.Next(1000, 9999),
                Name = name,
                Lastname = lastname,
                Age = age.ToString()
            };

            string docJson = JsonConvert.SerializeObject(doc, Formatting.Indented);

            target.svPlayer.CustomData["IdentityDocument"] = docJson;
            player.svPlayer.SendGameMessage(string.Format("&2[CITY] &f{0} {1} registered as citizen of NextRP correctly", name, lastname));
        }
        */
        public void DeleteDocument(ShPlayer player, ShPlayer target)
        {
            bool documentExists = target.svPlayer.CustomData.TryFetchCustomData("IdentityDocument", out string IdentityJson);
            
            if (!documentExists)
            {
                player.svPlayer.SendGameMessage(string.Format("&4[Err] &fThe player {0} don't have an identity document.", target.username));
                return;
            }

            IdentityDocument doc = JsonConvert.DeserializeObject<IdentityDocument>(IdentityJson);

            target.svPlayer.CustomData.TryRemoveCustomData("IdentityDocument");
            player.svPlayer.SendGameMessage(string.Format("&2[CITY] &f{0} {1} unregistered as citizen of NextRP correctly", doc.Name, doc.Lastname));
        }
    }
}

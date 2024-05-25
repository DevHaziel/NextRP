using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Threading.Tasks;

namespace NextRP.Models.Player.Identity
{
    public class IdentityDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }

        public bool VerifyData()
        {
            Regex validNameRegex = new Regex(@"^[a-zA-Z\s'-]+$");

            Name = Name.CapitalizeFirstLetter();
            Lastname = Lastname.CapitalizeFirstLetter();

            return !string.IsNullOrEmpty(Name) && validNameRegex.IsMatch(Name)
                && !string.IsNullOrEmpty(Lastname) && validNameRegex.IsMatch(Lastname)
                && int.TryParse(Age.ToString(), out _); 
        }
    }
}

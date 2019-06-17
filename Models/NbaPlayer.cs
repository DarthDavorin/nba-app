using System.ComponentModel.DataAnnotations.Schema;

namespace NbaApi.Models
{
    public class NbaPlayer
    {   
        // ID igrača
        public long Id { get; set; }

        // Broj na dresu
        public int Number { get; set; }

        // Ime
        public string Firstname { get; set; }

        // Prezime
        public string Lastname { get; set; }

        // Broj odigranih utakmica(Games Played)
        public int GP { get; set; }

        // Prosjek odigranih minuta po utakmici(Minutes)
        public float MIN { get; set; }

        // Prosječni broj postignutih bodova po utakmici(Points)
        public float PTS { get; set; }

        // Prosječni broj šuteva iz igre(Field Goals Attempted)
        public float FGA { get; set; }

        // Prosječni broj ubačenih šuteva iz igre(Field Goals Made)
        public float FGM { get; set; }
        
        // ID kluba u kojem igrač igra
        public long ClubId { get; set; } 

        // Stvaranje veze između Club modela i Player modela
        [ForeignKey("ClubId")]
        public NbaClub Club { get; set; }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NbaApi.Models
{
    public class NbaClub
    {
        // ID kluba
        public long Id { get; set; }

        // Ime kluba
        public string ClubName { get; set; }

        // Grad iz kojega je klub
        public string ClubCity { get; set; }

        // Broj odigranih utakmica kluba
        public int GP { get; set; }

        // Prosječni broj minuta koje igrači odigraju po utakmici
        public float MIN { get; set; }

        // Prosječni broj poena koje igrači postignu po utakmici
        public float PTS { get; set; }

        // Prosječni broj pokušaja šuta iz igre koje igrači naprave po utakmici
        public float FGA { get; set; }

        // Prosječni broj ubačenih šuteva iz igre koji igrači naprave po utakmici
        public float FGM { get; set; }

        // Stvaranje veze između kluba i igrača
        [InverseProperty("Club")]
        public List<NbaPlayer> Players { get; set; }
    }
}
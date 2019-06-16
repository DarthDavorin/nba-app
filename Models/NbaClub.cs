using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NbaApi.Models
{
    public class NbaClub
    {
        public long Id { get; set; }

        public string ClubName { get; set; }

        public string ClubCity { get; set; }

        public int GP { get; set; }

        public float MIN { get; set; }

        public float PTS { get; set; }

        public float FGM { get; set; }

        public float FGA { get; set; }

        [InverseProperty("Club")]
        public List<NbaPlayer> Players { get; set; }
    }
}
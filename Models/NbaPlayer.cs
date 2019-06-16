using System.ComponentModel.DataAnnotations.Schema;

namespace NbaApi.Models
{
    public class NbaPlayer
    {
        public long Id { get; set; }

        public int Number { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public int GP { get; set; }

        public float MIN { get; set; }

        public float PTS { get; set; }

        public float FGM { get; set; }

        public float FGA { get; set; }

        public long ClubId { get; set; } 

        [ForeignKey("ClubId")]
        public NbaClub Club { get; set; }
    }
}
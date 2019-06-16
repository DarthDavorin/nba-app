using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NbaApi.Models;
using System;
using Microsoft.AspNetCore.HttpsPolicy;

namespace NbaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class NbaController : ControllerBase
    {
        private readonly NbaContext _context;

        public NbaController(NbaContext context)
        {
            _context = context;
        }
    
        [HttpGet("Clubs")]

        public ActionResult<IEnumerable<NbaClub>> GetNbaClubs()
        {
            return _context.NbaClubs.Include(c => c.Players).ToList();
        }


        [HttpGet("Clubs/{idClub}")]

        public ActionResult<NbaClub> GetNbaClub(long idClub)
        {
            var query = _context.NbaClubs
                .Include(c => c.Players)
                .Where(c => c.Id == idClub);
                    
            var nbaClub = query.FirstOrDefault();
                  

            if (nbaClub == null)
            {
                return NotFound();
            }

            return nbaClub;
        }

        [HttpGet("Players/{idPlayer}")]

        public async Task<ActionResult<NbaPlayer>> GetNbaPlayer(long idPlayer)
        {
            var query = _context.NbaPlayers
                .Include(p => p.Club)
                .Where(p => p.Id == idPlayer);


            var nbaPlayer = await query.FirstOrDefaultAsync();


            if (nbaPlayer == null)
            {
                return NotFound();
            }

            return nbaPlayer;
        }


        [HttpPost("Clubs")]
        public async Task<ActionResult<NbaClub>> PostNbaClub(NbaClub club)
        {
            _context.NbaClubs.Add(club);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNbaClub), new { idClub = club.Id }, club);
        }

        [HttpPost("Players")]

        public async Task<ActionResult<NbaPlayer>> PostNbaPlayer(NbaPlayer player)
        {        
            _context.NbaPlayers.Add(player);

            await _context.SaveChangesAsync();

            var club = _context.NbaClubs
                .Include(c => c.Players)
                .Where(c => player.ClubId == c.Id)
                .FirstOrDefault();

            float fgmZbroj = 0, fgaZbroj = 0, minZbroj = 0, ptsZbroj = 0;


            foreach(NbaPlayer p in club.Players)
            {
                fgmZbroj += p.FGM;
                fgaZbroj += p.FGA;
                minZbroj += p.MIN;
                ptsZbroj += p.PTS;
            }

           int brojIgraca = club.Players.Count;

           club.FGM = fgmZbroj / brojIgraca;
           club.FGA = fgaZbroj / brojIgraca;
           club.MIN = minZbroj / brojIgraca;
           club.PTS = ptsZbroj / brojIgraca;

            _context.NbaClubs.Update(club);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNbaPlayer), new { idPlayer = player.Id }, player);
        }

        [HttpDelete("Clubs/{idClub}")]

        public async Task<IActionResult> DeleteClub(long idClub)
        {
            var nbaClub = await _context.NbaClubs.FindAsync(idClub);

            if(nbaClub == null)
            {
                return NotFound();
            }

            _context.NbaClubs.Remove(nbaClub);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("Players/{idPlayer}")]

        public async Task<IActionResult> DeletePlayer(long idPlayer)
        {
            var player = await _context.NbaPlayers.FindAsync(idPlayer);

            if(player == null)
            {
                return NotFound();
            }

            var club = _context.NbaClubs
                .Include(c => c.Players)
                .Where(c => player.ClubId == c.Id)
                .FirstOrDefault();

            
            _context.NbaPlayers.Remove(player);
            await _context.SaveChangesAsync();

            float fgmZbroj = 0, fgaZbroj = 0, minZbroj = 0, ptsZbroj = 0;
            int brojIgraca = club.Players.Count;

            foreach(NbaPlayer p in club.Players)
            {
                fgmZbroj += p.FGM;
                fgaZbroj += p.FGA;
                minZbroj += p.MIN;
                ptsZbroj += p.PTS;
            }

            if(brojIgraca == 0)
            {
                club.FGA = 0;
                club.FGM = 0;
                club.MIN = 0;
                club.PTS = 0;
            }
            else
            {
            club.FGM = fgmZbroj / brojIgraca;
            club.FGA = fgaZbroj / brojIgraca;
            club.MIN = minZbroj / brojIgraca;
            club.PTS = ptsZbroj / brojIgraca;
            }

            _context.NbaClubs.Update(club);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("Players/{idPlayer}")]

        public async Task<IActionResult> PutNbaPlayer(long idPlayer, NbaPlayer player)
        {
            if (idPlayer != player.Id)
            {
               return BadRequest();
            }

            var igrac = await _context.NbaPlayers.FindAsync(idPlayer);

            if(igrac == null)
            {
                return NotFound();
            }

            igrac.FGA = player.FGA;
            igrac.FGM = player.FGM;
            igrac.GP = player.GP;
            igrac.PTS = player.PTS;
            igrac.MIN = player.MIN;
            igrac.Number = player.Number;
                        
            _context.Entry(igrac).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var klub = await _context.NbaClubs
                        .Include(k => k.Players)
                        .Where(k => igrac.ClubId == k.Id)
                        .FirstOrDefaultAsync();

            float fgmZbroj = 0, fgaZbroj = 0, minZbroj = 0, ptsZbroj = 0;


            foreach(NbaPlayer p in klub.Players)
            {
                fgmZbroj += p.FGM;
                fgaZbroj += p.FGA;
                minZbroj += p.MIN;
                ptsZbroj += p.PTS;
            }

           int brojIgraca = klub.Players.Count;

           klub.FGM = fgmZbroj / brojIgraca;
           klub.FGA = fgaZbroj / brojIgraca;
           klub.MIN = minZbroj / brojIgraca;
           klub.PTS = ptsZbroj / brojIgraca;

           _context.NbaClubs.Update(klub);
           await _context.SaveChangesAsync();            

            return NoContent();
        }

        [HttpPut("Clubs/{idClub}")]

        public async Task<IActionResult> PutNbaClub(long idClub, NbaClub Club)
        {
            if(idClub != Club.Id)
            {
                return BadRequest();
            }

            var klub = await _context.NbaClubs.FindAsync(idClub);

            if(klub == null)
            {
                return NotFound(); 
            }

            klub.ClubName = Club.ClubName;
            klub.ClubCity = Club.ClubCity;

            _context.Entry(klub).State = EntityState.Modified;

           await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
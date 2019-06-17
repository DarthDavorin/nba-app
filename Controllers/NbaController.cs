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
    // NBA API controller
    [Route("api/[controller]")]
    [ApiController]

    public class NbaController : ControllerBase
    {
        // Context za komunikaciju s bazom
        private readonly NbaContext _context;

        // Konstruktor
        public NbaController(NbaContext context)
        {   
            // Postavljanje podataka
            _context = context;
        }

        // Dohvaćanje svih NBA klubova
        [HttpGet("Clubs")]

        public ActionResult<IEnumerable<NbaClub>> GetNbaClubs()
        {
            // Vraćanje svih igrača u obliku liste
            return _context.NbaClubs.Include(c => c.Players).ToList();
        }

        // Dohvaćanje jednog NBA kluba
        [HttpGet("Clubs/{idClub}")]

        public ActionResult<NbaClub> GetNbaClub(long idClub)
        {
            // Stvaranje upita
            var query = _context.NbaClubs
                .Include(c => c.Players)
                .Where(c => c.Id == idClub);

            // Vraća prvi klub iz upita        
            var nbaClub = query.FirstOrDefault();
                  
            // Ako je null - vrati Not Found
            if (nbaClub == null)
            {
                // Not found
                return NotFound();
            }

            // Vraća NBA klub
            return nbaClub;
        }


        // Dohvaćanje jednog igrača
        [HttpGet("Players/{idPlayer}")]

        public async Task<ActionResult<NbaPlayer>> GetNbaPlayer(long idPlayer)
        {
            // Stvaranje upita
            var query = _context.NbaPlayers
                .Include(p => p.Club)
                .Where(p => p.Id == idPlayer);

            // Vraća prvog igrača iz upita
            var nbaPlayer = await query.FirstOrDefaultAsync();

            // Ako je null - vrati Not Found
            if (nbaPlayer == null)
            {
                // Not Found
                return NotFound();
            }

            // Vraća igrača
            return nbaPlayer;
        }


        // Slanje NBA kluba
        [HttpPost("Clubs")]
        public async Task<ActionResult<NbaClub>> PostNbaClub(NbaClub club)
        {
            // Dodavanje kluba u context i spremanje promjena
            _context.NbaClubs.Add(club);
            await _context.SaveChangesAsync();

            // Vraćanje status 201(Created) responsea
            return CreatedAtAction(nameof(GetNbaClub), new { idClub = club.Id }, club);
        }

        // Slanje NBA igrača
        [HttpPost("Players")]

        public async Task<ActionResult<NbaPlayer>> PostNbaPlayer(NbaPlayer player)
        {   
            // Dodavanje igrača u context i spremanje promjena
            _context.NbaPlayers.Add(player);
            await _context.SaveChangesAsync();

            // Stvaranje upita i dohvaćanje prvog
            var club = _context.NbaClubs
                .Include(c => c.Players)
                .Where(c => player.ClubId == c.Id)
                .FirstOrDefault();


            // Ažuriranje statistike kluba nakon dodavanja igrača

            // Zbrajanje FGM-a, FGA-a, MIN-a i PTS-a svakog igrača u klubu
            float fgmZbroj = 0, fgaZbroj = 0, minZbroj = 0, ptsZbroj = 0;

            foreach(NbaPlayer p in club.Players)
            {
                fgmZbroj += p.FGM;
                fgaZbroj += p.FGA;
                minZbroj += p.MIN;
                ptsZbroj += p.PTS;
            }

           // Prebrojavanje igrača u klubu 
           int brojIgraca = club.Players.Count;

           // Spremanje nove statistike u property-je kluba nakon dodavanja igrača
           club.FGM = fgmZbroj / brojIgraca;
           club.FGA = fgaZbroj / brojIgraca;
           club.MIN = minZbroj / brojIgraca;
           club.PTS = ptsZbroj / brojIgraca;

            // Ažuriranje promjena
            _context.NbaClubs.Update(club);

            // Spremanje promjena
            await _context.SaveChangesAsync();

            // Vraćanje status 201(Created) response-a
            return CreatedAtAction(nameof(GetNbaPlayer), new { idPlayer = player.Id }, player);
        }

        // Brisanje jednog NBA kluba
        [HttpDelete("Clubs/{idClub}")]

        public async Task<IActionResult> DeleteClub(long idClub)
        {
            // Traženje kluba preko ID-a
            var nbaClub = await _context.NbaClubs.FindAsync(idClub);

            // Ako je null - vrati Not Found
            if(nbaClub == null)
            {   
                // Not Found
                return NotFound();
            }

            // Brisanje kluba iz baze podataka i spremanje promjena
            _context.NbaClubs.Remove(nbaClub);
            await _context.SaveChangesAsync();

            // Vraćanje status 204(No Content) responsea
            return NoContent();
        }

        // Brisanje jednog igrača
        [HttpDelete("Players/{idPlayer}")]

        public async Task<IActionResult> DeletePlayer(long idPlayer)
        {
            // Traženje igrača preko ID-a
            var player = await _context.NbaPlayers.FindAsync(idPlayer);

            // Ako je null - vrati Not Found
            if(player == null)
            {
                // Not Found
                return NotFound();
            }

            // Stvaranje upita 
            var club = _context.NbaClubs
                .Include(c => c.Players)
                .Where(c => player.ClubId == c.Id)
                .FirstOrDefault();

            // Brisanje igrača iz baze podataka i spremanje promjena
            _context.NbaPlayers.Remove(player);
            await _context.SaveChangesAsync();


            // Ažuriranje statistike kluba nakon što je igrač izbrisan

            float fgmZbroj = 0, fgaZbroj = 0, minZbroj = 0, ptsZbroj = 0;
            // Prebrojavanje igrača u klubu
            int brojIgraca = club.Players.Count;

            foreach(NbaPlayer p in club.Players)
            {
                fgmZbroj += p.FGM;
                fgaZbroj += p.FGA;
                minZbroj += p.MIN;
                ptsZbroj += p.PTS;
            }

            // Ako nema igrača u klubu - podaci su jednaki nuli
            if(brojIgraca == 0)
            {
                club.FGA = 0;
                club.FGM = 0;
                club.MIN = 0;
                club.PTS = 0;
            }
            // Ponovno izračunavanje statistike kluba s ispravnim brojem igrača
            else
            {
            club.FGM = fgmZbroj / brojIgraca;
            club.FGA = fgaZbroj / brojIgraca;
            club.MIN = minZbroj / brojIgraca;
            club.PTS = ptsZbroj / brojIgraca;
            }

            // Ažuriranje kluba u bazi podataka i spremanje promjena
            _context.NbaClubs.Update(club);
            await _context.SaveChangesAsync();
            
            // Vraćanje status 204(No Content) response-a
            return NoContent();
        }


        // Mijenjanje postojućeg igrača
        [HttpPut("Players/{idPlayer}")]

        public async Task<IActionResult> PutNbaPlayer(long idPlayer, NbaPlayer player)
        {   
            // Ako se ID-jevi ne podudaraju - vrati Bad Request
            if (idPlayer != player.Id)
            {   
               // Vraćanje status 400(Bad Request) response-a 
               return BadRequest();
            }

            // Traženje igrača preko ID-a
            var igrac = await _context.NbaPlayers.FindAsync(idPlayer);

            // Ako je null - vrati Not Found
            if(igrac == null)
            {   
                // Vraćanje status 404(Not Found) response-a
                return NotFound();
            }

            // Spremanje izmjena u property-je igrača
            igrac.FGA = player.FGA;
            igrac.FGM = player.FGM;
            igrac.GP = player.GP;
            igrac.PTS = player.PTS;
            igrac.MIN = player.MIN;
            igrac.Number = player.Number;
                        
            // Označavanje entiteta promjenjenim i spremanje promjena 
            _context.Entry(igrac).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Stvaranje upita
            var klub = await _context.NbaClubs
                        .Include(k => k.Players)
                        .Where(k => igrac.ClubId == k.Id)
                        .FirstOrDefaultAsync();

            float fgmZbroj = 0, fgaZbroj = 0, minZbroj = 0, ptsZbroj = 0;

            // Ažuriranje statistike kluba nakon promjene statistike igrača

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

            // Ažuriranje kluba i spremanje promjena
           _context.NbaClubs.Update(klub);
           await _context.SaveChangesAsync();            

            // Vraćanje status 204(No Content) response-a
            return NoContent();
        }

        // Mijenjanje postojućeg kluba
        [HttpPut("Clubs/{idClub}")]

        public async Task<IActionResult> PutNbaClub(long idClub, NbaClub Club)
        {   
            // Ako se ID-jevi ne podudaraju - vrati Bad Request
            if(idClub != Club.Id)
            {
                return BadRequest();
            }

            // Traženje kluba
            var klub = await _context.NbaClubs.FindAsync(idClub);

            // Ako je null - vrati Not Found
            if(klub == null)
            {
                return NotFound(); 
            }

            // Spremanje promjena u property-je
            klub.ClubName = Club.ClubName;
            klub.ClubCity = Club.ClubCity;

            // Označavanje entiteta promjenjenim i spremanje promjena
            _context.Entry(klub).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Vrati No Content
            return NoContent();
        }
    }
}
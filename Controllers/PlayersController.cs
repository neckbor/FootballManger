using FootballManager.Models;
using FootballManager.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Core;

namespace FootballManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly FootballManagerContext _db;

        public PlayersController(FootballManagerContext db)
        {
            _db = db;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerByIdAsync(int id)
        {
            try
            {
                PlayerInfo player = await _db.Players.Include(p => p.Club)
                                                     .Where(p => p.PlayerId == id)
                                                     .Select(p => new PlayerInfo
                                                     {
                                                         Name = p.Name,
                                                         Surname = p.Surname,
                                                         Number = p.Number,
                                                         BirthDate = p.BirthDate,
                                                         Club = p.Club.Name
                                                     }).FirstOrDefaultAsync();

                if (player == null)
                {
                    return NotFound($"there is no player with id = {id}");
                }

                return Ok(player);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayerAsync([FromBody]PlayerInfo playerInfo)
        {
            try
            {
                if (!(await _db.Clubs.AnyAsync(c => c.Name == playerInfo.Club)))
                {
                    return NotFound($"club \"{playerInfo.Club}\" is not found");
                }

                Player player = new Player
                {
                    Name = playerInfo.Name,
                    Surname = playerInfo.Surname,
                    Number = playerInfo.Number,
                    BirthDate = playerInfo.BirthDate,
                    Club = await _db.Clubs.FirstOrDefaultAsync(c => c.Name == playerInfo.Club)
                };

                await _db.Players.AddAsync(player);
                await _db.SaveChangesAsync();

                return Created($"/{player.PlayerId}", playerInfo);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ModifyPlayerAsync([FromBody]PlayerInfo playerInfo)
        {
            try
            {
                Player player = await _db.Players.Include(p => p.Club).FirstOrDefaultAsync(p => p.PlayerId == playerInfo.Id);

                if (player == null)
                {
                    return NotFound($"there is no player with id = {playerInfo.Id}");
                }

                player.Name = playerInfo.Name;
                player.Surname = playerInfo.Surname;
                player.Number = playerInfo.Number;
                player.BirthDate = playerInfo.BirthDate;

                await _db.SaveChangesAsync();
                return Ok(playerInfo);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("transfer")]
        public async Task<IActionResult> TransferPlayerAsync([FromBody]TransferInfo transfer)
        {
            try
            {
                Player player = await _db.Players.FindAsync(transfer.PlayerId);
                if (player == null)
                {
                    return NotFound($"player with id = {transfer.PlayerId} is not found");
                }

                Club newClub = await _db.Clubs.FindAsync(transfer.ToClubId);
                if (newClub == null)
                {
                    return NotFound($"club with id = {transfer.ToClubId} is not found");
                }

                player.Club = newClub;
                await _db.SaveChangesAsync();

                PlayerInfo playerInfo = new PlayerInfo
                {
                    Id = player.PlayerId,
                    Name = player.Name,
                    Surname = player.Surname,
                    Number = player.Number,
                    BirthDate = player.BirthDate,
                    Club = player.Club.Name
                };
                return Ok(playerInfo);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayerAsync(int id)
        {
            try
            {
                Player player = await _db.Players.FindAsync(id);
                if (player == null)
                {
                    return NotFound();
                }

                _db.Players.Remove(player);
                await _db.SaveChangesAsync();

                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

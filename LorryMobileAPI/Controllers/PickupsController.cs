using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LorryMobileAPI.Data;
using LorryModels;

namespace LorryMobileAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PickupsController : ControllerBase
    {
        private readonly LorryMobileAPIContext _context;

        public PickupsController(LorryMobileAPIContext context)
        {
            _context = context;
        }

        // GET: api/Pickups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pickup>>> GetPickup()
        {
            return await _context.Pickup.ToListAsync();
        }

        // GET: api/Pickups/5
        [HttpGet("getid/{id}")]
        public async Task<ActionResult<Pickup>> GetPickup(int id)
        {
            var pickup = await _context.Pickup.FindAsync(id);

            if (pickup == null)
            {
                return NotFound();
            }

            return pickup;
        }

        [HttpGet("getbarcode/{barcode}")]
        public async Task<ActionResult<Pickup>> GetPickup(string barcode)
        {
            var pickup = await _context.Pickup.Where(p => p.Barcode == barcode).SingleOrDefaultAsync();

            if (pickup == null)
            {
                return NotFound();
            }

            return pickup;
        }

        // PUT: api/Pickups/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutPickup(int id, Pickup pickup)
        {
            if (id != pickup.Id)
            {
                return BadRequest();
            }

            _context.Entry(pickup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PickupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pickups
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("create")]
        public async Task<ActionResult<Pickup>> PostPickup(Pickup pickup)
        {
            _context.Pickup.Add(pickup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPickup", new { id = pickup.Id }, pickup);
        }

        // DELETE: api/Pickups/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Pickup>> DeletePickup(int id)
        {
            var pickup = await _context.Pickup.FindAsync(id);
            if (pickup == null)
            {
                return NotFound();
            }

            _context.Pickup.Remove(pickup);
            await _context.SaveChangesAsync();

            return pickup;
        }

        private bool PickupExists(int id)
        {
            return _context.Pickup.Any(e => e.Id == id);
        }
    }
}

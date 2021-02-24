using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LorryModels;
using LorryLogAPI.Data;

namespace LorryLogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly LorryLogAPIContext _context;

        public VehiclesController(LorryLogAPIContext context)
        {
            _context = context;
        }

        // GET: api/Vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicle()
        {
            return await _context.Vehicle.ToListAsync();
        }

        // GET: api/Vehicles/5
        [HttpGet("getvehicleid/{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicleId(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return vehicle;
        }

        [HttpGet("getvehiclename/{name}")]
        public async Task<ActionResult<Vehicle>> GetVehicleName(string name)
        {
            var vehicle = await _context.Vehicle.Where(v => v.Name == name).SingleOrDefaultAsync();

            if (vehicle == null)
            {
                return NotFound();
            }
            return vehicle;
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        [HttpPut("updatevehicle/{id}")]
        public async Task<IActionResult> PutVehicle(int id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
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

        // POST: api/Vehicles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("savevehicle")]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            if (VehicleExists(vehicle.Name))
            {
                return BadRequest("Duplicate name");
            }
            else
            {
                _context.Vehicle.Add(vehicle);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetVehicle", new { id = vehicle.Id }, vehicle);
            }
        }

        // DELETE: api/Vehicles/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Vehicle>> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }

        private bool VehicleExists(string name)
        {
            return _context.Vehicle.Any(e => e.Name == name);
        }
    }
}

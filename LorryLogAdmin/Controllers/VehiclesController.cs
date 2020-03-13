using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LorryModels;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net.Http.Headers;

namespace LorryLogAdmin.Controllers
{
    public class VehiclesController : Controller
    {
        private IConfiguration _configuration;
        private static readonly HttpClient _client = new HttpClient();
        private static string _remoteUrl = string.Empty;
        private static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public VehiclesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _remoteUrl = configuration.GetConnectionString("LorryLogAPI_URL");
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var data = await _client.GetStringAsync($"{_remoteUrl}/vehicles");
            IEnumerable<LorryModels.Vehicle> vehicles = JsonSerializer.Deserialize<List<Vehicle>>(data, _jsonOptions);
            return View(vehicles);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _client.GetStringAsync($"{_remoteUrl}/vehicles/getvehicleid/{id}");
            var vehicle = JsonSerializer.Deserialize<Vehicle>(data, _jsonOptions);

            if (vehicle == null)
            {
                return NotFound();
            }
            else
            {
                vehicle.Model = _configuration.GetValue<string>("Model_App_Setting");
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,License,Make,Model,Year")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                var jsonString = JsonSerializer.Serialize<Vehicle>(vehicle);
                var content = new StringContent(jsonString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var data = await _client.PostAsync($"{_remoteUrl}/vehicles/savevehicle", content);
                return RedirectToAction(nameof(Index));

            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _client.GetStringAsync($"{_remoteUrl}/vehicles/getvehicleid/{id}");
            var vehicle = JsonSerializer.Deserialize<Vehicle>(data, _jsonOptions);

            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,License,Make,Model,Year")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var jsonString = JsonSerializer.Serialize<Vehicle>(vehicle);
                var content = new StringContent(jsonString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var data = await _client.PutAsync($"{_remoteUrl}/vehicles/updatevehicle/{id}", content);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _client.GetStringAsync($"{_remoteUrl}/vehicles/getvehicleid/{id}");
            var vehicle = JsonSerializer.Deserialize<Vehicle>(data, _jsonOptions);

            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _client.DeleteAsync($"{_remoteUrl}/vehicles/delete/{id}");
            return RedirectToAction(nameof(Index));
        }

    }
}

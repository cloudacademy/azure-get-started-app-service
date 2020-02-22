using NUnit.Framework;
using LorryModels;
using LorryLogAPI.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System;
using LorryLogAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LorryTests
{
    public class VehicleTests
    {
        private VehiclesController _controller;
        private IConfiguration _configuration;
        private LorryLogAPIContext _context;
        private List<Vehicle> testTrucks;
        private List<Vehicle> dbTrucks;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json", false, true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();

            string connStr =_configuration.GetConnectionString("LorryLogAPIContext");
            string testData = _configuration.GetSection("TestFiles")["vehicle"];

            var options = new DbContextOptionsBuilder<LorryLogAPIContext>()
                   .UseSqlServer(connStr)
                   .Options;

            _context = new LorryLogAPIContext(options);
            _controller = new VehiclesController(_context);
            string testPath = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) + testData;
            string json = File.ReadAllText(testPath);
            testTrucks = JsonSerializer.Deserialize<List<Vehicle>>(json);
        }

        [Test]
        public async Task aPostTruck0()
        {
            ActionResult<Vehicle> actionresult = await _controller.PostVehicle(testTrucks[0]);
            if (actionresult.Result is BadRequestObjectResult)
            {
                BadRequestObjectResult result = actionresult.Result as BadRequestObjectResult;
                Assert.Fail(result.Value.ToString());
            }
            else
            {
                CreatedAtActionResult result = actionresult.Result as CreatedAtActionResult;
                Vehicle truck = result.Value as Vehicle;
                if (truck.Name == testTrucks[0].Name && truck.Make == testTrucks[0].Make)
                    Assert.Pass();
                else
                    Assert.Fail();
            }
        }

        [Test]
        public async Task bPostTruck1()
        {
            ActionResult<Vehicle> actionresult = await _controller.PostVehicle(testTrucks[1]);
            if (actionresult.Result is BadRequestObjectResult)
            {
                BadRequestObjectResult result = actionresult.Result as BadRequestObjectResult;
                Assert.Fail(result.Value.ToString());
            }
            else
            {
                CreatedAtActionResult result = actionresult.Result as CreatedAtActionResult;
                Vehicle truck = result.Value as Vehicle;
                if (truck.Name == testTrucks[1].Name && truck.Make == testTrucks[1].Make)
                    Assert.Pass();
                else
                    Assert.Fail();
            }
        }

        [Test]
        public async Task cListTrucks()
        {

            ActionResult<IEnumerable<Vehicle>> actionresult = await _controller.GetVehicle();
            dbTrucks = ((List<Vehicle>)actionresult.Value);
            if (dbTrucks.Count == 2)
                Assert.Pass();
            else
                Assert.Fail();
        }

        [Test]
        public async Task dUpdateTruck()
        {

            ActionResult<Vehicle> result =  await _controller.GetVehicleName(testTrucks[1].Name);
            Vehicle truck = result.Value;
            truck.Model = testTrucks[2].Model;
            truck.Year = testTrucks[2].Year;
            var res = await _controller.PutVehicle(truck.Id, truck);

            result =  await _controller.GetVehicleId(truck.Id);
            Vehicle updatedTruck = result.Value;

            if (truck.Model == updatedTruck.Model && truck.Make == updatedTruck.Make && truck.Year == updatedTruck.Year)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task eDelete()
        {
            ActionResult<IEnumerable<Vehicle>> actionresult = await _controller.GetVehicle();
            dbTrucks = ((List<Vehicle>)actionresult.Value);
            try
            {
                foreach (Vehicle v in dbTrucks)
                {
                    await _controller.DeleteVehicle(v.Id);
                }
                actionresult = await _controller.GetVehicle();
                dbTrucks = ((List<Vehicle>)actionresult.Value);

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            if (dbTrucks.Count == 0)
                Assert.Pass();
            else
                Assert.Fail();
        }
    }
}
using LorryMobileAPI.Controllers;
using LorryMobileAPI.Data;
using LorryModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LorryMobileTests
{
    public class PickUpTests
    {
        private PickupsController _controller;
        private IConfiguration _configuration;
        private LorryMobileAPIContext _context;
        private List<Pickup> testTrucks;
        private string barcode = "12312312312";

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();
            _configuration = builder.Build();

            var options = new DbContextOptionsBuilder<LorryMobileAPIContext>()
                    .UseInMemoryDatabase(databaseName: "LorryLog")
                    .Options;

            _context = new LorryMobileAPIContext(options);
            _controller = new PickupsController(_context);
        }

        [Test]
        public async Task aPostPickup1()
        {
            Pickup pu = new Pickup(1, "21 The Terrace, Tamahere RD3, Hamilton 3283", "7 Oak Street, Rotal Oak, Auckland", true, null, null, barcode);
            ActionResult<Pickup> actionresult = await _controller.PostPickup(pu);
            if (actionresult.Result is BadRequestObjectResult)
            {
                BadRequestObjectResult result = actionresult.Result as BadRequestObjectResult;
                Assert.Fail(result.Value.ToString());
            }
            else
            {
                CreatedAtActionResult result = actionresult.Result as CreatedAtActionResult;
                Pickup pdu = result.Value as Pickup;
                if (pdu.ToAddress == pu.ToAddress && pdu.Barcode == pu.Barcode)
                    Assert.Pass();
                else
                    Assert.Fail();
            }
        }

        [Test]
        public async Task bPostPickup2()
        {
            Pickup pu = new Pickup(2, "Cambridge", "Amazon Returns, Las Vegas, NV 89030", false, null, null, "188872");

            ActionResult<Pickup> actionresult = await _controller.PostPickup(pu);
            if (actionresult.Result is BadRequestObjectResult)
            {
                BadRequestObjectResult result = actionresult.Result as BadRequestObjectResult;
                Assert.Fail(result.Value.ToString());
            }
            else
            {
                CreatedAtActionResult result = actionresult.Result as CreatedAtActionResult;
                Pickup pdu = result.Value as Pickup;
                if (pdu.ToAddress == pu.ToAddress && pdu.Barcode == pu.Barcode)
                    Assert.Pass();
                else
                    Assert.Fail();
            }
        }

        [Test]
        public async Task cListPickups()
        {

            ActionResult<IEnumerable<Pickup>> actionresult = await _controller.GetPickup();
            List<Pickup> pkups = ((List<Pickup>)actionresult.Value);
            if (pkups.Count == 2)
                Assert.Pass();
            else
                Assert.Fail();
        }

        [Test]
        public async Task dUpdateTruck()
        {

            ActionResult<Pickup> result = await _controller.GetPickup(barcode);
            Pickup pickup = result.Value;
            pickup.ToAddress = "7 Oak Street, Royal Oak, Auckland";
            pickup.SignatureRequired = false;
            var res = await _controller.PutPickup(pickup.Id, pickup);

            result = await _controller.GetPickup(pickup.Id);
            Pickup updatedPickup = result.Value;

            if (pickup.ToAddress == updatedPickup.ToAddress && pickup.SignatureRequired == updatedPickup.SignatureRequired)
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
            ActionResult<IEnumerable<Pickup>> actionresult = await _controller.GetPickup();
            List<Pickup> pickups = ((List<Pickup>)actionresult.Value);
            try
            {
                foreach (Pickup p in pickups)
                {
                    await _controller.DeletePickup(p.Id);
                }
                actionresult = await _controller.GetPickup();
                pickups = ((List<Pickup>)actionresult.Value);

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            if (pickups.Count == 0)
                Assert.Pass();
            else
                Assert.Fail();
        }
    }
}
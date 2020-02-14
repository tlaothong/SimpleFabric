using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using SharedModels;
using SimpleStateActor.Interfaces;
using WebStateless.Models;

namespace WebStateless.Controllers
{
    public class StateSvcController : Controller
    {
        private ISimpleStatefulService stateSvc;
        public StateSvcController()
        {
            stateSvc = ServiceProxy.Create<ISimpleStatefulService>(
                new Uri("fabric:/ManyFabrics/SimpleStatefulService"), new ServicePartitionKey(1));
        }

        // GET: SimpleItems
        public async Task<IActionResult> Index()
        {
            //return View(await _context.SimpleItems.ToListAsync());
            var items = await stateSvc.ListItemsAsync();
            return View(items);
        }

        // GET: SimpleItems/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var simpleItem = await stateSvc.GetItemAsync(id);

            if (simpleItem == null)
            {
                return NotFound();
            }

            return View(simpleItem);
        }

        // GET: SimpleItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SimpleItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Number")] SimpleItem simpleItem)
        {
            if (ModelState.IsValid)
            {
                await stateSvc.AddItemAsync(simpleItem);
                return RedirectToAction(nameof(Index));
            }

            return View(simpleItem);
        }

        // GET: SimpleItems/Edit/5
        public Task<IActionResult> Edit(string id)
        {
            return Details(id);
        }

        // POST: SimpleItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Number")] SimpleItem simpleItem)
        {
            if (id != simpleItem.Id)
            {
                return NotFound();
            }

            return View(simpleItem);
        }
    }
}

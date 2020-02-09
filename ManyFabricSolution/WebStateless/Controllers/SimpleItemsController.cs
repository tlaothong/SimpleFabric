using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharedModels;
using WebStateless.Models;

namespace WebStateless.Controllers
{
    public class SimpleItemsController : Controller
    {
        public SimpleItemsController()
        {
        }

        // GET: SimpleItems
        public async Task<IActionResult> Index()
        {
            //return View(await _context.SimpleItems.ToListAsync());
            var items = Enumerable.Range(1, 9).Select(it => new SimpleItem
            {
                Id = it.ToString(),
                Name = $"Item {it}",
                Number = it,
            }).ToList();
            return View(items);
        }

        // GET: SimpleItems/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var simpleItem = new SimpleItem
            {
                Id = 1.ToString(),
                Name = "Item details",
                Number = 1,
            };

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
            //if (ModelState.IsValid)
            //{
            //    _context.Add(simpleItem);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
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

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(simpleItem);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!SimpleItemExists(simpleItem.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            return View(simpleItem);
        }
    }
}

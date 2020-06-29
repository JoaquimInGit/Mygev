using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mygev.Data;
using Mygev.Models;

namespace Mygev.Controllers
{
    public class EventoUtilizadoresController : Controller
    {
        private readonly MygevDB _context;

        public EventoUtilizadoresController(MygevDB context)
        {
            _context = context;
        }

        // GET: EventoUtilizadores
        public async Task<IActionResult> Index()
        {
            var mygevDB = _context.EventoUtilizadores.Include(e => e.Evento).Include(e => e.Utilizador);
            return View(await mygevDB.ToListAsync());
        }

        // GET: EventoUtilizadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoUtilizadores = await _context.EventoUtilizadores
                .Include(e => e.Evento)
                .Include(e => e.Utilizador)
                .FirstOrDefaultAsync(m => m.IDEU == id);
            if (eventoUtilizadores == null)
            {
                return NotFound();
            }

            return View(eventoUtilizadores);
        }

        // GET: EventoUtilizadores/Create
        public IActionResult Create()
        {
            ViewData["IDEvento"] = new SelectList(_context.Evento, "ID", "Descricao");
            ViewData["IDUser"] = new SelectList(_context.Utilizadores, "ID", "Email");
            return View();
        }

        // POST: EventoUtilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDEU,IDUser,IDEvento,Permissao")] EventoUtilizadores eventoUtilizadores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventoUtilizadores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IDEvento"] = new SelectList(_context.Evento, "ID", "Descricao", eventoUtilizadores.IDEvento);
            ViewData["IDUser"] = new SelectList(_context.Utilizadores, "ID", "Email", eventoUtilizadores.IDUser);
            return View(eventoUtilizadores);
        }

        // GET: EventoUtilizadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoUtilizadores = await _context.EventoUtilizadores.FindAsync(id);
            if (eventoUtilizadores == null)
            {
                return NotFound();
            }
            ViewData["IDEvento"] = new SelectList(_context.Evento, "ID", "Descricao", eventoUtilizadores.IDEvento);
            ViewData["IDUser"] = new SelectList(_context.Utilizadores, "ID", "Email", eventoUtilizadores.IDUser);
            return View(eventoUtilizadores);
        }

        // POST: EventoUtilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDEU,IDUser,IDEvento,Permissao")] EventoUtilizadores eventoUtilizadores)
        {
            if (id != eventoUtilizadores.IDEU)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventoUtilizadores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoUtilizadoresExists(eventoUtilizadores.IDEU))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IDEvento"] = new SelectList(_context.Evento, "ID", "Descricao", eventoUtilizadores.IDEvento);
            ViewData["IDUser"] = new SelectList(_context.Utilizadores, "ID", "Email", eventoUtilizadores.IDUser);
            return View(eventoUtilizadores);
        }

        // GET: EventoUtilizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoUtilizadores = await _context.EventoUtilizadores
                .Include(e => e.Evento)
                .Include(e => e.Utilizador)
                .FirstOrDefaultAsync(m => m.IDEU == id);
            if (eventoUtilizadores == null)
            {
                return NotFound();
            }

            return View(eventoUtilizadores);
        }

        // POST: EventoUtilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventoUtilizadores = await _context.EventoUtilizadores.FindAsync(id);
            _context.EventoUtilizadores.Remove(eventoUtilizadores);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoUtilizadoresExists(int id)
        {
            return _context.EventoUtilizadores.Any(e => e.IDEU == id);
        }
    }
}

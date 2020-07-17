using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mygev.Data;
using Mygev.Models;

namespace Mygev.Controllers
{
    public class EventoConteudoController : Controller
    {
        private readonly MygevDB _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventoConteudoController(MygevDB context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EventoConteudo
        public async Task<IActionResult> Index()
        {
            var mygevDB = _context.EventoConteudo.Include(e => e.Evento);
            //var EventoConteudo = _context.EventoConteudo.Include(e => e.Evento);
            //var Evento = _context.EventoConteudo.Include(e => e.ID);
            return View(await mygevDB.ToListAsync());
            //return View(Tuple.Create(Evento,EventoConteudo));
        }

        // GET: EventoConteudo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoConteudo = await _context.EventoConteudo
                .Include(e => e.Evento)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventoConteudo == null)
            {
                return NotFound();
            }

            return View(eventoConteudo);
        }

        // GET: EventoConteudo/Create
        public IActionResult Create(int id)
        {
            ViewData["Evento"] = _context.Evento.Where(e => e.ID == id).Select(e => e.Nome).FirstOrDefault();
            ViewBag.id = id;
            return View();
        }

        // POST: EventoConteudo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Conteudo,Comentario,IDEvento")] EventoConteudo eventoConteudo)
        {
            //id do evento
            var evento = await _context.Evento.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                eventoConteudo.IDEvento = id;
                eventoConteudo.IDUser = _context.Utilizadores.Where(u => u.UserId == _userManager.GetUserId(User)).Select(u => u.ID).FirstOrDefault();
                _context.Add(eventoConteudo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Evento", new { id });
        }

        // GET: EventoConteudo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoConteudo = await _context.EventoConteudo.FindAsync(id);
            if (eventoConteudo == null)
            {
                return NotFound();
            }
            ViewData["IDEvento"] = new SelectList(_context.Evento, "ID", "ID", eventoConteudo.IDEvento);
            return View(eventoConteudo);
        }

        // POST: EventoConteudo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Conteudo,Comentario,IDEvento")] EventoConteudo eventoConteudo)
        {
            if (id != eventoConteudo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventoConteudo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoConteudoExists(eventoConteudo.ID))
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
            ViewData["IDEvento"] = new SelectList(_context.Evento, "ID", "ID", eventoConteudo.IDEvento);
            return View(eventoConteudo);
        }

        // GET: EventoConteudo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoConteudo = await _context.EventoConteudo
                .Include(e => e.Evento)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventoConteudo == null)
            {
                return NotFound();
            }

            return View(eventoConteudo);
        }

        // POST: EventoConteudo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventoConteudo = await _context.EventoConteudo.FindAsync(id);
            _context.EventoConteudo.Remove(eventoConteudo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoConteudoExists(int id)
        {
            return _context.EventoConteudo.Any(e => e.ID == id);
        }
    }
}

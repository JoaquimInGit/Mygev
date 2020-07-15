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
    public class EventoUtilizadoresController : Controller
    {
        private readonly MygevDB _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventoUtilizadoresController(MygevDB context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EventoUtilizadores
        public async Task<IActionResult> Index()
        {
            var mygevDB = _context.EventoUtilizadores
                .Include(e => e.Evento)
                .Include(e => e.Utilizador);
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
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventoUtilizadores == null)
            {
                return NotFound();
            }

            return View(eventoUtilizadores);
        }
        
        // GET: EventoUtilizadores/Create
        public IActionResult Create(int id)
        {
            ViewData["Evento"] = _context.Evento.Where(e=>e.ID==id).Select(e => e.Nome).FirstOrDefault();
            ViewBag.id = id;
            return View();
        }
        
        // POST: EventoUtilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,[Bind("IDEU,IDUser,IDEvento,Permissao")] EventoUtilizadores eventoUtilizadores)
        {
            //id do evento
            var evento = await _context.Evento.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                eventoUtilizadores.IDEvento = id;
                eventoUtilizadores.IDUser = _context.Utilizadores.Where(u => u.UserId == _userManager.GetUserId(User)).Select(u => u.ID).FirstOrDefault();
                //Tinhamos originalmente Participante e sem queres coloquei convidado, nao esquecer de discutir isto
                eventoUtilizadores.Permissao = "Convidado";
                _context.Add(eventoUtilizadores);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["IDEvento"] = new SelectList(_context.Evento, "ID", "Descricao", eventoUtilizadores.IDEvento);
            //ViewData["IDUser"] = new SelectList(_context.Utilizadores, "ID", "Email", eventoUtilizadores.IDUser);
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
            if (id != eventoUtilizadores.ID)
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
                    if (!EventoUtilizadoresExists(eventoUtilizadores.ID))
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
                .FirstOrDefaultAsync(m => m.ID == id);
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
            return _context.EventoUtilizadores.Any(e => e.ID == id);
        }
    }
}

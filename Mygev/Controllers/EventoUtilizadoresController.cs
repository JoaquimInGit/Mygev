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
    //Contexto para a BD
        private readonly MygevDB _context;
        //Contexto para os utilizadores 
        private readonly UserManager<ApplicationUser> _userManager;

        public EventoUtilizadoresController(MygevDB context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EventoUtilizadores
        /// <summary>
        /// retorna o index dos eventos utilizadores, com os eventos em que o utilizador participa, administra.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            //vai encontrar o id do Utilizador que está logado
            int cUserId = _context.Utilizadores.Where(u => u.UserId == _userManager.GetUserId(User)).Select(u => u.ID).FirstOrDefault();
            //Select de todos os registos onde o Utilizador logado participa
            var mygevDB = _context.EventoUtilizadores
                .Include(e => e.Evento)
                .Include(e => e.Utilizador)
                .Where(e => e.IDUser == cUserId);
            return View(await mygevDB.ToListAsync());
        }

        
        // GET: EventoUtilizadores/Create
        /// <summary>
        /// retorna a pagina de confirmaçao de participação num evento
        /// </summary>
        /// <param name="id">Id do evento </param>
        /// <returns></returns>
        public IActionResult Create(int id)
        {
            //Retorna para a view o Nome do Evento onde vai ser criado um novo registo de Utilizador no evento. 
            ViewData["Evento"] = _context.Evento.Where(e=>e.ID==id).Select(e => e.Nome).FirstOrDefault();
            //Retorna para a view se o Evento é publico.
            ViewBag.Publico = _context.Evento.Where(e => e.ID == id).Select(e => e.Publico).FirstOrDefault();
            //Retorna o id do evento
            ViewBag.id = id;
            return View();
        }

        // POST: EventoUtilizadores/Create
        /// <summary>
        /// Insere na Bd tabela Evento utilizadores a "confirmação", ou seja uma nova linha com as referencias 
        /// </summary>
        /// <param name="id">ID do evento</param>
        /// <param name="eventoUtilizadores">Onde são recebidos os valores de IDEU,IDUser,IDEvento,Permissao da view </param>
        /// <param name="passEvento">palavra chave para confirmaçao de participação em eventos privados</param>
        /// <returns></returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,[Bind("IDEU,IDUser,IDEvento,Permissao")] EventoUtilizadores eventoUtilizadores, string passEvento)
        {
        //procura a chave do evento na BD
            var pass = await _context.Evento
               .Where(v => v.ID == id)
               .Select(v => v.passEvento)
               .FirstOrDefaultAsync();

            //Testa de as chave recebida e a da BD coincide
            if (pass != passEvento)
            {
                ViewBag.pass = "Falhou";
                return View();
            }
            //id do evento
            var evento = await _context.Evento.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //Pesquisa na DB pelo ID do utilizador atual
                int idUser = _context.Utilizadores.Where(u => u.UserId == _userManager.GetUserId(User)).Select(u => u.ID).FirstOrDefault();
                //Pesquisa na tabela EventoUtilizadores por o id do utilizador atual
                var idDB = await _context.EventoUtilizadores
                .Where(v => v.IDEvento == id)
                .Where(v => v.IDUser == idUser)
                .Select(v =>v.IDUser)
                .FirstOrDefaultAsync();

                //Insere na BD
                if (!idDB.Equals(idUser))
                {
                    eventoUtilizadores.IDEvento = id;
                    eventoUtilizadores.IDUser = idUser;
                    eventoUtilizadores.Permissao = "Participante";
                    _context.Add(eventoUtilizadores);

                    await _context.SaveChangesAsync();
                    
                }

            }
            return RedirectToAction("Details", "Evento", new { id });
        }


        // GET: EventoUtilizadores/Edit/5
        /// <summary>
        /// Retorna a pagina de ediçao de participação
        /// </summary>
        /// <param name="id">ID do eventoUtilizador</param>
        /// <returns></returns>
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
            //Retorna para a view o id do evento com base o id do registo EventoUtilizadores
            ViewData["IDEvento"] = _context.EventoUtilizadores.Where(e => e.ID == id).Select(e => e.IDEvento).FirstOrDefault();
            //Retorna para a view o id do Utilizador com base o id do registo EventoUtilizadores
            ViewData["IDUser"] = _context.EventoUtilizadores.Where(e => e.ID == id).Select(e => e.IDUser).FirstOrDefault();
            return View(eventoUtilizadores);
        }

        // POST: EventoUtilizadores/Edit/5
        /// <summary>
        /// Guarda as alteraçoes feitas na BD
        /// </summary>
        /// <param name="id">ID do eventoUtilizador</param>
        /// <param name="eventoUtilizadores">Parametro que recebe da view os valores de ID,IDUser,IDEvento,Permissao </param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,IDUser,IDEvento,Permissao")] EventoUtilizadores eventoUtilizadores)
        {
            if (id != eventoUtilizadores.ID)
            {
                return NotFound();
                
            }
           
            //Guarda as alteraçoes
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
                //Redireciona de volta para o evento
                return Redirect("~/Evento/Details/" + eventoUtilizadores.IDEvento);
            }          
            return Redirect("~/Evento/Details/" + eventoUtilizadores.IDEvento);
        }

        // GET: EventoUtilizadores/Delete/5
        /// <summary>
        /// elimina a participaçao em um evento
        /// </summary>
        /// <param name="id">Id eventoUlizador</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Encrontra o id do registo EventoUtilizadores
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

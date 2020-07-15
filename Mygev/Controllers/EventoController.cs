using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mygev.Data;
using Mygev.Models;

namespace Mygev.Controllers
{
    public class EventoController : Controller
    {
        /// <summary>
        /// variável que identifica a BD do projeto
        /// </summary>
        private readonly MygevDB _context;
        

        /// <summary>
        /// variável que identifica o utilizador Atual
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// variável que contém os dados do 'ambiente' do servidor. 
        /// Em particular, onde estão os ficheiros guardados, no disco rígido do servidor
        /// </summary>
        private readonly IWebHostEnvironment _caminho;
        public EventoController(MygevDB context,IWebHostEnvironment caminho, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _caminho = caminho;
            _userManager = userManager;
        }

        // GET: Evento
        public async Task<IActionResult> Index()
        {
            return View(await _context.Evento.ToListAsync());
        }

        // GET: Evento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Se o utilizador não tem loggin ativo Coloca a Permissão como "convidado"
            if (User.Identity.IsAuthenticated){ 
            //select IDUser from eventoutilizadores where userId=userLogado idevento=id and permissao = 'Administrador'
            var admin = await _context.EventoUtilizadores
                .Where(e => e.IDEvento == id)
                .Where(e => e.Utilizador.UserId == _userManager.GetUserId(User))
                .FirstOrDefaultAsync();
                 ViewBag.Permissao = admin.Permissao;
            
            }
            else{
                ViewBag.Permissao = "Convidado";
            }

            var evento = await _context.Evento
                .Include(e => e.ListaConteudos)
                .Include(e => e.ListaUtilizadores)
                .ThenInclude(u => u.Utilizador)
                .Where(v => v.ID == id)
                .FirstOrDefaultAsync();

            if (evento == null)
            {
                return NotFound();
            }
                return View(evento);
        }

        // GET: Evento/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Evento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,Logo,Local,DataInicio,DataFim,Descricao,Estado,Publico")] Evento evento, EventoUtilizadores eventoUtilizadores, IFormFile logoEvento)
        {
            // variaveis auxiliares para processar a fotografia
            string caminhoLogo = "";
            bool haImagem = false;

            // Se não houver fotografia adicionar uma imagem default do sistema
            if (logoEvento == null) { evento.Logo = "logoDefault.jpg"; }
            else
            {
                // Se houver imagem
                // Verificar se é uma imagem
                if (logoEvento.ContentType == "image/jpeg" ||
                    logoEvento.ContentType == "image/png")
                {
                    // o ficheiro é uma imagem válida
                    // preparar a imagem para ser guardada no disco rígido
                    // e o seu nome associado ao Evento
                    Guid g;
                    g = Guid.NewGuid();
                    string extensao = Path.GetExtension(logoEvento.FileName).ToLower();
                    string nome = g.ToString() + extensao;
                    // onde guardar o ficheiro
                    caminhoLogo = Path.Combine(_caminho.WebRootPath, "Imagens\\LogosEventos", nome);
                    // associar o nome do ficheiro ao Evento
                    evento.Logo = nome;
                    // assinalar que existe imagem e é preciso guardá-la no disco rígido
                    haImagem = true;
                }
                else
                {
                    // há imagem, mas não é do tipo correto
                    evento.Logo = "logoDefault.jpg";
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //insere na BD
                    _context.Add(evento);
                    await _context.SaveChangesAsync();

                    //Vai bustar o ultimo registo á BD
                    eventoUtilizadores.IDEvento = _context.Evento.Max(e => e.ID);
                    eventoUtilizadores.IDUser = _context.Utilizadores.Where(u => u.UserId == _userManager.GetUserId(User)).Select(u => u.ID).FirstOrDefault();
                    eventoUtilizadores.Permissao = "Administrador";
                    _context.Add(eventoUtilizadores);

                    await _context.SaveChangesAsync();
                    // se há imagem, vou guardá-la no disco rígido
                    if (haImagem)
                    {
                        using var stream = new FileStream(caminhoLogo, FileMode.Create);
                        await logoEvento.CopyToAsync(stream);
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                }
            }
            return View(evento);
        }


        // GET: Evento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Evento.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }


        // POST: Evento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,Logo,Local,DataInicio,DataFim,Descricao,Estado,Publico")] Evento evento)
        {
            if (id != evento.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.ID))
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
            return View(evento);
        }

        // GET: Evento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoConteudo = await _context.Evento
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventoConteudo == null)
            {
                return NotFound();
            }

            return View(eventoConteudo);
        }

        // POST: Evento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Evento.FindAsync(id);
            _context.Evento.Remove(evento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(int id)
        {
            return _context.Evento.Any(e => e.ID == id);
        }
    }
}

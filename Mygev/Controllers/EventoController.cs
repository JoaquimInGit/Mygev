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
        /// <summary>
        /// metodo que prepara o Index dos eventos
        /// Se os parametros virem a null, significa que nao foi executada nenhuma pesquisa.
        /// </summary>
        /// <param name="nomeEvento">string com o nome do evento, para efeitos de pesquisa na BD</param>
        /// <param name="visibilidade">string a visibilidade, para efeitos de pesquisa na BD</param>
        /// <param name="local">string com o local do evento, para efeitos de pesquisa na BD</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(String nomeEvento, String visibilidade, String local)
        {
            if (nomeEvento == null) nomeEvento = ""; 
            if (visibilidade == null) visibilidade = "todos";
            if (local == null) local = "";

            //Se foi pesquisado: Evento Privado, com ou sem local, com ou sem Nome
            if (visibilidade.Equals("privado"))
            {
                var evento = (IEnumerable<Object>)await _context.Evento
                    .Where(v => v.Nome.Contains(nomeEvento))
                    .Where(v=> v.Local.Contains(local))
                    .Where(v => v.Publico == false)
                    .ToListAsync();
                return View(evento);
            }
            //Se foi pesquisado: Evento publico, com ou sem local, com ou sem Nome
            if (visibilidade.Equals("publico")){
                var evento = (IEnumerable<Object>)await _context.Evento
                        .Where(v => v.Nome.Contains(nomeEvento))
                        .Where(v => v.Local.Contains(local))
                        .Where(v => v.Publico == true)
                        .ToListAsync();
                return View(evento);

            }
            //REtorna TODOS os Eventos(default)
            else
            {          
                var evento = (IEnumerable<Object>)await _context.Evento
                    .ToListAsync();
                return View(evento);
            }
        }

        // GET: Evento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Necessario para impedir os participantes de acederem a configuraçoes de administrador de eventos
            ViewBag.Permissao = "Participante";
            //Se o utilizador não tem loggin ativo Coloca a Permissão como administrador
            if (User.Identity.IsAuthenticated){ 
            //select IDUser from eventoutilizadores where userId=userLogado idevento=id and permissao = 'Administrador'
            var admin = await _context.EventoUtilizadores
                .Where(e => e.IDEvento == id)
                .Where(e => e.Utilizador.UserId == _userManager.GetUserId(User))
                .FirstOrDefaultAsync();
                if (admin != null)
                {
                    ViewBag.Permissao = admin.Permissao;
                }
            }
            else{
                ViewBag.Permissao = "Participante";
            }
            //Retorna os detalhes do evento, dos conteudos, utilizadores
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
            //para obrigar a checkbox dos eventos privados a estar checkada por defenição
            ViewBag.check = "checked";
            return View();
        }

        // POST: Evento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,Logo,Local,DataInicio,DataFim,Descricao,Estado,Publico,passEvento")] Evento evento, EventoUtilizadores eventoUtilizadores, IFormFile logoEvento)
        {
            //Se o evento é privado, retorna a view da criaçao com o campo para inserir a Password
            if(evento.Publico == false && evento.passEvento == null){
                ViewBag.pass = "naoInserida";
                ViewBag.check = false;
                return View();
            }
            if(evento.DataInicio > evento.DataFim){
                ViewBag.ErrData = "ErroData";
                return View();
            }

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
        //Retorna a pagina para editar eventos
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
        //Faz os testes necessarios e depois atualiza a base de dados
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
        //Retorna a pagina de eliminaçao de um evento
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
        //Elimina a linha da BD tabela eventos
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

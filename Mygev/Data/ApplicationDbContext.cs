using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mygev.Models;

namespace Mygev.Data
{
    /// <summary>
    /// Esta classe estende o conjunto de dados de um utilizador, criado a quando da Identity
    /// É necessário, alterar a definição da BD, e redefinir a nossa aplicação para usar este novo utilizador
    /// Em todos os sítios onde se referenciar 'IdentityUser' deverá referenciar-se 'ApplicationUser'
    /// </summary>
    public class ApplicationUser : IdentityUser
    {

        /// <summary>
        /// nome da pessoa q se regista, e posteriormente, autentica
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// registo da hora+data da criação do registo
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// criação da BD do projeto.
    /// Neste caso concreto, estamos a usar os dados genéricos + os dados particulares da nossa aplicação
    /// </summary>
    public class MygevDB : IdentityDbContext<ApplicationUser>
    {
        public MygevDB(DbContextOptions<MygevDB> options) : base(options) { }

        public virtual DbSet<Evento> Evento { get; set; }
        public virtual DbSet<EventoConteudo> EventoConteudo { get; set; }
        public virtual DbSet<EventoUtilizadores> EventoUtilizadores { get; set; }
        public virtual DbSet<Utilizadores> Utilizadores { get; set; }
    }
    
}

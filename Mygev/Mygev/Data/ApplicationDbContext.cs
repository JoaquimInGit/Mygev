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
    public class MygevDB : IdentityDbContext
    {
        public MygevDB(DbContextOptions<MygevDB> options) : base(options) { }

        public virtual DbSet<Evento> Animais { get; set; }
        public virtual DbSet<EventoConteudo> Donos { get; set; }
        public virtual DbSet<EventoUtilizadores> Veterinarios { get; set; }
        public virtual DbSet<Utilizadores> Consultas { get; set; }
    }
    
}

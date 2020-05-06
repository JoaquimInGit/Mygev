using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mygev.Models {
    public class Utilizadores {

        public Utilizadores() {
            ListaUtilizadores = new HashSet<Utilizadores>();//coloca dados na lista dos animais de cada dono
        }

        /// <summary>
        /// ID de um ultilizador 
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Nome que o utilizador usará na aplicaçao (username / Nick)
        /// </summary>
        public string NomeUser { get; set; }

        /// <summary>
        /// Email do utilizador
        /// </summary>
        public string email { get; set; }
        
        /// <summary>
        /// Password do Utilizador
        /// </summary>
        public string password { get; set; }



        /// <summary>
        /// Lista dos Utilizadores
        /// </summary>
        public ICollection<Utilizadores> ListaUtilizadores { get; set; }
    }

}


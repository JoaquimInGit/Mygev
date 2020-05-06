using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mygev.Models {
    public class EventoUtilizadores {

        public EventoUtilizadores(){
            
    }
        

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(Utilizador))]
        public int idUser { get; set; }
        public Utilizadores Utilizador { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(Evento))]
        public int idEvento { get; set; }

        public Evento Evento { get; set; }

        /// <summary>
        /// Diz se o utilizador tem permições para alterar o evento
        /// </summary>
        public int permissao { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mygev.Models {
    public class Evento {

        public Evento() {
            ListaEventos = new HashSet<Evento>();//coloca dados na lista dos animais de cada dono
        }

        /// <summary>
        /// ID de um evento 
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Nome do evento
        /// </summary>
        public string nome { get; set; }

        /// <summary>
        /// imagem  ilustrativa de um evento
        /// </summary>
        public string logo { get; set; }

        /// <summary>
        /// data do evento
        /// </summary>
        public DateTime data { get; set; }

        /// <summary>
        /// hora em que decorrerá o evento
        /// </summary>
        public DateTime hora { get; set; }

        /// <summary>
        /// Estado de um evento (presente, passado, futuro)
        /// </summary>
        public string estado { get; set; }

        /// <summary>
        /// se o evento é publico ou privado
        /// </summary>
        public bool PublicoPrivado { get; set; }

        /// <summary>
        /// lista dos eventos 
        /// </summary>
        public ICollection<Evento> ListaEventos { get; set; }
    }
}


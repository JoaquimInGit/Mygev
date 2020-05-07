using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mygev.Models
{
    public class EventoConteudo
    {
        public EventoConteudo()
        {}

        /// <summary>
        /// ID do Conteudo de um Evento
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Usado para adicionar conteudos adicionais ao evento
        /// </summary>
        public string conteudo { get; set; }

        /// <summary>
        /// Comentarios extras ao conteudo adicionado
        /// </summary>
        public string comentario { get; set; }

        /// <summary>
        /// chave estrangeira para o Evento
        /// </summary>
        [ForeignKey(nameof(Evento))]
        public int idEvento { get; set; }
        public Evento Evento { get; set; }
    }
}

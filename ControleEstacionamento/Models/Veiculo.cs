using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estacionamento.Models
{
    public class Veiculo
    {
        public Veiculo(int id, string placa, string modelo, DateTime horaEntrada)
        {
            Id = id;
            Placa = placa;
            Modelo = modelo;
            HoraEntrada = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public DateTime HoraEntrada { get; set; }
        public DateTime? HoraSaida { get; set; }


    }
}

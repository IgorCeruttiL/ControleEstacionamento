using Estacionamento.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Estacionamento.Models
{
    public class RegistroSaida
    {


        public RegistroSaida(string placa, int horasEstacionado, decimal valorPago, DateTime horaSaida)
        {
            Placa = placa;
            HorasEstacionado = horasEstacionado;
            ValorPago = valorPago;
            HoraSaida = horaSaida;
        }

        public RegistroSaida()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Placa { get; set; }

        public int HorasEstacionado { get; set; }

        public decimal ValorPago { get; set; }

        public DateTime HoraSaida { get; set; }


        private const decimal PrimeiraHora = 4.0m;
        private const decimal DemaisHoras = 2.0m;

        public decimal CalcularValorEstacionamento(DateTime horaEntrada, DateTime horaSaida)
        {
            TimeSpan tempoEstacionado = horaSaida - horaEntrada;
            int horasEstacionadas = (int)Math.Ceiling(tempoEstacionado.TotalHours);

            decimal valorTotal = CalcularValor(horasEstacionadas);
            return valorTotal;
        }

        private decimal CalcularValor(int horasEstacionadas)
        {
            if (horasEstacionadas <= 1)
            {
                return PrimeiraHora;
            }

            decimal valorTotal = PrimeiraHora + (DemaisHoras * (horasEstacionadas - 1));
            return valorTotal;
        }

        public decimal CalcularValorTotalDia(DateTime data, AppDbContext context)
        {
            var registrosDoDia = context.RegistrosSaida
                .Where(registro => registro.HoraSaida.Date == data.Date)
                .ToList();

            decimal valorTotal = registrosDoDia.Sum(registro => registro.ValorPago);
            return valorTotal;
        }
    }
}

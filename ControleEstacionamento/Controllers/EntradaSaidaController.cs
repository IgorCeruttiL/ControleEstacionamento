using Estacionamento.Data;
using Estacionamento.Exceptions;
using Estacionamento.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Controllers
{
    [ApiController]
    public class EntradaSaidaController : ControllerBase
    {
        [HttpPost("v1/entrada")]
        public IActionResult PostVeiculo([FromBody] Veiculo veiculo, [FromServices] AppDbContext context)
        {
            try
            {
                ExceptionsPlaca.ThrowIfInvalidPlaca(veiculo.Placa);

                if (context.Veiculos.Any(x => x.Placa == veiculo.Placa))
                {
                    return BadRequest("Placa já registrada no estacionamento");
                }

                context.Add(veiculo);
                context.SaveChanges();

                return Ok(veiculo);
            }
            catch (ExceptionsPlaca ex) 
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("v1/saida/{placa}")]
        public IActionResult RegistrarSaida(string placa, [FromServices] AppDbContext context)
        {
            var veiculo = context.Veiculos.FirstOrDefault(x => x.Placa == placa);
            if (veiculo == null)
                return NotFound("Veiculo não encontrado");

            if (veiculo.HoraSaida != null)
                return BadRequest("O veiculo já saiu");

            veiculo.HoraSaida = DateTime.Now;

            var registroSaida = new RegistroSaida(
                placa: veiculo.Placa,
                horasEstacionado: (int)Math.Ceiling((veiculo.HoraSaida.Value - veiculo.HoraEntrada).TotalHours),
                valorPago: 0, // Valor temporário.
                horaSaida: veiculo.HoraSaida.Value
            );

            decimal valorPago = registroSaida.CalcularValorEstacionamento(veiculo.HoraEntrada, veiculo.HoraSaida.Value);

            registroSaida.ValorPago = valorPago;

            context.RegistrosSaida.Add(registroSaida);
            context.SaveChanges();

            context.Veiculos.Remove(veiculo); // Remove o veículo do registro de entrada
            context.SaveChanges();

            return Ok($"Saida registrada. Valor Pago: {valorPago}");
        }

        [HttpGet("v1/registro/{data}")]
        public IActionResult RegistroDoDia(DateTime data, [FromServices] AppDbContext context)
        {
            var registrosDoDia = context.RegistrosSaida
               .Where(registro => registro.HoraSaida.Date == data.Date)
               .ToList();

            decimal valorTotalDia = registrosDoDia.Sum(registro => registro.ValorPago);

            return Ok(new
            {
                Data = data,
                ValorTotalDia = valorTotalDia,
                Registros = registrosDoDia
            });
        }


    }
}

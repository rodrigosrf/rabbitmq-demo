using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace RabbitMQ_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProducerController : ControllerBase
    {
        
        [HttpPost]
        public IActionResult SendMessage([FromBody] string message)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost", UserName = "admin", Password = "123456" }; // Substitua pelo endereço do servidor RabbitMQ, se necessário

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    string queueName = "minha_fila"; // Substitua pelo nome da fila que deseja usar

                    channel.QueueDeclare(queue: queueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: null,
                                         body: body);

                    return Ok($"Mensagem enviada: {message}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao enviar a mensagem: {ex.Message}");
            }
        }
    }
}
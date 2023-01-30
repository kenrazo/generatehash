using Infrastructure.RabbitMq;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace JobExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashController : ControllerBase
    {
        private const int HASH_COUNT = 40000;
        private readonly IRabbitmqService _rmqService;
        private readonly IHashRepository _hashRepository;

        public HashController(IRabbitmqService rmqService, IHashRepository hashRepository)
        {
            _rmqService = rmqService;
            _hashRepository = hashRepository;
        }

        [HttpPost("/hashes")]
        public async Task<IActionResult> GenerateHashes()
        {
            var hashes = Enumerable.Range(0, HASH_COUNT)
                .Select(_ => GenerateRandomSHA1Hash())
                .ToArray();

            Task.Run(() => _rmqService.SendMessage(hashes));

            return Ok();
        }

        [HttpGet("/hashes")]
        public async Task<IActionResult> GetHashes()
        {
            return Ok(await _hashRepository.Get());

        }

        private string GenerateRandomSHA1Hash()
        {
            using (var sha1 = SHA1.Create())
            {
                var randomBytes = new byte[20];
                var random = new Random();
                random.NextBytes(randomBytes);
                var hash = sha1.ComputeHash(randomBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}

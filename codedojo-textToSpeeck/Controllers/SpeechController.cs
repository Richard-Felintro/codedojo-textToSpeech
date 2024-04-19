using codedojo_textToSpeeck.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using ApiSpeechToText.Models;
using codedojo_textToSpeeck.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace codedojo_textToSpeeck.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechController : ControllerBase
    {
        readonly string? speechKey = Environment.GetEnvironmentVariable("4016c83a20694e3d8f6d45f3aac3f2e2");
        readonly string? speechRegion = Environment.GetEnvironmentVariable("brazilsouth");
        private readonly AzureService _azureService;
        public SpeechController(AzureService azureService)
        {
            _azureService = azureService;
        }


        [HttpPost("TextToSpeech")]
        public async Task<IActionResult> TextToSpeech(string text)
        {
            try
            {
                var retorno = _azureService.TextToSpeech(text);
                return Ok(new { retorno = retorno});
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //tiramos o [] dps do string
        [HttpPost("SpeechToText")]
        public async Task<IActionResult> PostAudio([FromForm] FileUpload file)
        {
            try
            {
                // Verifique se o arquivo foi enviado
                if (file != null)
                {
                    // Processar o arquivo de áudio, e salvar no diretório informado
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Audios");

                    // Verifica se o diretório de destino especificado existe.
                    if (!Directory.Exists(filePath))
                    {
                        // Cria o diretório se não existir
                        Directory.CreateDirectory(filePath);
                    }

                    // Gera um nome de arquivo único
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.Arquivo.FileName);

                    // Caminho completo do arquivo
                    var fullPath = Path.Combine(filePath, fileName);

                    // Salva o arquivo
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.Arquivo.CopyTo(fileStream);
                    }

                    // Validar o tipo do arquivo para confirmar o uso do áudio
                    if (Path.GetExtension(file.Arquivo.FileName) != ".wav")
                    {
                        fullPath = await AzureService.Conversor(fullPath, fullPath.Replace(Path.GetExtension(file.Arquivo.FileName), ".wav"));
                    }

                    // Enviando para o serviço de tradução da fala
                    var teste = AzureService.SpeechToText(fullPath);

                    return Ok(new { Texto = teste.Result });
                }
                else
                {
                    return BadRequest("Nenhum arquivo de áudio foi enviado.");
                }
            }
            catch (Exception ex)
            {
                // Captura outras exceções
                return BadRequest(ex.Message);
            }
        }
    }

}
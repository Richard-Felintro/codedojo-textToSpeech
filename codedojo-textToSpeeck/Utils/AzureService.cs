using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using NAudio.Wave;

namespace codedojo_textToSpeeck.Utils
{
    public class AzureService
    {
        private static SpeechConfig Connection()
        {
            var config = SpeechConfig.FromSubscription("1d27ca97a7764cecafc9c26d3a5010ca", "eastus");

            return config;
        }
        static string? speechKey = "b0c56bed82844a209da345ada84c6352";
        static string? speechRegion = "brazilsouth";

        //Tiramos o static dps do async
        public async Task<string> TextToSpeech(string texto)
        {
            var caminho = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);

            using var audioConfig = AudioConfig.FromWavFileOutput(Directory.GetCurrentDirectory() + "/Audios/result.wav");

            //speechConfig.SpeechSynthesisVoiceName = "pt-BR";
            
            using (var speechSynthesizer = new SpeechSynthesizer(speechConfig, audioConfig))
            {
                using (SpeechSynthesisResult result = await speechSynthesizer.SpeakTextAsync(texto))
                {
                    return result.ToString();
                }
            }
        }

        //Tiramos o static dps do async
        public static async Task<string> SpeechToText(string audio)
        {
            try
            {
                // Chamando a comunicação com o serviço da azure
                var connection = Connection();

                // Cria um objeto AudioConfig com base no fluxo de entrada de áudio
                var audioConfig = AudioConfig.FromWavFileOutput(Directory.GetCurrentDirectory() + "/Audios/result.wav");

                // Cria um reconhecedor de fala usando a configuração de fala
                using (var recognizer = new SpeechRecognizer(connection, "pt-BR", audioConfig))
                {
                    // Substitua "audio.wav" pelo caminho do seu arquivo de áudio
                    var result = await recognizer.RecognizeOnceAsync();

                    // Processa o resultado do reconhecimento
                    switch (result.Reason)
                    {
                        case ResultReason.RecognizedSpeech:
                            return $"Texto identificado: \n\n '{result.Text}'";

                        case ResultReason.NoMatch:
                            return ("Não foi possível reconhecer o discurso.");

                        default:
                            return ("Ocorreu um erro durante o reconhecimento de fala.");

                    }

                }
            }
            catch (Exception ex)
            {
                return $"Erro ao converter áudio para texto: {ex.Message}";
            }
        }

        // Naudio
        public static async Task<string> Conversor(string caminhoOriginal, string caminhoConversao)
        {
            //Abre o arquivo de áudio de entrada
            using (var reader = new MediaFoundationReader(caminhoOriginal))
            {
                // Cria um novo arquivo WAV para escrita
                using (var writer = new WaveFileWriter(caminhoConversao, reader.WaveFormat))
                {
                    // Copia os dados de áudio do arquivo de entrada para o arquivo WAV
                    reader.CopyTo(writer);

                    return caminhoConversao;
                }
            }
        }
    }
}

        //async static Task Main(string[] args)
        //{
        //    var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
        //    speechConfig.SpeechRecognitionLanguage = "pt-BR";

        //    using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        //    using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        //    Console.WriteLine("speak into your mic");
        //    var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        //    OutputSpeechResult(speechRecognitionResult);
        //}

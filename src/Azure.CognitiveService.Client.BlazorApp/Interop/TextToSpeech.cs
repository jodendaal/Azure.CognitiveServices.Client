using Microsoft.JSInterop;

namespace Azure.CognitiveService.Client.BlazorApp.Interop
{
    public class TextToSpeech : IAsyncDisposable
    {
        private const string _modulePath = "./scripts/TextToSpeech.js";
        private readonly IJSRuntime _jSRuntime;
        private IJSObjectReference _module;
        private IJSObjectReference _textToSpeech;

        public TextToSpeech(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public async Task Initialise()
        {
            _module = await _jSRuntime!.InvokeAsync<IJSObjectReference>("import", _modulePath);
            _textToSpeech = await _module.InvokeAsync<IJSObjectReference>("createTextToSpeech");
        }

        public async Task Speak(string text,string voiceUri, CancellationToken canellationToken)
        {
            try
            {
                await _textToSpeech.InvokeAsync<object>("speak", canellationToken, text, voiceUri);
            }
            catch(TaskCanceledException) { }
        }

        public async Task Cancel()
        {
            await _textToSpeech.InvokeVoidAsync("cancel");
        }

        public async Task<List<Voice>> GetVoices()
        {
            var voices = await _textToSpeech.InvokeAsync<List<Voice>>("getVoices",CancellationToken.None);
            return voices;
        }

        public async ValueTask DisposeAsync()
        {
            if (_module != null)
            {
                await _module.DisposeAsync();
            }

            if (_textToSpeech != null)
            {
                await _textToSpeech.DisposeAsync();
            }
        }

        public class Voice
        {
            public string Name { get; set; }
            public string Lang { get; set; }
            public string VoiceURI { get; set; }
        }
    }
}

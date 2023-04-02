using Microsoft.JSInterop;

namespace Azure.CognitiveService.Client.BlazorApp.Interop
{
    public class SpeechToText : IAsyncDisposable
    {
        private const string _modulePath = "./scripts/SpeechToText.js";
        private readonly IJSRuntime _jSRuntime;
        private IJSObjectReference _module;
        private IJSObjectReference _speechToText;
        private bool _isRecording = false;
        private Action<string> _onTextReceievdAction;
        DotNetObjectReference<SpeechToText> _dotNetObjectReference { get; set; 
        }
        public SpeechToText(IJSRuntime jSRuntime, Action<string> onTextReceievdAction)
        {
            _jSRuntime = jSRuntime;
            _onTextReceievdAction = onTextReceievdAction;
            _dotNetObjectReference = DotNetObjectReference.Create(this);
        }

        public async Task Initialise()
        {
            _module = await _jSRuntime!.InvokeAsync<IJSObjectReference>("import", _modulePath);
            _speechToText = await _module.InvokeAsync<IJSObjectReference>("createSpeechToText");
        }

        public async Task Start()
        {
            _isRecording = true;
            await _speechToText.InvokeAsync<object>("startProxy", _dotNetObjectReference, nameof(OnTextReceived));
        }

        public async Task Stop()
        {
            _isRecording = false;
            await _speechToText.InvokeVoidAsync("stop");
        }

        [JSInvokable]
        public void OnTextReceived(string text)
        {
            if(_onTextReceievdAction != null)
            {
                _onTextReceievdAction(text);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_module != null)
            {
                await _module.DisposeAsync();
            }

            if (_speechToText != null)
            {
                await _speechToText.DisposeAsync();
            }

            if(_dotNetObjectReference != null)
            {
                _dotNetObjectReference.Dispose();
            }
        }
    }
}

class TextToSpeech {
    speak(text, voiceUri = "") {
        return new Promise((resolve) => {
            let voices = speechSynthesis.getVoices();
            let voice = voiceUri === "" ? voices.filter(i => i.default)[0] : voices.filter(i => i.voiceURI === voiceUri)[0];
            let utterance = new SpeechSynthesisUtterance();
            utterance.text = text;
            utterance.lang = "en";
            utterance.voice = voice;
            speechSynthesis.speak(utterance);
            utterance.addEventListener("end", () => {
                resolve();
            });
        });
    }
    cancel() {
        speechSynthesis.cancel();
    }
    getVoices() {
        return new Promise((resolve) => {
            let voices = speechSynthesis.getVoices();
            if (voices.length == 0) {
                speechSynthesis.onvoiceschanged = () => {
                    voices = speechSynthesis.getVoices();
                    let mappedVoices = voices.map((voice) => ({ Name: voice.name, Lang: voice.lang, VoiceURI: voice.voiceURI }));
                    resolve(mappedVoices);
                };
            }
            else {
                let mappedVoices = voices.map((voice) => ({ Name: voice.name, Lang: voice.lang, VoiceURI: voice.voiceURI }));
                resolve(mappedVoices);
            }
        });
    }
}
export function createTextToSpeech() {
    return new TextToSpeech();
}

class TextToSpeech {

    speak(text: string, selectedVoiceIndex: number = 0) {
        return new Promise<void>((resolve) => {
            let utterance = new SpeechSynthesisUtterance();
            utterance.text = text;
            utterance.lang = "en"
            utterance.voice = speechSynthesis.getVoices()[selectedVoiceIndex];
            speechSynthesis.speak(utterance);
            utterance.addEventListener("end", () => {
                resolve();
            });
        });
    }

    getVoices() {
        return new Promise((resolve) => {

            let voices = speechSynthesis.getVoices();

            if (voices.length == 0) {
                speechSynthesis.onvoiceschanged = () => {

                    voices = speechSynthesis.getVoices();
                    let mappedVoices =  voices.map((voice) => ({ name: voice.name, lang: voice.lang }));
                    resolve(mappedVoices);

                }
            } else {
                let mappedVoices = voices.map((voice) => ({ name: voice.name, lang: voice.lang }));
                resolve(mappedVoices);
            }

        });
    }
}

export function createTextToSpeech(): TextToSpeech {
    return new TextToSpeech();
}

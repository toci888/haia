import { Audio } from 'expo-av';
import { useState, useEffect } from 'react';
import axios from 'axios';

export default function AudioRecorder() {
  const [recording, setRecording] = useState(null);
  const [audioUri, setAudioUri] = useState(null);
  
  useEffect(() => {
    // Gdy nagranie zostanie zakończone, przetwarzamy dźwięk
    if (audioUri) {
      sendToOpenAI(audioUri);
    }
  }, [audioUri]);

  const startRecording = async () => {
    try {
      const { granted } = await Audio.requestPermissionsAsync();
      if (!granted) return alert('Permissions to access microphone is required.');

      await Audio.setAudioModeAsync({
        allowsRecordingIOS: true,
        playsInSilentModeIOS: true,
      });

      const recording = new Audio.Recording();
      await recording.prepareToRecordAsync(Audio.RECORDING_OPTIONS_PRESET_HIGH_QUALITY);
      await recording.startAsync();

      setRecording(recording);
    } catch (err) {
      console.error('Failed to start recording:', err);
    }
  };

  const stopRecording = async () => {
    try {
      await recording.stopAndUnloadAsync();
      const uri = recording.getURI();
      setRecording(null);
      setAudioUri(uri);
    } catch (err) {
      console.error('Failed to stop recording:', err);
    }
  };

  const sendToOpenAI = async (uri) => {
    const formData = new FormData();
    formData.append('file', {
      uri,
      type: 'audio/wav',
      name: 'audio.wav',
    });

    try {
      const response = await axios.post('YOUR_OPENAI_SPEECH_TO_TEXT_API_ENDPOINT', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
          Authorization: `Bearer YOUR_OPENAI_API_KEY`,
        },
      });
      const transcript = response.data.text;

      // Wyślij transkrypcję do ComedyTextController
      sendToComedyAPI(transcript);
    } catch (error) {
      console.error('Error sending audio to OpenAI:', error);
    }
  };

  const sendToComedyAPI = async (transcript) => {
    try {
      const response = await axios.post('https://yourapiurl.com/api/ComedyText', {
        text: transcript,
        author: 'warrior', // Zastąp faktycznym autorem
      });

      console.log('Text sent to Comedy API:', response.data);
    } catch (error) {
      console.error('Error sending text to Comedy API:', error);
    }
  };

  return (
    <View>
      <Button title={recording ? "Stop Recording" : "Start Recording"} onPress={recording ? stopRecording : startRecording} />
    </View>
  );
}

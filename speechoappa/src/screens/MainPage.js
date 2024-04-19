import api from '../services/Services'
import { Audio } from 'expo-av'
import { Container } from "../components/Container/Container"
import { useState } from 'react'
import { Text, TextInput, Button } from 'react-native'
// import { Button } from '../components/Button/Button'
import { Input } from '../components/Input/Input'
import { FontAwesome } from '@expo/vector-icons';


export const MainPage = () => {
    const [text, setText] = useState("");
    const [audio, setAudio] = useState()
    const [abacate, setAbacate] = useState("");

    async function PlaySound() {
        const { sound } = await Audio.Sound.createAsync(require("../assets/audioOutput.wav"))
        setAudio(sound)
        await audio.playAsync()
    }

    async function TextToSpeech() {
        await api.post("/Speech/TextToSpeech", {
            text: text,
        }).then(response => {
            console.log(response.data);
            setAudio(response.data)
            PlaySound()
        }).catch(error => {
            console.log(error);
        })
    }
    return (
        <Container>
            <TextInput
                onChangeText={(txt) => setText(txt)}
            />

            <Button title='manda' onPress={() => TextToSpeech}>manda</Button>
            {/* <Button onPress={() => TextToSpeech()}>
                <FontAwesome name="volume-up" size={75} color="#33242b"></FontAwesome>
            </Button>

            <Button onPress={() => PlaySound()}>
                <Text>Aperte e fale</Text>
            </Button> */}

            <Text>{abacate}</Text>

            
            
        </Container>
    )
}


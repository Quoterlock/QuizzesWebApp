import { ChangeEvent, useContext, useState } from "react"
import { useNavigate } from "react-router-dom";
import Notification from "../shared/Notification";
import { Button } from "../shared/Button";
import TextInputGroup from "../shared/TextInputGroup";
import { registerUser } from "../../services/authService";
import { AppContext } from "../../services/AppContext";

export default function RegisterPage() {
    const navigate = useNavigate()
    const [notification, setNotification] = useState({text:"",title:""})
    const [inputs, setInputs] = useState({username:"", email:"", password:"", confirmedPassword:""});
    const {authorizationApi} = useContext(AppContext)

    const hInputChange = (event:ChangeEvent<HTMLInputElement>) => {
        const name = event.target.name;
        const value = event.target.value;
        setInputs(values => ({...values, [name]: value}))
    }


    const hOnRegister = async () => {
        if(inputs.confirmedPassword === inputs.password){
            try{
                const result = await registerUser(inputs.username, inputs.email, inputs.password, authorizationApi)
                if(result.success){
                    navigate("/login")
                } else {
                    setNotification({text:result.message??"none", title: "Server error"})
                }
            } catch (error) {
                console.error(error)
                if(error instanceof Error)
                    setNotification({text: error.message, title:"Input error"})
            }
        }
    }

    const hOnLogin = () => {
        navigate("/login")
    }

    return(<div className="block-style col-lg-4 col-md-7 col-sm-12 mx-auto">
        <h5 className="text-center">Registration</h5>
        { notification.text !== "" && <Notification title={notification.title} message={notification.text}/>}
        <TextInputGroup name="username" value={inputs.username} 
                onChange={hInputChange} label="Username"
                isCorrect={(inputs.username !== "")}/>
        <TextInputGroup name="email" value={inputs.email} 
                onChange={hInputChange} label="Email"
                isCorrect={true}/>
        <TextInputGroup name="password" value={inputs.password} 
                onChange={hInputChange} label="Password" 
                isCorrect={true}/>
        <TextInputGroup name="confirmedPassword" value={inputs.confirmedPassword} 
                onChange={hInputChange} label="Confirm password"
                isCorrect={(inputs.password === inputs.confirmedPassword)}/>
        <div className="d-grid">
            <Button onClick={hOnRegister} type="active">Register</Button>
            <Button onClick={hOnLogin} type="minor">to Log-in</Button>
        </div>
    </div>)
}
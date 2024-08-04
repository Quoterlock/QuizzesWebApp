import { ChangeEvent, useContext, useState } from "react"
import { useNavigate } from "react-router-dom";
import Notification from "../shared/Notification";
import { Button } from "../shared/Button";
import TextInputGroup from "../shared/TextInputGroup";
import { loginUser } from "../../services/authService";
import { AppContext } from "../../services/AppContext";

export default function LoginPage(){
    const navigate = useNavigate()
    const [notification, setNotification] = useState({text:"", title:""}); 
    const [inputs, setInputs] = useState({email:"", password:""})
    const {authorizationApi} = useContext(AppContext);

    const hOnLogin = async () => {
        try{
            const result = await loginUser(inputs.email, inputs.password, authorizationApi)
            if(result.success){
                navigate('/');
            } else {
                setNotification({text:result.message??"none", title:"Service:Input error"})
            }
        } catch (error) {
            console.error(error);
            if(error instanceof Error)
                setNotification({text: error.message, title: "Server: Error"})
        }
    }

    const hOnRegister = () => {
        navigate("/register")
    }

    const hInputChange = (e:ChangeEvent<HTMLInputElement>) => {
        var name = e.target.name;
        var value = e.target.value;
        setInputs(values => ({...values, [name]: value}))
        setNotification({text:"", title:""})
    }

    return(<div className="block-style col-lg-4 col-md-7 col-sm-12 mx-auto">
            <h5 className="text-center mb-3">Authorization</h5>
            { notification.text !== "" && <Notification title={notification.title} message={notification.text}/>}        
            <TextInputGroup name="email" value={inputs.email} label="Email"
                isCorrect={true} onChange={hInputChange}/>
            <TextInputGroup name="password" value={inputs.password} label="Password"
                isCorrect={true} onChange={hInputChange}/>
            <div className="d-grid">
                <Button onClick={hOnLogin} type="active">Log-in</Button>
                <Button onClick={hOnRegister} type="minor">to Register</Button>
            </div>
        </div>)
}

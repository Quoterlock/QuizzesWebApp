import { ChangeEvent, useContext, useState } from "react"
import { AppContext } from "../services/AppContext";
import { Link } from "react-router-dom";
import Notification from "../components/Notification";

export default function RegisterPage() {
    const {authorizationService} = useContext(AppContext);
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmedPassword, setConfirmedPassword] = useState("");
    const [isNotify, setIsNotify] = useState(false);
    const [notificationText, setNotificationText] = useState(""); 
    const onRegister = () => {
        if(password === confirmedPassword)
        {
            authorizationService.Register(username, email, password)
            .then((result)=> { console.log(result)
                if(result.code !== 200) {
                    setNotificationText(result.message)
                    setIsNotify(true)
                } else {
                    setIsNotify(false)
                }
            })
            .catch((error)=> console.log(error));
        } 
    }

    return(<div className="col-lg-4 col-md-7 col-sm-12 mx-auto">
    <div className="block-style">
        <h5 className="text-center">Registration</h5>
        { isNotify && <Notification title="Error" message={notificationText}/>}
        <div className="d-grid form-inputs mb-3 mt-3">
                <label>Login</label>
                <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setUsername(e.currentTarget.value)} value={username}/>
            </div>
            <div className="d-grid form-inputs mb-3">
                <label>Email</label>
                <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setEmail(e.currentTarget.value)} value={email}/>
            </div>
            <div className="d-grid form-inputs mb-3">
                <label>Password</label>
                <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setPassword(e.currentTarget.value)} value={password}/>
            </div>
            <div className="d-grid form-inputs mb-3">
                <label>Confirm password</label>
                <input type="text" className={(password !== confirmedPassword) ? "wrong-input":""} onChange={(e:ChangeEvent<HTMLInputElement>)=>setConfirmedPassword(e.currentTarget.value)} value={confirmedPassword}/>
            </div>
        <div className="d-grid">
            <button className="btn active-btn mb-2" onClick={()=>onRegister()}>Register</button>
            <Link to="/login" className="btn minor-btn">to Log-in</Link>
        </div>
    </div>
</div>)
}
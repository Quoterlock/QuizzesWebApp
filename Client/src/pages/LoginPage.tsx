import { ChangeEvent, useContext, useState } from "react"
import { AppContext } from "../services/app-context";

export default function LoginPage(){
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const {authorizationService} = useContext(AppContext);
    const onLogin = () => {
        authorizationService.Login(login, password).then((result)=>
            console.log(result)
        )
    }

    const PingApi = () => {
        authorizationService.Logout().then((result)=>console.log(result))
    }
    return(<div className="container">
        <div className="col-5 block-style">
            <div className="d-grid form-inputs mb-3">
                <label>Login</label>
                <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setLogin(e.currentTarget.value)} value={login}/>
            </div>
            <div className="d-grid form-inputs mb-3">
                <label>Password</label>
                <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setPassword(e.currentTarget.value)} value={password}/>
            </div>
            <div className="d-grid">
                <button className="btn btn-active" onClick={()=>onLogin()}>Log-in</button>
                <button className="btn active-btn" onClick={()=>PingApi()}>Ping</button>
            </div>
        </div>
    </div>)
}
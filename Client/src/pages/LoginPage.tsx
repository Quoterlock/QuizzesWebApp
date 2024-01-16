import { ChangeEvent, useContext, useState } from "react"
import { AppContext } from "../services/AppContext";
import { Link } from "react-router-dom";

export default function LoginPage(){
    const [login, setLogin] = useState("")
    const [password, setPassword] = useState("")
    const [confirmedPassword, setConfirmed] = useState("")
    const [email, setEmail] = useState("")

    const {authorizationService} = useContext(AppContext)
    const onLogin = () => {
        /*
        authorizationService.Login(login, password).then((result)=>
            console.log(result)
        )
        */
    }

    const toRegister = () => {
        // authorizationService.Logout().then((result)=>console.log(result))
    }
    return(<div className="col-lg-4 col-md-7 col-sm-12 mx-auto">
        <div className="block-style">
            <h5 className="text-center">Authorization</h5>
            <div className="d-grid form-inputs mb-3">
                <label>Login</label>
                <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setLogin(e.currentTarget.value)} value={login}/>
            </div>
            <div className="d-grid form-inputs mb-3">
                <label>Email</label>
                <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setPassword(e.currentTarget.value)} value={password}/>
            </div>
            <div className="d-grid form-inputs mb-3">
                <label>Password</label>
                <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setPassword(e.currentTarget.value)} value={password}/>
            </div>
            <div className="d-grid form-inputs mb-3">
                <label>Confirm password</label>
                <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setPassword(e.currentTarget.value)} value={password}/>
            </div>
            <div className="d-grid">
                <button className="btn active-btn mb-2" onClick={()=>onLogin()}>Log-in</button>
                <Link to="/register" className="btn minor-btn" onClick={()=>toRegister()}>to Register</Link>
            </div>
        </div>
    </div>)
}
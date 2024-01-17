import { ChangeEvent, useContext, useState } from "react"
import { AppContext } from "../services/AppContext";
import { Link } from "react-router-dom";

export default function LoginPage(){
    const [password, setPassword] = useState("")
    const [email, setEmail] = useState("")

    const {authorizationService} = useContext(AppContext)
    const onLogin = () => {
        authorizationService.Login(email, password)
        .then((result)=>
            console.log(result)
        ).catch((error)=>
            console.log(error)
        )
    }

    return(<div className="col-lg-4 col-md-7 col-sm-12 mx-auto">
        <div className="block-style">
            <h5 className="text-center">Authorization</h5>
            <div className="d-grid form-inputs mb-3">
                <label>Email</label>
                <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setEmail(e.currentTarget.value)} value={email}/>
            </div>
            <div className="d-grid form-inputs mb-3">
                <label>Password</label>
                <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setPassword(e.currentTarget.value)} value={password}/>
            </div>
            <div className="d-grid">
                <button className="btn active-btn mb-2" onClick={()=>onLogin()}>Log-in</button>
                <Link to="/register" className="btn minor-btn">to Register</Link>
            </div>
        </div>
    </div>)
}
import { ChangeEvent, useContext, useState } from "react"
import { AppContext } from "../services/AppContext";
import { Link } from "react-router-dom";

export default function RegisterPage() {
    const {authorizationService} = useContext(AppContext);
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");

    const onRegister = () => {
        // register functionality
    }

    return(<div className="col-lg-4 col-md-7 col-sm-12 mx-auto">
    <div className="block-style">
        <h5 className="text-center">Registration</h5>
        <div className="d-grid form-inputs mb-3">
            <label>Login</label>
            <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setLogin(e.currentTarget.value)} value={login}/>
        </div>
        <div className="d-grid form-inputs mb-3">
            <label>Password</label>
            <input type="text" onChange={(e:ChangeEvent<HTMLInputElement>)=>setPassword(e.currentTarget.value)} value={password}/>
        </div>
        <div className="d-grid">
            <button className="btn active-btn mb-2" onClick={()=>onRegister()}>Register</button>
            <Link to="/login" className="btn minor-btn">to Log-in</Link>
        </div>
    </div>
</div>)
}
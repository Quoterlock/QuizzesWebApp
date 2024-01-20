import { ChangeEvent, useContext, useState } from "react"
import { AppContext } from "../services/AppContext";
import { Link, useNavigate } from "react-router-dom";
import Notification from "../components/Notification";

export default function RegisterPage() {
    const navigate = useNavigate()
    const {authorizationService} = useContext(AppContext)
    const [username, setUsername] = useState("")
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const [confirmedPassword, setConfirmedPassword] = useState("")
    const [notification, setNotification] = useState({text:"",title:""})

    const onRegister = () => {
        const emailRegex: RegExp = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/
        const passwordRegex: RegExp = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$/

        if (emailRegex.test(email)) {
            if(passwordRegex.test(password)) {
                if(password === confirmedPassword && username !== "") {
                    authorizationService.Register(username, email, password)
                    .then((result)=> { console.log(result)
                        if(result.code === 200) {
                            navigate("/login")
                        } else {
                            setNotification({text:result.message, title:"Server: Input error"})
                        }
                    })
                    .catch((error) => {
                        console.log(error)
                        setNotification({text:error, title:"Server: Error"})
                    });
                }
            } else {
                setNotification({text:"Password must contain:\n- At least one lowercase letter.\n- At least one uppercase letter.\n- At least one digit.\n- At least one non-alphanumeric character.\n- Minimum length of 8 characters.\n", title:"Input error"})
            }
        } else {
            setNotification({text:"Invalid email address!", title:"Input error"})
        }  
    }

    return(<div className="col-lg-4 col-md-7 col-sm-12 mx-auto">
    <div className="block-style">
        <h5 className="text-center">Registration</h5>
        { notification.text !== "" && <Notification title={notification.title} message={notification.text}/>}
        <div className="d-grid form-inputs mb-3 mt-3">
                <label>Username</label>
                <input type="text" className={(username === "") ? "wrong-input":""} onChange={(e:ChangeEvent<HTMLInputElement>)=>setUsername(e.currentTarget.value)} value={username}/>
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
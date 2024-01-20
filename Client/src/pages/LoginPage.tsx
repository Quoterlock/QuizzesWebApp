import { ChangeEvent, useContext, useState } from "react"
import { AppContext } from "../services/AppContext";
import { Link, useNavigate } from "react-router-dom";
import Notification from "../components/Notification";

export default function LoginPage(){
    const navigate = useNavigate()
    const [password, setPassword] = useState("")
    const [email, setEmail] = useState("")
    const [notification, setNotification] = useState({text:"", title:""}); 
    const {authorizationService, userProfileService} = useContext(AppContext)

    const onLogin = () => {
        const emailRegex: RegExp = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

        if (emailRegex.test(email)) {
            authorizationService.Login(email, password)
                .then((result)=> { console.log(result)
                    if(result.code === 200) {
                        userProfileService.GetCurrentUserProfileAsync().then((profile) =>{
                            localStorage.setItem("current-username", profile.owner.username)
                            localStorage.setItem("current-user-id", profile.owner.id)
                            navigate('/')
                        }).catch((error) => {
                            setNotification({text:error, title:"Server error"})
                        })
                    } else {
                        setNotification({text:result.message, title:"Server: Input error"})
                    }
                }).catch((error)=> {
                    console.log(error)
                    setNotification({text:error, title:"Server: Error"})
                })
        } else {
            setNotification({text:"Invalid email address!", title:"Input error"})
        }
    }

    return(<div className="col-lg-4 col-md-7 col-sm-12 mx-auto">
        <div className="block-style">
        <h5 className="text-center mb-3">Authorization</h5>
            { notification.text !== "" && <Notification title={notification.title} message={notification.text}/>}        
            <div className="d-grid form-inputs mb-3 mt-3">
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
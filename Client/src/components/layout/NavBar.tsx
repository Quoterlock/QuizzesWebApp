import { useNavigate } from "react-router-dom";
import { IconButton } from "../shared/IconButton"
import { Button } from "../shared/Button";

interface Props {
    username:string
}

export default function NavBar({username}:Props) {
    
    const navigate = useNavigate();
    const toProfile = () => {
        navigate("/profile")
    }

    const toLogin = () => {
        navigate("/login")
    }
    
    return(<div className="custom-navbar">
        <div className="col ms-2">
            <IconButton iconSrc="./src/assets/icons/home-icon.png" onClick={()=>{navigate("/")}}/>
        </div>
        { !(username as string)
            ? <div className="col-4 d-flex justify-content-end align-items-center">
                <Button onClick={toLogin} type="active">Log-in</Button>
            </div>
            : <div className="col-4 d-flex justify-content-end align-items-center">
                <p className="me-2 my-0">{username}</p>    
                <IconButton iconSrc="./src/assets/icons/account-icon.png" onClick={toProfile}/>
            </div>
        }
    </div>)
}
import { ReactNode, useContext, useEffect, useState } from "react";
import NavBar from "./NavBar";
import { AppContext } from "../../services/AppContext";
import { useNavigate } from "react-router";

interface Props {
    children: ReactNode
}

export function QuizLayout({children}: Props) {
    const [currentUsername, setUsername] = useState<string>("")
    const {userProfileApi} = useContext(AppContext)
    const navigate = useNavigate()
    
    useEffect(() => {
        userProfileApi.GetCurrentUserNameAsync()
            .then((result) => {
                if(result.code == 200){
                    setUsername(result.username as string)
                    if(result.username??"" !== localStorage.getItem("current-username"))
                        localStorage.setItem("current-username", result.username??"")
                }
                else {
                    console.log(result.username)
                    localStorage.setItem("current-username", "")
                    localStorage.setItem("current-user-id", "")
                    setUsername("");
                    navigate("/login")
                }
            })
            .catch((error) => {
                console.log(error)
                setUsername("");
                navigate("/login")
            })
    },[])

    return(
        <div className="bg-color">
            <NavBar username={currentUsername as string}/>
            <div className="mt-5">
                {children}
            </div>
        </div>
    )
}

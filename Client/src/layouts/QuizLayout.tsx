import { ReactNode, useContext, useEffect, useState } from "react";
import NavBar from "../components/NavBar";
//import { AppContext } from "../services/AppContext";


interface Props {
    children: ReactNode
}

export function QuizLayout({children}: Props) {
    //const {userProfileService} = useContext(AppContext)
    const currentUsername = localStorage.getItem("current-username")
    useEffect(() => {
        const fetchData = async () => {
          /*try {
            const response = await userProfileService.GetCurrentUserProfile()
            setCurrentUser(response)
            console.log(response);
          } catch (error) {
            // Handle errors
            console.error(error);
          }
          */
        }
    
        fetchData()
      }, [])

    return(
        <div className="bg-color">
            <NavBar username={currentUsername as string}/>
            <div className="mt-5">
                {children}
            </div>
        </div>
    )
}

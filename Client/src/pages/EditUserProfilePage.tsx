import { useContext, useEffect, useState } from "react"
import { AppContext } from "../services/AppContext"

export default function EditUserProfilePage(){

    const {userProfileService} = useContext(AppContext)
    const [userProfile, setUserProfile] = useState<UserProfile>()
    useEffect(()=> {
        const fetchData = async () => {
            try{
                const response = await userProfileService.GetCurrentUserProfile()
                setUserProfile(response)
            } catch(error){
                console.error(error)
            }
        }

        fetchData()
    }, [])

    return(<div>Edit user : {userProfile?.Username}</div>)
}
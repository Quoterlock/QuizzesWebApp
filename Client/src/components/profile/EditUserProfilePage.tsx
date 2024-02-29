import { useContext, useEffect, useState } from "react"
import { AppContext } from "../../services/AppContext"

export default function EditUserProfilePage(){

    const {userProfileApi: userProfileService} = useContext(AppContext)
    const [userProfile, setUserProfile] = useState<UserProfile>()
    useEffect(()=> {
        const fetchData = async () => {
            try{
                const response = await userProfileService.GetCurrentUserProfileAsync()
                setUserProfile(response)
            } catch(error){
                console.error(error)
            }
        }

        fetchData()
    }, [])

    return(<div>Edit user : {userProfile?.owner.username}</div>)
}
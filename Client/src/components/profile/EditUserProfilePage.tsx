import { ChangeEvent, useContext, useEffect, useState } from "react"
import { AppContext } from "../../services/AppContext"
import TextInputGroup from "../shared/TextInputGroup"
import { Button } from "../shared/Button"
import { useNavigate } from "react-router"

export default function EditUserProfilePage(){

    const {userProfileApi: userProfileService} = useContext(AppContext)
    const [userProfile, setUserProfile] = useState<UserProfile>()
    const [displayName, setDisplayName] = useState("")
    const navigate = useNavigate()
    useEffect(()=> {
        const fetchData = () => {
            try{
                userProfileService.GetCurrentUserProfileAsync()
                    .then((result)=> {
                        setDisplayName(result.profile?.displayName as string)
                        setUserProfile(result.profile)
                        
                    })
                    .catch((error) => console.log(error))
            } catch(error){
                console.error(error)
            }
        }
        fetchData()
    }, [])

    const onTextChange = (e:ChangeEvent<HTMLInputElement>) => {
        setDisplayName(e.target.value)
    }

    const onCancel = () => {
        navigate("/profile")
    }

    const onSave = () => {
        const profile = {
            id:userProfile?.id as string,
            owner:userProfile?.owner as Owner, 
            displayName:displayName, 
            createdQuizzesCount: 0,
            completedQuizzesCount: 0
        }
        userProfileService.UpdateProfile(profile.id as string, profile)
        navigate("/profile")
    }

    return(
        <div className="col-lg-6 col-md-8 col-sm-12 mx-auto">
            <div>Edit user : {userProfile?.owner.username as string}</div>
            <TextInputGroup value={displayName} onChange={onTextChange} label="Display name" isCorrect={true} name="displayName"/>
            <Button type="minor" onClick={onCancel}>Cancel</Button>
            <Button type="active" onClick={onSave}>Save</Button>
        </div>
    )
}
import { ChangeEvent, useContext, useEffect, useState } from "react"
import { AppContext } from "../../services/AppContext"
import TextInputGroup from "../shared/TextInputGroup"
import { Button } from "../shared/Button"
import { useNavigate } from "react-router"
import FileUpload from "../shared/FileUpload"

export default function EditUserProfilePage(){

    const {userProfileApi: userProfileService} = useContext(AppContext)
    const [userProfile, setUserProfile] = useState<UserProfileInfo>()
    const [displayName, setDisplayName] = useState("")
    const [imageBytes, setImageBytes] = useState<Uint8Array>()
    const navigate = useNavigate()
    useEffect(()=> {
        const fetchData = () => {
            try{
                userProfileService.GetCurrentUserProfileInfoAsync()
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
        let profile:UserProfileInfo = {
            id:userProfile?.id as string,
            owner:userProfile?.owner as Owner, 
            displayName:displayName, 
        }
        userProfileService.UpdateProfile(profile)
            .then(() => navigate("/profile"))
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
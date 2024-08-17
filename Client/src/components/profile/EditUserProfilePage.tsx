import { ChangeEvent, useContext, useEffect, useState } from "react"
import { AppContext } from "../../services/AppContext"
import TextInputGroup from "../shared/TextInputGroup"
import { Button } from "../shared/Button"
import { useNavigate } from "react-router"

export default function EditUserProfilePage(){

    const {userProfileApi: userProfileService} = useContext(AppContext)
    const [userProfile, setUserProfile] = useState<UserProfileInfo>()
    const [displayName, setDisplayName] = useState("")
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
        <div className="col-lg-6 col-md-8 col-sm-12 mx-auto block-style">
            <h5 className="text-center">Edit @{userProfile?.owner.username as string}</h5>
            <TextInputGroup value={displayName} onChange={onTextChange} label="Display name" isCorrect={true} name="displayName"/>
            <div className="row px-3">
                <div className="col-6 d-grid p-0">
                    <Button type="minor" onClick={onCancel}>Cancel</Button>
                </div>
                <div className="col-6 d-grid p-0">
                    <Button type="active" onClick={onSave}>Save</Button>
                </div>
            </div>
        </div>
    )
}
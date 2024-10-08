import { useContext, useEffect, useState } from "react";
import { AppContext } from "../../services/AppContext";
import { Button } from "../shared/Button";
import { useNavigate } from "react-router";
import ImageFile from "../shared/ImageFile";

export function ChangeProfilePhotoPage() {
    const {userProfileApi:userProfileApi} = useContext(AppContext)
    const [imageBytes, setImageBytes] = useState<File>()
    const navigate = useNavigate()
/*
    useEffect(()=>{
       userProfileApi.GetCurrentUserProfileAsync()
        .then((res)=> setImageBytes(
            res.profile?.imageBytes ?? new Uint8Array(0)
        ))
        .catch((error) => console.log(error))
    },[])
*/
    const onUpdate = () => {
        if(imageBytes !== undefined){
            userProfileApi
            .UpdateProfilePhoto(imageBytes)
            .then(()=>navigate('/profile'));
        }
    }

    return(
    <div className="block-style photo-upload-block mx-auto d-grid">
        <h5 className="text-center">New profile photo</h5>
        {
            imageBytes !== undefined &&
            <ImageFile file={imageBytes} width={200} height={200}/>
        }
        <input type="file" accept=".jpeg, .jpg" onChange={(e)=>{
            if(e.target.files !== null)
                setImageBytes(e.target.files[0])
        }}/>
        <div className="d-grid mt-2">
            <Button type="active" onClick={onUpdate}>Update</Button>
            <Button type="minor" onClick={()=>navigate('/profile')}>Cancel</Button>    
        </div>
    </div>

    )
}
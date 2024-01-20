import { useContext, useEffect, useState } from "react";
import { AppContext } from "../services/AppContext";
import { useNavigate, useParams } from "react-router";
import { Link } from "react-router-dom";

export default function UserProfilePage() {
    const navigate = useNavigate()
    const {id} = useParams()
    const isOwner = (id === null || id === undefined || id === "")
    const {userProfileService, authorizationService} = useContext(AppContext)
    const [profile, setUserProfile] = useState<UserProfile>()

    useEffect(()=>{
      const fetchData = async () => {
        try {
          const response = isOwner 
          ? await userProfileService.GetCurrentUserProfileAsync() 
          : await userProfileService.GetUserProfileAsync(id as string)
          setUserProfile(response)
        } catch (error) {
          console.error(error)
        }
      }
      fetchData()
    }, [])
    
    useEffect(()=> {
      localStorage.setItem("current-username", profile?.owner.username as string)
      localStorage.setItem("current-user-id", profile?.owner.id as string)
    })


    const onLogout = () => {
        authorizationService.Logout()
        .then((result)=>console.log(result))
        .catch((error)=>console.log(error))
        navigate("/")
    }
    
    return(<div className="col-lg-6 col-md-8 col-sm-12 mx-auto">
          { profile &&
          <div>
            <div className="d-flex justify-content-between mb-2">
                <Link to="/" className="styless-link">{"< Home"}</Link>
                {/*<Link to="/" className="styless-link">{"Logout"}</Link>*/}
            </div> 
              <div className="block-style d-flex justify-content-between">                
                <div className="d-flex">
                    <img height={150} width={150}/>
                    <div className="ms-4">
                        <div className="d-flex">
                            <h2>{profile.displayName || "null"}</h2>
                            {
                                isOwner && <Link to={"/profile/edit"} className="btn p-1 ms-1">
                                    <img src="./src/assets/icons/edit-icon.png" className="icon-img"/>
                                </Link>
                            }
                            
                        </div>
                        <h5>{profile.owner.username}</h5>
                        <div className="d-flex">
                            <div className="d-flex">
                                <img height={28} width={28} src="./src/assets/icons/star-icon.png"/>
                                <p className="ms-2">{profile.completedQuizzesCount ?? "99"}</p>
                            </div>
                            <div className="d-flex ms-3">
                                <img height={28} width={28} src="./src/assets/icons/plus-icon.png"/>
                                <p className="ms-2">{profile.createdQuizzesCount ?? "99"}</p>
                            </div>
                        </div>
                    </div>
                </div>
                {
                    isOwner && <div>
                        <button className="btn minor-btn" onClick={onLogout}>Logout</button>
                    </div>
                }
            </div>
          </div>
        }
      </div>
    )
}
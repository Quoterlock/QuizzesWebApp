import { useContext, useEffect, useState } from "react";
import { QuizLayout } from "../layouts/QuizLayout";
import { AppContext } from "../services/AppContext";
import { useParams } from "react-router";
import { Link } from "react-router-dom";

export default function UserProfilePage() {

    const {id} = useParams()
    const [userProfile, setUserProfile] = useState<UserProfile>()
    const [isOwner, setIsOwner] = useState(false)
    const {userProfileService} = useContext(AppContext)

    useEffect(() => {
        const fetchData = async () => {
          try {
            var response:UserProfile
            if(id === null || id === undefined){
                response = await userProfileService.GetCurrentUserProfile()
                setIsOwner(true)
            }
            else{
                response = await userProfileService.GetUserProfile(id as string)
            }

            if(response !== undefined){
                setUserProfile(response)
                console.log(response);
            } else {
                console.error("fetched profile in null or undefined")
            }
          } catch (error) {
            console.error(error);
          }
        };
    
        fetchData();
      }, []);
    
    const onLogout = () => {
        console.log(`Logout ${userProfile?.Username}...`)
        // TODO: logout logic
    }
    
    return(<QuizLayout>
        <div className="col-lg-6 col-md-8 col-sm-12 mx-auto">
            <div className="block-style d-flex justify-content-between">
                <div className="d-flex">
                    <img height={150} width={150}/>
                    <div className="ms-4">
                        <div className="d-flex">
                            <h2>{userProfile?.DisplayName}</h2>
                            {
                                isOwner && <Link to={"/profile/edit"} className="btn p-1 ms-1">
                                    <img src="./src/assets/icons/edit-icon.png" className="icon-img"/>
                                </Link>
                            }
                            
                        </div>
                        <h5>{userProfile?.Username}</h5>
                        <div className="d-flex">
                            <div className="d-flex">
                                <img height={28} width={28} src="./src/assets/icons/star-icon.png"/>
                                <p className="ms-2">{userProfile?.CompletedQuizzesCount}</p>
                            </div>
                            <div className="d-flex ms-3">
                                <img height={28} width={28} src="./src/assets/icons/plus-icon.png"/>
                                <p className="ms-2">{userProfile?.CompletedQuizzesCount}</p>
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
    </QuizLayout>)
}

function getDefaultProfile():UserProfile{
    return {
        Id:"",
        Username:"",
        DisplayName:"",
        CompletedQuizzesCount:0,
        CreatedQuizzesCount:0
    }
}
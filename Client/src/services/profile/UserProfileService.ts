const apiPath = "https://192.168.0.102:5001/api/profile"

export default class UserProfileService implements IUserProfileService {
    async GetUserProfileAsync(username:string): Promise<UserProfile> {
        const token = localStorage.getItem("jwt-token")
        try{
            const response = await fetch(`${apiPath}?user=${username}`, {
                method:"GET",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            })
            if(!response.ok){
                throw new Error("failed to fetch data")
            }
            else {
                const data: UserProfile = await response.json()
                return data
            }
        } catch (error) {
            console.error("Error fetching data", error)
            throw error
        }
    }
    
    async GetCurrentUserProfileAsync(): Promise<UserProfile> {
        const token = localStorage.getItem("jwt-token")
        try{
            const response = await fetch(`${apiPath}`, {
                method:"GET",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            })
            if(!response.ok){
                throw new Error("failed to fetch data")
            }
            else {
                const data: UserProfile = await response.json()
                return data
            }
        } catch (error) {
            console.error("Error fetching data", error)
            throw error
        }
    }
    
    /*
    GetUserProfile(username:string): UserProfile {
        const [profile, setProfile] = useState(getDefaultProfile())
        const token = localStorage.getItem("jwt-token")
        useEffect(() => {
            const fetchData = async () => {
                try {
                    const response = await fetch(`${apiPath}?user=${username}`, {
                        method:"GET",
                        headers: {
                            "Authorization": `Bearer ${token}`,
                            "Content-Type": "application/json"
                        }
                    })
                    const jsonResponce = await response.json()
                    console.log(jsonResponce)
                    setProfile(jsonResponce)
                } 
                catch(error) {
                    console.error(error)
                }
            }
            fetchData()
        }, [token])
        return profile
    }

    GetCurrentUserProfile(): UserProfile {
        const [profile, setProfile] = useState(getDefaultProfile())
        const token = localStorage.getItem("jwt-token")
        useEffect(() => {
            const fetchData = async () => {
                try {
                    const response = await fetch(`${apiPath}`, {
                        method:"GET",
                        headers: {
                            "Authorization": `Bearer ${token}`,
                            "Content-Type": "application/json"
                        }
                    })
                    const jsonResponce = await response.json()
                    console.log(jsonResponce)
                    setProfile(jsonResponce)
                } 
                catch(error) {
                    console.error(error)
                }
            }
            fetchData()
        }, [])
        return profile
    }
    */
    async UpdateProfile(id:string, profile:UserProfile): Promise<RequesResult> {
        return {code:500, message:"not implemented"}
    }

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

var apiPath:string

export default class UserProfileApi implements IUserProfileApi {
    
    constructor(apiRoute:string){
        apiPath = `${apiRoute}/profile`;
    }
    
    async GetUserProfileAsync(username:string): Promise<{code: number, profile?: UserProfile}> {
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
                return { code: response.status }
            }
            else {
                const data: UserProfile = await response.json()
                return { code:200, profile: data }
            }
        } catch (error) {
            console.error("Error fetching data", error)
            return { code: 0 }
        }
    }
    
    async GetCurrentUserProfileAsync(): Promise<{code: number, profile?: UserProfile}> {
        const token = localStorage.getItem("jwt-token")
        try{
            const response = await fetch(`${apiPath}`, {
                method:"GET",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            })
            if(!response.ok) {
                return { code: response.status }
            }
            else {
                const data: UserProfile = await response.json()
                return { code: 200, profile:data }
            }
        } catch (error) {
            console.error("Error fetching data", error)
            return { code: 0 }
        }
    }
   
    async UpdateProfile(id:string, profile:UserProfile): Promise<RequesResult> {
        return {code:500, message:"not implemented"}
    }

}

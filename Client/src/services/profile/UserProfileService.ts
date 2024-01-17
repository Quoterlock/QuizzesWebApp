const apiPath = "https://localhost:7118/api/profile"
export default class UserProfileService implements IUserProfileService {
    async GetUserProfile(username:string): Promise<UserProfile|undefined> {
        const response = await fetch(`${apiPath}?user=${username}`, {
            method:"GET"
        })
        if(response.ok){
            return await response.json()
        } else {
            return undefined
        }
    }

    async GetCurrentUserProfile(): Promise<UserProfile|undefined> {
        const token = localStorage.getItem("jwt-token")
        const response = await fetch(`${apiPath}`, {
            method:"GET",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            }
        })
        if(response.ok){
            console.log(response)
            return await response.json()
        } else {
            console.log(response)
            return undefined
        }
    }

    async UpdateProfile(id:string, profile:UserProfile): Promise<RequesResult> {
        return {code:200, message:"not implemented"}
    }
}
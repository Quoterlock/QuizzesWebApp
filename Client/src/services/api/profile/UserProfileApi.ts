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
    

    async GetCurrentUserNameAsync(): Promise<{code: number, username: string}> {
        const token = localStorage.getItem("jwt-token")
        try{
            const response = await fetch(`${apiPath}/current-username`, {
                method:"GET",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            })
            if(!response.ok) {
                return { code: response.status, username: "" }
            }
            else {
                const data: {username : string} = await response.json()
                return { code: 200, username:data.username || "" }
            }
        } catch (error) {
            console.error("Error fetching data", error)
            return { code: 0, username: "" }
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
        const token = localStorage.getItem("jwt-token")
        try{
            const response = await fetch(`${apiPath}`, {
                method:"POST",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(profile)
            })
            if(!response.ok) {
                return { code: response.status, message: "Error" }
            }
            else {
                return { code: 200, message:"Saved" }
            }
        } catch (error) {
            console.error("Error fetching data", error)
            return { code: 0, message : "Error" }
        }
    }

}

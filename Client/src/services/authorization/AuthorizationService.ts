
const apiPath = "https://localhost:7118/api/authorization"

export class AuthorizationService implements IAuthorizationService {
    async Register(username: string, email: string, password: string): Promise<RequesResult> {
        const response = await fetch(`${apiPath}/register`, {
            method:"POST",
            headers: {"Content-Type":"application/json"},
            body: JSON.stringify({username:username, email:email, password:password})
        })
        if(response.ok) {
            return({code:200, message:"registered"})
        } else {
            const responseBody = await response.json();
            console.log(responseBody)
            return({code:response.status, message:responseBody})
        }
    }

    async Login(email: string, password: string): Promise<RequesResult> {
        const result = await fetch(`${apiPath}/login`, {
            method:"POST",
            headers: {"Content-Type":"application/json"},
            body: JSON.stringify({email:email, password:password})
        });
        if(result.ok)  {
            console.log(result.text);
            const token = await result.text()
            console.log(`jwt-token:${token}`)
            localStorage.setItem("jwt-token", token)
            //TODO save current user name and id to a local storage
            return { code:200, message:"all is ok"}
        } else {
            return {code:result.status, message:"server error"}
        }
    }
    async Logout(): Promise<RequesResult> {
        localStorage.removeItem("jwt-token")
        return {code:200, message:"logged out"}
    }
}
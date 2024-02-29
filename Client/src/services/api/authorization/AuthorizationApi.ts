var apiPath:string

export class AuthorizationApi implements IAuthorizationApi {
    constructor(apiRoute:string){
        apiPath = `${apiRoute}/authorization`;
    }
    
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
        const response = await fetch(`${apiPath}/login`, {
            method:"POST",
            headers: {"Content-Type":"application/json"},
            body: JSON.stringify({email:email, password:password})
        });
        if(response.ok)  {
            console.log(response.text);
            const token = await response.text()
            console.log(`jwt-token:${token}`)
            localStorage.setItem("jwt-token", token)
            return { code:200, message:"all is ok"}
        } else {
            return {code:response.status, message:"Incorrect username or password"}
        }
    }
}
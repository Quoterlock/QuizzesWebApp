const apiPath = "https://localhost:7281/api/Accounts"

export class AuthorizationService implements IAuthorizationService {
    async Login(login: string, password: string): Promise<RequesResult> {
        const result = await fetch(`${apiPath}/login`, {
            method:"POST",
            headers: {"Content-Type":"application/json"},
            body: JSON.stringify({email:login, password:password, rememberMe:true, returnUrl:"/api/Accounts/ping"})
        });
        if(result.ok)  {
            console.log(result)
            return { code:200, message:"all is ok"}
        } else {
            return {code:result.status, message:"server error"}
        }
    }
    async Logout(): Promise<RequesResult> {
        const result = await fetch(`${apiPath}/ping`, {
            method: 'GET',
            headers: {
            'Content-Type': 'application/json',
            'Authorization':'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjdjOGY3OWUwLTI3NDktNDYxYy1hYjM1LWNjYjY5ZWJjODRjOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ1c2VyQG1haWwuY29tIiwiZXhwIjoxNzA0ODA2MDYwfQ.Mq4Xnsvk8vwsqV38HVF80A5UHRSeOTMMW7gscMGrvDM'
            },
            credentials: 'include', 
        });
        console.log(result);
        return {code:result.status, message:""}
    }
}
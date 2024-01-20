import { useContext, useState } from "react";
import { AppContext } from "../AppContext";

const apiPath = "https://192.168.0.102:5001/api/authorization"

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
    async Logout(): Promise<RequesResult> {
        localStorage.removeItem("jwt-token")
        localStorage.removeItem("current-username")
        localStorage.removeItem("current-user-id")
        return {code:200, message:"logged out"}
    }
}
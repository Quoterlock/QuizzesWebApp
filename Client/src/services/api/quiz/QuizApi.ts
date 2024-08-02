import { useEffect, useState } from "react"

var apiPath:string

export class QuizApi implements IQuizApi{

    constructor(apiRoute:string) {
        apiPath = `${apiRoute}/quizzes`
    }

    async SaveResultAsync(result: QuizResult): Promise<RequesResult> {
        const token = localStorage.getItem("jwt-token")
        const response = await fetch(`${apiPath}/save-result`, {
            method:"POST",
            headers: {
                "Content-Type":"application/json",
                "Authorization": `Bearer ${token}`
            },
            
            body: JSON.stringify(result)
        });
        if(response.ok)  {
            return { code:200, message:"all is ok"}
        } else {
            return {code:response.status, message:"server error"}
        }
    }


    async CreateNewQuiz(quiz: QuizItem): Promise<RequesResult> {
        const token = localStorage.getItem("jwt-token")
        const result = await fetch(`${apiPath}/create`, {
            method:"POST",
            headers: {
                "Content-Type":"application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(quiz)
        });
        if(result.ok)  {
            return { code:200, message:"all is ok"}
        } else {
            return {code:result.status, message:"server error"}
        }
    }

    async GetList(startIndex: number, endIndex: number): Promise<QuizListItem[]> {
        try{
            const responce = await fetch(`${apiPath}/list?startIndex=${startIndex}&endIndex=${endIndex}`)
            if(!responce.ok) {
                throw new Error ("failed to fetch data");
            }
            const data: QuizListItem[] = await responce.json()
            return data
        } catch (error) {
            console.error("Error fetching data:",error);
            throw error;
        }
    }


    async SearchAsync(searchValue: string): Promise<QuizListItem[]> {
        try{
            const responce = await fetch(`${apiPath}/search?value=${searchValue}`)
            if(!responce.ok) {
                throw new Error ("failed to fetch data");
            }
            const data: QuizListItem[] = await responce.json()
            return data
        } catch (error) {
            console.error("Error fetching data:",error);
            throw error;
        }
    }


    async GetByIdAsync(id: string): Promise<QuizItem> {
        try{
            const responce = await fetch(`${apiPath}/${id}`)
            if(!responce.ok) {
                throw new Error ("failed to fetch data");
            }
            const data: QuizItem = await responce.json()
            return data
        } catch (error) {
            console.error("Error fetching data:",error);
            throw error;
        }
    }

    async DeleteQuiz(id: string): Promise<RequesResult> {
        const token = localStorage.getItem("jwt-token")
        try {
            const responce = await fetch(`${apiPath}/delete/${id}`,{
                method:"POST",
                headers: {
                    "Content-Type":"application/json",
                    "Authorization": `Bearer ${token}`
                },
            })
            if(!responce.ok) {
                throw new Error("failed to delete quiz")
            }
            return {code:200, message:"quiz deleted"}
        } catch(error) {
            console.error("Error fetching data:", error)
            throw error;
        } 
    }
}